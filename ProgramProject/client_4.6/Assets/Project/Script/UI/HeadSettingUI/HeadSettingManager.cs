using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadSettingManager : MonoBehaviour , ProcessResponse, BWWarnUI{
	
//	public static HeadSettingManager mInstance;
	
	public GameObject ShowData;
	public GameObject ChangeName;
	public GameObject CostTip;
	public GameObject ChangeIcon;
	public GameObject SoundSetting;
	
	public GameObject MusicOpen;
	public GameObject MusicClose;
	public GameObject SoundEffOpen;
	public GameObject SoundEffClose;
	
	
	//头像设置主界面数据//
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel expLabel;
	public UILabel battleLabel;
	public UILabel fNumLabel;
	public UILabel idLabel;
	public UISprite headIconSpr;
	
	//更改名称界面//
	public UIInput nameInput;
	public UILabel changeNameLabel;
	public UISprite changeNameIcon;
	
	//花费提示界面//
	public UILabel CostLabel;
	
	//更改头像//
//	public GameObject headIconItem;
	public GameObject ScrollView;
	public GameObject GridList;
	public UIScrollBar ScrollBar;
	
	//基础信息中的数据//
	public GameObject GoldObj;
	public GameObject CrystalObj;
	public GameObject PowerObj;
	public GameObject EnergyObj;
		
	
	private Transform _myTransform;
	// 1, 获得界面信息即修改名字需要花费多少水晶， 2 确认修改名字， 3 关闭界面，同时向服务器发送头像信息,4 请求购买怒气 , 5 购买水晶//
	private int requestType;
	private int errorCode;
	//当前的界面 1 showData显示信息界面， 2 （changeNum）更改名称界面， 3 (CostTip)更改名称花费提示界面，4  (ChangIcon)更改头像, 5 声音设置//
	private int curState;
	private bool receiveData;
	private int changeNameCost;
	//当前可以选择的头像id//
	private int nameId;	
	//友情值//
	private int fNum;
	//账号id//
	private int playerId;
	
	//音乐音效的音量//
	private float musicVolume = 1;
	private float soundEffVolume = 1;
	
	private GameObject Music_Bg;
	private GameObject Music_Eff;
	
//	private GameObject MusicCloseLight;
//	private GameObject MusicCloseBlack;
//	private GameObject MusicOpenLight;
//	private GameObject MusicOpenBlack;
//	private GameObject SoundEffCloseLight;
//	private GameObject SoundEffCloseBlack;
//	private GameObject SoundEffOpenLight;
//	private GameObject SoundEffOpenBlack;
	
	private GameObject loadPrefab;
	private GameObject loadPrefab2;
	string loadPath = "Prefabs/UI/UI-HeadSettingPanel/NameIconsItem";
	string loadTextPath = "Prefabs/UI/UI-HeadSettingPanel/NameIconsItem2";
	string curIconName = "";
	UIAtlas curIconAtlas = null;
	string curInputPlayerName = "";
	
	
	int curPowerLastTime;
	
	//充值界面的json//
	private RechargeUiResultJson rechargeRJ;
	
	UILabel powerTipLB ;
	
	
	void Awake()
	{
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		_myTransform = transform;
		init();
//		hide ();
	}
	
	public void init ()
	{
//		base.init ();
		if(loadPrefab==null)
		{
			loadPrefab = Resources.Load(loadPath) as GameObject;
		}
		if(loadPrefab2==null)
		{
			loadPrefab2 = Resources.Load(loadTextPath) as GameObject;
		}
	}
	
	// Use this for initialization
	void Start () {
		curState = 1;
		if(Music_Bg == null)
		{
			Music_Bg = MusicManager.bgMusicObj;
		}
		if(Music_Eff == null)
		{
			Music_Eff = MusicManager.effectMusicObj;
		}
		
//		MusicCloseLight = MusicClose.transform.FindChild("LightLabel").gameObject;
//		MusicCloseBlack = MusicClose.transform.FindChild("BlackLabel").gameObject;
//		
//		MusicOpenLight = MusicOpen .transform.FindChild("LightLabel").gameObject;
//		MusicOpenBlack = MusicOpen.transform.FindChild("BlackLabel").gameObject;
//		
//		SoundEffCloseLight = SoundEffClose .transform.FindChild("LightLabel").gameObject;
//		SoundEffCloseBlack = SoundEffClose.transform.FindChild("BlackLabel").gameObject;
//		
//		SoundEffOpenLight = SoundEffOpen .transform.FindChild("LightLabel").gameObject;
//		SoundEffOpenBlack = SoundEffOpen.transform.FindChild("BlackLabel").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData = false;
			
			if(errorCode == -3)
				return;
			if(errorCode==70)			//vip等级不足！//
			{
//				ToastWindow.mInstance.showText(TextsData.getData(243).chinese);
				string str = TextsData.getData(243).chinese;
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(errorCode==71)			//水晶不足//
			{
//				ToastWindow.mInstance.showText(TextsData.getData(244).chinese);
				string str = TextsData.getData(244).chinese;
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(errorCode==72)			//购买次数已达上限！//
			{
//				ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
				string str = TextsData.getData(240).chinese;
			    //提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			
			
			switch(requestType)
			{
			case 1:
				ChangeState(1);
				break;
			case 2:			//改名字//
				if(errorCode == 0)
				{
					nameInput.value = curInputPlayerName;
					PlayerInfo.getInstance().player.name = nameInput.value;
					HeadUI.mInstance.refreshPlayerInfo();
					ChangeState(1);
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
					{
						GuideUI_ChangePlayerName.mInstance.showStep(2);
					}
					//向talkingdata发送数据//
					if(!TalkingDataManager.isTDPC)
					{
						TDGAItem.OnPurchase("changename" , 1, changeNameCost);
					}
				}
				
				else if(errorCode == 75)		//名字为空//
				{
					string str = TextsData.getData(302).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 19)		//金币或水晶不足//
				{
					string str = TextsData.getData(244).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 85)		//名字中还有非法字符//
				{
					string str = TextsData.getData(486).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 87)		//名字过长//
				{
					string str = TextsData.getData(355).chinese;
					ToastWindow.mInstance.showText(str);
				}
				
				break;
			case 3:			//改头像//
				
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().player.head = curIconName;
					HeadUI.mInstance.refreshPlayerInfo();
					hide();
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
					{
						GuideUI_ChangePlayerName.mInstance.hideAllStep();
						UISceneDialogPanel.mInstance.showDialogID(48);
					}
				}
				break;
			case 4:				//购买怒气上线//
				if(errorCode == 0)
				{
					HeadUI.mInstance.refreshPlayerInfo();
					//==怒气上限==//
					UILabel energyLabel = EnergyObj.transform.FindChild("Num").GetComponent<UILabel>();
					energyLabel.text=PlayerInfo.getInstance().player.maxEnergy+"";
					
					//统计购买怒气消费//
					if(!TalkingDataManager.isTDPC)
					{
						int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
						EnergyupData ed=EnergyupData.getData(number);
						if(ed!=null)
						{
							TDGAItem.OnPurchase("BuyEnergy",1,ed.cost);
						}
					}
				}
				break;
			case 5:
				if(errorCode == 0)
				{
					
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
					ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ, 
						"ChargePanel") as ChargePanel;
					charge.curRechargeResult = rechargeRJ;
					//如果vipCost是0表示没有充值过，是第一次充值//
					if(rechargeRJ.vipCost == 0)
					{
						charge.firstCharge = 0;
					}
					else
						charge.firstCharge = rechargeRJ.vipCost;
                    charge.isShowType = 1;
					charge.show();
				}
				break;
			}
			
		}
		
		if(curState == 1 && powerTipLB != null)
		{
			int time = HeadUI.mInstance.GetPowerRestoreTime();
			if(curPowerLastTime != time)
			{
				curPowerLastTime = time;
				//刷新体力剩余时间//
				int min = curPowerLastTime / 60;
				int sec = curPowerLastTime % 60;
				string lastTimes = "";
				if(sec > 0)
				{
					if(sec >= 10)
					{
						lastTimes = min + ":" + sec;
					}
					else 
					{
						lastTimes = min + ":0" + sec;
					}
				}
				else 
				{
					lastTimes = min.ToString() ;
				}
				powerTipLB.text = TextsData.getData(449).chinese + lastTimes;
			}
			
		}
	}
	
	public void show ()
	{
//		base.show ();
//		MainMenuManager.mInstance.isCanClick = false;
        Main3dCameraControl.mInstance.SetBool(true);
		//GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;

		ChangeState(1);
//		//获取更买名称的所需要花费的水晶//
//		requestType = 1;
//		//发送请求headSetting界面信息//
//		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_HEADSET_SHOWDATA),this);
		
		//==华为渠道打开改名界面时要隐藏浮标,关闭改名界面时再显示浮标==//
		//if(SDKManager.getInstance().isSDKGCUsing() && TalkingDataManager.channelId.Equals("879"))
		//{
		//	SDK_StubManager.sdk_hideFloatingView();
		//}
	}
	
	public void SetData(HeadSetResultJson hrj)
	{
		if(hrj != null)
		{
			changeNameCost = hrj.crystal;
			nameId = hrj.iconId;
			fNum = hrj.f;
			playerId = hrj.playerid;
		}
		curPowerLastTime = HeadUI.mInstance.GetPowerRestoreTime();
		show();
	}
	
	
	//当前的界面 1 showData显示信息界面， 2 （changeNum）更改名称界面， 3 (CostTip)更改名称花费提示界面，4  (ChangIcon)更改头像, 5 声音设置//
	public void ChangeState(int state)
	{
		curState = state;
		switch(state)
		{
		case 1:
			ShowDataInit();
			break;
		case 2:
			ChangeNameInit();
			break;
		case 3:
			CostTipInit();
			break;
		case 4:
			ChangeIconInit();
			break;
		case 5:
			SoundSettingInit();
			break;
		}
	}
	
	public void ShowDataInit()
	{
		ShowData.SetActive(true);
		ChangeName.SetActive(false);
		CostTip.SetActive(false);
		ChangeIcon.SetActive(false);
		SoundSetting.SetActive(false);
		
		nameLabel.text = PlayerInfo.getInstance().player.name;
		levelLabel.text = PlayerInfo.getInstance().player.level.ToString();
		PlayerData pd=PlayerData.getData(PlayerInfo.getInstance().player.level+1);
		expLabel.text = PlayerInfo.getInstance().player.curExp.ToString() + "/" + pd.exp;
		battleLabel .text = PlayerInfo.getInstance().player.battlePower.ToString();
		fNumLabel .text = fNum.ToString();
		idLabel.text = playerId.ToString();
		if(curIconName == "")
		{
			curIconName = PlayerInfo.getInstance().player.head;
		}
		string atlasName = CardData.getAtlas(curIconName);
		UIAtlas atlas = LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
		curIconAtlas = atlas;
		headIconSpr.spriteName = curIconName;
		headIconSpr.atlas = atlas;
		
		//绘制基础信息//
		refreshBasicData();
		
		
		//初始化声音设置界面//
		SoundSettingInit();
		
	}
	
	
	public void refreshBasicData()
	{
		//修改基础数据的内容//
		UILabel goldLabel = GoldObj.transform.FindChild("Num").GetComponent<UILabel>();
		UILabel crytalLabel = CrystalObj.transform.FindChild("Num").GetComponent<UILabel>();
		UILabel powerLabel =  PowerObj.transform.FindChild("Num").GetComponent<UILabel>();
		UILabel energyLabel = EnergyObj.transform.FindChild("Num").GetComponent<UILabel>();
		if(powerTipLB == null)
		{
			powerTipLB = PowerObj.transform.FindChild("Tip").GetComponent<UILabel>();
		}
		
		
		goldLabel.text = PlayerInfo.getInstance().player.gold.ToString();
		crytalLabel.text = PlayerInfo.getInstance().player.crystal.ToString();
		powerLabel.text = PlayerInfo.getInstance().player.power+"/"+PlayerInfo.getInstance().player.sPower; 
		energyLabel.text = PlayerInfo.getInstance().player.maxEnergy.ToString();
		if(PlayerInfo.getInstance().player.power >= PlayerInfo.getInstance().player.sPower)
		{
			powerTipLB.gameObject.SetActive(false);
		}
		else 
		{
			powerTipLB.gameObject.SetActive(true);
			int min = curPowerLastTime / 60;
			int sec = curPowerLastTime % 60;
			string time = "";
			if(min > 0 && sec > 0)
			{
				time = min +":"+sec;
			}
			else if(min > 0)
			{
				time = min.ToString();
			}
			powerTipLB.text = TextsData.getData(449).chinese + time;
		}
	}
	
	public void ChangeNameInit()
	{
		
		ShowData.SetActive(false);
		ChangeName.SetActive(true);
		CostTip.SetActive(false);
		ChangeIcon.SetActive(false);
		SoundSetting.SetActive(false);
		
		if(nameInput != null)
		{
			nameInput.value = PlayerInfo.getInstance().player.name;
		}
		changeNameLabel.text = PlayerInfo.getInstance().player.name;
		changeNameIcon.spriteName = curIconName;
		changeNameIcon.atlas = curIconAtlas;
	}
	
	public void CostTipInit()
	{
		
		ShowData.SetActive(false);
		ChangeName.SetActive(false);
		CostTip.SetActive(true);
		ChangeIcon.SetActive(false);
		SoundSetting.SetActive(false);
		
		CostLabel.text = "x" + changeNameCost;
	}
	
	public void ChangeIconInit()
	{
		
		ShowData.SetActive(false);
		ChangeName.SetActive(false);
		CostTip.SetActive(false);
		ChangeIcon.SetActive(true);
		SoundSetting.SetActive(false);
		
		CleanScrollView();
		for(int i = 1;i <= nameId + 1 && i <= IconUnlockData.GetHashLength();i ++)
		{
			
			
//			UISprite icon2 = item.transform.FindChild("Icon2").GetComponent<UISprite>();
//			UISprite icon3 = item.transform.FindChild("Icon3").GetComponent<UISprite>();
//			GameObject TipBar = item.transform.FindChild("TipBar").gameObject;
			IconUnlockData iud = IconUnlockData.getData(i);
			if(i <= nameId)
			{
				for(int j = 0;j < 3;j ++)
				{
					
					GameObject item = Instantiate(loadPrefab) as GameObject;
					GameObjectUtil.gameObjectAttachToParent(item,GridList);
					
					UISprite icon1 = item.transform.FindChild("Icon1").GetComponent<UISprite>();
					icon1.gameObject.SetActive(true);
					
					UIButtonMessage ubm1 = item.GetComponent<UIButtonMessage>();
					ubm1.param = i * 10 + (j+1);
					ubm1.target = _myTransform.gameObject;
					ubm1.functionName = "OnClickChangeIconBtn";
					string iconName = "";
					if(j == 0)
					{
						iconName = iud.headIcon1;
					}
					else if(j == 1)
					{
						iconName = iud.headIcon2;
					}
					else if(j == 2)
					{
						iconName = iud.headIcon3;
					}
					icon1.spriteName = iconName;
					string atlasName = CardData.getAtlas(iconName);
					UIAtlas atlas1 = LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
					icon1.atlas = atlas1;
				}
//				icon2.gameObject.SetActive(true);
//				icon3.gameObject.SetActive(true);
//				UIButtonMessage ubm2 = icon2.gameObject.GetComponent<UIButtonMessage>();
//				ubm2.param = i * 10 + 2;
//				ubm2.target = _myTransform.gameObject;
//				ubm2.functionName = "OnClickChangeIconBtn";
//				UIButtonMessage ubm3 = icon3.gameObject.GetComponent<UIButtonMessage>();
//				ubm3.param = i * 10 + 3;
//				TipBar.SetActive(false);
//				ubm3.target = _myTransform.gameObject;
//				ubm3.functionName = "OnClickChangeIconBtn";
//				
//				string iconName = iud.headIcon1;
//				icon1.spriteName = iconName;
//				string atlasName = CardData.getAtlas(iconName);
//				UIAtlas atlas1 = LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
//				icon1.atlas = atlas1;
//				
//				icon2.spriteName = iud.headIcon2;
//				atlasName = CardData.getAtlas(iud.headIcon2);
//				UIAtlas atlas2 = LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
//				icon2.atlas = atlas2;
//				
//				icon3.spriteName = iud.headIcon3;
//				atlasName = CardData.getAtlas(iud.headIcon3);
//				UIAtlas atlas3 = LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
//				icon3.atlas = atlas3;
			}
			else 
			{
				GameObject item = Instantiate(loadPrefab2) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(item,GridList);
				
				GameObject TipBar = item.transform.FindChild("TipBar").gameObject;
				
				TipBar.SetActive(true);
//				icon1.gameObject.SetActive(false);
//				icon2.gameObject.SetActive(false);
//				icon3.gameObject.SetActive(false);
				UILabel tip = TipBar.transform.FindChild("Label").GetComponent<UILabel>();
				string s1 = TextsData.getData(300).chinese;
				string s2 = TextsData.getData(301).chinese;
				int c = iud.unlock;
				tip.text = s1 + c + s2;
			}
		}
		
	}
	
	public void SoundSettingInit()
	{
//		ShowData.SetActive(false);
//		ChangeName.SetActive(false);
//		CostTip.SetActive(false);
//		ChangeIcon.SetActive(false);
//		SoundSetting.SetActive(true);
		musicVolume = PlayerInfo.getInstance().musicBgVolume;
		soundEffVolume = PlayerInfo.getInstance().soundEffVolume;
		
		if(musicVolume > 0)		//音乐开启//
		{
			
//			MusicOpenLight.SetActive(true);
//			MusicOpenBlack.SetActive(false);
//			MusicCloseLight.SetActive(false);
//			MusicCloseBlack.SetActive(true);
			MusicOpen.SetActive(true);
		 	MusicClose.SetActive(false);
		}
		else
		{
//			MusicOpenLight.SetActive(false);
//			MusicOpenBlack.SetActive(true);
//			MusicCloseLight.SetActive(true);
//			MusicCloseBlack.SetActive(false);
			MusicOpen.SetActive(false);
		 	MusicClose.SetActive(true);
		}
		
		if(soundEffVolume > 0)	//音效开启//
		{
			
//			SoundEffOpenLight.SetActive(true);
//			SoundEffOpenBlack.SetActive(false);
//			SoundEffCloseLight.SetActive(false);
//			SoundEffCloseBlack.SetActive(true);
			SoundEffOpen.SetActive(true);
			SoundEffClose.SetActive(false);
		}
		else 
		{
//			SoundEffOpenLight.SetActive(false);
//			SoundEffOpenBlack.SetActive(true);
//			SoundEffCloseLight.SetActive(true);
//			SoundEffCloseBlack.SetActive(false);
			SoundEffOpen.SetActive(false);
			SoundEffClose.SetActive(true);
		}
	}
	
	
	public string RangeName()
	{
		
		int frontNum = StringUtil.getInt( NameData.getData(0).front);
		int lastNum = StringUtil.getInt( NameData.getData(0).back );
		int id1 = Random.Range(1, frontNum);
		int id3 = Random.Range(1, lastNum);
		string n = NameData.getData(id1).front + NameData.getData(id3).back;
		return n;
	}
	
	public void AddEnergy()
	{
		int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
		int canBuyNumber = VipData.getData(PlayerInfo.getInstance().player.vipLevel).maxenergy;
		if(number >= canBuyNumber)
		{
			ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
			return;
		}
		EnergyupData ed=EnergyupData.getData(number+1);
		if(ed==null)
		{
			ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
			return;
		}
		else
		{
			ToastWarnUI.mInstance.showWarn(TextsData.getData(245).chinese.Replace("num1",ed.cost+"").Replace("num2",ed.energy+""),this);
		}
	}
	
	
	public void CleanScrollView(){
		GridList.GetComponent<UIGrid2>().repositionNow = true;
		ScrollBar.value = 1;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,400,320);
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
	}
	
	public void hide ()
	{
//		base.hide ();
        Main3dCameraControl.mInstance.SetBool(false);
		curIconName = "";
		CleanScrollView();
		if(UISceneStateControl.mInstace != null)
		{
			
			GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;

			if(obj!=null)
			{

                MainMenuManager main = obj.GetComponent<MainMenuManager>();
				if(main != null &&  obj.activeSelf)
				{
					main.SetEnterType(STATE.ENTER_MAINMENU_BACK);
					//					main.isCanClick = true;
					main.SendToGetData();
				}
			}
		}
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING);
		
		//==华为渠道打开改名界面时要隐藏浮标,关闭改名界面时再显示浮标==//
		//if(SDKManager.getInstance().isSDKGCUsing() && TalkingDataManager.channelId.Equals("879"))
		//{
		//	SDK_StubManager.sdk_showFloatingView();
		//}
	}
	
	
	
	public void warnningSure()
	{
		int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
		EnergyupData ed=EnergyupData.getData(number+1);
		if(ed==null)
		{
			return;
		}
		else
		{
			if(PlayerInfo.getInstance().player.vipLevel<ed.viplevel)
			{
				ToastWindow.mInstance.showText(TextsData.getData(241).chinese.Replace("num",ed.viplevel+""));
				return;
			}
			if(PlayerInfo.getInstance().player.crystal<ed.cost)
			{
				ToastWindow.mInstance.showText(TextsData.getData(244).chinese);
				return;
			}
			requestType=4;
			PlayerInfo.getInstance().sendRequest(new EnergyJson(),this);
		}
	}
	
	public void warnningCancel(){}
	
	
	
	
	private void gc()
	{
		loadPrefab=null;
		Resources.UnloadUnusedAssets();
	}
	
	//id 0 更改名称 1 更改头像  2 声音设置， 3 保存更改//
	public void OnClickBtn(int id)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		switch(id)
		{
		case 0:		
			ChangeState(2);
			break;
		case 1:
			ChangeState(4);
			break;
		case 2:
			ChangeState(5);
			break;
		}
	}
	
	public void OnClickBackBtn()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
		{
			if(GuideUI_ChangePlayerName.mInstance.runningStep == 1)
			{
				ToastWindow.mInstance.showText(TextsData.getData(495).chinese);
				return;
			}
		}
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
		requestType = 3;
		UIJson uij = new UIJson();
		uij.UIJsonForHeadSet(STATE.UI_HEADSET_CHANGEICON, curIconName);
		PlayerInfo.getInstance().sendRequest(uij,this);
