using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageTab : UIModule
{
	public List<UIButton> tabButtons;
	public UIBox capacityBg;

	public AutoSpriteControlBase blackBg;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].data = _UIType.UIPnlPackageItemTab;
		tabButtons[1].data = _UIType.UIPnlPackageAvatarTab;
		tabButtons[2].data = _UIType.UIPnlPackageEquipTab;
		tabButtons[3].data = _UIType.UIPnlPackageSkillTab;

		return true;
	}

	private int TotalCount()
	{
		int result = 0;

		List<string> guids = new List<string>();
		foreach (var s in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if (!guids.Contains(s.Guid) && ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s.ResourceId).type == CombatTurn._Type.PassiveSkill)
			{
				result++;
				guids.Add(s.Guid);
			}
		}

		List<int> resourceids = new List<int>();
		foreach (var c in SysLocalDataBase.Inst.LocalPlayer.Consumables)
		{
			if (!resourceids.Contains(c.Id) && c.Amount > 0)
			{
				result++;
				resourceids.Add(c.Id);
			}
		}

		result += SysLocalDataBase.Inst.LocalPlayer.Avatars.Count;
		result += SysLocalDataBase.Inst.LocalPlayer.Equipments.Count;

		return result;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;

		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlPackageItemTab);

		//当不再主场景显示的背包界面需要隐藏菜单栏
		blackBg.gameObject.SetActive(false);

		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower ||
			SysGameStateMachine.Instance.CurrentState is GameState_WolfSmoke)
				blackBg.gameObject.SetActive(true);		

		LogPackageCapacity();

		return false;
	}

	private void LogPackageCapacity()
	{
		KodGames.ClientClass.Player localDB = SysLocalDataBase.Inst.LocalPlayer;

		// Skip the superSkill.
		int skillCount = 0;
		foreach (var skill in localDB.Skills)
		{
			SkillConfig.Skill skillConfig = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
			if (skillConfig == null)
			{
				Debug.LogError("Skill " + skill.ResourceId.ToString("X") + " in SkillConfig is not found.");
				continue;
			}
			else if (skillConfig.type == ClientServerCommon.CombatTurn._Type.PassiveSkill)
				skillCount++;
		}
	}

	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != uiType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClickBtn(UIButton btn)
	{
		HideSelf();
		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower && !SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerScene))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTowerScene);

		if (SysGameStateMachine.Instance.CurrentState is GameState_WolfSmoke && !SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlWolfInfo))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlWolfInfo);
	}
}

