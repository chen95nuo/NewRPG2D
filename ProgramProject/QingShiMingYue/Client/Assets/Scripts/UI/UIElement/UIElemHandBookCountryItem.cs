using System;
using System.Collections.Generic;
using System.Collections;
using ClientServerCommon;
using UnityEngine;

/// <summary>
/// 国家和装备的横向list
/// </summary>
public class UIElemHandBookCountryItem : MonoBehaviour
{
	public UIButton countryButton;
	public UIBox imageBox;//填充国家或者装备的icon
	public UIBox lightBox;//选中高亮的框
	public UIBox notify;

	private const int liuGuo = -1;

	private const int imageBoxWidth = 40;
	private const int liuGuoImageBoxWidth = 30;
	private const int imageBoxHeight = 19;

	private int subType;//国家或者装备类型
	public int SubType
	{
		set { subType = value; }
		get { return subType; }
	}

	public void SetData(int pageType, int subType)
	{
		this.SubType = subType;

		countryButton.Data = this;

		// set imageBox
		switch (pageType)
		{
			case IDSeg._AssetType.Avatar:
				imageBox.SetSize(liuGuoImageBoxWidth, imageBoxHeight);
				UIElemTemplate.Inst.SetAvatarCountryIcon(imageBox, subType);
				break;
			case IDSeg._AssetType.Equipment:
				imageBox.SetSize(imageBoxWidth, imageBoxWidth);
				SysUIEnv.Instance.GetUIModule<UIPnlHandBook>().newEquipTemplate.SetNewEquipTypeIcon(imageBox, subType);
				break;
			case IDSeg._AssetType.CombatTurn:
				UIElemTemplate.Inst.SetSkillTypeIcon(imageBox, subType);
				break;
		}
	}

	public void EnableButton(bool enable)
	{
		countryButton.controlIsEnabled = enable;
	}

	/// <summary>
	/// 外部控制自己的显示与否
	/// </summary>
	public void ActiveLightBox(bool active)
	{
		lightBox.Hide(!active);
	}
}
