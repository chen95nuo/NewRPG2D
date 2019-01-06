using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendTitleRootItem : UIListItemContainerEx
{
	private UIElemFriendTitleItem uiItem;
	private string titleLabel;
	private bool shongTongBox;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendTitleItem>();

			SetData(titleLabel, shongTongBox);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(string tl, bool st)
	{
		this.titleLabel = tl;
		this.shongTongBox = st;

		if (uiItem == null)
			return;

		uiItem.titleText.Text = tl;
		uiItem.shoutongBox.Hide(!st);
	}

}
