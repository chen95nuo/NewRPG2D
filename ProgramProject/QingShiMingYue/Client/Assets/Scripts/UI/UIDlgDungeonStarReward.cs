using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDungeonStarReward : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool rewardPool;

	public AutoSpriteControlBase pickRewardButton;
	public AutoSpriteControlBase pickedBox;

	private int zoneId;
	private int dungeonDifficult;
	private int boxIndex;

	private const int C_COLUMN_COUNT = 3;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.zoneId = (int)userDatas[0];
		this.dungeonDifficult = (int)userDatas[1];
		this.boxIndex = (int)userDatas[2];

		StartCoroutine("FillRewardList");

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		// Clear List.
		StopCoroutine("FillRewardList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;

		// Clear Data.
		zoneId = IDSeg.InvalidId;
		dungeonDifficult = -1;
		boxIndex = -1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;


		var zoneCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(this.zoneId);
		if (zoneCfg == null)
		{
			Debug.LogError(string.Format("ZoneId : {0} is inValid.", zoneId.ToString("X")));
			yield break;
		}

		var zoneDifficult = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(this.zoneId).GetDungeonDifficultyByDifficulty(this.dungeonDifficult);
		if (zoneDifficult == null)
		{
			Debug.LogError(string.Format("Difficult not found in zoneId: {0}", zoneId.ToString()));
			yield break;
		}

		int startRewardId = zoneDifficult.starRewardConditions[boxIndex].starRewardId;
		var rewardCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetStarRewardById(startRewardId);
		if (rewardCfg == null)
		{
			Debug.LogError(string.Format("Difficult not found in zoneId: {0}", zoneId.ToString()));
			yield break;
		}

		var difficultRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(this.zoneId).GetDungeonDiffcultyByDiffcultyType(this.dungeonDifficult);

		int sumStarCount = 0;
		if (difficultRecord != null && difficultRecord.Dungeons != null)
		{
			for (int index = 0; index < difficultRecord.Dungeons.Count; index++)
				sumStarCount += difficultRecord.Dungeons[index].BestRecord;
		}

		// Set Operator State.
		pickRewardButton.Hide(difficultRecord != null && difficultRecord.BoxPickedIndexs.Contains(boxIndex));
		pickRewardButton.controlIsEnabled = sumStarCount >= zoneDifficult.starRewardConditions[boxIndex].requireStarCount;
		pickedBox.Hide(!pickRewardButton.IsHidden());

		int row = rewardCfg.rewards.Count % C_COLUMN_COUNT == 0 ? rewardCfg.rewards.Count / C_COLUMN_COUNT - 1 : rewardCfg.rewards.Count / C_COLUMN_COUNT;
		for (int index = 0; index <= row; index++)
		{
			List<Reward> rewards = null;
			if (index < row)
				rewards = rewardCfg.rewards.GetRange(index * C_COLUMN_COUNT, C_COLUMN_COUNT);
			else
				rewards = rewardCfg.rewards.GetRange(index * C_COLUMN_COUNT, Math.Min(rewardCfg.rewards.Count - index * C_COLUMN_COUNT, C_COLUMN_COUNT));

			UIElemDungeonStarReward starRewardItem = rewardPool.AllocateItem().GetComponent<UIElemDungeonStarReward>();
			starRewardItem.SetData(rewards);

			scrollList.AddItem(starRewardItem.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFixReward(UIButton btn)
	{
		RequestMgr.Inst.Request(new GetDungeonRewardReq(this.zoneId, this.dungeonDifficult, this.boxIndex));
	}

	public void OnResponseFixRewardSuccess()
	{
		// Reset UI.
		pickRewardButton.Hide(true);
		pickedBox.Hide(false);

		SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.OnResponseGetDungeonRewardSuccess();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcom(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId);
	}
}