//		hide();
	}
	
	//花费提示界面 id 0 取消, 1 确定//
	public void OnClickTipBtn(int id)
	{
		if(id == 0)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			ChangeState(2);
		}
		else if(id == 1)		//确定，向服务器发送更改名字数据//
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(curInputPlayerName != null && !curInputPlayerName.Equals(""))
			{
				
				requestType = 2;
				UIJson uij = new UIJson();
				uij.UIJsonForHeadSet(STATE.UI_HEADSET_CHANGENAME, curInputPlayerName);
				PlayerInfo.getInstance().sendRequest(uij,this);
			}
			else 
			{
				string str = TextsData.getData(302).chinese;
				ToastWindow.mInstance.showText(str);
			}
		}
	}
	
	//更改名称界面按钮 0 随机 1 取消， 2 确定//
	public void OnClickChangeBtn(int id)
	{
		switch(id)
		{
		case 0:
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			changeNameLabel.text = RangeName();
			nameInput.value = changeNameLabel.text ;
			break;
		case 1:			//返回到showData界面//
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
			{
				ToastWindow.mInstance.showText(TextsData.getData(495).chinese);
				return;
			}
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			ChangeState(1);
			curInputPlayerName = "";
			break;
		case 2:			//判断修改名字的花费，如果有花费则跳转到costTip界面//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			curInputPlayerName = nameInput.value;
			int len = curInputPlayerName.Length;
			if(len>6)
			{
				string str = TextsData.getData(594).chinese;
				ToastWindow.mInstance.showText(str);
//				curInputPlayerName = curInputPlayerName.Substring(0, 6);
				return ;
			}
			else if(len <= 0)		//名字不能为空//
			{

				string str = TextsData.getData(302).chinese;
				ToastWindow.mInstance.showText(str);
				return;
			}
			
			if(curInputPlayerName.Equals( PlayerInfo.getInstance().player.name))
			{
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ChangePlayerName))
				{
					ToastWindow.mInstance.showText(TextsData.getData(495).chinese);
					return;
				}
				else
				{
					ChangeState(1);
				}
				
			}
			else if(changeNameCost > 0)
			{
				ChangeState(3);
			}
			else 
			{
				requestType = 2;
				UIJson uij = new UIJson();
				uij.UIJsonForHeadSet(STATE.UI_HEADSET_CHANGENAME, curInputPlayerName);
				PlayerInfo.getInstance().sendRequest(uij,this);
			}
			break;
		}
	}
	
	//设置音乐界面按钮 id 0关， 1 开//
	public void OnClickMusicBtn(int id)
	{
		
		if( id == 0)		//关//
		{
//			MusicCloseLight.SetActive(true);
//			MusicCloseBlack.SetActive(false);
//			MusicOpenLight.SetActive(false);
//			MusicOpenBlack.SetActive(true);
			MusicOpen.SetActive(false);
			MusicClose.SetActive(true);
			if(Music_Bg != null)
			{
				Music_Bg.GetComponent<AudioSource>().volume = 0;
			}
			musicVolume = 0;
		}
		else if( id == 1)
		{
//			MusicOpenLight.SetActive(true);
//			MusicOpenBlack.SetActive(false);
//			MusicCloseLight.SetActive(false);
//			MusicCloseBlack.SetActive(true);
			MusicOpen.SetActive(true);
			MusicClose.SetActive(false);
			if(Music_Bg != null)
			{
				Music_Bg.GetComponent<AudioSource>().volume = 1;
			}
			musicVolume = 1;
		}
		PlayerInfo.getInstance().musicBgVolume = musicVolume;
		PlayerPrefs.SetFloat(STATE.SAVE_KEY_MUSICBG, musicVolume);
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
	}
	
	//设置音效界面按钮 0 关， 1 开//
	public void OnClickEffBtn(int id)
	{
		
		if(id == 0)
		{
//			SoundEffCloseLight.SetActive(true);
//			SoundEffCloseBlack.SetActive(false);
//			SoundEffOpenLight.SetActive(false);
//			SoundEffOpenBlack.SetActive(true);
			SoundEffOpen.SetActive(false);
			SoundEffClose.SetActive(true);
			if(Music_Eff != null)
			{
				Music_Eff.SetActive(false);
			}
			
			soundEffVolume = 0;
		}
		else if(id == 1)
		{
//			SoundEffOpenLight.SetActive(true);
//			SoundEffOpenBlack.SetActive(false);
//			SoundEffCloseLight.SetActive(false);
//			SoundEffCloseBlack.SetActive(true);
			SoundEffOpen.SetActive(true);
			SoundEffClose.SetActive(false);
			if(Music_Eff != null)
			{
				Music_Eff.SetActive(true);
			}
			soundEffVolume = 1;
		}
		PlayerInfo.getInstance().soundEffVolume = soundEffVolume;
		PlayerPrefs.SetFloat(STATE.SAVE_KEY_SOUNDEFF, soundEffVolume);
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
	}
	//id用来获得头像//
	public void OnClickChangeIconBtn(int id)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		int formId = id / 10;
		int n = id %10;
		IconUnlockData icd = IconUnlockData.getData(formId);
		if(n == 1)
		{
			curIconName = icd.headIcon1;
		}
		else if(n == 2)
		{
			curIconName = icd.headIcon2;
		}
		else if(n == 3)
		{
			curIconName = icd.headIcon3;
		}
		ChangeState(1);
	}
	
	//点击购买按钮 id : 1 金币， 2 水晶， 3 体力， 4， 怒气//
	public void OnClickBuyBtn(int id)
	{
		int buyType = 1;
		int	jsonType = 1;
		int	costType = 1;
		switch(id)
		{
		case 1:
			buyType = 1;
			jsonType = 1;
			costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_HEAD);
			break;
		case 2:
			requestType = 5;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
			break;
		case 3:
			buyType = 2;
			jsonType = 1;
			costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_HEAD);
			break;
		case 4:
			AddEnergy();
			break;
		}
		
		
	}
	
	public void receiveResponse (string json)
	{
		Debug.Log("HeadSettingManager : json ==== " + json);
		if(json != null)
		{
			PlayerInfo.getInstance().isShowConnectObj = false;
			receiveData = true;
			switch (requestType)
			{
			case 1:			//获取界面信息//
				HeadSetResultJson hrj = JsonMapper.ToObject<HeadSetResultJson>(json);
				errorCode = hrj.errorCode;
				if(errorCode == 0)
				{
					changeNameCost = hrj.crystal;
					nameId = hrj.iconId;
					fNum = hrj.f;
					playerId = hrj.playerid;
				}
				
				break;
			case 2:			//改名字//
				
				hrj = JsonMapper.ToObject<HeadSetResultJson>(json);
				errorCode = hrj.errorCode;
				if(errorCode == 0)
				{
					changeNameCost = hrj.crystal;
					PlayerInfo.getInstance().player.crystal = hrj.crystals;
					curInputPlayerName = hrj.name;
				}
				
				break;
			case 3:			//改头像//
				hrj = JsonMapper.ToObject<HeadSetResultJson>(json);
				errorCode = hrj.errorCode;
				break;
			case 4:			//购买体力//
				PlayerResultJson prj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode=prj.errorCode;
				if(errorCode==0)
				{
					PlayerInfo.getInstance().player=prj.list[0];
				}
				receiveData=true;
				break;
			case 5:			//购买水晶//
				RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
				errorCode = rechargej.errorCode;
				if(errorCode==0)
				{
					rechargeRJ = rechargej;
				}
				receiveData = true;
				break;
			}
		}
	}
}
