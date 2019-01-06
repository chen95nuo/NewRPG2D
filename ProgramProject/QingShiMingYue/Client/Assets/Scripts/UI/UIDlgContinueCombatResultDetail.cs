using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgContinueCombatResultDetail : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool rewardPool;
	private List<KodGames.ClientClass.CombatResultAndReward> combatResultAndRewards;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		ClearData();

		this.combatResultAndRewards = userDatas[0] as List<KodGames.ClientClass.CombatResultAndReward>;
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();

		if (!SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgCampaignContinueBattleResult)))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgCampaignContinueBattleResult, combatResultAndRewards);
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		if (combatResultAndRewards == null)
			yield break;

		for (int index = 0; index < combatResultAndRewards.Count; index++)
		{
			UIElemContinueCombatResultItem item = rewardPool.AllocateItem().GetComponent<UIElemContinueCombatResultItem>();
			item.SetData(index + 1, combatResultAndRewards[index].DungeonReward, combatResultAndRewards[index].DungeonReward_ExpSilver);

			scrollList.AddItem(item.container);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOk(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}