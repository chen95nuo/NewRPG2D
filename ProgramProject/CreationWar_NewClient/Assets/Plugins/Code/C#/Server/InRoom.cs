using UnityEngine;
using System.Collections;
using yuan.YuanPhoton;
using System.Collections.Generic;
using yuan.YuanMemoryDB;
using System.Security;
using ExitGames.Client.Photon;
using System;
using System.Linq;

public class InRoom : IPhotonPeerListener 
{
	public static string playerLevel="0";
	public static string playerExp="0";     
	public static string playerID="0";
	public static bool isUpdatePlayerLevel=false;
	
    /// <summary>
    /// ·þÎñÆ÷µØÖ·
    /// </summary>
    private string ServerAddress = "localhost:5055";
	
	public void SetAddress(string _strAddress)
	{
		PhotonHandler.ShowLog("SetAddress:"+_strAddress);
		ServerAddress=_strAddress;
	}
	
	public string GetSvrAddress()
	{
		return ServerAddress;
	}

    /// <summary>
    /// ·þÎñÆ÷ÓŠÓÃÃû³Æ
    /// </summary>
    public string ServerApplication = "YuanPhotonServer";

    public PhotonPeer peer;
	
	public PhotonPeer peerMessage;

    private bool serverConnected;
    /// <summary>
    /// ·þÎñÆ÷µÄÁ¬œÓ×ŽÌ¬(Ö»¶Á)
    /// </summary>
    public bool ServerConnected
    {
        get { return serverConnected; }
        set { 
            serverConnected = value;
        }
    }

    private bool isLogin = false;
    /// <summary>
    /// ÊÇ·ñÕýÔÚµÇÂœÖÐ
    /// </summary>
    public bool IsLogin
    {
        get { return isLogin; }
        set { isLogin = value; }
    }

    private float serviceTimeInterval;
    /// <summary>
    /// seviceË¢ÐÂÆµÂÊ
    /// </summary>
    public float ServiceTimeInterval
    {
        get { return serviceTimeInterval; }
        set { serviceTimeInterval = value; }
    }

    /// <summary>
    /// 服务器时间
    /// </summary>
    public System.DateTime serverTime;

    /// <summary>
    /// 检测服务器时间
    /// </summary>
    //public System.Timers.Timer connectTime;
	public System.Threading.Timer connectTime;
	public bool isConnectTime=false;
	
	//
	public System.Threading.Timer updateTime;
	
	
	private System.Timers.Timer serviceTimer;
	//public System.Threading.Timer serviceTimer;
	public bool isServiceTimer=false;

    public SendManager SM;

    public BtnGameManagerBack btnGameManagerBack;
	public MainMenuManage MMManage;
	
#region john.add.alive
    //收取数据时间间隔
	static long m_dwPeerInternal=50;
	//!心跳包发送间隔
	static long m_dwKeepAliveInternal=1;
	//
	static long m_dwKeepAliveTime=500;
	//下次数据发送时间
	static long m_dwNextSendTime=0;
	//保存数据间隔时间
	static long m_dwUpdateInterval=1000;
#endregion
	
	#region 可视玩家控制
	
	public static Dictionary<string,PhotonView> listLookPlayer=new Dictionary<string,PhotonView>();
	
	/// <summary>
	/// 加入可视玩家
	/// </summary>
	/// <param name='mPlayerID'>
	/// M player I.
	/// </param>
	public void AddLookPlayer(string mPlayerID,PhotonView mView)
	{
		if(!listLookPlayer.ContainsKey (mPlayerID))
		{
			listLookPlayer.Add (mPlayerID,mView);
		}
	}
	
	/// <summary>
	/// 移除可视玩家
	/// </summary>
	/// <param name='mPlayerID'>
	/// M player I.
	/// </param>
	public void RemoveLookPlayer(string mPlayerID)
	{
		listLookPlayer.Remove (mPlayerID);
	}
	
	/// <summary>
	/// 清空可视玩家列表
	/// </summary>
	/// <param name='mPlayerID'>
	/// M player I.
	/// </param>
	public void ClearLookPlayer()
	{
		listLookPlayer.Clear ();
	}
	
