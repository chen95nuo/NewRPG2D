using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgFriendMessage : UIModule
{
	public SpriteText nameLabel;

	private KodGames.ClientClass.FriendInfo friendInfo;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas != null)
			friendInfo = userDatas[0] as KodGames.ClientClass.FriendInfo;

		nameLabel.Text = friendInfo.Name;

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnViewLineup(UIButton btn)
	{
		HideSelf();
		RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq(friendInfo.PlayerId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnFightMsg(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.CombatFriend, friendInfo);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLeaveMsg(UIButton btn)
	{
		HideSelf();
		KodGames.ClientClass.PlayerRecord player = new KodGames.ClientClass.PlayerRecord();

		player.PlayerId = friendInfo.PlayerId;
		player.PlayerLevel = friendInfo.Level;
		player.VipLevel = friendInfo.VipLevel;
		player.PlayerName = friendInfo.Name;

		// Popup chat UI
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChatTab), player);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDeleteFriend(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnDeleteFriend(friendInfo);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}
}
