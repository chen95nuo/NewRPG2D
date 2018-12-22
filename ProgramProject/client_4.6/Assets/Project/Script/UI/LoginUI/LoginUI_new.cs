using UnityEngine;
using System.Collections.Generic;

public class LoginUI_new : BWUIPanel,ProcessResponse {
	
	public static LoginUI_new mInstance;
	
	public GameObject panelLogin;
	public UILabel loginDes;
	public GameObject btn_change;
	
	public GameObject regLogin;
	public GameObject login;
	public GameObject reg;
	
	public GameObject serverLogin;
	public GameObject chooseServer;
	public GameObject defaultServer;
	
	public UIInput uidInput;
	public UIInput psdInput;
	
	//public GameObject icon;//众神之光图标,v1.2.0图标放到了背景上//
//	public UISprite splashSpr;
	
	private bool newPlayer;
	//==本地记录数据==//
	public int lastServerId;
	public string username;
	public string password;
	public string nickname;
	//==元素格式:id-name-ip-port-type-state==//
	public List<string> servers;
	
	private string uidTemp;
	private string psdTemp;
	
	private GameObject serverCell;
	//==延迟登录==//
	private bool delayLogin;
	private float time;
	
	private bool receiveData;
	private int requestType;
	private int errorCode;
	//==0正常登陆,1渠道登陆==//
	private int loginType;
	
