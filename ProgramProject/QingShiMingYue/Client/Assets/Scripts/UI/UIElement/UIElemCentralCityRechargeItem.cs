using UnityEngine;
using ClientServerCommon;
using System.Collections;
using System.Collections.Generic;

public class UIElemCentralCityRechargeItem : UIElemCentralCityTempItem
{
	public override void Init()
	{
		SetData(this, "OnTempButtonClick", _UIType.UIPnlRecharge);

		if (SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB > 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysShow);
		else
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysHide);

		base.Init();
	}

	public override void Update()
	{
		if (SysLocalDataBase.Inst.LocalPlayer == null)
			return;

		if (IsHidden && SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB > 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysShow);
		else if (!IsHidden && SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB <= 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysHide);

		base.Update();
	}
}