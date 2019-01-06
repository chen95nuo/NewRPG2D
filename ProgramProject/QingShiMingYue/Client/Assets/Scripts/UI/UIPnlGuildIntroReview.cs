using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildIntroReview : UIModule
{
	public delegate void OnApplySuccess();
	public UIScrollList reviewList;
	public GameObjectPool reviewItemPool;
	public GameObjectPool viewMorePool;
	public SpriteText emptyLabel;
	public UIButton oneKeyRefuseBtn;

	private List<KodGames.ClientClass.GuildApplyInfo> guildApplyInfos;
	private OnApplySuccess onApplySuccess;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildIntroTab>().ChangeTabButtons(_UIType.UIPnlGuildIntroReview);

		RequestMgr.Inst.Request(new GuildQueryApplyListReq(InitView));

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillReviewList");
		reviewList.ClearList(false);
		reviewList.ScrollPosition = 0f;

		guildApplyInfos = null;
	}

	public bool InitView(object guildApplyInfos)
	{
		reviewList.ClearList(false);
		reviewList.ScrollPosition = 0f;

		this.guildApplyInfos = guildApplyInfos as List<KodGames.ClientClass.GuildApplyInfo>;
		this.guildApplyInfos.Sort((a1, a2) =>
			{
				return (int)(a1.Time - a2.Time);
			});

		StartCoroutine("FillReviewList");

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillReviewList()
	{
		yield return null;

		int maxItemPerPage = ConfigDatabase.DefaultCfg.GuildConfig.GuildCountPerPage;
		int currentIndex = GetApplyInfoCount();

		for (int index = currentIndex; index < guildApplyInfos.Count && index < (currentIndex + maxItemPerPage); index++)
		{
			var container = reviewItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemGuildIntroReviewItem>();

			container.Data = item;
			item.SetData(guildApplyInfos[index]);

			if (HasShowMoreButton())
				reviewList.InsertItem(container, reviewList.Count - 1, true, "", false);
			else
				reviewList.InsertItem(container, reviewList.Count, true, "", false);
		}

		if (GetApplyInfoCount() == guildApplyInfos.Count)
		{
			if (HasShowMoreButton())
				reviewList.RemoveItem(reviewList.Count - 1, false, true, false);
		}
		else
		{
			if (HasShowMoreButton() == false)
			{
				UIListItemContainer viewMoreContainer = viewMorePool.AllocateItem().GetComponent<UIListItemContainer>();
				reviewList.InsertItem(viewMoreContainer, reviewList.Count, true, "", false);
			}
		}

		if (this.guildApplyInfos.Count <= 0)
			emptyLabel.Text = GameUtility.GetUIString("UIPnlGuildIntroReview_Empty");
		else
			emptyLabel.Text = string.Empty;

		oneKeyRefuseBtn.Hide(this.guildApplyInfos.Count <= 0);
	}

	private int GetApplyInfoCount()
	{
		if (reviewList.Count <= 0)
			return 0;

		int count = reviewList.Count;
		if (reviewList.GetItem(reviewList.Count - 1).Data == null)
			count--;

		return count;
	}

	private bool HasShowMoreButton()
	{
		if (reviewList.Count == 0)
			return false;

		return reviewList.GetItem(reviewList.Count - 1).Data == null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnNextPageClick(UIButton btn)
	{
		StopCoroutine("FillReviewList");
		StartCoroutine("FillReviewList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickViewAvatar(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq((int)btn.Data));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefuseApply(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildReviewApplyReq((int)btn.Data, true, RefreshApplyList));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAgreeApply(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildReviewApplyReq((int)btn.Data, false, RefreshApplyList));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefuseAllButton(UIButton btn)
	{
		var playerIds = new List<int>();
		var playerNames = new List<string>();
		for (int index = 0; index < reviewList.Count; index++)
		{
			if (reviewList.GetItem(index).Data == null)
				continue;

			var item = reviewList.GetItem(index).Data as UIElemGuildIntroReviewItem;
			playerIds.Add(item.ApplyInfo.PlayerId);
			playerNames.Add(item.ApplyInfo.PlayerName);
		}

		string title = GameUtility.GetUIString("UIPnlGuildIntroReview_OnKeyRefuseTitle");
		string message = string.Empty;

		if (playerNames.Count == 1)
			message = GameUtility.FormatUIString("UIPnlGuildIntroReview_OnKeyRefuseMessage2", playerNames[0]);
		else if (playerNames.Count <= 5)
		{
			string playNameStr = string.Empty;
			for (int index = 0; index < playerNames.Count; index++)
			{
				if (index == playerNames.Count - 1)
					playNameStr += playerNames[index];
				else
					playNameStr += string.Format("{0}{1}", playerNames[index], GameUtility.GetUIString("UIPnlGuildIntroReview_Dot"));
			}
			message = GameUtility.FormatUIString("UIPnlGuildIntroReview_OnKeyRefuseMessage1", playerNames.Count, playNameStr);
		}
		else
		{
			string playNameStr = string.Empty;
			for (int index = 0; index < 5; index++)
			{
				if (index == 4)
					playNameStr += playerNames[index];
				else
					playNameStr += string.Format("{0}{1}", playerNames[index], GameUtility.GetUIString("UIPnlGuildIntroReview_Dot"));
			}

			message = GameUtility.FormatUIString("UIPnlGuildIntroReview_OnKeyRefuseMessage3", playerNames.Count, playNameStr);
		}

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
		okCallback.Callback = (userData) =>
		{
			RequestMgr.Inst.Request(new GuildOneKeyRefuseReq(playerIds, RefreshApplyList));

			return true;
		};

		MainMenuItem cancelCallback = new MainMenuItem();
		cancelCallback.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(title, message, cancelCallback, okCallback);

		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	private bool RefreshApplyList(List<int> deleteIds)
	{
		if (deleteIds.Count <= 0)
			return false;

		if (deleteIds.Count == 1)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildIntroReview_ReviewOneMember"));

		for (int index = guildApplyInfos.Count - 1; index >= 0; index--)
		{
			if (deleteIds.Contains(guildApplyInfos[index].PlayerId))
				guildApplyInfos.RemoveAt(index);
		}

		var deleteContainers = new List<UIListItemContainer>();
		for (int index = 0; index < reviewList.Count; index++)
		{
			if (reviewList.GetItem(index).Data == null)
				continue;

			if (deleteIds.Contains((reviewList.GetItem(index).Data as UIElemGuildIntroReviewItem).ApplyInfo.PlayerId))
				deleteContainers.Add(reviewList.GetItem(index) as UIListItemContainer);
		}

		foreach (var container in deleteContainers)
			reviewList.RemoveItem(container, false, true, false);

		if (GetApplyInfoCount() <= 0)
		{
			StopCoroutine("FillReviewList");
			reviewList.ClearList(false);
			reviewList.ScrollPosition = 0f;
			StartCoroutine("FillReviewList");
		}

		return true;
	}
}