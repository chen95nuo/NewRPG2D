using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTarven : UIModule
{
	public GameObject firstRechargeButton;
	public UIButton rechargeButton;
	public List<UIButton> tabButtons;
	public UIButton closeButton;
	public UIBox activityNotify;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// 注册活动按钮绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Unknown, UpdateActivityNotify);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		UpdateActivityNotify();
		InitTabBtns();

		return true;
	}

	private void InitTabBtns()
	{
		if (tabButtons.Count > 0)
			tabButtons[0].Data = _UIType.UIPnlShopWine;

		if (tabButtons.Count > 1)
			tabButtons[1].Data = _UIType.UIPnlShopMystery;
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消活动按钮绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Unknown, UpdateActivityNotify);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabBtnClick(UIButton btn)
	{
		if ((int)btn.Data == _UIType.UIPnlShopMystery)
		{
			if (ActivityManager.Instance == null || ActivityManager.Instance.GetActivity<ActivityMysteryer>() == null || !ActivityManager.Instance.GetActivity<ActivityMysteryer>().IsOpen)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlShopMystery_Tip_NoActivity"));
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlShopMystery);
		}
		else
			SysUIEnv.Instance.ShowUIModule((int)btn.Data);
	}

	public void SetSelectedBtn(int lnkUI)
	{
		for (int index = 0; index < tabButtons.Count; index++)
		{
			bool enableBtn = (lnkUI != (int)tabButtons[index].Data);

			if (tabButtons[index].controlIsEnabled != enableBtn)
				tabButtons[index].controlIsEnabled = enableBtn;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void CloseSelf(UIButton btn)
	{
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UnKonw);
		if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMainScene))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
	}

	public void UpdateActivityNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
		{
			bool showNotify = false;
			if (ActivityManager.Instance != null)
				if (ActivityManager.Instance.GetActivity<ActivityMysteryer>() != null)
					showNotify = ActivityManager.Instance.GetActivity<ActivityMysteryer>().IsActive;

			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopMystery))
				showNotify = false;

			activityNotify.Hide(!showNotify);
		}
	}
}
