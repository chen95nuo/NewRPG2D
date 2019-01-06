using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class UIPnlActivityInnTab : UIModule
{
	public UIScrollList timesList;
	public GameObjectPool timesPool;

	public UIButton restButton;
	public SpriteText restDescLabel;

	private bool enteredTimeSpan = false;
	private DateTime nextRefreshUITime;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// In case of that the show function is not called by UIPnlActivityTab
		//   set the selected item of UIPnlActivityTab
		if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityTab))
			SysUIEnv.Instance.LoadUIModule(typeof(UIPnlActivityTab));

		SysUIEnv.Instance.GetUIModule<UIPnlActivityTab>().SetActiveButton(_UIType.UIPnlActivityInnTab);

		ClearData();

		RequestMgr.Inst.Request(new QueryFixedTimeActivityRewardReq());

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		// Clear ScrollList.
		StopCoroutine("FillList");
		timesList.ClearList(false);
		timesList.ScrollPosition = 0f;

		// Clear Data.
		enteredTimeSpan = false;
	}

	private void Update()
	{
		if (!IsShown || ActivityManager.Instance == null)
			return;

		RefreshUI(SysLocalDataBase.Inst.LoginInfo.NowDateTime);
	}

	private void RefreshUI(DateTime nowTime)
	{
		if (nowTime < nextRefreshUITime || timesList.Count <= 0)
			return;

		// If [enteredTimeSpan] shows that last second it wasn't in a time-cell span
		//   and the current second it enters
		//   refresh list
		var activityFixedTime = ActivityManager.Instance.GetActivity<ActivityFixedTime>();
		if (enteredTimeSpan != activityFixedTime.CheckFixedTime(nowTime, false))
		{
			enteredTimeSpan = !enteredTimeSpan;

			// Refresh restButton.
			SetRestButtonState();
		}

		nextRefreshUITime = nowTime.AddSeconds(1);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		var activityFixedTime = ActivityManager.Instance.GetActivity<ActivityFixedTime>();
		var nowTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		var lastGetTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(activityFixedTime.LastGetTime);

		var startTimes = activityFixedTime.ActivityInfo.GetTimersByStatus(_ActivityTimerStatus.Start);
		var endTiems = activityFixedTime.ActivityInfo.GetTimersByStatus(_ActivityTimerStatus.End);

		for (int i = 0; i < startTimes.Count; i++)
		{
			var container = timesPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemTimesItem>();

			item.SetData(startTimes[i].Timer, endTiems[i].Timer, activityFixedTime.RestTimeType);
			item.RefreshData(nowTime, lastGetTime);
			container.Data = item;

			timesList.AddItem(container);
		}
	}

	private void SetRestDescLabel(List<KodGames.ClientClass.Reward> rewards)
	{
		if (rewards == null || rewards.Count <= 0)
			restDescLabel.Text = string.Empty;

		restDescLabel.Text = GameUtility.FormatUIString("UIActivityInnTab_Desc", rewards[0].Consumable[0].Amount, ItemInfoUtility.GetAssetName(rewards[0].Consumable[0].Id));
	}

	private void SetRestButtonState()
	{
		var nowTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		var activityFixedTime = ActivityManager.Instance.GetActivity<ActivityFixedTime>();

		// Set RestButton Status.
		restButton.Hide(!activityFixedTime.CheckFixedTime(nowTime, true));

		for (int index = 0; index < timesList.Count; index++)
		{
			UIListItemContainer itemContainer = timesList.GetItem(index) as UIListItemContainer;
			UIElemTimesItem item = itemContainer.data as UIElemTimesItem;

			if (item != null)
				item.RefreshData(nowTime, SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(activityFixedTime.LastGetTime));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRestButton(UIButton btn)
	{
		RequestMgr.Inst.Request(new GetFixedTimeActivityRewardReq());
	}

	public void OnResponseFixedActivitySuccess(KodGames.ClientClass.Reward reward)
	{
		SetRestButtonState();

		// Show reward tips
		string rewardDesc = SysLocalDataBase.GetRewardDesc(reward, true);

		if (string.IsNullOrEmpty(rewardDesc) == false)
		{
			UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
			showData.SetData(rewardDesc, true, true);
			SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
		}
	}

	public void OnResponseQueryFixedActivitySuccess(List<KodGames.ClientClass.Reward> rewards)
	{
		// Set Desc Label.
		SetRestDescLabel(rewards);

		// Set RestButton Status.
		SetRestButtonState();

		enteredTimeSpan = ActivityManager.Instance.GetActivity<ActivityFixedTime>().CheckFixedTime(SysLocalDataBase.Inst.LoginInfo.NowDateTime, false);

		StartCoroutine("FillList");

		nextRefreshUITime = SysLocalDataBase.Inst.LoginInfo.NowDateTime.AddSeconds(1);

	}
}


