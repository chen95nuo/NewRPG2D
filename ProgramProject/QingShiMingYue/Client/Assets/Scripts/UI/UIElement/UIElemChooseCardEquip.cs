using UnityEngine;
using System.Collections;

public class UIElemChooseCardEquip : UIElemChooseCardBasic
{
	public SpriteText[] attributeLabels;

	public void SetData(KodGames.ClientClass.Equipment equip)
	{
		SetData(equip, false);
	}

	public void SetData(KodGames.ClientClass.Equipment equip, bool selected)
	{
		SetBaseData(equip.ResourceId, equip.Guid, selected);

		// Set Attributes.
		for (int i = 0; i < attributeLabels.Length; i++)
			attributeLabels[i].Text = string.Empty;

		var equipmentAttrs = PlayerDataUtility.GetEquipmentAttributes(equip);
		for (int i = 0; i < equipmentAttrs.Count && i < attributeLabels.Length; i++)
			attributeLabels[i].Text = ItemInfoUtility.GetAttributeNameValueString(equipmentAttrs[i].type, equipmentAttrs[i].value);

		itemBg.Data = this;
		itemIcon.SetData(equip);
	}
}
