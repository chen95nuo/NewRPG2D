using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ClientServerCommon;
using KodGames.ExternalCall;
using KodGames.ExternalCall.Platform;
using LitJson;

public class Efun_IAPListener : IAPListener
{
	public override bool PayProduct (string productID, int goodsID, int count, int callback, string additionalData)
	{
		EfunPlugin.Pay(productID,				//需要确认是否是userId
		               SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString(),
		               SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID.ToString(),
		               SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level.ToString(),
		               SysLocalDataBase.Inst.LocalPlayer.Name,
		               GetPartnerOrderId(productID, additionalData));
	
		return true;
	}
}