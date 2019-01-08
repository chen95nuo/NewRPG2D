// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhotonHandler.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using ExitGames.Client.Photon;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

/// <summary>
/// Internal Monobehaviour that allows Photon to run an Update loop.
/// </summary>
internal class PhotonHandler : Photon.MonoBehaviour, IPhotonPeerListener
{
    public static PhotonHandler SP;

    public int updateInterval;  // time [ms] between consecutive SendOutgoingCommands calls

    public int updateIntervalOnSerialize;  // time [ms] between consecutive RunViewUpdate calls (sending syncs, etc)

    private int nextSendTickCount = 0;

    private int nextSendTickCountOnSerialize = 0;
    
    private static bool sendThreadShouldRun;
	
#region //!reaad some setup	
	private static bool m_bIsShowLog=false;
	private static bool m_bIsNotSerializeWrite=false;
	private static bool m_bIsNotSerializeRead=false;
	private static bool m_bIsWriteDelay=false;
	
	private static ExitGames.Client.Photon.ConnectionProtocol m_TcpConnectProtocol=ConnectionProtocol.Tcp;
	
	private static ExitGames.Client.Photon.ConnectionProtocol m_UdPConnectProtocol=ConnectionProtocol.Udp;
	
	private static int m_nWriteCount=0;
	
	private static PhotonView m_cPhotoView=null;
	
	private static string m_strPositionId;
	
	private static string m_strPlayerId;
	
	private static int m_nLastSendAsk=0;
	
	private static bool m_bIsAutoLogin=false;
	
	private static string m_strAutoUser;
	private static string m_strAutoPwd;
	
	private static string m_strLogicAddr="";
	
	private static bool m_bIsTcp=true;
	
	private static bool m_bIsFirst=true;
	
	private static string m_strLogicSvr;
	
	private static bool m_bIsGetLogicSvr=false;
	
	private static string m_strLogicPort;
	
	private static string m_strRoomPort;
	
	
	public static string GetLogicSvr()
	{
		ShowLog("get logic svr"+m_strLogicSvr);
		return m_strLogicSvr+":"+m_strLogicPort;
	}

    public static void SetBtnVisible()
    {
        if(m_bIsShowLog)
        {

        }
    }
	

	
	public static string GetRoomSvr()
	{
		return m_strLogicSvr+":"+m_strRoomPort;
	}
	
	
	public static bool IsGetLogicSvr()
	{
		return m_bIsGetLogicSvr;
	}
	
	public static void SetLogicAddr(string _strAddr)
	{
        ShowLog("set logic addr:" + _strAddr);
		m_strLogicAddr=_strAddr;
	}
	
	public static string GetLogicAddr()
	{
        ShowLog("get logic addr:" + m_strLogicAddr);
		return m_strLogicAddr;
	}
	
	
	public static bool IsSendSensAsk()
	{
		if(System.Environment.TickCount>m_nLastSendAsk)
		{
			m_nLastSendAsk=System.Environment.TickCount+2000;
			return true;
		}
		else
		{
			return false;
		}
		
	}
	
	public static bool IsAutoLogin()
	{
		return m_bIsAutoLogin;
	}
	
	public static string GetAutoStr()
	{
		return m_strAutoUser;
	}
	
	public static string GetAutoPwd()
	{
		return m_strAutoPwd;
	}
	
	public static void ShowLog(string _log)
	{
        if (IsShowLog())
        {
            Debug.Log(string.Format("johnlogref,time:{0},{1}", System.Environment.TickCount, _log));
        }
		
	}

    public static  void SetUpdMode()
    {

        ShowLog("SetUpdMode");
        m_bIsTcp=false;

    }
	
	public static void SetTcpMode()
	{
		ShowLog("SetTcpMode");
		m_bIsTcp=true;
	}
	
	/// <summary>
	/// 房间服务器连接方式
	/// </summary>
	public static  ExitGames.Client.Photon.ConnectionProtocol roomConnectType=ConnectionProtocol.Udp;
	
	
	public static ExitGames.Client.Photon.ConnectionProtocol GetProtocol()
	{
	
		//ReadIsShowLog();
       // m_bIsTcp = true;
		if(m_bIsTcp)
		{
			ShowLog("GetProtocol tcp mode");
			return m_TcpConnectProtocol;
		}
		else
		{
            ShowLog("GetProtocol udp mode");
			return m_UdPConnectProtocol;
		}
		
	}
   
	
	public static PhotonView GetSelfView()
	{
		return m_cPhotoView;
	}
	
