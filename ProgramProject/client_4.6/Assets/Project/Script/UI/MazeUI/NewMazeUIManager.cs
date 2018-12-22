using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NewMazeUIManager : MonoBehaviour,ProcessResponse,BWWarnUI {
	 
	public UISlider[] hpBar;//血条//
	
	public UISprite[] cardIcon;//头像//
	
	public UISprite[] cardStar;//卡牌星级//
	
	public UISprite[] flipCard;//三张卡//
	
	public GameObject[] addBloodEffect;//加血特效//
	
	private List<GameObject> curAddBloodEffect = new List<GameObject>();
	
	public GameObject[] addBoomEffect;//扣血特效//
	
	private List<GameObject> curAddBoomEffect = new List<GameObject>();
	
	public UILabel goldNumLab;//金币//
	
	public UILabel medicineNumLab;//血瓶数量//
	
	public UILabel hornNumLab;//号角//
	
	public UISprite bg;
	
	public UISlider scheduleBar;
	
	public GameObject danger;
	
	public GameObject bossDanger;
	
	public GameObject winReward;
	
	public int scheduleNum;//进度//
	
	private bool isMapMove;
	
	private int curMazeId;//迷宫关卡id//
	
	public UISprite[] bgArray;
	
	private int requestType;
	
	private int errorCode;
	
	private bool receiveData;
	
	private int dropItemType;//迷宫奖励类型//
	
	private MazeResultJson mrj;
	
	private bool isBattle = false;
	
	//private bool isShowCardOver;
	
	public static int hornNum;
	
	public static int goldNum;
	
	public static int medicineNum;//血瓶//
	
	public static bool isFirstComeIn = true;
	
	private int curSelectCardNum;//记录选择翻转的卡牌//
	
	//1,翻牌提示  2，返回选关界面提示  3，提示购买血瓶界面//
	private int toastType;
	
	//服务器返回的选中的迷宫的id//
	private int selMazeId;
	//冷却时间//
	private int cdTime;
	
	private bool isBossBattle;
	
	private int addBloodId;//加血量表格对应id//
	
	private int buyBloodUseCost;//购买血瓶需要钻石//
	
	//充值界面的json//
	private RechargeUiResultJson rechargeRJ;
	
	private string bossReward;//通关迷宫的boss奖励//
	
	public UISprite rewardIcon;//钻石金币等icon//
	
	public UILabel rewardName;
	
	public UILabel rewardNum;
	
	public SimpleCardInfo2 rewardIcon2;//卡牌icon//
	
	public GameObject bossRewardInfo;
	
	public GameObject bossRewardBox;
	
	public static int bossBattleId;
	
	public GameObject gameStart;
	
	bool waitMoveForShowGuideUseHP;
	
	public bool waitMoveForShowGuidePoint;
	
	public GameObject backBtnObj;
	
	bool isOnShowCard = false;//是否是正在翻转//
	
	public bool finishOnceMove = false;
	
	private int getBoomNum;//抽到炸弹的次数//
	
	public GameObject bloodEffect;//血瓶特效//
	
	public GameObject fightEffect;//战斗开始特效//
	
	MazeBattleResultJson mbrj;
	
	void Awake()
	{
		if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_MAZE&&!isFirstComeIn)
		{
			HeadUI.mInstance.hide();
			initData();
			for(int i = 0;i<flipCard.Length;i++)
			{
				flipCard[i].gameObject.SetActive(false);
				flipCard[i].transform.FindChild("Reward").gameObject.SetActive(false);
				flipCard[i].transform.FindChild("RewardLab").gameObject.SetActive(false);
				flipCard[i].spriteName = "new-015";
			}
			if(PlayerInfo.getInstance().mazeBattleType == STATE.MAZE_BATTLE_TYPE_BOSS&&Battle_Player_Info.instance.BattleResult==1)
			{
				//显示关卡胜利界面//
				bloodEffect.SetActive(false);
				winReward.SetActive(true);
				bossRewardBox.SetActive(true);
				bossRewardInfo.SetActive(false);
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
				{
					UISceneDialogPanel.mInstance.showDialogID(33);
				}
			}
			else
			{
				waitMoveForShowGuideUseHP = false;
				
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace) && isCanUseMedicine() && !GuideManager.getInstance().hasUseHP)
				{
					waitMoveForShowGuideUseHP = true;
				}
				Invoke("MapMove",0.3f);	
			}
			
		}
	}

	void Update ()
	{
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
				if(errorCode == 0)
				{
					if(mrj.cb!=null&&!mrj.cb.Equals(""))
					{
						PlayerInfo.getInstance().curMazeBattleCardHp.Clear();
						string[] curHpstr = mrj.cb.Split('&');
						for(int i = 0;i<curHpstr.Length;i++)
						{
							if(curHpstr[i]!="")
							{
								string[] ss = curHpstr[i].Split('-');
								PlayerInfo.getInstance().curMazeBattleCardHp.Add(StringUtil.getInt(ss[0]),StringUtil.getInt(ss[1]));
							}
						}
					}
					//ShowCard();
					if(!isBossBattle)
					{
						DoawCardSpirt();
					}
				}
				break;
			case 2:
				if(errorCode == 0)
				{
					//设置战斗数据//
					PlayerInfo.getInstance().mbrj = mbrj;
					PlayerInfo.getInstance().battleType = STATE.BATTLE_TYPE_MAZE;
					PlayerInfo.getInstance().curMazeId = mbrj.td;
					PlayerInfo.getInstance().curPosId = mbrj.state;
					PlayerInfo.getInstance().mazeBattleType = mbrj.type;
					bossBattleId = mbrj.md;
					PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_MAZE;
					isFirstComeIn = false;
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
				}
				else
				{
					Invoke("WaitShowErrorCodeToast",1f);
				}
				
				break;
			case 3:
				receiveData = false;
//				WarpSpaceUIManager.mInstance.SetData(PlayerInfo.getInstance().curOpenMazeId, PlayerInfo.getInstance().curIntoMaze, selMazeId, cdTime);
				//打开扭曲空间界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE);
				WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager")as WarpSpaceUIManager;
				warpSpace.SetData(PlayerInfo.getInstance().curOpenMazeId, PlayerInfo.getInstance().curIntoMaze, selMazeId, cdTime);
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				hide();
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
				{
					GuideUI18_Skill.mInstance.showStep(2);
				}
				break;
			case 4:
				if(errorCode == 0)
				{
					//把血量加满//
					medicineNum--;
					foreach(KeyValuePair<int,int> kv in PlayerInfo.getInstance().maxMazeBattleCardHp)
					{
						PlayerInfo.getInstance().curMazeBattleCardHp[kv.Key] = kv.Value;
					}
					ChangeBlood(1,1);
					getBoomNum = 0;
					bloodEffect.SetActive(false);
					RefreshData();
				}
				else if(errorCode == 120)
				{
					//请求血瓶购买界面//
					if(!isBossBattle)
					{
						requestType = 5;
						PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(1,10,1),this);
					}
					else
					{
						Invoke("WaitShowToast",1f);
					}
				}
				break;
			case 5:
				if(errorCode == 0)
				{
					toastType = 3;
					ToastWarnUI.mInstance.showWarn(TextsData.getData(689).chinese.Replace("num",buyBloodUseCost.ToString()),this);
				}
				break;
			case 6:
				if(errorCode == 0)
				{
					medicineNum++;
					OnClickUseMedicine();
				}
				else if(errorCode == 19)
				{
					
					requestType = 7;
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CHARGE),this);
				}
				break;
			case 7:
				if(errorCode == 0)
				{
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ);
					ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ, 
						"ChargePanel") as ChargePanel;
					charge.curRechargeResult = rechargeRJ;
					charge.SetIsMazeComeIn(true);
					//如果vipCost是0表示没有充值过，是第一次充值//
					if(rechargeRJ.vipCost == 0)
					{
						charge.firstCharge = 0;
					}
					else
					{
						charge.firstCharge = rechargeRJ.vipCost;
					}
                    charge.isShowType = 1;
					charge.show();
				}
				break;
			case 8:
				if(errorCode == 0)
				{
					ShowBossBoxEffect();
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
					{
						GuideUI18_Skill.mInstance.showStep(10);
					}
				}
				break;
			}
		}
	}
	
	void initData()
	{
		waitMoveForShowGuideUseHP = false;
		waitMoveForShowGuidePoint = false;
		curMazeId = PlayerInfo.getInstance().curMazeId;
		scheduleNum = PlayerInfo.getInstance().curPosId;
		RefreshData();
		MazeData md = MazeData.getData(curMazeId);
		if(md != null)
			scheduleBar.value = scheduleNum/((float)md.step);
		for(int i = 0;i<bgArray.Length;i++)
		{
			bgArray[i].gameObject.SetActive(false);
		}
		DrawIconAndHp();
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
		{
			backBtnObj.SetActive(false);
		}
		else
		{
			backBtnObj.SetActive(true);
		}
		bloodEffect.SetActive(false);
		isBattle = false;
	}
	
	void RefreshData()
	{
		medicineNumLab.text = medicineNum.ToString();
		goldNumLab.text = goldNum.ToString();
		hornNumLab.text = hornNum.ToString();
	}
	
	void hide()
	{
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE);
		Resources.UnloadUnusedAssets();
		HeadUI.mInstance.refreshPlayerInfo();
		isFirstComeIn = true;
		PlayerInfo.getInstance().curMazeBattleCardHp.Clear();
		PlayerInfo.getInstance().maxMazeBattleCardHp.Clear();
		goldNum = 0;
		hornNum = 0;
	}
	
	public void SetData(int medicneNum,int mazeId,string cardhpinfo,string cardmaxhpinfo)
	{
		medicineNum = medicneNum;
		curMazeId = mazeId;
		isMapMove = false;
		getBoomNum = 0;
		if(cardhpinfo!=null&&!cardhpinfo.Equals(""))
		{
			PlayerInfo.getInstance().curMazeBattleCardHp.Clear();
			string[] curHpstr = cardhpinfo.Split('&');
			for(int i = 0;i<curHpstr.Length;i++)
			{
				if(curHpstr[i]!="")
				{
					string[] ss = curHpstr[i].Split('-');
					PlayerInfo.getInstance().curMazeBattleCardHp.Add(StringUtil.getInt(ss[0]),StringUtil.getInt(ss[1]));
				}
			}
		}
		if(cardmaxhpinfo!=null&&!cardmaxhpinfo.Equals(""))
		{
			PlayerInfo.getInstance().maxMazeBattleCardHp.Clear();
			string[] maxHpstr = cardmaxhpinfo.Split('&');
			for(int i = 0;i<maxHpstr.Length;i++)
			{
				if(maxHpstr[i]!="")
				{
					string[] ss = maxHpstr[i].Split('-');
					PlayerInfo.getInstance().maxMazeBattleCardHp.Add(StringUtil.getInt(ss[0]),StringUtil.getInt(ss[1]));
				}
			}
		}
		
		initData();
		gameStart.SetActive(true);
		//地图播放前进效果，隐藏三个卡牌，关闭所有点击效果//
		for(int i = 0;i<flipCard.Length;i++)
		{
			flipCard[i].gameObject.SetActive(false);
			flipCard[i].transform.FindChild("Reward").gameObject.SetActive(false);
			flipCard[i].transform.FindChild("RewardLab").gameObject.SetActive(false);
			flipCard[i].spriteName = "new-015";
		}
		Invoke("ShowFightEffect",0.6f);
		Invoke("MapMove",0.8f);
	}
	
	public void DrawIconAndHp()
	{
		//设置阵容头像和血条//
		int j = 0;
		int k = 1;
		PlayerInfo.getInstance().MazeBattleHpScale.Clear();
		
		List<int> cardIdArray = new List<int>();
		foreach(KeyValuePair<int,int> kv in PlayerInfo.getInstance().maxMazeBattleCardHp)
		{
			cardIdArray.Add(kv.Key);
		}
		
		
		for(int i = 0;i<6;i++)
		{
			try
			{
				int cardId = cardIdArray[i];
				CardData cd = CardData.getData(cardId);
				cardIcon[j].gameObject.SetActive(true);
				cardIcon[j].atlas = LoadAtlasOrFont.LoadHeroAtlasByName(cd.atlas);
				cardIcon[j].spriteName = cd.icon;
				cardStar[j].atlas = LoadAtlasOrFont.LoadHeroAtlasByName("InterfaceFrameBgAtlas01");
				cardStar[j].spriteName = "head_star_"+cd.star;
				
				curAddBloodEffect.Add(addBloodEffect[j]);
				curAddBoomEffect.Add(addBoomEffect[j]);
				PlayerInfo.getInstance().MazeBattleHpScale.Add(cd.id,hpBar[j]);
				
				hpBar[j].gameObject.SetActive(true);
				float curHp = PlayerInfo.getInstance().curMazeBattleCardHp[cardId];
				float maxHp = PlayerInfo.getInstance().maxMazeBattleCardHp[cardId];
				hpBar[j].value = curHp/maxHp;
				j++;
			}
			catch
			{
				//位置头像设置为无阵容状态//
				int index = 6-k;
				hpBar[index].value = 0;
				cardIcon[index].gameObject.SetActive(false);
				cardStar[index].atlas = LoadAtlasOrFont.LoadHeroAtlasByName("InterfaceAtlas01");
				cardStar[index].spriteName = "frame04";
				k++;
			}
		}
	}
	
	void MapMove()
	{
		gameStart.SetActive(false);
		//地图播放前进效果，隐藏三个卡牌，关闭所有点击效果//
		for(int i = 0;i<flipCard.Length;i++)
		{
			flipCard[i].gameObject.SetActive(false);
			flipCard[i].transform.FindChild("Reward").gameObject.SetActive(false);
			flipCard[i].transform.FindChild("RewardLab").gameObject.SetActive(false);
			flipCard[i].spriteName = "new-015";
		}
		TweenPosition.Begin(bg.gameObject,0.5f,new Vector3(0,15,0));
		bg.GetComponent<TweenPosition>().style = UITweener.Style.PingPong;
		TweenScale.Begin(bg.gameObject,1f,new Vector3(1.6f,1.6f,1));
		EventDelegate.Add(bg.GetComponent<TweenScale>().onFinished,OnMoveFinshed);
		scheduleNum++;
		isMapMove = true;
	}
	
	void OnMoveFinshed()
	{
		bg.GetComponent<TweenPosition>().enabled = false;
		
		MazeData md = MazeData.getData(curMazeId);
		if(md!= null)
			scheduleBar.value = scheduleNum/((float)md.step);
		for(int i = 0;i<3;i++)
		{
			bgArray[i].gameObject.SetActive(true);
			GameObject bgc = bgArray[i].gameObject;
			bgc.name = "bg"+i;
			float scale = 1+0.2f*(i+1);
			bgc.transform.localScale = new Vector3(scale,scale,1);
			TweenAlpha.Begin(bgc,(0.4f-i*0.1f),0);
			if(i == 0)
				EventDelegate.Add(bgc.GetComponent<TweenAlpha>().onFinished,OnHideBgFinshed);
		}
		bg.transform.localScale = new Vector3(1,1,1);
		bg.transform.localPosition = new Vector3(0,-15,0);
	}
	
	void OnHideBgFinshed()
	{
		if(!finishOnceMove)
		{
			finishOnceMove = true;
		}
		for(int i = 0;i<bgArray.Length;i++)
		{
			bgArray[i].color = Color.white;
			bgArray[i].gameObject.SetActive(false);
		}
		MazeData md = MazeData.getData(curMazeId);
		if(scheduleNum > md.step)
		{
			//boss战,直接提示,不选择//
			ShowBossBattle();
			isBossBattle = true;
		}
		else
		{
			for(int i = 0;i<flipCard.Length;i++)
			{
				flipCard[i].gameObject.SetActive(true);
			}
		}
		isMapMove = false;
		IsNeedShowBloodEffect();
		
		//isShowCardOver = false;
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
		{
			if(waitMoveForShowGuidePoint)
			{
				waitMoveForShowGuidePoint = false;
				GuideUI17_WarpSpace.mInstance.showStep(4);
			}
			else	if(waitMoveForShowGuideUseHP)
			{
				GuideManager.getInstance().hasUseHP = true;
				waitMoveForShowGuideUseHP = false;
				GuideUI17_WarpSpace.mInstance.showStep(5);
			}
			
		}
		
	}
	
	public void IsNeedShowBloodEffect()
	{
		//判断血量满不满，不满就显示血瓶特效//
		bool isCanUse = false;
		List<int> curHp = new List<int>();
		List<int> maxHp = new List<int>();
		foreach(KeyValuePair<int,int> de in PlayerInfo.getInstance().curMazeBattleCardHp)
		{
			curHp.Add(de.Value);
		}
		foreach(KeyValuePair<int,int> de in PlayerInfo.getInstance().maxMazeBattleCardHp)
		{
			maxHp.Add(de.Value);
		}
		for(int i =0;i<curHp.Count;i++)
		{
			if(curHp[i]<maxHp[i]&&maxHp[i]!=-1)
			{
				isCanUse = true;
			}
		}
		
		if(isCanUse)
		{
			bloodEffect.SetActive(true);
		}
		else
		{
			bloodEffect.SetActive(false);
		}
	}
	
	public void OnClickSelectCard(int param)
	{
		//点击卡牌//
		if(isMapMove||isOnShowCard||isBattle)
			return;
		curSelectCardNum = param;
		requestType = 1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAZE_GETDATA, curMazeId, 0, scheduleNum),this);
		isOnShowCard = true;
	}
	
	void ShowCard()
	{
		//翻转点击卡牌//
		
		//isShowCardOver = true;
	}
	
	void DoawCardSpirt()
	{
		Quaternion rot = new Quaternion(1,1,1,1);
		rot.eulerAngles = new Vector3(0,90,0);
		TweenRotation.Begin(flipCard[curSelectCardNum].gameObject,0.15f,rot);
		EventDelegate.Add(flipCard[curSelectCardNum].GetComponent<TweenRotation>().onFinished,OnCardRotationFinished);
	}
	
	void OnCardRotationFinished()
	{
		//转半圈结束//
		EventDelegate.Remove(flipCard[curSelectCardNum].GetComponent<TweenRotation>().onFinished,OnCardRotationFinished);
		flipCard[curSelectCardNum].spriteName = "new-016";
		string spriteName = "";
		string labSpiriteName = "";
		//获取的物品的类型：1 治疗， 2 号角， 3 炸弹， 4 金币，5精英战斗，6普通战斗，7boss//
		switch(dropItemType)
		{
		case 1:
			spriteName = "new-010";
			labSpiriteName = "name3";
			break;
		case 2:
			spriteName = "haojiao";
			labSpiriteName = "name4";
			break;
		case 3:
			spriteName = "bomb";
			labSpiriteName = "name5";
			break;
		case 4:
			spriteName = "gold";
			labSpiriteName = "name1";
			break;
		case 5:
			spriteName = "jingying";
			labSpiriteName = "name7";
			break;
		case 6:
			spriteName = "zhandou";
			labSpiriteName = "name6";
			break;
		case 7:
			spriteName = "boss";
			labSpiriteName = "name2";
			break;
		}
		GameObject rewardSpirt = flipCard[curSelectCardNum].transform.FindChild("Reward").gameObject;
		GameObject rewardSpirtLab = flipCard[curSelectCardNum].transform.FindChild("RewardLab").gameObject;
		rewardSpirt.GetComponent<UISprite>().spriteName = spriteName;
		rewardSpirtLab.GetComponent<UISprite>().spriteName = labSpiriteName;
		rewardSpirt.SetActive(true);
		rewardSpirtLab.SetActive(true);
		Quaternion rot = new Quaternion(1,1,1,1);
		rot.eulerAngles = new Vector3(0,0,0);
		TweenRotation.Begin(flipCard[curSelectCardNum].gameObject,0.15f,rot);
		
		Invoke("ShowEffect",0.5f);
	}
	
	void OnCardRotationAllFinished()
	{
		//转整圈结束，显示奖励卡牌//
		switch(dropItemType)
		{
		case 3:
			flipCard[curSelectCardNum].transform.FindChild("fanpan_debuff").gameObject.SetActive(false);
			break;
		case 5:
		case 6:
			flipCard[curSelectCardNum].transform.FindChild("fanpan_fight").gameObject.SetActive(false);
			break;
		}
		//获取的物品的类型：1 治疗， 2 号角， 3 炸弹， 4 金币，5精英战斗，6普通战斗，7boss//
		toastType = 1;
		switch(dropItemType)
		{
		case 1:
			BloodBuffData bbd = BloodBuffData.getData(addBloodId);
			ToastWindow.mInstance.showText(TextsData.getData(685).chinese.Replace("num",(bbd.effect*100).ToString()),this);
			break;
		case 2:
			string text = TextsData.getData(681).chinese.Replace("num","1");
			ToastWindow.mInstance.showText(text,this);
			break;
		case 3:
			BloodBuffData bbd2 = BloodBuffData.getData(addBloodId);
			ToastWindow.mInstance.showText(TextsData.getData(682).chinese.Replace("num",(bbd2.effect*100).ToString()),this);
			break;
		case 4:
			text = TextsData.getData(680).chinese.Replace("num",mrj.i.ToString());
			ToastWindow.mInstance.showText(text,this);
			break;
		case 5:
			bloodEffect.SetActive(false);
			ToastWindow.mInstance.showText(TextsData.getData(684).chinese,this);
			break;
		case 6:
			bloodEffect.SetActive(false);
			ToastWindow.mInstance.showText(TextsData.getData(683).chinese,this);
			break;
		case 7:
			ToastWindow.mInstance.showText(TextsData.getData(685).chinese,this);
			break;		
		}
	}
	
	void ShowEffect()
	{
		switch(dropItemType)
		{
		case 3:
			flipCard[curSelectCardNum].transform.FindChild("fanpan_debuff").gameObject.SetActive(true);
			Invoke("OnCardRotationAllFinished",1f);
			break;
		case 5:
		case 6:
			flipCard[curSelectCardNum].transform.FindChild("fanpan_fight").gameObject.SetActive(true);
			Invoke("OnCardRotationAllFinished",1f);
			break;
		case 1:
		case 2:
		case 4:
		case 7:
			Invoke("OnCardRotationAllFinished",0.5f);
			break;
		}
	}
	
	void ShowBossBattle()
	{
		danger.SetActive(true);
		for(int i = 0;i<flipCard.Length;i++)
		{
			flipCard[i].gameObject.SetActive(false);
			flipCard[i].transform.FindChild("Reward").gameObject.SetActive(false);
			flipCard[i].transform.FindChild("RewardLab").gameObject.SetActive(false);
			flipCard[i].spriteName = "new-015";
		}
		Invoke("OnDangerMoveFinished",3);
	}
	
	void OnDangerMoveFinished()
	{
		danger.SetActive(false);
		bossDanger.SetActive(true);
		Invoke("ComeInBossBattle",1.5f);
	}
	
	void ComeInBossBattle()
	{
		bool isNeedTipUseBlood = false;
		for(int i =0;i<hpBar.Length;i++)
		{
			if(hpBar[i].value!=0&&hpBar[i].value<=0.5)
			{
				isNeedTipUseBlood = true;
			}
		}
		if(isNeedTipUseBlood)
		{
			toastType = 4;
			ToastWarnUI.mInstance.showWarn(TextsData.getData(691).chinese,this);
		}
		else
		{
			bloodEffect.SetActive(false);
			requestType = 2;
			GuideManager.getInstance().isMazeBoss = true;
			PlayerInfo.getInstance().mazeBattleType = STATE.MAZE_BATTLE_TYPE_BOSS;
			PlayerInfo.getInstance().sendRequest(new MazeBattleJson(STATE.MAZE_BATTLE_TYPE_BOSS, curMazeId, PlayerInfo.getInstance().curPosId),this);
		}
	}
	
	public void warnningCancel()
	{
		//获取的物品的类型：1 治疗， 2 号角， 3 炸弹， 4 金币，5精英战斗，6普通战斗，7boss//
		Debug.Log("+++++++++++++++"+toastType);
		if(toastType == 1)
		{ 
			switch(dropItemType)
			{
			case 1:
				BloodBuffData bbd = BloodBuffData.getData(addBloodId);
				ChangeBlood(bbd.type,bbd.effect);
				IsNeedShowBloodEffect();
				MapMove();
				break;
			case 2:
				hornNum = StringUtil.getInt(hornNumLab.text) + 1;
				RefreshData();
				MapMove();
				break;
			case 3:
				BloodBuffData bbd2 = BloodBuffData.getData(addBloodId);
				getBoomNum++;
				ChangeBlood(bbd2.type,bbd2.effect);
				MapMove();
				break;
			case 4:
				PlayerInfo.getInstance().player.gold += mrj.i;
				goldNum = StringUtil.getInt(goldNumLab.text)+ mrj.i;
				RefreshData();
				MapMove();
				break;
			case 5:
				
				requestType = 2;
				PlayerInfo.getInstance().mazeBattleType = STATE.MAZE_BATTLE_TYPE_DROPS;
				PlayerInfo.getInstance().sendRequest(new MazeBattleJson(3, curMazeId, scheduleNum),this);
				isBattle = true;
				break;
			case 6:
				requestType = 2;
				PlayerInfo.getInstance().mazeBattleType = STATE.MAZE_BATTLE_TYPE_DROPS;
				PlayerInfo.getInstance().sendRequest(new MazeBattleJson(1, curMazeId, scheduleNum),this);
				isBattle = true;
				break;
			case 7:
				
				break;
			}
			
			isOnShowCard = false;
		}
		else if(toastType == 2)
		{
			//取消返回选关操作//
		}
		else if(toastType == 3)
		{
			//取消购买血瓶//
			if(isBossBattle)
			{
				requestType = 2;
				GuideManager.getInstance().isMazeBoss = true;
				PlayerInfo.getInstance().mazeBattleType = STATE.MAZE_BATTLE_TYPE_BOSS;
				PlayerInfo.getInstance().sendRequest(new MazeBattleJson(STATE.MAZE_BATTLE_TYPE_BOSS, curMazeId, PlayerInfo.getInstance().curPosId),this);
			}
		}
		else if(toastType == 4)
		{
			requestType = 2;
			GuideManager.getInstance().isMazeBoss = true;
			PlayerInfo.getInstance().mazeBattleType = STATE.MAZE_BATTLE_TYPE_BOSS;
			PlayerInfo.getInstance().sendRequest(new MazeBattleJson(STATE.MAZE_BATTLE_TYPE_BOSS, curMazeId, PlayerInfo.getInstance().curPosId),this);
		}
		else if(toastType == 5)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
		}
	}
	
	public void warnningSure()
	{
		if(toastType == 2)
		{
				//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
		}
		else if(toastType == 3)
		{
			//确认购买血瓶//
			requestType = 6;
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(2,10,1),this);
		}
		else if(toastType == 4)
		{
			OnClickUseMedicine();
		}
		else if(toastType == 5)
		{
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
		}
	}
	
	public void OnClickBack(int param)
	{
		if(param == 0)
		{
			toastType = 2;
			ToastWarnUI.mInstance.showWarn(TextsData.getData(687).chinese,this);
		}
		else if(param == 1)
		{
				//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
		}
	}
	
	public bool isCanUseMedicine()
	{
		List<int> curHp = new List<int>();
		List<int> maxHp = new List<int>();
		foreach(KeyValuePair<int,int> de in PlayerInfo.getInstance().curMazeBattleCardHp)
		{
			curHp.Add(de.Value);
		}
		foreach(KeyValuePair<int,int> de in PlayerInfo.getInstance().maxMazeBattleCardHp)
		{
			maxHp.Add(de.Value);
		}
		for(int i =0;i<curHp.Count;i++)
		{
			if((curHp[i]<maxHp[i]&&maxHp[i]!=-1)||getBoomNum>0)
			{
				return true;
			}
		}
		return false;
	}
	
	public void OnClickUseMedicine()
	{
		if(!isCanUseMedicine())
			return;
		if(medicineNum>=0&&!isMapMove)
		{
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAZE_USEBLOOD,curMazeId,130001),this);
		}
	}
	
	public void ChangeBlood(int changeType,float changeValue)
	{
		if(changeType == 1)
		{
			for(int i =0;i<curAddBloodEffect.Count;i++)
			{
				curAddBloodEffect[i].SetActive(true);
			}
			DrawIconAndHp();
		}
		else if(changeType == 2)
		{
			for(int i =0;i<curAddBoomEffect.Count;i++)
			{
				curAddBoomEffect[i].SetActive(true);
			}
			DrawIconAndHp();
		}
		
		Invoke("ShowBloodEffectOver",0.5f);
	}
	
	public void ShowBloodEffectOver()
	{
		for(int i =0;i<curAddBloodEffect.Count;i++)
		{
			curAddBloodEffect[i].SetActive(false);
		}
		for(int i =0;i<curAddBoomEffect.Count;i++)
		{
			curAddBoomEffect[i].SetActive(false);
		}
		if(isBossBattle)
		{
			bloodEffect.SetActive(false);
			requestType = 2;
			GuideManager.getInstance().isMazeBoss = true;
			PlayerInfo.getInstance().mazeBattleType = STATE.MAZE_BATTLE_TYPE_BOSS;
			PlayerInfo.getInstance().sendRequest(new MazeBattleJson(STATE.MAZE_BATTLE_TYPE_BOSS, curMazeId, PlayerInfo.getInstance().curPosId),this);
		}
	}
	
	public void OnclickBossReward(int param)
	{
		requestType = 8;
		UIJson json = new UIJson();
		json.UIJsonForMazeBossReward(STATE.UI_MAZE_BOSSREWARD,bossBattleId,1);
		PlayerInfo.getInstance().sendRequest(json,this);
	}
	
	public void ShowBossBoxEffect()
	{
		bossRewardBox.GetComponent<UISprite>().spriteName = "new-09";
		bossRewardBox.transform.FindChild("kaixiang_ui").gameObject.SetActive(true);
		Invoke("OpenBossReward",1f);
	}
	
	public void OpenBossReward()
	{
		bossRewardBox.SetActive(false);
		bossRewardInfo.SetActive(true);
		string[] ss = bossReward.Split('-');
		string item = ss[1];
		//1,items 2,equip 3,card 4,skill 5,passiveskill 6,金币 7,人物经验值 8,水晶 9,符文值 10,体力 11，友情值//
		int type = StringUtil.getInt(ss[0]);
		string []str = item.Split(',');
		int id = 0;
		int num = 0;
		switch(type)
		{
		case 1:
			id = StringUtil.getInt(str[0]);
			num = StringUtil.getInt(str[1]);
			ItemsData itd = ItemsData.getData(id);
			if(itd!=null)
			{
				rewardIcon2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Item);
				rewardIcon2.gameObject.SetActive(true);	
				rewardIcon.gameObject.SetActive(false);
				rewardNum.text = "x"+num;
				rewardName.text = itd.name;
			}
			break;
		case 2:
			id = StringUtil.getInt(str[0]);
			num = StringUtil.getInt(str[1]);
			EquipData ed = EquipData.getData(id);
			if(ed!=null)
			{
				rewardIcon2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Equip);
				rewardIcon2.gameObject.SetActive(true);	
				rewardIcon.gameObject.SetActive(false);
				rewardNum.text = "x"+num;
				rewardName.text = ed.name;
			}
			break;
		case 3:
			id = StringUtil.getInt(str[0]);
			num = StringUtil.getInt(str[1]);
			CardData cd = CardData.getData(id);
			if(cd!=null)
			{
				rewardIcon2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Hero);
				rewardIcon2.gameObject.SetActive(true);	
				rewardIcon.gameObject.SetActive(false);
				rewardNum.text = "x"+num;
				rewardName.text = cd.name;
			}
			break;
		case 4:
			id = StringUtil.getInt(str[0]);
			num = StringUtil.getInt(str[1]);
			SkillData sd = SkillData.getData(id);
			if(sd!=null)
			{
				rewardIcon2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Skill);
				rewardIcon2.gameObject.SetActive(true);	
				rewardIcon.gameObject.SetActive(false);
				rewardNum.text = "x"+num;
				rewardName.text = sd.name;
			}
			break;
		case 5:
			id = StringUtil.getInt(str[0]);
			num = StringUtil.getInt(str[1]);
			PassiveSkillData pd = PassiveSkillData.getData(id);
			if(pd!=null)
			{
				rewardIcon2.setSimpleCardInfo(id,GameHelper.E_CardType.E_PassiveSkill);
				rewardIcon2.gameObject.SetActive(true);
				rewardIcon.gameObject.SetActive(false);
				rewardNum.text = "x"+num;
				rewardName.text = pd.name;
			}
			break;
		case 6:
			rewardName.text = TextsData.getData(658).chinese;
			rewardNum.text = "x"+item;
			rewardIcon.spriteName = "gold";
			rewardIcon.gameObject.SetActive(true);
			rewardIcon2.gameObject.SetActive(false);
			break;
		case 7:
			rewardIcon.gameObject.SetActive(false);
			rewardIcon2.gameObject.SetActive(false);
			break;
		case 8:
			rewardName.text = TextsData.getData(659).chinese;
			rewardNum.text = "x"+item;
			rewardIcon.spriteName = "crystal2";
			rewardIcon.gameObject.SetActive(true);
			rewardIcon2.gameObject.SetActive(false);
			break;
		case 9:
			rewardName.text = TextsData.getData(660).chinese;
			rewardNum.text = "x"+item;
			rewardIcon.spriteName = "rune";
			rewardIcon.gameObject.SetActive(true);
			rewardIcon2.gameObject.SetActive(false);
			break;
		case 10:
			rewardName.text = TextsData.getData(661).chinese;
			rewardNum.text = "x"+item;
			rewardIcon.spriteName = "power";
			rewardIcon.gameObject.SetActive(true);
			rewardIcon2.gameObject.SetActive(false);
			break;
		case 11:
			rewardIcon.gameObject.SetActive(false);
			rewardIcon2.gameObject.SetActive(false);
			break;
		}
	}
	
	//避免boss战斗前提示使用血瓶toast隐藏影响购买血瓶toast提示//
	public void WaitShowToast()
	{
		requestType = 5;
		PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(1,10,1),this);
	}
	
	//避免影响提示退出迷宫toast提示//
	public void WaitShowErrorCodeToast()
	{
		toastType = 5;
		ToastWindow.mInstance.showText(TextsData.getData(693).chinese,this);
	}
	
	public void ShowFightEffect()
	{
		fightEffect.SetActive(true);
	}
	
	public void receiveResponse (string json)
	{
		Debug.Log("MazeUIManager : " + json);
		if(json!= null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				mrj=JsonMapper.ToObject<MazeResultJson>(json);
				errorCode = mrj.errorCode;
				receiveData = true;
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().curMazeId = mrj.td;
					PlayerInfo.getInstance().curPosId = mrj.state;
					PlayerInfo.getInstance().curMazeNum = mrj.num;
					dropItemType = mrj.type;
					addBloodId = mrj.i;
				}
				
				break;
			case 2:
				mbrj=JsonMapper.ToObject<MazeBattleResultJson>(json);
				errorCode = mbrj.errorCode;
				receiveData=true;
				break;
			case 3:				//进入扭曲空间获取数据//
				MazeResultJson mrj02=JsonMapper.ToObject<MazeResultJson>(json);
				errorCode = mrj02.errorCode;
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().curOpenMazeId.Clear();
					PlayerInfo.getInstance().curOpenMazeId = mrj02.s;
					PlayerInfo.getInstance().curIntoMaze = mrj02.maze;
					selMazeId = mrj02.mId;
					cdTime = mrj02.cdtime;
					WarpSpaceUIManager.mazeWish = mrj02.mazeWish;
					WarpSpaceUIManager.mazeBossDrop = mrj02.mazeBossDrop;
				}
				receiveData = true;
				break;
			case 4:
				BloodResultJson brj = JsonMapper.ToObject<BloodResultJson>(json);
				errorCode = brj.errorCode;
				receiveData = true;
				break;
			case 5:
				BuyPowerOrGoldResultJson bpogr = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = bpogr.errorCode;
				buyBloodUseCost = bpogr.crystal;
				receiveData = true;
				break;
			case 6:
				BuyPowerOrGoldResultJson bpogr2 = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = bpogr2.errorCode;
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().player.crystal = bpogr2.crystal;
				}
				receiveData = true;
				break;
			case 7:
				//购买水晶//
				RechargeUiResultJson rechargej = JsonMapper.ToObject<RechargeUiResultJson>(json);
				errorCode = rechargej.errorCode;
				if(errorCode==0)
				{
					rechargeRJ = rechargej;
				}
				receiveData = true;
				break;
			case 8:
				MazeRewardResultJson mrrj = JsonMapper.ToObject<MazeRewardResultJson>(json);
				errorCode = mrrj.errorCode;
				if(errorCode == 0)
				{
					bossReward = mrrj.reward;
				}
				receiveData = true;
				break;
			}
		}
	}
}