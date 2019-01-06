using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlIllusionGuideDetail : UIModule
{
	public UIScrollList guideDetailList;
	public GameObjectPool guideDetailLabelPool;
	public SpriteText titleLabel;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		StartCoroutine("FillGuideDetailList", userDatas[0] as ClientServerCommon.MainType);

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillGuideDetailList");
		guideDetailList.ClearList(false);
		guideDetailList.ScrollListTo(0f);
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideDetailList(ClientServerCommon.MainType guideItem)
	{
		yield return null;

		titleLabel.Text = string.Format("{0}", guideItem.name);

		for (int index = 0; index < guideItem.subTypes.Count; index++)
		{
			UIListItemContainer guideDetailItem = guideDetailLabelPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemIllusionGuideDetail detail = guideDetailItem.gameObject.GetComponent<UIElemIllusionGuideDetail>();
			detail.SetData(guideItem.subTypes[index]);
			guideDetailList.AddItem(guideDetailItem);
		}
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcom(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId);
	}


}
