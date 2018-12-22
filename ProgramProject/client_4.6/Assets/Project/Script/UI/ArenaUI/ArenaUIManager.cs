using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * 竞技场
 */
public class ArenaUIManager : MonoBehaviour, ProcessResponse {
	
//	public static ArenaUIManager mInstance;
	
	//标签页按钮 0 排位赛， 1 天位赛， 2夺宝奇兵//
	public GameObject[] markBtnObjs;
	//挑战的对手的list//
	public GameObject[] enemyList;
	
	//排行榜的弹出框//
	public GameObject RankDragPanel;
	public GameObject ScrollView;
	public GameObject GridList;
	public GameObject ScrollBar;
	
	
	//当前排名//
	public UILabel curRankLabel;
    //当前战斗力//
    public UILabel fightingLabel;
	//排名获取的奖励//
	public UILabel rankGiftLabel;
	//pvp奖励//
	public UILabel pvpLabel;
	//挑战次数//
	public UILabel dekaronNumLabel;
    //pk剩余时间//
    public UILabel pkTimeLabel;

	
	//领取按钮//
	public GameObject receiveBtn;
	
	//玩家头像//
	public UISprite playerHeadIcon;
	
	public UIAtlas headAtlas;
	//当前选择战斗的敌人的id//
	public int curSelEmenyId;
	
	//普通字体//
	public UIFont FontNormal;
	//美术数字字体//
	public UIFont FontNum;
	
	
	//private Transform _myTransform;
	//1 领取奖励， 2 选择对手进行战斗， 3 请求界面信息, 4 请求卡组界面信息, 5 请求购买挑战次数(或冷却时间)界面信息, 6 请求排行榜数据//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	//当前的页面 0， 排位赛， 1 天位赛， 2 夺宝奇兵//
	//int curPage;
	//当前排名奖励领取的次数,领取过一次就不在领取//
	int curReceiveNum;
	//当前界面玩家的信息//
	string arenaData;
	//pk对手的list 格式：playerId-name-headName-rank(排名)//
	List<string> pkList = new List<string>();
	
	//领取按钮的两个状态//
	GameObject _light;
	GameObject _black;
	//剩余挑战次数//
	int dekaronNum;
	//排名奖励//
	int rankGift;
	//总的挑战次数//
	//int totalDekaronNum;
	//总的符文奖励//
	int totalPVPReward;
	//pk剩余时间--秒//
	int pkTime;
	
	/**购买界面信息 start **/
	//购买物品的类型 1 金币， 2 体力, 3 购买挑战次数， 4 购买冷却时间//
	//private int buyType;
	//请求类型,1 请求购买界面信息， 2 购买//
	private int jsonType;
	//花费类型 ，1 水晶， 2 金币//
	//private int costType;
	//购买花费的水晶数//
	private int costCrystal;
	//要购买的金币或体力的个数//
	private int num;
	//当天剩余的购买次数//
	private int times;
	/**购买界面信息 end **/
	private float timeCount;
	//战斗后掉落的物品//
	List<string> dropList = new List<string>();
	//战斗历史的记录//
	List<PkRecordElement> pkHistoryList = new List<PkRecordElement>();
	//pk历史记录父节点//
	public GameObject pkHistoryParent;
	//pk历史界面//
	public GameObject pkHistoryPanel;
	GameObject loadPkHistoryCell;
	string TopItemPath = "Prefabs/UI/UI-Arena/TopGridItem";
	GameObject TopItem;
	List<PkRankElement> topList = new List<PkRankElement>();
	
	public bool canClick = true;
	
	//卡组界面的json//
	CardGroupResultJson cardGroupRJ;

    public GameObject combatEndPanel;

    public int rank;

    private int types;

    public UISprite reminder;

    private List<string> cardIds = new List<string>();

	//商店json//
    private ShopResultJson shopRJ;

	
    //int rankN;
	void Awake(){
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		//_myTransform = transform ;
		init();
	}
	
	public void init ()
	{
//		base.init ();
		if(receiveBtn)
		{
			_light = receiveBtn.transform.FindChild("Light").gameObject;
			_black = receiveBtn.transform.FindChild("Black").gameObject;
		}
		TopItem =  Resources.Load(TopItemPath) as GameObject;
	}
	
