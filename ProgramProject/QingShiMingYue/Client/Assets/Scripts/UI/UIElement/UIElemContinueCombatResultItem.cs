using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemContinueCombatResultItem : MonoBehaviour
{
	public AutoSpriteControlBase itemBg;
	public SpriteText combatTimesLabel;
	public List<SpriteText> fixRewardLabels;
	public SpriteText combatRewardLabel;

	public UIListItemContainer container;

	public void SetData(int combatTimes, KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward expSilverReward)
	{
		// Set Combat Times.
		combatTimesLabel.Text = GameUtility.FormatUIString("UIContinueCombat_Times", ItemInfoUtility.GetLevelCN(combatTimes));

		// Set FixRewards.
		var fixRewards = SysLocalDataBase.ConvertIdCountList(expSilverReward);
		foreach (var fixRewardLabel in fixRewardLabels)
			fixRewardLabel.Text = string.Empty;

		for (int index = 0; index < fixRewards.Count && index < fixRewardLabels.Count; index++)
			fixRewardLabels[index].Text = GameUtility.FormatUIString(
				"UIPnlCampaign_ContinueCombat_Reward", 
				GameDefines.txColorOrange.ToString(), 
				ItemInfoUtility.GetAssetName(fixRewards[index].first), 
				GameDefines.txColorWhite.ToString(), 
				fixRewards[index].second);

		// Set Reward Description.
		string rewardDesc = "";
		var rewardParis = SysLocalDataBase.ConvertIdCountList(reward);
		if (rewardParis.Count > 0)
		{
			rewardDesc = GameUtility.AppendString(rewardDesc, GameUtility.FormatUIString("UIContinueCombat_Reward_Label", GameDefines.txColorOrange.ToString(), GameDefines.txColorWhite.ToString()), false);

			foreach (var valuePair in rewardParis)
				rewardDesc = GameUtility.AppendString(rewardDesc, GameUtility.FormatUIString("UIContinueCombat_Reward_Count", ItemInfoUtility.GetAssetName(valuePair.first), valuePair.second), false);
		}

		combatRewardLabel.Text = rewardDesc;
		container.ScanChildren();
	}
}