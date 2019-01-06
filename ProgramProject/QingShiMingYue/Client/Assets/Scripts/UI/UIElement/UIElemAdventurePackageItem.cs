using UnityEngine;
using System.Collections;
using ClientServerCommon;
public class UIElemAdventurePackageItem : MonoBehaviour
{
	public UIElemAssetIcon packageItem;
	public UIBox getBg;
	public SpriteText timeLabel;
	public UIBox selectLight;

	public com.kodgames.corgi.protocol.DelayReward delayPackage;
	private bool isTimeOut = false;

	public bool IsTimeOut
	{
		get { return isTimeOut; }
	}
	public void SetData(com.kodgames.corgi.protocol.DelayReward delayPackage)
	{
		this.delayPackage = delayPackage;

		if (delayPackage == null)
			return;

		var reward = (MarvellousAdventureConfig.RewardEvent)ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetEventById(delayPackage.eventId);

		packageItem.Data = delayPackage;
		packageItem.SetData(reward.OriginalIconId);
		UpdateDelayPackageItemTime();
	}

	public void SetSelectedStat(bool selected)
	{

		if (delayPackage == null)
			selectLight.Hide(true);
		else
			selectLight.Hide(!selected);
	}

	public void UpdateDelayPackageItemTime()
	{
		if (delayPackage == null)
			return;

		if (SysLocalDataBase.Inst.LoginInfo.NowTime < delayPackage.couldPickTime)
		{
			timeLabel.Text = GameUtility.Time2String(delayPackage.couldPickTime - SysLocalDataBase.Inst.LoginInfo.NowTime);
			getBg.Hide(true);
			isTimeOut = false;
		}
		else
		{
			timeLabel.Text = "";
			getBg.Hide(false);
			isTimeOut = true;
		}
	}
}
