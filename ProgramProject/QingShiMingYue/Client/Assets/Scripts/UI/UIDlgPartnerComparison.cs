using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgPartnerComparison : UIModule
{
	public SpriteText partnerLabel;
	public SpriteText oldNameLabel;
	public SpriteText oldLvLabel;
	public SpriteText oldBreakLable;
	public SpriteText oldEmptyLabel;
	public SpriteText newNameLabel;
	public SpriteText newLvLabel;
	public SpriteText newBreakLable;
	public SpriteText attributeChangeLabel;

	private Color C_Name = GameDefines.textColorBtnYellow;
	private Color C_Value = GameDefines.textColorWhite;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return true;

		int partnerId = (int)userDatas[0];
		string oldAvatarGuid = (string)userDatas[1];
		string newAvatarGuid = (string)userDatas[2];

		InitView(partnerId, oldAvatarGuid, newAvatarGuid);

		return true;
	}

	private void InitView(int partnerId, string oldAvatarGuid, string newAvatarGuid)
	{
		// Partner Name Label.
		partnerLabel.Text = ItemInfoUtility.GetAssetName(partnerId);

		// Find Avatar Data.
		var oldAvatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(oldAvatarGuid);
		var newAvatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(newAvatarGuid);

		// Reset Label.
		attributeChangeLabel.Text = string.Empty;

		// Show Empty Label ,if oldAvatar is null.
		oldEmptyLabel.Hide(oldAvatar != null);

		SetAvatarDesc(oldAvatar, false);
		SetAvatarDesc(newAvatar, true);

		// Set Attribute Label.
		var partnerCfg = ConfigDatabase.DefaultCfg.PartnerConfig.GetPartnerById(partnerId);
		Dictionary<int, double> attributeTypes = new Dictionary<int, double>();

		for (int i = 0; i < partnerCfg.Modifiers.Count; i++)
			attributeTypes.Add(partnerCfg.Modifiers[i].attributeType, 0);

		var attributes = GetCheerAvatarAttribute(partnerId, newAvatar);
		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				if (!attributeTypes.ContainsKey(attributes[i].type))
					continue;

				attributeTypes[attributes[i].type] = attributes[i].value;
			}
		}

		attributes = GetCheerAvatarAttribute(partnerId, oldAvatar);
		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				if (!attributeTypes.ContainsKey(attributes[i].type))
					continue;

				attributeTypes[attributes[i].type] -= attributes[i].value;
			}
		}

		string attributeChanges = string.Empty;
		foreach (var attribute in attributeTypes)
		{
			if (attribute.Value == 0)
				continue;

			string formatStr = attribute.Value > 0 ? "UIDlgPartnerComparison_Attribute1" : "UIDlgPartnerComparison_Attribute2";

			attributeChanges += GameUtility.FormatUIString(formatStr, C_Name, _AvatarAttributeType.GetDisplayNameByType(attribute.Key, ConfigDatabase.DefaultCfg), C_Value, attribute.Value > 0 ? GameDefines.textColorGreen : GameDefines.textColorRed, ItemInfoUtility.GetAttribDisplayString(attribute.Key, Math.Abs(attribute.Value))) + "\n";
		}

		attributeChangeLabel.Text = attributeChanges;
	}

	private List<AttributeCalculator.Attribute> GetCheerAvatarAttribute(int parterId, KodGames.ClientClass.Avatar avatar)
	{
		if (avatar == null)
			return null;

		var avatarData = PlayerDataUtility.GetCalculatorAvatar(avatar, IDSeg.InvalidId, IDSeg.InvalidId, SysLocalDataBase.Inst.LocalPlayer, false);
		avatarData.partnerId = parterId;

		return PlayerDataUtility.GetAvatarAttributesForAssistant(avatarData);
	}

	private void SetAvatarDesc(KodGames.ClientClass.Avatar avatar, bool newAvatar)
	{
		SpriteText nameLabel = newAvatar ? newNameLabel : oldNameLabel;
		SpriteText lvLabel = newAvatar ? newLvLabel : oldLvLabel;
		SpriteText breakLabel = newAvatar ? newBreakLable : oldBreakLable;

		nameLabel.Text = string.Empty;
		lvLabel.Text = string.Empty;
		breakLabel.Text = string.Empty;

		if (avatar == null)
			return;

		nameLabel.Text = GameUtility.FormatUIString("UIDlgPartnerComparison_Name", C_Name, C_Value, ItemInfoUtility.GetAssetName(avatar.ResourceId));
		lvLabel.Text = GameUtility.FormatUIString("UIDlgPartnerComparison_Lvl", C_Name, C_Value, avatar.LevelAttrib.Level);
		breakLabel.Text = GameUtility.FormatUIString("UIDlgPartnerComparison_BreakLvl", C_Name, C_Value, avatar.BreakthoughtLevel);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}