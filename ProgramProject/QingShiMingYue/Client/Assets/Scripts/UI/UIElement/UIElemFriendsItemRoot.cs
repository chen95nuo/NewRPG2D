using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendsItemRoot : UIListItemContainerEx
{
	//专门用来管理预制品的
	private UIElemFriendsItem uiItem;

	//数据保存
	private KodGames.ClientClass.FriendInfo uiPlayer;
	public int PlayerId
	{
		get { return uiPlayer.PlayerId; }
	}

	private bool uiChack;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendsItem>();

			SetData(uiPlayer, uiChack);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(KodGames.ClientClass.FriendInfo player, bool chack)
	{
		this.uiPlayer = player;
		this.uiChack = chack;

		if (uiItem == null)
			return;

		if (player == null)
			return;

		uiItem.avatarViewBtn.Data = player.PlayerId;
		uiItem.chackFriendBtn.Data = this;
		uiItem.chackFriendBox.Hide(!this.uiChack);
		uiItem.playerLevel.Text = GameUtility.FormatUIString("UIPnlSelectFriends_Level", player.Level);

		if (player.IsOnLine)
		{
			uiItem.playerLixian.Text = string.Empty;
			uiItem.playerName.color.a = 1f;
		}
		else
		{
			uiItem.playerLixian.Text = GameUtility.FormatUIString("UIPnlFriendNumbers_Lixian", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(),
				GameUtility.Time2SortString(SysLocalDataBase.Inst.LoginInfo.NowTime - player.LastLoginTime));

			uiItem.playerName.color.a = 0.5f;
		}

		uiItem.playerName.Text = player.Name;
	}

	public void SelectBoxToggle()
	{
		this.uiChack = !this.uiChack;
		uiItem.chackFriendBox.Hide(!uiItem.chackFriendBox.IsHidden());
	}

	public bool IsSelectBoxStage()
	{
		return this.uiChack;
	}

	public bool JudgeTimeWithDay()
	{
		long time = SysLocalDataBase.Inst.LoginInfo.NowTime - this.uiPlayer.LastLoginTime;
		long day = time / 1000 / 60 / 60 / 24;
		long hour = time / 1000 / 60 / 60 % 24;

		if (((int)(day * 24 + hour)) >= ConfigDatabase.DefaultCfg.FriendCampaignConfig.FriendOffLineTime)
			return false;

		return true;
	}
}
