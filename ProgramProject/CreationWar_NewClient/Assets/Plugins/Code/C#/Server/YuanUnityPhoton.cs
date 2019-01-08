using UnityEngine;
using System.Collections;
using yuan.YuanPhoton;
using System.Collections.Generic;
using yuan.YuanMemoryDB;
using System.Security;
using ExitGames.Client.Photon;

public class YuanUnityPhoton : IPhotonPeerListener
{
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string ServerAddress = "localhost:5055";

    /// <summary>
    /// 游戏版本
    /// </summary>
	public static readonly System.Version GameVersion = new System.Version("1.2.9");

    /// <summary>
    /// 服务器应用名称
    /// </summary>
    public string ServerApplication = "YuanPhotonServer";
	
	public static string LanguageVersion = "CH";
	public static string keyStore=string.Empty;

    public static Dictionary<short, object> dicBenefitsInfo;
	public Dictionary<string,string> dicLanguage;
	public string Get (string key)
	{
		if(dicLanguage!=null)
		{
			string val;
			return (dicLanguage.TryGetValue(key, out val)) ? val : key;
		}
		else
		{
			return key;
		}
	}

    public Dictionary<int, Dictionary<yuan.YuanPhoton.GuildLevelUp, int>> dicGuildLevel = new Dictionary<int, Dictionary<GuildLevelUp, int>>();

    private float serviceTimeInterval;
    /// <summary>
    /// sevice刷新频率
    /// </summary>
    public float ServiceTimeInterval
    {
        get { return serviceTimeInterval; }
        set { serviceTimeInterval = value; }
    }

    //public UILabel lblServerStatus;
    //public UILabel lblLoginWarning;
    //public UILabel lblLogonWarning;
    //public UILabel lblCreatWarning;
    //public UILabel lblGetPwdWarning;
    //public UILabel lblUpdatePwdWarning;
    //public UILabel lblPlayerBindWarning;
    //public List<UILabel> listPlayerBtn;
    //public List<GameObject> listMenu;
    //public UIButton btnCreatPlayer;
    //public BtnClick btnCreatPlayerBack;
    //public BtnClick btnCreatPlayerEnter;
    //public BtnClick btnSelectPlayerBack;
    //public BtnClick btnSelectPlayerBind;
    //public GameObject btnSelectPlayerPwd;
    //public GameObject btnServer;
    //public UIGrid gridServer;
    //public UIToggle cbxRemeberMe;
    //public UIInput txtUserID;
    //public UIInput txtUserPwd;

    [HideInInspector]
    public string userID;
    [HideInInspector]
    public string userPwd;

    private bool loginStatus;
    /// <summary>
    /// 正常登陆状态
    /// </summary>
    public bool LoginStatus
    {
        get { return loginStatus; }
    }

    private yuan.YuanPhoton.ReturnCode fastLoginStatus;
    /// <summary>
    /// 快速登陆状态
    /// </summary>
    public yuan.YuanPhoton.ReturnCode FastLoginStatus
    {
        get { return fastLoginStatus; }
    }

    [HideInInspector]
    public int getRet = 0;

    /// <summary>
    /// peer
    /// </summary>
    public PhotonPeer peer;
    //private ExitGames.Concurrency.Fibers.IFiber fiber;

    [HideInInspector]
    private bool serverConnected;
    /// <summary>
    /// 服务器的连接状态(只读)
    /// </summary>
    public bool ServerConnected
    {
        get { return serverConnected; }
        set
        {
            serverConnected = value;
            if (serverConnected == true)
            {
				//临时写死，需要张继新修改

				//				LoginValidation ("d16013603ced52c0dcb5e37fe5013e63");
                SendGameVersion();
                ReadTables();
            }
        }
    }

    private bool isLogin = false;
    /// <summary>
    /// 是否正在登陆中
    /// </summary>
    public bool IsLogin
    {
        get { return isLogin; }
        set { isLogin = value; }
    }

