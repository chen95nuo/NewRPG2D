using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityQinInfo : ActivityBase
{
	// this activity will not be closed

	public override int ActivityId
	{
		get { return -1; }
	}

	public override bool IsOpen
	{
		get
		{
			return GameUtility.CheckFuncOpened(this.functionType, false, true);
		}
	}

	public override bool IsActive
	{
		get
		{
			if (!IsOpen)
				return false;

			return SysLocalDataBase.Inst.LocalPlayer.QinInfoAnswerCount.Point.Value > 0;
		}
	}

	public ActivityQinInfo()
		: base(_OpenFunctionType.QinInfo)
	{
	}
}
