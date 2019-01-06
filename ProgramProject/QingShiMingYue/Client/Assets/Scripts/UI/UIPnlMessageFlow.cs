using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlMessageFlow : UIModule
{
	public const int cMinMessageCount = 5;

	//Flow message.
	public UIScrollList messageList;
	public GameObjectPool messageObjectPool;

	// Scroll speed
	public float scrollSpeed = 50f;
	public float addSpeed = 200f;
	public float msgStayTime = 2f;
	public float msgDeltaTime = 10f;

	private UIListItemContainer item;
	private const int GENERAL_MAX_SHOW_ROUND = 10;
	private const int SYSTEM_MAX_SHOW_ROUND = 0;
	private bool isInited = false;

	private void Update()
	{
		if (!this.IsShown)
			return;

		if (!isInited)
			AddMsg();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void AddMsg()
	{
		KodGames.ClientClass.MsgFlowData msgFlowData = SysLocalDataBase.Inst.LocalPlayer.MsgFlowData;
		if (msgFlowData.showMessage == null && msgFlowData.messages.Count == 0)
		{
			isInited = false;
			return;
		}

		com.kodgames.corgi.protocol.ChatMessage chatMsgToShow = (msgFlowData.showMessage == null ? msgFlowData.messages[0] : msgFlowData.showMessage);
		if (chatMsgToShow == null)
		{
			isInited = false;
			return;
		}

		if (chatMsgToShow.messageType == _ChatType.World)
		{
			isInited = false;
			return;
		}

		isInited = true;
		item = messageObjectPool.AllocateItem().GetComponent<UIListItemContainer>();

		switch (chatMsgToShow.messageType)
		{
			case _ChatType.System:
				item.Text = GameUtility.FormatUIString("UIPnlChatTab_FlowMsg_System", GameDefines.colorTipsRed, GameDefines.txColorWhite, chatMsgToShow.content);
				break;

			case _ChatType.FlowMessage:
				item.Text = GameUtility.FormatUIString("UIPnlChatTab_FlowMsg_FlowMsg", GameDefines.txColorOrange, GameDefines.txColorWhite, chatMsgToShow.content);
				break;

			//世界频道关闭
			//case _ChatType.World:
			//    item.Text = GameUtility.FormatUIString("UIPnlChatTab_FlowMsg_World", GameDefines.colorExpBlue, GameDefines.txColorGreen, chatMsgToShow.senderName, GameDefines.txColorWhite, chatMsgToShow.content);
			//    break;

			case _ChatType.Private:
				item.Text = GameUtility.FormatUIString("UIPnlChatTab_FlowMsg_Private", GameDefines.colorGoldYellow, GameDefines.txColorGreen, chatMsgToShow.senderName, GameDefines.txColorWhite, chatMsgToShow.content);
				break;
		}

		item.data = chatMsgToShow;
		messageList.AddItem(item);
		messageList.ScrollToItem(item, messageList.extraEndSpacing / addSpeed, EZAnimation.EASING_TYPE.Linear, messageList.extraEndSpacing);
		Invoke("ScrollMessage", Mathf.Max(msgStayTime, messageList.extraEndSpacing / addSpeed));

		if (msgFlowData.messages.Count > 0 && chatMsgToShow == msgFlowData.messages[0])
		{
			for (int index = 0; index < msgFlowData.msg_ShowTimeMap.Count; index++ )
			{
				if (chatMsgToShow == msgFlowData.msg_ShowTimeMap[index])
				{
					if (msgFlowData.msg_ShowTimeMap[index].displayCount <= 0)
						msgFlowData.msg_ShowTimeMap[index].displayCount = 1;
					else
						msgFlowData.msg_ShowTimeMap[index].displayCount++;
				}
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void ScrollMessage()
	{
		float scrollDistance = item.spriteText.TotalScreenWidth; //Mathf.Max(messageList.viewableArea.x, item.spriteText.TotalScreenWidth);
		messageList.ScrollToItem(item, scrollDistance / scrollSpeed, EZAnimation.EASING_TYPE.Linear, scrollDistance + messageList.viewableArea.x);
		Invoke("ClearList", scrollDistance / scrollSpeed);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void ClearList()
	{
		KodGames.ClientClass.MsgFlowData msgFlowData = SysLocalDataBase.Inst.LocalPlayer.MsgFlowData;
		com.kodgames.corgi.protocol.ChatMessage msg = item.data as com.kodgames.corgi.protocol.ChatMessage;

		if (msg == msgFlowData.showMessage)
		{
			msgFlowData.showMessage = null;
		}
		else if (msgFlowData.messages.Contains(msg))
		{
			msgFlowData.messages.Remove(msg);

			if (msgFlowData.messages.Count < cMinMessageCount)
			{
				if (msgFlowData.msg_ShowTimeMap.Contains(msg))
				{
					if ((msg.messageType != _ChatType.System && msg.displayCount < GENERAL_MAX_SHOW_ROUND)
						|| (msg.messageType == _ChatType.System && msg.displayCount < SYSTEM_MAX_SHOW_ROUND))
						msgFlowData.messages.Add(msg);
					else
						msgFlowData.messages.Remove(msg);
				}
			}
		}

		if (msgFlowData.messages.Contains(msg) && msg.messageType == _ChatType.System && msg.displayCount>= SYSTEM_MAX_SHOW_ROUND)
			msgFlowData.messages.Remove(msg);

		List<com.kodgames.corgi.protocol.ChatMessage> msgListToReMove = new List<com.kodgames.corgi.protocol.ChatMessage>();
		for (int index = 0; index < msgFlowData.msg_ShowTimeMap.Count; index++ )
		{
			if (!msgFlowData.messages.Contains(msgFlowData.msg_ShowTimeMap[index]))
				msgListToReMove.Add(msgFlowData.msg_ShowTimeMap[index]);
		}

		for (int index = 0; index < msgListToReMove.Count; index++ )
		{
			var msgToRemove = msgListToReMove[index];
			msgFlowData.msg_ShowTimeMap.Remove(msgToRemove);
		}

		messageList.ClearList(false);
		messageList.ScrollListTo(0f);
		Invoke("AddMsg", msgDeltaTime);
	}

}
