using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using yuan.YuanMemoryDB;

public class MainMenuManage : MonoBehaviour {

    public GameObject objDontDestroy;
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string ServerAddress = "localhost:5055";

    /// <summary>
    /// 服务器应用名称
    /// </summary>
    public string ServerApplication = "YuanPhotonServerRoom";
    public bool isFirstLevel = false;
    public BtnManager btnManage;
    public UILabel lblServerStatus;
    public UILabel lblLoginWarning;
    public UILabel lblLogonWarning;
    public UILabel lblCreatWarning;
    public UILabel lblGetPwdWarning;
    public UILabel lblUpdatePwdWarning;
    public UILabel lblPlayerBindWarning;
    public UILabel lblPlayerDeleteWarning;
	
	//角色列表相关变量
    public List<BtnPlayer> listPlayerBtn;
//	[HideInInspector]
    //public int maxPlayerNum = 10;	//角色列表最大角色数量
    //public int freePlayerNum = 2;	//角色列表免费角色数量
    //public int buyPlayerNum = 0;	//角色列表最大角色数量,且免费数量不能超过最大数量
	
    public List<GameObject> listMenu;
    public UIButton btnCreatPlayer;
    public BtnClick btnCreatPlayerBack;
    public BtnClick btnCreatPlayerEnter;
    public BtnClick btnSelectPlayerBack;
    public BtnClick btnSelectPlayerBind;
    public GameObject btnSelectPlayerPwd;
    public GameObject btnServer;
    public GameObject btnServerBind;
    public UIGrid gridServer;
    public UIGrid gridServerBind;
    public UIToggle cbxRemeberMe;
    public UIInput txtUserID;
    public UIInput txtUserPwd;
    public Warnings warnings;
	
	public enum LoginType
	{
		Login,
		FastLogin,
	}
	
	public static LoginType loginType;

    public enum GameLoginType
    {
        MainMenu,
        PlayerList,
        BindPlayer,
    }

    public static GameLoginType gameLoginType = GameLoginType.MainMenu;
	
	public static MainMenuManage my;

	void Awake()
	{
        isSetID = false;
		my=this;
	}
    void Start()
    {   
       //YuanUnityPhoton  yuan =  YuanUnityPhoton.GetYuanUnityPhotonInstantiate();
       //yuan.ServerAddress = this.ServerAddress;
       //yuan.ServerApplication = this.ServerApplication;
       //yuan.Connect();
				
        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage = this;
		InRoom.GetInRoomInstantiate ().MMManage=this;
        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerAddress = this.ServerAddress;

        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerApplication = this.ServerApplication;
        //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
        if (Application.loadedLevelName=="Login-2")
        {
            if (gameLoginType != GameLoginType.MainMenu)
            {
                //GameRetrunMenu();
				StartCoroutine (GameRetrunMenu ());
            }
            else
            {
                switch (loginType)
                {
                    case LoginType.Login:
                        LoginInServer(false);
                        break;
                    case LoginType.FastLogin:
                        LoginFastServer();
                        break;
                }
            }
        }
        
    }

    void OnApplicationQuit()
    {
//        InRoom.GetInRoomInstantiate().peer.Disconnect();
//        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Disconnect();
		InRoom.YuanDispose ();
		YuanUnityPhoton.YuanDispose ();
    }


    [HideInInspector]public bool isLogin = false;
    [HideInInspector]public bool isLogon = false;
    [HideInInspector]
    public bool isGetPlayers = false;
    [HideInInspector]
    public bool isPlayerCreat = false;
    [HideInInspector]
    public bool isPwdUpdate = false;
    [HideInInspector]
    public bool isDeletePlayer = false;
    [HideInInspector]
    public bool isGetPwd = false;
    [HideInInspector]
    public bool isFastLogin = false;
    [HideInInspector]
    public bool isFastLogon = false;
    [HideInInspector]
    public bool isBindID = false;
    [HideInInspector]
    public bool isGetServerInfo = false;
	[HideInInspector]
	public bool isValidationLoginNo=false;
    [HideInInspector]
    public static bool isSetID = false;
    [HideInInspector]
    public bool isGetSetID = false;
	
	
	public static bool isFirstInScene = true;
    void Update()
    {
        ////Debug.Log(InRoom.GetInRoomInstantiate().peer.PeerState);

        //当玩家进入角色选择界面时，如果还没有创建角色，则镜头直接切换到角色选择界面
		if(isFirstInScene && string.Equals(Application.loadedLevelName,"Login-2"))
		{
			if(listPlayerBtn[0].gameObject.active && yuan.YuanPhoton.ReturnCode.Nothing == returrnGetPlayers)
	        {
				listPlayerBtn[0].OnClick();
				
				isFirstInScene = false;
			}
		}


		
        if (isLogin)
        {
            this.Lgoin();
            isLogin = false;
        }
		if(isSetLicense)
		{
			SetNoLicense ();
		}
		if(isLoginUC)
		{
			LoginUC();
		}
        if (isLogon)
        {
            this.Logon();
            isLogon = false;
        }
        if (isGetPlayers)
        {
			//btnManage.objLoading.SetActiveRecursively (false);
			try
			{
				CancelInvoke ("TimeOut");
            	GetPlayers();
            	isGetPlayers = false;
				btnManage.EndTimeOut ();
			}
			catch(System.Exception ex)
			{
				isGetPlayers = false;
				Debug.LogError (ex.ToString ());
				btnManage.EndTimeOut ();
			}
        }
        if (isPlayerCreat)
        {
            this.PlayerCreat();
            isPlayerCreat = false;
			btnManage.EndTimeOut ();
        }
        if (isPwdUpdate)
        {
            this.PwdUpdate();
            isPwdUpdate = false;
        }
        if (isDeletePlayer)
        {
			btnManage.objLoading.SetActiveRecursively (false);
            this.DeletePlayer();
            isDeletePlayer = false;
			btnManage.EndTimeOut ();
        }
        if (isGetPwd)
        {
            this.GetPwd();
            isGetPwd = false;
        }
        if (isFastLogin)
        {
            this.FastLogin();
            isFastLogin = false;
        }
        if (isFastLogon)
        {
            this.FastLogon();
            isFastLogon = false;
        }
        if (isBindID)
        {
            this.BindID();
            isBindID = false;
			btnManage.EndTimeOut ();
        }
        if (isGetServerInfo)
        {
            this.GetServerInfo();
            isGetServerInfo = false;
        }
		if(isZSYBack)
		{
			isZSYBack=false;
			ZSYBack ();
		}
		if(isOtherPlayerLogin)
		{
			isOtherPlayerLogin=false;
			OtherPlayerLogin ();
		}
		if(isValidationLoginNo)
		{
			isValidationLoginNo=false;
			ValidationLoginNo();
		}
		if(isGetSetID)
		{
			isGetSetID=false;
			GetSetID();
			isSetID=true;
		}
		
	}
	
	public string[] setIDKey;
	public string[] setIDValue;
	private void GetSetID()
	{
		//RefershYT(setIDKey,setIDValue);
	}
	
	
	private void RefershYT(string[] strKey,string[] strValue)
	{
		for(int i=0;i<strKey.Length;i++)
		{
			if(!string.IsNullOrEmpty (strKey[i])&&BtnGameManager.yt.Rows[0].ContainsKey (strKey[i]))
			{
				BtnGameManager.yt.Rows[0][strKey[i]].YuanColumnText=strValue[i];
			}
		}	
	}
	
	void ValidationLoginNo()
	{
		 warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info810"));
	}
	
	public bool isZSYBack=false;
	public string strZSYBack=string.Empty;
	public void ZSYBack()
	{
#if UNITY_ANDROID
#if SDK_UC
//		SDKManager.zzsdk_passArguments(strZSYBack);
#else
		SDKManager.zzsdk_passArguments(strZSYBack);
#endif
#endif
	}
	
