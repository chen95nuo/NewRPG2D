using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemCentralCityStartServerItem : UIElemCentralCityTempItem
{
	public override void Init()
	{
		SetData(this, "OnTempButtonClick", _UIType.UIPnlStartServerReward);

		base.Init();
	}

	public override void Update()
	{
		if (SysLocalDataBase.Inst.LocalPlayer == null)
			return;

		if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo == null || SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.UnPickIds.Count <= 0)
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysHide);
		else
			ResetDisplayFactor(UIElemCentralCityTempItem.ElemShowFactor.AlwaysShow);

		base.Update();
	}

	public override bool ShowPartical()
	{
		bool hasUnPickedReward = false;

		if (ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.StartServerReward) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			return hasUnPickedReward;

		if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo != null)
		{
			foreach (var serverRewardID in SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.UnPickIds)
				if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.DayCount >= ConfigDatabase.DefaultCfg.StartServerRewardConfig.GetStartServerRewardByID(serverRewardID).day)
				{
					hasUnPickedReward = true;
					break;
				}
		}

		return hasUnPickedReward;
	}
}