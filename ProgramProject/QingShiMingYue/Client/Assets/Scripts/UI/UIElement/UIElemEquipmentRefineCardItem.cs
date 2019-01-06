using UnityEngine;
using System.Collections;
using KodGames.ClientClass;

public class UIElemEquipmentRefineCardItem : MonoBehaviour
{
	public UIElemAssetIcon equipIcon;
	public UIBox iconSelected;

	private Equipment equip;
	public Equipment Equip { get { return equip; } }

	public void SetData(Equipment equip)
	{
		this.equip = equip;

		// Set the equipment icon.
		equipIcon.SetData(equip);
		equipIcon.Data = this;

		SetIconSelected(false);
	}

	public bool IsIconSelected()
	{
		return iconSelected.StateNum == 0;
	}

	public void SetIconSelected(bool selected)
	{
		if (selected)
		{
			iconSelected.SetToggleState(0);
			UIUtility.CopyIconTrans(equipIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
		}
		else
		{
			iconSelected.SetToggleState(1);
			UIUtility.CopyIconTrans(equipIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
		}
	}
}