	public bool isOtherPlayerLogin=false;
	public void OtherPlayerLogin()
	{
		BtnManager.isOhterLogin=true;
		BtnManager.strOtherLogin=StaticLoc.Loc.Get("info481");
		Application.LoadLevel ("Login-1");	
	}

    /// <summary>
    /// 从游戏中退出到主菜单
    /// </summary>
    public IEnumerator GameRetrunMenu()
    {
        switch (gameLoginType)
        {
            case GameLoginType.MainMenu:
                {
                    
                }
                break;
            case GameLoginType.PlayerList:
                {
			        PhotonHandler.ShowLog("PlayerList");
                    string mAddress = PlayerPrefs.GetString("InAppServerIP");
                    Debug.Log("---------------Address----------" + mAddress);

					ZealmConnector.closeConnection();
				    InRoom.NewInRoomInstantiate().SetAddress(mAddress);
	        		InRoom.GetInRoomInstantiate().ServerApplication = PlayerPrefs.GetString("InAppServer");
	        		InRoom.GetInRoomInstantiate().Connect();
					while(!InRoom.GetInRoomInstantiate ().ServerConnected)
					{
						yield return new WaitForSeconds(0.1f);
					}			
					
                    //yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
                    ////Debug.Log(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected+",登录信息：" + PlayerPrefs.GetString("GameUserID") + "," + PlayerPrefs.GetString("GameUserPwd"));
                    //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin(PlayerPrefs.GetString("GameUserID"), PlayerPrefs.GetString("GameUserPwd"), "ZealmPass", "UserInfo",true);
                    //Debug.Log("获取玩家角色列表:" + BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText + ";" + PlayerPrefs.GetString("InAppServer"));
                    LoginInServer(true);
                    listMenu[8].SetActiveRecursively(false);
                    InRoom.GetInRoomInstantiate ().GetPlayers(BtnGameManager.yt.Rows[0]["UserInfo_userId"].YuanColumnText, PlayerPrefs.GetString("InAppServer"), "DarkSword2", "PlayerInfo");
                }
                break;
            case GameLoginType.BindPlayer:
                {

                }
                break;
        }

        gameLoginType = GameLoginType.MainMenu;
    }

    public void SwitchListOnlyOne(List<GameObject> list, int num, bool enable, bool includeChild)
    {
        yuan.YuanClass.SwitchListOnlyOne(list,num, enable, includeChild);
    }

    public void YuanSetActiveRecursively(GameObject obj, bool enable)
    {
        obj.SetActiveRecursively(enable);
    }
	
	public BtnServer btnSelectedServer;
	public UILabel lblSelectedServer;
	public static string firstServer=string.Empty;
    /// <summary>
    /// 切换场景后登陆
    /// </summary>
    public void LoginInServer(bool isOnlyShow)
    {
		//List<BtnServer> listBtnServer=new List<BtnServer>();
        //BtnServer[] strBtnServerTemp = this.gridServer.transform.GetComponentsInChildren<BtnServer>();
		//BtnServer tempSelectServer=null;
		//BtnServer tempMeberServer=null;
		//int numSelectServer=999999;
        //int numMeberServer=-1;
        //string strMeberServer=PlayerPrefs.GetString ("InAppServerIP");
        //int i = 0;

        listMenu[8].SetActiveRecursively(true);

        /*
		foreach (KeyValuePair<short, object> item in dicLogin)
        {
			object getObj = item.Value;
			Dictionary<object,object> getDic=(Dictionary<object, object>)getObj;
			Dictionary<string, string> dicTemp = getDic.DicObjTo<string,string>();
            if (i < strBtnServerTemp.Length)
            {
                SetServerBtn(strBtnServerTemp[i], dicTemp);
				if(strBtnServerTemp[i].isTest)
				{
					continue;
				}
                strBtnServerTemp[i].isFastBtn = false;
				if(strBtnServerTemp[i].ServerActorNum<=numSelectServer)
				{
					tempSelectServer=strBtnServerTemp[i];
					numSelectServer=tempSelectServer.ServerActorNum;
				}
				if(strMeberServer==string.Format ("{0}:{1}",strBtnServerTemp[i].ApplicationIp,strBtnServerTemp[i].ApplicationHost))
				{
					numMeberServer=i;
					tempMeberServer=strBtnServerTemp[i];
				}
				listBtnServer.Add (strBtnServerTemp[i]);
            }
            else
            {
                GameObject tempObj = (GameObject)GameObject.Instantiate(this.btnServer);
                tempObj.transform.parent = this.gridServer.transform;
                tempObj.transform.localPosition = Vector3.zero;
                tempObj.transform.localEulerAngles = Vector3.zero;
                tempObj.transform.localScale = new Vector3(1, 1, 1);
                BtnServer btnServerTemp = tempObj.GetComponent<BtnServer>();

                if (btnServerTemp != null)
                {
                	btnServerTemp.mainMenuManage = this;
               	    btnServerTemp.isFastBtn = false;					
                    SetServerBtn(btnServerTemp, dicTemp);
					if(btnServerTemp.isTest)
					{
						continue;
					}
					if(btnServerTemp.ServerActorNum<=numSelectServer)
					{
						tempSelectServer=btnServerTemp;
						numSelectServer=tempSelectServer.ServerActorNum;
					}
					if(strMeberServer==string.Format ("{0}:{1}",btnServerTemp.ApplicationIp,btnServerTemp.ApplicationHost))
					{
						numMeberServer=i;
						tempMeberServer=btnServerTemp;
					}
					listBtnServer.Add (btnServerTemp);
                }
            }
            i++;
        }
       */

        SortServer();

        /*
        gridServer.repositionNow = true;
		

			if(numMeberServer!=-1)
			{
				
				lblSelectedServer.gameObject.SetActiveRecursively(false);
				tempMeberServer.CopyTo(btnSelectedServer);
				if(!isOnlyShow)
				{
			//		tempMeberServer.OnClick();
				}
			}
			else
			{
				if(!string.IsNullOrEmpty (firstServer))
				{
					//string tempServer=string.Empty;
					foreach(BtnServer item in listBtnServer)
					{
						//tempServer=string.Format ("{0}:{1}",item.ApplicationIp,item.ApplicationHost);
						if(item.NumTitle==firstServer)
						{
							lblSelectedServer.gameObject.SetActiveRecursively(true);
							item.CopyTo (tempSelectServer);
							item.CopyTo (btnSelectedServer);
							if(!isOnlyShow)
							{
	//							item.OnClick ();
							}
							return;
						}
					}
				}
				lblSelectedServer.gameObject.SetActiveRecursively(true);
	//			Debug.Log (string.Format ("{0}-----{1}",tempSelectServer,btnSelectedServer));
				if(tempSelectServer!=null)
				{
				tempSelectServer.CopyTo (btnSelectedServer);
					if(!isOnlyShow)
					{
					//tempSelectServer.OnClick();
					}
				}
			}
	*/	
       
    }

