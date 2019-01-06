using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;
using KodGames;

public class ActivityAlchemy : ActivityBase
{
	public string GetActivityTime()
	{
		var activityInfo = ActivityInfo;
		if (activityInfo != null)
			return TimeEx.ToLocalDataTime(activityInfo.OpenTime).ToString("yyyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "-" + TimeEx.ToLocalDataTime(activityInfo.CloseTime).ToString("yyyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

		return "";
	}

	public ActivityAlchemy(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.Unknown)
	{

	}
}
