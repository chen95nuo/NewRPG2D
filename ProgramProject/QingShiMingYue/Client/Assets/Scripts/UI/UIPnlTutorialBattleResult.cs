using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTutorialBattleResult : UIModule
{
	public class TutorialBattleResultData : UIPnlBattleResultBase.BattleResultData
	{
		public TutorialBattleResultData()
			: base(_UIType.UIPnlTutorialBattleResult)
		{
			this.CombatType = _CombatType.Tutorial;
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;


		int dialogueId = ConfigDatabase.DefaultCfg.DialogueConfig.tutorialAfterCombat;

		SysUIEnv.Instance.GetUIModule<UITipAdviser>().ShowDialogue(dialogueId,
			() =>
			{
				HideSelf();
			});

		return true;
	}
}
