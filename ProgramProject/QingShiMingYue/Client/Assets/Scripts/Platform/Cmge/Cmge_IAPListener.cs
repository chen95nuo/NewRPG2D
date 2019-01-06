using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ClientServerCommon;
using KodGames.ExternalCall;
using KodGames.ExternalCall.Platform;
using LitJson;

public class Cmge_IAPListener : IAPListener
{
	public override bool PayProduct(string productID, int goodsID, int count, int callback, string additionalData)
	{
		CmgePlugin.Pay(SysLocalDataBase.Inst.LocalPlayer.Name,
						SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString(),
						SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID.ToString(),
						SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaName,
						GetPartnerOrderId(productID, additionalData),
						productID,
						ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodsID, GameUtility.GetDeviceInfo().DeviceType).costRMB);  //  需要确认下feePointId是不是就是productId

		return true;
	}
}