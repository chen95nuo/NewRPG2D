using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultTipManager : BWUIPanel ,ProcessResponse,BWWarnUI{
	
	public static ResultTipManager mInstance;
	public List<GameObject> StarList;
	public GameObject battleWin;			//胜利的界面的父节点//
	public GameObject Win;					//胜利//
	public GameObject Lose;					//失败//
	public GameObject MazeLose;				//迷宫失败//
	public GameObject LightObj;
	public GameObject TipLabel;
//	public GameObject KO_Obj;				//KO//
	public UILabel playerLv;
	public UILabel gold;
	public UILabel koNum;							//ko积分数//
	public UILabel PvpHonorLabel;					//pvp荣誉点//
	public UILabel PvpLoseHonorLabel;					//pvp荣誉点//
	public GameObject koPrefabs;					//ko特效//
	public GameObject playerExp;
	public GameObject RewardsBounes;
	public GameObject[] battleCards;
	public GameObject[] rewardsCards;
	public GameObject winResultPos;
	public GameObject ShowPlayerDataObj;			//显示人物等级，经验条和获得的物品//
	public GameObject ContinueBtn;					//继续按钮//
	
	public List<Vector3> starPosList;
	public ResultPlayerLUItem PlayerLevelUpObj;		//军团升级界面//
	
	public UIAtlas headAtlas;
	public UIAtlas skillAtlas;
	public UIAtlas itemAtlas;
	public UIAtlas equipAtla;


    bool isEventBattle;
	//int result ;
	int showStarNum;						//显示的星星的个数//
	int curStar;
	//float startRotateZ;						//背后的光初始的角度//
	float frameCount;
	float rotateFrameCount;
	float waitForStarCount;
	
	private int errorCode;
	private bool receiveData;
	private bool isShowKO;
	private bool isPlayBounesNumAnim;
	float bounesSprFrameCount;
	float bounesSprStayTime = 0.5f;
	private int requestType;
	//0-starNum星星移动回调，starNum + 1 win开始移动，starNum + 2 win移动回调//
	private int state0Step;
	private int curState;
	public int state2ShowCardNum;
	Battle_Player_Info battleCardInfo;
	private GameObject BounesSprObj;
	private UILabel BounesNum;
	private ChangeNumber cn;
	private float FrameCount = 0;
	
	GameObject LevelUpBgEffect;
	//特效//
	GameObject koEff;
	//string itemAtlasName = "ItemCircularIcon";
	//string equpiAtlasName = "EquipCircularIcon";
	//string cardAtlasName = "HeadIcon";
	//string skillAtlasName = "SkillCircularIcom";
	//string passSkillAtlasName = "PassSkillCircularIcon";
	
    public UILabel[] DefeatedLabels;  //战斗失败跳转场景提示Label//
	public UILabel[] MazeDefeatedLabels;//迷宫战斗失败跳转场景提示Label//
	void Awake(){
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
		 
	}
	
	public override void init ()
	{
		base.init ();
		//startRotateZ = 0;
		starPosList = new List<Vector3>();
		for(int i = 0 ;i < StarList.Count;i++){
			starPosList.Add(StarList[i].transform.position);
		}
		if(Win == null){
			Win = _MyObj.transform.FindChild("Win").gameObject;
		}
		
		if(Lose == null){
			Lose = _MyObj.transform.FindChild("Lose").gameObject;
		}
		
		if(MazeLose == null){
			MazeLose = _MyObj.transform.FindChild("MazeBattleLose").gameObject;
		}
		
		if(LightObj == null){
			LightObj = _MyObj.transform.FindChild("Light").gameObject;
		}
		
		if(TipLabel == null){
			TipLabel = _MyObj.transform.FindChild("ClickNextLabel").gameObject;
		}
		if(cn == null)
		{
			cn = gold.gameObject.GetComponent<ChangeNumber>();
		}
		
		if(battleCardInfo == null)
		{
			battleCardInfo = Battle_Player_Info.GetInstance();
		}
		koNum.text = string.Empty;	
		koNum.gameObject.SetActive(false);
	}
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isCanChangeScene && curState == 4)
		{
			frameCount += Time.deltaTime;
			if(frameCount > 0.5f){
				frameCount = 0;
				TipLabel.SetActive(!TipLabel.activeSelf);
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
				//推图胜利或者失败,竞技场失败,迷宫失败,异世界失败,发送talkingdta//
				
				PlayerElement pe = PlayerInfo.getInstance().player;
				
				switch(PlayerInfo.getInstance().battleType){
					case STATE.BATTLE_TYPE_NORMAL :
						//普通推图//
						Battle_Player_Info battle_Player_Info = Battle_Player_Info.GetInstance();
						BattleResultJson brj=PlayerInfo.getInstance().brj;
						MissionData md = MissionData.getData(brj.md);
						int power = md.cost;
						string eventName = "NormalBattle";
						
						if(Battle_Player_Info.instance.BattleResult == 1)
						{
							//胜利//		
							if(!TalkingDataManager.isTDPC)
							{
								
								TDGAMission.OnCompleted(md.name);
							}
							int coinsNum =(int)(battle_Player_Info.isUseBonus ? battle_Player_Info.getCoins*battle_Player_Info.bonusAddNum : battle_Player_Info.getCoins);
							Dictionary<string,object> dic = new Dictionary<string, object>();
							dic.Add("BattleTape","Normal");
							dic.Add("Result","Win");				
													
							dic.Add("MissionId",md.name.ToString());			//副本id//
							dic.Add("PlayerBattlePower",pe.battlePower.ToString());       								//战斗力//
							dic.Add("UsePower",power.ToString());			//体力消耗//
							dic.Add("GetCoins",coinsNum.ToString());		//金币获得//
							dic.Add("PlayerId",pe.id.ToString());			//玩家id//
							TalkingDataManager.SendTalkingDataEvent(eventName,dic);                                     //发送关卡信息的talkingdata//
							
							string rewardEvent = "BattleGetReward";
							Dictionary<string,object> getBattleReward = new Dictionary<string, object>();
							getBattleReward.Add("BattleTape","Normal");		//关卡类型//
							getBattleReward.Add("MissionId",md.id.ToString());	//关卡id//
							getBattleReward.Add("PlayerId",pe.id.ToString());	//玩家id//
							int itemIndex = 0;	
							if(battle_Player_Info.battleReward.Count>0)
							{
								foreach(DropItemInfo dii in battle_Player_Info.battleReward)							//奖励//
								{
									//物品的类型 1:item  2:equip  3:card  4:skill  5:passiveSkill//
									switch(dii.itemType)
									{
									case 1:
										itemIndex++;
										getBattleReward.Add("item "+itemIndex,dii.itemId);
										break;
									case 2:
										itemIndex++;
									 	getBattleReward.Add("equip "+itemIndex,dii.itemId);
										break;
									case 3:
										itemIndex++;	
										getBattleReward.Add("card "+itemIndex,dii.itemId);
										break;
									case 4:
										itemIndex++;
										getBattleReward.Add("skill "+itemIndex,dii.itemId);
										break;
									case 5:
										itemIndex++;
										getBattleReward.Add("passiveSkill "+itemIndex,dii.itemId);
										break;
									}
								}
							}
							itemIndex = 0;
							TalkingDataManager.SendTalkingDataEvent(rewardEvent,getBattleReward);						//发送奖励物品talkingdata//
						}
						else if(Battle_Player_Info.instance.BattleResult == 2)
						{
							//失败//
							if(!TalkingDataManager.isTDPC)
							{
								TDGAMission.OnBegin(md.name);
								TDGAMission.OnFailed(md.name,"Die");
							}
							Dictionary<string,object> dic = new Dictionary<string, object>();
							dic.Add("BattleTape","Normal");//战斗类型//
							dic.Add("Result","Lose");//结果输//
							dic.Add("MissionId",md.id.ToString());//关卡id//
							dic.Add("PlayerBattlePower",pe.battlePower.ToString());//战斗力//
							dic.Add("UsePower",power.ToString());//体力消耗//
							TalkingDataManager.SendTalkingDataEvent(eventName,dic);
						}
							break;
					case STATE.BATTLE_TYPE_MAZE :
					//迷宫失败  判断是否为boss战//
						eventName = "MazeBattle";
						MazeBattleResultJson mbrj=PlayerInfo.getInstance().mbrj;
						MazeData mdd = MazeData.getData(mbrj.td);
						string mazeType = mbrj.type ==2 ? "Boss":"Normal";
						Dictionary<string,object> mdic = new Dictionary<string, object>();
						mdic.Add("BattleTape","Maze");//战斗类型//
						mdic.Add("Result","Lose");//结果输//
						mdic.Add("MazeType",mazeType);//迷宫类型//
						mdic.Add("PlayerBattlePower",pe.battlePower.ToString());//战斗力//
						mdic.Add("UsePower",mdd.energy.ToString());			//体力消耗//                                                                     
						mdic.Add("PlayerId",pe.id.ToString());				//玩家id//
						TalkingDataManager.SendTalkingDataEvent(eventName,mdic);
					break;
					case STATE.BATTLE_TYPE_PVP :
					//竞技场失败//
						eventName = "PvpBattle";
						Dictionary<string,object> Pvpdic = new Dictionary<string, object>();
						Pvpdic.Add("BattleTape","PVP");//战斗类型//
						Pvpdic.Add("Result","Lose");//结果输//
						Pvpdic.Add("PlayerId",pe.id.ToString());			
								//玩家id//		
	
						TalkingDataManager.SendTalkingDataEvent(eventName,Pvpdic);
					break;
					case STATE.BATTLE_TYPE_EVENT :
					//异世界失败//
						eventName = "EventBattle";
						EventBattleResultJson ebrj = PlayerInfo.getInstance().ebrj;
						EventData ed = EventData.getEventData(ebrj.eid);
						FBeventData ebd = FBeventData.getData(ebrj.id);
						Dictionary<string,object> Eventdic = new Dictionary<string, object>();
						Eventdic.Add("BattleTape","EVENT");				//战斗s类型//
						Eventdic.Add("Result","Lose");					//结果输//
						Eventdic.Add("EventBattleType",ed.name);		//异世界种类//
						Eventdic.Add("PlayerBattlePower",pe.battlePower.ToString());	//战斗力//
						Eventdic.Add("UsePower",ebd.cost.ToString());			//体力消耗//
						Eventdic.Add("PlayerId",pe.id.ToString());				//玩家id//
						TalkingDataManager.SendTalkingDataEvent(eventName,Eventdic);
					break;
				}
				
				GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
				break;
			case 2:
				if(errorCode!=0)
				{
					ToastWindow.mInstance.showText(TextsData.getData(103).chinese,this);
				}
				else
				{
					ToastWindow.mInstance.showText(TextsData.getData(167).chinese,this);
				}
				break;
			}
		}
		
		//人物经验条修改//
		if(curState == 1)
		{
			if(isPlayBounesNumAnim){
				bounesSprFrameCount+=Time.deltaTime;
				if(bounesSprFrameCount > bounesSprStayTime){
					StartBounesNumAnim();
					isPlayBounesNumAnim = false;
					bounesSprFrameCount = 0;
				}
			}
			//如果有bounes奖励，则bounes动画播放完开始播放箱子动画//
			if(addGoldLabelCount && !cn.GetAnimIsRunning()){
				FrameCount += Time.deltaTime;
				if(FrameCount > 0.5f){
					addGoldLabelCount = false;
//					StartGiftBoxAnim();
					ChangeState(2);
				}
			}
		}
	}
	
	//starNum 星星的个数, result 结果0 失败，1 胜利, isShowKO 是否显示ko，只有在出现bonus时，并且点满进度条时才会出现//
	
	public void setData(int starNum, int result, bool isShowKO = false, bool isLoseShowQHBtn = true,bool isEventBattle = false){
		show();
		
		//this.result = result;
		this.showStarNum = starNum;
		this.isShowKO = isShowKO;
        this.isEventBattle = isEventBattle;
//		KO_Obj.SetActive(false);

		
		//修改时间流速//
		UIInterfaceManager.mInstance.doSpeedChange(STATE.SPEED_NORMAL);
		
		//播放声音//
		int type = STATE.MUSIC_TYPE_WIN;
		if(result == STATE.BATTLE_RESULT_LOSE){
			type = STATE.MUSIC_TYPE_LOSE;
		}
		string musicName = MusicData.getDataByType(type).music;
		MusicManager.playBgMusic(musicName);

		//失败//
		if(result == STATE.BATTLE_RESULT_LOSE)
		{
			if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
			{
				//迷宫失败//
				Lose.SetActive(false);
				MazeLose.SetActive(true);
	            MazeDefeatedLabels[0].text = TextsData.getData(482).chinese;
				battleWin.SetActive(false);
				PlayerLevelUpObj.gameObject.SetActive(false);
				isCanChangeScene=false;
				Lose.transform.FindChild("Btn_Intensify").gameObject.SetActive(isLoseShowQHBtn);
				NewMazeUIManager.goldNum = 0;
				NewMazeUIManager.hornNum = 0;
			}
			else
			{
                if (PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
                {
                    Lose.SetActive(false);
                    MazeLose.SetActive(false);
                    battleWin.SetActive(false);
                    PlayerLevelUpObj.gameObject.SetActive(false);
                    isCanChangeScene = false;
                    pvPEndPanel.SetActive(true);
                    PvPEndSetData();
                }
                else
                {
                    Lose.SetActive(true);
                    MazeLose.SetActive(false);
                    DefeatedLabels[0].text = TextsData.getData(482).chinese;
                    DefeatedLabels[1].text = TextsData.getData(490).chinese;
                    DefeatedLabels[2].text = TextsData.getData(488).chinese;
                    //for (int i = 1; i < DefeatedLabels.Length; i++)
                    //{
                    //    int unlockMark = PlayerInfo.getInstance().getUnLockData(DefeatedLabels[i].transform.parent.GetComponent<UIButtonMessage>().param);
                    //    if (unlockMark == 1)
                    //    {
                    //        DefeatedLabels[i].transform.parent.FindChild(DefeatedLabels[i].transform.parent.name).gameObject.SetActive(false);
                    //        DefeatedLabels[i].transform.parent.GetComponent<UISprite>().enabled = true;
                    //    }
                    //    else
                    //    {
                    //        DefeatedLabels[i].transform.parent.FindChild(DefeatedLabels[i].transform.parent.name).gameObject.SetActive(true);
                    //        DefeatedLabels[i].transform.parent.FindChild("Label").GetComponent<UILabel>().text = "[808080]" + DefeatedLabels[i].transform.parent.FindChild("Label").GetComponent<UILabel>().text;
                    //        DefeatedLabels[i].transform.parent.GetComponent<UISprite>().enabled = false;
                    //    }
                    //}
                    battleWin.SetActive(false);
                    PlayerLevelUpObj.gameObject.SetActive(false);
                    isCanChangeScene = false;
                    Lose.transform.FindChild("Btn_Intensify").gameObject.SetActive(isLoseShowQHBtn);
                }
			}
		}
		else {

            if (PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
            {
                Lose.SetActive(false);
                MazeLose.SetActive(false);
                battleWin.SetActive(false);
                PlayerLevelUpObj.gameObject.SetActive(false);
                isCanChangeScene = false;
                pvPEndPanel.SetActive(true);
                PvPEndSetData();
            }
            else
            {

                Lose.SetActive(false);
                MazeLose.SetActive(false);
                if (isEventBattle)
                {
                    battleWin.SetActive(false);
                    PlayerLevelUpObj.gameObject.SetActive(false);
                    isCanChangeScene = false;
                    pvPEndPanel.SetActive(true);
                    PvPEndSetData();
                }
                else
                {
                    battleWin.SetActive(true);
                    ContinueBtn.SetActive(false);
                    ChangeState(0);
                }
            }
		}
		TipLabel.SetActive(false);
		
		
	}
    public GameObject pvPEndPanel;

    public UISprite[] pvPEndEndSprites;

    GameObject victory = null;

    public UISprite victorySpriteRune;

    public UISprite loseSpriteRune;

    public UISprite diamondSprite;

    void PvPEndSetData()
    {
        //显示结束面板
        pvPEndPanel.SetActive(true);
        GameObject victory = pvPEndPanel.transform.FindChild("victory").gameObject;
        GameObject lose = pvPEndPanel.transform.FindChild("lose").gameObject;

        GameObject eventPanel = pvPEndPanel.transform.FindChild("eventPanel").gameObject;
        UILabel rankLabel = pvPEndPanel.transform.FindChild("victory/Rank").GetComponent<UILabel>();
        UILabel rankNLabel = pvPEndPanel.transform.FindChild("victory/RankN").GetComponent<UILabel>();
        UILabel victoryRuneLabel = pvPEndPanel.transform.FindChild("victory/Rune").GetComponent<UILabel>();
        UILabel loseRuneLabel = pvPEndPanel.transform.FindChild("lose/Rune").GetComponent<UILabel>();

        UILabel eventLabel = pvPEndPanel.transform.FindChild("eventPanel/diamond").GetComponent<UILabel>();

        UILabel eventHurtLabel = pvPEndPanel.transform.FindChild("eventPanel/LabelNum").GetComponent<UILabel>();

       //UILabel roundLabel = pvPEndPanel.transform.FindChild("eventPanel/round").GetComponent<UILabel>();
        //UILabel honorLabel = combatEndPanel.transform.FindChild("Honor").GetComponent<UILabel>();

        //活动副本-死亡洞窟结算界面
        if (isEventBattle)
        {
            victory.SetActive(false);
            lose.SetActive(false);
            eventPanel.SetActive(true);
            victory = Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/Victory", 1)) as GameObject;
            GameObjectUtil.gameObjectAttachToParent(victory, pvPEndPanel);
            victory.transform.localPosition = new Vector3(-10f, 110f, 0);
            eventLabel.text = Battle_Player_Info.GetInstance().diamond.ToString();





            eventHurtLabel.text = PlayerInfo.getInstance().hurt + "";
        }
        else
        {
            //PvP战斗胜利//
            if (Battle_Player_Info.GetInstance().BattleResult == 1)
            {
                // spriteRay.GetComponent<TweenScale>().PlayForward();
                //spriteisVictory.GetComponent<TweenColor>().PlayForward();
                victory.SetActive(true);
                lose.SetActive(false);
                eventPanel.SetActive(false);
                // spriteRay.gameObject.SetActive(true);

                victory = Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/Victory", 1)) as GameObject;
                GameObjectUtil.gameObjectAttachToParent(victory, pvPEndPanel);
                victory.transform.localPosition = new Vector3(-10f, 110f, 0);
                rankLabel.gameObject.SetActive(true);
                rankLabel.text = Battle_Player_Info.GetInstance().rank.ToString();
                rankNLabel.text = Battle_Player_Info.GetInstance().rank0 - Battle_Player_Info.GetInstance().rank + "";
                if (Battle_Player_Info.GetInstance().runeNum < Battle_Player_Info.GetInstance().sAward)
                {

                    victorySpriteRune.gameObject.SetActive(true);
                    victoryRuneLabel.gameObject.SetActive(true);
                    victoryRuneLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                    PvpHonorLabel.text = "X  " + Battle_Player_Info.GetInstance().honorNum.ToString();
                    //honorLabel.color = Color.yellow;
                }
                else
                {
                    victorySpriteRune.gameObject.SetActive(false);
                    victoryRuneLabel.gameObject.SetActive(false);
                }

            }
            else
            {
                victory.SetActive(false);
                lose.SetActive(true);
                eventPanel.SetActive(false);
                if (Battle_Player_Info.GetInstance().runeNum < Battle_Player_Info.GetInstance().sAward)
                {
                    loseSpriteRune.gameObject.SetActive(true);
                    loseRuneLabel.gameObject.SetActive(true);
                    loseRuneLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                    PvpLoseHonorLabel.text = "X  " + Battle_Player_Info.GetInstance().honorNum.ToString();
                    //honorLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                    //honorLabel.color = Color.gray;
                }
                else
                {
                    loseSpriteRune.gameObject.SetActive(false);
                    loseRuneLabel.gameObject.SetActive(false);
                }
            }
        }
    }
	//state 0, 显示胜利界面， 1 人物等级及经验条增长， 2 卡牌增加经验， 3 显示获得的物品, 4 人物升级界面//
	public void ChangeState(int state)
	{
		curState = state;
		switch(state)
		{
		case 0:
			ShowResult();
			break;
		case 1:
			ShowPlayerData();
			break;
		case 2:
			ShowBattleCardData();
			break;
		case 3:
			ShowRewards();
			break;
		case 4:
			ShowPlayerLevelUp();
			break;
		}
	}
	
	public void ShowResult()
	{
		//隐藏界面所有的物品//
		
//		TipLabel.SetActive(false);;
//		playerLv.gameObject.SetActive(false);
//		playerExp.SetActive(false);;
//		gold.gameObject.SetActive(false);
		RewardsBounes.SetActive(false);
		ShowPlayerDataObj.SetActive(false);
		PlayerLevelUpObj.gameObject.SetActive(false);
		for(int i = 0;i < battleCards.Length;i ++)
		{
			battleCards[i].SetActive(false);
		}
		for(int i = 0;i < rewardsCards.Length;i ++)
		{
			rewardsCards[i].SetActive(false);
		}

		
		//播放星星动画//
		if(StarList!= null && StarList.Count >= showStarNum){
			//先隐藏所有的星星//
			foreach(GameObject star in StarList){
				star.SetActive(false);
			}
			curStar = 0;
			//一个一个显示星星//
			CreateStar();
		}
		
	}
	
	//显示人物的信息//
	public void ShowPlayerData()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_EXP_CHANGE);
		
		ShowPlayerDataObj.SetActive(true);
		
		playerLv.text = battleCardInfo.playerLastLevel.ToString();
		if(gold != null && cn == null){
			cn = gold.gameObject.GetComponent<ChangeNumber>();
		}
		
		
		//修改获得的金币的数量//
		gold.text = battleCardInfo.getCoins + "";
		//初始化经验条//
		ExpManager PlayerExpMa = playerExp.GetComponent<ExpManager>();
		//为经验条设置数据//
		PlayerExpMa.setData(STATE.EXP_TYPE_RESULT_PLAYER, battleCardInfo.playerLastExp,battleCardInfo.playerLastLevel,battleCardInfo.playerCurExp, battleCardInfo.playerCurLevel, -1);
	}
	
	//人物等级的修改//
	public void ChangePlayerLevel(int level)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_LEVELUP);
		playerLv.text = level.ToString();
	}
	
	//显示bonus动画//
	public void ShowBounesAnim(){
		if(battleCardInfo.isUseBonus && battleCardInfo.bonusAddNum > 1 ){
			RewardsBounes.SetActive(true);
			if(BounesSprObj == null){
				BounesSprObj = RewardsBounes.transform.FindChild("BonusSprK").gameObject;
			}
			BounesSprObj.SetActive(true);
			if(BounesNum == null){
				BounesNum = RewardsBounes.transform.FindChild("BonusNum").GetComponent<UILabel>();
			}
			BounesNum.text = "x" + battleCardInfo.bonusAddNum;
			BounesNum.gameObject.SetActive(false);
			GameObjectUtil.PlayerMoveAndScaleAnim(RewardsBounes, 0.5f, iTween.EaseType.linear, _MyObj, "RewardsBoxCallBack");
		}
		else{
			ChangeState(2);
		}
	}
	
	public void RewardsBoxCallBack(){
		
		isPlayBounesNumAnim = true;
	}
	
	//bonus倍数移动到金币上//
	public void StartBounesNumAnim(){
		BounesSprObj.SetActive(false);
		BounesNum.gameObject.SetActive(true);
		//bounesNum 移动并淡入淡出//
		iTween.MoveTo(BounesNum.gameObject,iTween.Hash("position",gold.transform.position,"time",0.5f, "delay", 0.5f));
		GameObjectUtil.PlayTweenAlpha(BounesNum.gameObject, 1f, 0f, "BounesNumCallBack", 0.8f);
		
	}
	bool addGoldLabelCount = false;
	//金币开始计数//
	public void BounesNumCallBack(){
		if(gold != null && cn != null){
			addGoldLabelCount = true;

			float curCoins = battleCardInfo.getCoins * battleCardInfo.bonusAddNum;
			//Debug.Log("desCoins =============== "  + curCoins);
			//金币数字开始变化//
			cn.SetData(gold, battleCardInfo.getCoins, curCoins, 2f);
			cn.StartChange();
		}
	}
	
	//显示战斗卡牌的信息//
	public void ShowBattleCardData()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_EXP_CHANGE);
			
		ShowCard(0);
	}
	
	public void ShowCard(int i)
	{
		//创建卡牌//
        for (int r = 0; r < battleCardInfo.cardList.Count; r++)
        {
            if (state2ShowCardNum < battleCardInfo.cardList.Count)
            {
                CardInfo ca = (CardInfo)battleCardInfo.cardList[r];
                GameObject card = battleCards[r];
                card.SetActive(true);
                ResultCardManager rcm = card.GetComponent<ResultCardManager>();
                rcm.cardInfo.gameObject.SetActive(true);
                rcm.cardLevel.gameObject.SetActive(true);
                rcm.LevelUpEff.SetActive(false);
                rcm.CardExp.gameObject.SetActive(true);

                //CardData cd = CardData.getData(ca.formId);
                rcm.cardInfo.setSimpleCardInfo(ca.formId,GameHelper.E_CardType.E_Hero);
                rcm.cardLevel.text = "LV." + ca.lastLevel;
                rcm.CardExp.setData02(STATE.EXP_TYPE_RESULT_CARD, ca.lastExp, ca.lastLevel, ca.curExp, ca.level, ca.star, null, i, rcm);
                //			if(state2ShowCardNum == battleCardInfo.cardList.Count - 1)
                //			{
                //				ChangeState(3);
                //			}
                state2ShowCardNum++;
            }
            else
            {
                ChangeState(3);
                isCanChangeScene = true;
                ContinueBtn.SetActive(true);
            }
        }
		//此处，因为迷宫卡牌不会增加卡牌经验，所以没有第二次执行此方法，强制执行一次//
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
            ChangeState(3);
            isCanChangeScene = true;
            ContinueBtn.SetActive(true);
		}
		
	}
	
	//显示获得的物品//
	public void  ShowRewards()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_GETCARD);
		
		for(int i = 0;i< battleCardInfo.battleReward.Count; i++)
		{
			if(i < rewardsCards.Length)
			{
				GameObject card = rewardsCards[i];
				card.SetActive(true);
				UIButtonMessage_Press umb = card.GetComponent<UIButtonMessage_Press>();
				umb.target = _MyObj;
				umb.functionName = "OnClickRewards";
				umb.param = i;
				
				DropItemInfo drop = (DropItemInfo)battleCardInfo.battleReward[i];
				SimpleCardInfo2 cardInfo = card.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
				
				//UIAtlas atlas = headAtlas;
				
				GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;
				
				switch(drop.itemType)
				{
				case 1:			//item//
					//atlas = itemAtlas;
					//ItemsData id = ItemsData.getData(drop.itemId);
					cardType = GameHelper.E_CardType.E_Item;
					break;
				case 2:			//equip//
					//atlas = equipAtla;
					//EquipData ed = EquipData.getData(drop.itemId);
					cardType = GameHelper.E_CardType.E_Equip;
					break;
				case 3:			//card//
				case 6:			//固定card//
					//atlas = headAtlas;
					//CardData cd = CardData.getData(drop.itemId);
					cardType = GameHelper.E_CardType.E_Hero;
					break;
				case 4:			//skill//
					//atlas = skillAtlas;
					//SkillData sd = SkillData.getData(drop.itemId);
					cardType = GameHelper.E_CardType.E_Skill;
					break;
				case 5:			//passSkill//
					
					break;
				}
				
				//添加卡牌获得统计@zhangsai//
				if(drop.itemType == 3)
				{
					if(!UniteSkillInfo.cardUnlockTable.ContainsKey(drop.itemId))
					{
						UniteSkillInfo.cardUnlockTable.Add(drop.itemId,true);
						MissionUI.isGetNewCard = true;
					}
				}
				
				cardInfo.setSimpleCardInfo(drop.itemId,cardType);
			}
		}
		
		if(isShowKO && !koNum.gameObject.activeSelf)
		{
			//显示ko积分//
			BattleResultJson brj=PlayerInfo.getInstance().brj;
			MissionData md = MissionData.getData(brj.md);
			if(md!=null)
			{
				koNum.gameObject.SetActive(true);
				koNum.text = TextsData.getData(555).chinese+TextsData.getData(406).chinese+" "+md.kopoint.ToString();
				//创建特效//
				koEff = Instantiate(koPrefabs) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(koEff, koNum.gameObject);
				GameObjectUtil.setGameObjectLayer(koEff, STATE.LAYER_ID_NGUI);
				koEff.transform.localPosition = Vector3.zero;
				Invoke("RemoveKoEff", 1.5f);
			}
			else
			{
				koNum.gameObject.SetActive(false);
				koNum.text = string.Empty;
			}
		}
	}
	
	//显示人物升级界面//
	public void ShowPlayerLevelUp()
	{
		//关闭结果界面//
		Lose.SetActive(false);
		MazeLose.SetActive(false);
		battleWin.SetActive(false);
		PlayerLevelUpObj.gameObject.SetActive(true);
		PlayerLevelUpObj.lastLevel.text = battleCardInfo.playerLastLevel.ToString();
		PlayerLevelUpObj.curLevel.text = battleCardInfo.playerCurLevel.ToString();
		
		PlayerLevelUpObj.lastPower.text = battleCardInfo.playerLastPower.ToString();
		PlayerLevelUpObj.curPower.text = battleCardInfo.playerCurPower.ToString();
		
		int lastQH = battleCardInfo.playerLastLevel * 3;
		int curQH = battleCardInfo.playerCurLevel * 3;
		PlayerLevelUpObj.lastQH.text = lastQH.ToString();
		PlayerLevelUpObj.curQH.text = curQH.ToString();
		
		int lastCG = 3;
		int curCG = 3;
		for (int i = 0;i < 3;i ++)
		{
			UnlockData uld = UnlockData.getData(30 + i);
			if(battleCardInfo.playerLastLevel >= uld.method)
			{
				lastCG = 4 + i;
			}
			if(battleCardInfo.playerCurLevel >= uld.method)
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
		if(LevelUpBgEffect != null)
		{
			Destroy(LevelUpBgEffect);
		}
		if(LevelUpBgEffect == null)
		{
			LevelUpBgEffect=Instantiate(GameObjectUtil.LoadResourcesPrefabs("UIEffect/chouka_starbackground",1)) as GameObject;
		}
		GameObjectUtil.gameObjectAttachToParent(LevelUpBgEffect,PlayerLevelUpObj.gameObject);
		GameObjectUtil.setGameObjectLayer(LevelUpBgEffect,PlayerLevelUpObj.gameObject.layer);
		
		if(SDKManager.getInstance().isSDKCPYYUsing())
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				SDK_CPYY_manager.sdk_call_gameNotify(3+"",battleCardInfo.playerCurLevel.ToString(),"levelup");
			}
		}
	}
	
	//显示获得物品的详细信息//
	public void ShowRewardsDetails(int index)
	{
		DropItemInfo drop = (DropItemInfo)battleCardInfo.battleReward[index];
		int type = drop.itemType;
		int formID = drop.itemId;
		int level= drop.itemLevel;
		int star = 1;
		
		//string iconName = "";
		string name = "";
		string des = "";
		int sell = 0;
			
		//UIAtlas atlas = null;
		
		GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;
		
		switch(type)
		{
		case 1:				//items//
			ItemsData item = ItemsData.getData(formID);
			if(item == null)
				return;
			cardType = GameHelper.E_CardType.E_Item;
			star = item.star;
			//iconName = item.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(itemAtlasName);
			name = item.name;
			des = item.discription;
			sell = item.sell;
			break;
		case 2:				//equip//
			EquipData ed = EquipData.getData(formID);
			if(ed == null)
				return;
			cardType = GameHelper.E_CardType.E_Equip;
			star = ed.star;
			//iconName = ed.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(equpiAtlasName);
			name = ed.name;
			des = ed.description;
			sell = ed.sell;
			break;
		case 3:				//card//
			CardData cd = CardData .getData(formID);
			if(cd == null)
				return;
			cardType = GameHelper.E_CardType.E_Hero;
			star = cd.star;
			//iconName = cd.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(cardAtlasName);
			name = cd.name;
			des = cd.description;
			sell = cd.sell;
			break;
		case 4:				//skill//
			SkillData sd = SkillData.getData(formID);
			if(sd == null)
				return;
			cardType = GameHelper.E_CardType.E_Skill;
			star = sd.star;
			//iconName = sd.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(skillAtlasName);
			name = sd.name;
			if(sd.exptype == 2)
			{
				des = sd.description;
			}
			else
			{
				des = Statics.getSkillValueForUIShow(sd.index, 1);
			}
			
			sell = sd.sell;
			break;
		case 5:				//passiveSkill//
			PassiveSkillData psd = PassiveSkillData.getData(formID);
			if(psd == null)
				return;
			cardType = GameHelper.E_CardType.E_PassiveSkill;
			star = psd.star;
			//iconName = psd.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(passSkillAtlasName);
			name = psd.name;
			des = psd.describe;
			sell = psd.sell;
			break;
		}
		string frameName = "head_star_" + star;
		RewardsDatasControl.mInstance.SetData(formID,cardType,name,frameName, des, level, sell, type);
		Vector3 reV3 = rewardsCards[index].transform.position;
		RewardsDatasControl.mInstance.transform.position = new Vector3(reV3.x + 0.7f, reV3.y + 0.4f, 0f);
	}
	
	public void RemoveKoEff()
	{
		if(koEff != null)
		{
			Destroy(koEff);
		}
	}
	
	
	public override void hide ()
	{
		base.hide ();
		CleanData();
	}
	
	public void CleanData()
	{
		if(LevelUpBgEffect != null)
		{
			Destroy(LevelUpBgEffect);
		}
        if (victory != null)
        {
            Destroy(victory);
        }
	}
	

	bool isCanChangeScene = false;
	public void OnClickBtn(){
		//未升级的返回//
		//战斗结束后将场景切换到结算界面//
		if(isCanChangeScene){
			if(curState != 4 && (battleCardInfo!=null && battleCardInfo.playerLastLevel < battleCardInfo.playerCurLevel )&& PlayerInfo.getInstance().battleType != STATE.BATTLE_TYPE_PVP)
			{
				ChangeState(4);
				ContinueBtn.SetActive(false);
			}
			else 
			{
				if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
				{
					//竞技场胜利//
					PlayerElement pe = PlayerInfo.getInstance().player;
					string eventName = "PvpBattle";
					Dictionary<string,object> Pvpdic = new Dictionary<string, object>();
					Pvpdic.Add("BattleTape","PVP");						//战斗类型//
					Pvpdic.Add("Result","Win");							//结果赢//
					Pvpdic.Add("PlayerId",pe.id.ToString());			//玩家id//		
					TalkingDataManager.SendTalkingDataEvent(eventName,Pvpdic);
					
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
					
				}
				else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
				{
					
					ChangeScene();
					
				}
				else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
				{
					//迷宫成功//
					Battle_Player_Info battle_Player_Info = Battle_Player_Info.GetInstance();
					PlayerElement pe = PlayerInfo.getInstance().player;
					string eventName = "MazeBattle";
					MazeBattleResultJson mbrj=PlayerInfo.getInstance().mbrj;
					MazeData mdd = MazeData.getData(mbrj.td);
					string mazeType = mbrj.type ==2 ? "Boss":"Normal";
					Dictionary<string,object> mdic = new Dictionary<string, object>();
					mdic.Add("BattleTape","Maze");	//战斗类型//
					mdic.Add("Result","Win");		//结果赢//
					mdic.Add("MazeType",mazeType);	//迷宫类型//
					mdic.Add("PlayerBattlePower",pe.battlePower.ToString());//战斗力//
					mdic.Add("UsePower",mdd.energy.ToString());					//体力消耗//                                                         
					mdic.Add("PlayerId",pe.id.ToString());		//玩家id//
					mdic.Add("GetCoins",battle_Player_Info.getCoins.ToString());
					TalkingDataManager.SendTalkingDataEvent(eventName,mdic);
					
					eventName = "MazeBattleReward";
					Dictionary<string,object> mbrdic = new Dictionary<string, object>();
					int itemIndex = 0;
					if(battle_Player_Info.battleReward.Count>0)
					{
						foreach(DropItemInfo dii in battle_Player_Info.battleReward)							//奖励//
						{
							//物品的类型 1:item  2:equip  3:card  4:skill  5:passiveSkill//
							if(dii.itemType == 4)
							{
								if(itemIndex == 0){
								mbrdic.Add("RewardType","Skill");
								mbrdic.Add ("PlayerId",pe.id);
								}
								itemIndex++;
								mbrdic.Add ("SkillId"+itemIndex,dii.itemId);
								SkillData sd = SkillData.getData(dii.itemId);
								if(sd.exptype == 2)
								{
									itemIndex++;
									mbrdic.Add("ExpSkill"+itemIndex,sd.index.ToString());
								}
							}
						}
					}
					itemIndex = 0;
					TalkingDataManager.SendTalkingDataEvent(eventName,mbrdic);
					
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
				}
				else
				{
					//异世界成功//
					PlayerElement pe = PlayerInfo.getInstance().player;
					Battle_Player_Info battle_player_info = Battle_Player_Info.GetInstance();
					string eventName = "EventBattle";
					EventBattleResultJson ebrj = PlayerInfo.getInstance().ebrj;
					EventData ed = EventData.getEventData(ebrj.eid);
					FBeventData ebd = FBeventData.getData(ebrj.id);
					Dictionary<string,object> Eventdic = new Dictionary<string, object>();
					Eventdic.Add("BattleTape","EVENT");				//战斗类型//
					Eventdic.Add("Result","Win");					//结果赢//
					Eventdic.Add("EventBattleType",ed.name);		//异世界种类//
					Eventdic.Add("PlayerBattlePower",pe.battlePower.ToString());	//战斗力//
					Eventdic.Add("UsePower",ebd.cost.ToString());			//体力消耗//
					Eventdic.Add("PlayerId",pe.id.ToString());				//玩家id//
					Eventdic.Add("GetCoins",battle_player_info.getCoins.ToString());
					TalkingDataManager.SendTalkingDataEvent(eventName,Eventdic);
					
					eventName = "EventBalleReward";
					Dictionary<string,object> ebrdic = new Dictionary<string, object>();
					int itemIndex = 0;
					if(battle_player_info.battleReward.Count>0)
					{
						ebrdic.Add("EventBalleName",ed.name);
						foreach(DropItemInfo dii in battle_player_info.battleReward)
						{
							//物品的类型 1:item  2:equip  3:card  4:skill  5:passiveSkill//
							switch (dii.itemType)
							{
							case 4:
								itemIndex++;
								ebrdic.Add("SkillId"+itemIndex,dii.itemId);
								break;
							case 3:
								itemIndex++;
								ebrdic.Add("CardId "+itemIndex,dii.itemId);
								break;
							case 2:
								itemIndex++;
								ebrdic.Add("EquipId"+itemIndex,dii.itemId);
								break;
							case 1:
								itemIndex++;
								ebrdic.Add("ItemId"+itemIndex,dii.itemId);
								break;
							}
						}
					}
					itemIndex=0;
					TalkingDataManager.SendTalkingDataEvent(eventName,ebrdic);//异世界奖励//
					
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
				}
			}
		}
	}
	//升级时的返回//
	public void OnClickLVBG()
	{
		if(curState == 4 && isCanChangeScene)
		{
			isCanChangeScene = false;
			if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
			{
				GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
			}
			else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
			{
				
				ChangeScene();
				
			}
			else
			{
				GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
			}
		}
	}
	
	//失败的按键响应，id 0使返回//

	public void SkipClick(int id)
	{
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_WRAPSPACE;
		}
		if(id == 0)
		{
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
		}
		else if(id == 3 || id==16)
		{
            if (id == 3)
            {
                PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_QH;
                GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);//失败去强化//
            }
            else if (id == 16)
            {
                PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_ZH;
                GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);//失败去召唤界面//
            }
			
			
			PlayerElement pe = PlayerInfo.getInstance().player;
			
			switch(PlayerInfo.getInstance().battleType)
			{
				case STATE.BATTLE_TYPE_NORMAL :
					//推图失败//
						//Battle_Player_Info battle_Player_Info = Battle_Player_Info.GetInstance();
						BattleResultJson brj=PlayerInfo.getInstance().brj;
						MissionData md = MissionData.getData(brj.md);
						int power = md.cost;
						string eventName = "NormalBattle";
				
						Dictionary<string,object> dic = new Dictionary<string, object>();
						dic.Add("BattleTape","Normal");//战斗类型//
						dic.Add("Result","Lose");//结果输//
						dic.Add("MissionId",md.id.ToString());//关卡id//
						dic.Add("PlayerBattlePower",pe.battlePower.ToString());//战斗力//
						dic.Add("UsePower",power.ToString());//体力消耗//
						TalkingDataManager.SendTalkingDataEvent(eventName,dic);
					break;
				case STATE.BATTLE_TYPE_MAZE :
					//迷宫失败  判断是否为boss战//
						eventName = "MazeBattle";
						MazeBattleResultJson mbrj=PlayerInfo.getInstance().mbrj;
						MazeData mdd = MazeData.getData(mbrj.td);
						string mazeType = mbrj.type ==2 ? "Boss":"Normal";
						Dictionary<string,object> mdic = new Dictionary<string, object>();
						mdic.Add("BattleTape","Maze");//战斗类型//
						mdic.Add("Result","Lose");//结果输//
						mdic.Add("MazeType",mazeType);//迷宫类型//
						mdic.Add("PlayerBattlePower",pe.battlePower.ToString());//战斗力//
						mdic.Add("UsePower",mdd.energy.ToString());			//体力消耗//                                                                     
						mdic.Add("PlayerId",pe.id.ToString());				//玩家id//
						TalkingDataManager.SendTalkingDataEvent(eventName,mdic);
					break;
				case STATE.BATTLE_TYPE_PVP :
					//竞技场失败//
						eventName = "PvpBattle";
						Dictionary<string,object> Pvpdic = new Dictionary<string, object>();
						Pvpdic.Add("BattleTape","PVP");//战斗类型//
						Pvpdic.Add("Result","Lose");//结果输//
						Pvpdic.Add("PlayerId",pe.id.ToString());			//玩家id//
						TalkingDataManager.SendTalkingDataEvent(eventName,Pvpdic);
					break;
				case STATE.BATTLE_TYPE_EVENT :
					//异世界失败//
						eventName = "EventBattle";
						EventBattleResultJson ebrj = PlayerInfo.getInstance().ebrj;
						EventData ed = EventData.getEventData(ebrj.id);
						FBeventData ebd = FBeventData.getData(ebrj.id);
						Dictionary<string,object> Eventdic = new Dictionary<string, object>();
						Eventdic.Add("BattleTape","EVENT");				//战斗类型//
						Eventdic.Add("Result","Lose");					//结果输//
						Eventdic.Add("EventBattleType",ed.name);		//异世界种类//
						Eventdic.Add("PlayerBattlePower",pe.battlePower.ToString());	//战斗力//
						Eventdic.Add("UsePower",ebd.cost.ToString());			//体力消耗//
						Eventdic.Add("PlayerId",pe.id.ToString());				//玩家id//
						TalkingDataManager.SendTalkingDataEvent(eventName,Eventdic);
					break;
			}
		}

        //switch (id)
        //{
        //    case 0:
        //        requestType = 1;
        //        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP), this);
        //        break;
        //    case STATE.UI_MODEL_CARDGROUP: //阵容//
        //        break;
        //    case STATE.UI_MODLE_LOTCARD: //召唤//
        //        break;
        //    case STATE.UI_MODLE_BROKE: //突破//
        //        break;
        //    case STATE.UI_MODEL_CARDGROUP:
        //        break;
        //    case STATE.UI_MODEL_CARDGROUP:
        //        break;
        //}
	}
	
	//点击获得物品,显示物品详细信息//
	public void OnClickRewards(int index)
	{
		if(RewardsDatasControl.mInstance.gameObject.activeSelf)
		{
			RewardsDatasControl.mInstance.hide();
		}
		else 
		{
			ShowRewardsDetails(index);
		}
	}
	
	public void CreateStar(){
		float scaleT = 0.2f;
		GameObject curStarObj = StarList[curStar];
		curStarObj.SetActive(true);
//		GameObjectUtil.PlayerMoveAndScaleAnim(star1, 0.2f,iTween.EaseType.linear, _MyObj, "MoveCallBack", true, 0);
		TweenScale scale = curStarObj.GetComponent<TweenScale>();
		scale.enabled = true;
		scale.duration = scaleT;
		Invoke("MoveCallBack", scaleT );

		curStar++;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_STAR);
			
	}
	
	
	
	
	//星星移动的回调函数//
	public void MoveCallBack(){
		
		if(curStar < showStarNum ){
			
			CreateStar();
		}
//		else if(isShowKO)
//		{
//			KO_Obj.SetActive(true);
//			GameObjectUtil.PlayerMoveAndScaleAnim(KO_Obj, 0.2f,iTween.EaseType.linear, _MyObj, "MoveCallBack", true, 0);
//			isShowKO = false;
//		}
		else if(state0Step == showStarNum - 1)
		{
//			isCanChangeScene = true;
			//只有竞技场界面是不想要跳转到结算界面的//
			if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
			{
				isCanChangeScene = true;
				ContinueBtn.SetActive(true);
			}
			else 
			{
				//整个战斗胜利界面开始缩小，并且移动//
				iTween.MoveTo(Win,iTween.Hash("position",winResultPos.transform.position,"time",0.5f, "delay", 0.5f, "oncomplete", "MoveCallBack", 
					"oncompletetarget", _MyObj));
				iTween.ScaleTo(Win, iTween.Hash("scale",winResultPos.transform.localScale,"time",0.5f, "delay", 0.5f));
			}
		}
		else if(state0Step == showStarNum )
		{
			ChangeState(1);
		}
		state0Step++;
	}
	
	//胜利界面移动的回调函数//
	public void ResultMoveCallBack()
	{
		ChangeState(1);
	}
	
	public void ChangeScene(){
		//==添加支援玩家为好友==//
		int helperId=Battle_Player_Info.GetInstance().helperPlayerId;
		string helperName=Battle_Player_Info.GetInstance().helperPlayerName;
		if(helperId!=0)
		{
			ToastWarnUI.mInstance.showWarn(TextsData.getData(166).chinese.Replace("name",helperName),this);
		}
		else
		{
			ChangeSceneTrue();
		}
	}
	
	public void warnningSure()
	{
		requestType=2;
		PlayerInfo.getInstance().sendRequest(new FriendApplyJson(Battle_Player_Info.GetInstance().helperPlayerId,2),this);
	}
	
	public void warnningCancel()
	{
		ChangeSceneTrue();
	}
	
	//==真正切换场景==//
	public void ChangeSceneTrue()
	{
		requestType=1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
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
				MapResultJson mj=JsonMapper.ToObject<MapResultJson>(json);
				errorCode = mj.errorCode;
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().mrj=mj;
				}
				receiveData=true;
				break;
			case 2:
				FriendResultJson frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode=frj.errorCode;
				receiveData=true;
				break;
			}
		}
	}
	
	public void gc()
	{
		battleCardInfo=null;
		LevelUpBgEffect=null;
		GameObject.Destroy(_MyObj);
		mInstance = null;
		_MyObj = null;
		//Resources.UnloadUnusedAssets();
	}
}