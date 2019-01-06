using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemSkillPageItem : MonoBehaviour 
{
	public	List<UIElemSkillSelectToggleItem> skillList;
	
	public void SetData(List<KodGames.ClientClass.Skill> skills)
	{
		for(int index = 0; index < skillList.Count; index++)
		{
			if(skills.Count > index)
			{
				skillList[index].SetData(skills[index]);
				skillList[index].ResetToggleState(false);
			}
			else
			{
				skillList[index].SetData(null);
			}
		}
	}
}
