using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemTemplate : UIModule
{
	public static UIElemTemplate Inst
	{
		get { return SysUIEnv.Instance.GetUIModule<UIElemTemplate>(); }
	}

	public UIElemIconBorderTempate iconBorderTemplate;
	public UIElemAvatarQualityTemplate avatarQualityTemplate;
	public UIElemlShopIconTemplate shopIconTemplate;
	public UIElemListItemBgTemplate listItemBgTemplate;
	public UIElemItemSmallIconTemplate itemSmalIconTemplate;
	public UIElemEquipOrSkillTempate equipOrSkillTemplate;
	public UIElemBreakThroughTemplate breakThroughTemplate;
	public UIElemTowerRankTemplate towerRankTemplate;
	public UIElemDisableStyleClickableBtnTemplate disableStyleClickableBtnTemplate;
	public UIElemFriendCampaginRankTemplate friendcampaginTemplate;
	public UIElemOrganBeastTypeTemplate organBeastTypeTemplate;

	public void SetSkillTypeIcon(AutoSpriteControlBase targetIcon, int Type)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (Type)
		{
			case CombatTurn._Type.EnterBattleSkill:
				sourceIcon = equipOrSkillTemplate.iconSkillJinChang;
				break;
			case CombatTurn._Type.NormalSkill:
				sourceIcon = equipOrSkillTemplate.iconSkillXuLi;
				break;
			case CombatTurn._Type.ActiveSkill:
				sourceIcon = equipOrSkillTemplate.iconSkillBaoZou;
				break;
			case CombatTurn._Type.CompositeSkill:
				sourceIcon = equipOrSkillTemplate.iconSkillZuhe;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}

	public void SetEquipTypeIcon(AutoSpriteControlBase targetIcon, int Type)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (Type)
		{
			case EquipmentConfig._Type.Weapon:
				sourceIcon = equipOrSkillTemplate.iconEquipWuQi;
				break;
			case EquipmentConfig._Type.Armor:
				sourceIcon = equipOrSkillTemplate.iconEquipYiFu;
				break;
			case EquipmentConfig._Type.Shoe:
				sourceIcon = equipOrSkillTemplate.iconEquipXieZi;
				break;
			case EquipmentConfig._Type.Decoration:
				sourceIcon = equipOrSkillTemplate.iconEquipShouShi;
				break;
			case EquipmentConfig._Type.Treasure:
				sourceIcon = equipOrSkillTemplate.iconEquipBaoWu;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}

	public void SetAvatarCountryIcon(AutoSpriteControlBase targetIcon, int avatarCountryType)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (avatarCountryType)
		{
			case AvatarConfig._AvatarCountryType.QunXiong:
				sourceIcon = iconBorderTemplate.iconCountryQunXiong;
				break;
			case AvatarConfig._AvatarCountryType.All:
				sourceIcon = iconBorderTemplate.iconCountryAll;
				break;
			case AvatarConfig._AvatarCountryType.QinGuo:
				sourceIcon = iconBorderTemplate.iconCountryQinGuo;
				break;
			case AvatarConfig._AvatarCountryType.YanGuo:
				sourceIcon = iconBorderTemplate.iconCountryYanGuo;
				break;
			case AvatarConfig._AvatarCountryType.ChuGuo:
				sourceIcon = iconBorderTemplate.iconCountryChuGuo;
				break;
			case AvatarConfig._AvatarCountryType.HanGuo:
				sourceIcon = iconBorderTemplate.iconCountryHanGuo;
				break;
			default:
				sourceIcon = iconBorderTemplate.iconCountryLiuGuo;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}

	//设置机关兽类型
	public void SetBeastTraitIcon(AutoSpriteControlBase targetIcon, int beastType)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (beastType)
		{
			case BeastConfig._BeastTraitType.DPS:
				sourceIcon = organBeastTypeTemplate.beastTraitDPSBtn;
				break;
			case BeastConfig._BeastTraitType.Support:
				sourceIcon = organBeastTypeTemplate.beastTraitSupportBtn;
				break;
			case BeastConfig._BeastTraitType.Heal:
				sourceIcon = organBeastTypeTemplate.beastTraitHealBtn;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}

	public void SetAvatarTraitIcon(AutoSpriteControlBase targetIcon, int avatarTraitType)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (avatarTraitType)
		{
			case AvatarConfig._AvatarTraitType.DPS:
				sourceIcon = avatarQualityTemplate.avatarTraitDPSBtn;
				break;
			case AvatarConfig._AvatarTraitType.Tank:
				sourceIcon = avatarQualityTemplate.avatarTraitTankBtn;
				break;
			case AvatarConfig._AvatarTraitType.Support:
				sourceIcon = avatarQualityTemplate.avatarTraitSupportBtn;
				break;
			case AvatarConfig._AvatarTraitType.Heal:
				sourceIcon = avatarQualityTemplate.avatarTraitHealBtn;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}

	public void SetGuildConstructionQualityIcon(AutoSpriteControlBase targetIcon, int qualityLevel)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (qualityLevel)
		{
			case 1:
				sourceIcon = iconBorderTemplate.iconGuildConQuality1;
				break;
			case 2:
				sourceIcon = iconBorderTemplate.iconGuildConQuality2;
				break;
			case 3:
				sourceIcon = iconBorderTemplate.iconGuildConQuality3;
				break;
			case 4:
				sourceIcon = iconBorderTemplate.iconGuildConQuality4;
				break;
			case 5:
				sourceIcon = iconBorderTemplate.iconGuildConQuality5;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}
}
