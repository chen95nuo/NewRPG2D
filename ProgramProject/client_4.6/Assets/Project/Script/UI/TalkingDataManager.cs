using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalkingDataManager : MonoBehaviour {
	
	
	string ChannelTyp_Normal = "normal";
	string ChannelTyp_91 = "564";
	string ChannelTyp_PP = "518";
	string ChannelTyp_TB = "604";
	string ChannelTyp_DL = "606";
	string ChannelTyp_ITOOLS = "942";
	
	//==酷玩汇==//
	string ChannelTyp_KWH= "kwh";
	//==17173==//
	string ChannelTyp_17173="17173";
	//==GC==//
	string ChannelTyp_GC="62";
	//==酷派==//
	string ChannelTyp_Coolpad="coolpad";
	//==百度多酷==//
	string ChannelTyp_BDDK = "113";
	//==kuai yong==//
	string ChannelTyp_KY = "664";
	//==ai si==//
	string ChannelTyp_i4 = "940";
	//==ui ios==//
	string ChannelTyp_UC = "605";
	//==hai ma==//
	string ChannelTyp_HM = "869";
	
	Transform _myTransform;
    bool preIsRunBackgroud;
	public static string channelId = "";
	string appId;
    public static TalkingDataManager ins = null;
    public static bool isTDPC = false;
	
	void Awake()
	{
		if(PlayerInfo.getInstance().isLogout == true)
		{
			Destroy(gameObject);
			return;
		}
        ins = this;

        init();
		channelId = ChannelTyp_GC;
		
		_myTransform = transform;
		Object.DontDestroyOnLoad(_myTransform.gameObject);
	}

	// Use this for initialization
	void Start () {
		
		//TalkingDataPlugin.SessionStarted (appId, channelId);
		//==蜂巢的渠道编号从SDK回传过来==//
		if(!isTDPC && channelId!=ChannelTyp_GC)
		{
        	TalkingDataGA.OnStart(appId,channelId);					
		}
		Debug.Log("TalkingDataManager : Start--------------");
	}
	
	private void init()
	{
		if(Application.platform==RuntimePlatform.WindowsEditor || Application.platform==RuntimePlatform.OSXEditor)
		{
			isTDPC=true;
		}
		else
		{
			isTDPC=true;
		}
		appId = "E325350367EE4DDFD312D700208C8748";
		preIsRunBackgroud = Application.runInBackground;
	}

    //==蜂巢SDK回传的channelId==//
    public void initTalkingData()
    {
        initTalkingData(ChannelTyp_Normal);
    }


    //==蜂巢SDK回传的channelId==//
    public void initTalkingData(string channelIdFromAndroid)
	{
		init();
		channelId=channelIdFromAndroid;
		if(!isTDPC)
		{
        	TalkingDataGA.OnStart(appId,channelId);					
		}
		Debug.Log("TalkingDataManager : Start--------------");
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnApplicationPause(bool pauseState)
	{
		if(!pauseState)
		{
			//TalkingDataPlugin.SessionStarted (appId, channelId);
			if(!isTDPC)
			{
            	TalkingDataGA.OnStart(appId, channelId);  			
			}
			//Debug.Log("OnApplicationPause ====================== start" );
		}
		else
		{
			//TalkingDataPlugin.SessionStoped();
			if(!isTDPC)
			{
            	TalkingDataGA.OnEnd();							
			}
			//Debug.Log("OnApplicationPause ====================== stop" );
		}
		
	}
	
	void OnApplicationFocus(bool focusState)
	{
//		Debug.Log("focusState ====================== " + focusState);
	}
	
	
	
	void OnDestroy()
    {
		//统计玩家ID//
		//TalkingDataPlugin.SessionStoped();
		if(!isTDPC)
		{
        	TalkingDataGA.OnEnd();									
		
			PlayerElement pe = PlayerInfo.getInstance().player;
			if(pe != null)
			{
				string username = PlayerPrefs.GetString("username");
				TDGAAccount account = TDGAAccount.SetAccount(username);
				account.SetLevel(pe.level);
			}
		}
        Debug.Log("TalkingDataManager : destroy----------------stop");
    }
	//发送自定义事件//
	public static void SendTalkingDataEvent(string eventName,Dictionary<string,object> eventValue)
	{
		if(!TalkingDataManager.isTDPC)
		{
			TalkingDataGA.OnEvent(eventName,eventValue);				
		}
	}
}
