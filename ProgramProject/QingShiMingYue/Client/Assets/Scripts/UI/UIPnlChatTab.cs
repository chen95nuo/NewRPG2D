using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlChatTab : UIModule
{
	//////////////////////////////////////////////////////////////////////////
	// UI controls
	//////////////////////////////////////////////////////////////////////////
	public UIBox mainBorder;
	public UIBox chatBg;

	public UIBox newPersonMsg;
	public UIBox guildMsg;

	public SpriteText tipLabel;
	public UITextField msgField;
	public UIButton sendMsgBtn;
	public GameObject bottomCtrlBg;

	public UIButton worldTabBtn;
	public UIButton privateTabBtn;
	public UIButton guildTabBtn;

	public UIScrollList msgList;
	public GameObjectPool msgPool;

	//门派关卡底部挡板
	public UIButton backBg;

	//////////////////////////////////////////////////////////////////////////
	// Local params
	//////////////////////////////////////////////////////////////////////////
	private float ORIGIN_LIST_H = 300f;
	private float ORIGIN_BOT_CTRL_Y = -360f;
	private float ORIGIN_MAINBG_H = 380f;
	private float ORIGIN_CHATBG_H = 307f;

	private const int ORIGIN_SCREEN_WIDTH = 320;
	private const int ORIGIN_SCREEN_HEIGHT = 480;

	private bool canSend = true;

	// [mainMenuBot]'s hight
	private const float LENGTHEN = 55f;

	private DateTime nextResetTime;
	private bool screenValueResetted = false;
	private bool requestSent = false;

	private com.kodgames.corgi.protocol.ChatMessage selectedMsg = null;

	private bool IsWorldShowing { get { return worldTabBtn.controlIsEnabled == false; } }
	private bool IsPrivateShowing { get { return privateTabBtn.controlIsEnabled == false; } }
	private bool IsGuildShowing { get { return guildTabBtn.controlIsEnabled == false; } }

	#region PrepareUI
	private void RefreshNextResetTime(DateTime dateTime)
	{
		nextResetTime = KodGames.TimeEx.GetTimeAfterTime(
			ConfigDatabase.DefaultCfg.GameConfig.worldChatCfg.worldChatResetTime,
			dateTime,
			ClientServerCommon._TimeDurationType.Day);
	}

	private void ResetOriginValues()
	{
		if (screenValueResetted)
			return;

		int screenH = Screen.height;
		int screenW = Screen.width;

		if (screenW / 2 > screenH / 3)
			screenW = (screenH / 3) * 2;

		float lengthDelta = ((float)(screenH * ORIGIN_SCREEN_WIDTH)) / ((float)screenW) - (float)ORIGIN_SCREEN_HEIGHT;

		ORIGIN_LIST_H += lengthDelta;
		ORIGIN_BOT_CTRL_Y -= lengthDelta;
		ORIGIN_MAINBG_H += lengthDelta;
		ORIGIN_CHATBG_H += lengthDelta;

		screenValueResetted = true;
	}

	/// <summary>
	/// control the length of the main frame
	/// </summary>
	private void LengthenFrame(bool isMainMenuShowing)
	{
		if (isMainMenuShowing)
		{
			mainBorder.SetSize(mainBorder.width, ORIGIN_MAINBG_H);
			chatBg.SetSize(chatBg.width, ORIGIN_CHATBG_H);
			msgList.SetViewableArea(msgList.viewableArea.x, ORIGIN_LIST_H);

			bottomCtrlBg.transform.localPosition = GetPosWithNewY(bottomCtrlBg, ORIGIN_BOT_CTRL_Y);
		}
		else
		{
			mainBorder.SetSize(mainBorder.width, ORIGIN_MAINBG_H + LENGTHEN);
			chatBg.SetSize(chatBg.width, ORIGIN_CHATBG_H + LENGTHEN);
			msgList.SetViewableArea(msgList.viewableArea.x, ORIGIN_LIST_H + LENGTHEN);

			bottomCtrlBg.transform.localPosition = GetPosWithNewY(bottomCtrlBg, ORIGIN_BOT_CTRL_Y - LENGTHEN);
		}
	}

	private Vector3 GetPosWithNewY(GameObject transObject, float y)
	{
		return new Vector3(transObject.transform.localPosition.x, y, transObject.transform.localPosition.z);
	}

	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose()
	{
		SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs.Clear();
		SysLocalDataBase.Inst.LocalPlayer.ChatData.PrivateChatMsgs.Clear();
		SysLocalDataBase.Inst.LocalPlayer.ChatData.GuildChatMsgs.Clear();

		SysLocalDataBase.Inst.LocalPlayer.ChatData.OfflineMessageQueried = false;

		RequestMgr.Inst.Request(new CloseChatMessageDialog());

		HideSelf();
		if (!ItemInfoUtility.IsInMainScene())
		{
			if (SysUIEnv.Instance.GetUIModule<UIPnlGuildPointRankTab>() != null && SysUIEnv.Instance.GetUIModule<UIPnlGuildPointRankTab>().IsShown)
				SysUIEnv.Instance.HideUIModule<UIPnlGuildPointRankTab>();
			if (SysUIEnv.Instance.GetUIModule<UIPnlGuildPointBossRank>() != null && SysUIEnv.Instance.GetUIModule<UIPnlGuildPointBossRank>().IsShown)
				SysUIEnv.Instance.HideUIModule<UIPnlGuildPointBossRank>();
			if (SysUIEnv.Instance.GetUIModule<UIPnlGuildPointExplorationRank>() != null && SysUIEnv.Instance.GetUIModule<UIPnlGuildPointExplorationRank>().IsShown)
				SysUIEnv.Instance.HideUIModule<UIPnlGuildPointExplorationRank>();
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPointMain);
		}
	
	}

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		worldTabBtn.Data = _ChatType.World;
		privateTabBtn.Data = _ChatType.Private;
		guildTabBtn.Data = _ChatType.Guild;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		//门派关卡中显示挡板
		if (SysGameStateMachine.Instance.CurrentState is GameState_GuildPoint)
			backBg.Hide(false);
		else
			backBg.Hide(true);

		msgField.Text = string.Empty;
		ResetOriginValues();
		RefreshNextResetTime(SysLocalDataBase.Inst.LoginInfo.NowDateTime);

		if (userDatas.Length > 0)
		{
			KodGames.ClientClass.PlayerRecord player = userDatas[0] as KodGames.ClientClass.PlayerRecord;

			if (player != null)
			{
				// construct logical [selectedMsg]
				selectedMsg = new com.kodgames.corgi.protocol.ChatMessage();
				selectedMsg.senderId = player.PlayerId;
				selectedMsg.senderName = player.PlayerName;
				selectedMsg.senderVipLevel = player.VipLevel;
				selectedMsg.receiverId = SysLocalDataBase.Inst.LocalPlayer.PlayerId;
				selectedMsg.receiverName = SysLocalDataBase.Inst.LocalPlayer.Name;
				selectedMsg.receiverVipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;
				selectedMsg.time = SysLocalDataBase.Inst.LoginInfo.NowTime;
				selectedMsg.messageType = _ChatType.Private;

				ShowPrivateMsg();

				return true;
			}

			if ((bool)userDatas[1])
			{
				ShowGuildMsg();
				if (userDatas.Length > 2)
				{
					if ((bool)userDatas[2] == false)
						backBg.Hide(false);
					else
						backBg.Hide(true);
				}
				return true;
			}

		}

		SetNewPersonMsgCount();

		selectedMsg = null;
		ShowWolrdMsg();

		//清空未读世界聊天的数量
		SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadWorldChatMsgCount = 0;

		return true;
	}

	private void ShowWolrdMsg()
	{
		SetTabBtn(worldTabBtn);

		UpdateWorldChatTip();

		canSend = true;

		LoadMsgs();
	}

	private void UpdateWorldChatTip()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.CurrentChatCount < ConfigDatabase.DefaultCfg.GameConfig.worldChatCfg.count)
			tipLabel.Text = GameUtility.FormatUIString("UIPnlChatTab_Label_FreeLeft", ConfigDatabase.DefaultCfg.GameConfig.worldChatCfg.count - SysLocalDataBase.Inst.LocalPlayer.CurrentChatCount);
		else
			tipLabel.Text = GameUtility.FormatUIString("UIPnlChatTab_Label_ConsumableLeft", GetWChatItemCount());
	}

	// on showing this pnl in the normal way(show world chat view first)
	//   get the amount of [WorldChatItem]
	private int GetWChatItemCount()
	{
		var item = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(ConfigDatabase.DefaultCfg.ItemConfig.worldChatItemId);
		return item != null ? item.Amount : 0;
	}

	private void ShowPrivateMsg()
	{
		SetTabBtn(privateTabBtn);

		newPersonMsg.Hide(true);

		SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadPrivateChatMsgs = false;
		SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount = 0;

		if (selectedMsg == null)
		{
			tipLabel.Text = GameUtility.GetUIString("UIPnlChatTab_Tip_TabToSend");
			canSend = false;
		}
		else
		{
			tipLabel.Text = GameUtility.FormatUIString("UIPnlChatTab_Private_To", selectedMsg.senderName);
			canSend = true;
		}

		LoadMsgs();
	}

	private void ShowGuildMsg()
	{
		SetTabBtn(guildTabBtn);

		guildMsg.Hide(true);

		tipLabel.Text = "";

		SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadGuildChatMsgs = false;
		SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount = 0;

		canSend = true;

		LoadMsgs();
	}

	private void SetTabBtn(UIButton btn)
	{
		worldTabBtn.controlIsEnabled = (btn.Equals(worldTabBtn) == false);
		privateTabBtn.controlIsEnabled = (btn.Equals(privateTabBtn) == false);
		guildTabBtn.controlIsEnabled = (btn.Equals(guildTabBtn) == false);
	}

	private void LoadMsgs()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.ChatData.OfflineMessageQueried)
		{
			StopCoroutine("FillMsgList");
			msgList.ClearList(false);

			StartCoroutine("FillMsgList");
		}
		else if (requestSent == false)
		{
			RequestMgr.Inst.Request(new QueryChatMessageListReq());
			requestSent = true;
		}
	}

	public void OnRequestMsgListSuccess()
	{
		SetNewPersonMsgCount();

		requestSent = false;
		LoadMsgs();
	}

	private void SetNewPersonMsgCount()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount > 0)
		{
			newPersonMsg.Hide(false);
			newPersonMsg.Text = SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount.ToString();
		}
		else
			newPersonMsg.Hide(true);

		if (SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount > 0)
		{
			guildMsg.Hide(false);
			guildMsg.Text = SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount.ToString();
		}
		else
			guildMsg.Hide(true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillMsgList()
	{
		yield return null;

		if (IsWorldShowing)
		{
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadWorldChatMsgCount = 0;
			InsertMsgs(SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs);
		}

		if (IsPrivateShowing)
		{
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount = 0;
			InsertMsgs(SysLocalDataBase.Inst.LocalPlayer.ChatData.PrivateChatMsgs);
		}

		if (IsGuildShowing)
		{
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount = 0;
			InsertMsgs(SysLocalDataBase.Inst.LocalPlayer.ChatData.GuildChatMsgs);
		}

		yield return null;
	}

	private void InsertMsgs(List<com.kodgames.corgi.protocol.ChatMessage> msgs)
	{
		if (msgs == null || msgs.Count <= 0)
			return;

		for (int index = 0; index < msgs.Count; index++)
			InsertOneMsg(msgs[index]);
	}

	private void InsertOneMsg(com.kodgames.corgi.protocol.ChatMessage msg)
	{
		if ((IsPrivateShowing && msg.messageType != _ChatType.Private))
			return;

		if ((IsWorldShowing && msg.messageType == _ChatType.FlowMessage))
			return;

		if (IsGuildShowing && msg.messageType != _ChatType.Guild)
			return;

		UIListItemContainer container = msgPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemChatMessageItem item = container.gameObject.GetComponent<UIElemChatMessageItem>();
		item.SetData(msg, IsWorldShowing);
		container.Data = item;

		msgList.AddItem(container);
		msgList.ScrollToItem(msgList.Count - 1, 0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabBtnClick(UIButton btn)
	{
		switch ((int)btn.Data)
		{
			case _ChatType.World:
				ShowWolrdMsg();
				break;

			case _ChatType.Private:
				ShowPrivateMsg();
				break;

			case _ChatType.Guild:
				ShowGuildMsg();
				break;
		}
	}

	public void Update()
	{
		// update free chat count

		if (IsShown == false || IsWorldShowing == false)
			return;

		DateTime nowTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;

		if (nowTime < nextResetTime)
			return;

		RefreshNextResetTime(nowTime);
		SysLocalDataBase.Inst.LocalPlayer.CurrentChatCount = 0;
		return;
	}

	private void SetSelectedMsg(com.kodgames.corgi.protocol.ChatMessage msg)
	{
		if (selectedMsg == null)
			selectedMsg = new com.kodgames.corgi.protocol.ChatMessage();

		selectedMsg.messageType = msg.messageType;
		selectedMsg.time = msg.time;

		if (msg.senderId == SysLocalDataBase.Inst.LocalPlayer.PlayerId && msg.messageType == _ChatType.Private)
		{
			selectedMsg.senderId = msg.receiverId;
			selectedMsg.senderName = msg.receiverName;
			selectedMsg.senderVipLevel = msg.receiverVipLevel;
			selectedMsg.receiverId = msg.senderId;
			selectedMsg.receiverName = msg.senderName;
			selectedMsg.receiverVipLevel = msg.senderVipLevel;
		}
		else
		{
			selectedMsg.senderId = msg.senderId;
			selectedMsg.senderName = msg.senderName;
			selectedMsg.senderVipLevel = msg.senderVipLevel;
			selectedMsg.receiverId = msg.receiverId;
			selectedMsg.receiverName = msg.receiverName;
			selectedMsg.receiverVipLevel = msg.receiverVipLevel;
		}
	}

	// only world message sent by player 
	// and private message sent to the user can be clicked
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMsgClick(UIButton btn)
	{
		com.kodgames.corgi.protocol.ChatMessage msg = btn.Data as com.kodgames.corgi.protocol.ChatMessage;

		if (msg == null)
			return;

		SetSelectedMsg(msg);

		// in [worldMsgList] view, when a click-able button clicked
		//   show a dialog to offer varies of choices
		if (IsWorldShowing || IsGuildShowing)
		{
			List<MainMenuItem> menuItems = new List<MainMenuItem>();

			//// 查看阵容
			MainMenuItem chatPrivateMenu = new MainMenuItem();
			chatPrivateMenu.Callback = OnSendPrivateMsgCallback;
			chatPrivateMenu.ControlText = GameUtility.GetUIString("UIPnlChatTab_Dlg_Ctrl_SendPrivateMsg");
			menuItems.Add(chatPrivateMenu);

			MainMenuItem addFriendMenu = new MainMenuItem();
			addFriendMenu.Callback = OnAddFriendCallback;
			addFriendMenu.ControlText = GameUtility.GetUIString("UIPnlChatTab_Dlg_Ctrl_AddFriend");
			menuItems.Add(addFriendMenu);

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(selectedMsg.senderName, true, true, menuItems.ToArray());
			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
		}

		// in [privateMsgList] view, set [tipLabel]'s text and enable [sendMsgBtn]
		if (IsPrivateShowing)
		{
			tipLabel.Text = GameUtility.FormatUIString("UIPnlChatTab_Private_To", selectedMsg.senderName);
			canSend = true;
		}
	}

	private bool OnViewLineupCallback(object data)
	{
		//RequestMgr.Inst.Request(new QueryPlayerInfoReq(selectedMsg.senderId));
		return true;
	}

	private bool OnSendPrivateMsgCallback(object data)
	{
		ShowPrivateMsg();
		return true;
	}

	private bool OnAddFriendCallback(object data)
	{
		//SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("FUNC_CLOSE"));		
		RequestMgr.Inst.Request(new InviteFriendReq(selectedMsg.senderId, selectedMsg.senderName));

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSendClick(UIButton btn)
	{
		if (!canSend)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlChatTab_Tip_NoReceiver"));
			return;
		}

		if (msgField.Text.Equals(string.Empty))
			return;

		if (IsWorldShowing
			&& SysLocalDataBase.Inst.LocalPlayer.CurrentChatCount >= ConfigDatabase.DefaultCfg.GameConfig.worldChatCfg.count
			&& GetWChatItemCount() <= 0)
		{
			GameUtility.ShowNotEnoughtAssetUI(ConfigDatabase.DefaultCfg.ItemConfig.worldChatItemId, 0);
			return;
		}

		com.kodgames.corgi.protocol.ChatMessage msg = new com.kodgames.corgi.protocol.ChatMessage();

		// send up to 50 chars, the exceed part will be ignored directly(without notification)
		if (msgField.Text.Length > 50)
			msg.content = msgField.Text.Substring(0, 50);
		else
			msg.content = msgField.Text;

		msg.senderId = SysLocalDataBase.Inst.LocalPlayer.PlayerId;
		msg.senderName = SysLocalDataBase.Inst.LocalPlayer.Name;
		msg.senderVipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;

		// if [sendMsgBtn] tabbed when [worldMsgBg] on show
		//   this message is regard as a world chat message
		// otherwise, it will be regard as a private chat message
		if (IsWorldShowing)
			msg.messageType = _ChatType.World;
		else if (IsPrivateShowing)
		{
			// private message must have a receiver
			msg.messageType = _ChatType.Private;
			msg.receiverId = selectedMsg.senderId;
			msg.receiverName = selectedMsg.senderName;
			msg.receiverVipLevel = selectedMsg.senderVipLevel;
		}
		else if (IsGuildShowing)
			msg.messageType = _ChatType.Guild;
		else
			return;

		// after recording essential informations, cleat the [msgField] and insert the new message into both message views
		msgField.Text = string.Empty;

		RequestMgr.Inst.Request(new ChatReq(msg));
	}

	public void OnWorldMsgReceived(List<com.kodgames.corgi.protocol.ChatMessage> msgs)
	{
		if (false == IsShown || false == IsWorldShowing)
			return;

		InsertMsgs(msgs);
	}

	public void OnPrivateMsgReceived(com.kodgames.corgi.protocol.ChatMessage msg)
	{
		if (!IsShown)
			return;

		if (IsPrivateShowing)
		{
			InsertOneMsg(msg);
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount = 0;
			SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadPrivateChatMsgs = false;
		}
		else if (SysLocalDataBase.Inst.LocalPlayer.PlayerId != msg.senderId)
		{
			InsertOneMsg(msg);

			SetNewPersonMsgCount();

			SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadPrivateChatMsgs = true;
		}
	}

	public void OnGuildMsgReceived(com.kodgames.corgi.protocol.ChatMessage msg)
	{
		if (!IsShown)
			return;

		if (IsGuildShowing)
		{
			InsertOneMsg(msg);
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount = 0;
			SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadGuildChatMsgs = false;
		}
		else if (SysLocalDataBase.Inst.LocalPlayer.PlayerId != msg.senderId)
		{
			InsertOneMsg(msg);

			SetNewPersonMsgCount();

			SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadGuildChatMsgs = true;
		}
	}

	public void OnWorldMsgSent()
	{
		UpdateWorldChatTip();
	}
}
