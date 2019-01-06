using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityMonthCard : ActivityBase
{
	public override int ActivityId
	{
		get { return -1; }
	}

	private List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos;
	public List<com.kodgames.corgi.protocol.OneMonthCardInfo> MonthCardInfos
	{
		get { return monthCardInfos; }
		set { monthCardInfos = value; }
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

			for (int i = 0; i < monthCardInfos.Count; i++)
			{
				if (monthCardInfos[i].pickCounter >= 10 ||
					monthCardInfos[i].buyRewardCount > 0 ||
					monthCardInfos[i].isCouldPickDailyReward)
					return true;
			}

			return false;
		}
	}

	public ActivityMonthCard()
		: base(_OpenFunctionType.MonthCardFeedback)
	{
		this.monthCardInfos = new List<com.kodgames.corgi.protocol.OneMonthCardInfo>();
	}
}