    public MainMenuManage MMManage;
    public BtnGameManagerBack btnGameManagerBack;


#region //!john.add.alive.gate
    //!心跳包发送间隔
	static long m_dwKeepAliveInternal=5000;
	//
	static long m_dwKeepAliveTime=500;
	//下次数据发送时间
	static long m_dwNextSendTime=0;
#endregion
	
	
    public YuanUnityPhoton()
    {
		try
		{
	        this.serverConnected = false;
	        this.loginStatus = false;
	        this.fastLoginStatus = yuan.YuanPhoton.ReturnCode.No;

//            Debug.Log("johnlogref YuanUnityPhoton creat peer type");
            this.peer = new PhotonPeer(this, ConnectionProtocol.Tcp);
	        //this.serviceTimeInterval = 50;
	        //this.serviceTimer.Interval = this.serviceTimeInterval;
	        //this.serviceTimer.AutoReset = true;
	        //this.serviceTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnServiceTimer);
	        //this.serviceTimer.Enabled = true;
			this.serviceTimer=new System.Threading.Timer(OnServiceTimer,null,0,50);
			isServiceTimer=true;

            timerTableRead = new System.Timers.Timer();
            timerTableRead.AutoReset = true;
            timerTableRead.Interval = 1000;
            timerTableRead.Elapsed += new System.Timers.ElapsedEventHandler(ThreadReadTable);
            timerTableRead.Start();
			//this.timerTableRead=new System.Threading.Timer(ThreadReadTable,null,0,1000);
	       
	        ytGameItem = new YuanTable("GameItem", "id");
	        ytGameSkill = new YuanTable("GameSkill", "id");
	        ytMapLevel = new YuanTable("MapLevel", "id");
	        ytNPCInfo = new YuanTable("NPCInfo", "id");
	        ytTask = new YuanTable("Task", "id");
	        ytPlayerService = new YuanTable("PlayerService", "id");
	        ytObjective = new YuanTable("Objective", "id");
	        ytStoreItem = new YuanTable("StoreItem", "id");
	        ytTablePacks = new YuanTable("Packs", "id");
	        ytNotice = new YuanTable("Notice", "id");
	        ytBlueprint = new YuanTable("Blueprint", "id");
	        ytEverydayAim = new YuanTable("EverydayAim", "id");
	        ytPlayerTitle = new yuan.YuanMemoryDB.YuanTable("PlayerTitle", "id");
	        ytHelp = new yuan.YuanMemoryDB.YuanTable("GameHelp", "id");
	        ytActivity = new yuan.YuanMemoryDB.YuanTable("Activity", "id");
	        ytPlayer = new YuanTable("PlayerInfo", "PlayerID");
	        ytTaskItem = new YuanTable("TaskItem", "id");
			ytPlayerPet=new yuan.YuanMemoryDB.YuanTable("PlayerPet","id");
			ytBattlefield=new yuan.YuanMemoryDB.YuanTable("Battlefield","id");
            ytEquipmentenhance = new yuan.YuanMemoryDB.YuanTable("Equipmentenhance", "id");
            ytEquipmentresolve = new yuan.YuanMemoryDB.YuanTable("Equipmentresolve", "id");
            ytBosstower = new yuan.YuanMemoryDB.YuanTable("bosstower", "id");
			ytPlayerSkill=new yuan.YuanMemoryDB.YuanTable("playerskill","id");
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
            //!john.add.alive.gate
            m_dwKeepAliveInternal = 1;
			if(yuanUnityPhotonInstantiate!=null)
			{
				if(yuanUnityPhotonInstantiate.peer!=null)
				{
					yuanUnityPhotonInstantiate.peer.Disconnect ();
				}
				if(yuanUnityPhotonInstantiate.serviceTimer!=null)
				{
					//yuanUnityPhotonInstantiate.serviceTimer.Enabled=false;
					yuanUnityPhotonInstantiate.isServiceTimer=false;
					yuanUnityPhotonInstantiate.serviceTimer.Dispose ();
					yuanUnityPhotonInstantiate.serviceTimer=null;
				}
				if(yuanUnityPhotonInstantiate.timerTableRead!=null)
				{
					//yuanUnityPhotonInstantiate.timerTableRead.Enabled=false;
					yuanUnityPhotonInstantiate.isTimerTableRead=false;
					yuanUnityPhotonInstantiate.timerTableRead.Dispose ();
					yuanUnityPhotonInstantiate.timerTableRead=null;
				}
				//yuanUnityPhotonInstantiate.RefreshDic ();
				//yuanUnityPhotonInstantiate=null;
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
	}

    public static YuanUnityPhoton yuanUnityPhotonInstantiate;
    public static YuanUnityPhoton GetYuanUnityPhotonInstantiate()
    {   
				try
		{
        if (yuanUnityPhotonInstantiate == null)
        {
            yuanUnityPhotonInstantiate = new YuanUnityPhoton();
        }
        return yuanUnityPhotonInstantiate;
					}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
			return null;
		}
    }

    public static YuanUnityPhoton NewYuanUnityPhotonInstantiate()
    {
		try
		{
            Debug.Log("johnlogref NewYuanUnityPhotonInstantiate creat peer type");
            GetYuanUnityPhotonInstantiate().peer = new PhotonPeer(GetYuanUnityPhotonInstantiate(), ConnectionProtocol.Tcp);
	
	        //GetYuanUnityPhotonInstantiate().serviceTimer.Enabled = false;
	        //GetYuanUnityPhotonInstantiate().serviceTimer.Dispose();
	        //GetYuanUnityPhotonInstantiate().serviceTimer = new System.Timers.Timer();
	        //GetYuanUnityPhotonInstantiate().serviceTimeInterval = 50;
	        //GetYuanUnityPhotonInstantiate().serviceTimer.Interval = GetYuanUnityPhotonInstantiate().serviceTimeInterval;
	        //GetYuanUnityPhotonInstantiate().serviceTimer.AutoReset = true;
	        //GetYuanUnityPhotonInstantiate().serviceTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetYuanUnityPhotonInstantiate().OnServiceTimer);
	        //GetYuanUnityPhotonInstantiate().serviceTimer.Enabled = true;
			
			if(GetYuanUnityPhotonInstantiate().serviceTimer==null)
			{
				GetYuanUnityPhotonInstantiate().serviceTimer=new System.Threading.Timer(GetYuanUnityPhotonInstantiate().OnServiceTimer,null,0,50);
				GetYuanUnityPhotonInstantiate().isServiceTimer=true;
			}
			else
			{
				GetYuanUnityPhotonInstantiate().isServiceTimer=true;
			}
			
			if( GetYuanUnityPhotonInstantiate().timerTableRead==null)
			{
				GetYuanUnityPhotonInstantiate ().isTimerTableRead=false;

                GetYuanUnityPhotonInstantiate().timerTableRead = new System.Timers.Timer();
                GetYuanUnityPhotonInstantiate().timerTableRead.AutoReset = true;
                GetYuanUnityPhotonInstantiate().timerTableRead.Interval = 1000;
                GetYuanUnityPhotonInstantiate().timerTableRead.Elapsed += new System.Timers.ElapsedEventHandler(GetYuanUnityPhotonInstantiate().ThreadReadTable);
                GetYuanUnityPhotonInstantiate().timerTableRead.Start();
				 //GetYuanUnityPhotonInstantiate().timerTableRead=new System.Threading.Timer(GetYuanUnityPhotonInstantiate ().ThreadReadTable,null,0,1000);
				
			}			
			else
			{
				GetYuanUnityPhotonInstantiate ().isTimerTableRead=false;
			}
			
			
			//if( GetYuanUnityPhotonInstantiate().timerTableRead!=null)
			//{
			//	 GetYuanUnityPhotonInstantiate().timerTableRead.Enabled=false;
			//	GetYuanUnityPhotonInstantiate().timerTableRead.Dispose ();
			//}
	        // GetYuanUnityPhotonInstantiate().timerTableRead = new System.Timers.Timer();
	        // GetYuanUnityPhotonInstantiate().timerTableRead.AutoReset = true;
	        // GetYuanUnityPhotonInstantiate().timerTableRead.Interval = 1000;
	        // GetYuanUnityPhotonInstantiate().timerTableRead.Elapsed += new System.Timers.ElapsedEventHandler(GetYuanUnityPhotonInstantiate().ThreadReadTable);			
			
	        GetYuanUnityPhotonInstantiate().RefreshDic();
	        return GetYuanUnityPhotonInstantiate();
	        // return  yuanUnityPhotonInstantiate = new YuanUnityPhoton();
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
			return null;
		}
    }


    //!john.add.alive.gate
    private void OnServiceTimer(object sender)
    {
        //if (this.peer.PeerState == PeerStateValue.Disconnected)
        //{
        //    this.Connect();
        //}
        try
        {
		//	NetDataManager.update ();
            if (isServiceTimer)
            {
				 this.peer.Service();
                if (this.peer.PeerState != PeerStateValue.Disconnected)
                {
                    if (System.Environment.TickCount >= m_dwNextSendTime)
                    {
                       
                        m_dwNextSendTime = System.Environment.TickCount + m_dwKeepAliveInternal;
                    }
                    else
                    {
                        this.peer.DispatchIncomingCommands();

                        if (m_dwKeepAliveInternal < 100)
                        {
                            this.peer.SendOutgoingCommands();
                        }
                        else if (this.peer.QueuedIncomingCommands > 0 || this.peer.QueuedOutgoingCommands > 0)
                        {
                            this.peer.SendOutgoingCommands();

                        }
                    }


                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }



    void OnApplicationQuit()
    {
        this.peer.Disconnect();

    }



    public virtual void Connect()
    {
//        try
//        {
//            //Debug.Log("--------------------" + ServerAddress + "," + ServerApplication);
//            //!john.add.alive.gate
//            m_dwKeepAliveInternal = 1;
//			PhotonHandler.ShowLog(string.Format("YuanUnityPhoton Connect addr:{0},app:{1}",ServerAddress,ServerApplication));
//            this.peer.Connect(ServerAddress, ServerApplication);
//        }
//        catch (SecurityException ex)
//        {
//            this.DebugReturn(0, Get("info542") + ex.ToString());
//        }

		ZealmSocketConnection.SetConnectionEvent (this.OnStatusChanged2);
		NetDataManager.DataHandle+=this.OnOperationResponse;
		NetDataManager.DataHandlePhoton+=this.OnOperationResponse;
		ZealmConnector.createConnection(ServerAddress);

		tableRead.strInfo=StaticLoc.Loc.Get("meg0097");
    }

    /// <summary>
    /// 重置数据缓存
    /// </summary>
    private void RefreshDic()
    {
		try
		{
	        listSql.Clear();
	        dicUpdate.Clear();
	        foreach (YuanTable item in dicTempTable.Values)
	        {
	            item.IsUpdate = false;
	        }
	        dicTempTable.Clear();
			dicYT.Clear ();
			if(dicBenefitsInfo!=null)
			{
				//dicBenefitsInfo.Clear ();
			}
			if(dicGuildLevel!=null)
			{
				dicGuildLevel.Clear ();
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
    }

    public void OnReadTables()
    {

    }

    public yuan.YuanMemoryDB.YuanTable ytGameItem;
    public yuan.YuanMemoryDB.YuanTable ytGameSkill;
    public yuan.YuanMemoryDB.YuanTable ytMapLevel;
    public yuan.YuanMemoryDB.YuanTable ytNPCInfo;
    public yuan.YuanMemoryDB.YuanTable ytTask;
    public yuan.YuanMemoryDB.YuanTable ytPlayerService;
    public yuan.YuanMemoryDB.YuanTable ytObjective;
    public yuan.YuanMemoryDB.YuanTable ytStoreItem;
    public yuan.YuanMemoryDB.YuanTable ytTablePacks;
    public yuan.YuanMemoryDB.YuanTable ytNotice;
    public yuan.YuanMemoryDB.YuanTable ytPlayer;
    public yuan.YuanMemoryDB.YuanTable ytBlueprint;
    public yuan.YuanMemoryDB.YuanTable ytEverydayAim;
    public yuan.YuanMemoryDB.YuanTable ytPlayerTitle;
    public yuan.YuanMemoryDB.YuanTable ytHelp;
    public yuan.YuanMemoryDB.YuanTable ytActivity;
    public yuan.YuanMemoryDB.YuanTable ytTaskItem;
	public yuan.YuanMemoryDB.YuanTable ytPlayerPet;
	public yuan.YuanMemoryDB.YuanTable ytBattlefield;
    public yuan.YuanMemoryDB.YuanTable ytEquipmentenhance;
    public yuan.YuanMemoryDB.YuanTable ytEquipmentresolve;
    public yuan.YuanMemoryDB.YuanTable ytBosstower;
	public yuan.YuanMemoryDB.YuanTable ytPlayerSkill;

    private List<KeyValuePair<YuanTable, string>> dicYT = new List<KeyValuePair<YuanTable, string>>();
	
	public static Dictionary<string,YuanTable> dicGetYT=new Dictionary<string, YuanTable>();
    private YuanTable tempGetYT;
    public void ReadTables()
    {

        try
        {


            //dicYT.Clear();
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytGameItem, "Select * from GameItem"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytGameSkill, "Select * from GameSkill"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytMapLevel, "Select * from MapLevel"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytNPCInfo, "Select * from NPCInfo"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytTask, "Select * from Task"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytPlayerService, "Select * from PlayerService"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytObjective, "Select * from Objective"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytStoreItem, "Select * from StoreItem where ItemType=0"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytTablePacks, "Select * from Packs"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytNotice, "Select * from Notice where isStart='1'"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytBlueprint, "Select * from Blueprint"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytEverydayAim, "Select * from EverydayAim"));
            //dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytPlayer, "Select * from PlayerInfo where PlayerID=547"));


            //while (true)
            //{
            //    tableRead.strInfo = "正在检查游戏资源";
            //    if (dicBenefitsInfo!=null)
            //    {
            //        break;
            //    }
            //}
            //if (PlayerPrefs.GetString("DataVersion") == (string)dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.DataVersion])
            //{
            //    List<YuanTable> tempListYT = new List<YuanTable>();
            //    foreach (KeyValuePair<YuanTable, string> item in dicYT)
            //    {
            //        tempListYT.Add(item.Key);
            //    }
            //    for (int i = 0; i < tempListYT.Count; i++)
            //    {
            //        tempListYT[i] = yuan.YuanSerializationDataSet.SerializationDataSet.YuanDeserializeForFile<yuan.YuanMemoryDB.YuanTable>(string.Format(@"\{0}.dat", tempListYT[i].TableName));
            //    }
            //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isReadEnd = true;
            //    return;
            //}

            //foreach (KeyValuePair<YuanTable, string> item in dicYT)
            //{
            //    GetYuanTable(item.Value, "DarkSword2", item.Key);
            //}

            soc = 0;

            canGetTable = true;
            canReadTable = false;
            //timerTableRead.Enabled = true;
			isTimerTableRead=true;
			
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

   	public System.Timers.Timer timerTableRead;
	//public System.Threading.Timer timerTableRead;
	public bool isTimerTableRead=false;
	
	//private System.Timers.Timer serviceTimer = new System.Timers.Timer();
	private System.Threading.Timer serviceTimer;
	private bool isServiceTimer=false;
	
    public TableRead tableRead;
    public float soc = 0;
    private bool canGetTable = false;
    private bool canReadTable = false;
	public static string LanguageStr = "";
    public string serverVersion = string.Empty;

    private void ThreadReadTable(object sender,System.EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(serverVersion))
            {
				tableRead.strInfo =StaticLoc.Loc.Get("meg0100");
                return;
            }
			if(!isTimerTableRead)
			{
				//Debug.LogWarning ("000000000000000000000");
				return;
			}
//            if (dicBenefitsInfo == null)
//            {
//				//Debug.LogWarning ("1111111111111111111");
//				tableRead.strInfo =StaticLoc.Loc.Get("meg0100");
//                return;
//            }
            else
            {
				//Debug.LogWarning ("222222222222222222222");
             
			//	LanguageStr = (string)YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameLanguage];
                //LanguageStr = (string)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameLanguage];
                if ((new System.Version(serverVersion)) > GameVersion)
                {
					tableRead.isNeedUpdate=true;
                    tableRead.strInfo =  Get("info544");
                    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isOnlineFiled = true;
                    timerTableRead.Enabled = false;
					isTimerTableRead=false;
					timerTableRead.Dispose ();
					timerTableRead=null;
                    return;
                }
				if (/*(int)dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.ServerPlayerMax] == 1*/false)
                {
					tableRead.isPlayerMax=true;
                    tableRead.strInfo = Get("info545");
                    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isOnlineFiled = true;
					isTimerTableRead=false;
                    timerTableRead.Enabled = false;
					timerTableRead.Dispose ();
					timerTableRead=null;
                    return;
                }
                else
                {
                    if (canGetTable)
                    {
                        canGetTable = false;
//                        dicYT.Clear();
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytGameItem, "Select * from GameItem"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytGameSkill, "Select * from GameSkill"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytMapLevel, "Select * from MapLevel"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytNPCInfo, "Select * from NPCInfo"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytTask, "Select * from Task"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytPlayerService, "Select * from PlayerService"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytObjective, "Select * from Objective"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytStoreItem, "Select * from StoreItem"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytTablePacks, "Select * from Packs"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytNotice, "Select * from Notice where isStart='1'"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytBlueprint, "Select * from Blueprint"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytEverydayAim, "Select * from EverydayAim"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytPlayerTitle, "Select * from PlayerTitle"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytHelp, "Select * from GameHelp"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytActivity, "Select * from Activity"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytPlayer, "Select * from PlayerInfo where PlayerID=547"));
//                        dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytTaskItem, "Select * from TaskItem"));
//						dicYT.Add(new KeyValuePair<YuanTable, string>(GetYuanUnityPhotonInstantiate().ytPlayerPet, "Select * from PlayerPet"));
						dicGetYT.Clear ();
						dicGetYT.Add ("gameitem",ytGameItem);
						dicGetYT.Add ("gameskill",ytGameSkill);
						dicGetYT.Add ("maplevel",ytMapLevel);
						dicGetYT.Add ("npcinfo",ytNPCInfo);
						dicGetYT.Add ("task",ytTask);
						dicGetYT.Add ("playerservice",ytPlayerService);
						dicGetYT.Add ("objective",ytObjective);
						dicGetYT.Add ("storeitem",ytStoreItem);
						dicGetYT.Add ("packs",ytTablePacks);
						dicGetYT.Add ("notice",ytNotice);
						dicGetYT.Add ("blueprint",ytBlueprint);
						dicGetYT.Add ("everydayaim",ytEverydayAim);
						dicGetYT.Add ("playertitle",ytPlayerTitle);
						dicGetYT.Add ("gamehelp",ytHelp);
						dicGetYT.Add ("activity",ytActivity);
						dicGetYT.Add ("taskitem",ytTaskItem);
						dicGetYT.Add ("playerpet",ytPlayerPet);
						dicGetYT.Add ("battlefield",ytBattlefield);
                        dicGetYT.Add("equipmentenhance", ytEquipmentenhance);
                        dicGetYT.Add("equipmentresolve", ytEquipmentresolve);
                        dicGetYT.Add("bosstower", ytBosstower);
						dicGetYT.Add ("playerskill",ytPlayerSkill);
						tableRead.strInfo = Get("info547");
						tableRead.canReadTable=true;
                       // try
                       // {
                       //     string dataVersion = string.Empty;
                       //     if (System.IO.File.Exists(string.Format(@"{0}/DataVersion.dat", tableRead.applicationPath)))
                       //     {
					   //
                       //         dataVersion = yuan.YuanSerializationDataSet.SerializationDataSet.YuanDeserializeForFile<string>(string.Format(@"{0}/DataVersion.dat", tableRead.applicationPath));
                       //     }
					   //
                       //     if (dataVersion == (string)dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.DataVersion])
                       //     {
                       //         List<YuanTable> tempListYT = new List<YuanTable>();
                       //         foreach (KeyValuePair<YuanTable, string> item in dicYT)
                       //         {
                       //             tempListYT.Add(item.Key);
                       //         }
                       //         for (int i = 0; i < tempListYT.Count; i++)
                       //         {
                       //             //tempListYT[i] = yuan.YuanSerializationDataSet.SerializationDataSet.YuanDeserializeForFile<yuan.YuanMemoryDB.YuanTable>(string.Format(@"{0}.dat", tempListYT[i].TableName));
                       //             tempListYT[i].Rows = yuan.YuanSerializationDataSet.SerializationDataSet.YuanDeserializeForFile<yuan.YuanMemoryDB.YuanTable>(string.Format(@"{1}/{0}.dat", tempListYT[i].TableName, tableRead.applicationPath)).Rows;
                       //         }
                       //         YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isReadEnd = true;
                       //         timerTableRead.Enabled = false;
                       //         return;
                       //     }
                       // }
                       // catch (System.Exception ex)
                       // {
                       //     Debug.LogError(ex);
                       // }
                        foreach (KeyValuePair<YuanTable, string> item in dicYT)
                        {
                            GetYuanTable(item.Value, "DarkSword2", item.Key,YuanUnityPhoton.LanguageVersion);
                        }
                        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.maxNum = dicYT.Count;
                        canReadTable = true;


						timerTableRead.Enabled = false;
						isTimerTableRead=false;
						timerTableRead.Dispose ();
						timerTableRead=null;
                    }
                //    if (canReadTable)
                //    {
				//
                //        for (int i = 0; i < dicYT.Count; i++)
                //        {
                //            if (dicYT[i].Key.IsUpdate == false)
                //            {
                //               // try
                //               // {
                //               //     yuan.YuanSerializationDataSet.SerializationDataSet.YuanSerializeToFile<YuanTable>(dicYT[i].Key, string.Format(@"{1}/{0}.dat", dicYT[i].Key.TableName, tableRead.applicationPath));
                //               // }
                //               // catch (System.Exception ex)
                //               // {
                //               //     Debug.LogError(ex);
                //               // }
                //                dicYT.RemoveAt(i);
                //            }
                //        }
				//
                //        if (dicYT.Count == 0/*&&!tempGetYT.IsUpdate*/)
                //        {
                //           // try
                //           // {
                //           //     yuan.YuanSerializationDataSet.SerializationDataSet.YuanSerializeToFile<string>((string)dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.DataVersion], string.Format(@"{0}/DataVersion.dat", tableRead.applicationPath));
                //           // }
                //           // catch (System.Exception ex)
                //           // {
                //           //     Debug.LogError(ex.ToString());
                //           // }
                //            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isReadEnd = true;
                //            //timerTableRead.Enabled = false;
				//			isTimerTableRead=false;
                //            timerTableRead.Dispose();
				//			timerTableRead=null;
                //        }
				//
                //        //if (dicYT.Count > 0)
                //        //{
                //        //    if (tempGetYT == null || !tempGetYT.IsUpdate)
                //        //    {
                //        //        tempGetYT = dicYT[0].Key;
                //        //        GetYuanTable(dicYT[0].Value, "DarkSword2", dicYT[0].Key);
                //        //        dicYT.RemoveAt(0);
                //        //    }
                //        //}
				//
                //        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.readNum = dicYT.Count;
                //        if (!this.ServerConnected)
                //        {
                //            //timerTableRead.Enabled = false;
				//			isTimerTableRead=false;
                //            tableRead.strInfo = Get("info546");
                //            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isOnlineFiled = true;
				//			if(timerTableRead!=null)
				//			{
				//				timerTableRead.Dispose ();
				//				timerTableRead=null;
				//			}
                //            return;
                //        }
                //        if (soc >= 30)
                //        {
                //            //timerTableRead.Enabled = false;
				//			isTimerTableRead=false;
                //            tableRead.strInfo = Get("info546");
                //            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead.isOnlineFiled = true;
				//			timerTableRead.Dispose ();
				//			timerTableRead=null;
                //            return;
                //            // timerTableRead.Dispose();
                //        }
                //        soc += 1f;
                //        tableRead.strInfo = Get("info547");
                //    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    private Dictionary<string, YuanTable> dicTempTable = new Dictionary<string, YuanTable>();
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
            if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
            {
				Dictionary<short, object> parameter = new Dictionary<short, object>() { { (short)yuan.YuanPhoton.ParameterType.TableName, table.TableName },
					{(short)yuan.YuanPhoton.ParameterType.TableSql,strSql},
					{(short)yuan.YuanPhoton.ParameterType.DataBeas,DateBeas}};
                //this.fiber.Enqueue(() => this.peer.OpCustom((byte)OperationCode.YuanBD, parameter, true));
//                this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.YuanDBGet, parameter, true);
				ZMNetDataLikePhoton GetYuanTable = new ZMNetDataLikePhoton((short)OpCode.YuanDBGet, parameter);
				ZealmConnector.sendRequest(GetYuanTable);
                dicTempTable.Add(table.TableName, table);
                table.IsUpdate = true;
            }
        }
    }
	
    public void GetYuanTable(string strSql, string DateBeas, YuanTable table,string mLanguage)
    {
        if (this.serverConnected)
        {
            if (!table.IsUpdate && !dicTempTable.ContainsKey(table.TableName))
            {
				Dictionary<short, object> parameter = new Dictionary<short, object>() { { (short)yuan.YuanPhoton.ParameterType.TableName, table.TableName },
					{(short)yuan.YuanPhoton.ParameterType.TableSql,strSql},
					{(short)yuan.YuanPhoton.ParameterType.DataBeas,DateBeas}};
				parameter.Add ((short)yuan.YuanPhoton.ParameterType.LangugeVersion,mLanguage);
                //this.fiber.Enqueue(() => this.peer.OpCustom((byte)OperationCode.YuanBD, parameter, true));
//                this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.YuanDBGet, parameter, true);
				ZMNetDataLikePhoton GetYuanTable = new ZMNetDataLikePhoton((short)OpCode.YuanDBGet, parameter);
				ZealmConnector.sendRequest(GetYuanTable);
                dicTempTable.Add(table.TableName, table);
                table.IsUpdate = true;
            }
        }
    }

    private List<string> listSql = new List<string>();
	private Dictionary<short, object> dicUpdate = new Dictionary<short, object>();
    /// <summary>
    /// žüÐÂTableÊýŸÝ
    /// </summary>
    /// <param name="dateBeas">ÊýŸÝ¿âÃû</param>
    /// <param name="table">±íµÄÃû³Æ</param>
    public void UpdateYuanTable(string dateBeas, YuanTable table)
    {

        if (this.serverConnected)
        {
            if (!table.IsUpdate && table.Rows.Count > 0 && !dicTempTable.ContainsKey(table.TableName))
            {
                string strSqlFirst = string.Empty;
                string strSqlLast = string.Empty;
                string strSql = string.Empty;
                int num = 0;

                foreach (YuanRow r in table.Rows)
                {
                    switch (r.RowState)
                    {
                        case YuanRowState.Insert:
                            {

                                strSqlFirst = "Insert into " + table.TableName + "(";
                                strSqlLast = ") values('";
                                num = 0;
                                foreach (string mKey in r.GetColumnsName())
                                {
                                    num++;
                                    if (mKey != table.TableKey)
                                    {
                                        if (num == r.GetColumnsName().Length)
                                        {
                                            strSqlFirst += mKey;
                                            strSqlLast += r[mKey].YuanColumnText + "')";
                                        }
                                        else
                                        {
                                            strSqlFirst += mKey + ",";
                                            strSqlLast += r[mKey].YuanColumnText + "','";
                                        }
                                    }
                                }
                                strSql = strSqlFirst + strSqlLast;
                                listSql.Add(strSql);
                            }
                            break;
                        case YuanRowState.Update:
                            {
                                strSql = string.Format("Update {0} set ", table.TableName);
                                num = 0;
                                foreach (string mKey in r.GetColumnsName())
                                {

                                    //if (mKey == table.TableKey)
                                    //{

                                    //}
                                    //else if (num == r.GetColumnsName().Length)
                                    //{
                                    //    strSql += string.Format("{0}='{1}' where {2}='{3}'", mKey, r[mKey], table.TableKey, r[table.TableKey]);
                                    //}
                                    //else
                                    //{
                                    //    strSql += string.Format("{0}='{1}',", mKey, r[mKey]);
                                    //}
                                    if (r[mKey].YuanColumnState == YuanRowState.Update)
                                    {
                                        strSql += string.Format("{0}='{1}',", mKey, r[mKey].YuanColumnText);
                                        num++;
                                    }
                                }
                                if (num > 0)
                                {
                                    strSql = strSql.Substring(0, strSql.Length - 1);
                                    strSql += string.Format(" where {0}='{1}'", table.TableKey, r[table.TableKey].YuanColumnText);
                                    listSql.Add(strSql);
                                }
                            }
                            break;
                    }
                }

                foreach (YuanRow r in table.DeleteRows)
                {
                    strSql = string.Format("Delete {0} where {1}={2}", table.TableName, table.TableKey, r[table.TableKey].YuanColumnText);
                    listSql.Add(strSql);
                }
                dicUpdate.Clear();
                if (listSql.Count > 0)
                {
                    num = 100;
                    foreach (string sql in listSql)
                    {
						dicUpdate.Add((short)num, sql);
                        num++;
                        //Debug.Log("T-SQL:" + sql);
                    }
					dicUpdate.Add((short)ParameterType.TableName, table.TableName);
					dicUpdate.Add((short)ParameterType.DataBeas, dateBeas);
//                    this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.YuanDBUpdate, dicUpdate, true);

					ZMNetDataLikePhoton GetYuanTable = new ZMNetDataLikePhoton((short)OpCode.YuanDBUpdate, dicUpdate);
					ZealmConnector.sendRequest(GetYuanTable);


                    table.IsUpdate = true;
                    dicTempTable.Add(table.TableName, table);
                    listSql.Clear();
                    dicUpdate.Clear();

                }
            }
        }
    }

    /// <summary>
    /// 发送游戏版本号
    /// </summary>
    /// <param name="mVersion"></param>
    public void SendGameVersion()
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.GameVersion, GameVersion.ToString ());
			parameter.Add((short)yuan.YuanPhoton.ParameterType.LangugeVersion, YuanUnityPhoton.LanguageVersion);
//			this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SendGameVersion, parameter, true);
			ZMNetDataLikePhoton SendGameVersion=new ZMNetDataLikePhoton((short)OpCode.SendGameVersion,parameter);
			ZealmConnector.sendRequest(SendGameVersion);
        }
    }

    /// <summary>
    /// 详细注册通行证
    /// </summary>
    /// <param name="playerId">通行证ID</param>
    /// <param name="playerNickName">通行证昵称</param>
    /// <param name="playerPwd">通行证密码</param>
    /// <param name="playerEmail">邮箱</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void PlayerLogon(string playerId, string playerNickName, string playerPwd, string playerEmail, string license, string logonPlayerID, string dateBase, string table)
    {
        if (this.serverConnected)
        {

			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId.Trim());
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNickName, playerNickName.Trim());
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, yuan.YuanMd5.yuanMd5.GetMd5(playerPwd.Trim()));
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserEmail, playerEmail.Trim());
			parameter.Add((short)yuan.YuanPhoton.ParameterType.License, license);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.LogonPlayerID, logonPlayerID);


			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((int)OpCode.Logon,parameter);
			ZealmConnector.sendRequest(nd);
			
			
			this.isLogin = true;

        }
    }

    /// <summary>
    /// 快速注册
    /// </summary>
    /// <param name="deviceID">设备号</param>
    /// <param name="playerName">角色名称</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void PlayerFastLogon(string deviceID, string proID, string serverName, string playerName, string dateBase, string table)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, deviceID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.PlayerType, proID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNickName, playerName);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, serverName);
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.FastLogon, parameter, true);

			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.FastLogon,parameter);
			ZealmConnector.sendRequest(nd);

            this.isLogin = true;
        }
    }

    /// <summary>
    /// 玩家快速登陆
    /// </summary>
    /// <param name="deviceID">设备号码</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void PlayerFastLogin(string deviceID, string dateBase, string table)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, deviceID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
