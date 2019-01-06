using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendCampaginWeekReward : UIModule
{
	public SpriteText weekRewardLabel;
	public UIButton getRewardBtn;

	public SpriteText playerFriendship;
	public SpriteText playerFriendShipNot;

	public UIScrollList rewardList;
	public GameObjectPool rewardRootPool;

	private bool waitRequest;
	private long netResetTime;
	private float deltaTime;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		getRewardBtn.Hide(true);

		QueryRewardsList();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	public void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardList.ClearList(false);
		rewardList.ScrollListTo(0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList(List<com.kodgames.corgi.protocol.FCRewardInfo> rewardInfos)
	{
		yield return null;

		for (int index = 0; index < rewardInfos.Count; index++)
		{
			UIElemFriendCampaginWeekRewardRootItem item = rewardRootPool.AllocateItem(false).GetComponent<UIElemFriendCampaginWeekRewardRootItem>();
			item.SetData(rewardInfos[index]);

			rewardList.AddItem(item);
		}
	}

	private void Update()
	{
		if (!waitRequest)
		{
			deltaTime += Time.deltaTime;
			if (deltaTime >= 1.0f)
			{
				deltaTime = 0;
				if (netResetTime - SysLocalDataBase.Inst.LoginInfo.NowTime < 0)
					QueryRewardsList();
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickRewardIcon(UIButton btn)
	{
		UIElemAssetIcon item = btn.Data as UIElemAssetIcon;
		Reward reward = item.Data as Reward;
		GameUtility.ShowAssetInfoUI(reward);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickGetRewards(UIButton btn)
	{
		RequestMgr.Inst.Request(new FCRankGetRewardReq());
	}

	private void QueryRewardsList()
	{
		this.waitRequest = true;
		RequestMgr.Inst.Request(new QueryFCRankRewarReq());
	}

	public void OnQuerySuccess(bool isGetReward, int rankNumber, int maxRank, List<com.kodgames.corgi.protocol.FCRewardInfo> rewardInfos, string desc, long nextResetTime)
	{
		weekRewardLabel.Text = desc;
		getRewardBtn.Hide(false);

		Debug.Log(rankNumber.ToString() + maxRank.ToString());

		if (rankNumber == 0)
		{
			getRewardBtn.Hide(true);
			playerFriendship.Text = string.Empty;

			playerFriendShipNot.Text = GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_PlayerRankNot", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString());
		}
		else
		{
			if (rankNumber == -1)
			{
				getRewardBtn.Hide(true);
				playerFriendship.Text = string.Empty;
				playerFriendShipNot.Text = GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_PlayerRank_1",
												GameDefines.textColorBtnYellow.ToString(),
												GameDefines.textColorWhite.ToString(),
												maxRank);
			}
			else
			{
				playerFriendShipNot.Text = string.Empty;
				getRewardBtn.controlIsEnabled = !isGetReward;
				getRewardBtn.Text = !isGetReward ? GameUtility.GetUIString("UIPnlFriendCampaginWeekReward_GetRewards_1") : GameUtility.GetUIString("UIPnlFriendCampaginWeekReward_GetRewards_2");

				playerFriendship.Text = GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_PlayerRank_2",
												GameDefines.textColorBtnYellow.ToString(),
												GameDefines.textColorWhite.ToString(),
												rankNumber);
			}
		}

		ClearData();
		StartCoroutine("FillRewardList", rewardInfos);

		this.netResetTime = nextResetTime;
		this.waitRequest = false;
	}

	public void OnGetRewardSuccess(bool isGetReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
		//Show Reward.
		List<KodGames.Pair<int, int>> rewards = SysLocalDataBase.ConvertIdCountList(costAndRewardAndSync.Reward);
		string message = GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_GetRewards", GameDefines.textColorBtnYellow.ToString());

		for (int index = 0; index <= rewards.Count - 1; index++)
			if (index != rewards.Count - 1)
				message += GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_GetRewards_3",
							GameDefines.textColorBtnYellow.ToString(),
							ItemInfoUtility.GetAssetName(rewards[index].first),
							GameDefines.textColorWhite.ToString(),
							rewards[index].second);
			else
				message += GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_GetRewards_4", GameDefines.textColorBtnYellow.ToString(),
							ItemInfoUtility.GetAssetName(rewards[index].first),
							GameDefines.textColorWhite.ToString(),
							rewards[index].second);

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(message, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);

		getRewardBtn.controlIsEnabled = !isGetReward;
		getRewardBtn.Text = !isGetReward ? GameUtility.GetUIString("UIPnlFriendCampaginWeekReward_GetRewards_1") : GameUtility.GetUIString("UIPnlFriendCampaginWeekReward_GetRewards_2");
	}
}
