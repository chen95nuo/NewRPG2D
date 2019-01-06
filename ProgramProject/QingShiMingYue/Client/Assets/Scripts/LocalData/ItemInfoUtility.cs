using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using MathFactory;
using UnityEngine;

public static class ItemInfoUtility
{
	public static int GetRelativeId(int id, int breakLevel)
	{
		if (IDSeg.ToAssetType(id) == IDSeg._AssetType.Dan)
		{
			if (breakLevel == 0)
			{
				Debug.Log("Dan Quality Is Error" + id.ToString());
				breakLevel = 1;
			}

			var danCfg = ConfigDatabase.DefaultCfg.DanConfig.GetDanIconByResourseIdAndBreakLevel(id, breakLevel);
			return danCfg.IconId;
		}
		else if (IDSeg.ToAssetType(id) == IDSeg._AssetType.Beast)
		{
			if (breakLevel == 0)
			{
				Debug.Log("Beast Quality Is Error" + id.ToString());
				breakLevel = 1;
			}

			var beastCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastIconByResourseIdAndBreakLevel(id, breakLevel);
			return beastCfg.IconId;
		}
		else
		{
			switch (GetItemType(id))
			{
				case ItemConfig._Type.EquipScroll:
				case ItemConfig._Type.AvatarScorll:
				case ItemConfig._Type.SkillScroll:
					var illustrationCfg = ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationByFragmentId(id);
					if (illustrationCfg == null)
					{
						Debug.Log("FragmentId is Error in IllustrationConfig : " + id.ToString("X"));
						return id;
					}
					return illustrationCfg.Id;					
				case ItemConfig._Type.BeastScroll:
					var beastCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastFragmentId(id);
					var beastIcon = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastIconByResourseIdAndBreakLevel(beastCfg.Id, 1);
					
					return beastIcon.IconId;
				default:
					return id;
			}
		}
	}

	public static string GetAssetName(int assetId, int breakLevel)
	{
		string assetName = string.Empty;
		AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(GetRelativeId(assetId, breakLevel));

		switch (IDSeg.ToAssetType(assetId))
		{
			case IDSeg._AssetType.Item:
				{
					switch (ItemConfig._Type.ToItemType(assetId))
					{
						case ItemConfig._Type.SkillScroll:
						case ItemConfig._Type.EquipScroll:
							assetName = GameUtility.GetUIString("EquipScroll_ExtraNameDesc");
							break;
						case ItemConfig._Type.AvatarScorll:
							assetName = GameUtility.GetUIString("AvatarScroll_ExtraNameDesc");
							break;
					}
				}
				break;
		}

		if (assetDescCfg == null)
			assetName = string.Empty;
		else
			assetName = assetDescCfg.name + assetName;

		return assetName;
	}

	public static string GetAssetName(int assetId)
	{
		if (IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Beast)
			return GetAssetName(assetId, 1);
		return GetAssetName(assetId, 0);
	}

	public static string GetAssetDesc(int assetId, int breakLevel)
	{
		AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(GetRelativeId(assetId, breakLevel));
		if (assetDescCfg == null)
			return "";

		return assetDescCfg.desc;
	}

	public static string GetAssetDesc(int assetId)
	{
		return GetAssetDesc(assetId, 0);
	}

	public static string GetSkillLevelDesc(int skillId, int skillLevel)
	{
		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillId);
		var modifiers = skillCfg.GetLevelModifers(skillLevel);
		var mergedAttribs = PlayerDataUtility.MergeAttributes(modifiers, true, true);

		List<string> formatParams = new List<string>();
		for (int i = 0; i < mergedAttribs.Count; i++)
		{
			formatParams.Add(_AvatarAttributeType.GetDisplayNameByType(mergedAttribs[i].type, ConfigDatabase.DefaultCfg));
			formatParams.Add(ItemInfoUtility.GetAttribDisplayString(mergedAttribs[i].type, mergedAttribs[i].value));
		}

