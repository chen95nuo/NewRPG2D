using System;
using System.Collections.Generic;
using ClientServerCommon;

namespace MathFactory
{
	public class Avatar : IExpressionObject
	{
		//private string guid;
		//private int resourceId;
		//private string geniusSkillGuid;
		//private int level;
		//private int exprience;
		//private int breakthoughLevel;
		//private int totalTrainAvatar;
		//private int totalTrainItem;

		//public string Guid { get { return guid; } }
		//public string GeniusSkillGuid { get { return geniusSkillGuid; } }

		public void SetData(KodGames.ClientClass.Avatar avatar)
		{
			//this.guid = avatar.Guid;
			//this.geniusSkillGuid = avatar.GeniusSkillGuid;
			//this.resourceId = avatar.ResourceId;
			//this.level = avatar.LevelAttrib.Level;
			//this.exprience = avatar.LevelAttrib.Experience;
			//this.breakthoughLevel = avatar.BreakthoughtLevel;
			//this.totalTrainAvatar = avatar.TotalCostAvatar;
			//this.totalTrainItem = avatar.TotalCostItem;
		}

		public void SetupVariable(IMathParser parser)
		{
			//ClientServerCommon.AvatarConfig.Avatar avatarConfig= ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(this.resourceId);
			//ClientServerCommon.AvatarConfig avatarTotalConfig = ConfigDatabase.DefaultCfg.AvatarConfig;
			//ClientServerCommon.AvatarConfig.Avatar avatarConfig = avatarTotalConfig.GetAvatarById(this.resourceId);

			//// 角色累计升级经验
			//int avatar_TotalUpgradeExp = 0;
			//for (int levelIndex = 1; levelIndex < this.level && levelIndex < ConfigDatabase.DefaultCfg.LevelConfig.avatarMaxLevel; levelIndex++)
			//{
			//    avatar_TotalUpgradeExp += ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(levelIndex).avatarExp;
			//}
			//avatar_TotalUpgradeExp += this.exprience;

			//// 角色等级经验的下级所需经验
			//int avatar_ExpNextLevelUpgrading = 0;
			//if (this.level >= 0 && this.level <= ConfigDatabase.DefaultCfg.LevelConfig.avatarMaxLevel)
			//{
			//    avatar_ExpNextLevelUpgrading = ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(this.level).avatarExp;
			//}
			//else
			//{
			//    Debug.Log("AvatarConfig is error , resourceId :" + this.resourceId);
			//}

			//// 角色升级系数
			//float avatarConfig_UpgradeRequirementExpFactor = avatarConfig.upgradeRequirementExpFactor;

			//// 角色基本经验
			//int avatarConfig_BaseExperienceForUpgrading = avatarConfig.baseExperienceForUpgrading;

			//// 角色衰减折扣系数
			//float avatarConfig_ExpFactorForUpgrading = avatarConfig.expFactorForUpgrading;

			//// 角色身价基础值
			//int avatarConfig_EvaluationBase = avatarConfig.evaluation;

			//// 角色身价增长系数
			//float avatarConfig_EvaluationIncreaseFactor = avatarConfig.evaluationIncreaseFactor;

			//// 角色出售价格基数
			//int avatarConfig_SellPrice = 0;
			//if (avatarConfig != null && avatarConfig.sellRewards.Count >= 1)
			//{
			//    avatarConfig_SellPrice = avatarConfig.sellRewards[0].count;
			//}
			//else
			//{
			//    Debug.Log("AvatarConfig is error , resourceId :" + this.resourceId);
			//}

			//parser.SetVariable("AvatarConfig_SellPrice", avatarConfig_SellPrice);
			//parser.SetVariable("TotalCostAvatar", totalTrainAvatar);
			//parser.SetVariable("TotalCostItem", totalTrainItem);
			//parser.SetVariable("ItemSellVar1", avatarConfig.GetAvatarBreakthrough(this.breakthoughLevel).breakThrough.ItemSellVar1);
			//parser.SetVariable("ItemSellVar2", avatarConfig.GetAvatarBreakthrough(this.breakthoughLevel).breakThrough.ItemSellVar2);
			//parser.SetVariable("ItemSellVar3", avatarConfig.GetAvatarBreakthrough(this.breakthoughLevel).breakThrough.ItemSellVar3);
			//parser.SetVariable("Avatar_Level", this.level);
			//parser.SetVariable("Avatar_BreakthoughtLevel", this.breakthoughLevel);
			//parser.SetVariable("Avatar_ExpNextLevelUpgrading", avatar_ExpNextLevelUpgrading);

			//parser.SetVariable("AvatarConfig_UpgradeRequirementExpFactor", avatarConfig_UpgradeRequirementExpFactor);
			//parser.SetVariable("AvatarConfig_BaseExperienceForUpgrading", avatarConfig_BaseExperienceForUpgrading);
			//parser.SetVariable("Avatar_TotalUpgradeExp", avatar_TotalUpgradeExp);

			//parser.SetVariable("AvatarConfig_ExpFactorForUpgrading", avatarConfig_ExpFactorForUpgrading);

			//parser.SetVariable("AvatarConfig_EvaluationBase", avatarConfig_EvaluationBase);
			//parser.SetVariable("AvatarConfig_EvaluationIncreaseFactor", avatarConfig_EvaluationIncreaseFactor);
		}
	}


