using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIElemPackageItemItem : MonoBehaviour
{
	public UIElemAssetIcon itemIcon;
	public UIButton openBtn;
	public SpriteText itemNameText;
	public SpriteText itemDescText;
	public SpriteText itemLevelReqText;
	public UIListItemContainer container;

	private Consumable consumable;
	public Consumable Consumable { get { return consumable; } }

	private readonly Color MEET_LVL_REQ = GameDefines.txColorWhite;
	private readonly Color NOT_MEET_LVL_REQ = GameDefines.txColorRed;

	public void SetData(Consumable consumable)
	{
		// Set Assistant Data.
		openBtn.GetComponent<UIElemAssistantBase>().assistantData = consumable.Id;

		container.Data = this;
		this.consumable = consumable;
		openBtn.IndexData = consumable.Id;

		RefreshView();

		ItemConfig.Item itemCfg = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(consumable.Id);

		if (itemCfg == null)
			Debug.LogError("Invalid Item " + consumable.Id.ToString("X"));

		// Set Consumable Icon and Data.
		itemIcon.SetData(consumable);
		itemIcon.Data = this;

		// Set Consumable Name.
		itemNameText.Text = ItemInfoUtility.GetAssetName(consumable.Id);

		// Get Consumable Desc.
		itemDescText.Text = ItemInfoUtility.GetAssetDesc(consumable.Id);

		// Set consumable level requirement desc
		itemLevelReqText.Text = "";
		itemLevelReqText.Text = ItemInfoUtility.GetItemLevelReqDesc(MEET_LVL_REQ, NOT_MEET_LVL_REQ, consumable.Id);

		// Set the openButton's state and Data.
		bool hasConsumReward = itemCfg.HasConsumeReward() && itemCfg.vipLevel <= SysLocalDataBase.Inst.LocalPlayer.VipLevel && itemCfg.playerLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;

		if (itemCfg.type == ItemConfig._Type.IllusionCostItem || itemCfg.id == ConfigDatabase.DefaultCfg.ItemConfig.illusionStoneId)
			hasConsumReward = true;

		openBtn.Hide(!hasConsumReward);
		if (hasConsumReward)
		{
			openBtn.data = this;

			int consumabelType = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(consumable.Id).type;
			if (consumabelType == ItemConfig._Type.Package || consumabelType == ItemConfig._Type.Gacha)
				openBtn.Text = GameUtility.GetUIString("UIPackage_Item_Button_Open");
			else
				openBtn.Text = GameUtility.GetUIString("UIPackage_Item_Button_Use");
		}
	}

	public void RefreshView()
	{
		// Set the consumable count.
		itemIcon.rightLable.Text = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(consumable.Id).Amount.ToString();
	}
}
