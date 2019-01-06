using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPackageCapacity : MonoBehaviour
{
	public UIBox capacityBg;
	public SpriteText capacityLabel;

	private int currentCount = 0;
	private int maxCount = 0;

	public static int TotalCount { get { return GameUtility.GetPackageItemCount(); } }

	public static int TotalCapacity
	{
		get
		{
			return ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.PackageCapacity);
		}
	}

	public bool NeedRefresh
	{
		get
		{
			return (currentCount != UIElemPackageCapacity.TotalCount || maxCount != UIElemPackageCapacity.TotalCapacity);
		}
	}

	public void Refresh()
	{
		currentCount = UIElemPackageCapacity.TotalCount;
		maxCount = UIElemPackageCapacity.TotalCapacity;

		capacityLabel.Text = GameUtility.FormatUIString("UIPnlPackageTab_Capacity", currentCount, maxCount);
		capacityBg.SetSize(capacityLabel.GetWidth(capacityLabel.Text) + 16f, capacityBg.height);
	}
}
