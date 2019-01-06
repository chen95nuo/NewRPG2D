using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgCampaignContinue : UIModule
{
	public UIButton continueCombatButton;
	public SpriteText continueCombatTimeLable;
	public SpriteText combatCostLabel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		InitView((int)userDatas[0]);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		continueCombatButton.Data = null;
	}

	private void InitView(int dungeonId)
	{
		var dungeonConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		var dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonConfig.ZoneId, dungeonConfig.dungeonId);

		// Set CombatButton Data.
		continueCombatButton.Data = dungeonConfig;

		// Set Continue Combat Times.
		int continueTimes = System.Math.Min(dungeonConfig.enterCount - dungeonRecord.TodayCompleteTimes, ConfigDatabase.DefaultCfg.GameConfig.continueCombat.maxContinueCombatCount);
		continueTimes = System.Math.Min(continueTimes, (int)(ItemInfoUtility.GetGameItemCount(dungeonConfig.enterCosts[0].id) / dungeonConfig.enterCosts[0].count));
		continueCombatTimeLable.Text = continueTimes.ToString();

		// Set Cost Label.
		combatCostLabel.Text = GameUtility.FormatUIString("UIPnlCampaign_ContinueCombat_Cost", ItemInfoUtility.GetAssetName(dungeonConfig.enterCosts[0].id), dungeonConfig.enterCosts[0].count * continueTimes);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickContinueCombat(UIButton btn)
	{
		var dungeonCfg = btn.Data as CampaignConfig.Dungeon;
		RequestMgr.Inst.Request(new ContinueCombatReq(dungeonCfg.ZoneId, dungeonCfg.dungeonId));
	}

	public void OnContinueSuccess(int dungeonId, List<KodGames.ClientClass.CombatResultAndReward> rewards)
	{
		// Refresh Bottom UI.
		SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.OnResponseRefreshDungeonInfo(dungeonId);

		// Show ContinueCombat Result.
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgContinueCombatResultDetail, rewards);

		// HideSelf.
		HideSelf();
	}
}
