using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgPackageSkillFilter : UIModule
{
	public UIElemSelectItem[] qualitys;

	public delegate void OnSelectFilterSkill();
	private OnSelectFilterSkill onSelectFilterSkill;
	private int filterDataType;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (userDatas != null && userDatas.Length > 0)
			this.filterDataType = (int)userDatas[0];

		if (userDatas != null && userDatas.Length > 1)
			this.onSelectFilterSkill = userDatas[1] as OnSelectFilterSkill;

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		this.onSelectFilterSkill = null;
		this.filterDataType = PackageFilterData._DataType.UnKnow;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	private void InitUI()
	{
		var skillFilter = PackageFilterData.Instance.GetPackgetFilterByType(filterDataType);

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
				quality.SetState(skillFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel).Contains(qualityLevel));
			}
			else
				quality.SetActive(true);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOk(UIButton btn)
	{

		var skillFilter = PackageFilterData.Instance.GetPackgetFilterByType(filterDataType);
		var selectDatas = new List<int>();

		for (int i = 0; i < qualitys.Length; i++)
			if (qualitys[i].IsSelected)
				selectDatas.Add((int)qualitys[i].Data);

		skillFilter.Put(PackageFilterData._FilterType.QualityLevel, selectDatas);
		skillFilter.Save();
		selectDatas.Clear();

		if (this.onSelectFilterSkill != null)
			onSelectFilterSkill();

		HideSelf();
	}
}
