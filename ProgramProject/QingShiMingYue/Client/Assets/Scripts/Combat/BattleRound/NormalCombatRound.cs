using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
// 服务器下发的蓄力技能round，控制蓄力技能的释放。和暴走技能主要的区别就是暴走技能有相关的特效
public class NormalCombatRound : CombatRound
{
	//private float roundTime = 0;	

	public NormalCombatRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
		//delayTurnTime = ClientServerCommon.ConfigDatabase.DefaultCfg.GameConfig.combatSetting.normalAttackDelayTime;
		//delayTurnTimer = 0;

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
		}		
	}

	//public override bool CanStartAfterRound()
	//{
	//    if (base.CanStartAfterRound() == false)
	//    {
	//        return false;
	//    }

	//    if (RoundState == _RoundState.NotStarted)
	//        return false;

	//    //foreach (var turnRecord in RoundRecord.TurnRecords)
	//    //{
	//    //    BattleRole avatar = BattleRecordPlayer.BattleAvatars[turnRecord.AvatarIndex];

	//    //    if (avatar.IsMoving)
	//    //        return false;
	//    //}

	//    return roundTime >= 1f;
	//}

	//public override void Update()
	//{
	//    base.Update();

	//    if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
	//    {
	//        roundTime += Time.deltaTime;
	//    }
	//}
}