	private int[] lookInfo=new int[6];
	/// <summary>
	/// 上传可视玩家信息
	/// </summary>
	/// <param name='x'>
	/// X.
	/// </param>
	/// <param name='y'>
	/// Y.
	/// </param>
	/// <param name='z'>
	/// Z.
	/// </param>
	/// <param name='rotation'>
	/// Rotation.
	/// </param>
	/// <param name='animStuat'>
	/// Animation stuat.
	/// </param>
	/// <param name='animSeed'>
	/// Animation seed.
	/// </param>
	/// <param name='sendid'>
	/// Sendid.
	/// </param>
	public void SendPlayerLookInfo(int x,int y,int z,int rotation,int animStuat,int animSeed,string sendid)
	{
        if (this.ServerConnected)
        {
			
			//lookInfo.x=x;
			//lookInfo.y=y;
			//lookInfo.z=z;
			//lookInfo.rotation=rotation;
			//lookInfo.animStuat=animStuat;
			//lookInfo.animSeed=animSeed;
			//lookInfo.id=sendid;
			string[] playerIDs=new string[listLookPlayer.Count];
			listLookPlayer.Keys.CopyTo (playerIDs,0);
			lookInfo[0]=x;
			lookInfo[1]=y;
			lookInfo[2]=z;
			lookInfo[3]=rotation;
			lookInfo[4]=animStuat;
			lookInfo[5]=animSeed;
		
			
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamInfo, lookInfo);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID,playerIDs);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.MailSender,sendid);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PlayerLook, parameter, true);
        }
	}
	
	#endregion 可视玩家控制

    public InRoom()
    {
		try
		{
			listUpdateTime=new List<YuanServerParms>();
			dicUpdateTime=new YuanServerActor<string, YuanServerParms>(listUpdateTime);
//            Debug.Log("johnlogref inroom creat peer type");
	        //this.peer = new PhotonPeer(this,PhotonHandler.GetProtocol());
			//this.peer.TimePingInterval = 2000;
			//this.peer.DisconnectTimeout = 15000;
	       //this.serviceTimer = new System.Timers.Timer();
	       //this.serviceTimeInterval = 50;
	       //this.serviceTimer.Interval = this.serviceTimeInterval;F
	       //this.serviceTimer.AutoReset = true;
	       //this.serviceTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnServiceTimer);
	       //this.serviceTimer.Enabled = true;
			//!john.add.alive
            this.serviceTimer = new System.Timers.Timer();
            this.serviceTimer.AutoReset = true;
            this.serviceTimer.Interval = m_dwPeerInternal;
            this.serviceTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnServiceTimer);
            this.serviceTimer.Start();

			//this.serviceTimer=new System.Threading.Timer(OnServiceTimer,null,0,m_dwPeerInternal);


			isServiceTimer=true;
			//!john.add.alive
			this.updateTime=new System.Threading.Timer(OnUpdateTimer,null,0,m_dwUpdateInterval);
			
			
	
	        //this.connectTime = new System.Timers.Timer();
	        //this.connectTime.Interval = 1000;
	        //this.connectTime.AutoReset = true;
	        //this.connectTime.Elapsed += new System.Timers.ElapsedEventHandler(OnConnectTimer);
	        //this.connectTime.Enabled = true;
			//this.connectTime=new System.Threading.Timer(OnConnectTimer,null,0,1000);
			isConnectTime=true;
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}

    }
	
	public static void YuanDispose()
	{
		try
		{
			if(inRoomInstantiate!=null)
			{
				if(null!=inRoomInstantiate.peer)
				{
					inRoomInstantiate.peer.Disconnect ();
				}

				if(inRoomInstantiate.serviceTimer!=null)
				{
					//inRoomInstantiate.serviceTimer.Enabled=false;
					inRoomInstantiate.isServiceTimer=false;
					inRoomInstantiate.serviceTimer.Dispose ();
					inRoomInstantiate.serviceTimer=null;
				}
				
				if(inRoomInstantiate.connectTime!=null)
				{
					//inRoomInstantiate.connectTime.Enabled=false;
					inRoomInstantiate.isConnectTime=false;
					inRoomInstantiate.connectTime.Dispose ();
					inRoomInstantiate.connectTime=null;
				}
				//if(inRoomInstantiate.updateTime!=null)
				//{
				//	inRoomInstantiate.updateTime.Dispose ();
				//	inRoomInstantiate.updateTime=null;
				//}
				inRoomInstantiate.isServiceTimer=false;
				inRoomInstantiate.RefreshDic ();
				
				//inRoomInstantiate=null;	
				
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
	}

    public static InRoom inRoomInstantiate;
    /// <summary>
    /// »ñÈ¡·¿ŒäÊµÀý
    /// </summary>
    /// <returns></returns>
    public static InRoom GetInRoomInstantiate()
    {
        if (inRoomInstantiate == null)
        {
            inRoomInstantiate = new InRoom();
        }
        return inRoomInstantiate;
    }

    public static InRoom NewInRoomInstantiate()
    {
		try
		{
////            Debug.Log("johnlogref NewInRoomInstantiate creat peer type:"+PhotonHandler.GetProtocol());
	        GetInRoomInstantiate().peer = new PhotonPeer(GetInRoomInstantiate(),PhotonHandler.GetProtocol());
			GetInRoomInstantiate().peer.TimePingInterval = 2000;
			GetInRoomInstantiate().peer.DisconnectTimeout = 15000;
	        //GetInRoomInstantiate().serviceTimer.Enabled = false;
	        //GetInRoomInstantiate().serviceTimer.Dispose();
	        //GetInRoomInstantiate().serviceTimer = new System.Timers.Timer();
	        //GetInRoomInstantiate().serviceTimeInterval = 50;
	        //GetInRoomInstantiate().serviceTimer.Interval = GetInRoomInstantiate().serviceTimeInterval;
	        //GetInRoomInstantiate().serviceTimer.AutoReset = true;
	        //GetInRoomInstantiate().serviceTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetInRoomInstantiate().OnServiceTimer);
	        //GetInRoomInstantiate().serviceTimer.Enabled = true;
			
			if(GetInRoomInstantiate().serviceTimer==null)
			{
				//GetInRoomInstantiate().serviceTimer=new System.Threading.Timer(GetInRoomInstantiate().OnServiceTimer,null,0,m_dwPeerInternal);
                GetInRoomInstantiate().serviceTimer = new System.Timers.Timer();
                GetInRoomInstantiate().serviceTimer.AutoReset = true;
                GetInRoomInstantiate().serviceTimer.Interval = m_dwPeerInternal;
                GetInRoomInstantiate().serviceTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetInRoomInstantiate().OnServiceTimer);
                GetInRoomInstantiate().serviceTimer.Start();
                
                GetInRoomInstantiate().isServiceTimer=true;
			}
			else
			{
				GetInRoomInstantiate().isServiceTimer=true;
			}
			
			if(GetInRoomInstantiate().connectTime==null)
			{
				//GetInRoomInstantiate().connectTime=new System.Threading.Timer(GetInRoomInstantiate().OnConnectTimer,null,0,1000);
				//GetInRoomInstantiate().isConnectTime=true;
			}
			else
			{
				GetInRoomInstantiate().isConnectTime=true;
			}
			//if(GetInRoomInstantiate().connectTime!=null)
			//{
	        //	GetInRoomInstantiate().connectTime.Enabled = true;
			//}
			//else
			//{
			//	GetInRoomInstantiate().connectTime=new System.Timers.Timer();
			//	GetInRoomInstantiate().connectTime.Interval=1000;
			//	GetInRoomInstantiate().connectTime.AutoReset=true;
			//	GetInRoomInstantiate().connectTime.Elapsed+=new System.Timers.ElapsedEventHandler(GetInRoomInstantiate ().OnConnectTimer);
			//	GetInRoomInstantiate().connectTime.Enabled = true;
			//}
	        GetInRoomInstantiate().RefreshDic();
	        return GetInRoomInstantiate();
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
			return null;
		}
        // return inRoomInstantiate = new InRoom();
    }

    private int timeout = 0;
    Dictionary<short, object> parmsHeartPage = new Dictionary<short, object>() { { (short)yuan.YuanPhoton.BenefitsType.EverydayAims, new short[1] { 0 } } };
    private void OnConnectTimer(object sender)
    {
		try
		{
			if(isConnectTime)
			{
		        ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.HeartPage, parmsHeartPage, true);
//				Debug.Log("--------------timeout:"+timeout);
		        timeout++;
		        if (timeout >= 15)
		        {
//					Debug.Log ("___________Disconnect:Inroom");
		            this.ServerConnected = false;
		            //this.connectTime.Enabled = false;
					isConnectTime=false;
		            timeout = 0;
		        }
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
    }


	public static bool isMessageQueueRunning=true;
    private void OnServiceTimer(object sender,EventArgs e)
    {
        //if (//this.peer.PeerState == PeerStateValue.Connected)
        //{
        //    this.Connect();
        //}
         try
        {

			if(isMessageQueueRunning&&listOperationResponse.Count>0)
			{

				BtnGameManagerBack.operationResponse.Add(listOperationResponse.Dequeue ());
			}

			serverTime=serverTime.AddMilliseconds (m_dwPeerInternal);
			if(isServiceTimer)//this.peer!=null)
			{
				
				if(true)//this.peer.PeerState != PeerStateValue.Disconnected)
				{
					//this.peer.Service();
					//this.peer.DispatchIncomingCommands();
					if(System.Environment.TickCount>=m_dwNextSendTime)
					{
						////this.peer.Service();
						m_dwNextSendTime=System.Environment.TickCount+m_dwKeepAliveInternal;
					}
					/*else
					{
						//this.peer.DispatchIncomingCommands();
							
						if(//this.peer.QueuedIncomingCommands>0 || //this.peer.QueuedOutgoingCommands>0)
						{
							//this.peer.SendOutgoingCommands();
							
						}
					
					}*/
            		
				}
			}
			
        }
        catch(System.Exception ex)
        {
 			Debug.LogError (ex.ToString ());
        }

        
    }
	
	  private int numOutTime=8;
	private int outTimes=3;
	private void OnUpdateTimer(object sender)
	{
		//Debug.LogWarning ("--------------------Num:"+listUpdateTime.Count);
		//Debug.LogWarning ("-----------------"+BtnGameManager.yt.IsUpdate);
		for(int i=0;i<listUpdateTime.Count;i++)
		{
			try
			{
				if(!listUpdateTime[i].Yt.IsUpdate)
				{
					listUpdateTime.RemoveAt(i);
					return;
				}
				listUpdateTime[i].OutTime++;
				if(listUpdateTime[i].OutTime>=numOutTime)
				{
					if(listUpdateTime[i].Status==YuanerServerParmsStatus.Get)
					{
//						Debug.LogWarning ("--------------------Get:"+listUpdateTime[i].Prams);
						////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBGet, listUpdateTime[i].Prams, true);
						listUpdateTime[i].OutTime=0;
					}
					else
					{
						if(listUpdateTime[i].times<=outTimes)
						{
//							Debug.LogWarning ("--------------------Update:"+listUpdateTime[i].Prams);
							//listUpdateTime[i].Yt.IsUpdate=false;
							//dicUpdateTime.Remove (listUpdateTime[i].Yt.TableName);
							////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, listUpdateTime[i].Prams, true);
							listUpdateTime[i].OutTime=0;
							listUpdateTime[i].times++;
							
							//listUpdateTime[i].Dispose ();
						}
						else
						{
							//btnGameManagerBack.isTimeOut=true;
							listUpdateTime[i].Yt.IsUpdate=false;
							dicUpdateTime.Remove (listUpdateTime[i].Yt.TableName);							
						}
					}
				}
			}
			catch(System.Exception ex)
			{
				Debug.LogWarning (ex.ToString ());
			}
		}

	}

    public void Connect()
    {
		ZealmSocketConnection.SetConnectionEvent (this.OnStatusChanged);
		NetDataManager.SetInRoomHandle(this.OnOperationResponse);
		//NetDataManager.DataHandlePhoton+=this.OnOperationResponse;
		ZealmConnector.createConnection(ServerAddress);
    }

    /// <summary>
    /// 重置数据缓存
    /// </summary>
    private void RefreshDic()
    {
		try
		{
			if(inRoomInstantiate!=null)
			{
				inRoomInstantiate.dicUpdateInfoNew.Clear ();
			}
	        listSql.Clear();
	        //dicUpdate.Clear();
	        foreach (YuanTable item in dicTempTable.Values)
	        {
	            item.IsUpdate = false;
	        }
	        dicTempTable.Clear();
			listUpdateTime.Clear ();
			dicUpdateInfo.Clear ();
			dicUpdateInfoNew.Clear ();
			dicUpdateTime.Clear ();
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
    }

    private static Dictionary<string,YuanTable> dicTempTable = new Dictionary<string, YuanTable>();
	private static Dictionary<string,Dictionary<short,object>> dicUpdateInfo=new Dictionary<string, Dictionary<short, object>>();
	private static YuanServerActor<string,YuanServerParms> dicUpdateTime;
	private static List<YuanServerParms> listUpdateTime;
    //private readonly YuanTable EmptyTable = new YuanTable("", "");
    /// <summary>
    /// ŽÓÊýŸÝ¿â»ñµÃTable
    /// </summary>
    /// <param name="strSql">²éÑ¯ÓïŸä</param>
    /// <param name="DateBeas">ÊýŸÝ¿âÃû</param>
    /// <param name="table">±íµÄÃû³Æ</param>
    public void GetYuanTable(string strSql, string DateBeas, YuanTable table)
    {
        if (this.serverConnected) 
        {
            //if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
            //{
			//	table.IsUpdate = true;
            //    Dictionary<short, object> parameter = new Dictionary<short, object>() { { (short)yuan.YuanPhoton.ParameterType.TableName, table.TableName },
			//	{(short)yuan.YuanPhoton.ParameterType.TableSql,strSql},
            //    {(short)yuan.YuanPhoton.ParameterType.DataBeas,DateBeas}};
            //    //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBGet, parameter, true);
            //    dicTempTable.Add(table.TableName, table);
            //    dicUpdateTime.Add (table.TableName,new YuanServerParms(YuanerServerParmsStatus.Get,table,parameter));
            //}
        }
    }
	
    public void GetYuanTable(string strSql, string DateBeas, YuanTable table,string mHost)
    {
        if (this.serverConnected)
        {
            //if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
            //{
			//	table.IsUpdate = true; 
            //    Dictionary<short, object> parameter = new Dictionary<short, object>() { { (short)yuan.YuanPhoton.ParameterType.TableName, table.TableName },
			//	{(short)yuan.YuanPhoton.ParameterType.TableSql,strSql},
            //    {(short)yuan.YuanPhoton.ParameterType.DataBeas,DateBeas}};
			//	parameter.Add ((short)yuan.YuanPhoton.ParameterType.Host,mHost);
            //    //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBGet, parameter, true);
            //    dicTempTable.Add(table.TableName, table);
            //    dicUpdateTime.Add (table.TableName,new YuanServerParms(YuanerServerParmsStatus.Get,table,parameter));
            //}
        }
    }
	
	public void GetYuanTableNew(YuanTable table)
	{
		if (this.serverConnected) 
        {
            if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
            {
				table.IsUpdate = true;
                Dictionary<short, object> parameter = new Dictionary<short, object>() { { (short)yuan.YuanPhoton.ParameterType.TableName, table.TableName },
				};
                ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBGet, parameter, true);
                dicTempTable.Add(table.TableName, table);
                dicUpdateTime.Add (table.TableName,new YuanServerParms(YuanerServerParmsStatus.Get,table,parameter));
            }
        }
	}
	
	

    private static List<string> listSql = new List<string>();
    //private static Dictionary<short, object> dicUpdate = new Dictionary<short, object>();

	private System.Text.StringBuilder strSqlFirst = new System.Text.StringBuilder();
    private System.Text.StringBuilder strSqlLast = new System.Text.StringBuilder();
    private System.Text.StringBuilder strSql = new System.Text.StringBuilder();
    private int numUpdate = 0;
    /// <summary>
    /// žüÐÂTableÊýŸÝ
    /// </summary>
    /// <param name="dateBeas">ÊýŸÝ¿âÃû</param>
    /// <param name="table">±íµÄÃû³Æ</param>
    //public void UpdateYuanTable(string dateBeas, YuanTable table,string mDeviceID)
    //{
    //    
    //    if (this.serverConnected)
    //    {
    //        if (!table.IsUpdate && table.Rows.Count > 0&&!dicTempTable.ContainsKey(table.TableName))
    //        {
	//
	//
	//
	//			table.IsUpdate = true;
	//			if(dicUpdateInfo.ContainsKey (table.TableName))
	//			{
	//				//this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdateInfo[table.TableName], true);
	//				return;
	//			}
	//			Dictionary<short, object> dicUpdate = new Dictionary<short, object>();
    //            strSqlFirst.Length=0;
    //            strSqlLast.Length=0;
    //            strSql.Length=0;
    //            numUpdate = 0;
    //            foreach (YuanRow r in table.Rows)
    //            {
    //                switch (r.RowState)
    //                {
    //                    case YuanRowState.Insert:
    //                        {
    //                            
    //                            strSqlFirst.AppendFormat("Insert into {0}(", table.TableName);
	//							strSqlLast.Length=0;
    //                            strSqlLast.Append (") values('") ;
    //                            numUpdate = 0;
    //                            foreach (string mKey in r.GetColumnsName())
    //                            {
    //                                numUpdate++;
    //                                if (mKey != table.TableKey)
    //                                {
    //                                    if (numUpdate == r.GetColumnsName().Length)
    //                                    {
    //                                        strSqlFirst.Append (mKey);
    //                                        strSqlLast .AppendFormat("{0}')",r[mKey].YuanColumnText);
    //                                    }
    //                                    else
    //                                    {
    //                                        strSqlFirst.AppendFormat ("{0},",mKey);
    //                                        strSqlLast.AppendFormat ("{0}','",r[mKey].YuanColumnText);
    //                                    }
    //                                }
    //                            }
	//							strSql.Length=0;
    //                            strSql.AppendFormat ("{0}{1}",strSqlFirst,strSqlLast);
    //                            listSql.Add(strSql.ToString ());
    //                        }
    //                        break;
    //                    case YuanRowState.Update:
    //                        {
	//							strSql.Length=0;
    //                            strSql.AppendFormat("Update {0} set ", table.TableName);
    //                            numUpdate = 0;
    //                            foreach (string mKey in r.GetColumnsName())
    //                            {
    //                               
    //                                //if (mKey == table.TableKey)
    //                                //{
	//
    //                                //}
    //                                //else if (numUpdate == r.GetColumnsName().Length)
    //                                //{
    //                                //    strSql += string.Format("{0}='{1}' where {2}='{3}'", mKey, r[mKey], table.TableKey, r[table.TableKey]);
    //                                //}
    //                                //else
    //                                //{
    //                                //    strSql += string.Format("{0}='{1}',", mKey, r[mKey]);
    //                                //}
    //                                if (r[mKey].YuanColumnState == YuanRowState.Update)
    //                                {
    //                                        strSql.AppendFormat ("{0}='{1}',", mKey, r[mKey].YuanColumnText);
    //                                        numUpdate++;
    //                                }
    //                            }
    //                            if (numUpdate > 0)
    //                            {
	//								strSql.Remove (strSql.Length-1,1);
    //                                //strSql = strSql.Substring(0, strSql.Length - 1);
    //                                strSql.AppendFormat(" where {0}='{1}'", table.TableKey, r[table.TableKey].YuanColumnText);
    //                                listSql.Add(strSql.ToString ());
    //                            }
    //                        }
    //                        break;
    //                }
    //            }
	//
    //            foreach (YuanRow r in table.DeleteRows)
    //            {
	//				strSql.Length=0;
    //                strSql.AppendFormat("Delete {0} where {1}={2}", table.TableName, table.TableKey, r[table.TableKey].YuanColumnText);
    //                listSql.Add(strSql.ToString ());
    //            }
	//			
    //            dicUpdate.Clear();
    //            if (listSql.Count > 0)
    //            {
    //                numUpdate = 100;
    //                foreach (string sql in listSql)
    //                {
    //                    dicUpdate.Add((short)numUpdate, sql);
    //                    numUpdate++;
    //                    //Debug.Log("T-SQL：" + sql);
    //                }
    //                dicUpdate.Add((short)ParameterType.TableName, table.TableName);
    //                dicUpdate.Add((short)ParameterType.DataBeas, dateBeas);
	//				dicUpdate.Add((short)ParameterType.DeviceID, mDeviceID);
    //                //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdate, true);
	//				dicUpdateInfo.Add (table.TableName,dicUpdate);
    //                
	//
	//
    //                table.Refresh();
    //                dicTempTable.Add(table.TableName, table);
    //                listSql.Clear();
	//				dicUpdateTime.Add (table.TableName,new YuanServerParms(YuanerServerParmsStatus.Update,table,dicUpdate));
    //                //dicUpdate.Clear();
    //                
    //            }
	//			else
	//			{
	//				table.IsUpdate = false;
	//			}
    //        }
    //    }
    //}
	
	public void UpdateYuanTable(string dateBeas, YuanTable table,string mHost,string mDevice)
    {
        
        if (this.serverConnected)
        {
            //if (!table.IsUpdate && table.Rows.Count > 0&&!dicTempTable.ContainsKey(table.TableName))
            //{
			//
			//	table.IsUpdate = true;
			//	if(dicUpdateInfo.ContainsKey (table.TableName))
			//	{
			//		//this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdateInfo[table.TableName], true);
			//		return;
			//	}
			//	Dictionary<short, object> dicUpdate = new Dictionary<short, object>();
            //    strSqlFirst.Length=0;
            //    strSqlLast.Length=0;
            //    strSql.Length=0;
            //    numUpdate = 0;
            //    foreach (YuanRow r in table.Rows)
            //    {
            //        switch (r.RowState)
            //        {
            //            case YuanRowState.Insert:
            //                {
            //                    
            //                    strSqlFirst.AppendFormat("Insert into {0}(", table.TableName);
			//					strSqlLast.Length=0;
            //                    strSqlLast.Append (") values('") ;
            //                    numUpdate = 0;
            //                    foreach (string mKey in r.GetColumnsName())
            //                    {
            //                        numUpdate++;
            //                        if (mKey != table.TableKey)
            //                        {
            //                            if (numUpdate == r.GetColumnsName().Length)
            //                            {
            //                                strSqlFirst.Append (mKey);
            //                                strSqlLast .AppendFormat("{0}')",r[mKey].YuanColumnText);
            //                            }
            //                            else
            //                            {
            //                                strSqlFirst.AppendFormat ("{0},",mKey);
            //                                strSqlLast.AppendFormat ("{0}','",r[mKey].YuanColumnText);
            //                            }
            //                        }
            //                    }
			//					strSql.Length=0;
            //                    strSql.AppendFormat ("{0}{1}",strSqlFirst,strSqlLast);
            //                    listSql.Add(strSql.ToString ());
            //                }
            //                break;
            //            case YuanRowState.Update:
            //                {
			//					strSql.Length=0;
            //                    strSql.AppendFormat("Update {0} set ", table.TableName);
            //                    numUpdate = 0;
            //                    foreach (string mKey in r.GetColumnsName())
            //                    {
            //                       
            //                        //if (mKey == table.TableKey)
            //                        //{
			//
            //                        //}
            //                        //else if (numUpdate == r.GetColumnsName().Length)
            //                        //{
            //                        //    strSql += string.Format("{0}='{1}' where {2}='{3}'", mKey, r[mKey], table.TableKey, r[table.TableKey]);
            //                        //}
            //                        //else
            //                        //{
            //                        //    strSql += string.Format("{0}='{1}',", mKey, r[mKey]);
            //                        //}
            //                        if (r[mKey].YuanColumnState == YuanRowState.Update)
            //                        {
            //                                strSql.AppendFormat ("{0}='{1}',", mKey, r[mKey].YuanColumnText);
            //                                numUpdate++;
            //                        }
            //                    }
            //                    if (numUpdate > 0)
            //                    {
			//						strSql.Remove (strSql.Length-1,1);
            //                        //strSql = strSql.Substring(0, strSql.Length - 1);
            //                        strSql.AppendFormat(" where {0}='{1}'", table.TableKey, r[table.TableKey].YuanColumnText);
            //                        listSql.Add(strSql.ToString ());
            //                    }
            //                }
            //                break;
            //        }
            //    }
			//
            //    foreach (YuanRow r in table.DeleteRows)
            //    {
			//		strSql.Length=0;
            //        strSql.AppendFormat("Delete {0} where {1}={2}", table.TableName, table.TableKey, r[table.TableKey].YuanColumnText);
            //        listSql.Add(strSql.ToString ());
            //    }
            //    dicUpdate.Clear();
            //    if (listSql.Count > 0)
            //    {
            //        numUpdate = 100;
            //        foreach (string sql in listSql)
            //        {
            //            dicUpdate.Add((short)numUpdate, sql);
            //            numUpdate++;
            //            //Debug.Log("T-SQL：" + sql);
            //        }
            //        dicUpdate.Add((short)ParameterType.TableName, table.TableName);
            //        dicUpdate.Add((short)ParameterType.DataBeas, dateBeas);
			//		dicUpdate.Add((short)ParameterType.Host, mHost);
			//		dicUpdate.Add((short)ParameterType.DeviceID, mDevice);
            //        //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdate, true);
			//		dicUpdateInfo.Add (table.TableName,dicUpdate);
            //        
            //        table.Refresh();
            //        dicTempTable.Add(table.TableName, table);
            //        listSql.Clear();
			//		dicUpdateTime.Add (table.TableName,new YuanServerParms(YuanerServerParmsStatus.Update,table,dicUpdate));
            //        //dicUpdate.Clear();
            //        
            //    }
			//	else
			//	{
			//		table.IsUpdate = false;
			//	}
            //}
        }
    }
	
	Dictionary<string,string> dicUpdateInfoNew=new Dictionary<string, string>();
	public void UpdateYuanTable(string dateBeas, YuanTable table,string mDeviceID)
	{
		
		if (this.serverConnected)
        {
			if (!table.IsUpdate && table.Rows.Count > 0&&!dicTempTable.ContainsKey(table.TableName))
			{
				table.IsUpdate = true;
				//if(dicUpdateInfo.ContainsKey (table.TableName))
				//{
				//	//this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdateInfo[table.TableName], true);
				//	return;
				//}
				dicUpdateInfoNew.Clear ();
				if(table.Rows[0].RowState==YuanRowState.Update)
				{
					foreach(string strRow in table.Rows[0].GetColumnsName())
					{
						if(table.Rows[0][strRow].YuanColumnState==YuanRowState.Update)
						{
							dicUpdateInfoNew.Add (strRow,table.Rows[0][strRow].YuanColumnText);
						}
					}
				}
				//Debug.LogWarning ("----------------------YuanUpdate:"+dicUpdateInfoNew.Count);
				if(dicUpdateInfoNew.Count>0)
				{
					string[] strName=new string[dicUpdateInfoNew.Count];
					string[] strValue=new string[dicUpdateInfoNew.Count];
					int num=0;
					foreach(KeyValuePair<string,string> item in dicUpdateInfoNew)
					{
						strName[num]=item.Key;
						strValue[num]=item.Value;
					}
					dicUpdateInfoNew.Keys.CopyTo (strName,0);
					dicUpdateInfoNew.Values.CopyTo(strValue,0);
					Dictionary<short,object> dicUpdate=new Dictionary<short, object>();
					dicUpdate.Add((short)ParameterType.TableName, table.TableName);
					dicUpdate.Add((short)ParameterType.DeviceID, mDeviceID);
					dicUpdate.Add((short)ParameterType.TableKey, strName);
					dicUpdate.Add((short)ParameterType.TableSql, strValue);

					ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.YuanDBUpdate, dicUpdate);
					ZealmConnector.sendRequest(zn);
                    //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdate, true);
				//Debug.Log (string.Format ("---------------Updaet:{0},{1}",strName,strValue));
					//dicUpdateInfo.Add (table.TableName,dicUpdate);
                    
                    table.Refresh();
                    dicTempTable.Add(table.TableName, table);
					dicUpdateTime.Add (table.TableName,new YuanServerParms(YuanerServerParmsStatus.Update,table,dicUpdate));
				}
				else
				{
					table.IsUpdate=false;
				}
			}
		}
	}

    /// <summary>
    /// 发送聊天
    /// </summary>
    /// <param name="messageBodys">ÏûÏ¢ÁÐ±í</param>
    /// <param name="messageType">ÏûÏ¢œÓÊÕ·¶Î§</param>
    /// <param name="messageText">ÏûÏ¢ÎÄ±Ÿ</param>
    /// <param name="messageText">ÏûÏ¢·¢ËÍÕß</param>
    public void SendMessage(string[] messageBodys, yuan.YuanPhoton.MessageType messageType, string messageText,string sender)
    {
        if (this.ServerConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.MessageBodys, messageBodys);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.MessageType, (short)messageType);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.MessageText, messageText);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.MessageSender, sender);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SendMessage, parameter);
			ZealmConnector.sendRequest(zn);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendMessage, parameter, true);
        }
    }

    /// <summary>
    /// 发送电视消息
    /// </summary>
    /// <param name="mText"></param>
    public void SendTVMessage(string mText)
    {
        if (this.ServerConnected)
        {

            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.MessageText, mText);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SendTVMessage, parameter);
			ZealmConnector.sendRequest(zn);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendTVMessage, parameter, true);
        }
    }


    /// <summary>
    /// œøÈë·¿Œä
    /// </summary>
    /// <param name="id">ÍæŒÒid</param>
	public void SendID(string id,string pro,string nickname,bool isReOnline,string mLanguageVersion,string mDeviceID,int mMapID,float mPositionX,float mPositionY,float mPositionZ)//,int teaminstenid)
    {
        if (this.ServerConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, id);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNickName, nickname);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPro, pro);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.IsReOnline, isReOnline);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.LangugeVersion, mLanguageVersion);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, mDeviceID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.RoomName, mMapID);
			parameter.Add((short)Zealm.ParameterType.x, mPositionX);
			parameter.Add((short)Zealm.ParameterType.y, mPositionY);
			parameter.Add((short)Zealm.ParameterType.z, mPositionZ);
     //       parameter.Add((short)Zealm.ParameterType.TeamInfo, teaminstenid);

            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.EnterGame, parameter);
			ZealmConnector.sendRequest(zn);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SetID, parameter, true);

        }
    }
	
	public void SendJoinRoom(string _strRoom)
	{
		 if (this.ServerConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add(1, _strRoom);
            //this.peer.OpCustom((short)210, parameter, true);
        }
		
	}
	
    public void PlayerCreat(string passID, string proID, string nickName, string dateBase, string table,string mDeviceID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.UserID, passID);
            parameter.Add((short)Zealm.ParameterType.PlayerType, proID);
            parameter.Add((short)Zealm.ParameterType.UserNickName, nickName);
            parameter.Add((short)Zealm.ParameterType.DataBeas, dateBase);
            parameter.Add((short)Zealm.ParameterType.TableName, table);
            parameter.Add((short)Zealm.ParameterType.ServerName, PlayerPrefs.GetString("InAppServer"));
            parameter.Add((short)Zealm.ParameterType.DeviceID, mDeviceID);
            parameter.Add((short)Zealm.ParameterType.SDK, TableRead.strPageName);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PlayerCreat, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PlayerCreat, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }
	
    /// <summary>
    /// »ñÈ¡ÍæŒÒÐÅÏ¢
    /// </summary>
    /// <param name="ids">ÍæŒÒidÁÐ±í</param>
    /// <param name="yt">YuanTable</param>
    /// <param name="dateBase">ÊýŸÝ¿â</param>
    /// <param name="table">±í</param>
    public void GetPlayerList(string[] ids,YuanTable yt, string dateBase, string table)
    {
        if (this.serverConnected && !yt.IsUpdate && !dicTempTable.ContainsKey(yt.TableName))
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, ids);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, yt.TableName);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetPlayerList, parameter);
            ZealmConnector.sendRequest(zn);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetPlayerList, parameter, true);
            dicTempTable.Add(yt.TableName, yt);
            yt.IsUpdate = true;
        }
    }

    public void GetGuildPlayerList(string guildID, YuanTable yt, string dateBase, string table)
    {
        if (this.serverConnected && !yt.IsUpdate && !dicTempTable.ContainsKey(yt.TableName))
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
        }
    }

    /// <summary>
    /// žùŸÝêÇ³Æ»ñµÃÍæŒÒID
    /// </summary>
    /// <param name="playerName">ÍæŒÒêÇ³Æ</param>
    /// <param name="dateBase">ÊýŸÝ¿â</param>
    /// <param name="table">±í</param>
    public void GetPlayerID(string playerName,string dateBase,string table)
    {
        if (this.serverConnected)
        {
			
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNickName,playerName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetPlayerID, parameter);
            ZealmConnector.sendRequest(zn);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetPlayerID,parameter,true);
        }
    }

    /// <summary>
    /// ÌíŒÓœÇÉ«µœºÚÃûµ¥
    /// </summary>
    /// <param name="playerName">ÍæŒÒêÇ³Æ</param>
    /// <param name="dateBase">ÊýŸÝ¿â</param>
    /// <param name="table">±í</param>
    public void BlackFirend(string playerName, string dateBase, string table)
    {
        if (this.serverConnected)
        {
            GetPlayerID(playerName, dateBase, table);
           
            btnGameManagerBack.isBlackFirend = true;
        }
    }

    /// <summary>
    /// 判定是否是队长
    /// </summary>
    public void IsTeamHead()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.IsTeamHead, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 小队人员晋升
    /// </summary>
    /// <param name="playerID">角色ID</param>
    public void TeamPlayerUp(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamPlayerUp, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamPlayerUp, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 军团人员晋升
    /// </summary>
    /// <param name="playerID">角色ID</param>
    public void LegionPlayerUp(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionPlayerUp, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LegionPlayerUp, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// PVP战队人员晋升
    /// </summary>
    /// <param name="playerID">角色ID</param>
    /// <param name="teamID">队伍ID</param>
    public void PVPPlayerUp(string playerID, CorpType corpType)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.CorpType, corpType);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PVPPlayerUp, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPPlayerUp, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 公会人员权限设置
    /// </summary>
    /// <param name="playerID">角色ID</param>
    /// <param name="isUp">晋升或降级</param>
    public void GuildPlayerPurview(string playerID, bool isUp)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.UserID, playerID);
            parameter.Add((short)Zealm.ParameterType.PlayerPurview, isUp);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildPlayerPurview, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildPlayerPurview, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    

    /// <summary>
    /// ÌíŒÓºÃÓ
    /// </summary>
    /// <param name="playerName">ÍæŒÒêÇ³Æ</param>
    /// <param name="dateBase">ÊýŸÝ¿â</param>
    /// <param name="table">±í</param>
    public void AddFirend(string playerName, string dateBase, string table)
    {
		//Debug.Log (string.Format ("-------------{0},{1},{2}",playerName,dateBase,table));
        if (this.serverConnected)
        {
            GetPlayerID(playerName, dateBase, table);
            btnGameManagerBack.isFirend = true;
        }
    }
   
    /// <summary>
    /// ŽŽœš¶ÓÎé
    /// </summary>
    /// <param name="teamName"></param>
    public void TeamCreate(string teamName)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamName, teamName);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamCreate, parameter);
            ZealmConnector.sendRequest(zn);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamCreate, parameter, true);
            BtnGameManager.isTeamCreat = true;
        }
    }

    /// <summary>
    /// »ñÈ¡¶ÓÎéÁÐ±í
    /// </summary>
    public void GetTeams(string teamLevel)
    {
        if (this.serverConnected)
        {
            
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamLevel, teamLevel);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetTeam, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetTeam, parameter);
            ZealmConnector.sendRequest(zn);

        }
    }

    /// <summary>
    /// »ñÈ¡×ÔÉí¶ÓÎé
    /// </summary>
    /// <param name="playerID"></param>
    public void GetMyTeams(YuanTable yt, string dateBase, string table)
    {
       
        if (this.serverConnected && !yt.IsUpdate && !dicTempTable.ContainsKey(yt.TableName))
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, yt.TableName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);

            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetMyTeam, parameter);
            ZealmConnector.sendRequest(zn);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetMyTeam, parameter, true);
            dicTempTable.Add(yt.TableName, yt);
            yt.IsUpdate = true;
        }

    }

    /// <summary>
    /// 获取自身战队人员列表
    /// </summary>
    /// <param name="yt"></param>
    /// <param name="dateBase"></param>
    /// <param name="table"></param>
    public void GetMyLegion(YuanTable yt, string dateBase, string table)
    {
        if (this.serverConnected && !yt.IsUpdate && !dicTempTable.ContainsKey(yt.TableName))
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, yt.TableName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetMyLegion, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetMyLegion, parameter);
            ZealmConnector.sendRequest(zn);
            dicTempTable.Add(yt.TableName, yt);
            yt.IsUpdate = true;
        }

    }

    /// <summary>
    /// ŒÓÈë¶ÓÎé
    /// </summary>
    /// <param name="teamID">¶ÓÎéID</param>
    public void TeamAdd(string teamID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamID, teamID);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamAdd, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 邀请玩家加入队伍
    /// </summary>
    /// <param name="playerID"></param>
    public void TeamInviteAdd(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamInviteAdd, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamInviteAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 退出队伍
    /// </summary>
    public void TeamRemove(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.UserID, playerID);
 
			// //this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.TeamRemove, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamRemove, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }


    /// <summary>
    /// 解散队伍
    /// </summary>
    public void TeamDissolve()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamDissolve, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamDissolve, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获取玩家所在队伍ID
    /// </summary>
    /// <param name="playerID"></param>
    public void GetPlayerTeamID(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetPlayerTeamID, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetPlayerTeamID, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    // <summary>
    // 从队伍中移除某人
    // </summary>
    // <param name="playerID">角色ID</param>
    // <param name="teamID">队伍ID</param>
    public void TeamPlayerRemove(string playerID, string teamID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamID, teamID);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamPlayerRemove, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TeamPlayerRemove, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }


    /// <summary>
    /// 返回请求数据
    /// </summary>
    /// <param name="returnCode">»ØÓŠŽð°ž</param>
    /// <param name="requstType">ÇëÇóÀàÐÍ</param>
    /// <param name="list">Ïà¹ØÄÚÈÝ</param>
    public void ReturnRequest(ReturnCode returnCode,Dictionary<short,object> parameter)
    {
        if (this.serverConnected)
        {
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RetureRequestType, (short)returnCode);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ReturnRequest, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.ReturnRequest, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// Õœ¶ÓœšÁ¢
    /// </summary>
    /// <param name="GorpsName">Õœ¶ÓÃû³Æ</param>
    /// <param name="playerNum">ÍæŒÒÊýÁ¿</param>
    /// <param name="dateBase">ÊýŸÝ¿â</param>
    /// <param name="table">±í</param>
    public void GorpsCreate(string GorpsName, string playerNum,string picID, string dateBase, string table)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamName, GorpsName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNumber, playerNum);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.PicID, picID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GropsCreate, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GropsCreate, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// ŒÓÈëÕœ¶Ó
    /// </summary>
    /// <param name="gorpsID"></param>
    public void GorpsAdd(string gorpsID,string dateBase, string table)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamID, gorpsID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GropsAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GropsAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 邀请玩家加入PVP
    /// </summary>
    /// <param name="corpType">PVP类型</param>
    /// <param name="playerID">角色ID</param>
    public void GorpsInviteAdd(CorpType corpType,string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.CorpType, corpType);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PVPInviteAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPInviteAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// »ñÈ¡Õœ¶ÓÁÐ±í
    /// </summary>
    /// <param name="corpsLevel">Õœ¶ÓµÈŒ¶</param>
    /// <param name="dateBase">ÊýŸÝ¿â</param>
    /// <param name="table">±í</param>
    public void GetCorps(string corpsLevel, string dateBase, string table)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamLevel, corpsLevel);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, this.ServerApplication);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetGrops, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetGrops, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }


    /// <summary>
    /// 获取自身的PVP小队信息
    /// </summary>
    /// <param name="corpType">PVP类型</param>
    /// <param name="yt">表</param>
    public void GetMyCorp(CorpType corpType,YuanTable yt)
    {
        if (this.serverConnected && !yt.IsUpdate && !dicTempTable.ContainsKey(yt.TableName))
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, yt.TableName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.CorpType, corpType);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetMyGrop, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetMyGrop, parameter);
            ZealmConnector.sendRequest(zn);
            dicTempTable.Add(yt.TableName, yt);
            yt.IsUpdate = true;
        }
    }

  /// <summary>
    /// 退出PVP小队
  /// </summary>
  /// <param name="teamID">队伍ID</param>
    public void CorpRemove(string teamID,string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamID, teamID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
           // //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GropsRemove, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GropsRemove, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 交易请求
    /// </summary>
    /// <param name="playerID">请求者id</param>
    public void TransactionRequest(string playerID)
    {
//        Debug.Log("transaction---------------------playerID:"  + playerID);
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TransactionRequest, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 发送交易信息
    /// </summary>
    /// <param name="mTransactionID">交易ID</param>
    /// <param name="mItemID">装备id</param>
    /// <param name="mBloodStone">血石</param>
    /// <param name="mGolds">金币</param>
    /// <param name="mIsReady">是否准备好</param>
    /// <param name="mIsTransaction">是否确定交易</param>
    public void SendTransactionInfo(string mTransactionID, string mItemID, string mBloodStone, string mGolds, bool mIsReady, bool mIsTransaction)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.TransactionID, mTransactionID);
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.ItemID, mItemID);
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.BloodStone, mBloodStone);
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.Golds, mGolds);
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.IsReady, mIsReady);
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.IsTransaction, mIsTransaction);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SendTransactionInfo, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 关闭交易
    /// </summary>
    /// <param name="mTransactionID">交易ID</param>
    public void TransactionClose(string mTransactionID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.TransactionInfo.TransactionID, mTransactionID);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.TransactionClose, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    #region 拍卖行

    /// <summary>
    /// 一口价拍卖
	/// </summary>
	/// <param name="itemIDAndCount">物品id和数量的字符串</param>
	/// <param name="fixedPrice">拍卖价格（只能是血石个数）</param>
	/// <param name="auctionTime">拍卖时长</param>
    public void FixedPriceAuction(string itemIDAndCount, int fixedPrice, int auctionTime)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)AuctionCompanyParams.AuctionCompanyType, (short)AuctionCompanyType.FixedPriceAuction);
            parameter.Add((short)AuctionCompanyParams.ItemIDAndCount, itemIDAndCount);
            parameter.Add((short)AuctionCompanyParams.FixedPrice, fixedPrice);
            parameter.Add((short)AuctionCompanyParams.AuctionTime, auctionTime);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.AuctionCompany, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 搜索拍卖品
    /// </summary>
    /// <param name="minLvl">最小等级</param>
    /// <param name="maxLvl">最大等级</param>
    /// <param name="quality">品质</param>
    /// <param name="equip">装备类型</param>
    /// <param name="mat">材料类型</param>
    public void AuctionSearch(int minLvl, int maxLvl, int[] quality, int[] equip, int[] mat)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)AuctionCompanyParams.AuctionCompanyType, (short)AuctionCompanyType.AuctionSearch);
            parameter.Add((short)AuctionCompanyParams.MinLvl, minLvl);
            parameter.Add((short)AuctionCompanyParams.MaxLvl, maxLvl);
            
            parameter.Add((short)AuctionCompanyParams.ItemQuality, quality);
            parameter.Add((short)AuctionCompanyParams.EquipType, equip);
            parameter.Add((short)AuctionCompanyParams.MatType, mat);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.AuctionCompany, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 购买拍卖品
    /// </summary>
    /// <param name="auctionID">拍卖的id</param>
    public void BuyAuctions(int auctionID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)AuctionCompanyParams.AuctionCompanyType, (short)AuctionCompanyType.BuyAuctions);
            parameter.Add((short)AuctionCompanyParams.AuctionID, auctionID);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.AuctionCompany, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 玩家的拍卖信息
    /// </summary>
    public void PlayerAuctionInfo()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)AuctionCompanyParams.AuctionCompanyType, (short)AuctionCompanyType.PlayerAuctionInfo);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.AuctionCompany, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 购买拍卖位
    /// </summary>
    /// <param name="count"></param>
    public void BuyAuctionSlot(int count)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)AuctionCompanyParams.AuctionCompanyType, (short)AuctionCompanyType.BuyAuctionSlot);
            parameter.Add((short)AuctionCompanyParams.AuctionSlotCount, count);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.AuctionCompany, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    #endregion

    /// <summary>
    /// 装备拆分
    /// </summary>
    /// <param name="itemID">装备id</param>
    /// <param name="useBlood">是否使用血石</param>
    public void EquipmentResolve(string itemID, bool useBlood)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.ItemID, itemID);
            parameter.Add((short)Zealm.ParameterType.UseBlood, useBlood);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.EquipmentResolve, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 请求服务器加体力，数据库字段为“Power”
    /// </summary>
    /// <param name="power">增加的体力值</param>
    public void AddPower(int power)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.power, power);
            
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.AddPower, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    #region 宝藏相关
    /// <summary>
    /// 请求服务器返回宝藏九宫格抽奖初始显示血石的值
    /// </summary>
    public void GambleInfo()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Gamble_Info, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 九宫格翻牌请求
    /// </summary>
    public void GambleCard(bool isCostBlood)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            //parameter.Add((short)Zealm.ParameterType.ItemIDS, itemIDs);
            parameter.Add((short)Zealm.ParameterType.IsWin, isCostBlood);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Gamble_Card, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 九宫格开始抽奖请求
    /// </summary>
    //public void GambleLottery(string itemID, int index, bool isCostBlood)
    public void GambleLottery(bool isCostBlood)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            //parameter.Add((short)Zealm.ParameterType.ItemID, itemID);
            //parameter.Add((short)Zealm.ParameterType.SolutionNum, index);
            parameter.Add((short)Zealm.ParameterType.IsWin, isCostBlood);

            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Gamble_Lottery, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }
    #endregion

    /// <summary>
    /// 请求服务器修改精铁粉末和精金结晶
    /// </summary>
    /// <param name="marrowIron">精铁粉末</param>
    /// <param name="marrowGold">精金结晶</param>
    public void ModifyMarrow(int marrowIron, int marrowGold)
    {
        if (this.serverConnected)
        {
            //Dictionary<short, object> parameter = new Dictionary<short, object>();
            //parameter.Add((short)Zealm.ParameterType.MarrowIron, marrowIron);
            //parameter.Add((short)Zealm.ParameterType.MarrowGold, marrowGold);

            //ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ModifyMarrow, parameter);
            //ZealmConnector.sendRequest(resp);
        }
    } 


    /// <summary>
    /// 自由军团创建
    /// </summary>
    /// <param name="mLegionName">军团名称</param>
    public void LegionTempCreate(string mLegionName,string picID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamName, mLegionName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.PicID, picID);
           ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionTempCreate, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LegionTempCreate, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 自由军团加入
    /// </summary>
    /// <param name="mLegionID"></param>
    public void LegionTempAdd(string mLegionID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamID, mLegionID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionTempAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LegionTempAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 铁血军团创建
    /// </summary>
    /// <param name="mLegionName">军团名称</param>
    public void LegionDBCreate(string mLegionName, string picID, string dateBase, string table)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamName, mLegionName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.PicID, picID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionDBCreate, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LegionDBCreate, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

   /// <summary>
   /// 铁血军团加入
   /// </summary>
   /// <param name="mLegionID">军团ID</param>
    public void LegionDBAdd(string mLegionID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamID, mLegionID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionDBAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LegionDBAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 邀请玩家加入
    /// </summary>
    /// <param name="playerID"></param>
    public void LegionInviteAdd(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionInviteAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LegionInviteAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 退出军团
    /// </summary>
    public void LegionRemove(string playerID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.RemoveLegion, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.RemoveLegion, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获取军团
    /// </summary>
    /// <param name="mLegionLevel"></param>
    public void GetLegion(string mLegionLevel)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamLevel, mLegionLevel);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetLegion, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetLegion, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }



    /// <summary>
    /// 公会创建
    /// </summary>
    /// <param name="mName"></param>
    public void GuildCreate(string mName,string mPicID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.TeamName, mName);
            parameter.Add((short)Zealm.ParameterType.PicID, mPicID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildCreate, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildCreate, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 加入公会
    /// </summary>
    /// <param name="guildID"></param>
    public void GuildAdd(string guildID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.TeamID, guildID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 公会邀请加入
    /// </summary>
    /// <param name="playerID"></param>
    public void GuildInviteAdd(string playerID,string teamID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.UserID, playerID);
            parameter.Add((short)Zealm.ParameterType.TeamID, teamID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildInviteAdd, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildInviteAdd, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }


    /// <summary>
    /// 公会移除会员
    /// </summary>
    /// <param name="playerID">会员ID</param>
    public void GuildRemove(string playerID,string teamID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.UserID, playerID);
            parameter.Add((short)Zealm.ParameterType.TeamID, teamID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildRemove, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildRemove, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 公会玩家禁言
    /// </summary>
    /// <param name="playerID"></param>
    public void GuildStopTalk(string playerID,string teamID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.UserID, playerID);
            parameter.Add((short)Zealm.ParameterType.TeamID, teamID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildStopTalk, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildStopTalk, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }


  /// <summary>
  /// 公会建设
  /// </summary>
  /// <param name="guildID">公会ID</param>
  /// <param name="buildGoldNum">贡献金币数</param>
  /// <param name="buildBloodStoneNum"></param>
    public void GuildBuild(string guildID,MoneyType moneyType)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.TeamID, guildID);
            parameter.Add((short)Zealm.ParameterType.MoneyType, (short)moneyType);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildBuild, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildBuild, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 公会资金
    /// </summary>
    /// <param name="guildID"></param>
    /// <param name="fundsGoldNum"></param>
    /// <param name="fundsBloodStoneNum"></param>
    public void GuildFunds(string guildID, MoneyType moneyType,int money) //注释地方先去掉，等待UI修改后打开
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.TeamID, guildID);
            parameter.Add((short)Zealm.ParameterType.MoneyType, (short)moneyType);   
            parameter.Add((short)Zealm.ParameterType.MoneyNumb, money);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildFund, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 公会升级
    /// </summary>
    /// <param name="guildID"></param>
    public void GuildLevelUp(string guildID,bool ClickORYes)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.TeamID, guildID);
            parameter.Add((short)Zealm.ParameterType.Value, ClickORYes);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildLevelUp, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildLevelUp, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获得公会所有成员
    /// </summary>
    public void GetGuildAll()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetGuildAll, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetGuildAll, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 公会解散
    /// </summary>
    /// <param name="playerID">角色ID</param>
    /// <param name="isUp">晋升或降级</param>
    public void GuildDismiss(string guildID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.GuildID, guildID);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GuildPlayerPurview, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GuildDismiss, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }
    /// <summary>
    /// 获得打折商品
    /// </summary>
    public void GetFavorableItem()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetFavorableItem, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetFavorableItem, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获得所有道具商品
    /// </summary>
    public void GetItems()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetItems, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetItems, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获得所有装备商品
    /// </summary>
    public void GetEquipment()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetEquipment, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetEquipment, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 购买物品
    /// </summary>
    /// <param name="id">物品的数据id</param>
    public void BuyItem(string id)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.ItemID, id);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BuyItem, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BuyItem, parameter);
			ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="mMailTitle">邮件主题</param>
    /// <param name="mMailAddressee">收件人</param>
    /// <param name="mMailText">邮件正文</param>
    /// <param name="mMailTool1">邮件附件</param>
    /// <param name="mIsPaymentPickup">是否付款取件</param>
    /// <param name="mGold">金币</param>
    /// <param name="mBloodStone">血石</param>
	public void MailSend( string mMailTitle, string mMailAddressee, string mMailText, string mMailTool1, string mIsPaymentPickup, string mGold, string mBloodStone,bool isGM)// int mPageNum,int mInPageNum)
    {
        if (this.serverConnected)
        {
           
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.MailTitle, mMailTitle);
            parameter.Add((short)ParameterType.MailAddressee, mMailAddressee);
            parameter.Add((short)ParameterType.MailText, mMailText);
            parameter.Add((short)ParameterType.MailTool1, mMailTool1);
            parameter.Add((short)ParameterType.isPaymentPickup, mIsPaymentPickup);
            parameter.Add((short)ParameterType.Gold, mGold);
            parameter.Add((short)ParameterType.BloodStone, mBloodStone);
            parameter.Add((short)ParameterType.MailType, isGM);
		//	parameter.Add((short)ParameterType.NumStart, mPageNum);
          //  parameter.Add((short)ParameterType.NumEnd, mInPageNum);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.MailSend, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.MailSend, parameter);
            ZealmConnector.sendRequest(zn);
            
        }
    }

   /// <summary>
   /// 获取发件箱列表
   /// </summary>
    public void MailGetOutList()
    {
        if (this.serverConnected)
        {
            //Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.MailGetOut, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.MailGetOut, null);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获取收件箱列表
    /// </summary>
    public void MailGetInList()
    {
        if (this.serverConnected)
        {
            //Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.MailGetIn, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.MailGetIn, null);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 读取邮件
    /// </summary>
    /// <param name="mailID"></param>
    public void MailRead(string mailID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.MailID, mailID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.MailRead, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.MailRead, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获得邮件附件
    /// </summary>
    /// <param name="mailID">邮件ID</param>
    public void MailGetTool(string mailID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.MailID, mailID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetMailTool, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetMailTool, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 删除邮件
    /// </summary>
    /// <param name="mailID">邮件ID</param>
    public void MailDelete(string mailID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.MailID, mailID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.MailDelete, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.MailDelete, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 获取活动
    /// </summary>
    public void GetActivity()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetActivity, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetActivity, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 上传排行榜分数
    /// </summary>
    /// <param name="fraction">分数</param>
    /// <param name="type">排行榜</param>
    public void SendRanking(int fraction,RankingType type)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.Fraction, fraction);
            parameter.Add((short)ParameterType.RankingType, type);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendRanking, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SendRanking, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// Home按键加入与解除
    /// true为按下，false为回来
    /// </summary>
    public void InHomeQueue(bool InOrOut)
    {
        if(this.serverConnected)
        {
            ZMNetData zm = new ZMNetData((short)OpCode.HomeQueue);
            zm.writeBoolean(InOrOut);
            ZealmConnector.sendRequest(zm);
        }      
    }
    /// <summary>
    /// 获取连续登陆奖励
    /// </summary>
    public void GetDailyBenefits()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetDailyBenefits, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetDailyBenefits, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 设置荣耀并检测
    /// </summary>
    /// <param name="honorType">类型</param>
    /// <param name="mValue">值，0为只检测，其他为递增1并检测</param>
    public void SetHonor(HonorType honorType,int mValue)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.HonorType, (short)honorType);
            parameter.Add((short)ParameterType.Value, mValue);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SetHonor, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SetHonor, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

	public void SetHonorGetEquipItem(string mValue)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)ParameterType.HonorType, (short)HonorType.GetEquipItem);
			parameter.Add((short)ParameterType.Value, mValue);
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SetHonor, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SetHonor, parameter);
			ZealmConnector.sendRequest(zn);
		}
	}

    /// <summary>
    /// 设置称号并检测
    /// </summary>
    /// <param name="mTitleType">类型</param>
    /// <param name="mValue"></param>
    public void SetTitle(TitleType mTitleType, int mValue)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.TitleType, (short)mTitleType);
            parameter.Add((short)ParameterType.Value, mValue);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SetTitle, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.SetTitle, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 建立PVP小队
    /// </summary>
    public void PVPTeamCreate(string teamID,int mUserNum)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.TeamID, teamID);
            parameter.Add((short)ParameterType.UserNumber, mUserNum);
            parameter.Add((short)ParameterType.TeamLevel, BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText);
			Debug.Log(parameter[(short)ParameterType.TeamLevel] + " ===== 1123123123");

			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPCreate, parameter);            
            ZealmConnector.sendRequest(zn);
        }
    }

	public void PVPCancel(string battlefieldID)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)0, battlefieldID);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPCancel, parameter);            
			ZealmConnector.sendRequest(zn);
		}
	}

	/// <summary>
	/// 建立PVP小队
	/// </summary>
	public void PVPTeamCreate(string battlefildID)
	{
		if (this.serverConnected)
		{
            PlayerUtil.battlefieldId = battlefildID;
			Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)0, battlefildID);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPCreate, parameter);
            ZealmConnector.sendRequest(zn);
		}
	}

    public void GetPVETeamList()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.TeamLevel, BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetPVETeamList, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.GetPVETeamList, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 解散PVP小队
    /// </summary>
    public void PVPTeamDissolve()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PVPDissolve, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPDissolve, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// PVP小队加入队列
    /// </summary>
    public void LegionPVPAddQueue()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.AddLegionPVPQueue, parameter, true);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.AddLegionPVPQueue, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 发送PVP排队取消
    /// </summary>
    public void PVPInviteIsNo()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.PVPInviteIsNo, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 发送PVP小队消息
    /// </summary>
    public void SendLegionPVPInfo(int isStart)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)0, isStart);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendLegionPVPInfo, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.SendLegionPVPInfo, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 军团小队加入队列
    /// </summary>
    public void LegionAddQueue()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.AddLegionQueue, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.AddLegionQueue, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 发送军团小队消息
    /// </summary>
    public void SendLegionInfo(bool isStart)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)TransactionInfo.IsTransaction, isStart);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendLegionInfo, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.SendLegionInfo, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 加入临时队伍
    /// </summary>
    public void AddTempTeam(string mTeamName,string mTeamID,string pass="")
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.TeamName,mTeamName);
            parameter.Add((short)ParameterType.TeamID, mTeamID);
            parameter.Add((short)ParameterType.ItemID,pass);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.AddTempTeam, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.AddTempTeam, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>]
    ///发送是否准备
    /// </summary>
    /// <param name="isStart"></param>
    public void SendTeamTeamInfo(bool isStart)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)TransactionInfo.IsReady, isStart);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendTeamTeamInfo, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.SendTeamTeamInfo, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 退出临时队伍
    /// </summary>
    public void RemoveTempTeam()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.RemoveTempTeam, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.RemoveTempTeam, parameter);
            ZealmConnector.sendRequest(zz);
            Debug.Log("exit temp FB-------------------------------------------------------");
        }
    }

    /// <summary>
    /// 获取临时队伍列表
    /// </summary>
    /// <param name="mTeamLevel"></param>
    public void GetTempTeams(string mTeamLevel)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.TeamLevel, mTeamLevel);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetTempTeam, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetTempTeam, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 设置玩家行为记录
    /// </summary>
    /// <param name="mType"></param>
    /// <param name="mValue"></param>
    public void SetSetPlayerBehavior(PlayerBehaviorType mType, string mValue)
    {
        if (this.serverConnected)
        {
			try{
				if(mType == PlayerBehaviorType.GetItem){
					if(mValue.Length > 3 && int.Parse(mValue.Substring(0,1)) < 7){
						SetHonorGetEquipItem(mValue);
					}
				}
			}catch(System.Exception e){
				
			}
			Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.PlayerBehaviorType, (short)mType);
            parameter.Add((short)ParameterType.PlayerBehaviorValue, mValue);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.SetPlayerBehavior, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 战场排队
    /// </summary>
    public void LegionOneAdd(string mLevel)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.LegionOneMap, mLevel);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionOneAdd, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.LegionOneAdd, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 退出战场
    /// </summary>
    public void LegionOneRemove()
    {
        return;
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionOneRemove, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.LegionOneRemove, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

 /// <summary>
 /// 加入指定ID的战场
 /// </summary>
 /// <param name="mTeamID"></param>
 /// <param name="mMap"></param>
    public void LegionOneTeamAdd(int mTeamID,string mMap)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.LegionOneID,mTeamID);
            parameter.Add((short)ParameterType.LegionOneMap,InRoom.isUpdatePlayerLevel?InRoom.playerLevel: BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionOneTeamAdd, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.LegionOneTeamAdd, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 战场列表
    /// </summary>
    public void GetLegionOneList()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.TeamLevel,InRoom.isUpdatePlayerLevel?InRoom.playerLevel: BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LegionOneList, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.LegionOneList, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	
	    /// <summary>
    /// 活动战场排队
    /// </summary>
    public void ActivityPVPAdd(string mMap)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.LegionOneMap, mMap);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ActivityPVPAdd, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ActivityPVPAdd, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 退出活动战场
    /// </summary>
    public void ActivityPVPRemove()
    {
        //目前统一用新战场逻辑
        return;
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ActivityPVPRemove, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ActivityPVPRemove, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	
	/// <summary>
	/// 检查兑换码
	/// </summary>
	/// <param name='mCode'>
	/// 兑换码
	/// </param>
	public void RedemptionCode(string mCode)
	{
		 if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)ParameterType.ItemID, mCode);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.RedemptionCode, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.RedemptionCode, parameter);
            ZealmConnector.sendRequest(zz);
        }
	}

    /// <summary>
    /// 转换Photon主客户端
    /// </summary>
    /// <returns></returns>
    public bool SetPhotonMasterClient()
    {
        PhotonPlayer[] otherPlayers = PhotonNetwork.otherPlayers;
        if (otherPlayers.Length > 0)
        {
            if (PhotonNetwork.SetMasterClient(otherPlayers[0]))
            {
                //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                return true;
            }
            else
            {
                //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
                return false;
            }
        }
        else
        {
            //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
            return true;
        }
    }

    /// <summary>
    /// 转换Photon主客户端
    /// </summary>
    /// <returns></returns>
    public bool SetPhotonMasterClient(PhotonPlayer mDeletePlayer)
    {
        PhotonPlayer[] otherPlayers = PhotonNetwork.otherPlayers;
        if (otherPlayers.Length > 0)
        {
            List<int> tempPlayerIDs = new List<int>();
            foreach (PhotonPlayer item in otherPlayers)
            {
				if(item.ID!=mDeletePlayer.ID)
                tempPlayerIDs.Add(item.ID);
            }
            tempPlayerIDs.Sort();

            PhotonPlayer tempPalyer = PhotonPlayer.Find(tempPlayerIDs[0]);
           
            if (tempPalyer == null)
            {
                //PhotonNetwork.DestroyPlayerObjects(mDeletePlayer);
                return false;
            }
            if (PhotonNetwork.SetMasterClient(tempPalyer))
            {
                //PhotonNetwork.DestroyPlayerObjects(mDeletePlayer);
                return true;
            }
            else
            {
                //PhotonNetwork.DestroyPlayerObjects(mDeletePlayer);
                return false;
            }
        }
        else
        {
            if (PhotonNetwork.SetMasterClient(PhotonNetwork.player))
            {
                //PhotonNetwork.DestroyPlayerObjects(mDeletePlayer);
                return true;
            }
            else
            {
                //PhotonNetwork.DestroyPlayerObjects(mDeletePlayer);
                return false;
            }
        }
    }
	


    /// <summary>
    /// 获取炼金加成时间段
    /// </summary>
    /// <returns></returns>
    public string GetMakeGoldTime()
    {
		//return (string)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldTime];
        return (string)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldTime];
    }

    /// <summary>
    /// 获取炼金固定比例
    /// </summary>
    /// <returns></returns>
    public string GetMakeGoldScale()
    {
		//return (string)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldScale];
        return (string)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldScale];
    }


    /// <summary>
    /// 获取炼金加成
    /// </summary>
    /// <returns></returns>
    public int GetMakeGoldAddtion()
    {
		//return (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldAddtion];
        return (int)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.MakeGoldAddtion];
    }


    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="mRoomName">房间名称</param>
    public void InGameRoom(string mRoomName)
    {
        if (this.serverConnected&&mRoomName!="")
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.RoomName,mRoomName );
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.InRoom, parameter, true);
                        ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.InRoom, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }

    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="mRoomName">房间名称</param>
    public void LeaveRoom(string mRoomName)
    {
        if (this.serverConnected && mRoomName != "")
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.RoomName, mRoomName);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LeaveRoom, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.LeaveRoom, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }
	
	
	/// <summary>
	/// 邀请玩家决斗
	/// </summary>
	/// <param name='mPlayerID'>
	/// 邀请的玩家ID
	/// </param>
	public void PVP1Invite(string mPlayerID)
	{
		if (this.serverConnected )
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)0, mPlayerID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PVP1Invite, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.PVP1Invite, parameter);
            ZealmConnector.sendRequest(zz);
        }
	}
	
	/// <summary>
	/// 退出决斗状态
	/// </summary>
	public void PVP1InviteRemove()
	{
		if (this.serverConnected )
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PVP1InviteRemove, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.PVP1InviteRemove, parameter);
            ZealmConnector.sendRequest(zz);
        }
	}
	
	/// <summary>
	/// 邀请玩家进入自己的副本
	/// </summary>
	/// <param name='mPlayerID'>
	/// 邀请的玩家id
	/// </param>
	/// <param name='mRoomName'>
	/// 当前的房间名称
	/// </param>
	public void InviteGoPVE(string mPlayerID,string mRoomName,bool mIsInCity)
	{
		if (this.serverConnected )
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)ParameterType.UserID, mPlayerID);
			parameter.Add((short)ParameterType.RoomName, mRoomName);
			parameter.Add((short)ParameterType.IsGetIP, mIsInCity);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.InviteGoPVE, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.InviteGoPVE, parameter);
            ZealmConnector.sendRequest(zn);
        }
	}
	
	/// <summary>
	/// 玩家最大等级
	/// </summary>
	/// <returns>
	/// The player max level.
	/// </returns>
	public int GetPlayerMaxLevel()
	{
		//return (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerMaxLevel];
        return (int)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerMaxLevel];
	}
	
	 /// <summary>
    /// 绑定设备到通行证
    /// </summary>
    /// <param name="userID">通行证ID</param>
    /// <param name="userPwd">通行证密码</param>
    /// <param name="deviceID">设备ID</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void BindUserID(string userEmail, string userPwd, string deviceID, string dateBase, string table)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserEmail, userEmail);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, yuan.YuanMd5.yuanMd5.GetMd5(userPwd));
            // parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, appServer);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, deviceID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BindUserID, parameter, true);
            ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.BindUserID, parameter);
            ZealmConnector.sendRequest(zn);
        }
    }
	
	/// <summary>
	/// 绑定角色到设备
	/// </summary>
	/// <param name='userEmail'>
	/// User email.
	/// </param>
	/// <param name='deviceID'>
	/// Device I.
	/// </param>
	public void BindDevice(string userEmail,string deviceID)
	{
		if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserEmail, userEmail);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, deviceID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BindDevice, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.BindDevice,parameter);
			ZealmConnector.sendRequest(nd);
        }
	}

    /// <summary>
    /// 获取通行证角色
    /// </summary>
    /// <param name="userID">通行证ID</param>
    /// <param name="serverName">服务器名称</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void GetPlayers(string userID, string serverName, string dateBase, string table)
    {
//		Debug.Log ("------------------------GetPlayers");
        if (this.serverConnected)
        {
//			Debug.Log ("11111111111------------------------GetPlayers");
  //          Debug.Log(this.ServerAddress+"-------------======================ddddddddddddddddddddddddd");
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, userID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, serverName);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
            ////this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.GetPlayers, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.GetPlayers,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="playerID">角色ID</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void DelectPlayer(string playerID, string dateBase, string table, bool isFast)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.PlayerType, isFast);
            ////this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.DeletePlayer, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.DeletePlayer,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }

    /// <summary>
    /// 离线决斗挑战结果
    /// </summary>
    /// <param name="mPlayerID">对方ID</param>
    /// <param name="mIsWin">自己是否胜利</param>
    public void PVP1Fruit(string mPlayerID, bool mIsWin)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mPlayerID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.IsWin, mIsWin);

