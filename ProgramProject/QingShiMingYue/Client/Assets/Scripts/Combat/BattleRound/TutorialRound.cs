using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
// 纯客户端逻辑round，专门用来处理战斗中的新手引导
public class TutorialRound : BattleRound
{
	private bool isFinished = false;

	public TutorialRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	public override bool CanStartAfterRound()
	{
		return isFinished;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			int dialugeId = ConfigDatabase.DefaultCfg.DialogueConfig.tutorialBeforeCombat;
			uiEnv.GetUIModule<UITipAdviser>().ShowDialogue(dialugeId,
			() =>
			{
				isFinished = true;
			});
		}

		return true;
	}

	public override void Finish()
	{
		base.Finish();
		isFinished = true;
	}
}
