using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CombinationInterManager : MonoBehaviour,ProcessResponse ,BWWarnUI{
	
	public GameObject uSkillBox;
	//==怒气上限==//
	public UILabel energy;
	//新合体技标识:1有新合体技,0没有//
	public int unitMark;
	public GameObject unitCost;
	public GameObject listpanel;
	public GameObject bottomFrame1;
	public GameObject bottomFrame2;
	
	//当前卡组的信息//
	public CardGroup curCardGroup;
	//1请求卡组数据,2保存卡组数据,3后退到MainCardSetPanel,4设置上阵或者休息,10后退到竞技场界面ArenaUIManager,11点击合体技按钮,请求背包卡牌信息//
	//private Transform _myTransform;
    private Zhen zhenrong;
	private TianFuTiao[] tftmen;
	private GameObject tftGo;
	private int requestType;
	private int errorCode;
	private bool receiveData;
	//返回的页面的类型,1 MainCardSetPanel,2 ArenaUIManager,3 MissionUI;//
	int curBackType;
	//进入竞技场界面信息--玩家信息//
	string playerArenaData;
	//排位赛界面pk对手信息//
    List<string> pkList;
    //List<string> cardIds;
	//总的挑战次数//
	int totalDekaronNum;
	//总的符文值//
	int totalPVPReward;
	//pk剩余时间//
	int pkTime;
	
	public GameObject teamTalent;
	
	public GameObject uniteSkill;


    public GameObject getWayPanel;

    public GameObject getWayItemPrefab;

    public GameObject getWayDragParent;

    public GameObject battleButton;

    public GameObject explainButton;

    public GameObject explainPanel;

    public UILabel explainLabel;


    //float getWayItemStartY = 180f;

    //float getWayItemOffY = 160f;
	

    private Hashtable uniteSkillCards = new Hashtable();
	
	string listUnitePath = "Prefabs/UI/ScrollViewPanel/ListItemUnitSkill";
	
	GameObject loadunitePrefab = null;
	
	public GameObject uniteParent;
	
	//int clickedIndex = -1;
	
	public UIAtlas unitSkillUIAtlas;
	
	List<UniteScrollViewItem> usvItemList = new List<UniteScrollViewItem>();
	
	public UIScrollBar listScrollBar;
	
	public UILabel talentTipLab;
	
	//推图界面的json//
	private MapResultJson mapRJ;

    public GameObject talentButton;

    public GameObject uniteSkillButton;

    public int num;

    List<string> strData;
	void Awake(){
		//_myTransform = transform ;
		ZhenRoot.mInstance.gameObject.SetActive(true);
        zhenrong = ZhenRoot.mInstance.zhen;
        zhenrong.yuanlai = this;
		clearData();
	}
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			
			if(errorCode==65)			//当前出战阵容里没有可出战的卡！//
			{
				ToastWindow.mInstance.showText(TextsData.getData(198).chinese);
				return;
			}
			if(errorCode==70)			//vip等级不足！//
			{
				string str = TextsData.getData(243).chinese;
//				ToastWindow.mInstance.showText(str);
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(errorCode==71)			//您的钻石数量不足！//
			{
				string str = TextsData.getData(244).chinese;
//				ToastWindow.mInstance.showText(str);
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(errorCode==72)			//购买次数达到上限//
			{
				ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
				return;
			}
			switch(requestType)
			{
			case 3://退出//
				if(curBackType == 1)
				{
					ChangeUIToComb();
					//onClickShowTalent(2);
				}
				else if(curBackType == 2)
				{
					requestType = 10;
					//发送获取竞技场界面信息请求//
					PlayerInfo.getInstance().sendRequest(new RankJson(0),this);
				}
				else if(curBackType==3)
				{
					changeUIToMission();
				}
                else if (curBackType == 4)
                {
                    requestType = 10;
                    //发送获取竞技场界面信息请求//
                    PlayerInfo.getInstance().sendRequest(new RankJson(0), this);
                }
                else if (curBackType == 5)
                {
                    requestType = 16;
                    //发送获取异世界界面信息请求//
                    PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_EVENT), this);
                }
				
				break;
			case 10:
				changeUIToArena();
				break;
			case 11://==增加怒气上限==//
				HeadUI.mInstance.refreshPlayerInfo();
				//==怒气上限==//
				energy.text=PlayerInfo.getInstance().player.maxEnergy+"";
				break;
			case 12:
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
				MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI")as MissionUI;
				mission.mrj=mapRJ;
				mission.showNewCombinationTip();
				changeUIToMission();
				break;
            case 15:
                if(errorCode == 0){
					PlayerInfo.getInstance().battleType = STATE.BATTLE_TYPE_EVENT;
					PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_EVENT;
                    PlayerInfo.getInstance().isEvent = true;
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
                    if (curCardGroup.changeMark == 1 && curBackType != 2)
                    {
                        requestType = 3;
                        PlayerInfo.getInstance().sendRequest(new SaveCGJson(curCardGroup), this);
                        return;
                    }
				}
				else if(errorCode == 24)		//进入次数到达上限，更高的vip等级可以增加进入次数//
				{
					string str = TextsData.getData(277).chinese;
					ToastWindow.mInstance.showText(str);
					//提示去充值//
					//UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 27)		//体力不足//
				{
//					string str = TextsData.getData(149).chinese;
//					ToastWindow.mInstance.showText(str);
					int buyType = 2;
					int jsonType = 1;
					int costType = 1;
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_ACTIVESEL);
				}
				else if(errorCode == 57)		//等级不足//
				{
					string str = TextsData.getData(148).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 86)			//冷却时间未到，无法进入//
				{
					int buyType = 5;
					int jsonType = 1;
					int costType = 1;
					int cdType = 3;			//活动副本//
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, 
						BuyTipManager.UI_TYPE.UI_ACTIVESEL, cdType, PlayerInfo.getInstance().copyId);
				}
				break;
            case 16:
                case 2:
                PlayerInfo.getInstance().bBackActivity = true;  //返回活动界面//

                //打开活动副本界面//
                UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
                ActiveWroldUIManager activeCopy = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY,
                    "ActiveWroldUIManager") as ActiveWroldUIManager;
                activeCopy.setData(strData,num);
                if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ActiveCopy))
                {
                    UISceneDialogPanel.mInstance.showDialogID(22);
                }
                hide();
                break;
			}
		}
	}
	//合体技在表中的id//
    public void GetWay(int index)
    {
        ShowGetWayData(index);
    }

    public void OnClickGetWayCloseBtn()
    {
//        getWayPanel.SetActive(false);
    }
	public void show ()
	{
		//显示head//
		HeadUI.mInstance.show();
		//==绘制阵容==//
        showzhen();
		//==绘制种族属性==//
        setTF();
		//==绘制合体技==//
		//showUnit();
		teamTalent.SetActive(false);
		uniteSkill.SetActive(true);
		ShowUniteSkill();
		listScrollBar.value = 0;
		//==绘制底框==//
		showBottom();
       


        if (curBackType == 2)
        {
            GetArena(true);
        }
        else if (curBackType == 5)
        {
            SetBattleButton();
        }
	}
    void SetBattleButton()
    {
        bottomFrame1.SetActive(false);
        bottomFrame2.SetActive(false);
        battleButton.SetActive(true);


        explainButton.SetActive(true);

        this.transform.parent.FindChild("UI-head").GetComponent<UIPanel>().depth = 0;
    }
    void GetArena(bool isShow)
    {
        talentButton.transform.localPosition = new Vector3(215, talentButton.transform.localPosition.y, talentButton.transform.localPosition.z);
        talentButton.GetComponent<UIToggle>().enabled = false;
        uniteSkillButton.SetActive(false);
        teamTalent.SetActive(true);
        uniteSkill.SetActive(false);
        setTF();
        listScrollBar.value = 0;
        bottomFrame1.SetActive(false);
        this.transform.parent.FindChild("UI-head").GetComponent<UIPanel>().enabled = isShow ? false : true;
    }
    List<string> pkList1 = new List<string>();

    List<string> cards1 = new List<string>();
    int curSelEventId;

	//page 当前显示的页,backType 返回的类型,1 返回到 mainCardSetPanel,2 返回到竞技场界面,3返回到推图//
    public void SetData(int backType, List<string> cards = null, List<string> pkList = null, int curSelEventId = 0)
	{
		curBackType = backType;
        if (curBackType == 2 || curBackType == 4)
        {
            pkList1 = pkList;
            cards1 = cards;
            
        }
        if (curBackType == 5)
        {
            this.curSelEventId = curSelEventId;
        }
        
		show();
	}
	
	private void showBottom()
	{
		bottomFrame1.SetActive(true);
		bottomFrame2.SetActive(true);
		
		UnlockData ud3=UnlockData.getData(29);
		UnlockData ud4=UnlockData.getData(30);
		UnlockData ud5=UnlockData.getData(31);
		UnlockData ud6=UnlockData.getData(32);
		
		int cardNum=curCardGroup.getCardNum();
		int level=PlayerInfo.getInstance().player.level;
		if(level < ud3.method)
		{
			bottomFrame1.transform.FindChild("Label").GetComponent<UILabel>().text=cardNum+"/"+2;
			bottomFrame2.transform.FindChild("Label").GetComponent<UILabel>().text=TextsData.getData(471).chinese.Replace("x",ud3.method+"").Replace("y",3+"");
		}
		else if(level<ud4.method)
		{
			bottomFrame1.transform.FindChild("Label").GetComponent<UILabel>().text=cardNum+"/"+3;
			bottomFrame2.transform.FindChild("Label").GetComponent<UILabel>().text=TextsData.getData(471).chinese.Replace("x",ud4.method+"").Replace("y",4+"");
		}
		else if(level<ud5.method)
		{
			bottomFrame1.transform.FindChild("Label").GetComponent<UILabel>().text=cardNum+"/"+4;
			bottomFrame2.transform.FindChild("Label").GetComponent<UILabel>().text=TextsData.getData(471).chinese.Replace("x",ud5.method+"").Replace("y",5+"");
		}
		else if(level<ud6.method)
		{
			bottomFrame1.transform.FindChild("Label").GetComponent<UILabel>().text=cardNum+"/"+5;
			bottomFrame2.transform.FindChild("Label").GetComponent<UILabel>().text=TextsData.getData(471).chinese.Replace("x",ud6.method+"").Replace("y",6+"");
		}
		else
		{
			bottomFrame1.transform.FindChild("Label").GetComponent<UILabel>().text=cardNum+"/"+6;
			bottomFrame2.SetActive(false);
		}
	}
	
    private void showzhen()
    {
		int[] modelIds = new int[6];
		int[] cardIds = new int[6];
		int[] skillTypes=new int[6];
		int[] bn = new int[6];		//突破次数//
        for (int i = 0; i < curCardGroup.cards.Length; i++)
        {
            //修改卡牌图片名字//
            PackElement dbCard = curCardGroup.cards[i];
            if (dbCard == null)
            {
                modelIds[i] = -1;
				bn[i] = -1;
            }
            else
            {
                CardData cd = CardData.getData(dbCard.dataId);
				cardIds[i]=cd.id;
                string cardIconName = cd.icon;
                modelIds[i] = int.Parse(cardIconName.Substring(4));
				PackElement pe=curCardGroup.skills[i];
				if(pe!=null)
				{
					SkillData sd=SkillData.getData(pe.dataId);
					skillTypes[i]=sd.type;
				}
				else
				{
					SkillData sd=SkillData.getData(cd.basicskill);
					skillTypes[i]=sd.type;
				}
				bn[i] = dbCard.bn;
            }
        }
		bool canShangzhen=true;
		//==如果没有解锁,显示提示==//
		UnlockData ud3=UnlockData.getData(29);
		UnlockData ud4=UnlockData.getData(30);
		UnlockData ud5=UnlockData.getData(31);
		UnlockData ud6=UnlockData.getData(32);
		int cardNum=curCardGroup.getCardNum();
		int level=PlayerInfo.getInstance().player.level;
		if(level<ud3.method && cardNum >= 2)
		{
			canShangzhen=false;
		}
		else if(level<ud4.method && cardNum>=3)
		{
			canShangzhen=false;
		}
		else if(level<ud5.method && cardNum>=4)
		{
			canShangzhen=false;
		}
		else if(level<ud6.method && cardNum>=5)
		{
			canShangzhen=false;
		}
        zhenrong.init(modelIds, cardIds, skillTypes, canShangzhen, curCardGroup.cardTips, bn, curBackType == 2 ? true : false);
    }
	
	private void setTF()
    {
		GameObjectUtil.destroyGameObjectAllChildrens(listpanel);
		tftmen = null;
		talentTipLab.text = TextsData.getData(571).chinese;
//        if (tftmen != null)
//        {
//            for (int i = 0; i < tftmen.Length; i++)
//            {
//                Destroy(tftmen[i].gameObject);
//            }
//            tftmen = null;
//        }
		//==获取各种族人数,中国,亚洲,希腊,北欧==//
		int[] races={1,2,3,4};
		int[] nums={0,0,0,0};
		foreach(PackElement pe in curCardGroup.cards)
		{
			if(pe!=null)
			{
				CardData cd=CardData.getData(pe.dataId);
				if(cd!=null)
				{
					for(int k=0;k<races.Length;k++)
					{
						if(cd.race==races[k])
						{
							nums[k]=nums[k]+1;
						}
					}
				}
			}
		}
		//==获取人数最多的种族,如果人数相同按照：中国、亚洲、希腊、北欧取值==//
		int maxNum=0;
		int race=0;
		for(int k=0;k<nums.Length;k++)
		{
			if(nums[k]>maxNum)
			{
				maxNum=nums[k];
				race=races[k];
			}
		}
		//==获取种族属性==//
		RacePowerData rpd=RacePowerData.getData(maxNum);
		if(rpd == null)
		{
			GameObject tftEmpty = GameObjectUtil.LoadResourcesPrefabs("EmbattlePanel/tianfutiaoEmpty",3);
			if(tftEmpty == null)
				return;
			GameObject tftEmptyObj = Instantiate(tftEmpty) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(tftEmptyObj,listpanel);
			UILabel infoLabel = tftEmpty.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
			infoLabel.text = TextsData.getData(501).chinese;
			CardInfoPanelManager.teamATK = 0;
			CardInfoPanelManager.teamDEF = 0;
			CardInfoPanelManager.teamHP = 0;
			CardInfoPanelManager.teamATKAdd = 0;
			CardInfoPanelManager.teamDEFAdd = 0;
			CardInfoPanelManager.teamHPAdd = 0;
		}
		else
		{
			//==显示种族属性==//
			clearData();
	        tftmen = new TianFuTiao[5];
	        float ytemp = 0;
	        for (int i = 2; i <= 6; i++)
	        {
				int index=i-2;
				if(tftGo==null)
				{
					tftGo=GameObjectUtil.LoadResourcesPrefabs("EmbattlePanel/tianfutiao",3);
				}
	            GameObject gobj = Instantiate(tftGo) as GameObject;
	            gobj.transform.parent = listpanel.transform;
	            gobj.transform.localScale = new Vector3(1, 1, 1);
	            gobj.transform.localPosition = new Vector3(0,ytemp,0);
	            tftmen[index] = gobj.GetComponent<TianFuTiao>();
				string raceIcon="race_"+race;
				string raceName="";
				switch(race)
				{
				case 1:
					raceName=TextsData.getData(5).chinese;
					break;
				case 2:
					raceName=TextsData.getData(6).chinese;
					break;
				case 3:
					raceName=TextsData.getData(8).chinese;
					break;
				case 4:
					raceName=TextsData.getData(7).chinese;
					break;
				}
				raceName+="x"+i;
				RacePowerData rd=RacePowerData.getData(i);
				string raceAttris="";
				for(int k=0;k<rd.attris.Count;k++)
				{
					string[] ss=rd.attris[k].Split('-');
					int type=StringUtil.getInt(ss[0]);
					int num=StringUtil.getInt(ss[1]);
					int textId=0;
					switch(type)
					{
					case 1:
						textId=419;
						break;
					case 2:
						textId=420;
						break;
					case 3:
						textId=421;
						break;
					case 4:
						textId=422;
						break;
					case 5:
						textId=423;
						break;
					case 6:
						textId=424;
						break;
					case 7:
						textId=425;
						break;
					case 8:
						textId=426;
						break;
					}
					string des=TextsData.getData(textId).chinese.Replace("num",num+"");
					raceAttris+=des+"\n";
				}
	            tftmen[index].reset(raceIcon, raceName, raceAttris,maxNum>=i);
				if(maxNum>=i)
				{
					for(int k=0;k<rpd.attris.Count;k++)
					{
						string[] ss=rpd.attris[k].Split('-');
						int type=StringUtil.getInt(ss[0]);
						int num=StringUtil.getInt(ss[1]);
						//int textId=0;
						switch(type)
						{
						case 1:
							CardInfoPanelManager.teamATK += num;
							break;
						case 2:
							CardInfoPanelManager.teamDEF += num;
							break;
						case 3:
							CardInfoPanelManager.teamHP += num;
							break;
						case 4:
							CardInfoPanelManager.teamATKAdd += num/100f;
							break;
						case 5:
							CardInfoPanelManager.teamDEFAdd += num/100f;
							break;
						case 6:
							CardInfoPanelManager.teamHPAdd += num/100f;
							break;
						}
					}
				}
	            ytemp -= tftmen[index].height;
	        }
		}
    }
	
	//==显示合体技内容==//
	private void showUnit()
	{
		int unitId=curCardGroup.unitSkillId;
		GameObject icon = uSkillBox.transform.FindChild("Icon").gameObject;
		GameObject iconEmpty = uSkillBox.transform.FindChild("SkillBtn_Frame").gameObject;
		GameObject name = uSkillBox.transform.FindChild("nmLabel").gameObject;
        GameObject info = uSkillBox.transform.FindChild("ifLabel").gameObject;
		if(unitId>0)
		{
			icon.SetActive(true);
			iconEmpty.SetActive(false);
			name.SetActive(true);
            info.SetActive(true);
			UnitSkillData usd=UnitSkillData.getData(unitId);
			icon.GetComponent<UISprite>().spriteName=usd.icon;
            name.GetComponent<UILabel>().text = usd.name;
            info.GetComponent<UILabel>().text = usd.description;
			unitCost.SetActive(true);
			unitCost.transform.FindChild("Label").GetComponent<UILabel>().text=usd.cost+"";
		}
		else
		{
			icon.SetActive(false);
			iconEmpty.SetActive(true);
			name.SetActive(false);
            info.SetActive(false);
			unitCost.SetActive(false);
		}
		GameObject changes = uSkillBox.transform.FindChild("Changes").gameObject;
		if(unitMark == 1)
		{
			changes.SetActive(true);
		}
		else
		{
			changes.SetActive(false);
		}
		//==怒气上限==//
		energy.text=PlayerInfo.getInstance().player.maxEnergy+"";
	}
	
	//==点击增加怒气上限==//
	public void onClickEnergy()
	{
		int number=EnergyupData.getNumber(PlayerInfo.getInstance().player.maxEnergy);
		int canBuyNumber = VipData.getData(PlayerInfo.getInstance().player.vipLevel).maxenergy;
		if(number >= canBuyNumber)			//购买次数达到上限//
		{
			ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
			return;
		}
		EnergyupData ed=EnergyupData.getData(number+1);
		if(ed==null)		 //购买次数达到上限//
		{
			ToastWindow.mInstance.showText(TextsData.getData(240).chinese);
			return;
		}
		else
		{
			//购买怒气提示//
			ToastWarnUI.mInstance.showWarn(TextsData.getData(245).chinese.Replace("num1",ed.cost+"").Replace("num2",ed.energy+""),this);
		}
	}
	
	public void SelectBtn(int id)
	{
		if(id == 2)
		{
			//点击合体技按钮//
			//UnitSkillPanel.mInstance.openUnitSkillPanel(UnitSkillPanel.SELECTSKILLTYPE.E_NULL);
			//将自己的panel隐藏//
			hide();
		}
	}

    public void onClickbattleBtn(int param)
    {
        if (param == 0)
        {
            requestType = 15;
            PlayerInfo.getInstance().sendRequest(new EventBattleJson(curSelEventId, PlayerInfo.getInstance().copyId), this);
        }
        else if (param == 1)
        {

            explainPanel.SetActive(true);

            explainLabel.text = TextsData.getData(715).chinese;
        }
        else if (param == 2)
        {
            explainPanel.SetActive(false);
        }
    }
	public void onClickBackBtn()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
			return;
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_UseCombo3) && GuideUI_UseCombo3.mInstance.runningStep != 2)
		{
			return;
		}
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		//如果卡组为空,则弹框//
		if(curCardGroup.getCardNum()<=0)		
		{
			//当前出战阵容里没有可出战的卡！//
			string str = TextsData.getData(198).chinese;
			ToastWindow.mInstance.showText(str);
			return;
		}
        if (curCardGroup.changeMark == 1 && curBackType!=2)
		{
			requestType=3;
            PlayerInfo.getInstance().sendRequest(new SaveCGJson(curCardGroup), this);
			return;
		}
		else
		{
			if(curBackType == 1)
			{
				ChangeUIToComb();
			}
			else if(curBackType == 2)
			{
				requestType = 10;
				//发送获取竞技场界面信息请求//
				PlayerInfo.getInstance().sendRequest(new RankJson(0),this);
			}
			else if(curBackType==3)
			{
				//发送获取推图界面信息请求//
				requestType = 12;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
			}
            else if (curBackType == 4)
            {
                requestType = 10;
                //发送获取竞技场界面信息请求//
                PlayerInfo.getInstance().sendRequest(new RankJson(0), this);
            }
            else if (curBackType == 5)
            {
                requestType = 16;
                //发送获取异世界界面信息请求//
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_EVENT), this);
            }
		}
	}
	
	public void renclick(int zid,int cid)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
        if(cid == -1)
        {
            //==如果没有解锁,显示提示==//
			UnlockData ud3=UnlockData.getData(29);
			UnlockData ud4=UnlockData.getData(30);
			UnlockData ud5=UnlockData.getData(31);
			UnlockData ud6=UnlockData.getData(32);
			int cardNum=curCardGroup.getCardNum();
			int level=PlayerInfo.getInstance().player.level;
			if(level < ud3.method && cardNum >=2)
			{
				//玩家等级达到level级解锁下一上阵位//
				ToastWindow.mInstance.showText(TextsData.getData(197).chinese.Replace("level", ud3.method + ""));
				return;
			}
			else if(level<ud4.method && cardNum>=3)
			{
				ToastWindow.mInstance.showText(TextsData.getData(197).chinese.Replace("level", ud4.method + ""));
				return;
			}
			else if(level<ud5.method && cardNum>=4)
			{
				ToastWindow.mInstance.showText(TextsData.getData(197).chinese.Replace("level", ud5.method + ""));
				return;
			}
			else if(level<ud6.method && cardNum>=5)
			{
				ToastWindow.mInstance.showText(TextsData.getData(197).chinese.Replace("level", ud6.method + ""));
				return;
			}
        }
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO);
        CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager") as CardInfoPanelManager;
        cardInfo.curCardBoxId = zid;
        cardInfo.curType = curBackType;
        cardInfo.show();

        baseHide();


       
    }
	
	//合体技的id//
    public void ShowGetWayData(int index)   
    {
//        getWayPanel.SetActive(true);
       // CardGroup cg = curCardGroup;
		//合体技//
        List<int> uniteSkillIds = new List<int>();

        UnitSkillData data = UnitSkillData.getData(index);
        foreach (int c in data.cards)
        {
            if (c != 0)
            {
                uniteSkillIds.Add(c);
            }
        }
		uniteSkillCards.Clear();
		//float nextOffY = 0;
		//float x = 0;
		//修改需要卡牌id//
		List<int> ids = new List<int>();
		int d = data.card1;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card2;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card3;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card4;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card5;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card6;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card7;
		if (d > 0)
		{
			ids.Add(d);
		}
		d = data.card8;
		if (d > 0)
		{
			ids.Add(d);
		}
            
        List<int> cardIds = ids;
        //有几个卡牌就绘制几个item//
//        GameObjectUtil.destroyGameObjectAllChildrens(getWayDragParent);
//        for (int i = 0; cardIds != null && i < cardIds.Count; i++)
//        {
//            CardData cd = CardData.getData(cardIds[i]);
//            GameObject item = Instantiate(getWayItemPrefab) as GameObject;
//            GameObjectUtil.gameObjectAttachToParent(item, getWayDragParent);
//            float y = getWayItemStartY - i * getWayItemOffY;
//            item.transform.localPosition = new Vector3(0, y, 0);
//
//            GetWayItem gwi = item.GetComponent<GetWayItem>();
//            gwi.sci2.setSimpleCardInfo(cardIds[i], GameHelper.E_CardType.E_Hero);
//            gwi.Name.text = cd.name;
//
//            //修改卡牌出处//
//            string getWay = cd.waytoget;
//            string[] str = getWay.Split(',');
//            string showData = "";
//            for (int m = 0; m < str.Length; m++)
//            {
//                int id = StringUtil.getInt(str[m]);
//                string ss = "";
//                switch (id)
//                {
//                    case 0:
//                        ss = TextsData.getData(517).chinese;
//                        break;
//                    case 1:
//                        ss = TextsData.getData(510).chinese;
//                        break;
//                    case 2:
//                        ss = TextsData.getData(511).chinese;
//                        break;
//                    case 3:
//                        ss = TextsData.getData(512).chinese;
//                        break;
//                    case 4:
//                        ss = TextsData.getData(513).chinese;
//                        break;
//                    case 5:
//                        ss = TextsData.getData(514).chinese;
//                        break;
//                }
//                showData += ss + "\r\n";
//            }
//            gwi.Des.text = showData;
//        }
		//显示合体技获得途径界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL);
		GetWayPanelManager getWay = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_GETWAYPANEL, "GetWayPanelManager")as GetWayPanelManager;
		//3 表示从阵容界面进入//
		getWay.SetData(index, cardIds, 3);	 


    }
	//==Type. 1 主要组合技能，2 辅助组合技能==//
	public void UniteSkillBtn(int type)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		//set OPENTYPE,item list and selectID
		CardGroup cg = curCardGroup;
        List<UnitSkillData> us = UnitSkillData.GetSkills();

		List<UnitSkillData> units=UnitSkillData.getUnitSkills(cg.getCardIds());
		if(units.Count==0)
		{
			//无可组成合体技//
			ToastWindow.mInstance.showText(TextsData.getData(238).chinese);
			return;
		}
		List<string> sortList=new List<string>();
        for (int i = 0; i < us.Count; i++)
		{
            sortList.Add(us[i].index + "-" + 2 + "-null");
		}
		int unitId = curCardGroup.unitSkillId;

        List<PackElement> pe = cg.GetPc();
		//显示黑色半透底框//
		BlackBgUI.mInstance.SetData(2);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW);
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW,"ScrollViewPanel" ) as ScrollViewPanel;
        scrollView.openScrollViewPanel(ScrollViewPanel.OPENTYPE.E_UNITSKILL, UnitSkillData.getSortedUnitSkill(cg, pe), unitId);
	}
	
	public void ShowUniteSkill()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		CardGroup cg = curCardGroup;
        //List<UnitSkillData> us = UnitSkillData.GetSkills();
		
		//List<UnitSkillData> units=UnitSkillData.getUnitSkills(cg.getCardIds());
		//List<PackElement> pe = cg.GetPc();
		int unitId = curCardGroup.unitSkillId;
		OpenUniteScrollViewPanel(UnitSkillData.getUnitSkillList(cg),unitId);
	}
	
	
	public void OpenUniteScrollViewPanel(List<string> list, int selectID)
	{
		loadunitePrefab = Resources.Load(listUnitePath) as GameObject;
		usvItemList.Clear();
		GameObjectUtil.destroyGameObjectAllChildrens(uniteParent);
		foreach (string str in list)
        {
			string[] ss = str.Split('-');
//			if(!ss[0].StartsWith("%"))
//			{
//				if(ss[0].Equals("60001")||ss[0].Equals("60002")||ss[0].Equals("60003"))
//				{
//					continue;
//				}
//			}
			UnitSkillData usd = UnitSkillData.getData(StringUtil.getInt(ss[0]));
			if(usd.number>=100)
			{
				continue;
			}
			GameObject listItemObj = (GameObject)GameObject.Instantiate(loadunitePrefab);
			GameObjectUtil.gameObjectAttachToParent(listItemObj, uniteParent,true);
			if (listItemObj == null)
                continue;
			UniteScrollViewItem usvItem = listItemObj.GetComponent<UniteScrollViewItem>();
			if (usvItem == null)
                continue;
			
        	//int index = -1;
			if (ss[0].StartsWith("%"))
            {
                usvItem.baseSkillID = StringUtil.getInt(ss[0].Substring(1));
                usvItem.isBaseSkill = true;
            }
            else
            {
                usvItem.index = StringUtil.getInt(ss[0]);
            }
//			usvItem.bm.param = usvItem.index;
//            usvItem.bm.target = _myTransform.gameObject;
//            usvItem.bm.functionName = "OnClickUniteListItemUseBtn";
			
			usvItem.mark = StringUtil.getInt(ss[1]);
            string heroIndexString = ss[2];
            if (heroIndexString != "null")
            {
                usvItem.useCardHeroIndex = StringUtil.getInt(heroIndexString);
            }
            else
            {
                usvItem.useCardHeroIndex = -1;
            }
            usvItem.useBtn.SetActive(false);
			usvItem.usedText.text = "";
			if(usvItem.mark == 1)
			{
				usvItem.isUseTween.from = 0;
				usvItem.isUseTween.to = 1;
				usvItem.isUseTween.PlayForward();
				usvItem.isUseUnite = true;
			}
			else
			{
				usvItem.isUseTween.from = 1;
				usvItem.isUseTween.to = 0;
				usvItem.isUseTween.PlayForward();
				usvItem.isUseUnite = false;
			}
//            else
//            {
//                if (usvItem.useCardHeroIndex >= 0)
//                {
//                    if (openType != OPENTYPE.E_CARD)
//                    {
//                        PackElement dbc = combination.curCardGroup.cards[usvItem.useCardHeroIndex];
//                        if (dbc != null)
//                        {
//                            int formID = dbc.dataId;
//                            CardData heroData = CardData.getData(formID);
//                            string name = heroData.name;
//                            usvItem.usedText.text = name + TextsData.getData(262).chinese;
//                        }
//                    }
//                    else
//                    {
//                        usvItem.usedText.text = TextsData.getData(263).chinese;
//                    }
//                }
//                else
//                {
//                    if (openType != OPENTYPE.E_CARD)
//                    {
//                        usvItem.usedText.text = TextsData.getData(262).chinese;
//                    }
//                    else
//                    {
//                        usvItem.usedText.text = TextsData.getData(263).chinese;
//                    }
//                }
//            }
            if (usd == null)
                continue;
            usvItem.icon.atlas = unitSkillUIAtlas;
            usvItem.icon.spriteName = usd.icon;
            usvItem.nameText.text = usd.name;

            if (usvItem.mark == 4)
            {
				
                usvItem.descText.text = usd.description;
                usvItem.icon.color = Color.grey;
                usvItem.bg.color = Color.grey;
                usvItem.nameText.color = Color.grey;
                usvItem.descText.color = Color.grey;
				
                usvItem.WayBtn.SetActive(true);
                usvItem.WayBtn.transform.GetComponent<UIButtonMessage>().param = usvItem.index;
                usvItem.WayBtn.transform.GetComponent<UIButtonMessage>().target = this.transform.gameObject;
				usvItem.WayBtn.transform.GetComponent<UIButtonMessage>().functionName = "GetWay";
				
				usvItem.usedText.text = TextsData.getData(546).chinese;
				usvItem.useBtn.gameObject.SetActive(false);
            }
            else
            {
                usvItem.descText.text = usd.description;
				usvItem.useBtn.gameObject.SetActive(true);
            }
			usvItem.needNum.text = usd.cost.ToString();
                  
            usvItem.selectObj.SetActive(false);
            usvItem.obj.name = usvItem.index.ToString();
            usvItemList.Add(usvItem);
		}
		uniteParent.GetComponent<UIGrid>().repositionNow = true;
	}
	
	/*public void cancelAllSelectUnitSkill()
	{
		for(int i = 0;i < usvItemList.Count;++i)
		{
			curCardGroup.changeMark = 1;
			curCardGroup.unitSkillId = 0;
			usvItemList[i].isUseUnite = false;
			TweenAlpha ta = usvItemList[i].useBtn.transform.FindChild("true").GetComponent<TweenAlpha>();
			ta.from = 1;
			ta.to = 0;
			ta.PlayForward();
		}
	}*/
	
	public void OnClickUniteListItemUseBtn(int param)
	{
		        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		UniteScrollViewItem usvItem = null;
		foreach(UniteScrollViewItem usvi in usvItemList)
		{
			if(usvi.index == param)
			{
				usvItem = usvi;
				break;
			}
		}
		if(usvItem !=null)
		{
			if(!usvItem.isUseUnite)
			{
				//如果未选中就选中//
				curCardGroup.changeMark = 1;
        		curCardGroup.unitSkillId = param;
				usvItem.isUseUnite = true;
				for(int i = 0;i<usvItemList.Count;i++)
				{
					if(usvItemList[i].index != param)
					{
						usvItemList[i].isUseUnite = false;
						usvItemList[i].isUseTween.from = 1;
						usvItemList[i].isUseTween.to = 0;
						usvItemList[i].isUseTween.PlayForward();
					}
				}
				usvItem.isUseTween.from = 0;
				usvItem.isUseTween.to = 1;
				usvItem.isUseTween.PlayForward();
			}
			else
			{
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_UseCombo3))
				{
					return;
				}
				//如果已经选中就取消//
				curCardGroup.changeMark = 1;
				curCardGroup.unitSkillId = 0;
				usvItem.isUseUnite = false;
				usvItem.isUseTween.from = 1;
				usvItem.isUseTween.to = 0;
				usvItem.isUseTween.PlayForward();
			}
		}

		
	}
	
	public void ChangePositionData(int sid,int eid)
    {
        curCardGroup.changeMark = 1;
        //获取当前界面的卡组信息//
        CardGroup curGroup = curCardGroup;
        //保存起始位置里面的内容//
        PackElement tempDBCard = curGroup.cards[sid];
        int tempTipMark = curGroup.cardTips[sid];
        PackElement tempDBSkill = curGroup.skills[sid];
        List<PackElement> tempPassiveSkill = curGroup.passiveSkills[sid];
        List<PackElement> tempEquipList = curGroup.equips[sid];

        //将结束位置的内容放到起始位置中//
        curGroup.cards[sid] = curGroup.cards[eid];
        curGroup.cardTips[sid] = curGroup.cardTips[eid];
        curGroup.skills[sid] = curGroup.skills[eid];
        curGroup.passiveSkills[sid] = curGroup.passiveSkills[eid];
        curGroup.equips[sid] = curGroup.equips[eid];

        //将起始位置的内容放到结束位置上//
        curGroup.cards[eid] = tempDBCard;
        curGroup.cardTips[eid] = tempTipMark;
        curGroup.skills[eid] = tempDBSkill;
        curGroup.passiveSkills[eid] = tempPassiveSkill;
        curGroup.equips[eid] = tempEquipList;
    }
	
	//返回主菜单界面//
	private void ChangeUIToComb()
	{
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		hide();
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_UseCombo3))
		{
			GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_UseCombo3);
			GuideUI_UseCombo3.mInstance.hide();
		}
	}
	//返回竞技场界面//
	private void changeUIToArena(){
		//显示竞技场界面//
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA);
		ArenaUIManager arena = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA,"ArenaUIManager") as ArenaUIManager;
        arena.SetData(0, playerArenaData, pkList1, totalPVPReward, totalDekaronNum, pkTime, cards1);
		hide();
	}
	
	//返回大地图界面//
	private void changeUIToMission()
	{
		UISceneStateControl.mInstace.ShowObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI2")as MissionUI2;
		mission2.baseShow();
		hide();
	}
	
	public void hide ()
	{
        if (curBackType == 2)
        {
            GetArena(false);
        }
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
	}
	
	public void baseHide()
	{
		UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
	}
	
	private void gc()
	{
		curCardGroup=null;
        if (pkList != null)
        {
            pkList.Clear();
        }
        pkList = null;
    	zhenrong.gc();
		zhenrong=null;
		ZhenRoot.mInstance.gameObject.SetActive(false);
		if(tftmen != null)
        {
            for (int i = 0; i < tftmen.Length; i++)
            {
                Destroy(tftmen[i].gameObject);
            }
            tftmen = null;
        }
		tftGo=null;
		Resources.UnloadUnusedAssets();
	}
	private void clearData()
	{
		CardInfoPanelManager.teamATK = 0;
		CardInfoPanelManager.teamDEF = 0;
		CardInfoPanelManager.teamHP = 0;
		CardInfoPanelManager.teamATKAdd = 0;
		CardInfoPanelManager.teamDEFAdd = 0;
		CardInfoPanelManager.teamHPAdd = 0;
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
				//再次购买需要vip等级达到num级！//
				string  str = TextsData.getData(241).chinese.Replace("num",ed.viplevel+"");
//				ToastWindow.mInstance.showText(str);
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			if(PlayerInfo.getInstance().player.crystal<ed.cost)
			{
				//您的钻石数量不足！//
				string  str = TextsData.getData(244).chinese;
//				ToastWindow.mInstance.showText(str);
				//提示去充值//
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				return;
			}
			requestType=11;
			PlayerInfo.getInstance().sendRequest(new EnergyJson(),this);
		}
	}
	
	public void warnningCancel(){}
	
	public void receiveResponse(string json)
	{
		Debug.Log("CardGroupResultJson : json : " + json);
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 3:
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
					curCardGroup=cgrj.transformCardGroup();
					PlayerInfo.getInstance().curCardGroup = curCardGroup;
					unitMark = cgrj.unit;
				}

                if (curBackType == 5)
                {
                    return;
                }
                else
                    receiveData = true;
				break;
			case 10:		//请求竞技场界面信息//
				RankResultJson rrj = JsonMapper.ToObject<RankResultJson>(json);
				errorCode = rrj.errorCode;
				if(errorCode == 0)
				{
					playerArenaData = rrj.s;
                    pkList = rrj.ss;
					
                    //cardIds = rrj.cardIds;
					totalDekaronNum = rrj.sPknum;
					totalPVPReward = rrj.sAward;
					pkTime = (int)(rrj.cdtime );
				}
				receiveData = true;
				break;
			case 11:		//==增加怒气上限==//
				PlayerResultJson prj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode=prj.errorCode;
				if(errorCode==0)
				{
					PlayerInfo.getInstance().player=prj.list[0];
				}
				receiveData=true;
				break;
			case 12:  		//==返回mission界面==//
				MapResultJson mj = JsonMapper.ToObject<MapResultJson>(json);
                errorCode = mj.errorCode;
                if (errorCode == 0)
                {
                    mapRJ = mj;
                }
                receiveData = true;
                break;

            case 15:
                EventBattleResultJson ebrj = JsonMapper.ToObject<EventBattleResultJson>(json);
                errorCode = ebrj.errorCode;
                if (errorCode == 0)
                {
                    //设置战斗数据//
                    //				PlayerInfo.getInstance().bNum=ebrj.bNum;
                    PlayerInfo.getInstance().ebrj = ebrj;
                    errorCode = ebrj.errorCode;
                }
                receiveData = true;
                break;
            case 16://返回活动副本（异世界）//
                //				strData.Clear();
                EventResultJson erj = JsonMapper.ToObject<EventResultJson>(json);
                errorCode = erj.errorCode;
                if (errorCode == 0)
                {
                    strData = erj.s;

                    num = erj.num;
                }
                receiveData = true;
                break;
			}
		}
	}
	
	//1,战队天赋  2,合体技
	public void onClickShowTalent(int param)
	{
		if(param == 1)
		{
			if(uniteSkill.activeSelf)
			{
				teamTalent.SetActive(true);
				uniteSkill.SetActive(false);
				setTF();
				listScrollBar.value = 0;
			}
		}
		else if(param == 2)
		{
			if(teamTalent.activeSelf)
			{
				teamTalent.SetActive(false);
				uniteSkill.SetActive(true);
				ShowUniteSkill();
				listScrollBar.value = 0;
			}
		}
	}
	
	public int findCardGroupFirstEmptyPos()
	{
		int pos = -1;
		for(int i = 0 ;i < curCardGroup.cards.Length;++i)
		{
			pos = i;
			if(curCardGroup.cards[i] == null)
			{
				return pos;
			}
		}
		return pos;
	}
	
	public int findCardGroupFirstExistCardPos()
	{
		int pos = -1;
		for(int i = 0; i < curCardGroup.cards.Length;++i)
		{
			pos = i;
			if(curCardGroup.cards[i] != null)
			{
				return pos;
			}
		}
		return pos;
	}
	
}