//            Debug.Log("wei-----------------------离线决斗挑战结果，mPlayerID:" + mPlayerID + ";mIsWin:" + mIsWin);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.PVP1Fruit, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	
	/// <summary>
	/// 获取游戏语言
	/// </summary>
	/// <returns>
	/// The lauguage.
	/// </returns>
	public string GetLauguage()
	{
        //if(YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo!=null)
        //{
        //    if(YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo.ContainsKey ((short)yuan.YuanPhoton.BenefitsType.GameLanguage))
        //    {
        //        return (string)YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameLanguage];
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        //else
        //{
        //    return "";
        //}

        if (YuanUnityPhoton.dicBenefitsInfo != null)
        {
            if (YuanUnityPhoton.dicBenefitsInfo.ContainsKey((short)yuan.YuanPhoton.BenefitsType.GameLanguage))
            {
                return (string)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameLanguage];
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
	}
	
	
	public int GetBloodTran()
	{
		//return (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.BloodTarn];
        return (int)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.BloodTarn];
	}
	
	/// <summary>
	///获取开关类字符串
	/// </summary>
	/// <returns>
	/// 类型
	/// </returns>
	/// <param name='mType'>
	/// M type.
	/// </param>
	public string GetServerSwitchString(yuan.YuanPhoton.BenefitsType mType)
	{
		//return (string)YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo[(short)mType];
        return (string)YuanUnityPhoton.dicBenefitsInfo[(short)mType];
	}
	
	/// <summary>
	/// 队伍队长消息
	/// </summary>
	/// <param name='mTeamInfo'>
	/// 队伍队长消息
	/// </param>
    public void TeamHeadIn(string mTeamInfo)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TeamInfo, mTeamInfo);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TeamHeadIn, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.TeamHeadIn, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    private Dictionary<string, YuanRank> dicYuanRank = new Dictionary<string, YuanRank>();

	public Dictionary<string, YuanRank> GetdicYuanRank()
	{
		return dicYuanRank;
	}
    /// <summary>
    /// 获取排行榜信息
    /// </summary>
    /// <param name="mRankingType"></param>
    /// <param name="mRankRowName"></param>
    /// <param name="mYuanRank"></param>
    public void GetRank(RankingType mRankingType,string mRankRowName,YuanRank mYuanRank)
    {
        if (this.serverConnected&&!mYuanRank.IsUpdate)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RankingType, (short)mRankingType);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RankRowName, mRankRowName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, mYuanRank.rankName);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetRank, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetRank, parameter);
            ZealmConnector.sendRequest(zz);
            mYuanRank.IsUpdate = true;
            dicYuanRank[mYuanRank.rankName] = mYuanRank;
        }
    }

	/// <summary>
	/// 获取排行榜信息by me
	/// </summary>
	/// <param name="mRankingType"></param>
	/// <param name="mRankRowName"></param>
	/// <param name="mYuanRank"></param>
	public void GetRankByMe(RankingType mRankingType,string mRankRowName,string mYuanRank)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.RankingType, (short)mRankingType);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.RankRowName, mRankRowName);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, mYuanRank);
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetRank, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetRank, parameter);
			ZealmConnector.sendRequest(zz);
		}
	}


   
    /// <summary>
    /// 获取某个排行榜信息
    /// </summary>
    /// <param name="mRankingType"></param>
    /// <param name="mRankRowName"></param>
    /// <param name="mYuanRank"></param>
    public void GetRankOne(RankingType mRankingType, string mRankRowName, YuanRank mYuanRank)
    {
        if (this.serverConnected && !mYuanRank.IsUpdate)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RankingType, mRankingType);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RankRowName, mRankRowName);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, mYuanRank.rankName);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetRankOne, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetRankOne, parameter);
            ZealmConnector.sendRequest(zz);
            mYuanRank.IsUpdate = true;
            dicYuanRank[mYuanRank.rankName] = mYuanRank;
        }
    }
	
	/// <summary>
	/// 客户端购买相应物品
	/// </summary>
	/// <param name='mItemID'>
	/// M item I.
	/// </param>
	/// <param name='mGold'>
	/// M gold.
	/// </param>
	/// <param name='mBlood'>
	/// M blood.
	/// </param>/
    public void ClientBuyItem(string mItemID,string mGold,string mBlood)
    {
        if (this.serverConnected )
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, mItemID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, mGold);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, mBlood);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ClientBuyItem, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ClientBuyItem, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	
		/// <summary>
	/// 客户端获取物品和金钱
	/// </summary>
	/// <param name='mItemID'>
	/// M item I.
	/// </param>
	/// <param name='mGold'>
	/// M gold.
	/// </param>
	/// <param name='mBlood'>
	/// M blood.
	/// </param>/
    public void ClientGetItemSome(string[] mItemIDs,string mGold,string mBlood)
    {
        if (this.serverConnected )
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, mItemIDs);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, mGold);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, mBlood);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ClientGetItemSome, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ClientGetItemSome, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	
	private static Dictionary<string,YuanBackInfo> dicYuanBack=new Dictionary<string, YuanBackInfo>();
	/// <summary>
	/// 客户端金钱申请
	/// </summary>
	/// <param name='mGold'>
	/// M gold.
	/// </param>
	/// <param name='mBlood'>
	/// M blood.
	/// </param>
	/// <param name='mBackInfo'>
	/// M back info.
	/// </param>
	public void ClientMoney(string mGold,string mBlood,YuanBackInfo mBackInfo)
    {
        if (this.serverConnected&&!dicYuanBack.ContainsKey (mBackInfo.Name)&&!mBackInfo.isUpate)
        {
			mBackInfo.isUpate=true;
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, mBackInfo.Name);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, mGold);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, mBlood);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ClientMoney, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ClientMoney, parameter);
            ZealmConnector.sendRequest(zz);
			dicYuanBack.Add (mBackInfo.Name,mBackInfo);
			
        }
    }

	/// <summary>
	/// 使用血瓶
	/// </summary>
	public void UseBottle()
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ChangeBottle, parameter);
			ZealmConnector.sendRequest(resp);		
		}			
	}
	
	/// <summary>
	/// 根据id获得表值
	/// </summary>
	/// <param name='tableID'>
	/// id
	/// </param>
	/// <param name='mTableType'>
	/// 表类型
	/// </param>
	/// <param name='table'>
	/// 表容器
	/// </param>
	public void GetTableForID(string tableID,TableType mTableType,YuanTable table)
    {
        
        if (this.serverConnected )
        {
			if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
			{
				table.IsUpdate = true;
				Dictionary<short, object> parameter = new Dictionary<short, object>();
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, tableID);
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableType, (short)mTableType);
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table.TableName);
	            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetTableForID, parameter, true);
 				ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetTableForID, parameter);
                ZealmConnector.sendRequest(zz);				
                dicTempTable.Add(table.TableName, table);
			}
 
        }
    }
	
	/// <summary>
	/// 根据ids获取表内某些字段数据
	/// </summary>
	/// <param name='tableIDs'>
	/// Table I ds.
	/// </param>
	/// <param name='tableRows'>
	/// Table rows.
	/// </param>
	/// <param name='mTableType'>
	/// M table type.
	/// </param>
	/// <param name='table'>
	/// Table.
	/// </param>
	public void GetTablesSomeForIDs(string[] tableIDs,string[] tableRows,TableType mTableType,YuanTable table)
    {
        
        if (this.serverConnected )
        {
			if (table!=null&&!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
			{
				table.IsUpdate = true;
				Dictionary<short, object> parameter = new Dictionary<short, object>();
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, tableIDs);
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableWhere, tableRows);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.TableType, (short)mTableType);
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table.TableName);
	            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetTablesSomeForIDs, parameter, true);
				ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetTablesSomeForIDs, parameter);
                ZealmConnector.sendRequest(zz);				
                dicTempTable.Add(table.TableName, table);
			}
 
        }
    }

	/// <summary>
	/// 根据Name获取表内某些字段数据
	/// </summary>
	/// <param name='tableIDs'>
	/// Table I ds.
	/// </param>
	/// <param name='tableRows'>
	/// Table rows.
	/// </param>
	/// <param name='mTableType'>
	/// M table type.
	/// </param>
	/// <param name='table'>
	/// Table.
	/// </param>
	public void GetTablesSomeForNames(string[] tableNames,string[] tableRows,TableType mTableType,YuanTable table)
	{
		
		if (this.serverConnected )
		{
			if (table!=null&&!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
			{
				table.IsUpdate = true;
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.TableKey, tableNames);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.TableWhere, tableRows);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.TableType, (short)mTableType);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table.TableName);
				////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetTablesSomeForIDs, parameter, true);
				ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetTablesSomeForNames, parameter);
				ZealmConnector.sendRequest(zz);				
				dicTempTable.Add(table.TableName, table);
			}
			
		}
	}
	
	
	/// <summary>
	/// 根据名称获得玩家数据
	/// </summary>
	/// <param name='mPlayerName'>
	/// M player name.
	/// </param>
	/// <param name='table'>
	/// Table.
	/// </param>
	public void GetPlayerForName(string mPlayerName,YuanTable table)
    {
        
        if (this.serverConnected )
        {
			if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
			{
				table.IsUpdate = true;
				Dictionary<short, object> parameter = new Dictionary<short, object>();
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNickName, mPlayerName);
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table.TableName);
	            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetPlayerForName, parameter, true);
                ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetPlayerForName, parameter);
                ZealmConnector.sendRequest(zz);				
                dicTempTable.Add(table.TableName, table);
			}
 
        }
    }
	
	
	/// <summary>
	/// 获取随机玩家数据
	/// </summary>
	/// <param name='mPlayerNum'>
	///获取玩家数量
	/// </param>
	/// <param name='playerLevelRang'>
	/// 玩家等级范围
	/// </param>
	/// <param name='table'>
	/// Table.
	/// </param>
	public void GetRandomPlayer(int mPlayerNum,int playerLevelRang,YuanTable table)
    {
        
        if (this.serverConnected )
        {
			if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
			{
				table.IsUpdate = true;
				Dictionary<short, object> parameter = new Dictionary<short, object>();
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.NumRandom, mPlayerNum);
	            parameter.Add((short)yuan.YuanPhoton.ParameterType.PlayerLevel, playerLevelRang);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table.TableName);
	            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetRandomPlayer, parameter, true);
                ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetRandomPlayer, parameter);
                ZealmConnector.sendRequest(zz);				
                dicTempTable.Add(table.TableName, table);
			}
 
        }
    }
	
	/// <summary>
	/// 获取排行榜前几名详细信息
	/// </summary>
	/// <param name='mRankingType'>
	/// M ranking type.
	/// </param>
	/// <param name='mRankRowName'>
	/// M rank row name.
	/// </param>
	/// <param name='mYuanRank'>
	/// M yuan rank.
	/// </param>
    public void GetRankTopYT(RankingType mRankingType,YuanTable mYt)
    {
	//	
		if (this.serverConnected&&!mYt.IsUpdate)
        {
			mYt.IsUpdate = true;
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RankingType, mRankingType.ToString());
//			Debug.Log("mRankingType================================:"+mRankingType);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, mYt.TableName);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetRankTopYT, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetRankTopYT, parameter);
            ZealmConnector.sendRequest(zz);            
            dicTempTable.Add(mYt.TableName, mYt);
        }
    }
	
	/// <summary>
	/// Gets my mail.
	/// </summary>
	/// <param name='mYt'>
	/// M yt.
	/// </param>
	public void GetMyMail(YuanTable mYt)
    {
        if (this.serverConnected&&!mYt.IsUpdate)
        {
			mYt.IsUpdate = true;
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, mYt.TableName);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetMyMail, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetMyMail, parameter);
            ZealmConnector.sendRequest(zz);            
            dicTempTable.Add(mYt.TableName, mYt);
        }
    }

    /// <summary>
    /// 购买精铁粉末
    /// </summary>
    public void BuyIronMaterial()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            //parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, oldID);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.buyIron, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 购买精金结晶
    /// </summary>
    public void BuyGoldMaterial()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            //parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, oldID);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.buyGold, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	
	/// <summary>
	/// 装备强化
	/// </summary>
	/// <param name='oldID'>
	/// 要强化的装备号
	/// </param>
	/// <param name='index'>
	/// 装备所在包裹编号
	/// </param>
	/// <param name='useBlood'>
	/// 是否使用血石
	/// </param>
	/// <param name='needGold'>
	/// 需要的金币
	/// </param>
	/// <param name='needBlood'>
	/// 需要的血石
	/// </param>
    /// <param name='isCostMatiral'>
    /// 是否消耗材料
    /// </param>
	public void EquepmentBuild(string oldID,int index,int useBlood,int needGold,int needBlood, bool isCostMatiral)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, oldID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.PageNum, index);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Safety, useBlood);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, needGold);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, needBlood);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.isPaymentPickup, isCostMatiral);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.EquepmentBuild, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.EquepmentBuild, parameter);
            ZealmConnector.sendRequest(zz);		}
	}
	
	/// <summary>
	/// 装备打孔
	/// </summary>
	/// <param name='oldID'>
	/// 装备号
	/// </param>
	/// <param name='index'>
	/// 装备所在包裹编号
	/// </param>
	/// <param name='needGold'>
	/// 需要的金币
	/// </param>
	/// <param name='needBlood'>
	/// 需要的血石
	/// </param>
	public void EquepmentHole(string oldID,int index,int num1)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, oldID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.PageNum, index);
			parameter.Add((short)1, num1);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.EquepmentHole, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.EquepmentHole, parameter);
            ZealmConnector.sendRequest(zz);		}
	}	
	
	/// <summary>
	/// 装备镶嵌宝石
	/// </summary>
	/// <param name='invID'>
	/// 装备id
	/// </param>
	/// <param name='id'>
	/// 装备包裹编号
	/// </param>
	/// <param name='oneHoleID'>
	/// 装备的空位id
	/// </param>
	/// <param name='holeItemID'>
	/// 宝石id
	/// </param>
	/// <param name='needGold'>
	/// 需要金币
	/// </param>
	/// <param name='needBlood'>
	/// 需要血石
	/// </param>
	public void EquepmentMosaic(string invID,int id,int oneHoleID,string holeItemID,int needGold,int needBlood)
	{
		if(this.serverConnected)
		{
			
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, invID);
            parameter.Add((short)yuan.YuanPhoton.ParameterType.PageNum, id);
			 parameter.Add((short)yuan.YuanPhoton.ParameterType.HoleID, oneHoleID);
			 parameter.Add((short)yuan.YuanPhoton.ParameterType.GemID, holeItemID);
			
			
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, needGold);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, needBlood);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.EquepmentMosaic, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.EquepmentMosaic, parameter);
            ZealmConnector.sendRequest(zz);		}
	}	
	
	/// <summary>
	/// 装备打造
	/// </summary>
	/// <param name='invID'>
	/// 配方ID
	/// </param>
	/// <param name='needGold'>
	/// Need gold.
	/// </param>
	/// <param name='needBlood'>
	/// Need blood.
	/// </param>
	public void EquepmentProduce(string itemID,int needGold,int needBlood)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, itemID);
			
			
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, needGold);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, needBlood);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.EquepmentProduce, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.EquepmentProduce, parameter);
            ZealmConnector.sendRequest(zz);		}
	}	
	/// <summary>
	/// 点击购买商品的时候，需要上传当前渠道标识和购买商品的id;string qudao
	/// </summary>
	/// <param name="qudao">Qudao.</param>
	/// <param name="id">Identifier.</param>
	public void GetBuyLanWei(string userName, string qudao)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();

            parameter.Add((short)yuan.YuanPhoton.PayLanWeiParams.userName, userName);
			parameter.Add((short)yuan.YuanPhoton.PayLanWeiParams.Qudao, qudao);
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.payLanwei, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.payLanwei, parameter);
            ZealmConnector.sendRequest(zz);
		}
	}

	/// <summary>
	/// 请求返回商品的价格
	/// </summary>
	/// <param name="qudao">Qudao.</param>
	/// <param name="id">Identifier.</param>
	public void GetBuyInfor(string qudao)
	{
		
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.PayinfoParams.Qudao, qudao);
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PayGameRole, null, true);
			ZMNetDataLikePhoton resps = new ZMNetDataLikePhoton((short)OpCode.PayGameRole, parameter);
			ZealmConnector.sendRequest(resps);
		}
	}

    /// <summary>
    /// IOS内购后请求服务器验证
    /// </summary>
    /// <param name="qudao">Qudao.</param>
    /// <param name="id">Identifier.</param>
    public void VerifyToServer(string verifyInfo)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)PayLanWeiParams.userID, verifyInfo);// 区服,登录名（login_name)，订单（ios返回的加密数据），产品id

            ZMNetDataLikePhoton resps = new ZMNetDataLikePhoton((short)OpCode.IphonePay, parameter);
            ZealmConnector.sendRequest(resps);
        }
    }
	
	/// <summary>
	/// 训练
	/// </summary>
	/// <param name='itemID'>
	/// 训练等级
	/// </param>
	/// <param name='needGold'>
	/// Need gold.
	/// </param>
	/// <param name='needBlood'>
	/// Need blood.
	/// </param>
	public void Training(int itemID,int needGold,int needBlood)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, itemID);
			
			
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, needGold);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, needBlood);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Training, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.Training, parameter);
            ZealmConnector.sendRequest(zz);		}
	}		
	
	/// <summary>
	/// 保存训练
	/// </summary>
	/// <param name='itemID'>
	/// 1为保存，0为放弃
	/// </param>
	public void TrainingSave(int itemID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, itemID);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.TrainingSave, parameter);
            ZealmConnector.sendRequest(zz);			
			
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TrainingSave, parameter, true);
		}
	}		
	
	/// <summary>
	/// 获取商店列表
	/// </summary>
	/// <param name='storeType'>
	/// 商店类型
	/// </param>
	/// <param name='id'>
	/// 地图等级
	/// </param>
	public void GetStoreList(StoreType storeType,int mapLevel)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.StoreType, (short)storeType);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.RoomName, mapLevel);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.GetStoreList, parameter);
			ZealmConnector.sendRequest(resp);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetStoreList, parameter, true);
		}
	}		
	
	
	/// <summary>
	/// 购买商店物品
	/// </summary>
	/// <param name='storeType'>
	/// 商店类型
	/// </param>
	/// <param name='id'>
	/// 道具所在数组索引
	/// </param>
	/// <param name='needGold'>
	/// Need gold.
	/// </param>
	/// <param name='needBlood'>
	/// Need blood.
	/// </param>
	public void BuyStoreClient(StoreType storeType,int id,int needGold,int needBlood,int mapLevel,string storeID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)Zealm.ParameterType.StoreType, (short)storeType);
            parameter.Add((short)Zealm.ParameterType.ItemID, id);
            parameter.Add((short)Zealm.ParameterType.RoomName, mapLevel);

            parameter.Add((short)Zealm.ParameterType.Gold, needGold);
            parameter.Add((short)Zealm.ParameterType.BloodStone, needBlood);
            parameter.Add((short)Zealm.ParameterType.StoreID, storeID);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BuyStoreClient, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BuyStoreClient, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}	
	
	/// <summary>
	/// 购买随机道具
	/// </summary>
	/// <param name='id'>
	/// 道具所在数组索引
	/// </param>
	/// <param name='mapLevel'>
	/// 地图等级
	/// </param>
	/// <param name='needGold'>
	/// Need gold.
	/// </param>
	/// <param name='needBlood'>
	/// Need blood.
	/// </param>
	public void GetRandomItem(int id,int mapLevel,int needGold,int needBlood)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, id);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.RoomName, mapLevel);
			
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Gold, needGold);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BloodStone, needBlood);			
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetRandomItem, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetRandomItem, parameter);
            ZealmConnector.sendRequest(zz);		}
	}		
	
	/// <summary>
	/// 玩家所在地图
	/// </summary>
	/// <param name='myMap'>
	/// My map.
	/// </param>
	public void PlayerInMap(string myMap)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.RoomName, myMap);		
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PlayerInMap, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.PlayerInMap, parameter);
            ZealmConnector.sendRequest(zz);		}
	}	
	
	/// <summary>
	/// 增加经验值
	/// </summary>
	/// <param name='mExp'>
	/// 经验值
	/// </param>
	/// <param name='mDoubleCard'>
	/// 双倍卡
	/// </param>
	public void AddExperience(int[] mExp,int mDoubleCard)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.ItemID, mExp);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.Value, mDoubleCard);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.AddExperience, parameter, true);
            ZMNetDataLikePhoton nd = new ZMNetDataLikePhoton((short)OpCode.AddExperience, parameter);
            ZealmConnector.sendRequest(nd);		}
	}	
	

	
	/// <summary>
	/// 挂机加经验
	/// </summary>
	public void HangUpAddExp()
	{
		if(this.serverConnected)
		{
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.HangUpAddExp, null, true);
            ZMNetDataLikePhoton nd = new ZMNetDataLikePhoton((short)OpCode.HangUpAddExp,null);
            ZealmConnector.sendRequest(nd);		}
	}
	public void PayTestPC(string playerid,string money,string xueshi){
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			string payinfo=playerid+","+money+","+xueshi;
			parameter[(short)PayLanWeiParams.userID]=payinfo;
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.PayTest, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

	
	/// <summary>
	/// 副本翻牌
	/// </summary>
	/// <param name='timesDead'>
	/// 死亡次数
	/// </param>
	/// <param name='timesXuePing'>
	/// 使用血瓶次数
	/// </param>
	/// <param name='mapLevel'>
	/// 副本等级
	/// </param>
	/// <param name='fenTime'>
	/// 花费时间
	/// </param>
	/// <param name='useXXStr'>
	/// 主角完成的所有副本评星字段
	/// </param>
	/// <param name='psLength'>
	/// 当前副本人数
	/// </param>
	/// <param name='nowMapLevel'>
	/// 副本难度
	/// </param>
	/// <param name='ranItem'>
	/// 固定掉落
	/// </param>
	/// <param name='needGold'>
	/// 拾取的金币
	/// </param>
	/// <param name='needBlood'>
	/// 拾取的血石
	/// </param>
	public void DoneCard(int timesDead,int timesXuePing,int mapLevel,int fenTime,string useXXStr,int psLength,int nowMapLevel,string ranItem,int needGold,int needBlood,float enemyClear,string mapID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();

			//parameter.Add(ParameterType.NumEnd, timesDead);
			parameter[(short)Zealm.ParameterType.NumEnd]=timesDead;
		//	parameter.Add(ParameterType.PageNum, timesXuePing);
			parameter[(short)Zealm.ParameterType.PageNum]=timesXuePing;
           // parameter.Add(ParameterType.RoomName, mapLevel);
			parameter[(short)Zealm.ParameterType.RoomName]=mapLevel;
			//parameter.Add(ParameterType.Time, fenTime);
			parameter[(short)Zealm.ParameterType.Time]=fenTime;
		//	parameter.Add(ParameterType.MyRank, useXXStr);
			parameter[(short)Zealm.ParameterType.MyRank]=useXXStr;
			//parameter.Add(ParameterType.UserNumber, psLength);
			parameter[(short)Zealm.ParameterType.UserNumber]=psLength;
			//parameter.Add(ParameterType.RankRowName, nowMapLevel);
			parameter[(short)Zealm.ParameterType.RankRowName]=nowMapLevel;
			//parameter.Add(ParameterType.ItemID, ranItem);
			parameter[(short)Zealm.ParameterType.ItemID]=ranItem;
			//parameter.Add(ParameterType.IsReOnline, enemyClear);
			parameter[(short)Zealm.ParameterType.IsReOnline]=enemyClear;
			//parameter.Add(ParameterType.PicID, mapID);
			parameter[(short)Zealm.ParameterType.PicID]=mapID;
			//parameter.Add(ParameterType.Gold, needGold);
			parameter[(short)Zealm.ParameterType.Gold]=needGold;
		//	parameter.Add(ParameterType.BloodStone, needBlood);	
			parameter[(short)Zealm.ParameterType.BloodStone]=needBlood;
			parameter[(short)Zealm.ParameterType.ShouYi]=1;
          //  //this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.DoneCard, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.DoneCard, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}	
	
	/// <summary>
	/// PVP翻牌
	/// </summary>
	/// <param name='mapLevel'>
	/// Map level.
	/// </param>
	/// <param name='nowMapLevel'>
	/// Now map level.
	/// </param>
	/// <param name='ranItem'>
	/// Ran item.
    /// PVPType  0为影魔 1为单人 2为双人 3为雪地 4为O诺诚
	/// </param>
	public void DoneCardPVP(int SuccessORfailure,short PVPType)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)100, SuccessORfailure);//1是挑战成功，2是挑战失败
			parameter.Add((short)102,PVPType);
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.DoneCardPVP, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.DoneCardPVP, parameter);
            ZealmConnector.sendRequest(zz);			
		}
	}

    /// <summary>
    /// PVP失败
    /// PVPType  0为影魔 1为单人 2为双人 3为雪地 4为O诺诚
    /// </summary>
    public void PVPisFall(short PVPType)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)yuan.YuanPhoton.ParameterType.MoneyType, PVPType);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.PVPisFall, parameter);
            ZealmConnector.sendRequest(zz);
        }
    }
	/// <summary>
	/// 副本抽取卡片
	/// </summary>
	public void OpenCard()
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();			
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.OpenCard, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.OpenCard, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}	
	
		/// <summary>
	/// 同步客户端参数
	/// </summary>
	public void GetClientPrams()
	{
		if(this.serverConnected)
		{
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetClientParms, null, true);
            ZMNetDataLikePhoton nd = new ZMNetDataLikePhoton((short)OpCode.GetClientParms, null);
            ZealmConnector.sendRequest(nd);		}
	}
	
	/// <summary>
	/// 是否保存记录了
	/// </summary>
	public void IsSaveData()
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
		
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.IsSaveDate, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.IsSaveDate, parameter);
            ZealmConnector.sendRequest(zz);				}
	}	
	
	/// <summary>
	/// 请求获取活动信息
	/// </summary>
	public void GetActivityInfo(yuan.YuanPhoton.ActivityType mActivityType)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.ParameterType.ActivityType]=mActivityType;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ActivityGetInfo, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ActivityGetInfo, parameter);
            ZealmConnector.sendRequest(zz);			}
	}	
	
	/// <summary>
	/// 获取活动奖励
	/// </summary>
	/// <param name='mActivityType'>
	/// 活动类型
	/// </param>
	public void GetActivityReward(yuan.YuanPhoton.ActivityType mActivityType)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.ParameterType.ActivityType]=mActivityType;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ActivityGetReward, parameter, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.ActivityGetReward, parameter);
            ZealmConnector.sendRequest(zz);			}
	}
    public void FirstRecharge()
    {
        if (this.serverConnected)
        {
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ActivityFirstCharge, null);
            ZealmConnector.sendRequest(resp);
        }
    }
	
		/// <summary>
	/// 使用物品
	/// </summary>
	/// <param name='mActivityType'>
	/// 物品ID
	/// </param>
	public void UseItem(string itemID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.ParameterType.ItemID]=itemID;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.UseItem, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.UseItem, parameter);
			ZealmConnector.sendRequest(resp);		
        }
	}
    #region 进入游戏礼包相关


    public void StartLogonTime()
    {
        if (this.serverConnected)
        {
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetFirstPacks, null, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.StartLogonTime, null);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 获取首次奖励礼包
    /// </summary>
    public void GetFirstPacks()
    {
        if (this.serverConnected)
        {
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.GetFirstPacks, null, true);
            ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GetFirstPacks, null);
            ZealmConnector.sendRequest(zz);
        }
    }

    /// <summary>
    /// 首充奖励领取奖励
    /// </summary>
    public void PlayerFirstRecharge()
    {
        if (this.serverConnected)
        {
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.PlayerFirstCharge, null);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 获取在线礼包
    /// </summary>
    public void OnlineChests()
    {
        if (this.serverConnected)
        {
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.OnlineChests, null);
            ZealmConnector.sendRequest(resp);
        }
    }

    public void OnlineActivityInfo()
    {
        if (this.serverConnected)
        {
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.TriggerActivity, null);
            ZealmConnector.sendRequest(resp);
        }
    }
    public void GetServerTime()
    {
        if (this.serverConnected)
        {
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.GetServerTime, null);
            ZealmConnector.sendRequest(resp);
        }
    }

    #endregion 进入游戏礼包相关
    #region 任务相关

    /// <summary>
	/// Tasks the complete.
	/// </summary>
	/// <param name='mTaskID'>
	/// M task I.
	/// </param>
	public void TaskComplete(string mTaskID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.ParameterType.ItemID]=mTaskID;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.TaskCompleted, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.TaskCompleted, parameter);
            ZealmConnector.sendRequest(resp);		}
	}
	
	/// <summary>
	/// 接受任务
	/// </summary>
	/// <param name='mTaskID'>
	/// 任务id
	/// </param>
	public void TaskAcceptedAsID(string mTaskID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)TaskParams.TaskID]=mTaskID;
			parameter[(short)TaskParams.TaskType]=(short)TaskType.TaskAcceptedAsID;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Task, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Task, parameter);
            ZealmConnector.sendRequest(resp);
		}
	}
	
	/// <summary>
	/// 达成任务所需条目
	/// </summary>
	/// <param name='mTaskID'>
	/// 任务id
	/// </param>
	public void TaskAddNumsAsID(string mTaskID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.TaskParams.TaskID]=mTaskID;
			parameter[(short)yuan.YuanPhoton.TaskParams.TaskType]=(short)yuan.YuanPhoton.TaskType.TaskAddNumsAsID;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Task, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Task, parameter);
            ZealmConnector.sendRequest(resp);
		}
	}
	
	/// <summary>
	/// 放弃任务
	/// </summary>
	/// <param name='mTaskID'>
	/// 任务id
	/// </param>
	public void TaskGiveUpAsID(string mTaskID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.TaskParams.TaskID]=mTaskID;
			parameter[(short)yuan.YuanPhoton.TaskParams.TaskType]=(short)yuan.YuanPhoton.TaskType.TaskGiveUpAsID;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Task, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Task, parameter);
            ZealmConnector.sendRequest(resp);
		}
	}	
	
	#endregion 任务相关
	
	#region 活动相关
	
	/// <summary>
	/// 奖池抽奖
	/// </summary>
	/// <param name='mTimes'>
	/// 连抽次数
	/// </param>
	/// <param name='poolType'>
	/// 奖池类型（0为金币，1为血石）
	/// </param>
	public void JockPotLottery(int mTimes,int poolType)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();		
			parameter[(short)yuan.YuanPhoton.ActivityParams.LotteryTimes]=mTimes;
			parameter[(short)yuan.YuanPhoton.ActivityParams.PoolType]=poolType;
			parameter[(short)yuan.YuanPhoton.ActivityParams.ActivityType]=(short)yuan.YuanPhoton.ActivityType.JockPotLottery;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Activity, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Activity, parameter);
			ZealmConnector.sendRequest(resp);		}		
	}
	
	/// <summary>
	/// 获取奖池信息
	/// </summary>
	public void JockPotShowInfo()
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.ActivityParams.ActivityType]=(short)yuan.YuanPhoton.ActivityType.JockPotShowInfo;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Activity, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Activity, parameter);
            ZealmConnector.sendRequest(resp);		}	
	}
	
	#endregion 活动相关
	
	#region 好友相关
	
	/// <summary>
	/// 请求加好友
	/// </summary>
	/// <param name='mFirendID'>
	/// 玩家ID
	/// </param>
	public void FirendsAddInvit(string mFirendID)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendsType]=(short)yuan.YuanPhoton.FirendsType.FirendsAddInvit;
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendID]=mFirendID;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Firends, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Firends, parameter);
            ZealmConnector.sendRequest(resp);		}				
	}
	
	/// <summary>
	/// 请求加好友
	/// </summary>
	/// <param name='mFirendName'>
	/// 玩家名称
	/// </param>
	public void FirendsAddInvitForName(string mFirendName)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendsType]=(short)yuan.YuanPhoton.FirendsType.FirendsAddInvitForName;
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendName]=mFirendName;
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Firends, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Firends, parameter);
            ZealmConnector.sendRequest(resp);		}			
	}
	
	/// <summary>
	/// 返回加好友请求
	/// </summary>
	/// <param name='mFirendID'>
	/// 对方ID
	/// </param>
	/// <param name='mFirendName'>
	/// 对方名称
	/// </param>
	/// <param name='returnType'>
	/// 响应结果
	/// </param>
	public void RetrunFirendsAddInvit(string mFirendID,string mFirendName,ReturnCode returnType)
	{
		if(this.serverConnected)
		{
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendsType]=(short)yuan.YuanPhoton.FirendsType.RetrunFirendsAddInvit;
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendID]=mFirendID;
			parameter[(short)yuan.YuanPhoton.FirendsParams.FirendName]=mFirendName;
			parameter[(short)yuan.YuanPhoton.FirendsParams.RetrunType]=(short)returnType;
