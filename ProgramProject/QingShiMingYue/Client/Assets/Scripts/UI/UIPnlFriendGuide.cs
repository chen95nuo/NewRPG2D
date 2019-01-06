using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendGuide : UIModule
{
	public UIScrollList guideList;
	public GameObjectPool guidePool;

	private int maxId;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		maxId = IDSeg.InvalidId;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		maxId = (int)userDatas[0];

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillGuideList");
		guideList.ClearList(false);
		guideList.ScrollListTo(0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideList()
	{
		yield return null;

		for (int index = 0; index < ConfigDatabase.DefaultCfg.FriendCampaignConfig.MainTypes.Count; index++)
		{
			UIElemFriendGuideItemRoot item = guidePool.AllocateItem(false).GetComponent<UIElemFriendGuideItemRoot>();
			item.SetData(ConfigDatabase.DefaultCfg.FriendCampaignConfig.MainTypes[index]);
			guideList.AddItem(item);
		}
	}

	private void InitUI()
	{
		StartCoroutine("FillGuideList");
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击标签
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGuideItemClick(UIButton btn)
	{
		ClientServerCommon.MainType guideItem = btn.Data as ClientServerCommon.MainType;
		//SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendGuideDetail), guideItem, maxId);
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendGuideDetail_1), guideItem, maxId);
	}
}
