using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

using FeedbackType = ClientServerCommon._UserFeedbackType;

public class UIDlgFeedBack : UIModule
{
	public List<UIRadioBtn> feedBackWayBtns;
	public UITextField feedBackInput;

	private int selectedWay;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		foreach (UIRadioBtn btn in feedBackWayBtns)
		{
			if (feedBackWayBtns.IndexOf(btn) == 2)
				btn.Value = true;
			else
				btn.Value = false;
		}
		selectedWay = 2;
		feedBackInput.Text = "";

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCancelClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCommitClick(UIButton btn)
	{
		if (feedBackInput.Text.TrimEnd().Equals(""))
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFeedBack_TextEmpty"));
			return;
		}

		RequestMgr.Inst.Request(new SettingFeedbackReq(selectedWay, feedBackInput.Text.TrimEnd()));
	}

	public void OnSettingFeedBackSuccess()
	{
		feedBackInput.Text = "";
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFeedBack_Label_Commit"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnFeedBackWaySelected(UIRadioBtn btn)
	{
		switch (feedBackWayBtns.IndexOf(btn))
		{
			case FeedbackType.Bug:
			case FeedbackType.Complaint:
			case FeedbackType.Advance:
			case FeedbackType.Other:
				selectedWay = feedBackWayBtns.IndexOf(btn);
				break;

			default:
				selectedWay = FeedbackType.Other;
				break;
		}
	}
}