	public class Equipment : IExpressionObject
	{
		//private int resourceId;
		//private int level;
		//private int breakthoughLevel;

		public void SetData(KodGames.ClientClass.Equipment equipment)
		{
			//this.resourceId = equipment.ResourceId;
			//this.level = equipment.LevelAttrib.Level;
			//this.breakthoughLevel = equipment.BreakthoughtLevel;
		}

		public void SetupVariable(IMathParser parser)
		{
			//ClientServerCommon.EquipmentConfig.Equipment equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(this.resourceId);

			//// EquipConfig_TotalMoney 
			//int equipConfig_TotalMoney = EquipmentConfig.GetEquipmentTotalMoney(ConfigDatabase.DefaultCfg, resourceId, level);

			//// 装备出售价格基数
			//int equipConfig_SellPrice = 0;
			//if (equipConfig != null && equipConfig.sellRewards.Count >= 1)
			//{
			//    equipConfig_SellPrice = equipConfig.sellRewards[0].count;
			//}
			//else
			//{
			//    Debug.Log("EquipmentConfig is error , resourceId :" + this.resourceId);
			//}

			//// 装备等级经验的下级所需经验
			//int equip_ExpNextLevelUpgrading = 0;
			//if (this.level >= 0 && this.level <= ConfigDatabase.DefaultCfg.LevelConfig.equipmentMaxLevel)
			//{
			//    equip_ExpNextLevelUpgrading = ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(this.level).equipmentExp;
			//}
			//else
			//{
			//    Debug.Log("EquipmentConfig is error , resourceId :" + this.resourceId);
			//}

			////装备升级系数
			//float equipConfig_UpgradeRequirementExpFactor = equipConfig.upgradeRequirementExpFactor;

			//// 此等级消耗的钱
			//int equip_EquipmentUpgradeMoney = ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(this.level).equipmentUpgradeMoney;

			//// 装备强化系数
			//float Equip_UpgradeRequirementMoneyFactor = equipConfig.upgradeRequirementMoneyFactor;

			//// 装备基本经验
			//int equipConfig_BaseExperienceForUpgrading = equipConfig.baseExperienceForUpgrading;

			//// 装备累计升级经验
			//int equip_TotalUpgradeExp = 0;

			//for (int levelIndex = 1; levelIndex < this.level && levelIndex <= ConfigDatabase.DefaultCfg.LevelConfig.equipmentMaxLevel; ++levelIndex)
			//{
			//    equip_TotalUpgradeExp += ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(levelIndex).equipmentExp;
			//}
			//equip_TotalUpgradeExp += this.exprience;

			//// 装备衰减折扣系数
			//float equipConfig_ExpFactorForUpgrading = equipConfig.expFactorForUpgrading;

			//// 装备基础身价
			//int equipConfig_Evaluation = equipConfig.evaluation;

			//// 装备身价增长系数 (装备增长值)
			//float equipConfig_EvaluationIncreaseFactor = equipConfig.evaluationIncreaseFactor;

			//parser.SetVariable("ItemSellVar1", equipConfig.GetBreakthroughByTimes(breakthoughLevel).breakThrough.ItemSellVar1);
			//parser.SetVariable("ItemSellVar2", equipConfig.GetBreakthroughByTimes(breakthoughLevel).breakThrough.ItemSellVar2);
			//parser.SetVariable("ItemSellVar3", equipConfig.GetBreakthroughByTimes(breakthoughLevel).breakThrough.ItemSellVar3);
			//parser.SetVariable("Equip_Level", this.level);

			//parser.SetVariable("EquipConfig_TotalMoney", equipConfig_TotalMoney);
			//parser.SetVariable("EquipConfig_SellPrice", equipConfig_SellPrice);
			//parser.SetVariable("TotalCostEquipment", totalBreakCostEquip);
			//parser.SetVariable("TotalCostItem", totalBreakCostItem);
			//parser.SetVariable("Equip_BreakthoughtLevel", this.breakthoughLevel);
			//parser.SetVariable("Equip_ExpNextLevelUpgrading", equip_ExpNextLevelUpgrading);
			//parser.SetVariable("EquipConfig_UpgradeRequirementExpFactor", equipConfig_UpgradeRequirementExpFactor);
			//parser.SetVariable("EquipConfig_BaseExperienceForUpgrading", equipConfig_BaseExperienceForUpgrading);
			//parser.SetVariable("Equip_TotalUpgradeExp", equip_TotalUpgradeExp);
			//parser.SetVariable("EquipConfig_ExpFactorForUpgrading", equipConfig_ExpFactorForUpgrading);
			//parser.SetVariable("EquipConfig_Evaluation", equipConfig_Evaluation);
			//parser.SetVariable("EquipConfig_EvaluationIncreaseFactor", equipConfig_EvaluationIncreaseFactor);
			//parser.SetVariable("Equip_EquipmentUpgradeMoney", equip_EquipmentUpgradeMoney);
			//parser.SetVariable("Equip_UpgradeRequirementMoneyFactor", Equip_UpgradeRequirementMoneyFactor);
		}
	}



