using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfCheckPoint : UIModule
{
	public UIScrollList thisPassRewardList;
	public GameObjectPool rewardTitlePool;
	public GameObjectPool rewardIconPool;
	public GameObjectPool rewardPassTitlePool;

	private int stage;
	private int maxStage;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas != null)
			stage = (int)userDatas[0];
		if (userDatas.Length > 1)
			maxStage = (int)userDatas[1];

		FillData();

		return true;
	}

	private void FillData()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	private void ClearData()
	{
		StopCoroutine("FillList");

		thisPassRewardList.ClearList(false);
		thisPassRewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		//var stageCfg = ConfigDatabase.DefaultCfg.WolfSmokeConfig.Stages[stage];
		var stageCfg = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageById(stage);

		//int iconCount = new UIElemWolfPointIcon().GetIconCount();
		int iconCount = 3;
		List<Reward> rewards = new List<Reward>();
		bool isFirst = false;

		int stageIndex = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetIndexByStageId(stage);
		if (stageIndex > ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetIndexByStageId(maxStage))
			isFirst = true;

		int row = 0;
		row = stageCfg.PassRewards.Count % iconCount == 0 ? stageCfg.PassRewards.Count / iconCount - 1 : stageCfg.PassRewards.Count / iconCount;

		UIListItemContainer uiContainerPassText = rewardPassTitlePool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemWolfPointPassReward titlePassItem = uiContainerPassText.GetComponent<UIElemWolfPointPassReward>();
		uiContainerPassText.data = titlePassItem;
		thisPassRewardList.AddItem(titlePassItem.gameObject);

		for (int index = 0; index <= row; index++)
		{

			UIListItemContainer uiContainer = rewardIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemWolfPointIcon passItem = uiContainer.GetComponent<UIElemWolfPointIcon>();
			uiContainer.data = passItem;

			for (int j = 0; j < iconCount && j + index * iconCount < stageCfg.PassRewards.Count; j++)
			{
				rewards.Add(stageCfg.PassRewards[index * iconCount + j]);
			}

			//Debug.Log("rewards.Count = " + rewards.Count);

			passItem.SetData(rewards);
			thisPassRewardList.AddItem(passItem.gameObject);

			rewards.Clear();
		}

		UIListItemContainer uiContainerText = rewardTitlePool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemWolfPointReward titleItem = uiContainerText.GetComponent<UIElemWolfPointReward>();
		uiContainerText.data = titleItem;
		titleItem.SetData(isFirst);
		thisPassRewardList.AddItem(titleItem.gameObject);

		for (int index = 0; index <= stageCfg.FirstPassRewards.Count / iconCount; index++)
		{
			UIListItemContainer uiContainer = rewardIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemWolfPointIcon firstItem = uiContainer.GetComponent<UIElemWolfPointIcon>();
			uiContainer.data = firstItem;

			for (int j = 0; j <= iconCount && j + index * iconCount < stageCfg.FirstPassRewards.Count; j++)
			{
				rewards.Add(stageCfg.FirstPassRewards[index * iconCount + j]);
			}

			firstItem.SetData(rewards);
			thisPassRewardList.AddItem(firstItem.gameObject);

			rewards.Clear();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSpecialRewardIcon(UIButton btn)
	{
		UIElemAssetIcon itemIcon = btn.Data as UIElemAssetIcon;
		ClientServerCommon.Reward item = itemIcon.Data as ClientServerCommon.Reward;
		GameUtility.ShowAssetInfoUI(item.id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}
}
