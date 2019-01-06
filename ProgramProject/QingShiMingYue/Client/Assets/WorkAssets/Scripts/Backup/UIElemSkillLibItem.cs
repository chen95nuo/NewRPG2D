#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemSkillLibItem : MonoBehaviour 
{
	public List<UISkillIconItem> skillIcons;
	
	public void SetData(List<KodGames.ClientClass.Skill> skills, MonoBehaviour script, string method)
	{
		foreach(UISkillIconItem skillIcon in skillIcons)
		{
			int index = skillIcons.IndexOf(skillIcon);
			
			KodGames.ClientClass.Skill itemSkill = null;
			if(index < skills.Count)
			{
				itemSkill = skills[index];
			}
			
			skillIcon.SetData(itemSkill, script, method);
		}
	}
}

#endif