	public class Skill : IExpressionObject
	{
		//private int resourceId;
		//private int level;
		//private int exprience;

		public void SetData(KodGames.ClientClass.Skill skill)
		{
			//this.resourceId = skill.ResourceId;
			//this.level = skill.LevelAttrib.Level;
			//this.exprience = skill.LevelAttrib.Experience;
		}

		public void SetupVariable(IMathParser parser)
		{
			//ClientServerCommon.SkillConfig skillTotalConfig = ConfigDatabase.DefaultCfg.SkillConfig;
			//ClientServerCommon.SkillConfig.Skill skillConfig = skillTotalConfig.GetSkillById(this.resourceId);

			////武功升级系数
			//float skillConfig_upgradeRequirementExpFactor = skillConfig.upgradeRequirementExpFactor;

			//// 技能出售价格基数
			//int skillConfig_SellPrice = 0;
			//if (skillConfig != null && skillConfig.sellRewards.Count >= 1)
			//{
			//    skillConfig_SellPrice = skillConfig.sellRewards[0].count;
			//}
			//else
			//{
			//    Debug.Log("SkillConfig is error , resourceId :" + this.resourceId);
			//}

			//// 武功累计升级经验
			//float skill_TotalUpgradeExp = 0;

			//for (int levelIndex = 1; levelIndex < this.level && levelIndex <= skillTotalConfig.GetUpgradeMaxLevel(); levelIndex++)
			//{
			//    skill_TotalUpgradeExp += skillTotalConfig.GetUpgradeLevelRequiredExpByLevel(levelIndex) * skillConfig_upgradeRequirementExpFactor;
			//}
			//skill_TotalUpgradeExp += this.exprience;

			//// 武功等级经验的下级所需经验
			//int Skill_ExpNextLevelUpgrading = 0;
			//if (this.level >= 0 && this.level <= skillTotalConfig.GetUpgradeMaxLevel())
			//{
			//    Skill_ExpNextLevelUpgrading = skillTotalConfig.GetUpgradeLevelRequiredExpByLevel(this.level);
			//}
			//else
			//{
			//    Debug.Log("SkillConfig is error ,resourceId :" + this.resourceId);
			//}

			//// 武功衰减折扣系数
			//float skillConfig_ExpFactorForUpgrading = skillConfig.expFactorForUpgrading;
			//// 武功基础经验值
			//int skillConfig_BaseExperienceForUpgrading = skillConfig.baseExperienceForUpgrading;
			//// 同种武功加成系数
			//float skillConfig_SameSupplyFactor = skillConfig.sameSupplyFactor;

			//// 武功基础身价
			//int skillConfig_Evaluation = skillConfig.evaluation;
			//// 武功身价增长系数 (武功增长值)
			//float skillConfig_EvaluationIncreaseFactor = skillConfig.evaluationIncreaseFactor;

			//parser.SetVariable("SkillConfig_SellPrice", skillConfig_SellPrice);
			//parser.SetVariable("Skill_Level", this.level);
			//parser.SetVariable("Skill_TotalUpgradeExp", skill_TotalUpgradeExp);
			//parser.SetVariable("SkillConfig_ExpFactorForUpgrading", skillConfig_ExpFactorForUpgrading);
			//parser.SetVariable("SkillConfig_BaseExperienceForUpgrading", skillConfig_BaseExperienceForUpgrading);
			//parser.SetVariable("SkillConfig_SameSupplyFactor", skillConfig_SameSupplyFactor);
			//parser.SetVariable("Skill_ExpNextLevelUpgrading", Skill_ExpNextLevelUpgrading);
			//parser.SetVariable("SkillConfig_UpgradeRequirementExpFactor", skillConfig_upgradeRequirementExpFactor);
			//parser.SetVariable("SkillConfig_Evaluation", skillConfig_Evaluation);
			//parser.SetVariable("SkillConfig_EvaluationIncreaseFactor", skillConfig_EvaluationIncreaseFactor);
		}
	}

