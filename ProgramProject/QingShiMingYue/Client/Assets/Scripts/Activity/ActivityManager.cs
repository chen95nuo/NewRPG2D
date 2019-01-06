#define ENABLE_ACTIVITY_LOG
using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class ActivityBase
{
	protected KodGames.ClientClass.ActivityData activityData;
	public KodGames.ClientClass.ActivityData ActivityData
	{
		get { return activityData; }
	}

	protected List<KodGames.ClientClass.ActivityInfo> activityInfos = new List<KodGames.ClientClass.ActivityInfo>();
	public virtual KodGames.ClientClass.ActivityInfo ActivityInfo
	{
		get
		{
			int indexOfInfos = GetIndexByActiveTime();
			if (indexOfInfos < 0 || indexOfInfos >= activityInfos.Count)
				return null;
			else
				return activityInfos[indexOfInfos];
		}
	}

	public virtual int ActivityId
	{
		get
		{
			if (activityData == null)
				return IDSeg.InvalidId;
			else
				return activityData.ActivityId;
		}
	}

	public virtual bool IsAccessible
	{
		get
		{
			return IsOpen;
		}
	}

	public virtual bool IsOpen
	{
		get
		{
			var activityInfo = ActivityInfo;
			if (activityInfo == null)
				return false;

			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			return (nowTime >= activityInfo.OpenTime && (activityInfo.CloseTime == 0 || nowTime < activityInfo.CloseTime)) &&
				(functionType == _OpenFunctionType.Unknown || GameUtility.CheckFuncOpened(functionType, false, true));
		}
	}

	public virtual bool IsActive
	{
		get
		{
			return IsOpen;
		}
	}

	protected int functionType;

	public ActivityBase(com.kodgames.corgi.protocol.ActivityData activityData, int functionType)
	{
		this.functionType = functionType;
		ResetData(activityData);
	}

	public ActivityBase(com.kodgames.corgi.protocol.ActivityData activityData) : this(activityData, _OpenFunctionType.Unknown) { }

	public ActivityBase(int functionType) : this(null, functionType) { }

	public virtual void ResetData(com.kodgames.corgi.protocol.ActivityData activityData)
	{
		if (activityData == null)
			return;

		if (this.activityData == null)
			this.activityData = new KodGames.ClientClass.ActivityData();

		this.activityData.FromProtobuf(activityData);

		foreach (var info in this.activityData.ActivityInfos)
		{
			foreach (var oldInfo in this.activityInfos)
				if (!oldInfo.IsEqual(info))
				{
					this.activityInfos.Add(info);
					break;
				}

			if (this.activityInfos.Count <= 0)
				this.activityInfos.Add(info);
		}
	}

	public virtual void OnUpdate() { }

	public long NextRefreshTime(long lastRefreshTime)
	{
		int activityInfoIndex = GetIndexByActiveTime();

		if (activityInfoIndex < 0 || activityInfoIndex >= activityInfos.Count)
			return -1;

		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		var startTimes = activityInfos[activityInfoIndex].GetTimersByStatus(_ActivityTimerStatus.Start);
		var refreshTimes = activityInfos[activityInfoIndex].GetTimersByStatus(_ActivityTimerStatus.Refresh);

		// Get RefreshTimes And Sort.
		var refreshList = new List<long>();
		refreshList.Add(activityInfos[activityInfoIndex].OpenTime);

		if (activityInfoIndex < activityInfos.Count - 1)
			refreshList.Add(activityInfos[activityInfoIndex + 1].OpenTime);

		for (int i = 0; i < startTimes.Count; i++)
			refreshList.Add(startTimes[i].Timer);

		for (int i = 0; i < refreshTimes.Count; i++)
			refreshList.Add(refreshTimes[i].Timer);

		refreshList.Sort();

		// Get Refresh Index.
		for (int i = 0; i < refreshList.Count; i++)
		{
			if (refreshList[i] >= nowTime)
			{
				if (i > 0 && lastRefreshTime < refreshList[i - 1])
					return refreshList[i - 1];
				else
					return refreshList[i];
			}
		}

		return -1;
	}

	protected int GetIndexByActiveTime()
	{
		if (activityData == null || activityInfos == null || activityInfos.Count <= 0)
		{
			PrintfLog("GetIndexByActvityTime Error.");
			return -1;
		}

		// ActivityInfo 's CloseTime equal 0 : Activity Opened Forever.
		var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		for (int index = 0; index < activityInfos.Count; index++)
		{
			if (activityInfos[index].OpenTime <= nowTime && (activityInfos[index].CloseTime <= 0 || activityInfos[index].CloseTime > nowTime))
				return index;
			else if (activityInfos[index].OpenTime > nowTime)
				return index;
		}

		return -1;
	}

	protected void PrintfLog(object logMsg)
	{
#if ENABLE_ACTIVITY_LOG
		Debug.Log(logMsg);
#endif
	}
}

