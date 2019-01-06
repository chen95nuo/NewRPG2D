using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTravelShopGuid : MonoBehaviour
{
	public UIBox shopEmptyBg;
	public SpriteText shopDescLabel;
	public List<UIElemAssetIcon> shopRewardIcons;
	public UIButton gotoBtn;

	public void SetData(CampaignConfig.TravelTrader travel)
	{
		var travelData = SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonTravelDataByDungeonId(travel.dungeonId);

		bool isAllSelled = true;
		if (travelData != null)
		{
			foreach (var shopRewardId in travel.canBuyGoodsIds)
			{
				if (!travelData.AlreadyBuyGoodIds.Contains(shopRewardId))
				{
					isAllSelled = false;
					break;
				}
			}
		}
		else
			isAllSelled = false;
		
		int enterLevel = 0;
		foreach (var diff in ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(travel.zoneId).dungeonDifficulties)
		{
			if (diff.difficultyType == ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(travel.dungeonId).DungeonDifficulty)
			{
				enterLevel = diff.levelLimit;
				break;
			}
		}

		// Set Shop State.
		shopEmptyBg.Hide(!isAllSelled);

		// Set Shop Desc.
		shopDescLabel.Text = GameUtility.FormatUIString(
								"UIPnlCampaign_TravelShopGuid_DescLabel",
								GameDefines.textColorBtnYellow,
								CampaignData.GetZoneIndexInCfg(travel.zoneId),
								GameDefines.textColorBtnYellow_font,
								ItemInfoUtility.GetAssetName(travel.zoneId),
								GameDefines.textColorBtnYellow,
								enterLevel);


		// Set Reward.
		List<int> rewardIds = new List<int>();
		foreach (var travelGoodId in travel.canBuyGoodsIds)
		{
			var travelGoodCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetTravelGoodById(travelGoodId);
			if (travelGoodCfg == null)
				continue;

			foreach (var reward in travelGoodCfg.rewards)
			{
				if (IDSeg.ToAssetType(reward.id) != IDSeg._AssetType.Avatar)
					continue;

				if (rewardIds.Contains(reward.id))
					continue;

				rewardIds.Add(reward.id);
			}
		}

		foreach (var rewardIcon in shopRewardIcons)
			rewardIcon.Hide(true);

		for (int index = 0; index < rewardIds.Count; index++)
		{
			shopRewardIcons[index].Hide(false);
			shopRewardIcons[index].SetData(rewardIds[index]);
			shopRewardIcons[index].border.Text = ItemInfoUtility.GetAssetName(rewardIds[index]);
		}

		// Set Goto Button State.
		string dungeonErrorMsg = CampaignData.CheckDungeonEnterErrorMsg(travel.dungeonId, false);
		if (string.IsNullOrEmpty(dungeonErrorMsg))
			UIUtility.CopyIcon(gotoBtn, UIElemTemplate.Inst.disableStyleClickableBtnTemplate.bigButton1Normal);
		else
			UIUtility.CopyIcon(gotoBtn, UIElemTemplate.Inst.disableStyleClickableBtnTemplate.bigButton1);

		gotoBtn.Data = travel.dungeonId;
	}
}