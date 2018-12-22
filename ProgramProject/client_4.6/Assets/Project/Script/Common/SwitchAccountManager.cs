using UnityEngine;
using System.Collections;

public class SwitchAccountManager : MonoBehaviour,ProcessResponse{
	
	public static SwitchAccountManager mInstance;
	
	private string username;
	private string password;
	
	private int requestType;
	private bool receiveData;
	private int errorCode;
	private PlayerResultJson pj;
	
	// Use this for initialization
	void Start () {
		if(PlayerInfo.getInstance().isLogout == true)
		{
			Destroy(gameObject);
			return;
		}
		Object.DontDestroyOnLoad(gameObject);
		mInstance=this;
        //PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
				if(errorCode!=0)
				{
					if(errorCode==1)
					{
						ToastWindow.mInstance.showText(TextsData.getData(322).chinese);
					}
					else if(errorCode==-4)
					{
						ToastWindow.mInstance.showText(TextsData.getData(125).chinese.Replace("name",username));
					}
					else if(errorCode==-5)
					{
						ToastWindow.mInstance.showText(TextsData.getData(126).chinese.Replace("name",username));
					}
					else
					{
						ToastWindow.mInstance.showText("errorCode:"+errorCode);
					}
					return;
				}
				else
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
							
							
							//ExtendData ed=new ExtendData(pe.id+"",pe.name,pe.level+"",PlayerPrefs.GetInt("lastServerId"),PlayerPrefs.GetString("lastServerName"),pe.crystal+"","",pe.vipLevel+"");
							//SDK_GCStubManager.sdk_submitExtendData("loginGameRole",JsonMapper.ToJson(ed));
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
						string username = PlayerPrefs.GetInt("username").ToString();
						//if(!TalkingDataManager.isTDPC)
						{
							TDGAAccount account = TDGAAccount.SetAccount(username);
							account.SetLevel(pe.level);
							//account.SetAge(pe);
							account.SetGameServer(PlayerPrefs.GetString("lastServerId"));
							
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
						//gc();
						PlayerInfo.getInstance().isShowConnectObj=false;
						PlayerInfo.getInstance().BattleOverBackType=0;
						PlayerInfo.isFirstLogin = true;
						GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
					}
				}
				break;
			case 3:
				if(errorCode == 0)
				{
					PlayerInfo.isFirstLogin = true;
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
				}
				break;
			}
		}
		
		if(Input.GetKey("p"))
		{
			logout();
		}
	}
	
	//==发送登录请求==//
	public void switchAccount(string username,string password,string platform,string nickname)
	{
		this.username=username;
		this.password=password;
		if(string.IsNullOrEmpty(nickname))
		{
			nickname=username;
		}
		writePlayerPrefs();
		string json=JsonMapper.ToJson(new LoginJson(username,password,0,platform,nickname));
		requestType=1;
		RequestSender.getInstance().clearCookie();
		RequestSender.getInstance().request(1,json,true,this);
	}
	
	public void logout()
	{
		Debug.Log("******logout-");
		PlayerInfo.getInstance().isLogout=true;
		PlayerInfo.getInstance().BattleOverBackType = 0;
        GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_LOADING);
	}
	
	private void writePlayerPrefs()
	{
		//PlayerPrefs.SetInt("lastServerId",lastServerId);
		PlayerPrefs.SetString("username",username);
		PlayerPrefs.SetString("password",password);
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//==此处特殊,收到消息但不处理isShowConnectObj标识==//
			switch(requestType)
			{
			case 1:
				pj=JsonMapper.ToObject<PlayerResultJson>(json);
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
			}
		}
	}
}
