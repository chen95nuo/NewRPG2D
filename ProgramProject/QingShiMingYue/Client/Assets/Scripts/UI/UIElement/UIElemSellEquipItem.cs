using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemSellEquipItem : UIElemSellBase
{
	public UIElemAssetIcon itemIcon;
	//public UIElemAssetIcon itemPriceLabel;
	public List<UIElemAssetIcon> itemRewards;

	public SpriteText itemQualityLabel;
	public SpriteText itemNameLabel;
	public AutoSpriteControlBase itemAttrIcon;
	public List<SpriteText> attributeLabels;

	private KodGames.ClientClass.Equipment equipment;
	public KodGames.ClientClass.Equipment Equipment { get { return equipment; } }

	public void SetData(KodGames.ClientClass.Equipment equipment)
	{
		sellData = new SellData();
		sellData.SetData(equipment.Guid, equipment.ResourceId, equipment.BreakthoughtLevel);

		this.equipment = equipment;
		container.Data = this;

		// Set Equip Icon and Data.
		itemIcon.SetData(equipment);
		itemIcon.Data = equipment.ResourceId;

		// Set the Equip name.
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(equipment.ResourceId);

		// Set Equip Quality.
		itemQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(equipment.ResourceId);

		// Set Equip Attribute.
		var equipmentAttrs = PlayerDataUtility.GetEquipmentAttributes(equipment);
		int i = 0;
		for (; i < equipmentAttrs.Count && i < attributeLabels.Count; i++)
			attributeLabels[i].Text = ItemInfoUtility.GetAttributeNameValueString(equipmentAttrs[i].type, equipmentAttrs[i].value);

		for (; i < attributeLabels.Count; i++)
			attributeLabels[i].Text = string.Empty;

		// Set the Price Icon and value.
		for (int j = 0; j < itemRewards.Count; j++)
		{
			itemRewards[j].Hide(true);
		}

		//int index = 0;
		//int priceId = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel).breakThrough.ItemSellItemId;
		//if (priceId != IDSeg.InvalidId)
		//{
		//    itemRewards[index].Hide(false);
		//    itemRewards[index].SetData(priceId);
		//    itemRewards[index].border.Text = MathFactory.ExpressionCalculate.GetValue_EquipSellPrice(equipment).ToString();
		//    index++;
		//}

		//// Set the Reward Icon and value
		//var sellRewards = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel).sellRewards;
		//for (; index < sellRewards.Count + 1 && index < itemRewards.Count; index++)
		//{
		//    var sellReward = sellRewards[index];
		//    if (sellReward.count == 0)
		//        continue;

		//    itemRewards[index].Hide(false);
		//    itemRewards[index].SetData(sellReward.id);
		//    itemRewards[index].border.Text = sellReward.count.ToString();
		//    index++;
		//}

		var sellRewards = UIPnlPackageSell.GetEquipmentSellRewards(equipment);
		sellRewards.Sort((m, n) =>
		{
			return m.id - n.id;
		});

		for (int index = 0; index < sellRewards.Count && index < itemRewards.Count; index++)
		{
			var sellReward = sellRewards[index];
			if (sellReward.count == 0)
				continue;

			itemRewards[index].Hide(false);
			itemRewards[index].SetData(sellReward.id);
			itemRewards[index].border.Text = sellReward.count.ToString();
			index++;
		}

		// Set the itemIconBg 's icon and Data.
		UIElemTemplate.Inst.listItemBgTemplate.SetListItemBg(itemIconBg, false);
		itemIconBg.data = this;

		itemSelected.SetState(false);
	}
}
