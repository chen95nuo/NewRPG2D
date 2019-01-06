using System;
using ClientServerCommon;
using System.Collections.Generic;
using KodGames.ClientClass;
using System.Text;

//千机楼战斗战斗接管面板显示战斗具体数据使用。用于获取每一波战斗中avatar的剩余血量
public class BattleDataAnalyser
{
	//Avatar的所有属性都可以从CombatAvatarData中获取，要注意取整。


	private CombatResultAndReward battleData;

	public int BattleCount
	{
		get
		{
			return battleData.BattleRecords.Count;
		}
	}

	public BattleDataAnalyser(CombatResultAndReward battleData)
	{
		this.battleData = battleData;
	}

	public int GetAvatarLeftHP(int avatarIdx, int battleIdx)
	{
		if (!CheckBattleIdx(battleIdx))
			return 0;

		return (int)battleData.BattleRecords[battleIdx].GetAvatarResult(avatarIdx).LeftHP;
	}

	public int GetAvatarLeftMaxHP(int avatarIdx, int battleIdx)
	{
		if (!CheckBattleIdx(battleIdx))
			return 1;

		//计算精度问题，强制取整，防止在满血状态下LeftHP!=MaxHP，造成血量比值有问题，显示结果为血量未满（99%）
		//return battleData.BattleRecords[battleIdx].GetAvatarResult(avatarIdx).CombatAvatarData.GetAttributeByType(ClientServerCommon._AvatarAttributeType.MaxHP).Value;
		int maxHp = battleData.BattleRecords[battleIdx].GetAvatarResult(avatarIdx).MaxHP;
		if (maxHp == 0)
		{
			Debug.LogError("MaxHP=0");
			maxHp = 1;
		}

		return maxHp;
	}

	public List<int> GetAvatarIdxes(int teamIdx, int battleIdx)
	{
		List<int> res = new List<int>();

		if (!CheckBattleIdx(battleIdx))
			return res;

		var rcd = battleData.BattleRecords[battleIdx];

		foreach (var teamRcd in rcd.TeamRecords)
		{
			if (teamRcd.TeamIndex == teamIdx)
			{
				foreach (var avatarRes in teamRcd.AvatarResults)
				{
					if (!res.Contains(avatarRes.AvatarIndex))
						res.Add(avatarRes.AvatarIndex);
				}
			}
		}

		return res;
	}

	public int GetAvatarResourceIDByIndex(int avatarIdx, int battleIdx)
	{
		if (!CheckBattleIdx(battleIdx))
			return 0;

		return battleData.BattleRecords[battleIdx].GetAvatarResult(avatarIdx).CombatAvatarData.ResourceId;
	}

	bool CheckBattleIdx(int battleIdx)
	{
		if (battleData.BattleRecords.Count <= battleIdx)
		{
			Debug.LogError("battleIdx Out of range");
			return false;
		}

		return true;
	}

	public void LogFirstBattleAllAvatarAttributes()
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < battleData.BattleRecords[0].TeamRecords.Count; i++)
		{
			var team = battleData.BattleRecords[0].TeamRecords[i];
			sb.Append(string.Format("Team [{0}]\n", i));

			foreach (var avatar in team.AvatarResults)
			{
				sb.Append(string.Format("\tAvatar {0} \n", avatar.CombatAvatarData.DisplayName));
				foreach (var attribute in avatar.CombatAvatarData.Attributes)
				{
					sb.Append(string.Format("\t\t [{0}][{1}]:\t\t{2}\n", ClientServerCommon._AvatarAttributeType.GetNameByType(attribute.Type), ClientServerCommon._AvatarAttributeType.GetDisplayNameByType(attribute.Type, ConfigDatabase.DefaultCfg), attribute.Value));
				}
			}
		}
		Debug.Log(sb.ToString());
	}
}
