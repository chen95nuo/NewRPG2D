using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientServerCommon;
// 专门用来处理跳过战斗的round，会在玩家点击跳过的时候动态加到round队列里面
public class SkipBattleRound : BattleRound
{
	public SkipBattleRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		// Initialize actor list
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (role.Hide)
				continue;

			ActingRoles.Add(role);
		}

		return this;
	}

	public override bool CanStart()
	{
		if (!base.CanStart())
		{
			return false;
		}

		if (!BattleRecordPlayer.IsSkip)
		{
			return false;
		}

		return true;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		AvatarAction actionConfig = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Die, 0);
		if (actionConfig == null)
		{
			return false;
		}

		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			uiEnv.GetUIModule<UIPnlBattleRoleInfo>().SetCanShowRoleInfo(false);
			uiEnv.GetUIModule<UIPnlBattleRoleInfo>().Hide();
		}

		foreach (var role in ActingRoles)
		{
			int leftHP = BattleRecordPlayer.BattleRecord.GetAvatarResult(role.AvatarIndex).LeftHP;

			// role has already dead ,do not need process die action anymore
			if (role.AvatarHP <= 0)
			{
				continue;
			}

			role.AvatarHP = leftHP;
			role.BattleBar.UpdateHP();

			if (leftHP <= 0)
			{
				KodGames.ClientClass.ActionRecord actionRecord = new KodGames.ClientClass.ActionRecord();
				actionRecord.ActionId = actionConfig.id;
				actionRecord.SrcAvatarIndex = role.AvatarIndex;
				actionRecord.TargetAvatarIndices.Add(role.AvatarIndex);
				BattleRecordPlayer.PlayActionRecord(actionRecord);
			}
		}

		return true;
	}
}
