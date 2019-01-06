using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityGuildShop : ActivityBase
{
	public ActivityGuildShop(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.Unknown)
	{

	}

	public override void ResetData(com.kodgames.corgi.protocol.ActivityData activityData)
	{
		base.ResetData(activityData);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildShopActivity))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().ResetWaitControlValue();
	}
}

