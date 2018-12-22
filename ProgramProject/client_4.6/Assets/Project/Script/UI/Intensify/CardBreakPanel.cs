using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardBreakPackInfo
{
	public SimpleCardInfo1 sInfo;
	public PackElement data;
	public GameObject selectObj;
	public GameObject canBreakObj;
	public void clear()
	{
		sInfo.clear();
		selectObj.SetActive(false);
		canBreakObj.SetActive(false);
	}
}

public class CardBreakPanel : MonoBehaviour ,ProcessResponse
{
//	public static CardBreakPanel mIntance = null;
	
	public enum PAGETYPE : int
	{
		PAGE_PACK = 0,
		PAGE_BREAKCTRL = 1,
	}
	PAGETYPE mPageType;
	
	// bag ctrl
	public GameObject bagCtrl;
	// pack grid list
	public GameObject packGridListCtrl;
	public GameObject packWindowScrollPanel;
	public UIScrollBar packWindowScrollBar;

	// break ctrl
	public GameObject breakCtrl;

    public GameObject popCardDetail;

    public GameObject popOtherDetail;
	public NewExpManager breakExp;
    public UILabel card2DNameLabel;
	public SimpleCardInfo2 card2DInfo;
	public UILabel c2DHeroATKLabel;
	public UILabel c2DHeroDEFLabel;
	public UILabel c2DHeroHPLabel;
	public UILabel maxLevelText;
	public UILabel attrValueText;
	public UILabel costValueText;
	// pop wnd
	public GameObject popWnd;
	public UILabel popText1;
	public UILabel popText2;
	
	public GameObject consumeListCtrl;
	public GameObject consumeListPanelCtrl;
	public GameObject consumeListGrid;
	public UIScrollBar consumeListScrollBar;
	
	public List<PackElement> sortItemList;
	// pack grid gameobejct list
	List<CardBreakPackInfo> packGridList = new List<CardBreakPackInfo>();
	List<CardBreakPackInfo> consumeGirdList = new List<CardBreakPackInfo>();
	List<PackElement> consumeDataList = new List<PackElement>();
	PackElement targetData = null;
	

	bool notSetExp = false;
	int  oldExp = 0;
	int  oldLevel = 0;
	int moneyCost = 0;
	

	/**1 select target card,2 show select consume card UI,3 break**/
	private int requestType;
	private bool receiveData;
	
	private Transform _myTransform;
	
	// break card prefab
	string packCardPrefabPath = "Prefabs/UI/CardBreakPanel/CardBreakPackCard";
	GameObject breakPackCardPerfab;
	
	//bool needShowResurlt = false;
	bool needRequestPlayerInfo =false;
	bool doRequestPlayerInfo =false;
	
	//int selectCardSkillID = 0;
	
	public GameObject levelUpEffectNode;
	string expUpgradeEffectPath ="Prefabs/Effects/UIEffect/qianghua_jingyantiao";
	GameObject expUpgradeEffectPrefab = null;
	
	BreakResultJson brj;

    public GameObject lockHints;

    public int mark;


    int errorCode;
     
    CardInfoResultJson cirj;

    int diamonds;

    int cardN;

    int diamond;

    int pcardN;

    int multCard;

    List<PackElement> cards = new List<PackElement>();

    public UILabel diamondLbaelDiamond;

    public UILabel diamondLbaelCardNum;

    public UILabel diamondLbaelMultCardNum;

    public GameObject[] BreakLabels;

    public UISprite iconCards;

    public GameObject labels;

    public UILabel hint;

    private BreakResultJson breakJson;

    public GameObject breakButtons;

    public UISprite psSkillIcon;

    public GameObject[] icons;

    public List<PackElement> breakCardList = new List<PackElement>();

    PackElement card;


    public SpreadManager spread;

