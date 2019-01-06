using System;
using System.Collections.Generic;
using ClientServerCommon;

class UIElemCentralCityFirstRechargeItem : UIElemCentralCityTempItem
{
	public override void Init()
	{
		SetData(this, "OnTempButtonClick", _UIType.UIPnlRecharge);

		if (SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB <= 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysShow);
		else
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysHide);

		base.Init();
	}

	public override void Update()
	{
		if (SysLocalDataBase.Inst.LocalPlayer == null)
			return;

		if (IsHidden && SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB <= 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysShow);
		else if (!IsHidden && SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB > 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysHide);

		base.Update();
	}

	public override bool ShowPartical()
	{
		return SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB <= 0;
	}

	protected override string ParticalName()
	{
		return GameDefines.centralCityFirstRecharge;
	}
}