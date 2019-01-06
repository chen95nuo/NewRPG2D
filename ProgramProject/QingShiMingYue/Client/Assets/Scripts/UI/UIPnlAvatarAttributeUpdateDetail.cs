using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System.Text;

public class UIPnlAvatarAttributeUpdateDetail : UIModule
{
	public enum _UIShowType
	{
		LevelUPDetail,//人物升级 强化
		BreakThroughDetail,//人物突破
		EquipmentRefine,//装备精练
		EquipmentUpgrate,//装备强化
	}

	public class ChangeData
	{
		int resourceId;
		int level;
		int breakthoughtLevel;

		public int ResourceId { get { return resourceId; } set { resourceId = value; } }
		public int Level { get { return level; } set { level = value; } }
		public int BreakthoughtLevel { get { return breakthoughtLevel; } set { breakthoughtLevel = value; } }
	}

	public UIBox refineTitle;
	public UIBox breakTitle;
	public UIBox backBg;
	public SpriteText preLevelText;
	public SpriteText afterLevelText;
	public SpriteText preAttributeText;
	public SpriteText afterAttributeText;

	//突破星星Root
	public GameObject breakStarRoot;
	
	//突破等级显示
	public UIElemBreakThroughBtn avatarBreakProgressLift;
	public UIElemBreakThroughBtn avatarBreakProgressRight;

	private const int originalLinesCount = 6;
	private const float originalWidth = 255;
	private const float originalHeight = 150;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		bool isShowTitle = (bool)userDatas[0];
		_UIShowType showType = (_UIShowType)userDatas[1];
		ChangeData data0 = userDatas[2] as ChangeData;
		ChangeData data1 = userDatas[3] as ChangeData;

		Dictionary<int, double> attributes0 = GetAttributes(data0);
		Dictionary<int, double> attributes1 = GetAttributes(data1);

		GetAttributesString(showType, attributes0, data0, attributes1, data1);
		SetLevelTextUI(showType, data0, data1);
		SetTitleUI(isShowTitle, showType);
		UpdateBackBg();

