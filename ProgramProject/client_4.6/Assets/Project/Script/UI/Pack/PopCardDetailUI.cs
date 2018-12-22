using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopCardDetailUI : BWUIPanel {
	
	public enum PCDPType : int
	{
		E_Null = -1,
		E_Normal = 0,
		E_Passive = 1,
	}
	
	PCDPType pageType = PCDPType.E_Null;
	
	public GameObject passivePageNode;
	public GameObject normalPageNode;
	
	//0主动技能,1防具,2武器,3饰品,4 ps1,5 ps2,6 ps3//
	public GameObject[] Boxs;
	
	public PopOtherDetailUI popOtherDetail;
	public CardInfoContrl ciControl;
	
	private GameObject CardModelPos;
	private PackElement card;
	private PackElement skill;
	private List<PackElement> pSkillList;
	private List<PackElement> equipsList;
	
	public GameObject card3DNode;
	public UILabel card3DName;
	public UISprite card3DStarIcon;
    public UISprite psSkillIcon;


    public UILabel psSkillLbael;

    public GameObject psSkillBox;
	
	public bool fromLot;
	
	public bool fromActLot;
	
	public GameObject clickCardObj;
	
	public RotateCard rotateCard;
	
	public int cardDataID = 0;
	public bool isOnlyShowCardData = false;
	
	void Awake()
	{
		
		init();
	}
	
	void Start()
	{
		if(isOnlyShowCardData)
		{
			return;
		}
		hide();
	}
	
	public override void hide()
	{
		clear();
		base.hide();
		isOnlyShowCardData = false;
	}
	
	public override void show()
	{
		base.show();
		showCardInfo();
		showPCDPType(PCDPType.E_Normal);
		//不显示head//
		HeadUI.mInstance.hide();
	}
	
	public void showPCDPType( PCDPType t)
	{
		if(t == pageType)
			return;
		pageType = t;
		if(pageType == PCDPType.E_Normal)
		{
			passivePageNode.SetActive(false);
			normalPageNode.SetActive(true);
		}
		else if(pageType == PCDPType.E_Passive)
		{
			passivePageNode.SetActive(true);
			normalPageNode.SetActive(false);
		}
	}
	
	public void clear()
	{
		if(rotateCard != null)
		{
			rotateCard.gc();
		}
		GameObjectUtil.destroyGameObjectAllChildrens(card3DNode);
		card3DName.text = "";
		card3DStarIcon.spriteName = "";
		for(int i = 0; i < Boxs.Length;++i)
		{
			if(Boxs[i] == null)
				continue;
			Boxs[i].GetComponent<SimpleCardInfo2>().clear();
		}
		clickCardObj.SetActive(true);
		pageType = PCDPType.E_Null;
	}
	
	public void showCardInfo()
	{
		clear();
		CardData cd = null;
		if(!isOnlyShowCardData)
		{
			if (card == null)
	        {
	            clickCardObj.SetActive(false);
	            return;
	        }
			cd  = CardData.getData(card.dataId);
		}
		else
		{
			cd  = CardData.getData(cardDataID);
		}
        
		if(cd == null)
			return;
		GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,STATE.PREFABS_TYPE_CARD))as GameObject;
		if(cardModel == null)
			return;
		clickCardObj.SetActive(false);
		GameObjectUtil.gameObjectAttachToParent(cardModel,card3DNode,true);
		GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
		cardModel.transform.localPosition = new Vector3(0,cd.modelposition,0);
		cardModel.transform.localScale = new Vector3(cd.modelsize,cd.modelsize,cd.modelsize);
		float rotaY = cd.modelrotation;
		cardModel.transform.localEulerAngles =new Vector3(0,rotaY,0);
		GameObjectUtil.hideCardEffect(cardModel);
