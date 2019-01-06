#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UISkillIconItem : MonoBehaviour
{
	public UIButton iconBgBtn;
	public UIElemAssetIcon iconBtn;
	public UIButton selectedBtn;

	public void SetData(KodGames.ClientClass.Skill skill, MonoBehaviour script, string method)
	{
		iconBtn.SetTriggerMethod(script, method);
		iconBtn.Data = skill;

		if (skill == null)
		{
			iconBtn.Hide(true);
			iconBgBtn.Hide(true);
			selectedBtn.Hide(true);

			return;
		}

		iconBtn.Hide(false);
		iconBtn.SetData(skill.ResourceId);
		iconBgBtn.Hide(false);

		iconBgBtn.Text = ItemInfoUtility.GetAssetName(skill.ResourceId);
	}

	public void SetSelected(bool selected)
	{
		selectedBtn.Hide(!selected);
	}
}

#endif
