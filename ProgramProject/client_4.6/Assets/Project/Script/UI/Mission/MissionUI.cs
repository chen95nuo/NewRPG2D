using UnityEngine;
using System.Collections.Generic;

public class MissionUI : MonoBehaviour ,ProcessResponse{
	
	/**
	 * 控制推图界面弹窗
	 */
	public GameObject mapModel1;
	public GameObject mapModel2;
	public GameObject popwindow;
	public GameObject missions;
	public GameObject missionCells;
	public GameObject missionParent;
	public GameObject missionScrollBar;
	public GameObject missionDetail;
	public GameObject helpers;
	public GameObject helperCells;
	public GameObject helperParent;
	public GameObject helperScrollBar;
	public UILabel powerLabel;
	public UILabel zoneName;
	public GameObject star3Btn;
	public UILabel star3RewardNum;
	public UILabel star3Num;
	public UILabel freeTimes; 
	public UILabel totalTimes;
	public UILabel helperDes;
	
	public GameObject enemyWindow;
	public GameObject enemyBtn;
    public GameObject koCardInfo;
	//掉落物品查看界面//
	public GameObject DropInfoPanel;
//	public MissionDropItemInfo[] dropsItemInfoList;
	public GameObject MissionDropDragPanel;
	public GameObject dropScrollBar;
	public GameObject DragParent;
	public GameObject MissionDropItem;
	public GameObject back;
	public UISprite newKoAwardTip;
	public UISprite newCombinationTip;
	
	private GameObject enemyCardSample;
	/**关卡内容**/
	private GameObject loadMissionCell;
	private GameObject loadHelpCell;
	/**1选关,2选好友**/
	public int curStep;
	public MapResultJson mrj;
	private Transform _myTransform;
	private int curMissionId;
	private int curHelperId;
	private string cur3StarParam;
	private List<FriendElement> helperList;
	private int todayInviteTimes;
	private int nextInviteCost;
	/**1请求援护者,2请求战斗数据, 5 扫荡, 6 获取任务奖励信息, 7 获取界面信息**/
	private int requestType;
	private bool receiveData;
	private int errorCode;
	
	bool finishRequestHelp = true;
	bool needShowGuideByFinishBattle = false;
	public GameObject pointMapModel2;
	//==0返回主界面,1返回合成界面==, 2 返回卡拍详细信息界面 3 从阵容界面过来（删除阵容界面），返回主界面 ,4 从合体技界面过来//
	public int backType;
	private SweepUiResultJson sweepUiResult;
	private string curZoneNum;
	//cxl--3星奖励的钻石数//
	private int cur3StarNum;
	//卡组界面的json//
	private CardGroupResultJson cardGroupRJ;
	MissionUI2 mission2;
	
    public KoAward koaward;
	public UILabel koExchangeName;
	public UILabel haveKoNum1;
	public UILabel haveKoNum2;
	public UILabel koIntroduce;
	public GameObject koExchangeParent;
	public KOExchangeResultJson koerj;
	private GameObject loadKoCell;
	private Vector3 missionsInitPos;
	private Vector4 missionsInitClip;
	private GameObject star3Effect;
	private bool star3EffectStatus;
    public GameObject fogs;
	
	public static bool isGetNewCard = false;
	private bool isCanShowUnlockUnitSkill = false;
	public NewUnitSkillResultJson nusrj;

	void Awake(){
		_myTransform = transform ;
		if(mission2 == null)
		{
			mission2 = _myTransform.GetComponent<MissionUI2>();
		}
		missionsInitPos=missionCells.transform.localPosition;
		missionsInitClip=missionCells.GetComponent<UIPanel>().clipRange;
		helperDes.text = TextsData.getData(528).chinese;
	}
	
	public void show()
	{
		finishRequestHelp = true;
		if(mrj==null)
		{
			mrj=PlayerInfo.getInstance().mrj;
		}
		popwindow.SetActive(false);
		if(PlayerInfo.getInstance().player.missionId < 110407)
		{
            //mapModel1.SetActive(false);
            //mapModel2.SetActive(false);
		}
		else
		{
            //mapModel1.SetActive(true);
            //mapModel2.SetActive(true);
			if(PlayerInfo.getInstance().player.missionId2 == 1 && mission2.curMissionType != 2)
			{
				pointMapModel2.SetActive(true);
			}
			else
			{
				pointMapModel2.SetActive(false);	
			}
		}
        koCardInfo.SetActive(false);
		DropInfoPanel.SetActive(false);
//		for(int i = 0 ;i < dropsItemInfoList.Length;i++)
//		{
//			MissionDropItemInfo item = dropsItemInfoList[i];
//			item.gameObject.SetActive(false);
//		}
		if(mrj.koType == 1)
		{
			newKoAwardTip.gameObject.SetActive(true);
		}
		else if(mrj.koType == 0)
		{
			newKoAwardTip.gameObject.SetActive(false);
		}
	}
	
	public void baseShow()
	{
//		base.show();
		UISceneStateControl.mInstace.ShowObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
	}
	
	public void showFromCompose(int missionId, int backT = 1)
	{
		show();
		MissionData md=MissionData.getData(missionId);
		onClickZone(md.map+"-"+md.zone+"-"+md.missiontype);
		backType=backT;
	}
	
	public void BaseHide()
	{
		UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
	}
	
	
	public void hide()
	{
//		base.hide();
		closePop();
		gc();
//		MissionUI2.mInstance.hide();
		mission2.hide();
		
		if(UISceneStateControl.mInstace != null)
		{
			UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		}
	}
	
	private void gc()
	{
		loadKoCell = null;
		loadMissionCell=null;
		loadHelpCell=null;
		mrj=null;
		if(helperList!=null)
		{
			helperList.Clear();
			helperList=null;
		}
		enemyCardSample=null;
		Resources.UnloadUnusedAssets();
	}
	
	// Use this for initialization
	void Start () {
		if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_MAP)
		{
//			MainMenuManager.mInstance.hide();
			//隐藏主城//
			if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
			{
				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			}
			show();
			//播放声音//
			string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MISSION).music;
			MusicManager.playBgMusic(musicName);
		
