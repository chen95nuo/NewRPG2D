using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuide : UIModule
{
	//Tab button.
	public UIScrollList guideList;
	public GameObjectPool guidePool;
	public SpriteText TitleLable;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlGuide);

		TitleLable.Text = GameUtility.FormatUIString("UIGuide_Label_Tip");

		StartCoroutine("FillGuideList");
		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		StopCoroutine("FillGuideList");
		guideList.ClearList(false);
		guideList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideList()
	{
		yield return null;

		foreach (ClientServerCommon.GuideConfig.MainType guideItem in ConfigDatabase.DefaultCfg.GuideConfig.mainTypes)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuideItem guide = item.gameObject.GetComponent<UIElemGuideItem>();
			guide.SetData(guideItem);
			guideList.AddItem(item);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGuideItemClick(UIButton btn)
	{
		ClientServerCommon.GuideConfig.MainType guideItem = btn.data as ClientServerCommon.GuideConfig.MainType;
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlGuideDetail, guideItem);
	}
}
