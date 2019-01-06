#if UNITY_EDITOR
#define FORCE_USE_TEST_IAP_LISTENER
#endif
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ClientServerCommon;

public abstract class IAPListener : MonoBehaviour
{
	public enum VerifyPaymentResult
	{
		Successed,
		Successed_AlreadyProcessed,
		Payment_Failed,
		VerifyPayment_Failed,
	}

	// 根据平台类型创建支付listener
	public static void CreateOnGameObject(GameObject gameObject)
	{
		Type iapListenerType = null;
#if FORCE_USE_TEST_IAP_LISTENER
		iapListenerType = typeof(Test_IAPListener);
#else
		switch (KodGames.ExternalCall.KodConfigPlugin.GetPublisher())
		{
			case _ProductPublisher.KodGames: iapListenerType = typeof(IOS_IAPListener); break;
			case _ProductPublisher.ChangYou: iapListenerType = typeof(ChangYou_IAPListener); break;
			case _ProductPublisher.Cmge: iapListenerType = typeof(Cmge_IAPListener); break; 
			case _ProductPublisher.Efun: iapListenerType = typeof(Efun_IAPListener); break;
			default: Debug.LogError("Invalid publisher type when create IAP listener"); break;
		}
#endif
		if (iapListenerType != null)
		{
			Debug.Log("Create IAP Listener : " + iapListenerType);
			gameObject.AddComponent(iapListenerType);
		}
	}

	// 上一次支付的消息号
	protected int payProductCallback = Request.InvalidID;

	// 上次支付是否完成
	public bool WaitingforPreviousPayment()
	{
		if (payProductCallback == Request.InvalidID)
			return false;

		Debug.LogError("Only one payment can be maked at the sametimes.");
		return true;
	}

	// 当前平台是否支持支付
	public virtual bool CanMakePayment()
	{
		return true;
	}

	// 支付商品
	public abstract bool PayProduct(string productID, int goodsID, int count, int callback, string additionalData);

	// 商品支付结果回调
	public virtual void OnVerifyPaymentTransaction(VerifyPaymentResult result, string transactionID, int goodsID, int count,string errMeg,List<int> payStatus)
	{
		switch (result)
		{
			case VerifyPaymentResult.Successed:
				RequestMgr.Inst.Response(new IAPPaymentResponse(payProductCallback, goodsID, count, errMeg,payStatus));
				payProductCallback = Request.InvalidID;
				break;

			case VerifyPaymentResult.Successed_AlreadyProcessed:
				break;

			case VerifyPaymentResult.Payment_Failed:
				RequestMgr.Inst.Response(new IAPPaymentResponse(payProductCallback, "Payment failed!"));
				payProductCallback = Request.InvalidID;
				break;

			case VerifyPaymentResult.VerifyPayment_Failed:
				RequestMgr.Inst.Response(new IAPPaymentResponse(payProductCallback, "VerifyPayment failed!"));
				payProductCallback = Request.InvalidID;
				break;
		}
	}

	protected string GenerateOrderId()
	{
		var now = DateTime.UtcNow;
		return string.Format("{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}{6:D4}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
	}

	// 构造平台支付Id
	protected string GetPartnerOrderId(string productID, string additionalData)
	{
		int osType = _DeviceType.Unknown;
#if UNITY_IPHONE
		osType = _DeviceType.iPhone;
#elif UNITY_ANDROID
		osType = _DeviceType.Android;
#endif
		int mergeAreaId = SysLocalDataBase.Inst.LoginInfo.LoginArea.MergeAreaId;
		string channelUniqueId = Platform.Instance.ChannelMessage.ChannelUniqueId;
		int serverType = Platform.Instance.ServerType;
		var sb = new System.Text.StringBuilder();
		sb.Append(SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString());
		sb.Append("#"); sb.Append(osType.ToString());
		sb.Append("#"); sb.Append(mergeAreaId.ToString());
		sb.Append("#"); sb.Append(Platform.Instance.ChannelId);
		sb.Append("#"); sb.Append(channelUniqueId);
		sb.Append("#"); sb.Append(serverType);
		sb.Append("#"); sb.Append(additionalData);
		sb.Append("#"); sb.Append(KodGames.ExternalCall.KodConfigPlugin.GetPromoteId());
		return sb.ToString();
	}
}