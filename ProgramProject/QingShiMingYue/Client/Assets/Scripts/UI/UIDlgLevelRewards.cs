using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgLevelRewards : UIModule
{

	//标题
	public SpriteText titleText;

	//list
	public UIScrollList levelRewardList;
	public GameObjectPool levelRewardsPool;

	private int LevelId;

	//控制每行有多少个Icon
	private const int C_COLUMN_COUNT = 3;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		Debug.Log("UIDlgLevelRewards");

		if (!base.OnShow(layer, userDatas))
			return false;


		Debug.Log((int)userDatas[0]);
		this.LevelId = (int)userDatas[0];

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
		//清理List
		StopCoroutine("FillRewardList");
		levelRewardList.ClearList(false);
		levelRewardList.ScrollPosition = 0f;
	}


	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		titleText.Text = ItemInfoUtility.GetAssetName(LevelId);

		var rewardCfg = ConfigDatabase.DefaultCfg.LevelRewardConfig.GetLevelRewardById(LevelId);
		int row = rewardCfg.rewards.Count % C_COLUMN_COUNT == 0 ? rewardCfg.rewards.Count / C_COLUMN_COUNT - 1 : rewardCfg.rewards.Count / C_COLUMN_COUNT;

		for (int index = 0; index <= row; index++)
		{
			List<Reward> rewards = null;
			if (index < row)
				rewards = rewardCfg.rewards.GetRange(index * C_COLUMN_COUNT, C_COLUMN_COUNT);
			else
				rewards = rewardCfg.rewards.GetRange(index * C_COLUMN_COUNT, Math.Min(rewardCfg.rewards.Count - index * C_COLUMN_COUNT, C_COLUMN_COUNT));

			UIElemLevelRewardItem item = levelRewardsPool.AllocateItem().GetComponent<UIElemLevelRewardItem>();
			item.SetData(rewards);
			levelRewardList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcom(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}
}