	UserResultJson userResJson;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
	}
	
	// Use this for initialization
	void Start () {
		init();
		hide();
	
	}
	public void CleanData()
	{
		gc();
	}
	private void gc()
	{
		serverCell=null;
		if(servers!= null)
		{
			servers.Clear();
		}
		servers=null;
		
//		splashSpr.spriteName = null;
//		splashSpr.atlas = null;
//		splashSpr = null;
		
		GameObject.Destroy(_MyObj);
		_MyObj = null;
		mInstance = null;
		Resources.UnloadUnusedAssets();
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			switch(requestType)
			{
			case 1:
				if(errorCode!=0)
				{
					ToastWindow.mInstance.showText(TextsData.getData(121).chinese);
				}
				else
				{
					uidInput.value=uidTemp;
					psdInput.value=psdTemp;
					onClickLogin(1);
				}
				break;
			case 2:
				if(errorCode==1)
				{
					ToastWindow.mInstance.showText(TextsData.getData(322).chinese);
					showPanel(1);
				}
				else if(errorCode==-4)
				{
					ToastWindow.mInstance.showText(TextsData.getData(125).chinese.Replace("name",username));
					showPanel(2);
				}
				else if(errorCode==-5)
				{
					ToastWindow.mInstance.showText(TextsData.getData(126).chinese.Replace("name",username));
					showPanel(2);
				}
				else if(errorCode==0)
				{
					PlayerElement pe=PlayerInfo.getInstance().player;
					//==gc登录统计==//
					if(SDKManager.getInstance().isSDKGCUsing())
					{
						if(Application.platform==RuntimePlatform.Android)
						{
							SDK_GCStubManager.sdk_startGameServerLogin(PlayerPrefs.GetInt("lastServerId")+"");
							SDK_GCStubManager.sdk_showFloatingView();
							
							//==UC自己的扩展数据==//
							if(TalkingDataManager.channelId.Equals("71"))
							{
								ExtendDataForUC ed=new ExtendDataForUC(pe.id+"",pe.name,pe.level+"",PlayerPrefs.GetInt("lastServerId"),PlayerPrefs.GetString("lastServerName"));
								SDK_GCStubManager.sdk_submitExtendData("loginGameRole",JsonMapper.ToJson(ed));
							}
							else
							{
								ExtendData ed=new ExtendData(pe.id+"",pe.name,pe.level+"",PlayerPrefs.GetInt("lastServerId"),PlayerPrefs.GetString("lastServerName"),pe.crystal+"","",pe.vipLevel+"");
								SDK_GCStubManager.sdk_submitExtendData("loginGameRole",JsonMapper.ToJson(ed));
							}
						}
					}
					if(pe.owncards!=null)
					{
						UniteSkillInfo.cardUnlockTable.Clear();
						string[] ss = pe.owncards.Split('&');
						for(int i = 0;i<ss.Length;i++)
						{
							int id = StringUtil.getInt(ss[i]);
							UniteSkillInfo.cardUnlockTable.Add(id,true);
						}
					}
					int newPlayerType = PlayerInfo.getInstance().player.newPlayerType;
					GuideManager.getInstance().finishGuide(newPlayerType);
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
					{
						requestType = 3;
						PlayerInfo.getInstance().sendRequest(new BattleJson(0,0),this);
					}
					else
					{
						//发送唯一id,等级,登入区服,账号类型,//
						string username = PlayerPrefs.GetString("username");
						if(!TalkingDataManager.isTDPC)
						{
							TDGAAccount account = TDGAAccount.SetAccount(username);
							account.SetLevel(pe.level);
							//account.SetAge(pe);
							account.SetGameServer(PlayerPrefs.GetInt("lastServerId").ToString());
							
							if(TalkingDataManager.channelId.Equals("normal"))
							{
								account.SetAccountType(AccountType.REGISTERED);
							}
							else if(TalkingDataManager.channelId.Equals("91"))
							{
								account.SetAccountType(AccountType.ND91);
							}
							else if(TalkingDataManager.channelId.Equals("604"))
							{
								account.SetAccountType(AccountType.TYPE1);//同步推//
							}
						}
						gc();
						
						GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
					}
					PlayerInfo.isFirstLogin = true;
					Debug.Log("44444444444444444444444444444444444");
				}
				break;
				
			case 3:
				if(errorCode == 0)
				{
					EffectPreLoad epl = GameObject.Find("EffectPreLoad").GetComponent<EffectPreLoad>();
					if(epl != null)
					{
						epl.startWaitForLoad();
					}
					Invoke("changeSceneToGuideBattle",1f);
					
				}
				break;
			case 4:
				username = "KY_" + userResJson.userId;
            	password = userResJson.userId;
            	writePlayerPrefs();

            	requestType = 2;
            	string json = JsonMapper.ToJson(new LoginJson(username, "", 0, Constant.OS_IOS, userResJson.userId));
            	RequestSender.getInstance().clearCookie();
            	RequestSender.getInstance().request(1, json, true, this);
				break;
			}
		}
		if(delayLogin)
		{
			if(time>0)
			{
				if(time>=1f)
				{
					loginDes.text=TextsData.getData(25).chinese.Replace("a",uidTemp);
				}
				else
				{
					loginDes.text=TextsData.getData(124).chinese.Replace("name",username)+"...";
				}
				time-=Time.deltaTime;
			}
			else
			{
				time=0;
				delayLogin=false;
				loginServer();
			}
		}
	}
	
	public void changeSceneToGuideBattle()
	{
		GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
	}
	
	public override void show()
	{
		base.show();
		
		//播放音乐//
		string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_SPLASH).music;
		MusicManager.playBgMusic(musicName);
		
		if(!haveTheServerId(lastServerId))
		{
			string[] ss=servers[servers.Count-1].Split('-');
			lastServerId=StringUtil.getInt(ss[0]);
			newPlayer=true;
		}
		showPanel(1);
	}
	
	private bool haveTheServerId(int theServerId)
	{
		for(int k=0;k<servers.Count;k++)
		{
			string[] ss=servers[k].Split('-');
			int serverId=StringUtil.getInt(ss[0]);
			if(lastServerId==serverId)
			{
				return true;
			}
		}
		return false;
	}
	
	private void drawChooseServer()
	{
		GameObject serverParent=chooseServer.transform.FindChild("servers/server-parent").gameObject;
		GameObjectUtil.destroyGameObjectAllChildrens(serverParent);
		//==上次登录==//
		List<GameObject> serverList1=new List<GameObject>();
		//==推荐==//
		List<GameObject> serverList2=new List<GameObject>();
		//==新服==//
		List<GameObject> serverList3=new List<GameObject>();
		//==其他==//
		List<GameObject> serverList4=new List<GameObject>();
		
		for(int k=0;k<servers.Count;k++)
		{
			string serverInfo=servers[k];
			string[] ss=serverInfo.Split('-');
			int id=StringUtil.getInt(ss[0]);
			string name=StringUtil.getString(ss[1]);
			int type=StringUtil.getInt(ss[4]);
			int state=StringUtil.getInt(ss[5]);
			
			if(serverCell==null)
			{
				serverCell=GameObjectUtil.LoadResourcesPrefabs("UI-login/server-cell",3);
			}
			GameObject server=Instantiate(serverCell) as GameObject;
			server.name=id+"";
			server.transform.localScale=Vector3.one;
			server.transform.FindChild("serverId").GetComponent<UILabel>().text=id+TextsData.getData(23).chinese;
			GameObject serverName=server.transform.FindChild("serverName").gameObject;
			serverName.GetComponent<UILabel>().text=name;
			server.GetComponent<UIButtonMessage>().target=_MyObj;
			server.GetComponent<UIButtonMessage>().functionName="onClickChosedServer";
			server.GetComponent<UIButtonMessage>().param=id;
			UILabel des=server.transform.FindChild("serverDes").GetComponent<UILabel>();
			UISprite desSprite = server.transform.FindChild("serverDesSpr").GetComponent<UISprite>();
			
			if(id==lastServerId)
			{
				serverList1.Add(server);
				if(state==1)
				{
					des.text=TextsData.getData(131).chinese;
					serverName.GetComponent<UILabel>().color = Color.grey;
					desSprite.spriteName = "server_locked";
				}
				else if(type == 2)
				{
					des.text=TextsData.getData(130).chinese;
					desSprite.spriteName = "server_hot";
				}
				else
				{
					if(newPlayer)
					{
						des.text=TextsData.getData(129).chinese;
						desSprite.spriteName = "server_new";
					}
					else
					{
						des.text=TextsData.getData(127).chinese;
						desSprite.spriteName = "server_suggest";
					}
				}
			}
			else if(type==0)
			{
				serverList2.Add(server);
				if(state==1)
				{
					des.text=TextsData.getData(131).chinese;
					desSprite.spriteName = "server_locked";
					serverName.GetComponent<UILabel>().color = Color.grey;
				}
				else
				{
					des.text=TextsData.getData(128).chinese;
					desSprite.spriteName = "server_suggest";
				}
			}
			else if(type==1)
			{
				serverList3.Add(server);
				if(state==1)
				{
					des.text=TextsData.getData(131).chinese;
					desSprite.spriteName = "server_locked";
					serverName.GetComponent<UILabel>().color = Color.grey;
				}
				else
				{
					des.text=TextsData.getData(129).chinese;
					desSprite.spriteName = "server_new";
				}
			}
			else
			{
				serverList4.Add(server);
				if(state==1)
				{
					des.text=TextsData.getData(131).chinese;
					desSprite.spriteName = "server_locked";
					serverName.GetComponent<UILabel>().color = Color.grey;
				}
				else if(state==2)
				{
					des.text=TextsData.getData(130).chinese;
					desSprite.spriteName = "server_hot";
				}
				else
				{
					des.text="";
				}
			}
		}
		
		for(int k=0;k<serverList1.Count;k++)
		{
			GameObjectUtil.gameObjectAttachToParent(serverList1[k],serverParent);
		}
		for(int k=0;k<serverList2.Count;k++)
		{
			GameObjectUtil.gameObjectAttachToParent(serverList2[k],serverParent);
		}
		for(int k=0;k<serverList3.Count;k++)
		{
			GameObjectUtil.gameObjectAttachToParent(serverList3[k],serverParent);
		}
		for(int k=0;k<serverList4.Count;k++)
		{
			GameObjectUtil.gameObjectAttachToParent(serverList4[k],serverParent);
		}
		serverParent.GetComponent<UIGrid2>().repositionNow=true;
	}
	
	private void drawDefaultServer()
	{
		GameObject serverParent=chooseServer.transform.FindChild("servers/server-parent").gameObject;
		Transform serverTf=serverParent.transform.FindChild(""+lastServerId);
		string serverIdTemp=serverTf.FindChild("serverId").GetComponent<UILabel>().text;
		GameObject serverNameTemp=serverTf.FindChild("serverName").gameObject;
		string serverDesTemp=serverTf.FindChild("serverDes").GetComponent<UILabel>().text;
		
		defaultServer.transform.FindChild("serverId").GetComponent<UILabel>().text=serverIdTemp;
		GameObject serverName=defaultServer.transform.FindChild("serverName").gameObject;
		serverName.GetComponent<UILabel>().text=serverNameTemp.GetComponent<UILabel>().text;
		UIButtonMessage msg=defaultServer.transform.FindChild("serverSelect").GetComponent<UIButtonMessage>();
		msg.target=_MyObj;
		msg.functionName="onClickDefaultServer";
		msg.param=lastServerId;
		defaultServer.transform.FindChild("serverDes").GetComponent<UILabel>().text=serverDesTemp;
	}
	
	private void drawLastServer()
	{
		GameObject serverParent=chooseServer.transform.FindChild("servers/server-parent").gameObject;
		Transform serverTf=serverParent.transform.FindChild(""+lastServerId);
		string serverIdTemp=serverTf.FindChild("serverId").GetComponent<UILabel>().text;
		GameObject serverNameTemp=serverTf.FindChild("serverName").gameObject;
		string serverDesTemp=serverTf.FindChild("serverDes").GetComponent<UILabel>().text;
		string serverDesTempSpriteName = serverTf.FindChild("serverDesSpr").GetComponent<UISprite>().spriteName;
		//==设置上次登录信息==//
		Transform lastServerTf=chooseServer.transform.FindChild("lastServer/server-cell");
		lastServerTf.FindChild("serverId").GetComponent<UILabel>().text=serverIdTemp;
		lastServerTf.FindChild("serverName").GetComponent<UILabel>().text=serverNameTemp.GetComponent<UILabel>().text;
		lastServerTf.FindChild("serverDes").GetComponent<UILabel>().text=serverDesTemp;
		lastServerTf.FindChild("serverDesSpr").GetComponent<UISprite>().spriteName = serverDesTempSpriteName;
		lastServerTf.GetComponent<UIButtonMessage>().param=lastServerId;
	}
	
	public void onClickDefaultServer(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		chooseServer.SetActive(true);
		defaultServer.SetActive(false);
		//icon.SetActive(false);
		GameObject serverParent=chooseServer.transform.FindChild("servers/server-parent").gameObject;
		serverParent.GetComponent<UIGrid>().repositionNow = true;
	}
	
	public void onClickChosedServer(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		if(!canLogin(param))
		{
			ToastWindow.mInstance.showText(TextsData.getData(132).chinese);
			return;
		}
		lastServerId=param;
		drawDefaultServer();
		chooseServer.SetActive(false);
		defaultServer.SetActive(true);
		//icon.SetActive(true);
		setNetInfo();
	}
	
	public void onClickOverChosedServer()
	{
		drawDefaultServer();
		chooseServer.SetActive(false);
		defaultServer.SetActive(true);
		//icon.SetActive(true);
		setNetInfo();
	}
	
	private void drawRegLogin()
	{
		uidInput.value=username;
		psdInput.value=password;
		
		//登录//
		string str = TextsData.getData(524).chinese;
		regLogin.transform.FindChild("title").GetComponent<UILabel>().text=str;
	}
	
	private void setNetInfo()
	{
		//设置ip//
		for(int k=0;k<servers.Count;k++)
		{
			string[] ss=servers[k].Split('-');
			int id=StringUtil.getInt(ss[0]);
			if(id==lastServerId)
			{
				string ip=StringUtil.getString(ss[2]);
				string port=StringUtil.getString(ss[3]);
                // TTTERRRY
				RequestSender.serverIp = "211.149.198.194";
				RequestSender.serverPort=port;
				Debug.Log("ip:"+RequestSender.serverIp);
				Debug.Log("port:"+RequestSender.serverPort);
				break;
			}
		}
	}
	
	public void showPanel(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		switch(param)
		{
		case 1:
			panelLogin.SetActive(false);
			regLogin.SetActive(false);
			serverLogin.SetActive(true);
			drawChooseServer();
			drawDefaultServer();
			drawLastServer();
			chooseServer.SetActive(false);
			defaultServer.SetActive(true);
			//icon.SetActive(true);
			delayLogin=false;
			time=0;
			setNetInfo();
			
			if(SDKManager.getInstance().isNormal())
			{
				btn_change.SetActive(true);	
			}
			else
			{
				btn_change.SetActive(false);
			}
			break;
		case 2:
			panelLogin.SetActive(false);
			regLogin.SetActive(true);
			serverLogin.SetActive(false);
			login.SetActive(true);
			drawRegLogin();
			reg.SetActive(false);
			//icon.SetActive(false);
			delayLogin=false;
			time=0;
			GameObject serverParent=chooseServer.transform.FindChild("servers/server-parent").gameObject;
			serverParent.GetComponent<UIGrid>().repositionNow = true;
			break;
		case 3:
			//==维护状态不可进==//
			if(!canLogin(lastServerId))
			{
				ToastWindow.mInstance.showText(TextsData.getData(132).chinese);
				return;
			}
			panelLogin.SetActive(true);
			regLogin.SetActive(false);
			serverLogin.SetActive(false);
			delayLogin=true;
			
			if(loginType==0)
			{
				time=1f;
				btn_change.SetActive(true);
			}
			else
			{
				time=0.1f;
				btn_change.SetActive(false);
			}
			break;
		}
	}
	
	private bool canLogin(int targetId)
	{
		for(int k=0;k<servers.Count;k++)
		{
			string[] ss=servers[k].Split('-');
			int id=StringUtil.getInt(ss[0]);
			int state=StringUtil.getInt(ss[5]);
			if(id==targetId && state==1)
			{
				return false;
			}
		}
		return true;
	}
	
