using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

using Dan = KodGames.ClientClass.Dan;

public enum DanCultureUIType
{
	None,
	DanLevelUp,
	DanBreakthought,
	DanAttributeRefresh
}

public class UIPnlDanCultureTab : UIModule
{
	//public UIScrollList menuList;
	public List<UIBox> tabLights;
	public List<UIButton> tabBtns;
	public List<UIBox> notifIcons;

	private Dan dan = new Dan();
	private DanCultureUIType cultureType = DanCultureUIType.None;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		tabBtns[0].Data = DanCultureUIType.DanLevelUp;
		tabBtns[1].Data = DanCultureUIType.DanBreakthought;
		tabBtns[2].Data = DanCultureUIType.DanAttributeRefresh;
		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas != null && userDatas.Length > 0)
		{
			dan = userDatas[0] as Dan;
		}
		OpenShowUI(DanCultureUIType.DanLevelUp);
		return true;
	}

	private void OpenShowUI(DanCultureUIType type)
	{
		if (cultureType != type)
		{
			SetLight(type);
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanCulture)))
				SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().UpdateUI(type);
			else
				SysUIEnv.Instance.ShowUIModule<UIPnlDanCulture>(dan, type);
			cultureType = type;
		}
		SetNotifIcon(type);
	}

	private void HideUI()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlConsumableInfo)))
			SysUIEnv.Instance.GetUIModule<UIPnlConsumableInfo>().OnClose();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		if (SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().OnHide();
		SysUIEnv.Instance.GetUIModule<UIPnlDanInfo>().UpdateDanData(SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().Dan);
		cultureType = DanCultureUIType.None;
		HideSelf();
	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		HideUI();
		OpenShowUI((DanCultureUIType)btn.Data);
	}
	public void UpdateData(Dan dan)
	{
		this.dan = dan;
		SetNotifIcon(cultureType);
	}

	private void SetNotifIcon(DanCultureUIType cultureType)
	{
		for (int i = 0; i < tabBtns.Count; i++)
		{
			switch ((DanCultureUIType)tabBtns[i].Data)
			{
				case DanCultureUIType.DanLevelUp:
					notifIcons[i].Hide(DanCultureUIType.DanLevelUp == cultureType || !ItemInfoUtility.IsLevelNotifyActivity_Dan(dan));
					break;
				case DanCultureUIType.DanBreakthought:
					notifIcons[i].Hide(DanCultureUIType.DanBreakthought == cultureType || !ItemInfoUtility.IsBreakNotifyActivity_Dan(dan));
					break;
				case DanCultureUIType.DanAttributeRefresh:
					notifIcons[i].Hide(true);
					break;
			}
		}
	}

	public void SetLight(DanCultureUIType cultureType)
	{
		for (int i = 0; i < tabBtns.Count; i++)
		{
			if ((DanCultureUIType)tabBtns[i].Data == cultureType)
				tabLights[i].Hide(false);
			else
				tabLights[i].Hide(true);
		}
	}
}
