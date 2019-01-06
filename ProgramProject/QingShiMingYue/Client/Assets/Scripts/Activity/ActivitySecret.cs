using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivitySecret : ActivityBase
{
	public override bool IsActive
	{
		get
		{
			if (!IsOpen)
				return false;

			var nowDateTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
			var startTimes = ActivityInfo.GetTimersByStatus(_ActivityTimerStatus.Start);
			var endTimes = ActivityInfo.GetTimersByStatus(_ActivityTimerStatus.End);

			for (int i = 0; i < startTimes.Count; i++)
			{
				var startTime = KodGames.TimeEx.ToLocalDataTime(startTimes[i].Timer);
				var endTime = KodGames.TimeEx.ToLocalDataTime(endTimes[i].Timer);

				if (nowDateTime >= startTime && nowDateTime <= endTime)
					return true;
			}

			return false;
		}
	}

	public ActivitySecret(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.Secret)
	{
	}
}
