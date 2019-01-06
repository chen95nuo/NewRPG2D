using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIElemAdventureDelayPackageProcessor : UIListItemContainerEx
{
	private UIElemAdventurePackageItem uiItem;

	public com.kodgames.corgi.protocol.DelayReward delayPackage;

	private bool isTimeOut = false;

	public bool IsTimeOut
	{
		get { return isTimeOut; }
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemAdventurePackageItem>();
			SetData(this.delayPackage);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
		{
			uiItem = null;
		}

		base.OnDisabled();
	}

	public void SetData(com.kodgames.corgi.protocol.DelayReward delayPackage)
	{
		this.delayPackage = delayPackage;

		if (uiItem == null || delayPackage == null)
			return;

		var reward = (MarvellousAdventureConfig.RewardEvent)ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetEventById(delayPackage.eventId);

		uiItem.packageItem.Data = delayPackage;
		uiItem.packageItem.SetData(reward.OriginalIconId);
		UpdateDelayPackageItemTime();
	}

	public void SetSelectedStat(bool selected)
	{
		if (uiItem == null)
			return;
		if (delayPackage == null)
			uiItem.selectLight.Hide(true);
		else
			uiItem.selectLight.Hide(!selected);
	}

	public void UpdateDelayPackageItemTime()
	{
		if (uiItem == null || delayPackage == null)
			return;

		if (SysLocalDataBase.Inst.LoginInfo.NowTime < delayPackage.couldPickTime)
		{
			uiItem.timeLabel.Text = GameUtility.Time2String(delayPackage.couldPickTime - SysLocalDataBase.Inst.LoginInfo.NowTime);
			uiItem.getBg.Hide(true);
			isTimeOut = false;
		}
		else
		{
			uiItem.timeLabel.Text = "";
			uiItem.getBg.Hide(false);
			isTimeOut = true;
		}
	}
}