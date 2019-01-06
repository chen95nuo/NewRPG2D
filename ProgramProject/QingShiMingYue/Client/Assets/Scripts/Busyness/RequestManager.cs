//#define ENABLE_REQUEST_MANAGER_LOG
//#define REQUEST_MANAGER_METRICS
//#define DELAY_PROCESS_RESPONSE
#define CHECK_REQUEST_TIME_OUT
using UnityEngine;
using System;
using System.Collections.Generic;

// Request manager.
class RequestMgr
{
	// Callback to notice outside request manager has been broken.
	public delegate void BrokenDelegate(string brokenMessage, bool isRelogin);

	// Callback to update outside busy state.
	public delegate void BusyDelegate(bool busy);

	// 发送超时时候的提示.
	public delegate void TimeOutDelegate();

	// 发送超时时候的提示.
	public delegate void ReceiveResponseDelegate();

	// 数据不同步时候的提示
	public delegate void ConnectionOutOfSyncDelegate();

	// 配置文件不同步时候的提示
	public delegate void ConfigOutOfSyncDelegate(string brokenMessage);

	// RequestMgr instance.
	public static RequestMgr Inst
	{
		get
		{
			if (sInstance == null)
				sInstance = new RequestMgr();

			return sInstance;
		}
	}

	private bool initialized = false;

	// Request manager is singleton.
	private static RequestMgr sInstance;

	// Request & Response list.
	private List<Request> requestList = new List<Request>();
	private List<Response> responseList = new List<Response>();

	// Executing candidate list.
	private List<Request> excRequestList = new List<Request>(); // Request candidates.
	private List<Response> excResponseList = new List<Response>(); // Response candidates.

	// Business processor.
	private IBusiness bussiness;
	public IBusiness Bussiness
	{
		get { return bussiness; }
	}

	public int NetStatus
	{
		get { return bussiness != null ? bussiness.GetNetStatus() : com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_CLOSED; }
	}

	public bool DoesSupprotReconnect
	{
		get { return bussiness != null ? bussiness.DoesSupprotReconnect() : false; }
	}

	// Callbacks
	private BrokenDelegate brokenDelegate;
	private TimeOutDelegate timeOutDeletate;
	private BusyDelegate busyDelegate;
	private ReceiveResponseDelegate receiveResponseDeletate;
	private ConnectionOutOfSyncDelegate connectionOutOfSyncDelegate;
	private ConfigOutOfSyncDelegate configOutOfSyncDelegate;

	// Busy flag.
	private int busyNumber;
	private int lastBusyNumber;

	public bool IsBusy
	{
		get { return busyNumber != 0; }
	}


	private const float cMaxRequestDelayTime = 3.0f;
	private float lastAddRequestTime = 0;

	private float responseTimeoutTime = 10f;
	private float lastRequestTime = float.MaxValue;

#if DELAY_PROCESS_RESPONSE
	private const float delayProcessTime = 5f;
	private float lastUpateResponseTime = float.MinValue;
#endif

	// Metrics
#if REQUEST_MANAGER_METRICS
	private int maxRequestInQueue = 0;
	private int maxResponseInQueue = 0;
	private float lastShowMetricsElapse = 0;
	private float showMetricsTime = 10.0f;
#endif

	public void Initialze(float responseTimeoutTime, BrokenDelegate brokenDelegate, BusyDelegate busyDelegate,
		TimeOutDelegate timeOutDeletate, ReceiveResponseDelegate receiveResponseDeletate, ConnectionOutOfSyncDelegate connectionOutOfSyncDelegate, ConfigOutOfSyncDelegate configOutOfSyncDelegate)
	{
		this.responseTimeoutTime = responseTimeoutTime;

		busyNumber = 0;
		lastBusyNumber = 0;

		this.brokenDelegate = brokenDelegate;
		this.busyDelegate = busyDelegate;
		this.timeOutDeletate = timeOutDeletate;
		this.receiveResponseDeletate = receiveResponseDeletate;
		this.connectionOutOfSyncDelegate = connectionOutOfSyncDelegate;
		this.configOutOfSyncDelegate = configOutOfSyncDelegate;

		// Create protocol business
		bussiness = new ServerBusiness();
		//bussiness = new LocalBusiness();
		bussiness.Initialze();

		initialized = true;

#if DELAY_PROCESS_RESPONSE
		responseTimeoutTime = float.MaxValue;
#endif
	}