    Dictionary<string, List<Dictionary<string, string>>> mDic = new Dictionary<string, List<Dictionary<string, string>>>();
    /// <summary>
    /// 将区服按大区分类
    /// </summary>
    public void SortServer()
    {
        string strMeberServer = PlayerPrefs.GetString("NumTitleS1");
        int num = 0;

        Dictionary<string,string> noServer = null;
        bool isHaveFirstServer = false;
        mDic.Clear();
        foreach (KeyValuePair<short, object> item in dicLogin)
        {
            object getObj = item.Value;
            Dictionary<object, object> getDic = (Dictionary<object, object>)getObj;
            Dictionary<string, string> dicTemp = getDic.DicObjTo<string, string>();           

            if (mDic.ContainsKey(dicTemp["area"]))
            {
                mDic[dicTemp["area"]].Add(dicTemp);
            }
            else
            {
                List<Dictionary<string, string>> mList = new List<Dictionary<string, string>>();
                mList.Add(dicTemp);
                mDic.Add(dicTemp["area"], mList);
            }

            if (num == 0 )
            {
                noServer = dicTemp;
                //SetServerBtn(btnSelectedServer, dicTemp);
            }
            else if (strMeberServer == dicTemp["numTitle"])
            {
                isHaveFirstServer = true;
                SetServerBtn(btnSelectedServer, dicTemp);
                lblSelectedServer.gameObject.SetActive(true);//记住上次登录
                lblSelectedServer.text = StaticLoc.Loc.Get("info1069");// 上次登录
            }
            else if (firstServer == dicTemp["numTitle"])
            {
                isHaveFirstServer = true;
                SetServerBtn(btnSelectedServer, dicTemp);
                lblSelectedServer.gameObject.SetActive(true);//gm已配置第一次推荐
                lblSelectedServer.text = StaticLoc.Loc.Get("info1070");// 推荐登录
            }
            num++;
        }

        if (!isHaveFirstServer && noServer != null)
        {
            SetServerBtn(btnSelectedServer, noServer);
            lblSelectedServer.gameObject.SetActive(true);//默认推荐
            lblSelectedServer.text = StaticLoc.Loc.Get("info1070");// 推荐登录
        }

        ShowAreaServer();
    }

    public GameObject btnServerArea;
    public UIGrid gridServerArea;
    private List<string> listServerArea = new List<string>();
    /// <summary>
    /// 显示大区
    /// </summary>
    void ShowAreaServer()
    {
        foreach (KeyValuePair<string, List<Dictionary<string, string>>> item in mDic)
        {
            if (listServerArea.Contains(item.Key))
            {
                continue;
            }
            GameObject tempObj = (GameObject)GameObject.Instantiate(btnServerArea);
            tempObj.transform.parent = this.gridServerArea.transform;
            tempObj.transform.localPosition = Vector3.zero;
            tempObj.transform.localEulerAngles = Vector3.zero;
            tempObj.transform.localScale = new Vector3(1, 1, 1);
            BtnServerArea btnServerAreaTemp = tempObj.GetComponent<BtnServerArea>();
            btnServerAreaTemp.AreaName = item.Key;
            listServerArea.Add(item.Key);
        }
        this.gridServerArea.repositionNow = true;

        Transform firstTran = this.gridServerArea.GetChild(0);
        firstTran.GetComponent<UIToggle>().value = true;
        string areaName = firstTran.GetComponent<BtnServerArea>().AreaName;
        ShowServer(areaName);
    }


    public List<BtnServer> strBtnServerTemp = new List<BtnServer>();
    /// <summary>
    /// 显示每个区服
    /// </summary>
    void ShowServer(string areaStr)
    {
        List<Dictionary<string, string>> servers = null;
        if(mDic.ContainsKey(areaStr)) 
        {
            servers = mDic[areaStr];
        }
        else
        {
            return;
        }


        bool isOnlyShow = true;
        //BtnServer[] strBtnServerTemp = this.gridServer.transform.GetComponentsInChildren<BtnServer>();
        List<BtnServer> listBtnServer = new List<BtnServer>();

        foreach (BtnServer bs in strBtnServerTemp)
        {
            bs.gameObject.SetActive(false);
        }

        int numSelectServer = 999999;
        BtnServer tempSelectServer = null;
        BtnServer tempMeberServer = null;
        int numMeberServer = -1;
        //string strMeberServer = PlayerPrefs.GetString("InAppServerIP");
        string strMeberServer = PlayerPrefs.GetString("NumTitleS1");
        for (int i = 0; i < servers.Count; i++)
        {
            Dictionary<string, string> dicTemp = servers[i];
            if (i < strBtnServerTemp.Count)
            {
                SetServerBtn(strBtnServerTemp[i], dicTemp);
                if (strBtnServerTemp[i].isTest)
                {
                    continue;
                }
                strBtnServerTemp[i].isFastBtn = false;
                if (strBtnServerTemp[i].ServerActorNum <= numSelectServer)
                {
                    tempSelectServer = strBtnServerTemp[i];
                    numSelectServer = tempSelectServer.ServerActorNum;
                }
                //if (strMeberServer == string.Format("{0}:{1}", strBtnServerTemp[i].ApplicationIp, strBtnServerTemp[i].ApplicationHost))
                if (strMeberServer == strBtnServerTemp[i].NumTitle)
                {
                    numMeberServer = i;
                    tempMeberServer = strBtnServerTemp[i];
                }
                listBtnServer.Add(strBtnServerTemp[i]);
                strBtnServerTemp[i].gameObject.SetActive(true);
            }
            else
            {
                GameObject tempObj = (GameObject)GameObject.Instantiate(this.btnServer);
                tempObj.transform.parent = this.gridServer.transform;
                tempObj.transform.localPosition = Vector3.zero;
                tempObj.transform.localEulerAngles = Vector3.zero;
                tempObj.transform.localScale = new Vector3(1, 1, 1);
                BtnServer btnServerTemp = tempObj.GetComponent<BtnServer>();

                if (btnServerTemp != null)
                {
                    btnServerTemp.mainMenuManage = this;
                    btnServerTemp.isFastBtn = false;
                    SetServerBtn(btnServerTemp, dicTemp);
                    if (btnServerTemp.isTest)
                    {
                        continue;
                    }
                    if (btnServerTemp.ServerActorNum <= numSelectServer)
                    {
                        tempSelectServer = btnServerTemp;
                        numSelectServer = tempSelectServer.ServerActorNum;
                    }
                    //if (strMeberServer == string.Format("{0}:{1}", btnServerTemp.ApplicationIp, btnServerTemp.ApplicationHost))
                    if (strMeberServer == btnServerTemp.NumTitle)
                    {
                        numMeberServer = i;
                        tempMeberServer = btnServerTemp;
                    }
                    listBtnServer.Add(btnServerTemp);
                    strBtnServerTemp.Add(btnServerTemp);
                }
            }
        }

        gridServer.repositionNow = true;

		if (!string.IsNullOrEmpty(firstServer)&&string.IsNullOrEmpty(strMeberServer))
		{
			btnSelectedServer.OnClick ();
		}

        //if (numMeberServer != -1)
        //{

        //    lblSelectedServer.gameObject.SetActiveRecursively(false);
        //    tempMeberServer.CopyTo(btnSelectedServer);
        //    if (!isOnlyShow)
        //    {
        //        //		tempMeberServer.OnClick();
        //    }
        //}
        //else
        //{
        //    if (!string.IsNullOrEmpty(firstServer))
        //    {
        //        //string tempServer=string.Empty;
        //        foreach (BtnServer item in listBtnServer)
        //        {
        //            //tempServer=string.Format ("{0}:{1}",item.ApplicationIp,item.ApplicationHost);
        //            if (item.NumTitle == firstServer)
        //            {
        //                lblSelectedServer.gameObject.SetActiveRecursively(true);
        //                item.CopyTo(tempSelectServer);
        //                item.CopyTo(btnSelectedServer);
        //                if (!isOnlyShow)
        //                {
        //                    //							item.OnClick ();
        //                }
        //                return;
        //            }
        //        }
        //    }
        //    lblSelectedServer.gameObject.SetActiveRecursively(true);
        //    //			Debug.Log (string.Format ("{0}-----{1}",tempSelectServer,btnSelectedServer));
        //    if (tempSelectServer != null)
        //    {
        //        tempSelectServer.CopyTo(btnSelectedServer);
        //        if (!isOnlyShow)
        //        {
        //            //tempSelectServer.OnClick();
        //        }
        //    }
        //}
    }

	public void btnNextStep(){
		btnSelectedServer.OnClick();
	}
	
	public void LoginFastServer()
	{
		YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerFastLogin(SystemInfo.deviceUniqueIdentifier, "DarkSword2", "PlayerInfo");
	}

