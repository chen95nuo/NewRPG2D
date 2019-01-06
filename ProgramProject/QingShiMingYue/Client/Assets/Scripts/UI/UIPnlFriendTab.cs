using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendTab : UIModule
{
	public UIButton addFriendBtn;
	public UIButton backBtn;

	public UIScrollList friendList;
	public GameObjectPool friendObjectPool;

	public GameObject friendCountRoot;
	public SpriteText friendCountLabel;

	public UIScrollList searchList;
	public GameObjectPool searchObjectPool;

	public UITextField searchForm;

	public SpriteText emptyTip;

	private List<KodGames.ClientClass.FriendInfo> friendInfos;
	private bool showFriendPage = false;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlFriendTab);

		ShowFriendPage();

		return true;
	}

	public override void OnHide()
	{
		ClearList();

		base.OnHide();
	}

	public void ClearList()
	{
		ClearFriendList();
		ClearSearchList();
	}

	//切换搜索与好友列表状态切换
	private void ResetContrlStatus(bool showFriendPage)
	{
		// Clear list.
		ClearList();

		this.showFriendPage = showFriendPage;

		searchForm.gameObject.SetActive(!showFriendPage);
		friendCountRoot.SetActive(showFriendPage);

		addFriendBtn.Hide(!showFriendPage);
		backBtn.Hide(showFriendPage);

		searchForm.Text = "";
	}

	//好友列表
	#region Friend List
	private void ShowFriendPage()
	{
		ResetContrlStatus(true);

		RequestMgr.Inst.Request(new QueryFriendListReq());
	}

	public void OnQueryFriendListSuccess(List<KodGames.ClientClass.FriendInfo> friendInfos)
	{
		this.friendInfos = friendInfos;
		StartCoroutine("FillFriendUI");
	}

	public void ClearFriendList()
	{
		StopCoroutine("FillFriendUI");
		friendList.ClearList(false);
		friendList.ScrollPosition = 0;

		emptyTip.Text = "";
	}

	public void SortFriend()
	{
		//时间排序后添加的在上面
		friendInfos.Sort((a1, a2) =>
		{
			int online1 = a1.IsOnLine ? 1 : 0;
			int online2 = a2.IsOnLine ? 1 : 0;

			if (online1 != online2)
				return online2 - online1;

			return (int)(a2.MakeFriendTime - a1.MakeFriendTime);
		});
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillFriendUI()
	{
		yield return null;

		SortFriend();

		// Fill list
		for (int i = 0; i < friendInfos.Count; i++)
		{
			UIListItemContainer item = friendObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemFriendItem friendItem = item.gameObject.GetComponent<UIElemFriendItem>();
			item.Data = friendItem;
			friendItem.SetData(friendInfos[i]);
			friendList.AddItem(item);
		}

		if (friendList.Count <= 0)
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Friend");
		else
			emptyTip.Text = "";

		// Set Friend count
		SetFriendCountUI();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAddFriendClick(UIButton btn)
	{
		ShowAddFriendPage();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCommunicateClick(UIButton btn)
	{
		KodGames.ClientClass.FriendInfo friendPlayer = btn.Data as KodGames.ClientClass.FriendInfo;
		KodGames.ClientClass.PlayerRecord player = new KodGames.ClientClass.PlayerRecord();

		player.PlayerId = friendPlayer.PlayerId;
		player.PlayerLevel = friendPlayer.Level;
		player.VipLevel = friendPlayer.VipLevel;
		player.PlayerName = friendPlayer.Name;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChatTab), player);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnFriendItemClick(UIButton btn)
	{
		// Show friend action dialog		
		KodGames.ClientClass.FriendInfo friendInfo = btn.Data as KodGames.ClientClass.FriendInfo;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendMessage), friendInfo);
	}

	public void OnDeleteFriend(KodGames.ClientClass.FriendInfo playerToDel)
	{
		SysModuleManager.Instance.GetSysModule<SysUIEnv>().HideUIModule<UIDlgMessage>();
		// Check if the player has been delete.
		MainMenuItem okMenu = new MainMenuItem();
		okMenu.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_OK");
		okMenu.Callback = OnConfirmDeleteFriend;
		okMenu.CallbackData = playerToDel;

		MainMenuItem cancelMenu = new MainMenuItem();
		cancelMenu.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIDlgFriendMsg_Title_Delete"),
			string.Format(GameUtility.GetUIString("UIDlgFriendMsg_Msg_Delete"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, playerToDel.Name, GameDefines.textColorBtnYellow),
			cancelMenu,
			okMenu,
			null);

		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIDlgMessage>().ShowDlg(showData, true);
	}

	private bool OnConfirmDeleteFriend(object data)
	{
		KodGames.ClientClass.FriendInfo friendPlayer = data as KodGames.ClientClass.FriendInfo;
		RequestMgr.Inst.Request(new RemoveFriendReq(friendPlayer));

		return true;
	}

	public void OnRemoveFriendAlready(KodGames.ClientClass.FriendInfo removePlayer)
	{
		MainMenuItem okMenu = new MainMenuItem();
		okMenu.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_OK");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgFriendMsg_Title_Delete"),
			string.Format(GameUtility.GetUIString("UIDlgFriendMsg_Msg_AlreadyDelete"), removePlayer.Name), okMenu, null, null);

		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	//实时添加好友
	public void OnAddFriendSuccess(KodGames.ClientClass.FriendInfo friendInfo)
	{
		//不在好友列表不添加
		if (!showFriendPage)
			return;
		//已存在好友不添加
		for (int index = 0; index < friendList.Count; ++index)
		{
			UIElemFriendItem friendItem = friendList.GetItem(index).Data as UIElemFriendItem;
			if (friendItem.FriendInfo.PlayerId == friendInfo.PlayerId)
				return;
		}

		friendInfos.Add(friendInfo);

		ClearFriendList();
		StartCoroutine("FillFriendUI");
		//UIListItemContainer item = friendObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		//UIElemFriendItem addItem = item.gameObject.GetComponent<UIElemFriendItem>();
		//item.Data = addItem;
		//addItem.SetData(friendInfo);
		//friendList.AddItem(item);

		SetFriendCountUI();
	}


	public void OnRemoveFriendSuccess(int removePlayerId)
	{
		// Remove from scroll list

		for (int index = 0; index < friendList.Count; ++index)
		{
			UIElemFriendItem friendItem = friendList.GetItem(index).Data as UIElemFriendItem;
			if (friendItem.FriendInfo.PlayerId == removePlayerId)
			{
				friendList.RemoveItem(index, false, true, true);
				break;
			}
		}

		SetFriendCountUI();
	}

	private void SetFriendCountUI()
	{
		friendCountLabel.Text = GameUtility.FormatUIString("UIPnlFriendTab_Label_FirendCount",
			friendList.Count,
			ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.MaxFriendCount));
	}
	#endregion

	//搜索列表
	#region Add Friend List
	private void ShowAddFriendPage()
	{
		ResetContrlStatus(false);

		RequestMgr.Inst.Request(new RandomFriendReq());
	}

	public void OnRandomFriendSuccess(List<KodGames.ClientClass.FriendInfo> searchInfos)
	{
		//Fill friend list.
		ClearSearchList();
		StartCoroutine("FillSearchList", searchInfos);
	}

	public void ClearSearchList()
	{
		StopCoroutine("FillSearchList");
		searchList.ClearList(false);
		searchList.ScrollPosition = 0;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillSearchList(List<KodGames.ClientClass.FriendInfo> searchInfos)
	{
		yield return null;

		for (int i = 0; i < searchInfos.Count; i++)
		{
			//Add To List
			UIListItemContainer item = searchObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemFriendSearchResultItem searchItem = item.gameObject.GetComponent<UIElemFriendSearchResultItem>();
			item.Data = searchItem;
			searchItem.SetData(searchInfos[i]);
			searchList.AddItem(item);
		}
	}

	private UIElemFriendSearchResultItem SearchFriendItemInList(int playerId)
	{
		for (int index = 0; index < searchList.Count; index++)
		{
			UIElemFriendSearchResultItem searchItem = searchList.GetItem(index).Data as UIElemFriendSearchResultItem;
			if (searchItem.PlayerId == playerId)
				return searchItem;
		}

		return null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		ShowFriendPage();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSearchFriendClick(UIButton btn)
	{
		string formTxt = searchForm.spriteText.Text.Trim();
		string formPlaceHolder = string.Format("{0}{1}", searchForm.placeHolderColorTag, searchForm.placeHolder);
		ResetContrlStatus(false);
		if (formTxt.Equals("") || formTxt.Equals(formPlaceHolder))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFriendMsg_Msg_InputNull"));
		else
		{
			Debug.Log(formTxt);
			RequestMgr.Inst.Request(new QueryPlayerNameReq(formTxt));
		}
	}

	public void OnQueryPlayerNameSuccess(List<KodGames.ClientClass.FriendInfo> friendInfos)
	{
		// Fill friend list.
		ClearSearchList();
		StartCoroutine("FillSearchList", friendInfos);
	}

	public void OnQueryPlayerNameFailed(string errMsg)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFriendMsg_Msg_NotFind"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnInviteFriendClick(UIButton btn)
	{
		GameUtility.InviteFriend(this.friendInfos.Count, (btn.Data as KodGames.ClientClass.FriendInfo).PlayerId);
	}

	public void OnInviteFriendSuccess(int playerId, bool inviteSuccess)
	{
		// Update UI
		var listItem = SearchFriendItemInList(playerId);

		if (listItem != null)
			listItem.SetSearchState(ClientServerCommon._FriendStatus.Invited);

		// Show Message
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFriendMsg_Title_InviteFriendSuccess"));
	}
	#endregion
}