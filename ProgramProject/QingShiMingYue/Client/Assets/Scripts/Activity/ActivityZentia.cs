using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class ActivityZentia : ActivityBase
{
	public override bool IsAccessible
	{
		get
		{
			return true;
		}
	}

	private static ActivityZentia inst = null;
	public static ActivityZentia Instance
	{
		get
		{
			if (inst == null)
				inst = new ActivityZentia(null);
			return inst;
		}
	}

	private ActivityZentia(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData)
	{
	}

	public static ActivityZentia GetInstance(com.kodgames.corgi.protocol.ActivityData activityData)
	{
		if (inst == null)
			inst = new ActivityZentia(activityData);
		return inst;
	}
}