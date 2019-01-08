using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using yuan.YuanPhoton;

public class BtnManager : MonoBehaviour {
	
	//public YuanUnityPhoton yuanServer;
	public GameObject objLoading;
	public UILabel lblWarning;
	public UIInput txtName;
	public UIInput txtPwd;
	public List<GameObject> listMenu=new List<GameObject>();
	public UILabel lblLogonWarning;
    public UILabel lblGetPwdWarning;
    public UILabel lblUpdatePwdWarning;
    public UILabel lblPlayerBindWarning;
    public UILabel lblDeletePlayerWarning;

	public UIInput txtLogonName;
	public UIInput txtLogonPwd;
	public UIInput txtLogonPwdAgain;
	public UIInput txtLogonNickName;
	public UIInput txtLogonEmail;
	public UIInput txtLicense;
    public UIInput txtPlayerInvite;
    public UIInput txtGetPwdName;
    public UIInput txtGetPwdEmail;
    public UIInput txtPlayerBindName;
    public UIInput txtPlayerBindPwd;

    public UIToggle ckbRemeberMe;

    public UIGrid gridServerBind;

    public Warning waring;
	
	public string warningNoText;
	public string warningEmailValid;
	public string warningPwdValid;
    public string warningSensitive;
	public string warningTDLogin;
	public string noInfo;
	public string noName;
	public string noPwd;
	public string noPwdAgain;
	public string noNickName;
	public string noEmail;
	public Warnings warnings;
	
	public static bool isOhterLogin=false;
	public static BtnManager my;

	public static bool isTDlogin = false;
	public static bool isTDselet = false;
	
	void Awake()
	{
		my=this;
	}

    public Transform btn;
	void Start()
	{
        PhotonHandler.ReadIsShowLog();
        PhotonHandler.ShowLog("start btnmanager");
        if(PhotonHandler.IsShowLog())
        {
            if(btn !=null)
            {
                PhotonHandler.ShowLog("set btn visible");
                btn.localScale = Vector3.one;

            }

            
        }
        
		//yuan.YuanClass.SwitchListOnlyOne (listMenu,7,true,true);
		if(Application.loadedLevelName=="Login-1")
		{
			if(isOhterLogin)
			{
				OtherLogin ();
			}
       	 SelectBtnSize();
		}
		yuan.YuanClass.SwitchList (listPlayerInfo,false,false);
	}

    void Update()
    {
		if(null != enterButton && !enterButton.isEnabled)
		{
			if(null != nameObj && nameObj.active)
			{
				nameObj.SetActiveRecursively(false);
			}
			if(null != randomNameObj && randomNameObj.active) 
			{
				randomNameObj.SetActiveRecursively(false);
			}
			
			if(null != masterObj && masterObj.active)
			{
				masterObj.SetActiveRecursively(false);
			}
		}
		
        //if (InRoom.GetInRoomInstantiate() != null && !InRoom.GetInRoomInstantiate().ServerConnected)
        //{
        //    InRoom.GetInRoomInstantiate().Connect();
        //}
        
    }
	
	public delegate void Action();
	public delegate IEnumerator ActionCon(int mNum);
	
	/// <summary>
	/// YuanUnity服务器重连方法
	/// </summary>
	/// <returns>
	/// The yuan unity.
	/// </returns>
	/// <param name='mNum'>
	/// 超时时间
	/// </param>
	public IEnumerator ConnectYuanUnity(int mNum)
	{
		if(ZealmConnector.connection==null)
		//if(!YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ServerConnected)
		{
			YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = PlayerPrefs.GetString ("ConnectionIP");
			Debug.Log("BtmManageer:"+PlayerPrefs.GetString ("ConnectionIP"));
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerApplication =PlayerPrefs.GetString ("ConnectionApp");
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage =MainMenuManage.my;
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead = TableRead.my;
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
				int tempNum=0;
				while(true)
				{
					yield return new WaitForSeconds(1);
					tempNum++;
					if(tempNum>=mNum||YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ServerConnected)
					{
						break;
					}
				}
			
		}
	}
	
	/// <summary>
	/// InRoom服务器重连方法
	/// </summary>
	/// <returns>
	/// The in room.
	/// </returns>
	/// <param name='mNum'>
	/// 超时时间.
	/// </param>
	public IEnumerator ConnectInRoom(int mNum)
	{
		if(ZealmConnector.connection==null)
		//if(!InRoom.GetInRoomInstantiate ().ServerConnected)
		{
	            InRoom.NewInRoomInstantiate().SetAddress(PlayerPrefs.GetString("InAppServerIP"));
			    Debug.Log("ConnectInRoom:"+PlayerPrefs.GetString("InAppServerIP"));
	            InRoom.GetInRoomInstantiate().ServerApplication = PlayerPrefs.GetString("InAppServer");
	            InRoom.GetInRoomInstantiate().btnGameManagerBack = BtnGameManagerBack.my;
	            InRoom.GetInRoomInstantiate().SM = SendManager.my;
				InRoom.GetInRoomInstantiate ().Connect ();
				int tempNum=0;
				while(true)
				{
					yield return new WaitForSeconds(1);
					tempNum++;
					if(tempNum>=mNum||InRoom.GetInRoomInstantiate ().ServerConnected)
					{
						break;
					}
				}
		}
	}
	
