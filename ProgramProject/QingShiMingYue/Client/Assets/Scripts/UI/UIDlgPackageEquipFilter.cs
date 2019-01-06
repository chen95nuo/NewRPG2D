using System;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIDlgPackageEquipFilter : UIModule
{
	public UIChildLayoutControl filterControl;
	public GameObject equipTypeRoot;
	public GameObject equipQuaityRoot;
	public UIElemSelectItem[] equipTypes;
	public UIElemSelectItem[] qualitys;

	public delegate void OnSelectFilterEquip();
	private OnSelectFilterEquip onSelectFilterEquip;
	private int filterDataType;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (userDatas != null && userDatas.Length > 0)
			this.filterDataType = (int)userDatas[0];

		if (userDatas != null && userDatas.Length > 1)
			this.onSelectFilterEquip = userDatas[1] as OnSelectFilterEquip;

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		this.onSelectFilterEquip = null;
		this.filterDataType = PackageFilterData._DataType.UnKnow;
	}

	private void InitUI()
	{
		filterControl.HideChildObj(equipTypeRoot, filterDataType == PackageFilterData._DataType.SelectEquip);
		filterControl.HideChildObj(equipQuaityRoot, false);

		var equipFilter = PackageFilterData.Instance.GetPackgetFilterByType(filterDataType);

		for (int i = 0; i < equipTypes.Length; i++)
		{
			var equipType = equipTypes[i];
			if (i < EquipmentConfig._Type.GetRegisterTypeCount())
			{
				var type = EquipmentConfig._Type.GetRegisterTypeByIndex(i);
				equipType.SetActive(true);
				equipTypes[i].InitState(type);
				equipTypes[i].selectBtn.Text = EquipmentConfig._Type.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg);
				equipTypes[i].SetState(equipFilter.GetFilterDataByType(PackageFilterData._FilterType.EquipType).Contains(type));
			}
			else
				equipType.SetActive(false);
		}

		// 品质
		int maxQualityLevel = 5;
		for (int i = 0; i < qualitys.Length; i++)
		{
			var quality = qualitys[i];

			if (i < maxQualityLevel)
			{
				int qualityLevel = maxQualityLevel - i;
				quality.SetActive(true);
				quality.InitState(qualityLevel);
				quality.selectBtn.Text = GameUtility.GetUIString("UIDlgAvatarFilter_UI_QualityString_" + qualityLevel + "_Star");
				quality.SetState(equipFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel).Contains(qualityLevel));
			}
			else
				quality.SetActive(true);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOk(UIButton btn)
	{
		var equipFilter = PackageFilterData.Instance.GetPackgetFilterByType(filterDataType);

		var selectDatas = new List<int>();

		if (equipTypeRoot.activeInHierarchy)
		{
			for (int i = 0; i < equipTypes.Length; i++)
				if (equipTypes[i].IsSelected)
					selectDatas.Add((int)equipTypes[i].Data);

			equipFilter.Put(PackageFilterData._FilterType.EquipType, selectDatas);
		}

		selectDatas.Clear();

		for (int i = 0; i < qualitys.Length; i++)
			if (qualitys[i].IsSelected)
				selectDatas.Add((int)qualitys[i].Data);

		equipFilter.Put(PackageFilterData._FilterType.QualityLevel, selectDatas);
		equipFilter.Save();

		if (this.onSelectFilterEquip != null)
			onSelectFilterEquip();

		// 界面后hide，否则del 会清空。
		HideSelf();
	}
}
