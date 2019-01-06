using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemLevelBattleFirstResult : MonoBehaviour
{
	public SpriteText levelLabel;
	public SpriteText pointLabel;
	public SpriteText gameMoneyLabel;

	public List<UIElemAssetIcon> rewardIcons;

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
		
		for (int index = 0; index < rewardIcons.Count; index++)
			rewardIcons[index].Hide(true);

		for (int i = 0; i < floorCfg.FirstPassReward.Count && i < rewardIcons.Count; i++)
		{
			rewardIcons[i].Hide(false);
			rewardIcons[i].Data = floorCfg.FirstPassReward[i].id;
			rewardIcons[i].SetData(floorCfg.FirstPassReward[i].id, floorCfg.FirstPassReward[i].count);
		}
	}
}