	public class SkillCheck_SameSkill : IExpressionObject
	{
		private bool bSameSkillId = false;

		public SkillCheck_SameSkill(bool bSameSkillId)
		{
			this.bSameSkillId = bSameSkillId;
		}

		public void SetupVariable(IMathParser parser)
		{
			int SameSkill = 0;
			if (bSameSkillId)
			{
				SameSkill = 1;
			}
			parser.SetVariable("__SameSkill", SameSkill);
		}
	}

	public class ExpressionCalculate
	{

		private static Dictionary<string, IMathParser> parsers = new Dictionary<string, IMathParser>();

		private static double CalcExpression(string exp, params IExpressionObject[] objs)
		{
			IMathParser parser = null;

			if (parsers.ContainsKey(exp))
				parsers.TryGetValue(exp, out parser);

			if (parser == null)
			{
				parser = ConfigDatabase.MathParserFactory.CreateMathParser(exp);
				parsers.Add(exp, parser);
			}

			foreach (IExpressionObject obj in objs)
			{
				obj.SetupVariable(parser);
			}

			return parser.Evaluate();
		}


		// 角色升级消耗金币公式=（被消耗角色1基数+被消耗角色2基数+…..被消耗角色n）
		public static int GetValue_Avatar_AvatarUpgradeCostCount(List<KodGames.ClientClass.Avatar> avatars)
		{
			//double costTotal = 0;
			//foreach (KodGames.ClientClass.Avatar avatar in avatars)
			//{
			//    int AvatarConfig_UpgradeRequirementAddtionalCost = 0;
			//    ClientServerCommon.AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);
			//    if (avatarConfig != null)
			//    {
			//        // 被消耗角色1基数 AvatarConfig_UpgradeRequirementAddtionalCost
			//        AvatarConfig_UpgradeRequirementAddtionalCost = avatarConfig.upgradeRequirementAddtionalCost.count;
			//        costTotal += AvatarConfig_UpgradeRequirementAddtionalCost;
			//    }
			//}
			//return KodGames.Math.RoundToInt(costTotal);
			return 0;
		}

		// 装备升级消耗金币公式=（被消耗装备1基数+被消耗装备2基数+…..被消耗装备n）
		public static int GetValue_Equip_EquipUpgradeCostCount(List<KodGames.ClientClass.Equipment> equipments)
		{
			//double costTotal = 0;
			//foreach (KodGames.ClientClass.Equipment equipment in equipments)
			//{
			//    int EquipConfig_UpgradeRequirementAddtionalCost = 0;
			//    ClientServerCommon.EquipmentConfig.Equipment equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId);
			//    if (equipConfig != null)
			//    {
			//        // 被消耗装备1基数 EquipConfig_UpgradeRequirementAddtionalCost
			//        EquipConfig_UpgradeRequirementAddtionalCost = equipConfig.upgradeRequirementAddtionalCost.count;
			//        costTotal += EquipConfig_UpgradeRequirementAddtionalCost;
			//    }
			//}
			//return KodGames.Math.RoundToInt(costTotal);
			return 0;
		}

		// 装备卖出价格公式=变量1+变量2*(当前卡牌等级-1)*(1+变量3)*0.3
		// ItemSellVar1 + ItemSellVar2 * (Equip_Level - 1) * (1 + ItemSellVar3) * 0.5
		public static int GetValue_EquipSellPrice(KodGames.ClientClass.Equipment kd_equipment)
		{
			//Equipment equipment = new Equipment();
			//equipment.SetData(kd_equipment);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.EquipmentConfig.sellPrice_Expression, equipment));
			return 0;
		}

