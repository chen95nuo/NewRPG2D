using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendCampaginThisWeekRank : UIModule
{
	public UIScrollList rankList;
	public GameObjectPool rankRootPool;

	public SpriteText playerRank;
	public SpriteText playerFriendship;

	public SpriteText thisWeekRankLabel;
	public SpriteText notList;

	private bool waitRequest;
	private long netResetTime;
	private float deltaTime;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		QueryRankList();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillRankList");
		rankList.ClearList(false);
		rankList.ScrollListTo(0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRankList(List<com.kodgames.corgi.protocol.FCRankInfo> rankInfos)
	{
		yield return null;

		rankInfos.Sort((r1, r2) =>
		{
			return r1.rank - r2.rank;
		});

		foreach (var rankInfo in rankInfos)
		{
			if (rankInfo.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId && rankInfo.rank <= 0)
				continue;

			UIElemLastWeekRankRootItem item = rankRootPool.AllocateItem(false).GetComponent<UIElemLastWeekRankRootItem>();
			item.SetData(rankInfo);
			rankList.AddItem(item);
		}

		if (rankList.Count <= 0)
			notList.Text = GameUtility.GetUIString("UIPnlFriendCampaginThisWeekRank_NotList");
		else
			notList.Text = string.Empty;
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
					QueryRankList();
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAvatarView(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq((int)btn.Data));
	}

	//点击情义值详细记录
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickShowShips(UIButton btn)
	{
		//发送查询详细协议，参数为周期约定
		RequestMgr.Inst.Request(new QueryFCPointDetailReq(_FCRankType.CurrentPeriod));
	}

	private void QueryRankList()
	{
		this.waitRequest = true;
		RequestMgr.Inst.Request(new QueryFCRankReq(_FCRankType.CurrentPeriod));
	}

	public void OnQuerySuccess(int maxRankPlayer, List<com.kodgames.corgi.protocol.FCRankInfo> rankInfos, string desc, long nextResetTime)
	{
		thisWeekRankLabel.Text = desc;

		com.kodgames.corgi.protocol.FCRankInfo myRankInfo = null;
		foreach (var rankInfo in rankInfos)
		{
			if (rankInfo.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			{
				myRankInfo = rankInfo;
				break;
			}
		}

		if (myRankInfo != null)
		{
			if (myRankInfo.rank == 0)
				playerRank.Text = GameUtility.FormatUIString("UIPnlFriendCampaginLastWeekRank_RankNot_1",
											GameDefines.textColorBtnYellow.ToString(),
											GameDefines.textColorWhite.ToString());

			else if (myRankInfo.rank == -1)
				playerRank.Text = GameUtility.FormatUIString("UIPnlFriendCampaginLastWeekRank_RankNot",
											GameDefines.textColorBtnYellow.ToString(),
											GameDefines.textColorWhite.ToString(),
											maxRankPlayer);
			else
				playerRank.Text = GameUtility.FormatUIString("UIPnlFriendCampaginLastWeekRank_Rank",
											GameDefines.textColorBtnYellow.ToString(),
											GameDefines.textColorWhite.ToString(),
											myRankInfo.rank);

			playerFriendship.Text = GameUtility.FormatUIString("UIPnlFriendCampaginLastWeekRank_Ships",
											GameDefines.textColorBtnYellow.ToString(),
											GameDefines.textColorWhite.ToString(),
											myRankInfo.point);
		}

		ClearData();
		StartCoroutine("FillRankList", rankInfos);

		this.netResetTime = nextResetTime;
		this.waitRequest = false;
	}
}
