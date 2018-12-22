using UnityEngine;
using System.Collections;


// 2d card simple info
// pack card , intensify pack card,intensify consume card, compose pack card, compose consume item card and so on...

public class SimpleCardInfo1 : MonoBehaviour {
	
	public GameObject child;
	
	public UILabel cardNameLabel;
	public UISprite bgSprite;
	public UISprite frameSprite;
	public UISprite raceSprite;
	public UISprite iconHeroSprite;
	public UISprite iconOtherSprite;
	public UISprite iconFragment;
	public UILabel levelLabel;
	public UIButtonMessage bm;

	public UILabel haveLabel;
 	public UILabel cardUserLabel;
	public UILabel composeLockLabel;
	public GameObject topNewObj;
	
	public int cardId ;
	public GameHelper.E_CardType ct;
	public UIAtlas atl;
	public PackElement pe;
	
	public void clear()
	{
		cardId = 0;
		ct = GameHelper.E_CardType.E_Null;
		atl = null;
		pe = null;
		cardNameLabel.text = string.Empty;
		if(iconHeroSprite != null)
		{
			iconHeroSprite.atlas = null;
			iconHeroSprite.spriteName = "";
			iconHeroSprite.gameObject.SetActive(false);
		}
		if(iconOtherSprite != null)
		{
			iconOtherSprite.atlas = null;
			iconOtherSprite.spriteName = "";
			iconOtherSprite.gameObject.SetActive(false);
		}
		if(bgSprite != null)
		{
			bgSprite.spriteName = "";
			bgSprite.gameObject.SetActive(false);
		}
		if(frameSprite != null)
		{
			frameSprite.spriteName = "";	
			frameSprite.gameObject.SetActive(false);
		}
		if(raceSprite != null)
		{
			raceSprite.spriteName = "";
			raceSprite.gameObject.SetActive(false);
		}
		if(iconFragment!=null)
		{
			iconFragment.gameObject.SetActive(false);
		}
		if(bm != null)
		{
			bm.param = -1;
		}
		if(levelLabel != null && levelLabel.gameObject != null)
		{
			levelLabel.text = string.Empty;
		}
		if(haveLabel != null && haveLabel.gameObject != null)
		{
			haveLabel.text = string.Empty;
		}
		if(composeLockLabel != null)
		{
			composeLockLabel.gameObject.SetActive(false);
			composeLockLabel.text = "";
		}
		if(topNewObj != null)
		{
			topNewObj.SetActive(false);
		}
		child.SetActive(false);
	}
	
	
	
	//id frameId cardLv，当类型是卡牌，并且pe = null的时候，用cardLv, 去显示人物等级//
	public void setSimpleCardInfo(int id,GameHelper.E_CardType ct,PackElement pe = null, int cardLv = 1)
	{
		clear();
		this.cardId = id;
		this.ct = ct;
		this.pe = pe;
		string name = string.Empty;
		string iconName = string.Empty;
		int starNum = 1;
		int raceType = 1;
		
		if(child!=null)
			child.SetActive(true);
		switch(ct)
		{
		case GameHelper.E_CardType.E_Hero:
		{
			CardData cd = CardData.getData(id);
			if(cd == null)
				return;
			starNum = cd.star;
			raceType = cd.race;
			name = cd.name;
			//显示名字和强化次数//
			if(pe != null && pe.bn > 0)
			{
				cardNameLabel.text = "+" + pe.bn + "\r\n" + name;
			}
			else 
			{
				cardNameLabel.text = name;
			}
			this.atl = LoadAtlasOrFont.LoadHeroAtlasByName(cd.atlas);
			iconName = cd.icon;
			iconHeroSprite.gameObject.SetActive(true);
			iconHeroSprite.atlas = atl;
			iconHeroSprite.spriteName = iconName;
			showHero(id,pe, cardLv);
		}break;
		case GameHelper.E_CardType.E_Skill:
		{
			SkillData sd = SkillData.getData(id);
			if(sd == null)
				return;
			starNum = sd.star;
			//10,主动技能//
			raceType = 10;
			name = sd.name;
			iconName = sd.icon;
			showOtherIcon(iconName,ct);
			//显示名字//
			cardNameLabel.text = name;
			showSkill(pe);
		}break;
		case GameHelper.E_CardType.E_PassiveSkill:
		{
			PassiveSkillData psd = PassiveSkillData.getData(id);
			if(psd == null)
				return;
			starNum = psd.star;
			raceType = 11;
			name = psd.name;
			iconName = psd.icon;
			showOtherIcon(iconName,ct);
			//显示名字//
			cardNameLabel.text = name;
			showPassiveSkill(id);
		}break;
		case GameHelper.E_CardType.E_Equip:
		{
			EquipData ed = EquipData.getData(id);
			if(ed == null)
				return;
			starNum = ed.star;
			raceType = 9;
			name = ed.name;
			iconName = ed.icon;
			showOtherIcon(iconName,ct);
			//显示名字//
			cardNameLabel.text = name;

			showEquip(pe);
		}break;
		case GameHelper.E_CardType.E_Item:
		{
			if(iconFragment==null)
			{
				GameObject iconFragmentGameObject = Instantiate(GameObjectUtil.LoadResourcesPrefabs("UI-FragmentSprite",3)) as GameObject;
				iconFragment = iconFragmentGameObject.GetComponent<UISprite>();
				iconFragment.transform.parent = child.transform;
				iconFragment.transform.localScale = new Vector3(1,1,1);
				
			}
			iconFragment.atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas02");
			iconFragment.spriteName = "pieces";
			iconFragment.transform.localPosition = new Vector3(-30,0,0);
			iconFragment.GetComponent<UIWidget>().height = 52;
			iconFragment.GetComponent<UIWidget>().width = 46;
			ItemsData itemData = ItemsData.getData(id);
			if(itemData == null)
				return;
			starNum = itemData.star;
			raceType = 5;
			name = itemData.name;
			if(itemData.fragment == 1)//碎片//
			{
				iconFragment.gameObject.SetActive(true);
				//1.skill 2.item 3.equip 4.card 5.passiveskill//
				switch(itemData.goodztype)
				{
				case 1:
					ct = GameHelper.E_CardType.E_Skill;
					iconName = SkillData.getData(itemData.goodsid).icon;
					break;
				case 2:
					ct = GameHelper.E_CardType.E_Item;
					iconName = ItemsData.getData(itemData.goodsid).icon;
					break;
				case 3:
					ct = GameHelper.E_CardType.E_Equip;
					iconName = EquipData.getData(itemData.goodsid).icon;
					break;
				case 4:
					ct = GameHelper.E_CardType.E_Hero;
					iconName = CardData.getData(itemData.goodsid).icon;
					break;
				case 5:
					ct = GameHelper.E_CardType.E_PassiveSkill;
					iconName = PassiveSkillData.getData(itemData.goodsid).icon;
					break;
				}
			}
			else
			{
				iconFragment.gameObject.SetActive(false);
				iconName = itemData.icon;	
			}
			if(ct!=GameHelper.E_CardType.E_Hero)
			{
				showOtherIcon(iconName,ct);	
			}
			else
			{
				showHeroIcon(iconName,itemData.goodsid)	;
			}
			//显示名字//
			cardNameLabel.text = name;
            if (pe != null)
            {
                haveLabel.text = TextsData.getData(388).chinese + pe.pile;
                haveLabel.gameObject.SetActive(true);
            }
			if(levelLabel.gameObject !=null)
			{
				levelLabel.gameObject.SetActive(false);
			}
		}break;
		}
		raceSprite.spriteName = "race_" + raceType;
		raceSprite.gameObject.SetActive(true);
		bgSprite.spriteName = "card_big_" + starNum;
		bgSprite.gameObject.SetActive(true);
		frameSprite.spriteName = "head_star_" + starNum;
		frameSprite.gameObject.SetActive(true);
	}
	
