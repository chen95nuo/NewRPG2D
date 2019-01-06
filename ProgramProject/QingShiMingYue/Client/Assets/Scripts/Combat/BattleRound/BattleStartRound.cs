using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 战斗开始round，纯客户端逻辑round，与服务器无关
// 控制战斗ui的显示与隐藏
public class BattleStartRound : BattleRound
{
	private float roundTime = 0;

	public BattleStartRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	//手动配置战斗使用
	public BattleStartRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
	}

	public override bool CanStartAfterRound()
	{
		if (roundTime >= 1f)
		{
			return base.CanStartAfterRound();
		}
		else
		{ 	
			return false;
		}
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		// Show battle start UI
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			uiEnv.GetUIModule<UIPnlBattleBar>().HideBattleStart(false);

			uiEnv.GetUIModule<UIPnlBattleBar>().UpdateLevelText(SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().CombatResultAndReward.BattleRecords.Count);
		}

		return true;
	}

	public override void Finish()
	{
		// Show battle bar
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			UIPnlBattleBar battleBar = uiEnv.GetUIModule<UIPnlBattleBar>();
			battleBar.HideAll();
			if (!battleRecordPlayer.IsTutorial)
			{
				battleBar.HideTopBar(false);
				battleBar.HideBottomBar(false);
				battleBar.ShowSpeedupButton();
				battleBar.HideResetCameraButton(false);
				battleBar.HideRound(false);
				BattleRecordPlayer.CanShowSkip = true;
				battleBar.SetTopBarData(BattleRecordPlayer.BattleRecord);
				/*
				 * bug记录.
				 * 遗留问题：首次进入场景skillCameraAnimationButton阻挡UI输入，在本Round结束前不能拖动场景
				 * 回放战斗时skillCameraAnimationButton已经hide，不会再次阻挡UI输入，此前虽然战斗相机ExternalControl为true但是其注册
				 * 的MouseTouchPtrListener检测函数会接受UI输入，在后台改变相机旋转，导致ExternalControl为false的一瞬间相机突然改变位置。
				 * 
				 * 正确的方式是应该使用LockTouch来阻止用户输入
				 */
				battleBar.skillCameraAnimationButton.gameObject.SetActive(false);
				uiEnv.GetUIModule<UIPnlBattleRoleInfo>().SetCanShowRoleInfo(true);
			}
		}

		if (roundRecord == null || roundRecord.Parameter("ShowAllBattleBar") == "True")
		{
			// Show avatar battle bar
			foreach (var role in BattleRecordPlayer.BattleRoles)
			{
				if (role.IsDead())
					role.BattleBar.Hide(true);
				else
					role.BattleBar.Hide(false);
			}
		}

		base.Finish();
	}

	public override void Update()
	{
		base.Update();

		if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
		{
			roundTime += Time.deltaTime;
		}
	}
}
