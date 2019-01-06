using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityConvert : ActivityBase
{
	public override bool IsActive
	{
		get
		{
			return IsOpen;
		}
	}

	public override bool IsOpen
	{
		get
		{
			var activityInfo = ActivityInfo;
			if (activityInfo == null)
				return false;

			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			return SysLocalDataBase.Inst.LocalPlayer.Function.ShowSevenElevenGift &&
				(nowTime >= activityInfo.OpenTime && (activityInfo.CloseTime == 0 || nowTime < activityInfo.CloseTime)) &&
				(functionType == _OpenFunctionType.Unknown || GameUtility.CheckFuncOpened(functionType, false, true));
		}
	}

	public ActivityConvert(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.Unknown)
	{
	}
}