			needShowGuideByFinishBattle = true;
		}
		else
		{
//			hide();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(finishRequestHelp)
		{
			if(needShowGuideByFinishBattle)
			{
				needShowGuideByFinishBattle = false;
				isCanShowUnlockUnitSkill = false;
				checkIsCanRunGuide();
			}
			else
			{
				isCanShowUnlockUnitSkill = true;
			}
		}
		
		if(isGetNewCard)
		{
			isGetNewCard = false;
			//向服务器请求判断是否有新解锁合体技//
			if((!GuideManager.getInstance().isGuideRunning())&&isCanShowUnlockUnitSkill)
			{
				requestType = 8;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_UNITESKILL_UNLOCK), this);
			}
		}
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			
			switch(requestType)
			{
			case 1:
				curStep=2;
				missions.SetActive(false);
				helpers.SetActive(true);
				showHelpers();
				finishRequestHelp = true;
				break;
			case 2:
				if(errorCode==27)			//体力不足//
				{
					int buyType = 2;
					int jsonType = 1;
					int costType = 1;
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_MISSION);
					return;
				}
				if(errorCode==89)			//金币不足//
				{
					int buyType = 1;
					int jsonType = 1;
					int costType = 1;
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_MISSION);
					return;
				}
				if(errorCode==50)			//挑战次数不足//
				{
					CheckToBuyBattleNum();
					return;
				}
				if(errorCode==53)			//背包空间不足且没有达到上限//
				{
					string str = TextsData.getData(78).chinese;
					//UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, str);
					UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, str);
									//==清空援护玩家,下次战斗重新请求==//
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
					{
						if(GuideUI_UintSkill.mInstance != null && GuideUI_UintSkill.mInstance.isVisible())
						{
							GuideUI_UintSkill.mInstance.hide();
						}
					}
					else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
					{
						if(GuideUI_Bounes.mInstance != null && GuideUI_Bounes.mInstance.isVisible())
						{
							GuideUI_Bounes.mInstance.hide();
						}
					}
					else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle3_Friend))
					{
						if(GuideUI_Friend.mInstance != null && GuideUI_Friend.mInstance.isVisible())
						{
							GuideUI_Friend.mInstance.hide();
						}
					}
					return;
				}
				if(errorCode == 131)		//背包空间不足且达到上限//
				{
					string str = TextsData.getData(78).chinese;
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, str);
									//==清空援护玩家,下次战斗重新请求==//
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
					{
						if(GuideUI_UintSkill.mInstance != null && GuideUI_UintSkill.mInstance.isVisible())
						{
							GuideUI_UintSkill.mInstance.hide();
						}
					}
					else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
					{
						if(GuideUI_Bounes.mInstance != null && GuideUI_Bounes.mInstance.isVisible())
						{
							GuideUI_Bounes.mInstance.hide();
						}
					}
					else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle3_Friend))
					{
						if(GuideUI_Friend.mInstance != null && GuideUI_Friend.mInstance.isVisible())
						{
							GuideUI_Friend.mInstance.hide();
						}
					}
					return;
				}
				if(errorCode==65)			//当前出战阵容里没有可出战的卡！//
				{
					ToastWindow.mInstance.showText(TextsData.getData(198).chinese);
					return;
				}
				if(errorCode==14)			//完成普通关卡后才可以进入精英关卡哦//
				{
					ToastWindow.mInstance.showText(TextsData.getData(547).chinese);
					return;
				}
				if(errorCode!=0)
				{
					ToastWindow.mInstance.showText("errorCode:"+errorCode);
					return;
				}
				//==清空援护玩家,下次战斗重新请求==//
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
				{
					if(GuideUI_UintSkill.mInstance != null && GuideUI_UintSkill.mInstance.isVisible())
					{
						GuideUI_UintSkill.mInstance.hide();
					}
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
				{
					if(GuideUI_Bounes.mInstance != null && GuideUI_Bounes.mInstance.isVisible())
					{
						GuideUI_Bounes.mInstance.hide();
					}
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle3_Friend))
				{
					if(GuideUI_Friend.mInstance != null && GuideUI_Friend.mInstance.isVisible())
					{
						GuideUI_Friend.mInstance.hide();
					}
				}
				MissionData md = MissionData.getData(curMissionId);
				if(!TalkingDataManager.isTDPC)
				{
					TDGAMission.OnBegin(md.name);
				}
				hide();
				GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
				return;
			case 3:			//进入卡组界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
				CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
				combination.curCardGroup=cardGroupRJ .transformCardGroup();
				combination.SetData(3);
				
				cardGroupRJ = null;
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				BaseHide();
				mission2.baseHide();
				break;
			case 4:				//兑换3星奖励//
				if(errorCode==63)		//3星奖励条件还未达成//
				{
					ToastWindow.mInstance.showText(TextsData.getData(194).chinese);
					return;
				}
				if(errorCode==64)		//3星奖励已领过//
				{
					ToastWindow.mInstance.showText(TextsData.getData(195).chinese);
					return;
				}
				AllThreeStarData ad=AllThreeStarData.getData(cur3StarParam);
				if(ad.rewardtype==6)
				{
					//成功领取numname！//
					ToastWindow.mInstance.showText(TextsData.getData(196).chinese.Replace("num",ad.reward+"").Replace("name",TextsData.getData(59).chinese));
				}
				else
				{
					ToastWindow.mInstance.showText(TextsData.getData(196).chinese.Replace("num",ad.reward+"").Replace("name",TextsData.getData(49).chinese));
				}
				//已领取//
				star3Btn.transform.FindChild("label").GetComponent<UILabel>().text=TextsData.getData(397).chinese;
				UIButtonMessage3 msg=star3Btn.GetComponent<UIButtonMessage3>();
				msg.target=_myTransform.gameObject;
				msg.functionName="onClick3StarReward";
				msg.stringParam="-1";
				HeadUI.mInstance.refreshPlayerInfo();
				powerLabel.text=PlayerInfo.getInstance().player.power+"/"+PlayerInfo.getInstance().player.sPower;
				star3Effect.SetActive(false);
				
				//副本3星奖励//
				if(!TalkingDataManager.isTDPC && errorCode == 0)
				{
					TDGAVirtualCurrency.OnReward(cur3StarNum, "threestaraward");
				}
				
				break;
			case 5:					//请求扫荡界面数据，进入界面//
				if(errorCode == 0)
				{
//					SweepUIManager.mInstance.SetData(sweepUiResult);
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP);
					SweepUIManager sweep = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP, "SweepUIManager") as SweepUIManager;
					sweep.SetData(sweepUiResult);
					closeStar3Effect();
				}
				else if(errorCode == 70)				//vip等级不足//
				{
					string str = TextsData.getData(243).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else 
				{
					sweepUiResult = null;
					if(errorCode == 88)				//改关卡未完成3星无法扫荡//
					{
						string str = TextsData.getData(356).chinese;
						ToastWindow.mInstance.showText(str);
					}
				}
				break;
			case 6:			//ko兑换界面//
				if(errorCode == 0)
				{
					koExchangeName.text = TextsData.getData(405).chinese;
					koIntroduce.text = TextsData.getData(408).chinese;
//					haveKoNum1.text = TextsData.getData(406).chinese;
					haveKoNum2.text = koerj.point.ToString();
					showKoExchange();
					koaward.show();
				}
				break;
			case 7:			//获取界面信息//
				showMissions(curZoneNum);
				openStar3Effect();
				break;
			case 8:	
				if(nusrj.errorCode == 0)
				{
					if(!nusrj.unitskills.Equals(""))
					{
						UniteSkillUnlockManager.mInstance.SetDataAndShow(nusrj.unitskills,gameObject);	
					}
				}
				break;
			}
		}
		
		//==若开启了新的关卡，则自动前往新的关卡处==//
		//==若未开启新的关卡，则自动返回本关卡的列表==//
		int lastMissionId=PlayerInfo.getInstance().lastMissionId;
		if(lastMissionId!=0 && PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_MAP)
		{
			PlayerInfo.getInstance().lastMissionId=0;
			PlayerInfo.getInstance().BattleOverBackType=0;
			MissionData md=MissionData.getData(lastMissionId);
			onClickZone(md.map+"-"+md.zone+"-"+md.missiontype);
		}
	}
	
	//判断vip等级去购买挑战次数//
	public void CheckToBuyBattleNum()
	{
		VipData vd = VipData.getData(PlayerInfo.getInstance().player.vipLevel);
		string[] ss=curZoneNum.Split('-');
		int missionType=StringUtil.getInt(ss[2]);
		int needVipNum = 0;
		if(missionType == 1)		//普通关卡//
		{
			needVipNum = vd.normal;
		}
		else if(missionType == 2)	//精英关卡//
		{
			needVipNum = vd.hero;
		}
		if(needVipNum > 0)			//普通关卡可直接购买//
		{
			int buyType = 4;
			int jsonType = 1;
			int costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, curMissionId, BuyTipManager.UI_TYPE.UI_MISSION);
		}
		else 			//普通副本没有<=0的情况//
		{
			//vip等级不足，vip4时可以重置精英副本！//
			string str = TextsData.getData(500).chinese;
//			ToastWindow.mInstance.showText(str);
			//提示去充值//
			UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
		}
		
	}
	
	public void checkIsCanRunGuide()
	{
		// force guide 
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_LotCard))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}
		else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
		{
			GuideUI_Bounes.mInstance.hideAllStep();
			UISceneDialogPanel.mInstance.showDialogID(38);
		}
		else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_KO_Exchange))
		{
			if(!GuideUI7_KOExchange.mInstance.finishExchange)
			{
				GuideUI7_KOExchange.mInstance.showStep(1);
			}
		}
		
		// unlock guide
		int curGuideID = GuideManager.getInstance().getCurrentGuideID();
		switch(curGuideID)
		{
		case (int)GuideManager.GuideType.E_IntensifyCard:
		{
			if(GuideUI_Intesnify.mInstance.runningStep == -1)
			{
				if(RequestUnlockManager.mInstance.isCanUnlockFunctionByFinishMissionID((int)RequestUnlockManager.MODELID.E_Intensify))
				{
					RequestUnlockManager.mInstance.showUnlockPanel((int)RequestUnlockManager.MODELID.E_Intensify);
					GuideManager.getInstance().runGuide();
				}
			}
		}break;
		case (int)GuideManager.GuideType.E_Achievement:
		{
			if(RequestUnlockManager.mInstance.isCanUnlockFunctionByFinishMissionID((int)RequestUnlockManager.MODELID.E_Achievement))
			{
				RequestUnlockManager.mInstance.showUnlockPanel((int)RequestUnlockManager.MODELID.E_Achievement);
				GuideManager.getInstance().runGuide();
			}
		}break;
		case (int)GuideManager.GuideType.E_Equip:
		{
			if(PlayerInfo.getInstance().player.missionId >= GuideManager.getInstance().finishGiveEquipMissionID)
			{
				GuideManager.getInstance().runGuide();
				GuideUI12_Equip.mInstance.needRunStep = 0;
				UISceneDialogPanel.mInstance.showDialogID(27);
			}
		}break;
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
	
	/**点击区域**/
	public void onClickZone(string zoneNum)
	{

        TweenPosition tw = fogs.GetComponent<TweenPosition>();
        if (tw.enabled == true)
        {
            return;
        }
		if(!canClick(zoneNum))
		{
			return;
		}
		if(curStep==1)
		{
			return;
		}
		GameObject zoneGo=(GameObject)mission2.zoneGos[zoneNum];
		if(zoneGo==null)
		{
			return;
		}
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		showPop(zoneNum);
	}
	
	private void showPop(string zoneNum)
	{
		curStep=1;
		mission2.canMove=false;
		popwindow.SetActive(true);
		helpers.SetActive(false);
		missions.SetActive(true);
		showMissions(zoneNum);
	}
	
	/**显示小关**/
	private void showMissions(string zoneNum)
	{
		powerLabel.text=PlayerInfo.getInstance().player.power+"/"+PlayerInfo.getInstance().player.sPower;
		curZoneNum = zoneNum;
		enemyWindow.SetActive(false);
		missionDetail.SetActive(false);
		missionCells.transform.localPosition=Vector3.zero;
		missionCells.GetComponent<UIPanel>().clipRange=new Vector4(0,0,550,400);
		missionScrollBar.GetComponent<UIScrollBar>().value=0;
		missionCells.GetComponent<UIDraggablePanel>().ResetPosition();
		
		/**销毁子节点**/
		GameObjectUtil.destroyGameObjectAllChildrens(missionParent);
		/**设置区域名字**/
		string[] ss=zoneNum.Split('-');
		int map=StringUtil.getInt(ss[0]);
		int zone=StringUtil.getInt(ss[1]);
		int missionType=StringUtil.getInt(ss[2]);
		zoneName.text=MissionData.getZoneName(map,zone,missionType);
		/**添加小关卡**/
		List<MissionData> missionDatas=MissionData.getData(map,zone,missionType);
		int playerMissionId=mrj.m1;
		if(missionType==2)
		{
			playerMissionId=mrj.m2;
		}
		
		string defaultShowMissionParam=null;
		int cellCount = 0;
		int starNums=0;
		bool haveGotStar3Reward=false;
		List<Transform> tfs=new List<Transform>();
		for(int i=0;i<missionDatas.Count;i++)
		{
			MissionData md=missionDatas[i];
			if(loadMissionCell==null)
			{
				loadMissionCell=GameObjectUtil.LoadResourcesPrefabs("UI-mission/mission-cell",3);
			}
			GameObject cell=Instantiate(loadMissionCell) as GameObject;
            GameObjectUtil.gameObjectAttachToParent(cell, missionParent);
			int sequence=md.getSequence();
			int bonus=0;
			if(md.missiontype==1 && mrj.b1.Length>=sequence)
			{
				bonus=StringUtil.getInt(mrj.b1[sequence-1]+"");
			}
			if(md.missiontype==2 && mrj.b2.Length>=sequence)
			{
				bonus=StringUtil.getInt(mrj.b2[sequence-1]+"");
			}
			char star='0';
			if(md.missiontype==1 && mrj.star1.Length>=sequence)
			{
				star=mrj.star1[sequence-1];
			}
			if(md.missiontype==2 && mrj.star2.Length>=sequence)
			{
				star=mrj.star2[sequence-1];
			}
			int starNum=0;
			switch(star)
			{
			case '0':
				starNum=0;
				break;
			case '1':
				starNum=1;
				break;
			case '2':
				starNum=2;
				break;
			case '3':
				starNum=3;
				break;
			case '4':
				starNum=3;
				haveGotStar3Reward=true;
				break;
			}
			int times=0;
			if(md.missiontype==1)
			{
				if(mrj.t1.Length/2>=sequence)
				{
					times=StringUtil.getInt(mrj.t1.Substring((sequence-1)*2,2));
				}
			}
			else
			{
				if(mrj.t2.Length/2>=sequence)
				{
					times=StringUtil.getInt(mrj.t2.Substring((sequence-1)*2,2));
				}
			}
			int newMark=0;
			if(md.id>playerMissionId)
			{
				newMark=1;
				/**找到可以显示的md**/
				if(!md.canShowUp(playerMissionId))
				{
					newMark=2;
				}
			}
			//==设置小关数据==//
			cell.GetComponent<MissionCellInfo>().showData(md,starNum,_myTransform.gameObject,"onClickSelectMission",newMark,bonus,times);
			starNums+=starNum;
			tfs.Add(cell.transform);
			if(sweepUiResult!=null)
			{
				if(sweepUiResult.md==md.id)
				{
					defaultShowMissionParam=md.id+"-"+starNum+"-"+bonus+"-"+times+"-"+(newMark==2?0:1);
				}
			}
			else
			{
				if(md.canShowUp(playerMissionId))
				{
					defaultShowMissionParam=md.id+"-"+starNum+"-"+bonus+"-"+times+"-"+(newMark==2?0:1);
				}
			}
			if(newMark!=2)
			{
				cellCount++;
			}
		}
		AllThreeStarData ad=AllThreeStarData.getData(map+"-"+zone+"-"+missionType);
		star3RewardNum.text=ad.reward;
		cur3StarNum = StringUtil.getInt(ad.reward);
		List<MissionData> mds=MissionData.getData(map,zone,missionType);
		star3Num.text=starNums+"/"+mds.Count*3;
		if(starNums==mds.Count*3 && !haveGotStar3Reward)
		{
			star3Btn.GetComponent<UISprite>().color=Color.white;
			//领取//
			star3Btn.transform.FindChild("label").GetComponent<UILabel>().text=TextsData.getData(304).chinese;
			UIButtonMessage3 msg=star3Btn.GetComponent<UIButtonMessage3>();
			msg.target=_myTransform.gameObject;
			msg.functionName="onClick3StarReward";
			msg.stringParam=zoneNum;
			//显示特效//
			if(star3Effect == null)
			{
				star3Effect = Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/lingqu_flyingspark",1)) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(star3Effect,star3Btn);
				star3Effect.transform.localPosition = new Vector3(0,2,0);
				star3Effect.transform.localScale=new Vector3(0.8f,1f,1f);
			}
			else
			{
				star3Effect.SetActive(true);
			}
		}
		else if(haveGotStar3Reward)
		{
			star3Btn.GetComponent<UISprite>().color=Color.gray;
			//已领取//
			star3Btn.transform.FindChild("label").GetComponent<UILabel>().text=TextsData.getData(397).chinese;
			UIButtonMessage3 msg=star3Btn.GetComponent<UIButtonMessage3>();
			msg.target=_myTransform.gameObject;
			msg.functionName="onClick3StarReward";
			msg.stringParam="-1";
			if(star3Effect != null)
			star3Effect.SetActive(false);
		}
		else
		{
			star3Btn.GetComponent<UISprite>().color=Color.gray;
			//领取//
			star3Btn.transform.FindChild("label").GetComponent<UILabel>().text=TextsData.getData(304).chinese;
			UIButtonMessage3 msg=star3Btn.GetComponent<UIButtonMessage3>();
			msg.target=_myTransform.gameObject;
			msg.functionName="onClick3StarReward";
			msg.stringParam="0";
			if(star3Effect != null)
			star3Effect.SetActive(false);
		}
		//==设置小关位置,显示列表最下边==//
		for(int k=0;k<tfs.Count;k++)
		{
			Transform tf=tfs[k];
			tf.localPosition=new Vector3(110f*k,0,0);
			tf.GetComponent<UIDragPanelContents>().enabled=true;
		}
		if(cellCount>5)
		{
			float xOffset=110f*(cellCount-5);
            missionCells.transform.localPosition=new Vector3(missionsInitPos.x-xOffset,missionsInitPos.y,missionsInitPos.z);
            missionCells.GetComponent<UIPanel>().clipRange = new Vector4(missionsInitClip.x + xOffset, missionsInitClip.y, missionsInitClip.z, missionsInitClip.w);
		}
		else
		{
			//==个数不足,不可滚动==//
			//foreach(Transform tf in tfs)
			//{
				//tf.GetComponent<UIDragPanelContents>().enabled=false;
			//}
		}
		//==默认显示关==//
		onClickSelectMission(defaultShowMissionParam);
		sweepUiResult=null;
		
		//==显示阵容提示==//
		showNewCombinationTip();
	}
	
	private void closeStar3Effect()
	{
		if(star3Effect!=null)
		{
			star3EffectStatus=star3Effect.activeSelf;
			star3Effect.SetActive(false);
		}
	}
	
	private void openStar3Effect()
	{
		if(star3Effect!=null)
		{
			star3Effect.SetActive(star3EffectStatus);
		}
	}
	
	//cxl---显示关卡掉落物品界面//
	private void showDropItemInfos(List<string> dropInfos)
	{
		DropInfoPanel.SetActive(true);
		float startY = 0;
		float offY = 124;
		CleanScrollData();
		//显示物品//
		for(int i=0;i<dropInfos.Count;i++)
		{
			
			string[] ss=dropInfos[i].Split('-');
			int droptype=StringUtil.getInt(ss[0]);
			string dropitem=StringUtil.getString(ss[1]);
			//int pro=StringUtil.getInt(ss[2]);
			
			string[] dropSs=dropitem.Split(',');
			int dropId = StringUtil.getInt(dropSs[0]);;
			GameHelper.E_CardType type = GameHelper.E_CardType.E_Equip;
			string name = "";
			string des = "";
			int sale = 0;
			switch(droptype)
			{
			case 1://==items==//
				type = GameHelper.E_CardType.E_Item;
				ItemsData itemD = ItemsData.getData(dropId);
				name = itemD.name;
				des = itemD.discription;
				sale = itemD.sell;
				break;
			case 2://==equip==//
				type = GameHelper.E_CardType.E_Equip;
				EquipData ed = EquipData.getData(dropId);
				name = ed.name;
				string equipAdd = Statics.getEquipValueForUIShow(dropId, 1);
				des = ed.description + equipAdd;
				sale = ed.sell;
				break;
			case 4: //--skill//
				type = GameHelper.E_CardType.E_Skill;
				SkillData sd = SkillData.getData(dropId);
				name = sd.name;
				des = sd.description;
				sale = sd.sell;
				break;
			case 5:	//passSkill//
				type = GameHelper.E_CardType.E_PassiveSkill;
				PassiveSkillData psd = PassiveSkillData.getData(dropId);
				name = psd.name;
				des = psd.describe;
				sale = psd.sell;
				break;
			case 6:		//hero//
				type = GameHelper.E_CardType.E_Hero;
				CardData cd = CardData.getData(dropId);
				name = cd.name;
				des = cd.description;
				sale = cd.sell;
				break;
			}

			
			GameObject item = Instantiate(MissionDropItem) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(item, DragParent);
			float y = startY - i*offY;
			item.transform.localPosition = new Vector3(0, y, 0);
			MissionDropItemInfo info = item.GetComponent<MissionDropItemInfo>();
			info.sci2.setSimpleCardInfo(dropId,type);
			info.nameLabel.text = name;
			info.sale.text = sale.ToString();
			info.des.text = des;
		}
	}
	
	//cxl---清除DropInfo的数据//
	public void CleanScrollData()
	{
		
		dropScrollBar.GetComponent<UIScrollBar>().value = 0;
		
		MissionDropDragPanel.transform.localPosition = Vector3.zero;
		MissionDropDragPanel.GetComponent<UIPanel>().clipRange = new Vector4(0,0,480,367);
		GameObjectUtil.destroyGameObjectAllChildrens(DragParent);
	}
	
	/**显示援护界面**/
	private void showHelpers()
	{
		int todayRe=(5-todayInviteTimes)>0?(5-todayInviteTimes):0;
		//今日剩余免费邀请次数：num//
		freeTimes.text=TextsData.getData(359).chinese.Replace("num",todayRe+"");
		//今日累计邀请次数：num//
		totalTimes.text=TextsData.getData(358).chinese.Replace("num",todayInviteTimes+"");
		helperCells.transform.localPosition=Vector3.zero;
		helperCells.GetComponent<UIPanel>().clipRange=new Vector4(0,0,600,330);
		helperScrollBar.GetComponent<UIScrollBar>().value=0;
		helperCells.GetComponent<UIDraggablePanel>().ResetPosition();
		
		/**销毁子节点**/
		GameObjectUtil.destroyGameObjectAllChildrens(helperParent);
		//绘制援护者//
		if(helperList==null)
		{
			return;
		}
		if(GuideManager.getInstance().getCurrentGuideID() == (int)GuideManager.GuideType.E_Battle3_Friend)
		{
			if(curMissionId == GuideUI_Friend.mInstance.missionID)
			{
				GuideManager.getInstance().runGuide();
				UISceneDialogPanel.mInstance.showDialogID(8);
			}
		}
		/**添加援护者**/
		for(int i=0;i<helperList.Count;i++)
		{
			FriendElement helper=helperList[i];
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle3_Friend))
			{
				if(i == 0)
				{
					GuideUI_Friend.mInstance.guideFriend = helper;
				}
			}
			if(loadHelpCell==null)
			{
				loadHelpCell=GameObjectUtil.LoadResourcesPrefabs("UI-mission/help-cell",3);
			}
			GameObject cell=Instantiate(loadHelpCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,helperParent);
			cell.transform.localPosition=new Vector3(0,-110f*i,0);
			cell.GetComponent<HelperCellInfo>().setData(helper,nextInviteCost,_myTransform.gameObject,"onClickSelectHelper");
			if(helperList.Count<=3)
			{
				cell.GetComponent<UIDragPanelContents>().enabled=false;
			}
			else
			{
				cell.GetComponent<UIDragPanelContents>().enabled=true;
			}
		}
	}
	
	public void lookEnemy(int param)
	{
		MissionData md=MissionData.getData(param);
		if(md==null)
		{
			return;
		}
		enemyWindow.SetActive(true);
		GameObjectUtil.destroyGameObjectAllChildrens(enemyWindow.transform.FindChild("enemys").gameObject);
		Vector3[] v3s=new Vector3[6]{new Vector3(-150,-100,0),new Vector3(0,-100,0),new Vector3(150,-100,0),new Vector3(-150,80,0),new Vector3(0,80,0),new Vector3(150,80,0)};
		for(int k=0;k<md.monsters.Length;k++)
		{
			string[] ss=md.monsters[k].Split('-');
			int monsterId=StringUtil.getInt(ss[0]);
			int skillId=StringUtil.getInt(ss[5]);
			int boss=StringUtil.getInt(ss[6]);
			CardData cd=CardData.getData(monsterId);
			if(cd!=null)
			{
				if(enemyCardSample==null)
				{
					enemyCardSample=GameObjectUtil.LoadResourcesPrefabs("UI-mission/Card-enemy",3);
				}
				GameObject enemyCard=Instantiate(enemyCardSample) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(enemyCard,enemyWindow.transform.FindChild("enemys").gameObject);
				enemyCard.GetComponent<SimpleCardInfo2>().clear();
				enemyCard.GetComponent<SimpleCardInfo2>().setSimpleCardInfo(monsterId,GameHelper.E_CardType.E_Hero);
				enemyCard.transform.FindChild("name").GetComponent<UILabel>().text=cd.name;
				if(boss==1)
				{
					enemyCard.transform.FindChild("boss").GetComponent<UILabel>().text="BOSS";
				}
				else
				{
					enemyCard.transform.FindChild("boss").GetComponent<UILabel>().text="";
				}
				SkillData sd=SkillData.getData(skillId);
				if(sd!=null)
				{
					switch(sd.type)
					{
					case 1:
						enemyCard.transform.FindChild("skill").GetComponent<UISprite>().spriteName="atk";
						break;
					case 2:
						enemyCard.transform.FindChild("skill").GetComponent<UISprite>().spriteName="def";
						break;
					case 3:
						enemyCard.transform.FindChild("skill").GetComponent<UISprite>().spriteName="hp";
						break;
					}
				}
				enemyCard.transform.localPosition=v3s[k];
			}
		}
	}
	
	public void closeEnemy()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(enemyWindow.transform.FindChild("enemys").gameObject);
		enemyWindow.SetActive(false);
	}
	
	/*显示ko兑换界面*/
	public void showKoExchange()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(koExchangeParent);
		List<KOAwardData> kdList = new List<KOAwardData>();
		kdList = KOAwardData.getList();
		int maxNumName = kdList.Count;
		for(int i = 0;i<maxNumName;i++)
		{
			if(loadKoCell==null)
			{
				loadKoCell=GameObjectUtil.LoadResourcesPrefabs("UI-mission/KOExchange-cell",3);
			}
			GameObject cell=Instantiate(loadKoCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,koExchangeParent);
			cell.GetComponent<KOExchangeInfo>().setData(i+1,koaward.gameObject,koerj.awards,koerj.point,i);
		}
		koExchangeParent.GetComponent<UIGrid2>().repositionNow=true;
	}
	
	public void friendMoveOver()
	{
		if(curStep==1)
		{
			popwindow.transform.FindChild("Panel-friend").gameObject.SetActive(false);
			popwindow.transform.FindChild("Panel-mission").gameObject.SetActive(true);
		}
		else if(curStep==2)
		{
			popwindow.transform.FindChild("Panel-friend/btn-group").gameObject.SetActive(true);
		}
	}
	
	public void onClick3StarReward(string param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(param.Equals("-1"))
		{
			ToastWindow.mInstance.showText(TextsData.getData(598).chinese);
			return;
		}
		if(param.Equals("0"))
		{
			ToastWindow.mInstance.showText(TextsData.getData(597).chinese);
			return;
		}
		cur3StarParam=param;
		requestType=4;
		PlayerInfo.getInstance().sendRequest(new Star3RewardJson(param),this);
	}
	
	public void showGuide6Task()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
	}
	
	public void onClickOtherBtn(int param)
	{
		//播放音效//

        TweenPosition tw = fogs.GetComponent<TweenPosition>();
        if (tw.enabled == true)
        {
            return;
        }
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		backToMainMenu();
	}
	
	public void backToCompose()
	{
		hide();
		if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE))
		{
			ComposePanel compose = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE, "ComposePanel" )as ComposePanel;
			compose.baseShow();
		}
	}
	
	public void backToCardInfo()
	{
		//GameObjectUtil.gameObjectAttachToParent(pointFinger,missions);
		hide();
		if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO))
		{
			CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" )as CardInfoPanelManager;
			cardInfo.gameObject.SetActive(true);
		}
	}
	
	public void backToMainMenu()
	{
		PlayerInfo.getInstance().lastMissionId=0;
		PlayerInfo.getInstance().BattleOverBackType = 0;
//		MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager")as MainMenuManager;
		if(main!= null)
		{
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		hide();
	}
	
	public void closePop()
	{
		curStep=0;
		curMissionId=0;
		curHelperId=0;
		GameObjectUtil.destroyGameObjectAllChildrens(missionParent);
		GameObjectUtil.destroyGameObjectAllChildrens(helperParent);
		popwindow.SetActive(false);
		mission2.canMove=true;
	}
	
	/**点击按钮:1后退,2确定**/
	public void onClickBtn(int param)
	{
		if(param==1)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			if(curStep==1)
			{
				if(backType==0 || backType==3 || backType == 4)
				{
					closePop();
				}
				else if(backType==1)
				{
					backType=0;
					backToCompose();
				}
				else if(backType==2)
				{
					backType=0;
					backToCardInfo();
				}
				
				if(star3Effect!=null)
				{
					star3Effect.SetActive(false);
				}
			}
			else if(curStep==2)
			{
				curStep=1;
				missions.SetActive(true);
				helpers.SetActive(false);
				curHelperId=0;
			}
		}
		else
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(curStep==1)
			{
				if(finishRequestHelp)
				{
					requestType=1;
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_REFRESH_HELPER),this);
					finishRequestHelp = false;
				}
			}
			else if(curStep==2)
			{
				requestType=2;
				PlayerInfo.getInstance().sendRequest(new BattleJson(curMissionId,curHelperId),this);
			}
		}
	}
	
	public bool canClick(string param)
	{
		string[] ss=param.Split('-');
		int map=StringUtil.getInt(ss[0]);
		int zone=StringUtil.getInt(ss[1]);
		int missionType=StringUtil.getInt(ss[2]);
		int missionId=0;
		if(missionType==1 && mrj != null)
		{
			missionId=mrj.m1;
		}
		else if(missionType==2 && mrj != null)
		{
			missionId=mrj.m2;
			if(missionId==0)
			{
				return false;
			}
		}
		MissionData md=MissionData.getUnlockData(missionId);
		if(md!=null)
		{
			if(map>md.map)
			{
				return false;
			}
			if(map==md.map && zone>md.zone)
			{
				return false;
			}
		}
		return true;
	}
	
	public bool isNewZone(string param)
	{
		string[] ss=param.Split('-');
		int map=StringUtil.getInt(ss[0]);
		int zone=StringUtil.getInt(ss[1]);
		int missionType=StringUtil.getInt(ss[2]);
		int missionId=0;
		if(missionType==1 && mrj!=null)
		{
			missionId=mrj.m1;
		}
		else if(missionType==2 && mrj!=null)
		{
			missionId=mrj.m2;
			if(missionId==0)
			{
				return false;
			}
		}
		MissionData md=MissionData.getUnlockData(missionId);
		if(md!=null && map==md.map && zone==md.zone && md.isFirstZoneMission())
		{
			return true;
		}
		return false;
	}
	
	public bool isZoneNotCompleted(string param)
	{
		string[] ss=param.Split('-');
		int map=StringUtil.getInt(ss[0]);
		int zone=StringUtil.getInt(ss[1]);
		int missionType=StringUtil.getInt(ss[2]);
		int missionId=0;
		if(missionType==1 && mrj!=null)
		{
			missionId=mrj.m1;
		}
		else if(missionType==2 && mrj!=null)
		{
			missionId=mrj.m2;
			if(missionId==0)
			{
				return false;
			}
		}
		MissionData md=MissionData.getUnlockData(missionId);
		if(md!=null && map==md.map && zone==md.zone)
		{
			return true;
		}
		return false;
	}
	
	//==点击关卡==//
	public void onClickSelectMission(string param)
	{
		string[] ss=param.Split('-');
		int mdId=StringUtil.getInt(ss[0]);
		int starNum=StringUtil.getInt(ss[1]);
		int bonus=StringUtil.getInt(ss[2]);
		int times=StringUtil.getInt(ss[3]);
		int canEnter=StringUtil.getInt(ss[4]);
		missionDetail.SetActive(true);
		missionDetail.GetComponent<MissionInfo>().setData(mdId,starNum,bonus,times,_myTransform.gameObject,"onClickMissionToHelper","OnClickSweepBtn","OnClickDropBtn",canEnter);
		UIButtonMessage msg=enemyBtn.GetComponent<UIButtonMessage>();
		msg.target=_myTransform.gameObject;
		msg.functionName="lookEnemy";
		msg.param=mdId;

        for (int k = 0; k < missionParent.transform.childCount; k++)
        {
            Transform tf = missionParent.transform.GetChild(k);
			//Vector3 v3=tf.transform.localPosition;
            if (tf.GetComponent<UIButtonMessage3>().stringParam == param)
            {
                tf.GetComponent<MissionCellInfo>().effect.SetActive(true);
                //tf.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
				//tf.transform.localPosition=new Vector3(v3.x,15,v3.z);
            }
            else
            {
                tf.GetComponent<MissionCellInfo>().effect.SetActive(false);
                //tf.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
				//tf.transform.localPosition=new Vector3(v3.x,0,v3.z);
            }
        }
	}
	
	public void onClickMissionToHelper(int missionId)
	{
		curMissionId = missionId;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		MissionData md=MissionData.getData(missionId);
		//==挑战次数已满==//
		int times=0;
		int sequence=md.getSequence();
		if(md.missiontype==1)
		{
			times=StringUtil.getInt(mrj.t1.Substring((sequence-1)*2,2));
		}
		else
		{
			times=StringUtil.getInt(mrj.t2.Substring((sequence-1)*2,2));
		}
		if(times>=md.times)
		{
			CheckToBuyBattleNum();
			return;
		}
		
		if(PlayerInfo.getInstance().player.level<md.unlocklevel)
		{
			//等级不足，无法进入！//
			ToastWindow.mInstance.showText(TextsData.getData(148).chinese);
			return;
		}
		
		if(missionId < STATE.BATTEL_WITH_FRIEDN_ID &&GuideManager.getInstance().getCurrentGuideID() <=(int)GuideManager.GuideType.E_Battle3_Friend )
		{
			requestBattleByMissionID(curMissionId);
			return;
		}
		onClickBtn(2);
	}
	
	public void onClickSelectHelper(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(curHelperId==param)
		{
			curHelperId=0;
		}
		else
		{
			curHelperId=param;
		}
		int childCount=helperParent.transform.childCount;
		for(int k=0;k<childCount;k++)
		{
			UIButtonMessage msg=helperParent.transform.GetChild(k).FindChild("btn-select").GetComponent<UIButtonMessage>();
			if(msg.param==curHelperId)
			{
				msg.GetComponent<UISprite>().spriteName="ture_circle";
			}
			else
			{
				msg.GetComponent<UISprite>().spriteName="false_circle";
			}
		}
	}
	
	public void onClickEnterMission()
	{
		onClickBtn(2);
	}
	
	//cxl---点击合体技查看详细信息//
	public void OnClickUniteDataBtn(int uniteId)
	{
		
	}
	
	
	//点击扫荡//
	public void OnClickSweepBtn(int missionId)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(missionId==0)
		{
			ToastWindow.mInstance.showText(TextsData.getData(596).chinese);
			return;
		}
		//向服务器发送扫荡请求//
		requestType = 5;
		PlayerInfo.getInstance().sendRequest(new SweepUiJson(missionId),this);
		
	}
	
	//点击掉落物品按钮//
	public void OnClickDropBtn(int missionId)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		MissionData md = MissionData.getData(missionId);
		
		//显示掉落物品详细信息界面//
		List<string> dropInfos=new List<string>();
		//==只显示equip、item==//
		for(int i=0;i<md.drops.Count;i++)
		{
			string[] ss=md.drops[i].Split('-');
			int droptype=StringUtil.getInt(ss[0]);
			//掉落类型，1 item, 2 equip, 3, card序列（不显示）, 4 skill, 5 passiveSkill , 6 hero//
			if(droptype!=3)
			{
				dropInfos.Add(md.drops[i]);
			}
		}
		if(dropInfos.Count > 0)
		{
			//打开掉落物品详细信息界面//
			showDropItemInfos(dropInfos);
		}
	}
	
	//掉落物品的关闭按钮//
	public void OnClickDropCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		DropInfoPanel.SetActive(false);
		
