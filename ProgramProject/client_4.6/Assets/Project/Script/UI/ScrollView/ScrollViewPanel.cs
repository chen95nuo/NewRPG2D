using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollViewPanel : MonoBehaviour, ProcessResponse
{
    public enum OPENTYPE : int
    {
        E_NULL = -1,
        E_UNITSKILL = 0,
        E_EQUIP = 1,			//防具//		
        E_CARD = 2,			//卡牌//
        E_WEAPON = 3,			//武器//
        E_ORNAMENTS = 4,			//饰品//
        E_ACTIVESKILL = 5,			//主动技能//
		E_PS1 = 6, // passive skill 1
		E_PS2 = 7, // passive skill 2
		E_PS3 = 8, // passive skill 3
    }



//    public static ScrollViewPanel mInstance;
	private Transform _myTransform;

    //ctrls
    GameObject scrollViewParent;
    public GameObject scrollViewCtrl;
    public GameObject gridCtrl;

    // unit skill list item
    string listItemUnitSkillPath = "Prefabs/UI/ScrollViewPanel/ListItemUnitSkill";
    string listItemCardPath = "Prefabs/UI/ScrollViewPanel/ListItemCard";
    string listItemEmptyPath = "Prefabs/UI/ScrollViewPanel/ListItemEmpty";

    public UIAtlas unitSkillUIAtlas;
    public UIAtlas equipAtlas;
    public UIAtlas cardAtlas;
    public UIAtlas skillAtlas;
    public UIAtlas passiveAtlas;

    public GameObject closeBtn;

    GameObject loadPrefab = null;

    List<ScrollViewItem> svItemList = new List<ScrollViewItem>();
    OPENTYPE openType = OPENTYPE.E_NULL;
    int maxCountInView = 5;


    private List<PackElement> datas;
    private List<int> dataIds;//卡组中已存在的dataId//

    bool isMoveShow = false;
    bool isMoving = false;
    float movingTime = 0.6f;
    float nowMovingTime = 0;

    int clickedIndex = -1;

    int selectCardIndex = -1;

    private int toIntensifyPanelCardi = 0;

    public int intensifyType = -1;

    private int errorCode;

    private int requestType;

    private bool receiveData = false;
	
	public List<PackElement> allCells = new List<PackElement>();
	
	public List<PackElement> packItemInfo = new List<PackElement>();
	//背包的json.//
	private PackResultJson packRJ;

	private int prjI = 0;

    bool isMove;
	
	//int openPSCardIndex = -2;
	bool isGoIntensifyOk = false;

    void Awake()
    {
		_myTransform = transform ;
        init();
		
//        close();
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (nowMovingTime >= movingTime)
            {
                nowMovingTime = 0;
                isMoving = false;
                moveFinish();
            }
            else
            {
                nowMovingTime += Time.deltaTime;
            }
        }
        if (receiveData)
        {
            receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
				if(errorCode == 56)
				{
					ToastWindow.mInstance.showText(TextsData.getData(678).chinese);
					return;
				}
				else if (errorCode == 0)
				{
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2,toIntensifyPanelCardi, 1,0), this);
					requestType = 5;
				}
				break;
			case 2:
				if(errorCode == 56)
				{
					ToastWindow.mInstance.showText(TextsData.getData(678).chinese);
					return;
				}
				else if (errorCode == 0)
				{
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2,toIntensifyPanelCardi, 2,0), this);
					requestType = 6;
				}
				break;
			case 3:
				if(errorCode == 56)
				{
					ToastWindow.mInstance.showText(TextsData.getData(678).chinese);
					return;
				}
				else if (errorCode == 0)
				{
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2,toIntensifyPanelCardi, 3,0), this);
					requestType = 7;
				}
				break;
			case 4:
				if(errorCode == 56)
				{
					ToastWindow.mInstance.showText(TextsData.getData(678).chinese);
					return;
				}
				else if (errorCode == 0)
				{
					requestType = 8;
				}
				break;
			}
			TweenPosition position = scrollViewParent.GetComponent<TweenPosition>();
    		EventDelegate.Add (position.onFinished, OnFinished);
			hide();
        }
    }

    public void init()
    {
        nowMovingTime = 0;
        isMoving = false;
		 scrollViewParent = GameObjectUtil.findGameObjectByName(_myTransform.gameObject, "ScrollParent");
		isGoIntensifyOk = false;
    }

    public void show()
    {
        isMoving = true;
        isMoveShow = true;
        scrollViewParent.GetComponent<TweenPosition>().enabled = true;
        scrollViewParent.GetComponent<TweenPosition>().tweenFactor = 1;
        scrollViewParent.GetComponent<TweenPosition>().PlayReverse();
		//显示或收起结束,延时调用黑底点击事件//
		Invoke("setCanClickBlackBg",2f);
    }

    public void hide()
    {
        isMoving = true;
        isMoveShow = false;
        scrollViewParent.GetComponent<TweenPosition>().enabled = true;
        scrollViewParent.GetComponent<TweenPosition>().tweenFactor = 0;
        scrollViewParent.GetComponent<TweenPosition>().PlayForward();
		Invoke("setCanClickBlackBg",2f);
        gc();
    }
	
	public void BaseHide()
	{
        if (requestType == 5&&!isGoIntensifyOk)
            OnFinished();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW);
	}
	
	public void setCanClickBlackBg()
	{
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		if(cardInfo != null)
			cardInfo.setCanClickBlackBg();
	}
		
    private void gc()
    {
        //清除数据//
        if (datas != null)
        {
            datas.Clear();
        }
        datas = null;
        if (dataIds != null)
        {
            dataIds.Clear();
        }
        dataIds = null;
        dataIds = null;
        loadPrefab = null;
		unitSkillUIAtlas = null;
		equipAtlas = null;
		cardAtlas = null;
		skillAtlas = null;
		passiveAtlas = null;
        svItemList.Clear();

        Resources.UnloadUnusedAssets();
    }
	public void OnFinished()
	{
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY);
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, "IntensifyPanel")as IntensifyPanel;
		intensify.isScrollViewCome = true;
		MissionUI missionUi = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI") as MissionUI;
		
		intensify.sortItemList = packItemInfo;
		intensify.selectCardSkillID = prjI;
		
		intensify.targetData = intensify.getSortItem(toIntensifyPanelCardi);
		intensify.sortItemList.Clear();
		intensify.sortItemList = allCells;
        if (packRJ == null)
        {
            return;
        }
		intensify.allFromIdList = packRJ.pejs;
        switch (requestType)
        {
        case 5:
            intensify.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_HERO);
			PackElement dbc = intensify.targetData;
			if (dbc != null)
        	{
                intensify.oldExp = dbc.curExp;
                intensify.oldLevel = dbc.lv;
        	}
            break;
        case 6:
            intensify.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_SKILL);
			PackElement dbs = intensify.targetData;
			if (dbs != null)
        	{
                intensify.oldExp = dbs.curExp;
                intensify.oldLevel = dbs.lv;
        	}
			
            break;
        case 7:
            intensify.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_PASSIVESKILL);
			dbs = intensify.targetData;
			if (dbs != null)
        	{
                intensify.oldExp = dbs.curExp;
                intensify.oldLevel = PassiveSkillData.getData(dbs.dataId).level;
        	}
            break;
        case 8:
            intensify.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_EQUIP);
            if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
			{
				GuideUI_IntensifyEquip.mInstance.showStep(5);
			}
            break;
        }
		cardInfo.hide();
		combination.hide();
		if(missionUi != null)
		{
			missionUi.hide();
		}
		intensify.showPage(IntensifyPanel.PAGETYPE.PAGE_INTENSIFY);
		intensify.allCells = null;
		intensify.notSetExp = false;
		
		if (GuideManager.getInstance().isRunningGuideID(((int)GuideManager.GuideType.E_IntensifyCard)))
        {
            if (intensify.isGuideSelectTargetCard && intensify.sortItemList.Count > 0)
            {
                intensify.guideConsumeCard = intensify.sortItemList[0];
                intensify.isGuideSelectTargetCard = false;
                GuideUI_Intesnify.mInstance.showStep(3);
            }
            if (intensify.isGuideIntensify)
            {
                intensify.isGuideIntensify = false;
                GuideUI_Intesnify.mInstance.showStep(5);
            }
        }
		
		isGoIntensifyOk = true;
		
		HeadUI.mInstance.show();
		packRJ = null;
	}
    public void onClickWayBtn(int param)
    {
        CombinationInterManager c = this.transform.parent.FindChild("CombinationPanel(Clone)").GetComponent<CombinationInterManager>();
        c.GetWay(param);
    }
    public void moveFinish()
    {
        if (!isMoveShow)
        {
            GameObjectUtil.destroyGameObjectAllChildrens(gridCtrl);
            svItemList.Clear();
            scrollViewCtrl.transform.localPosition = Vector3.zero;
            scrollViewCtrl.GetComponent<UIPanel>().clipRange = new Vector4(0, 0, 380, 540);
            //隐藏卡牌详细信息界面的黑框//
            if (openType != OPENTYPE.E_UNITSKILL)
            {
				CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
				if(cardInfo!= null)
				{
					
					cardInfo.CloseScrollBlackBg();
				}
            }
			BaseHide();
        }
        else
        {
            if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
            {
                GuideUI_CardInTeam.mInstance.showStep(3);
            }
			else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
			{
				GuideUI_CardInTeam2.mInstance.showStep(3);
			}
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip))
            {
                GuideUI12_Equip.mInstance.showStep(5);
            }
            else if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
            {
                GuideUI18_Skill.mInstance.showStep(8);
            }
			else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
			{
				GuideUI_IntensifyEquip.mInstance.showStep(1);
			}
        }
    }

    // must set open type first
    public void setOpenType(OPENTYPE ot)
    {
        openType = ot;
        switch (openType)
        {
        case OPENTYPE.E_UNITSKILL:
        {
            loadPrefab = Resources.Load(listItemUnitSkillPath) as GameObject;
        } break;
        case OPENTYPE.E_EQUIP:
        case OPENTYPE.E_CARD:
        case OPENTYPE.E_WEAPON:
        case OPENTYPE.E_ORNAMENTS:
        case OPENTYPE.E_ACTIVESKILL:
        case OPENTYPE.E_PS1:
		case OPENTYPE.E_PS2:
		case OPENTYPE.E_PS3:
        {
            loadPrefab = Resources.Load(listItemCardPath) as GameObject;
        } break;
		}
    }

    // set list item info
    public void setOpenTypeItemList(List<string> list, int selectID,int psCardIndex = -1)
    {
		
		//openPSCardIndex = -2;
		
        GameObjectUtil.destroyGameObjectAllChildrens(gridCtrl);
        svItemList.Clear();
        scrollViewCtrl.transform.localPosition = Vector3.zero;
        scrollViewCtrl.GetComponent<UIPanel>().clipRange = new Vector4(0, 0, 380, 540);
		
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,
						"CombinationInterManager")as CombinationInterManager;
		
        foreach (string str in list)
        {
            GameObject listItemObj = (GameObject)GameObject.Instantiate(loadPrefab);
            if (listItemObj == null)
                continue;
            ScrollViewItem svItem = listItemObj.GetComponent<ScrollViewItem>();
            if (svItem == null)
                continue;
            GameObjectUtil.gameObjectAttachToParent(listItemObj, gridCtrl);

            string[] ss = str.Split('-');
            //int index = -1;
            if (ss[0].StartsWith("%"))
            {
                svItem.baseSkillID = StringUtil.getInt(ss[0].Substring(1));
                svItem.isBaseSkill = true;
            }
            else
            {
                svItem.index = StringUtil.getInt(ss[0]);
            }
            svItem.bm.param = svItem.index;
            svItem.bm.target = _myTransform.gameObject;
            svItem.bm.functionName = "onClickListItemUseBtn";

            svItem.ibm.param = svItem.index;
            svItem.ibm.target = _myTransform.gameObject;
            svItem.ibm.functionName = "onClickListItemIntensifyBtn";

            svItem.mark = StringUtil.getInt(ss[1]);
            string heroIndexString = ss[2];
			svItem.useCurPassiveSkill = false;
            if (heroIndexString != "null")
			{
				if(heroIndexString == "cur")
				{
					svItem.useCurPassiveSkill = true;
					svItem.useCardHeroIndex = psCardIndex;
					//openPSCardIndex = psCardIndex;
				}
				else
				{
					svItem.useCardHeroIndex = StringUtil.getInt(heroIndexString);
				}
			}
            else
            {
                svItem.useCardHeroIndex = -1;
            }

            if (openType != OPENTYPE.E_UNITSKILL)
            {
                if (svItem.index == selectID)
                {
					if(openType == OPENTYPE.E_PS1 || openType == OPENTYPE.E_PS2 || openType == OPENTYPE.E_PS3 )
					{
						if(svItem.useCurPassiveSkill)
						{
							svItem.isCanUnEquiped = true;
		                    svItem.btnText.text = TextsData.getData(155).chinese;
		                    svItem.useBtn.SetActive(true);
						}
						else
						{
							svItem.isCanUnEquiped = false;
							svItem.btnText.text = TextsData.getData(154).chinese;
							svItem.useBtn.SetActive(false);
                			svItem.intensifyBtn.SetActive(false);
						}
					}
					else
					{
						svItem.isCanUnEquiped = true;
	                    svItem.btnText.text = TextsData.getData(155).chinese;
	                    svItem.useBtn.SetActive(true);
					}
                    if (openType == OPENTYPE.E_CARD)
                    {
                        svItem.intensifyBtn.SetActive(false);
                    }
                }
                else
                {
                    svItem.isCanUnEquiped = false;
                    switch (openType)
                    {
                        case OPENTYPE.E_UNITSKILL:
                            svItem.btnText.text = TextsData.getData(154).chinese;
                            break;
                        case OPENTYPE.E_EQUIP:
                            svItem.btnText.text = TextsData.getData(649).chinese;
                            break;
                        case OPENTYPE.E_CARD:
                            svItem.intensifyBtn.SetActive(false);
                            svItem.btnText.text = TextsData.getData(648).chinese;
                            break;
                        case OPENTYPE.E_WEAPON:
                            svItem.btnText.text = TextsData.getData(649).chinese;
                            break;
                        case OPENTYPE.E_ORNAMENTS:
                            svItem.btnText.text = TextsData.getData(649).chinese;
                            break;
                        case OPENTYPE.E_ACTIVESKILL:
                            svItem.btnText.text = TextsData.getData(154).chinese;
                            break;
                        case OPENTYPE.E_PS1:
						case OPENTYPE.E_PS2:
						case OPENTYPE.E_PS3:
                            svItem.btnText.text = TextsData.getData(154).chinese;
                            break;
                    }
                   
                    svItem.useBtn.SetActive(false);
                    svItem.intensifyBtn.SetActive(false);
                }
            }
            else
            {
                svItem.btnText.text = TextsData.getData(154).chinese;
                svItem.useBtn.SetActive(false);
                svItem.intensifyBtn.SetActive(false);

            }

            if (svItem.mark != 1)
            {
                svItem.usedText.text = "";
            }
            else
            {
				// 被动技能特殊处理：被动技能这里自身装备显示为有useCardHeroIndex，用于自身被动技能换位置的操作 //
                if (svItem.useCardHeroIndex >= 0)
                {
                    if (openType != OPENTYPE.E_CARD)
                    {
						if(svItem.useCardHeroIndex == psCardIndex)
						{
							svItem.usedText.text = TextsData.getData(262).chinese;
						}
						else
						{
							PackElement dbc = combination.curCardGroup.cards[svItem.useCardHeroIndex];
	                        if (dbc != null)
	                        {
	                            int formID = dbc.dataId;
	                            CardData heroData = CardData.getData(formID);
	                            string name = heroData.name;
	                            svItem.usedText.text = name + TextsData.getData(262).chinese;
	                        }
						}
                    }
                    else
                    {
                        svItem.usedText.text = TextsData.getData(263).chinese;
                    }
                }
                else
                {
                    if (openType != OPENTYPE.E_CARD)
                    {
                        svItem.usedText.text = TextsData.getData(262).chinese;
                    }
                    else
                    {
                        svItem.usedText.text = TextsData.getData(263).chinese;
                    }
                }
            }
            if (svItem.isBaseSkill)
            {
                svItem.useBtn.SetActive(false);
                svItem.intensifyBtn.SetActive(false);
                
            }
            switch (openType)
            {
                case OPENTYPE.E_UNITSKILL:
                    {
                        UnitSkillData usd = UnitSkillData.getData(svItem.index);
                        if (usd == null)
                            continue;
                        svItem.icon.atlas = unitSkillUIAtlas;
                        svItem.icon.spriteName = usd.icon;
                        svItem.nameText.text = usd.name;

                        if (svItem.mark == 4)
                        {
                            svItem.descText.text = usd.description;
                            svItem.icon.color = Color.grey;
                            svItem.bg.color = Color.grey;
                            svItem.nameText.color = Color.grey;
                            svItem.descText.color = Color.grey;

                            svItem.WayBtn.SetActive(true);
                            svItem.usedText.text = TextsData.getData(546).chinese;
                            svItem.WayBtn.transform.GetComponent<UIButtonMessage>().param = svItem.index;
                            svItem.WayBtn.transform.GetComponent<UIButtonMessage>().target = this.transform.gameObject;
                        }
                        else
                        {
                            svItem.descText.text = usd.description;
                        }
                    } break;
                //防具//
                case OPENTYPE.E_EQUIP:
                case OPENTYPE.E_WEAPON:
                case OPENTYPE.E_ORNAMENTS:
                    {
                        intensifyType = 3;

                        PackElement dbe = datas[svItem.index];

                        if (dbe.lv >= PlayerInfo.getInstance().player.level * 3)
                        {
                            svItem.intensifyBtn.SetActive(false);
                        }

                        if (dbe != null)
                        {
                            EquipData ed = EquipData.getData(dbe.dataId);
                            if (ed != null)
                            {
                                svItem.nameText.text = ed.name;

                                svItem.descText.text = ed.description + Statics.getEquipValueForUIShow(dbe.dataId, dbe.lv);
                                if (svItem.mark == 4)
                                {
                                    svItem.warnningText.SetActive(true);
                                }
                                else
                                {
                                    svItem.warnningText.SetActive(false);
                                }
                                //icon//
                        		svItem.cardInfo.setSimpleCardInfo(dbe.dataId,GameHelper.E_CardType.E_Equip);        
						
                                //race//
                                showRaceCtrl(9, svItem.race);
                                //level//
                                svItem.levelText.text = "LV." + dbe.lv;
                            }
                        }

                    } break;
                //卡牌//
                case OPENTYPE.E_CARD:
                    {
                        intensifyType = 0;

                        PackElement dbc = datas[svItem.index];

                        CardData cd = CardData.getData(dbc.dataId);

                        int maxLevel = 0;

                        if (cd != null)
                        {

                            if (dbc.bn > 0)
                            {
                                maxLevel = cd.maxLevel + EvolutionData.getData(cd.star, dbc.bn).lvl;
                            }
                            else
                            {
                                maxLevel = cd.maxLevel;
                            }
                        }

                        if (dbc.lv > maxLevel)
                        {
                            svItem.intensifyBtn.SetActive(false);
                        }

                        if (dbc != null)
                        {
                            cd = CardData.getData(dbc.dataId);
                            if (cd != null)
                            {
                                svItem.nameText.text = GameHelper.getCardName(dbc);
                                int HP = 0;
                                int ATK = 0;
                                int DEF = 0;
                                HP = (int)Statics.getCardSelfMaxHpForUI(dbc.dataId, dbc.lv, dbc.bn);
                                ATK = (int)Statics.getCardSelfMaxAtkForUI(dbc.dataId, dbc.lv, dbc.bn);
                                DEF = (int)Statics.getCardSelfMaxDefForUI(dbc.dataId, dbc.lv, dbc.bn);

                                svItem.descText.text = TextsData.getData(261).chinese + " :  " + HP.ToString() + "\n" + TextsData.getData(259).chinese + " :  " + ATK.ToString() + "\n" + TextsData.getData(260).chinese + " :  " + DEF.ToString();
                                if (svItem.mark == 4)
                                {
                                    svItem.warnningText.SetActive(true);
                                }
                                else
                                {
                                    svItem.warnningText.SetActive(false);
                                }
                                //icon//
								svItem.cardInfo.setSimpleCardInfo(dbc.dataId,GameHelper.E_CardType.E_Hero);
                                
								//race//
                                showRaceCtrl(cd.race, svItem.race);
                                //level//
                                svItem.levelText.text = "LV." + dbc.lv;
                            }
                        }

                    } break;

                //主动技能//
                case OPENTYPE.E_ACTIVESKILL:
                    {
                        intensifyType = 1;
				
                        if (!svItem.isBaseSkill)
                        {
							PackElement dbs = datas[svItem.index];
                            
							if (dbs != null)
                            {
                                SkillData sd = SkillData.getData(dbs.dataId);
                                if (sd != null)
                                {
                                    svItem.nameText.text = sd.name;
                                    svItem.descText.text = sd.description + Statics.getSkillValueForUIShow02(dbs.dataId, dbs.lv);
                                    if (svItem.mark == 4)
                                    {
                                        svItem.warnningText.SetActive(true);
                                    }
                                    else
                                    {
                                        svItem.warnningText.SetActive(false);
                                    }
                                    //icon//
									svItem.cardInfo.setSimpleCardInfo(dbs.dataId,GameHelper.E_CardType.E_Skill);
                                    //race//
                                    showRaceCtrl(10, svItem.race);
                                    //level//
                                    svItem.levelText.text = "LV." + dbs.lv;
									//int j = PlayerInfo.getInstance().player.level;
									if (dbs.lv >= PlayerInfo.getInstance().player.level || dbs.lv > 100)
                         	   		{
                        	        	svItem.intensifyBtn.SetActive(false);
                        	    	}
                                }
                            }
                        }
                        else
                        {
                            SkillData sd = SkillData.getData(svItem.baseSkillID);
                            if (sd != null)
                            {
                                string nameStr = sd.name + "(" + TextsData.getData(319).chinese + ")";
                                svItem.nameText.text = nameStr;

                                svItem.descText.text = sd.description + Statics.getSkillValueForUIShow02(svItem.baseSkillID, 1);
                                svItem.warnningText.SetActive(false);
                                //icon//
								svItem.cardInfo.setSimpleCardInfo(svItem.baseSkillID,GameHelper.E_CardType.E_Skill);
                                //race//
                                showRaceCtrl(10, svItem.race);
                                //level//
                                svItem.levelText.text = "LV.1";
                            }
                        }

                    } break;
                //被动技能//
                case OPENTYPE.E_PS1:
				case OPENTYPE.E_PS2:
				case OPENTYPE.E_PS3:
                {
                    intensifyType = 2;
					PackElement dbps = datas[svItem.index];
					if (dbps != null)
                    {
                        PassiveSkillData psd = PassiveSkillData.getData(dbps.dataId);

                    	if (psd.level >= 10)
                   		{
                        	svItem.intensifyBtn.SetActive(false);
                   		}
					
                        if (psd != null)
                        {
                            svItem.nameText.text = psd.name;
                            svItem.descText.text = psd.describe;
                            if (svItem.mark == 4)
                            {
                                svItem.warnningText.SetActive(true);
                            }
                            else
                            {
                                svItem.warnningText.SetActive(false);
                            }
                            //icon//
                           svItem.cardInfo.setSimpleCardInfo(dbps.dataId,GameHelper.E_CardType.E_PassiveSkill);
                            //race//
                            showRaceCtrl(11, svItem.race);
                            //level//
                            svItem.levelText.text = "LV." + psd.level;
                        }
                    }
                    } break;
            }
            svItem.selectObj.SetActive(false);
            svItem.obj.name = svItem.index.ToString();
            svItemList.Add(svItem);
        }
        setSelectIitemByID(selectID);
        addEmptyItem();
    }

    // set select item id and display from other panel
    public void setSelectIitemByID(int index)
    {
        if (index == -1)
            return;
        for (int i = 0; i < svItemList.Count; ++i)
        {
            ScrollViewItem svItem = svItemList[i];
            if (svItem.index == index && !svItem.isBaseSkill)
            {
				if(openType == OPENTYPE.E_PS1 || openType == OPENTYPE.E_PS2 || openType == OPENTYPE.E_PS3)
				{
					if(svItem.useCurPassiveSkill)
					{
						svItem.selectObj.SetActive(true);
					}
				}
				else
				{
					svItem.selectObj.SetActive(true);
                }
                return;
            }
        }
    }


    public void addEmptyItem()
    {
        // add empty item for display
        int count = svItemList.Count;
		bool needShowInfoLabel = false;
		if(count == 0)
		{
			needShowInfoLabel = true;
		}
        if (count < maxCountInView)
        {
            GameObject emptyPrefab = Resources.Load(listItemEmptyPath) as GameObject;
            for (int i = 0; i < maxCountInView - count; ++i)
            {
				GameObject obj = GameObject.Instantiate(emptyPrefab) as GameObject;
				if(i == 0 && needShowInfoLabel)
				{
					if((openType == OPENTYPE.E_EQUIP) || (openType == OPENTYPE.E_WEAPON) || (openType == OPENTYPE.E_ORNAMENTS))
					{
						obj.transform.FindChild("InfoLabel").gameObject.GetComponent<UILabel>().text = TextsData.getData(654).chinese;
					}
					else if((openType == OPENTYPE.E_PS1) || (openType == OPENTYPE.E_PS2) || (openType == OPENTYPE.E_PS3) )
					{
						obj.transform.FindChild("InfoLabel").gameObject.GetComponent<UILabel>().text = TextsData.getData(653).chinese;
					}
				}
				else
				{
					obj.transform.FindChild("InfoLabel").GetComponent<UILabel>().text = "";
				}
                
                GameObjectUtil.gameObjectAttachToParent(obj, gridCtrl);
            }
        }
        gridCtrl.GetComponent<UIGrid>().repositionNow = true;
    }

    public void clickScrollViewItem(int index)
    {
        if (clickedIndex == index)
        {
            return;
        }

        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
        for (int i = 0; i < svItemList.Count; ++i)
        {
            ScrollViewItem svItem = svItemList[i];
            if (svItem.index == index)
            {
                svItem.selectObj.SetActive(true);
				PackElement dbc = new PackElement();
				//CardData cd = new CardData();
				if(openType!=OPENTYPE.E_UNITSKILL)
				{
	                dbc = datas[svItem.index];
	                //cd = CardData.getData(dbc.dataId);
				}
           		//int maxLevel = 0;

	            //if (cd != null)
	            //{
	            //    if (dbc.bn > 0)
	            //    {
	            //        maxLevel = cd.maxLevel + EvolutionData.getData(cd.star, dbc.bn).lvl;
	            //    }
	            //    else
	            //    {
	            //        maxLevel = cd.maxLevel;
	            //    }
	            //}

	            switch (openType)
	            {
	            case OPENTYPE.E_UNITSKILL:
	            {
	                if (svItem.mark == 2)
	                {
	                    {
	                        svItem.useBtn.SetActive(true);
	                        svItem.intensifyBtn.SetActive(false);
	                    }
	                }
	            } break;
	            case OPENTYPE.E_EQUIP:
	            case OPENTYPE.E_CARD:
	            case OPENTYPE.E_WEAPON:
	            case OPENTYPE.E_ORNAMENTS:
	            case OPENTYPE.E_ACTIVESKILL:
	            case OPENTYPE.E_PS1:
				case OPENTYPE.E_PS2:
				case OPENTYPE.E_PS3:
	            {
	                if (svItem.isBaseSkill)
	                {
	                    svItem.useBtn.SetActive(false);
	
	                    svItem.intensifyBtn.SetActive(false);
	                }
	                else
	                {
	                    svItem.useBtn.SetActive(true);
	                }
	            } break;
	            }
		        switch (openType)
		        {
		            //1,//防具//2,//卡牌//3,//武器//4,//饰品//5,//主动技能//6,//被动技能//
		            case OPENTYPE.E_EQUIP:
		            case OPENTYPE.E_WEAPON:
		            case OPENTYPE.E_ORNAMENTS:
		                if (dbc.lv < PlayerInfo.getInstance().player.level * 3)
		                {
		                    svItem.intensifyBtn.SetActive(true);
		                }
		                break;
		            case OPENTYPE.E_CARD:
                        svItem.intensifyBtn.SetActive(false);
                        //if (dbc.lv < maxLevel)
                        //{
                        //    svItem.intensifyBtn.SetActive(false);
                        //}
		                break;
		            case OPENTYPE.E_ACTIVESKILL:
		                if (dbc.lv < PlayerInfo.getInstance().player.level && dbc.lv < 100 && !svItem.isBaseSkill)
		                {
							//int j = PlayerInfo.getInstance().player.level;
		                    svItem.intensifyBtn.SetActive(true);
		                }
		                break;
		            case OPENTYPE.E_PS1:
					case OPENTYPE.E_PS2:
					case OPENTYPE.E_PS3:
						
						PassiveSkillData psd = PassiveSkillData.getData(dbc.dataId);
		                if (psd.level < 10)
		                {
		                    svItem.intensifyBtn.SetActive(true);
		                }
		                break;
		        }
            }
            else
            {
                svItem.selectObj.SetActive(false);
                svItem.useBtn.SetActive(false);
                svItem.intensifyBtn.SetActive(false);

            }
        }
    }

    public void onClickListItemIntensifyBtn(int param)
    {
        //点击强化按钮,跳转到该物品的强化界面//
		PackElement dbps = datas[param];

        toIntensifyPanelCardi = dbps.i;
		
		switch (intensifyType)
        {
            case 0:
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,1),this);
                requestType = 1;
                break;
            case 1:
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,2),this);
                requestType = 2;
                break;
            case 2:
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,3),this);
                requestType = 3;
                break;
            case 3:
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,4),this);
                requestType = 4;
                break;
        }
    }

    public void onClickListItemUseBtn(int param)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);

        bool isCanUnEquiped = false;
        int useCardHeroIndex = -1;
        for (int i = 0; i < svItemList.Count; ++i)
        {
            ScrollViewItem svItem = svItemList[i];
            if (svItem.index == param)
            {
                useCardHeroIndex = svItem.useCardHeroIndex;
                isCanUnEquiped = svItem.isCanUnEquiped;
                break;
            }
        }
		
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,"CombinationInterManager")as CombinationInterManager;
		
        /*  to cxl param 和 useCardHeroIndex PlayerInfo中对象所在的数据库中Index */
        switch (openType)
        {
            case OPENTYPE.E_CARD:
                {
                    if (isCanUnEquiped)
                    {
						  cardInfo.RemoveItemFromCard(param, selectCardIndex);
                    }
                    else
                    {
                        if (useCardHeroIndex == -1)
                        {
                            PackElement dbc = datas[param];
                            if (dataIds.Contains(dbc.dataId))
                            {
								string str =  TextsData.getData(47).chinese + CardData.getData(dbc.dataId).name;
								ToastWindow.mInstance.showText(str);
						
                                return;
                            }
							  cardInfo.ReplaceCardData(param);
                        }
                        else
                        {
							  cardInfo.ExchangeCard(param);
                        }
                    }
                } break;
            case OPENTYPE.E_EQUIP:
            case OPENTYPE.E_WEAPON:
            case OPENTYPE.E_ORNAMENTS:
            case OPENTYPE.E_ACTIVESKILL:
            case OPENTYPE.E_PS1:
			case OPENTYPE.E_PS2:
			case OPENTYPE.E_PS3:
                {
                    if (isCanUnEquiped)
                    {
						cardInfo.RemoveItemFromCard(param, selectCardIndex);
                    }
                    else
                    {
                        if (useCardHeroIndex == -1)
                        {
							  cardInfo.ReplaceCardData(param);
                        }
                        else
                        {
                            PackElement dbc = combination.curCardGroup.cards[useCardHeroIndex];
                            if (dbc != null)
                            {
								  cardInfo.ReplaceCardData02(param, useCardHeroIndex);
                            }
                        }
                    }

                } break;
            case OPENTYPE.E_UNITSKILL:
                combination.curCardGroup.changeMark = 1;
                combination.curCardGroup.unitSkillId = param;
                combination.show();
                BlackBgUI.mInstance.OnClickBtn(1);
                break;
        }
        hide();
    }

    public void close()
    {
        if (isMoving)
            return;
        BlackBgUI.mInstance.OnClickBtn(1);
        hide();
    }

    /* cardIndex : 所选择的装备，技能对应的卡牌index */
    public void setDatas(List<PackElement> datas, List<int> dataIds, int cardIndex = -1)
    {
        this.datas = datas;
        this.dataIds = dataIds;
        selectCardIndex = cardIndex;
    }

    //list 的格式是id-标记-所在的位置的id//
    /*传过来的 List<string>的格式改为 PlayerInfo中对象所在的数据库中Index － mark － 卡组中位置信息  */
    /*selectID : 所选中的卡牌，装备，技能等的Index*/
	/* psCardIndex : 被动技能的相应卡牌索引*/
    public void openScrollViewPanel(OPENTYPE ot, List<string> list, int selectID,int psCardIndex = -1)
    {
        setOpenType(ot);
        show();
        setOpenTypeItemList(list, selectID,psCardIndex);

    }

    void showRaceCtrl(int race, UISprite raceSpr)
    {
        int num = 0;
        if (race >= 5 && race <= 8)
        {
            num = 5;
        }
        else
        {
            num = race;
        }
        raceSpr.spriteName = "race_" + num;
    }

    public int finidGuideUITargetIndex(int id)
    {
        for (int i = 0; i < datas.Count; ++i)
        {
            if (datas[i].dataId == id)
            {
                return i;
            }
        }
        return -1;
    }
	
	public void OnShowFinished()
	{
		Debug.Log("1231212312312312312312312312312313123123123123123123");
	}
	
    public void receiveResponse(string json)
    {
        Debug.Log("json ====== " + json);
        //关闭连接界面的动画//
        PlayerInfo.getInstance().isShowConnectObj = false;
        if (json != null)
        {
            switch (requestType)
            {
            case 1:  //英雄//
			case 2: //主动技能//
			case 3:
			case 4:
			{
				packRJ = JsonMapper.ToObject<PackResultJson>(json);
				errorCode = packRJ.errorCode;
				if(errorCode == 0)
				{
					packItemInfo.Clear();
					for(int i = 0;i<packRJ.pejs.Count;i++)
					{
						packItemInfo.Add(packRJ.pejs[i].pe);
					}	
				}
				receiveData = true;
			}break;
			case 5: //具体强化界面//
			case 6:
			case 7:
				SelectCardResultJson scrj = JsonMapper.ToObject<SelectCardResultJson>(json);
                errorCode = scrj.errorCode;
				allCells.Clear();
                if (errorCode == 0)
                {
					allCells = scrj.pes;
					prjI = scrj.i;
                }
				receiveData = true;
                break;
            }
        }

    }
	
	public bool getIsMoving()
	{
		return isMoving;
	}
}
