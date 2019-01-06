using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlBattleTab : UIModule
{
	public UIScrollList scrollList;

	public List<UIButton> tabButtons;
	public List<UIBox> tabLightButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = _UIType.UIPnlRobSkill;
		tabLightButtons[0].Data = _UIType.UIPnlRobSkill;

		tabButtons[1].Data = _UIType.UIPnlPveBlankBoard;
		tabLightButtons[1].Data = _UIType.UIPnlPveBlankBoard;

		tabButtons[2].Data = _UIType.UIPnlWorldBoss;
		tabLightButtons[2].Data = _UIType.UIPnlWorldBoss;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		AddSubButton();

		SetDefalutLight();

		return true;
	}

	public override void OnHide()
	{
		// Clear ScrollList 's items.
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;

		// ReSet the Scroll Item 's Parent.
		foreach (var box in tabLightButtons)
			box.CachedTransform.parent = scrollList.CachedTransform;

		base.OnHide();
	}

	private void AddSubButton()
	{
		scrollList.AddItem(tabLightButtons[0].gameObject);
		scrollList.AddItem(tabLightButtons[1].gameObject);
	}

	private void SetDefalutLight()
	{
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlRobSkill);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlRobSkill))
			SetLight(_UIType.UIPnlRobSkill);
		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlWorldBoss))
			SetLight(_UIType.UIPnlWorldBoss);
		else
			SetLight(_UIType.UIPnlPveBlankBoard);
	}

	public void SetLight(int uitype)
	{
		foreach (var button in tabButtons)
			button.controlIsEnabled = ((int)button.Data) != uitype;

		foreach (var box in tabLightButtons)
			box.Hide((int)box.Data != uitype);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickTabButton(UIButton btn)
	{
		GameUtility.JumpUIPanel((int)btn.Data);
	}
}
