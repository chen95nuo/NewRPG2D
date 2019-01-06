using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIDlgGuildMemberOperation : UIModule
{
	public enum OperatorType
	{
		ViewAvatar = 1 << 1,
		AddFirend = 1 << 2,
		Chat = 1 << 3,
		RoleChange = 1 << 4,
		KickOfGuild = 1 << 5,
		TransferGuild = 1 << 6,
		Quit = 1 << 7,
	}

	public AutoSpriteControlBase uiRoot;
	public UIBox childLayoutBox;
	public UIChildLayoutControl childLayout;
	public List<UIButton> operationButtons;

	public delegate void FinishOperationDel(OperatorType operation, int operatorPlayerId);

	private float uiTitleWidth = 30f;
	private float uiBottomWidth = 20f;
	private Vector2 originalSize;
	private KodGames.ClientClass.GuildMemberInfo guildMemberInfo;
	private List<KodGames.ClientClass.FriendInfo> friendInfos;
	private FinishOperationDel finishOperationDel;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// Record Original Size.
		originalSize = new Vector2(uiRoot.width, uiRoot.height);

		// Set Button Data.
		var operatorItems = (OperatorType[])Enum.GetValues(typeof(OperatorType));
		for (int index = 0; index < operatorItems.Length; index++)
			operationButtons[index].Data = (int)operatorItems[index];

		// Set Default View.
		SetChildLayoutStatus(0);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.guildMemberInfo = userDatas[0] as KodGames.ClientClass.GuildMemberInfo;
		this.friendInfos = userDatas[1] as List<KodGames.ClientClass.FriendInfo>;

		if (userDatas.Length > 2)
			this.finishOperationDel = userDatas[2] as FinishOperationDel;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		this.guildMemberInfo = null;
		this.friendInfos = null;
		this.finishOperationDel = null;

		SetChildLayoutStatus(0);
	}

	private void InitView()
	{
		if (this.IsShown == false)
			return;

		// Set ChildLayout.
		//if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId <= 0)
		//    SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		var guildRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId);
		var memberRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(guildMemberInfo.RoleId);

		int status = 0;
		if (SysLocalDataBase.Inst.LocalPlayer.PlayerId == guildMemberInfo.PlayerId)
		{
			if (!guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeLeader))
				status = (int)OperatorType.Quit;
			else
				status = (int)OperatorType.TransferGuild;
		}
		else
		{
			status = (int)OperatorType.ViewAvatar | (int)OperatorType.Chat | (int)OperatorType.AddFirend;

			foreach (var friend in friendInfos)
			{
				if (friend.PlayerId == guildMemberInfo.PlayerId)
				{
					status = status ^ (int)OperatorType.AddFirend;
					break;
				}
			}

			if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeRoleId) && guildRoleCfg.Id < guildMemberInfo.RoleId)
				status |= (int)OperatorType.RoleChange;


			bool canShowRoleChange = false;

			if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeRoleId))
			{
				foreach (var role in ConfigDatabase.DefaultCfg.GuildConfig.Roles)
				{
					if (role.Id <= guildRoleCfg.Id)
						continue;

					if (role.CanOffered == false)
						continue;

					if (role.Id == memberRoleCfg.Id)
						continue;

					if (role.Id < memberRoleCfg.Id)
						canShowRoleChange = true;
					else if (role.Id > memberRoleCfg.Id)
					{
						if (memberRoleCfg.CanDownRole)
							break;
						else
							canShowRoleChange = true;
					}
				}
			}

			if (canShowRoleChange)
				status |= (int)OperatorType.RoleChange;

			if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.KickMember) && guildRoleCfg.Id < guildMemberInfo.RoleId)
				status |= (int)OperatorType.KickOfGuild;
		}

		SetChildLayoutStatus(status);
	}

	private void SetChildLayoutStatus(int status)
	{
		// Set Button Status.
		foreach (var button in operationButtons)
			childLayout.HideChildObj(button.gameObject, (status & (int)button.Data) == 0);

		// ReSet BgRoot Height.
		float totalSize = 0f;
		foreach (var item in childLayout.childLayoutControls)
			totalSize += item.hide ? 0 : item.GetWidthHeight(false) + item.bottomRightOffset;

		if (totalSize + uiTitleWidth + uiBottomWidth > originalSize.y)
			uiRoot.SetSize(originalSize.x, totalSize + uiTitleWidth + uiBottomWidth);
		else
			uiRoot.SetSize(originalSize.x, originalSize.y);

		// Reset relative position.
		var components = this.gameObject.GetComponentsInChildren<EZScreenPlacement>();
		if (components != null)
		{
			foreach (var ez in components)
				ez.PositionOnScreenRecursively();
		}

		if ((uiRoot.height - childLayoutBox.height) / 2 < uiTitleWidth)
		{
			var pos = childLayout.transform.localPosition;
			pos.y -= uiTitleWidth - (uiRoot.height - childLayoutBox.height) / 2;
			childLayout.transform.localPosition = pos;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickViewAvatar(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq(this.guildMemberInfo.PlayerId));
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAddFirend(UIButton btn)
	{
		GameUtility.InviteFriend(this.friendInfos.Count, guildMemberInfo.PlayerId);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChat(UIButton btn)
	{
		var player = new KodGames.ClientClass.PlayerRecord();
		player.PlayerId = guildMemberInfo.PlayerId;
		player.VipLevel = guildMemberInfo.VipLevel;
		player.PlayerLevel = guildMemberInfo.PlayerLevel;
		player.PlayerName = guildMemberInfo.PlayerName;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChatTab), player);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRoleOperation(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildChangeRole), guildMemberInfo, new UIDlgGuildChangeRole.OnChangeRoleSuccess(() =>
		{
			if (finishOperationDel != null)
				finishOperationDel(OperatorType.RoleChange, guildMemberInfo.PlayerId);

			HideSelf();
		}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickKickOfGuild(UIButton btn)
	{
		string title = GameUtility.GetUIString("UIDlgGuildMemberOperation_KickOfGuild");
		string message = GameUtility.FormatUIString("UIDlgGuildMemberoOperationKickMember", guildMemberInfo.PlayerName);

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
		okCallback.Callback = (userData) =>
		{
			RequestMgr.Inst.Request(new GuildKickPlayerReq(guildMemberInfo.PlayerId, () =>
			{
				if (finishOperationDel != null)
					finishOperationDel(OperatorType.KickOfGuild, guildMemberInfo.PlayerId);

				HideSelf();
				return true;
			}));

			return true;
		};

		MainMenuItem cancelCallback = new MainMenuItem();
		cancelCallback.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(title, message, cancelCallback, okCallback);

		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTransferGuild(UIButton btn)
	{
		if (guildMemberInfo.PlayerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildAssignment), new UIDlgGuildAssignment.OnAssignmentSuccess(() =>
			{
				if (finishOperationDel != null)
					finishOperationDel(OperatorType.TransferGuild, guildMemberInfo.PlayerId);

				HideSelf();
			}));
			return;
		}

		RequestMgr.Inst.Request(new GuildTransferReq(guildMemberInfo.PlayerId, () =>
			{
				if (finishOperationDel != null)
					finishOperationDel(OperatorType.TransferGuild, guildMemberInfo.PlayerId);

				HideSelf();
				return true;
			}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuitGuild(UIButton btn)
	{
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
		okCallback.Callback = (userData) =>
		{
			RequestMgr.Inst.Request(new GuildQuitReq());

			return true;
		};

		MainMenuItem cancelCallback = new MainMenuItem();
		cancelCallback.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIPnlGuildIntroInfo_QuitGuildTitle"), GameUtility.GetUIString("UIPnlGuildIntroInfo_QuitGuildMessage"), cancelCallback, okCallback);

		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}