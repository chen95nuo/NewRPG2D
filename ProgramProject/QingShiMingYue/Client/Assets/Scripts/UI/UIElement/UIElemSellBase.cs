using System;
using System.Collections.Generic;

public class UIElemSellBase : MonoBehaviour
{
	public UIButton itemIconBg;

	public UIElemSelectItem itemSelected;

	public UIListItemContainer container;

	[UnityEngine.HideInInspector]
	public int indexInItemList;

	public class SellData
	{
		public string sellGUID;
		public int sellResourceID;
		public int sellCount;
		public int breakhroughOrUpgradeLevel;

		public SellData()
		{
			sellGUID = "";
			sellResourceID = ClientServerCommon.IDSeg.InvalidId;
			sellCount = 1;
		}

		public void SetData(string sellGUID, int sellResourceID, int breakhroughOrUpgradeLevel)
		{
			SetData(sellGUID, sellResourceID, breakhroughOrUpgradeLevel, 1);
		}

		public void SetData(string sellGUID, int sellResourceID, int breakhroughOrUpgradeLevel, int sellCount)
		{
			this.sellGUID = sellGUID;
			this.sellResourceID = sellResourceID;
			this.sellCount = sellCount;
			this.breakhroughOrUpgradeLevel = breakhroughOrUpgradeLevel;
		}

		public bool Equals(SellData sellData)
		{
			if (sellData == null)
				return false;

			return this.sellGUID.Equals(sellData.sellGUID) && this.sellResourceID == sellData.sellResourceID;
		}
	}

	public SellData sellData;

	public bool IsItemSelected()
	{
		return itemSelected.IsSelected;
	}

	public void SetToggleState(bool selected)
	{
		itemSelected.SetState(selected);

		//UIElemTemplate.Inst.listItemBgTemplate.SetListItemBg(itemIconBg, selected);
	}
}