		return GameUtility.FormatStringOnlyWithParams(skillCfg.GetLevelDesc(skillLevel), formatParams.ToArray());
	}

	public static string GetIllusionDesc(int illusionId)
	{
		var illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(illusionId);
		var modifiers = illusionCfg.IllusionModifers;
		var mergedAttribs = PlayerDataUtility.MergeAttributes(modifiers, true, true);

		List<string> formatParams = new List<string>();
		for (int i = 0; i < mergedAttribs.Count; i++)
		{
			formatParams.Add(_AvatarAttributeType.GetDisplayNameByType(mergedAttribs[i].type, ConfigDatabase.DefaultCfg));
			formatParams.Add(ItemInfoUtility.GetAttribDisplayString(mergedAttribs[i].type, mergedAttribs[i].value));
		}

		return GameUtility.FormatStringOnlyWithParams(illusionCfg.ModifierSetDesc, formatParams.ToArray());
	}

	public static string GetAssetExtraDesc(int assetId)
	{
		AssetDescConfig.AssetDesc assetDescCfg = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(GetRelativeId(assetId, 1));
		if (assetDescCfg == null)
			return "";

		return assetDescCfg.extraDesc;
	}

	public static string GetAssetBreakLevel(int assetType, int breakLevel)
	{
		switch (assetType)
		{
			case IDSeg._AssetType.Dan:
				return GetDanTextQuality(breakLevel);

			default:
				return breakLevel.ToString();
		}
	}

	public static Color GetAssetQualityColor(int qualityLevel)
	{
		switch (qualityLevel)
		{
			case 5:
				return GameDefines.cardColorChenSe;
			case 4:
				return GameDefines.cardColorZiSe;
			case 3:
				return GameDefines.cardColorLanSe;
			case 2:
				return GameDefines.cardColorLvSe;
			case 1:
				return GameDefines.cardColorBaiSe;
		}

		return GameDefines.textColorWhite;
	}

	public static int GetAssetQualityLevel(int assetId)
	{
		switch (IDSeg.ToAssetType(assetId))
		{
			case IDSeg._AssetType.Avatar:
				return ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(assetId).qualityLevel;

			case IDSeg._AssetType.Equipment:
				return ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(assetId).qualityLevel;

			case IDSeg._AssetType.CombatTurn:
				return ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(assetId).qualityLevel;
			default:
				return 0;
		}
	}

	public static string GetAssetQualityDesc(int assetId, bool shortDesc, bool hasColor)
	{
		List<string> datas = new List<string>();

		string formatString = string.Empty;
		int qualityLevel = GetAssetQualityLevel(assetId);
		if (hasColor)
		{
			datas.Add(GetAssetQualityColor(qualityLevel).ToString());
			formatString = shortDesc ? GameUtility.GetUIString("UI_Quality_Level_ShortColor") : GameUtility.GetUIString("UI_Quality_LevelColor");
		}
		else
			formatString = shortDesc ? GameUtility.GetUIString("UI_Quality_Level_Short") : GameUtility.GetUIString("UI_Quality_Level");

		datas.Add(GetAssetQualityLevelCNDesc(qualityLevel));

		return GameUtility.FormatStringOnlyWithParams(formatString, datas.ToArray());
	}

	public static string GetAssetQualityLevelCNDesc(int qualityLevel)
	{
		switch (qualityLevel)
		{
			case 5:
				return GameUtility.GetUIString("UI_CN_Quality_5");
			case 4:
				return GameUtility.GetUIString("UI_CN_Quality_4");
			case 3:
				return GameUtility.GetUIString("UI_CN_Quality_3");
			case 2:
				return GameUtility.GetUIString("UI_CN_Quality_2");
			case 1:
				return GameUtility.GetUIString("UI_CN_Quality_1");
		}

		return string.Empty;
	}

	public static string GetDanTextQuality(int qualityLevel)
	{
		switch (qualityLevel)
		{
			case 5:
				return GameUtility.GetUIString("UIPnlDanInfo_Quality_5");
			case 4:
				return GameUtility.GetUIString("UIPnlDanInfo_Quality_4");
			case 3:
				return GameUtility.GetUIString("UIPnlDanInfo_Quality_3");
			case 2:
				return GameUtility.GetUIString("UIPnlDanInfo_Quality_2");
			case 1:
				return GameUtility.GetUIString("UIPnlDanInfo_Quality_1");
		}

		return string.Empty;
	}

	public static string GetAssetQualityShortDesc(int assetId)
	{
		return GetAssetQualityDesc(assetId, true, false);
	}

	public static string GetAssetQualityShortColorDesc(int assetId)
	{
		return GetAssetQualityDesc(assetId, true, true);
	}

	public static string GetAssetQualityLongDesc(int assetId)
	{
		return GetAssetQualityDesc(assetId, false, false);
	}

	public static string GetAssetQualityLongColorDesc(int assetId)
	{
		return GetAssetQualityDesc(assetId, false, true);
	}

	public static string GetAssetQualityLongColor(int assetId)
	{
		int qualityLevel = GetAssetQualityLevel(assetId);
		return GetAssetQualityColor(qualityLevel).ToString();
	}

	public static string GetLevelString(int level)
	{
		return string.Format(GameUtility.GetUIString("UI_Level"), level);
	}

	public static string GetAssetEvalutionDesc(KodGames.ClientClass.Avatar avatar)
	{
		return string.Format(GameUtility.GetUIString("UI_Value"), ExpressionCalculate.GetValue_AvatarEvaluation(avatar));
	}

	public static string GetAssetEvalutionDesc(KodGames.ClientClass.Equipment equipment)
	{
		return string.Format(GameUtility.GetUIString("UI_Value"), (int)ExpressionCalculate.GetValue_EquipmentEvaluation(equipment));
	}

	public static string GetAssetEvalutionDesc(KodGames.ClientClass.Skill skill)
	{
		return string.Format(GameUtility.GetUIString("UI_Value"), ExpressionCalculate.GetValue_SkillEvaluation(skill));
	}

	public static void ShowLineUpAvatarDesc(KodGames.ClientClass.Avatar avatar)
	{
		var lineupDatas = PlayerDataUtility.GetLineUpInPositions(SysLocalDataBase.Inst.LocalPlayer, avatar);
		var lineupInpartners = PlayerDataUtility.GetLineUpInPartners(SysLocalDataBase.Inst.LocalPlayer, avatar);

		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		lineupDatas.Sort((m, n) =>
		{
			return m.PositionId - n.PositionId;
		});

		lineupInpartners.Sort((m, n) =>
		{
			return m.PositionId - n.PositionId;
		});

		for (int i = 0; i < lineupDatas.Count; i++)
			sb.Append(GameUtility.FormatUIString("UIPnlPackageAvatarTab_Detail_LineUp", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineupDatas[i].PositionId), GameDefines.textColorWhite)).Append('\n');

		for (int i = 0; i < lineupInpartners.Count; i++)
			sb.Append(GameUtility.FormatUIString("UIPnlPackageAvatarTab_Detail_Partner", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineupInpartners[i].PositionId), GameDefines.textColorWhite)).Append('\n');

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(sb.ToString(), true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	public static void ShowLineUpEquipDesc(KodGames.ClientClass.Equipment equipment)
	{
		string equipInformation = string.Empty;
		var lineUpEquips = PlayerDataUtility.GetLineUpInPositions(SysLocalDataBase.Inst.LocalPlayer, equipment);

		lineUpEquips.Sort((a1, a2) =>
		{
			return a1.PositionId - a2.PositionId;
		});

		for (int i = 0; i < lineUpEquips.Count; i++)
		{
			var avatarLocation = PlayerDataUtility.GetLineUpAvatar(SysLocalDataBase.Inst.LocalPlayer, lineUpEquips[i].PositionId, lineUpEquips[i].ShowLocationId);

			if (avatarLocation != null)
				equipInformation += GameUtility.FormatUIString("UIPnlSelect_DetailInfo", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpEquips[i].PositionId), GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(avatarLocation.ResourceId));
			else
				equipInformation += GameUtility.FormatUIString("UIPnlSelect_DetailInfo2", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpEquips[i].PositionId), GameDefines.textColorWhite);
			equipInformation += "\n";
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(equipInformation, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	public static void ShowLineUpDanDesc(KodGames.ClientClass.Dan dan)
	{
		string danInformation = string.Empty;
		var lineUpDans = PlayerDataUtility.GetLineUpInPositions(SysLocalDataBase.Inst.LocalPlayer, dan);

		lineUpDans.Sort((a1, a2) =>
		{
			return a1.PositionId - a2.PositionId;
		});

		for (int i = 0; i < lineUpDans.Count; i++)
		{
			var avatarLocation = PlayerDataUtility.GetLineUpAvatar(SysLocalDataBase.Inst.LocalPlayer, lineUpDans[i].PositionId, lineUpDans[i].ShowLocationId);

			if (avatarLocation != null)
				danInformation += GameUtility.FormatUIString("UIPnlSelect_DetailInfo", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpDans[i].PositionId), GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(avatarLocation.ResourceId));
			else
				danInformation += GameUtility.FormatUIString("UIPnlSelect_DetailInfo2", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpDans[i].PositionId), GameDefines.textColorWhite);
			danInformation += "\n";
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(danInformation, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	public static void ShowLineUpSkillDesc(KodGames.ClientClass.Skill skill)
	{
		string lineUpDescription = string.Empty;
		var lineUpSkills = PlayerDataUtility.GetLineUpInPositions(SysLocalDataBase.Inst.LocalPlayer, skill);

		lineUpSkills.Sort((a1, a2) =>
		{
			return a1.PositionId - a2.PositionId;
		});

		for (int i = 0; i < lineUpSkills.Count; i++)
		{
			var avatarLocation = PlayerDataUtility.GetLineUpAvatar(SysLocalDataBase.Inst.LocalPlayer, lineUpSkills[i].PositionId, lineUpSkills[i].ShowLocationId);

			if (avatarLocation != null)
				lineUpDescription += GameUtility.FormatUIString("UIPnlSelect_DetailInfo", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpSkills[i].PositionId), GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(avatarLocation.ResourceId));
			else
				lineUpDescription += GameUtility.FormatUIString("UIPnlSelect_DetailInfo2", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpSkills[i].PositionId), GameDefines.textColorWhite);
			lineUpDescription += "\n";
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(lineUpDescription, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	public static void ShowLineUpBeastDesc(KodGames.ClientClass.Beast beast)
	{
		string beastInformation = string.Empty;
		var lineUpBeasts = PlayerDataUtility.GetLineUpInPositions(SysLocalDataBase.Inst.LocalPlayer, beast);

		lineUpBeasts.Sort((a1, a2) =>
		{
			return a1.PositionId - a2.PositionId;
		});

		for (int i = 0; i < lineUpBeasts.Count; i++)
		{
			var avatarLocation = PlayerDataUtility.GetLineUpAvatar(SysLocalDataBase.Inst.LocalPlayer, lineUpBeasts[i].PositionId, lineUpBeasts[i].ShowLocationId);

			if (avatarLocation != null)
				beastInformation += GameUtility.FormatUIString("UIPnlSelect_DetailInfo", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpBeasts[i].PositionId), GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(avatarLocation.ResourceId));
			else
				beastInformation += GameUtility.FormatUIString("UIPnlSelect_DetailInfo2", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(lineUpBeasts[i].PositionId), GameDefines.textColorWhite);
			beastInformation += "\n";
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(beastInformation, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	public static string GetAttribDisplayString(AttributeCalculator.Attribute attribute)
	{
		return GetAttribDisplayString(attribute.type, attribute.value);
	}

	public static string GetAttribDisplayString(int type, double value)
	{
		if (type < _AvatarAttributeType.NUMBER_RATE_SPACE)
			return KodGames.Math.RoundToInt(value).ToString();
		else
			return string.Format("{0}%", GameUtility.FloatToPercentageValue((float)value));
	}

	public static string GetAttribDisplayString(KodGames.ClientClass.PropertyModifier kodModifier)
	{
		var modifier = new PropertyModifier();
		modifier.type = kodModifier.Type;
		modifier.attributeType = kodModifier.AttributeType;
		modifier.attributeValue = kodModifier.AttributeValue;
		modifier.modifyType = kodModifier.ModifyType;

		return GetAttribDisplayString(modifier);
	}

	public static string GetAttribDisplayString(PropertyModifier modifier)
	{
		if (modifier.modifyType == PropertyModifier._ValueModifyType.Percentage)
		{
			return GameUtility.FloatToPercentage(modifier.attributeValue);
		}
		else
		{
			if (modifier.attributeType < _AvatarAttributeType.NUMBER_RATE_SPACE)
			{
				int _value = KodGames.Math.RoundToInt(modifier.attributeValue);
				//if (_value >= 0)
				//    return string.Format("+{0}", _value);
				//else
				return _value.ToString();
			}
			else
			{
				return GameUtility.FloatToPercentage(modifier.attributeValue);
			}
		}
	}

	public static string GetAttributeNameValueString(AttributeCalculator.Attribute attribute)
	{
		return GetAttributeNameValueString(attribute.type, attribute.value);
	}

	public static string GetAttributeNameValueString(int type, double value)
	{
		return GameUtility.FormatUIString("UIAttributeText", _AvatarAttributeType.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg), GetAttribDisplayString(type, value));
	}

	public static string GetAttributeNameValueString(int type, double value, Color nameColor, Color valueColor)
	{
		return GameUtility.FormatUIString("UIAttributeTextWithColor", nameColor, _AvatarAttributeType.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg), valueColor, GetAttribDisplayString(type, value));
	}

	public static string GetAttributeNameValueStringWithAdd(int type, double value, Color nameColor, Color valueColor)
	{
		return GameUtility.FormatUIString("UIAttributeTextWithColor_Add", nameColor, _AvatarAttributeType.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg), valueColor, GetAttribDisplayString(type, value));
	}

	public static string GetLevelCN(int level)
	{
		string cnKey = "";
		switch (level)
		{
			case 0: cnKey = "UI_CN_BreakLevel_0"; break;
			case 1: cnKey = "UI_CN_BreakLevel_1"; break;
			case 2: cnKey = "UI_CN_BreakLevel_2"; break;
			case 3: cnKey = "UI_CN_BreakLevel_3"; break;
			case 4: cnKey = "UI_CN_BreakLevel_4"; break;
			case 5: cnKey = "UI_CN_BreakLevel_5"; break;
			case 6: cnKey = "UI_CN_BreakLevel_6"; break;
			case 7: cnKey = "UI_CN_BreakLevel_7"; break;
			case 8: cnKey = "UI_CN_BreakLevel_8"; break;
			case 9: cnKey = "UI_CN_BreakLevel_9"; break;
			case 10: cnKey = "UI_CN_BreakLevel_10"; break;
			case 11: cnKey = "UI_CN_BreakLevel_11"; break;
			case 12: cnKey = "UI_CN_BreakLevel_12"; break;
			case 13: cnKey = "UI_CN_BreakLevel_13"; break;
			case 14: cnKey = "UI_CN_BreakLevel_14"; break;
			case 15: cnKey = "UI_CN_BreakLevel_15"; break;
			case 16: cnKey = "UI_CN_BreakLevel_16"; break;
			case 17: cnKey = "UI_CN_BreakLevel_17"; break;
			case 18: cnKey = "UI_CN_BreakLevel_18"; break;
			case 19: cnKey = "UI_CN_BreakLevel_19"; break;
			case 20: cnKey = "UI_CN_BreakLevel_20"; break;
		}
		return GameUtility.GetUIString(cnKey);
	}

	public static string GetCNStageIndex(int index)
	{
		string cnKey = "";
		switch (index)
		{
			case 0: cnKey = "UI_CN_StageIndex_0"; break;
			case 1: cnKey = "UI_CN_StageIndex_1"; break;
			case 2: cnKey = "UI_CN_StageIndex_2"; break;
			case 3: cnKey = "UI_CN_StageIndex_3"; break;
			case 4: cnKey = "UI_CN_StageIndex_4"; break;
			case 5: cnKey = "UI_CN_StageIndex_5"; break;
			case 6: cnKey = "UI_CN_StageIndex_6"; break;
			case 7: cnKey = "UI_CN_StageIndex_7"; break;
			case 8: cnKey = "UI_CN_StageIndex_8"; break;
			case 9: cnKey = "UI_CN_StageIndex_9"; break;
			case 10: cnKey = "UI_CN_StageIndex_10"; break;
			case 11: cnKey = "UI_CN_StageIndex_11"; break;
			case 12: cnKey = "UI_CN_StageIndex_12"; break;
			case 13: cnKey = "UI_CN_StageIndex_13"; break;
			case 14: cnKey = "UI_CN_StageIndex_14"; break;
			case 15: cnKey = "UI_CN_StageIndex_15"; break;
			case 16: cnKey = "UI_CN_StageIndex_16"; break;
			case 17: cnKey = "UI_CN_StageIndex_17"; break;
			case 18: cnKey = "UI_CN_StageIndex_18"; break;
			case 19: cnKey = "UI_CN_StageIndex_19"; break;
			case 20: cnKey = "UI_CN_StageIndex_20"; break;
		}
		return string.Format(GameUtility.GetUIString("UIPnlWolfInfo_Stage_Name"), GameUtility.GetUIString(cnKey));
	}

	public static string GetCurrencyNameByType(int type)
	{
		string cnKey = string.Empty;

		switch (type)
		{
			case _CurrencyType.Dollar: cnKey = "CurrencyName_Doller"; break;
			case _CurrencyType.RMB: cnKey = "CurrencyName_RMB"; break;
			case _CurrencyType.NTB: cnKey = "CurrencyName_RMB"; break;
		}

		return GameUtility.GetUIString(cnKey);
	}

	public static string GetDecimalMedianByType(int type)
	{
		string mat = string.Empty;
		switch (type)
		{
			case _CurrencyType.UnKnow: mat = ""; break;
			case _CurrencyType.RMB: mat = "F0"; break;
			case _CurrencyType.Dollar: mat = "F2"; break;
			case _CurrencyType.NTB: mat = "F0"; break;
			default: mat = ""; break;
		}

		return mat;
	}

	public static string GetAttributeStr(int attributeType)
	{
		return _AvatarAttributeType.GetDisplayNameByType(attributeType, ConfigDatabase.DefaultCfg);
	}

	public static string GetItemCountStr(int count)
	{
		if (count < 100000)
			return count.ToString();
		if (count < 100000000 && count >= 100000)
			return GameUtility.FormatUIString("ConsumableCount", count / 10000);
		if (count >= 100000000)
			return GameUtility.FormatUIString("CalculateCount", count / 100000000);

		return string.Empty;
	}

	public static string GetItemCountStr(float count, int type)
	{
		if (count < 100000)
			return count.ToString(GetDecimalMedianByType(type));
		if (count < 100000000 && count >= 100000)
			return GameUtility.FormatUIString("ConsumableCount", (count / 10000).ToString(GetDecimalMedianByType(type)));
		if (count >= 100000000)
			return GameUtility.FormatUIString("CalculateCount", (count / 100000000).ToString(GetDecimalMedianByType(type)));

		return string.Empty;
	}

	public static string GetItemCountStr(KodGames.ClientClass.Consumable consumable)
	{
		return GetItemCountStr(consumable.Amount);
	}

	public static bool IsAvatarEquipped(KodGames.ClientClass.Avatar avatar)
	{
		if (avatar == null)
			return false;

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions.Count; i++)
		{
			var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions[i];

			if (position == null)
				continue;

			for (int j = 0; j < position.AvatarLocations.Count; j++)
			{
				if (position.AvatarLocations[j].Guid.Equals(avatar.Guid))
					return true;
			}
		}

		return false;
	}

	public static bool IsAvatarCheered(KodGames.ClientClass.Avatar avatar)
	{
		if (avatar == null)
			return false;

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions.Count; i++)
		{
			var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions[i];
			if (position == null)
				continue;

			for (int j = 0; j < position.Partners.Count; j++)
				if (position.Partners[j].AvatarGuid.Equals(avatar.Guid))
					return true;
		}

		return false;
	}

	public static bool IsEquiped(KodGames.ClientClass.Avatar avatar)
	{
		if (avatar == null)
			return false;

		return PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, avatar);
	}

	public static bool IsEquiped(KodGames.ClientClass.Equipment equipment)
	{
		if (equipment == null)
			return false;

		return PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, equipment);
	}

	public static bool IsEquiped(KodGames.ClientClass.Skill skill)
	{
		if (skill == null)
			return false;

		return PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, skill);
	}

	public static bool IsEquiped(KodGames.ClientClass.Dan dan)
	{
		if (dan == null)
			return false;

		return PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, dan);
	}

	public static int GetEquipmentType(int equipId)
	{
		EquipmentConfig.Equipment equipCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipId); ;

		return equipCfg != null ? equipCfg.type : EquipmentConfig._Type.Unknown;
	}

	public static int GetDanType(int danId)
	{
		var danCfg = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(danId);

		return danCfg != null ? danCfg.Type : DanConfig._DanType.Unknow;
	}

	public static KodGames.ClientClass.Consumable FindConsumableById(List<KodGames.ClientClass.Consumable> consumables, int assetId)
	{
		if (consumables != null)
			foreach (var consumable in consumables)
				if (consumable.Id == assetId)
					return consumable;

		return null;
	}

	public static KodGames.ClientClass.Consumable FindConsumableByItemType(List<KodGames.ClientClass.Consumable> consumables, int itemType)
	{
		if (consumables != null)
			foreach (var consumable in consumables)
				if (IDSeg.ToAssetType(consumable.Id) == IDSeg._AssetType.Item && ItemConfig._Type.ToItemType(consumable.Id) == itemType)
					return consumable;

		return null;
	}

	public static int GetItemType(int id)
	{
		if (IDSeg.ToAssetType(id) != IDSeg._AssetType.Item)
			return ItemConfig._Type.Unknown;

		return ItemConfig._Type.ToItemType(id);
	}

	public static string GetItemLevelReqDesc(Color meetColor, Color notMeetColor, int itemId)
	{
		int levelReq = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(itemId).playerLevel;
		int vipReq = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(itemId).vipLevel;

		string result = "";
		if (levelReq > 0 && vipReq > 0)
			result = GameUtility.FormatUIString("UIPnlPackageItem_BothReq",
				GameUtility.FormatUIString("UIPnlPackageItem_PlayerLevelReq", (levelReq <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level ? meetColor : notMeetColor).ToString(), levelReq),
				GameUtility.FormatUIString("UIPnlPackageItem_VIPLevelReq", (vipReq <= SysLocalDataBase.Inst.LocalPlayer.VipLevel ? meetColor : notMeetColor).ToString(), vipReq));
		else if (levelReq > 0)
			result = GameUtility.FormatUIString("UIPnlPackageItem_PlayerLevelReq", (levelReq <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level ? meetColor : notMeetColor).ToString(), levelReq);
		else if (vipReq > 0)
			result = GameUtility.FormatUIString("UIPnlPackageItem_VIPLevelReq", (vipReq <= SysLocalDataBase.Inst.LocalPlayer.VipLevel ? meetColor : notMeetColor).ToString(), vipReq);

		return result;
	}

	public static string GetSuiteDesc(KodGames.ClientClass.Player player, int resourceId, KodGames.ClientClass.Location location, bool showSuitProcess)
	{
		List<SuiteConfig.AssembleSetting> suits = new List<SuiteConfig.AssembleSetting>();
		suits = ConfigDatabase.DefaultCfg.SuiteConfig.GetEquipmentSuitesByRequireId(resourceId);
		if (suits == null)
			return string.Empty;

		string suiteLabelStr = GameDefines.textColorBtnYellow.ToString();
		if (suits.Count <= 0)
			return suiteLabelStr + "\n";

		var equipedEquipments = new List<KodGames.ClientClass.Equipment>();
		var equipedSkills = new List<KodGames.ClientClass.Skill>();

		if (location != null)
		{
			equipedEquipments = PlayerDataUtility.GetLineUpEquipments(player, location.PositionId, location.ShowLocationId);
			equipedSkills = PlayerDataUtility.GetLineUpSkills(player, location.PositionId, location.ShowLocationId);
		}

		for (int i = 0; i < suits.Count; i++)
		{
			List<List<int>> matchedIds = new List<List<int>>();
			for (int j = 0; j < suits[i].Parts.Count; j++)
			{
				var part = suits[i].Parts[j];
				matchedIds.Add(new List<int>());

				for (int k = 0; k < part.Requiremets.Count; k++)
				{
					for (int l = 0; l < equipedEquipments.Count; l++)
						if (equipedEquipments[l].ResourceId == part.Requiremets[k].Value && !matchedIds[j].Contains(equipedEquipments[l].ResourceId))
							matchedIds[j].Add(equipedEquipments[l].ResourceId);
				}

				for (int k = 0; k < part.Requiremets.Count; k++)
				{
					for (int l = 0; l < equipedSkills.Count; l++)
						if (equipedSkills[l].ResourceId == part.Requiremets[k].Value && !matchedIds[j].Contains(equipedSkills[l].ResourceId))
							matchedIds[j].Add(equipedSkills[l].ResourceId);
				}
			}

			// Get Matched Count.
			int matchedCount = 0;
			for (int j = 0; j < matchedIds.Count; j++)
				if (matchedIds[j].Count > 0)
					matchedCount++;

			if (showSuitProcess)
			{
				// Suit Name
				suiteLabelStr += string.Format("{0}({1}/{2})",
				   suits[i].Name,
				   matchedCount,
				   suits[i].Parts.Count);
			}
			else
				suiteLabelStr += suits[i].Name;

			// Suit Parts' name.
			suiteLabelStr += string.Format("{0}{1}", GameDefines.textColorWhite, "\n(");
			for (int j = 0; j < suits[i].Parts.Count; j++)
			{
				var part = suits[i].Parts[j];

				for (int k = 0; k < part.Requiremets.Count; k++)
				{
					if (showSuitProcess)
					{
						string formatStr = k < part.Requiremets.Count - 1 ? GameUtility.GetUIString("UiItemUtility_And") : "{0}{1}{2} ";

						if (matchedIds[j].Contains(part.Requiremets[k].Value))
							suiteLabelStr += string.Format(formatStr, GameDefines.txColorGreen, ItemInfoUtility.GetAssetName(part.Requiremets[k].Value), GameDefines.textColorWhite);
						else
							suiteLabelStr += string.Format(formatStr, GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(part.Requiremets[k].Value), GameDefines.textColorWhite);
					}
					else
					{
						string formatStr1 = k < part.Requiremets.Count - 1 ? GameUtility.GetUIString("UiItemUtility_And") : "{0}{1}{2} ";
						suiteLabelStr += string.Format(formatStr1, GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(part.Requiremets[k].Value), GameDefines.textColorWhite);
					}
				}
			}
			suiteLabelStr += GameDefines.textColorWhite.ToString() + " )\n";

			// Suite Assemble.
			for (int j = 0; j < suits[i].Assembles.Count; j++)
			{
				suiteLabelStr += GameUtility.FormatUIString("UiItemUtility_Suite", matchedCount >= suits[i].Assembles[j].RequiredCount ? GameDefines.txColorGreen : GameDefines.textColorBtnYellow, suits[i].Assembles[j].RequiredCount, suits[i].Assembles[j].AssembleEffectDesc);
				suiteLabelStr += "\n";
			}

			suiteLabelStr += "\n " + GameDefines.textColorBtnYellow.ToString();
		}

		return suiteLabelStr;
	}

	public static List<int> GetAvatarAssembleRequireIds(KodGames.ClientClass.Avatar avatar)
	{
		var assembleRequireIds = new List<int>();
		if (avatar != null)
		{
			var assembes = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).assemableIds;
			for (int i = 0; i < assembes.Count; i++)
			{
				var assembleCfg = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(assembes[i]);

				for (int j = 0; j < assembleCfg.Parts.Count; j++)
				{
					for (int k = 0; k < assembleCfg.Parts[j].Requiremets.Count; k++)
					{
						if (!assembleRequireIds.Contains(assembleCfg.Parts[j].Requiremets[k].Value))
							assembleRequireIds.Add(assembleCfg.Parts[j].Requiremets[k].Value);
					}
				}
			}
		}

		return assembleRequireIds;
	}

	public static int GetGameItemCount(int assetId)
	{
		var localPlayer = SysLocalDataBase.Inst.LocalPlayer;
		int count = 0;

		switch (IDSeg.ToAssetType(assetId))
		{
			case IDSeg._AssetType.Special:

				switch (assetId)
				{
					case IDSeg._SpecialId.GameMoney:
						count = localPlayer.GameMoney;
						break;
					case IDSeg._SpecialId.RealMoney:
						count = localPlayer.RealMoney;
						break;
					case IDSeg._SpecialId.Stamina:
						count = localPlayer.Stamina.Point.Value;
						break;
					case IDSeg._SpecialId.WorldChatTimes:
						count = localPlayer.CurrentChatCount;
						break;
					case IDSeg._SpecialId.ArenaHonorPoint:
						if (localPlayer.ArenaData != null)
							count = (int)localPlayer.ArenaData.HonorPoint;
						break;
					case IDSeg._SpecialId.ArenaChallengeTimes:
						if (localPlayer.ArenaData != null)
							count = localPlayer.ArenaData.ChallengePoint;
						break;
					case IDSeg._SpecialId.Badge:
						count = localPlayer.Badge;
						break;
					case IDSeg._SpecialId.Soul:
						count = localPlayer.Soul;
						break;
					case IDSeg._SpecialId.UseItemCount_AddStamina:
						count = localPlayer.StaminaBuyCount;
						break;
					case IDSeg._SpecialId.Spirit:
						count = localPlayer.Spirit;
						break;
					case IDSeg._SpecialId.Iron:
						count = localPlayer.Iron;
						break;
					case IDSeg._SpecialId.TrialStamp:
						count = localPlayer.TrialStamp;
						break;
					case IDSeg._SpecialId.Medals:
						count = localPlayer.Medals;
						break;
					case IDSeg._SpecialId.QinInfoAnswerCount:
						count = localPlayer.QinInfoAnswerCount.Point.Value;
						break;
					case IDSeg._SpecialId.Energy:
						count = localPlayer.Energy.Point.Value;
						break;
					case IDSeg._SpecialId.WineSoul:
						count = localPlayer.WineSoul;
						break;
					case IDSeg._SpecialId.GuildMoney:
						count = localPlayer.GuildMoney;
						break;
					case IDSeg._SpecialId.GuildBossCount:
						count = localPlayer.GuildBossCount;
						break;
				}

				break;

			case IDSeg._AssetType.Avatar:

				foreach (var avatar in localPlayer.Avatars)
					if (avatar.ResourceId == assetId)
						count++;

				break;

			case IDSeg._AssetType.Equipment:

				foreach (var equip in localPlayer.Equipments)
					if (equip.ResourceId == assetId)
						count++;
				break;

			case IDSeg._AssetType.CombatTurn:

				foreach (var skill in localPlayer.Skills)
					if (skill.ResourceId == assetId)
						count++;
				break;
			case IDSeg._AssetType.Dan:

				foreach (var dan in localPlayer.Dans)
					if (dan.ResourceId == assetId)
						count++;
				break;

			case IDSeg._AssetType.Item:
				var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(assetId);
				if (consumable != null)
					count = consumable.Amount;
				break;
		}

		return count;
	}

	public static bool CheckCostEnough(int costId, int count)
	{
		return GetGameItemCount(costId) >= count;
	}

	// HandBook
	public static bool HaveMergeIllustration()
	{
		foreach (IllustrationConfig.Illustration illustration in ConfigDatabase.DefaultCfg.IllustrationConfig.Illustrations)
		{
			if (illustration != null && illustration.FragmentCount <= (SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(illustration.FragmentId) != null ? SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(illustration.FragmentId).Amount : 0))
			{
				return true;
			}
		}

		return false;
	}

	// Avatar : PowerUp.
	public static bool IsAbilityUpImprove_Avatar(KodGames.ClientClass.Avatar avatarData)
	{
		return IsLevelNotifyActivity_Avatar(avatarData) || IsBreakNotifyActivity_Avatar(avatarData);
	}

	// Avatar : LevelUp.
	public static bool IsLevelNotifyActivity_Avatar(KodGames.ClientClass.Avatar avatarData)
	{
		if (avatarData == null || string.IsNullOrEmpty(avatarData.Guid) || !avatarData.IsAvatar)
			return false;

		// Check Player Level.
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.AvatarLevelUp) ||
			SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.AvatarLevelUp))
			return false;

		var avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		int currentMaxLevel = avatarConfig.GetAvatarBreakthrough(avatarData.BreakthoughtLevel).breakThrough.powerUpLevelLimit;

		if (avatarData.LevelAttrib.Level < currentMaxLevel &&
			SysLocalDataBase.Inst.LocalPlayer.GameMoney >= GetMaxLevelCostCount(avatarData, avatarData.LevelAttrib.Level, currentMaxLevel))
			return true;
		else
			return false;
	}

	// Avatar : BreakThough.
	public static bool IsBreakNotifyActivity_Avatar(KodGames.ClientClass.Avatar avatarData)
	{
		if (avatarData == null || string.IsNullOrEmpty(avatarData.Guid) || !avatarData.IsAvatar)
			return false;

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);

		// Check Player Level.
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.AvatarBreakThrough) ||
			SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.AvatarBreakThrough))
			return false;

		int maxBreakThroughLevel = avatarCfg.GetMaxBreakthoughtLevel();
		var breakThroughConfig = avatarCfg.GetAvatarBreakthrough(avatarData.BreakthoughtLevel);

		if (avatarData.LevelAttrib.Level < breakThroughConfig.breakThrough.powerUpLevelLimit || avatarData.BreakthoughtLevel >= maxBreakThroughLevel)
			return false;

		// 判定所需最少同名卡
		if (breakThroughConfig.leastSameCardCount > 0 && GetSameAvatarCount(avatarData) < breakThroughConfig.leastSameCardCount)
			return false;

		// Item Count.
		KodGames.ClientClass.Consumable consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(breakThroughConfig.breakThrough.itemCostItemId);
		int itemRequireCount = breakThroughConfig.breakThrough.itemCostItemCount - GetSameAvatarCount(avatarData) * breakThroughConfig.breakThrough.sameCardDeductItemCount;
		int itemOwerCount = 0;
		if (consumable != null)
			itemOwerCount = consumable.Amount;

		// Other Cost.
		bool matchRequire = true;
		foreach (var cost in breakThroughConfig.breakThrough.otherCosts)
		{
			matchRequire = GetGameItemCount(cost.id) >= cost.count;

			if (!matchRequire)
				break;
		}

		if (itemOwerCount >= itemRequireCount && matchRequire)
			return true;
		else
			return false;
	}

	// Avatar : the cost avatar required for LevelUp to Max Level
	private static int GetMaxLevelCostCount(KodGames.ClientClass.Avatar avatarData, int fromLevel, int toLevel)
	{
		var avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		int costCount = 0;
		for (int i = fromLevel; i < toLevel; i++)
		{
			QualityCost qualityCost = ConfigDatabase.DefaultCfg.AvatarConfig.GetQualityCostByLevelAndQuality(i, avatarConfig.qualityLevel);
			costCount += qualityCost.costs[0].count;
		}

		return costCount;
	}

	private static int GetSameAvatarCount(KodGames.ClientClass.Avatar avatar)
	{
		int sameCount = 0;
		foreach (KodGames.ClientClass.Avatar avatarItem in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (avatarItem.IsAvatar == false || avatar.Guid.Equals(avatarItem.Guid) || ItemInfoUtility.IsAvatarEquipped(avatarItem) || ItemInfoUtility.IsAvatarCheered(avatarItem))
				continue;

			if (avatarItem.ResourceId == avatar.ResourceId)
				sameCount++;
		}

		return sameCount;
	}

	// Dan : PowerUp.
	public static bool IsAbilityUpImprove_Dan(KodGames.ClientClass.Dan dan)
	{
		return IsLevelNotifyActivity_Dan(dan) || IsBreakNotifyActivity_Dan(dan);
	}

	// Dan : LevelUp.
	public static bool IsLevelNotifyActivity_Dan(KodGames.ClientClass.Dan dan)
	{
		if (dan == null || string.IsNullOrEmpty(dan.Guid))
			return false;

		// Check Player Level.
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.DanHome) ||
			SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DanHome))
			return false;

		var danCfg = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId);

		if (dan.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.DanConfig.MaxLevel)
			return false;

		var danInfo = danCfg.GetLevelInfoByBreakAndLevel(dan.BreakthoughtLevel, dan.LevelAttrib.Level);
		if (danInfo == null)
			return false;
		for (int index = 0; index < danInfo.Costs.Count; index++)
		{
			var cost = danInfo.Costs[index];
			if (cost.count > GetGameItemCount(cost.id))
				return false;
		}

		if (GetGameItemCount(danInfo.MoneyCost.id) < danInfo.MoneyCost.count)
			return false;

		return true;
	}

	public static string GetDanAttributeDesc(KodGames.ClientClass.DanAttributeGroup danAttributeGroup, int danLevel)
	{
		string attr = "";
		//描述
		attr = attr + danAttributeGroup.AttributeDesc;
		//数值
		attr = attr + string.Format(GameUtility.GetUIString("UIPnlDanInfo_UpAttr_Label"), System.Math.Round(danAttributeGroup.DanAttributes[0].PropertyModifierSets[danLevel - 1].Modifiers[0].AttributeValue * 100, 3));

		return attr;
	}

	// Dan : Refine.
	public static bool IsBreakNotifyActivity_Dan(KodGames.ClientClass.Dan dan)
	{
		if (dan == null || string.IsNullOrEmpty(dan.Guid))
			return false;

		// Check Player Level.
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.DanHome) ||
			SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DanHome))
			return false;


		var danCfg = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId);

		if (dan.BreakthoughtLevel >= ConfigDatabase.DefaultCfg.DanConfig.MaxBreakthought)
			return false;

		var breakThoughCfg = danCfg.GetBreakthoughtInfoByBreakthought(dan.BreakthoughtLevel);

		foreach (var cost in breakThoughCfg.Costs)
		{
			if (cost.count > GetGameItemCount(cost.id))
				return false;
		}

		if (GetGameItemCount(breakThoughCfg.MoneyCost.id) < breakThoughCfg.MoneyCost.count)
			return false;
		return true;
	}


	// Equipment : PowerUp.
	public static bool IsAbilityUpImprove_Equip(KodGames.ClientClass.Equipment equipment)
	{
		if (equipment == null)
			return false;
		return IsLevelNotifyActivity_Equip(equipment) || IsBreakNotifyActivity_Equip(equipment, ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId));
	}

	// Equipment : LevelUp.
	public static bool IsLevelNotifyActivity_Equip(KodGames.ClientClass.Equipment equipment)
	{
		if (equipment == null || string.IsNullOrEmpty(equipment.Guid))
			return false;

		// Check Player Level.
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.EquipmentLevelUp) ||
			SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.EquipmentLevelUp))
			return false;

		var equipmentCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId);
		var breakThoughCfg = equipmentCfg.GetBreakthroughByTimes(equipment.BreakthoughtLevel);

		int currentLevel = equipment.LevelAttrib.Level;
		int currentMaxLevel = breakThoughCfg.breakThrough.powerUpLevelLimit;
		int costMaxCount = 0;

		for (int index = 0; index < currentMaxLevel - equipment.LevelAttrib.Level; index++)
		{
			var costSettingCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetQualityCostByLevelAndQuality(currentLevel + index, equipmentCfg.qualityLevel);
			foreach (var cost in costSettingCfg.costs)
				costMaxCount += cost.count;
		}

		if (currentLevel < currentMaxLevel && SysLocalDataBase.Inst.LocalPlayer.GameMoney >= costMaxCount)
			return true;
		else
			return false;
	}

	// Equipment : Refine.
	public static bool IsBreakNotifyActivity_Equip(KodGames.ClientClass.Equipment equipment, EquipmentConfig.Equipment equipConfig)
	{
		if (equipment == null || string.IsNullOrEmpty(equipment.Guid))
			return false;

		// Check Player Level.
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.EquipmentRefine) ||
			SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.EquipmentRefine))
			return false;


		var equipBreakThrough = equipConfig.GetBreakthroughByTimes(equipment.BreakthoughtLevel);

		if (equipment.LevelAttrib.Level < equipBreakThrough.breakThrough.powerUpLevelLimit ||
		   equipment.BreakthoughtLevel >= ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetMaxBreakthoughtLevel())
			return false;

		bool matchrRequire = true;
		foreach (var cost in equipBreakThrough.breakThrough.otherCosts)
		{
			matchrRequire = GetGameItemCount(cost.id) >= cost.count;

			if (!matchrRequire)
				break;
		}

		if (GetGameItemCount(equipBreakThrough.breakThrough.itemCostItemId) >= equipBreakThrough.breakThrough.itemCostItemCount && matchrRequire)
			return true;
		else
			return false;
	}

	/// <summary>
	/// 获取玩家头，使用玩家防守阵容中的第一个位置角色，第一个位置无角色使用第二
	/// 个位置的角色，以此类推
	/// </summary>
	/// <returns></returns>
	public static int GetAvatarFirstIconID()
	{
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
		position.AvatarLocations.Sort((a1, a2) =>
		{
			return a1.LocationId - a2.LocationId;
		});

		return position.AvatarLocations[0].ResourceId;

	}

	#region Backpack

	/// <summary>
	/// 验证品质是否全选
	/// </summary>
	/// <returns></returns>
	public static bool CheckQualityAllSelected(int dataType)
	{
		var filter = PackageFilterData.Instance.GetPackgetFilterByType(dataType);
		// 品质
		int maxQualityLevel = 5;
		for (int i = 0; i < maxQualityLevel; i++)
		{
			if (i == maxQualityLevel - 1)
				continue;
			int qualityLevel = maxQualityLevel - i;
			if (!filter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel).Contains(qualityLevel))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 验证国家是否全选
	/// </summary>
	/// <param name="dataType"></param>
	/// <returns></returns>
	public static bool CheckAvatarCountryAllSelected(int dataType)
	{
		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(dataType);
		int count = AvatarConfig._AvatarCountryType.GetRegisterTypeCount();

		// 国家.
		for (int i = 0; i < count; ++i)
		{
			int type = AvatarConfig._AvatarCountryType.GetRegisterTypeByIndex(i);
			if (type == AvatarConfig._AvatarCountryType.All)
				break;

			List<int> lists = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType);
			if (!lists.Contains(type))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 验证定位是否全选
	/// </summary>
	/// <param name="dataType">类型：背包 、 阵容</param>
	/// <returns></returns>
	public static bool CheckAvatarTraitAllSelected(int dataType)
	{
		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(dataType);

		for (int i = 0; i < AvatarConfig._AvatarTraitType.GetRegisterTypeCount(); ++i)
		{
			var type = AvatarConfig._AvatarTraitType.GetRegisterTypeByIndex(i);
			if (!avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType).Contains(type))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 验证装备位置是否全选
	/// </summary>
	/// <param name="dataType">类型：背包 、 阵容</param>
	/// <returns></returns>
	public static bool CheckEquipPositionAllSelected(int dataType)
	{
		var equipFilter = PackageFilterData.Instance.GetPackgetFilterByType(dataType);

		for (int i = 0; i < EquipmentConfig._Type.GetRegisterTypeCount(); i++)
		{
			var type = EquipmentConfig._Type.GetRegisterTypeByIndex(i);
			if (type != -1 && !equipFilter.GetFilterDataByType(PackageFilterData._FilterType.EquipType).Contains(type))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 书籍品质
	/// </summary>
	/// <param name="dataType"></param>
	/// <returns></returns>
	public static bool CheckSkillQualityAllSelect(int dataType)
	{
		var skillFilter = PackageFilterData.Instance.GetPackgetFilterByType(dataType);
		// 品质
		int maxQualityLevel = 5;
		for (int i = 0; i < maxQualityLevel; i++)
		{
			if (i == maxQualityLevel - 1)
				continue;
			int qualityLevel = maxQualityLevel - i;
			if (!skillFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel).Contains(qualityLevel))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 筛选角色是否全选
	/// </summary>
	/// <returns></returns>
	public static bool CheckAvatarAllSelected(int dataType)
	{
		if (!CheckQualityAllSelected(dataType) || !CheckAvatarTraitAllSelected(dataType) || !CheckAvatarCountryAllSelected(dataType))
			return false;
		return true;
	}

	/// <summary>
	/// 筛选装备是否全选
	/// </summary>
	/// <param name="dataType">类型：背包 、 阵容</param>
	/// <returns></returns>
	public static bool CheckEquipAllSelected(int dataType)
	{
		if (dataType == PackageFilterData._DataType.SelectEquip)
		{
			if (!CheckQualityAllSelected(dataType))
				return false;
		}
		else if (dataType == PackageFilterData._DataType.PackageEquip)
		{
			if (!CheckEquipPositionAllSelected(dataType) || !CheckQualityAllSelected(dataType))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 筛选书籍是否全选
	/// </summary>
	/// <param name="dataType">类型：背包 、 阵容</param>
	/// <returns></returns>
	public static bool CheckSkillAllSelected(int dataType)
	{
		if (!CheckSkillQualityAllSelect(dataType))
			return false;
		return true;
	}

	#endregion Backpack

	#region Guild
	public static string GetGuildAnnouncement()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo == null ||
		   string.IsNullOrEmpty(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildAnnouncement))
			return GameUtility.GetUIString("UITextHolder_Announcement");
		else
			return SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildAnnouncement;
	}

	public static string GetGuildDeclaration()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo == null ||
		   string.IsNullOrEmpty(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildDeclaration))
			return GameUtility.GetUIString("UITextHolder_Declaration");
		else
			return SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildDeclaration;
	}

	public static List<UIPnlGuildGuideDetail.Goods> GetGoodsListByGuildShopType(int shopType)
	{
		List<UIPnlGuildGuideDetail.Goods> goodsList = new List<UIPnlGuildGuideDetail.Goods>();

		switch (shopType)
		{
			case GuildConfig._GuideType.PublicShopGuide:
				var publicGoods = ConfigDatabase.DefaultCfg.GuildPublicShopConfig.Goods;
				foreach (var goodsCfg in publicGoods)
				{
					UIPnlGuildGuideDetail.Goods good = new UIPnlGuildGuideDetail.Goods();
					good.SetData(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId, ItemInfoUtility.GetAssetName(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId));
					goodsList.Add(good);
					goodsList = GetDefaultGoodsList(goodsList);
				}
				break;
			case GuildConfig._GuideType.PrivateShopGuide:
				var privateGoods = ConfigDatabase.DefaultCfg.GuildPrivateShopConfig.Goods;
				foreach (var goodsCfg in privateGoods)
				{
					UIPnlGuildGuideDetail.Goods good = new UIPnlGuildGuideDetail.Goods();
					good.SetData(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId, ItemInfoUtility.GetAssetName(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId));
					goodsList.Add(good);
					goodsList = GetDefaultGoodsList(goodsList);
				}
				break;
			case GuildConfig._GuideType.ExchangeShopGuide:
				var exchangeGoods = ConfigDatabase.DefaultCfg.GuildExchangeShopConfig.Activities;
				foreach (var exchangeGood in exchangeGoods)
				{
					foreach (var goodsCfg in exchangeGood.Goods)
					{
						UIPnlGuildGuideDetail.Goods good = new UIPnlGuildGuideDetail.Goods();
						good.SetData(goodsCfg.Reward.id, ItemInfoUtility.GetAssetName(goodsCfg.Reward.id));
						goodsList.Add(good);
						goodsList = GetDefaultGoodsList(goodsList);
					}
				}
				break;
		}

		return goodsList;
	}

	public static List<UIPnlGuildGuideDetail.Goods> GetDefaultGoodsList(List<UIPnlGuildGuideDetail.Goods> goods)
	{
		List<UIPnlGuildGuideDetail.Goods> defaultGoods = new List<UIPnlGuildGuideDetail.Goods>();

		foreach (var good in goods)
		{
			bool isHas = false;
			foreach (var newGoods in defaultGoods)
			{
				if (good.id == newGoods.id)
				{
					isHas = true;
					break;
				}
			}
			if (!isHas)
				defaultGoods.Add(good);
		}

		return defaultGoods;
	}

	public static bool IsInMainScene()
	{
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.mainSceneId);
		return SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName);
	}

	#endregion

	#region Exchange Data.
	public static List<string> ExchangeGetItemAvaliableGuids(KodGames.ClientClass.CostAsset costAsset)
	{
		List<string> result = new List<string>();

		switch (costAsset.Type)
		{
			case IDSeg._AssetType.Avatar:
				foreach (var a in ExchangeGetAvaliableAvatar(costAsset))
					result.Add(a.Guid);
				break;

			case IDSeg._AssetType.Equipment:
				foreach (var e in ExchangeGetAvaliableEquip(costAsset))
					result.Add(e.Guid);
				break;

			case IDSeg._AssetType.CombatTurn:
				foreach (var s in ExchangeGetAvaliableSkill(costAsset))
					result.Add(s.Guid);
				break;

			case IDSeg._AssetType.Dan:
				foreach (var d in ExchangeGetAvailiableDan(costAsset))
					result.Add(d.Guid);
				break;
		}

		return result;
	}

	public static List<string> ExchangeGetItemAvaliableGuids(KodGames.ClientClass.ItemEx cost)
	{
		List<string> result = new List<string>();

		switch (IDSeg.ToAssetType(cost.Id))
		{
			case IDSeg._AssetType.Avatar:
				foreach (var a in ExchangeGetAvaliableAvatar(cost))
					result.Add(a.Guid);
				break;

			case IDSeg._AssetType.Equipment:
				foreach (var e in ExchangeGetAvaliableEquip(cost))
					result.Add(e.Guid);
				break;

			case IDSeg._AssetType.CombatTurn:
				foreach (var s in ExchangeGetAvaliableSkill(cost))
					result.Add(s.Guid);
				break;

			case IDSeg._AssetType.Dan:
				foreach (var d in ExchangeGetAvailiableDan(cost))
					result.Add(d.Guid);
				break;
		}

		return result;
	}

	public static List<KodGames.ClientClass.Avatar> ExchangeGetAvaliableAvatar(KodGames.ClientClass.ItemEx cost)
	{
		List<KodGames.ClientClass.Avatar> result = new List<KodGames.ClientClass.Avatar>();

		SysLocalDataBase.Inst.LocalPlayer.Avatars.Sort(DataCompare.CompareAvatarReverse);
		foreach (var a in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (!a.IsAvatar)
				continue;

			if (a.ResourceId == cost.Id
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), a.BreakthoughtLevel, cost.ExtensionBreakThroughLevelFrom, cost.ExtensionBreakThroughLevelTo)
				&& !ItemInfoUtility.IsAvatarEquipped(a)
				&& !ItemInfoUtility.IsAvatarCheered(a)
				&& !result.Contains(a))
				result.Add(a);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Avatar> ExchangeGetAvaliableAvatar(KodGames.ClientClass.CostAsset cost)
	{
		List<KodGames.ClientClass.Avatar> result = new List<KodGames.ClientClass.Avatar>();

		SysLocalDataBase.Inst.LocalPlayer.Avatars.Sort(DataCompare.CompareAvatarReverse);
		foreach (var a in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (!a.IsAvatar)
				continue;

			if (cost.SubType != AvatarConfig._AvatarCountryType.UnKnow
				&& ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a.ResourceId).countryType == cost.SubType
				&& !ItemInfoUtility.IsAvatarEquipped(a)
				&& !ItemInfoUtility.IsAvatarCheered(a)
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), a.BreakthoughtLevel, cost.BreakThroughLevelFrom, cost.BreakThroughLevelTo)
				&& ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a.ResourceId).qualityLevel == cost.QualityLevel
				&& !result.Contains(a))
				result.Add(a);
			else if (cost.SubType == AvatarConfig._AvatarCountryType.UnKnow
				&& !ItemInfoUtility.IsAvatarEquipped(a)
				&& !ItemInfoUtility.IsAvatarCheered(a)
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), a.BreakthoughtLevel, cost.BreakThroughLevelFrom, cost.BreakThroughLevelTo)
				&& ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a.ResourceId).qualityLevel == cost.QualityLevel
				&& !result.Contains(a))
				result.Add(a);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Equipment> ExchangeGetAvaliableEquip(KodGames.ClientClass.ItemEx cost)
	{
		List<KodGames.ClientClass.Equipment> result = new List<KodGames.ClientClass.Equipment>();

		SysLocalDataBase.Inst.LocalPlayer.Equipments.Sort(DataCompare.CompareEquipmentReverse);
		foreach (var e in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			if (e.ResourceId == cost.Id
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), e.BreakthoughtLevel, cost.ExtensionBreakThroughLevelFrom, cost.ExtensionBreakThroughLevelTo)
				&& !ItemInfoUtility.IsEquiped(e)
				&& !result.Contains(e))
				result.Add(e);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Equipment> ExchangeGetAvaliableEquip(KodGames.ClientClass.CostAsset cost)
	{
		List<KodGames.ClientClass.Equipment> result = new List<KodGames.ClientClass.Equipment>();

		SysLocalDataBase.Inst.LocalPlayer.Equipments.Sort(DataCompare.CompareEquipmentReverse);
		foreach (var e in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			if (cost.SubType != EquipmentConfig._Type.Unknown
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), e.BreakthoughtLevel, cost.BreakThroughLevelFrom, cost.BreakThroughLevelTo)
				&& ItemInfoUtility.GetEquipmentType(e.ResourceId) == cost.SubType
				&& !ItemInfoUtility.IsEquiped(e)
				&& ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e.ResourceId).qualityLevel == cost.QualityLevel
				&& !result.Contains(e))
				result.Add(e);
			else if (cost.SubType == EquipmentConfig._Type.Unknown
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), e.BreakthoughtLevel, cost.BreakThroughLevelFrom, cost.BreakThroughLevelTo)
				&& !ItemInfoUtility.IsEquiped(e)
				&& ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e.ResourceId).qualityLevel == cost.QualityLevel
				&& !result.Contains(e))
				result.Add(e);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Skill> ExchangeGetAvaliableSkill(KodGames.ClientClass.ItemEx cost)
	{
		List<KodGames.ClientClass.Skill> result = new List<KodGames.ClientClass.Skill>();

		SysLocalDataBase.Inst.LocalPlayer.Skills.Sort(DataCompare.CompareSkillReverse);
		foreach (var s in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if (s.ResourceId == cost.Id
				&& !ItemInfoUtility.IsEquiped(s)
				&& !result.Contains(s))
				result.Add(s);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Skill> ExchangeGetAvaliableSkill(KodGames.ClientClass.CostAsset cost)
	{
		List<KodGames.ClientClass.Skill> result = new List<KodGames.ClientClass.Skill>();

		SysLocalDataBase.Inst.LocalPlayer.Skills.Sort(DataCompare.CompareSkillReverse);
		foreach (var s in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if (!ItemInfoUtility.IsEquiped(s)
				&& ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s.ResourceId).qualityLevel == cost.QualityLevel
				&& !result.Contains(s))
				result.Add(s);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Dan> ExchangeGetAvailiableDan(KodGames.ClientClass.ItemEx cost)
	{
		List<KodGames.ClientClass.Dan> result = new List<KodGames.ClientClass.Dan>();

		SysLocalDataBase.Inst.LocalPlayer.Dans.Sort(DataCompare.CompareDanReverse);
		foreach (var d in SysLocalDataBase.Inst.LocalPlayer.Dans)
		{
			if (d.ResourceId == cost.Id
			   && ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), d.BreakthoughtLevel, cost.ExtensionBreakThroughLevelFrom, cost.ExtensionBreakThroughLevelTo)
			   && !ItemInfoUtility.IsEquiped(d)
			   && !result.Contains(d))
				result.Add(d);
		}

		return result;
	}

	public static List<KodGames.ClientClass.Dan> ExchangeGetAvailiableDan(KodGames.ClientClass.CostAsset cost)
	{
		List<KodGames.ClientClass.Dan> result = new List<KodGames.ClientClass.Dan>();

		SysLocalDataBase.Inst.LocalPlayer.Dans.Sort(DataCompare.CompareDanReverse);
		foreach (var d in SysLocalDataBase.Inst.LocalPlayer.Dans)
		{
			if (cost.SubType != DanConfig._DanType.Unknow
				&& ItemInfoUtility.GetDanType(d.ResourceId) == cost.SubType
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), d.BreakthoughtLevel, cost.BreakThroughLevelFrom, cost.BreakThroughLevelTo)
				&& !ItemInfoUtility.IsEquiped(d)
				&& !result.Contains(d))
				result.Add(d);
			else if (cost.SubType == DanConfig._DanType.Unknow
				&& ExchangeCheckBreakLevelValid(ExchangeGetCostType(cost), d.BreakthoughtLevel, cost.BreakThroughLevelFrom, cost.BreakThroughLevelTo)
				&& !ItemInfoUtility.IsEquiped(d)
				&& !result.Contains(d))
				result.Add(d);
		}

		return result;
	}

	private static bool ExchangeCheckBreakLevelValid(int assetType, int targetBreakLevel, int conditionMinLevel, int conditionMaxLevel)
	{
		if (ExchangeISBreakLevelValid(conditionMinLevel, conditionMaxLevel, assetType) == false)
			return true;

		return targetBreakLevel >= conditionMinLevel && targetBreakLevel <= conditionMaxLevel;
	}

	public static bool ExchangeIsOptionCost(object costOption)
	{
		// 判断cost的Id类型
		int costType = ExchangeGetCostType(costOption);

		return costType == IDSeg._AssetType.Avatar
			|| costType == IDSeg._AssetType.Equipment
			|| costType == IDSeg._AssetType.CombatTurn
			|| costType == IDSeg._AssetType.Dan;
	}

	public static int ExchangeGetCostType(object costOption)
	{
		// 获得cost的Id
		if (costOption is KodGames.ClientClass.ItemEx)
			return IDSeg.ToAssetType((costOption as KodGames.ClientClass.ItemEx).Id);

		if (costOption is KodGames.ClientClass.CostAsset)
			return (costOption as KodGames.ClientClass.CostAsset).Type;

		return IDSeg.InvalidId;
	}

	public static int ExchangeGetCostCount(object costOption)
	{
		//获取 cost count
		if (costOption is KodGames.ClientClass.ItemEx)
			return (costOption as KodGames.ClientClass.ItemEx).Count;

		if (costOption is KodGames.ClientClass.CostAsset)
			return (costOption as KodGames.ClientClass.CostAsset).Count;

		return 0;
	}

	public static int GetResourceIdByGuid(string guid)
	{
		//通过Guild获得ResourceId
		foreach (var a in SysLocalDataBase.Inst.LocalPlayer.Avatars)
			if (a.Guid.Equals(guid))
				return a.ResourceId;

		foreach (var e in SysLocalDataBase.Inst.LocalPlayer.Equipments)
			if (e.Guid.Equals(guid))
				return e.ResourceId;

		foreach (var s in SysLocalDataBase.Inst.LocalPlayer.Skills)
			if (s.Guid.Equals(guid))
				return s.ResourceId;

		foreach (var d in SysLocalDataBase.Inst.LocalPlayer.Dans)
			if (d.Guid.Equals(guid))
				return d.ResourceId;

		return IDSeg.InvalidId;
	}

	public static int GetBreakLevelByGuid(string guid)
	{
		//通过Guild获得BreakLevel
		foreach (var a in SysLocalDataBase.Inst.LocalPlayer.Avatars)
			if (a.Guid.Equals(guid))
				return a.BreakthoughtLevel;

		foreach (var e in SysLocalDataBase.Inst.LocalPlayer.Equipments)
			if (e.Guid.Equals(guid))
				return e.BreakthoughtLevel;

		foreach (var s in SysLocalDataBase.Inst.LocalPlayer.Skills)
			if (s.Guid.Equals(guid))
				return s.LevelAttrib.Level;

		foreach (var d in SysLocalDataBase.Inst.LocalPlayer.Dans)
			if (d.Guid.Equals(guid))
				return d.BreakthoughtLevel;

		return IDSeg.InvalidId;
	}

	public static string ExchangeGetCostName(object costOption)
	{
		//获取cost name
		if (costOption is KodGames.ClientClass.ItemEx)
		{
			var clientCost = costOption as KodGames.ClientClass.ItemEx;
			switch (IDSeg.ToAssetType(clientCost.Id))
			{
				case IDSeg._AssetType.Dan:
					return DanConfig._DanType.GetDisplayNameByType(ConfigDatabase.DefaultCfg.DanConfig.GetDanById(clientCost.Id).Type, ConfigDatabase.DefaultCfg);

				default:
					return ItemInfoUtility.GetAssetName(clientCost.Id);
			}
		}

		if (costOption is KodGames.ClientClass.CostAsset)
		{
			var costAsset = costOption as KodGames.ClientClass.CostAsset;
			string name = IDSeg._AssetType.GetDisplayNameByType(costAsset.Type, ConfigDatabase.DefaultCfg);
			string subTypeName = string.Empty;

			switch (costAsset.Type)
			{
				case IDSeg._AssetType.Avatar:
					if (costAsset.SubType != AvatarConfig._AvatarCountryType.UnKnow)
						subTypeName = GameUtility.FormatUIString(
							"UIPnlChooseCard_CostName_AvatarType",
							AvatarConfig._AvatarCountryType.GetDisplayNameByType(costAsset.SubType, ConfigDatabase.DefaultCfg));
					break;

				case IDSeg._AssetType.Equipment:
					if (costAsset.SubType != EquipmentConfig._Type.Unknown)
						subTypeName = EquipmentConfig._Type.GetDisplayNameByType(costAsset.SubType, ConfigDatabase.DefaultCfg);
					break;

				case IDSeg._AssetType.Dan:
					if (costAsset.SubType != DanConfig._DanType.Unknow)
						subTypeName = DanConfig._DanType.GetDisplayNameByType(costAsset.SubType, ConfigDatabase.DefaultCfg);
					break;
			}

			if (!string.IsNullOrEmpty(subTypeName))
			{
				if (costAsset.Type == IDSeg._AssetType.Dan)
					return subTypeName;
				else
					return GameUtility.FormatUIString("UIPnlChooseCard_CostName2",
								ItemInfoUtility.GetAssetQualityLevelCNDesc((costOption as KodGames.ClientClass.CostAsset).QualityLevel),
								subTypeName,
								name);
			}
			else
			{
				if ((costAsset.Type != IDSeg._AssetType.Dan))
					return GameUtility.FormatUIString("UIPnlChooseCard_CostName", ItemInfoUtility.GetAssetQualityLevelCNDesc((costOption as KodGames.ClientClass.CostAsset).QualityLevel), name);
				else
					return name;
			}
		}

		return "";
	}

	public static string ExchangeGetBreakLabelDesc(object costOption)
	{
		string breakLableDesc = string.Empty;
		if (ExchangeIsOptionCost(costOption))
		{
			int minBreakLevel = (costOption is KodGames.ClientClass.ItemEx) ? (costOption as KodGames.ClientClass.ItemEx).ExtensionBreakThroughLevelFrom : (costOption as KodGames.ClientClass.CostAsset).BreakThroughLevelFrom;
			int maxBreakLevel = (costOption is KodGames.ClientClass.ItemEx) ? (costOption as KodGames.ClientClass.ItemEx).ExtensionBreakThroughLevelTo : (costOption as KodGames.ClientClass.CostAsset).BreakThroughLevelTo;
			int resourceType = (costOption is KodGames.ClientClass.ItemEx) ? IDSeg.ToAssetType((costOption as KodGames.ClientClass.ItemEx).Id) : (costOption as KodGames.ClientClass.CostAsset).Type;

			if (ExchangeISBreakLevelValid(minBreakLevel, maxBreakLevel, resourceType))
			{
				var formatStr = string.Empty;
				if (minBreakLevel < maxBreakLevel)
				{
					switch (resourceType)
					{
						case IDSeg._AssetType.Avatar:
							formatStr = "UIPnlActivityExchangeTab_BreakRequireLabel1";
							break;
						case IDSeg._AssetType.Dan:
							formatStr = "UIPnlActivityExchangeTab_CultureRequireLabel1";
							break;
						default:
							formatStr = "UIPnlActivityExchangeTab_RefineRequireLabel1";
							break;

					}

					breakLableDesc = GameUtility.FormatUIString(formatStr, GetAssetBreakLevel(resourceType, minBreakLevel), GetAssetBreakLevel(resourceType, maxBreakLevel));
				}
				else
				{
					switch (resourceType)
					{
						case IDSeg._AssetType.Avatar:
							formatStr = "UIPnlActivityExchangeTab_BreakRequireLabel2";
							break;
						case IDSeg._AssetType.Dan:
							formatStr = "UIPnlActivityExchangeTab_CultureRequireLabel2";
							break;
						default:
							formatStr = "UIPnlActivityExchangeTab_RefineRequireLabel2";
							break;

					}

					breakLableDesc = GameUtility.FormatUIString(formatStr, GetAssetBreakLevel(resourceType, minBreakLevel));
				}
			}
		}

		return breakLableDesc;
	}

	private static bool ExchangeISBreakLevelValid(int minBreakLevel, int maxBreakLevel, int assetType)
	{
		if (assetType == IDSeg._AssetType.Dan)
			return minBreakLevel > 0 && maxBreakLevel > 0 && minBreakLevel <= maxBreakLevel;
		else
			return minBreakLevel >= 0 && maxBreakLevel >= 0 && minBreakLevel <= maxBreakLevel;
	}
	#endregion
}