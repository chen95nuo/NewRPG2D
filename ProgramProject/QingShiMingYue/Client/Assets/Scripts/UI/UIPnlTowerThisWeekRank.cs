using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerThisWeekRank : UIModule
{
	public UIScrollList rankScrollList;
	public GameObjectPool rankPool;
	public GameObjectPool moreItemPool;
	public SpriteText titleLabel;
	public SpriteText dayPointLabel;
	public SpriteText dayLayerLabel;
	public SpriteText maxWeekPointLabel;
	public SpriteText predictRankLabel;
	public SpriteText emptyLabel;

	public UIBox playerBg0;
	public UIBox playerBg1;

	private List<com.kodgames.corgi.protocol.MfRankInfo> mfRankInfos;
	private com.kodgames.corgi.protocol.MfRankInfo viewRankInfo;

	private const int sMaxRows = 10;
	private int currentPosition = -1;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Set the tab button
		SysUIEnv.Instance.GetUIModule<UIPnlTowerPoint>().SetSelectedBtn(_UIType.UIPnlTowerThisWeekRank);

		RequestMgr.Inst.Request(new MelaleucaFloorThisWeekRankReq());
		return true;
	}

	public override void OnHide()
	{
		ClearData();

		base.OnHide();
	}

	private void ClearData()
	{
		// Clear List.
		StopCoroutine("FillList");
		rankScrollList.ClearList(false);
		rankScrollList.ScrollPosition = 0f;

		// Clear Local Data.
		currentPosition = -1;
	}

	public void OnQueryThisWeekInfoSuccess(int currentLayer, int currentPoint, int maxPointWeek, int predictRank, List<com.kodgames.corgi.protocol.MfRankInfo> mfRankInfos)
	{
		titleLabel.Text = GameUtility.GetUIString("UIPnlTowerRank_Label_Tips");
		dayPointLabel.Text = currentPoint.ToString();

		dayLayerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerRank_Label_LayerCount"), currentLayer);

		int maxPoint = 0;
		if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint > maxPointWeek)
			maxPoint = SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint;
		else
			maxPoint = maxPointWeek;

		maxWeekPointLabel.Text = maxPoint.ToString();
		predictRankLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerRank_Label_RankCount"), predictRank.ToString());
		this.mfRankInfos = mfRankInfos;

		ClearData();

		if (mfRankInfos.Count > 0)
		{
			emptyLabel.Text = string.Empty;
			currentPosition = 0;
			StartCoroutine("FillList");
		}
		else
			emptyLabel.Text = GameUtility.GetUIString("UIPnlTowerRank_Label_EmptyTips");
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
			
			item.SetData(mfRankInfos[index], SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.ThisWeekRank == mfRankInfos[index].rank);

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