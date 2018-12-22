using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHelper
{
	public static bool isTestCritCameraShow = false;
	public static string getCardAttributeString(int type)
	{
		if(type == 0)
			return "";
		return TextsData.getData(type).chinese;
	}
	
	public static string getCardName(PackElement pe)
	{
		string s = string.Empty;
		if(pe != null)
		{
			CardData cd = CardData.getData(pe.dataId);
			if(cd != null)
			{
				if(pe.bn == 0)
				{
					s = cd.name;
				}
				else
				{
					s = cd.name + "+" + pe.bn.ToString();
				}
			}
		}
		return s;
	}
	
	public static string getCardNameNew(PackElement pe,int formID)
	{
		string s = string.Empty;
		if(pe != null )
		{
			string levelS = "LV." + pe.lv.ToString() + " ";
			s += levelS;
			CardData cd = CardData.getData(pe.dataId);
			if(cd != null)
			{
				if(pe.bn == 0)
				{
					s += cd.name;
				}
				else
				{
					string nameS = cd.name + "+" + pe.bn.ToString() ;
					s += nameS;
				}
			}
		}
		else
		{
			s += "LV.1 ";
			CardData cd = CardData.getData(formID);
			if(cd != null)
			{
				s += cd.name;
			}
		}
		return s;
	}
	
	// from pack element
	public static string getEquipAttrDescText(PackElement pe)
	{
		string s = string.Empty;
		if(pe != null)
		{
			EquipData ed = EquipData.getData(pe.dataId);
			if(ed != null)
			{
				int level = pe.lv;
				int star = ed.star;
				switch(ed.type)
				{
				case 1:
				{
					// atk
					s += TextsData.getData(259).chinese + " : ";
				}break;
				case 2:
				{
					// def
					s += TextsData.getData(260).chinese + " : ";
				}break;
				case 3:
				{
					// hp
					s += TextsData.getData(261).chinese + " : ";
				}break;
				}
				EquippropertyData epd = EquippropertyData.getData(ed.type,level);
				s += epd.starNumbers[star -1];
			}
		}
		return s;
	}

	public static string getEquipAttrDescText(int dataID,int level)
	{
		string s = string.Empty;
		EquipData ed = EquipData.getData(dataID);
		if(ed != null)
		{
			int star = ed.star;
			switch(ed.type)
			{
			case 1:
			{
				// atk
				s += TextsData.getData(259).chinese + " : ";
			}break;
			case 2:
			{
				// def
				s += TextsData.getData(260).chinese + " : ";
			}break;
			case 3:
			{
				// hp
				s += TextsData.getData(261).chinese + " : ";
			}break;
			}
			EquippropertyData epd = EquippropertyData.getData(ed.type,level);
			s += epd.starNumbers[star -1];
		}
		return s;
	}
	
	public static int getEquipAttrNum(int dataID,int level)
	{
		int r = 0;
		EquipData ed = EquipData.getData(dataID);
		if(ed != null)
		{
			int star = ed.star;
			EquippropertyData epd = EquippropertyData.getData(ed.type,level);
			if(epd != null)
			{
				r += epd.starNumbers[star -1];
			}
		}
		return r;
	}
	
	// from form
	public static string getEquipAttrDescText(int formID)
	{
		string s = string.Empty;
		EquipData ed = EquipData.getData(formID);
		if(ed != null)
		{
			int level = 1;
			int star = ed.star;
			switch(ed.type)
			{
			case 1:
			{
				// atk
				s += TextsData.getData(259).chinese + " : ";
			}break;
			case 2:
			{
				// def
				s += TextsData.getData(260).chinese + " : ";
			}break;
			case 3:
			{
				// hp
				s += TextsData.getData(261).chinese + " : ";
			}break;
			}
			EquippropertyData epd = EquippropertyData.getData(ed.type,level);
			s += epd.starNumbers[star -1];
		}
		return s;
	}
	
	
	public enum E_CardType : int
	{
		E_Null = 0,
		E_Hero = 1,
		E_Skill = 2,
		E_PassiveSkill = 3,
		E_Equip = 4,
		E_Item = 5,
		E_Rune = 6,
		E_Power = 7,
		E_Friend = 8,
	}
	
	public static UIAtlas getIconOtherAtlas(E_CardType cardType)
	{
		UIAtlas atlas = null;
		string s = "";
		switch(cardType)
		{
		case E_CardType.E_Equip:
		{
			s = "EquipCircularIcon";
		}break;
		case E_CardType.E_Item:
		{
			s = "ItemCircularIcon";
		}break;
		case E_CardType.E_Skill:
		{
			s = "SkillCircularIcom";
		}break;
		case E_CardType.E_PassiveSkill:
		{
			s = "PassSkillCircularIcon";
		}break;
		case E_CardType.E_Rune:
		{
			s = "InterfaceAtlas01";
		}break;
		case E_CardType.E_Power:
		{
			s = "InterfaceAtlas01";
		}break;
		case E_CardType.E_Friend:
		{
			s = "InterfaceAtlas01";
		}break;
		}
		atlas = LoadAtlasOrFont.LoadAtlasByName(s);
		return atlas;
	}
	
	// bn , level 用来强化或者突破 卡牌属性变化时用的数值 // 
	public static void setCardAttr(int cardFormID,PackElement dbc, int cardLevel,int bn,UILabel atkLabel,UILabel defLabel,UILabel hpLabel,int canLevelUpLevel = -1,int canBN = -1)
	{
		// to do
		if(dbc == null )
		{
			CardData cd = CardData.getData(cardFormID);
			if(cd != null)
			{
				int breakNum=bn;
				atkLabel.text = ((int)Statics.getCardSelfMaxAtkForUI(cardFormID, cardLevel, breakNum)).ToString();
				defLabel.text = ((int)Statics.getCardSelfMaxDefForUI(cardFormID, cardLevel, breakNum)).ToString();
				hpLabel.text = ((int)Statics.getCardSelfMaxHpForUI(cardFormID, cardLevel, breakNum)).ToString();
			}
		}
		else
		{
			CardData cd = CardData.getData(dbc.dataId);
			if(cd != null)
			{
				CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager")as CardInfoPanelManager;
				CombinationInterManager  combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP,
						"CombinationInterManager")as CombinationInterManager;
				if(cardInfo != null && cardInfo.gameObject.activeSelf)
				{
					int atk=0;
					int def=0;
					int hp=0;
					/**1.卡牌自身的属性（初始攻防血）,2.卡牌突破增加的属性**/
					atk+=dbc.getSelfAtk();
					def+=dbc.getSelfDef();
					hp+=dbc.getSelfHp();
					/**3.装备属性的变动**/
					CardGroup cg=combination.curCardGroup;
					int index=cg.getIndex(dbc);
					List<PackElement> equips=cg.equips[index];
					if(equips!=null)
					{
						foreach(PackElement equip in equips)
						{
							EquipData ed=EquipData.getData(equip.dataId);
							EquippropertyData epd=EquippropertyData.getData(ed.type,equip.lv);
							switch(ed.type)
							{
							case 1:
								atk+=epd.starNumbers[ed.star-1];
								break;
							case 2:
								def+=epd.starNumbers[ed.star-1];
								break;
							case 3:
								hp+=epd.starNumbers[ed.star-1];
								break;
							}
						}
					}
					/**4.被动技能中单纯增加攻击力与防御力和生命值的，见passiveskill中类型1,2,3**/
					List<PackElement> pSkillList=cg.passiveSkills[index];
					if(pSkillList!=null)
					{
						for(int i = 0 ; i < pSkillList.Count;++i)
						{
							PackElement pSkill = pSkillList[i];
							if(pSkill == null)
								continue;
							PassiveSkillData psd=PassiveSkillData.getData(pSkill.dataId);
							switch(psd.type)
							{
							case 1:
								atk+=psd.numbers;
								break;
							case 2:
								def+=psd.numbers;
								break;
							case 3:
								hp+=psd.numbers;
								break;
							}
						}
						
					}
					/**5.符文值增加的相应属性**/
					string runeId=PlayerInfo.getInstance().player.runeId;
					atk+=Statics.getRuneValue(runeId,1);
					def+=Statics.getRuneValue(runeId,2);
					hp+=Statics.getRuneValue(runeId,3);
					
					hpLabel.text = hp+"";
					atkLabel.text = atk+"";
					defLabel.text = def+"";
				}
				else
				{
					if(canLevelUpLevel == -1)
					{
						if(bn == canBN)
						{
							hpLabel.text = dbc.getSelfHp()+"";
							atkLabel.text = dbc.getSelfAtk()+"";
							defLabel.text = dbc.getSelfDef()+"";
						}
						else
						{
							hpLabel.text = dbc.getSelfHp()+" + " + ((int)Statics.getCardSelfMaxHpForUI(dbc.dataId,dbc.lv,canBN) - dbc.getSelfHp());
							atkLabel.text = dbc.getSelfAtk()+" + " + ((int)Statics.getCardSelfMaxAtkForUI(dbc.dataId,dbc.lv,canBN) - dbc.getSelfAtk());
							defLabel.text = dbc.getSelfDef()+" + " + ((int)Statics.getCardSelfMaxDefForUI(dbc.dataId,dbc.lv,canBN) - dbc.getSelfDef());
						}
					}
					else if(canBN == -1)
					{
						if(canLevelUpLevel == cardLevel)
						{
							hpLabel.text = dbc.getSelfHp()+"";
							atkLabel.text = dbc.getSelfAtk()+"";
							defLabel.text = dbc.getSelfDef()+"";
						}
						else
						{
							hpLabel.text = dbc.getSelfHp()+" + " + ((int)Statics.getCardSelfMaxHpForUI(dbc.dataId,canLevelUpLevel,bn) - dbc.getSelfHp());
							atkLabel.text = dbc.getSelfAtk()+" + " + ((int)Statics.getCardSelfMaxAtkForUI(dbc.dataId,canLevelUpLevel,bn) - dbc.getSelfAtk());
							defLabel.text = dbc.getSelfDef()+" + " + ((int)Statics.getCardSelfMaxDefForUI(dbc.dataId,canLevelUpLevel,bn) - dbc.getSelfDef());
						}
					}
					
					
				}
			}
		}
	}
	

}

