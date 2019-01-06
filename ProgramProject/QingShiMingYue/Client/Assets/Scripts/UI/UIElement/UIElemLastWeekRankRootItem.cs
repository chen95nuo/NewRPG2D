using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLastWeekRankRootItem : UIListItemContainerEx
{
	private UIElemLastWeekRankItem uiItem;

	private com.kodgames.corgi.protocol.FCRankInfo rankInfo;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemLastWeekRankItem>();

			SetData(rankInfo);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(com.kodgames.corgi.protocol.FCRankInfo rankInfo)
	{
		this.rankInfo = rankInfo;

		if (uiItem == null)
			return;

		UIElemTemplate elemTemplate = SysUIEnv.Instance.GetUIModule<UIElemTemplate>();
		elemTemplate.friendcampaginTemplate.SetFriendCampaginRankBg(uiItem.playerbackbg, rankInfo.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId);

		uiItem.rankPlayerName.Text = rankInfo.playerName;
		uiItem.number.Text = rankInfo.rank.ToString();
		uiItem.friendship.Text = GameUtility.FormatUIString("UIPnlFriendCampaginLastWeekRank_Ships_Number", rankInfo.point);
		uiItem.playerView.Data = rankInfo.playerId;
		uiItem.powerLabel.Text = GameUtility.FormatUIString("UIPnlRank_Power_Label", PlayerDataUtility.GetPowerString(rankInfo.power));
	}
}
