using System;
using System.Collections.Generic;
using System.Collections;
using ClientServerCommon;
using UnityEngine;

public class UIPnlStartServerReward : UIModule
{
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public SpriteText titleText;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		ClearData();
		titleText.Text = GameUtility.GetUIString("UIDlgStartServerReawrd_Title");
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		List<StartServerRewardConfig.StartServerReward> startServerRewards = ConfigDatabase.DefaultCfg.StartServerRewardConfig.startServerRewards;

		if (startServerRewards == null || startServerRewards.Count <= 0)
			Debug.LogError("StartServerConfig not valid data.");

		// Sort datas by  Day.
		startServerRewards.Sort(
			(r1, r2) =>
			{
				return r1.day - r2.day;
			});

		// Fill Data.
		foreach (var rewardData in startServerRewards)
		{
			UIElemStartServerReawrdItem item = rewardPool.AllocateItem().GetComponent<UIElemStartServerReawrdItem>();
			item.SetData(rewardData);

			rewardList.AddItem(item.container);

		}
	}

	private UIElemStartServerReawrdItem GetItemByPickID(int pickID)
	{
		for (int index = 0; index < rewardList.Count; index++)
			if ((rewardList.GetItem(index).Data as UIElemStartServerReawrdItem).PickID == pickID)
				return rewardList.GetItem(index).Data as UIElemStartServerReawrdItem;

		return null;
	}

	/// <summary>
	///  领取成功的回调
	/// </summary>
	public void OnPickRewardSuccess(int pickId, KodGames.ClientClass.Reward reward)
	{
		UIElemStartServerReawrdItem item = GetItemByPickID(pickId);

		if (item != null)
			item.RefreshControllView();

		List<KodGames.Pair<int, int>> rewardColls = SysLocalDataBase.ConvertIdCountList(reward);
		string rewardDesc = string.Format("{0}{1}", GameDefines.cardColorChenSe.ToString(), GameUtility.GetUIString("UI_Reward") + "\n\n");

		for (int index = 0; index < rewardColls.Count; index++)
		{
			rewardDesc += string.Format("{0}{1}{2}x{3}\n", GameDefines.textColorBtnYellow.ToString(), ItemInfoUtility.GetAssetName(rewardColls[index].first),
																					GameDefines.textColorWhite.ToString(), rewardColls[index].second);
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI((int)assetIcon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPickReawrd(UIButton btn)
	{
		RequestMgr.Inst.Request(new PickStartServerRewardReq((int)btn.Data));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
		//SysUIEnv.Instance.GetUIModule<UIPnlCentralCityPlayerInfo>().SetStartServerRewardParticle();
	}
}