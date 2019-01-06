using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgFriendCampaginChackPoint : UIModule
{

	public UIScrollList thisPassRewardList;
	public GameObjectPool rewardTitlePool;
	public GameObjectPool rewardIconPool;
	public GameObjectPool rewardPassTitlePool;

	private int C_COLUMN_COUNT = 3;

	private bool passpRet;
	private int passStageId;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		passpRet = false;
		passStageId = IDSeg.InvalidId;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		passpRet = (bool)userDatas[0];
		passStageId = (int)userDatas[1];
		passStageId = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetNextStageIdById(passStageId);

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardsList");
		thisPassRewardList.ClearList(false);
		thisPassRewardList.ScrollPosition = 0;

		passpRet = false;
		passStageId = IDSeg.InvalidId;
	}

	public void InitUI()
	{
		StartCoroutine("FillRewardsList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardsList()
	{
		yield return null;

		//通关奖励
		List<Reward> rewards = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageById(passStageId).PassRewards;
		int row = rewards.Count % C_COLUMN_COUNT == 0 ? rewards.Count / C_COLUMN_COUNT - 1 : rewards.Count / C_COLUMN_COUNT;
		List<Reward> oneRewards = null;

		UIListItemContainer uiContainerPassText = rewardPassTitlePool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemWolfPointPassReward titlePassItem = uiContainerPassText.GetComponent<UIElemWolfPointPassReward>();
		uiContainerPassText.data = titlePassItem;
		thisPassRewardList.AddItem(titlePassItem.gameObject);

		for (int index = 0; index <= row; index++)
		{
			if (index < row)
				oneRewards = rewards.GetRange(index * C_COLUMN_COUNT, C_COLUMN_COUNT);
			else
				oneRewards = rewards.GetRange(index * C_COLUMN_COUNT, Mathf.Min(rewards.Count - index * C_COLUMN_COUNT, C_COLUMN_COUNT));

			UIListItemContainer uiContainer = rewardIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemWolfPointIcon passItem = uiContainer.GetComponent<UIElemWolfPointIcon>();
			uiContainer.data = passItem;

			passItem.SetData(oneRewards);
			thisPassRewardList.AddItem(passItem.gameObject);

			oneRewards.Clear();
		}

		rewards = null;
		oneRewards = null;

		//首通奖励
		List<Reward> stRewards = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageById(passStageId).FirstPassRewards;
		int stRow = stRewards.Count % C_COLUMN_COUNT == 0 ? stRewards.Count / C_COLUMN_COUNT - 1 : stRewards.Count / C_COLUMN_COUNT;
		List<Reward> stOneRewards = null;

		UIListItemContainer uiContainerText = rewardTitlePool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemWolfPointReward titleItem = uiContainerText.GetComponent<UIElemWolfPointReward>();
		uiContainerText.data = titleItem;
		titleItem.SetData(!passpRet);
		thisPassRewardList.AddItem(titleItem.gameObject);

		for (int index = 0; index <= stRow; index++)
		{
			if (index < stRow)
				stOneRewards = stRewards.GetRange(index * C_COLUMN_COUNT, C_COLUMN_COUNT);
			else
				stOneRewards = stRewards.GetRange(index * C_COLUMN_COUNT, Mathf.Min(stRewards.Count - index * C_COLUMN_COUNT, C_COLUMN_COUNT));

			UIListItemContainer uiContainer = rewardIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemWolfPointIcon firstItem = uiContainer.GetComponent<UIElemWolfPointIcon>();
			uiContainer.data = firstItem;

			firstItem.SetData(stOneRewards);
			thisPassRewardList.AddItem(firstItem.gameObject);

			stOneRewards.Clear();
		}

		stRewards = null;
		stOneRewards = null;
	}

	//关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSpecialRewardIcon(UIButton btn)
	{
		UIElemAssetIcon itemIcon = btn.Data as UIElemAssetIcon;
		ClientServerCommon.Reward item = itemIcon.Data as ClientServerCommon.Reward;
		GameUtility.ShowAssetInfoUI(item.id, _UILayer.Top);
	}
}
