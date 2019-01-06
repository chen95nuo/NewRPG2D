using UnityEngine;
using System;

namespace KodGames
{
	/// <summary>
	/// UnityEngin.Time.realtimeSinceStartup was not updated when system locked on iOS, use System.DateTime instead
	/// </summary>
	public static class TimeEx
	{
		// Should always be same with ClientServerCommon._TimeDurationType
		public class _TimeDurationType
		{
			public const int Unknown = 0;
			public const int Era = 1;
			public const int Year = 2;
			public const int Month = 3;
			public const int Day = 4;
			public const int Hour = 5;
			public const int Minute = 6;
			public const int Second = 7;
			public const int Week = 8;
		}

		public static void Initialize()
		{
#if !UNITY_EDITOR && UNITY_ANDROID
			if (initialized == false)
				startUpTime = System.DateTime.Now.AddMilliseconds(-Time.realtimeSinceStartup * 1000);
#endif
			initialized = true;

			Debug.Log("TimeZoneOffset " + LocalZoneOffset);
		}

		private static bool initialized = false;
#if !UNITY_EDITOR && UNITY_ANDROID
		private static System.DateTime startUpTime;
#endif

		public static int LocalZoneOffset
		{
			get { return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours; }
		}

		public static float realtimeSinceStartup
		{
			get
			{
				if (initialized == false)
				{
					Debug.LogError("TimeEx has not been initialized");
					return 0;
				}

#if !UNITY_EDITOR && UNITY_ANDROID
				// Android设备上面Time.realtimeSinceStartup不会计算后台时间
				return (float)((System.DateTime.Now - startUpTime).TotalSeconds);
#else
				return Time.realtimeSinceStartup;
#endif
			}
		}

		public static DateTime GetNextWeekTimeToMaxNowTime(DateTime refreshTime)
		{
			if (refreshTime < SysLocalDataBase.Inst.LoginInfo.NowDateTime)
				return GetNextWeekTimeToMaxNowTime(refreshTime.AddDays(7));
			return refreshTime;
		}

		public static System.DateTime ToUTCDateTime(long time)
		{
			System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			System.DateTime utcDate = origin.AddMilliseconds(time);
			return utcDate;
		}

		public static System.DateTime ToLocalDataTime(long time)
		{
			System.DateTime origin = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			System.DateTime utcDate = origin.AddMilliseconds(time);
			return utcDate.ToLocalTime();
		}

		public static long DateTimeToInt64(System.DateTime dateTime)
		{
			System.DateTime epoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			return (dateTime.Ticks - epoch.Ticks) / System.TimeSpan.TicksPerMillisecond;
		}

		public const int cMillisecondInSecend = 1000;
		public const int cSecondInMinute = 60;
		public const int cMinuteInHour = 60;
		public const int cHourInDay = 24;
		public const int cMillisecondInMinute = cMillisecondInSecend * cSecondInMinute;
		public const int cMillisecondInHour = cMillisecondInMinute * cMinuteInHour;
		public const int cMillisecondInDay = cMillisecondInHour * cHourInDay;

		/// <summary>
		/// Check if time is between times
		/// </summary>
		public static bool IsInTimeSpan(System.DateTime time, System.DateTime from, System.DateTime to, int timeDurationType)
		{
			long ms, fromMS, toMS;
			switch (timeDurationType)
			{
				case _TimeDurationType.Year:
					ms = ((int)time.DayOfYear) * cMillisecondInDay + time.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					fromMS = ((int)from.DayOfYear) * cMillisecondInDay + from.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					toMS = ((int)to.DayOfYear) * cMillisecondInDay + to.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					break;

				case _TimeDurationType.Month:
					ms = ((int)time.Day) * cMillisecondInDay + time.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					fromMS = ((int)from.Day) * cMillisecondInDay + from.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					toMS = ((int)to.Day) * cMillisecondInDay + to.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					break;

				case _TimeDurationType.Day:
					ms = time.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					fromMS = from.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					toMS = to.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					break;

				case _TimeDurationType.Week:
					ms = ((int)time.DayOfWeek) * cMillisecondInDay + time.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					fromMS = ((int)from.DayOfWeek) * cMillisecondInDay + from.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					toMS = ((int)to.DayOfWeek) * cMillisecondInDay + to.TimeOfDay.Ticks / TimeSpan.TicksPerMillisecond;
					break;

				default:
					ms = time.Ticks / TimeSpan.TicksPerMillisecond;
					fromMS = from.Ticks / TimeSpan.TicksPerMillisecond;
					toMS = to.Ticks / TimeSpan.TicksPerMillisecond;
					break;
			}

			return ms > fromMS && ms <= toMS;
		}