//            this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.FastLogin, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.FastLogin,parameter);
			ZealmConnector.sendRequest(nd);
            this.isLogin = true;
        }
    }
	
	/// <summary>
	/// 设置邀请码
	/// </summary>
	/// <param name='parameter'>
	/// Parameter.
	/// </param>
	public void SetLicense(Dictionary<short, object> parameter)
	{
		if (this.serverConnected)
        {
//			this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.SetLicense, parameter, true);
			ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.SetLicense,parameter);
			ZealmConnector.sendRequest(send);
        }
	}
	public void PlayerLoginAS(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "itools");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.I4,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogini4__tryCatch_01__" + ex.ToString());
		}
	}

	public void PlayerLoginPP(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "pp");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.PP,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogini4__tryCatch_01__" + ex.ToString());
		}
	}
	public void PlayerLoginTB(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "tbt");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.TB,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogintbt__tryCatch_01__" + ex.ToString());
		}
	}

	public void PlayerLoginHM(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "hm");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.HM,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogintbt__tryCatch_01__" + ex.ToString());
		}
	}

	/// <summary>
	/// Players the login CMG.
	/// ios 正版
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="playerPwd">Player pwd.</param>
	/// <param name="isGetIP">If set to <c>true</c> is get I.</param>
	public void PlayerLoginCMGE(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "zsyios");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.iosZsy,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogincmge__tryCatch_01__" + ex.ToString());
		}
	}

	public void PlayerLoginZSYIos(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "cmgeios");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.LoginZSY,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogincmge__tryCatch_01__" + ex.ToString());
		}
	}
	/// <summary>
	/// Players the login X.
	/// XY login
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="playerPwd">Player pwd.</param>
	/// <param name="isGetIP">If set to <c>true</c> is get I.</param>
	public void PlayerLoginXY(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "XY");
				//				parameter.Add((short)yuan.YuanPhoton.ParameterType.PhoneType, "XY");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.XY,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLoginXY__tryCatch_01__" + ex.ToString());
		}
	}
	/// <summary>
	/// Players the login itools.
	/// itools 登陆
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="playerPwd">Player pwd.</param>
	/// <param name="isGetIP">If set to <c>true</c> is get I.</param>
	public void PlayerLoginItools(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "itools");
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.iTools,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLoginItools__tryCatch_01__" + ex.ToString());
		}
	}
	/// <summary>
	/// Players the login KYSD.
	/// 快用登录
	/// </summary>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="playerPwd">Player pwd.</param>
	/// <param name="isGetIP">If set to <c>true</c> is get I.</param>
	public void PlayerLoginKYSDK(string playerId, string playerPwd, bool isGetIP)
	{
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, playerPwd);
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				if (isGetIP)
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.KY,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLoginkysdk__tryCatch_01__" + ex.ToString());
		}
	}
    /// <summary>
    /// 玩家登陆91
    /// </summary>
    /// <param name="playerId">玩家id</param>
    /// <param name="playerPwd">玩家密码</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void PlayerLogin91(string playerId, string playerPwd, bool isGetIP)
    {
		try{
			if (this.serverConnected)
			{
				Dictionary<short, object> parameter = new Dictionary<short, object>();
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
				
				parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "91");
				
				parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, SystemInfo.deviceUniqueIdentifier);
				
				
				if (isGetIP)
				{
					
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
					userID = playerId;
				}
				else
				{
					parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
				}
				//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Login91, parameter, true);
				ZMNetDataLikePhoton send = new ZMNetDataLikePhoton((short)OpCode.Login91,parameter);
				ZealmConnector.sendRequest(send);
				this.isLogin = true;
			}
		}catch(System.Exception ex){
			Debug.Log("PlayerLogin91__tryCatch_01__" + ex.ToString());
		}
        
    }
	
	/// <summary>
	/// 玩家登陆UC
	/// </summary>
	/// <param name='sid'>
	/// Sid.
	/// </param>
	/// <param name='isGetIP'>
	/// Is get I.
	/// </param>
    public void PlayerLoginUC(string sid, bool isGetIP)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, sid);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "uc");

            if (isGetIP)
            {

                parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
		//	this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LoginUC, parameter, true);
			ZMNetDataLikePhoton fuck = new ZMNetDataLikePhoton((short)OpCode.LoginUC,parameter);
			ZealmConnector.sendRequest(fuck);
            this.isLogin = true;
        }
    }
	
	/// <summary>
	/// 玩家登陆DL
	/// </summary>
	/// <param name='sid'>
	/// Sid.
	/// </param>
	/// <param name='dateBase'>
	/// 数据库
	/// </param>
	/// <param name='table'>
	/// 表
	/// </param>
	/// <param name='isGetIP'>
	/// Is get I.
	/// </param>
    public void PlayerLoginDL(string mid,string mToken , bool isGetIP)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mid);