	// Use this for initialization
	void Start () {
		if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_PVP ){
			PlayerInfo.getInstance().BattleOverBackType = 0;
//			MainMenuManager.mInstance.hide();
			//隐藏主城//
			if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
			{
				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			}
			
//			WarpSpaceUIManager.mInstance.hide();
			RankDragPanel.SetActive(false);
			
			//请求界面信息//	
			SendToGetData();
				
			//隐藏pk列表//
            //for(int i = 0; i< enemyList.Length;i++)
            //{
            //    enemyList[i].SetActive(false);
            //}
			//播放声音//
			string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MENU).music;
			MusicManager.playBgMusic(musicName);
		}
		else {
//			hide();
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData){
			receiveData = false;
			if(errorCode == -3)
				return;
			
			switch(requestType){
			case 1:			//领取奖励//
				//如果领取次数>0,则不能再次领取,把按钮的状态变为灰色//
				if(curReceiveNum > 0){
					
					_light.SetActive(false);
					_black.SetActive(true);
				}
				else {
					_light.SetActive(true);
					_black.SetActive(false);
				}
				if(errorCode == 0)
				{
					
					//显示提示--领取成功//
					string str = TextsData.getData(133).chinese;
					str += rankGift;
					ToastWindow.mInstance.showText(str);
				}
				break;
			case 2:				//选择对手进行pk//
				if(errorCode == 0)
				{
					PlayerInfo.getInstance().battleType = STATE.BATTLE_TYPE_PVP;
					PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_PVP;
					PlayerInfo.getInstance().pvpDropList.Clear();
					for(int i = 0;i < dropList.Count; i ++)
					{
						PlayerInfo.getInstance().pvpDropList.Add(dropList[i])  ;
					}
					
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
					canClick = false;
				}
				else if(errorCode == 86)		//冷却时间未到，无法进入//
				{
					int buyType = 5;
					int jsonType = 1;
					int costType = 1;
					int cdType = 1;
					if(ShowBuyTipControl.mInstance  != null)
					{
						ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_ARENA, cdType);
					}
					
				}
				else if(errorCode == 50)		//挑战次数用尽，请求购买//
				{
					int buyType = 6;
					int jsonType = 1;
					int costType = 1;
					if(ShowBuyTipControl.mInstance  != null)
					{
						ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_ARENA, 0);
					}
				}
				break;
			case 3:
                if (pkList.Count == 0)
                {
                    requestType = 3;
                    PlayerInfo.getInstance().sendRequest(new RankJson(0), this);
                }
