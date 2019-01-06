using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDungeonTravelShopItem : MonoBehaviour
{
	public SpriteText titleLabel;
	public List<UIElemAssetIcon> rewardIcons;
	public List<SpriteText> iconNames;
	public UIButton operatorButton;
	public UIElemAssetIcon costIcon;
	public UIBox fixRewardBox;

	private int dungeonId;
	public int DungeonId { get { return dungeonId; } }

	private CampaignConfig.TravelGood travelGood;
	public CampaignConfig.TravelGood TravelGood { get { return travelGood; } }

	private bool canUpdateView;

	public void Awake()
	{
		operatorButton.Data = this;
	}

	public void ClearData()
	{
		this.dungeonId = IDSeg.InvalidId;
		this.travelGood = null;
		this.canUpdateView = false;
	}

	public void SetData(int dungeonId, int needStarNum, CampaignConfig.TravelGood travelGood, KodGames.ClientClass.Dungeon dungeonRecord)
	{
		this.dungeonId = dungeonId;
		this.travelGood = travelGood;

		// Fill Reward Data.
		for (int i = 0; i < Math.Min(rewardIcons.Count, iconNames.Count); i++)
		{
			rewardIcons[i].Hide(true);
			iconNames[i].Hide(true);
		}

		int index = 0;
		for (; index < rewardIcons.Count && index < travelGood.rewards.Count; index++)
		{
			if (rewardIcons[index].IsHidden())
			{
				rewardIcons[index].Hide(false);
				iconNames[index].Hide(false);
			}

			rewardIcons[index].SetData(travelGood.rewards[index].id, travelGood.rewards[index].count);
			iconNames[index].Text = ItemInfoUtility.GetAssetName(travelGood.rewards[index].id);
		}

		// Set CostIcon.
		costIcon.SetData(travelGood.costs[0].id);
		costIcon.border.Text = travelGood.costs[0].count.ToString();

		// Set sign.
		if (dungeonRecord == null || dungeonRecord.BestRecord < needStarNum)
		{
			operatorButton.gameObject.SetActive(true);
			operatorButton.controlIsEnabled = false;
			fixRewardBox.Hide(true);
			canUpdateView = false;
			titleLabel.Text = travelGood.continueTime <= 0 ?
				GameUtility.FormatUIString("UIPnlCampaign_TravelShop_ItemTitleStatic", needStarNum) :
				GameUtility.FormatUIString("UIPnlCampaign_TravelShop_ItemTitle", GameUtility.Time2String(travelGood.continueTime * KodGames.TimeEx.cMillisecondInSecend));
		}
		else
		{
			operatorButton.controlIsEnabled = true;
			canUpdateView = true;
			// Update View.
			UpdateView();
		}
	}

	public void UpdateView()
	{
		if (this.dungeonId == IDSeg.InvalidId || this.travelGood == null || !this.canUpdateView)
			return;

		var travelData = SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonTravelDataByDungeonId(this.dungeonId);
		if (travelData.AlreadyBuyGoodIds.Contains(this.travelGood.goodId))
		{
			canUpdateView = false;
			titleLabel.Text = string.Empty;
			operatorButton.gameObject.SetActive(false);
			fixRewardBox.Hide(false);
		}
		else
		{
			if (!operatorButton.gameObject.activeInHierarchy)
				operatorButton.gameObject.SetActive(true);

			if (!fixRewardBox.IsHidden())
				fixRewardBox.Hide(true);

			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			if (travelGood.continueTime <= 0)
			{
				if (!string.IsNullOrEmpty(titleLabel.Text))
					titleLabel.Text = string.Empty;
			}
			else if (travelData.OpenTime + travelGood.continueTime * KodGames.TimeEx.cMillisecondInSecend >= nowTime)
			{
				titleLabel.Text = GameUtility.Time2String(travelData.OpenTime + travelGood.continueTime * KodGames.TimeEx.cMillisecondInSecend - nowTime);
			}
			else
			{
				canUpdateView = false;
				titleLabel.Text = GameUtility.GetUIString("UIPnlCampaign_TravelShop_ItemTitle_EndTime");
			}
		}
	}
}