//			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
//			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, mToken);

            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
			ZMNetDataLikePhoton fuck = new ZMNetDataLikePhoton((short)OpCode.LoginDL,parameter);
			ZealmConnector.sendRequest(fuck);
            this.isLogin = true;
        }
    }

    /// <summary>
    /// 玩家登陆小米
    /// </summary>
    /// <param name='sid'>
    /// Sid.
    /// </param>
    /// <param name='dateBase'>
    /// 数据库
    /// </param>
    /// <param name='table'>
    /// 表
    /// </param>
    /// <param name='isGetIP'>
    /// Is get I.
    /// </param>
    public void PlayerLoginMI(string mid, string mToken,  bool isGetIP)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mid);

			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, mToken);
            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.LoginMI, parameter, true);
			ZMNetDataLikePhoton PlayerLoginMI = new ZMNetDataLikePhoton((short)OpCode.LoginMI,parameter);
			ZealmConnector.sendRequest(PlayerLoginMI);
            this.isLogin = true;
        }
    }

    /// <summary>
    /// 玩家登陆360
    /// </summary>
    /// <param name='sid'>
    /// Sid.
    /// </param>
    /// <param name='dateBase'>
    /// 数据库
    /// </param>
    /// <param name='table'>
    /// 表
    /// </param>
    /// <param name='isGetIP'>
    /// Is get I.
    /// </param>
    public void PlayerLoginTSZ(string mid, string mToken, bool isGetIP)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mid);

			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, mToken);

            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.LoginTSZ, parameter, true);
			ZMNetDataLikePhoton PlayerLoginTSZ = new ZMNetDataLikePhoton((short)OpCode.LoginTSZ,parameter);
			ZealmConnector.sendRequest(PlayerLoginTSZ);
            this.isLogin = true;
        }
    }

    /// <summary>
    /// 玩家登陆中手游
    /// </summary>
    /// <param name='sid'>
    /// Sid.
    /// </param>
    /// <param name='dateBase'>
    /// 数据库
    /// </param>
    /// <param name='table'>
    /// 表
    /// </param>
    /// <param name='isGetIP'>
    /// Is get I.
    /// </param>
    public void PlayerLoginZSY(string mid, bool isGetIP)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mid);

			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "360");

            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.LoginZSY, parameter, true);
			ZMNetDataLikePhoton PlayerLoginZSY = new ZMNetDataLikePhoton((short)OpCode.LoginZSY,parameter);
			ZealmConnector.sendRequest(PlayerLoginZSY);
            this.isLogin = true;
        }
    }	
	
	/// <summary>
	/// 登陆联运sdk,客户端把authcode传到服务器端，然后服务器端携带authcode去请求token，mid=authcode，mSdk为各渠道的识别参数由TableRead.cs脚本传过来。
	/// </summary>
	/// <param name='mid'>
	/// Middle.
	/// </param>
	/// <param name='mSDK'>
	/// M SD.
	/// </param>
	/// <param name='dateBase'>
	/// Date base.
	/// </param>
	/// <param name='table'>
	/// Table.
	/// </param>
	/// <param name='isGetIP'>
	/// Is get I.
	/// </param>
    public void PlayerLoginLianYun(string mid,string mSDK, bool isGetIP)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mid);

			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "123");
			parameter.Add((short)yuan.YuanPhoton.ParameterType.SDK, mSDK);
            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
