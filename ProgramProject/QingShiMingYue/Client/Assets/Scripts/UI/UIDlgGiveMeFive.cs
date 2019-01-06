using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgGiveMeFive : UIModule
{
	public SpriteText tipText;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		tipText.Text = GameUtility.GetUIString("UIDlgGiveMeFive_Tip3");
		SysLocalDataBase.Inst.LocalPlayer.HasEvaluate = true;

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
		RequestMgr.Inst.Request(new GiveFiveStarsEvaluateReq(false));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOk(UIButton btn)
	{
		HideSelf();

		// Open Url
		int deviceType = _DeviceType.Unknown;
#if UNITY_IPHONE
		deviceType = _DeviceType.iPhone;
#elif UNITY_ANDROID
		deviceType = _DeviceType.Android;
#endif

		Application.OpenURL(ConfigDatabase.DefaultCfg.LevelConfig.GetGiveFiveUrlSettingByDeviceType(deviceType));
		RequestMgr.Inst.Request(new GiveFiveStarsEvaluateReq(true));
	}
}
