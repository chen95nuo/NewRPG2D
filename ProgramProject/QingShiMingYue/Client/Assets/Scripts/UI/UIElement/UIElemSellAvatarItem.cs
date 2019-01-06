using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemSellAvatarItem : UIElemSellBase
{
	public UIElemAssetIcon itemIcon;
	//public UIElemAssetIcon itemPriceLabel;
	public List<UIElemAssetIcon> itemRewards;

	public SpriteText itemNameLabel;
	public SpriteText itemQualityLabel;
	public SpriteText itemGrowthLabel;

	private KodGames.ClientClass.Avatar avatar;
	public KodGames.ClientClass.Avatar Avatar { get { return avatar; } }

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		sellData = new SellData();
		sellData.SetData(avatar.Guid, avatar.ResourceId, avatar.BreakthoughtLevel);

		this.avatar = avatar;
		container.Data = this;

		// Set the Avatar icon and Data.
		itemIcon.SetData(avatar);
		itemIcon.Data = avatar.ResourceId;

		// Set the Avatar Name.
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);

		// Set the Avatar Quality.
		itemQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(avatar.ResourceId);

		// Set the Avatar GrownStr.
		itemGrowthLabel.Text = string.Format(GameUtility.GetUIString("UI_Growth"), ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).growthDesc);

		// Set the Price Icon and value.
		for (int i = 0; i < itemRewards.Count; i++)
		{
			itemRewards[i].Hide(true);
		}

		var sellRewards = UIPnlPackageSell.GetAvatarSellRewards(avatar);
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