	private bool isStartTimeOut=false;
	/// <summary>
	/// 开始超时记录
	/// </summary>
	/// <returns>
	/// 
	/// </returns>
	/// <param name='mNum'>
	/// 超时时间
	/// </param>
	/// <param name='mNum'>
	/// 连接超时时间
	/// </param>
	/// <param name='actionConnect'>
	/// 连接方法
	/// </param>
	/// <param name='action'>
	/// 执行方法
	/// </param>
	public IEnumerator BeginTimeOut(int mNum,int mNumCon,ActionCon actionConnect,Action action,Action actionTimeOut)
	{
		try{
			isStartTimeOut=true;
			objLoading.SetActiveRecursively (true);
		}catch(System.Exception ex){
			Debug.Log("BeginTimeOut___try_01"+ex.ToString());
		}
		if(actionConnect!=null)
		{
			yield return StartCoroutine (actionConnect(mNumCon));
		}
		if(action!=null)
		{
			action();
		}
		int outTime=0;
		while(true)
		{
			yield return new WaitForSeconds(1);
			try{
				outTime++;
				if(actionConnect==ConnectYuanUnity)
				{
					if(outTime>=mNum||!YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ServerConnected||!isStartTimeOut)
					{
						break;
					}
				}
				else
				{
					if(outTime>=mNum||!InRoom.GetInRoomInstantiate ().ServerConnected||!isStartTimeOut)
					{
						break;
					}
				}
			}catch(System.Exception ex){
				Debug.Log("BeginTimeOut___try_02"+ex.ToString());
			}

		}
		
		//yield return new WaitForSeconds(mNum);
		try{
			if(isStartTimeOut)
			{
//				NGUIDebug.Log("==timeout :" + action.Target.ToString() + "\n");
				if(isTDlogin){
					//TD_info.loginFail();
					isTDlogin = false;
				}
				
				if(isTDselet){
					//TD_info.selectFail(PlayerPrefs.GetString("InAppServerName", "NON"));
					isTDselet = false;
				}
				
				warnings.warningAllTimeOut.ShowTimeOut (StaticLoc.Loc.Get ("info358"),StaticLoc.Loc.Get ("info720"),()=>RunBeginTimeOut (mNum,mNumCon,actionConnect,action,actionTimeOut));
				objLoading.SetActiveRecursively (false);
				isStartTimeOut=false;
				if(actionTimeOut!=null)
				{
					actionTimeOut();
				}
			}
		}catch(System.Exception ex){
			Debug.Log("BeginTimeOut___try_03"+ ex.ToString());
		}

		
	}
	
	public IEnumerator BeginTimeOutNoRe(int mNum,int mNumCon,ActionCon actionConnect,Action action,Action actionTimeOut)
	{
		isStartTimeOut=true;
		objLoading.SetActiveRecursively (true);
		if(actionConnect!=null)
		{
			yield return StartCoroutine (actionConnect(mNumCon));
		}
		if(action!=null)
		{
			action();
		}
		int outTime=0;
		while(true)
		{
			yield return new WaitForSeconds(1);
			outTime++;
			if(outTime>=mNum||!YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ServerConnected||!isStartTimeOut)
			{
				break;
			}
		}
		
		//yield return new WaitForSeconds(mNum);
		if(isStartTimeOut)
		{
//            NGUIDebug.Log("==timeout :" + action.Target.ToString() + "\n");
			if(isTDlogin){
				//TD_info.loginFail();
				isTDlogin = false;
			}
			if(isTDselet){
				//TD_info.selectFail(PlayerPrefs.GetString("InAppServerName", "NON"));
				isTDselet = false;
			}

			warnings.warningAllEnter.Show (StaticLoc.Loc.Get ("info358"),StaticLoc.Loc.Get ("info720"));
			objLoading.SetActiveRecursively (false);
			isStartTimeOut=false;
			if(actionTimeOut!=null)
			{
				actionTimeOut();
			}
		}
		
	}	
	
	public void RunBeginTimeOut(int mNum,int mNumCon,ActionCon actionConnect,Action action,Action actionTimeOut)
	{
		StartCoroutine (BeginTimeOut (mNum,mNumCon,actionConnect,action,actionTimeOut));
	}
	
	/// <summary>
	/// 结束超时
	/// </summary>
	public void EndTimeOut()
	{
		isStartTimeOut=false;
		objLoading.SetActiveRecursively (false);
	}
	
