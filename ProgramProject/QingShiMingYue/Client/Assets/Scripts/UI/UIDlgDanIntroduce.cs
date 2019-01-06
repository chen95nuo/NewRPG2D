using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDanIntroduce : UIModule
{
	public UIScrollList descList;
	public UIScrollList menuList;
	public GameObjectPool descPool;
	public GameObjectPool iconPool;

	private int defaultType;
	private List<DanConfig.DanHelpInfo> danHelpInfos = new List<DanConfig.DanHelpInfo>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (userDatas.Length > 0)
			defaultType = (int)userDatas[0];
		else
			defaultType = danHelpInfos[0].InfoTpye;				

		StopCoroutine("FillDanIconList");
		menuList.ClearList(false);
		menuList.ScrollPosition = 0f;
		StartCoroutine("FillDanIconList");
		return true;
	}

	private void ClearData()
	{
		StopCoroutine("FillDescList");
		descList.ClearList(false);
		descList.ScrollPosition = 0f;
	}

	private void ChangeBtnSelect(int type)
	{
		this.defaultType = type;

		ClearData();
		StartCoroutine("FillDescList");
		for (int i = 0; i < menuList.Count; i++)
		{
			if (menuList.GetItem(i).Data is UIElemDanIntroduce)
			{
				UIElemDanIntroduce danIntro = menuList.GetItem(i).Data as UIElemDanIntroduce;
				if (danIntro.LnkUI == type)
					danIntro.SetSelectedStat(true);
				else danIntro.SetSelectedStat(false);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillDanIconList()
	{
		yield return null;

		danHelpInfos = ConfigDatabase.DefaultCfg.DanConfig.DanHelpInfos;

		for (int i = 0; i < danHelpInfos.Count; i++)
		{
			UIListItemContainer uiContainer = iconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemDanIntroduce iconItem = uiContainer.GetComponent<UIElemDanIntroduce>();
			uiContainer.data = iconItem;
			iconItem.SetData(danHelpInfos[i].IconId);	
			iconItem.LnkUI = danHelpInfos[i].InfoTpye;
			menuList.AddItem(iconItem.gameObject);	
		}

		menuList.ScrollToItem(defaultType - 1, 0f);
		ChangeBtnSelect(defaultType);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillDescList()
	{
		yield return null;

		string message = ConfigDatabase.DefaultCfg.DanConfig.GetDanHelpByInfoType(defaultType).Desc;
		if (defaultType == DanConfig._IntroduceType.DanAttri)
		{
			List<string> attributeStrings = ConfigDatabase.DefaultCfg.DanConfig.DanAttrInfos;

			for (int i = 0; i < attributeStrings.Count; i++)
			{
				message = message + "\n" + attributeStrings[i];
			}
		}

		UIElemDanIntroduceDescItem textItem = descPool.AllocateItem().GetComponent<UIElemDanIntroduceDescItem>();
		textItem.SetData(message);
		descList.AddItem(textItem.gameObject);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMenuButtonClick(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		ChangeBtnSelect((int)assetIcon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}
}

