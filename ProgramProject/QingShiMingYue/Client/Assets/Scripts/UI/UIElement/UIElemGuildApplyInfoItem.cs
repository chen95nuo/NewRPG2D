using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemGuildApplyInfoItem : MonoBehaviour
{
	public SpriteText roleLabel;
	public SpriteText nameLabel;
	public SpriteText levelLabel;
	public UIButton addFriendBtn;
	public GameObject IsFriend;

	public void SetData(KodGames.ClientClass.GuildMemberInfo playerRecord, List<FriendInfo> firends)
	{
		nameLabel.Text = playerRecord.PlayerName;
		levelLabel.Text = GameUtility.FormatUIString("UIPnlGuildApplyInfo_Level", playerRecord.PlayerLevel);
		addFriendBtn.Data = playerRecord.PlayerId;

		//if (playerRecord.RoleId <= 0)
		//    playerRecord.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		GuildConfig.Role role = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(playerRecord.RoleId);
		roleLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_MemberRole", role.Color, role.Name);

		if (playerRecord.PlayerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			addFriendBtn.Hide(true);
		else
			addFriendBtn.Hide(false);

		foreach (var firend in firends)
		{
			if (firend.PlayerId == playerRecord.PlayerId)
			{
				addFriendBtn.Hide(true);
				IsFriend.SetActive(true);
				return;
			}
		}
		addFriendBtn.Hide(false);
		IsFriend.SetActive(false);

	}
}
