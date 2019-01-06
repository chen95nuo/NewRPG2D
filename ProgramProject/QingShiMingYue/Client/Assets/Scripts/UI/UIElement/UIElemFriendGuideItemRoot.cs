using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendGuideItemRoot : UIListItemContainerEx
{
	//专门用来管理预制品的
	private UIElemFriendGuideItem uiItem;
	private ClientServerCommon.MainType uiData;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendGuideItem>();

			SetData(uiData);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(ClientServerCommon.MainType mainTypeItem)
	{
		this.uiData = mainTypeItem;

		if (uiItem == null || mainTypeItem == null)
			return;

		uiItem.guideInfoBtn.Data = mainTypeItem;
		uiItem.guideTitleLabel.Text = mainTypeItem.name;
	}
}
