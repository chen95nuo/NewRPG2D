using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIElemSellSkillItem : UIElemSellBase
{
	public UIElemAssetIcon itemIcon;
	public UIElemAssetIcon itemPriceLabel;
	public SpriteText itemNameLabel;
	public SpriteText itemQualityLabel;
	public List<SpriteText> attributeLabels;

	private KodGames.ClientClass.Skill skill;
	public KodGames.ClientClass.Skill Skill { get { return skill; } }

	public void SetData(KodGames.ClientClass.Skill skill)
	{
		sellData = new SellData();
		sellData.SetData(skill.Guid, skill.ResourceId, skill.LevelAttrib.Level);

		container.Data = this;
		this.skill = skill;

		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
		if (skillCfg == null)
		{
			Debug.LogError("SkillCfg Not Found Id " + skill.ResourceId.ToString("X"));
			return;
		}

		// Set the Skill icon and Data.
		itemIcon.SetData(skill);
		itemIcon.Data = skill.ResourceId;

		// Set the Skill Name.
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(skill.ResourceId);

		// Set the Skill Quality.
		itemQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(skill.ResourceId);

		// Set Attribute.
		for (int i = 0; i < attributeLabels.Count; i++)
			attributeLabels[i].Text = string.Empty;

		var modifiers = skillCfg.GetLevelModifers(skill.LevelAttrib.Level);
		if (modifiers != null)
		{
			var attributes = PlayerDataUtility.MergeAttributes(modifiers, true, true);

			for (int i = 0; i < attributes.Count && i < attributeLabels.Count; i++)
				attributeLabels[i].Text = GameUtility.FormatUIString(
						"UIDlgAttributeDetailTip_AttributeDetail",
						GameDefines.textColorBtnYellow,
						_AvatarAttributeType.GetDisplayNameByType(attributes[i].type, ConfigDatabase.DefaultCfg),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
		}

		// Set the Price icon and value.
		var sellRewards = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).GetSellRewards(skill.LevelAttrib.Level);
		int priceId = IDSeg.InvalidId;
		if (sellRewards != null && sellRewards.Count > 0)
			priceId = sellRewards[0].id;

		itemPriceLabel.SetData(priceId);
		itemPriceLabel.border.Text = sellRewards[0].count.ToString();

		// Set the itemIconBg 's icon and Data.
		UIElemTemplate.Inst.listItemBgTemplate.SetListItemBg(itemIconBg, false);
		itemIconBg.data = this;

		itemSelected.SetState(false);
	}
}
