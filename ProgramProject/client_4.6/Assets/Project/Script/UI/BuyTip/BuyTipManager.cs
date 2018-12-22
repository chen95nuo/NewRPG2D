using UnityEngine;
using System.Collections;

public class BuyTipManager : BWUIPanel, ProcessResponse {
	
	//当前发送购买信息的ui的类型//
	public enum UI_TYPE:int
	{
		UI_HEAD = 			0,			//点击head购买//
		UI_SWEEP = 			1,			//扫荡界面//
		UI_SWRITEWORLD = 	2,			//冥想//
		UI_INTENSIFY = 		3,			//强化//
		UI_BREAK = 			4,			//突破//
		UI_COMPOSE = 		5,			//合成//
		UI_ACTIVESEL = 		6,			//异世界(活动副本)//
		UI_MAZE = 			7,			//迷宫//
		UI_MISSION = 		8,			//推图//
		UI_ARENA = 			9,			//竞技场//
	}
	
	public static BuyTipManager mInstance;
	
	public GameObject BuyResultObj;
	public GameObject BuyTipObj;

	
	public UILabel TitleLabel;
	public UILabel BuyTipDesLabel;
	public UILabel BuyResultDesLabel;
	
//	public UILabel CostLabel;
//	public UILabel BuyNumLabel;
//	public UILabel LastTimesLabel;
	
	// 1请求界面信息， 2  购买//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	//购买物品的类型 1 金币， 2 体力, 3 扫荡券，4 挑战次数, 5 购买冷却时间, 6 竞技场挑战次数,//
	private int buyType;
	//请求类型,1 请求购买界面信息， 2 购买//
	private int jsonType;
	//花费类型 ，1 水晶， 2 金币//
	private int costType;
	//购买花费的水晶数//
	private int costCrystal;
	//要购买的金币或体力的个数//
	private int num;
	//当天剩余的购买次数//
	private int times;
	
	//点击扫荡的次数//
	private int sweepTimes;
	
	//当前是在什么界面提示购买,0 在head上购买， 1 扫荡界面//
	private UI_TYPE curUIType;
	//当前关卡id//
	private int curMd;
	//当前冷却类型，1 pk, 2maze, 3 event//
	private int curCDType;
	//当前副本id//
	private int copyId;
	//购买的金币数//
	//private int getGold;
	//购买的体力数//
	//private int getPower;
	//花费钻石数//
	private double getCrystal;
	
	private bool isMissionBuy = false;
	
