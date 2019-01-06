using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEastSeaExchangeTab : UIModule
{
	public List<UIButton> tabButtons;
	public UIButton backBtn;
	public UIButton goEastSeaBtn;
	public UIBox notifIconBox;

	private int index = 0;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = _UIType.UIPnlEastSeaElementItem;
		tabButtons[1].Data = _UIType.UIPnlEastElementAllServerReward;
		tabButtons[2].Data = _UIType.UIPnlEastElementRankingList;

		// 注册活动绿点数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Activity, UpdataActivityIconNotify);

		return true;
	}


	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;
		SetButtonNotify(false);
		UpdataActivityIconNotify();
		return true;
	}

	private void Update()
	{
		UpdataActivityIconNotify();
	}

	private void UpdataActivityIconNotify()
	{
		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.FunctionStates.Count; i++)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.FunctionStates[i].id == GreenPointType.ZentiaServerReward)
			{
				SetButtonNotify(SysLocalDataBase.Inst.LocalPlayer.FunctionStates[i].isOpen && index != _UIType.UIPnlEastElementAllServerReward);
				break;
			}
		}
	}

	private void SetButtonNotify(bool show)
	{
		if (notifIconBox != null)
			notifIconBox.Hide(!show);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)(btn.Data));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGoEastSeaBtn(UIButton btn)
	{
		for (int index = 0; index < tabButtons.Count; index++)
			if (SysUIEnv.Instance.IsUIModuleShown((int)(tabButtons[index].Data)))
				SysUIEnv.Instance.HideUIModule((int)(tabButtons[index].Data));

		var runActivitys = ActivityManager.Instance.GetActivityIdInRunActivity();
		foreach (var activityid in runActivitys)
		{
			var currentActivity = ActivityManager.Instance.GetActivityInRunActivity(activityid);

			if (currentActivity != null && currentActivity.ActivityData.ActivityType == _ActivityType.ZENTIA)
			{
				if (ActivityZentia.Instance.IsOpen)
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEastSeaFindFairyMain), activityid);
				else
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEastSeaCloseActivity), activityid);

				break;
			}
		}
	}

	public void HidetankingTab(bool isRankOpen)
	{
		tabButtons[2].Hide(!isRankOpen);
	}

	public void SetButtonType(int type)
	{
		//Set TabButton.
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.Data) != type;
		index = type;
		//Set back button.
		if (backBtn != null)
			backBtn.gameObject.SetActive(!ActivityZentia.Instance.IsOpen);
		if (goEastSeaBtn != null)
			goEastSeaBtn.gameObject.SetActive(ActivityZentia.Instance.IsOpen);
	}
}


