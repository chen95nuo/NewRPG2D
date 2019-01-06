#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Security;

namespace ClientServerCommon
{
	public class CombatDataConfig
	{
		public KodGames.ClientClass.CombatResultAndReward combatResultAndReward = new KodGames.ClientClass.CombatResultAndReward();

		public void LoadFromXml(SecurityElement element)
		{
			List<KodGames.ClientClass.BattleRecord> battleRecords = new List<KodGames.ClientClass.BattleRecord>();
			combatResultAndReward.BattleRecords = battleRecords;

			if (element.Tag != "CombatDataConfig")
				return;

			combatResultAndReward.CombatNumMax = StrParser.ParseDecIntEx(element.Attribute("CombatNumMax"), 1);

			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					switch (subElem.Tag)
					{
						case "BattleRecord":
							battleRecords.Add(LoadBattleRecordFromXml(subElem));
							break;
					}
				}
			}
		}

		private KodGames.ClientClass.BattleRecord LoadBattleRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.BattleRecord battleRecord = new KodGames.ClientClass.BattleRecord();
			battleRecord.SceneId = StrParser.ParseHexInt(element.Attribute("SceneId"), 0);
			battleRecord.MaxRecordCount = StrParser.ParseDecIntEx(element.Attribute("MaxRecordCount"), 0);

			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					switch (subElem.Tag)
					{
						case "RoundRecord":
							battleRecord.MatchinRoundRecord = LoadRoundRecordFromXml(subElem);
							break;

						case "TeamRecord":
							battleRecord.TeamRecords.Add(LoadTeamRecordFromXml(subElem));
							break;

						case "CombatRecord":
							battleRecord.CombatRecord = LoadCombatRecordFromXml(subElem);
							break;
					}
				}
				battleRecord.TeamRecords.Sort((m, n) =>
				{
					return m.TeamIndex - n.TeamIndex;
				});
			}

			return battleRecord;
		}

		private KodGames.ClientClass.RoundRecord LoadRoundRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.RoundRecord roundRecord = new KodGames.ClientClass.RoundRecord();
			roundRecord.RoundType = _CombatRoundType.Parse(element.Attribute("RoundType"), _CombatRoundType.Unknown);
			roundRecord.TeamIndex = StrParser.ParseDecIntEx(element.Attribute("TeamIndex"), 0);
			roundRecord.RowIndex = StrParser.ParseDecIntEx(element.Attribute("RowIndex"), 0);
			roundRecord.RoundIndex = StrParser.ParseDecIntEx(element.Attribute("RoundIndex"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "TurnRecord":
							roundRecord.TurnRecords.Add(LoadTurnRecordFromXml(subElement));
							break;

						case "RoundParameters":
							roundRecord.configParameterDic = LoadConfigParametersFromXml(subElement);
							break;
					}
				}
			}

			return roundRecord;
		}

		private Dictionary<string, string> LoadConfigParametersFromXml(SecurityElement element)
		{
			Dictionary<string, string> res = new Dictionary<string, string>();
			foreach (SecurityElement subElement in element.Children)
			{
				switch (subElement.Tag)
				{
					case "RoundParameter":
						KodGames.ClientClass.ConfigParameter parameter = LoadConfigParameterFromXml(subElement);
						if (string.IsNullOrEmpty(parameter.name))
						{
							//Debug.LogError("Configuration err: ConfigParameter name is Empty");
							continue;
						}

						if (res.ContainsKey(parameter.name))
						{
							//Debug.LogError("Configuration err: ConfigParameter name is repeated");
							continue;
						}

						res.Add(parameter.name, parameter.value);
						break;
				}
			}
			return res;
		}

		private KodGames.ClientClass.ConfigParameter LoadConfigParameterFromXml(SecurityElement element)
		{
			KodGames.ClientClass.ConfigParameter parameter = new KodGames.ClientClass.ConfigParameter();
			parameter.name = element.Attribute("Name");
			parameter.value = element.Attribute("Value");
			return parameter;
		}

		private KodGames.ClientClass.TurnRecord LoadTurnRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.TurnRecord turnRecord = new KodGames.ClientClass.TurnRecord();
			turnRecord.AvatarIndex = StrParser.ParseDecIntEx(element.Attribute("AvatarIndex"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "ActionRecord":
							turnRecord.ActionRecords.Add(LoadActionRecordFromXml(subElement));
							break;
					}
				}
			}

			return turnRecord;
		}

		private KodGames.ClientClass.ActionRecord LoadActionRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.ActionRecord actionRecord = new KodGames.ClientClass.ActionRecord();
			actionRecord.ActionId = StrParser.ParseHexInt(element.Attribute("ActionId"), 0);
			actionRecord.SrcAvatarIndex = StrParser.ParseDecIntEx(element.Attribute("SrcAvatarIndex"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "TargetAvatarIndeices":
							actionRecord.TargetAvatarIndices.Add(StrParser.ParseDecIntEx(subElement.Text, 0));
							break;

						case "EventRecord":
							actionRecord.EventRecords.Add(LoadEventRecordFromXml(subElement));
							break;

					}
				}
			}

			return actionRecord;
		}

		private KodGames.ClientClass.EventRecord LoadEventRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.EventRecord eventRecord = new KodGames.ClientClass.EventRecord();
			eventRecord.EventIndex = StrParser.ParseDecIntEx(element.Attribute("EventIdx"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "EventTargetRecord":
							eventRecord.EventTargetRecords.Add(LoadEventTargetRecordFromXml(subElement));
							break;
					}
				}
			}
			return eventRecord;
		}

		private KodGames.ClientClass.EventTargetRecord LoadEventTargetRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.EventTargetRecord eventTargetRecord = new KodGames.ClientClass.EventTargetRecord();
			eventTargetRecord.TargetIndex = StrParser.ParseDecIntEx(element.Attribute("TargetIndex"), 0);
			eventTargetRecord.EventType = AvatarAction.Event._Type.Parse(element.Attribute("EventType"), AvatarAction.Event._Type.Unknown);
			eventTargetRecord.TestType = CombatTurn._TestType.Parse(element.Attribute("TestType"), CombatTurn._TestType.None);
			eventTargetRecord.Value = StrParser.ParseDecIntEx(element.Attribute("Value"), 0);
			eventTargetRecord.Value1 = StrParser.ParseDecIntEx(element.Attribute("Value1"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "ActionRecord":
							eventTargetRecord.PassiveActionRecords.Add(LoadActionRecordFromXml(subElement));
							break;
					}
				}
			}
			return eventTargetRecord;
		}

		private KodGames.ClientClass.TeamRecord LoadTeamRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.TeamRecord teamRecord = new KodGames.ClientClass.TeamRecord();
			teamRecord.TeamIndex = StrParser.ParseDecIntEx(element.Attribute("TeamIndex"), 0);
			teamRecord.IsWinner = StrParser.ParseBool(element.Attribute("IsWinner"), false);
			teamRecord.DisplayName = StrParser.ParseStr(element.Attribute("DisplayName"), "");
			teamRecord.Evaluation = StrParser.ParseDecIntEx(element.Attribute("Evaluation"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "AvatarResult":
							teamRecord.AvatarResults.Add(LoadAvatarResultFromXml(subElement));
							break;
					}
				}
			}
			return teamRecord;
		}

		private KodGames.ClientClass.AvatarResult LoadAvatarResultFromXml(SecurityElement element)
		{
			KodGames.ClientClass.AvatarResult avatarResult = new KodGames.ClientClass.AvatarResult();
			avatarResult.AvatarIndex = StrParser.ParseDecIntEx(element.Attribute("AvatarIndex"), 0);
			avatarResult.TeamIndex = StrParser.ParseDecIntEx(element.Attribute("TeamIndex"), 0);
			avatarResult.MaxHP = StrParser.ParseDecIntEx(element.Attribute("MaxHP"), 0);
			avatarResult.LeftHP = StrParser.ParseDecIntEx(element.Attribute("LeftHP"), 0);
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "ActionRecord":
							avatarResult.EndAction = LoadActionRecordFromXml(subElement);
							break;
						case "CombatAvatarData":
							avatarResult.CombatAvatarData = LoadCombatAvatarDataFromXml(subElement);
							break;
					}
				}
			}
			return avatarResult;
		}

		private KodGames.ClientClass.CombatAvatarData LoadCombatAvatarDataFromXml(SecurityElement element)
		{
			KodGames.ClientClass.CombatAvatarData combatAvatarData = new KodGames.ClientClass.CombatAvatarData();
			combatAvatarData.AvatarType = _AvatarType.Parse(element.Attribute("AvatarType"), _AvatarType.Unknown);
			combatAvatarData.DisplayName = StrParser.ParseStr(element.Attribute("DisplayName"), "");
			combatAvatarData.ResourceId = StrParser.ParseHexInt(element.Attribute("ResourceId"), 0);
			combatAvatarData.BattlePosition = StrParser.ParseDecIntEx(element.Attribute("BattlePosition"), 0);
			combatAvatarData.Evaluation = StrParser.ParseDecIntEx(element.Attribute("Evaluation"), 0);
			combatAvatarData.Scale = StrParser.ParseFloat(element.Attribute("Scale"), 1);
			combatAvatarData.NpcId = StrParser.ParseHexInt(element.Attribute("NpcId"), 0);
			combatAvatarData.NpcType = _NpcType.Parse(element.Attribute("NpcType"), _NpcType.Normal);
			combatAvatarData.BreakThrough = StrParser.ParseDecIntEx(element.Attribute("BreakThrough"), 0);

			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "LevelAttrib":
							combatAvatarData.LevelAttrib = LoadLevelAttribFromXml(subElement);
							break;

						case "EquipmentData":
							combatAvatarData.Equipments.Add(LoadEquipmentDataFromXml(subElement));
							break;

						case "Attribute":
							combatAvatarData.Attributes.Add(LoadAttributeFromXml(subElement));
							break;

						case "SkillData":
							combatAvatarData.Skills.Add(LoadSkillDataFromXml(subElement));
							break;

						// 						case "ModifierSet":
						// 							{
						// 								combatAvatarData..Add(LoadModifierSetFromXml(subElement));
						// 							}
						// 							break;
						case "BuffData":
							combatAvatarData.Buffs.Add(LoadBuffDataFromXml(subElement));
							break;
					}
				}
			}
			return combatAvatarData;
		}

		private KodGames.ClientClass.LevelAttrib LoadLevelAttribFromXml(SecurityElement element)
		{
			KodGames.ClientClass.LevelAttrib levelAttrib = new KodGames.ClientClass.LevelAttrib();
			levelAttrib.Level = StrParser.ParseDecIntEx(element.Attribute("Level"), 0);
			levelAttrib.Experience = StrParser.ParseDecIntEx(element.Attribute("Experience"), 0);
			return levelAttrib;
		}

		private KodGames.ClientClass.EquipmentData LoadEquipmentDataFromXml(SecurityElement element)
		{
			KodGames.ClientClass.EquipmentData equipmentData = new KodGames.ClientClass.EquipmentData();
			equipmentData.Id = StrParser.ParseHexInt(element.Attribute("Id"), 0);
			equipmentData.BreakThrough = StrParser.ParseDecIntEx(element.Attribute("BreakThrough"), 0);
			return equipmentData;
		}
		
		private KodGames.ClientClass.Attribute LoadAttributeFromXml(SecurityElement element)
		{
			KodGames.ClientClass.Attribute attribute = new KodGames.ClientClass.Attribute();
			attribute.Type = _AvatarAttributeType.Parse(element.Attribute("Type"), _AvatarAttributeType.Unknown);
			attribute.Value = StrParser.ParseDouble(element.Attribute("Value"), 0);
			return attribute;
		}
		
		private KodGames.ClientClass.SkillData LoadSkillDataFromXml(SecurityElement element)
		{
			KodGames.ClientClass.SkillData skillData = new KodGames.ClientClass.SkillData();
			skillData.Id = StrParser.ParseHexInt(element.Attribute("Id"), 0);
			skillData.Level = StrParser.ParseDecIntEx(element.Attribute("Level"), 0);
			return skillData;
		}
		
		private KodGames.ClientClass.BuffData LoadBuffDataFromXml(SecurityElement element)
		{
			KodGames.ClientClass.BuffData buffData = new KodGames.ClientClass.BuffData();
			buffData.InstId = StrParser.ParseHexInt(element.Attribute("InstId"), 0);
			buffData.BuffId = StrParser.ParseHexInt(element.Attribute("BuffId"), 0);
			return buffData;
		}

		private KodGames.ClientClass.CombatRecord LoadCombatRecordFromXml(SecurityElement element)
		{
			KodGames.ClientClass.CombatRecord combatRecord = new KodGames.ClientClass.CombatRecord();
			if (element.Children != null)
			{
				foreach (SecurityElement subElement in element.Children)
				{
					switch (subElement.Tag)
					{
						case "RoundRecord":
							combatRecord.RoundRecords.Add(LoadRoundRecordFromXml(subElement));
							break;

					}
				}
			}
			return combatRecord;
		}
	}
}
#endif