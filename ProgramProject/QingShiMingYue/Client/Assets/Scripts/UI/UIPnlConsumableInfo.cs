using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIPnlConsumableInfo : UIPnlItemInfoBase
{
	public SpriteText titleLabel;
	public SpriteText consumableName;
	public UIElemAssetIcon consumableIcon;
	public SpriteText consumableDesc;
	public SpriteText consumableExtraDesc;

	public UIChildLayoutControl childLayout;
	public UIButton scrollButton;
	public UIButton closeButton;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		int consumableId = (int)userDatas[0];
		bool isScroll = IDSeg.ToAssetType(consumableId) == IDSeg._AssetType.Item
			&& ItemConfig._Type.ToItemType(consumableId) == ItemConfig._Type.EquipScroll;

		// Set title.
		titleLabel.Text = isScroll ? GameUtility.GetUIString("UIConsumableInfo_ScrollTitle") : GameUtility.GetUIString("UIConsumableInfo_Title");

		// Set consumable info.
		consumableIcon.SetData(consumableId);
		consumableName.Text = ItemInfoUtility.GetAssetName(consumableId);
		consumableExtraDesc.Text = ItemInfoUtility.GetAssetExtraDesc(consumableId);
		consumableDesc.Text = ItemInfoUtility.GetAssetDesc(consumableId);

		// Set scrollButton Data.
		scrollButton.Data = consumableId;

		// Hide the mixScroll Button if consumable is not equipScroll.
		HideChildButton(isScroll);

		return true;
	}

	public void OnClose()
	{
		HideSelf();
	}

	private void HideChildButton(bool isScroll)
	{
		childLayout.HideChildObj(scrollButton.gameObject, !isScroll);
		childLayout.HideChildObj(closeButton.gameObject, false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMixScroll(UIButton btn)
	{
		//RequestMgr.Inst.Request(new EquipMixRequest((int)btn.Data));
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
		Debug.Log("OnClickMenuBot  " + "UIPnlConsumableInfo");
	}
}