//            this.peer.OpCustom(OpCode.LoginZSYAll, parameter, true);
			ZMNetDataLikePhoton PlayerLoginLianYun = new ZMNetDataLikePhoton((short)OpCode.LoginZSYAll, parameter);
			ZealmConnector.sendRequest(PlayerLoginLianYun);
            this.isLogin = true;
        }
    }
	
	/// <summary>
	/// 登录OPPO
	/// </summary>
	/// <param name='mid'>
	/// Middle.
	/// </param>
	/// <param name='dateBase'>
	/// Date base.
	/// </param>
	/// <param name='table'>
	/// Table.
	/// </param>
	/// <param name='isGetIP'>
	/// Is get I.
	/// </param>
	 public void PlayerLoginOPPO(string mid, string mToken, bool isGetIP)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, mid);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, mToken);

            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.LoginOPPO, parameter, true);
			ZMNetDataLikePhoton PlayerLoginOPPO = new ZMNetDataLikePhoton((short)OpCode.LoginOPPO, parameter);
			ZealmConnector.sendRequest(PlayerLoginOPPO);
            this.isLogin = true;
        }
    }	
	/// <summary>
	/// 玩家登陆联想
	/// </summary>
	/// <param name='sid'>
	/// Sid.
	/// </param>
	/// <param name='isGetIP'>
	/// Is get I.
	/// </param>
	public void PlayerLoginLenovo(string sid, bool isGetIP)
	{
		if (this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, sid);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, "lenovo");
			
			if (isGetIP)
			{
				
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
			}
			else
			{
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
			}
			//	this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.LoginUC, parameter, true);
			ZMNetDataLikePhoton Lenovo = new ZMNetDataLikePhoton((short)OpCode.LOGIN_LENOVO,parameter);
			ZealmConnector.sendRequest(Lenovo);
			this.isLogin = true;
		}
	}
    /// <summary>
    /// 玩家登陆
    /// </summary>
    /// <param name="playerId">玩家id</param>
    /// <param name="playerPwd">玩家密码</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void PlayerLogin(string playerId, string playerPwd, string dateBase, string table, bool isGetIP)
    {
		PhotonHandler.ShowLog(string.Format("playerid:{0}",playerId));
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, playerId);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, yuan.YuanMd5.yuanMd5.GetMd5 (playerPwd));
            if (isGetIP)
            {

				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "1");
                userID = playerId;
            }
            else
            {
				parameter.Add((short)yuan.YuanPhoton.ParameterType.IsGetIP, "0");
            }
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.Login,parameter);
			ZealmConnector.sendRequest(nd);
            this.isLogin = true;
        }
    }

    /// <summary>
    ///  统计不让注册的ios设备
    /// </summary>
    /// <param name="phoneType">设备类型</param>
    public void DeviceType(string phoneID, string phoneType)
    {
        if (this.serverConnected)
        {
            Dictionary<short, object> parameter = new Dictionary<short, object>();
            parameter.Add((short)ParameterType.DeviceID, phoneID);
            parameter.Add((short)ParameterType.ActivityType, phoneType);
            parameter.Add((short)ParameterType.FlagID, TableRead.strPageName);

			//TD_info.deviceReject(phoneType, phoneID, TableRead.strPageName);
            ZMNetDataLikePhoton nd = new ZMNetDataLikePhoton((short)OpCode.IphoneType, parameter);
            ZealmConnector.sendRequest(nd);
        }
    }

    /// <summary>
    /// 新建角色
    /// </summary>
    /// <param name="passID">通行证ID</param>
    /// <param name="proID">职业ID</param>
    /// <param name="nickName">角色名称</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void PlayerCreat(string passID, string proID, string nickName, string dateBase, string table)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, passID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.PlayerType, proID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserNickName, nickName);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, PlayerPrefs.GetString("InAppServer"));
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.PlayerCreat, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.PlayerCreat,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }

    /// <summary>
    /// 申请找回密码
    /// </summary>
    /// <param name="userID">通行证ID</param>
    /// <param name="userEmail">邮箱</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void GetPwd(string userID, string userEmail)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, userID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserEmail, userEmail);
//			this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.PwdGet, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.PwdGet,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="userID">通行证ID</param>
    /// <param name="userPwd">原密码</param>
    /// <param name="userPwdNew">新密码</param>
    /// <param name="dateBase">数据库</param>
    /// <param name="table">表</param>
    public void UpdatePwd(string userEmail, string userPwd, string userPwdNew)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserEmail, userEmail);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwd, yuan.YuanMd5.yuanMd5.GetMd5(userPwd));
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserPwdNew, yuan.YuanMd5.yuanMd5.GetMd5(userPwdNew));
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.PwdUpdate, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.PwdUpdate,parameter);
			ZealmConnector.sendRequest(nd);
        }
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
            // parameter.Add((byte)yuan.YuanPhoton.ParameterType.ServerName, appServer);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DeviceID, deviceID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
