using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//2014.08。29 由ko兑换界面改为常规的卡牌详细信息 @zhangsai//
public class KOawardElement
{
	public int id;
	public int rewardtype;//兑换类型//
	public string reward;//奖励：CardId,num//
	public int state;//0可兑换，1已经兑换//
}

public class ChangeCardDetail : BWUIPanel
{
	public enum CCDPType : int
	{
		E_Null = -1,
		E_Normal = 0,
		E_Passive = 1,
	}
	
	CCDPType pageType;
	
	public GameObject passivePageNode;
	public GameObject normalPageNode;
	
	
	
	//0主动技能,1被动技能,2防具,3武器,4饰品//
	public GameObject[] Boxs;
	
	public List<KOawardElement> keList = new List<KOawardElement>();
	
	public ChangeCardDetail changeCardDetail;
	
	public CardInfoContrl ciControl;
	
	private GameObject CardModelPos;
	
	public GameObject card3DNode;
	public UILabel card3DName;
	public UISprite card3DStarIcon;
	
	public int haveNum = 0;
	
	//int requestType =0;
	
	bool receiveData;
	
	int exchangeId = 0;
	
	public int numName = 0;
	
	bool isCanExchange;
	
	bool isHaveEnoughNum;
	
	public KoAward koAward;
	
	public MissionUI missionui;
	
	int errorCode;
	
	CardGroupResultJson cardGroupRJ;
	
	GameObject effects;//粒子效果父对象//
	
	public RotateCard rotateCard;
	
	public PopOtherDetailUI popOtherDetail;
	
	void Awake()
	{
		init();
	}
	
	public override void init()
	{
		base.init();
	}
	
	public override void hide()
	{
		clear();
		base.hide();
	}
	
	public override void show()
	{
		base.show();
		showCardInfo();
		showCCDPType(CCDPType.E_Normal);
		//不显示head//
		HeadUI.mInstance.hide();
	}
	
	public void clear()
	{
		pageType = CCDPType.E_Null;
		if(rotateCard != null)
		{
			rotateCard.gc();
		}
		GameObjectUtil.destroyGameObjectAllChildrens(card3DNode);
		card3DName.text = "";
		card3DStarIcon.spriteName = "";
		
		for(int i = 0; i < Boxs.Length;++i)
		{
			Boxs[i].GetComponent<SimpleCardInfo2>().clear();
		}
	}
    public GameObject psSkillBox;

    public UISprite psSkillIcon;
    
