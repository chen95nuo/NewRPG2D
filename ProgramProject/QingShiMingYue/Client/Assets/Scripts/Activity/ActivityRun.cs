using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityRun : ActivityBase
{
	#region Accumlative Data
	private List<com.kodgames.corgi.protocol.OperationActivityItem> operationActivityItems;
	public List<com.kodgames.corgi.protocol.OperationActivityItem> OperationActivityItems
	{
		get { return operationActivityItems; }
		set { operationActivityItems = value; }
	}

	private int accumulateMoney;
	public int AccumulateMoney
	{
		get { return accumulateMoney; }
		set { accumulateMoney = value; }
	}

	private int accumulateIndex;
	public int AccumulateIndex
	{
		get { return accumulateIndex; }
		set { accumulateIndex = value; }
	}

	private long rechargeStart;
	public long RechargeStart
	{
		get { return rechargeStart; }
		set { rechargeStart = value; }
	}

	private long rechargeEnd;
	public long RechargeEnd
	{
		get { return rechargeEnd; }
		set { rechargeEnd = value; }
	}

	private long rewardStart;
	public long RewardStart
	{
		get { return rewardStart; }
		set { rewardStart = value; }
	}

	private long rewardEnd;
	public long RewardEnd
	{
		get { return rewardEnd; }
		set { rewardEnd = value; }
	}

	#endregion

	public ActivityRun(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.Unknown)
	{
	}

	public com.kodgames.corgi.protocol.OperationActivityItem GetOperationItemById(int operationId)
	{
		if (this.operationActivityItems != null && this.operationActivityItems.Count > 0)
		{
			for (int index = 0; index < this.operationActivityItems.Count; index++)
			{
				if (this.operationActivityItems[index].itemId == operationId)
				{
					return this.operationActivityItems[index];
				}
			}
		}

		return null;
	}

	public override void ResetData(com.kodgames.corgi.protocol.ActivityData activityData)
	{
		base.ResetData(activityData);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlRunAccumulativeTab))
			SysUIEnv.Instance.GetUIModule<UIPnlRunAccumulativeTab>().ResetWaitForResValue();
	}
}