	public static Dictionary<short, object> dicLogin;
    public static yuan.YuanPhoton.ReturnCode returnLogin;
    public string DebugLogin;
    /// <summary>
    /// 登录
    /// </summary>
    public void Lgoin()
    {
		
        switch (returnLogin)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
			if(BtnManager.isTDlogin){
				BtnManager.isTDlogin = false;
				//TD_info.loginSuccess();//TD统计进入游戏成功
			}
					
                    if (this.txtUserID.text.Trim() != "")
                    {
                        PlayerPrefs.SetString("GameUserID", this.txtUserID.text.Trim());
                        PlayerPrefs.SetString("GameUserPwd", this.txtUserPwd.text.Trim());
						YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().userID=this.txtUserID.text.Trim();

                        if (this.cbxRemeberMe.value)
                        {
                            PlayerPrefs.SetInt("RemeberMe", 1);
                            PlayerPrefs.SetString("UserID", this.txtUserID.text.Trim());
                            PlayerPrefs.SetString("UserPwd", this.txtUserPwd.text.Trim());
                        }
                        else
                        {
                            PlayerPrefs.SetInt("RemeberMe", 0);
                            PlayerPrefs.SetString("UserID", "");
                            PlayerPrefs.SetString("UserPwd", "");
                        }
                    }
					loginType=LoginType.Login;
					//songselect.SendMessage("SongLoad" , "Login-2" , SendMessageOptions.DontRequireReceiver);
                 Application.LoadLevel(1);

                }
                break;
            case yuan.YuanPhoton.ReturnCode.GetServer:
                {
                    //BtnServer[] strBtnServerTemp = this.gridServerBind.transform.GetComponentsInChildren<BtnServer>();
                    //int i = 0;

                    //foreach (KeyValuePair<byte, object> item in dicLogin)
                    //{
                    //    Dictionary<string, string> dicTemp = (Dictionary<string, string>)item.Value;
                    //    if (i < strBtnServerTemp.Length)
                    //    {
                    //        SetServerBtn(strBtnServerTemp[i], dicTemp);
                    //    }
                    //    else
                    //    {
                    //        GameObject tempObj = (GameObject)GameObject.Instantiate(this.btnServerBind);
                    //        tempObj.transform.parent = this.gridServerBind.transform;
                    //        tempObj.transform.localPosition = Vector3.zero;
                    //        tempObj.transform.localEulerAngles = Vector3.zero;
                    //        tempObj.transform.localScale = new Vector3(1, 1, 1);
                    //        BtnServer btnServerTemp = tempObj.GetComponent<BtnServer>();
                    //        btnServerTemp.isFastBtn = false;
                    //        if (btnServerTemp != null)
                    //        {
                    //            SetServerBtn(btnServerTemp, dicTemp);
                    //        }

                    //        UIToggle ckbTemp = tempObj.GetComponent<UIToggle>();
                    //        ckbTemp.radioButtonRoot = gridServerBind.transform;
                    //    }
                    //    i++;
                    //}
                    //this.gridServerBind.repositionNow = true;
                    //Debug.Log("yyyyyyyyyyyyyyyyyyyyyyy");
                    //yuan.YuanClass.SwitchListOnlyOne(listMenu, 8, true, true);
                    listMenu[8].gameObject.SetActiveRecursively(true);

    /*
                    BtnServer[] strBtnServerTemp = this.gridServer.transform.GetComponentsInChildren<BtnServer>();
					BtnServer tempSelectServer=null;
					BtnServer tempMeberServer=null;
					int numSelectServer=999999;
					int numMeberServer=-1;
					string strMeberServer=PlayerPrefs.GetString ("InAppServerIP");
                    int i = 0;

			foreach (KeyValuePair<short, object> item in dicLogin)
                    {
                        Dictionary<string, string> dicTemp = (Dictionary<string, string>)item.Value;
                        if (i < strBtnServerTemp.Length)
                        {
                            SetServerBtn(strBtnServerTemp[i], dicTemp);
							if(strBtnServerTemp[i].isTest)
							{
								continue;
							}
                            strBtnServerTemp[i].mainMenuManage = this;
                            strBtnServerTemp[i].isFastBtn = true;
							if(strBtnServerTemp[i].ServerActorNum<=numSelectServer)
							{
								tempSelectServer=strBtnServerTemp[i];
								numSelectServer=tempSelectServer.ServerActorNum;
							}
							if(strMeberServer==string.Format ("{0}:{1}",strBtnServerTemp[i].ApplicationIp,strBtnServerTemp[i].ApplicationHost))
							{
								numMeberServer=i;
								tempMeberServer=strBtnServerTemp[i];
							}					
                        }
                        else
                        {
                            GameObject tempObj = (GameObject)GameObject.Instantiate(this.btnServer);
                            tempObj.transform.parent = this.gridServer.transform;
                            tempObj.transform.localPosition = Vector3.zero;
                            tempObj.transform.localEulerAngles = Vector3.zero;
                            tempObj.transform.localScale = new Vector3(1, 1, 1);
                            BtnServer btnServerTemp = tempObj.GetComponent<BtnServer>();
                            btnServerTemp.btnManage = btnManage;
                            btnServerTemp.mainMenuManage = this;
                            btnServerTemp.isFastBtn = true;
                            if (btnServerTemp != null)
                            {
                                SetServerBtn(btnServerTemp, dicTemp);
								if(btnServerTemp.isTest)
								{
									continue;
								}
								if(btnServerTemp.ServerActorNum<=numSelectServer)
								{
									tempSelectServer=btnServerTemp;
									numSelectServer=tempSelectServer.ServerActorNum;
								}
								if(strMeberServer==string.Format ("{0}:{1}",btnServerTemp.ApplicationIp,btnServerTemp.ApplicationHost))
								{
									numMeberServer=i;
									tempMeberServer=btnServerTemp;
								}						
                            }
                        }
                        i++;
                    }
                    gridServer.repositionNow = true;
					if(numMeberServer!=-1)
					{
						
						lblSelectedServer.gameObject.SetActiveRecursively(false);
						tempMeberServer.CopyTo(btnSelectedServer);
					}
					else
					{
						
						lblSelectedServer.gameObject.SetActiveRecursively(true);
						tempSelectServer.CopyTo (btnSelectedServer);
					}
                    */

                    SortServer();
                }
                break;
            case yuan.YuanPhoton.ReturnCode.No:
                {
					if(BtnManager.isTDlogin){
					//TD_info.loginFail();//TD统计进入游戏失败
					BtnManager.isTDlogin = false;
					}
                    this.isLogin = false;
                    if (this.lblLoginWarning != null)
                    {
				this.lblLoginWarning.text = StaticLoc.Loc.Get(DebugLogin);
                    }
                    yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Nothing:
                {
					if(BtnManager.isTDlogin){
					//TD_info.loginFail();//TD统计进入游戏失败
					BtnManager.isTDlogin = false;
					}
                    this.isLogin = false;
                    if (this.lblLoginWarning != null)
                    {
					this.lblLoginWarning.text = StaticLoc.Loc.Get(DebugLogin);
                    }
                    yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasID:
                {
                    this.isLogin = false;
			PlayerPrefs.SetString("GameUserID",StaticLoc.Loc.Get(DebugLogin));
			YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().userID=StaticLoc.Loc.Get(DebugLogin);
                }
                break;			
            case yuan.YuanPhoton.ReturnCode.NeedLicense:
                {
                    this.isLogin = false;
					ShowLicense();
			PlayerPrefs.SetString("GameUserID",StaticLoc.Loc.Get(DebugLogin));
			YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().userID=StaticLoc.Loc.Get(DebugLogin);
                }
                break;				
            case yuan.YuanPhoton.ReturnCode.Error:
                {
					//TD_info.loginFail();//TD统计进入游戏失败
                    this.isLogin = false;
			Debug.LogError(StaticLoc.Loc.Get(DebugLogin));
                    //yuan.YuanClass.SwitchListOnlyOne(listMenu, 0, true, true);
                }
                break;
        }
		btnManage.EndTimeOut ();
    }
	