	public static string GetPositionId()
	{
		Debug.Log(string.Format("johnlogref GetPositionId:{0}",m_strPositionId));
		return m_strPositionId;
	}
	
	public static string GetPlayerId()
	{
		return m_strPlayerId;
	}
	
	public static void SetSelfVilw(string _str,PhotonView _SelfView,params object[] parameters)
	{
		
			if(_str=="SynPositiontable")
			{
				Debug.Log(string.Format("johnlogref SetSelfVilw:{0},position:{1},pacount:{2},playerid:{3}",_SelfView.name,parameters.GetValue(1),parameters.Length,parameters.GetValue(0)));
				m_cPhotoView=_SelfView;
				m_strPositionId=string.Format("{0}",parameters.GetValue(1));
			    m_strPlayerId=string.Format("{0}",parameters.GetValue(0));
				return;
			}

			
		
		
		//Debug.Log(string.Format("have SetSelfVilw:{0}",_SelfView.name));
		
	}
	
	public static void SetViewNull()
	{
		m_cPhotoView=null;
	}
	
	public static bool IsShowLog()
	{
		return m_bIsShowLog;
	}
	
	public static bool IsWritelDelay()
	{
		return true;
		return m_bIsWriteDelay;
	}
	
	public static void AddWriteCount()
	{
		m_nWriteCount++;
	}
	
	public static int GetWriteCount()
	{
		return m_nWriteCount;
	}
	
	public static bool IsNotWriteSerial()
	{
		return m_bIsNotSerializeWrite;
	}
	
	public static bool IsNotReadSerial()
	{
		return m_bIsNotSerializeRead;
	}
	
	public static void ReadIsShowLog()
	{
		
		if(!m_bIsFirst)
		{
			ShowLog("not first");
			return;
		}
		ShowLog("first ready read johntag.txt");
		m_bIsFirst=false;
	
		if(File.Exists("johnlogtag.txt"))
		{
			StreamReader t_objReader = new StreamReader("johnlogtag.txt");
			string t_str=t_objReader.ReadLine();
			
			ShowLog("find johnlogtag.txt");
			
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="showlog")
			{
				m_bIsShowLog=true;
				
			}
			
		    t_str=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="notwrite")
			{
				m_bIsNotSerializeWrite=true;
				
			}
			
		    t_str=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="notread")
			{
				m_bIsNotSerializeRead=true;
				
			}
			
				
		    t_str=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="writedelay")
			{
				m_bIsWriteDelay=true;
				
			}
			
			t_str=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="auto")
			{
				m_bIsAutoLogin=true;
				
			}
			
		
			
			m_strAutoUser=t_objReader.ReadLine();
			m_strAutoPwd=t_objReader.ReadLine();
			ShowLog(string.Format("readtag,user:{0},pwd:{1}",m_strAutoUser,m_strAutoPwd));
			
			
			t_str=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="istcp")
			{
				m_bIsTcp=true;
				
			}
			
			
			t_str=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",t_str));
			if(t_str=="islogicsvr")
			{
				m_bIsGetLogicSvr=true;
				
			}
			
			m_strLogicSvr=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",m_strLogicSvr));
			
			m_strLogicPort=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",m_strLogicPort));
			
			m_strRoomPort=t_objReader.ReadLine();
			ShowLog(string.Format("readtag:{0}",m_strRoomPort));
			
			
			
			
		
		}
		else
		{
//			Debug.Log("not find johnlogtag.txt");
			
		}
	}
		