//		for(int i = 0;i < dropsItemInfoList.Length;i++)
//		{
//			dropsItemInfoList[i].gameObject.SetActive(false);
//		}
		
		CleanScrollData();
	}
	
	public void requestBattleByMissionID(int missionID)
	{
		curMissionId = missionID;
		requestType=2;
		PlayerInfo.getInstance().sendRequest(new BattleJson(missionID,0),this);
	}
	
	public void requestBattle(int missionID,int friendID)
	{
		requestType=2;
		PlayerInfo.getInstance().sendRequest(new BattleJson(missionID,friendID),this);
	}
	
	public void changeGroup(int param)
	{
		requestType = 3;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP,0),this);
	}
	
	public void receiveResponse(string json)
	{
		Debug.Log("mission ui : json ======== " + json);
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				FriendResultJson frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				helperList=frj.list;
				todayInviteTimes=frj.times;
				nextInviteCost=frj.cost;
				receiveData=true;
				break;
			case 2:
				BattleResultJson brj=JsonMapper.ToObject<BattleResultJson>(json);
				errorCode=brj.errorCode;
				if(brj.errorCode==0)
				{
					//设置战斗数据//
					PlayerInfo.getInstance().brj=brj;
					PlayerInfo.getInstance().lastMissionId=brj.md;
					PlayerInfo.getInstance().BattleOverBackType=STATE.BATTLE_BACK_MAP;
					PlayerInfo.getInstance().battleType = STATE.BATTLE_TYPE_NORMAL;
				}
				receiveData=true;
				break;
			case 3:
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
					
//					CombinationInterManager.mInstance.curCardGroup=cgrj.transformCardGroup();
//					CombinationInterManager.mInstance.curPage=0;
//					CombinationInterManager.mInstance.isUsed=true;
					
					cardGroupRJ = null;
					cardGroupRJ = cgrj;
				}
				receiveData=true;
				break;
			case 4:
				MapResultJson ej=JsonMapper.ToObject<MapResultJson>(json);
				errorCode=ej.errorCode;
				if(errorCode==0)
				{
					mrj=ej;
					PlayerInfo.getInstance().player.crystal=ej.c;
					PlayerInfo.getInstance().player.gold=ej.d;
				}
				receiveData=true;
				break;
			case 5:		//扫荡界面请求//
				SweepUiResultJson surj = JsonMapper.ToObject<SweepUiResultJson>(json);
				errorCode = surj.errorCode;
				sweepUiResult = surj;
				receiveData=true;
				break;
			case 6:   	//ko兑换奖励请求//
				KOExchangeResultJson kerj = JsonMapper.ToObject<KOExchangeResultJson>(json);
				errorCode = kerj.errorCode;
				koerj = kerj;
				//List<KOawardElement> k = koerj.awards;
				receiveData = true;
				break;
			case 7:		//获取界面信息//
				MapResultJson mj=JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mj.errorCode;
				if(errorCode == 0)
				{
					mrj=mj;
				}
				receiveData=true;
				break;
			case 8:		//查看是否解锁合体技//	
				nusrj = JsonMapper.ToObject<NewUnitSkillResultJson>(json);
				errorCode = nusrj.errorCode;
				receiveData = true;
				break;
			}
		}
	}
	//cuixl 获取当前小关卡数据//
	public void SendToGetData()
	{
		requestType = 7;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
	}
	
	//点击奖励兑换显示兑换列表//
	public void onClickKOAwardBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		requestType = 6;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_KO_EXCHANGE1),this);
	}
	
	//购买体力//
	public void onClickBuyPowerBtn()
	{
		ShowBuyTipControl.mInstance.SendToGetUIData(1, 2, 1, 0, 0, BuyTipManager.UI_TYPE.UI_HEAD);
		BuyTipManager.mInstance.setisMissionBuy();
	}
	
	public void setPowerText()
	{
		powerLabel.text=PlayerInfo.getInstance().player.power+"/"+PlayerInfo.getInstance().player.sPower;
	}
	
	public string GetCurZoneNum()
	{
		return curZoneNum;
	}
	
	public void showNewCombinationTip()
	{
		if(mrj.fType == 1)
		{
			newCombinationTip.spriteName = "tip_mark_1";
		}
		else if(mrj.fType == 2)
		{
			newCombinationTip.spriteName = "tip_mark_2";
		}
		else if(mrj.fType == 0)
		{
			newCombinationTip.spriteName = "";
		}
	}
}