//				show();
				ChangeData();
				//显示战斗成功数据//
				//ShowBattleResult();
               
                DrawRankData();
                RankDragPanel.SetActive(false);
				break;
			case 4:		//进入卡组界面//
			{
				types = 1;
				
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
				CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
				
				combination.curCardGroup=cardGroupRJ.transformCardGroup();

                combination.SetData(4, cardIds, pkList);
				
//				//关闭主菜单选项卡//
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				hide();
			}break;
			case 5:					//请求购买挑战次数//
				if(errorCode == 0)
				{
					BuyTipManager.mInstance.SetData(0, 0 , costCrystal, num, times);
				}
				break;
			case 6:					//获取排行榜信息//
				if(errorCode == 0)
				{
					//绘制排行榜信息//
					DrawRankData();
				}
				break;
			case 7:					//查看战斗记录信息//
				if(errorCode == 0)
				{
					//绘制信息//
					DrawPkHistoryData();
					pkHistoryPanel.SetActive(true);
				}
				break;
            case 8:					//换一批对手//
                if (errorCode == 0)
                {
                    DrawPKListData();
                }
                break;
            case 9:
			{
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
				CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
				combination.curCardGroup=cardGroupRJ .transformCardGroup();
                combination.SetData(2, cardIds, pkList);
				types = 1;
				cardGroupRJ = null;
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
                hide();
			}break;
			case 10:
				if (errorCode == 0)
                {
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP);
                    ShopUI shop = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SHOP, "ShopUI") as ShopUI;
                    shop.ShopResJson = shopRJ;
                    shopRJ = null;

                    shop.show(4);
                }
				break;
			}
		}
		//修改显示时间//
		if(pkTime > 0 && pkTimeLabel != null && pkTimeLabel.gameObject.activeSelf)
		{
			timeCount += Time.deltaTime;
			if(timeCount > 1)				//每过一秒钟修改一次//
			{
				pkTime --;
				timeCount = 0;
				
				int min = pkTime / 60;		//分钟//
				int sec = pkTime % 60;		//秒//
				
				if(sec > 0)
				{
					min +=1;
				}
				pkTimeLabel.text = TextsData.getData(310).chinese + min + TextsData.getData(335).chinese;
			}
		}
		else if(pkTime <= 0 && pkTimeLabel.gameObject.activeSelf)
		{
			timeCount = 0;
			pkTimeLabel.gameObject.SetActive(false);
		}
	}
    public UISprite[] combatEndSprites;
	//战斗结束显示提示框//
	public void ShowBattleResult(){
        //显示结束面板
        combatEndPanel.SetActive(true);
        GameObject list = combatEndPanel.transform.FindChild("lift").gameObject;
        UISprite spriteBG = combatEndPanel.transform.FindChild("BG").GetComponent<UISprite>();
        UISprite spriteisVictory = combatEndPanel.transform.FindChild("BG/isVictory").GetComponent<UISprite>();
        UISprite spriteRay = combatEndPanel.transform.FindChild("BG/Sprite").GetComponent<UISprite>();
        UISprite spriteRune = combatEndPanel.transform.FindChild("RuneSprite").GetComponent<UISprite>();
        UISprite spriteLight1 = combatEndPanel.transform.FindChild("Light1").GetComponent<UISprite>();
        UISprite spriteLight2 = combatEndPanel.transform.FindChild("Light2").GetComponent<UISprite>();
        


        UILabel rankLabel =  combatEndPanel.transform.FindChild("Rank").GetComponent<UILabel>();
        UILabel rankNLabel = combatEndPanel.transform.FindChild("RankN").GetComponent<UILabel>();
        UILabel runeLabel = combatEndPanel.transform.FindChild("Rune").GetComponent<UILabel>();
        //UILabel honorLabel = combatEndPanel.transform.FindChild("Honor").GetComponent<UILabel>();

		//战斗胜利//
		if(Battle_Player_Info.GetInstance().BattleResult == 1){
            for (int i = 0; i < combatEndSprites.Length; i++)
            {
                combatEndSprites[i].color = Color.white;
            }
			string s0 = TextsData.getData(134).chinese; //你战胜了//
			string s1 = TextsData.getData(135).chinese;	//名次提升为//
			string s2 = TextsData.getData(136).chinese;	//获得符文提示//
			string str = "";
            spriteisVictory.spriteName = "victory";
            spriteRay.gameObject.SetActive(true);
            list.SetActive(true);
            rankLabel.gameObject.SetActive(true);
            spriteLight1.gameObject.SetActive(true);
            spriteLight2.gameObject.SetActive(true);
            rankLabel.text = Battle_Player_Info.GetInstance().rank.ToString();
            rankNLabel.text = Battle_Player_Info.GetInstance().rank0 - Battle_Player_Info.GetInstance().rank + "";
			if(Battle_Player_Info.GetInstance().runeNum < totalPVPReward){

                spriteRune.gameObject.SetActive(true);
                runeLabel.gameObject.SetActive(true);
                runeLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                runeLabel.color = Color.yellow;
                //honorLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                //honorLabel.color = Color.yellow;
				str = s0 + Battle_Player_Info.GetInstance().pkEnemyName + "\r\n" 
					+ s1 + Battle_Player_Info.GetInstance().rank + "\r\n" 
					+ s2 + Battle_Player_Info.GetInstance().runeAward;
			}
			else {
				str = s0 + Battle_Player_Info.GetInstance().pkEnemyName + "\r\n" 
					+ s1 + Battle_Player_Info.GetInstance().rank ;
                spriteRune.gameObject.SetActive(false);
                runeLabel.gameObject.SetActive(false);
			}
			//判断是否有掉落物品，如果有的话，则显示提示框//
			str += "\r\n" + ShowDrops();
			//ToastWindow.mInstance.showText(str);
            
          
		}
		else {
            for (int i = 0; i < combatEndSprites.Length; i++)
            {
                combatEndSprites[i].color = Color.gray;
            }
            spriteBG.color = Color.gray;
            spriteisVictory.spriteName = "pvp-028";
            spriteRay.gameObject.SetActive(false);
            list.SetActive(false);
            rankLabel.gameObject.SetActive(false);
            rankNLabel.gameObject.SetActive(false);
            spriteLight1.gameObject.SetActive(false);
            spriteLight2.gameObject.SetActive(false);
            rankLabel.text = Battle_Player_Info.GetInstance().rank.ToString();
            if (Battle_Player_Info.GetInstance().runeNum < totalPVPReward)
            {
                spriteRune.gameObject.SetActive(true);
                runeLabel.gameObject.SetActive(true);
                string str = TextsData.getData(136).chinese
                    + Battle_Player_Info.GetInstance().runeAward;	//获得符文提示//
                runeLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                runeLabel.color = Color.gray;
                //honorLabel.text = "X  " + Battle_Player_Info.GetInstance().runeAward.ToString();
                //honorLabel.color = Color.gray;
                //判断是否有掉落物品，如果有的话，则显示提示框//
                str += "\r\n" + ShowDrops();
                //ToastWindow.mInstance.showText(str);
            }
            else
            {
                spriteRune.gameObject.SetActive(false);
                runeLabel.gameObject.SetActive(false);
            }
		}
        DrawRankData();
        RankDragPanel.SetActive(false);
	}

    public void ChangeData()
    {
		//修改玩家基础信息， 头像等//
		DrawBasicData();
		//绘制玩家在排位赛的界面信息//
        DrawPvpPlayerData();
		//绘制pk对手的信息//
        DrawPKListData();

		
	}

    public void BGOnClick(int param)
    {
        switch (param)
        {
            case 1:
                combatEndPanel.SetActive(false);
                DrawPvpPlayerData();
                break;
            case 2:
                //背景对话框//
                reminder.transform.FindChild("Label").GetComponent<UILabel>().text = TextsData.getData(679).chinese;
                reminder.gameObject.SetActive(true);
                TweenScale ts = reminder.gameObject.GetComponent<TweenScale>();
                ts.PlayForward();
                break;
        }
    }
	public void DrawBasicData(){
		//修改头像信息//
//		if(playerHeadIcon != null && headAtlas != null){
//			playerHeadIcon.atlas = headAtlas;
//			playerHeadIcon.spriteName = PlayerInfo.getInstance().player.head;
//		}
	}
  
	public void DrawPvpPlayerData(){
		string[] str = arenaData.Split('-');
		//int ranking = StringUtil.getInt(str[0]);	//当前排名//
        //rankN = ranking;
		curReceiveNum = StringUtil.getInt(str[1]);	//领取次数//
		//int pvpGift = StringUtil.getInt(str[2]);	//pvp奖励//
		//剩余挑战次数//
		dekaronNum = StringUtil.getInt(str[3]);	//挑战次数//
		//排名奖励//
		rankGift = StringUtil.getInt(str[4]);
		curRankLabel.text = str[0];
        rank = StringUtil.getInt(str[0]);
		rankGiftLabel.text = str[4];
		string ss0 = TextsData.getData(122).chinese;
		pvpLabel.text = ss0 + str[2] + "/" + totalPVPReward;
		ss0 = TextsData.getData(123).chinese;
		dekaronNumLabel.text = ss0 + str[3] ;
        fightingLabel.text = TextsData.getData(676).chinese + PlayerInfo.getInstance().player.battlePower.ToString();
		//修改挑战时间//
		if(pkTime > 0)
		{
			pkTimeLabel.gameObject.SetActive(true);
			int min = pkTime / 60;		//分钟//
			int sec = pkTime % 60;		//秒//
			if(sec > 0)
			{
				min += 1;
			}
			pkTimeLabel.text = TextsData.getData(310).chinese + min + TextsData.getData(335).chinese;
		}
		else 
		{ 
			pkTimeLabel.gameObject.SetActive(false);
		}
		
		//如果领取次数>0,则不能再次领取,把按钮的状态变为灰色//
		if(curReceiveNum > 0){
		
			_light.SetActive(false);
			_black.SetActive(true);
		}
		else {
			_light.SetActive(true);
			_black.SetActive(false);
		}
		
		
	}
	
	//绘制pk列表//
	public void DrawPKListData(){
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        
        //for(int i = 0; i< enemyList.Length;i++)
        //{
        //    enemyList[i].SetActive(true);
        //}

        Object enemy1 = Resources.Load("Prefabs/UI/UI-Arena/ItemEnemy");

        GameObject target = transform.FindChild("GameObject/EnemyItemLists").gameObject;
        GameObjectUtil.destroyGameObjectAllChildrens(target);
		for(int i = pkList.Count - 1;i >= 0; i--){
			string str = pkList[i];
			string[] ss = str.Split('-');
			int pkPlayerId = StringUtil.getInt( ss[0]);
			string pkName = ss[1];
			string pkHead = ss[2];
			int enemyRank = StringUtil.getInt(ss[3]);
			int power=StringUtil.getInt(ss[4]);
			
			//GameObject enemy = enemyList[enemyList.Length - 1 - i];
            GameObject enemy = Instantiate(enemy1) as GameObject;
            GameObjectUtil.gameObjectAttachToParent(enemy, target);
			GameObject pkBtn = enemy.transform.FindChild("Btn_PK").gameObject;
			UIButtonMessage message = pkBtn.GetComponent<UIButtonMessage>();
            message.target = this.gameObject;
			message.param = pkPlayerId;
			
            GameObject CheckBtn = enemy.transform.FindChild("Btn_CheckInformation").gameObject;
            UIButtonMessage message2 = CheckBtn.GetComponent<UIButtonMessage>();
            message2.target = this.gameObject;
            message2.functionName = "CheckClick";
            message2.param = pkPlayerId;
			UILabel name = enemy.transform.FindChild("Name").GetComponent<UILabel>();
			name.text = pkName;
			
			
			//修改enemy，头像//
			UISprite icon = enemy.transform.FindChild("Icon").GetComponent<UISprite>();
//			icon.atlas = headAtlas;
			icon.spriteName = pkHead;
			//获得头像所在的图集//
			string iconAtlasName = CardData.getAtlas(pkHead);
			UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(iconAtlasName);
			icon.atlas = iconAtlas;

            
			UILabel rank = enemy.transform.FindChild("Rank").GetComponent<UILabel>();
			//rank.text = TextsData.getData(309).chinese + enemyRank;
            rank.text = enemyRank+"";
			UILabel battlePower=enemy.transform.FindChild("power").GetComponent<UILabel>();
			//battlePower.text=TextsData.getData(203).chinese+power;
            battlePower.text = power + "";
            string cards = cardIds[i];
            string[] st = cards.Split('-');
            for (int k = 0; k < 6; k++)
            {
                UISprite icon1 = enemy.transform.FindChild((k + 1).ToString()).GetComponent<UISprite>();
               
                if (k >= st.Length)
                {
                    icon1.gameObject.SetActive(false);
                }
                else
                {
                    CardData cd = CardData.getData(StringUtil.getInt(st[k]));
                    icon1.spriteName = cd.icon;
                    icon1.atlas = LoadAtlasOrFont.LoadAtlasByName(cd.atlas);
                   
                }
            }
		}

        target.GetComponent<UIGrid>().repositionNow = true;
	}

    void CheckClick(int param)
    {
        types = 1;
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
        requestType = 9;
        PlayerInfo.getInstance().sendRequest(new UIJson(4,param), this);
    }
	//绘制排行榜信息//
	public void DrawRankData()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
		RankDragPanel.SetActive(true);
		string topSprName1 = "jjc_";
		for(int i = 0;i < topList.Count;i++)
		{
			PkRankElement pre = topList[i];
			GameObject top = Instantiate(TopItem)as GameObject;
			GameObjectUtil.gameObjectAttachToParent(top, GridList);
			TopGridItem tgi = top.GetComponent<TopGridItem>();
			tgi.Icon.spriteName = pre.icon;
			
			//获得头像所在的图集//
			string iconAtlasName = CardData.getAtlas(pre.icon);
			UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(iconAtlasName);
			tgi.Icon.atlas = iconAtlas;
			
			tgi.Name.text = pre.name;
			tgi.Power.text = TextsData.getData(365).chinese + pre.battlePower.ToString();
			tgi.RewardsRune.text = TextsData.getData(363).chinese + pre.award.ToString();
			tgi.TopLabel.text = pre.rank.ToString();
			if(i < 3)
			{

				tgi.TopSpr.gameObject.SetActive(true);
				tgi.TopLabel.gameObject.SetActive(false);
				tgi.TopSpr.spriteName = topSprName1 + (i + 1);
			}
//			else if(i == 1)
//			{
////				tgi.TopLabel.bitmapFont = FontNormal;
////				tgi.TopLabel.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
////				tgi.TopLabel.color = Color.blue;
//				tgi.TopSpr.gameObject.SetActive(true);
//				tgi.TopSpr.spriteName = topSprName1 + (i + 1);
//			}
//			else if(i == 2)
//			{
//				tgi.TopLabel.bitmapFont = FontNormal;
//				tgi.TopLabel.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
//				tgi.TopLabel.color = Color.red;
//			}
			else 
			{
				tgi.TopSpr.gameObject.SetActive(false);
				tgi.TopLabel.gameObject.SetActive(true);
				tgi.TopLabel.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			
			//为scrollView复制//
			UIDragPanelContents udpc = top.GetComponent<UIDragPanelContents>();
			udpc.draggablePanel = ScrollView.GetComponent<UIDraggablePanel>();
		}
		
		GridList.GetComponent<UIGrid>().repositionNow = true;
	}
	
	
	//显示掉落物品//
	public string ShowDrops()
	{
		string showStr = "";
		dropList.Clear();
		for(int i = 0; i< PlayerInfo.getInstance().pvpDropList.Count;i++)
		{
			dropList.Add(PlayerInfo.getInstance().pvpDropList[i]);
		}
		if(dropList != null && dropList.Count > 0)
		{
			string s1 = TextsData.getData(253).chinese;
			string s2 = TextsData.getData(254).chinese;
			
			for(int i = 0; i< dropList.Count;i++)
			{
				string str = dropList[i];
				string[] ss = str.Split('-');
				int type = StringUtil.getInt(ss[0]);
				int id = StringUtil.getInt(ss[1]);
				int num = StringUtil.getInt(ss[2]);
				string name = "";
				if(type == 1)		//item//
				{
					ItemsData item = ItemsData.getData(id);
					if(item != null)
					{
						name = item.name;
					}
				}
				else if(type == 2)		//equip//
				{
					EquipData ed = EquipData.getData(id);
					if(ed != null)
					{
						name = ed.name;
					}
				}
				else if(type == 3)		//card//
				{
					CardData cd = CardData.getData(id);
					if(cd != null)
					{
						name = cd.name;
					}
				}
				else if(type == 4)		//skill//
				{
					SkillData sd = SkillData.getData(id);
					if(sd != null)
					{
						name = sd.name;
					}
				}
				else if(type == 5)		//passiveSkill//
				{
					PassiveSkillData psd = PassiveSkillData.getData(id);
					if(psd != null)
					{
						name = psd.name;
					}
				}
				if(i == 0)
				{
					showStr = s1 + num + s2 + name;
				}
				else 
				{
					showStr += "," + num + s2 + name;
				}
			}
		}
		
		return showStr;
	}
	
	public void show ()
	{
		canClick = true;
//		base.show ();
		RankDragPanel.SetActive(false);
		//修改界面信息//
        ChangeData();
	}
   // int type = 0;
	//设置数据 markId 当前是哪个界面， 0 排位赛， 1 天位赛， 2夺宝奇兵; data 玩家在界面的信息。 pkL pk列表//
	//pkL格式：playerId-name-headName-rank(排名)//
	//time剩余冷却时间//
	public void SetData(int markId, string data, List<string> pkL, int pvpReward, int pkNum, int time,List<string> cardIDS,int type = 0){
		//curPage = markId;
		arenaData = data;
		pkList = pkL;
		this.totalPVPReward = pvpReward;
		//this.totalDekaronNum = pkNum;
		this.pkTime = time;
        this.cardIds = cardIDS;
      //  this.type = type;
        show();
	}
	
	public void SendToGetData()
	{
		//获取界面数据//
		requestType = 3;
		PlayerInfo.getInstance().sendRequest(new RankJson(0),this);
	}
	
	
	public void SendToPK(int emenyId)
	{
		requestType = 2;
		PlayerInfo.getInstance().sendRequest(new PkBattleJson(emenyId),this);
	}
	
	public void hide ()
	{
//		base.hide ();
		pkTimeLabel.gameObject.SetActive(false);
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA);
	}
	
	public void CleanScrollData()
	{
		
		ScrollBar.GetComponent<UIScrollBar>().value = 0;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,670,420);
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
		GridList.GetComponent<UIGrid>().repositionNow = true;
	}
	
	private void gc()
	{
        if (types != 1)
        {
            if (pkList != null)
            {
                pkList.Clear();
            }
            pkList = null;
        }
		headAtlas = null;
		FontNormal = null;
		FontNum = null;
		dropList.Clear();
		TopItem = null;
		Resources.UnloadUnusedAssets();
	}
	
	//------------------------------------------------- 按键响应 --------------------------------------//
	//挑战按钮的响应。。。id 对手的playerID//
	public void EnemyItemClickBtn(int id){
		if(!canClick)
			return;
		curSelEmenyId = id;
		//bool debug = false;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		/**
		if(pkTime > 0)		//冷却时间未到,则提示购买冷却时间//
		{
			requestType = 5;
			buyType = 4;
			jsonType = 1;
			costType = 1;
			
			PlayerInfo.getInstance().sendRequest(new BuyPowerOrGoldJson(jsonType, buyType, costType),this);
		}
		else 
		**/
//		dekaronNum = 0;
		if(dekaronNum <= 0){		//挑战次数已用完, 则提示购买挑战次数//

			
//			if(debug )
//			{
//				string str = TextsData.getData(137).chinese;
//				ToastWindow.mInstance.showText(str);
//			}
//			else 
			{
				
				int buyType = 6;
				int jsonType = 1;
				int costType = 1;
				if(ShowBuyTipControl.mInstance  != null)
				{
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_ARENA, 0);
				}
			}
		}
		else {					//挑战次数没用完//				
			SendToPK(id);
		}
	}
	
	//markId 下面的标签页的id 0, 排位赛， 1 天位赛，2 夺宝奇兵//
	public void MarkClickBtn(int markId){
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(markId > 0){
			string str = TextsData.getData(138).chinese;
			ToastWindow.mInstance.showText(str);
		}
		else {
			
			//curPage = markId;
			requestType = 3;
		}
	}
	
	//其他按键处理 type 0 领取按钮， 1 调整阵型，进入卡组界面， 2 back, 3 获取获取排行榜信息,4 查看战斗历史,15pvp商城//
	public void OtherClickBtn(int type){
		switch(type){
		case 0:					//领取按钮， 发送数据//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(curReceiveNum <= 0){
				
				requestType = 1;
				//发送请求数据//
				PlayerInfo.getInstance().sendRequest(new ReceiveAwardJson(),this);
			}
			break;
		case 1:					//调整阵型，进入卡组界面//
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP,0),this);
			break;
		case 2:					//back//
			
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			
//			MainUI.mInstance.show();
			//返回主城界面//
			UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU,
				"MainMenuManager") as MainMenuManager;
			main.SetData(STATE.ENTER_MAINMENU_BACK);
			
			hide();
			break;
		case 3:					//查看排行榜功能//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType = 6;
			PlayerInfo.getInstance().sendRequest(new PkRankJson(),this);
			break;
		case 4:					//查看pk历史//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType = 7;
			PlayerInfo.getInstance().sendRequest(new PkRecordJson(),this);
			break;
        case 5:					//换一批对手//
            //播放音效//
            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
            requestType = 8;
            PlayerInfo.getInstance().sendRequest(new UIJson(55, rank), this);
            break;
        case 15:					//商人//
			requestType = 10;
            PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SHOP, 4), this);
            break;
        case 16:					//点击帮助//
            break;
		}
	}
	
	//关闭dragPanel//
	public void OnClickDragCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		CleanScrollData();
		RankDragPanel.SetActive(false);
	}
	
	public void OnClickPkHistoryCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		pkHistoryPanel.SetActive(false);
	}
	//绘制pk历史记录//
	public void DrawPkHistoryData()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(pkHistoryParent);
		for(int i = 0;i<pkHistoryList.Count;i++)
		{
			PkRecordElement pre = pkHistoryList[i];
			if(loadPkHistoryCell == null)
			{
				loadPkHistoryCell=GameObjectUtil.LoadResourcesPrefabs("UI-Arena/Fightinghistory-cell",3);
			}
			GameObject cell=Instantiate(loadPkHistoryCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,pkHistoryParent);
			UILabel time = cell.transform.FindChild("time").GetComponent<UILabel>();
			UILabel rank = cell.transform.FindChild("rank").GetComponent<UILabel>();
			if(pre.type == 0)//挑战别人//
			{
				if(pre.rank == 0)
				{
					if(pre.r == 1)
					{	//战胜，排名不变//
						rank.text = TextsData.getData(467).chinese + pre.name + TextsData.getData(464).chinese + TextsData.getData(466).chinese;
					}
					else if(pre.r == 2)
					{	//战败，排名不变//
						rank.text = TextsData.getData(467).chinese + pre.name + TextsData.getData(465).chinese + TextsData.getData(466).chinese;
					}
				}
				else if(pre.r == 1)
				{
					//战胜，排名上升//
					rank.text = TextsData.getData(467).chinese + pre.name + TextsData.getData(464).chinese + TextsData.getData(469).chinese + pre.rank;
				}
			}
			else if (pre.type == 1)//别人挑战//
			{
				if(pre.rank == 0)
				{
					if(pre.r == 1)
					{	//战胜，排名不变//
						rank.text =  pre.name + TextsData.getData(463).chinese + TextsData.getData(464).chinese + TextsData.getData(466).chinese;
					}
					else if(pre.r == 2)
					{	//战败，排名不变//
						rank.text = pre.name + TextsData.getData(463).chinese +  TextsData.getData(465).chinese + TextsData.getData(466).chinese;
					}
				}
				else if(pre.r == 2)
				{
					//战败，排名下降//
					rank.text = pre.name + TextsData.getData(463).chinese +  TextsData.getData(465).chinese + TextsData.getData(468).chinese + pre.rank;
				}
			}
			
			if(pre.time <= 1)
			{//一小时内//
				time.text = 1+TextsData.getData(475).chinese;
			}
			else if (pre.time>=24)
			{//一天以上//
				int dayTime = (pre.time/24)==0?1:pre.time/24;
				time.text = dayTime + TextsData.getData(477).chinese;
			}
			else
			{//一天内//
				time.text = pre.time+TextsData.getData(476).chinese;
			}
		}
		
		if(pkHistoryList.Count == 0)
		{
			if(loadPkHistoryCell == null)
			{
				loadPkHistoryCell=GameObjectUtil.LoadResourcesPrefabs("UI-Arena/Fightinghistory-cell",3);
			}
			GameObject cell=Instantiate(loadPkHistoryCell) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(cell,pkHistoryParent);
			UILabel time = cell.transform.FindChild("time").GetComponent<UILabel>();
			UILabel rank = cell.transform.FindChild("rank").GetComponent<UILabel>();
			time.text = "";
			rank.text = TextsData.getData(478).chinese;
		}
		pkHistoryParent.GetComponent<UIGrid>().repositionNow=true;
	}
	
	public void receiveResponse (string json)
	{
		Debug.Log("ArenaUIManager : " + json);
		if(json != null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType){
			case 1:
				ReceiveAwardResultJson rarj = JsonMapper.ToObject<ReceiveAwardResultJson>(json);
				errorCode = rarj.errorCode;
				if(errorCode == 0)
				{
					curReceiveNum = rarj.type;
				}
				receiveData = true;
				break;
			case 2:
				dropList.Clear();
				PkBattleResultJson pbrj=JsonMapper.ToObject<PkBattleResultJson>(json);
				errorCode = pbrj.errorCode;
				if(errorCode == 0)
				{
					//设置战斗数据//
					//				PlayerInfo.getInstance().bNum=mbrj.bNum;
					PlayerInfo.getInstance().pbrj = pbrj;
					dropList = pbrj.ds;
				}
				
			
				receiveData = true;
				break;
			case 3:			//请求界面信息//
				RankResultJson rrj = JsonMapper.ToObject<RankResultJson>(json);
				errorCode = rrj.errorCode;
				if(errorCode == 0)
				{
					arenaData = rrj.s;
					pkList = rrj.ss;
                    cardIds = rrj.cardIds;
					//totalDekaronNum = rrj.sPknum;
					totalPVPReward = rrj.sAward;
					pkTime = (int)(rrj.cdtime);
					
				}
				receiveData = true;
				
				break;
			case 4:
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
					cardGroupRJ = cgrj;
//					CombinationInterManager.mInstance.curCardGroup=cgrj.transformCardGroup();
//					CombinationInterManager.mInstance.curPage=0;
//					CombinationInterManager.mInstance.isUsed=true;
				}
				receiveData=true;
				break;
			case 5:
				BuyPowerOrGoldResultJson brj = JsonMapper.ToObject<BuyPowerOrGoldResultJson>(json);
				errorCode = brj.errorCode;
				this.costCrystal = brj.crystal;
				this.num = brj.num;
				this.times = brj.times;
				receiveData = true;
				break;
			case 6:					//请求排行榜数据//
				PkRankResultJson prrj = JsonMapper.ToObject<PkRankResultJson>(json);
				errorCode = prrj.errorCode;
				topList = prrj.pes;
				receiveData = true;
				
				break;
			case 7:
				PkRecordResultJson pkrrj = JsonMapper.ToObject<PkRecordResultJson>(json);
				errorCode = pkrrj.errorCode;
				pkHistoryList = pkrrj.list;
				receiveData = true;
				break;
            case 8:
				PkrankListResultJson pkjson = JsonMapper.ToObject<PkrankListResultJson>(json);
				receiveData = true;
                pkList = pkjson.pks;
                cardIds = pkjson.cardIds;
				break;
            case 9:
               CardGroupResultJson cgrj1=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj1.errorCode;
				if(errorCode == 0)
				{
					cardGroupRJ = cgrj1;
//					CombinationInterManager.mInstance.curCardGroup=cgrj.transformCardGroup();
//					CombinationInterManager.mInstance.curPage=0;
//					CombinationInterManager.mInstance.isUsed=true;
				}
				receiveData=true;
                break;
			case 10:
				    receiveData = true;
                    ShopResultJson temp = JsonMapper.ToObject<ShopResultJson>(json);
                    errorCode = temp.errorCode;
                    if (errorCode == 0)
                    {
                        shopRJ = temp;
                    }
				break;
			}
		}
	}
	
}
