using UnityEngine;
using System;
using System.Collections;
using ClientServerCommon;

public class UIElemChatMessageItem : MonoBehaviour
{
	public SpriteText msgTitleLabel;
	public SpriteText msgContentLabel;
	public SpriteText msgTimeLabel;
	public UIButton itemBg;

	private static Color CLR_CHANNEL_WORLD = GameDefines.colorExpBlue;
	//private static Color CLR_CHANNEL_FLOWMES = GameDefines.txColorOrange;
	private static Color CLR_CHANNEL_SYS = GameDefines.colorTipsRed;
	private static Color CLR_CHANNEL_PRIVATE = GameDefines.colorGoldYellow;
	private static Color CLR_TXT_PLAYER = GameDefines.colorFollowerGreen;
	private static Color CLR_TXT_VIP = GameDefines.colorWpnNameOrange;
	private static Color CLR_TXT_OUTSTAND = GameDefines.colorGoldYellow;
	private static Color CLR_TXT_NORMAL = GameDefines.colorWhite;

	private const float ONE_LINE_HIGHT = 28f;
	private const float ORG_ITM_BG_HIGHT = 40f;

	public void SetData(com.kodgames.corgi.protocol.ChatMessage msg, bool worldMsgShowing)
	{
		msgTitleLabel.Text = string.Empty;
		msgContentLabel.Text = string.Empty;

		itemBg.controlIsEnabled = false;
		if ((msg.messageType == _ChatType.Private)
			|| (msg.messageType == _ChatType.Guild)
			|| ((msg.messageType == _ChatType.World) && (msg.senderId != SysLocalDataBase.Inst.LocalPlayer.PlayerId)))
		{
			itemBg.controlIsEnabled = true;
			itemBg.Data = msg;
		}

		msgTitleLabel.Text = GetMsgTitle(msg, worldMsgShowing);

		if (((msg.messageType == _ChatType.Private) && worldMsgShowing)
			|| (msg.senderId == SysLocalDataBase.Inst.LocalPlayer.PlayerId && (!worldMsgShowing) && msg.messageType != _ChatType.Guild))
			msgContentLabel.Text = CLR_TXT_OUTSTAND.ToString();

		if (msg.messageType == _ChatType.Guild && !worldMsgShowing)
			msgContentLabel.Text = GameDefines.textColorGuildChat.ToString();

		msgContentLabel.Text += msg.content;

		if (msgContentLabel.GetDisplayLineCount() <= 1)
			itemBg.SetSize(itemBg.width, ONE_LINE_HIGHT);
		else
			itemBg.SetSize(itemBg.width, ORG_ITM_BG_HIGHT);

		if (msg.messageType == _ChatType.Guild)
		{
			msgTimeLabel.Hide(false);

			DateTime date = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(msg.time);
			msgTimeLabel.Text = GameUtility.FormatUIString("UIDlgGuildMessage_Time", date.Year, date.Month, date.Day) + GameUtility.FormatUIString("UITimeFormat_Hour", date.Hour, date.Minute, date.Second);
		}
		else
			msgTimeLabel.Hide(true);
	}

	public string GetMsgTitle(com.kodgames.corgi.protocol.ChatMessage msg, bool worldMsgShowing)
	{
		string msgTitle = string.Empty;

		switch (msg.messageType)
		{
			case _ChatType.World:
				if (worldMsgShowing)
					msgTitle += GameUtility.FormatUIString("UIPnlChatTab_Msg_World", CLR_CHANNEL_WORLD);

				msgTitle += (CLR_TXT_PLAYER + msg.senderName);

				if (msg.senderVipLevel > 0)
					msgTitle += CLR_TXT_VIP + GameUtility.FormatUIString("UIPnlChatTab_Private_VIP", msg.senderVipLevel);

				return msgTitle;
			//case _ChatType.FlowMessage:
			//    if (worldMsgShowing)
			//        msgTitle += GameUtility.FormatUIString("UIPnlChatTab_Msg_FlowMsg", CLR_CHANNEL_FLOWMES);

			//    return msgTitle;
			case _ChatType.System:
				if (worldMsgShowing)
					msgTitle += GameUtility.FormatUIString("UIPnlChatTab_Msg_System", CLR_CHANNEL_SYS);

				return msgTitle;

			case _ChatType.Private:
				if (worldMsgShowing)
					msgTitle += GameUtility.FormatUIString("UIPnlChatTab_Msg_Private", CLR_CHANNEL_PRIVATE);

				string msgSender = string.Empty;
				string msgReciver = string.Empty;

				if (msg.senderId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
				{
					msgSender = (CLR_TXT_PLAYER + GameUtility.GetUIString("UIPnlChatTab_Private_You"));
					msgReciver = (CLR_TXT_PLAYER + msg.receiverName);

					if (msg.receiverVipLevel > 0)
						msgReciver += CLR_TXT_VIP + GameUtility.FormatUIString("UIPnlChatTab_Private_VIP", msg.receiverVipLevel);
				}
				else if (msg.receiverId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
				{
					msgSender = (CLR_TXT_PLAYER + msg.senderName);
					msgReciver = (CLR_TXT_PLAYER + GameUtility.GetUIString("UIPnlChatTab_Private_You"));

					if (msg.senderVipLevel > 0)
						msgSender += CLR_TXT_VIP + GameUtility.FormatUIString("UIPnlChatTab_Private_VIP", msg.senderVipLevel);
				}
				else
				{
					Debug.LogError("neither the sender nor the receiver is current player");
					return string.Empty;
				}

				msgTitle += GameUtility.FormatUIString("UIPnlChatTab_Private_A_To_B", (msgSender + CLR_TXT_NORMAL), (msgReciver + CLR_TXT_NORMAL));
				return msgTitle;

			case _ChatType.Guild:
				//if (msg.senderRoleId <= 0)
				//    msg.senderRoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

				var role = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(msg.senderRoleId);

				if (msg.senderVipLevel > 0)
					msgTitle = GameUtility.FormatUIString("UIPnlChatTab_Title_GuildVip", role.Color, role.Name, GameDefines.cardColorChenSe, msg.senderName, msg.senderVipLevel);
				else
					msgTitle = GameUtility.FormatUIString("UIPnlChatTab_Title_Guild", role.Color, role.Name, GameDefines.cardColorChenSe, msg.senderName);

				return msgTitle;

			default:
				Debug.LogError("missing chat type");
				return msgTitle;
		}
	}
}
