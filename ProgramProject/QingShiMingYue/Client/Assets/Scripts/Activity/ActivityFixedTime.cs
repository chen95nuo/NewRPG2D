using UnityEngine;
using ClientServerCommon;

public class ActivityFixedTime : ActivityBase
{
	public override bool IsActive
	{
		get
		{
			if (!IsOpen)
				return false;

			return CheckFixedTime(SysLocalDataBase.Inst.LoginInfo.NowDateTime, true);
		}
	}

	// 判定1：Time是否在时间区间内 
	// 判定2：判定1成立时，进行判定上次休息时间(checkLastGetTime控制是否判定）
	public bool CheckFixedTime(System.DateTime time, bool checkLastGetTime)
	{
		var activitInfo = ActivityInfo;
		var startTimes = activitInfo.GetTimersByStatus(_ActivityTimerStatus.Start);
		var endTimes = activitInfo.GetTimersByStatus(_ActivityTimerStatus.End);

		for (int index = 0; index < startTimes.Count; index++)
		{
			var startTime = KodGames.TimeEx.ToLocalDataTime(startTimes[index].Timer);
			var endTime = KodGames.TimeEx.ToLocalDataTime(endTimes[index].Timer);

			if (KodGames.TimeEx.IsInTimeSpan(time, startTime, endTime, restTimeType) &&
				(checkLastGetTime == false || !KodGames.TimeEx.IsInSameTimeSpan(time, SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(lastGetTime), startTime, endTime, restTimeType)))
				return true;
		}

		return false;
	}

	// 客栈功能数据:上次领取时间
	private long lastGetTime;
	public long LastGetTime
	{
		get { return lastGetTime; }
		set { lastGetTime = value; }

	}

	// 客栈功能数据:重置类型
	private int restTimeType;
	public int RestTimeType
	{
		get { return restTimeType; }
		set { restTimeType = value; }
	}

	public ActivityFixedTime(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.FixedTimeActivity)
	{

	}
}