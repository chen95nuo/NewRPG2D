using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemLevelBattleResult : MonoBehaviour
{

	public SpriteText levelLabel;
	public SpriteText pointLabel;
	public SpriteText gameMoneyLabel;
	MelaleucaFloorConfig.Floor floorCfg;

	public void SetData(int layer)
	{
		levelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_TowerLayer_Label"), layer.ToString());
				
		int maxLayer = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Floors.Count;

		if (layer > maxLayer)
			floorCfg = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(maxLayer);
		else
			floorCfg = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(layer);


		if (floorCfg.PassReward.Count > 0)
			pointLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_Reward_Label"),GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(floorCfg.PassReward[0].id), GameDefines.textColorWhite, floorCfg.PassReward[0].count);
		else pointLabel.Text = string.Empty;

		if (floorCfg.PassReward.Count > 1)
			gameMoneyLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_Reward_Label"), GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(floorCfg.PassReward[1].id), GameDefines.textColorWhite, floorCfg.PassReward[1].count);
		else gameMoneyLabel.Text = string.Empty;

	}
}