		// 装备卖出，精炼石价格公式
		public static int GetValue_EquipSellItemPrice(KodGames.ClientClass.Equipment kd_equipment)
		{
			//Equipment equipment = new Equipment();
			//equipment.SetData(kd_equipment);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.EquipmentConfig.sellPrice_Item_Expression, equipment));
			return 0;
		}

		// 装备碎片卖出，价格公式
		public static int GetValue_EquipScrollSellItemPrice(KodGames.ClientClass.Equipment kd_equipment)
		{
			//EquipmentConfig.Equipment equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipByFragmentId(kd_equipment.ResourceId);
			//int gameMoney = 0;
			//int oneEquipMoney = equipConfig.sellRewards[0].count;
			//gameMoney += oneEquipMoney * ((kd_equipment as EquipScroll).Amount / equipConfig.equipScorll.count);
			//gameMoney += (oneEquipMoney / equipConfig.equipScorll.count) * ((kd_equipment as EquipScroll).Amount % equipConfig.equipScorll.count);

			return 0;
		}

		// 角色碎片卖出，价格公式
		public static int GetValue_AvatarScrollSellItemPrice(KodGames.ClientClass.Avatar kd_avatar)
		{
			//AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarByFragmentId(kd_avatar.ResourceId);
			//return avatarConfig.avatarScorll.sellPrice * (kd_avatar as AvatarScroll).Amount;
			return 0;
		}

		// 角色卖出价格公式变量1+变量2*(当前卡牌等级-1)*(1+变量3)*0.3
		// ItemSellVar1 + ItemSellVar2 * (Avatar_Level - 1) * (1 + ItemSellVar3) * 0.5
		public static int GetValue_AvatarSellPrice(KodGames.ClientClass.Avatar kd_avatar)
		{
			//Avatar avatar = new Avatar();
			//avatar.SetData(kd_avatar);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.AvatarConfig.sellPrice_Expression, avatar));
			return 0;
		}

		// 武功卖出价格公式=武功出售价格基数+（武功出售价格基数/常量）*（lv-1）
		// SkillConfig_SellPrice+(SkillConfig_SellPrice/1.0) * (Skill_Level-1)
		public static int GetValue_SkillSellPrice(KodGames.ClientClass.Skill kd_skill)
		{
			//Skill skill = new Skill();
			//skill.SetData(kd_skill);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.SkillConfig.sellPrice_Expression, skill));
			return 0;
		}


		// 角色提供经验=角色基本经验 + 累计升级经验 * 角色衰减折扣系数
		// AvatarConfig_BaseExperienceForUpgrading+Avatar_TotalUpgradeExp*AvatarConfig_ExpFactorForUpgrading
		public static int GetValue_AvatarSupplyExperience(KodGames.ClientClass.Avatar kd_avatar)
		{
			//Avatar avatar = new Avatar();
			//avatar.SetData(kd_avatar);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.AvatarConfig.supplyExperience_Expression, avatar));
			return 0;
		}

		// 角色升级所需经验=等级经验*角色升级系数
		// Avatar_ExpNextLevelUpgrading*AvatarConfig_UpgradeRequirementExpFactor
		public static int GetValue_AvatarUpgradeNeedExperience(KodGames.ClientClass.Avatar kd_avatar)
		{
			//Avatar avatar = new Avatar();
			//avatar.SetData(kd_avatar);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.AvatarConfig.upgradeNeedExperience_Expression, avatar));
			return 0;
		}

		// 装备强化消耗金币 = 基本金币 * 系数
		// 装备升级消耗金币公式= Equip_UpgradeRequirementMoneyFactor * Equip_EquipmentUpgradeMoney
		public static int GetValue_EquipmentStrengthCostCount(KodGames.ClientClass.Equipment kd_equipment)
		{
			//Equipment equipment = new Equipment();
			//equipment.SetData(kd_equipment);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.EquipmentConfig.upgradeRequirementCostCount_Expression, equipment));
			return 0;
		}

		//  武功升级所需经验：武功等级经验的下级所需经验*武功升级系数
		// Skill_ExpNextLevelUpgrading*SkillConfig_UpgradeRequirementExpFactor
		public static int GetValue_SkillUpgradeNeedExperience(KodGames.ClientClass.Skill kd_skill)
		{
			//Skill skill = new Skill();
			//skill.SetData(kd_skill);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.SkillConfig.upgradeNeedExperience_Expression, skill));
			return 0;
		}