	public void Dispose()
	{
		bussiness.Dispose();
	}

	public void OnUpdate()
	{
		if (initialized == false)
			return;

#if REQUEST_MANAGER_METRICS
		lastShowMetricsElapse += Time.deltaTime;
		if (lastShowMetricsElapse > showMetricsTime)
		{
			lastShowMetricsElapse = 0;
			Debug.Log(string.Format("Request manager metrics : max request count({0}), max response count({1})", maxRequestInQueue, maxResponseInQueue));
			foreach (var request in requestList)
				Debug.Log(request);
			foreach (var response in responseList)
				Debug.Log(response);
		}
#endif

#if CHECK_REQUEST_TIME_OUT
		// 检测网络发送超时
		if (Time.realtimeSinceStartup - lastRequestTime > responseTimeoutTime)
		{
			// 断线重连逻辑:
			// 如果包含不能重发的协议, 直接断线
			for (int i = 0; i < this.requestList.Count; ++i)
			{
				var request = requestList[i];
				if (request.IsExecuted && request.CanResend == false)
				{
					Debug.Log("Request time out, can not reconnect, disconnect.");
					
					bussiness.DisconnectAS();
					bussiness.DisconnectIS();
					this.Broke(null, true);
					return;
				}
			}

			Debug.Log("Request time out, reconnect.");

			/*
			 * 可以尝试重连
			 */
			// 移除等待标记
			if (busyDelegate != null)
				busyDelegate(false);

			// 移除超时时间
			lastRequestTime = float.MaxValue;

			// 提示超时, (之后用户应该调用Reconnect
			if (timeOutDeletate != null)
				timeOutDeletate();

			return;
		}
#endif

		OnUpdate(Time.realtimeSinceStartup - lastAddRequestTime > cMaxRequestDelayTime);
	}

	private void OnUpdate(bool execAllRequest)
	{
		// Reset busy flag.
		lastBusyNumber = busyNumber;

		// Update business.
		bussiness.Update();

#if DELAY_PROCESS_RESPONSE
		if (Time.realtimeSinceStartup - lastUpateResponseTime > delayProcessTime)
		{
			UpdateResponse();
			lastUpateResponseTime = Time.realtimeSinceStartup;
		}
#else
		// Update response
		UpdateResponse();
#endif

		// Update request
		UpdateRequest(execAllRequest);

		// If busy change, notice outside.
		if (lastBusyNumber != busyNumber)
			CallBusyDelegate();
	}

	private void UpdateRequest(bool execAllRequest)
	{
		// Save request & response to another list to prevent modify-while-traverse problem.
		excRequestList.Clear();

		// Get executing response list and remove invalid request from head
		for (int requestIdx = 0; requestIdx < requestList.Count; ++requestIdx)
		{
			Request request = requestList[requestIdx];

			// Skip discarded request, combined request, processed request
			if (request.IsDiscarded ||
				request.IsCombined ||
				(request.IsExecuted && (request.HasResponse == false || request.IsResponded)))
			{
				continue;
			}

			excRequestList.Add(request);
		}

		// Process request
		for (int curQuestIdx = 0; curQuestIdx < excRequestList.Count; ++curQuestIdx)
		{
			Request request = excRequestList[curQuestIdx];

			// Skip processed request
			if (request.IsDiscarded || request.IsCombined || request.IsExecuted)
				continue;

			// If it's a combinable request, check the next request if can be combined with it
			if (request.Combinable && curQuestIdx + 1 < excRequestList.Count)
			{
				//	If can be combined, combined it to the next request, and mark it executed, and go to the next quest.
				Request nextRequest = excRequestList[curQuestIdx + 1];
				if (nextRequest.CombineWithPrevRequest(request) == true)
				{
#if ENABLE_REQUEST_MANAGER_LOG
					Debug.Log(string.Format("Combine request ({0}):({1})", request.ToString(), nextRequest.ToString()));
#endif

					request.IsCombined = true;
					continue;
				}
			}

			// Else, try to send it.
			// If it's a delay-able request, skip it and send it later.
			if (execAllRequest == false && request.Delayable)
				continue;

			// Else, try to send it.
			// If there is previous delay-able request. send them.
			for (int questIdx = 0; questIdx < curQuestIdx; ++questIdx)
			{
				Request prevQuest = excRequestList[questIdx];
				if (prevQuest.IsExecuted || prevQuest.IsDiscarded || prevQuest.IsCombined)
					continue;

#if ENABLE_REQUEST_MANAGER_LOG
				Debug.Log(string.Format("Process delayed request " + prevQuest.ToString()));
#endif

				ExecRequest(prevQuest);
			}

			// Send this quest.
			ExecRequest(request);
		}

		// Remove invalid request from head
		for (int requestIdx = 0; requestIdx < requestList.Count; ++requestIdx)
		{
			Request request = requestList[requestIdx];

			// Remove discarded request, combined request, processed request
			if (request.IsDiscarded ||
				request.IsCombined ||
				(request.IsExecuted && (request.HasResponse == false || request.IsResponded)))
			{
				requestList.RemoveAt(requestIdx);
				--requestIdx;

				// Reduce busy number.
				if (request.ExecResult && request.HasResponse && request.WaitingResponse)
					busyNumber = Math.Max(busyNumber - 1, 0);

#if ENABLE_REQUEST_MANAGER_LOG
				Debug.Log("Remove invalid request " + request.ToString());
#endif
			}
		}
	}

