using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendLabelRootItem : UIListItemContainerEx
{
	private UIElemFriendLabelItem uiItem;

	private ClientServerCommon.SubType subType;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendLabelItem>();

			SetData(subType);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(ClientServerCommon.SubType sub)
	{
		this.subType = sub;

		if (uiItem == null)
			return;

		uiItem.titleLabel.Text = string.Empty;
		uiItem.desc.Text = string.Empty;

		if (sub == null)
			return;

		uiItem.titleLabel.Text = sub.name;
		uiItem.desc.Text = sub.desc;
	}
}