//			this.peer.OpCustom((short)yuan.YuanPhoton.OperationCode.BindUserID, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.BindUserID,parameter);
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
        if (this.serverConnected)
        {
			Debug.Log ("---------------------------GetPlayers");
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.UserID, userID);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.ServerName, serverName);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.DataBeas, dateBase);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.TableName, table);
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.GetPlayers, parameter, true);
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
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.DeletePlayer, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.DeletePlayer,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }
	
	/// <summary>
	/// 上传错误报告
	/// </summary>
	/// <param name='mPlatform'>
	/// M platform.
	/// </param>
	/// <param name='mBundleIdentifier'>
	/// M bundle identifier.
	/// </param>
	/// <param name='mBundleVersion'>
	/// M bundle version.
	/// </param>
	/// <param name='mErrorInfo'>
	/// M error info.
	/// </param>
	public void SendError(string mPlatform,string mBundleIdentifier,string mBundleVersion,string mErrorInfo)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)yuan.YuanPhoton.ParameterType.RuntimePlatform, mPlatform);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.GameVersion, YuanUnityPhoton.GameVersion.ToString ());
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BundleIdentifier, mBundleIdentifier);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.BundleVersion, mBundleVersion);
			parameter.Add((short)yuan.YuanPhoton.ParameterType.ErrorInfo, mErrorInfo);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.SendError,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }
	
	    /// <summary>
    /// 设置玩家行为记录
    /// </summary>
    /// <param name="mType"></param>
    /// <param name="mValue"></param>
    public void SetPlayerBehavior(ConsumptionType mType, string mValue,string mDeviceID)
    {
        if (this.serverConnected)
        {
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter.Add((short)ParameterType.PlayerBehaviorType, mType);
			parameter.Add((short)ParameterType.PlayerBehaviorValue, mValue);
			parameter.Add((short)ParameterType.DeviceID, mDeviceID);
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.SetPlayerBehavior, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.SetPlayerBehavior,parameter);
			ZealmConnector.sendRequest(nd);
        }
    }
	
	/// <summary>
	/// 登录验证
	/// </summary>
	/// <param name='mKeyStore'>
	/// M key store.
	/// </param>
	public void LoginValidation(string mKeyStore)
	{
		if(this.serverConnected)
		{
			Dictionary<short, object> parameter = new Dictionary<short, object>();
			parameter[(short)yuan.YuanPhoton.ValidationParams.ValidationType]=yuan.YuanPhoton.ValidationType.Login;
			parameter[(short)yuan.YuanPhoton.ValidationParams.KeyStore]=mKeyStore;
//            this.peer.OpCustom((byte)yuan.YuanPhoton.OperationCode.Validation, parameter, true);
			ZMNetDataLikePhoton nd=new ZMNetDataLikePhoton((short)OpCode.Validation,parameter);
			ZealmConnector.sendRequest(nd);
		}			
	}

    #region IPhotonPeerListener 成员

    public void DebugReturn(DebugLevel level, string message)
    {
        //        //Debug.Log(message);
    }

    public void OnEvent(EventData eventData)
    {
    }


	public void OnOperationResponse(ZMNetData mData)
	{
		
	}

	public void OnOperationResponse(Zealm.OperationResponse operationResponse)
	{
		try
		{
			switch((OpCode)operationResponse.OperationCode)
			{
				case OpCode.Login:
				{
					PlayerLoginGet(operationResponse);
				}
				break;
				case OpCode.SendBenefitsInfo:
				{

                    if(DeviceAdapter.DetectDevice())
                    {
                        DeviceAdapter.deviceTypeFit = true;
                        return;
                    }

                    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().serverVersion = (string)operationResponse.Parameters[(short)ParameterType.GameVersion];
				    MainMenuManage.firstServer=(string)operationResponse.Parameters[(short)ParameterType.ServerIp];
//                    Debug.Log("---------------------------VerSion----------------------" + YuanUnityPhoton.GetYuanUnityPhotonInstantiate().serverVersion);
                    //Dictionary<short,object> getParms=new Dictionary<short, object>();
                    //foreach(KeyValuePair<short,object> item in operationResponse.Parameters)
                    //{
                    //getParms[(short)item.Key]=item.Value;
                    //}

                    //dicBenefitsInfo = getParms;
                    //MMManage.isGetServerInfo = true;					
				}
				break;
				case OpCode.Logon:
				{
					ZealmLogon(operationResponse);

				}
				break;


				case OpCode.Activity:
				{
					foreach(KeyValuePair<short,object> item in operationResponse.Parameters)
					{

					//	Debug.LogError ("yyyyyyyyyyyyyyyyyyyyyyyyyyyyy:"+item.Key+"ffffffffff"+item.Value);
					}
	                Debug.Log(operationResponse.Parameters.Count);
				}
				break;
			case OpCode.LoginUC: //uc 登陆
			{
				Debug.LogError("login uc" + operationResponse.ReturnCode);
				switch (operationResponse.ReturnCode)
				{
					
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					Debug.LogError("login uc yes" + MainMenuManage.dicLogin.Count);
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;		
					Debug.LogError("login uc HasID" + id);
				}
					break;	
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				}
			}
				break;
			case OpCode.SetLicense:
			{
				switch (operationResponse.ReturnCode)
				{
					
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
  		            MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugSetLicense = operationResponse.DebugMessage;
					MMManage.isSetLicense = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				}				
			}
				break;
			case OpCode.XY:
			{
				switch (operationResponse.ReturnCode)
				{
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;					
				}
					break;
				}
			}
				break;
			case OpCode.I4:
			{
				switch (operationResponse.ReturnCode)
				{
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;					
				}
					break;
				}
			}
				break;
			case OpCode.KY:
			{
				switch (operationResponse.ReturnCode)
				{
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;					
				}
					break;
				}
			}
				break;
			case OpCode.iTools:
			{
				switch (operationResponse.ReturnCode)
				{
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;					
				}
					break;
				}
			}
				break;
			case OpCode.Login91:
			{
				switch (operationResponse.ReturnCode)
				{
					
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
                	MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
            	    MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;					
				}
					break;
				}
			}
				break;
			case OpCode.LoginMI:
			case OpCode.LoginTSZ:
			case OpCode.LoginZSY:
			case OpCode.iosZsy:
			case OpCode.HM:
			case OpCode.TB:
			case OpCode.PP:
			case OpCode.LoginOPPO:
			case OpCode.LoginZSYAll:
			case OpCode.LOGIN_LENOVO:
			case OpCode.LoginDL:
			{
				switch (operationResponse.ReturnCode)
				{
					
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.GetServer:
				{
					this.isLogin = false;
					this.loginStatus = true;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
					
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.No:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.Nothing:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.HasID:
				{
					this.isLogin = false;
					string id=(string)operationResponse.Parameters[(short)ParameterType.UserID];
					BtnManager.passID= id;
					this.userID=id;
					MMManage.idUC=id;
					MMManage.isLoginUC = true;					
				}
					break;	
				case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
				{
					this.isLogin = false;
					MainMenuManage.dicLogin = operationResponse.Parameters;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				case (short)yuan.YuanPhoton.ReturnCode.ZSYBack:
				{
					string itemID = (string)operationResponse.Parameters[(short)yuan.YuanPhoton.ParameterType.ItemID];
					MMManage.strZSYBack=itemID;
					MMManage.isZSYBack = true;
				}
					break;					
				case (short)yuan.YuanPhoton.ReturnCode.Error:
				{
					this.isLogin = false;
					MMManage.DebugLogin = operationResponse.DebugMessage;
					MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
					MMManage.isLogin = true;
				}
					break;
				}
			}
				break;	
			case OpCode.FastLogin:
			{
				MMManage.dicFastLogin = operationResponse.Parameters;
				MMManage.returnFastLogin = (ReturnCode)operationResponse.ReturnCode;
				MMManage.DebugFastLogin = operationResponse.DebugMessage;
				MMManage.isFastLogin = true;
			}
				break;
			case OpCode.FastLogon:
			{
				MMManage.returnFastLogon = (ReturnCode)operationResponse.ReturnCode;
				MMManage.DebugFastLogon = operationResponse.DebugMessage;
				MMManage.isFastLogon = true;
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
			case OpCode.PwdGet:
			{
				MMManage.returnGetPwd = (ReturnCode)operationResponse.ReturnCode;
				MMManage.DebugGetPwd = operationResponse.DebugMessage;
				MMManage.isGetPwd = true;
			}
				break;
			case OpCode.PwdUpdate:
			{
				MMManage.returnPwdUpdate = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.DebugPwdUpdate = operationResponse.DebugMessage;
				MMManage.isPwdUpdate = true;
			}
				break;
			case OpCode.BindUserID:
			{
				try
				{
					MMManage.returnBindID = (ReturnCode)operationResponse.ReturnCode;
					MMManage.DebugBindID = operationResponse.DebugMessage;
					MMManage.isBindID = true;
				}
				catch(System.Exception ex)
				{
					Debug.Log (ex.ToString ());
				}
			}
				break;
				
			case OpCode.GetPlayers:
			{
				switch (operationResponse.ReturnCode)
				{
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
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
					
				}
			}
				break;
			case OpCode.DeletePlayer:
			{
				MMManage.returnDeletePlayer = (ReturnCode)operationResponse.ReturnCode;
				MMManage.DebugDeletePlayer = operationResponse.DebugMessage;
				//MMManage.isFast = (bool)operationResponse.Parameters[(short)ParameterType.PlayerType];
				MMManage.isDeletePlayer = true;
			}
				break;
//			case OpCode.SendBenefitsInfo:
//			{
//				switch (operationResponse.ReturnCode)
//				{
//				case (short)yuan.YuanPhoton.ReturnCode.Yes:
//				{
//					dicBenefitsInfo = operationResponse.Parameters;
//					Dictionary<short, object> dicTemp = (Dictionary<short, object>)dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GuildLevelUp];
//					string[] strTemp;
//					foreach (object item in dicTemp.Values)
//					{
//						strTemp = ((string)item).Split(';');
//						Dictionary<yuan.YuanPhoton.GuildLevelUp, int> dicGuild = new Dictionary<GuildLevelUp, int>();
//						dicGuild.Add(yuan.YuanPhoton.GuildLevelUp.Build, int.Parse(strTemp[1]));
//						dicGuild.Add(yuan.YuanPhoton.GuildLevelUp.Fund, int.Parse(strTemp[2]));
//						dicGuildLevel.Add(int.Parse(strTemp[0]), dicGuild);
//					}
//					MMManage.isGetServerInfo = true;
//				}
//					break;
//					
//				}
//			}
//				break;
			case OpCode.OtherLogin:
			{
				switch (operationResponse.ReturnCode)
				{
				case (short)yuan.YuanPhoton.ReturnCode.Yes:
				{
					MMManage.isOtherPlayerLogin = true;
				}
					break;
					
				}
			}
				break;
			case OpCode.Validation:
			{
				AnlaysValidation (operationResponse);
			}
				break;						
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
	}
	
	private void PlayerLoginGet(Zealm.OperationResponse operationResponse)
	{
		switch(operationResponse.ReturnCode)
		{
		case (short)yuan.YuanPhoton.ReturnCode.Yes:
		{
			this.isLogin = false;
			this.loginStatus = true;
			MainMenuManage.dicLogin = operationResponse.Parameters;
			MMManage.DebugLogin = operationResponse.DebugMessage;
			MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
			MMManage.isLogin = true;
		}
			break;
		case (short)yuan.YuanPhoton.ReturnCode.GetServer:
		{
			this.isLogin = false;
			this.loginStatus = true;
			MainMenuManage.dicLogin = operationResponse.Parameters;
			MMManage.DebugLogin = operationResponse.DebugMessage;
			MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
			MMManage.isLogin = true;
			
		}
			break;
		case (short)yuan.YuanPhoton.ReturnCode.No:
		{
			this.isLogin = false;
			MMManage.DebugLogin = operationResponse.DebugMessage;
			MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
			MMManage.isLogin = true;
		}
			break;
		case (short)yuan.YuanPhoton.ReturnCode.Nothing:
		{
			this.isLogin = false;
			MMManage.DebugLogin = operationResponse.DebugMessage;
			MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
			MMManage.isLogin = true;
		}
			break;
			case (short)yuan.YuanPhoton.ReturnCode.Error:
			{
				this.isLogin = false;
				MMManage.DebugLogin = operationResponse.DebugMessage;
				MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogin = true;
			}
			break;
		}
	}

	private void ZealmLogon(Zealm.OperationResponse operationResponse)
	{
		switch (operationResponse.ReturnCode)
		{
			case (short)yuan.YuanPhoton.ReturnCode.Yes:
			{
				this.isLogin = false;
				MMManage.DebugLogon = operationResponse.DebugMessage;
				MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogon = true;
			}
			break;
			case (short)yuan.YuanPhoton.ReturnCode.No:
			{
				this.isLogin = false;
				MMManage.DebugLogon = operationResponse.DebugMessage;
				MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogon = true;
			}
			break;
			case (short)yuan.YuanPhoton.ReturnCode.HasID:
			{
				this.isLogin = false;
				MMManage.DebugLogon = operationResponse.DebugMessage;
				MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogon = true;
			}
			break;
			case (short)yuan.YuanPhoton.ReturnCode.HasNickName:
			{
				this.isLogin = false;
				MMManage.DebugLogon = operationResponse.DebugMessage;
				MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogon = true;
			}
			break;
			case (short)yuan.YuanPhoton.ReturnCode.HasEmail:
			{
				this.isLogin = false;
				MMManage.DebugLogon = operationResponse.DebugMessage;
				MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogon = true;
			}
			break;
			case (short)yuan.YuanPhoton.ReturnCode.Error:
			{
				this.isLogin = false;
				MMManage.DebugLogon = operationResponse.DebugMessage;
				MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
				MMManage.isLogon = true;
			}
			break;
		}
	}
	
	
	public GameObject obj;
    public virtual void OnOperationResponse(OperationResponse operationResponse)
    {
		try
		{
        switch (operationResponse.OperationCode)
        {
            case (byte)yuan.YuanPhoton.OperationCode.SendError:
                {

                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Error:
                            {

								Debug.LogWarning (operationResponse.DebugMessage);
                            }
                            break;
                    }
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.YuanDBGet:
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
                        case (short)ReturnCode.No:
                            {
                                string tableName = (string)operationResponse.Parameters[(byte)ErrorParameterCode.TableName];
                                string errorText = (string)operationResponse.Parameters[(byte)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                                Debug.LogError("ÊýŸÝ¿â²éÑ¯ŽíÎó,DB Error!\n" + errorText);
                            }
                            break;
                    }
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.YuanDBUpdate:
                {
                    string tableName = (string)operationResponse.Parameters[(byte)ErrorParameterCode.TableName];
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)ReturnCode.Yes:
                            {
                                dicTempTable[tableName].Refresh();
                                dicTempTable[tableName].DeleteRows.Clear();
                                dicTempTable[tableName].IsUpdate = false;
                                dicTempTable.Remove(tableName);
                            }
                            break;
                        case (short)ReturnCode.No:
                            {
                                string errorText = (string)operationResponse.Parameters[(byte)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
                                Debug.LogError("ÊýŸÝ¿âžüÐÂŽíÎó!DB Error Update£¡" + errorText);
                            }
                            break;
                        case (short)ReturnCode.Nothing:
                            {
                                string errorText = (string)operationResponse.Parameters[(byte)ErrorParameterCode.ErrorText];
                                dicTempTable[tableName].IsUpdate = false;
                                Debug.LogWarning("ÊýŸÝ¿âÎÞ±äžü!" + errorText);
                            }
                            break;
                    }

                }
                break;
		    case (byte)yuan.YuanPhoton.OperationCode.SetLicense:
			{
                    switch (operationResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                this.isLogin = false;
                                MMManage.DebugSetLicense = operationResponse.DebugMessage;
                                MMManage.isSetLicense = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                    }				
			}
			break;
            case (byte)yuan.YuanPhoton.OperationCode.Login91:
                {
                    switch (operationResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.GetServer:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
					   case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
                            {
                                this.isLogin = false;
//								MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                    }
                }
                break;
			case (byte)yuan.YuanPhoton.OperationCode.LoginUC: //uc 登陆
                {
                    switch (operationResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.GetServer:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                this.isLogin = false;
								string id=(string)operationResponse.Parameters[(byte)ParameterType.UserID];
								BtnManager.passID= id;
								this.userID=id;
								MMManage.idUC=id;
                                MMManage.isLoginUC = true;					
                            }
                            break;	
					    case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
                            {
                                this.isLogin = false;
//								MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                    }
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.LoginMI:
            case (byte)yuan.YuanPhoton.OperationCode.LoginTSZ:
			case (byte)yuan.YuanPhoton.OperationCode.LoginZSY:
			case (byte)yuan.YuanPhoton.OperationCode.LoginOPPO:
			case (byte)yuan.YuanPhoton.OperationCode.LoginZSYAll:
			case (byte)yuan.YuanPhoton.OperationCode.LoginDL:
                {
                    switch (operationResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.GetServer:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                this.isLogin = false;
								string id=(string)operationResponse.Parameters[(byte)ParameterType.UserID];
								BtnManager.passID= id;
								this.userID=id;
								MMManage.idUC=id;
                                MMManage.isLoginUC = true;					
                            }
                            break;	
					    case (short)yuan.YuanPhoton.ReturnCode.NeedLicense:
                            {
                                this.isLogin = false;
//								MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
					    case (short)yuan.YuanPhoton.ReturnCode.ZSYBack:
                            {
                                string itemID = (string)operationResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.ItemID];
								MMManage.strZSYBack=itemID;
                                MMManage.isZSYBack = true;
                            }
                            break;					
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                    }
                }
                break;				
            case (byte)yuan.YuanPhoton.OperationCode.Login:
                {
                    switch (operationResponse.ReturnCode)
                    {

                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                                //MMManage.SwitchListOnlyOne(MMManage.listMenu, 8, true, true);



                                //YuanTable yt = new YuanTable("PlayerInfo", "PlayerID");
                                //yt.CopyToDictionary(operationResponse.Parameters);
                                //MMManage.SwitchListOnlyOne(listMenu,2, true, true);
                                //foreach (UILabel lbl in listPlayerBtn)
                                //{
                                //    lbl.transform.parent.gameObject.SetActiveRecursively(false);
                                //}
                                //btnSelectPlayerBind.gameObject.SetActiveRecursively(false);
                                //int i = 0;
                                //obj.SendMessage("GetYT",yt,SendMessageOptions.DontRequireReceiver);
                                //foreach (YuanRow yr in yt.Rows)
                                //{
                                //    if (listPlayerBtn.Count>i)
                                //    {
                                //        listPlayerBtn[i].text = yr["PlayerName"].YuanColumnText.Trim();

                                //        listPlayerBtn[i].transform.parent.gameObject.SetActiveRecursively(true);

                                //    }
                                //    i++;
                                //}
                                //if (i >= 6)
                                //{
                                //    btnCreatPlayer.gameObject.SetActiveRecursively(false);
                                //}

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.GetServer:
                            {
                                this.isLogin = false;
                                this.loginStatus = true;
//                                MainMenuManage.dicLogin = operationResponse.Parameters;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;

                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogin = operationResponse.DebugMessage;
                                MainMenuManage.returnLogin = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogin = true;
                            }
                            break;
                    }
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.Logon:
                {

                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogon = operationResponse.DebugMessage;
                                MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogon = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.No:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogon = operationResponse.DebugMessage;
                                MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogon = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasID:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogon = operationResponse.DebugMessage;
                                MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogon = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasNickName:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogon = operationResponse.DebugMessage;
                                MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogon = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.HasEmail:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogon = operationResponse.DebugMessage;
                                MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogon = true;
                            }
                            break;
                        case (short)yuan.YuanPhoton.ReturnCode.Error:
                            {
                                this.isLogin = false;
                                MMManage.DebugLogon = operationResponse.DebugMessage;
                                MMManage.returnLogon = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                                MMManage.isLogon = true;
                            }
                            break;
                    }
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.FastLogin:
                {
//                    MMManage.dicFastLogin = operationResponse.Parameters;
                    MMManage.returnFastLogin = (ReturnCode)operationResponse.ReturnCode;
                    MMManage.DebugFastLogin = operationResponse.DebugMessage;
                    MMManage.isFastLogin = true;
                    //switch (operationResponse.ReturnCode)
                    //{
                    //    case (short)yuan.YuanPhoton.ReturnCode.Yes:
                    //        {
                    //            YuanTable yt = new YuanTable("PlayerInfo", "PlayerID");
                    //            yt.CopyToDictionary(operationResponse.Parameters);
                    //            MMManage.SwitchListOnlyOne(MMManage.listMenu, 2, true, true);
                    //            foreach (BtnPlayer lbl in MMManage.listPlayerBtn)
                    //            {
                    //                MMManage.YuanSetActiveRecursively(lbl.transform.parent.gameObject,true);
                    //            }
                    //            //MMManage.listPlayerBtn[4].text = yt.Rows[0]["PlayerName"].YuanColumnText.Trim();
                    //            MMManage.YuanSetActiveRecursively(MMManage.listPlayerBtn[4].transform.parent.gameObject,true);
                    //            MMManage.YuanSetActiveRecursively(MMManage.btnCreatPlayer.gameObject,false);
                    //            MMManage.YuanSetActiveRecursively(MMManage.btnSelectPlayerPwd, false);
                    //            MMManage.btnSelectPlayerBack.invokMethodName = "Back";
                    //            MMManage.YuanSetActiveRecursively(MMManage.btnSelectPlayerBind.gameObject,true);
                    //            MMManage.btnSelectPlayerBind.invokMethodName = "BindUserIDMenu";

                    //            this.fastLoginStatus = yuan.YuanPhoton.ReturnCode.Yes;
                    //            this.isLogin = false;
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.No:
                    //        {
                    //            //string deviceID = operationResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.DeviceID].ToString();
                    //            MMManage.SwitchListOnlyOne(MMManage.listMenu, 4, true, true);
                    //            MMManage.btnCreatPlayerBack.invokMethodName = "Back";
                    //            MMManage.btnCreatPlayerEnter.invokMethodName = "PlayerFastLogon";
                    //            this.fastLoginStatus = yuan.YuanPhoton.ReturnCode.No;
                    //            this.isLogin = false;
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.HasRegister:
                    //        {
                    //            this.fastLoginStatus = yuan.YuanPhoton.ReturnCode.HasRegister;
                    //            this.isLogin = false;
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.Error:
                    //        {
                    //            this.isLogin = false;
                    //            Debug.LogError(operationResponse.DebugMessage);
                    //        }
                    //        break;
                    //}
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.FastLogon:
                {
                    MMManage.returnFastLogon = (ReturnCode)operationResponse.ReturnCode;
                    MMManage.DebugFastLogon = operationResponse.DebugMessage;
                    MMManage.isFastLogon = true;
                    //switch (operationResponse.ReturnCode)
                    //{
                    //    case (short)yuan.YuanPhoton.ReturnCode.Yes:
                    //        {
                    //            this.isLogin = false;
                    //            if (MMManage.lblCreatWarning != null)
                    //            {
                    //                MMManage.lblCreatWarning.text = operationResponse.DebugMessage;
                    //            }
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.HasDevice:
                    //        {
                    //            if (MMManage.lblCreatWarning != null)
                    //            {
                    //                MMManage.lblCreatWarning.text = operationResponse.DebugMessage;
                    //            }
                    //            this.isLogin = false;
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.HasNickName:
                    //        {
                    //            if (MMManage.lblCreatWarning != null)
                    //            {
                    //                MMManage.lblCreatWarning.text = operationResponse.DebugMessage;
                    //            }
                    //            this.isLogin = false;
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.Error:
                    //        {
                    //            this.isLogin = false;
                    //            Debug.LogError(operationResponse.DebugMessage);
                    //        }
                    //        break;
                    //}
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.PlayerCreat:
                {
                    MMManage.returnPlayerCreat = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
//                    MainMenuManage.dicPlayerCreat = operationResponse.Parameters;
                    MMManage.DebugPlayerCreat = operationResponse.DebugMessage;
                    MMManage.isPlayerCreat = true;
                    //switch (operationResponse.ReturnCode)
                    //{
                    //    case (short)yuan.YuanPhoton.ReturnCode.Yes:
                    //        {
                    //            //if (lblCreatWarning != null)
                    //            //{
                    //            //    lblCreatWarning.text = operationResponse.DebugMessage;
                    //            //}
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                    //        {
                    //            //if (lblCreatWarning != null)
                    //            //{
                    //            //    lblCreatWarning.text = operationResponse.DebugMessage;
                    //            //}
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.HasNickName:
                    //        {
                    //            //if (lblCreatWarning != null)
                    //            //{
                    //            //    lblCreatWarning.text = operationResponse.DebugMessage;
                    //            //}
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.Error:
                    //        {
                    //            Debug.LogError(operationResponse.DebugMessage);
                    //        }
                    //        break;
                    //}
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.PwdGet:
                {
                    MMManage.returnGetPwd = (ReturnCode)operationResponse.ReturnCode;
                    MMManage.DebugGetPwd = operationResponse.DebugMessage;
                    MMManage.isGetPwd = true;
                    //switch (operationResponse.ReturnCode)
                    //{
                    //    case (short)yuan.YuanPhoton.ReturnCode.Yes:
                    //        {
                    //            if (MMManage.lblGetPwdWarning != null)
                    //            {
                    //                MMManage.lblGetPwdWarning.text = operationResponse.DebugMessage;
                    //            }
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.No:
                    //        {
                    //            if (MMManage.lblGetPwdWarning != null)
                    //            {
                    //                MMManage.lblGetPwdWarning.text = operationResponse.DebugMessage;
                    //            }
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.Nothing:
                    //        {
                    //            if (MMManage.lblGetPwdWarning != null)
                    //            {
                    //                MMManage.lblGetPwdWarning.text = operationResponse.DebugMessage;
                    //            }
                    //        }
                    //        break;
                    //    case (short)yuan.YuanPhoton.ReturnCode.Error:
                    //        {
                    //            Debug.LogError(operationResponse.DebugMessage);
                    //        }
                    //        break;
                    //}
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.PwdUpdate:
                {
                    MMManage.returnPwdUpdate = (yuan.YuanPhoton.ReturnCode)operationResponse.ReturnCode;
                    MMManage.DebugPwdUpdate = operationResponse.DebugMessage;
                    MMManage.isPwdUpdate = true;
                    //switch (operationResponse.ReturnCode)
                    //{

                    //case (short)yuan.YuanPhoton.ReturnCode.Yes:
                    //    {
                    //        if (MMManage.lblUpdatePwdWarning != null)
                    //        {
                    //            MMManage.lblUpdatePwdWarning.text = operationResponse.DebugMessage;
                    //        }
                    //    }
                    //    break;
                    //case (short)yuan.YuanPhoton.ReturnCode.No:
                    //    {
                    //        if (MMManage.lblUpdatePwdWarning != null)
                    //        {
                    //            MMManage.lblUpdatePwdWarning.text = operationResponse.DebugMessage;
                    //        }
                    //    }
                    //    break;
                    //case (short)yuan.YuanPhoton.ReturnCode.Error:
                    //    {
                    //        Debug.LogError(operationResponse.DebugMessage);
                    //    }
                    //    break;
                    //}
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.BindUserID:
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
                   //CZZY     btnGameManagerBack.operationResponse.Add(operationResponse);
                    }
                    catch
                    { }
                }
                break;

            case (byte)yuan.YuanPhoton.OperationCode.GetPlayers:
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

                    }
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.DeletePlayer:
                {
                    MMManage.returnDeletePlayer = (ReturnCode)operationResponse.ReturnCode;
                    MMManage.DebugDeletePlayer = operationResponse.DebugMessage;
                    MMManage.isFast = (bool)operationResponse.Parameters[(byte)ParameterType.PlayerType];
                    MMManage.isDeletePlayer = true;
                }
                break;
            case (byte)yuan.YuanPhoton.OperationCode.SendBenefitsInfo:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
//                                dicBenefitsInfo = operationResponse.Parameters;
                                Dictionary<byte, object> dicTemp = (Dictionary<byte, object>)dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.GuildLevelUp];
                                string[] strTemp;
                                foreach (object item in dicTemp.Values)
                                {
                                    strTemp = ((string)item).Split(';');
                                    Dictionary<yuan.YuanPhoton.GuildLevelUp, int> dicGuild = new Dictionary<GuildLevelUp, int>();
                                    dicGuild.Add(yuan.YuanPhoton.GuildLevelUp.Build, int.Parse(strTemp[1]));
                                    dicGuild.Add(yuan.YuanPhoton.GuildLevelUp.Fund, int.Parse(strTemp[2]));
                                    dicGuildLevel.Add(int.Parse(strTemp[0]), dicGuild);
                                }
                                MMManage.isGetServerInfo = true;
                            }
                            break;

                    }
                }
				break;
            case (byte)yuan.YuanPhoton.OperationCode.OtherLogin:
                {
                    switch (operationResponse.ReturnCode)
                    {
                        case (short)yuan.YuanPhoton.ReturnCode.Yes:
                            {
                                MMManage.isOtherPlayerLogin = true;
                            }
                            break;

                    }
                }
				break;
				case (byte)yuan.YuanPhoton.OperationCode.Validation:
				{
//					AnlaysValidation (operationResponse);
				}
				break;						
        }
		}
		catch(System.Exception ex)
		{
			Debug.Log (ex.ToString ());
		}
    }
	
	/// <summary>
	/// 解析验证相关
	/// </summary>
	/// <param name='mResponse'>
	/// M response.
	/// </param>
	private void AnlaysValidation(Zealm.OperationResponse mResponse)
	{
		yuan.YuanPhoton.ValidationType validationType=(yuan.YuanPhoton.ValidationType)mResponse.Parameters[(short)yuan.YuanPhoton.ValidationParams.ValidationType];
		switch(validationType)
		{
			case yuan.YuanPhoton.ValidationType.Login://登录验证
			{
			       switch (mResponse.ReturnCode)
	                {
						
	                    case (short)yuan.YuanPhoton.ReturnCode.No:
	                        {
								//没有验证通过，为修改版本
									MMManage.isValidationLoginNo=true;
							}
	                        break;			
	                }				
			}
			break;	
				
		}
		
		if(mResponse.ReturnCode==(short)yuan.YuanPhoton.ReturnCode.Error)
		{
			Debug.LogError(mResponse.DebugMessage);
		}
	}	

    public void OnStatusChanged(StatusCode statusCode)
    {
        this.DebugReturn(0, string.Format("{0}{1}", Get("info548") , statusCode));
        switch (statusCode)
        {
            case StatusCode.Connect:
                Debug.Log("Connected:YuanUnityPhoton");
                GetYuanUnityPhotonInstantiate().ServerConnected = true;

                //this.serviceTimer.Enabled = true;
				isServiceTimer=true;
                //!john.add.alive.gate
                m_dwKeepAliveInternal = m_dwKeepAliveTime;
                //if (lblServerStatus != null)
                //{
                //    lblServerStatus.text = "Connected";
                //}
                break;
            case StatusCode.Disconnect:
                Debug.Log("Disconnect:YuanUnityPhoton");
                ServerConnected = false;

                //this.serviceTimer.Enabled = false;
				isServiceTimer=false;
                //if (lblServerStatus != null)
                //{
                //    lblServerStatus.text = "Disconnect";
                //}
                loginStatus = false;
                fastLoginStatus = yuan.YuanPhoton.ReturnCode.No;
                break;
        }
    }

	public void OnStatusChanged2(bool isConnected)
	{
		if(isConnected)
		{
//			Debug.Log("Connected:YuanUnityPhoton");
			GetYuanUnityPhotonInstantiate().ServerConnected = true;
			isServiceTimer=true;
			m_dwKeepAliveInternal = m_dwKeepAliveTime;
		}
		else
		{
//			Debug.Log("Disconnect:YuanUnityPhoton");
			ServerConnected = false;
			isServiceTimer=false;
			loginStatus = false;
			fastLoginStatus = yuan.YuanPhoton.ReturnCode.No;
		}
	}

    /// <summary>
    /// 设置服务器按钮
    /// </summary>
    /// <param name="mBtnServer">服务器按钮</param>
    /// <param name="mDic">信息</param>
    public void SetServerBtn(BtnServer mBtnServer, Dictionary<string, string> mDic)
    {
        foreach (KeyValuePair<string, string> item in mDic)
        {
            switch (item.Key)
            {
                case "name":
                    {
                        mBtnServer.ApplicationName = item.Value;
                    }
                    break;
                case "ip":
                    {
                        mBtnServer.ApplicationIp = item.Value;
                    }
                    break;
                case "host":
                    {
                        mBtnServer.ApplicationHost = item.Value;
                    }
                    break;
                case "nickname":
                    {
                        mBtnServer.lblServerName.text = item.Value;
                    }
                    break;
                case "area":
                    {
                        mBtnServer.lblServerArea.text = item.Value;
                    }
                    break;
                case "serverActorNum":
                    {
                        mBtnServer.lblServerState.text = item.Value;
                    }
                    break;
                case "tcp":
                    {
                        string t_strValue = item.Value;
                        if (t_strValue == "1")
                        {
                            PhotonHandler.SetUpdMode();
                        }

                    }
                    break;
                case "rm":
                    {
                        string t_strValue = item.Value;
                        PhotonHandler.SetLogicAddr(t_strValue);
                    }
                    break;
            }
        }
    }

    #endregion
}

