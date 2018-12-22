using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SweepUIManager : MonoBehaviour,ProcessResponse {
	
//	public static SweepUIManager mInstance;
	public UISprite missionIcon;		//关卡的头像//
	public UILabel missionName;			//关卡名称//
	public UILabel	powerLabel;			//消耗体力//
	public UILabel battleNumLabe;		//可挑战次数//
	public UILabel itemNumLabel;		//扫荡券个数//
	public UILabel sweepNumLabel;		//连续扫荡次数//
	public GameObject Btn_Sweep;		//扫荡//
	public GameObject Btn_Sweep10;		//扫n次//
	public GameObject GridList;
	public GameObject ScrollBar;
	public GameObject ScrollView;
	public GameObject CGLevelUp;		//军团升级界面//
	public GameObject RewardsDataObj;	//获得物品详细信息界面//
	
	UIButtonMessage ubm_sweep10;
	
	private Transform _myTransform;
	//1 开始扫荡，2 请求购买信息， 3 请求界面信息//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	
	string DesItemPath = "Prefabs/UI/UI-Sweep/SweepDesItem";
	string SweepItemPath = "Prefabs/UI/UI-Sweep/SweepItem";
	string SweepResultPath = "Prefabs/UI/UI-Sweep/SweepResultItem";
	GameObject DesItem;
	GameObject SweepItem;
	GameObject BattleCardItem;
	
	//当前关卡的id//
	private int missionId;
	//场次信息//
	private int bNum;
	private int costPower;
	private int battleNum;
	private int itemNum;
	private int sweepNum;
	private int curMissionId;
	private int curClickSweepNum;
	
	/************************扫荡结果数据start***************************/
	//站前//
	private int lv0;				//军团等级//
	private int power0;
	private List<string> cs0 = new List<string>();	//卡组信息//
	//private int ce0;				//经验值//
	
	//战后//
	private int lv1;
	private int power1;
	private List<string> cs1 = new List<string>();
	private int ce1;				
	
	//扫荡信息//
	private List<SweepCardJson> sweepInfo = new List<SweepCardJson>();		//扫荡之后的军团，人物以及掉落物品//
	
	/************************扫荡结果数据end***************************/
	
	/************************请求购买购买start***************************/
	//购买物品的类型 1 金币， 2 体力, 3 扫扫荡券， 4 挑战次数//
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
	/************************请求购买购买end***************************/
	
	bool isRunningSweep = false;
	//在开始扫荡之后就不许点击扫荡按钮//
	bool isCanClickSweepBtn = true;
	int curShowRewardNum = 0;
	float startY = 0;
	float offy = -220;
	SweepUiResultJson sweepUiResult;
	public NewUnitSkillResultJson nusrj;
	
	bool isWaitRefreshUIData = false;
	void Awake()
	{
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		init();
//		hide();
		_myTransform = transform ;
	}
	
	public void init ()
	{
//		base.init ();
		DesItem =  Resources.Load(DesItemPath) as GameObject;
		SweepItem =  Resources.Load(SweepItemPath) as GameObject;
		BattleCardItem = Resources.Load(SweepResultPath) as GameObject;
		if(ubm_sweep10 == null)
		{
			ubm_sweep10 = Btn_Sweep10.GetComponent<UIButtonMessage>();
		}
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(curState == 1 )
		{
			if(curShowRewardNum > 0 && curShowRewardNum <= curClickSweepNum + 1 && !isRunningSweep)
			{
				
				DrawRewards();
			}
			else if(curShowRewardNum > curClickSweepNum + 1)
			{
				if(lv0 < lv1)
				{
					Invoke("DrawPlayerLevelUp", 1.5f);
					curState = 3;
				}
				checkIsCanRunGuide();
				
			}
		}
		
		
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:				//请求扫荡//
				
				if(errorCode == 0)
				{
					CleanGridData();
					ShowInterface(1);
					HeadUI.mInstance.refreshPlayerInfo();
				}
				else if(errorCode == 83)		//扫荡券不足//
				{
					requestType = 2;
					jsonType = 1;			//请求购买界面信息//
					costType = 1;			//花费类型：1 水晶，2金币//
					buyType = 3;			//购买的物品的类型 ·1 金币， 2 体力,  3 购买扫荡券， 4 购买战斗次数//
					PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType, curClickSweepNum),this);
				}
				else if(errorCode == 50)		//挑战次数已达上限//
				{
					//判断vip等级是否可以购买扫荡次数//
					VipData vd = VipData.getData(PlayerInfo.getInstance().player.vipLevel);
					MissionData md=MissionData.getData(missionId);
					if(md!=null && vd!=null)
					{
						if(md.missiontype==1)
						{
							requestType = 2;
							buyType = 4;
							jsonType = 1;
							costType = 1;
							PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType, 0, missionId ),this);
						}
						else
						{
							if(vd.hero>0)
							{
								requestType = 2;
								buyType = 4;
								jsonType = 1;
								costType = 1;
								PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType, 0, missionId ),this);
							}
							else
							{
								string str = TextsData.getData(500).chinese;
//								ToastWindow.mInstance.showText(str);
								//提示去充值//
								UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
							}
						}
					}
				}
				else if(errorCode == 27)		//体力不足//
				{
					requestType = 2;
					buyType = 2;
					jsonType = 1;
					costType = 1;
					PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType),this);
				}
				else if(errorCode == 53)		//背包空间不足且没有达到上限//
				{
//					string s = TextsData.getData(78).chinese;
//					ToastWindow.mInstance.showText(s);
					string str = TextsData.getData(78).chinese;
					UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, str);
				}
				else if(errorCode == 131)		//背包空间不足且达到上限//
				{
					string str = TextsData.getData(78).chinese;
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, str);
				}
				else if(errorCode == 88)				//改关卡未完成3星无法扫荡//
				{
					string str = TextsData.getData(356).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 70)				//vip等级不足//
				{
//					string str = TextsData.getData(243).chinese;
					
					
					string str = "";
					if(curClickSweepNum > 1)		//扫多次提示//
					{
						str = TextsData.getData(480).chinese;
					}
					else if(curClickSweepNum == 1)		//扫单次//
					{
						
						str = TextsData.getData(479).chinese;
					}
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				break;
			case 2:				//请求购买//
				if(errorCode == 0)
				{
					
					BuyTipManager.mInstance.SetData(buyType, costType , costCrystal, num, times, curClickSweepNum, BuyTipManager.UI_TYPE.UI_SWEEP, missionId);
				}
				else if(errorCode == 51)		//体力达到上限//
				{
					string str = TextsData.getData(270).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 79)		//购买次数达到上限//
				{
					string str = TextsData.getData(240).chinese;
					ToastWindow.mInstance.showText(str);
				}
				break;
			case 3:								//请求界面信息//
				if(errorCode == 0)
				{
					initUIData(sweepUiResult);
					
				}
				else 
				{
					sweepUiResult = null;
				}
				break;
			case 4:	
				if(nusrj.errorCode == 0)
				{
					hide();
					//删除当前ui//
					UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP);
			//		MissionUI.mInstance.ChangeCellInfoTimes(battleNum, missionId);
			//		MissionUI.mInstance.SendToGetData();
					MissionUI mission1 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI")as MissionUI;
					if(mission1!= null)
					{
						mission1.SendToGetData();
					}
					if(!nusrj.unitskills.Equals(""))
					{
						UniteSkillUnlockManager.mInstance.SetDataAndShow(nusrj.unitskills,mission1.gameObject);	
					}
				}
				break;
			}
		}
	}
	int curState;
	//绘制界面state0,第一次进入后的界面信息//
	public void ShowInterface(int state)
	{
		curState = state;
		switch(state)
		{
		case 0:				//显示界面信息//
			isCanClickSweepBtn = true;
			DrawSweepScene();
			break;
		case 1:				//绘制扫荡结果界面//
			isCanClickSweepBtn = false;
			DrawSweepResultScene();
			break;
		case 2:				//绘制购买信息//
			
			break;
		case 3:				//绘制军团升级界面//
			DrawPlayerLevelUp();
			break;
		}
	}
	
	public void DrawSweepScene()
	{
		//清空scrollView数据//
		CleanScrollViewData();
		DrawBaseData();
		
		//显示详细信息//
		GameObject des = Instantiate(DesItem)as GameObject;
		GameObjectUtil.gameObjectAttachToParent(des, GridList);
		UILabel label = des.GetComponentInChildren<UILabel>();
		string s1 = TextsData.getData(325).chinese;
		label.text = s1;
		
		//设置信息//
		UIButtonMessage ubm_sweep = Btn_Sweep.GetComponent<UIButtonMessage>();
		ubm_sweep.target = _myTransform.gameObject;
		ubm_sweep.functionName = "OnClickSweepBtn";
		ubm_sweep.param = 1;
		
		
		ubm_sweep10.target = _myTransform.gameObject;
		ubm_sweep10.functionName = "OnClickSweepBtn";
		ubm_sweep10.param = sweepNum;
	}
	
	public void DrawBaseData()
	{
		MissionData md = MissionData.getData(missionId);
		missionIcon.spriteName = md.bossicon;
		string atlasName = CardData.getAtlas(md.bossicon);
		UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(atlasName);
		missionIcon.atlas = iconAtlas;
		
		missionName.text = md.name;
		string s1 = TextsData.getData(13).chinese;			//体力值消耗//
		powerLabel.text = s1 + costPower;
		s1 = TextsData.getData(329).chinese;				//可挑战次数//
		battleNumLabe.text = s1.Replace("num", battleNum.ToString());
		itemNumLabel.text = itemNum.ToString();
		s1 = TextsData.getData(327).chinese;				//扫n次//
		sweepNumLabel.text = s1.Replace("num", sweepNum.ToString());
		
		if(ubm_sweep10!=null)
		{
			ubm_sweep10.param = sweepNum;
		}
		
	}
	
	public void DrawSweepResultScene()
	{
		DrawBaseData();
		CleanScrollViewData();
		curShowRewardNum = 0;
		DrawRewards();
	}
	
	public void DrawRewards()
	{
		//一轮一轮开始绘制//
		if(curShowRewardNum < curClickSweepNum)
		{
			
			isRunningSweep = true;
			GameObject sweepResult = Instantiate(SweepItem)as GameObject;
			GameObjectUtil.gameObjectAttachToParent(sweepResult, GridList);
			sweepResult.transform.localPosition = new Vector3(0, startY + curShowRewardNum * offy , 0);
			SweepItemControl sic = sweepResult.GetComponent<SweepItemControl>();
			sic.SetData(curShowRewardNum + 1, sweepInfo[curShowRewardNum]);
			
			//添加卡牌获得统计@zhangsai//
			for(int i =0;i<sweepInfo[curShowRewardNum].ds.Count;i++)
			{
				string[] ss = sweepInfo[curShowRewardNum].ds[i].Split('-');
				if(StringUtil.getInt(ss[0]) == 3)
				{
					int id = StringUtil.getInt(ss[1].Split(',')[0]);
					if(!UniteSkillInfo.cardUnlockTable.ContainsKey(id))
						UniteSkillInfo.cardUnlockTable.Add(id,true);
				}
			}
		}
		else if(curShowRewardNum == curClickSweepNum)		//显示卡牌经验界面//
		{
			isRunningSweep = false;
			if(lv0 == lv1)
			{
				isCanClickSweepBtn = true;
			}
			GameObject battleCard = Instantiate(BattleCardItem)as GameObject;
			GameObjectUtil.gameObjectAttachToParent(battleCard, GridList);
			battleCard.transform.localPosition = new Vector3(0, curShowRewardNum * offy + offy/2, 0);
			SweepResultItem sri = battleCard.GetComponent<SweepResultItem>();
			//循环绘制卡牌//
			for(int i = 0; i < sri.BattleCards.Length;i++)
			{
				GameObject card = sri.BattleCards[i];
				card.SetActive(false);
			}
			
			for(int i = 0; i < cs0.Count;i++)
			{
				string[] str = cs0[i].Split('-');
				int cardId = StringUtil.getInt(str[0]);
				int lastLevel = StringUtil.getInt(str[1]);
				int lastExp = StringUtil.getInt(str[2]);
				
				string[] str1 = cs1[i].Split('-');
				int curLevel = StringUtil.getInt(str1[1]);
				int curExp = StringUtil.getInt(str1[2]);
				
				
				GameObject card = sri.BattleCards[i];
				card.SetActive(true);
				
				ResultCardManager rcm = card.GetComponent<ResultCardManager>();
				//rcm.Icon.gameObject.SetActive(true);
				rcm.cardInfo.gameObject.SetActive(true);
				rcm.cardLevel.gameObject.SetActive(true);
				rcm.LevelUpEff.SetActive(false);
				rcm.CardExp.gameObject.SetActive(true);
				
				CardData cd = CardData.getData(cardId);
				rcm.cardInfo.setSimpleCardInfo(cardId,GameHelper.E_CardType.E_Hero);
				rcm.cardLevel.text = "LV." + lastLevel;
				rcm.CardExp.setData02(STATE.EXP_TYPE_RESULT_CARD, lastExp, lastLevel, curExp, curLevel, cd.star, null, i, rcm);
			}
		}
		
		curShowRewardNum++;
		if(curShowRewardNum > 2)
		{
			ScrollBar.GetComponent<UIScrollBar>().value = 1;
		}
	}
	
	public void SetScrollBar()
	{
		if(curShowRewardNum > 2)
		{
			ScrollBar.GetComponent<UIScrollBar>().value = 1;
		}
	}
	
	
	//绘制人物升级界面//
	public void DrawPlayerLevelUp()
	{
		isCanClickSweepBtn = true;
		CGLevelUp.SetActive(true);		
		ResultPlayerLUItem PlayerLevelUpObj = CGLevelUp.GetComponent<ResultPlayerLUItem>();
		
		PlayerLevelUpObj.lastLevel.text = lv0.ToString();
		PlayerLevelUpObj.curLevel.text = lv1.ToString();
		
		PlayerLevelUpObj.lastPower.text = power0.ToString();
		PlayerLevelUpObj.curPower.text = power1.ToString();
		
		int lastQH = lv0 * 3;
		int curQH = lv1 * 3;
		PlayerLevelUpObj.lastQH.text = lastQH.ToString();
		PlayerLevelUpObj.curQH.text = curQH.ToString();
		
		int lastCG = 3;
		int curCG = 3;
		for (int i = 0;i < 3;i ++)
		{
			UnlockData uld = UnlockData.getData(30 + i);
			if(lv0 >= uld.method)
			{
				lastCG = 4 + i;
			}
			if(lv1 >= uld.method)
			{
				curCG = 4 + i;
			}
		}
		if(lastCG != curCG)
		{
			PlayerLevelUpObj.lastCG.text = lastCG.ToString();
			PlayerLevelUpObj.curCG.text = curCG.ToString();
		}
		else 
		{
			PlayerLevelUpObj.lastCG.text = string.Empty;
			PlayerLevelUpObj.curCG.text = string.Empty;
			PlayerLevelUpObj.TitleCG.text = string.Empty;
			PlayerLevelUpObj.ShowCGData.SetActive(false);
		}
		
		//创建背景特效//
//		if(LevelUpBgEffect != null)
//		{
//			Destroy(LevelUpBgEffect);
//		}
//		if(LevelUpBgEffect == null)
//		{
//			LevelUpBgEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_starbackground",1)) as GameObject;
//		}
//		GameObjectUtil.gameObjectAttachToParent(LevelUpBgEffect,PlayerLevelUpObj.gameObject);
//		GameObjectUtil.setGameObjectLayer(LevelUpBgEffect,PlayerLevelUpObj.gameObject.layer);
		
		
		
		if(SDKManager.getInstance().isSDKCPYYUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				SDK_CPYY_manager.sdk_call_gameNotify(3+"",lv1.ToString(),"levelup");
			}
		}
	}
	
	void checkIsCanRunGuide()
	{
		// unlock guide
		int curGuideID = GuideManager.getInstance().getCurrentGuideID();
		switch(curGuideID)
		{
		case (int)GuideManager.GuideType.E_UnlockCompose:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Compose))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_ActiveCopy:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_ActiveCopy))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_WarpSpace:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_WarpSpace))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_PVP:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_PVP))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_Rune:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Rune))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_UnlockBreak:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Break))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_Spirit:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Spirit))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		}
	}

	public void show ()
	{
//		base.show ();
	}
	
	public void SetData(SweepUiResultJson surj)
	{
		show();
		initUIData(surj);
	}
	
	public void initUIData(SweepUiResultJson surj)
	{
		this.costPower = surj.power;
		this.battleNum = surj.entryTimes;
		this.itemNum = surj.itemNum;
		this.sweepNum = surj.sweepTimes;
		this.missionId = surj.md;
		this.bNum = surj.bNum;
		ShowInterface(0);
	}
	
	public void hide ()
	{
//		base.hide ();
		DesItem =  null;
		SweepItem =  null;
		BattleCardItem = Resources.Load(SweepResultPath) as GameObject;
		ubm_sweep10 = null;
		_myTransform .gameObject.SetActive(false);
		CleanScrollViewData();
		CGLevelUp.SetActive(false);
	}
	
	public void CleanGridData()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
	}
	
	public void CleanScrollViewData()
	{
		ScrollBar.GetComponent<UIScrollBar>().value = 1;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,450,320);
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
	}
	
	public void SetDrawSweep(bool isRunning)
	{
		isRunningSweep = isRunning;
	}
	
	public void OnClickSweepBtn(int times)
	{
		if(isCanClickSweepBtn&&!HeadUI.mInstance.wairRequestPlayerInfo && !isWaitRefreshUIData)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(times <= 0)
			{
				times = 1;
			}
			curClickSweepNum = times;
			SendToSweep(times);
			curState = -1;
		}
	}
	
	public void OnClickCloseBtn()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
		//向服务器请求判断是否有新解锁合体技//
		{
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
		}
	}
	
	//点击军团升级界面按钮//
	public void OnClickLUBtn()
	{
		
		CGLevelUp.SetActive(false);	
	}
	
	//开始扫荡 times扫荡的次数//
	public void SendToSweep(int times)
	{
		
		int level = VipData.getLevelForSweep(times);
		if(PlayerInfo.getInstance().player.vipLevel >= level)
		{
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new SweepJson(missionId, times, bNum),this);
		}
		else 		//vip等级不足无法扫荡//
		{
			string str = "";
			if(times > 1)		//扫多次提示//
			{
				str = TextsData.getData(480).chinese;
			}
			else if(times == 1)
			{
				
				str = TextsData.getData(479).chinese;
			}
//			ToastWindow.mInstance.showText(str);
			//提示去充值//
			UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
		}
	}
	
	
	//获取扫荡界面信息//
	public void SendToGetUIData()
	{
		requestType = 3;
		isWaitRefreshUIData = true;
		PlayerInfo.getInstance().sendRequest(new SweepUiJson(missionId),this);
	}
	
	public void receiveResponse (string json)
	{
		if(json != null)
		{
			Debug.Log("sweepUIManager : json ======================= " + json);
			receiveData = true;
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:				//请求扫荡//
				SweepResultJson srj = JsonMapper.ToObject<SweepResultJson>(json);
				errorCode = srj.errorCode;
				if(errorCode == 0)
				{
					
					cs0.Clear();
					cs1.Clear();
					sweepInfo.Clear();
					
					this.lv0 = srj.lv0;
					this.cs0 = srj.cs0;
					//this.ce0 = srj.ce0;
					this.lv1 = srj.lv1;
					this.cs1 = srj.cs1;
					this.ce1 = srj.ce1;
					this.sweepInfo = srj.sweepInfo;
					this.costPower = srj.power;
					this.battleNum = srj.entryTimes;
					this.itemNum = srj.itemNum;
					this.sweepNum = srj.sweepTimes;
					this.power0 = srj.power0;
					this.power1 = srj.power1;
					PlayerInfo.getInstance().player.level = lv1;
					PlayerInfo.getInstance().player.curExp = ce1;
					PlayerInfo.getInstance().player.power = power1;
					PlayerInfo.getInstance().SetUnLockData(srj.s);
				}
			
				break;
			case 2:						//请求购买信息//
				BuyPowerOrGoldResultJson brj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj.errorCode;
				this.costCrystal = brj.crystal;
				this.num = brj.num;
				this.times = brj.times;
				break;
			case 3:						//请求扫荡界面信息//
				SweepUiResultJson surj = JsonMapper.ToObject<SweepUiResultJson>(json);
				errorCode = surj.errorCode;
				sweepUiResult = surj;
				isWaitRefreshUIData = false;
				break;
				
			case 4:						//判断是否解锁新的合体技//
				nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
				errorCode = nusrj.errorCode;
				receiveData = true;
				break;
			}
		}
	}
}
