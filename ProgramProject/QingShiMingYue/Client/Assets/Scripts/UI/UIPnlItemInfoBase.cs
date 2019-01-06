using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPnlItemInfoBase : UIModule
{
	public AutoSpriteControlBase blackBg;
	public AutoSpriteControlBase balckTitleBg;
	public AutoSpriteControlBase getWayButton;
	public AutoSpriteControlBase goBackButton;
	public GameObject noClick;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (SysGameStateMachine.Instance.CurrentState is GameState_ActivityDungeon ||
			SysGameStateMachine.Instance.CurrentState is GameState_Dungeon ||
			SysGameStateMachine.Instance.CurrentState is GameState_Tower ||
			SysGameStateMachine.Instance.CurrentState is GameState_WolfSmoke ||
			SysGameStateMachine.Instance.CurrentState is GameState_Battle)
		{
			if (getWayButton != null)
				getWayButton.Hide(true);

			if (blackBg != null)
				blackBg.Hide(false);

			if (balckTitleBg != null)
			{
				if (!SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlMessageFlow)))
					balckTitleBg.Hide(false);
				else
					balckTitleBg.Hide(true);

				//如果是这些界面同样使用最高层显示，那么强制显示最上层挡板
				if (this.ShowLayer == _UILayer.Top)
					balckTitleBg.Hide(false);
			}
		}
		else
		{
			if (getWayButton != null)
				getWayButton.Hide(false);

			// 炼丹时需要显示下面的底板
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanAlchemy)))
			{
				if (blackBg != null)
					blackBg.Hide(false);
			}
			// top层显示的时候表示可以覆盖对话框, 需要显示下面的地板
			else if (blackBg != null)
				blackBg.Hide(this.ShowLayer != _UILayer.Top);
		}

		//判定历练秘境里面是否显示返回按钮
		if (goBackButton != null)
		{
			if (SysGameStateMachine.Instance.CurrentState is GameState_ActivityDungeon ||
				SysGameStateMachine.Instance.CurrentState is GameState_Dungeon)
			{
				goBackButton.Hide(false);
				goBackButton.Text = GameUtility.GetUIString((SysGameStateMachine.Instance.CurrentState is GameState_ActivityDungeon ?
					"UIPnlItemInfoBase_GoBack_Text_02" : "UIPnlItemInfoBase_GoBack_Text_01"));
				if (noClick != null)
					noClick.SetActive(true);
			}
			else
			{
				goBackButton.Hide(true);
				goBackButton.Text = string.Empty;
				if (noClick != null)
					noClick.SetActive(false);
			}
		}

		//判断是否在烽火狼烟里面的场景，如果在，那么单独控制上面面板
		if (SysGameStateMachine.Instance.CurrentState is GameState_WolfSmoke)
			if (balckTitleBg != null)
				balckTitleBg.Hide(this.ShowLayer != _UILayer.Top);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAllUI(UIButton btn)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatar));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectAvatarList)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSelectAvatarList));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectEquipmentList)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSelectEquipmentList));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectSkillList)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSelectSkillList));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSkillInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSkillInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlEquipmentInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlEquipmentInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarPowerUpTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarPowerUpTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarLevelUp)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarLevelUp));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDomineerTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarDomineerTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDomineerTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarDomineerTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarBreakThrough)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarBreakThrough));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarMeridianTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarMeridianTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlHandBook)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlHandBook));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDomineerDescTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDomineerDescTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlEquipmentPowerUpTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlEquipmentPowerUpTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlEquipmentLevelup)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlEquipmentLevelup));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlEquipmentRefine)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlEquipmentRefine));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSkillPowerUp)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSkillPowerUp));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlConsumableInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlConsumableInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanCulture)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanCulture));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectOrganList)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSelectOrganList));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlOrganSelectInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlOrganSelectInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlOrganGrowPage)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlOrganGrowPage));
	}
}