		// 武功提供经验：（当前累计经验*折扣比例+基础经验值）*（1+同种武功加成系数）
		// (Skill_TotalUpgradeExp*SkillConfig_ExpFactorForUpgrading+SkillConfig_BaseExperienceForUpgrading)*(1+SkillConfig_SameSupplyFactor)
		public static int GetValue_SkillSupplyExperience(KodGames.ClientClass.Skill kd_skill, bool isSameSkillId)
		{
			//Skill skill = new Skill();
			//skill.SetData(kd_skill);

			//SkillCheck_SameSkill skillCheck_SameSkill = new SkillCheck_SameSkill(isSameSkillId);

			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.SkillConfig.supplyExperience_Expression, skill, skillCheck_SameSkill));
			return 0;
		}

		// 门派身价=所有出战角色总身价之和
		public static int GetValue_PlayerEvaluation()
		{
			//double playerEvaluation = 0;

			//foreach (KodGames.ClientClass.Avatar avatar in PlayerDataUtility.GetLineUpAvatars(SysLocalDataBase.Inst.LocalPlayer))
			//    playerEvaluation += GetValue_AvatarTotalEvaluation(avatar);

			//return KodGames.Math.RoundToInt(playerEvaluation);
			return 0;
		}

		// 角色总身价=角色身价+装备身价+技能身价+阵法身价
		public static int GetValue_AvatarTotalEvaluation(KodGames.ClientClass.Avatar kd_avatar)
		{
			//return KodGames.Math.RoundToInt(GetValue_AvatarTotalEvaluation(SysLocalDataBase.Inst.LocalPlayer, kd_avatar));
			return 0;
		}


		// 角色总身价=角色身价+装备身价+技能身价+阵法身价
		public static int GetValue_AvatarTotalEvaluation(KodGames.ClientClass.Player player, KodGames.ClientClass.Avatar kd_avatar)
		{
			//Avatar avatar = new Avatar();
			//avatar.SetData(kd_avatar);

			//// 角色总身价
			//double AvatarTotalEvaluation = 0;

			//// 角色身价
			//AvatarTotalEvaluation += GetValue_AvatarEvaluation(kd_avatar);

			//// 装备身价
			//foreach (KodGames.ClientClass.Equipment equip in player.Equipments)
			//{
			//    if (equip.AvatarGUID.Equals(avatar.Guid))
			//    {
			//        AvatarTotalEvaluation += GetValue_EquipmentEvaluation(equip);
			//    }
			//}

			//// 技能身价
			//foreach (KodGames.ClientClass.Skill skill in player.Skills)
			//{
			//    if (skill.AvatarGUID.Equals(avatar.Guid))
			//    {
			//        AvatarTotalEvaluation += GetValue_SkillEvaluation(skill);
			//    }
			//}

			//return KodGames.Math.RoundToInt(AvatarTotalEvaluation);
			return 0;
		}

		// 角色身价公式=角色身价基础值+（角色等级-1）*角色身价增长系数
		// AvatarConfig_EvaluationBase+(Avatar_Level-1)*AvatarConfig_EvaluationIncreaseFactor
		public static int GetValue_AvatarEvaluation(KodGames.ClientClass.Avatar kd_avatar)
		{
			//Avatar avatar = new Avatar();
			//avatar.SetData(kd_avatar);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.AvatarConfig.evaluation_Expression, avatar));
			return 0;
		}

		// 装备身价公式=装备基础身价+（等级-1）*当前装备增长值+（常量*精练等级）
		// EquipConfig_Evaluation+(Equip_Level-1) * EquipConfig_EvaluationIncreaseFactor + (1.0*Equip_BreakthoughtLevel)
		public static int GetValue_EquipmentEvaluation(KodGames.ClientClass.Equipment kd_equipment)
		{
			//Equipment equipment = new Equipment();
			//equipment.SetData(kd_equipment);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.EquipmentConfig.evaluation_Expression, equipment));
			return 0;
		}

		// 技能/阵法 身价公式=技能基础身价+（等级-1）*当前技能增长值
		// SkillConfig_Evaluation+(Skill_Level-1) * SkillConfig_EvaluationIncreaseFactor
		public static int GetValue_SkillEvaluation(KodGames.ClientClass.Skill kd_skill)
		{
			//Skill skill = new Skill();
			//skill.SetData(kd_skill);
			//return KodGames.Math.RoundToInt(CalcExpression(ConfigDatabase.DefaultCfg.SkillConfig.evaluation_Expression, skill));
			return 0;
		}
	}
}
