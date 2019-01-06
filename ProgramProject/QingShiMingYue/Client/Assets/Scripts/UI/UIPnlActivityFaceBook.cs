using System;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;
using KodGames;

public class UIPnlActivityFaceBook : UIModule
{
	public UIBox facebookSuccessBox;

	private bool wait = false;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlActivityTab>().SetActiveButton(_UIType.UIPnlActivityFaceBook);
		ActivityManager.Instance.ActivityJumpUI = _UIType.UIPnlActivityFaceBook;

		SetFacebookSuccessBoxHide(true);
		StopCoroutine("ActiveClickFaceBookBtn");
		
		wait = false;
		RequestMgr.Inst.Request(new QueryFacebookReq());
		return true;
	}

	public void SetFacebookSuccessBoxHide(bool isHide)
	{
		facebookSuccessBox.Hide(isHide);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFacebook(UIButton btn)
	{
		if (!wait)
		{
			wait = true;
			Platform.Instance.FacebookPublishShare();
			StartCoroutine("ActiveClickFaceBookBtn");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator ActiveClickFaceBookBtn()
	{
		yield return new WaitForSeconds(1f);
		wait = false;
	}

	public void FacebookShareSuccess()
	{
		wait = false;
		SetFacebookSuccessBoxHide(false);
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityFacebook_Share_Success"));
		RequestMgr.Inst.Request(new FacebookShareReq());
	}
}
