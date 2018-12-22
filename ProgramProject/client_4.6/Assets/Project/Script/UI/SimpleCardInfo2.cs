using UnityEngine;
using System.Collections;

public class SimpleCardInfo2 : MonoBehaviour {

	public GameObject child;
	public UISprite frameSprite;
	public UISprite iconHeroSprite;
	public UISprite iconOtherSprite;
	
	public int cardId;
	public GameHelper.E_CardType ct;
	public PackElement pe;
	public UIAtlas atl;
	
	public UISprite iconFragment;

	public void clear()
	{
		cardId = 0;
		ct = GameHelper.E_CardType.E_Null;
		pe = null;
		atl = null;
		
		if(iconHeroSprite != null)
		{
			iconHeroSprite.atlas = null;
			iconHeroSprite.spriteName = "";
		}
		if(iconOtherSprite != null)
		{
			iconOtherSprite.atlas = null;
			iconOtherSprite.spriteName = "";
		}
		if(frameSprite != null)
		{
			frameSprite.spriteName = "";	
			frameSprite.gameObject.SetActive(false);
		}
		if(iconFragment!=null)
		{
			iconFragment.gameObject.SetActive(false);
		}
	}
    public void setPssSkillIcon(string iconName,string atlas)
    {
        this.atl = LoadAtlasOrFont.LoadHeroAtlasByName(atlas);
        iconOtherSprite.gameObject.SetActive(true);
        iconOtherSprite.atlas = atl;
        iconOtherSprite.spriteName = iconName;
		frameSprite.gameObject.SetActive(false);
        iconFragment.gameObject.SetActive(false);
        
    }
	// 使用时候清空牌框留空底的话 要先调用下clear() // 
	public void setSimpleCardInfo(int id,GameHelper.E_CardType cardType,PackElement packE = null)
	{
		clear();
		this.cardId = id;
		this.ct = cardType;
		this.pe = packE;
		int starNum = -1;
		string iconName = string.Empty;
		switch(ct)
		{
		case GameHelper.E_CardType.E_Hero:
		{
			CardData cd = CardData.getData(cardId);
			if(cd == null)
				return;
			starNum = cd.star;
			this.atl = LoadAtlasOrFont.LoadHeroAtlasByName(cd.atlas);
			iconName = cd.icon;
			iconHeroSprite.gameObject.SetActive(true);
			iconHeroSprite.atlas = atl;
			iconHeroSprite.spriteName = iconName;
		}break;
		case GameHelper.E_CardType.E_Equip:
		{
			EquipData ed = EquipData.getData(cardId);
			if(ed == null)
				return;
			starNum = ed.star;
			iconName = ed.icon;
			showOtherIcon(iconName,ct);
		}break;
		case GameHelper.E_CardType.E_Item:
		{
			if(iconFragment==null)
			{
				GameObject prefab = GameObjectUtil.LoadResourcesPrefabs("UI-FragmentSprite",3);
				GameObject iconFragmentGameObject = Instantiate(prefab) as GameObject;
				iconFragment = iconFragmentGameObject.GetComponent<UISprite>();
				if(child!=null)
				{
					iconFragment.transform.parent = child.transform;
				}
				
				iconFragment.transform.localScale = new Vector3(1,1,1);
				
			}
			iconFragment.atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas02");
			iconFragment.spriteName = "pieces";
			iconFragment.transform.localPosition = new Vector3(-30,-50,0);
			iconFragment.GetComponent<UIWidget>().height = 52;
			iconFragment.GetComponent<UIWidget>().width = 46;
			ItemsData itemData = ItemsData.getData(cardId);
			if(itemData == null)
				return;
			starNum = itemData.star;
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
		}break;
		case GameHelper.E_CardType.E_Skill:
		{
			SkillData sd = SkillData.getData(cardId);
			if(sd == null)
				return;
			starNum = sd.star;
			iconName = sd.icon;
			showOtherIcon(iconName,ct);
		}break;
		case GameHelper.E_CardType.E_PassiveSkill:
		{
			PassiveSkillData psd = PassiveSkillData.getData(cardId);
			if(psd == null)
				return;
			starNum = psd.star;
			iconName = psd.icon;
			showOtherIcon(iconName,ct);
		}break;
		}
		frameSprite.spriteName = "head_star_" + starNum;
		frameSprite.gameObject.SetActive(true);
	}
	
	//Use this to add frame icon to "rune" , "power"  and "friend" for special.
	public void setSpecialIconInfo(GameHelper.E_CardType cardType)
	{
		this.ct = cardType;
		this.pe = null;
		//int starNum = -1;
		string iconName = string.Empty;
		switch(ct)
		{
		case GameHelper.E_CardType.E_Rune:
		{
			iconName = "rune";
			showOtherIcon(iconName,ct);
		}break;
		case GameHelper.E_CardType.E_Power:
		{
			iconName = "power";
			showOtherIcon(iconName,ct);
		}break;
		case GameHelper.E_CardType.E_Friend:
		{
			iconName = "friend";
			showOtherIcon(iconName,ct);
		}break;
		}
	}
	
	public void showOtherIcon(string iconName,GameHelper.E_CardType cardType)
	{
		this.atl = GameHelper.getIconOtherAtlas(cardType);
		iconOtherSprite.gameObject.SetActive(true);
		iconOtherSprite.atlas = atl;
		iconOtherSprite.spriteName = iconName;
	}
	
	public void showHeroIcon(string iconName,int cardId)
	{
		CardData cd = CardData.getData(cardId);
		this.atl = LoadAtlasOrFont.LoadAtlasByName(cd.atlas);
		iconHeroSprite.gameObject.SetActive(true);
		iconHeroSprite.atlas = atl;
		iconHeroSprite.spriteName = iconName;
		iconOtherSprite.gameObject.SetActive(false);
	}

}
