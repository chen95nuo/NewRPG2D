using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerThisWeekRank : MonoBehaviour
{
	public SpriteText getLayerLabel;
	public SpriteText playerName;
	public SpriteText rankLabel;
	public SpriteText pointLabel;
	public SpriteText playerPowerLabel;
	public UIButton lineUpBtn;
	public UIButton playerBg;

	public void SetData(com.kodgames.corgi.protocol.MfRankInfo rankInfo, bool isMyRank)
	{
		UIElemTemplate elemTemplate = SysUIEnv.Instance.GetUIModule<UIElemTemplate>();
		elemTemplate.towerRankTemplate.SetTowerRankBg(playerBg, isMyRank);

		getLayerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerRank_Label_LayerCount"), rankInfo.arrivalLayer);
		playerName.Text = rankInfo.playerName;
		rankLabel.Text = rankInfo.rank.ToString();
		pointLabel.Text = rankInfo.point.ToString();
		lineUpBtn.Data = rankInfo;
		playerPowerLabel.Text = PlayerDataUtility.GetPowerString(rankInfo.power);
	}
}