   public	GameObject objLicense;
   public	UIButtonMessage btnLicense;
   public	UIInput txtLicense;
	public UILabel  lblLicenseWarring;
	
	public void ShowLicense()
	{
		TableRead.my.sdklogin.SetActiveRecursively (false);
		objLicense.gameObject.SetActiveRecursively (true);
		btnLicense.target=this.gameObject;
		btnLicense.functionName="SetLicense";
	}
	
	public void SetLicense()
	{
		if(txtLicense.text.Trim ()!="")
		{
			if(!dicLogin.ContainsKey((short)yuan.YuanPhoton.ParameterType.License))
			{
				dicLogin.Add ((short)yuan.YuanPhoton.ParameterType.License,txtLicense.text.Trim ());
			}
			else
			{
				dicLogin[(short)yuan.YuanPhoton.ParameterType.License]=txtLicense.text.Trim ();
			}
			//YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SetLicense (dicLogin);
			StartCoroutine(btnManage.BeginTimeOut (10,2,btnManage.ConnectYuanUnity,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SetLicense(dicLogin),null));
		}
	}
	

 	public bool isSetLicense=false;
    public string DebugSetLicense;
	public void SetNoLicense()
	{
		isSetLicense=false;
		lblLicenseWarring.text=StaticLoc.Loc.Get(DebugSetLicense);
	}
	
	
	
	public bool isLoginUC=false;
	public string idUC=string.Empty;
	public void LoginUC()
	{
		isLoginUC=false;
		  PlayerPrefs.SetString("GameUserID",idUC);
		YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().userID=idUC;
	}
	
