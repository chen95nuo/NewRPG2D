using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIElemGuildChangeRoleItem : MonoBehaviour
{
	public SpriteText roleNameLabel;
	public SpriteText roleLeftCountLabel;
	public UIButton acceptRoot;
	public UIBox acceptBox;

	private int roleId;
	public int RoleId { get { return roleId; } }

	public void SetData(GuildConfig.Role roleCfg)
	{
		this.roleId = roleCfg.Id;

		// Hide Start.
		acceptBox.Hide(true);

		// Set RoleName.
		roleNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_MemberRole", roleCfg.Color, roleCfg.Name);

		// Set LeftCount.
		var guildLvCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetGuildLevelByLevel(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildLevel);
		int currentCount = 0;
		int maxCount = guildLvCfg.GetRoleLimitCoutByRoleId(roleCfg.Id);
		foreach (var member in SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers)
		{
			if (member.RoleId == roleCfg.Id)
				currentCount++;
		}

		acceptRoot.gameObject.SetActive(currentCount < maxCount);

		int memberCount = maxCount - currentCount >= 0 ? (maxCount - currentCount) : 0;

		roleLeftCountLabel.Text = memberCount.ToString();

		acceptRoot.Data = roleId;
	}

	public bool IsSelected
	{
		get { return acceptBox.IsHidden() == false; }
	}

	public void ChangeSelectStatus(bool selected)
	{
		acceptBox.Hide(!selected);
	}
}