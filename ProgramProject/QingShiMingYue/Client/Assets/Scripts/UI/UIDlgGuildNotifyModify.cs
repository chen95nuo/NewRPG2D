using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgGuildNotifyModify : UIModule
{
	public delegate void OnModifySuccess();

	public SpriteText titleLabel;
	public SpriteText tipLabel;
	public UITextField textFiled;

	private bool declarationModifiy;
	private OnModifySuccess onModifySuccess;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		textFiled.SetValueChangedDelegate(OnTextFieldValueChanged);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.declarationModifiy = (bool)userDatas[0];

		if (userDatas.Length > 1)
			this.onModifySuccess = userDatas[1] as OnModifySuccess;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		this.onModifySuccess = null;
	}

	private void InitView()
	{
		// Set Title Label.
		titleLabel.Text = GameUtility.GetUIString(declarationModifiy ? "UIDlgGuildNotifyModify_DeclarationTitle" : "UIDlgGuildNotifyModify_AnnouncementTitle");

		// Set TipLabel.
		tipLabel.Text = string.Empty;

		// Set PlaceHolder.
		textFiled.placeHolder = GameUtility.GetUIString(declarationModifiy ? "UITextHolder_Declaration" : "UITextHolder_Announcement");

		// Set textFiled.
		textFiled.Text = declarationModifiy ? SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildDeclaration : SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildAnnouncement;

		//UIManager.instance.FocusObject = textFiled;
	}

	private void OnTextFieldValueChanged(IUIObject obj)
	{
		int maxCount = declarationModifiy ? ConfigDatabase.DefaultCfg.GuildConfig.DeclarationMaxLength : ConfigDatabase.DefaultCfg.GuildConfig.AnnouncementMaxLength;

		tipLabel.Text = GameUtility.FormatUIString("UIDlgGuildNotifyModify_Tip", maxCount - textFiled.Text.Length >= 0 ? maxCount - textFiled.Text.Length : 0);

		if (textFiled.Text.Length > maxCount)
			textFiled.Text = textFiled.Text.Substring(0, maxCount);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOKButton(UIButton btn)
	{
		string errorMsg = string.Empty;
		int maxCount = declarationModifiy ? ConfigDatabase.DefaultCfg.GuildConfig.DeclarationMaxLength : ConfigDatabase.DefaultCfg.GuildConfig.AnnouncementMaxLength;

		if (string.IsNullOrEmpty(textFiled.Text))
			errorMsg = GameUtility.GetUIString("UIDlgGuildNotifyModify_Empty");
		else if (textFiled.Text.Length > maxCount)
			errorMsg = GameUtility.FormatUIString("UIDlgGuildNotifyModify_OverMaxValue", maxCount);

		if (string.IsNullOrEmpty(errorMsg) == false)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errorMsg);
			return;
		}

		if (declarationModifiy)
			RequestMgr.Inst.Request(new GuildSetDeclarationReq(textFiled.Text));
		else
			RequestMgr.Inst.Request(new GuildSetAnnouncementReq(textFiled.Text));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCancelButton(UIButton btn)
	{
		HideSelf();
	}

	public void OnModifiyResSuccess()
	{
		if (onModifySuccess != null)
			onModifySuccess();

		HideSelf();
	}
}