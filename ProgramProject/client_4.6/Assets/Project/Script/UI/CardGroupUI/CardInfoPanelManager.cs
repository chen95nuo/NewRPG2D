using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CardInfoPanelManager : MonoBehaviour ,ProcessResponse
{
	public enum CIPType : int
	{
		E_Null = -1,
		E_Normal = 0,
		E_Passive = 1
	}
	
	CIPType cipType = CIPType.E_Null;
	
	public GameObject normalPageNode;
	public GameObject passivePageNode;

	//private Transform myTranform;
	//当前显示的卡牌信息在卡组中的位置//
	public int curCardBoxId;
	
	//------------------------- 基础界面信息start ---------------------//

	//0 主动技能， 1 防具， 2 武器， 3 饰品 , 4 ps1, 5 ps2 ,6 ps3//
	public GameObject[] Boxs;
	//页数//
	public UILabel pageLabel;
	//------------------------- 基础界面信息end ---------------------//
	
	
	//选择界面//
	int curSelTypeId;						//0 是选择卡牌，1是选择主动技能，，3 武器，4， 防具，5，饰品, 6 ps1 , 7 ps2 , 8 ps3//
	public GameObject InformationView;		//卡牌详细信息//

	//弹出scroll时出现的黑色半透背景//
	public GameObject[] ScrollBlackBg;
	public GameObject InfoScrollBar;
	public GameObject InfoScrollView;
	
	private int requestType;
	public bool receiveData;
	public List<PackElement> list;
	public int typeTemp;
	
	public CardInfoContrl ciControl;

	public GameObject card3DNode;
	public UILabel card3DName;
	public UISprite card3DStarIcon;
	public GameObject clickCardObj;
	
	private Color cardTalentName2Color;
	private	Color cardTalentDetails2Color;
	
	public PackElement checkIsEmptyCard;
	
	public UISprite npTip;
	public UISprite psTip;  // passive skill
	public UISprite ps1Tip; // ps1
	public UISprite ps2Tip; // ps2
	public UISprite ps3Tip; // ps3
	public UISprite e1Tip; // equip 1
	public UISprite e2Tip; // equip 2
	public UISprite e3Tip; // equip 3

    public UISprite psSkillIcon;

    public UILabel psSkillLbael;

    private Player player;
	CardInfoResultJson cirj;
	
	//卡组界面的引用//
	CombinationInterManager combination;
	
	public GameObject nextCard;
	public GameObject lastCard;
	
	public GameObject changeLabel1Obj;
	public GameObject changeLabel2Obj;
	
	public RotateCard rotateCard;
	
	public static int teamATK = 0;
	public static int teamDEF = 0;
	public static int teamHP = 0;
	//种族百分比加成//
	public static float teamATKAdd = 0;
	public static float teamDEFAdd = 0;
	public static float teamHPAdd = 0;
	
	bool canClickBlackBg = false;
	
	int errorCode = -1;
	CardGroupResultJson cgrj;
	
	CardGroup tempCardGroup;


    public int curType;

    public PopOtherDetailUI popOtherDetail;

    public GameObject changeButton;

    public GameObject intensifyButton;
	
	public List<GameObject> psInfoObjList;
	public List<UILabel> psInfoLabelList;

    public UILabel diamondLbaelDiamond;

    public UILabel diamondLbaelCardNum;

    public UILabel diamondLbaelMultCardNum;

    public GameObject[] BreakLabels;

    public UISprite iconCards;

    public GameObject labels;

    public UILabel hint;

    private BreakResultJson breakJson;

    public GameObject breakButtons;
    List<PackElement> cards = new List<PackElement>();

    public GameObject breakPanel;



    public GameObject[] icons;
    public GameObject psSkillBox;

    public GameObject popWnd;
    public UILabel popText1;
    public UILabel popText2;

    int oldLevel;
	void Awake()
	{
		//myTranform = transform;
		init();
	}
	

	public void clear()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(card3DNode);
		rotateCard.gc();
		card3DName.text = "";
		card3DStarIcon.spriteName = "";
		for(int i = 0; i < Boxs.Length;++i)
		{
			if(Boxs[i] != null)
			{
				Boxs[i].GetComponent<SimpleCardInfo2>().clear();
			}
		}
		clickCardObj.SetActive(true);
	}

	public void ChangeCardInfoData()
	{
		hideTips();
		CleanScrollData();
		//------------------基础信息界面start-----------------//
		clear();
		//获取模型名称//
		PackElement card = combination.curCardGroup.cards[curCardBoxId];
        this.card = card;
		checkIsEmptyCard = card;
		changeLabel1Obj.SetActive(false);
		changeLabel2Obj.SetActive(false);
		if(card == null)
		{
			//清空内容//
			ciControl.SetData(-1,-1,-1,0,0,0,0,0,0,0, "",card);
			SelectBtnCallBack(0); 
			changeLabel2Obj.SetActive(true);
			return;
		}
		CardData cd = CardData.getData(card.dataId);
		if(cd == null)
			return;
		
		GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,STATE.PREFABS_TYPE_CARD))as GameObject;
		if(cardModel == null)
			return;
		changeLabel1Obj.SetActive(true);
		clickCardObj.SetActive(false);
		GameObjectUtil.gameObjectAttachToParent(cardModel,card3DNode,true);
		GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
		cardModel.transform.localPosition = new Vector3(0,cd.modelposition,0);
		cardModel.transform.localScale = new Vector3(cd.modelsize,cd.modelsize,cd.modelsize);
		float rotaY = cd.modelrotation;
		cardModel.transform.localEulerAngles =new Vector3(0,rotaY,0);
		GameObjectUtil.hideCardEffect(cardModel);
		if( card.bn > 0)
		{
			card3DName.text = "LV." + card.lv+"  " +cd.name + "+" + card.bn;
		}
		else 
		{
			card3DName.text = "LV." + card.lv+"  " +cd.name;
		}
		card3DStarIcon.spriteName = "card_side_s"+cd.star.ToString();
		
		rotateCard.setCard3DObj(cardModel);
		//三围属性//
		//CardPropertyData cpd=CardPropertyData.getData(card.lv);
		int atk = (int)Statics.getCardSelfMaxAtkForUI(card.dataId, card.lv, card.bn);
		int def = (int)Statics.getCardSelfMaxDefForUI(card.dataId, card.lv, card.bn);
		int hp = (int)Statics.getCardSelfMaxHpForUI(card.dataId, card.lv, card.bn);
		
		atk += (int)(atk*teamATKAdd);
		def += (int)(def*teamDEFAdd);
		hp += (int)(hp*teamHPAdd);
		//主动技能//
		PackElement skill = combination.curCardGroup.skills[curCardBoxId];
		
		int activeSkillId = 0;
		int skillLevel = 1;
		if(skill != null)
		{
			activeSkillId = skill.dataId;
			skillLevel = skill.lv;
		}
		else 
		{
			activeSkillId = cd.basicskill;
			skillLevel = 1;
		}
		Boxs[0].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(activeSkillId,GameHelper.E_CardType.E_Skill);
        if (curType == 2)
        {
            this.skill = skill;
        }
			
		//被动技能//
		List<PackElement> psList = combination.curCardGroup.passiveSkills[curCardBoxId];
		if (curType == 2)
        {
            this.pSkillList = psList;
        }
        if (card != null)
        {
            CardData data = CardData.getData(card.dataId);

            iconCards.atlas = LoadAtlasOrFont.LoadAtlasByName(data.atlas);

            iconCards.spriteName = data.icon;
            if (data.PSSskilltype1 != 0)
            {
                if (!psSkillBox.activeSelf)
                    psSkillBox.SetActive(true);




               
                psSkillIcon.spriteName = data.PSSicon;
                if (card.bn < 6)
                {
                    psSkillIcon.color = Color.gray;
                    psSkillLbael.text = TextsData.getData(726).chinese;
                }
                else
                {
                    psSkillIcon.color = Color.white;
                    psSkillLbael.text = TextsData.getData(725).chinese;
                }
            }
            else
            {
                psSkillBox.SetActive(false);
            }
        }
		int playerLevel = PlayerInfo.getInstance().player.level;
		if(psList != null)
		{
			for(int i = 0 ; i < psList.Count;++i)
			{
				if(psList[i] == null)
				{
					Boxs[i+4].GetComponent<SimpleCardInfo2>().clear();
					if(curType != 2)
					{
						psInfoObjList[i].SetActive(true);
						switch(i)
						{
						case 0:
						{
							UnlockData ud = UnlockData.getData(34);
							if(ud != null && playerLevel>= ud.method)
							{
								psInfoLabelList[i].text =  TextsData.getData(673).chinese;
							}
							else
							{
								psInfoLabelList[i].text = ud.method.ToString() + TextsData.getData(159).chinese;
							}
						}break;
						case 1:
						{
							UnlockData ud = UnlockData.getData(35);
							if(ud != null && playerLevel>= ud.method)
							{
								psInfoLabelList[i].text =  TextsData.getData(673).chinese;
							}
							else
							{
								psInfoLabelList[i].text = ud.method.ToString() + TextsData.getData(159).chinese;
							}
						}break;
						case 2:
						{
							UnlockData ud = UnlockData.getData(36);
							if(ud != null && playerLevel>= ud.method)
							{
								psInfoLabelList[i].text =  TextsData.getData(673).chinese;
							}
							else
							{
								psInfoLabelList[i].text = ud.method.ToString() + TextsData.getData(159).chinese;
							}
						}break;
						}
					}
					else
					{
						psInfoObjList[i].SetActive(false);
					}
				}
				else
				{
					psInfoObjList[i].SetActive(false);
					Boxs[i+4].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(psList[i].dataId,GameHelper.E_CardType.E_PassiveSkill);
					PassiveSkillData psd = PassiveSkillData.getData(psList[i].dataId);
					if(psd == null)
						continue;
					switch(psd.type)
					{
						case 1://攻击力//
						atk += psd.numbers;
						break;
					case 2://防御力//
						def += psd.numbers;
						break;
					case 3://生命值//
						hp += psd.numbers;
						break;
					}
				}
			}
		}
		else
		{
			for(int i = 0 ; i < 3; ++i)
			{
				if(curType == 2)
				{
					psInfoObjList[i].SetActive(false);
				}
				else
				{
					psInfoObjList[i].SetActive(true);
					switch(i)
					{
					case 0:
					{
						UnlockData ud = UnlockData.getData(34);
						if(ud != null && playerLevel>= ud.method)
						{
							psInfoLabelList[i].text =  TextsData.getData(673).chinese;
						}
						else
						{
							psInfoLabelList[i].text = ud.method.ToString() + TextsData.getData(159).chinese;
						}
					}break;
					case 1:
					{
						UnlockData ud = UnlockData.getData(35);
						if(ud != null && playerLevel>= ud.method)
						{
							psInfoLabelList[i].text =  TextsData.getData(673).chinese;
						}
						else
						{
							psInfoLabelList[i].text = ud.method.ToString() + TextsData.getData(159).chinese;
						}
					}break;
					case 2:
					{
						UnlockData ud = UnlockData.getData(36);
						if(ud != null && playerLevel>= ud.method)
						{
							psInfoLabelList[i].text =  TextsData.getData(673).chinese;
						}
						else
						{
							psInfoLabelList[i].text = ud.method.ToString() + TextsData.getData(159).chinese;
						}
					}break;
					}
				}
				
			}
		}
		
		//用来存储装备拼接的字符串格式为equipId-level//
		List<string> equipInfo = new List<string>();
		//防具，武器，饰品//
		List<PackElement> equipsList = combination.curCardGroup.equips[curCardBoxId];
		//清空防具，武器，饰品里面的图集//
		for(int i = 0;equipsList!=null && i < equipsList.Count; i++)
		{
			EquipData ed = EquipData.getData(equipsList[i].dataId);
			if(ed == null)
				continue;
			
			string info = equipsList[i].dataId+"-"+equipsList[i].lv;
			equipInfo.Add(info);
			
			Boxs[1+ed.type-1].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(equipsList[i].dataId,GameHelper.E_CardType.E_Equip);
            if (curType == 2)
            {
                this.equipsList = equipsList;

            }
			
			EquippropertyData epd = EquippropertyData.getData(ed.type,equipsList[i].lv);
			switch(ed.type)
			{
			case 1://攻击力//
				atk += epd.starNumbers[ed.star-1];
				break;
			case 2://防御力//
				def += epd.starNumbers[ed.star-1];
				break;
			case 3://生命值//
				hp += epd.starNumbers[ed.star-1];
				break;
			}
		}
		//------------------基础信息界面end-----------------//
			
		//---------------------内容介绍界面start ------------------------//
		//详细信息界面//
		//基本信息//
		
		string skillAddData = Statics.getSkillValueForUIShow02(activeSkillId, skillLevel);
		
		//------------------计算符文的攻防血加成--------------------------//
		string runeId = PlayerInfo.getInstance().player.runeId;
		string[] ss = runeId.Split('-');
		int runeLv = StringUtil.getInt(ss[0]);
		switch(runeLv)
		{
		case 1:
			break;
		case 2:
			for(int i = 0;i<ss.Length-1;i++)
			{
				int key = 101 + i;
				RuneTotalData rtd=RuneTotalData.getData(key);
				switch(rtd.proprety)
				{
				case 1://攻击//
					atk += RuneData.getValues(key)+rtd.value;
					break;
				case 2://防御//
					def += RuneData.getValues(key)+rtd.value;
					break;
				case 3://生命//
					hp += RuneData.getValues(key)+rtd.value;
					break;
				}
			}
			break;
		case 3:
			for(int j = 1;j<3;j++)
			{
				for(int i = 0;i<ss.Length-1;i++)
				{
					int key = j*100 + i +1;
					RuneTotalData rtd=RuneTotalData.getData(key);
					switch(rtd.proprety)
					{
					case 1://攻击//
						atk += RuneData.getValues(key)+rtd.value;
						break;
					case 2://防御//
						def += RuneData.getValues(key)+rtd.value;
						break;
					case 3://生命//
						hp += RuneData.getValues(key)+rtd.value;
						break;
					}
				}
			}
			break;
		case 4:
			for(int j = 1;j<4;j++)
			{
				for(int i = 0;i<ss.Length-1;i++)
				{
					int key = j*100 + i +1;
					RuneTotalData rtd=RuneTotalData.getData(key);
					switch(rtd.proprety)
					{
					case 1://攻击//
						atk += RuneData.getValues(key)+rtd.value;
						break;
					case 2://防御//
						def += RuneData.getValues(key)+rtd.value;
						break;
					case 3://生命//
						hp += RuneData.getValues(key)+rtd.value;
						break;
					}
				}
			}
			break;
		}
		for(int i = 0;i<ss.Length-1;i++)
		{
			int key = runeLv * 100 + i + 1;
			RuneTotalData rtd=RuneTotalData.getData(key);
			RuneData rd=RuneData.getNextData(runeLv,i+1,StringUtil.getInt(ss[i+1]));
			switch(rtd.proprety)
			{
			case 1://攻击//
				atk += RuneData.getValues(key,StringUtil.getInt(ss[i+1]));
				if(rd == null)
				{
					atk += rtd.value;
				}
				break;
			case 2://防御//
				def += RuneData.getValues(key,StringUtil.getInt(ss[i+1]));
				if(rd == null)
				{
					def += rtd.value;
				}
				break;
			case 3://生命//
				hp += RuneData.getValues(key,StringUtil.getInt(ss[i+1]));
				if(rd == null)
				{
					hp += rtd.value;
				}
				break;
			}
		}
		
		atk+=teamATK;
		def+=teamDEF;
		hp+=teamHP;
		
		int otherAtk = atk - (int)Statics.getCardSelfMaxAtkForUI(card.dataId, card.lv, card.bn);
		int otherDef = def - (int)Statics.getCardSelfMaxDefForUI(card.dataId, card.lv, card.bn);
		int otherHp = hp - (int)Statics.getCardSelfMaxHpForUI(card.dataId, card.lv, card.bn);
		//----------------end-----------------//
		ciControl.SetData(card.dataId, activeSkillId, card.bn, card.bp,atk,def,hp,otherAtk,otherDef,otherHp, skillAddData,card);
		//---------------------内容介绍界面end -------------------------//
		
		Resources.UnloadUnusedAssets();
	}
    void ArenaClick(int param)
    { 
        
    }
	// Update is called once per frame
	void Update ()
	{
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
				curSelTypeId = typeTemp;

				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
				{
					GuideUI_CardInTeam.mInstance.hideAllStep();
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip))
				{
					//GuideUI12_Equip.mInstance.hideAllStep();
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
				{
					GuideUI18_Skill.mInstance.hideAllStep();
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
				{
					GuideUI_IntensifyEquip.mInstance.hideAllStep();
				}
				for(int i = 0;i < ScrollBlackBg.Length; i++)
				{
					GameObject obj = ScrollBlackBg[i];
					obj.SetActive(true);
				}
				OpenScrollView(curSelTypeId);
				break;
			case 2:
				HeadUI.mInstance.refreshPlayerInfo();
				if(curType != 2)
				{
					requestType = 11;
					UIJson uijson = new UIJson();
					uijson.UIJsonForCardInfo(STATE.UI_CARDINFO,curCardBoxId);
					PlayerInfo.getInstance().sendRequest(uijson,this);
				}
				else
				{
					goNext();
				}
				
				break;
			case 3:
			{
				switch(errorCode)
				{
				case 0:
				{
					CardGroup cg=cgrj.transformCardGroup();
					//PackElement temp=cg.cards[curCardBoxId];
					//if(temp!=null)
					//{
					//	battlePower=temp.bp;
					//}
					//else
					//{
					//	battlePower=0;
					//}
					combination.curCardGroup=cg;
					PlayerInfo.getInstance().curCardGroup = combination.curCardGroup;
					
					requestType=2;
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
				}break;
				case 8:
				{
					combination.curCardGroup =tempCardGroup.Clone();
					switch(curSelTypeId)
					{
					case STATE.SHOW_SCROLLVIEW_PS1:
					case STATE.SHOW_SCROLLVIEW_PS2:
					case STATE.SHOW_SCROLLVIEW_PS3:
					{
						ToastWindow.mInstance.showText(TextsData.getData(670).chinese);
					}break;
					}
				}break;
				case 119:
				{
					combination.curCardGroup =tempCardGroup.Clone();
					switch(curSelTypeId)
					{
					case STATE.SHOW_SCROLLVIEW_PS1:
					{
						UnlockData ud = UnlockData.getData(34);
						if(ud != null )
						{
							ToastWindow.mInstance.showText( ud.description );
						}
					}break;
					case STATE.SHOW_SCROLLVIEW_PS2:
					{
						UnlockData ud = UnlockData.getData(35);
						if(ud != null )
						{
							ToastWindow.mInstance.showText( ud.description );
						}
					}break;
					case STATE.SHOW_SCROLLVIEW_PS3:
					{
						UnlockData ud = UnlockData.getData(36);
						if(ud != null )
						{
							ToastWindow.mInstance.showText( ud.description );
						}
					}break;
					}
				}break;
				case 56:
				{
					ToastWindow.mInstance.showText(TextsData.getData(677).chinese);
				}break;
				}
			}break;
			case 10:
				ChangeCardInfoData();
				handleCardInfoResultJson();
				break;
			case 11:
				
				goNext();
				handleCardInfoResultJson();
				break;
            case 12:
                if (errorCode == 56)
                {
                    ToastWindow.mInstance.showText(TextsData.getData(678).chinese);
                    return;
                }
                else if (errorCode == 0)
                {
                    int toIntensifyPanelCardi = card.i;
                    PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify2, toIntensifyPanelCardi, 1, 0), this);
                    requestType = 15;
                }
                break;
            case 15:
                SkipIntensify();
                break;
            case 16:
                if (errorCode == 0)
                {
                    ChangeCardInfoData();
                    handleCardInfoResultJson();
                    HeadUI.mInstance.refreshPlayerInfo();
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
                    {
                        GuideUI22_Break.mInstance.hideAllStep();
                        UISceneDialogPanel.mInstance.showDialogID(13);
                    }

                    int nowBreakNum = card.bn;

                    int lastBreakNum = oldLevel;

                    






                    CardData cd = CardData.getData(card.dataId);
                    if (cd != null)
                    {
                        string talentStr1 = "";
                        string talentStr2 = "";
                        TalentData td1 = TalentData.getData(cd.talent2);
                        TalentData td2 = TalentData.getData(cd.talent3);
                        if (td1 != null)
                        {
                            talentStr1 = TalentData.getData(cd.talent2).name;
                        }
                        else
                        {
                            talentStr1 = string.Empty;
                        }
                        if (td2 != null)
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
                                    if (CardData.getData(card.dataId).star < 4)
                                        popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString());
                                    else
                                        popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) + TextsData.getData(752).chinese;
                                }
                            }
                            else if (nowBreakNum > Constant.MaxBreakNum2)
                            {
                                if (CardData.getData(card.dataId).star < 4)
                                {
                                    popText1.text = cd.name + " " + TextsData.getData(563).chinese.Replace("num", nowBreakNum.ToString()) +
                                        TextsData.getData(753).chinese + CardData.getData(card.dataId).PPSname;
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
			}
		}
	}
    int prjI;

    public List<PackElement> allCells = new List<PackElement>();

    public List<PackElement> packItemInfo = new List<PackElement>();

    private PackResultJson packRJ;

    public void closePopWnd()
    {
        popWnd.SetActive(false);
    }
    void SkipIntensify()
    {
        //CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager") as CardInfoPanelManager;
        CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager") as CombinationInterManager;
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY);
        IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, "IntensifyPanel") as IntensifyPanel;
        intensify.isScrollViewCome = true;
        MissionUI missionUi = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI") as MissionUI;

        intensify.sortItemList = packItemInfo;
        intensify.selectCardSkillID = prjI;

        intensify.targetData = intensify.getSortItem(card.i);
        intensify.sortItemList.Clear();
        intensify.sortItemList = allCells;
        if (packRJ == null)
        {
            return;
        }
        intensify.allFromIdList = packRJ.pejs;
        intensify.setIntensifyType(IntensifyPanel.INTENSIFYTYPE.TYPE_HERO);
        PackElement dbc = intensify.targetData;
        if (dbc != null)
        {
            intensify.oldExp = dbc.curExp;
            intensify.oldLevel = dbc.lv;
        }
        hide();
        combination.hide();
        if (missionUi != null)
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


        HeadUI.mInstance.show();
        packRJ = null;
    }
	public void CloseScrollBlackBg()
	{
		for(int i = 0;i < ScrollBlackBg.Length; i++)
		{
			GameObject obj = ScrollBlackBg[i];
			obj.SetActive(false);
		}
	}
	
	public void hideTips()
	{
		npTip.gameObject.SetActive(false);
		psTip.gameObject.SetActive(false);
		ps1Tip.gameObject.SetActive(false); 
		ps2Tip.gameObject.SetActive(false); 
		ps3Tip.gameObject.SetActive(false); 
		e1Tip.gameObject.SetActive(false); 
		e2Tip.gameObject.SetActive(false);
		e3Tip.gameObject.SetActive(false);
	}
    int diamonds;

    int cardN;

    int pcardN;

    int multCard;

    int diamond;

    int sell = 0;


	void handleCardInfoResultJson()
	{
		if(cirj != null)
		{
			if(cirj.errorCode == 0)
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
               
                //if (cardN == -1)
                //{
                //    diamondLbaelCardNum.text = "  -";
                //}
                //else
                //{
                //    if (pcardN + multCard >= cardN)
                //    {
                //        diamondLbaelCardNum.text = "[FFFFCC]" + pcardN + "/" + "[-]" + cardN;
                //    }
                //    else
                //    {
                //        diamondLbaelCardNum.text = "[CC0000]" + pcardN + "/" + "[-]" + cardN;
                //    }
                //}
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
                    CardData d = CardData.getData(card.dataId);
                    if (d.PSSskilltype1 == 0)
                    {
                        psSkillIcon.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                       // iconCards.atlas = LoadAtlasOrFont.LoadAtlasByName(d.atlas);
                       // iconCards.spriteName = d.icon;
                        psSkillIcon.transform.parent.gameObject.SetActive(true);
                    }
                }
                else
                {
                    psSkillIcon.transform.parent.gameObject.SetActive(false);
                }
                if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
                {
                    if (sell == 0)
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
				int findMaxPSTipNum = 0;
				if(cirj.ps1 >= findMaxPSTipNum)
				{
					findMaxPSTipNum = cirj.ps1;
				}
				if(cirj.ps2 >= findMaxPSTipNum)
				{
					findMaxPSTipNum = cirj.ps2;
				}
				if(cirj.ps3 >= findMaxPSTipNum)
				{
					findMaxPSTipNum = cirj.ps3;
				}
				
				if(findMaxPSTipNum == 0)
				{
					psTip.gameObject.SetActive(false);
				}
				else
				{
					psTip.gameObject.SetActive(true);
					psTip.spriteName = "tip_mark_" + findMaxPSTipNum.ToString();
				}
				
				if(cirj.ps1 == 0)
				{
					ps1Tip.gameObject.SetActive(false);
				}
				else
				{
					ps1Tip.gameObject.SetActive(true);
					ps1Tip.spriteName = "tip_mark_" + cirj.ps1.ToString();
				}
				
				if(cirj.ps2 == 0)
				{
					ps2Tip.gameObject.SetActive(false);
				}
				else
				{
					ps2Tip.gameObject.SetActive(true);
					ps2Tip.spriteName = "tip_mark_" + cirj.ps2.ToString();
				}
				
				if(cirj.ps3 == 0)
				{
					ps3Tip.gameObject.SetActive(false);
				}
				else
				{
					ps3Tip.gameObject.SetActive(true);
					ps3Tip.spriteName = "tip_mark_" + cirj.ps3.ToString();
				}
				
				int numTip = 0;
				if(cirj.equip1 == 0)
				{
					e1Tip.gameObject.SetActive(false);
				}
				else
				{
					e1Tip.gameObject.SetActive(true);
					e1Tip.spriteName = "tip_mark_" + cirj.equip1.ToString();
					if(cirj.equip1 >= numTip)
					{
						numTip = cirj.equip1;
					}
				}
				
				if(cirj.equip2 == 0)
				{
					e2Tip.gameObject.SetActive(false);
				}
				else
				{
					e2Tip.gameObject.SetActive(true);
					e2Tip.spriteName = "tip_mark_" + cirj.equip2.ToString();
					if(cirj.equip2 >= numTip)
					{
						numTip = cirj.equip2;
					}
				}
				
				if(cirj.equip3 == 0)
				{
					e3Tip.gameObject.SetActive(false);
				}
				else
				{
					e3Tip.gameObject.SetActive(true);
					e3Tip.spriteName = "tip_mark_" + cirj.equip3.ToString();
					if(cirj.equip3 >= numTip)
					{
						numTip = cirj.equip3;
					}
				}
				
				if(numTip == 0)
				{
					npTip.gameObject.SetActive(false);
				}
				else
				{
					npTip.gameObject.SetActive(true);
					npTip.spriteName = "tip_mark_" + numTip.ToString();
				}
				
			}
		}
	}

	public void init ()
	{
		combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
	}
	
	public void hide ()
	{
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO);
	}
	
	private void gc()
	{
		if(list!=null)
		{
			list.Clear();
		}
		list=null;
		checkIsEmptyCard=null;
		cgrj = null;
		tempCardGroup = null;
		Resources.UnloadUnusedAssets();
	}
	
	public void show ()
	{
        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
        {
            if (GuideUI22_Break.mInstance.runningStep == 1)
            {
                GuideUI22_Break.mInstance.showStep(2);
            }
        }
		for(int i = 0;i < ScrollBlackBg.Length;i ++)
		{
			GameObject obj = ScrollBlackBg[i];
			if(obj == null)
				continue;
			obj.SetActive(false);
		}
		//显示卡牌在阵型中的位置//
		pageLabel.text = (curCardBoxId+1) + "/6" ;
		
		checkIsEmptyCard = combination.curCardGroup.cards[curCardBoxId];

		//不显示head//
		HeadUI.mInstance.hide();

        if (curType == 2)
        {
            changeButton.SetActive(false);
            intensifyButton.SetActive(false);
			ChangeCardInfoData();

            breakPanel.SetActive(false);
        }
		else
		{
			requestType = 10;
			UIJson uijson = new UIJson();
			uijson.UIJsonForCardInfo(STATE.UI_CARDINFO,curCardBoxId);
			PlayerInfo.getInstance().sendRequest(uijson,this);
		}
		showCIPType(CIPType.E_Normal);
        if (checkIsEmptyCard != null)
        {
            oldLevel = checkIsEmptyCard.bn;
        }
	}
	
	public void showCIPType(CIPType t)
	{
		if(cipType == t)
			return;
		cipType = t;
		if(cipType == CIPType.E_Normal)
		{
			passivePageNode.SetActive(false);
			normalPageNode.SetActive(true);
		}
		else if(cipType == CIPType.E_Passive)
		{
			passivePageNode.SetActive(true);
			normalPageNode.SetActive(false);
		}
	}



    public void BreakClick(int param)
    {
        oldLevel = card.bn;
        if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Break))
        {
            requestType = 16;
            PlayerInfo.getInstance().sendRequest(new BreakJson(card.i, new List<int>()), this);
        }
        else
        {
            if (pcardN + multCard >= cardN && PlayerInfo.getInstance().player.diamond >= diamond)
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

                requestType = 16;
                PlayerInfo.getInstance().sendRequest(new BreakJson(card.i, listCards), this);
            }
            else
            {
                if (card.bn > 3 && PlayerInfo.getInstance().player.diamond >= diamond)
                {
                    requestType = 16;
                    PlayerInfo.getInstance().sendRequest(new BreakJson(card.i, new List<int>()), this);
                }
                else
                    ToastWindow.mInstance.showText(TextsData.getData(706).chinese);
            }
           
        }
    }
    private PackElement card;
    private PackElement skill;
    private List<PackElement> pSkillList;
    private List<PackElement> equipsList;

	//0 卡牌按钮， 1 主动技能按钮，  3武器 ， 4 防具， 5 饰品, 6 passive page, 7 normal page, 8 ps1 , 9 ps2 , 10 ps3//
	public void SelectBtnCallBack(int type)
	{
        //播放音效//
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_CARDGROUP);
        if (curType == 2)
        {
            switch (type)
            {
                case 1:
                    {
                        if (skill != null)
                        {
                            PackElement pe = skill;
                            SkillData sd = SkillData.getData(pe.dataId);
                            if (sd != null)
                            {
                                string skillAddData = Statics.getSkillValueForUIShow02(pe.dataId, pe.lv);
                                popOtherDetail.setContentNew(pe.eType, GameHelper.E_CardType.E_Skill, pe.dataId, sd.name, pe.lv.ToString(), sd.star.ToString(), sd.getElementName(), sd.description, sd.sell.ToString(), skillAddData);
                            }
                        }
                        else
                        {
                            CardData cd = CardData.getData(card.dataId);
                            SkillData sd = SkillData.getData(cd.basicskill);
                            if (sd != null)
                            {
                                int skillLevel = 1;
                                string skillAddData = Statics.getSkillValueForUIShow02(sd.index, skillLevel);
                                popOtherDetail.setContentNew(2, GameHelper.E_CardType.E_Skill, cd.basicskill, sd.name, skillLevel.ToString(), sd.star.ToString(), sd.getElementName(), sd.description, sd.sell.ToString(), skillAddData);
                            }
                        }
                    } break;
                case 3:
                case 4:
                case 5:
                    {
                        if (equipsList != null)
                        {
                            PackElement pe = null;
                            EquipData ed = null;
                            foreach (PackElement temp in equipsList)
                            {
                                EquipData tempD = EquipData.getData(temp.dataId);
                                if (tempD.type + 2 == type)
                                {
                                    pe = temp;
                                    ed = tempD;
                                    break;
                                }
                            }
                            if (pe != null)
                            {
                                string equipAddData = Statics.getEquipValueForUIShow(pe.dataId, pe.lv);
                                popOtherDetail.setContentNew(pe.eType, GameHelper.E_CardType.E_Equip, pe.dataId, ed.name, pe.lv.ToString(), ed.star.ToString(), Statics.getEquipValue(ed, pe.lv).ToString(), ed.description, ed.sell.ToString(), equipAddData);
                            }
                        }
                    } break;
                 case 6:
                {
                    showCIPType(CIPType.E_Passive);
				    UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
                } break;
                case 7:
                {
                    showCIPType(CIPType.E_Normal);
					UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
                } break;
                case 8:
                case 9:
                case 10:
                    {
                        if (pSkillList == null)
                            return;

                        if (pSkillList[type - 8] == null)
                        {
                            return;
                        }
                        PassiveSkillData pd = PassiveSkillData.getData(pSkillList[type - 8].dataId);
                        if (pd != null)
                        {
                            popOtherDetail.setContentNew(3, GameHelper.E_CardType.E_PassiveSkill, pSkillList[type - 8].dataId, pd.name, pd.level.ToString(), pd.star.ToString(), "", pd.describe, pd.sell.ToString());
                        }
                    } break;
                case 16:
                    CardData data = CardData.getData(card.dataId);
                    if (data != null && data.PSSskilltype1 == 0)
                    {
                        return;
                    }
                    popOtherDetail.showPssSkill(data.PPSname, data.PPSdescription, data.PSSicon, "UnitSkillIcon");
                    break;
            }
        }
        else
        {
           

            //当前要选择的类型//
            switch (type)
            {
                case 0:
                    {
                        openCardList(type);
                    } break;
                case 1:
                    {
                        openSkillList(type);
                    } break;
                case 3:
                case 4:
                case 5:
                    {
                        openEquipList(type);
                    } break;
                case 6:
                    {
                        showCIPType(CIPType.E_Passive);
						UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
                    } break;
                case 7:
                    {
                        showCIPType(CIPType.E_Normal);
						UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
                    } break;
                case 8:
                    {
                        UnlockData ud = UnlockData.getData(34);
                        if (ud != null && ud.type == 2 && PlayerInfo.getInstance().player.level < ud.method)
                        {
                            ToastWindow.mInstance.showText( ud.description );
                        }
                        else
                        {
                            openPassiveSkillList(type - 2);
                        }
                    } break;
                case 9:
                    {
                        UnlockData ud = UnlockData.getData(35);
                        if (ud != null && ud.type == 2 && PlayerInfo.getInstance().player.level < ud.method)
                        {
                            ToastWindow.mInstance.showText( ud.description );
                        }
                        else
                        {
                            openPassiveSkillList(type - 2);
                        }
                    } break;
                case 10:
                    {
                        UnlockData ud = UnlockData.getData(36);
                        if (ud != null && ud.type == 2 && PlayerInfo.getInstance().player.level < ud.method)
                        {
                            ToastWindow.mInstance.showText( ud.description );
                        }
                        else
                        {
                            openPassiveSkillList(type - 2);
                        }
                    } break;
                case 15:
                    PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,1),this);
                    requestType = 12;
                    break;
                case 16:
                    CardData data = CardData.getData(card.dataId);
                    if (data != null && data.PSSskilltype1 == 0)
					{
						return;
					}
                    popOtherDetail.showPssSkill(data.PPSname, data.PPSdescription, data.PSSicon, "UnitSkillIcon");
                    break;
            }
        }
	}
	
	
	public void openCardList(int type)
	{
		typeTemp=type;
		requestType=1;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP2,1),this);
	}
	
	public void openSkillList(int type)
	{
		typeTemp=type;
		if(checkIsEmptyCard != null)
		{
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP2,2),this);
		}
		else 
		{
			string s1 = TextsData.getData(258).chinese;
			ToastWindow.mInstance.showText(s1);
		}
	}
	
	public void openPassiveSkillList(int type)
	{
		typeTemp=type;
		if(checkIsEmptyCard != null)
		{
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP2,3),this);
		}
		else 
		{
			string s1 = TextsData.getData(258).chinese;
			ToastWindow.mInstance.showText(s1);
		}
	}
	
	public void openEquipList(int type)
	{
		typeTemp=type;
		if(checkIsEmptyCard != null)
		{
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP2,4),this);
		}
		else 
		{
			string s1 = TextsData.getData(258).chinese;
			ToastWindow.mInstance.showText(s1);
		}
	}
	
	public void OpenScrollView(int type)
	{
		List<string> temp ;
		CardGroup cg = combination.curCardGroup;
//		//获得卡牌在playerInfo中的index//
//		int cardInPlayerListId = list.IndexOf(cg.cards[curCardBoxId]);
		int indexInList = -1;
		
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW);
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		
		switch(type){
		case STATE.SHOW_SCROLLVIEW_TYPE_CARD:
			list=PlayerInfo.getCanFightCards(list);
			temp = PlayerInfo.getCardStrList(cg,list,curCardBoxId);
//			int indexInList = list.IndexOf(cg.cards[curCardBoxId]);
			//获取卡牌在背包中的索引//
			if(cg.cards != null)
			{
				for(int i = 0;i < list.Count ;i++){
					PackElement pe = list[i];
					PackElement ca = cg.cards[curCardBoxId];
					if(ca != null && pe.i == ca.i){
						indexInList = i;
					}
				}
			}
			scrollView.setDatas(list,cg.getCardIds(), curCardBoxId);
			scrollView.openScrollViewPanel(ScrollViewPanel.OPENTYPE.E_CARD,temp,indexInList);

			break;
		case STATE.SHOW_SCROLLVIEW_TYPE_ACTIVESKILL:
			//得到的列表是别包里面所有主动技能的列表，在这处理一下，把非正常卡都去掉//
			list = PlayerInfo.getCanFightSkill(list);
			temp = PlayerInfo.getActiveSkillStrList(cg, curCardBoxId,list);
			PackElement card = combination.curCardGroup.cards[curCardBoxId];
			CardData cd = CardData.getData(card.dataId);
			//在list中手动添加一条数据-即添加默认技能//
			PackElement skill = combination.curCardGroup.skills[curCardBoxId];
			string str = "";
			//没有主动技能，则添加默认技能为主动技能，默认技能在playerInfo中的所以定为10000//
			if(skill == null){
				str = "%" + cd.basicskill + "-1"  + "-" + curCardBoxId;
			}
			else {
				str = "%" + cd.basicskill + "-4" + "-null" ;
			}
			temp.Insert(0, str);
			if(cg.skills != null)
			{
				
				for(int i = 0;i < list.Count ;i++){
					PackElement pe = list[i];
					PackElement sk = cg.skills[curCardBoxId];
					if(sk != null && pe.i == sk.i){
						indexInList = i;
					}
				}
			}
			scrollView.setDatas(list,null, curCardBoxId);
			scrollView.openScrollViewPanel(ScrollViewPanel.OPENTYPE.E_ACTIVESKILL,temp,indexInList);
			break;
		case STATE.SHOW_SCROLLVIEW_TYPE_WEAPON:
			temp = PlayerInfo.getEquipedStrList(1, cg, curCardBoxId,list);
			PackElement weapon = null;
			if(cg.equips != null)
			{
				
				List<PackElement> eqList = cg.equips[curCardBoxId];
				if(eqList != null)
				{
					
					for(int i = 0;i <eqList.Count;i++){
						PackElement pe = eqList[i];
						EquipData ed = EquipData.getData(pe.dataId);
						if(ed.type == 1){
							weapon = pe;
						}
					}
					//			indexInList = list.IndexOf(weapon);
					for(int i = 0;i < list.Count ;i++){
						PackElement pe = list[i];
						if(weapon != null && pe.i == weapon.i){
							indexInList = i;
						}
					}
				}
			}
			scrollView.setDatas(list,null, curCardBoxId);
			scrollView.openScrollViewPanel(ScrollViewPanel.OPENTYPE.E_WEAPON,temp,indexInList);
			break;
		case STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON:
			temp = PlayerInfo.getEquipedStrList(2, cg, curCardBoxId,list);
			PackElement proWeapon = null;
			if(cg.equips != null){
				List<PackElement> proEquipList =  cg.equips[curCardBoxId];
				if(proEquipList != null)
				{
					for(int i = 0;i <proEquipList.Count;i++){
						PackElement pe = proEquipList[i];
						EquipData ed = EquipData.getData(pe.dataId);
						if(ed.type == 2){
							proWeapon = pe;
						}
					}
					//			indexInList = list.IndexOf(proWeapon);
					for(int i = 0;i < list.Count ;i++){
						PackElement pe = list[i];
						if(proWeapon != null && pe.i == proWeapon.i){
							indexInList = i;
						}
					}
				}
			}
			scrollView.setDatas(list,null, curCardBoxId);
			scrollView.openScrollViewPanel(ScrollViewPanel.OPENTYPE.E_EQUIP,temp,indexInList);
			break;
		case STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS:
			temp = PlayerInfo.getEquipedStrList(3, cg, curCardBoxId,list);
			PackElement orn = null;
			if(cg.equips != null){
				List<PackElement> ornList =  cg.equips[curCardBoxId];
				if(ornList != null)
				{
					
					for(int i = 0;i <ornList.Count;i++){
						PackElement pe = ornList[i];
						EquipData ed = EquipData.getData(pe.dataId);
						if(ed.type == 3){
							orn = pe;
						}
					}
					
					//			indexInList = list.IndexOf(orn);
					for(int i = 0;i < list.Count ;i++){
						PackElement pe = list[i];
						if(orn != null && pe.i == orn.i){
							indexInList = i;
						}
					}
				}
			}
			scrollView.setDatas(list,null, curCardBoxId);
			scrollView.openScrollViewPanel(ScrollViewPanel.OPENTYPE.E_ORNAMENTS,temp,indexInList);
			break;
		case STATE.SHOW_SCROLLVIEW_PS1:
		case STATE.SHOW_SCROLLVIEW_PS2:
		case STATE.SHOW_SCROLLVIEW_PS3:
		{
			temp = PlayerInfo.getunActiveSkillStrList(type,cg, curCardBoxId,list);
			//int psIndex = -1;
			if(cg.passiveSkills != null )
			{
				if(cg.passiveSkills[curCardBoxId] == null)
				{
					cg.passiveSkills[curCardBoxId] = new List<PackElement>();
					for(int i = 0; i < 3; ++i)
					{
						cg.passiveSkills[curCardBoxId].Add(null);
					}
				}
				for(int i = 0;i < list.Count ;i++)
				{
					PackElement pe = list[i];
					PackElement psk = cg.passiveSkills[curCardBoxId][type-6];
					if(psk == null)
						continue;
					if(psk.i == pe.i)
					{
						indexInList = i;
						//psIndex = j;
						break;
					}
				}
			}
			scrollView.setDatas(list,null, curCardBoxId);
			scrollView.openScrollViewPanel((ScrollViewPanel.OPENTYPE)type,temp,indexInList,curCardBoxId);
		}break;
		}
	}
	
	//返回按钮 返回到卡组界面//
	public void CardInfo_Back(){
		
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		if(scrollView != null && scrollView.getIsMoving())
			return;
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip)
			|| GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
		{
			return;
		}
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
		combination.show();
		
		//清空详细内容界面数据//
		ciControl.CleanData();
		hide();
		
		//再返回的同时修改表中的数据为当前界面修改后的数据//
		//doSamething()
	}
	
	//uniteSkillId 为合体技的id, 用来显示合体技的详细内容//
	//public void SkillBoxBtnListener(int uniteSkillId){
		//if(uniteSkillId > 0){
			
			//UnitSkillData usd = UnitSkillData.getData(uniteSkillId);
