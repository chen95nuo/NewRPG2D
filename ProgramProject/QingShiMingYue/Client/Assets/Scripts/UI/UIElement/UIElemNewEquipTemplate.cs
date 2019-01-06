using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemNewEquipTemplate : MonoBehaviour 
{
	public UIButton iconEquipBao;
	public UIButton iconEquipShi;
	public UIButton iconEquipWu;
	public UIButton iconEquipXie;
	public UIButton iconEquipYi;

	/// <summary>
	/// 用新的图片设置装备icon
	/// </summary>
	public void SetNewEquipTypeIcon(AutoSpriteControlBase targetIcon, int Type)
	{
		AutoSpriteControlBase sourceIcon = null;

		switch (Type)
		{
			case EquipmentConfig._Type.Shoe:
				sourceIcon = iconEquipXie;
				break;
			case EquipmentConfig._Type.Treasure:
				sourceIcon = iconEquipBao;
				break;
			case EquipmentConfig._Type.Weapon:
				sourceIcon = iconEquipWu;
				break;
			case EquipmentConfig._Type.Armor:
				sourceIcon = iconEquipYi;
				break;
			case EquipmentConfig._Type.Decoration:
				sourceIcon = iconEquipShi;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}
}