public void onClickEnter(int param)
	{
		//是否可登陆//
		if(!canLogin(lastServerId))
		{
			//ToastWindow.mInstance.showText(TextsData.getData(132).chinese);
		//	return;
		}

        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            showPanel(2);
            return;
        }
        loginType = 0;
        showPanel(3);
        return;
     
		//播放音效//
		if(SDKManager.getInstance().isUseBreakSDK())
		{
			if(Application.platform  == RuntimePlatform.IPhonePlayer)
			{
				if(SDKManager.getInstance().isSDK91Using())
				{
					/*int loginState91 = SDK91ConectorHelper.GetCurrentLoginState();
					string uin = "";
					string nickName = ""; 
					switch(loginState91)
					{
					case 0:
					{
						SDK91ConectorHelper.NdLogin();
						return;
					}break;
					case 1:
					{
						
					}break;
					case 2:
					{
						uin = SDK91ConectorHelper.LoginUin();
						nickName = SDK91ConectorHelper.NickName();
						
						username = "91_"+nickName;
						password = uin;
						if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
						{
							SDK91ConectorHelper.NdLogin();
						}
						
						loginType=1;
						showPanel(3);
					}break;
					}*/
        			SDKPlatform91.SdkLogin();
				}
				else if(SDKManager.getInstance().isSDKTBUsing())
				{/*
					string didInitFinished = TBControl.msgDidInitFinished;
					Debug.Log("didInitFinished:"+didInitFinished);
					Debug.Log("logoinfo:"+TBControl.msgLoginInfo);
					string loginState = TBControl.msgIsLogined;
					Debug.Log("loginstateInfo:"+loginState);
					if(string.IsNullOrEmpty(loginState))
					{
						Debug.Log("null login state!");
					}
					else
					{
						if(loginState.Contains(":"))
						{
							string[] strArr = loginState.Split(":"[0]);
							string state = strArr[1];
							if(state == "0")
							{
								Debug.Log("Not Login!");
								SDK.TBLogin(0);
								string loginInfo = TBControl.msgLoginInfo;
								Debug.Log("state0 logininfo:"+loginInfo);
							}
							else if(state == "1")
							{
								Debug.Log("Login succeed!");
								string loginInfo = TBControl.msgLoginInfo;
								Debug.Log("state0 logininfo:"+loginInfo);
								
								string userIDMsg = TBControl.msgUserID;
								string userNickName = TBControl.msgNickName;
								Debug.Log("nickname:"+userNickName+" userID:"+userIDMsg);
								
								string[] userIDArray = userIDMsg.Split(":"[0]);
								string tUserID = userIDArray[1];
								
								string[] uNickNameArray = userNickName.Split(":"[0]);
								string tNickName = uNickNameArray[1];
								
								username = "TB_"+tNickName;
								password = tUserID;
								
								Debug.Log("tNickName:"+tNickName+" tUserID:"+tUserID);
								
								//Check UserID is valid or not.
								if(string.IsNullOrEmpty(userIDMsg) || !userIDMsg.Contains(":"))
								{
									SDK.TBLogin(0);
								}
								else
								{
									loginType=1;
									showPanel(3);
									SDK.TBShowToolBar(3,true);
								}
							}
							else
							{
								Debug.Log("Unknow message!");
							}
						}
					}*/
				}
			}
		}
		else if(SDKManager.getInstance().isSDKGCUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				SDK_GCStubManager.sdk_startLogin();
			}
		}
		else if(SDKManager.getInstance().isSDKCPYYUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				//SDK_CPYY_manager.msg="start login";
				SDK_CPYY_manager.sdk_call(SDK_CPYY_manager.LoginId);
			}
		}
		else if(SDKManager.getInstance().isSDKCoolpadUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				//SDK_CPYY_manager.msg="start login";
				SDK_CoolPadManager.sdk_login();
			}
		}
		//百度多酷//
		else if(SDKManager.getInstance().isSDKBDDKUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				//SDK_CPYY_manager.msg="start login";
				SDK_BDDK_Manager.sdk_call(SDK_BDDK_Manager.LoginId);
			}
		}
		//kuai yong//
		else if(SDKManager.getInstance().isSDKKYUsing())
		{
				SDKPlatform91.SdkCallback(gameObject.name, "LoginCallbackKy");
				SDKPlatform91.SdkLogin();
		}
		else
		{
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				showPanel(2);
				return;
			}
			loginType=0;
			showPanel(3);
		}
	}
	
	//91 login callback
    public void LoginCallback91(string param)
    {
        Debug.Log("unity log - LoginCallback param:" + param);

        if (!string.IsNullOrEmpty(param))
        {
            username = "91_" + param;
            password = param;
            writePlayerPrefs();

            requestType = 2;
            string json = JsonMapper.ToJson(new LoginJson(username, "", 0, Constant.OS_IOS, param));
            RequestSender.getInstance().clearCookie();
            RequestSender.getInstance().request(1, json, true, this);
        }
		else
		{
			Debug.Log("LoginCallback91 error");
		}
    }
	
	public void LoginCallbackKy(string param)
	{
		Debug.Log("unity log - LoginCallbackKy param:" + param);
		
		if (!string.IsNullOrEmpty(param))
        {
			requestType = 4;
			RequestSender.getInstance().request(61,param,false,this);
		}
	}
	
	public void onClickLogin(int param)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		username=uidInput.value;
		password=psdInput.value;
		if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			ToastWindow.mInstance.showText(TextsData.getData(22).chinese);
			return;
		}
		loginType=0;
		showPanel(3);
		if(param==1)
		{
			time=2f;
		}
	}
	
	public void onClickReg(int param)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		switch(param)
		{
		case 0://==打开注册界面==//
			login.SetActive(false);
			reg.SetActive(true);
			reg.transform.FindChild("Input-uid").GetComponent<UIInput>().value="";
			reg.transform.FindChild("Input-psd").GetComponent<UIInput>().value="";
			reg.transform.FindChild("Input-psd-re").GetComponent<UIInput>().value="";
			string str = TextsData.getData(523).chinese;
			regLogin.transform.FindChild("title").GetComponent<UILabel>().text=str;
			break;
		case 1://==返回登录界面==//
			login.SetActive(true);
			reg.SetActive(false);
			string str1 = TextsData.getData(524).chinese;
			regLogin.transform.FindChild("title").GetComponent<UILabel>().text=str1;
			break;
		case 2://==注册==//
			string reg_uid=reg.transform.FindChild("Input-uid").GetComponent<UIInput>().value;
			string reg_psd=reg.transform.FindChild("Input-psd").GetComponent<UIInput>().value;
			string reg_psd_re=reg.transform.FindChild("Input-psd-re").GetComponent<UIInput>().value;
			if(string.IsNullOrEmpty(reg_uid) || string.IsNullOrEmpty(reg_psd) || string.IsNullOrEmpty(reg_psd_re))
			{
				ToastWindow.mInstance.showText(TextsData.getData(22).chinese);
				return;
			}
			if(reg_psd!=reg_psd_re)
			{
				ToastWindow.mInstance.showText(TextsData.getData(120).chinese);
				return;
			}
			uidTemp=reg_uid;
			psdTemp=reg_psd;
			requestType=1;
			//string name="test"+Random.Range(0,1000000);
			string nickname=uidTemp;
			string platform="";
			if(Application.platform==RuntimePlatform.Android)
			{
				platform=Constant.OS_ANDROID;
			}
			else if(Application.platform==RuntimePlatform.IPhonePlayer)
			{
				platform=Constant.OS_IOS;
			}
			else
			{
				platform=Constant.OS_PC;
			}
			RequestSender.getInstance().clearCookie();
			RequestSender.getInstance().request(2,JsonMapper.ToJson(new RegistJson(uidTemp,psdTemp,nickname,platform)),true,this);
			break;
		}
	}
	
	private void loginServer()
	{
		writePlayerPrefs();
		if(SDKManager.getInstance().isSDKGCUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				SDK_GCStubManager.sdk_setLogoutCallback();
				SDK_GCStubManager.sdk_setAccountSwitchCallback();
			}
		}
		
		string platform="";
		if(Application.platform==RuntimePlatform.Android)
		{
			platform=Constant.OS_ANDROID;
		}
		else if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
			platform=Constant.OS_IOS;
		}
		else
		{
			platform=Constant.OS_PC;
		}
		
		if(string.IsNullOrEmpty(nickname))
		{
			nickname=username;
		}
		requestType=2;
		string json=JsonMapper.ToJson(new LoginJson(username,password,0,platform,nickname));
		RequestSender.getInstance().clearCookie();
		RequestSender.getInstance().request(1,json,true,this);
		Debug.Log("3333333333333333333333333333333");
	}
	
	private void writePlayerPrefs()
	{
		PlayerPrefs.SetInt("lastServerId",lastServerId);
		PlayerPrefs.SetString("username",username);
		PlayerPrefs.SetString("password",password);
		
		string serverName="";
		for(int k=0;k<servers.Count;k++)
		{
			string[] ss=servers[k].Split('-');
			int id=StringUtil.getInt(ss[0]);
			string name=StringUtil.getString(ss[1]);
			if(id==lastServerId)
			{
				serverName=name;
				break;
			}
		}
		PlayerPrefs.SetString("lastServerName",serverName);
	}
	
	public void sdk_login(string uid,string psd,string nick)
	{
		Debug.Log("22222222222222222222222222222222222");
		if(SDKManager.getInstance().isSDKGCUsing())
		{
			username="GC_"+uid;
		}
		else if(SDKManager.getInstance().isSDKCPYYUsing())
		{
			username="CP_"+uid;
		}
		else if(SDKManager.getInstance().isSDKCoolpadUsing())
		{
			username="CoP_"+uid;
		}
		else if(SDKManager.getInstance().isSDKBDDKUsing())
		{
			username="BD_"+uid;
		}
		password=psd;
		nickname=nick;
		loginServer();
	}
	
	//==糖果登录==//
	public void sdk_tangguo_login(string uid,string psd,string nick,string ss)
	{
		username="GC_"+uid;
		password=psd;
		nickname=nick;
		if(!string.IsNullOrEmpty(ss))
		{
			List<TangguoServerJson> ss2=JsonMapper.ToObject<List<TangguoServerJson>>(ss);
			foreach(TangguoServerJson s in ss2)
			{
				if((lastServerId+"").Equals(s.code) && !string.IsNullOrEmpty(s.host) && !string.IsNullOrEmpty(s.port))
				{
					RequestSender.serverIp=s.host;
					RequestSender.serverPort=s.port;
					break;
				}
			}
		}
		loginServer();
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				ErrorJson ej=JsonMapper.ToObject<ErrorJson>(json);
				errorCode=ej.errorCode;
				receiveData=true;
				break;
			case 2:
				PlayerResultJson pj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode=pj.errorCode;
				if(pj.errorCode==0)
				{
					//Debug.Log("LoginUI_new(PlayerResultJson) : " + json);
					//设置解锁模块信息//
					string[] str = pj.s;
					PlayerInfo.getInstance().SetUnLockData(str);
					PlayerInfo.getInstance().player=pj.list[0];
				}
				receiveData=true;
				break;
			case 3:
				NewPlayerBattleResultJson npbrj = JsonMapper.ToObject<NewPlayerBattleResultJson>(json);
				PlayerInfo.getInstance().npbrj = npbrj;
				PlayerInfo.getInstance().battleType = STATE.BATTLE_TYPE_DEMO;
				errorCode = npbrj.errorCode;
				receiveData = true;
				break;
			case 4:
				userResJson = JsonMapper.ToObject<UserResultJson>(json);
				
				receiveData = true;
				break;
			}
		}
	}
}