	private bool receiveByeNum;
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init ();
		hide();
	}
	
	public override void init ()
	{
//		base.init (); 
		_MyObj.transform.localPosition = new Vector3(0,0,-720);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			
			switch(requestType)
			{
			case 1:									//请求界面信息//
				if(errorCode == 0)
				{
					
					BuyTipInit();
				}
				else if(errorCode == 51)			//体力达到上限无需购买//
				{
					
					hide();
					string str = TextsData.getData(270).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 79)		//购买次数达到上限//
				{
					hide();
					string str = TextsData.getData(240).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 81)		//无需购买冷却时间， 直接切入战斗//
				{
					BuyResultInit();
				}
				else if(errorCode == 84)		//挑战次数还未用尽，无需购买，直接进入战斗//
				{
					BuyResultInit();
				}
				break;
			case 2:								//开始购买//
				if(errorCode == 0)
				{
					BuyResultInit();
					RefreshUIData();
					if(isMissionBuy)
					{
						MissionUI mission1 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI")as MissionUI;
						mission1.setPowerText();
						//isMissionBuy = false;
					}
				}
				else if(errorCode == 19)		//水晶不足//
				{
					hide();
					string str = TextsData.getData(244).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 79)		//次数已达上限//
				{
					hide();
					string str = TextsData.getData(273).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 80)		//购买类型错误//
				{
					
				}
				else if(errorCode == 51)			//体力达到上限无需购买//
				{
					hide();
					string str = TextsData.getData(289).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 84)		//挑战次数还未用尽，无需购买，直接进入战斗//
				{
					BuyResultInit();
				}
				
				
				break;
			}
		}
		
		if(receiveByeNum)
		{
			receiveByeNum = false;
			if(errorCode == 0)
			{
				if(buyType == 1)
				{
					if(!TalkingDataManager.isTDPC&&errorCode==0)
					{
						TDGAItem.OnPurchase("Gold",1,getCrystal);//记录购买金币//
					}
				}
				else if(buyType == 2)
				{
					if(!TalkingDataManager.isTDPC&&errorCode==0)
					{
						TDGAItem.OnPurchase("Power", 1 , getCrystal);//记录购买体力//
					}
				}
				else if(buyType == 4)
				{
					
					if(!TalkingDataManager.isTDPC)
					{
						TDGAItem.OnPurchase("battlenumber", 1 , costCrystal);//记录购买体力//
					}
				}
			}
		}
	}
	
	public override void show ()
	{
		base.show ();
		
		
		BuyTipInit ();
		
//		MainMenuManager.mInstance.isCanClick = false;
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.isCanClick = false;
		}

        tweenScale(new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1), new Vector3(1, 1, 1));
	}
	//buyType 1 金币， 2 体力, 3 购买扫荡券， 4 购买挑战次数, 5 购买冷却时间//
	//sweepTimes 连续扫荡的次数1次还是多次， UIType 当前购买界面的类型，0 在head点击购买， 1 扫荡界面, missionId 当type = 4 时，传关卡id//
	public void SetData(int buyType, int costT, int cost, int num, int times, int sweepTimes = 0, 
		UI_TYPE UIType = UI_TYPE.UI_HEAD, int missionId = 0, int cdType = 0, int copyId = 0)
	{
		this.buyType = buyType;
		this.costType = costT;
		this.costCrystal = cost;
		this.num = num;
		this.times = times;
		this.sweepTimes = sweepTimes;
		this.curUIType = UIType;
		this.curMd = missionId;
		this.curCDType = cdType;
		this.copyId = copyId;
		show();
	}
	
	public void BuyTipInit()
	{
		BuyResultObj.SetActive(false);
		BuyTipObj.SetActive(true);
		
		
		string s1 = "";
		string s2 = "";
		string s3 = "";
		string s4 = "";
		string s5 = "";
		if(buyType == 1)	//金币//
		{
			if(curUIType == UI_TYPE.UI_HEAD)
			{
				TitleLabel.text = TextsData.getData(271).chinese;
			}
			else 
			{
				TitleLabel.text = TextsData.getData(46).chinese;
			}
			
			s1 = TextsData.getData(272).chinese;
			s2 = TextsData.getData(267).chinese;
			s3 = TextsData.getData(59).chinese;
			s4 = TextsData.getData(268).chinese;
			s5 = TextsData.getData(269).chinese;
		}
		else if(buyType == 2)	//体力//
		{
			
			if(curUIType == UI_TYPE.UI_HEAD)
			{
				TitleLabel.text = TextsData.getData(276).chinese;
			}
			else 
			{
				TitleLabel.text = TextsData.getData(274).chinese;
			}
			
			s1 = TextsData.getData(272).chinese;
			s2 = TextsData.getData(267).chinese;
			s3 = TextsData.getData(62).chinese;
			s4 = TextsData.getData(268).chinese;
			s5 = TextsData.getData(269).chinese;
		}
		else if(buyType == 3)	//购买扫荡券//
		{
			
			TitleLabel.text = "";
			
			s1 = TextsData.getData(340).chinese;
			s2 = TextsData.getData(341).chinese;
			s3 = TextsData.getData(342).chinese;
			s4 = "";
			s5 = "";
		}
		else if(buyType == 4)	//购买挑战次数//
		{
			TitleLabel.text = TextsData.getData(277).chinese;
			
			s1 = TextsData.getData(288).chinese;
			s2 = TextsData.getData(267).chinese;
			s3 = TextsData.getData(269).chinese;
			s4 = TextsData.getData(268).chinese;
			s5 = TextsData.getData(269).chinese;
		}
		else if(buyType == 5)	//购买冷却时间//
		{
			TitleLabel.text = TextsData.getData(307).chinese;
			s1 = TextsData.getData(308).chinese;
			s2 = "";
			s3 = "";
			s4 = "";
			s5 = "";
		}
		else if(buyType == 6)		//购买竞技场挑战次数//
		{
			TitleLabel.text = TextsData.getData(277).chinese;
			s1 = TextsData.getData(288).chinese;
			s2 = TextsData.getData(267).chinese;
			s3 = TextsData.getData(269).chinese;
			s4 = "";
			s5 = "";
		}
		
		
		if(buyType == 3 || buyType == 6)
		{
			BuyTipDesLabel.text = s1 + costCrystal + s2 +num + s3 ;
		}
		else if(buyType == 5)
		{
			BuyTipDesLabel.text = s1 + costCrystal ;
		}
		else
		{
			
			BuyTipDesLabel.text = s1 + costCrystal + s2 +num + s3 + "\r\n"+ s4 + times +s5;
		}
		HeadUI.mInstance.refreshPlayerInfo();
	}
	
	
	public void BuyResultInit()
	{	
		//购买扫荡券后直接进行扫荡//
		if(curUIType == UI_TYPE.UI_SWEEP)			//扫荡//
		{
			//2， 体力， 3 扫荡券， 4 次数//
			if(buyType >= 2 && buyType <= 4)
			{
				hide();
				//发送界面信息请求
//				SweepUIManager.mInstance.SendToGetUIData();
				SweepUIManager sweep = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP, "SweepUIManager") as SweepUIManager;
				sweep.SendToGetUIData();
				HeadUI.mInstance.refreshPlayerInfo();
				if(buyType == 3)
				{
					if(!TalkingDataManager.isTDPC&&errorCode==0)
					{
						//记录购买扫荡卷//
						TDGAItem.OnPurchase("BuySweepMission",1,getCrystal);	
					}
				}
				return;
			}
//			//发送请求战斗数据//
//			ArenaUIManager.mInstance.SendToPK(ArenaUIManager.mInstance.curSelEmenyId);
			
		}
		else if(curUIType == UI_TYPE.UI_MISSION)//推图购买进入次数//
		{
			MissionUI mission1 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI")as MissionUI;
			if(mission1!= null)
			{
				mission1.SendToGetData();
				//string curZoneNum = mission1.GetCurZoneNum();
				//int MissionType = StringUtil.getInt(curZoneNum.Split('-')[2]);
//				if(MissionType == 1)
//				{
//					if(!TalkingDataManager.isTDPC&&errorCode==0)
//					{
//						//记录普通推图进入次数//
//						TDGAItem.OnPurchase("P-battlenumber",1,getCrystal);	
//					}
//				}
//				else if(MissionType == 2)
//				{
//					if(!TalkingDataManager.isTDPC&&errorCode==0)
//					{
//						//记录精英推图进入次数//
//						TDGAItem.OnPurchase("J-battlenumber",1,getCrystal);	
//					}
//				}
			}
			hide();
			return;
		}
		else if(buyType == 5)		//购买冷却时间//
		{
			
			if(curUIType == UI_TYPE.UI_ARENA)		//竞技场//
			{
//				ArenaUIManager.mInstance.SendToPK(ArenaUIManager.mInstance.curSelEmenyId);
				ArenaUIManager arena = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA, 
					"ArenaUIManager") as ArenaUIManager;
				arena.SendToPK(arena.curSelEmenyId);
				if(!TalkingDataManager.isTDPC&&errorCode==0)
				{
					//记录竞技场清除cd//
					TDGAItem.OnPurchase("PVPResetCD",1,getCrystal);	
				}
			}
			else if(curUIType == UI_TYPE.UI_MAZE)		//迷宫//
			{
//				WarpSpaceUIManager.mInstance.SendToIntoMaze();
				WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager") as WarpSpaceUIManager;
				warpSpace.SendToIntoMaze();
				if(!TalkingDataManager.isTDPC&&errorCode==0)
				{
					//记录迷宫清除cd//
					TDGAItem.OnPurchase("MAZEResetCD",1,getCrystal);	
				}
			}
			else if(curUIType == UI_TYPE.UI_ACTIVESEL)	//异世界-活动副本（关卡选择）//
			{
//				ActiveWroldSelManager.mInstance.SendToBattle();
				ActiveWroldSelManager activeSel = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COPYSELECT, 
					"ActiveWroldSelManager")as ActiveWroldSelManager;
				activeSel.SendToBattle();
                CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager") as CombinationInterManager;

                combination.onClickbattleBtn(0);

				if(!TalkingDataManager.isTDPC&&errorCode==0)
				{
					//记录异世界清除cd//
					TDGAItem.OnPurchase("ACTIVESELResetCD",1,getCrystal);	
				}
			}
			hide();
			return ;
		}
		else if( buyType == 6)
		{
			if(curUIType == UI_TYPE.UI_ARENA)		//竞技场//
			{
//				ArenaUIManager.mInstance.SendToPK(ArenaUIManager.mInstance.curSelEmenyId);
				ArenaUIManager arena = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA, 
					"ArenaUIManager") as ArenaUIManager;
				arena.SendToPK(arena.curSelEmenyId);
				if(!TalkingDataManager.isTDPC&&errorCode==0)
				{
					//记录竞技场购买次数//
					TDGAItem.OnPurchase("PVPBuyNum",1,getCrystal);	
				}
			}
			hide();
			return ;
		}
		BuyResultObj.SetActive(true);
		BuyTipObj.SetActive(false);
		string s1 = TextsData.getData(253).chinese;
		string s2 = "";
		if(buyType == 1)		//金币//
		{
			s2 = TextsData.getData(59).chinese;
		}
		else if(buyType == 2)	//水晶//
		{
			s2 = TextsData.getData(62).chinese;
		}
		
		BuyResultDesLabel.text = s1 + num + s2;
		HeadUI.mInstance.refreshPlayerInfo();
	}
	
	public void RefreshUIData()
	{
		//通知相应界面更新一下//
		switch(curUIType)
		{
		case UI_TYPE.UI_HEAD:
		case UI_TYPE.UI_INTENSIFY:
		case UI_TYPE.UI_BREAK:
		case UI_TYPE.UI_COMPOSE:
		case UI_TYPE.UI_ACTIVESEL:
		case UI_TYPE.UI_MAZE:
		case UI_TYPE.UI_MISSION:
		case UI_TYPE.UI_ARENA:
			HeadUI.mInstance.refreshPlayerInfo();
			
			break;
		case UI_TYPE.UI_SWEEP:
			
			break;
		case UI_TYPE.UI_SWRITEWORLD:
//			SpriteWroldUIManager.mInstance.refreshGoldInfo();
			SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, "SpriteWroldUIManager")as SpriteWroldUIManager;
			spriteWorld.refreshGoldInfo();
			break;
		}
	}

    void baseHide()
    {
        gameObject.transform.localScale = new Vector3(PANEL_SCALE_SIZE, PANEL_SCALE_SIZE, 1);
        gameObject.GetComponent<UIPanel>().alpha = 1;
        hide();
    }
	
	public override void hide ()
	{
		base.hide ();
		BuyResultObj.SetActive(false);
		BuyTipObj.SetActive(false);
//		MainMenuManager.mInstance.isCanClick = true;

		if(UISceneStateControl.mInstace!=null)
		{
			MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager")as MainMenuManager;
			if(main!=null)
			{
				
				main.isCanClick = true;
			}
			
		}
		
		RefreshUIData();
	}
	
	//购买按钮 id 0 取消， 1 购买//
	public void OnClickBuyBtn(int id)
	{
		if(id == 0)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			//hide();
            tweenAlpha(1, PANEL_ALPHA_SIZE, baseHide);
		}
		else if(id == 1)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType = 2;
			jsonType = 2;
			
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType, sweepTimes, curMd, curCDType, copyId),this);
		}
	}
	
	//0 取消， 1 再次购买//
	public void OnClickResultBuyBtn(int id)
	{
		if(id == 0)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			if(curUIType == UI_TYPE.UI_HEAD)
			{
				//刷新头像区域//
				HeadUI.mInstance.refreshPlayerInfo();
				
			}
			hide();
		}
		else if(id == 1) 
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType = 1;
			jsonType = 1;
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType, sweepTimes, curMd),this);
		}
	}
	
	public void setisMissionBuy()
	{
		isMissionBuy = true;
	}
	
	public void receiveResponse (string json)
	{
		Debug.Log("buyTipManager json : " + json);
		if(json != null)
		{
			receiveData = true;
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:			//请求界面信息//
				BuyPowerOrGoldResultJson brj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj.errorCode;
				this.costCrystal = brj.crystal;
				this.num = brj.num;
				this.times = brj.times;
				break;
			case 2:			//确定购买//
				BuyPowerOrGoldResultJson brj2 = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj2.errorCode;
				if(errorCode == 0)
				{
					int oldCrystal = PlayerInfo.getInstance().player.crystal;
					PlayerInfo.getInstance().player.crystal = brj2.crystal;
					getCrystal = oldCrystal - brj2.crystal;
					if(buyType == 1)
					{
						//int oldGold = PlayerInfo.getInstance().player.gold;
						PlayerInfo.getInstance().player.gold = brj2.num;
						//getGold = brj2.num - oldGold;
						receiveByeNum = true;
					}
					else if(buyType == 2)
					{
						//int oldPower = PlayerInfo.getInstance().player.power;
						PlayerInfo.getInstance().player.power = brj2.num;
						//getPower = brj2.num - oldPower ;
						receiveByeNum = true;
					}
					else if(buyType == 4)
					{
						receiveByeNum = true;
					}
				}
				break;
			}
		}
	}
}
