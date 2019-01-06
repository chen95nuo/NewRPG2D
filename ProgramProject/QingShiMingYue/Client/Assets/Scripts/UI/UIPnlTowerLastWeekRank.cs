using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerLastWeekRank : UIModule
{
	public UIScrollList rankScrollList;
	public GameObjectPool rankPool;
	public GameObjectPool moreItemPool;
	public SpriteText rankLabel;
	public SpriteText pointLabel;
	public SpriteText arrivalLayerLabel;
	public SpriteText playerNameLabel;
	public SpriteText notRankLabel;

	private List<com.kodgames.corgi.protocol.MfRankInfo> mfRankInfos;
	private com.kodgames.corgi.protocol.MfRankInfo viewRankInfo;

	private const int sMaxRows = 10;
	private int currentPosition = -1;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Set the tab button
		SysUIEnv.Instance.GetUIModule<UIPnlTowerPoint>().SetSelectedBtn(_UIType.UIPnlTowerLastWeekRank);

		ClearData();
		RequestMgr.Inst.Request(new MelaleucaFloorLastWeekReq());
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
		rankScrollList.ClearList(false);
		rankScrollList.ScrollPosition = 0f;
	}

	public void OnQueryLastWeekInfoSuccess(int rank, int point, int arrivalLayer, List<com.	kodgames.corgi.protocol.MfRankInfo> mfRankInfos)
	{
		//pointLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerRank_Label_DayPoint"), point);
		pointLabel.Text = point.ToString();
		arrivalLayerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerRank_Label_LayerCount"), arrivalLayer);
		playerNameLabel.Text = SysLocalDataBase.Inst.LocalPlayer.Name;

		if(SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekPoint > 0)
		{
			rankLabel.Text = rank.ToString();
			notRankLabel.Text = string.Empty;
		}
		else 		
		{
			rankLabel.Text = string.Empty;
			notRankLabel.Text = GameUtility.GetUIString("UIPnlTowerRank_Label_WeekReward_NO");
		}
		this.mfRankInfos = mfRankInfos;

		ClearData();

		if (this.mfRankInfos.Count > 0)
		{
			currentPosition = 0;
			StartCoroutine("FillList");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		int index = currentPosition;
		for (; index < currentPosition + sMaxRows && index < mfRankInfos.Count; index++)
		{
			UIListItemContainer uiContainer = rankPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemTowerThisWeekRank item = uiContainer.GetComponent<UIElemTowerThisWeekRank>();
			uiContainer.data = item;

			item.SetData(mfRankInfos[index], SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekRank == mfRankInfos[index].rank);
			// Add the new item in last of the scrollList if the last Item of scrollList is not the MoreItem.
			if (HasShowMoreButton() == false)
				rankScrollList.AddItem(uiContainer);
			else
				rankScrollList.InsertItem(uiContainer, rankScrollList.Count - 1);
		}

		currentPosition = index;

		if (currentPosition < mfRankInfos.Count)
		{
			if (HasShowMoreButton() == false)
				AddShowMoreButton();
		}
		else if (HasShowMoreButton())
			RemoveShowMoreButton();
	}

	private bool HasShowMoreButton()
	{
		return rankScrollList.Count == 0 || (rankScrollList.GetItem(rankScrollList.Count - 1).Data is UIElemTowerThisWeekRank) == false;
	}

	private void AddShowMoreButton()
	{
		rankScrollList.AddItem(moreItemPool.AllocateItem());
	}

	private void RemoveShowMoreButton()
	{
		rankScrollList.RemoveItem(rankScrollList.Count - 1, false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMoreCardShow(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}

	// LineUp Show
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLineUpShowClick(UIButton btn)
	{
		// LineUp Show
		viewRankInfo = btn.Data as com.kodgames.corgi.protocol.MfRankInfo;
		RequestMgr.Inst.Request(new QueryPlayerInfoReq(viewRankInfo.playerId));
	}

	public void ShowViewLineUp(KodGames.ClientClass.Player player)
	{
		GameUtility.ShowViewAvatarUI(player, viewRankInfo.rank);
	}
}