	private void UpdateResponse()
	{
		// Save request & response to another list to prevent modify-while-traverse problem.
		excResponseList.Clear();

		// Get executing response list and remove executed response
		for (int i = 0; i < responseList.Count; ++i)
		{
			var response = responseList[i];
			if (response.IsExecuted)
				continue;

			excResponseList.Add(response);
		}

		// Process response.
		for (int i = 0; i < excResponseList.Count; ++i)
		{
			var response = excResponseList[i];
			if (response.IsExecuted)
				continue;

			ExecResponse(response);
		}

		responseList.RemoveAll((Response item) =>
								{
									if (item.IsExecuted)
										return true;
									else
										return false;
								}

			);
	}

	private void CallBusyDelegate()
	{
		if (busyDelegate != null)
			busyDelegate(busyNumber > 0);
	}

	public void RetainBusy()
	{
		busyNumber++;

		CallBusyDelegate();
	}

	public void ReleaseBusy()
	{
		busyNumber = Math.Max(busyNumber - 1, 0);

		CallBusyDelegate();
	}

	public bool IsWaitingResponse(Type requestType)
	{
		for (int i = 0; i < requestList.Count; ++i)
		{
			var request = requestList[i];
			if (request.IsDiscarded == false && request.HasResponse && request.WaitingResponse && request.GetType() == requestType)
				return true;
		}

		return false;
	}

	public bool Request(Request request)
	{
		// If request has response and has any one is still waiting for response,
		// discard this new request.
		if (request.HasResponse && request.WaitingResponse && request.MutuallyExclusive && IsWaitingResponse(request.GetType()))
		{
			Debug.LogError("Previous request is not responded, skip new one " + request.ToString());
			return false;
		}

		// Push this request.
		requestList.Add(request);

		// Record request time
		lastAddRequestTime = Time.realtimeSinceStartup;

		if (request.HasResponse && request.CheckTimeout && request.WaitingResponse)
			lastRequestTime = Time.realtimeSinceStartup;

#if REQUEST_MANAGER_METRICS
		maxRequestInQueue = Math.Max(maxRequestInQueue, requestList.Count);
#endif

		return true;
	}

	public bool Response(Response response)
	{
		// Push this response.
		responseList.Add(response);

#if REQUEST_MANAGER_METRICS
		maxResponseInQueue = Math.Max(maxResponseInQueue, responseList.Count);
		Debug.Log("Add Response " + response);
#endif
		Request request = FindRequest(response.ReuqestID);
		if (request != null && request.HasResponse && request.CheckTimeout)
			lastRequestTime = float.MaxValue;

		if (receiveResponseDeletate != null)
			receiveResponseDeletate();

		return true;
	}

