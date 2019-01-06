using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendCampaginShipsRootItem : UIListItemContainerEx
{
	private UIElemFriendCampaginShipsItem uiItem;

	private com.kodgames.corgi.protocol.FCPointInfo pointInfo;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendCampaginShipsItem>();

			SetData(pointInfo);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(com.kodgames.corgi.protocol.FCPointInfo pointInfo)
	{
		this.pointInfo = pointInfo;

		if (uiItem == null)
			return;

		Color color;
		if (pointInfo.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			color = GameDefines.cardColorChenSe;
		else
			color = GameDefines.textColorGray;

		uiItem.left.Text = color + pointInfo.playerName;
		uiItem.center.Text = color + pointInfo.stageCount.ToString();
		uiItem.right.Text = color + pointInfo.point.ToString();
	}
}
