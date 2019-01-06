using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEastSeaFindFairyTimer : UIModule
{
	public enum UITimeType
	{
		NONE,
		UIPnlEastElementRankingList,
		UIPnlEastSeaFindFairyMain,
		UIPnlEastSeaCloseActivity
	}

	public SpriteText activityCountDownTimerLable;
	public SpriteText refreshTimerLable;

	public UITimeType uiType = UITimeType.NONE;


	private long nextRefreshTime;
	private long lastRefreshTime;
	private bool isZentiaOpen;
	private float deltaTime = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		this.nextRefreshTime = ActivityZentia.Instance.NextRefreshTime(lastRefreshTime);
		this.isZentiaOpen = ActivityZentia.Instance.IsOpen;

		if (userDatas != null && userDatas.Length > 0)
			SysUIEnv.Instance.GetUIModule<UIPnlRunActivityTab>().SetActivityIconBackLight((int)userDatas[0]);
		QueryInfo();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		this.lastRefreshTime = 0;
		this.nextRefreshTime = 0;
		this.isZentiaOpen = false;
	}

	public virtual void Update()
	{
		if (IsShown == false)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1.0f)
		{
			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

			if (activityCountDownTimerLable != null)
			{
				if (ActivityZentia.Instance.IsOpen)
					UIUtility.UpdateUIText(activityCountDownTimerLable, string.Format("{0}{1}", GameDefines.zentiaTimerTextColor, GameUtility.Time2String(ActivityZentia.Instance.ActivityInfo.CloseTime - nowTime)));
				else
					activityCountDownTimerLable.Text = GameUtility.FormatUIString("UIPnlEastElementRankingList_ActivityOver", GameDefines.colorTipsRed);
			}
			// Set Refresh Label.
			if (refreshTimerLable != null)
				UIUtility.UpdateUIText(refreshTimerLable, nextRefreshTime <= 0 ? (ActivityZentia.Instance.ActivityInfo != null ? GameUtility.Time2String(ActivityZentia.Instance.ActivityInfo.CloseTime - nowTime) : "00:00:00") : GameUtility.Time2String(nextRefreshTime - nowTime));

			if (nextRefreshTime > 0 && nextRefreshTime < nowTime && uiType == UITimeType.UIPnlEastSeaFindFairyMain)
				QueryInfo();

			if (isZentiaOpen != ActivityZentia.Instance.IsOpen)
			{
				isZentiaOpen = !isZentiaOpen;
				ChangeCurrentShowUI();
			}

			deltaTime = 0f;
		}
	}

	private void ChangeCurrentShowUI()
	{
		switch (uiType)
		{
			case UITimeType.UIPnlEastElementRankingList:

				RequestMgr.Inst.Request(new EastSeaQueryZentiaRankReq());
				break;

			case UITimeType.UIPnlEastSeaCloseActivity:

				SysUIEnv.Instance.ShowUIModule<UIPnlEastSeaFindFairyMain>();
				break;

			case UITimeType.UIPnlEastSeaFindFairyMain:

				SysUIEnv.Instance.ShowUIModule<UIPnlEastSeaCloseActivity>();
				break;
		}
	}

	protected void QueryInfo()
	{
		switch (uiType)
		{
			case UITimeType.UIPnlEastSeaCloseActivity:
			case UITimeType.UIPnlEastSeaFindFairyMain:
				RequestMgr.Inst.Request(new EastSeaQueryZentiaReq((isSuccess) =>
				{
					this.lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
					this.nextRefreshTime = ActivityZentia.Instance.NextRefreshTime(lastRefreshTime);

					return true;
				}));
				break;
		}
	}
}


