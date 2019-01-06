using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildPointTalentBossItem : MonoBehaviour
{
	public UIElemAssetIcon BossIcon;
	public SpriteText talentAttrLabel;
	public SpriteText bossName;
	public SpriteText talentCount;
	public UIButton addTalent;

	public void SetData(GuildStageConfig.GuildTalent talent, int level)
	{
		addTalent.Data = talent;

		var guildRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId);

		if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.StageTalent)  && level < talent.TalentValues.Count)		
			addTalent.controlIsEnabled = true;
		else addTalent.controlIsEnabled = false;
		
		BossIcon.SetData(talent.IconId);		

		if(level <= 0)
			talentAttrLabel.Text = string.Format(talent.TalentDesc, GameDefines.textColorBtnYellow, GameDefines.textColorRed, 0);
		else
			talentAttrLabel.Text = string.Format(talent.TalentDesc, GameDefines.textColorBtnYellow, GameDefines.textColorGreen,talent.TalentValues[level -1].Value);

		bossName.Text = talent.TalentName;
		talentCount.Text = GameUtility.FormatUIString("UIPnlGuildPointTalentTab_Level", level, talent.TalentValues.Count);
	}
}