	public void showHero(int formID,PackElement pe, int cardLv)
	{
		if(levelLabel.gameObject !=null)
		{
			levelLabel.gameObject.SetActive(true);
			if(pe != null)
			{
				levelLabel.text="LV."+pe.lv;
			}
			else
			{
				levelLabel.text="LV." + cardLv;
			}
		}
	}
	
	public void showSkill(PackElement pe)
	{
		if(levelLabel.gameObject != null )
		{
			levelLabel.gameObject.SetActive(true);
			if(pe != null)
			{
				levelLabel.text="LV."+pe.lv;
			}
			else
			{
				levelLabel.text="LV.1";
			}
		}
	}
	
	public void showPassiveSkill(int formID)
	{
		PassiveSkillData psd = PassiveSkillData.getData(formID);
		if(levelLabel.gameObject != null && psd != null)
		{
			levelLabel.gameObject.SetActive(true);
			levelLabel.text="LV."+psd.level;
		}
	}
	
	public void showEquip(PackElement pe)
	{
		if(levelLabel.gameObject != null )
		{
			levelLabel.gameObject.SetActive(true);
			if(pe != null)
			{
				levelLabel.text="LV."+pe.lv;
			}
			else
			{
				levelLabel.text="LV.1";
			}
		}
	}
	
	public void showLockText(string str)
	{
		if(composeLockLabel != null)
		{
			composeLockLabel.gameObject.SetActive(true);
			composeLockLabel.text = str;
		}
	}
	
	public void clearLockText()
	{
		if(composeLockLabel != null)
		{
			composeLockLabel.text = "";
			composeLockLabel.gameObject.SetActive(false);
		}
	}
	
	public void showTopNewObj()
	{
		if(topNewObj != null)
		{
			topNewObj.SetActive(true);
		}
	}
	
	public void hideTopNewObj()
	{
		if(topNewObj != null)
		{
			topNewObj.SetActive(false);
		}
	}
	
	public void showOtherIcon(string iconName,GameHelper.E_CardType cardType)
	{
		this.atl = GameHelper.getIconOtherAtlas(cardType);
		iconOtherSprite.gameObject.SetActive(true);
		iconOtherSprite.atlas = atl;
		iconOtherSprite.spriteName = iconName;
	}
	
    public void setCardUserInfo(int fromId)
    {
        if (fromId != 0)
        {
            cardUserLabel.gameObject.SetActive(true);
            cardUserLabel.text = CardData.getData(fromId).name + TextsData.getData(262).chinese;
        }
        else
        {
            cardUserLabel.gameObject.SetActive(false);
        }

    }
	
	public void setNumText(int num)
	{
		if(haveLabel == null)
			return;
		if(num > 0)
		{
			haveLabel.gameObject.SetActive(true);
			haveLabel.text = num.ToString();
		}
		else
		{
			haveLabel.gameObject.SetActive(false);
		}
	}
	
	public void showHeroIcon(string iconName,int cardId)
	{
		CardData cd = CardData.getData(cardId);
		this.atl = LoadAtlasOrFont.LoadAtlasByName(cd.atlas);
		iconHeroSprite.gameObject.SetActive(true);
		iconHeroSprite.atlas = atl;
		iconHeroSprite.spriteName = iconName;
	}

}