	public void FlushAllRequest()
	{
		OnUpdate(true);
	}

	public void DiscardRequests(System.Type requestType)
	{
		// Discard all request of this type.
		foreach (var request in requestList)
			if (request.GetType() == requestType)
				request.IsDiscarded = true;
	}

	public void DiscardAllRqsts()
	{
		// Discard all request of this type.
		foreach (var request in requestList)
			request.IsDiscarded = true;
	}

	public void ConnectionOutOfSync()
	{
		DiscardAllRqsts();

		busyNumber = 0;
		lastBusyNumber = 0;
		lastRequestTime = float.MaxValue;

		if (connectionOutOfSyncDelegate != null)
			connectionOutOfSyncDelegate();
	}

	public void ConfigOutOfSync(string brokenMessage)
	{
		DiscardAllRqsts();

		busyNumber = 0;
		lastBusyNumber = 0;
		lastRequestTime = float.MaxValue;

		if (configOutOfSyncDelegate != null)
			configOutOfSyncDelegate(brokenMessage);
	}

	public void Broke(string brokenMessage, bool isRelogin)
	{
		DiscardAllRqsts();

		busyNumber = 0;
		lastBusyNumber = 0;
		lastRequestTime = float.MaxValue;

		if (brokenDelegate != null)
			brokenDelegate(brokenMessage, isRelogin);
	}

	// 超时之后的重试
	public void Reconnect()
	{
		if (bussiness.SendTimeout())
		{
			// 恢复超时标记
			if (busyDelegate != null)
				busyDelegate(busyNumber > 0);

			// 恢复超时时间
			lastRequestTime = Time.realtimeSinceStartup;
		}
		else
		{
			// 如果不支持短线重连, 直接break
			bussiness.DisconnectAS();
			bussiness.DisconnectIS();
			SysGameStateMachine.Instance.EnterState<GameState_RetriveGameData>(null, true);
		}
	}

	public void ResetBusyState()
	{
		// 恢复超时标记
		if (busyDelegate != null)
			busyDelegate(busyNumber > 0);
	}

	private void ExecRequest(Request request)
	{
		try
		{
			request.ExecResult = request.Execute(bussiness);
		}
		catch (Exception e)
		{
			request.ExecResult = false;
			request.IsDiscarded = true;
			Debug.LogError(e.StackTrace);
		}
		finally
		{
			if (request.ExecResult)
			{
#if ENABLE_REQUEST_MANAGER_LOG
				Debug.Log("Exec request " + request.ToString());
#endif
				// If request need to waiting for response, mark busy flag.
				if (request.ExecResult && request.HasResponse && request.WaitingResponse)
					busyNumber++;
			}
			else
			{
				Debug.LogError("Failed to exec request " + request.ToString());
				int status = NetStatus;
				if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_NEEDQUERYINITINFO)
					ConnectionOutOfSync();
				else
				{
					bool isRelogin = false;
					if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_CLOSED)
						isRelogin = true;
					if (request.CanResend == false)
						isRelogin = true;
					Broke(null, isRelogin);
				}
			}
		}
	}

	private void ExecResponse(Response response)
	{
		bool errOcc = false;

		Request request = FindRequest(response.ReuqestID);

		// Execute response.
		try
		{
			errOcc = !response.Execute(request);
		}
		catch (Exception e)
		{
			errOcc = true;
			Debug.LogError(e.StackTrace);
		}
		finally
		{
			// Mark request responded flag.
			if (request != null && request.HasResponse)
				request.IsResponded = true;

			if (errOcc)
				Debug.Log("Failed to respond " + response.ToString() + " " + request);
#if ENABLE_REQUEST_MANAGER_LOG
			else
				Debug.Log("Respond " + response.ToString() + " " + request);
#endif
		}
	}

	private Request FindRequest(int requestID)
	{
		for (int i = 0; i < requestList.Count; ++i)
		{
			var request = requestList[i];
			if (request.ID == requestID)
				return request;
		}

		return null;
	}

	private int CompareRequestID(Request a, Request b)
	{
		return a.ID - b.ID;
	}

	private int CompareResponseID(Response a, Response b)
	{
		return a.ReuqestID - b.ReuqestID;
	}
}
