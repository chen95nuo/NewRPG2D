using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIPnlIllusionGuide : UIModule
{
	public UIScrollList guideList;
	public GameObjectPool guidePool;
	public UIButton backBtn;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas)==false)
			return false;

		StartCoroutine("FillGuideList");

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillGuideList");
		guideList.ClearList(false);
		guideList.ScrollListTo(0);
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideList()
	{
		yield return null;

		for (int index = 0; index < ConfigDatabase.DefaultCfg.IllusionConfig.MainTypes.Count; index++)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemIllusionGuideItem guide = item.gameObject.GetComponent<UIElemIllusionGuideItem>();
			guide.SetData(ConfigDatabase.DefaultCfg.IllusionConfig.MainTypes[index]);
			guideList.AddItem(item);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGuideItemClick(UIButton btn)
	{
		ClientServerCommon.MainType guideItem = btn.Data as ClientServerCommon.MainType;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlIllusionGuideDetail), guideItem);
	}

}
