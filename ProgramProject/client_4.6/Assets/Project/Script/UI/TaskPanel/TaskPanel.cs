using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskPanel : MonoBehaviour,ProcessResponse {
	
	public GameObject tipCtrl;
	public GameObject helpUint;
	
	public TaskResultJson taskRJ;
	public UIScrollBar sb;
	
	public GameObject ClipPanel;
	public GameObject uiGrid;
	GameObject taskItemPrefab = null;
	public List<TaskElement> tes;
	public int activeValue;
	public string activeState;
	
	//是否是第一次充值//
	public int firstCharge;
	
	public UILabel hydNum;
	public UILabel hydDesc;
	
	public GameObject hydBoxUIGrid;
	
	ActiveResultJson arJson;
	
	List<VitalityData> vDataList;
	int maxHYDNum;
	Dictionary<int,int> hydBoxDict;
	int curVitality;
	
	bool needRefreshPlayer;
	//GiftCodeResultJson gcrj;
	RechargeUiResultJson rechargeRJ;
	
	ActivityInfoResultJson gmaij;
	ExchangeResultJson exrj;
    private RankResultJson rankJson;
	//抽卡界面的json//
	LotResultJson lotRJ;
	
	/*1获取右侧界面列表，2获取领奖励之后的列表，3获取推图普通难度界面，4获取推图精英难度界面，
	 * 5获取竞技场界面，6获取迷宫界面，7获取强化列表，8获取金币购买界面，9获取异世界界面，
	 * 10获取冥想界面，11获取抽卡界面,	12获取激活码信息,	13获取充值界面, 14请求宝箱奖励
	 * */
	int requestType = 0;
	bool receiveData = false;
	int errorCode = -1;
	
	//推图界面的json//
	MapResultJson mapRJ;
	
	//背包界面的json//
	PackResultJson packRJ;
	public enum ActRewardType
	{
		E_Null = 0,
		E_Item = 1,
		E_Equip = 2,
		E_Card = 3,
		E_Skill = 4,
		E_PassiveSkill = 5,
		E_Gold = 6,
		E_Exp = 7,
		E_Crystal = 8,
		E_Rune = 9,
		E_Power = 10,
		E_Friend = 11,
	}
	
	public UIAtlas otherAtlas;
	//string iconFrameName = "head_star_";
	string expSpriteName = "reward_exp";
	string crystalSpriteName = "reward_crystal";
	string goldSpriteName = "reward_gold";
	//string runeSpriteName = "rune";
	//string powerSpriteName = "power";
	
	int curTaskId = 0;
	TaskElement curGetTaskElement;
	
	//进入竞技场界面信息--玩家信息//
	string playerArenaData;
	
	//requestType 3 排位赛界面pk对手信息, 2 异世界中副本的信息（id-mark-time）//
	List<string> strData;
	
	//总的挑战次数//
	int totalDekaronNum;
	
	//总的符文值//
	int totalPVPReward;
	
	//request == 3 竞技场的冷却时间 ， 2 活动副本， 5 扭曲空间 pk剩余时间//
	int pkTime;
	
	//进入灵界界面获得的数据//
	int nullPackNum;					//背包空余格子数//
	int curNpcId;						//当前激活的npc的id//






    int mid; //当前玩家激活的领奖id编号//

    int mnum; //当前玩家的冥想次数//
	
	int selMazeId;
	
	ActivityInfoExchangeElement tempActInfoEElement;
	int curRechargeId;

    int num;
	void Awake()
	{
		vDataList = VitalityData.vitalityDataList;
		int vDataListCount = vDataList.Count;
		if(vDataListCount>0)
		{
			maxHYDNum = vDataList[vDataListCount-1].vitality;
		}
		
		hydBoxDict = new Dictionary<int, int>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			if(errorCode!=0)
			{
				//尚未解锁，请提升军团等级//
				if(errorCode == 56)
				{
					string errorMsg = TextsData.getData(384).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//任务尚未完成，无法领取奖励！//
				else if(errorCode == 90)
				{
					string errorMsg = TextsData.getData(376).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//您已领取该奖励//
				else if(errorCode == 91)
				{
					string errorMsg = TextsData.getData(217).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//服务器出错//
				else if(errorCode == 96)
				{
					string errorMsg = TextsData.getData(385).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//不存在该活跃度的礼包//
				else if(errorCode == 116)
				{
					string errorMsg = TextsData.getData(666).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//活跃度不够，不能领取礼包//
				else if(errorCode == 117)
				{
					string errorMsg = TextsData.getData(667).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				//已经领取过该礼包//
				else if(errorCode == 118)
				{
					string errorMsg = TextsData.getData(668).chinese;
					ToastWindow.mInstance.showText(errorMsg);
					return;
				}
				return;
			}
			switch(requestType)
			{
			case 1:
				InitClipPanel();
				break;
			case 2:
			{
				if(curGetTaskElement != null)
				{
					tipCtrl.SetActive(true);
					tipCtrl.transform.FindChild("OkBtn").GetComponent<UIButtonMessage>().param = 0;
					tipCtrl.transform.FindChild("ActName").GetComponent<UILabel>().text = curGetTaskElement.name;
					ShowReward(curGetTaskElement.reward,tipCtrl);
					if(!TalkingDataManager.isTDPC)
					{
						for(int i =0;i<curGetTaskElement.reward.Count;i++)
						{
							string[] ss = curGetTaskElement.reward[i].Split('-');
							switch(StringUtil.getInt(ss[0]))
							{
//							case (int)ActRewardType.E_Gold://金币//
//								TDGAVirtualCurrency.OnReward(StringUtil.getInt(ss[1]),"EveryDayTask-"+TextsData.getData(58).chinese);
//								break;
							case (int)ActRewardType.E_Crystal://钻石//
								TDGAVirtualCurrency.OnReward(StringUtil.getInt(ss[1]),"EveryDayTask-"+TextsData.getData(48).chinese);
								break;
//							case (int)ActRewardType.E_Rune://符文//
//								TDGAVirtualCurrency.OnReward(StringUtil.getInt(ss[1]),"EveryDayTask-"+TextsData.getData(221).chinese);
//								break;
							}
						}
					}
					HeadUI.mInstance.requestPlayerInfo();
				}
				checkIsCanRunGuide();
			}break;
			case 3:
			{
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
				
				MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
					"MissionUI2")as MissionUI2;
				
				MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
					"MissionUI")as MissionUI;
				mission.mrj = mapRJ;
				mission2.show();
				mission.show();
				
				//删除主场景界面//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				

				CloseTaskPanelOnly();
				mapRJ = null;
			}
			break;
			case 4:
			{
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
				
				MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
					"MissionUI2")as MissionUI2;
				MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
					"MissionUI")as MissionUI;
				mission.mrj = mapRJ;
				mission2.show();
				mission2.onClickModelBtn(2);
				
				mission.show();
				
				//删除主场景界面//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				
				CloseTaskPanelOnly();
				mapRJ = null;
			}
			break;
			case 5:
			{
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				//加载竞技场界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA);
				ArenaUIManager arena = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA, 
					"ArenaUIManager") as ArenaUIManager;
				arena.SetData(0, playerArenaData, strData, totalPVPReward, totalDekaronNum, pkTime,rankJson.cardIds,1);
				
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_PVP))
				{
					UISceneDialogPanel.mInstance.showDialogID(16);
				}
				CloseTaskPanelOnly();
			}
			break;
			case 6:
			{
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				
				//打开扭曲空间界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE);
				WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager")as WarpSpaceUIManager;
				warpSpace.SetData(PlayerInfo.getInstance().curOpenMazeId, PlayerInfo.getInstance().curIntoMaze, selMazeId, pkTime);
				HeadUI.mInstance.hide();
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
				{
					UISceneDialogPanel.mInstance.showDialogID(20);
				}
				CloseTaskPanelOnly();
			}
			break;
			case 7:
			{
				
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY);
				IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
					"IntensifyPanel")as IntensifyPanel;
				
				intensify.allCells.Clear();
				for(int i=0;i<packRJ.pejs.Count;i++)
				{
					intensify.allCells.Add(packRJ.pejs[i].pe);
				}
				intensify.allFromIdList = packRJ.pejs;
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
				{
					intensify.guideTargetCard = packRJ.list[1];
				}
				
				intensify.show();
				
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyCard))
				{
					GuideUI_Intesnify.mInstance.showStep(2);
				}
				CloseTaskPanelOnly();
				packRJ = null;
			}
			break;
			case 9:
			{
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				
				//打开活动副本界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
				ActiveWroldUIManager activeCopy = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY, 
					"ActiveWroldUIManager")as ActiveWroldUIManager;
				activeCopy.setData(strData,num);
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ActiveCopy))
				{
					UISceneDialogPanel.mInstance.showDialogID(22);
				}
				CloseTaskPanelOnly();
			}
			break;
			case 10:
			{
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				//进入冥想界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
				SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, "SpriteWroldUIManager")as SpriteWroldUIManager;
                spriteWorld.SetData(nullPackNum, curNpcId, mid, mnum);
				
				
				HeadUI.mInstance.hide();
				CloseTaskPanelOnly();
			}
			break;
			case 11:
			{
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_GetCard))
				{
					GuideUI_GetCard.mInstance.showStep(1);
				}
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT);
				LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
				lotCard.lrj=lotRJ;
                lotCard.freeTimes = lotRJ.t;
				lotCard.show();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				CloseTaskPanelOnly();
				lotRJ = null;
			}
			break;
			case 13:
			{
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
				CloseTaskPanelOnly();
				ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ, 
						"ChargePanel") as ChargePanel;
				charge.curRechargeResult = rechargeRJ;
				charge.firstCharge = firstCharge;
                charge.isShowType = 0;
				charge.show();
				
			}
			break;
			case 14:
			{
				UpdateHYDBoxState();
				HeadUI.mInstance.requestPlayerInfo();
				string str = TextsData.getData(669).chinese;
				ToastWindow.mInstance.showText(str+"\n"+GetHYDTipText(curVitality));
			}
			break;
			}
		}
	}
	
	public void checkIsCanRunGuide()
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
	
	void InitClipPanel()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(uiGrid);
		hydNum.text = activeValue.ToString();
		//初始化活跃度描述//
		if(activeValue<maxHYDNum)
		{
			int nextHYDNum = GetNextHYDNum(activeValue);
			hydDesc.text = TextsData.getData(664).chinese.Replace("num",(nextHYDNum-activeValue).ToString());
		}
		else
		{
			hydDesc.text = TextsData.getData(663).chinese;
		}
		//初始化宝箱数据//
		InitCurHYDBoxData();
		//设置宝箱状态//
		SetHYDBoxState();
		int index = -1;
		foreach(Transform trans in hydBoxUIGrid.transform)
		{
			index++;
			UIButtonMessage[] btnMsgs = trans.GetComponents<UIButtonMessage>();
			btnMsgs[0].target = gameObject;
			btnMsgs[0].functionName = "OnClickBox";
			btnMsgs[0].param = vDataList[index].vitality;
			
			btnMsgs[1].target = gameObject;
			btnMsgs[1].functionName = "OnPressBox";
			btnMsgs[1].param = vDataList[index].vitality;
			
			btnMsgs[2].target = gameObject;
			btnMsgs[2].functionName = "OnReleaseBox";
			btnMsgs[2].param = vDataList[index].vitality;
		}
		
		for(int i=0;i<tes.Count;i++)
		{
			TaskElement te = tes[i];
			if(taskItemPrefab == null)
			{
				taskItemPrefab = Resources.Load("Prefabs/UI/TaskPanel/ETaskItem") as GameObject;
			}
			GameObject pItem = Instantiate(taskItemPrefab) as GameObject;
			ShowReward(te.reward,pItem);
			if(te.reward.Count == 0)
			{
				pItem.transform.FindChild("RewardLabel").GetComponent<UILabel>().text = "";
			}
			pItem.transform.FindChild("eTaskDesc").GetComponent<UILabel>().text = te.description;
			pItem.transform.FindChild("eTaskName").GetComponent<UILabel>().text = te.name;
			pItem.transform.FindChild("Icon").GetComponent<UISprite>().spriteName = te.icon;
			
			GameObject finishedBG,unfinishedBG;
			GameObject finishedGroup,unFinishedGroup;
			GameObject finishedLabel,unFinishedLabel;
			finishedGroup = pItem.transform.FindChild("FinishedGroup").gameObject;
			unFinishedGroup = pItem.transform.FindChild("UnFinishedGroup").gameObject;
			finishedBG = pItem.transform.FindChild("FinishBG").gameObject;
			unfinishedBG = pItem.transform.FindChild("UnFinishBG").gameObject;
			finishedLabel = pItem.transform.FindChild("FinishLabel").gameObject;
			unFinishedLabel = pItem.transform.FindChild("UnFinishLabel").gameObject;
			
			finishedLabel.SetActive(false);
			unFinishedLabel.SetActive(false);
			
			UILabel hydLabel = pItem.transform.FindChild("HYDDesc").GetComponent<UILabel>();
			UILabel unlockLabel = pItem.transform.FindChild("UnlockDesc").GetComponent<UILabel>();
			if(te.activeNum == 0)
			{
				hydLabel.gameObject.SetActive(false);
			}
			else
			{
				string hydText = TextsData.getData(665).chinese;
				hydLabel.text = hydText.Replace("num",te.activeNum.ToString());
			}
			if(string.IsNullOrEmpty(te.ulDesc))
			{
				unlockLabel.text = "";
			}
			else
			{
				unlockLabel.text = te.ulDesc;
			}
			
			GameObject progressObj = pItem.transform.FindChild("PBSpriteBG").gameObject;
			
			//判断特殊id的条目---补充体力活动条目//
			if(te.id == 12 || te.id == 13)
			{
				finishedGroup.SetActive(false);
				unFinishedGroup.SetActive(false);
				//补充体力活动不显示进度条//
				progressObj.SetActive(false);
				if(te.type == 0)
				{
					finishedLabel.SetActive(false);
					unFinishedLabel.SetActive(true);
					finishedBG.SetActive(false);
					unfinishedBG.SetActive(true);
					//补充体力活动尚未开始//
					unFinishedLabel.GetComponent<UILabel>().text = TextsData.getData(396).chinese;
				}
				else if(te.type == 2)
				{
					finishedLabel.SetActive(true);
					unFinishedLabel.SetActive(false);
					finishedBG.SetActive(true);
					unfinishedBG.SetActive(false);
					//已领取体力//
					finishedLabel.GetComponent<UILabel>().text = TextsData.getData(397).chinese;
				}
				else
				{
					//到领取活动体力时间了但是没有领取的情况//
					finishedBG.SetActive(true);
					unfinishedBG.SetActive(false);
					finishedGroup.SetActive(true);
					unFinishedGroup.SetActive(false);
					finishedGroup.transform.FindChild("Btn").GetComponent<UIButtonMessage>().target = gameObject;
					finishedGroup.transform.FindChild("Btn").GetComponent<UIButtonMessage>().param = te.id;
				}
			}
			//补充体力条目设置，注：补充体力的type值无实际意义,故不分情况设置//
			else if(te.id == 14)
			{
				finishedBG.SetActive(false);
				unfinishedBG.SetActive(true);
				finishedGroup.SetActive(false);
				unFinishedGroup.SetActive(true);
				//补充体力不显示进度条//
				progressObj.SetActive(false);
				unFinishedGroup.transform.FindChild("Button").GetComponent<UIButtonMessage>().target = gameObject;
				unFinishedGroup.transform.FindChild("Button").GetComponent<UIButtonMessage>().param = te.id;
			}
			else
			{
				if(te.type == 0)
				{
					
					finishedBG.SetActive(false);
					unfinishedBG.SetActive(true);
					finishedGroup.SetActive(false);
					unFinishedGroup.SetActive(true);
					
					unFinishedGroup.transform.FindChild("Button").GetComponent<UIButtonMessage>().target = gameObject;
					unFinishedGroup.transform.FindChild("Button").GetComponent<UIButtonMessage>().param = te.id;
					//判断是否是月卡//
					if(te.id == 11)
					{
						//不显示进度条//
						progressObj.SetActive(false);
//						unFinishedGroup.transform.FindChild("UnFinishText").gameObject.SetActive(false);
					}
					else
					{
						//显示进度条//
						progressObj.SetActive(true);
						progressObj.transform.FindChild("PBLabel").GetComponent<UILabel>().text = te.num.ToString()+"/"+te.sNum.ToString();
						UISprite pbValue = progressObj.transform.FindChild("PBValue").GetComponent<UISprite>();
						//如果当前次数是0，则隐藏进度条//
						if(te.num == 0)
						{
							pbValue.alpha = 0;
						}
						else
						{
							pbValue.alpha = 1;
							pbValue.width = (int)((float)te.num/te.sNum*70);
						}
//						unFinishedGroup.transform.FindChild("UnFinishText").GetComponent<UILabel>().text = te.num.ToString()+"/"+te.sNum.ToString();
					}
				}
				else
				{
					finishedBG.SetActive(true);
					unfinishedBG.SetActive(false);
					finishedGroup.SetActive(true);
					unFinishedGroup.SetActive(false);
					//可以领奖励不显示进度条了//
					progressObj.SetActive(false);
					finishedGroup.transform.FindChild("Btn").GetComponent<UIButtonMessage>().target = gameObject;
					finishedGroup.transform.FindChild("Btn").GetComponent<UIButtonMessage>().param = te.id;
				}
			}
			
			pItem.GetComponent<UIDragPanelContents>().draggablePanel = ClipPanel.GetComponent<UIDraggablePanel>();
			GameObjectUtil.gameObjectAttachToParent(pItem,uiGrid);
		}
		uiGrid.GetComponent<UIGrid>().repositionNow = true;
		//重置滚动位置//
		sb.value = 0;
	}
	
	//得到下次活跃度的值//
	int GetNextHYDNum(int curHYD)
	{
		int nextHYDNum=0;
		//如果活跃度达到定义的最大活跃度值//
		if(curHYD>=maxHYDNum)
		{
			nextHYDNum = maxHYDNum;
		}
		else
		{
			for(int i=0;i<vDataList.Count;i++)
			{
				VitalityData tvData = vDataList[i];
				if(curHYD<tvData.vitality)
				{
					nextHYDNum = tvData.vitality;
					break;
				}
			}
		}
		return nextHYDNum;
	}
	
	//把各个盒子的活跃度状态装入字典里面//
	void InitCurHYDBoxData()
	{
		if(hydBoxDict.Count>0)
		{
			hydBoxDict.Clear();
		}
		string[] sArray = activeState.Split(',');
		for(int i=0;i<sArray.Length;i++)
		{
			string[] aStr = sArray[i].Split('-');
			hydBoxDict.Add(StringUtil.getInt(aStr[0]),StringUtil.getInt(aStr[1]));
		}
	}
	
	//更新宝箱状态//
	void UpdateHYDBoxState()
	{
		activeState = arJson.activeState;
		InitCurHYDBoxData();
		
		SetHYDBoxState();
	}
	
	//设置宝箱状态//
	void SetHYDBoxState()
	{
		int index = -1;
		foreach(Transform trans in hydBoxUIGrid.transform)
		{
			index++;
			int vitality = vDataList[index].vitality;
			int boxState = hydBoxDict[vitality];
			switch(trans.name)
			{
			case "Box01":
				UseStateSetBox(trans.GetComponent<UISprite>(),boxState,"close-tie","open-tie");
				break;
			case "Box02":
				UseStateSetBox(trans.GetComponent<UISprite>(),boxState,"close-tong","open-tong");
				break;
			case "Box03":
				UseStateSetBox(trans.GetComponent<UISprite>(),boxState,"close-yin","open-yin");
				break;
			case "Box04":
				UseStateSetBox(trans.GetComponent<UISprite>(),boxState,"close-jin","open-jin");
				break;
			}
		}
	}
	
	void UseStateSetBox(UISprite boxSprite,int state,string closeSpriteName,string openSpriteName)
	{
		switch(state)
		{
		case 0:
			boxSprite.spriteName = closeSpriteName;
			boxSprite.transform.FindChild("Mark").gameObject.SetActive(false);
			break;
		case 1:
			boxSprite.spriteName = closeSpriteName;
			boxSprite.transform.FindChild("Mark").gameObject.SetActive(true);
			break;
		case 2:
			boxSprite.spriteName = openSpriteName;
			boxSprite.transform.FindChild("Mark").gameObject.SetActive(false);
			break;
		}
	}
	
	void OnClickBox(int param)
	{
		int boxState = hydBoxDict[param];
		curVitality = param;
		switch(boxState)
		{
		case 0:
			break;
		case 1:
			requestType = 14;
			PlayerInfo.getInstance().sendRequest(new ActiveJson(param),this);
			break;
		case 2:
			break;
		}
	}
	
	void OnPressBox(int param)
	{
		int boxState = hydBoxDict[param];
		
		//可以领取状态和已经领取状态不显示tip//
		if(boxState == 1 || boxState == 2)
		{
			return;
		}
		helpUint.SetActive(true);
		helpUint.transform.FindChild("unit-des").GetComponent<UILabel>().text = GetHYDTipText(param);
	}
	
	void OnReleaseBox(int param)
	{
		helpUint.SetActive(false);
	}
	
	string GetHYDTipText(int vitality)
	{
		string str = "";
		List<string> hydList = VitalityData.getHYDRewardList(vitality);
		for(int i=0;i<hydList.Count;i++)
		{
			int type = StringUtil.getInt(hydList[i].Split('-')[0]);
			string idStr = hydList[i].Split('-')[1];
			str += GetRewardString(type,idStr) +"\n";
		}
		return str;
	}
	
	void OnETaskFinishedBtn(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		curTaskId = param;
		curGetTaskElement = GetTaskInfo(curTaskId);
		requestType = 2;
		PlayerInfo.getInstance().sendRequest(new ActivityRewardJson(param),this);
	}
	
	TaskElement GetTaskInfo(int taskId)
	{
		TaskElement te = null;
		for(int i=0;i<tes.Count;i++)
		{
			TaskElement tte = tes[i];
			if(tte.id == taskId)
			{
				te = tte;
			}
		}
		return te;
	}
	
	/*param: 1前往推图普通难度界面，2前往推图精英难度界面，
	 * 3前往竞技场界面，4前往迷宫界面，5前往强化列表，6前往金币购买界面，7前往异世界界面，
	 * 10前往冥想界面，9前往抽卡界面, 11前往充值界面购买月卡
	 * */
	void OnETaskUnFinishedBtn(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		switch(param)
		{
		case 1:
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
			break;
		case 2:
			if(PlayerInfo.getInstance().player.missionId<110407)
			{
				string warningTxt = TextsData.getData(387).chinese;
				ToastWindow.mInstance.showText(warningTxt);
				return;
			}
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
			break;
		case 3:
			ChangeWorld(2);
			break;
		case 4:
			ChangeWorld(4);
			break;
		case 5:
			requestType = 7;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,1),this);
			break;
		case 6:
			requestType = 8;
			HeadUI.mInstance.OnClickBuyInfoBtn(0);
			CloseTaskPanelOnly();
			break;
		case 7:
			ChangeWorld(1);
			break;
		case 8:
			break;
		case 9:
			requestType = 11;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_LOT),this);
			break;
		case 10:
			ChangeWorld(3);
			break;
		case 11:
			openChargePanel();
			break;
		case 14:
			int buyType = 2;
			int jsonType = 1;
			int costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_HEAD);
			CloseTaskPanelOnly();
			break;
		}
	}
	
	public void ChangeWorld(int param)
	{
	switch(param)
		{
		case 1:/**异世界**/
			openAcitveCopy();
			return;
		case 2:/**竞技场**/
			openPVP();
			break;
		case 3:/**灵界**/
			openSpirit();
			break;
		case 4:/**扭曲空间**/
			openWarpSpace();
			break;
		}
	}
	
	public void openAcitveCopy()
	{
		requestType = 9;
		//发送进入异世界（活动副本）请求信息//
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_EVENT),this);
	}
	
	public void openPVP()
	{
		requestType = 5;
		//发送进入竞技场请求信息//
		PlayerInfo.getInstance().sendRequest(new RankJson(0),this);
	}
	
	public void openSpirit()
	{
		requestType = 10;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SPRITEWROLD_INTO),this);
	}
	
	public void openWarpSpace()
	{
		requestType = 6;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
	}
	
	public void openChargePanel()
	{
		requestType = 13;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:	//任务//
				TaskResultJson trj=JsonMapper.ToObject<TaskResultJson>(json);
				errorCode=trj.errorCode;
				if(errorCode==0)
				{
					tes = trj.tes;
					activeValue = trj.active;
					activeState = trj.activeState;
					InitCurHYDBoxData();
				}
				receiveData=true;
				break;
			case 2:
			{
				ActivityRewardResultJson arrj = JsonMapper.ToObject<ActivityRewardResultJson>(json);
				errorCode = arrj.errorCode;
				if(errorCode==0)
				{
					tes.Clear();
					tes = arrj.tes;
					activeValue = arrj.active;
					activeState = arrj.activeState;
				}
				receiveData=true;
			}break;
			case 3:
			{
				MapResultJson mj=JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mj.errorCode;
				if(errorCode == 0)
				{
//					MissionUI.mInstance.mrj=mj;
					
					mapRJ = mj;
				}
				receiveData=true;
			}
			break;
			case 4:
			{
				MapResultJson mj=JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mj.errorCode;
				if(errorCode == 0)
				{
//					MissionUI.mInstance.mrj=mj;
					
					mapRJ = mj;
				}
				receiveData=true;
			}
			break;
			case 5:
			{
				RankResultJson rrj = JsonMapper.ToObject<RankResultJson>(json);
				errorCode = rrj.errorCode;
				if(errorCode == 0)
				{
                    rankJson = rrj;
					playerArenaData = rrj.s;
					strData = rrj.ss;
					totalDekaronNum = rrj.sPknum;
					totalPVPReward = rrj.sAward;
					pkTime = (int)(rrj.cdtime);
				}
				receiveData = true;
			}
				
			break;
			case 6:
			{
				MazeResultJson mrj=JsonMapper.ToObject<MazeResultJson>(json);
				errorCode = mrj.errorCode;
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().curOpenMazeId.Clear();
					PlayerInfo.getInstance().curOpenMazeId = mrj.s;
					PlayerInfo.getInstance().curIntoMaze = mrj.maze;
					selMazeId = mrj.mId;
					pkTime = mrj.cdtime;
				}
				receiveData = true;
			}
				break;
			case 7:
			{
				PackResultJson prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode == 0)
				{
					
					packRJ = prj;
				}
				receiveData=true;
			}
			break;
			case 9:
			{
				EventResultJson erj = JsonMapper.ToObject<EventResultJson>(json);
				errorCode = erj.errorCode;
				if(errorCode == 0)
				{
					strData = erj.s;
                    num = erj.num;
				}
				receiveData = true;
			}
				break;
			case 10:
			{
				ImaginationResultJson irj = JsonMapper.ToObject<ImaginationResultJson>(json);
				errorCode = irj.errorCode;
				if(errorCode == 0)
				{
					curNpcId = irj.id;
					nullPackNum = irj.i;
                    mid = irj.mid;
                    mnum = irj.mnum;
					PlayerInfo.getInstance().player.gold = irj.g;
				}
				receiveData = true;
			}
				break;
			case 11:
			{
				LotResultJson lrj=JsonMapper.ToObject<LotResultJson>(json);
				errorCode = lrj.errorCode;
				if(errorCode == 0)
				{
					lotRJ = lrj;
				}
				receiveData=true;
			}
			break;
			case 12:
			{
				/*GiftCodeResultJson gcrj = JsonMapper.ToObject<GiftCodeResultJson>(json);
				errorCode = gcrj.errorCode;
				if(errorCode == 0)
				{
					this.gcrj = gcrj;
				}
				receiveData = true;
				*/
			}
			break;
			case 13:
			{
				RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
				errorCode = rechargej.errorCode;
				if(errorCode==0)
				{
					rechargeRJ = rechargej;
				}
				receiveData = true;
			}
			break;
			case 14:
			{
				ActiveResultJson arj = JsonMapper.ToObject<ActiveResultJson>(json);
				errorCode = arj.errorCode;
				if(errorCode==0)
				{Debug.Log("ActiveResultJson"+json);
					arJson = arj;
				}
				receiveData = true;
			}
			break;
			}
		}
	}
	
	//param:0每日任务的领奖励//
	void CloseTipWnd(int param)
	{
		tipCtrl.SetActive(false);
		switch(param)
		{
		case 0:
			InitClipPanel();
			break;
		}
	}
	
	public void show()
	{
        Main3dCameraControl.mInstance.SetBool(true);
		tipCtrl.SetActive(false);
		helpUint.SetActive(false);
		InitClipPanel();
	}
	
	public void hide()
	{
        Main3dCameraControl.mInstance.SetBool(false);
		TalkMainToGetData();
//		base.hide();
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_TASK);
	}
	
	public void TalkMainToGetData()
	{
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			if(main!= null && obj.activeSelf)
			{
				main.SendToGetData();
				
				
			}
		}
	}
	
	public void gc()
	{
		tipCtrl = null;
		helpUint = null;
		sb = null;
		ClipPanel = null;
		uiGrid = null;
		hydDesc = null;
		hydNum = null;
		hydBoxUIGrid = null;
		
		taskItemPrefab=null;
		tes = null;
		otherAtlas = null;
		
		//==释放资源==//
		Resources.UnloadUnusedAssets();
	}
	
	public void CloseTaskPanel()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		GameObjectUtil.destroyGameObjectAllChildrens(uiGrid);
		hide();
	}
	
	public void CloseTaskPanelOnly()
	{
        Main3dCameraControl.mInstance.SetBool(false);
		GameObjectUtil.destroyGameObjectAllChildrens(uiGrid);
//		base.hide();
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_TASK);
	}
	
	void ShowReward(List<string> rewardList,GameObject rItem)
	{
		List<GameObject> tipRewardList = new List<GameObject>();
		tipRewardList.Add(rItem.transform.FindChild("Rward0").gameObject);
//		tipRewardList.Add(rItem.transform.FindChild("Rward1").gameObject);
		for(int i = 0; i < tipRewardList.Count;++i)
		{
			tipRewardList[i].SetActive(false);
		}
		
		for(int j=0;j<rewardList.Count;j++)
		{
			if(j<2)
			{
				tipRewardList[j].SetActive(true);
				string rewardItem = rewardList[j];
				string[] ss = rewardItem.Split('-');
				int rewardType = StringUtil.getInt(ss[0]);
				string rewardText = StringUtil.getString(ss[1]);
				if(rItem.transform.FindChild("Rward"+j.ToString()))
				{
					GameObject rewardObj = rItem.transform.FindChild("Rward"+j.ToString()).gameObject;
					UISprite rewardIconBG = rewardObj.transform.FindChild("IconBG").GetComponent<UISprite>();
					rewardIconBG.gameObject.SetActive(false);
					UILabel rewardLabel = rewardObj.transform.FindChild("Text").GetComponent<UILabel>();
					
					rewardLabel.text = string.Empty;
					SimpleCardInfo2 cardInfo = rewardObj.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
					cardInfo.clear();
					cardInfo.gameObject.SetActive(false);
					
					switch(rewardType)
					{
						case (int)ActRewardType.E_Item:
						{
							string[] tempS = rewardText.Split(',');
							int itemID = StringUtil.getInt(tempS[0]);
							int num = StringUtil.getInt(tempS[1]);
							ItemsData itemData = ItemsData.getData(itemID);
							if(itemData == null)
							{
								rewardObj.SetActive(false);
								continue;
							}
							cardInfo.gameObject.SetActive(true);
							cardInfo.setSimpleCardInfo(itemID,GameHelper.E_CardType.E_Item);

							rewardLabel.text = itemData.name + " x " + num.ToString();
							
						}break;
						case (int)ActRewardType.E_Equip:
						{
							string[] tempS = rewardText.Split(',');
							int equipID = StringUtil.getInt(tempS[0]);
							int num = StringUtil.getInt(tempS[1]);
							
							EquipData ed = EquipData.getData(equipID);
							if(ed == null)
							{
								rewardObj.SetActive(false);
								continue;
							}
							cardInfo.gameObject.SetActive(true);
							cardInfo.setSimpleCardInfo(equipID,GameHelper.E_CardType.E_Equip);	

							rewardLabel.text = ed.name + " x " + num.ToString();
							
						}break;
						case (int)ActRewardType.E_Card:
						{
							string[] tempS = rewardText.Split(',');
							int heroID = StringUtil.getInt(tempS[0]);
							int num = StringUtil.getInt(tempS[1]);
							
							CardData cd = CardData.getData(heroID);
							if(cd == null)
							{
								rewardObj.SetActive(false);
								continue;
							}
							cardInfo.gameObject.SetActive(true);
							cardInfo.setSimpleCardInfo(heroID,GameHelper.E_CardType.E_Hero);

							rewardLabel.text = cd.name + " x " + num.ToString();
							
						}break;
						case (int)ActRewardType.E_Skill:
						{
							string[] tempS = rewardText.Split(',');
							int skillID = StringUtil.getInt(tempS[0]);
							int num = StringUtil.getInt(tempS[1]);
							
							SkillData sd = SkillData.getData(skillID);
							if(sd == null)
							{
								rewardObj.SetActive(false);
								continue;
							}
							cardInfo.gameObject.SetActive(true);
							cardInfo.setSimpleCardInfo(skillID,GameHelper.E_CardType.E_Skill);

							rewardLabel.text = sd.name + " x " + num.ToString();
							
						}break;
						case (int)ActRewardType.E_PassiveSkill:
						{
							string[] tempS = rewardText.Split(',');
							int passiveSkillID = StringUtil.getInt(tempS[0]);
							int num = StringUtil.getInt(tempS[1]);
							
							PassiveSkillData psd = PassiveSkillData.getData(passiveSkillID);
							if(psd == null)
							{
								rewardObj.SetActive(false);
								continue;
							}
							cardInfo.gameObject.SetActive(true);
							cardInfo.setSimpleCardInfo(passiveSkillID,GameHelper.E_CardType.E_PassiveSkill);
							
							rewardLabel.text = psd.name + " x " + num.ToString();
							
						}break;
						case (int)ActRewardType.E_Gold:
						{
							rewardIconBG.gameObject.SetActive(true);
							rewardIconBG.atlas = otherAtlas;
							rewardIconBG.spriteName = goldSpriteName;
							rewardLabel.text = "x " + rewardText;
							
						}break;
						case (int)ActRewardType.E_Exp:
						{
							rewardIconBG.gameObject.SetActive(true);
							rewardIconBG.atlas = otherAtlas;
							rewardIconBG.spriteName = expSpriteName;
							rewardLabel.text = "x " + rewardText;
						}break;
						case (int)ActRewardType.E_Crystal:
						{
							rewardIconBG.gameObject.SetActive(true);
							rewardIconBG.atlas = otherAtlas;
							rewardIconBG.spriteName = crystalSpriteName;
							rewardLabel.text = "x " + rewardText;
						}break;
						case (int)ActRewardType.E_Rune:
						{
						cardInfo.gameObject.SetActive(true);
						cardInfo.setSpecialIconInfo(GameHelper.E_CardType.E_Rune);

							rewardLabel.text = "x " + rewardText;
						}break;
						case (int)ActRewardType.E_Power:
						{
						cardInfo.gameObject.SetActive(true);
						cardInfo.setSpecialIconInfo(GameHelper.E_CardType.E_Power);
						
							rewardLabel.text = "x " + rewardText;
						}break;
					}
				}
			}
		}
	}
	
	//得到物品的名称及数量//
	string GetRewardString(int type,string rId)
	{
		string str = "";
		switch(type)
		{
			case (int)ActRewardType.E_Item:
			{
				string[] tempS = rId.Split(',');
				int itemID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				ItemsData itemData = ItemsData.getData(itemID);
				if(itemData != null)
				{
					str = itemData.name + " x " + num.ToString();
				}
				
			}break;
			case (int)ActRewardType.E_Equip:
			{
				string[] tempS = rId.Split(',');
				int equipID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				EquipData ed = EquipData.getData(equipID);
				if(ed != null)
				{
					str = ed.name + " x " + num.ToString();
				}
			}break;
			case (int)ActRewardType.E_Card:
			{
				string[] tempS = rId.Split(',');
				int heroID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				CardData cd = CardData.getData(heroID);
				if(cd != null)
				{
					str = cd.name + " x " + num.ToString();
				}
			}break;
			case (int)ActRewardType.E_Skill:
			{
				string[] tempS = rId.Split(',');
				int skillID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				SkillData sd = SkillData.getData(skillID);
				if(sd != null)
				{
					str = sd.name + " x " + num.ToString();
				}
			}break;
			case (int)ActRewardType.E_PassiveSkill:
			{
				string[] tempS = rId.Split(',');
				int passiveSkillID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				PassiveSkillData psd = PassiveSkillData.getData(passiveSkillID);
				if(psd != null)
				{
					str = psd.name + " x " + num.ToString();
				}
			}break;
			case (int)ActRewardType.E_Gold:
			{
				string s = TextsData.getData(59).chinese;
				str = s + " x " + rId;
			}break;
			case (int)ActRewardType.E_Exp:
			{
				string s = TextsData.getData(662).chinese;
				str = s + " x " + rId;
			}break;
			case (int)ActRewardType.E_Crystal:
			{
				string s = TextsData.getData(659).chinese;
				str = s + " x " + rId;
			}break;
			case (int)ActRewardType.E_Rune:
			{
				string s = TextsData.getData(660).chinese;
				str = s + " x " + rId;
			}break;
			case (int)ActRewardType.E_Power:
			{
				string s = TextsData.getData(661).chinese;
				str = s + " x " + rId;
			}break;
			case (int)ActRewardType.E_Friend:
			{
				string s = TextsData.getData(657).chinese;
				str = s + " x " + rId;
			}break;
		}
		return str;
	}
}
