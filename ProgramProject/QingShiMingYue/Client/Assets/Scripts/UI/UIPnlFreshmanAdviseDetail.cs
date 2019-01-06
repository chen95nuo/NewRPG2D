using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFreshmanAdviseDetail : UIModule
{
	public UIScrollList guideDescList;

	public GameObjectPool guideDetailObjPool;
	public SpriteText TitleLable;
	public SpriteText itemTitleLable;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().closeBtn.Hide(true);

		TitleLable.Text = GameUtility.GetUIString("UIPnlAssistant_FreshmanAdviseTab_Title");

		itemTitleLable.Text = string.Format("{0}", (userDatas[0] as GuideConfig.MainType).name);

		// Fill guide details.
		StartCoroutine("FillGuideDescList", userDatas[0] as GuideConfig.MainType);

		return true;
	}

	public override void OnHide()
	{
		ClearLsit();
		base.OnHide();
		SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().closeBtn.Hide(false);
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

	//Click to return to UIPnlFreshmanAdvise.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		OnHide();
		SysUIEnv.Instance.ShowUIModule<UIPnlFreshmanAdvise>();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGotoClick(UIButton btn)
	{
		GuideConfig.SubType subType = btn.data as GuideConfig.SubType;

		//功能暂时不能使用
		if (!GameUtility.CheckUIAccess(subType.gotoUI, true))
			return;

		switch (subType.gotoUI)
		{
			//下面4个UI为支持子界面跳转，配置了第二个参数
			//TODO 重命名handBookParam变量
			case _UIType.UIPnlHandBook:
			case _UIType.UIPnlPackageSell:
				GameUtility.JumpUIPanel(subType.gotoUI, subType.handBookParam);
				break;

			case _UIType.UI_ActivityDungeon:
			case _UIType.UI_Dungeon:
				if (subType.handBookParam == IDSeg.InvalidId)
					GameUtility.JumpUIPanel(subType.gotoUI);
				else
					GameUtility.JumpUIPanel(subType.gotoUI, subType.handBookParam);
				break;

			default:
				GameUtility.JumpUIPanel(subType.gotoUI);
				break;
		}
	}
}
