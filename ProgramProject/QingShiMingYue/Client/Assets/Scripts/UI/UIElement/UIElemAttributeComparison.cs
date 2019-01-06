using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAttributeComparison : MonoBehaviour
{
	public SpriteText nameLabel;
	public List<SpriteText> attributeLabels;
	public SpriteText statusLabel;
	public SpriteText emptyLabel;

	public void SetData(KodGames.ClientClass.Equipment equip)
	{
		if (equip == null)
		{
			nameLabel.Text = string.Empty;

			for (int index = 0; index < attributeLabels.Count; index++)
				attributeLabels[index].Text = string.Empty;

			if (emptyLabel != null)
				emptyLabel.Text = GameUtility.GetUIString("UIDlgAttributeComparison_EmptyLabel");

			if (statusLabel != null)
				statusLabel.Text = string.Empty;
		}
		else
		{
			// Set Equip Name.
			nameLabel.Text = ItemInfoUtility.GetAssetName(equip.ResourceId);

			// Set Attribute Label.
			var attributes = PlayerDataUtility.GetEquipmentAttributes(equip);
			int index = 0;
			for (; index < Math.Min(attributeLabels.Count, attributes.Count); index++)
			{
				attributeLabels[index].Text = string.Format(
					"{0}{1}: {2}{3}",
					GameDefines.textColorBtnYellow.ToString(), _AvatarAttributeType.GetDisplayNameByType(attributes[index].type, ConfigDatabase.DefaultCfg),
					GameDefines.textColorWhite.ToString(), ItemInfoUtility.GetAttribDisplayString(attributes[index].type, attributes[index].value));
			}

			for (; index < attributeLabels.Count; index++)
				attributeLabels[index].Text = string.Empty;

			// Set Empty Label.
			if (emptyLabel != null)
				emptyLabel.Text = string.Empty;

			if (statusLabel != null)
				statusLabel.Text = GameUtility.GetUIString("UIDlgAttributeComparison_Status");
		}
	}
}