    public UILabel psSkillLbael;
	public void showCardInfo()
	{
		clear();	

        KOAwardData kad = KOAwardData.getData(exchangeId);
		string[] aa = kad.reward1.Split(',');
		int kocardId = Convert.ToInt32(aa[0]);
        CardData data = CardData.getData(kocardId);
        // iconCards.atlas = LoadAtlasOrFont.LoadAtlasByName(data.atlas);
        if (data.PSSskilltype1 != 0)
        {
            if (!psSkillBox.activeSelf)
                psSkillBox.SetActive(true);
            psSkillLbael.text = data.PPSname;
            psSkillIcon.spriteName = data.PSSicon;
        }
        else
        {
            psSkillBox.SetActive(false);
        }
	
//		if(card == null)
//			return;
		CardData cd = CardData.getData(kocardId);
		if(cd == null)
			return;
		GameObject cardModel = Instantiate(GameObjectUtil.LoadResourcesPrefabs(cd.cardmodel,STATE.PREFABS_TYPE_CARD))as GameObject;
		if(cardModel == null)
			return;
		GameObjectUtil.gameObjectAttachToParent(cardModel,card3DNode,true);
		GameObjectUtil.setGameObjectLayer(cardModel,STATE.LAYER_ID_NGUI);
		cardModel.transform.localPosition = new Vector3(0,cd.modelposition,0);
		cardModel.transform.localScale = new Vector3(cd.modelsize,cd.modelsize,cd.modelsize);
		float rotaY = cd.modelrotation;
		cardModel.transform.localEulerAngles =new Vector3(0,rotaY,0);
		GameObjectUtil.hideCardEffect(cardModel);
		card3DName.text = cd.name;
		card3DStarIcon.spriteName = "card_side_s"+cd.star.ToString();
		if(rotateCard != null)
		{
			rotateCard.setCard3DObj(cardModel);
		}
		
		//if(haveNum>=kad.number)
		//{
		//	isHaveEnoughNum = true;
		//}
		//else
		//{
		//	isHaveEnoughNum = false;
		//}
		
		//基本信息//
		string skillAddData = Statics.getSkillValueForUIShow02(cd.basicskill, 1);
		
		//三围属性//
		int atk = (int)Statics.getCardSelfMaxAtkForUI(kocardId, 1, 0);
		int def = (int)Statics.getCardSelfMaxDefForUI(kocardId, 1, 0);
		int hp = (int)Statics.getCardSelfMaxHpForUI(kocardId, 1, 0);
		
		ciControl.isKoExchange = true;
		
		ciControl.SetData(kocardId, cd.basicskill, 0,0,atk,def,hp,0,0,0, skillAddData);
		
		if(exchangeId == 0)
		{
			return;
		}
		
		//主动技能//
		int activeSkillId = 0;
		//int skillLevel=0;
		activeSkillId = cd.basicskill;
		//skillLevel=1;
		Boxs[0].GetComponent<SimpleCardInfo2>().clear();
		Boxs[0].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(activeSkillId,GameHelper.E_CardType.E_Skill);
	}
	
	//返回按钮 返回到兑换界面//
	public void back()
	{
		hide();
		koAward.show();
		transform.parent.GetComponent<MissionUI>().onClickKOAwardBtn();
		HeadUI.mInstance.show();
		//显示推图的积分兑换界面//
	}
	
	public void showCCDPType(CCDPType t)
	{
		if(pageType == t)
			return;
		pageType = t;
		if(pageType == CCDPType.E_Normal)
		{
			passivePageNode.SetActive(false);
			normalPageNode.SetActive(true);
		}
		else if(pageType == CCDPType.E_Passive)
		{
			passivePageNode.SetActive(true);
			normalPageNode.SetActive(false);
		}
	}

	public void setContent(int exchangeId)
	{
		this.exchangeId = exchangeId;
		show();
	}
	














	//0卡牌按钮,1主动技能按钮,2 passive page, 3 normal page //
	public void SelectBtnCallBack(int type)
    {
        KOAwardData kad = KOAwardData.getData(exchangeId);
        string[] aa = kad.reward1.Split(',');
        int kocardId = Convert.ToInt32(aa[0]);
		switch(type)
		{
		case 0:
		{
			
		}break;
		case 1:
		{
			CardData cd=CardData.getData(kocardId);
			SkillData sd=SkillData.getData(cd.basicskill);
			if(sd!=null)
			{
				int skillLevel=1;
				string skillAddData = Statics.getSkillValueForUIShow02(sd.index, skillLevel);
				popOtherDetail.setContentNew(2,GameHelper.E_CardType.E_Skill,cd.basicskill,sd.name,skillLevel.ToString(),sd.star.ToString(),sd.getElementName(),sd.description,sd.sell.ToString(),skillAddData);
			}
		}break;
		case 2:
		{
			showCCDPType(CCDPType.E_Passive);
			UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
		}break;
		case 3:
		{
			showCCDPType(CCDPType.E_Normal);
			UISceneEffectNodeManager.mInstance.showChangeCardInfoPageEffect();
		}break;

        case 5:
		{
			CardData data = CardData.getData(kocardId);
            if (data.PSSskilltype1 == 0)
                return;
            popOtherDetail.showPssSkill(data.PPSname, data.PPSdescription, data.PSSicon, "UnitSkillIcon");
		}break;
		}
		
	}
	

}
