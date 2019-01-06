using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityMysteryer : ActivityBase
{
	//界面上的绿点是否显示
	public override bool IsActive
	{
		get
		{
			if (!IsOpen)
				return false;

			foreach (var state in SysLocalDataBase.Inst.LocalPlayer.FunctionStates)
				if (state.id == GreenPointType.MySteryer && state.isOpen)
					return true;

			return false;
		}
	}

	public ActivityMysteryer(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.MysteryShop)
	{
	}

	public override void ResetData(com.kodgames.corgi.protocol.ActivityData activityData)
	{
		base.ResetData(activityData);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopMystery))
			SysUIEnv.Instance.GetUIModule<UIPnlShopMystery>().ResetWaitControlValue();
	}
}