	public static string strOtherLogin=string.Empty;
	public void OtherLogin()
	{
		isOhterLogin=false;
		BtnGameManagerBack.isOhterLogin=false;
		warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),strOtherLogin);
	}

    public BtnClick btnLgionBack;
    public void PlayerLoginTimeOut()
    {
        lblWarning.text = StaticLoc.Loc.Get("info321")+"";
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
        btnLgionBack.btnManager = this;
        btnLgionBack.invokMethodName = "Reonline";
    }

    /// <summary>
    /// Í¨ÐÐÖ¤µÇÂ¼
    /// </summary>
	public void PlayerLogin()
	{
		if(PhotonHandler.IsShowLog())
		{
			PhotonHandler.ShowLog(string.Format("PlayerLogin:{0}",txtName.text.Trim()));
		}
		
		
		//YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SetPlayerBehavior (yuan.YuanPhoton.ConsumptionType.GameSchedule,((int)GameScheduleType.Login).ToString (),SystemInfo.deviceUniqueIdentifier);
        
		
		//YuanUnityPhoton.GetYuanUnityPhotonInstantiate().SetPlayerBjoehavior (yuan.YuanPhoton.ConsumptionType.GameSchedule,((int)GameScheduleType.Login).ToString (),SystemInfo.deviceUniqueIdentifier);
        
		if(PhotonHandler.IsAutoLogin())
		{
			PhotonHandler.ShowLog("autologin mode");
			YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(PhotonHandler.GetAutoStr(), PhotonHandler.GetAutoPwd(), "ZealmPass", "UserInfo", true);
		}
		else
		{
			//YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(txtName.text.Trim(), txtPwd.text.Trim(), "ZealmPass", "UserInfo", true);
        	StartCoroutine (BeginTimeOut (10,2,ConnectYuanUnity,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(txtName.text.Trim(), txtPwd.text.Trim(), "ZealmPass", "UserInfo", true),null));
		}
		
		//yuan.YuanClass.SwitchListOnlyOne(listMenu, 10, true, true);
        passID = txtName.text.Trim();
		
		
		
		
        //if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected == true)
        //{
        //    if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null)
        //    {
        //        if (txtName.text.Trim() != "" && txtPwd.text.Trim() != "")
        //        {
        //            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(txtName.text.Trim(), txtPwd.text.Trim(), "ZealmPass", "UserInfo", true);
        //            yuan.YuanClass.SwitchListOnlyOne(listMenu, 10, true, true);
        //            passID = txtName.text.Trim();
        //            Invoke("PlayerLoginTimeOut", 20);
        //        }
        //        else
        //        {
        //            lblWarning.text = StaticLoc.Loc.Get(warningNoText);
        //        }
		//
        //    }
        //}
        //else
        //{
        //    lblWarning.text =StaticLoc.Loc.Get("info322")+ "";
        //}
	}


    /// <summary>
    /// ¿ìËÙµÇÂ¼
    /// </summary>
    public void PlayerFastLogin()
    {
       // YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerFastLogin(SystemInfo.deviceUniqueIdentifier, "DarkSword2", "PlayerInfo");
       // PlayerPrefs.SetInt("BtnSize", 1);
        //lblCreatWarning.text = "";
		//MainMenuManage.loginType=MainMenuManage.LoginType.FastLogin;
		//Application.LoadLevel (1);
		warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info482"));
    }
    

    /// <summary>
    /// ¿ìËÙ×¢²á
    /// </summary>
    public void PlayerFastLogon()
    {
        if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null)
        {
            if (txtCreatPlayerName.text.Trim() != "")
            {
                if (GetStringMatch(txtCreatPlayerName.text.Trim(), randomName.shieldedWord.myStr) == false)
                {
                    string proID = GetOpenPlayerType();
                    if (proID != "")
                    {
                        //Debug.Log("1111111111111111111111111" + PlayerPrefs.GetString("InFastServer"));
                        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerFastLogon(SystemInfo.deviceUniqueIdentifier, proID, PlayerPrefs.GetString("InFastServer"), txtCreatPlayerName.text.Trim(), "DarkSword2", "PlayerInfo");
                    }
                }
                else
                {
                    lblCreatWarning.text = StaticLoc.Loc.Get(warningSensitive) ;
                }
            }
            else
            {
                lblCreatWarning.text = StaticLoc.Loc.Get(noNickName) ;
            }
        }
    }

    public void MainMenu()
    {
       
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 7, true, true);
    }
	
	public UILabel lblStartPlayerID;
    /// <summary>
    /// ½øÈëµÇÂ¼½çÃæ
    /// </summary>
    public void LoginMenu()
    {
        //TD_info.setLogin(); // 点击登录按钮的TD统计
        if (PlayerPrefs.GetInt("RemeberMe", 1) == 1 && PlayerPrefs.GetString("UserID")!="")
		{
			lblStartPlayerID.text=StaticLoc.Loc.Get("info662")+PlayerPrefs.GetString ("UserID");
        	yuan.YuanClass.SwitchListOnlyOne(listMenu, 11, true, true);
		}
		else
		{
			yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
		}
        PlayerPrefs.SetInt("BtnSize", 0);
        if (PlayerPrefs.GetInt("RemeberMe",1) == 1)
        {
            ckbRemeberMe.value = true;
        }
        else
        {
            ckbRemeberMe.value = false;
        }

        txtName.text = PlayerPrefs.GetString("UserID");
		
        txtPwd.text = PlayerPrefs.GetString("UserPwd");
		txtPwd.selected=true;
		txtPwd.selected=false;
    }
	
	
	public void BtnGOWirteLogin()
	{
		yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
		txtPwd.selected=true;
		txtPwd.selected=false;
	}
	
    /// <summary>
    /// ½øÈëÏêÏ¸×¢²á½çÃæ
    /// </summary>
	public void LogonMenu()
	{
        txtLogonEmail.text = "";
        txtLogonName.text = "";
        txtLogonNickName.text = "";
        txtLogonPwd.text = "";
        txtLogonPwdAgain.text = "";
        lblLogonWarning.text = "";
		yuan.YuanClass.SwitchListOnlyOne (listMenu,1,true,true);
	}

    /// <summary>
    /// ½øÈëÃÜÂëÕÒ»Ø½çÃæ
    /// </summary>
    public void GetPwdMenu()
    {
        txtGetPwdEmail.text = "";
        txtGetPwdName.text = "";
        lblGetPwdWarning.text = "";
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 3, true, true);
    }

    private float timeTemp = 0;
    public string strSelectMost;
    /// <summary>
    /// ÉêÇëÕÒ»ØÃÜÂë
    /// </summary>
    public void GetPwd()
    {
        if (timeTemp < Time.time)
        {
            timeTemp = Time.time + 5;
            if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null)
            {
                if ( txtGetPwdEmail.text.Trim() != "")
                {
                    if (EmailValid(txtGetPwdEmail.text.Trim()))
                    {
                        //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().GetPwd(txtGetPwdName.text.Trim(), txtGetPwdEmail.text.Trim(), "ZealmPass", "UserInfo");
						StartCoroutine (BeginTimeOut (10,2,ConnectYuanUnity,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().GetPwd(txtGetPwdName.text.Trim(), txtGetPwdEmail.text.Trim()),null));
                    }
                    else
                    {
                        lblGetPwdWarning.text = StaticLoc.Loc.Get(warningEmailValid);
                    }
                }
                else
                {
                    lblGetPwdWarning.text = StaticLoc.Loc.Get(noInfo);
                }
            }
        }
        else
        {
            lblGetPwdWarning.text = StaticLoc.Loc.Get(strSelectMost) ;
        }
    }



    /// <summary>
    /// °ó¶¨Éè±¸²Ëµ¥µ¯³ö
    /// </summary>
    public void BindUserIDMenu()
    {
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 6, true, true);
        //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin("zealm", "", "", "", false);
    }


    /// <summary>
    /// °ó¶¨Éè±¸µ½Í¨ÐÐÖ¤
    /// </summary>
    public void BindUserID()
    {
        if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null)
        {
            if (txtPlayerBindName.text.Trim()!=""&&txtPlayerBindPwd.text.Trim()!="")
            {
                //UIToggle[] ckbTemp=gridServerBind.GetComponentsInChildren<UIToggle>();
                //foreach(UIToggle ckb in ckbTemp)
                //{
                //    if(ckb.value)
                //    {
                        //BtnServer btnServer=ckb.GetComponent<BtnServer>();
                        InRoom.GetInRoomInstantiate ().BindUserID(txtPlayerBindName.text.Trim(), txtPlayerBindPwd.text.Trim() ,"123456", "ZealmPass", "UserInfo");
                //    }
                //}
                
            }
            else
            {
               lblPlayerBindWarning.text= StaticLoc.Loc.Get(noInfo);
            }
        }
    }
	
    /// <summary>
    /// Íæ¼ÒÏêÏ¸×¢²á
    /// </summary>
	public void PlayerLogon()
	{
        if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null)
        {
                if ( txtLogonPwd.text.Trim() != "" && txtLogonPwdAgain.text.Trim() != "" && txtLogonEmail.text.Trim() != "")
                {
                    if (txtLogonPwd.text.Trim() == txtLogonPwdAgain.text.Trim())
                    {
                        if (EmailValid(txtLogonEmail.text.Trim()))
                        {
									//YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SetPlayerBehavior (yuan.YuanPhoton.ConsumptionType.GameSchedule,((int)GameScheduleType.Logon).ToString (),SystemInfo.deviceUniqueIdentifier);
                                //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogon("", "", txtLogonPwd.text.Trim(), txtLogonEmail.text.Trim(), txtLicense.text.Trim (),txtPlayerInvite.text.Trim(),"ZealmPass", "UserInfo");
                        		StartCoroutine (BeginTimeOut (10,2,ConnectYuanUnity,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogon("", "", txtLogonPwd.text.Trim(), txtLogonEmail.text.Trim(), txtLicense.text.Trim (),txtPlayerInvite.text.Trim(),"ZealmPass", "UserInfo"),null));
						}
                        else
                        {
                            lblLogonWarning.text = StaticLoc.Loc.Get(warningEmailValid);
                        }
                    }
                    else
                    {
                        lblLogonWarning.text = StaticLoc.Loc.Get(warningPwdValid);
                    }
                }
                else
                {
                    lblLogonWarning.text =  StaticLoc.Loc.Get(noInfo);
                }
            
        }
	}
	
   /// <summary>
   /// ¼ì²éÓÊ¼þ¸ñÊ½
   /// </summary>
   /// <param name="strEmail"></param>
   /// <returns></returns>
	private bool EmailValid(string strEmail)
	{
		return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); 
	}

    public string[] strSensitive;
    /// <summary>
    /// ±È½ÏÃô¸Ð´Ê
    /// </summary>
    /// <param name="str">×Ö·û´®</param>
    /// <param name="strChat">Ãô¸Ð´ÊÊý×é</param>
    /// <returns></returns>
    private bool GetStringMatch( string str,string[] strChat)
    {
        bool isSame = false;
        foreach (string temp in strChat)
        {
            if (str.IndexOf(temp)!=-1)
            {
                isSame = true;
                break;
            }
        }
        return isSame;
    }
	
    /// <summary>
    /// ·µ»ØÖ÷²Ëµ¥
    /// </summary>
	public void Back()
	{
        Application.LoadLevel(0);
		yuan.YuanClass.SwitchListOnlyOne (listMenu,7,true,true);
        lblWarning.text = "";
        SelectBtnSize();
        //this.background.gameObject.active = true;
	}

    public enum CameraStatus
    {
        NewPlayer,
        SelectPlayer,
    }
    public CameraStatus cameraStatus = CameraStatus.SelectPlayer;
    public Animation AnimCamera;
    public GameObject btnStartGame;
  	private bool isFirstInScene = true;	//第一次进入场景标志位
	public GameObject playerSelectionHint;	//角色选择提示
	public TweenRotation tweenCamera;
    public void CameraToNew()
    {
        if (cameraStatus == CameraStatus.SelectPlayer)
        {
			tweenCamera.PlayForward();
            //AnimCamera.CrossFade("CameraToNewPlayer");
            cameraStatus = CameraStatus.NewPlayer;
        }
		
        //AnimCamera.transform.localEulerAngles = new Vector3(354.2721f, 14.65f, 359.2356f);
        PlayerCreatIn();
        btnStartGame.SetActiveRecursively(false);
		
		if(isFirstInScene)
		{
			BeforeSelectCharacter (false);
			isFirstInScene = false;
		}
		
		playerSelectionHint.active = true;
		AnlyseSelectPlayer ();
    }

	private void AnlyseSelectPlayer()
	{
		if(listPlayerType[0].value)
		{
			yuan.YuanClass.SwitchListOnlyOne (listPlayerInfo,0,true,true);
		}
		else if(listPlayerType[1].value)
		{
			yuan.YuanClass.SwitchListOnlyOne (listPlayerInfo,1,true,true);
		}
		else if(listPlayerType[2].value)
		{
			yuan.YuanClass.SwitchListOnlyOne (listPlayerInfo,2,true,true);
		}
	}
	
	public GameObject randomNameObj;
	public GameObject nameObj; 
	public GameObject masterObj;
	public UIButton enterButton;
	/// <summary>
	/// 选择角色前后的一些UI设置
	/// </summary>
	public void BeforeSelectCharacter (bool flag)
	{
		if(null != randomNameObj) randomNameObj.SetActiveRecursively(flag);
		if(null != nameObj) nameObj.SetActiveRecursively(flag);
		if(null != masterObj && !masterObj.active) masterObj.SetActiveRecursively(true);
//		enterButton.isEnabled = flag;
	}

    public void CameraToSelectPlayer()
    {
        if (cameraStatus == CameraStatus.NewPlayer)
        {
			this.tweenCamera.PlayReverse();
            //AnimCamera.CrossFade("CameraToSelectPlayer");
            cameraStatus = CameraStatus.SelectPlayer;
            PlayerStartIn();
//			defaultRole.SendMessage("Back", SendMessageOptions.RequireReceiver);
        }
        //AnimCamera.transform.localEulerAngles = new Vector3(354.2721f, 280.7f, 359.2356f);
        //PlayerStartInRefresh();
        
		playerSelectionHint.active = false;
    }
	
	/// <summary>
	/// 购买一个角色槽，可以多创建一个角色
	/// </summary>
	public void BuyPlayerSlot(UIToggle toggle)
	{
        //Debug.Log(string.Format("===================={0},{1}", BtnGameManager.yt,BtnGameManager.yt[0]["userEmail"].YuanColumnText));
        string userName = BtnManager.passID;
        string quDao = TableRead.strPageName;

        toggle.value = false;
        btnStartGame.SetActiveRecursively(false);

        //StartCoroutine (BeginTimeOut(15,3,ConnectInRoom,() => InRoom.GetInRoomInstantiate().GetBuyLanWei(userName, quDao),null));
        
#if SDK_ZSYIOS
        // 正版iOS平台内购相关逻辑
        int playerSlotCount = PlayerPrefs.GetInt("PlayerSlotCount",2);
        if(playerSlotCount < 5)
        {
            StoreKitBinding.requestProductData("cszz.061");
	        StoreKitBinding.purchaseProduct("cszz.061",1);
        }
        else
        {
            warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info1013"));
        }
#else
        //10月29号出封测包先注释掉
        InRoom.GetInRoomInstantiate().GetBuyLanWei(userName, quDao);
#endif
	}

    public TweenPosition tweenPlayerSelect;
    public TweenPosition tweenPlayerCreat;
    public TweenPosition tweenPlayerPwd;
    public BtnClick btnPlayerCreatBack;
    public BtnClick btnPlayerCreatEnter;
    /// <summary>
    /// ÐÂ½¨½ÇÉ«²Ëµ¥µ¯³ö
    /// </summary>
    public void PlayerCreatIn()
    {
        //AnimCamera.transform.localEulerAngles = new Vector3(354.2721f, 14.65f, 359.2356f);
        txtCreatPlayerName.text = "";
        lblCreatWarning.text = "";
        //tweenPlayerCreat.Play(false);
        //tweenPlayerSelect.Play(true);
        //tweenPlayerPwd.Play(true);
        listMenu[4].SetActiveRecursively(true);
        listMenu[2].transform.localScale = new Vector3(0, 0, 0);
        btnPlayerCreatBack.invokMethodName = "CameraToSelectPlayer";
        btnPlayerCreatEnter.invokMethodName="CreatPlayer";
    }



    public UISprite background;
    /// <summary>
    /// µ¯»Ø½ÇÉ«Ñ¡Ôñ²Ëµ¥²¢Ë¢ÐÂ
    /// </summary>
    public void PlayerStartIn()
    {
        //AnimCamera.transform.localEulerAngles = new Vector3(354.2721f, 280.7f, 359.2356f);
        //tweenPlayerCreat.Play(true);
        //tweenPlayerSelect.Play(false);
        //tweenPlayerPwd.Play(true);
        //if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null && YuanUnityPhoton.GetYuanUnityPhotonInstantiate().userID.Trim() != "" && YuanUnityPhoton.GetYuanUnityPhotonInstantiate().userPwd.Trim() != "")
        //{
        //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().userID, YuanUnityPhoton.GetYuanUnityPhotonInstantiate().userPwd, "ZealmPass", "UserInfo", false);
        //}
		
		objLoading.SetActiveRecursively (true);
        InRoom.GetInRoomInstantiate ().GetPlayers(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().userID, PlayerPrefs.GetString("InAppServer"), "DarkSword2", "PlayerInfo");
        
       // background.gameObject.active = true;
    }

    /// <summary>
    /// µ¯»Ø½ÇÉ«Ñ¡Ôñ²Ëµ¥
    /// </summary>
    public void PlayerStartInRefresh()
    {
    	listMenu[4].SetActiveRecursively(false);
        listMenu[2].transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Ñ¡Ôñ·þÎñÆ÷
    /// </summary>
    public void SelectServer()
    {

        //if (this.txtName.text.Trim()!=""&&this.txtPwd.text.Trim()!="")
        //{
        //    //Debug.Log(this.txtName.text.Trim() + "+++" + this.txtPwd.text.Trim());
        //    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(this.txtName.text.Trim(), this.txtPwd.text.Trim(), "ZealmPass", "UserInfo", false);
        //}
        //background.gameObject.active = true;
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 8, true, true);
		listMenu[8].transform.localScale=Vector3.one;
		//listMenu[8].SetActiveRecursively(true);

        MainMenuManage.my.SortServer();

        MainMenuManage.isFirstInScene = true;//当点击选择服务器按钮后，将标志位置回true,然后玩家再次选择服务器后，如果角色列表仍为空，就直接将镜头切换到角色选择界面
    	
		warnings.warningAllEnter.Close();
		object a=null;
		object b=null;
		warnings.warningAllEnter.btnEnterEvent.SetEvent(null);
		warnings.warningAllEnter.btnEnter.target = warnings.warningAllEnter.gameObject;
		warnings.warningAllEnter.btnEnter.functionName = "Close";
	}

    /// <summary>
    /// ÐÞ¸ÄÃÜÂë²Ëµ¥µ¯³ö
    /// </summary>
    public void PlayerPwdIn()
    {
        txtUpdatePwdNew.text = "";
        txtUpdatePwdNewAgain.text = "";
        txtUpdatePwdOrigin.text = "";
        lblUpdatePwdWarning.text = "";


        //tweenPlayerCreat.Play(true);
        //tweenPlayerSelect.Play(true);
        //tweenPlayerPwd.Play(false);
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 5, true, true);
    }


    public UILabel lblCreatWarning;
    public UIInput txtCreatPlayerName;
    public static string passID;
    /// <summary>
    /// ÐÂ½¨½ÇÉ«
    /// </summary>
    public void CreatPlayer()
    {
		//TD_info.setCreatRole();
        if (InRoom.GetInRoomInstantiate() != null)
        {
			//string mName=randomName.GetNoString (txtCreatPlayerName.text.Trim());
			string mName=txtCreatPlayerName.text.Trim();
			 if (GetStringMatch(mName, randomName.shieldedWord.myStr) == false)
             {
	            if (mName!= "")
	            {
	                string proID = GetOpenPlayerType();
	                if (passID != ""&&proID!="")
	                {
						
							//YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SetPlayerBehavior (yuan.YuanPhoton.ConsumptionType.GameSchedule,((int)GameScheduleType.CreatePlayer).ToString (),SystemInfo.deviceUniqueIdentifier);
						InRoom.GetInRoomInstantiate ().SetSetPlayerBehavior (yuan.YuanPhoton.PlayerBehaviorType.GameSchedule,((int)GameScheduleType.CreatePlayer).ToString ());	
						//InRoom.GetInRoomInstantiate().PlayerCreat (passID, proID, txtCreatPlayerName.text.Trim(), "DarkSword2", "PlayerInfo",SystemInfo.deviceUniqueIdentifier);
	       					StartCoroutine (BeginTimeOut (10,2,ConnectInRoom,()=>InRoom.GetInRoomInstantiate().PlayerCreat (passID, proID, mName, "DarkSword2", "PlayerInfo",SystemInfo.deviceUniqueIdentifier),null));
	                }
	            }
	            else
	            {
	                lblCreatWarning.text =StaticLoc.Loc.Get(noNickName);
	            }
			}
			else
			{
					lblCreatWarning.text = StaticLoc.Loc.Get(warningSensitive) ;
			}
        }
    }

    public List<UIToggle> listPlayerType;
    private string GetOpenPlayerType()
    {
        string type = string.Empty;
        for (int i = 0; i <3; i++)
        {
            if (listPlayerType[i].value)
            {
                type = (i+1).ToString();
                break;
            }
        }
        return type;

    }

    public UIInput txtUpdatePwdOrigin;
    public UIInput txtUpdatePwdNew;
    public UIInput txtUpdatePwdNewAgain;


    /// <summary>
    /// ¸ü¸ÄÃÜÂë
    /// </summary>
    public void UpdatePwd()
    {
        //SetWarningNO();
        listMenu[5].transform.localScale = Vector3.one;
        if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate() != null)
        {
            if (txtUpdatePwdOrigin.text.Trim() != "" && txtUpdatePwdNew.text.Trim() != "" && txtUpdatePwdNewAgain.text.Trim() != "")
            {
                if (txtUpdatePwdNew.text.Trim() == txtUpdatePwdNewAgain.text.Trim())
                {
                    if (passID != "")
                    {
                        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().UpdatePwd(passID, txtUpdatePwdOrigin.text.Trim(), txtUpdatePwdNew.text.Trim());
                    }
                }
                else
                {
                    lblUpdatePwdWarning.text = StaticLoc.Loc.Get(warningPwdValid);
                }
            }
            else
            {
                lblUpdatePwdWarning.text =  StaticLoc.Loc.Get(noInfo);
            }
        }
    }

    public string strUpdatePwd;
    /// <summary>
    /// ÐÞ¸ÄÃÜÂëÈ·ÈÏ
    /// </summary>
    public void UpdatePwdBtnClick()
    {
        listMenu[5].transform.localScale = Vector3.zero;
        waring.btnYes.btnManager = this;
        waring.btnYes.invokMethodName = "UpdatePwd";
        waring.lblText.text = StaticLoc.Loc.Get("info310");
        waring.Out();
        waring.btnNo.invokMethodName = "SetWarningNO";
        warningNoNum = 5;
    }

    public int warningNoNum;
    public void SetWarningNO()
    {
        listMenu[warningNoNum].gameObject.SetActiveRecursively(true);
    }


    public string strDeleteNoSelect;
    /// <summary>
    /// É¾³ý½ÇÉ«
    /// </summary>
    public void DeletePlayer()
    {

         UIToggle[] cbkTemp = listMenu[2].GetComponentsInChildren<UIToggle>();
         if (cbkTemp.Length > 0)
         {

             foreach (UIToggle cbx in cbkTemp)
             {
                 if (cbx.value)
                 {
					
                     BtnPlayer btnPlayer = cbx.GetComponent<BtnPlayer>();
					
                     //InRoom.GetInRoomInstantiate ().DelectPlayer(btnPlayer.yuanRow["PlayerID"].YuanColumnText.Trim(), "DarkSword2", "PlayerInfo",false);
					StartCoroutine (BeginTimeOut (10,2,ConnectInRoom,()=>InRoom.GetInRoomInstantiate ().DelectPlayer(btnPlayer.yuanRow["PlayerID"].YuanColumnText.Trim(), "DarkSword2", "PlayerInfo",false),null));
                     break;
                 }
             }
         }
         else
         {
             lblDeletePlayerWarning.text =StaticLoc.Loc.Get(strDeleteNoSelect) ;
         }
    }

    public void DeleteFastPlayer()
    {
        UIToggle[] cbkTemp = listMenu[2].GetComponentsInChildren<UIToggle>();
        if (cbkTemp.Length > 0)
        {

            foreach (UIToggle cbx in cbkTemp)
            {
                if (cbx.value)
                {
                    BtnPlayer btnPlayer = cbx.GetComponent<BtnPlayer>();
                    InRoom.GetInRoomInstantiate ().DelectPlayer(btnPlayer.yuanRow["PlayerID"].YuanColumnText.Trim(), "DarkSword2", "PlayerInfo",true);
                    //PlayerFastLogin();
                    break;
                }
            }
        }
        else
        {
            lblDeletePlayerWarning.text = StaticLoc.Loc.Get(strDeleteNoSelect);
        }
    }

    public string strDeletePlayer;
    public void DeletePlayerBtnClick()
    {
        UIToggle[] cbkTemp = listMenu[2].GetComponentsInChildren<UIToggle>();
        if (cbkTemp.Length > 0)
        {

            foreach (UIToggle cbx in cbkTemp)
            {
                if (cbx.value)
                {
                    //listMenu[2].transform.localScale = Vector3.zero;
                    waring.btnYes.btnManager = this;
                    //waring.btnYes.invokMethodName = "DeleteFastPlayer";
                    waring.btnYes.invokMethodName = "DeletePlayer";
                    waring.lblText.text = StaticLoc.Loc.Get(strDeletePlayer);
                    waring.Out();
                    //waring.btnNo.invokMethodName = "PlayerFastLogin";
                    waring.btnNo.invokMethodName = "OpneSelectPlayerMenu";
                    break;
                }
            }
        }
        else
        {
            lblDeletePlayerWarning.text = StaticLoc.Loc.Get(strDeleteNoSelect);
        }

    }

    public void OpneSelectPlayerMenu()
    {
        listMenu[2].transform.localScale = new Vector3(1, 1, 1);
    }


    public void DeleteFastPlayerBtnClick()
    {
        UIToggle[] cbkTemp = listMenu[2].GetComponentsInChildren<UIToggle>();
        if (cbkTemp.Length > 0)
        {

            foreach (UIToggle cbx in cbkTemp)
            {
                if (cbx.value)
                {
                    listMenu[2].transform.localScale = Vector3.zero;
                    waring.btnYes.btnManager = this;
                    waring.btnYes.invokMethodName = "DeleteFastPlayer";
                    waring.lblText.text = StaticLoc.Loc.Get(strDeletePlayer);
                    waring.Out();
                    waring.btnNo.invokMethodName = "PlayerFastLogin";
                    break;
                }
            }
        }
        else
        {
            lblDeletePlayerWarning.text = StaticLoc.Loc.Get(strDeleteNoSelect);
        }

    }

    public UILabel[] strTxtBtnSize;
    public BtnClick[] strBtnBtnSize;
    public string[] strBtnSize;
    /// <summary>
    /// ×Ô¶¯¼ÇÒä°´Å¥
    /// </summary>
    public void SelectBtnSize()
    {
		try
		{
	        int sizeType = PlayerPrefs.GetInt("BtnSize");
	        
	        if (sizeType == 1)
	        {
	            strTxtBtnSize[0].text = StaticLoc.Loc.Get(strBtnSize[0]);
	            strBtnBtnSize[0].invokMethodName = "LoginMenu";
	            strTxtBtnSize[1].text = StaticLoc.Loc.Get(strBtnSize[1]);
	            strBtnBtnSize[1].invokMethodName = "PlayerFastLogin";
	        }
	        else if (sizeType == 0)
	        {
	            strTxtBtnSize[0].text = StaticLoc.Loc.Get(strBtnSize[1]);
	            strBtnBtnSize[0].invokMethodName = "PlayerFastLogin";
	            strTxtBtnSize[1].text = StaticLoc.Loc.Get(strBtnSize[0]);
	            strBtnBtnSize[1].invokMethodName = "LoginMenu";
	        }
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning (ex.ToString ());
		}
    }

    public CkbToPanel ctpPlayerInfo;

    public TweenPosition objSoldier;
    public TweenPosition objRobber;
    public TweenPosition objMaster;

	public List<GameObject> listPlayerInfo;
    /// <summary>
    /// Ñ¡ÔñÕ½Ê¿
    /// </summary>
    public void SelectSoldier()
    {
        listPlayerType[0].value = true;
        ctpPlayerInfo.CbkClick();
        objSoldier.Reset();
		yuan.YuanClass.SwitchListOnlyOne (listPlayerInfo,0,true,true);
			        //objSoldier.localPosition = new Vector3(objSoldier.localPosition.x, -300, objSoldier.localPosition.z);
        //objSoldier.GetComponent<TweenPosition>().Play(true);
    }

    /// <summary>
    /// Ñ¡ÔñµÁÔô
    /// </summary>
    public void SelectRobber()
    {
        listPlayerType[1].value = true;
        ctpPlayerInfo.CbkClick();
        objRobber.Reset();
		yuan.YuanClass.SwitchListOnlyOne (listPlayerInfo,1,true,true);
        //objRobber.localPosition = new Vector3(objRobber.localPosition.x, -300, objRobber.localPosition.z);
        //objRobber.GetComponent<TweenPosition>().Play(true);
    }

    /// <summary>
    /// Ñ¡Ôñ·¨Ê¦
    /// </summary>
    public void SelectMaster()
    {
        listPlayerType[2].value = true;
        ctpPlayerInfo.CbkClick();
        objMaster.Reset();
		yuan.YuanClass.SwitchListOnlyOne (listPlayerInfo,2,true,true);
        //objMaster.localPosition = new Vector3(objMaster.localPosition.x, -300, objMaster.localPosition.z);
        //objMaster.GetComponent<TweenPosition>().Play(true);
    }


    /// <summary>
    /// ²âÊÔ½×¶ÎÊý¾Ý¿âµ÷½Ú
    /// </summary>
    public void TestServerBtnClick()
    {
        yuan.YuanClass.SwitchListOnlyOne(listMenu, 9, true, true);

    }
	
	public GetRandomName randomName;
	public void GetName()
	{
		if(GetOpenPlayerType ()=="2")
		{
			txtCreatPlayerName.text=randomName.GetFemaleName ();
		}
		else
		{
			txtCreatPlayerName.text=randomName.GetMaleName ();
		}
			
	}
	
	public void Reonline()
	{
        //if (//TD_info.isTDStartGame)
        //{
        //    //TD_info.startFail();// TD接入读条失败
        //}
		Application.LoadLevel (0);
	}
}
