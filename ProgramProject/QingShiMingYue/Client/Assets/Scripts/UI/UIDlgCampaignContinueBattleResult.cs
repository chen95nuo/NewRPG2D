using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgCampaignContinueBattleResult : UIModule
{
	public SpriteText continueCombatTimesLabel;
	public List<SpriteText> continueFixRewardLabels;

	public UIScrollList rewardScrollList;
	public GameObjectPool rewardItemPool;

	private List<KodGames.ClientClass.CombatResultAndReward> combatResultAndRewards;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		ClearData();

		// Set Data.
		combatResultAndRewards = userDatas[0] as List<KodGames.ClientClass.CombatResultAndReward>;

		InitView();

		return true;
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardScrollList.ClearList(false);
		rewardScrollList.ScrollPosition = 0f;

		combatResultAndRewards = null;
	}

	private void InitView()
	{
		// Set the Continue Combat Times.
		continueCombatTimesLabel.Text = GameUtility.FormatUIString("UIContinueCombat_Result_Times", combatResultAndRewards.Count);

		// Set FixReward Label.
		var fixRewards = GetRewardDic();
		foreach (var fixRewardLabel in continueFixRewardLabels)
			fixRewardLabel.Text = string.Empty;

		for (int index = 0; index < fixRewards.Count && index < continueFixRewardLabels.Count; index++)
			continueFixRewardLabels[index].Text = GameUtility.FormatUIString(
				"UIPnlCampaign_ContinueCombat_Reward",
				GameDefines.txColorOrange.ToString(),
				ItemInfoUtility.GetAssetName(fixRewards[index].first),
				GameDefines.txColorWhite.ToString(),
				fixRewards[index].second);

		// Set Reward List.
		StartCoroutine("FillRewardList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		foreach (var keyValue in GetRewardPariValues())
		{
			UIElemBattleResultDungeonItem item = rewardItemPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
			item.SetData(keyValue.first, keyValue.second);

			rewardScrollList.AddItem(item.gameObject);
		}
	}

	private List<KodGames.Pair<int, int>> GetRewardDic()
	{
		var rewards = new List<KodGames.Pair<int, int>>();

		foreach (var battle in combatResultAndRewards)
		{
			SysLocalDataBase.CombineReward(ref rewards, battle.DungeonReward_ExpSilver);
			SysLocalDataBase.CombineReward(ref rewards, battle.DungeonReward);
		}

		return rewards;
	}

	private List<KodGames.Pair<int, int>> GetRewardPariValues()
	{
		var rewards = new List<KodGames.Pair<int, int>>();

		foreach (var battle in combatResultAndRewards)
			SysLocalDataBase.CombineReward(ref rewards, battle.DungeonReward);

		rewards.Sort(
			(r1, r2) =>
			{
				int q1 = ItemInfoUtility.GetAssetQualityLevel(r1.first);
				int q2 = ItemInfoUtility.GetAssetQualityLevel(r2.first);

				if (q1 != q2)
					return q2 - q1;
				else
					return r1.first - r2.first;

			});

		return rewards;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetail(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgContinueCombatResultDetail, combatResultAndRewards);
	}

	//µã»÷Í¼±ê
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}
}
