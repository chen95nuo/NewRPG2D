using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemEquipSelectItem : MonoBehaviour
{
	public UIListItemContainer container;

	public UIElemAssetIcon equipIcon;
	public UIBox assembleIcon;
	public SpriteText equipNameLabel;
	public SpriteText equipOwnerLabel;
	public SpriteText equipQualityLabel;
	public List<SpriteText> equipAttributeLabels;

	public UIButton middleInfoBtn;
	public UIButton detailInfoBtn;
	public UIButton selectBtn;

	private KodGames.ClientClass.Equipment equipment;
	public KodGames.ClientClass.Equipment Equipment { get { return equipment; } }

	public void SetData(KodGames.ClientClass.Equipment equipment, int positionId)
	{
		this.container.Data = this;
		this.equipment = equipment;
		this.equipIcon.Data = this;
		this.middleInfoBtn.Data = this;
		this.detailInfoBtn.Data = this;
		this.selectBtn.Data = this;

		// Set equipment Icon.
		equipIcon.SetData(equipment);

		// Set Assemble Icon.
		assembleIcon.Hide(!equipment.IsAssembleActive);

		selectBtn.IndexData = equipment.ResourceId;

		// Set equip name.
		equipNameLabel.Text = ItemInfoUtility.GetAssetName(equipment.ResourceId);

		// Set equip Quality Description.
		equipQualityLabel.Text = GameDefines.textColorBtnYellow.ToString() + GameUtility.GetUIString("UIDlgAvatarFilter_QualityTitle") + ": " + ItemInfoUtility.GetAssetQualityDesc(equipment.ResourceId, true, true);

		// Set equip owner.
		// Only LineUp In Current Position, show lineUp message label.

		if (!PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, positionId, equipment.Guid, equipment.ResourceId))
			equipOwnerLabel.Text = "";
		else
			equipOwnerLabel.Text = GameUtility.GetUIString("UIPnlSelectEquipmentList_ItemOwnerLabel");
		//equipOwnerLabel.Hide();
		// Whatever is lined up in position , show Line Up button.
		bool isLineUp = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, equipment.Guid, equipment.ResourceId);
		detailInfoBtn.Hide(!isLineUp);

		// attribute.
		for (int i = 0; i < equipAttributeLabels.Count; i++)
			equipAttributeLabels[i].Text = string.Empty;

		var attributes = PlayerDataUtility.GetEquipmentAttributes(equipment);
		for (int i = 0; i < equipAttributeLabels.Count && i < attributes.Count; i++)
			equipAttributeLabels[i].Text = GameUtility.FormatUIString(
					"UIElemEquipSelectItem_AttributeDetail",
					GameDefines.textColorBtnYellow,
					_AvatarAttributeType.GetDisplayNameByType(attributes[i].type, ConfigDatabase.DefaultCfg),
					GameDefines.txColorWhite,
					ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
	}
}