#endregion
	
	public float sendTimes=0.1f;
	public static int johnSendTimes=333;
	public static bool isStremUp=true;
	public static bool isRPCUp=true;

	public string pathXML=Application.persistentDataPath+@"/YuanXml.xml";
    protected void Awake()
    {
//		Debug.Log ("----------------"+pathXML);
		XmlDocument doc=new XmlDocument();
		if(!File.Exists (pathXML))
		{
			XmlElement rootElement =doc.CreateElement("YuanXml");
			doc.AppendChild (rootElement);
			doc.Save (pathXML);
		}
		else
		{
			doc.Load (pathXML);
		}
	
			
			XmlNode root=doc.SelectSingleNode ("YuanXml");
			//XmlNode xn=root.SelectSingleNode ("SendTimes");
			XmlNode xn;
			GetXMLNode (doc,root,out xn,"SendTimes","0.1");
			sendTimes=float.Parse (xn.InnerText);
			BtnGameManager.rpcSendTime=sendTimes;
			//xn=root.SelectSingleNode ("StremUp");
			GetXMLNode (doc,root,out xn,"StremUp","yes");
			if(xn.InnerText=="yes")
			{
				isStremUp=true;
			}
			else if(xn.InnerText=="no")
			{
				isStremUp=false;
			}
			//xn=root.SelectSingleNode ("RPCUp");
			GetXMLNode (doc,root,out xn,"RPCUp","yes");
			if(xn.InnerText=="yes")
			{
				isRPCUp=true;
			}
			else if(xn.InnerText=="no")
			{
				isRPCUp=false;
			}			
			//xn=root.SelectSingleNode ("RomMaxPlayerNum");
			GetXMLNode (doc,root,out xn,"RomMaxPlayerNum","25");
			BtnGameManager.roomPlayerNum=int.Parse (xn.InnerText);
		
			GetXMLNode (doc,root,out xn,"PublicSkillCD","1");
			BtnGameManager.numPubSkillCD=int.Parse (xn.InnerText);
		
					GetXMLNode (doc,root,out xn,"SeviceMianCity","0.33");
		
			BtnGameManager.numSeviceMianCity=float.Parse (xn.InnerText);
		
					GetXMLNode (doc,root,out xn,"SeviceDuplicate","0.2");
			BtnGameManager.numSeviceDuplicate=float.Parse (xn.InnerText);
		
					GetXMLNode (doc,root,out xn,"SevicePVP","0.1");
			BtnGameManager.numSevicePVP=float.Parse (xn.InnerText);
		
					GetXMLNode (doc,root,out xn,"johnSendTimes","333");
			johnSendTimes=int.Parse (xn.InnerText);
		
			
		
			doc.Save(pathXML);
		
//		Debug.Log ("---------------------isRPCUp:"+isRPCUp);
        if (SP != null && SP != this && SP.gameObject != null)
        {
            GameObject.DestroyImmediate(SP.gameObject);
        }

        SP = this;
        DontDestroyOnLoad(this.gameObject);

        this.updateInterval = 1000 / PhotonNetwork.sendRate;
        this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;

        PhotonHandler.StartFallbackSendAckThread();
		
		
    }
	
	public void GetXMLNode(XmlDocument doc,XmlNode mRoot,out XmlNode mNode,string mName,string mText)
	{
		mNode=null;
		mNode= mRoot.SelectSingleNode (mName);
		if(mNode==null)
		{
			XmlElement rootElement =doc.CreateElement(mName);
			rootElement.InnerText=mText;
			mNode=mRoot.AppendChild (rootElement);
		}
	}

    /// <summary>Called by Unity when the application is closed. Tries to disconnect.</summary>
    protected void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
        PhotonHandler.StopFallbackSendAckThread();
    }

     void Start()
    {  
		String str = Application.loadedLevelName.Substring(3,1);
		
		ReadIsShowLog();

//		if(str=="1"){
//	    InvokeRepeating("SendandRecive",0,sendTimes);		
//		}
//		else
//	    InvokeRepeating("SendandRecive",0,0.1f);
    }
	
	
		
  #region //!john.add.alive.room
     static long m_dwKeepAliveTime = 100;
     static long m_dwSendAliveInternal = 0;
     static long m_dwLastSendTime = 0;
	

	
	

     bool SendKeepAlive()
     {
         bool doSend = false;
         if (System.Environment.TickCount >= m_dwLastSendTime)
         {
             doSend = PhotonNetwork.networkingPeer.SendOutgoingCommands();
             m_dwLastSendTime = System.Environment.TickCount + m_dwSendAliveInternal;

         }
         else
         {
             if (PhotonNetwork.networkingPeer.QueuedOutgoingCommands > 0 || PhotonNetwork.networkingPeer.QueuedIncomingCommands > 0)
             {
				
                 doSend = PhotonNetwork.networkingPeer.SendOutgoingCommands();

             }
         }
         return doSend;
     }
  #endregion
     void Update(){
		try
		{

				
		        if (PhotonNetwork.networkingPeer == null)
		        {
		            Debug.LogError("NetworkPeer broke!");
		          return;
		        }
		
		        if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerState.Disconnected || PhotonNetwork.offlineMode)
		        {
		           return;
		        }
		
		        // the messageQueue might be paused. in that case a thread will send acknowledgements only. nothing else to do here.
		        if (!PhotonNetwork.isMessageQueueRunning)
		        {
		          return;
		        }
		
		        bool doDispatch = true;
		        while (PhotonNetwork.isMessageQueueRunning && doDispatch)
		        {
		            // DispatchIncomingCommands() returns true of it found any command to dispatch (event, result or state change)
                    //Profiler.BeginSample("DispatchIncomingCommands");
		            doDispatch = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
                    //Profiler.EndSample();
		        }
		
		        int currentMsSinceStart = (int)(Time.realtimeSinceStartup * 1000);  // avoiding Environment.TickCount, which could be negative on long-running platforms
		        if (PhotonNetwork.isMessageQueueRunning && currentMsSinceStart > this.nextSendTickCountOnSerialize)
		        {
		            PhotonNetwork.networkingPeer.RunViewUpdate();
		            this.nextSendTickCountOnSerialize = currentMsSinceStart + this.updateIntervalOnSerialize;
		            this.nextSendTickCount = 0;     // immediately send when synchronization code was running
		        }
		
		        currentMsSinceStart = (int)(Time.realtimeSinceStartup * 1000);
		        if (currentMsSinceStart > this.nextSendTickCount)
		        {
		            bool doSend = true;
		            while (PhotonNetwork.isMessageQueueRunning && doSend)
		            {
		                // Send all outgoing commands
                        //Profiler.BeginSample("SendOutgoingCommands");
                        //!john.add.alive.room
                        //doSend = SendKeepAlive();
					doSend= PhotonNetwork.networkingPeer.SendOutgoingCommands();
                        //Profiler.EndSample();
		            }
		
		            this.nextSendTickCount = currentMsSinceStart + this.updateInterval;
		        }
			
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
    }

    /// <summary>Called by Unity after a new level was loaded.</summary>
    protected void OnLevelWasLoaded(int level)
    {
        PhotonNetwork.networkingPeer.NewSceneLoaded();

        if (PhotonNetwork.automaticallySyncScene)
        {
            this.SetSceneInProps();
        }
    }

    protected void OnJoinedRoom()
    {
        PhotonNetwork.networkingPeer.AutomaticallySyncScene();
    }

    protected void OnCreatedRoom()
    {
        if (PhotonNetwork.automaticallySyncScene)
        {
            this.SetSceneInProps();
        }
    }

    protected internal void SetSceneInProps()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Hashtable setScene = new Hashtable();
            setScene[NetworkingPeer.CurrentSceneProperty] = Application.loadedLevelName;
            PhotonNetwork.room.SetCustomProperties(setScene);
        }
    }

    public static void StartFallbackSendAckThread()
    {
        if (sendThreadShouldRun)
        {
            return;
        }

        sendThreadShouldRun = true;
        SupportClass.CallInBackground(FallbackSendAckThread);   // thread will call this every 100ms until method returns false
    }

    public static void StopFallbackSendAckThread()
    {
        sendThreadShouldRun = false;
    }

    public static bool FallbackSendAckThread()
    {
        if (sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
        {
			//
			
			if(!PhotonHandler.IsSendSensAsk())
			{
				return sendThreadShouldRun;
			}
			
			if(PhotonHandler.IsShowLog())
			{
				//PhotonHandler.ShowLog("FallbackSendAckThread");
			}
            PhotonNetwork.networkingPeer.SendAcksOnly();
        }

        return sendThreadShouldRun;
    }

    #region Implementation of IPhotonPeerListener

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
//            Debug.LogError(message);
        }
        else if (level == DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else if (level == DebugLevel.INFO && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
        {
            Debug.Log(message);
        }
        else if (level == DebugLevel.ALL && PhotonNetwork.logLevel == PhotonLogLevel.Full)
        {
            Debug.Log(message);
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                {
                    //!john.add.alive.room
                    m_dwSendAliveInternal = m_dwKeepAliveTime;
                    break;
                }

        }
    }

    public void OnEvent(EventData photonEvent)
    {
    }

    #endregion
}
