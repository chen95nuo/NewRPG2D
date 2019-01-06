using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendBattleRewardItemRoot : UIListItemContainerEx
{
	private UIElemFriendBattleRewardItem uiItem;

	private int assetId;
	private int assetCount;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendBattleRewardItem>();

			SetData(assetId, assetCount);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(int assetId, int assetCount)
	{
		if (uiItem == null || assetId == IDSeg.InvalidId)
			return;

		this.assetId = assetId;
		this.assetCount = assetCount;

		uiItem.assetIcon.Hide(false);
		uiItem.assetIcon.SetData(assetId, assetCount);
	}
}
