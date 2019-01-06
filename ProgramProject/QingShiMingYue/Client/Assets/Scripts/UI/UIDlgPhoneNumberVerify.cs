using UnityEngine;
using System.Collections;

public class UIDlgPhoneNumberVerify : UIModule 
{
	public UITextField phoneNumber;
	public UITextField confirmPhoneNumber;


	private int itemId;
	private int chooseIndex;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length < 2)
			return true;

		itemId = (int)userDatas[0];
		chooseIndex = (int)userDatas[1];

		phoneNumber.Text = "";
		confirmPhoneNumber.Text = "";

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCloseClick(UIButton btn)
	{
		SysUIEnv.Instance.HideUIModule(typeof(UIDlgPhoneNumberVerify));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClearClick(UIButton btn)
	{
		phoneNumber.Text = "";
		confirmPhoneNumber.Text = "";
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnOkClick(UIButton btn)
	{
		string phoneInput = phoneNumber.Text.TrimEnd();
		string reInput = confirmPhoneNumber.Text.TrimEnd();
		if (string.IsNullOrEmpty(phoneInput) == false && phoneInput.Equals(reInput))
			RequestMgr.Inst.Request(new ConsumeItemReq(itemId, 1, chooseIndex, phoneInput));
		else
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgPhoneNumberVerify_InvalidInput"));
			phoneNumber.Text = "";
			confirmPhoneNumber.Text = "";
		}
	}
}
