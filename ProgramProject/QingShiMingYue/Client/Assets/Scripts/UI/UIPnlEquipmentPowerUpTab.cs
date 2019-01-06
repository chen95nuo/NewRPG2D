using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEquipmentPowerUpTab : UIPnlItemInfoBase
{
	public List<UIButton> tabBtns;
	private KodGames.ClientClass.Equipment equipment;
	public UIBox levelNotifyActivity;
	public UIBox breakNotifyActivity;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabBtns[0].Data = _UIType.UIPnlEquipmentLevelup;
		tabBtns[1].Data = _UIType.UIPnlEquipmentRefine;

		return true;
	}

	public void SetNotifyState(int type)
	{
		bool isLevelUpUIShow = type == _UIType.UIPnlEquipmentLevelup;

		levelNotifyActivity.Hide(isLevelUpUIShow || !ItemInfoUtility.IsLevelNotifyActivity_Equip(equipment));
		breakNotifyActivity.Hide(!isLevelUpUIShow || !ItemInfoUtility.IsBreakNotifyActivity_Equip(equipment, ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId)));
	}

	public void ChangeTabButtons(int uiType, KodGames.ClientClass.Equipment equipment)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().SetIsOverlaySetPower();

		this.equipment = equipment;
		foreach (UIButton btn in tabBtns)
			btn.controlIsEnabled = ((int)btn.Data) != uiType;

		SetNotifyState(uiType);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuit(UIButton btn)
	{
		foreach (UIButton button in tabBtns)
			if (button.controlIsEnabled == false)
				SysUIEnv.Instance.HideUIModule((int)button.data);

		// Refresh Package Equipment View.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageEquipTab))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageEquipTab>().RefreshView(equipment);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTabChange(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.Data, equipment);
	}
}
