using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemActivityItem : MonoBehaviour
{
	public UIElemAssetIcon activityBtn;
	public UIBox activityLight;
	public UIBox activityNotify;
	public UIBox activityResetBox;

	public void SetData(int activityID)
	{
		// Default View.
		SetButtonLight(false);
		SetButtonNotify(false);
		SetButtonResetBox(false);

		// Set Icon.
		var config = ConfigDatabase.DefaultCfg.GameConfig.GetOperationActivityByType(ActivityManager.Instance.GetActivityInRunActivity(activityID).ActivityData.ActivityType);
		if (config != null)
			activityBtn.SetData(config.activityIcon);

		// Set Icon Data.
		activityBtn.Data = activityID;
	}

	public void SetButtonLight(bool show)
	{
		activityLight.Hide(!show);
		activityBtn.border.controlIsEnabled = !show;
	}

	public void SetButtonNotify(bool show)
	{
		activityNotify.Hide(!show);
	}

	public void SetButtonResetBox(bool show)
	{
		activityResetBox.Hide(!show);
	}
}