    public yuan.YuanPhoton.ReturnCode returnLogon;
    public string DebugLogon;
    /// <summary>
    /// 注册
    /// </summary>
    public void Logon()
    {
		
        switch (returnLogon)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                    this.isLogin = false;
                    if (this.lblLogonWarning != null)
                    {
				this.lblLogonWarning.text = StaticLoc.Loc.Get(DebugLogon);
                    }
					txtUserID.text=btnManage.txtLogonName.text;
					txtUserPwd.text=btnManage.txtLogonPwd.text;
					btnManage.ckbRemeberMe.value=true;
					btnManage.PlayerLogin ();
			
                    //Application.LoadLevel(0);
                }
                break;
            case yuan.YuanPhoton.ReturnCode.No:
                {
                    this.isLogin = false;
                    if (this.lblLogonWarning != null)
                    {
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugLogon));
                        //this.lblLogonWarning.text = DebugLogon;
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasID:
                {
                    this.isLogin = false;
                    if (this.lblLogonWarning != null)
                    {
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugLogon));
                        //this.lblLogonWarning.text = DebugLogon;
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasNickName:
                {
                    this.isLogin = false;
                    if (this.lblLogonWarning != null)
                    {
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugLogon));
                        //this.lblLogonWarning.text = DebugLogon;
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasEmail:
                {
                    this.isLogin = false;
                    if (this.lblLogonWarning != null)
                    {
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugLogon));
                        //this.lblLogonWarning.text = DebugLogon;
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
                    this.isLogin = false;
			Debug.LogError(StaticLoc.Loc.Get(DebugLogon));
                }
                break;
        }
		btnManage.EndTimeOut ();
    }

	public yuan.YuanMemoryDB.YuanTable getPlayersYT=new YuanTable("GetPlayers","");
	public Dictionary<short, object> slotNumDic;
    public yuan.YuanPhoton.ReturnCode returrnGetPlayers;
	public GameObject songSelectPlayer;
    public string DebugGetPlayers;
	public Transform playerListGrid;
	public Transform playerItemPrefab;
	public BoxCollider btnDelete;
	public BoxCollider buttonStartGame;
	public GameObject songPlayerSelect;
    /// <summary>
    /// 获取玩家
    /// </summary>
	public void GetPlayers()
	{
		try
		{
			listMenu[2].transform.localScale = new Vector3(1, 1, 1);
			BtnServer.outTime=100;
			
			switch (returrnGetPlayers)
			{
			case yuan.YuanPhoton.ReturnCode.Yes:
			{
				if(BtnManager.isTDselet){
					//TD_info.selectSuccess(PlayerPrefs.GetString("InAppServerName", "NON"));//TD统计选择区服成功
					BtnManager.isTDselet = false;
				}
				yuan.YuanClass.SwitchListOnlyOne(listMenu, 2, true, true);
				int itemCount = 0;

				if(null != getPlayersYT)
				{
					itemCount = getPlayersYT.Rows.Count;
				}

                PlayerPrefs.SetInt("PlayerSlotCount", itemCount); // 缓存角色栏位数量，防止ios内购时，把钱扣了，但是不能购买栏位了
				
				if(itemCount >= playerListGrid.childCount)
				{
					//int addCount = itemCount - 5;
                    int addCount = itemCount - playerListGrid.childCount;
					while(addCount >= 0){
						Transform playerItem = Instantiate(playerItemPrefab) as Transform;
						playerItem.localPosition = Vector3.one;
						playerItem.parent = playerListGrid;
						playerItem.localScale = Vector3.one;
						playerListGrid.GetComponent<UIGrid>().repositionNow = true;
						BtnPlayer btnPlayer = playerItem.GetComponent<BtnPlayer>();
						listPlayerBtn.Add(btnPlayer);
						addCount --;
					}
				}
				
                //foreach (BtnPlayer player in listPlayerBtn)
                //{
                //    if(player.gameObject.active)
                //    {
                //        player.gameObject.SetActiveRecursively(false);
                //    }
                //}

                for (int k = listPlayerBtn.Count - 1; k >= 0; k--)
                {
                    BtnPlayer player = listPlayerBtn[k];
                    if (player.gameObject.activeSelf)
                    {
                        player.gameObject.SetActive(false);
                    }
                }

				btnSelectPlayerBind.gameObject.SetActiveRecursively(false);
				int i = 0;
				foreach (yuan.YuanMemoryDB.YuanRow yr in getPlayersYT.Rows)
				{
                    //if(i > maxPlayerNum)
                    //{
                    //    i--;
                    //    break;
                    //}

					if (listPlayerBtn.Count > i)
					{
						// listPlayerBtn[i].text = yr["PlayerName"].YuanColumnText.Trim();
						
						listPlayerBtn[i].gameObject.SetActiveRecursively(true);
                        listPlayerBtn[i].HideSlotNumObj();
						listPlayerBtn[i].btnType = BtnPlayer.BtnType.Read;
                        listPlayerBtn[i].EnableToggle(true);
                        if (i > 1)
                        {
                            for (int j = listPlayerBtn.Count - 1; j >= 0; j--)
                            {

                                if (j != listPlayerBtn.Count - 1)
                                {
                                    listPlayerBtn[j].checkbox.value = false;
                                }
                                else
                                {
                                    listPlayerBtn[j].checkbox.value = true;
                                }
                            }
                        }
						listPlayerBtn[i].lblNew.gameObject.active = false;
						listPlayerBtn[i].lblPlayerName.text = yr["PlayerName"].YuanColumnText.Trim();
						listPlayerBtn[i].lblLevelNum.text = yr["PlayerLevel"].YuanColumnText.Trim();
						listPlayerBtn[i].lblPro.text = GetPro(yr["ProID"].YuanColumnText.Trim());
						YuanRow yrPlace = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID", yr["Place"].YuanColumnText == "" ? "111" : yr["Place"].YuanColumnText);

						if(yrPlace==null)
						{
							
							yrPlace = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID", "111");
						}
						if(yrPlace!=null)
						{
							listPlayerBtn[i].lblAear.text = yrPlace["MapName"].YuanColumnText;
						}
						listPlayerBtn[i].yuanRow = yr;
						
						//if (i == 0)
						//{
						//    listPlayerBtn[i].OnClick();
						//                              listPlayerBtn[i].GetComponent<UIToggle>().value = true;
						//}
					}
					i++;
				}
				listPlayerBtn[i-1].OnClick();
				listPlayerBtn[i-1].GetComponent<UIToggle>().value = true;
				
                //if (itemCount < maxPlayerNum)
                //{
					listPlayerBtn[itemCount].gameObject.SetActiveRecursively(true);
//				Debug.Log("wei----------------------------" + slotNumDic[(short)yuan.YuanPhoton.lanweiNumber.itemID]);
				int slotNum = (int)slotNumDic[(short)yuan.YuanPhoton.lanweiNumber.itemID];
                    //if(itemCount < freePlayerNum + buyPlayerNum)
                    if (itemCount < slotNum)
					{
						listPlayerBtn[itemCount].lblNew.text = StaticLoc.Loc.Get("buttons603");
                        listPlayerBtn[itemCount].SetSlotNum(itemCount, slotNum);
                        listPlayerBtn[itemCount].EnableToggle(false);
						listPlayerBtn[itemCount].btnType = BtnPlayer.BtnType.New;
					}
					else
					{
						listPlayerBtn[itemCount].lblNew.text = StaticLoc.Loc.Get("info729");
                        listPlayerBtn[itemCount].HideSlotNumObj();
                        listPlayerBtn[itemCount].EnableToggle(false);
						listPlayerBtn[itemCount].btnType = BtnPlayer.BtnType.Buy;
					}
					
					listPlayerBtn[itemCount].lblAear.gameObject.active = false;
					listPlayerBtn[itemCount].lblLevelNum.gameObject.active = false;
					listPlayerBtn[itemCount].lblLevel.gameObject.active = false;
					listPlayerBtn[itemCount].lblPlayerName.gameObject.active = false;
					listPlayerBtn[itemCount].lblPro.gameObject.active = false;
                //}
				//btnDeletePlayer.invokMethodName = "DeletePlayerBtnClick";
				
				btnDelete.enabled = true;
				buttonStartGame.enabled = true;
                //StartCoroutine (SetPlayerList (itemCount));
			}
				break;
			case yuan.YuanPhoton.ReturnCode.Nothing:
			{
				if(BtnManager.isTDselet){
					//TD_info.selectFail(PlayerPrefs.GetString("InAppServerName", "NON"));//TD统计选择服务器失败
					BtnManager.isTDselet = false;
				}
				yuan.YuanClass.SwitchListOnlyOne(listMenu, 2, true, true);
				songSelectPlayer.SendMessage ("PlayerClear",SendMessageOptions.DontRequireReceiver);
				foreach (BtnPlayer player in listPlayerBtn)
				{
					player.lblNew.text = StaticLoc.Loc.Get("buttons603");
                    player.SetSlotNum(2, 2);
					player.btnType=BtnPlayer.BtnType.New;
					player.gameObject.SetActiveRecursively(false);
				}
				btnSelectPlayerBind.gameObject.SetActiveRecursively(false);
				listPlayerBtn[0].gameObject.SetActiveRecursively(true);
				
				listPlayerBtn[0].lblNew.text = StaticLoc.Loc.Get("buttons603");
				listPlayerBtn[0].btnType = BtnPlayer.BtnType.New;
				
				listPlayerBtn[0].lblAear.gameObject.active = false;
				listPlayerBtn[0].lblLevelNum.gameObject.active = false;
				listPlayerBtn[0].lblLevel.gameObject.active = false;
				listPlayerBtn[0].lblPlayerName.gameObject.active = false;
				listPlayerBtn[0].lblPro.gameObject.active = false;
				
				btnDelete.enabled = false;
				buttonStartGame.enabled = false;
				songPlayerSelect.SendMessage("PlayerClear", SendMessageOptions.RequireReceiver);
			}
				break;
			case yuan.YuanPhoton.ReturnCode.PlayerNumMax:
			{
                int timetime = int.Parse(DebugGetPlayers) * 40/60;
                if (timetime < 1)
                {
                    timetime = 1;
                }
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info718") + "[ffff00]" + DebugGetPlayers + "[-]" + StaticLoc.Loc.Get("info1048") + "[ffff00]" + timetime + "[-]" + StaticLoc.Loc.Get("info1049"));

				warnings.warningAllEnter.btnEnterEvent.SetEvent ((object sender,object prams)=>{btnManage.SelectServer();});
//				warnings.warningAllEnter.btnEnter.target = btnManage.gameObject;
//				warnings.warningAllEnter.btnEnter.functionName = "SelectServer";
	
//				mainMenuManage.warnings.warningAllEnter.btnEnter.functionName = "";
				yuan.YuanClass.SwitchListOnlyOne (listMenu,8,true,true);
			//	InRoom.GetInRoomInstantiate().peer.Disconnect ();
			}
				break;
			case yuan.YuanPhoton.ReturnCode.Error:
			{
				if(BtnManager.isTDselet){
					//TD_info.selectFail(PlayerPrefs.GetString("InAppServerName", "NON"));//TD统计选择服务器失败
					BtnManager.isTDselet = false;
				}
				Debug.LogError(StaticLoc.Loc.Get(DebugGetPlayers));
			}
				break;
			}
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
	}

	public IEnumerator SetPlayerList(int mNum)
	{
		listPlayerBtn[mNum].GetComponent<UIToggle>().value=true;
		yield return new WaitForEndOfFrame();
		listPlayerBtn[mNum-1].GetComponent<UIToggle>().value=true;
	}
	
	public void GoMenu(){
		btnManage.PlayerStartIn();
	}
    public yuan.YuanPhoton.ReturnCode returnPlayerCreat;
	public static Dictionary<short, object> dicPlayerCreat;
    public string DebugPlayerCreat;
    public BtnGameStart btnStartGame;
    public GameObject txtInputName;
	public GameObject btnRandomName;
    /// <summary>
    /// 新建角色
    /// </summary>
    /// 
    public void PlayerCreat()
    {
        switch (returnPlayerCreat)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                    if (lblCreatWarning != null)
                    {
                        //lblCreatWarning.text = DebugPlayerCreat;
				warnings.warningAllTime.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugPlayerCreat));
                        YuanTable ytTemp = new YuanTable("", "");
                        ytTemp.CopyToDictionaryAndParms(dicPlayerCreat);
					string proId = (string)ytTemp.mParms[(byte)yuan.YuanPhoton.ParameterType.PlayerType];
 					//TD_info.creatSuccess(string.Format("{0};{1}", proId,PlayerPrefs.GetString("InAppServerName", "NON")));//TD统计创建角色成功
                        //Debug.Log("--------------------玩家:" + ytTemp.Rows[0]["ProID"].YuanColumnText);
						if(ytTemp.Count>0)
						{
	                        btnStartGame.yr = ytTemp.Rows[0];
	                        btnStartGame.gameObject.SetActiveRecursively(true);
	                        txtInputName.SetActiveRecursively(false);
							btnRandomName.SetActiveRecursively (false);
						}
				
						//Invoke("GoMenu" , 0.5f);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                {
                    if (lblCreatWarning != null)
                    {
                        //lblCreatWarning.text = DebugPlayerCreat;
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugPlayerCreat));
					//TD_info.creatFail(DebugPlayerCreat);//TD统计创建角色失败
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasNickName:
                {
                    if (lblCreatWarning != null)
                    {
                        //lblCreatWarning.text = DebugPlayerCreat;
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get(DebugPlayerCreat));
				//TD_info.creatFail(StaticLoc.Loc.Get(DebugPlayerCreat));//TD统计创建角色失败
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
			Debug.LogError(StaticLoc.Loc.Get(DebugPlayerCreat));
			//TD_info.creatFail(StaticLoc.Loc.Get(DebugPlayerCreat));//TD统计创建角色失败
                }
                break;
        }
    }

    public yuan.YuanPhoton.ReturnCode returnPwdUpdate;
    public string DebugPwdUpdate;
    /// <summary>
    /// 密码修改
    /// </summary>
    public void PwdUpdate()
    {
        switch (returnPwdUpdate)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                    if (this.lblUpdatePwdWarning != null)
                    {
				this.lblUpdatePwdWarning.text = StaticLoc.Loc.Get(DebugPwdUpdate);
                        btnManage.PlayerStartIn();
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.No:
                {
                    if (this.lblUpdatePwdWarning != null)
                    {
				this.lblUpdatePwdWarning.text = StaticLoc.Loc.Get(DebugPwdUpdate);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
				Debug.LogError(StaticLoc.Loc.Get(DebugPwdUpdate));
                }
                break;
        }
        
    }

    public yuan.YuanPhoton.ReturnCode returnDeletePlayer;
    public string DebugDeletePlayer;
    public bool isFast;
    /// <summary>
    /// 删除
    /// </summary>
    public void DeletePlayer()
    {
        listMenu[2].transform.localScale = new Vector3(1, 1, 1);
        switch (returnDeletePlayer)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
			lblPlayerDeleteWarning.text = StaticLoc.Loc.Get(DebugDeletePlayer);
                    if (isFast)
                    {
                        btnManage.PlayerFastLogin();
                    }
                    else
                    {
                        btnManage.PlayerStartIn();
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Nothing:
                {
			lblPlayerDeleteWarning.text = StaticLoc.Loc.Get(DebugDeletePlayer);
                }
                break;
			case yuan.YuanPhoton.ReturnCode.No:
			{
			warnings.warningAllEnter.Show(StaticLoc.Loc.Get ("info358"),StaticLoc.Loc.Get ("info967"));
			}
			break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
			Debug.LogError(StaticLoc.Loc.Get(DebugDeletePlayer));
                }
                break;
        }
    }

    public yuan.YuanPhoton.ReturnCode returnGetPwd;
    public string DebugGetPwd;
    /// <summary>
    /// 找回密码
    /// </summary>
    public void GetPwd()
    {
        switch (returnGetPwd)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                    if (this.lblGetPwdWarning != null)
                    {
				this.lblGetPwdWarning.text = StaticLoc.Loc.Get(DebugGetPwd);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.No:
                {
                    if (this.lblGetPwdWarning != null)
                    {
				this.lblGetPwdWarning.text = StaticLoc.Loc.Get(DebugGetPwd);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Nothing:
                {
                    if (this.lblGetPwdWarning != null)
                    {
				this.lblGetPwdWarning.text = StaticLoc.Loc.Get(DebugGetPwd);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
			Debug.LogError(StaticLoc.Loc.Get(DebugGetPwd));
                }
                break;
        }
		btnManage.EndTimeOut ();
    }

	public Dictionary<short, object> dicFastLogin;
    public yuan.YuanPhoton.ReturnCode returnFastLogin;
    public string DebugFastLogin;
    public GameObject btnSelectServerSelect;
    public GameObject btnSelectServerCreate;
    public BtnClick btnDeletePlayer;
    /// <summary>
    /// 快速登录
    /// </summary>
    public void FastLogin()
    {
        switch (returnFastLogin)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                   // btnManage.background.gameObject.active = true;
                    YuanTable yt = new YuanTable("PlayerInfo", "PlayerID");
                    yt.CopyToDictionaryAndParms(dicFastLogin);
                    this.SwitchListOnlyOne(this.listMenu, 2, true, true);
                    listMenu[2].transform.localScale = new Vector3(1, 1, 1);
                    foreach (BtnPlayer lbl in this.listPlayerBtn)
                    {
                        this.YuanSetActiveRecursively(lbl.transform.gameObject, false);
                    }
                    //this.listPlayerBtn[4].text = yt.Rows[0]["PlayerName"].YuanColumnText.Trim();
                    this.YuanSetActiveRecursively(this.listPlayerBtn[0].transform.gameObject, true);
                    listPlayerBtn[0].lblNew.gameObject.active = false;
                    listPlayerBtn[0].lblPlayerName.text = yt[0]["PlayerName"].YuanColumnText.Trim();
                    listPlayerBtn[0].lblLevelNum.text = yt[0]["PlayerLevel"].YuanColumnText.Trim();
                    listPlayerBtn[0].lblPro.text = GetPro(yt[0]["ProID"].YuanColumnText.Trim());
                    listPlayerBtn[0].yuanRow = yt[0];
                    listPlayerBtn[0].btnType = BtnPlayer.BtnType.Read;

                    this.YuanSetActiveRecursively(this.btnSelectPlayerPwd, false);
                    //this.btnSelectPlayerBack.invokMethodName = "Back";
                    this.YuanSetActiveRecursively(this.btnSelectPlayerBind.gameObject, true);
                    this.btnSelectPlayerBind.invokMethodName = "BindUserIDMenu";
                    this.YuanSetActiveRecursively(this.btnSelectServerSelect, false);
                    btnDeletePlayer.invokMethodName = "DeleteFastPlayerBtnClick";
					
                    this.isLogin = false;
                }
                break;
            case yuan.YuanPhoton.ReturnCode.No:
                {
                    //string deviceID = operationResponse.Parameters[(byte)yuan.YuanPhoton.ParameterType.DeviceID].ToString();
                   // btnManage.background.gameObject.active = false;
                   // yuan.YuanClass.SwitchListOnlyOne(listMenu, 4, true,true);
                    YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin("zealm", "", "", "", false);
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasRegister:
                {
                    this.isLogin = false;
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
                    this.isLogin = false;
			Debug.LogError(StaticLoc.Loc.Get(DebugFastLogin));
                }
                break;
        }
    }


    public yuan.YuanPhoton.ReturnCode returnFastLogon;
    public string DebugFastLogon;
    /// <summary>
    /// 快速注册
    /// </summary>
    public void FastLogon()
    {
        switch (returnFastLogon)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                    this.isLogin = false;
                    if (this.lblCreatWarning != null)
                    {
				this.lblCreatWarning.text = StaticLoc.Loc.Get(DebugFastLogon);
                        btnManage.PlayerFastLogin();
                        btnManage.lblDeletePlayerWarning.text = "";
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasDevice:
                {
                    if (this.lblCreatWarning != null)
                    {
				this.lblCreatWarning.text = StaticLoc.Loc.Get(DebugFastLogon);
                    }
                    this.isLogin = false;
                }
                break;
            case yuan.YuanPhoton.ReturnCode.HasNickName:
                {
                    if (this.lblCreatWarning != null)
                    {
				this.lblCreatWarning.text = StaticLoc.Loc.Get(DebugFastLogon);
                    }
                    this.isLogin = false;
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
                    this.isLogin = false;
			Debug.LogError(StaticLoc.Loc.Get(DebugFastLogon));
                }
                break;
        }
    }

    public yuan.YuanPhoton.ReturnCode returnBindID;
    public string DebugBindID;
   /// <summary>
   /// 绑定账号
   /// </summary>
    public void BindID()
    {
        switch (returnBindID)
        {
            case yuan.YuanPhoton.ReturnCode.Yes:
                {
                    if (this.lblPlayerBindWarning != null)
                    {
				this.lblPlayerBindWarning.text = StaticLoc.Loc.Get(DebugBindID);
                        btnManage.Back();
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.No:
                {
                    if (this.lblPlayerBindWarning != null)
                    {
				this.lblPlayerBindWarning.text = StaticLoc.Loc.Get(DebugBindID);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.PlayerNumMax:
                {
                    if (this.lblPlayerBindWarning != null)
                    {
				this.lblPlayerBindWarning.text = StaticLoc.Loc.Get(DebugBindID);
                    }
                }
                break;
            case yuan.YuanPhoton.ReturnCode.Error:
                {
			Debug.LogError(StaticLoc.Loc.Get(DebugBindID));
                }
                break;
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
				        PhotonHandler.ShowLog("SetServerBtn:"+mBtnServer.ApplicationIp);
                    }
                    break;
                case "host":
                    {
                        mBtnServer.ApplicationHost = item.Value;
				        PhotonHandler.ShowLog("SetServerBtn,port:"+mBtnServer.ApplicationHost);
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
        				//int playerMaxNum=(int) YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerMaxNum];
                        int playerMaxNum = (int)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.PlayerMaxNum];
						mBtnServer.ServerActorNum=item.Value.Parse(0);
						mBtnServer.lblServerState.text = item.Value.Parse(0)>(playerMaxNum/3*2)?"[ff0000]"+StaticLoc.Loc.Get("info648"):"[00ff00]"+StaticLoc.Loc.Get("info647");
                    }
                    break;
                case "tcp":
                    {
                        string t_strValue = item.Value; 
                        if(t_strValue=="1")
                        {
                            PhotonHandler.SetUpdMode();
                        }
						mBtnServer.tcp=item.Value;

                    }
                    break;
                case "rm":
                    {
                        string t_strValue = item.Value;
                        PhotonHandler.SetLogicAddr(t_strValue);
						mBtnServer.RoomIP=item.Value;
                    }
                    break;
                case "rmtcp":
                    {
						PhotonHandler.ShowLog ("-------------------BtnServer:"+item.Value);
                   		mBtnServer.rmtcp=item.Value;		
                    }
                    break;	
                case "rmMaxPlayer":
                    {
                   		mBtnServer.rmMaxPlayer=item.Value.Parse (25);		
                    }
                    break;	
			case "numTitle":
				{
					mBtnServer.NumTitle=item.Value;
				}
				break;
			case "test":
				{
			//	if((new System.Version(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameVersion].ToString())) >= YuanUnityPhoton.GameVersion)
                    if ((new System.Version(YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.GameVersion].ToString())) >= YuanUnityPhoton.GameVersion)
					{
						mBtnServer.gameObject.SetActiveRecursively (false);
						mBtnServer.isTest=true;
						return;
					}	
					
				}   
				break;
            }
        }
    }

    public UILabel lblSystemInfo;
    /// <summary>
    /// 获取服务器相关信息
    /// </summary>
    public void GetServerInfo()
    {
		
       // lblSystemInfo.text = (string)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.SystemInfo];
    }
	

    public void Connect(string mAppName,string mAppIP,string mAppHost,bool mIsFastBtn,string mNickName)
    {
        StartCoroutine(StartConnect(mAppName, mAppIP, mAppHost, mIsFastBtn, mNickName));
    }
	
	private IEnumerator StartConnect(string mAppName,string mAppIP,string mAppHost,bool mIsFastBtn,string mNickName)
    {
        if (mIsFastBtn)
        {
            yuan.YuanClass.SwitchListOnlyOne(listMenu, 4, true, true);
            listMenu[4].transform.localScale = new Vector3(1, 1, 1);
            btnCreatPlayerBack.invokMethodName = "Back";
            btnCreatPlayerEnter.invokMethodName = "PlayerFastLogon";
            YuanSetActiveRecursively(btnSelectServerCreate, false);
            btnManage.AnimCamera.CrossFade("CameraToNewPlayer");
            btnManage.cameraStatus = BtnManager.CameraStatus.NewPlayer;
            isLogin = false;
            PlayerPrefs.SetString("InFastServer", mAppName);
			btnStartGame.gameObject.SetActiveRecursively (false);
        }
        else
        {
//	if(InRoom.GetInRoomInstantiate().ServerConnected)
//	{
//		InRoom.GetInRoomInstantiate ().peer.Disconnect ();
//	}
//	
  //     if (!InRoom.GetInRoomInstantiate().ServerConnected)
  //     {
  //         InRoom.NewInRoomInstantiate().ServerAddress = mAppIP + ":" + mAppHost;
  //         InRoom.GetInRoomInstantiate().ServerApplication = mAppName;
  //         InRoom.GetInRoomInstantiate().Connect();
  //     }
  //     while (!InRoom.GetInRoomInstantiate().ServerConnected)
  //     {
  //         yield return new WaitForSeconds(0.1f);
  //     }
//	numTimeOut=0;
//	InvokeRepeating ("TimeOut",1,1);
//	YuanUnityPhoton.YuanDispose ();
  //     yuan.YuanClass.SwitchListOnlyOne(listMenu, 9, true, true);
  //     listMenu[9].SetActiveRecursively(true);
//	btnManage.objLoading.SetActiveRecursively (true);
  //   	InRoom.GetInRoomInstantiate ().GetPlayers(PlayerPrefs.GetString ("GameUserID"), mAppName, "DarkSword2", "PlayerInfo");
  //     PlayerPrefs.SetString("InAppServer", mAppName);
  //     PlayerPrefs.SetString("InAppServerIP", mAppIP+ ":" + mAppHost);

			ZealmConnector.closeConnection();
		if(ZealmConnector.connection==null)
		{
			InRoom.GetInRoomInstantiate ().peer.Disconnect ();
		}
	
		if (ZealmConnector.connection==null)
	    {
	        InRoom.NewInRoomInstantiate().SetAddress(mAppIP+":"+mAppHost) ;
	        InRoom.GetInRoomInstantiate().ServerApplication = mAppName;
	        InRoom.GetInRoomInstantiate().Connect();
	    }
		int tempNum=0;
	    while (true)
	    {
				yield return new WaitForSeconds(0.05f);
				tempNum++;
				if(InRoom.GetInRoomInstantiate().ServerConnected||tempNum>=40)
				{
					 PlayerPrefs.SetString("InAppServer", mAppName);
  				    PlayerPrefs.SetString("InAppServerIP", InRoom.GetInRoomInstantiate().GetSvrAddress());
					PlayerPrefs.SetString ("InAppServerName",mNickName);
					break;
				}
	    }
		 yield return	StartCoroutine (btnManage.BeginTimeOut (10,2,BtnManager.my.ConnectInRoom,()=>InRoom.GetInRoomInstantiate ().GetPlayers(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().userID, mAppName, "DarkSword2", "PlayerInfo"),OpenSelectServer));
       
		}
    }


	
	private void OpenSelectServer()
	{
		listMenu[8].transform.localScale=Vector3.one;
		listMenu[8].SetActiveRecursively (false);
		listMenu[8].SetActiveRecursively (true);
		 UIPanel myPanle=gridServer.transform.parent.GetComponent<UIPanel>();
		
	}
	
	private int numTimeOut=0;
	private void TimeOut()
	{
		numTimeOut++;
		if(numTimeOut>=15)
		{
			yuan.YuanClass.SwitchListOnlyOne(listMenu, 8, true, true);
			listMenu[8].SetActiveRecursively(true);
			btnManage.objLoading.SetActiveRecursively (false);
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get ("info358"),StaticLoc.Loc.Get ("info720"));
			CancelInvoke ("TimeOut");
		}
		
	}

    public string strMaster;
    public string strRobber;
    public string strSoldier;
    private string GetPro(string proID)
    {
        string pro = string.Empty;
        switch (proID)
        {
            case "1":
                {
                    pro =StaticLoc.Loc.Get(strSoldier) ;
                }
                break;
            case "2":
                {
                    pro = StaticLoc.Loc.Get(strRobber);
                }
                break;
            case "3":
                {
                    pro = StaticLoc.Loc.Get(strMaster);
                }
                break;
        }
        return pro;
    }
	public GameObject songselect;
}
