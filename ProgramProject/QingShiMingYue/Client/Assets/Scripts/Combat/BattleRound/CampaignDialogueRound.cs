using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
// 处理副本中对话的round，纯客户端逻辑round，与服务器无关
public class CampaignDialogueRound : BattleRound
{
	private bool isFinished = false;
	private int stageDialogueType = _StateDialogueType.Unkonw;
	private float originalSpeed = 1.0f;

	public CampaignDialogueRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound, int stageDialogueType)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
		this.stageDialogueType = stageDialogueType;
		originalSpeed = battleRecordPlayer.GetScaleTime();
	}

	public override bool CanStartAfterRound()
	{
		return isFinished;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		originalSpeed = battleRecordPlayer.GetScaleTime();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;

		if (battleState.BattleResultData is UIPnlCampaignBattleResult.CampaignBattleResultData)
		{
			UIPnlCampaignBattleResult.CampaignBattleResultData battleResultData = battleState.BattleResultData as UIPnlCampaignBattleResult.CampaignBattleResultData;
			CampaignConfig.StageDialogue stageDialogue = CampaignData.GetDungeonDialogue(battleResultData.ZoneId, battleResultData.DungeonId, battleRecordPlayer.BattleIndex, stageDialogueType);

			if (stageDialogue == null)
			{
				isFinished = true;
			}
			else
			{
				battleRecordPlayer.ResetScaleTime();

				SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
				uiEnv.GetUIModule<UITipAdviser>().ShowDialogue(stageDialogue.dialogueId,
					() =>
					{
						CampaignData.SetDungeonDialogueState(battleResultData.ZoneId, battleResultData.DungeonId, battleRecordPlayer.BattleIndex, stageDialogueType);
						isFinished = true;
					});
			}
		}
		else
			isFinished = true;

		return true;
	}

	public override void Finish()
	{
		base.Finish();
		isFinished = true;
		battleRecordPlayer.ScaleTime(originalSpeed);
	}
}
