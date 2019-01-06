using UnityEngine;
using System.Collections;

public class UIElemActivityTop : MonoBehaviour {

	public UIBox bgBtn;
	public UIButton activityBtn;

	public void SetData(int activityID)
	{
		activityBtn.Data = activityID;
	}

	public void Selected(bool ifElemSelected)
	{
		bgBtn.Hide(!ifElemSelected);
		activityBtn.controlIsEnabled = (!ifElemSelected);
	}
}
