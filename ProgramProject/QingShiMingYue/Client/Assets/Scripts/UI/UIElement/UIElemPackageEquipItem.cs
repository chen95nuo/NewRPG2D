using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPackageEquipItem : UIElemPackageItemBase
{
	public UIElemAssetIcon equipIcon;

	public UIButton refineBtn;//

	public UIButton explicitBtn;
	public UIButton powerUpBtn;

	public SpriteText equipName;
	public SpriteText equipQuality;

	public SpriteText isEquiped;
	public List<SpriteText> equipAttrbutes;

	private KodGames.ClientClass.Equipment equip;
	public KodGames.ClientClass.Equipment Equip { get { return equip; } }

	public void SetData(KodGames.ClientClass.Equipment equip)
	{
		container.Data = this;
		this.equip = equip;

		// Set the Equipment Icon and Data.
		equipIcon.SetData(equip);
		equipIcon.Data = equip;

		// Set the Equipment Name.
		equipName.Text = ItemInfoUtility.GetAssetName(equip.ResourceId);

		// Set the Equipment Quality label.	
		equipQuality.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(equip.ResourceId);

		// Set Equip Attribute.
		var equipmentAttrs = PlayerDataUtility.GetEquipmentAttributes(equip);
		int i = 0;
		for (; i < equipmentAttrs.Count && i < equipAttrbutes.Count; i++)
			//equipAttrbutes[i].Text = ItemInfoUtility.GetAttributeNameValueString(equipmentAttrs[i].type, equipmentAttrs[i].value);
			equipAttrbutes[i].Text = GameUtility.FormatUIString(
						"UIDlgAttributeDetailTip_AttributeDetail",
						GameDefines.textColorBtnYellow,
						_AvatarAttributeType.GetDisplayNameByType(equipmentAttrs[i].type, ConfigDatabase.DefaultCfg),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetAttribDisplayString(equipmentAttrs[i].type, equipmentAttrs[i].value));

		for (; i < equipAttrbutes.Count; i++)
			equipAttrbutes[i].Text = string.Empty;

		// Set the Equipment isEquiped.
		if (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, equip))
		{
			isEquiped.Text = GameUtility.GetUIString("UIPackage_EquipmentOwner");
			explicitBtn.Hide(false);
		}
		else
		{
			isEquiped.Text = string.Empty;
			explicitBtn.Hide(true);
		}

		// Set the Equipment RefineButton's Data.
		refineBtn.Data = equip;

		// Set the Equipment Information's Data.
		explicitBtn.Data = equip;
	}
}
