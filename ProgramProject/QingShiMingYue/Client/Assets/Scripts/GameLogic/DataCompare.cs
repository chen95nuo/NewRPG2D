using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;

public static class DataCompare
{
	public static int CompareAttribute(AttributeCalculator.Attribute a1, AttributeCalculator.Attribute a2)
	{
		return ConfigDatabase.DefaultCfg.GameConfig.GetShowAttributeSortIndex(a1.type) - ConfigDatabase.DefaultCfg.GameConfig.GetShowAttributeSortIndex(a2.type);
	}

	public static int CompareClientServerAttribute(ClientServerCommon.Attribute a1, ClientServerCommon.Attribute a2)
	{
		return ConfigDatabase.DefaultCfg.GameConfig.GetShowAttributeSortIndex(a1.type) - ConfigDatabase.DefaultCfg.GameConfig.GetShowAttributeSortIndex(a2.type);
	}

	public static int CompareLocationByShowPos(KodGames.ClientClass.Location l1, KodGames.ClientClass.Location l2)
	{
		return l1.ShowLocationId - l2.ShowLocationId;
	}

	public static int CompareBattlePosition(KodGames.ClientClass.Avatar a1, KodGames.ClientClass.Avatar a2)
	{
		//return a1.BattlePosition - a2.BattlePosition;
		return 0;
	}

	public static int CompareBeastPart(KodGames.ClientClass.Consumable c1, KodGames.ClientClass.Consumable c2)
	{
		var i1 = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartByBeastPartId(c1.Id);
		var i2 = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartByBeastPartId(c2.Id);

		if (i1.SortPart != i2.SortPart)
			return i2.SortPart - i1.SortPart;

		return 0;
	}

	public static int CompareConsumable(KodGames.ClientClass.Consumable c1, KodGames.ClientClass.Consumable c2)
	{
		var i1 = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(c1.Id);
		var i2 = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(c2.Id);

		if (i1 == null)
		{
			Debug.Log(string.Format("ItemConfig id {0} not Found.", c1.Id.ToString("X")));
			return 0;
		}

		if (i2 == null)
		{
			Debug.Log(string.Format("ItemConfig id {0} not Found.", c2.Id.ToString("X")));
			return 0;
		}

		if (i1.sortIdx != i2.sortIdx)
			return i2.sortIdx - i1.sortIdx;
		else
			return i2.id - i1.id;
	}

	/// <summary>
	/// 1、	已上阵的角色靠上
	/// 2、	品质越高越靠上
	/// 3、	同品质时，按国家标识进行排序，使用角色优先级控制（策划用）
	/// 4、	同品质、同国家时，根据角色表中角色的优先级显示，优先级越高越靠上
	/// 5、	同一角色有多个时，根据突破等级排序，突破等级越高越靠上
	/// 6、	同一角色同突破等级时，根据升级等级排序，等级越高越靠上
	/// 7、	同角色同突破同升级等级时，随机排序。