//            //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.Firends, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.Firends, parameter);
            ZealmConnector.sendRequest(resp);		}			
	}
	


	
	#endregion 好友相关

	/// <summary>
	/// 客户端使用金钱方法
	/// </summary>
	/// <param name="useMoneyType">使用类型.</param>
	/// <param name="num1">Num1.</param>
	/// <param name="num2">Num2.</param>
	/// <param name="itemID">Item I.</param>
	public void UseMoney(yuan.YuanPhoton.UseMoneyType useMoneyType,int num1,int num2,string itemID)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.UseMoneyType]=(short)useMoneyType;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.Num1]=num1;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.Num2]=num2;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.ItemID]=itemID;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.UseMoney, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.UseMoney, parameter);
            ZealmConnector.sendRequest(resp);		}			
	}
	/// <summary>
	/// 做显示花费金钱和血石的预处理
	/// </summary>
	/// <param name="activityID">Activity I.</param>
	/// 
	public void ShowALLMoney(yuan.YuanPhoton.UseMoneyType useMoneyType,int num1,int num2,string itemID)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.UseMoneyType]=(short)useMoneyType;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.Num1]=num1;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.Num2]=num2;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.ItemID]=itemID;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.UseMoney, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ShowAllMONEY, parameter);
			ZealmConnector.sendRequest(resp);	
		}			
	}

    /// <summary>
    /// 获取重置职业的血石消耗
    /// </summary>
    //public void GetResetProfessionCost()
    //{
    //    if (this.serverConnected)
    //    {  
    //        ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ZhuanZhi, null);
    //        ZealmConnector.sendRequest(resp);
    //    }
    //}

	/// <summary>
	/// 显示体力
	/// </summary>
	/// <param name="useMoneyType">Use money type.</param>
	/// <param name="num1">Num1.</param>
	/// <param name="num2">Num2.</param>
	/// <param name="itemID">Item I.</param>
	public void Showstrength(yuan.YuanPhoton.CostPowerType CostPowerType,int num1,int num2,string itemID)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)0]=(short)CostPowerType;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.Num1]=num1;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.UseMoney, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ShowStrength, parameter);
			ZealmConnector.sendRequest(resp);	
		}			
	}
	// 训练界面请求服务器获得所求的值
    public void ShowTrainingInfo()
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
		
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.UseMoney, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ShowTraining, parameter);
			ZealmConnector.sendRequest(resp);	
		}			
	}

	// 实际扣除体力的方法
	public void Coststrength(yuan.YuanPhoton.CostPowerType CostPowerType,int num1,int num2,string itemID)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)0]=(short)CostPowerType;
			parameter[(short)yuan.YuanPhoton.UseMoneyParams.Num1]=num1;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.UseMoney, parameter, true);
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.DeductStrength, parameter);
			ZealmConnector.sendRequest(resp);	
		}			
	}
	public void JoinActivity(string activityID)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.ParameterType.ActivityID] = activityID;

			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.JoinActivity, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.JoinActivity, parameter);
            ZealmConnector.sendRequest(resp);		}			
	}

	public void FinishActivity(string activityID)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.ParameterType.ActivityID] = activityID;
			
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.FinishActivity, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.FinishActivity, parameter);
            ZealmConnector.sendRequest(resp);		}			
	}

	public void BattlefieldInfo()
	{
		if (this.serverConnected)
		{
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BattlefieldInfo,null, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BattlefieldInfo, null);
            ZealmConnector.sendRequest(resp);		}	
	}

	public void BattlefieldExit()
	{
		if (this.serverConnected)
		{
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BattlefieldExit,null, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BattlefieldExit, null);
            ZealmConnector.sendRequest(resp);		}	
	}

	public void BattlefieldKill()
	{
		if (this.serverConnected)
		{
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BattlefieldKill,null, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BattlefieldKill, null);
            ZealmConnector.sendRequest(resp);		}	
	}


    public void BattlefieldDie()
    {
        if (this.serverConnected)
        {
            ////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BattlefieldDie, null, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BattlefieldDie, null);
            ZealmConnector.sendRequest(resp);        }	
    }

    public void BattlefieldGetFlag(string flagID)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter[(short)yuan.YuanPhoton.ParameterType.FlagID] = flagID;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BattlefieldGetFlag, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BattlefieldGetFlag, parameter);
            ZealmConnector.sendRequest(resp);        }	
    }

	/// <summary>
	/// 欧诺城世界boss血同步 -
	/// </summary>
	/// <param name="bossLabel">Boss label.</param>
	/// <param name="hitValue">Hit value.</param>
    public void BattlefieldHitBoss(string bossLabel,int hitValue)
    {
        if (this.serverConnected)
		{	
//			Debug.Log("@$#@$#@%$#@%##%#%@$%@$%@$%@$%@$%#@%#@%$#@$%#@$%@%@$@%@$@$@$%@$@$#@$%@$@$%@$@$%@$#@$#@$#@%$@$%#@$%#@$%#@%$#@%");
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter[(short)0] = bossLabel;
            parameter[(short)1] = hitValue;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BattlefieldHitBoss, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BattlefieldHitBoss, parameter);
            ZealmConnector.sendRequest(resp);        }	
    }
	/// <summary>
	/// 世界boss掉血同步 -
	/// </summary>
	/// <param name="hitValue">Hit value.</param>
	public void ActivityHitBoss(int hitValue)
	{
		if (this.serverConnected)
		{
//			Debug.Log("@$#@$#@%$#@%##%#%@$%@$%@$%@$%@$%#@%#@%$#@$%#@$%@%@$@%@$@$@$%@$@$#@$%@$@$%@$@$%@$#@$#@$#@%$@$%#@$%#@$%#@%$#@%");
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.ParameterType.hitValue] = hitValue;
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.ActivityBossDamage, parameter, true);
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ActivityBossDamage, parameter);
            ZealmConnector.sendRequest(resp);		}	
	}

    public void DefenceBattleStart(int lightBallHp)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter[(short)0] = lightBallHp;
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.DefenceBattleStart, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    public void BallApplyDamage(int damage)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter[(short)yuan.YuanPhoton.ParameterType.hitValue] = damage;
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.BallApplyDamage, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }
    /// <summary>
    /// 清除怪物仇恨
    /// </summary>
    /// <param name="MosterID">怪物实例ID</param>
    public void ClearMonsterHate(int MosterId)
    {
        if (this.serverConnected)
        {
            ZMNetData resp = new ZMNetData((short)OpCode.MonsterClearHate);
            resp.writeInt(MosterId);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 怪物仇恨列表
    /// </summary>
    /// <param name="MosterID">怪物实例ID</param>
    /// <param name="instensid">上传的玩家或者骷髅的实例id</param>
    public void MonsterHateList(int MosterID, int TargtID, int HateValue)
    {
        if (this.serverConnected)
        {
            ZMNetData resp = new ZMNetData((short)OpCode.MonsterHateList);
            resp.writeInt(MosterID);
            resp.writeInt(TargtID);
            resp.writeInt(HateValue);
            ZealmConnector.sendRequest(resp);
        }
    }


    /// <summary>
	/// 清除怪物仇恨
	/// </summary>
    /// <param name="MosterID">怪物实例ID</param>
    /// <param name="instensid">上传的玩家或者骷髅的实例id</param>
    public void RemoveMosterHate(int MosterID,int instensid)
    {
        if (this.serverConnected)
        {
            ZMNetData resp = new ZMNetData((short)OpCode.RemoveMosterHate);
            resp.writeInt(MosterID);
            resp.writeInt(instensid);
            ZealmConnector.sendRequest(resp);
        }
    }

	/// <summary>
	/// 获取成长福利信息
	/// </summary>
	public void GetGrowthWelfareInfo()
	{
		if (this.serverConnected)
		{
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.GrowthWelfareInfo, null);
			ZealmConnector.sendRequest(resp);
		}
	}

	/// <summary>
	/// 获取成长福利.
	/// </summary>
	/// <param name="getNum">要获取的福利等级</param>
	public void GetGrowthWelfare(int getNum)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.ParameterType.ItemID] = getNum;
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.GetGrowthWelfare, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

    /// <summary>
    /// 连续充值奖励
    /// 3为3日，5为5日，7为7日
    /// </summary>
    public void NumberRechargeDay(int Day)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter[(short)yuan.YuanPhoton.ParameterType.ItemID] = Day;
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.NumberRechargeDay, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

	/// <summary>
	/// 一键分解
	/// </summary>
	/// <param name="itemLevel">品质</param>
	/// <param name="useBlood">是否双倍</param>
	public void EquipmentResolveAll(int[] itemLevel,bool useBlood)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)Zealm.ParameterType.ItemID] = itemLevel;
			parameter[(short)Zealm.ParameterType.UseBlood] = useBlood;
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.EquipmentResolveAll, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

	/// <summary>
	/// 一键售卖
	/// </summary>
	/// <param name="itemLevel">品质</param>
	public void EquipmentSellAll(int[] itemLevel)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)Zealm.ParameterType.ItemID] = itemLevel;
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.EquipmentSellAll, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

	/// <summary>
	/// 临时队伍玩家退队
	/// </summary>
	public void TempTeamPlayerRemove()
	{
		if (this.serverConnected)
		{
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.TempTeamPlayerRemove, null);
			ZealmConnector.sendRequest(resp);
		}
	}

	/// <summary>
	/// 临时队伍队长进入地图 
	/// </summary>
	/// <param name="mapInstensID">地图实例id</param>
	public void TempTeamHeadGoMap(int mapInstensID)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)Zealm.ParameterType.ItemID] = mapInstensID;
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.TempTeamHeadGoMap, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

    /// <summary>
    /// 一键训练
    /// </summary>
	public void QuickTraining()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.QuickTraining, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }
    /// <summary>
    /// 请求动态配置活动信息
    /// </summary>
    public void AskDynamicActivity()
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.DynamicActivity, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

    /// <summary>
    /// 视角选择，用于统计新增玩家视角选择占比
    /// </summary>
    public void SetViewSelection(int viewType)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter[(short)1] = viewType;
            ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.ViewSelection, parameter);
            ZealmConnector.sendRequest(resp);
        }
    }

	/// <summary>
	/// 获取等级礼包信息
	/// </summary>
	public void GetLevelPackInfo()
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.GetLevelPackInfo, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

	/// <summary>
	/// 获取等级礼包
	/// </summary>
	/// <param name="mLevel">M level.</param>
	public void GetLevelPack(int mLevel)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)Zealm.ParameterType.ItemID] = mLevel;
			ZMNetDataLikePhoton resp = new ZMNetDataLikePhoton((short)OpCode.GetLevelPack, parameter);
			ZealmConnector.sendRequest(resp);
		}
	}

	public void CZPGetOutOf()
	{
		btnGameManagerBack.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info1014"));
    }

	///// ÉèÖÃºÃ
	
	
	///// <summary>
    ///// </summary>
    ///// <param name="playerID">ºÃÓÑID</param>
    ///// <param name="setFirendType">ÉèÖÃºÃÓÑÀàÐÍ</param>
    ///// <param name="dateBas">ÊýŸÝ¿â</param>
    ///// <param name="table">±í</param>
    //public void Setadd(string playerID,SetFirendType setFirendType,string dateBas, string table)
    //{
    //    if (this.serverConnected)
    //    {
    //        Dictionary<short, object> parameter = new Dictionary<short, object>();
    //        parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerID);
    //        parameter.Add((short)yuan.YuanPhoton.ParameterType.SetFriendType, setFirendType);
    //        parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBas);
    //        parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
    //        //this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SetFirend, parameter, true);
   
    //    }
    //}

    #region IPhotonPeerListener ³ÉÔ±
    public void DebugReturn(DebugLevel level, string message)
    {
      
    }

   public void OnEvent(EventData eventData)
   {
	//	try
	//	{
	      //  switch (eventData.Code)
	     //   {
	      //  //    case (byte)yuan.YuanPhoton.EventCode.SendMessage:
	             //   {
	                    //string messageText = (string)eventData.Parameters[(short)yuan.YuanPhoton.ParameterType.MessageText];
	                    //string messageSender = (string)eventData.Parameters[(short)yuan.YuanPhoton.ParameterType.MessageSender];
	                    //MessageType messageType = (MessageType)eventData.Parameters[(short)yuan.YuanPhoton.ParameterType.MessageType];
	                    //SM.messageText = messageText;
	                    //SM.messageSender = messageSender;
	                    //SM.messageType = messageType;
	                    //SM.isSend = true;
	                 //   SM.listMessage.Add(eventData.Parameters);
	              //  }
	              //  break;
			//case (byte)yuan.YuanPhoton.EventCode.BeOffline:
	              //  {
	                   
	              //  }
	             //   break;
			//case (byte)yuan.YuanPhoton.EventCode.SendTVMessage:
	             //   {
	               //     string messageText = (string)eventData.Parameters[(byte)yuan.YuanPhoton.ParameterType.MessageText];
	               //     TVMessage.Add(messageText);
	              //  }
	              //  break;
	       // }
		//}
		//catch(System.Exception ex)
		//{
		//	Debug.LogWarning (ex.ToString ());
		//}
   }

   public void OnOperationResponse(Zealm.OperationResponse operationResponse)
   {
       try
       {
         //      Debug.Log("InRoomOpcode++++++++++++++++++++++++++++++++++++++" + operationResponse.OperationCode);
           /*
           Zealm.OperationResponse tempOperation = new Zealm.OperationResponse();
           tempOperation.DebugMessage = operationResponse.DebugMessage ?? "";
           tempOperation.OperationCode = operationResponse.OperationCode == null ? (short)0 : operationResponse.OperationCode;
           tempOperation.Parameters = new Dictionary<short, object>();
           foreach (KeyValuePair<short, object> itemKey in operationResponse.Parameters)
           {
               tempOperation.Parameters.Add(itemKey.Key, itemKey.Value);
           }
           tempOperation.ReturnCode = operationResponse.ReturnCode == null ? (short)0 : operationResponse.ReturnCode;
           */
           Zealm.OperationResponse tempOperation = new Zealm.OperationResponse();
           tempOperation.DebugMessage = operationResponse.DebugMessage ?? "";
           tempOperation.OperationCode = operationResponse.OperationCode == null ? (short)0 : (short)operationResponse.OperationCode;
           tempOperation.Parameters = new Dictionary<short, object>();
           foreach (KeyValuePair<short, object> itemKey in operationResponse.Parameters)
           {
               tempOperation.Parameters.Add((short)itemKey.Key, itemKey.Value);
           }
           tempOperation.ReturnCode = operationResponse.ReturnCode == null ? (short)0 : operationResponse.ReturnCode;




           switch ((OpCode)operationResponse.OperationCode)
           {
               case OpCode.TriggerActivity:
                   {

                       switch (operationResponse.ReturnCode)
                       {
                           case (short)yuan.YuanPhoton.ReturnCode.Yes:
                               {
                                   string activityID = (string)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ActivityID];
                                   // 1: startup state . 2: shutdown state
                                   int activityState = (int)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ActivityState];

                                   try
                                   {
                                       PanelActivity.activityIDAndState[activityID] = activityState;
//                                       Debug.Log("-----------------------------------activityID:" + activityID + ",activityState" + activityState);
                                       if (null != PanelActivity.panelActivity)
                                       {
                                           PanelActivity.panelActivity.isRefresh = true;
                                       }
                                   }
                                   catch (System.Exception ex)
                                   {
                                       Debug.LogWarning(ex.ToString());
                                   }

                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Error:
                               {
                                   Debug.LogError(operationResponse.DebugMessage);
                               }
                               break;
                       }
                   }
                   break;

               case OpCode.SendMessage:
                   {
                       SM.listMessage.Add(operationResponse.Parameters);
                   }
                   break;
			case OpCode.SendTVMessage:
			
					{
						string messageText = (string)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.MessageText];
						bool isSystem =(bool)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.IsWin];

				     	TVMessage.Add(messageText,isSystem);
					}
				break;
               case OpCode.YuanDBUpdate:
                   {
                       string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)ReturnCode.Yes:
                               {
                                   //dicTempTable[tableName].Refresh();

                               }
                               break;
                           case (short)ReturnCode.No:
                               {
                                   string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                   //dicTempTable[tableName].IsUpdate = false;

                                   Debug.LogError("ÊýŸÝ¿âžUpdateDB Error Update£¡" + errorText);
                               }
                               break;
                           case (short)ReturnCode.Nothing:
                               {
                                   string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                   //dicTempTable[tableName].IsUpdate = false;

                                   Debug.LogWarning("ÊýŸÝ¿âÎÞ±äžü!" + errorText);
                               }
                               break;
                       }
                       if (dicTempTable.ContainsKey(tableName))
                       {
                           dicTempTable[tableName].DeleteRows.Clear();
                           dicTempTable[tableName].IsUpdate = false;
                           dicTempTable.Remove(tableName);
                       }
                       if (dicUpdateInfo.ContainsKey(tableName))
                       {
                           dicUpdateInfo[tableName].Clear();
                           dicUpdateInfo.Remove(tableName);
                       }
                       if (dicUpdateTime.ContainsKey(tableName))
                       {
                           //dicUpdateTime[tableName].Dispose ();
                           dicUpdateTime.Remove(tableName);
                       }
                   }
                   break;
               case OpCode.GetPlayers:
                   {
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)yuan.YuanPhoton.ReturnCode.Yes:
                               {
//                                   YuanTable yt = new YuanTable("PlayerInfo", "PlayerID");
//                                   yt.CopyToDictionaryAndParms(operationResponse.Parameters);
//                                   MMManage.getPlayersYT = yt;

									MMManage.getPlayersYT.Clear ();
									short num=0;
									foreach(KeyValuePair<short,object> item in operationResponse.Parameters)
									{
										if(num==(short)0)
										{
											MMManage.slotNumDic=((Dictionary<object,object>)item.Value).DicObjTo<short,object>(); 
										}
										else
										{
											Dictionary<string,string> dicYR=((Dictionary<object,object>)item.Value).DicObjTo<string,string>();
											yuan.YuanMemoryDB.YuanRow yr=new YuanRow();
											foreach(KeyValuePair<string,string> itemYR in dicYR)
											{
					
												yr.Add(itemYR.Key,itemYR.Value);
											}
											MMManage.getPlayersYT.Add (yr);
										}
										num++;
									}

                                   MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                   MMManage.DebugGetPlayers = operationResponse.DebugMessage;
                                   MMManage.isGetPlayers = true;
								   btnGameManagerBack.warnings.warningAllEnter.Close();
                                   return;
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                               {
                                   MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                   MMManage.DebugGetPlayers = operationResponse.DebugMessage;
                                   MMManage.isGetPlayers = true;
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Error:
                               {
                                   MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                   MMManage.DebugGetPlayers = operationResponse.DebugMessage;
                                   MMManage.isGetPlayers = true;
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                               {
                                   MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                   MMManage.DebugGetPlayers = operationResponse.DebugMessage;
                                   MMManage.isGetPlayers = true;
                                   Debug.Log("服务器负载已满，请耐心等待............");
                               }
                               break;

                       }
                   }
                   break;
               case OpCode.PlayerCreat:
                   {
                       MMManage.returnPlayerCreat = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                       MainMenuManage.dicPlayerCreat = operationResponse.Parameters;
                       MMManage.DebugPlayerCreat = operationResponse.DebugMessage;
                       MMManage.isPlayerCreat = true;
                   }
                   break;
               case OpCode.DeletePlayer:
                   {
                       MMManage.returnDeletePlayer = (ReturnCode)operationResponse.ReturnCode;
                       MMManage.DebugDeletePlayer = operationResponse.DebugMessage;
                       MMManage.isFast = (bool)operationResponse.Parameters[(short)ParameterType.PlayerType];
                       MMManage.isDeletePlayer = true;
                       return;
                   }
                   break;
               case OpCode.AddExperience:
                   {

                       switch (operationResponse.ReturnCode)
                       {
                           case (short)yuan.YuanPhoton.ReturnCode.Yes:
                               {
//                                   Debug.Log("-----------------------addExp");
                                   string[] strKey = (string[])operationResponse.Parameters[(short)Zealm.ParameterType.TableKey];
                                   string[] strValue = (string[])operationResponse.Parameters[(short)Zealm.ParameterType.TableSql];
                                   for (int i = 0; i < strKey.Length; i++)
                                   {
                                       if (strKey[i] == "PlayerLevel")
                                       {
                                           playerLevel = strValue[i];
                                           isUpdatePlayerLevel = true;
                                       }
                                       if (strKey[i] == "Exp")
                                       {
                                           playerExp = strValue[i];
                                           isUpdatePlayerLevel = true;
                                       }
                                       if (strKey[i] == "PlayerID")
                                       {
                                           playerID = strValue[i];
                                           isUpdatePlayerLevel = true;
                                       }
                                   }
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Error:
                               {
                                   Debug.LogError(operationResponse.DebugMessage);
                               }
                               break;
                       }
                   }
                   break;
               case OpCode.GetMyTeam:
                   {
                       ////Debug.Log("SSSSSSSSSSSSS" + operationResponse.ReturnCode);
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)yuan.YuanPhoton.ReturnCode.Yes:
                               {

                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                               {

                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Error:
                               {
                                   string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                   string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                   dicTempTable[tableName].IsUpdate = false;
                                   dicTempTable.Remove(tableName);
                                   Debug.LogError(operationResponse.DebugMessage);
                               }
                               break;
                       }
                   }
                   break;
               case OpCode.GetPlayerList:
                   {
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)ReturnCode.Yes:
                               {
                                   YuanTable responseTable = new YuanTable("", "");
                                   responseTable.CopyToDictionary(operationResponse.Parameters);

                                   if (dicTempTable.ContainsKey(responseTable.TableName))
                                   {
                                       dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                       dicTempTable[responseTable.TableName].Refresh();
                                       dicTempTable[responseTable.TableName].IsUpdate = false;
                                       dicTempTable.Remove(responseTable.TableName);
                                   }

                               }
                               break;
                           case (short)ReturnCode.Nothing:
                               {
                                   string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                   YuanTable nullYt = new YuanTable("", "");
                                   dicTempTable[tableName].Rows = nullYt.Rows;
                                   dicTempTable[tableName].IsUpdate = false;
                                   dicTempTable[tableName].Clear();
                                   dicTempTable.Remove(tableName);
//                                   Debug.Log(operationResponse.DebugMessage);
                               }
                               break;
                           case (short)ReturnCode.Error:
                               {
                                   string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                   string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                   dicTempTable[tableName].IsUpdate = false;
                                   dicTempTable[tableName].Clear();
                                   dicTempTable.Remove(tableName);
                                   Debug.LogError("ÊýŸÝ¿â²GetPlayerList,DB Error!/n" + errorText);
                               }
                               break;
                       }
                       return;
                   }
                   break;
              

               case OpCode.GetRank:
                   {
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)ReturnCode.Yes:
                               {
                                   
					Dictionary<string, int> rankTable =new Dictionary<string, int>();
					Dictionary<string, int> rankTables =new Dictionary<string, int>();

					string tableName = (string)operationResponse.Parameters[(short)21];

					for(short i=0;i<operationResponse.Parameters.Count-2;i++){
						
						Dictionary<string,string> dic1=((Dictionary<object, object>)operationResponse.Parameters[(short)i]).DicObjTo<string ,string>(); ;
						String name=dic1["name"];
						int rank=Int32.Parse( dic1["rank"]);
						
						rankTable.Add(name,rank);
					}

					//Dictionary<short,object> dic=((Dictionary<object, object>)operationResponse.Parameters[(short)ParameterType.RankTable]).DicObjTo<short ,object>();


					//Dictionary<string, int> rankTable = ((Dictionary<object, object>)operationResponse.Parameters[(short)ParameterType.RankTable]).DicObjTo<string, int>();
                     int myRank = (int)operationResponse.Parameters[(short)ParameterType.MyRank];
                                  

                                   if (dicYuanRank.ContainsKey(tableName))
                                   {
                                       dicYuanRank[tableName].dicMyRank = rankTable;
                                       dicYuanRank[tableName].myRank = myRank;
                                       dicYuanRank[tableName].IsUpdate = false;
                                       dicYuanRank.Remove(tableName);
                                   }

									if(tableName.Equals("GuildRankingrank"))
									{
									RefreshGuildBuild.GuildRankNum = myRank ; 
									}
					if(tableName.Equals("ColosseumPointrank"))
					{
						RefreshPVP.PVPRankNum = myRank;
					}

                               }
                               break;
                           case (short)ReturnCode.Error:
                               {
                                   Debug.LogError(operationResponse.DebugMessage);
                                   string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                   if (dicYuanRank.ContainsKey(tableName))
                                   {
                                       dicYuanRank[tableName].IsUpdate = false;
                                       dicYuanRank.Remove(tableName);

                                   }

                               }
                               break;
                       }
                       return;
                   }
                   break;
               case OpCode.GetServerTime:
                   {
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)yuan.YuanPhoton.ReturnCode.Yes:
                               {

                                   try
                                   {
                                       serverTime = System.DateTime.Parse((string)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ServerTime]); ;
                                       timeout = 0;
                                       return;
                                   }
                                   catch (System.Exception ex)
                                   {
                                       Debug.LogError(ex.ToString());
                                   }
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Error:
                               {
                                   Debug.LogError(operationResponse.DebugMessage);
                               }
                               break;
                       }
                   }
                   break;
				case OpCode.GetRankTopYT:
               case OpCode.GetPlayerForName:
               case OpCode.GetRandomPlayer:
               case OpCode.GetMyMail:
               case OpCode.GetTableForID:
               case OpCode.GetTablesSomeForIDs:
				case OpCode.GetTablesSomeForNames:
                   {
                       switch (operationResponse.ReturnCode)
                       {
                           case (short)yuan.YuanPhoton.ReturnCode.Yes:
                               {
                                   YuanTable responseTable = new YuanTable("", "");
                                   responseTable.CopyToDictionary(operationResponse.Parameters);

                                   if (dicTempTable.ContainsKey(responseTable.TableName))
                                   {
                                       dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                       dicTempTable[responseTable.TableName].Refresh();
                                       dicTempTable[responseTable.TableName].IsUpdate = false;
                                       dicTempTable.Remove(responseTable.TableName);
                                   }
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.No:
                               {
                                   string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                   YuanTable nullYt = new YuanTable(tableName, "id");

                                   if (dicTempTable.ContainsKey(tableName))
                                   {
                                       dicTempTable[tableName].Rows = nullYt.Rows;
                                       dicTempTable[tableName].IsUpdate = false;
                                       dicTempTable.Remove(tableName);
                                   }
                               }
                               break;
                           case (short)yuan.YuanPhoton.ReturnCode.Error:
                               {
                                   string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                   if (dicTempTable.ContainsKey(tableName))
                                   {
                                       dicTempTable[tableName].IsUpdate = false;
                                       dicTempTable.Remove(tableName);
                                   }
                                   Debug.LogWarning(operationResponse.DebugMessage);
                               }
                               break;
                       }
                   }
                   break;
           }

           if (operationResponse.OperationCode == (short)OpCode.YuanDBUpdate ||
               operationResponse.OperationCode == (short)OpCode.GetServerTime ||
               operationResponse.OperationCode == (short)OpCode.TriggerActivity ||
               operationResponse.OperationCode == (short)OpCode.Decision ||
               operationResponse.OperationCode == (short)OpCode.ACTOR_LOGIN_SERVER ||
               operationResponse.OperationCode == (short)OpCode.UNIT_REFRESH_SERVER ||
               operationResponse.OperationCode == (short)OpCode.UNIT_MULTI_REFRESH_SERVER ||
               operationResponse.OperationCode == (short)OpCode.PLAYER_MOVE_SERVER ||
               operationResponse.OperationCode == (short)OpCode.BROADCAST_USE_SKILL ||
               operationResponse.OperationCode == (short)OpCode.SERVER_NEW_DAY ||
               operationResponse.OperationCode == (short)OpCode.EnterGame ||
               operationResponse.OperationCode == (short)OpCode.ATTACK_TARGET ||
               operationResponse.OperationCode == (short)OpCode.AttackMonster ||
               operationResponse.OperationCode == (short)OpCode.SYNC_ACT||
               operationResponse.OperationCode == (short)OpCode.MOVE_CLIENT||
               operationResponse.OperationCode == (short)OpCode.SET_CUR_HP ||
               operationResponse.OperationCode == (short)OpCode.DecisionFallBack ||
               operationResponse.OperationCode == (short)OpCode.HP_CHANGED)
           {

           }
           else
           {
               this.listOperationResponse.Enqueue(tempOperation);
            //   Debug.Log("list------------------------------------------------");
           }
           //btnGameManagerBack.operationResponse.Add(tempOperation);
       }
       catch (System.Exception ex)
       {
           Debug.LogError(ex.ToString());
       }
   }

	private Queue<Zealm.OperationResponse> listOperationResponse=new Queue<Zealm.OperationResponse>();

    public void OnOperationResponse(OperationResponse operationResponse)
    {/*
    
        try
		{
			ExitGames.Client.Photon.OperationResponse tempOperation=new ExitGames.Client.Photon.OperationResponse();
			tempOperation.DebugMessage=operationResponse.DebugMessage??"";
			tempOperation.OperationCode=operationResponse.OperationCode==null?(byte)0:operationResponse.OperationCode;
			tempOperation.Parameters=new Dictionary<byte, object>();
			foreach(KeyValuePair<short,object> itemKey in operationResponse.Parameters)
			{
				tempOperation.Parameters.Add (itemKey.Key,itemKey.Value);
			}
		tempOperation.ReturnCode=operationResponse.ReturnCode==null?(byte)0:operationResponse.ReturnCode;
			
        switch (operationResponse.OperationCode)
        {

			case (short)yuan.YuanPhoton.OperationCode.BindUserID:
                {
                    try
                    {
                        MMManage.returnBindID = (ReturnCode)operationResponse.ReturnCode;
                        MMManage.DebugBindID = operationResponse.DebugMessage;
                        MMManage.isBindID = true;
                    }
                    catch
                    {

                    }

                    try
                    {
                        BtnGameManagerBack.operationResponse.Add(operationResponse);
                    }
                    catch
                    { }
					return;
                }
                break;
			case (short)yuan.YuanPhoton.OperationCode.SetID:
			{

				//string[] strKey = (string[])operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
				//string[] strValue= (string[])operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
				//MMManage.setIDKey=strKey;
				//MMManage.setIDValue=strValue;
				//MMManage.isGetSetID=true;
			}
				break;	
            case (short)yuan.YuanPhoton.OperationCode.GetPlayers:
                {
					try
					{
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                YuanTable yt = new YuanTable("PlayerInfo", "PlayerID");
                                yt.CopyToDictionaryAndParms(operationResponse.Parameters);
                                MMManage.getPlayersYT = yt;
                                MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                MMManage.DebugGetPlayers = operationResponse.DebugMessage;
                                MMManage.isGetPlayers = true;
								return;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                MMManage.DebugGetPlayers = operationResponse.DebugMessage;
								MMManage.slotNumDic = operationResponse.Parameters;
                                MMManage.isGetPlayers = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                MMManage.returrnGetPlayers = (ReturnCode)operationResponse.ReturnCode;
                                MMManage.DebugGetPlayers = operationResponse.DebugMessage;
                                MMManage.isGetPlayers = true;
                            }
                            break;
							
                    }
				}
				catch(System.Exception ex)
				{
					Debug.LogError (ex.ToString ());
				}
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.DeletePlayer:
                {
                    MMManage.returnDeletePlayer = (ReturnCode)operationResponse.ReturnCode;
                    MMManage.DebugDeletePlayer = operationResponse.DebugMessage;
                    MMManage.isFast = (bool)operationResponse.Parameters[(short)ParameterType.PlayerType];
                    MMManage.isDeletePlayer = true;
					return;
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.PlayerCreat:
                {
                    MMManage.returnPlayerCreat = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                    MainMenuManage.dicPlayerCreat = operationResponse.Parameters;
                    MMManage.DebugPlayerCreat = operationResponse.DebugMessage;
                    MMManage.isPlayerCreat = true;
                }
                break;				
            case (short)yuan.YuanPhoton.OperationCode.GetServerTime:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
								
								try
								{
	                                serverTime = System.DateTime.Parse((string)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ServerTime]);;
	                                timeout=0;
									return;
								}
								catch(System.Exception ex)
								{
									Debug.LogError (ex.ToString ());
								}
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;
                    }
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.YuanDBGet:
                {
			 
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Yes:
                            {
                              
                                YuanTable responseTable = new YuanTable("", "");
                                responseTable.CopyToDictionary(operationResponse.Parameters);
                                if (dicTempTable.ContainsKey(responseTable.TableName))
                                {
                                    dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                    dicTempTable[responseTable.TableName].Refresh();
                                    dicTempTable[responseTable.TableName].IsUpdate = false;
                                    dicTempTable.Remove(responseTable.TableName);
									if(dicUpdateTime.ContainsKey (responseTable.TableName))
									{
										dicUpdateTime[responseTable.TableName].Dispose ();
										dicUpdateTime.Remove (responseTable.TableName);
									}
                                }
								return;
                            }
                            break;
                        case (short)ReturnCode.No:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
 									if(dicUpdateTime.ContainsKey (tableName))
									{
										dicUpdateTime[tableName].Dispose ();
										dicUpdateTime.Remove (tableName);
									}                               
                                Debug.LogError("ÊýŸÝ¿â²GetDB Error!\n" + errorText);
                            }
                            break;
                    }
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.YuanDBUpdate:
                {
                    string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Yes:
                            {
                                //dicTempTable[tableName].Refresh();

                            }
                            break;
                        case (short)ReturnCode.No:
                            {
                                string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                //dicTempTable[tableName].IsUpdate = false;
				
                                Debug.LogError("ÊýŸÝ¿âžUpdateDB Error Update£¡" + errorText);
                            }
                            break;
                        case (short)ReturnCode.Nothing:
                            {
                                string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                //dicTempTable[tableName].IsUpdate = false;
			
                                Debug.LogWarning("ÊýŸÝ¿âÎÞ±äžü!" + errorText);
                            }
                            break;
                    }
					if(dicTempTable.ContainsKey (tableName))
					{
                             dicTempTable[tableName].DeleteRows.Clear();
                             dicTempTable[tableName].IsUpdate = false;
                             dicTempTable.Remove(tableName);
					}
					if(dicUpdateInfo.ContainsKey (tableName))
					{
						dicUpdateInfo[tableName].Clear ();
						dicUpdateInfo.Remove (tableName);
					}
					if(dicUpdateTime.ContainsKey (tableName))
					{
						//dicUpdateTime[tableName].Dispose ();
						dicUpdateTime.Remove (tableName);
					}		
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.GetPlayerList:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Yes:
                            {
                                YuanTable responseTable = new YuanTable("", "");
                                responseTable.CopyToDictionary(operationResponse.Parameters);

                                if (dicTempTable.ContainsKey(responseTable.TableName))
                                {
                                    dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                    dicTempTable[responseTable.TableName].Refresh();
                                    dicTempTable[responseTable.TableName].IsUpdate = false;
                                    dicTempTable.Remove(responseTable.TableName);
                                }

                            }
                            break;
                        case (short)ReturnCode.Nothing:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                YuanTable nullYt = new YuanTable("","");
                                dicTempTable[tableName].Rows = nullYt.Rows;
                                dicTempTable[tableName].IsUpdate = false;
								dicTempTable[tableName].Clear ();
                                dicTempTable.Remove(tableName);
                                Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;
                        case (short)ReturnCode.Error:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
								dicTempTable[tableName].Clear ();
                                dicTempTable.Remove(tableName);
                                Debug.LogError("ÊýŸÝ¿â²GetPlayerList,DB Error!/n" + errorText);
                            }
                            break;
                    }
					return;
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.GetRank:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Yes:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                Dictionary<string, int> rankTable = (Dictionary<string, int>)operationResponse.Parameters[(short)ParameterType.RankTable];
                                 int myRank = (int)operationResponse.Parameters[(short)ParameterType.MyRank];

                                if (dicYuanRank.ContainsKey(tableName))
                                {
                                    dicYuanRank[tableName].dicMyRank = rankTable;
                                    dicYuanRank[tableName].myRank = myRank;
                                    dicYuanRank[tableName].IsUpdate = false;
                                    dicYuanRank.Remove(tableName);
                                }

                            }
                            break;
                        case (short)ReturnCode.Error:
                            {
                                Debug.LogError(operationResponse.DebugMessage);
                                string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                if (dicYuanRank.ContainsKey(tableName))
                                {
                                    dicYuanRank[tableName].IsUpdate = false;
                                    dicYuanRank.Remove(tableName);
                                   
                                }
                                
                            }
                            break;
                    }
                    return;
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.GetRankOne:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Yes:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                int myRank = (int)operationResponse.Parameters[(short)ParameterType.MyRank];

                                if (dicYuanRank.ContainsKey(tableName))
                                {
                                    dicYuanRank[tableName].myRank = myRank;
                                    dicYuanRank[tableName].IsUpdate = false;
                                    dicYuanRank.Remove(tableName);
                                }

                            }
                            break;
                        case (short)ReturnCode.Error:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                if (dicYuanRank.ContainsKey(tableName))
                                {
                                    dicYuanRank[tableName].IsUpdate = false;
                                    dicYuanRank.Remove(tableName);
                                    Debug.LogError(operationResponse.DebugMessage);
                                }
                            }
                            break;
                    }
                    return;
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.GetMyTeam:
                {
                    ////Debug.Log("SSSSSSSSSSSSS" + operationResponse.ReturnCode);
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                //YuanTable responseTable = new YuanTable("", "");
                                //responseTable.CopyToDictionary(operationResponse.Parameters);

                                //if (dicTempTable.ContainsKey(responseTable.TableName))
                                //{
                                //    dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                //    dicTempTable[responseTable.TableName].Refresh();
                                //    dicTempTable[responseTable.TableName].IsUpdate = false;
                                //    dicTempTable.Remove(responseTable.TableName);
                                //}
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                ////Debug.Log("QQQQQQQQQQQ:");
                                //string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                ////Debug.Log("WWWWWWWWW:");
                                //YuanTable nullYt = new YuanTable(tableName, "id");

                                ////Debug.Log("SSSSSSggggg:" + tableName);
                                //dicTempTable[tableName].Rows = nullYt.Rows;
                                //dicTempTable[tableName].IsUpdate = false;
                                //dicTempTable.Remove(tableName);
                                //Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                                Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;
                    }
                }
                break;
            case (short)yuan.YuanPhoton.OperationCode.GetMyGrop:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                YuanTable responseTable = new YuanTable("", "");
                                responseTable.CopyToDictionary(operationResponse.Parameters);

                                if (dicTempTable.ContainsKey(responseTable.TableName))
                                {
                                    dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                    dicTempTable[responseTable.TableName].Refresh();
                                    dicTempTable[responseTable.TableName].IsUpdate = false;
                                    dicTempTable.Remove(responseTable.TableName);
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                YuanTable nullYt = new YuanTable(tableName, "id");

                                dicTempTable[tableName].Rows = nullYt.Rows;
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                                Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ErrorParameterCode.TableName];
                                string errorText = (string)operationResponse.Parameters[(short)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                                Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;
                    }
                }
                break;
			case (short)yuan.YuanPhoton.OperationCode.ClientMoney:
                {
					string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
				
					if(dicYuanBack.ContainsKey(tableName))
					{
					
						dicYuanBack[tableName].isUpate=false;
						dicYuanBack[tableName].opereationResponse=operationResponse;
						dicYuanBack.Remove (tableName);
					}
				}
				break;
			case (short)yuan.YuanPhoton.OperationCode.GetRankTopYT:	
			case (short)yuan.YuanPhoton.OperationCode.GetPlayerForName:	
			case (short)yuan.YuanPhoton.OperationCode.GetRandomPlayer:	
			case (short)yuan.YuanPhoton.OperationCode.GetMyMail:	
			case (short)yuan.YuanPhoton.OperationCode.GetTableForID:
			case (short)yuan.YuanPhoton.OperationCode.GetTablesSomeForIDs:
                {
					switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                YuanTable responseTable = new YuanTable("", "");
                                responseTable.CopyToDictionary(operationResponse.Parameters);

                                if (dicTempTable.ContainsKey(responseTable.TableName))
                                {
                                    dicTempTable[responseTable.TableName].Rows = responseTable.Rows;
                                    dicTempTable[responseTable.TableName].Refresh();
                                    dicTempTable[responseTable.TableName].IsUpdate = false;
                                    dicTempTable.Remove(responseTable.TableName);
                                }
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                YuanTable nullYt = new YuanTable(tableName, "id");

                                dicTempTable[tableName].Rows = nullYt.Rows;
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                string tableName = (string)operationResponse.Parameters[(short)ParameterType.TableName];
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                                Debug.LogWarning(operationResponse.DebugMessage);
                            }
                            break;
                    }
				}
				break;		
			case  (short)yuan.YuanPhoton.OperationCode.AddExperience:
                {
				
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//								Debug.Log ("-----------------------addExp");
								string[] strKey = (string[])operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableKey];
								string[] strValue= (string[])operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.TableSql];
								for(int i=0;i<strKey.Length;i++)
								{
									if(strKey[i]=="PlayerLevel")
									{
										playerLevel=strValue[i];
										isUpdatePlayerLevel=true;
									}
									if(strKey[i]=="Exp")
									{
										playerExp=strValue[i];
										isUpdatePlayerLevel=true;
									}
									if(strKey[i]=="PlayerID")
									{
										playerID=strValue[i];
										isUpdatePlayerLevel=true;
									}									
								}
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                Debug.LogError(operationResponse.DebugMessage);
                            }
                            break;						
                    }
                }
                break;

				
        }

			this.listOperationResponse.Enqueue (tempOperation);

		}
		catch(System.Exception ex)
		{
			
			Debug.LogError (ex.ToString ());
		}
		*/
    }


	public void OnStatusChanged(bool isConnected)
	{
		if(isConnected)
		{
			Debug.Log("Connected:LgoicServer");
			ServerConnected = true;
			//!john.add.alive
			m_dwKeepAliveInternal=m_dwKeepAliveTime;
			LoadbalancingPeer.SetLogicObj(this);
		}
		else
		{
			Debug.Log("Disconnect:LgoicServer");
			ServerConnected = false;
			LoadbalancingPeer.SetLogicObj(null);
		}
	}    public void OnStatusChanged(StatusCode statusCode)
    {
        this.DebugReturn(0, string.Format("Peer×ŽÌ¬»Øµ÷:{0}", statusCode));
        switch (statusCode)
        {
            case StatusCode.Connect:
                ServerConnected = true;
			      //!john.add.alive
			    m_dwKeepAliveInternal=m_dwKeepAliveTime;
                Debug.Log("johnlogref Connected:InRoom");
			    LoadbalancingPeer.SetLogicObj(this);
                break;
            case StatusCode.Disconnect:
                ServerConnected = false;
                Debug.Log("johnlogref Disconnet:InRoom");
			    LoadbalancingPeer.SetLogicObj(null);
                break;
        }
    }
	/// <summary>
	/// 充值月卡
	/// </summary>
	public void Rechargecard(String qudao)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			
		//	parameter.Add((short)yuan.YuanPhoton.PayLanWeiParams.userName, userName);
			parameter.Add((short)yuan.YuanPhoton.PayLanWeiParams.Qudao, qudao);
			////this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.payLanwei, parameter, true);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.PaycardBuy, parameter);
			ZealmConnector.sendRequest(zz);

		}
	}
	/// <summary>
	/// 领取月卡福利
	/// </summary>
	public void Receivemonthlybenefits()
	{
		if(this.serverConnected)
		{
		Dictionary<short, object> parameter = new Dictionary<short, object>();

		ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.payCard, parameter);
		ZealmConnector.sendRequest(zz);
	}

	}
	//    public void ReturnRequest(ReturnCode returnCode,RequstType requstType,params KeyValuePair<short,object>[] list)
	/// <summary>
	/// 获取充值第一名的数据
	/// </summary>
	public void getfirstMoney()
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)57, (short)10);

			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GETRANKMONEY, parameter);
			ZealmConnector.sendRequest(zz);
		}
		
	}
	/// <summary>
	/// 获取战斗力第一名的数据
	/// </summary>
	public void getfirstForceValue()
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)57, (short)4);

			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GETRANKForceValue, parameter);
			ZealmConnector.sendRequest(zz);
		}
		
	}
	/// <summary>
	/// 获取决斗场第一名的数据
	/// </summary>
	public void getfirstPVP()
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)57, (short)3);

			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.GETRANKPVP, parameter);
			ZealmConnector.sendRequest(zz);
		}
		
	}
	/// <summary>
	/// 进狩猎模式扣血石
	/// </summary>
	public void getHuntingMap(int maplevel,string mapid,int level,int Type)//副本等级，副本id,副本难度
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)1, maplevel);
			parameter.Add((short)2, mapid);
			parameter.Add((short)3, level);
			parameter.Add((short)4, Type);

			
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.HuntingMap, parameter);
			ZealmConnector.sendRequest(zz);
		}
		
	}
	/// <summary>
	///决斗影魔的时候,把影魔的playerid传给服务器
	/// </summary>
	public void Playerdueld(int playerid)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)1, playerid);
			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.Playerduel, parameter);
			ZealmConnector.sendRequest(zz);
		}
		
	}
	/// <summary>
	///添加离线体力
	/// </summary>
	public void addtili()
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();

			ZMNetDataLikePhoton zz = new ZMNetDataLikePhoton((short)OpCode.Addtili, parameter);
			ZealmConnector.sendRequest(zz);
		}
		
	}

	
    #endregion
}
