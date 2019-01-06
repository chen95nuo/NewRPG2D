using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlRunActivityTab : UIModule
{
	public UIScrollList activityList;
	public GameObjectPool activityPool;

	private int activityId = IDSeg.InvalidId;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// 注册活动绿点数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Activity, UpdataActivityIconNotify);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (ActivityManager.Instance.GetActivityIdInRunActivity().Count <= 0)
			SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowMainScene();
		else
			StartCoroutine("FillData");

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		// Clear ScrollList.
		StopCoroutine("FillData");
		activityList.ClearList(false);
		activityList.ScrollPosition = 0f;

		// Clear ActivityList.
		//activityId = IDSeg.InvalidId;
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消注册活动绿点数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Activity, UpdataActivityIconNotify);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public IEnumerator FillData()
	{
		yield return null;

		var runActivitys = ActivityManager.Instance.GetActivityIdInRunActivity();
		for (int index = 0; index < runActivitys.Count; index++)
		{
			var container = activityPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemActivityItem>();

			container.Data = item;
			item.SetData(runActivitys[index]);

			activityList.AddItem(item.gameObject);
		}

		yield return null;

		if (this.activityId == IDSeg.InvalidId || !runActivitys.Contains(this.activityId))
			this.activityId = runActivitys[0];

		SetActivityListStage();
	}

	private void SetActivityListStage()
	{
		SetIconStage();

		//Show UI.
		ShowUIAsActivityType();
	}

	private void SetIconStage()
	{
		for (int index = 0; index < activityList.Count; index++)
		{
			var item = activityList.GetItem(index).Data as UIElemActivityItem;
			item.SetButtonLight(activityId == (int)item.activityBtn.Data);
		}

		//Set Notify.
		UpdataActivityIconNotify();
	}

	private void ShowUIAsActivityType()
	{
		int uiType = _UIType.UnKonw;
		var currentActivity = ActivityManager.Instance.GetActivityInRunActivity(activityId);

		if (currentActivity != null)
		{
			switch (currentActivity.ActivityData.ActivityType)
			{
				case _ActivityType.ACCUMULATEACTIVITY:
					uiType = _UIType.UIPnlRunAccumulativeTab;
					break;
				case _ActivityType.ZENTIA:
					if (ActivityZentia.Instance.IsOpen)
						uiType = _UIType.UIPnlEastSeaFindFairyMain;
					else
						uiType = _UIType.UIPnlEastSeaCloseActivity;
					break;
			}
		}

		if (uiType == _UIType.UnKonw)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlRunAccumulativeTab_CentralCity"));
		else
		{
			SysUIEnv.Instance.ShowUIModule(uiType, currentActivity.ActivityId);
		}
	}

	private void UpdataActivityIconNotify()
	{
		for (int index = 0; index < activityList.Count; index++)
		{
			UIListItemContainer container = activityList.GetItem(index) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemActivityItem item = container.GetComponent<UIElemActivityItem>();

			var activity = ActivityManager.Instance.GetActivityInRunActivity((int)(item.activityBtn.Data));
			var activityType = activity != null && activity.ActivityData != null ? activity.ActivityData.ActivityType : _ActivityType.Unknown;
			int greenPointType = GreenPointType.UnKnow;

			switch (activityType)
			{
				case _ActivityType.ACCUMULATEACTIVITY: greenPointType = GreenPointType.RunActivityAccumulative; break;
				case _ActivityType.ZENTIA: greenPointType = GreenPointType.ZentiaServerReward; break;
			}

			for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.FunctionStates.Count; i++)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.FunctionStates[i].id == greenPointType)
				{
					item.SetButtonNotify(SysLocalDataBase.Inst.LocalPlayer.FunctionStates[i].isOpen && (activityId != (int)(item.activityBtn.Data)));
					break;
				}
			}

		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAcitivityIcon(UIButton btn)
	{
		this.activityId = (int)(btn.Data as UIElemAssetIcon).Data;
		SetActivityListStage();
	}

	//供子界面调用的方法(获取当前活动的图标节点)
	public UIElemActivityItem GetThisItem()
	{
		for (int index = 0; index < activityList.Count; index++)
		{
			var item = activityList.GetItem(index).Data as UIElemActivityItem;
			if (((int)(item.activityBtn.Data)) == this.activityId)
				return item;
		}

		return null;
	}

	public void SetActivityIconBackLight(int activityId)
	{
		this.activityId = activityId;
	}
}