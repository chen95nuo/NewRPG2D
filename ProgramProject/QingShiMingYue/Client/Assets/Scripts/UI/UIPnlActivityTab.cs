using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlActivityTab : UIModule
{

	public UIScrollList tabScrollList;
	public List<GameObjectPool> pools;

	private int[] uitypes = {
					_UIType.UIPnlActivityInnTab,
					_UIType.UIPnlLevelRewardTab,
					_UIType.UIPnlActivityQinInfo,
					_UIType.UIPnlActivityMonthCardTab,
					_UIType.UIPnlActivityInvite,
					_UIType.UIPnlActivityFaceBook
							};
	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// 注册活动绿点数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Unknown, SetActivityTabNotify);

		return true;
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消注册活动绿点数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Unknown, SetActivityTabNotify);
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(ActivityManager.Instance.ActivityJumpUI);

		ClearData();
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");

		// Clear ScrollList 's items.
		tabScrollList.ClearList(false);
		tabScrollList.ScrollPosition = 0f;


	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		int itemIndex = 0;

		for (int index = 0; index < pools.Count; index++)
		{
			//跳过客栈  客栈图标始终显示
			if (index == 0 || ActivityManager.Instance.IsActivityTabAccessible(uitypes[index]))
			{
				var container = pools[index].AllocateItem().GetComponent<UIListItemContainer>();
				UIElemNotifAndBgLightItem nabItem = container.gameObject.GetComponent<UIElemNotifAndBgLightItem>();
				itemIndex = ActivityManager.Instance.ActivityJumpUI == uitypes[index] ? index : 0;
				nabItem.SetData(uitypes[index], ActivityManager.Instance.ActivityJumpUI == uitypes[index]);
				tabScrollList.AddItem(container.gameObject);
			}
		}
		tabScrollList.ScrollToItem(itemIndex, 0);
		SetActivityTabNotify();
	}

	/// <summary>
	/// When a "UIPnlActivityxxxxTab" is called to show
	///   this function must be called
	///   to ensure the right tab button is in selected status
	/// </summary>
	/// <param name="type">The _UIType value of the caller</param>
	public void SetActiveButton(int type)
	{
		for (int index = 0; index < tabScrollList.Count; index++)
		{
			UIElemNotifAndBgLightItem nabItem = tabScrollList.GetItem(index).gameObject.GetComponent<UIElemNotifAndBgLightItem>();
			int uitype = (int)nabItem.btn.Data;
			bool isSameType = (uitype == type);
			nabItem.btn.controlIsEnabled = !isSameType;
			nabItem.bg.Hide(!isSameType);
		}
		tabScrollList.RepositionItems();

		ActivityManager.Instance.ActivityJumpUI = type;
	}

	private void SetActivityTabNotify()
	{
		for (int index = 0; index < tabScrollList.Count; index++)
		{
			UIListItemContainer container = tabScrollList.GetItem(index) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemNotifAndBgLightItem nabItem = container.gameObject.GetComponent<UIElemNotifAndBgLightItem>();
			UIBox bgBox = container.gameObject.GetComponent<UIBox>();

			if (nabItem != null && bgBox != null)
				nabItem.SetActive(ActivityManager.Instance.GetActivityNotifyState((int)(tabScrollList.GetItem(index).gameObject.GetComponentInChildren<UIButton>().Data)));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnActivityButtonClick(UIButton btn)
	{
		if ((int)(btn.Data) != _UIType.UIPnlActivityInvite)
			SysUIEnv.Instance.ShowUIModule((int)btn.Data);
		else
		{
			if (ActivityManager.Instance.IsActivityTabAccessible(_UIType.UIPnlActivityInvite))
				RequestMgr.Inst.Request(new QueryInviteCodeInfoReq());
			else
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityInvite_Level_Count"));
		}
	}
}