//			Show3DTipBoxPanel.mInstance.showData(usd.name, usd.description);
		//}
	//}
	
	public void setTempCardGroup(CardGroup curCG,ref CardGroup targetCG)
	{
		targetCG = new CardGroup();
		targetCG = curCG;
	}
	
	/// <summary>
	/// Replaces the data.
	/// </param>
	/// <param name='changeId'>
	///  人物身上要替换的物品在PlayerInfo中的List中的id，即在list中的位置
	/// </param>
	public void ReplaceCardData( int changeId)
	{
		tempCardGroup = (CardGroup)combination.curCardGroup.Clone();
		
	
		//==如果更换了角色卡,记录卸下前的合体技id集合==//
		List<int> oldUnitIds=null;
		if(curSelTypeId==STATE.SHOW_SCROLLVIEW_TYPE_CARD)
		{
			List<int> cardIds=combination.curCardGroup.getCardIds();
			oldUnitIds=UnitSkillData.getUnitSkillIds(cardIds);
		}
		
		for(int i = 0;i < ScrollBlackBg.Length;i ++)
		{
			GameObject obj = ScrollBlackBg[i];
			obj.SetActive(false);
		}
		//替换cardGroup中相应位置上的信息为新选择的信息//
		switch(curSelTypeId){
		case STATE.SHOW_SCROLLVIEW_TYPE_CARD:
			List<PackElement> cardList = list;
			combination.curCardGroup.cards[curCardBoxId] = cardList[changeId];
			break;
		case STATE.SHOW_SCROLLVIEW_TYPE_ACTIVESKILL:
			if(combination.curCardGroup.cards[curCardBoxId] != null)
			{
				List<PackElement> skillList = list;
				combination.curCardGroup.skills[curCardBoxId] = skillList[changeId];
			}
			break;
		case STATE.SHOW_SCROLLVIEW_PS1:
		case STATE.SHOW_SCROLLVIEW_PS2:
		case STATE.SHOW_SCROLLVIEW_PS3:
		{
			if(combination.curCardGroup.cards[curCardBoxId] != null)
			{
				if(combination.curCardGroup.passiveSkills[curCardBoxId] != null)
				{
					for(int i = 0; i < combination.curCardGroup.passiveSkills[curCardBoxId].Count; ++i)
					{
						if((curSelTypeId - 6) == i)
							continue;
						if(combination.curCardGroup.passiveSkills[curCardBoxId][i] == list[changeId])
						{
							combination.curCardGroup.passiveSkills[curCardBoxId][i] = null;
						}
					}
				}
				else
				{
					combination.curCardGroup.passiveSkills[curCardBoxId] = new List<PackElement>();
					for(int j = 0; j < 3; ++j)
					{
						combination.curCardGroup.passiveSkills[curCardBoxId].Add(null);
					}
				}
				combination.curCardGroup.passiveSkills[curCardBoxId][curSelTypeId - 6] = list[changeId];
			}
		}break;
		case STATE.SHOW_SCROLLVIEW_TYPE_WEAPON:
		case STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON:
		case STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS:
			//要替换的物品所在的卡牌不为空，才进行替换//
			if(combination.curCardGroup.cards[curCardBoxId] != null)
			{
				List<PackElement> equipList = list;
				PackElement dBequips = equipList[changeId];

				List<PackElement> eql = combination.curCardGroup.equips[curCardBoxId];
				if(eql == null)
				{
					eql = new List<PackElement>();
					eql.Add(dBequips);
					combination.curCardGroup.equips[curCardBoxId] = eql;
				}
				else 
				{
					if(eql.Count > 0)
					{
						EquipData ed = null;
						for(int i = 0;i < eql.Count; i++)
						{
							EquipData equip = EquipData.getData(eql[i].dataId);
							//判断当前的list中是否含有该装备，如果有则替换，如果没有，则添加//
							if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_WEAPON )
							{
								if(equip.type == 1)
								{
									eql[i] = dBequips;
									ed = equip;
								}
							}
							else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON)
							{
								if(equip.type == 2)
								{
									ed = equip;
									eql[i] = dBequips;
								}
							}
							else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS)
							{
								if(equip.type == 3)
								{
									eql[i] = dBequips;
									ed = equip;
								}
							}
						}
						if(ed == null)
						{
							eql.Add(dBequips);
						}
					}
					else 
					{
						eql.Add(dBequips);
					}
				}
			}
			break;
		}
		
		//==此处截断,向服务器保存一下卡组数据以获取最新的战斗力,之后会在update里调用goNext()方法继续原本的执行流程  @litao=====//
		//==如果更换了角色卡,记录卸下前的合体技id集合==//
		if(curSelTypeId==STATE.SHOW_SCROLLVIEW_TYPE_CARD)
		{
			List<int> cardIds=combination.curCardGroup.getCardIds();
			List<int> newUnitIds=UnitSkillData.getUnitSkillIds(cardIds);
			int oldUnitId=combination.curCardGroup.unitSkillId;
			if(newUnitIds.Count>0)
			{
				if(oldUnitId!=0&&!newUnitIds.Contains(oldUnitId))
				{
					combination.unitMark=1;
					//combination.curCardGroup.unitSkillId=newUnitIds[0];
				}
				else
				{
					foreach(int newUnitId in newUnitIds)
					{
						if(!oldUnitIds.Contains(newUnitId))
						{
							combination.unitMark=1;
							break;
						}
					}
					
				}
				//changeUniteByPower(newUnitIds);
			}
			else
			{
				if(oldUnitId!=0)
				{
					combination.unitMark=1;
				}
				//combination.curCardGroup.unitSkillId=0;
			}
		}
		//修改标志//
		combination.curCardGroup.changeMark = 0;
		
		goNextParam=2;
		requestType=3;
		PlayerInfo.getInstance().sendRequest(new SaveCGJson(combination.curCardGroup),this);
		
	}
	
	//如果有新增的合体技，自动更换//
	public void changeUniteByPower(List<int> newIds)
	{
		int tempId = combination.curCardGroup.unitSkillId;
		for(int i = 0;i < newIds.Count;i++)
		{
			int id = newIds[i];
			UnitSkillData usd = UnitSkillData.getData(id);
			UnitSkillData tempUsd = UnitSkillData.getData(tempId);
			if(usd.power > tempUsd.power)
			{
				tempId = id;
			}
			
		}
		//combination.curCardGroup.unitSkillId = tempId;
	}
	
	/// <summary>
	/// Replaces the card data02.
	/// 从其他卡牌身上扒装备或者技能到当前卡牌身上
	/// </summary>
	/// <param name='changeId'>
	/// Change identifier.
	/// 要添加到当前卡牌的物品的在playerInfo list中的id
	/// </param>
	/// <param name='cardId'>
	/// Item store card identifier.
	/// 选定的物品在的卡组中的id，即位置
	/// </param>
	public void ReplaceCardData02(int changeId, int cardId)
	{
		tempCardGroup =combination.curCardGroup.Clone();
		
		//==如果卸下了角色卡,记录卸下前的合体技id集合==//
		//List<int> oldUnitIds=null;
		//if(curSelTypeId==STATE.SHOW_SCROLLVIEW_TYPE_CARD)
		//{
			//List<int> cardIds=combination.curCardGroup.getCardIds();
			//oldUnitIds=UnitSkillData.getUnitSkillIds(cardIds);
		//}
		
		PackElement[] dBCardArr = combination.curCardGroup.cards;
		PackElement desCard = dBCardArr[cardId];
		//当前这张卡牌在卡组中的位置//
		int index = -1;
		for(int i =0 ;i < dBCardArr.Length;i++)
		{
			PackElement dbc = dBCardArr[i];
			if(dbc == desCard){
				index = i;
			}
		}
		if(index > -1)
		{
			switch(curSelTypeId)
			{
			/*case STATE.SHOW_SCROLLVIEW_TYPE_CARD:		//卡牌//
				CleanItemForCard(index);
				List<PackElement> cardList = list;
				combination.curCardGroup.cards[curCardBoxId] = cardList[changeId];
				break;
			*/
			case STATE.SHOW_SCROLLVIEW_TYPE_ACTIVESKILL:		//主动技能//
				if(combination.curCardGroup.cards[curCardBoxId] != null)
				{
					List<PackElement> skillList = list;
					combination.curCardGroup.skills[curCardBoxId] = skillList[changeId];
				}
				PackElement[] dBSkill = combination.curCardGroup.skills;
				dBSkill[index] = null;
				break;
			case STATE.SHOW_SCROLLVIEW_PS1:
			case STATE.SHOW_SCROLLVIEW_PS2:
			case STATE.SHOW_SCROLLVIEW_PS3:
			{
				if(combination.curCardGroup.cards[curCardBoxId] != null)
				{
					PackElement tempPE = null;
					if(combination.curCardGroup.passiveSkills[curCardBoxId][curSelTypeId - 6] != null)
					{
						tempPE = combination.curCardGroup.passiveSkills[curCardBoxId][curSelTypeId - 6].Clone();
					}
					else
					{
						tempPE = null;	
					}
					
					for(int i = 0 ; i < combination.curCardGroup.passiveSkills[index].Count;++i)
					{
						if(combination.curCardGroup.passiveSkills[index][i] != null && combination.curCardGroup.passiveSkills[index][i].i == list[changeId].i)
						{
							if(tempPE != null)
							{
								combination.curCardGroup.passiveSkills[index][i] = tempPE.Clone();
							}
							else
							{
								combination.curCardGroup.passiveSkills[index][i] = null;
							}
							
						}
					}
					combination.curCardGroup.passiveSkills[curCardBoxId][curSelTypeId - 6] = list[changeId];
				}
			}break;
			case STATE.SHOW_SCROLLVIEW_TYPE_WEAPON:
			case STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON:			//装备//
			case STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS:
				//要替换的物品所在的卡牌不为空，才进行替换//
				if(combination.curCardGroup.cards[curCardBoxId] != null)
				{
					
					List<PackElement> equipList = list;
					PackElement dBequips = equipList[changeId];
					for(int i =0;i < list.Count; i++)
					{
						EquipData ed = EquipData.getData(list[i].dataId);
						Debug.Log("type_" + i + "============ " + ed.type);
					}
					
					List<PackElement> eql = combination.curCardGroup.equips[curCardBoxId];
					if(eql == null)
					{
						eql = new List<PackElement>();
						eql.Add(dBequips);
						combination.curCardGroup.equips[curCardBoxId] = eql;
					}
					else 
					{
						
						if(eql.Count > 0)
						{
							EquipData ed = null;
							for(int i = 0;i < eql.Count; i++)
							{
								EquipData equip = EquipData.getData(eql[i].dataId);
								//判断当前的list中是否含有该装备，如果有则替换，如果没有，则添加//
								if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_WEAPON )
								{
									if(equip.type == 1)
									{
										eql[i] = dBequips;
										ed = equip;
									}
								}
								else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON)
								{
									if(equip.type == 2)
									{
										ed = equip;
										eql[i] = dBequips;
									}
								}
								else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS)
								{
									if(equip.type == 3)
									{
										eql[i] = dBequips;
										ed = equip;
									}
								}
							}
							if(ed == null)
							{
								eql.Add(dBequips);
							}
							
						}
						else
						{
							eql.Add(dBequips);
						}
					}
				}
				List<PackElement>[] dBEquip = combination.curCardGroup.equips;
				List<PackElement> dbeList = dBEquip[index];
				{
					for(int i = 0;i < dbeList.Count ;)
					{
						PackElement dbe = dbeList[i];
						EquipData ed = EquipData.getData(dbe.dataId);
						if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_WEAPON )
						{
							if(ed.type == 1)
							{
								//							dbe = null;
								dbeList.Remove(dbe);
							}
							else 
							{
								i++;
							}
						}
						else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON)
						{
							if(ed.type == 2)
							{
								//							dbe = null;
								dbeList.Remove(dbe);
							}
							else {
								i++;
							}
						}
						else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS)
						{
							if(ed.type == 3)
							{
								//							dbe = null;
								dbeList.Remove(dbe);
							}
							else
							{
								i ++;
							}
						}
				
					}
				}
				break;
			}
		}
		combination.curCardGroup.changeMark = 0;
		goNextParam=1;
		requestType=3;
		PlayerInfo.getInstance().sendRequest(new SaveCGJson(combination.curCardGroup),this);
	}
	
	
	//交换卡牌，只有在选择卡牌时，并且两张卡牌都是已装备的时候会调用//
	//exChangeCardId要交换的卡牌在playerInfo中的位置id,用当前的卡牌和传过来的卡牌进行交换数据//
	public void ExchangeCard(int exChangeCardId)
	{
		tempCardGroup =combination.curCardGroup.Clone();
		
		//根据传过来的卡牌在playerInfo中的位置id，来找到要替换的卡牌在卡组中的位置
		PackElement[] cardArr = combination.curCardGroup.cards;
//		PackElement dbCard = cardArr[exChangeCardId];
		PackElement dbCard = list[exChangeCardId];
		int changeCardIdInGroup = -1;
		for(int i =0 ;i< cardArr.Length;i++){
			PackElement dbc = cardArr[i];
			if(dbc!=null && dbc.i == dbCard.i){
				changeCardIdInGroup = i;
			}
		}
		
		//记录卡牌在卡组中的位置//
		CardGroup curGroup = combination.curCardGroup;
		PackElement[] cards = curGroup.cards;
		int startCardPosId = curCardBoxId;
		int endCardPosId = changeCardIdInGroup;
		PackElement startDBCard = cards[curCardBoxId];
		PackElement endDBCard = cards[endCardPosId];
		
		//交换两张卡牌即，将1位置的卡牌换成2位置的，将2位置的换成1位置的//
		cards[startCardPosId] = endDBCard;
		cards[endCardPosId] = startDBCard;
		//当前卡组界面组成的卡牌的list、//
		List<int> curCL = new List<int>();
		for(int i =0;i< cards.Length;i++){
			if(cards[i]!=null)
			{
				PackElement dbc = cards[i];
				curCL.Add(dbc.dataId);
			}
		}
		
		//判断当前的卡组是否能够组成改合体技,如果不能组成，则将合体技变成原始的合体技//
		List<UnitSkillData> uniteSkillList = UnitSkillData.getUnitSkills(curCL);
		UnitSkillData usd = UnitSkillData.getData(curGroup.unitSkillId);
		if(!uniteSkillList.Contains(usd))
		{
			//curGroup.unitSkillId = -1;
		}
		
		//==此处截断,向服务器保存一下卡组数据以获取最新的战斗力,之后会在update里调用goNext()方法继续原本的执行流程  @litao=====//
		//修改标志//
		combination.curCardGroup.changeMark = 0;
		
		goNextParam=3;
		requestType=3;
		PlayerInfo.getInstance().sendRequest(new SaveCGJson(combination.curCardGroup),this);
	}
	
	//从当前卡牌身上拆卸物品(技能，武器，防具，饰品等//
	//itemId 当前要卸下的物品在playerInfo的list中的id, cardId,当前要卸下物品的卡牌在卡组的list中id//
	public void RemoveItemFromCard(int itemId, int cardId)
	{
		tempCardGroup =combination.curCardGroup.Clone();
		
		//==如果卸下了角色卡,记录卸下前的合体技id集合==//
		List<int> oldUnitIds=null;
		if(curSelTypeId==STATE.SHOW_SCROLLVIEW_TYPE_CARD)
		{
			List<int> cardIds=combination.curCardGroup.getCardIds();
			oldUnitIds=UnitSkillData.getUnitSkillIds(cardIds);
		}
		
		PackElement[] dBCardArr = combination.curCardGroup.cards;
		PackElement desCard = dBCardArr[cardId];
		//当前这张卡牌在卡组中的位置//
		int index = -1;
		for(int i =0 ;i < dBCardArr.Length;i++)
		{
			PackElement dbc = dBCardArr[i];
			if(dbc == desCard)
			{
				index = i;
			}
		}
		if(index > -1){
			
			switch(curSelTypeId)
			{
			case STATE.SHOW_SCROLLVIEW_TYPE_CARD:		//卡牌//
				
				CleanItemForCard(index);
				break;
			case STATE.SHOW_SCROLLVIEW_TYPE_ACTIVESKILL:		//主动技能//
				PackElement[] dBSkill = combination.curCardGroup.skills;
				dBSkill[index] = null;
				break;
			case STATE.SHOW_SCROLLVIEW_PS1:
			case STATE.SHOW_SCROLLVIEW_PS2:
			case STATE.SHOW_SCROLLVIEW_PS3:
			{
				
				for(int i = 0 ; i < combination.curCardGroup.passiveSkills[index].Count;++i)
				{
					if( i == (curSelTypeId - 6))
					{
						combination.curCardGroup.passiveSkills[index][i] = null;
					}
				}
			}break;
			case STATE.SHOW_SCROLLVIEW_TYPE_WEAPON:
			case STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON:			//装备//
			case STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS:
				List<PackElement>[] dBEquip = combination.curCardGroup.equips;
				
				List<PackElement> dbeList = dBEquip[index];
				{
					
					for(int i = 0;i < dbeList.Count ;){
						PackElement dbe = dbeList[i];
						EquipData ed = EquipData.getData(dbe.dataId);
						if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_WEAPON ){
							if(ed.type == 1){
								//							dbe = null;
								dbeList.Remove(dbe);
							}
							else {
								i++;
							}
						}
						else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_PROWEAPON){
							if(ed.type == 2){
								//							dbe = null;
								dbeList.Remove(dbe);
							}
							else {
								i++;
							}
						}
						else if(curSelTypeId == STATE.SHOW_SCROLLVIEW_TYPE_ORNAMENTS){
							if(ed.type == 3){
								//							dbe = null;
								dbeList.Remove(dbe);
							}
							else {
								i ++;
							}
						}
						
					}
				}
				
				break;
				
			}
		}
		
		//==此处截断,向服务器保存一下卡组数据以获取最新的战斗力,之后会在update里调用goNext()方法继续原本的执行流程  @litao=====//
		//==如果卸下了角色卡,记录卸下前的合体技id集合==//
		if(curSelTypeId==STATE.SHOW_SCROLLVIEW_TYPE_CARD)
		{
			List<int> cardIds=combination.curCardGroup.getCardIds();
			List<int> newUnitIds=UnitSkillData.getUnitSkillIds(cardIds);
			int oldUnitId=combination.curCardGroup.unitSkillId;
			if(newUnitIds.Count>0)
			{
				if(!newUnitIds.Contains(oldUnitId))
				{
					//combination.unitMark=1;
					//combination.curCardGroup.unitSkillId=newUnitIds[0];
				}
				else
				{
					foreach(int newUnitId in newUnitIds)
					{
						if(!oldUnitIds.Contains(newUnitId))
						{
							//combination.unitMark=1;
							break;
						}
					}
					
					//changeUniteByPower(newUnitIds);
				}
			}
			else
			{
				if(oldUnitId!=0)
				{
					combination.unitMark=1;
				}
				//combination.curCardGroup.unitSkillId=0;
			}
		}
		
		
		//修改标志//
		combination.curCardGroup.changeMark = 0;
		
		goNextParam=1;
		requestType=3;
		PlayerInfo.getInstance().sendRequest(new SaveCGJson(combination.curCardGroup),this);
	}
	
	//private int battlePower;
	private int goNextParam;
	private void goNext()
	{
		switch(goNextParam)
		{
		case 1:
			//重新绘制//
			ChangeCardInfoData();
			//重新传list//
			//OpenScrollView(curSelTypeId);
			break;
		case 2:
			//重新绘制//
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam))
			{
				GuideUI_CardInTeam.mInstance.showStep(5);
			}
			else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_CardInTeam2))
			{
				GuideUI_CardInTeam2.mInstance.showStep(5);
			}
			else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip))
			{
				GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Equip);
				GuideUI_IntensifyEquip.mInstance.normalType = false;
				UISceneDialogPanel.mInstance.showDialogID(44);
			}
			else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
			{
				GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Skill);
			}
			ChangeCardInfoData();
			break;
		case 3:
			//重新绘制//
			ChangeCardInfoData();
			break;
		}
		//ciControl.battlePower.text=TextsData.getData(203).chinese+battlePower;
	}
	
	//将当前的卡牌从当前的卡组界面中拆卸下来
	public void RemoveCardFromCardGroup(){

		CleanItemForCard(curCardBoxId);
		ChangeCardInfoData();
	}
	
	//卸下卡牌时清空卡牌身上的物品 index 卡牌在卡组中的索引//
	public void CleanItemForCard(int index)
	{
		CardGroup curG = combination.curCardGroup;
		curG.cards[index] = null;
		curG.skills[index] = null;
		curG.passiveSkills[index] = null;
		if(curG.equips[index] != null)
		{
			curG.equips[index].Clear();
		}
		curG.equips[index] = null;
		
	}
	
	public void CleanScrollData()
	{
		InfoScrollBar.GetComponent<UIScrollBar>().value = 0;
		//InfoScrollView.transform.localPosition = Vector3.zero;
		//InfoScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,360,350);
		
	}
	
	public void onClickBigBlackBg()
	{
//		ScrollViewPanel.mInstance.close();
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		if(scrollView!=null&&canClickBlackBg)
		{
			Debug.Log("1111111111111111111");
			scrollView.close();
			canClickBlackBg = false;
		}
	}
	
	//切换下一个卡牌信息//
	public void onClickNextCard(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(param == 1)
		{
			curCardBoxId ++;
		}
		else if(param == -1)
		{
			curCardBoxId --;
		}
		
		for(int i = 0;i<7;i++)
		{
			
			if(curCardBoxId>combination.curCardGroup.cards.Length-1)
			{
				curCardBoxId = 0;
			}
			if(curCardBoxId<0)
			{
				curCardBoxId = combination.curCardGroup.cards.Length-1;
			}
			
			PackElement card = combination.curCardGroup.cards[curCardBoxId];
			
			if(card == null)
			{
				if(param == 1)
				{
					curCardBoxId ++;
				}
				else if(param == -1)
				{
					curCardBoxId --;
				}
			}
			else
			{
				break;
			}
		}


		pageLabel.text = (curCardBoxId+1) + "/6" ;
		checkIsEmptyCard = combination.curCardGroup.cards[curCardBoxId];
		if(curType != 2)
		{
			requestType = 10;
			UIJson uijson = new UIJson();
			uijson.UIJsonForCardInfo(STATE.UI_CARDINFO,curCardBoxId);
			PlayerInfo.getInstance().sendRequest(uijson,this);
		}
		else
		{
			ChangeCardInfoData();
		}

	}
	
	public void setCanClickBlackBg()
	{
		canClickBlackBg = true;
	}
	
	public void receiveResponse(string json)
	{
		Debug.Log("json ============= " + json);
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				PackResultJson prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode == 0)
				{
					list=prj.list;
				}
				receiveData=true;
				break;
			case 2:
				PlayerResultJson playerRJ = JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode = playerRJ.errorCode;
				PlayerElement pe=playerRJ.list[0];
				PlayerInfo.getInstance().player=pe;
				receiveData=true;
				break;
			case 3:
				cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				receiveData=true;
				break;
			case 10:
			case 11:
				cirj = JsonMapper.ToObject<CardInfoResultJson>(json);
				errorCode = cirj.errorCode;;
                PlayerInfo.getInstance().player.diamond = cirj.pd;
                this.diamonds = cirj.pd;
                this.cardN = cirj.cardN;
                this.diamond = cirj.diamond;
                this.pcardN = cirj.pCardN;
                this.multCard = cirj.multCard;
                cards = cirj.pes;
				receiveData = true;
				break;
            case 12:
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
                break;
            case 15:
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
            case 16:
                breakJson = JsonMapper.ToObject<BreakResultJson>(json);
                errorCode = breakJson.errorCode;
				allCells.Clear();
                if (errorCode == 0)
                {
                    combination.curCardGroup.cards[curCardBoxId] = breakJson.pe;
                    PlayerInfo.getInstance().player.diamond = breakJson.pd;
                    this.cardN = breakJson.cn;
                    this.diamond = breakJson.d;
                    this.pcardN = breakJson.pcn;
                    this.multCard = breakJson.pmc;
                    this.diamonds = breakJson.pd;
                    this.sell = breakJson.sell;
                    cards = breakJson.pes;
                }
				receiveData = true;
                break;
			}
		}
	}
	
}
