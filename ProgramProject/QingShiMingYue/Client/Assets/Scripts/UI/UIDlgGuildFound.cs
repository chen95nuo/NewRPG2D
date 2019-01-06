using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgGuildFound : UIModule
{
	public SpriteText foundCostLabel;
	public SpriteText foundExplainLabel;
	public UIButton selectBtn;
	public UIBox selectBox;
	public UIButton closeBtn;
	public UIButton foundBtn;
	public UITextField searchForm;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SetData();

		return true;
	}

	public override void OnHide()
	{
		string formTxt = searchForm.spriteText.Text.Trim();
		string formPlaceHolder = string.Format("{0}{1}", searchForm.placeHolderColorTag, searchForm.placeHolder);

		if (!formTxt.Equals("") && !formTxt.Equals(formPlaceHolder))
			SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("CreateGuildName", formTxt);

		base.OnHide();
	}

	private void SetData()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("CreateGuildName"))
			searchForm.Text = (string)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("CreateGuildName");
		else
			searchForm.Text = string.Empty;

		searchForm.placeHolder = GameUtility.FormatUIString("UIPnlGuildFound_NameCount", ConfigDatabase.DefaultCfg.GuildConfig.NameMaxLength);
		foundCostLabel.Text = GameUtility.FormatUIString("UIPnlGuildFound_Cost", SysLocalDataBase.GetCostsDesc(ConfigDatabase.DefaultCfg.GuildConfig.CreateCosts));
		foundExplainLabel.Text = GameUtility.FormatUIString("UIPnlGuildFound_Explain", GameDefines.textColorBtnYellow, GameDefines.textColorGuildChat);
		selectBox.Hide(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFound(UIButton btn)
	{
		string formTxt = searchForm.spriteText.Text.Trim();
		string formPlaceHolder = string.Format("{0}{1}", searchForm.placeHolderColorTag, searchForm.placeHolder);

		if (formTxt.Equals("") || formTxt.Equals(formPlaceHolder))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildFound_GuildNameNull"));
		else
			RequestMgr.Inst.Request(new GuildCreateReq(formTxt, !selectBox.IsHidden()));

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		selectBox.Hide(!selectBox.IsHidden());
	}

	public void OnResponseCreateGuild(bool success)
	{
		if (success)
		{
			// Show Tips.
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildApplyList_CreateSuccess"));

			// Change UI.
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
		}
		else
			HideSelf();
	}
}
