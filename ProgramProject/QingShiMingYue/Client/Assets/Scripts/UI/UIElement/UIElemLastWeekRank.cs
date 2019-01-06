using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLastWeekRank : MonoBehaviour
{
	public SpriteText getLayerLabel;
	public SpriteText playerName;
	public SpriteText rankLabel;
	public SpriteText pointLabel;

	public void SetData(com.kodgames.corgi.protocol.MfRankInfo rankInfo)
	{
		getLayerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerRank_Label_LayerCount"), rankInfo.arrivalLayer);		
		playerName.Text = rankInfo.playerName;
		rankLabel.Text = rankInfo.rank.ToString();
		pointLabel.Text = rankInfo.point.ToString();
	}
}