	void Awake()
	{
		_myTransform = transform;
		init();
	}
	
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(receiveData)
		{
			receiveData=false;
            if (cirj.errorCode == -3)
				return;
			
			switch(requestType)
			{
			case 1:
			{
				if(brj.errorCode == 0)
				{
                    if (type == 1)
                    {
                        popCardDetail.SetActive(false);
                        bagCtrl.SetActive(true);
                        type = 0;
                    }
					targetData = null;
					consumeDataList.Clear();
					sortItemList = brj.pes;
					showPage(PAGETYPE.PAGE_PACK);
					
				}
			}break;
			case 2:
			{
				if(brj.errorCode == 0)
				{
					sortItemList = brj.pes;
					showPage(PAGETYPE.PAGE_BREAKCTRL);
					notSetExp = false;
					
				}
			}break;
			case 3:
			{
				if(brj.errorCode == 0)
				{
					//needShowResurlt = true;
					int lastBreakNum = oldLevel;
					//targetData = brj.pe;
					int nowBreakNum = targetData.bn;
					consumeDataList.Clear();
					
					if(targetData != null)
					{
						showPage(PAGETYPE.PAGE_BREAKCTRL);
						oldExp = targetData.breakType;
						oldLevel = targetData.bn;
					}
					notSetExp = true;
					CardData cd = CardData.getData(targetData.dataId);
					if(cd != null)
					{
						string talentStr1 = "";
						string talentStr2 = "";
						TalentData td1 =TalentData.getData(cd.talent2);
						TalentData td2 =TalentData.getData(cd.talent3);
						if(td1!=null)
						{
							talentStr1 = TalentData.getData(cd.talent2).name;
						}
						else
						{
							talentStr1 = string.Empty;
						}
						if(td2!=null)
						{
							talentStr2 = TalentData.getData(cd.talent3).name;
						}
						else
						{
							talentStr2 = string.Empty;
						}
						if(lastBreakNum != nowBreakNum)
						{
							popWnd.SetActive(true);
							if(nowBreakNum < Constant.MaxBreakNum1)
							{
								if(!talentStr1.Equals(""))
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString())+
										TextsData.getData(564).chinese.Replace("num",Constant.MaxBreakNum1.ToString())+talentStr1;	
								}
								else if(!talentStr2.Equals(""))
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString())+
										TextsData.getData(564).chinese.Replace("num",Constant.MaxBreakNum2.ToString())+talentStr2;	
								}
								else
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString());
								}
							}
							else if(nowBreakNum == Constant.MaxBreakNum1)
							{
								if(!talentStr1.Equals(""))
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString())+
										TextsData.getData(565).chinese+talentStr1;	
								}
								else if(!talentStr2.Equals(""))
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString())+
										TextsData.getData(564).chinese.Replace("num",Constant.MaxBreakNum2.ToString())+talentStr2;	
								}
								else
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString());
								}
							}
							else if(nowBreakNum<Constant.MaxBreakNum2)
							{
								if(!talentStr2.Equals(""))
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString())+
										TextsData.getData(564).chinese.Replace("num",Constant.MaxBreakNum2.ToString())+talentStr2;	
								}
								else
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString());
								}
							}
							else if(nowBreakNum == Constant.MaxBreakNum2)
							{
								if(!talentStr2.Equals(""))
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString())+
										TextsData.getData(565).chinese+talentStr2;	
								}
								else
								{
									popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num",nowBreakNum.ToString());
								}
							}
						}
					}
					
					//needShowResurlt = false;
					requestType = 2;
					UIJson uiJson = new UIJson();
					uiJson.UIJsonForBreak(STATE.UI_CardBreak1,targetData.i);
					PlayerInfo.getInstance().sendRequest(uiJson ,this);
					needRequestPlayerInfo = true;
				
					
					string eventName = "CardBreak";
					Dictionary<string,object> dic = new Dictionary<string, object>();
					dic.Add("GoldCost",moneyCost.ToString());
					dic.Add("PlayerId",PlayerPrefs.GetString("username"));
					dic.Add("CardId",targetData.dataId.ToString());
					TalkingDataManager.SendTalkingDataEvent(eventName,dic);
				}
			}break;
            case 5:
                if (errorCode == 0)
                {
                    showPage(PAGETYPE.PAGE_BREAKCTRL);
                    setBreakData();
                    popCardDetail.GetComponent<PopCardDetailUI>().setContent(breakJson.pe, null, null, null);
                    HeadUI.mInstance.refreshPlayerInfo();
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
                    {
                        GuideUI22_Break.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(13);
                    }
                    
					//needShowResurlt = true;
					int lastBreakNum = oldLevel;
					//targetData = brj.pe;
					int nowBreakNum = targetData.bn;
					consumeDataList.Clear();
					
					if(targetData != null)
					{
						showPage(PAGETYPE.PAGE_BREAKCTRL);
						oldExp = targetData.breakType;
						oldLevel = targetData.bn;
					}
					notSetExp = true;
					CardData cd = CardData.getData(targetData.dataId);
					if(cd != null)
					{
						string talentStr1 = "";
						string talentStr2 = "";
						TalentData td1 =TalentData.getData(cd.talent2);
						TalentData td2 =TalentData.getData(cd.talent3);
						if(td1!=null)
						{
							talentStr1 = TalentData.getData(cd.talent2).name;
						}
						else
						{
							talentStr1 = string.Empty;
						}
						if(td2!=null)
						{
							talentStr2 = TalentData.getData(cd.talent3).name;
						}
						else
						{
							talentStr2 = string.Empty;
						}
                    if (lastBreakNum != nowBreakNum)
                    {
                        popWnd.SetActive(true);
                        if (nowBreakNum < Constant.MaxBreakNum1)
                        {
                            if (!talentStr1.Equals(""))
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(564).chinese.Replace("num", Constant.MaxBreakNum1.ToString()) + talentStr1;
                            }
                            else if (!talentStr2.Equals(""))
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(564).chinese.Replace("num", Constant.MaxBreakNum2.ToString()) + talentStr2;
                            }
                            else
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString());
                            }
                        }
                        else if (nowBreakNum == Constant.MaxBreakNum1)
                        {
                            if (!talentStr1.Equals(""))
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(565).chinese + talentStr1;
                            }
                            else if (!talentStr2.Equals(""))
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(564).chinese.Replace("num", Constant.MaxBreakNum2.ToString()) + talentStr2;
                            }
                            else
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString());
                            }
                        }
                        else if (nowBreakNum < Constant.MaxBreakNum2)
                        {
                            if (!talentStr2.Equals(""))
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(564).chinese.Replace("num", Constant.MaxBreakNum2.ToString()) + talentStr2;
                            }
                            else
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString());
                            }
                        }
                        else if (nowBreakNum == Constant.MaxBreakNum2)
                        {
                            if (!talentStr2.Equals(""))
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(565).chinese + talentStr2 + TextsData.getData(752).chinese;
                            }
                            else
                            {
                                if (CardData.getData(targetData.dataId).star < 4)
                                    popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString());
                                else
                                    popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) + TextsData.getData(752).chinese;
                            }
                        }
                        else if (nowBreakNum > Constant.MaxBreakNum2)
                        {
                            if (CardData.getData(targetData.dataId).star < 4)
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                    TextsData.getData(753).chinese + CardData.getData(targetData.dataId).PPSname;
                            }
                            else
                            {
                                popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                   TextsData.getData(753).chinese;
                            }
                        }
                    }

                    }
                }
                else if (errorCode == 126)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(704).chinese);
                }
                else if (errorCode == 67)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(705).chinese);
                }
                else if (errorCode == 129)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(706).chinese);
                }
                break;

            case 6:
                {
                    //popCardDetail.GetComponent<PopCardDetailUI>().setContent(targetData, null, null, null);
                    setBreakData();
                    showPage(PAGETYPE.PAGE_BREAKCTRL);
                }
                break;
			}
		}
		if(doRequestPlayerInfo)
		{
			HeadUI.mInstance.requestPlayerInfo();
			doRequestPlayerInfo = false;
		}
	}
    public void IsHaveBreak()
    {
        lockHints.SetActive(mark == 0);
    }
	public void init()
	{
		mPageType = PAGETYPE.PAGE_PACK;
		breakExp.setPanelType(NewExpManager.PANELTYPE.E_CardBreak);
	}
	
	public void show()
	{
		if(expUpgradeEffectPrefab == null)
		{
			expUpgradeEffectPrefab = Resources.Load(expUpgradeEffectPath) as GameObject;
		}

		if(breakPackCardPerfab == null)
		{
			breakPackCardPerfab = Resources.Load(packCardPrefabPath) as GameObject;
		}
		c2DHeroATKLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		c2DHeroDEFLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		c2DHeroHPLabel.gameObject.GetComponent<TweenScale>().enabled = false;
		targetData = null;
		consumeDataList.Clear();
		popWnd.SetActive(false);
		showPage(PAGETYPE.PAGE_PACK);
	}
	
	public void hide()
	{
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_BREAKCARD);
	}
	
	private void gc()
	{
		//==释放资源==//
		targetData = null;
		sortItemList.Clear();
		sortItemList=null;
		packGridList.Clear();
		consumeGirdList.Clear();
		consumeDataList.Clear();
		GameObjectUtil.destroyGameObjectAllChildrens(packGridListCtrl);
		GameObjectUtil.destroyGameObjectAllChildrens(consumeListGrid);
		breakPackCardPerfab=null;
		expUpgradeEffectPrefab=null;
		brj=null;
		Resources.UnloadUnusedAssets();
	}
    public GameObject nextCard;

    public GameObject lastCard;

    void SendBreak(int param)
    {
        requestType = 6;
        UIJson uiJson = new UIJson();
        uiJson.UIJsonForBreak(STATE.UI_CARDINFO, param);
        PlayerInfo.getInstance().sendRequest(uiJson, this);
    }

    int getBreakItemNum(int index)
    {
        for (int i = 0; i < breakCardList.Count; i++)
        {
            if (breakCardList[i].bnType == 1 && breakCardList[i].i == index)
            {
                return i;
            }
        }
        return 0;
    }
    int getBreakNum()
    {
        int k = 0;
        for (int i = 0; i < breakCardList.Count; i++)
        {
            if (breakCardList[i].bnType == 1)
            {
                k++;
            }
        }
        return k;
    }

    void getBreakCard(int index)
    {
        targetData = breakCardList[index];
        SendBreak(breakCardList[index].i);
    }
    public void onClickNextCard(int param)
    {
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
       
        if (param == 1)
        {

            if (getBreakItemNum(targetData.i) < breakCardList.Count)
            {
                getBreakCard(getBreakItemNum(targetData.i) + 1);
               
            }
            //setBreakData();
           
           
        }
        else if (param == -1)
        {
            if (getBreakItemNum(targetData.i) > 0)
            {
                getBreakCard(getBreakItemNum(targetData.i)-1);
            }
            //setBreakData();
            //popCardDetail.GetComponent<PopCardDetailUI>().setContent(targetData, null, null, null);
        }
        if (getBreakItemNum(targetData.i) + 1 >= getBreakNum())
        {
            nextCard.SetActive(false);
        }
        else
        {
            nextCard.SetActive(true);
        }

        if (getBreakItemNum(targetData.i) - 1 < 0)
        {
            lastCard.SetActive(false);
        }
        else
        {
            lastCard.SetActive(true);
        }
    }
	//  param : 1 back to break pack btn,2 do break
	public void onClickBreakCtrlBtn(int param)
	{
		switch(param)
		{
		case 1:
		{
            mark = 0;
			backToBreakPack();
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		}break;
		case 2:
		{
			doBreak();
		}break;
		}
	}
	
	public void backToBreakPack()
	{
		requestType = 1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CardBreak),this);
	}
	
	public void doBreak()
	{
		if(targetData == null)
		{
			return;
		}
		CardData cd = CardData.getData(targetData.dataId);
		if(cd == null)
			return;
		
		if(targetData.bn == 5)
		{
			ToastWindow.mInstance.showText(TextsData.getData(212).chinese);
			return;
		}
		if(PlayerInfo.getInstance().player.gold < moneyCost)	//金币不足//
		{
			int buyType = 1;
			int jsonType = 1;
			int costType = 1;
			ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_BREAK);
			return;
		}
		oldExp = targetData.breakType;
		oldLevel = targetData.bn;
		int maxLevel = 0;
		int attrValue = 0;
		if(targetData.bn > 0)
		{
			maxLevel = cd.maxLevel + EvolutionData.getData(cd.star,targetData.bn).lvl;
			attrValue = EvolutionData.getData(cd.star,targetData.bn).status;
		}
		else
		{
			maxLevel = cd.maxLevel;
			attrValue = 0;
		}
		
		maxLevelText.text = maxLevel.ToString();
		if(attrValue > 0)
		{
			attrValueText.text = attrValue.ToString() + "%";	
		}
		else
		{
			attrValueText.text = "0";
		}
		breakExp.recover();
		
		requestType = 3;
		List<int> idList = new List<int>();
		for(int i = 0 ;i < consumeDataList.Count;++i)
		{
			if(consumeDataList[i] != null)
			{
				idList.Add(consumeDataList[i].i);
			}
		}
		PlayerInfo.getInstance().sendRequest(new BreakJson(targetData.i,idList),this);
	}
	
	public void onClickCloseBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		hide();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
	}
	
	public void closePopWnd()
	{
		popWnd.SetActive(false);
	}

    public void setBreakData()
    {
        if (cardN == -1)
        {
            breakButtons.GetComponent<UIButtonMessage>().enabled = false;

            breakButtons.GetComponent<UISprite>().color = Color.gray;

            labels.SetActive(false);
            hint.gameObject.SetActive(true);
            hint.text = TextsData.getData(716).chinese;

        }
        else
        {
            breakButtons.GetComponent<UIButtonMessage>().enabled = true;

            breakButtons.GetComponent<UISprite>().color = Color.white;

            labels.SetActive(true);
            hint.gameObject.SetActive(false);

            if (pcardN + multCard >= cardN)
            {
                diamondLbaelCardNum.text = "[FFFFCC]" + pcardN + "/" + "[-]" + cardN;
            }
            else
            {
                diamondLbaelCardNum.text = "[CC0000]" + pcardN + "/" + "[-]" + cardN;
            }
        }
        if (cardN == 0)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].SetActive(true);
            }
        }
        if (card != null)
        {

            CardData d = CardData.getData(targetData.dataId);

            iconCards.atlas = LoadAtlasOrFont.LoadAtlasByName(d.atlas);
            iconCards.spriteName = d.icon;
            if (d.PSSskilltype1 == 0)
            {
                psSkillIcon.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                psSkillIcon.transform.parent.gameObject.SetActive(true);
            }
        }
        else
        {
            psSkillIcon.transform.parent.gameObject.SetActive(false);
        }
        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
        {



            if (PlayerInfo.getInstance().player.diamond > diamonds)
            {
                diamondLbaelDiamond.text = "[CC0000]" + (diamonds + diamond) + "[-]" + "/" + diamond;
            }
            else
                diamondLbaelDiamond.text = "[FFFFCC]" + (diamonds + diamond) + "[-]" + "/" + diamond;
        }
        else
        {
            if (diamond == -1)
            {
                diamondLbaelDiamond.text = "  -";
            }
            else
            {
                if (PlayerInfo.getInstance().player.diamond >= diamond)
                {
                    diamondLbaelDiamond.text = "[FFFFCC]" + diamonds + "/" + "[-]" + diamond;
                }
                else
                {
                    diamondLbaelDiamond.text = "[CC0000]" + diamonds + "/" + "[-]" + diamond;
                }
            }
        }

        diamondLbaelMultCardNum.text = multCard + "";
    }

    public void BreakClick(int param)
    {
        
        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
        {
            requestType = 5;
            PlayerInfo.getInstance().sendRequest(new BreakJson(targetData.i, new List<int>()), this);
        }
        else
        {
            if (pcardN + multCard >= cardN && PlayerInfo.getInstance().player.diamond > diamond)
            {
                List<int> listCards = new List<int>();

                for (int i = 0; i < cards.Count; i++)
                {
                    if (i < cardN)
                    {
                        if (cards[i].eType == 1)
                        {
                            listCards.Add(cards[i].i);
                        }
                    }
                }
                if (listCards.Count < cardN)
                {
                    for (int c = 0; c < cardN - (listCards.Count); c++)
                    {
                        if (cards[c].eType == 5)
                        {
                            listCards.Add(cards[c].i);
                        }
                    }
                }
                requestType = 5;
                PlayerInfo.getInstance().sendRequest(new BreakJson(targetData.i, listCards), this);
            }
            else
            {
                if (targetData.bn > 3 && PlayerInfo.getInstance().player.diamond > diamond)
                {
                    requestType = 5;
                    PlayerInfo.getInstance().sendRequest(new BreakJson(targetData.i, new List<int>()), this);
                }
                else
                    ToastWindow.mInstance.showText(TextsData.getData(706).chinese);
            }

        }
    }
    int type = 0;
    public void BackClick(int param)
    {
        type = 1;
        requestType = 1;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CardBreak), this);

    }
	public void showPage(PAGETYPE pt)
	{
		mPageType = pt;
		switch(mPageType)
		{
		case PAGETYPE.PAGE_PACK:
		{
			bagCtrl.SetActive(true);
            //popCardDetail.SetActive(false);
			drawPackGrid();
		}break;
		case PAGETYPE.PAGE_BREAKCTRL:
		{
			bagCtrl.SetActive(false);
			//popCardDetail.SetActive(true);
            //showCardInfo();
            //drawConsumeListGrid();
            //showBreakCtrlPageAllInfo();
            onClickNextCard(0);
            setBreakData();
            popCardDetail.GetComponent<PopCardDetailUI>().setContent(targetData, null, null, null);
		}break;
		}
	}
	
	public void drawPackGrid()
	{
		for(int i = 0 ;i < packGridList.Count;++i)
		{
			CardBreakPackInfo  info = packGridList[i];
			info.clear();
		}
		packGridListCtrl.GetComponent<UIGrid2>().repositionNow = true;
		packWindowScrollBar.value = 0;
		packWindowScrollPanel.transform.localPosition = Vector3.zero;
		packWindowScrollPanel.GetComponent<UIPanel>().clipRange = new Vector4(0,0,720,390);
		
		if(packGridList.Count < 10)
		{
			for(int i = 0 ; i < 10;++i)
			{
				GameObject obj = GameObject.Instantiate(breakPackCardPerfab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(obj,packGridListCtrl,true);
				CardBreakPackInfo info = new CardBreakPackInfo();
				info.data = null;
				info.sInfo = obj.GetComponent<SimpleCardInfo1>();
				info.sInfo.bm.target = _myTransform.gameObject;
				info.sInfo.bm.functionName = "onSelectPackCardItem";
				info.selectObj = GameObjectUtil.findGameObjectByName(obj,"MarkSelect");
				info.canBreakObj = GameObjectUtil.findGameObjectByName(obj,"CanBreak");
				info.clear();
				packGridList.Add(info);
			}	
		}
		
		int hasItemLineCount = (sortItemList.Count-1)/5+1;
		int totalGridLineCout= (packGridList.Count-1)/5+1;
		
		if(hasItemLineCount <= totalGridLineCout)
		{
			for(int i = 0; i < packGridList.Count;++i)
			{
				CardBreakPackInfo info = packGridList[i];
				//bool used = false;
				if(i < sortItemList.Count)
				{
					info.data = sortItemList[i];
				}
				else
				{
					info.data = null;
				}
				setCardItemDisplay(info);
				if(info.data == null )
				{
					if( i > (hasItemLineCount*5 -1) && i > 9)
					{
						info.sInfo.gameObject.SetActive(false);
					}
					else
					{
						info.sInfo.gameObject.SetActive(true);
					}
				}
				else
				{
					info.sInfo.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			for(int i = 0; i < packGridList.Count;++i)
			{

				CardBreakPackInfo info = packGridList[i];
				//bool used = false;
				info.data = sortItemList[i];
				setCardItemDisplay(info);
			}
			for(int i = totalGridLineCout*5; i< hasItemLineCount*5;++i)
			{
				GameObject obj = GameObject.Instantiate(breakPackCardPerfab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(obj,packGridListCtrl,true);
				CardBreakPackInfo info = new CardBreakPackInfo();
				info.data = null;
				info.sInfo = obj.GetComponent<SimpleCardInfo1>();
				info.sInfo.bm.target = _myTransform.gameObject;
				info.sInfo.bm.functionName = "onSelectPackCardItem";
				info.selectObj = GameObjectUtil.findGameObjectByName(obj,"MarkSelect");
				info.canBreakObj = GameObjectUtil.findGameObjectByName(obj,"CanBreak");
				info.clear();
				if(i < sortItemList.Count)
				{
					info.data = sortItemList[i];
				}
				else
				{
					info.data = null;
				}
				setCardItemDisplay(info);
				packGridList.Add(info);
			}
		}
	}

	public void drawConsumeListGrid()
	{
		for(int i = 0 ;i < consumeGirdList.Count;++i)
		{
			CardBreakPackInfo  info = consumeGirdList[i];
			info.clear();
		}
		
		consumeListGrid.GetComponent<UIGrid2>().repositionNow = true;
		consumeListPanelCtrl.transform.localPosition = Vector3.zero;
		consumeListPanelCtrl.GetComponent<UIPanel>().clipRange = new Vector4(0,0,592,390);
		consumeListScrollBar.value = 0;
		
		if(consumeGirdList.Count < 8)
		{
			for(int i =0;i < 8; ++i)
			{
				GameObject obj = GameObject.Instantiate(breakPackCardPerfab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(obj,consumeListGrid,true);
				CardBreakPackInfo info = new CardBreakPackInfo();
				info.data = null;
				info.sInfo = obj.GetComponent<SimpleCardInfo1>();
				info.sInfo.bm.target = _myTransform.gameObject;
				info.sInfo.bm.functionName = "onSelectConsumeCardItem";
				info.selectObj = GameObjectUtil.findGameObjectByName(obj,"MarkSelect");
				info.canBreakObj = GameObjectUtil.findGameObjectByName(obj,"CanBreak");
				info.clear();
				consumeGirdList.Add(info);
			}
		}
		
		int hasItemLineCount = (sortItemList.Count-1)/4 +1;
		int totalGridLineCout= (consumeGirdList.Count-1)/4 +1;
		
		if(hasItemLineCount <= totalGridLineCout)
		{
			for(int i = 0; i < consumeGirdList.Count;++i)
			{
				CardBreakPackInfo info = consumeGirdList[i];
				if(i < sortItemList.Count)
				{
					info.data = sortItemList[i];
				}
				else
				{
					info.data = null;
				}
				setCardItemDisplay(info);
				// TODO
				if(checkCardIsSelectInPack(info.data))
				{
					info.selectObj.SetActive(true);
				}
				else
				{
					info.selectObj.SetActive(false);
				}
				if(info.data == null )
				{
					if( i > (hasItemLineCount*4 -1) && i > 7)
					{
						info.sInfo.gameObject.SetActive(false);
					}
					else
					{
						info.sInfo.gameObject.SetActive(true);
					}
				}
				else
				{
					info.sInfo.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			for(int i = 0; i < consumeGirdList.Count;++i)
			{
				CardBreakPackInfo info = consumeGirdList[i];
				info.data = sortItemList[i];
				setCardItemDisplay(info);
				
				if(checkCardIsSelectInPack(info.data))
				{
					info.selectObj.SetActive(true);
				}
				else
				{
					info.selectObj.SetActive(false);
				}
				
			}
			for(int i = totalGridLineCout*4 ; i< hasItemLineCount*4;++i)
			{
				GameObject obj = GameObject.Instantiate(breakPackCardPerfab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(obj,consumeListGrid,true);
				CardBreakPackInfo info = new CardBreakPackInfo();
				info.data = null;
				info.sInfo = obj.GetComponent<SimpleCardInfo1>();
				info.sInfo.bm.target = _myTransform.gameObject;
				info.sInfo.bm.functionName = "onSelectConsumeCardItem";
				info.selectObj = GameObjectUtil.findGameObjectByName(obj,"MarkSelect");
				info.canBreakObj = GameObjectUtil.findGameObjectByName(obj,"CanBreak");
				info.clear();
				if(i < sortItemList.Count)
				{
					info.data = sortItemList[i];
				}
				else
				{
					info.data = null;
				}
				setCardItemDisplay(info);
				if(checkCardIsSelectInPack(info.data))
				{
					info.selectObj.SetActive(true);
				}
				else
				{
					info.selectObj.SetActive(false);
				}
				consumeGirdList.Add(info);
			}
		}
	}
	
	public void showCardInfo()
	{
		if(targetData != null && !notSetExp)
		{
			CardData cd = CardData.getData(targetData.dataId);
			if(cd != null)
			{
				card2DInfo.clear();
				card2DInfo.setSimpleCardInfo(targetData.dataId,GameHelper.E_CardType.E_Hero,targetData);
				//breakExp
				breakExp.hideLightAndYellowBar();
				breakExp.setData(STATE.EXP_TYPE_MOVE_BREAK,oldExp,oldLevel,targetData.breakType,targetData.bn,cd.star,targetData);
			}
		}
	}
	
	public void showBreakCtrlPageAllInfo(bool showExpShake = false)
	{
		CardData cd = CardData.getData(targetData.dataId);
		if(cd == null)
			return;
		card2DNameLabel.text = cd.name;
		//calc cur max level and can promote max level
		int maxLevel = 0;
		int attrValue = 0;
		int changeAttrValue = 0;
		if(targetData.bn > 0)
		{
			maxLevel = cd.maxLevel + EvolutionData.getData(cd.star,targetData.bn).lvl;
			attrValue = EvolutionData.getData(cd.star,targetData.bn).status;
		}
		else
		{
			maxLevel = cd.maxLevel;
			attrValue = 0;
		}
		int curBn = targetData.bn;
		int curBnExp = targetData.breakType;
		moneyCost = 0;
		int promoteLevel = 0;
		int canBreakNum = 0;
	 	int showAfterBreakExp = 0; // 突破后计算出来的当前级别经验//

		int consumeCount = consumeDataList.Count;
		int curNeedUpExp = 0;
		int costFactor =  0;
		if(curBn < 5 )
		{
			curNeedUpExp = EvolutionData.getData(cd.star,curBn+1).cards - curBnExp;
			costFactor =  EvolutionData.getData(cd.star,curBn+1).moneypercard;
		}
		if(consumeCount > 0)
		{
			showAfterBreakExp = consumeCount + oldExp;
			while(consumeCount > 0 && curBn <5)
			{
				bool needSetData = false;
				if(curNeedUpExp > 0)
				{
					--curNeedUpExp;
					moneyCost += costFactor;
					if(curNeedUpExp==0)
					{
						++curBn;
						++canBreakNum;
						if(curBn <5)
						{
							costFactor =  EvolutionData.getData(cd.star,curBn+1).moneypercard;
							curNeedUpExp = EvolutionData.getData(cd.star,curBn+1).cards;
							needSetData = true;
							
						}
						else
						{
							showAfterBreakExp = 0;
							break;
						}
					}
				}
				--consumeCount;
				if(needSetData)
				{
					showAfterBreakExp = consumeCount;
				}
			}
		}
		else
		{
			showAfterBreakExp = oldExp;	
		}
		
		
		costValueText.text = moneyCost.ToString();
		
		if(breakExp.finishShowExp)
		{
			if((targetData.bn + canBreakNum) > 0)
			{
				promoteLevel = cd.maxLevel + EvolutionData.getData(cd.star,targetData.bn + canBreakNum).lvl  - maxLevel;
				if(promoteLevel == 0)
				{
					maxLevelText.text = maxLevel.ToString();
				}
				else
				{
					maxLevelText.text = maxLevel.ToString()+ " +" + promoteLevel.ToString();
				}
				changeAttrValue = EvolutionData.getData(cd.star,targetData.bn + canBreakNum).status - attrValue;
				if(changeAttrValue == 0)
				{
					attrValueText.text = attrValue.ToString() + "%";
				}
				else
				{
					attrValueText.text = attrValue.ToString() + "% +" + changeAttrValue.ToString() + "%"; 
				}
			}
			else
			{
				promoteLevel = 0;
				maxLevelText.text = maxLevel.ToString();
				changeAttrValue = 0;
				attrValueText.text = "0%";
			}
			GameHelper.setCardAttr(targetData.dataId,targetData,targetData.lv,targetData.bn,c2DHeroATKLabel,c2DHeroDEFLabel,c2DHeroHPLabel,-1,targetData.bn+canBreakNum);
		}
		
		if(showExpShake)
		{
			breakExp.showExpShake(STATE.EXP_TYPE_MOVE_BREAK,oldLevel+canBreakNum,showAfterBreakExp,targetData);
		}
		else
		{
			breakExp.recover();
		}
		
	}

	public void setCardItemDisplay(CardBreakPackInfo cardInfo)
	{
		cardInfo.clear();
		if(cardInfo.data != null)
		{
			cardInfo.sInfo.setSimpleCardInfo(cardInfo.data.dataId,GameHelper.E_CardType.E_Hero,cardInfo.data);
			cardInfo.sInfo.bm.gameObject.name = cardInfo.data.i.ToString();
			cardInfo.sInfo.bm.param = cardInfo.data.i;
			if(cardInfo.data.bnType == 1)
			{
                mark = 1;
				cardInfo.canBreakObj.SetActive(true);
			}
			else
			{
				cardInfo.canBreakObj.SetActive(false);
			}
		}
        IsHaveBreak();
	}
	
	PackElement getSortItem(int index)
	{
        sortItemList = breakCardList;
		for(int i = 0 ;i < sortItemList.Count ;++i)
		{
			if(sortItemList[i].i == index)
			{
				return sortItemList[i];
			}
		}
		return null;
	}
	
	public int getGuideTargetIndex()
	{
		if(sortItemList.Count == 0)
		{
			return -1;
		}
		return sortItemList[0].i;
	}
	
	public bool checkCardIsSelectInPack(PackElement data)
	{
		bool result = false;
		if(data == null)
		{
			return result;
		}
		for(int i = 0; i < consumeDataList.Count; ++i)
		{
			PackElement cpe = consumeDataList[i];
		
			if(cpe.i == data.i)
			{
				result = true;
				break;
			}
		}

		return result;
	}

	public void onSelectPackCardItem(int index)
	{
		if(index == -1)
			return;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		PackElement data = getSortItem(index);
		if(data.bnType == 0)
		{
			return;
		}
		targetData = data;
		oldExp = targetData.breakType;
		oldLevel = targetData.bn;
		requestType = 6;
		UIJson uiJson = new UIJson();
		uiJson.UIJsonForBreak(STATE.UI_CARDINFO,data.i);

        card = targetData;
		PlayerInfo.getInstance().sendRequest(uiJson,this);
        spread.onClickSpread(3);
	}
	
	public void onSelectConsumeCardItem(int index)
	{
		if(index == -1)
			return;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
		PackElement data = getSortItem(index);
		bool isInConsumeList = false;
		PackElement needRemoveData = null;
		for(int i = 0; i < consumeDataList.Count; ++i)
		{
			PackElement itemInfo = consumeDataList[i];
			if(itemInfo.i == data.i)
			{
				needRemoveData = itemInfo;
				isInConsumeList = true;
				break;
			}
		}
		for(int i = 0; i < consumeGirdList.Count;++i)
		{
			CardBreakPackInfo info = consumeGirdList[i];
			if(info.data.i == data.i)
			{
				if(isInConsumeList)
				{
					info.selectObj.SetActive(false);
					consumeDataList.Remove(needRemoveData);
				}
				else
				{
					if (checkIsCanSelectConsumeCard(consumeDataList.Count))
					{
						info.selectObj.SetActive(true);
						consumeDataList.Add(data);
					}
				}
				break;
			}
		}
		if(consumeDataList.Count > 0)
		{
			showBreakCtrlPageAllInfo(true);
		}
		else
		{
			showBreakCtrlPageAllInfo(false);
		}
		
		
	}
	
	public bool checkIsCanSelectConsumeCard(int count)
	{
		if(targetData == null)
			return false;
		CardData cd = CardData.getData(targetData.dataId);
		if(cd == null)
			return false;
		int curBn = targetData.bn;
		int curBnExp = targetData.breakType;
		if(curBn == 5)
			return false;
		int needCount = 0;
		for(int i = curBn+1; i < 6; ++i)
		{
			needCount += EvolutionData.getData(cd.star,i).cards;
		}
		needCount -= curBnExp;
		if(count >=needCount)
			return false;
		return true;
	}
	
	public void notifyLevelUp(int level)
	{
		GameObject levelUpEffectObj = GameObject.Instantiate(expUpgradeEffectPrefab) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(levelUpEffectObj,levelUpEffectNode);
		if(targetData == null)
			return;
		CardData cd = CardData.getData(targetData.dataId);
		if(cd == null)
			return;
		
		int maxLevel = cd.maxLevel + EvolutionData.getData(cd.star,level).lvl;
		maxLevelText.text = maxLevel.ToString();
		int attrValue = EvolutionData.getData(cd.star,level).status;
		attrValueText.text = attrValue.ToString() +"%";
		GameObjectUtil.playForwardUITweener(maxLevelText.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(attrValueText.gameObject.GetComponent<TweenScale>());
		notifyCardAttr(level);
	}
	
	public void notifyCardAttr(int bn)
	{
		GameHelper.setCardAttr(targetData.dataId,null,targetData.lv,bn,c2DHeroATKLabel,c2DHeroDEFLabel,c2DHeroHPLabel);
		GameObjectUtil.playForwardUITweener(c2DHeroATKLabel.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(c2DHeroDEFLabel.gameObject.GetComponent<TweenScale>());
		GameObjectUtil.playForwardUITweener(c2DHeroHPLabel.gameObject.GetComponent<TweenScale>());
	}
	
	public void receiveResponse(string json)
	{
		if(json != null)
		{
            Debug.Log(json);
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
			{
				brj = JsonMapper.ToObject<BreakResultJson>(json);
				receiveData = true;
                breakCardList = brj.pes;
			}break;
			case 2:
			{
				brj = JsonMapper.ToObject<BreakResultJson>(json);
				if(needRequestPlayerInfo)
				{
					//needShowResurlt = false;
					doRequestPlayerInfo = true;
                    breakCardList = brj.pes;
                    targetData = brj.pe;
                    
				}

                errorCode = brj.errorCode;
                this.diamonds = PlayerInfo.getInstance().player.diamond;
                this.cardN = brj.cn;
                this.diamond = brj.d;
                this.pcardN = brj.pd;
                this.multCard = brj.pmc;
                cards = brj.pes;
				receiveData = true;
			}break;
			case 3:
			{
				brj = JsonMapper.ToObject<BreakResultJson>(json);
                breakCardList = brj.pes;
				receiveData = true;
			}break;
            case 5:
                breakJson = JsonMapper.ToObject<BreakResultJson>(json);
                errorCode = breakJson.errorCode;
                if (errorCode == 0)
                {
                    PlayerInfo.getInstance().player.diamond = breakJson.pd;
                    this.cardN = breakJson.cn;
                    this.diamond = breakJson.d;
                    this.pcardN = breakJson.pcn;
                    this.multCard = breakJson.pmc;
                    this.diamonds = breakJson.pd;
                    cards = breakJson.pes;
                    targetData = breakJson.pe;
                }
				receiveData = true;

                break;

            case 6:
                cirj = JsonMapper.ToObject<CardInfoResultJson>(json);
				errorCode = cirj.errorCode;;
                this.diamonds = PlayerInfo.getInstance().player.diamond;
                this.cardN = cirj.cardN;
                this.diamond = cirj.diamond;
                this.pcardN = cirj.pCardN;
                this.multCard = cirj.multCard;
                cards = cirj.pes;
				receiveData = true;
                break;
			}


		}		
	}

}