	/// </summary>
	public static int CompareAvatar(KodGames.ClientClass.Avatar a1, KodGames.ClientClass.Avatar a2)
	{
		int lineuped1 = (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, a1) || PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, a1)) ? 1 : 0;
		int lineuped2 = (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, a2) || PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, a2)) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		AvatarConfig.Avatar a1Cfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a1.ResourceId);
		AvatarConfig.Avatar a2Cfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a2.ResourceId);

		if (a1Cfg.qualityLevel != a2Cfg.qualityLevel)
			return a2Cfg.qualityLevel - a1Cfg.qualityLevel;
		else
		{
			if (a1Cfg.sortIndex != a2Cfg.sortIndex)
				return a2Cfg.sortIndex - a1Cfg.sortIndex;
			else if (a1.BreakthoughtLevel != a2.BreakthoughtLevel)
				return a2.BreakthoughtLevel - a1.BreakthoughtLevel;
			else if (a1.LevelAttrib.Level != a2.LevelAttrib.Level)
				return a2.LevelAttrib.Level - a1.LevelAttrib.Level;
		}

		return 0;
	}

	/// <summary>
	/// 1、	品质越低越靠上
	/// 2、	同品质时，按国家标识进行排序，使用角色优先级控制（策划用）
	/// 3、	同品质、同国家时，根据角色表中角色的优先级显示，优先级越低越靠上
	/// 4、	同一角色有多个时，根据突破等级排序，突破等级越低越靠上
	/// 5、	同一角色同突破等级时，根据升级等级排序，等级越低越靠上
	/// 6、	同角色同突破同升级等级时，随机排序。
	/// </summary>
	public static int CompareAvatarReverse(KodGames.ClientClass.Avatar a1, KodGames.ClientClass.Avatar a2)
	{
		int lineuped1 = (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, a1) || PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, a1)) ? 1 : 0;
		int lineuped2 = (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, a2) || PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, a2)) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped1 - lineuped2;

		AvatarConfig.Avatar a1Cfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a1.ResourceId);
		AvatarConfig.Avatar a2Cfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(a2.ResourceId);

		if (a1Cfg.qualityLevel != a2Cfg.qualityLevel)
			return a1Cfg.qualityLevel - a2Cfg.qualityLevel;
		else
		{
			if (a1Cfg.sortIndex != a2Cfg.sortIndex)
				return a1Cfg.sortIndex - a2Cfg.sortIndex;
			else if (a1.BreakthoughtLevel != a2.BreakthoughtLevel)
				return a1.BreakthoughtLevel - a2.BreakthoughtLevel;
			else if (a1.LevelAttrib.Level != a2.LevelAttrib.Level)
				return a1.LevelAttrib.Level - a2.LevelAttrib.Level;
		}
		return 0;
	}

	// 图鉴等使用配置信息的列表中直接使用排序id，正序，无反序
	public static int CompareAvatar(AvatarConfig.Avatar a1, AvatarConfig.Avatar a2)
	{
		return a2.sortIndex - a1.sortIndex;
	}

	public static int CompareAvatar(int avatarAssetId1, int avatarAssetId2)
	{
		AvatarConfig.Avatar avatar1 = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarAssetId1);
		AvatarConfig.Avatar avatar2 = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarAssetId2);
		return CompareAvatar(avatar1, avatar2);
	}

	public static int CompareTavern(TavernConfig.Tavern t1, TavernConfig.Tavern t2)
	{
		return t2.Priority - t1.Priority;
	}

	/// <summary>
	/// 1、	已被装备的装备靠上
	/// 2、	品质越高越靠上
	/// 3、	同品质时，使用装备优先级控制，优先级越高越靠上，依次是武器、衣服、鞋子、首饰、宝物（策划用）
	/// 4、	同品质、同部位时，根据装备表中装备的优先级显示，优先级越高越靠上
	/// 5、	同装备有多个时，根据精炼等级排序，精炼等级越高越靠上
	/// 6、	同装备同突破等级时，根据装备等级排序，等级越高越靠上
	/// 7、	同装备同突破同强化等级时，随机排序。
	/// </summary>
	public static int CompareEquipment(KodGames.ClientClass.Equipment e1, KodGames.ClientClass.Equipment e2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		EquipmentConfig.Equipment e1Cfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e1.ResourceId);
		EquipmentConfig.Equipment e2Cfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e2.ResourceId);

		if (e1Cfg.qualityLevel != e2Cfg.qualityLevel)
			return e2Cfg.qualityLevel - e1Cfg.qualityLevel;
		else if (e1Cfg.sortIndex != e2Cfg.sortIndex)
			return e2Cfg.sortIndex - e1Cfg.sortIndex;
		else if (e1.BreakthoughtLevel != e2.BreakthoughtLevel)
			return e2.BreakthoughtLevel - e1.BreakthoughtLevel;
		else if (e1.LevelAttrib.Level != e2.LevelAttrib.Level)
			return e2.LevelAttrib.Level - e1.LevelAttrib.Level;

		return 0;
	}

	/// <summary>
	/// 1、	品质越低越靠上
	/// 2、	同品质时，优先级越低越靠上显示，从上向下依次是武器、衣服、鞋子、首饰、宝物（策划用）
	/// 3、	同品质、同部位时，根据装备表中装备的优先级显示，优先级越低越靠上
	/// 4、	同装备有多个时，根据精炼等级排序，精炼等级越低越靠上
	/// 5、	同装备同精炼等级时，根据装备等级排序，等级越低越靠上
	/// 6、	同装备同突破同强化等级时，随机排序。
	/// </summary>
	public static int CompareEquipmentReverse(KodGames.ClientClass.Equipment e1, KodGames.ClientClass.Equipment e2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped1 - lineuped2;

		EquipmentConfig.Equipment e1Cfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e1.ResourceId);
		EquipmentConfig.Equipment e2Cfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e2.ResourceId);

		if (e1Cfg.qualityLevel != e2Cfg.qualityLevel)
			return e1Cfg.qualityLevel - e2Cfg.qualityLevel;
		else if (e1Cfg.sortIndex != e2Cfg.sortIndex)
			return e1Cfg.sortIndex - e2Cfg.sortIndex;
		else if (e1.BreakthoughtLevel != e2.BreakthoughtLevel)
			return e1.BreakthoughtLevel - e2.BreakthoughtLevel;
		else if (e1.LevelAttrib.Level != e2.LevelAttrib.Level)
			return e1.LevelAttrib.Level - e2.LevelAttrib.Level;

		return 0;
	}

	/// <summary>
	/// 1、	是否缘分(是，排在上面使用sortIndex排序）
	/// 2、 已被装备的装备靠上
	/// 3、	品质越高越靠上
	/// 4、	同品质时，使用装备优先级控制，优先级越高越靠上，依次是武器、衣服、鞋子、首饰、宝物（策划用）
	/// 5、	同品质、同部位时，根据装备表中装备的优先级显示，优先级越高越靠上
	/// 6、	同装备有多个时，根据精炼等级排序，精炼等级越高越靠上
	/// 7、	同装备同突破等级时，根据装备等级排序，等级越高越靠上
	/// 8、	同装备同突破同强化等级时，随机排序。
	/// </summary>
	public static int CompareEquipmentForLineUp(KodGames.ClientClass.Equipment e1, KodGames.ClientClass.Equipment e2)
	{
		int assemble1 = e1.IsAssembleActive ? 1 : 0;
		int assemble2 = e2.IsAssembleActive ? 1 : 0;

		if (assemble1 != assemble2)
			return assemble2 - assemble1;

		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		var e1Cfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e1.ResourceId);
		var e2Cfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e2.ResourceId);

		if (e1Cfg.qualityLevel != e2Cfg.qualityLevel)
			return e2Cfg.qualityLevel - e1Cfg.qualityLevel;
		else if (e1Cfg.sortIndex != e2Cfg.sortIndex)
			return e2Cfg.sortIndex - e1Cfg.sortIndex;
		else if (e1.BreakthoughtLevel != e2.BreakthoughtLevel)
			return e2.BreakthoughtLevel - e1.BreakthoughtLevel;
		else if (e1.LevelAttrib.Level != e2.LevelAttrib.Level)
			return e2.LevelAttrib.Level - e1.LevelAttrib.Level;

		return 0;
	}

	public static int CompareEquipment(EquipmentConfig.Equipment e1, EquipmentConfig.Equipment e2)
	{
		return e2.sortIndex - e1.sortIndex;
	}

	public static int CompareEquipment(int e1, int e2)
	{
		EquipmentConfig.Equipment c1 = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e1);
		EquipmentConfig.Equipment c2 = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(e2);
		return CompareEquipment(c1, c2);
	}





	/// <summary>
	/// 1、	已被装备的靠上
	/// 2、	品质越高越靠上
	/// 3、	同品质，优先级越高越靠上
	/// 4、	同品质同书籍时，根据书籍等级排序，等级越高越靠上
	/// 5、	同品质同书籍同等级时，随机排序。
	/// </summary>
	public static int CompareSkill(KodGames.ClientClass.Skill s1, KodGames.ClientClass.Skill s2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, s1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, s2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		SkillConfig.Skill s1Cfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s1.ResourceId);
		SkillConfig.Skill s2Cfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s2.ResourceId);

		if (s1Cfg.qualityLevel != s2Cfg.qualityLevel)
			return s2Cfg.qualityLevel - s1Cfg.qualityLevel;
		else if (s1Cfg.sortIndex != s2Cfg.sortIndex)
			return s2Cfg.sortIndex - s1Cfg.sortIndex;
		else if (s1.LevelAttrib.Level != s2.LevelAttrib.Level)
			return s2.LevelAttrib.Level - s1.LevelAttrib.Level;

		return 0;
	}

	public static int CompareBeast(KodGames.ClientClass.Beast b1, KodGames.ClientClass.Beast b2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, b1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, b2) ? 1 : 0;
	
		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		BeastConfig.BaseInfo b1Cfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(b1.ResourceId);
		BeastConfig.BaseInfo b2Cfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(b2.ResourceId);

		if (b1.BreakthoughtLevel != b2.BreakthoughtLevel)
			return b2.BreakthoughtLevel - b1.BreakthoughtLevel;
		else if (b1.LevelAttrib.Level != b2.LevelAttrib.Level)
			return b2.LevelAttrib.Level - b1.LevelAttrib.Level;
		else if (b1Cfg.Priority != b2Cfg.Priority)
			return b2Cfg.Priority - b1Cfg.Priority;

		return 0;
	}

	public static int CompareBeast(BeastConfig.BaseInfo b1, BeastConfig.BaseInfo b2)
	{
		 if (b1.Priority != b2.Priority)
			return b1.Priority - b2.Priority;

		return 0;
	}

	/// <summary>
	/// 1、	品质越低越靠上
	/// 2、	同品质，优先级越低越靠上
	/// 3、	同品质同书籍时，根据书籍等级排序，等级越低越靠上
	/// 4、	同品质同书籍同等级时，随机排序。
	/// </summary>
	public static int CompareSkillReverse(KodGames.ClientClass.Skill s1, KodGames.ClientClass.Skill s2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, s1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, s2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped1 - lineuped2;

		SkillConfig.Skill s1Cfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s1.ResourceId);
		SkillConfig.Skill s2Cfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s2.ResourceId);

		if (s1Cfg.qualityLevel != s2Cfg.qualityLevel)
			return s1Cfg.qualityLevel - s2Cfg.qualityLevel;
		else if (s1Cfg.sortIndex != s2Cfg.sortIndex)
			return s1Cfg.sortIndex - s2Cfg.sortIndex;
		else if (s1.LevelAttrib.Level != s2.LevelAttrib.Level)
			return s1.LevelAttrib.Level - s2.LevelAttrib.Level;

		return 0;
	}

	/// <summary>
	/// 1、 是否缘分
	/// 2、	已被装备的靠上
	/// 3、	品质越高越靠上
	/// 4、	同品质，优先级越高越靠上
	/// 5、	同品质同书籍时，根据书籍等级排序，等级越高越靠上
	/// 6、	同品质同书籍同等级时，随机排序。
	/// </summary>
	public static int CompareSkillForLineUp(KodGames.ClientClass.Skill s1, KodGames.ClientClass.Skill s2)
	{
		int assemble1 = s1.IsAssembleActive ? 1 : 0;
		int assemble2 = s2.IsAssembleActive ? 1 : 0;

		if (assemble1 != assemble2)
			return assemble2 - assemble1;

		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, s1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, s2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		var s1Cfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s1.ResourceId);
		var s2Cfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s2.ResourceId);

		if (s1Cfg.qualityLevel != s2Cfg.qualityLevel)
			return s2Cfg.qualityLevel - s1Cfg.qualityLevel;
		else if (s1Cfg.sortIndex != s2Cfg.sortIndex)
			return s2Cfg.sortIndex - s1Cfg.sortIndex;
		else if (s1.LevelAttrib.Level != s2.LevelAttrib.Level)
			return s2.LevelAttrib.Level - s1.LevelAttrib.Level;

		return 0;
	}

	public static int CompareDanForLineUp(KodGames.ClientClass.Dan d1, KodGames.ClientClass.Dan d2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, d1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, d2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;
		if (d1.BreakthoughtLevel != d2.BreakthoughtLevel)
			return d2.BreakthoughtLevel - d1.BreakthoughtLevel;

		if (d1.LevelAttrib.Level != d2.LevelAttrib.Level)
			return d2.LevelAttrib.Level - d1.LevelAttrib.Level;

		if (d1.CreateTime != d2.CreateTime)
			return (int)(d2.CreateTime - d1.CreateTime);

		return 0;
	}


	public static int CompareDanReverse(KodGames.ClientClass.Dan d1, KodGames.ClientClass.Dan d2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, d1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, d2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;
		if (d1.BreakthoughtLevel != d2.BreakthoughtLevel)
			return d1.BreakthoughtLevel - d2.BreakthoughtLevel;

		if (d1.LevelAttrib.Level != d2.LevelAttrib.Level)
			return d1.LevelAttrib.Level - d2.LevelAttrib.Level;

		if (d1.CreateTime != d2.CreateTime)
			return (int)(d1.CreateTime - d2.CreateTime);

		return 0;
	}

	public static int CompareSkill(SkillConfig.Skill s1, SkillConfig.Skill s2)
	{
		return s2.sortIndex - s1.sortIndex;
	}

	public static int CompareSkill(int s1, int s2)
	{
		SkillConfig.Skill c1 = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s1);
		SkillConfig.Skill c2 = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s2);
		return CompareSkill(c1, c2);
	}

	public static int CompareQuestData(KodGames.ClientClass.Quest q1, KodGames.ClientClass.Quest q2)
	{
		//任务排列顺序
		//*****先按照是否已经领取完奖励，领取完的在最下面
		//*****再按照是否已经完成任务而未领取奖励的在最上面
		//*****最后是没有完成正在进行中的任务
		//*****各类当中按照优先级顺序进行排列
		QuestConfig.Quest q1Config = ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(q1.QuestId);
		QuestConfig.Quest q2Config = ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(q2.QuestId);

		if (q1Config == null)
			Debug.LogError("questID " + q1.QuestId.ToString("X") + " is not Found in QuestConfig.");
		if (q2Config == null)
			Debug.LogError("questID " + q2.QuestId.ToString("X") + " is not Found in QuestConfig.");

		//int q1ShowTop = q1Config.notHideWhenFinished ? 0 : 1;
		//int q2ShowTop = q2Config.notHideWhenFinished ? 0 : 1;

		//int q1Priority = q1Config.totalStepCount

		int q1GoToReawrd = q1Config.rewards.Count > 0 && q1.Phase == QuestConfig._PhaseType.Finished ? 1 : 0;
		int q2GoToReward = q2Config.rewards.Count > 0 && q2.Phase == QuestConfig._PhaseType.Finished ? 1 : 0;

		int q1GoToUI = q1Config.gotoUI != _UIType.UnKonw && q1.Phase < QuestConfig._PhaseType.Finished ? 1 : 0;
		int q2GoToUI = q2Config.gotoUI != _UIType.UnKonw && q2.Phase < QuestConfig._PhaseType.Finished ? 1 : 0;

		//if (q1ShowTop != q2ShowTop)
		//    return q1ShowTop - q2ShowTop;
		/*else */
		if (q1GoToReawrd != q2GoToReward)
			return q2GoToReward - q1GoToReawrd;
		else
		{
			if (q1GoToUI != q2GoToUI)
				return q2GoToUI - q1GoToUI;
			//else if (q1.Phase != q2.Phase)
			//    return q1.Phase - q2.Phase;
			else if (q1Config.index != q2Config.index)
				return q2Config.index - q1Config.index;
			else
				return q1Config.questId - q2Config.questId;
		}
	}

	public static int CompareChageMessage(com.kodgames.corgi.protocol.ChatMessage msgX, com.kodgames.corgi.protocol.ChatMessage msgY)
	{
		if (msgX.messageType == msgY.messageType)
			return -1;

		if (msgX.messageType == _ChatType.System)
			return 1;

		if (msgX.messageType == _ChatType.Private)
		{
			if (msgY.messageType == _ChatType.System)
				return -1;
			else
				return 1;
		}

		if (msgX.messageType == _ChatType.World)
		{
			if (msgY.messageType == _ChatType.System || msgY.messageType == _ChatType.Private)
				return -1;
			else
				return 1;
		}

		if (msgX.messageType == _ChatType.FlowMessage)
			return -1;

		return 1;
	}

	public static int CompareGuildMember(KodGames.ClientClass.GuildMemberInfo m1, KodGames.ClientClass.GuildMemberInfo m2)
	{
		if (m1.PlayerId == m2.PlayerId)
			return 0;

		if (m1.PlayerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			return -1;
		else if (m2.PlayerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			return 1;

		int o1 = m1.Online ? 0 : 1;
		int o2 = m2.Online ? 0 : 1;

		if (o1 != o2)
			return o1 - o2;
		else
		{
			//if (m1.RoleId <= 0)
			//    m1.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

			//if (m2.RoleId <= 0)
			//    m2.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

			if (m1.RoleId != m2.RoleId)
			{
				var r1 = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(m1.RoleId);
				var r2 = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(m2.RoleId);

				return r1.SortIndex - r2.SortIndex;
			}
			else if (m1.TotalContribution != m2.TotalContribution)
				return m2.TotalContribution - m1.TotalContribution;
			else
			{
				if (m1.Online)
					return (int)m1.JoinTime - (int)m2.JoinTime;
				else
					return (int)m2.OfflineTime - (int)m1.OfflineTime;
			}
		}
	}

	public static int CompareGuildTransferMember(KodGames.ClientClass.GuildTransferMember m1, KodGames.ClientClass.GuildTransferMember m2)
	{
		//if (m1.RoleId <= 0)
		//    m1.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		//if (m2.RoleId <= 0)
		//    m2.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		if (m1.RoleId != m2.RoleId)
		{
			var r1 = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(m1.RoleId);
			var r2 = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(m2.RoleId);

			return r1.SortIndex - r2.SortIndex;
		}

		if (m1.TotalContribution != m2.TotalContribution)
			return m2.TotalContribution - m1.TotalContribution;
		else
			return (int)m1.JoinTime - (int)m2.JoinTime;
	}

	public static int CompaceApplyGuildRecord(KodGames.ClientClass.GuildRecord m1, KodGames.ClientClass.GuildRecord m2)
	{
		int isFull1 = m1.GuildMemberMax - m1.GuildMemberNum > 0 ? 1 : -1;
		int isFull2 = m2.GuildMemberMax - m2.GuildMemberNum > 0 ? 1 : -1;

		if (isFull1 != isFull2)
			return isFull2 - isFull1;
		else if (m1.GuildLevel != m2.GuildLevel)
			return m2.GuildLevel - m1.GuildLevel;
		else if (m1.ActiveValue != m2.ActiveValue)
			return m2.ActiveValue - m1.ActiveValue;
		else if (m1.GuildMemberNum != m2.GuildMemberNum)
			return m2.GuildMemberNum - m1.GuildMemberNum;
		else
			return m1.GuildId - m2.GuildId;
	}

	public static int CompaceRankGuildRecord(KodGames.ClientClass.GuildRankRecord m1, KodGames.ClientClass.GuildRankRecord m2)
	{
		if (m1.GuildLevel != m2.GuildLevel)
			return m2.GuildLevel - m1.GuildLevel;

		if (m1.GuildConstruct != m2.GuildConstruct)
			return m2.GuildConstruct - m1.GuildConstruct;
		else
			return (int)m1.GuildConstructTime - (int)m2.GuildConstructTime;
	}

	public static int CompaceGuildInfoRecord(KodGames.ClientClass.GuildMemberInfo m1, KodGames.ClientClass.GuildMemberInfo m2)
	{
		//if (m1.RoleId <= 0)
		//    m1.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		//if (m2.RoleId <= 0)
		//    m2.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		if (m1.RoleId != m2.RoleId)
		{
			var r1 = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(m1.RoleId);
			var r2 = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(m2.RoleId);

			return r1.SortIndex - r2.SortIndex;
		}
		else if (m1.TotalContribution != m2.TotalContribution)
			return m2.TotalContribution - m1.TotalContribution;
		else
			return (int)m1.JoinTime - (int)m2.JoinTime;
	}

}