public class ActivityManager : SysModule
{
	private List<ActivityBase> activities = new List<ActivityBase>();
	private Dictionary<Type, ActivityBase> type_activityDict = new Dictionary<Type, ActivityBase>();
	private List<ActivitySecret> activitySecrets = new List<ActivitySecret>();
	//运营活动列表
	private Dictionary<int, ActivityBase> id_activityRunDict = new Dictionary<int, ActivityBase>();

	private bool pause = true;
	public bool Pause { set { pause = value; } }

	private const int DEFAULT_ACTIVITY_JUMP_DES = _UIType.UIPnlActivityInnTab;
	private int activityJumpUI;
	public int ActivityJumpUI
	{
		get
		{
			if (IsActivityTabAccessible(activityJumpUI) == false)
				activityJumpUI = DEFAULT_ACTIVITY_JUMP_DES;

			return activityJumpUI;
		}

		set
		{
			activityJumpUI = value;

			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMainMenuBot))
				SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetActivityJumpDest(activityJumpUI);
		}
	}

	private static ActivityManager inst = null;
	public static ActivityManager Instance
	{
		get
		{
			if (inst == null)
				inst = SysModuleManager.Instance.GetSysModule<ActivityManager>();

			return inst;
		}
	}

	public bool IsActive
	{
		get
		{
			for (int i = 0; i < activities.Count; ++i)
				if (activities[i].IsActive)
					return true;

			return false;
		}
	}

	public override void OnUpdate()
	{
		if (pause)
			return;

		base.OnUpdate();

		for (int i = 0; i < activities.Count; ++i)
			activities[i].OnUpdate();
	}

	public bool IsActivityAccessiable(int activityId)
	{
		ActivityBase activityBase = GetActivity(activityId);
		return activityBase != null ? activityBase.IsAccessible : false;
	}

	public bool IsActivityTabAccessible(int uitype)
	{
		switch (uitype)
		{
			case _UIType.UIPnlLevelRewardTab:
				return IsActivityAccessiable(ConfigDatabase.DefaultCfg.LevelRewardConfig.activityId);

			case _UIType.UIPnlActivityInnTab:
				return IsActivityAccessiable(ActivityManager.Instance.GetActivity<ActivityFixedTime>().ActivityId);

			case _UIType.UIPnlActivityQinInfo:
				return ActivityManager.Instance.GetActivity<ActivityQinInfo>().IsAccessible;

			case _UIType.UIPnlActivityMonthCardTab:
				return ActivityManager.Instance.GetActivity<ActivityMonthCard>().IsAccessible;

			case _UIType.UIPnlShopMystery:
				return ActivityManager.Instance.GetActivity<ActivityMysteryer>().IsAccessible;

			case _UIType.UIPnlActivityInvite:
				return ConfigDatabase.DefaultCfg.GameConfig.isInviteCode &&
					ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.InviteCode) <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level &&
					SysLocalDataBase.Inst.LocalPlayer.Function.ShowInviteCode;
			case _UIType.UIPnlActivityFaceBook:
				return ConfigDatabase.DefaultCfg.GameConfig.isShowFacebook &&
					ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.InviteCode) <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level &&
					SysLocalDataBase.Inst.LocalPlayer.Function.FaceBookOpen;
			default:
				return false;
		}
	}

	public bool GetActivityNotifyState(int uiType)
	{
		int notifyType = GreenPointType.UnKnow;

		switch (uiType)
		{
			case _UIType.UIPnlActivityInnTab:
				notifyType = GreenPointType.FixedTimeActivity;
				break;

			case _UIType.UIPnlLevelRewardTab:
				notifyType = GreenPointType.LevelRewardActivity;
				break;

			case _UIType.UIPnlActivityQinInfo:
				notifyType = GreenPointType.QinInfo;
				break;

			case _UIType.UIPnlActivityMonthCardTab:
				notifyType = GreenPointType.MonthCardFeedBack;
				break;

			case _UIType.UIPnlShopMystery:
				notifyType = GreenPointType.MySteryer;
				break;

			case _UIType.UIPnlActivityInvite:
				notifyType = GreenPointType.InviteCodeReward;
				break;
		}

		if (notifyType == GreenPointType.UnKnow)
			return false;

		foreach (var notifyState in SysLocalDataBase.Inst.LocalPlayer.FunctionStates)
		{
			if (notifyState.id == notifyType)
				return notifyState.isOpen;
		}

		return false;
	}

	public void SetData(List<com.kodgames.corgi.protocol.ActivityData> activityDatas)
	{
		activities.Clear();
		type_activityDict.Clear();
		activitySecrets.Clear();
		id_activityRunDict.Clear();
		activityJumpUI = DEFAULT_ACTIVITY_JUMP_DES;

		for (int i = 0; i < activityDatas.Count; i++)
		{
			switch (activityDatas[i].activityType)
			{
				case _ActivityType.GETFIXTEDTIMEACTIVITY:
					AddActivity(new ActivityFixedTime(activityDatas[i]));
					break;
				case _ActivityType.LEVLEREWARDACTIVITY:
					AddActivity(new ActivityLevelReward(activityDatas[i]));
					break;
				case _ActivityType.SECRETACTIVIYT:
					activitySecrets.Add(new ActivitySecret(activityDatas[i]));
					break;
				case _ActivityType.MYSTERYER:
					AddActivity(new ActivityMysteryer(activityDatas[i]));
					break;
				case _ActivityType.ACCUMULATEACTIVITY:
					AddActivityRunDic(new ActivityRun(activityDatas[i]));
					break;

				case _ActivityType.SEVENELEVENGIFT:
					AddActivity(new ActivityConvert(activityDatas[i]));
					break;
				case _ActivityType.ZENTIA:
					AddActivityRunDic(ActivityZentia.GetInstance(activityDatas[i]));
					break;
				case _ActivityType.ALCHEMY:
					AddActivity(new ActivityAlchemy(activityDatas[i]));
					break;

				case _ActivityType.DECOMPOSE:
					AddActivity(new ActivityDecompose(activityDatas[i]));
					break;

				case _ActivityType.GUILDSHOP:
					AddActivity(new ActivityGuildShop(activityDatas[i]));
					break;
			}
		}

		//客户端构造的活动类(永久开启，无时间推送)
		//QinInfo
		AddActivity(new ActivityQinInfo());

		//MonthCard
		AddActivity(new ActivityMonthCard());

		this.pause = false;
	}

	public void ResetData(List<com.kodgames.corgi.protocol.ActivityData> activityDatas)
	{
		for (int i = 0; i < activityDatas.Count; i++)
		{
			ActivityBase activity;

			switch (activityDatas[i].activityType)
			{
				case _ActivityType.GETFIXTEDTIMEACTIVITY:

					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityFixedTime(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);

					type_activityDict[typeof(ActivityFixedTime)] = activity;
					break;

				case _ActivityType.LEVLEREWARDACTIVITY:

					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityLevelReward(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);

					type_activityDict[typeof(ActivityLevelReward)] = activity;

					break;

				case _ActivityType.SECRETACTIVIYT:

					bool hasValue = false;
					for (int j = 0; j < activitySecrets.Count; j++)
					{
						if (activitySecrets[j].ActivityId == activityDatas[i].activityId)
						{
							activitySecrets[j].ResetData(activityDatas[i]);
							hasValue = true;
							break;
						}
					}
					if (!hasValue)
						activitySecrets.Add(new ActivitySecret(activityDatas[i]));
					break;

				case _ActivityType.MYSTERYER:

					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityMysteryer(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);
					break;

				case _ActivityType.SEVENELEVENGIFT:
					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityConvert(activityDatas[i]));
					else
						(type_activityDict[typeof(ActivityConvert)] as ActivityConvert).ResetData(activityDatas[i]);
					break;

				case _ActivityType.ACCUMULATEACTIVITY:

					activity = GetActivityInRunActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivityRunDic(new ActivityRun(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);
					break;

				case _ActivityType.ZENTIA:
					activity = GetActivityInRunActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivityRunDic(ActivityZentia.GetInstance(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);
					break;

				case _ActivityType.ALCHEMY:
					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityAlchemy(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);
					break;

				case _ActivityType.DECOMPOSE:
					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityDecompose(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);
					break;

				case _ActivityType.GUILDSHOP:
					activity = GetActivity(activityDatas[i].activityId);
					if (activity == null)
						AddActivity(new ActivityGuildShop(activityDatas[i]));
					else
						activity.ResetData(activityDatas[i]);

					break;
			}
		}
	}

	private void AddActivity(ActivityBase activity)
	{
		activities.Add(activity);
		type_activityDict.Add(activity.GetType(), activity);
	}

	private void AddActivityRunDic(ActivityBase activity)
	{
		if (activity.ActivityId > 0)
		{
			id_activityRunDict.Add(activity.ActivityId, activity);
			activities.Add(activity);
		}
	}

	public ActivityBase GetActivityInRunActivity(int activityId)
	{
		ActivityBase instance;
		if (id_activityRunDict.TryGetValue(activityId, out instance))
			return instance;

		return null;
	}

	public List<int> GetActivityIdInRunActivity()
	{
		var runActivitys = new List<ActivityBase>();
		var runActivityIds = new List<int>();
		foreach (var runEnv in id_activityRunDict)
		{
			if (runEnv.Value == null)
				continue;

			if (runEnv.Value.IsAccessible)
				runActivitys.Add(runEnv.Value);
		}

		// Sort List.
		runActivitys.Sort((a1, a2) =>
		{
			var config1 = ConfigDatabase.DefaultCfg.GameConfig.GetOperationActivityByType(a1.ActivityData.ActivityType);
			var config2 = ConfigDatabase.DefaultCfg.GameConfig.GetOperationActivityByType(a2.ActivityData.ActivityType);

			return config1.activityIndex - config2.activityIndex;
		});

		foreach (var activity in runActivitys)
			runActivityIds.Add(activity.ActivityId);

		return runActivityIds;
	}

	public ActivityBase GetActivity(int activityId)
	{
		foreach (var activity in type_activityDict)
			if (activity.Value.ActivityId == activityId)
				return activity.Value;

		return null;
	}

	public T GetActivity<T>() where T : ActivityBase
	{
		ActivityBase instance;
		if (type_activityDict.TryGetValue(typeof(T), out instance))
			return instance as T;

		return null;
	}

	public ActivitySecret GetActivitySecret(int activityId)
	{
		foreach (var activity in activitySecrets)
		{
			if (activity.ActivityId == activityId)
				return activity;
		}

		return null;
	}
}