//		card3DName.text = "LV." + card.lv+"  " +cd.name;
	
		if(!isOnlyShowCardData)
		{
			if( card.bn > 0)
			{
				card3DName.text = "LV." + card.lv+"  " +cd.name + "+" + card.bn;
			}
			else 
			{
				card3DName.text = "LV." + card.lv+"  " +cd.name;
			}
		}
		else
		{
			card3DName.text = "LV.1" +"  " +cd.name;
		}

		card3DStarIcon.spriteName = "card_side_s"+cd.star.ToString();
		rotateCard.setCard3DObj(cardModel);
		//三围属性//
		int atk = 0; 
		int def = 0;
		int hp = 0;
		if(!isOnlyShowCardData)
		{
			atk = (int)Statics.getCardSelfMaxAtkForUI(card.dataId, card.lv, card.bn);
			def = (int)Statics.getCardSelfMaxDefForUI(card.dataId, card.lv, card.bn);
			hp = (int)Statics.getCardSelfMaxHpForUI(card.dataId, card.lv, card.bn);
		}
		else
		{
			atk = (int)Statics.getCardSelfMaxAtkForUI(cardDataID, 1, 0);
			def = (int)Statics.getCardSelfMaxDefForUI(cardDataID, 1, 0);
			hp = (int)Statics.getCardSelfMaxHpForUI(cardDataID, 1, 0);
		}
		//主动技能//
		int activeSkillId = 0;
		int skillLevel=0;
		if(skill!=null)
		{
			activeSkillId = skill.dataId;
			skillLevel=skill.lv;
		}
		else
		{
			activeSkillId = cd.basicskill;
			skillLevel=1;
		}
		Boxs[0].GetComponent<SimpleCardInfo2>().clear();
		Boxs[0].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(activeSkillId,GameHelper.E_CardType.E_Skill);
		//被动技能//
        if (cd.PSSskilltype1 != 0)
        {
            if (!psSkillBox.activeSelf)
                psSkillBox.SetActive(true);
            psSkillLbael.text = cd.PPSname;
            psSkillIcon.spriteName = cd.PSSicon;
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
		if(pSkillList!=null)
		{
			for(int i = 0 ; i < pSkillList.Count;++i)
			{
				if(pSkillList[i] == null)
					continue;
				Boxs[i+4].GetComponent<SimpleCardInfo2>().clear();
				Boxs[i+4].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(pSkillList[i].dataId,GameHelper.E_CardType.E_PassiveSkill);
				PassiveSkillData adhpsd = PassiveSkillData.getData(pSkillList[i].dataId);
				if(adhpsd == null)
					continue;
//				switch(adhpsd.type)
//				{
//					case 1://攻击力//
//					atk += adhpsd.numbers;
//					break;
//				case 2://防御力//
//					def += adhpsd.numbers;
//					break;
//				case 3://生命值//
//					hp += adhpsd.numbers;
//					break;
//				}
			}
		}
		//用来存储装备拼接的字符串格式为equipId-level//
		//List<string> equipInfo = new List<string>();
		for(int i = 0;equipsList!=null && i < equipsList.Count; i++)
		{
			PackElement pe=equipsList[i];
			EquipData ed = EquipData.getData(pe.dataId);
			Boxs[ed.type].GetComponent<SimpleCardInfo2>().clear();
			Boxs[ed.type].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(pe.dataId,GameHelper.E_CardType.E_Equip);
			
			//EquippropertyData epd = EquippropertyData.getData(ed.type,pe.lv);
//			switch(ed.type)
//			{
//			case 1://攻击力//
//				atk += epd.starNumbers[ed.star-1];
//				break;
//			case 2://防御力//
//				def += epd.starNumbers[ed.star-1];
//				break;
//			case 3://生命值//
//				hp += epd.starNumbers[ed.star-1];
//				break;
//			}
		}
		//基本信息//
		string skillAddData = Statics.getSkillValueForUIShow02(activeSkillId, skillLevel);
		if(!isOnlyShowCardData)
		{
			ciControl.SetData(card.dataId, activeSkillId, card.bn, card.bp,atk,def,hp,0,0,0, skillAddData);
		}
		else
		{
			ciControl.SetData(cardDataID, activeSkillId, 1, 0,atk,def,hp,0,0,0, skillAddData);
		}

	}
	
	//0卡牌按钮,1主动技能按钮,3武器,4防具,5饰品,6 ps1,7 ps2,8 ps3 ,15 ps4//
	public void SelectBtnCallBack(int type)
	{
		switch(type)
		{
		case 1:
		{
			if(skill!=null)
			{
				PackElement pe=skill;
				SkillData sd=SkillData.getData(pe.dataId);
				if(sd!=null)
				{
					string skillAddData = Statics.getSkillValueForUIShow02(pe.dataId, pe.lv);
					popOtherDetail.setContentNew(pe.eType,GameHelper.E_CardType.E_Skill,pe.dataId,sd.name,pe.lv.ToString(),sd.star.ToString(),sd.getElementName(),sd.description,sd.sell.ToString(),skillAddData);
				}
			}
			else
			{
				CardData cd= null;
				if(!isOnlyShowCardData)
				{
					cd = CardData.getData(card.dataId);
				}
				else
				{
					cd = CardData.getData(cardDataID);
				}
				if(cd == null )
					return;
				
				SkillData sd=SkillData.getData(cd.basicskill);
				if(sd!=null)
				{
					int skillLevel=1;
					string skillAddData = Statics.getSkillValueForUIShow02(sd.index, skillLevel);
					popOtherDetail.setContentNew(2,GameHelper.E_CardType.E_Skill,cd.basicskill,sd.name,skillLevel.ToString(),sd.star.ToString(),sd.getElementName(),sd.description,sd.sell.ToString(),skillAddData);
				}
			}
		}break;
		case 3:
		case 4:
		case 5:
		{
			if(equipsList!=null)
			{
				PackElement pe=null;
				EquipData ed=null;
				foreach(PackElement temp in equipsList)
				{
					EquipData tempD=EquipData.getData(temp.dataId);
					if(tempD.type+2==type)
					{
						pe=temp;
						ed=tempD;
						break;
					}
				}
				if(pe!=null)
				{
					string equipAddData = Statics.getEquipValueForUIShow(pe.dataId, pe.lv);
					popOtherDetail.setContentNew(pe.eType,GameHelper.E_CardType.E_Equip,pe.dataId,ed.name,pe.lv.ToString(),ed.star.ToString(),Statics.getEquipValue(ed,pe.lv).ToString(),ed.description,ed.sell.ToString(),equipAddData);
				}
			}	
		}break;
		case 6:
		{
			showPCDPType(PCDPType.E_Passive);
			UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
		}break;
		case 7:
		{
			showPCDPType(PCDPType.E_Normal);
			UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
		}break;
		case 8:
		case 9:
		case 10:
		{
			if(pSkillList == null)
				return;

			if(pSkillList[type - 8] == null)
			{
				return;
			}
			PassiveSkillData pd=PassiveSkillData.getData(pSkillList[type - 8].dataId);
			if(pd!=null)
			{
				popOtherDetail.setContentNew(3,GameHelper.E_CardType.E_PassiveSkill,pSkillList[type - 8].dataId,pd.name,pd.level.ToString(),pd.star.ToString(),"",pd.describe,pd.sell.ToString());
			}
		}break;
        case 15:
            {
				CardData cd= null;
				if(!isOnlyShowCardData)
				{
					cd = CardData.getData(card.dataId);
				}
				else
				{
					cd = CardData.getData(cardDataID);
				}
				if(cd == null )
					return;
                if (cd.PSSskilltype1 == 0)
                    return;

                popOtherDetail.showPssSkill(cd.PPSname, cd.PPSdescription, cd.PSSicon, "UnitSkillIcon");
            }break;
		}
	}
	
	//返回按钮 返回到卡组界面//
	public void back()
	{
		card=null;
		skill=null;
		pSkillList=null;
		equipsList=null;
		if(!isOnlyShowCardData)
		{
			if(fromActLot)
			{
				fromActLot = false;
				ActivityPanel actPanel = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE, "ActivityPanel")as ActivityPanel;
				actPanel.cardDetailBack();
			}
			else
			{
				if(fromLot)
				{
					fromLot=false;
		//			LotCardUI.mInstance.cardDetailBack();
					LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
					lotCard.cardDetailBack();
				}
				else
				{
		//			PackUI.mInstance.pack.SetActive(true);
					PackUI packUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_PACK, "PackUI")as PackUI;
					if(packUI != null)
					{
						packUI.pack.SetActive(true);
					}
					
					HeadUI.mInstance.show();
				}
			}
		}
		else
		{
			HeadUI.mInstance.show();
		}
		
		if(LotActivityPanel.isHaveBtnClick)
		{
			LotActivityPanel.isHaveBtnClick = false;
		}
		hide();
	}
	
	//uniteSkillId 为合体技的id, 用来显示合体技的详细内容//
	//public void SkillBoxBtnListener(int uniteSkillId)
	//{
	//	if(uniteSkillId > 0)
	//	{
	//		UnitSkillData usd = UnitSkillData.getData(uniteSkillId);
	//	}
	//}
	
	public void setContent(PackElement card,PackElement skill,List<PackElement> psList,List<PackElement> equips)
	{
		isOnlyShowCardData = false;
		this.card=card;
		this.skill=skill;
		this.pSkillList=psList;
		this.equipsList=equips;
		show();
	}
	
	public void setOnlyCardDataContent(int cardID)
	{
		this.card = null;
		cardDataID = cardID;
		skill = null;
		pSkillList = null;
		equipsList = null;
		isOnlyShowCardData = true;
		show();
	}
	
}
