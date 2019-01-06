using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuideDetail : UIModule
{
	public UIScrollList guideDescList;

	public GameObjectPool guideDetailObjPool;
	public SpriteText TitleLable;
	public SpriteText itemTitleLable;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;


		TitleLable.Text = GameUtility.GetUIString("UIGuide_Label_Tip");

		itemTitleLable.Text = string.Format("{0}", (userDatas[0] as GuideConfig.MainType).name);

		// Fill guide details.
		StartCoroutine("FillGuideDescList", userDatas[0] as GuideConfig.MainType);

		return true;
	}

	public override void OnHide()
	{
		ClearLsit();
		base.OnHide();
	}

	// Clear guide detail list.
	private void ClearLsit()
	{
		StopCoroutine("FillGuideDescList");
		guideDescList.ClearList(false);
		guideDescList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideDescList(GuideConfig.MainType guideItem)
	{
		yield return null;

		foreach (GuideConfig.SubType guideDescItem in guideItem.subTypes)
		{
			UIListItemContainer guideDetailItem = guideDetailObjPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuideDetailItem detail = guideDetailItem.gameObject.GetComponent<UIElemGuideDetailItem>();
			detail.SetData(guideDescItem);
			guideDescList.AddItem(guideDetailItem);
		}
	}

	//Click to return to UIPnlGuide.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}
}
