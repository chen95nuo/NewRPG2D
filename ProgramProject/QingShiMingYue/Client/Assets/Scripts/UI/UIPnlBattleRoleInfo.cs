using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlBattleRoleInfo : UIModule
{
	public UIElemBattleRoleInfo roleInfo;

	private bool canShowRoleInfo = false;

	public void SetCanShowRoleInfo(bool canShowRoleInfo)
	{
		this.canShowRoleInfo = canShowRoleInfo;
	}

	public override void Dispose()
	{
		// Invoke Hide will remove Listener from UIManager
		this.gameObject.GetComponent<UIBistateInteractivePanel>().Hide();

		base.Dispose();
	}

	public override void OnHide()
	{
		base.OnHide();
		// Invoke Hide will remove Listener from UIManager
		this.gameObject.GetComponent<UIBistateInteractivePanel>().Hide();
	}

	public void Hide()
	{
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickRole(UIButton3D btn)
	{
		Debug.Log("OnClickRole");
		if (!canShowRoleInfo)
		{
			return;
		}

		BattleRole role = btn.Data as BattleRole;
		if (role == null)
		{
			return;
		}

		//看不见的角色不能点
		if (role.Hide)
			return;

		this.gameObject.GetComponent<UIBistateInteractivePanel>().Reveal();
		this.roleInfo.SetData(role);
	}
}
