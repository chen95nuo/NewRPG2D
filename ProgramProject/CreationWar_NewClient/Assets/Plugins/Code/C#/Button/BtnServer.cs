using UnityEngine;
using System.Collections;

public class BtnServer : MonoBehaviour {

    private string applicationName;
    /// <summary>
    /// ·þÎñÆ÷Ãû³Æ
    /// </summary>
    public string ApplicationName
    {
        get { return applicationName; }
        set { applicationName = value; }
    }

    private string applicationIp;
    /// <summary>
    /// ·þÎñÆ÷ip
    /// </summary>
    public string ApplicationIp
    {
        get { return applicationIp; }
        set { applicationIp = value; }
    }

    private string applicationHost;
    /// <summary>
    /// ·þÎñÆ÷¶Ë¿Ú
    /// </summary>
    public string ApplicationHost
    {
        get { return applicationHost; }
        set { applicationHost = value; }
    }
	
	private int serverActorNum;

	public int ServerActorNum {
		get {
			return this.serverActorNum;
		}
		set {
			serverActorNum = value;
		}
	}
	
	private string roomIP;

	public string RoomIP {
		get {
			return this.roomIP;
		}
		set {
			roomIP = value;
		}
	}

	private string numTitle;
	/// <summary>
	/// 服务器标识
	/// </summary>
	/// <value>The number title.</value>
	public string NumTitle {
		get {
			return numTitle;
		}
		set {
			numTitle = value;
		}
	}
	
	/// <summary>
	/// 逻辑服连接方式（0为tcp，1为udp）
	/// </summary>
	public string tcp="1";
	
	/// <summary>
	/// 房间服连接方式0为tcp，1为udp）
	/// </summary>
	public string rmtcp="1";
	
	/// <summary>
	/// 房间服房间最大人数
	/// </summary>
	public int rmMaxPlayer=25;
	
	public bool isTest=false;


    public UILabel lblServerName;
    public UILabel lblServerArea;
    public UILabel lblServerState;
    public BtnManager btnManage;

    public bool isFastBtn = false;
    public MainMenuManage mainMenuManage;
	
	public void CopyTo(BtnServer btnCopy)
	{
		btnCopy.ApplicationName=this.ApplicationName;
		btnCopy.ApplicationIp=this.ApplicationIp;
		btnCopy.ApplicationHost=this.ApplicationHost;
		btnCopy.lblServerName.text=this.lblServerName.text;
		btnCopy.lblServerArea.text=this.lblServerArea.text;
		btnCopy.lblServerState.text=this.lblServerState.text;
		btnCopy.btnManage=this.btnManage;
		btnCopy.isFastBtn=this.isFastBtn;
		btnCopy.mainMenuManage=this.mainMenuManage;
		btnCopy.RoomIP=this.RoomIP;
		btnCopy.tcp=this.tcp;
		btnCopy.rmtcp=this.rmtcp;
		btnCopy.rmMaxPlayer=this.rmMaxPlayer;
		btnCopy.isTest=this.isTest;
		btnCopy.NumTitle=this.NumTitle;
	}
	
	public void OnEnable()
	{
		if(this.isTest)
		{
			this.gameObject.SetActiveRecursively (false);
		}
	}
	
   public void OnClick()
    {
		PlayerPrefs.SetString("lblServerNameS1" , lblServerName.text);
		PlayerPrefs.SetString("NumTitleS1" , NumTitle);
		//TD_info.setSelectServer(this.lblServerName.text);
		 if(tcp=="1")//设置逻辑服务器连接方式
         {
             PhotonHandler.SetUpdMode();
         }
		else
		{
			PhotonHandler.SetTcpMode ();
		}
		
		PhotonHandler.SetLogicAddr(roomIP);//设置房间服务器地址
		if(rmtcp=="1")//设置房间服务器连接方式
		{
			PhotonHandler.roomConnectType=ExitGames.Client.Photon.ConnectionProtocol.Udp;
		}
		else
		{
			PhotonHandler.roomConnectType=ExitGames.Client.Photon.ConnectionProtocol.Tcp;
		}
		PhotonNetwork.SetPhoton ();
		
		BtnGameManager.roomPlayerNum=rmMaxPlayer;//设置房间服务器房间最大人数
		
		
	//	int playerMaxNum=(int) YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.PlayerMaxNum];
        int playerMaxNum = (int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.PlayerMaxNum];
		if(serverActorNum<playerMaxNum)
		{
            if (!MainMenuManage.my.Equals(null))
			{

                MainMenuManage.my.Connect(this.applicationName, "115.29.36.226", this.applicationHost, isFastBtn, this.lblServerName.text);
			}
		}
		else
		{
			mainMenuManage.warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info655"));
		}
    }
	


    IEnumerator Connect()
    {
        if (isFastBtn)
        {
			

//			Debug.Log ("8888888888");
            mainMenuManage.btnCreatPlayerBack.invokMethodName = "Back";
            mainMenuManage.btnCreatPlayerEnter.invokMethodName = "PlayerFastLogon";
            
            btnManage.AnimCamera.CrossFade("CameraToNewPlayer");
            btnManage.cameraStatus = BtnManager.CameraStatus.NewPlayer;
            mainMenuManage.isLogin = false;
            PlayerPrefs.SetString("InFastServer", this.applicationName);
			mainMenuManage.listMenu[4].transform.localScale = new Vector3(1, 1, 1);
			yuan.YuanClass.SwitchListOnlyOne(mainMenuManage.listMenu, 4, true, true);
//			Debug.Log ("99999999999");
			//mainMenuManage.YuanSetActiveRecursively(mainMenuManage.btnSelectServerCreate, false);
            
        }
        else
        {
			
			outTime=0;
			InvokeRepeating ("ReOnline",1,1);
            if (!InRoom.GetInRoomInstantiate().ServerConnected)
            {
				PhotonHandler.ShowLog("ReOnlin");
                InRoom.NewInRoomInstantiate().SetAddress(this.applicationIp + ":" + this.applicationHost);
                InRoom.GetInRoomInstantiate().ServerApplication = this.applicationName;
                InRoom.GetInRoomInstantiate().Connect();
            }
            while (!InRoom.GetInRoomInstantiate().ServerConnected)
            {
                yield return new WaitForSeconds(0.1f);
            }
			
			YuanUnityPhoton.YuanDispose ();
            yuan.YuanClass.SwitchListOnlyOne(mainMenuManage.listMenu, 9, true, true);
            mainMenuManage.listMenu[9].SetActiveRecursively(true);
          	InRoom.GetInRoomInstantiate ().GetPlayers(YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().userID, this.applicationName, "DarkSword2", "PlayerInfo");
            PlayerPrefs.SetString("InAppServer", this.applicationName);
			PhotonHandler.ShowLog("InAppServerIP Set:"+this.applicationIp );
            PlayerPrefs.SetString("InAppServerIP", InRoom.GetInRoomInstantiate().GetSvrAddress());
        }
    }
	
	public static int outTime;
	void ReOnline()
	{
		if(outTime==100)
		{
			CancelInvoke ("ReOnline");
		}
		if(outTime==5)
		{
			this.gameObject.SetActiveRecursively (true);
			StartCoroutine (Connect ());
			CancelInvoke ("ReOnline");
		}
		outTime++;
	}
	

	
	
}
