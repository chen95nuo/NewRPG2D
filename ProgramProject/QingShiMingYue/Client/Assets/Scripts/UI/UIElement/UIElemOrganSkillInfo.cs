using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganSkillInfo : MonoBehaviour
{
	public SpriteText titleLabel;
	public SpriteText skillLabel;

	public AutoSpriteControlBase bg;

	private int defaultBgHeight = 80;

	//激活技能列表，所有技能列表，标头
	public void SetData(List<BeastConfig.BeastSkill> actSkills, List<BeastConfig.BeastSkill>maxSkills, string title, bool isStartSkill, bool isShowActive)
	{
		titleLabel.Text = title;		

		string skillMsg = "";

		for (int i = 0; i < maxSkills.Count; i ++)
		{
			bool isActive = false;

			for (int j = 0; j < actSkills.Count; j++)
			{
				if(actSkills[j].SkillId == maxSkills[i].SkillId)
					isActive = true;
			}

			if (!isActive && !isShowActive)
				continue;
			string msg = "";
			var skill = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastSkillByBeastSkillId(maxSkills[i].SkillId);
			if(skill != null)
				msg = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Info_Skill"), skill.SkillName, skill.Desc);

			if (isActive)
				skillMsg = skillMsg + string.Format(GameUtility.GetUIString("UIOrganSkillInfo_LineUp"), GameDefines.textColorGreen, msg) + "\n";
			else
			{
				string noActiveMsg = "";

				if(isStartSkill)
				{		
					int breakThough = ConfigDatabase.DefaultCfg.BeastConfig.GetMinStartSkillBeastLevelBySkillId(maxSkills[i].SkillId);
					noActiveMsg = string.Format(GameUtility.GetUIString("UIOrganSkillInfo_ActiveSkill_Label_Break"), ItemInfoUtility.GetLevelCN(breakThough));
					noActiveMsg = string.Format(GameUtility.GetUIString("UIOrganSkillInfo_ActiveSkill_Label"), noActiveMsg);
				}
				else
				{
					int level = ConfigDatabase.DefaultCfg.BeastConfig.GetMinLevelSkillBeastLevelBySkillId(maxSkills[i].SkillId);
					var levelCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(level);

					noActiveMsg = string.Format(GameUtility.GetUIString("UIOrganSkillInfo_ActiveSkill_Label"), ItemInfoUtility.GetAssetQualityLevelCNDesc(levelCfg.Quality));

					if (levelCfg.AddLevel > 0)
					{
						noActiveMsg = string.Format(GameUtility.GetUIString("UIOrganSkillInfo_ActiveSkill_Label_ADD"), ItemInfoUtility.GetAssetQualityLevelCNDesc(levelCfg.Quality), levelCfg.AddLevel);
						noActiveMsg = string.Format(GameUtility.GetUIString("UIOrganSkillInfo_ActiveSkill_Label"), noActiveMsg);
					}					
				}

				skillMsg = skillMsg + string.Format(GameUtility.GetUIString("UIOrganSkillInfo_LineUp_UnActive"), GameDefines.textColorGray, noActiveMsg, msg) + "\n";
			}			
		}

		if (skillMsg.Equals(""))
			skillLabel.Text = string.Format(GameUtility.GetUIString("UIOrganSkillInfo_NoLabel"), GameDefines.textColorBtnYellow);
		else
			skillLabel.Text = skillMsg;
	}

	public void AdaptSize()
	{
		int lineCount = skillLabel.GetDisplayLineCount() - 4;
		lineCount = lineCount > 0 ? lineCount : 0;
		bg.SetSize(bg.width, skillLabel.GetLineHeight() * lineCount + defaultBgHeight);
	}

	public void ReSetColor()
	{
		skillLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_ColorReset"), GameDefines.textColorGray, skillLabel.text);
	}
}

