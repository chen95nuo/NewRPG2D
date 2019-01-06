using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ExternalCall;

public class UIDlgShare : UIModule
{
	private bool wait = false;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		if (!wait)
			HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFacebook(UIButton btn)
	{
		if (!wait)
		{
			wait = true;
			Platform.Instance.FacebookPublishShare();
		}

#if !UNITY_EDITOR
#if UNITY_IPHONE
#elif UNITY_ANDROID
#endif
#else
		wait = false;
#endif
	}

	public void FacebookShareSuccess(int faceBookSuccess)
	{
		wait = false;

		if (faceBookSuccess == 0)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityInvite_Facebook_Share_Success"));

			RequestMgr.Inst.Request(new FacebookShareReq());
		}
	}
}