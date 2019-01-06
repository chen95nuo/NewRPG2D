using UnityEngine;
using System.Collections.Generic;

// 服务器下发的暴走技能round
public class SkillCombatRound : CombatRound
{
	//private bool fadeoutStart = false;
	private float fadeoutTimer;

	//剧情战斗用
	//技能释放完毕后，取消屏幕遮罩，此时是否显示所有其他角色的血条，还是保持屏幕遮罩之前各个角色血条显示的状态
	private bool keepBattleBarHideStatus = false;
	//保存遮罩前角色的battleBar的显示状态
	private Dictionary<BattleRole, bool> battleBarHideStatusBeforeBattleDic = new Dictionary<BattleRole, bool>();

	public override string LogName
	{
		get
		{
			return string.Format("{0} teamIndex({1}) avatarIndex({2})",
				this.GetType(), RoundRecord.TeamIndex, RoundRecord.TurnRecords.Count != 0 ? RoundRecord.TurnRecords[0].AvatarIndex.ToString() : "null");
		}
	}

	public SkillCombatRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
		//保证获取到所有目标角色
		foreach (var turnRecord in RoundRecord.TurnRecords)
		{
			if (turnRecord.AvatarIndex < 0)
				continue;

			var turnRole = BattleRecordPlayer.BattleRoles[turnRecord.AvatarIndex];

			if (ActingRoles.Contains(turnRole) == false)
				ActingRoles.Add(turnRole);

			foreach (var actionRecord in turnRecord.ActionRecords)
			{
				foreach (var eventRecord in actionRecord.EventRecords)
				{
					foreach (var targetRecrod in eventRecord.EventTargetRecords)
					{
						var targetRole = BattleRecordPlayer.BattleRoles[targetRecrod.TargetIndex];

						if (ActingRoles.Contains(targetRole) == false)
							ActingRoles.Add(targetRole);
					}
				}
			}

			//剧情战斗会配置变量KeepBattleBarHideStatus
			if (!string.IsNullOrEmpty(roundRecord.Parameter("KeepBattleBarHideStatus")))
				this.keepBattleBarHideStatus = ClientServerCommon.StrParser.ParseBool(roundRecord.Parameter("KeepBattleBarHideStatus"), false);
		}

	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		BattleRecordPlayer.BattleScene.EnableSceneMask(true);

		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (ActingRoles.Contains(role))
			{
				role.Avatar.SetGameObjectLayer(GameDefines.SceneMaskLayer);
			}
			else
			{
				//记录遮罩前是否显示了血条
				if (keepBattleBarHideStatus)
					battleBarHideStatusBeforeBattleDic.Add(role, role.BattleBar.IsHide);

				role.BattleBar.Hide(true);
			}
		}

		return true;
	}

	public override void Finish()
	{
		BattleRecordPlayer.BattleScene.EnableSceneMask(false);

		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (ActingRoles.Contains(role))
			{
				role.Avatar.SetGameObjectLayer(GameDefines.DefaultLayer);
			}
			else
			{
				//恢复遮罩之前血条的显示状态
				if (keepBattleBarHideStatus && battleBarHideStatusBeforeBattleDic != null && battleBarHideStatusBeforeBattleDic.ContainsKey(role))
					role.BattleBar.Hide(battleBarHideStatusBeforeBattleDic[role]);
				else
					role.BattleBar.Hide(role.Hide);
			}
		}

		//内存释放
		if (battleBarHideStatusBeforeBattleDic != null)
		{
			battleBarHideStatusBeforeBattleDic.Clear();
			battleBarHideStatusBeforeBattleDic = null;
		}

		base.Finish();
	}

}