		return true;
	}

	private Dictionary<int, double> GetAttributes(ChangeData data)
	{
		int assetType = IDSeg.ToAssetType(data.ResourceId);
		Dictionary<int, double> attributesDic = new Dictionary<int,double>();
		List<AttributeCalculator.Attribute> attributes=  new List<AttributeCalculator.Attribute>();

		switch(assetType)
		{
			case IDSeg._AssetType.Avatar:
				KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
				avatar.LevelAttrib.Level = data.Level;
				avatar.BreakthoughtLevel = data.BreakthoughtLevel;
				avatar.ResourceId = data.ResourceId;
				attributes = PlayerDataUtility.GetAvatarAttributes(avatar, true, false);
				break;

			case IDSeg._AssetType.Equipment:
				KodGames.ClientClass.Equipment equipment = new KodGames.ClientClass.Equipment();
				equipment.LevelAttrib.Level = data.Level;
				equipment.BreakthoughtLevel = data.BreakthoughtLevel;
				equipment.ResourceId = data.ResourceId;
				
				attributes = PlayerDataUtility.GetEquipmentAttributes(equipment);
				break;
		}

		foreach(var attribute in attributes)
		{
			attributesDic.Add(attribute.type, attribute.value);
		}

		return attributesDic;
	}

	private void GetAttributesString(_UIShowType showType, Dictionary<int, double> beforeAttributes, ChangeData before, Dictionary<int, double> afterAttributes, ChangeData after)
	{
		int maxLevel = 0;
		int maxLevelTwo = 0;

		switch (IDSeg.ToAssetType(before.ResourceId))
		{
			case IDSeg._AssetType.Avatar:
				maxLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(before.ResourceId).GetAvatarBreakthrough(before.BreakthoughtLevel).breakThrough.powerUpLevelLimit;
				maxLevelTwo = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(after.ResourceId).GetAvatarBreakthrough(after.BreakthoughtLevel).breakThrough.powerUpLevelLimit;
				break;
			case IDSeg._AssetType.Equipment:
				maxLevel = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(after.ResourceId).GetBreakthroughByTimes(before.BreakthoughtLevel).breakThrough.powerUpLevelLimit;
				maxLevelTwo = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(after.ResourceId).GetBreakthroughByTimes(after.BreakthoughtLevel).breakThrough.powerUpLevelLimit;
				break;
		}

		StringBuilder beforeSb = new StringBuilder();
		StringBuilder afterSb = new StringBuilder();

		string a = GameUtility.FormatUIString("UIPnlArena_Label_Level_With_Color", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, before.Level.ToString(), maxLevel.ToString());
		beforeSb.Append(a);
		a = GameUtility.FormatUIString("UIPnlArena_Label_Level_With_Color", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, after.Level.ToString(), maxLevelTwo.ToString());
		afterSb.Append(a);

		List<int> attriTypes = new List<int>();
		
		if(beforeAttributes.Count > afterAttributes.Count)
			foreach (var beforeDic in beforeAttributes)
			{
				attriTypes.Add(beforeDic.Key);
			}
		else
			foreach (var afterDic in afterAttributes)
			{
				attriTypes.Add(afterDic.Key);
			}

		if (before.Level == after.Level && after.BreakthoughtLevel == before.BreakthoughtLevel)
		{
			foreach (var beforeAttr in beforeAttributes)
			{
				string mes = ItemInfoUtility.GetAttributeNameValueString(beforeAttr.Key, beforeAttr.Value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

				beforeSb.AppendLine();
				afterSb.AppendLine();
				beforeSb.Append(mes);
				afterSb.Append(mes);					
			}
		}
		else
		{
			foreach (var type in attriTypes)
			{
				if (beforeAttributes.ContainsKey(type) && afterAttributes.ContainsKey(type) && beforeAttributes[type] == afterAttributes[type])
					continue;

				beforeSb.AppendLine();
				double beforeValue = 0;
				if (beforeAttributes.ContainsKey(type))
					beforeValue = beforeAttributes[type];
				beforeSb.Append(ItemInfoUtility.GetAttributeNameValueString(type, beforeValue, GameDefines.textColorBtnYellow, GameDefines.textColorWhite));

				afterSb.AppendLine();
				double afterValue = 0;
				if (afterAttributes.ContainsKey(type))
					afterValue = afterAttributes[type];

				afterSb.Append(ItemInfoUtility.GetAttributeNameValueString(type, afterValue, GameDefines.textColorBtnYellow, GameDefines.colorFollowerGreen));								
			}
		}

		if (showType == _UIShowType.BreakThroughDetail)
		{
			var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(before.ResourceId);			
			
			//如果存在组合技
			if (avatarCfg.compositeSkillId != IDSeg.InvalidId)
			{
				string compositeSkillOpenMes = GameUtility.FormatUIString("UIPnlAvatarLevelUpTab_CompositeOpenTips", GameDefines.textColorBtnYellow, ItemInfoUtility.GetLevelCN(avatarCfg.GetBreakThoughtLevelByCompositeSkillLevel(1)));

				var beforeAvatarBreakCfg = avatarCfg.GetAvatarBreakthrough(before.BreakthoughtLevel);
				var afterAvatarBreakCfg = avatarCfg.GetAvatarBreakthrough(after.BreakthoughtLevel);

				beforeSb.AppendLine();
				beforeSb.Append(GameUtility.FormatUIString(
					"UIPnlAvatarLevelUpTab_CompositeLevel",
					GameDefines.textColorBtnYellow,
					GameDefines.textColorWhite,
					beforeAvatarBreakCfg.compositeSkillLevel));

				if (beforeAvatarBreakCfg.compositeSkillLevel <= 0)
					beforeSb.Append(compositeSkillOpenMes);
				
				afterSb.AppendLine();
				afterSb.Append(GameUtility.FormatUIString(
					"UIPnlAvatarLevelUpTab_CompositeLevel",
					GameDefines.textColorBtnYellow,
					afterAvatarBreakCfg.compositeSkillLevel > beforeAvatarBreakCfg.compositeSkillLevel ? GameDefines.colorFollowerGreen : GameDefines.textColorWhite,
					afterAvatarBreakCfg.compositeSkillLevel));

				if (afterAvatarBreakCfg.compositeSkillLevel <= 0)
					afterSb.Append(compositeSkillOpenMes);
			}
		}

		preAttributeText.Text = beforeSb.ToString();
		afterAttributeText.Text = afterSb.ToString();
	}

	private void SetLevelTextUI(_UIShowType showType, ChangeData data0, ChangeData data1)
	{
		switch(showType)
		{
				//升级
			case _UIShowType.LevelUPDetail:
				preLevelText.Text = GameUtility.GetUIString("UIPnlAvatarLevelUpTab_Title_AvatarBeforeLevelUp");
				afterLevelText.Text = GameUtility.GetUIString("UIPnlAvatarLevelUpTab_Title_AvatarAfterLevelUp");
				breakStarRoot.SetActive(false);
				break;

			case _UIShowType.EquipmentUpgrate:
				preLevelText.Text = string.Format(GameUtility.GetUIString("UIPnlAvatarLevelUpTab_Title_EquipBeforeLevelUp"));
				afterLevelText.Text = string.Format(GameUtility.GetUIString("UIPnlAvatarLevelUpTab_Title_EquipAfterLevelUp"));
				breakStarRoot.SetActive(false);
				break;

				//突破精炼
			case _UIShowType.BreakThroughDetail:
				preLevelText.Text = "";
				afterLevelText.Text = "";
				breakStarRoot.SetActive(true);
				avatarBreakProgressLift.SetBreakThroughIcon(data0.BreakthoughtLevel);
				avatarBreakProgressRight.SetBreakThroughIcon(data1.BreakthoughtLevel);
				break;

			case _UIShowType.EquipmentRefine:
				preLevelText.Text = "";
				afterLevelText.Text = "";
				breakStarRoot.SetActive(true);
				avatarBreakProgressLift.SetBreakThroughIcon(data0.BreakthoughtLevel);
				avatarBreakProgressRight.SetBreakThroughIcon(data1.BreakthoughtLevel);
				break;

			default:
				preLevelText.Text = "";
				afterLevelText.Text = "";
				breakStarRoot.SetActive(false);
				break;
		}
	}

	private void SetTitleUI(bool isShowTitle, _UIShowType showType)
	{
		if (isShowTitle)
		{
			switch (showType)
			{
				case _UIShowType.EquipmentRefine:
					breakTitle.Hide(true);
					refineTitle.Hide(false);
					break;
				case _UIShowType.BreakThroughDetail:
					breakTitle.Hide(false);
					refineTitle.Hide(true);
					break;
			}
		}
		else
		{
			breakTitle.Hide(true);
			refineTitle.Hide(true);
		}
	}

	private void UpdateBackBg()
	{
		float lineHeight = preAttributeText.GetLineHeight();
		int lineNeed = preAttributeText.GetDisplayLineCount() - originalLinesCount;
		if (lineNeed > 0)
		{
			float heightNeed = lineNeed * lineHeight + (lineNeed - 1) * preAttributeText.lineSpacing;
			backBg.SetSize(originalWidth, originalHeight + heightNeed);
		}
		else
			backBg.SetSize(originalWidth, originalHeight);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}