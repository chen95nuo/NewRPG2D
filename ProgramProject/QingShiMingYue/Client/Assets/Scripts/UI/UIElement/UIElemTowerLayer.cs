using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerLayer : MonoBehaviour
{
	public SpriteText layerLabel;
	public SpriteText rewardLabel1;
	public SpriteText rewardLabel2;

	public void SetData(int layer, bool isWin)
	{
		layerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_TowerLayer_Label"), layer.ToString());

		MelaleucaFloorConfig.Floor floor = new MelaleucaFloorConfig.Floor();

		int maxLayer = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Floors.Count;

		if (layer > maxLayer)
			floor = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(maxLayer);
		else
			floor = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(layer);
		
		if (floor.PassReward.Count > 0)
		{
			if (isWin)
				rewardLabel1.Text = string.Format(GameUtility.GetUIString("UIPnlTowerSweepBattle_Reward_Label"),
					ItemInfoUtility.GetAssetName(floor.PassReward[0].id),
					floor.PassReward[0].count);
			else
				rewardLabel1.Text = string.Format(GameUtility.GetUIString("UIPnlTowerSweepBattle_Reward_Label"),
					ItemInfoUtility.GetAssetName(floor.PassReward[0].id), 0);
		}
		else
			rewardLabel1.Text = string.Empty;

		if (floor.PassReward.Count > 1)
		{
			if (isWin)
				rewardLabel2.Text = string.Format(GameUtility.GetUIString("UIPnlTowerSweepBattle_Reward_Label"),
					ItemInfoUtility.GetAssetName(floor.PassReward[1].id),
					floor.PassReward[1].count);
			else
				rewardLabel2.Text = string.Format(GameUtility.GetUIString("UIPnlTowerSweepBattle_Reward_Label"),
					ItemInfoUtility.GetAssetName(floor.PassReward[1].id), 0);
		}
		else
			rewardLabel2.Text = string.Empty;
	}
}
