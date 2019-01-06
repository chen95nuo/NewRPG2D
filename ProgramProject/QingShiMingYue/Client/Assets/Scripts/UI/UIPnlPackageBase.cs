using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageBase : UIModule 
{
	public UIElemPackageCapacity packageCap;

	protected DateTime nextRefreshUITime;

	protected void Update()
	{
		RefreshUI(SysLocalDataBase.Inst.LoginInfo.NowDateTime);
	}

	protected void RefreshUI(DateTime nowTime)
	{
		if (nowTime < nextRefreshUITime)
			return;

		if (packageCap.NeedRefresh)
			packageCap.Refresh();

		nextRefreshUITime = nowTime.AddSeconds(1);
	}

	protected virtual void InitData()
	{
		nextRefreshUITime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
	}
}
