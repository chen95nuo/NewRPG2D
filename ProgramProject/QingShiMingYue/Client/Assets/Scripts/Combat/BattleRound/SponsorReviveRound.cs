using System;
using System.Collections.Generic;
using ClientServerCommon;

public class SponsorReviveRound : BattleRound
{
	public SponsorReviveRound(BattleRecordPlayer battleRecordPlayer)
		: base(battleRecordPlayer)
	{

	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		// Initialize actor list
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (role.TeamIndex != BattleRecordPlayer.SponsorTeamIndex)
				continue;

			// 死亡的角色不显示，千机楼战斗继承人物血量
			if (role.IsDead())
			{
				role.Hide = true;
				continue;
			}

			ActingRoles.Add(role);
		}

		return this;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		// Hide battle UI
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
			uiEnv.GetUIModule<UIPnlBattleBar>().HideAll();

		foreach (var role in ActingRoles)
		{
			role.BattleBar.gameObject.SetActive(true);
			role.gameObject.SetActive(true);
			//如果之前人物死亡会隐藏人物，显示出来
			role.Hide = false;
			// reset layer , must be call when the game object is active
			role.Avatar.SetGameObjectLayer(GameDefines.DefaultLayer);
			// set forward , must be call when the game object is active
			role.Avatar.CachedTransform.forward = BattleRecordPlayer.BattleScene.GetTeamForward(BattleRecordPlayer.BattleIndex, role.TeamIndex);

			//if (role.gameObject.activeInHierarchy == false)
			//{
			//    role.Hide = true;
			//    DoReviveAction(role);
			//}

			// Hide battle bar
			role.BattleBar.Hide(true);
		}

		return true;
	}

	//private void DoReviveAction(BattleRole role)
	//{
	//    AvatarAction actionCfg = role.GetActionByType(AvatarAction._Type.Revive);
	//    Debug.Log(actionCfg.id.ToString("X8"));
	//    Debug.Assert(actionCfg != null);

	//    // Play enter scene action
	//    KodGames.ClientClass.ActionRecord actionRecord = new KodGames.ClientClass.ActionRecord();
	//    actionRecord.ActionId = actionCfg.id;
	//    actionRecord.SrcAvatarIndex = role.AvatarIndex;
	//    actionRecord.TargetAvatarIndices.Add(role.AvatarIndex);		

	//    BattleRecordPlayer.PlayActionRecord(actionRecord);
	//}
}
