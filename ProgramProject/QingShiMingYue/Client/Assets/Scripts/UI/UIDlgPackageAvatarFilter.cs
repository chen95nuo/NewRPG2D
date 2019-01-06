using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgPackageAvatarFilter : UIModule
{
	public UIElemSelectItem[] traitTypes;
	public UIElemSelectItem[] qualitys;
	public UIElemSelectItem[] countryTypes;

	public delegate void OnSelectFilterAvatar();
	private OnSelectFilterAvatar onSelectFilterAvatar;
	private int filterDataType;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;

		if (userDatas != null && userDatas.Length > 0)
			this.filterDataType = (int)userDatas[0];

		if (userDatas != null && userDatas.Length > 1)
			this.onSelectFilterAvatar = userDatas[1] as OnSelectFilterAvatar;

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		this.onSelectFilterAvatar = null;
		this.filterDataType = PackageFilterData._DataType.UnKnow;
	}

	private void InitUI()
	{
		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(filterDataType);

		// 角色类型
		for (int i = 0; i < traitTypes.Length; ++i)
		{
			var traitType = traitTypes[i];
			if (i < AvatarConfig._AvatarTraitType.GetRegisterTypeCount())
			{
				var type = AvatarConfig._AvatarTraitType.GetRegisterTypeByIndex(i);
				traitType.SetActive(true);
				traitType.InitState(type);
				traitType.selectBtn.Text = AvatarConfig._AvatarTraitType.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg);
				traitType.SetState(avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType).Contains(type));
			}
			else
				traitType.SetActive(false);
		}

		// 品质
		const int maxQualityLevel = 5;
		for (int i = 0; i < qualitys.Length; i++)
		{
			var quality = qualitys[i];

			if (i < maxQualityLevel)
			{
				int qualityLevel = maxQualityLevel - i;
				quality.SetActive(true);
				quality.InitState(qualityLevel);
				quality.selectBtn.Text = GameUtility.GetUIString("UIDlgAvatarFilter_UI_QualityString_" + qualityLevel + "_Star");
				quality.SetState(avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel).Contains(qualityLevel));
			}
			else
				quality.SetActive(true);
		}

		// 国家.
		for (int i = 0; i < countryTypes.Length; ++i)
		{
			var countryType = countryTypes[i];
			if (i < AvatarConfig._AvatarCountryType.GetRegisterTypeCount())
			{
				int type = AvatarConfig._AvatarCountryType.GetRegisterTypeByIndex(i);
				if (type == AvatarConfig._AvatarCountryType.All
				|| type == AvatarConfig._AvatarCountryType.NoCountry
				|| type == AvatarConfig._AvatarCountryType.LiuGuo)
				{
					countryType.SetActive(false);
					continue;
				}

				countryType.SetActive(true);
				countryType.InitState(type);
				countryType.selectBtn.Text = AvatarConfig._AvatarCountryType.GetDisplayNameByType(type, ConfigDatabase.DefaultCfg);
				countryType.SetState(avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType).Contains(type));
			}
			else
				countryType.SetActive(false);
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
		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(filterDataType);

		// Avatar Type.
		List<int> selectDatas = new List<int>();
		for (int i = 0; i < traitTypes.Length; i++)
			if (traitTypes[i].IsSelected)
				selectDatas.Add((int)traitTypes[i].Data);

		avatarFilter.Put(PackageFilterData._FilterType.AvatarTraitType, selectDatas);

		// Quality.
		selectDatas.Clear();
		for (int i = 0; i < qualitys.Length; i++)
			if (qualitys[i].IsSelected)
				selectDatas.Add((int)qualitys[i].Data);

		avatarFilter.Put(PackageFilterData._FilterType.QualityLevel, selectDatas);

		// AvatarCountry.
		selectDatas.Clear();
		for (int i = 0; i < countryTypes.Length; i++)
		{
			if (countryTypes[i].IsSelected)
				selectDatas.Add((int)countryTypes[i].Data);
		}
		avatarFilter.Put(PackageFilterData._FilterType.AvatarCountryType, selectDatas);

		avatarFilter.Save();

		if (this.onSelectFilterAvatar != null)
			this.onSelectFilterAvatar();

		// 界面后hide，否则del 会清空。
		HideSelf();
	}
}
