using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemGuildAssignmentItem : MonoBehaviour
{
	public SpriteText nameLabel;
	public SpriteText roleLabel;
	public SpriteText levelLabel;
	public UIButton selectBtn;
	public UIBox selectBox;
	public UIListItemContainer container;

	public void SetData(KodGames.ClientClass.GuildTransferMember memberData)
	{
		nameLabel.Text = memberData.PlayerName;

		//if (memberData.RoleId <= 0)
		//    memberData.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		GuildConfig.Role role = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(memberData.RoleId);
		roleLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_MemberRole", role.Color, role.Name);
		levelLabel.Text = GameUtility.FormatUIString("UIPnlGuildApplyInfo_Level", memberData.PlayerLevel);
		selectBtn.Data = memberData.PlayerId;
		container.Data = this;
		SetButtonState(false);
	}

	public void SetButtonState(bool isSelect)
	{
		selectBox.Hide(!isSelect);
	}
}
