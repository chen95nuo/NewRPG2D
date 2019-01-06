using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class ActivityLevelReward : ActivityBase
{
	public override bool IsActive
	{
		get
		{
			if (!IsOpen)
				return false;

			if (this.Config.upgradeRewards == null
				|| this.Config.upgradeRewards.Count <= 0
				|| this.levelRewards == null
				|| this.levelRewards.Count <= 0)
				return false;

			for (int index = 0; index < this.Config.upgradeRewards.Count && index < this.LevelRewards.Count; index++)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= this.Config.upgradeRewards[index].level
					&& this.LevelRewards[index].State == _LevelRewardStateType.Available)
					return true;
			}
			return false;
		}

	}

	public LevelRewardConfig Config
	{
		get { return ConfigDatabase.DefaultCfg.LevelRewardConfig; }
	}

	private List<KodGames.ClientClass.LevelReward> levelRewards;
	public List<KodGames.ClientClass.LevelReward> LevelRewards
	{
		get { return levelRewards; }
		set { levelRewards = value; }
	}

	public ActivityLevelReward(com.kodgames.corgi.protocol.ActivityData activityData)
		: base(activityData, _OpenFunctionType.LevelRewardActivity)
	{
	}
}
