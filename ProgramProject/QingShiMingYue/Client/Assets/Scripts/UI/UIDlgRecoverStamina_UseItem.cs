using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgRecoverStamina_UseItem : UIModule
{
	public UIElemAssetIcon itemIcon;
	public SpriteText itemNameLabel;
	public SpriteText titleLabel;
	public SpriteText itemDescLabel;
	public SpriteText promptLabel;

	private System.Action onOkClick;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		int itemAssetId = (int)userDatas[0];
		string promptStr = (string)userDatas[1];
		onOkClick = userDatas[2] as System.Action;
		int rewardId = (int)userDatas[3];

		itemIcon.SetData(itemAssetId);
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(itemAssetId);

		titleLabel.Text = string.Format(GameUtility.GetUIString("UIDlgRecoverStamina_UseItem_Title"), ItemInfoUtility.GetAssetName(rewardId));
		itemDescLabel.Text = ItemInfoUtility.GetAssetDesc(itemAssetId);
		promptLabel.Text = promptStr;

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOkBtnClick(UIButton btn)
	{
		onOkClick();
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}

