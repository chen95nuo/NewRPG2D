using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemBattleResultDungeonItem : MonoBehaviour
{
	public UIElemAssetIcon itemIcon;
	public SpriteText iconName;

	public void SetData(int rewardId, int count)
	{
		switch (IDSeg.ToAssetType(rewardId))
		{
			case IDSeg._AssetType.Item:
			case IDSeg._AssetType.Special:
				itemIcon.SetData(rewardId, count);
				break;
			default:
				itemIcon.SetData(rewardId);
				break;
		}

		iconName.Text = ItemInfoUtility.GetAssetName(rewardId);
	}
}