		public static bool IsInSameTimeSpan(System.DateTime time1, System.DateTime time2, System.DateTime from, System.DateTime to, int timeDurationType)
		{
			if (IsInTimeSpan(time1, from, to, timeDurationType) == false || IsInTimeSpan(time2, from, to, timeDurationType) == false)
				return false;

			var fromTime1 = GetTimeBeforeTime(from, time1, timeDurationType);
			var toTime1 = GetTimeAfterTime(to, time1, timeDurationType);
			return time2 > fromTime1 && time2 <= toTime1;
		}

		/// <summary>
		/// Get previous time by duration.
		/// </summary>
		public static System.DateTime GetTimeBeforeTime(System.DateTime time, System.DateTime before, int timeDurationType)
		{
			switch (timeDurationType)
			{
				case _TimeDurationType.Year:
					time = new System.DateTime(before.Year, before.Month, before.Day).AddTicks(time.TimeOfDay.Ticks).AddDays(time.DayOfYear - before.DayOfYear);
					if (time.Ticks > before.Ticks)
						time = time.AddYears(-1);
					break;

				case _TimeDurationType.Month:
					time = new System.DateTime(before.Year, before.Month, before.Day).AddTicks(time.TimeOfDay.Ticks).AddDays(time.Day - before.Day);
					if (time.Ticks > before.Ticks)
						time = time.AddMonths(-1);
					break;

				case _TimeDurationType.Day:
					time = new System.DateTime(before.Year, before.Month, before.Day).AddTicks(time.TimeOfDay.Ticks);
					if (time.Ticks >= before.Ticks)
						time = time.AddDays(-1);
					break;

				case _TimeDurationType.Week:
					time = new System.DateTime(before.Year, before.Month, before.Day).AddTicks(time.TimeOfDay.Ticks).AddDays(time.DayOfWeek - before.DayOfWeek);
					if (time.Ticks > before.Ticks)
						time = time.AddDays(-7);
					break;
			}

			return time;
		}

		/// <summary>
		/// Get next time by duration.
		/// </summary>
		public static System.DateTime GetTimeAfterTime(System.DateTime time, System.DateTime after, int timeDurationType)
		{
			switch (timeDurationType)
			{
				case _TimeDurationType.Year:
					time = new System.DateTime(after.Year, after.Month, after.Day).AddTicks(time.TimeOfDay.Ticks).AddDays(time.DayOfYear - after.DayOfYear);
					if (time.Ticks <= after.Ticks)
						time = time.AddYears(1);
					break;

				case _TimeDurationType.Month:
					time = new System.DateTime(after.Year, after.Month, after.Day).AddTicks(time.TimeOfDay.Ticks).AddDays(time.Day - after.Day);
					if (time.Ticks <= after.Ticks)
						time = time.AddMonths(1);
					break;

				case _TimeDurationType.Day:
					time = new System.DateTime(after.Year, after.Month, after.Day).AddTicks(time.TimeOfDay.Ticks);
					if (time.Ticks <= after.Ticks)
						time = time.AddDays(1);
					break;

				case _TimeDurationType.Week:
					time = new System.DateTime(after.Year, after.Month, after.Day).AddTicks(time.TimeOfDay.Ticks).AddDays(time.DayOfWeek - after.DayOfWeek);
					if (time.Ticks <= after.Ticks)
						time = time.AddDays(7);
					break;
			}

			return time;
		}
	}
}
