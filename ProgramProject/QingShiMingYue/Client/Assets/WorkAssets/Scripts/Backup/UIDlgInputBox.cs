#if ENABLE_AUCTION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgInputBox : UIModule
{
	public class ShowData
	{
		public delegate bool Callback(object data);
		public Callback OnOkCallBack;

		public string cancelBtnLabel = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");
		public string okBtnLabel = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_OK");

		public string inputHolderMessage = "";
		public string title = "";
	}
	public SpriteText titleLabel;
	public UITextField inputField;
	public ShowData showData;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.showData = userDatas[0] as ShowData;

		titleLabel.Text = showData.title;
		inputField.placeHolder = showData.inputHolderMessage;
		inputField.placeHolderColorTag = GameDefines.txColorHolder;
		inputField.Text = "";

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickOk(UIButton btn)
	{
		if(inputField.spriteText.Text.Equals("") == false && showData.OnOkCallBack(inputField.spriteText.Text))
		{
			HideSelf();
		}
		else
		{
            SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgInputBox_Input_Erro"));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}
#endif