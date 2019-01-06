using UnityEngine;
using System;
using ClientServerCommon;
using System.Collections.Generic;

public class UIElemShopWineItem : MonoBehaviour
{
	public UIElemAssetIcon recruitIcon;
	public UIElemAssetIcon qualityIcon;
	public UIElemAssetIcon countryIcon;
	public UIElemAssetIcon rewardIcon;
	public UIElemAssetIcon goldIcon;
	public UIElemAssetIcon tenGoldIcon;

	public SpriteText tenGoldLabel;
	public SpriteText goldRecruitCostLabel;
	public SpriteText firstRecruitLabel;
	public SpriteText firstTenRecruitLabel;
	public SpriteText freeRecruitTimesLabel;
	public SpriteText levelLimte;

	public UIButton recruitBtn;
	public SpriteText recruitReward;

	public UIButton recruitTenBtn;
	public UIBox recruitStage;
	public SpriteText recruitTenReward;

	public UIButton rewardShowBtn;
	public UIButton accidentRewardBtn;
	public int tavernId;
	private float percentageValue;

	public void OnDisable()
	{
		recruitBtn.GetComponent<UIElemAssistantBase>().assistantData = 0;
	}

	public void SetData(int tavernId, List<int> sepicalRewardIds,com.kodgames.corgi.protocol.TavernInfo travernInfo)
	{
		// Set Assistant Data.
		recruitBtn.GetComponent<UIElemAssistantBase>().assistantData = tavernId;
		this.tavernId = tavernId;
		//this.container = container;
		var tavernConfig = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavernId);

		if (travernInfo.tavernRewardInfo != null && travernInfo.tavernRewardInfo.reward != null)
			recruitReward.Text = GameUtility.FormatUIString("UIPnlBar_JiejiaoReward", GameDefines.textColorBtnYellow.ToString(),
												ItemInfoUtility.GetAssetName(travernInfo.tavernRewardInfo.reward.id),
												GameDefines.textColorWhite.ToString(),
												travernInfo.tavernRewardInfo.reward.amount);
		else
			recruitReward.Text = string.Empty;

		if (travernInfo.tavernRewardInfo != null && travernInfo.tavernRewardInfo.tenTavernReward != null)
			recruitTenReward.Text = GameUtility.FormatUIString("UIPnlBar_JiejiaoReward", GameDefines.textColorBtnYellow.ToString(),
												ItemInfoUtility.GetAssetName(travernInfo.tavernRewardInfo.tenTavernReward.id),
												GameDefines.textColorWhite.ToString(),
												travernInfo.tavernRewardInfo.tenTavernReward.amount);
		else
			recruitTenReward.Text = string.Empty;

		//Set TavernItem Icon
		recruitIcon.SetData(tavernConfig.BackGroundId);
		qualityIcon.SetData(tavernConfig.QualityIconId);
		countryIcon.SetData(tavernConfig.CountyIconId);
		rewardIcon.SetData(tavernConfig.Reward.id);
		goldIcon.SetData(tavernConfig.Cost.id);

		if (sepicalRewardIds != null && sepicalRewardIds.Count > 0)
		{
			accidentRewardBtn.gameObject.SetActive(true);
			accidentRewardBtn.Data = new object[] { sepicalRewardIds, tavernConfig.SepicalRewardContext };
		}
		else
			accidentRewardBtn.gameObject.SetActive(false);

		//Set Ten Button.
		rewardIcon.border.Text = tavernConfig.Reward.count.ToString();
		if (tavernConfig.CanTenTavern)
		{
			recruitTenBtn.gameObject.SetActive(true);
			tenGoldIcon.SetData(tavernConfig.TenTavernCost.id);
			recruitStage.Hide(tavernConfig.RecruitStage != _TavernRecruitStage.Sale);

			//Set Recruit MoneyCost
			if (tavernConfig.Cost.count < 10000)
				tenGoldLabel.Text = tavernConfig.TenTavernCost.count.ToString();
			else
				tenGoldLabel.Text = string.Format(GameUtility.GetUIString("ConsumableCount"), (tavernConfig.TenTavernCost.count / 10000).ToString());
		}
		else
		{
			recruitTenBtn.gameObject.SetActive(false);
			recruitStage.Hide(true);
		}

		//Set Recruit MoneyCost
		if (tavernConfig.Cost.count < 10000)
			goldRecruitCostLabel.Text = tavernConfig.Cost.count.ToString();
		else
			goldRecruitCostLabel.Text = string.Format(GameUtility.GetUIString("ConsumableCount"), (tavernConfig.Cost.count / 10000).ToString());

		//Set Init FreeRecruit Label
		freeRecruitTimesLabel.Text = string.Empty;

		//Set Recruit Group data
		recruitBtn.Data = tavernConfig;
		rewardShowBtn.Data = tavernConfig;
		recruitTenBtn.Data = tavernConfig;
		//Set Recruit limit.
		string levelLimteStr = string.Empty;

		if (tavernConfig.Level > 0)
			levelLimteStr += string.Format(GameUtility.GetUIString("UIPnlShopWine_LevelLimit"), tavernConfig.Level.ToString());
		if (tavernConfig.VipLevel > 0)
		{
			if (string.IsNullOrEmpty(levelLimteStr))
				levelLimteStr += string.Format(GameUtility.GetUIString("UIPnlShopWine_VipLimitOnly"), tavernConfig.VipLevel.ToString());
			else
				levelLimteStr += string.Format(GameUtility.GetUIString("UIPnlShopWine_VipLimit"), tavernConfig.VipLevel.ToString());
		}

		levelLimte.Text = levelLimteStr;

		RefreshDynamicUI(travernInfo);

		UpdateFreeRecruitCountDownMessage();
	}

	public void RefreshDynamicUI(com.kodgames.corgi.protocol.TavernInfo tempTavernInfo)
	{
		var tavernInfo = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetTavernInfoById(tavernId);
		var tavernConfig = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavernId);

		if (tavernInfo.isFirstMoneyBuy)
			firstRecruitLabel.Text = tavernConfig.NormalFirstDesc;
		else
			firstRecruitLabel.Text = string.Empty;

		if (tavernInfo.alreadyTenTavern)
			firstTenRecruitLabel.Text = string.Empty;
		else
			firstTenRecruitLabel.Text = tavernConfig.TenTavernFristDesc;

		if (tempTavernInfo.tavernRewardInfo != null && tempTavernInfo.tavernRewardInfo.reward != null)
			recruitReward.Text = GameUtility.FormatUIString("UIPnlBar_JiejiaoReward", GameDefines.textColorBtnYellow.ToString(),
												ItemInfoUtility.GetAssetName(tempTavernInfo.tavernRewardInfo.reward.id),
												GameDefines.textColorWhite.ToString(),
												tempTavernInfo.tavernRewardInfo.reward.amount);
		else
			recruitReward.Text = string.Empty;

		if (tempTavernInfo.tavernRewardInfo != null && tempTavernInfo.tavernRewardInfo.tenTavernReward != null)
			recruitTenReward.Text = GameUtility.FormatUIString("UIPnlBar_JiejiaoReward", GameDefines.textColorBtnYellow.ToString(),
												ItemInfoUtility.GetAssetName(tempTavernInfo.tavernRewardInfo.tenTavernReward.id),
												GameDefines.textColorWhite.ToString(),
												tempTavernInfo.tavernRewardInfo.tenTavernReward.amount);
		else
			recruitTenReward.Text = string.Empty;

	}

	public void UpdateFreeRecruitCountDownMessage()
	{
		com.kodgames.corgi.protocol.TavernInfo tavernInfo = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetTavernInfoById(tavernId);
		ClientServerCommon.TavernConfig.Tavern tavernConfig = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavernInfo.id);

		// If has free recruit,update count down.
		if (tavernConfig.CoolDownTime > 0)
		{
			long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			long endTime = tavernInfo.nextFreeStartTime;
			if (nowTime > endTime)
			{
				// Time up, add free count
				tavernInfo.nextFreeStartTime = nowTime;
				tavernInfo.leftFreeCount = 1;
				//Set free Recruit Label
				string freeRecruitTimes = tavernInfo.leftFreeCount == 0 ? string.Empty : GameUtility.GetUIString("UIPnlShopWine_FreeRecruit_Count");

				if (freeRecruitTimesLabel.Text.Equals(freeRecruitTimes) == false)
					freeRecruitTimesLabel.Text = freeRecruitTimes;
			}

			//Set Cool Down Time Label
			else
			{
				if (tavernInfo.leftFreeCount > 0)
					freeRecruitTimesLabel.Text = GameUtility.GetUIString("UIPnlShopWine_FreeRecruit_Count");
				else //if (GameUtility.EqualsFormatTimeString(freeRecruitTimesLabel.Text, endTime - nowTime) == false)
					freeRecruitTimesLabel.Text = GameUtility.Time2String(endTime - nowTime);
				//GameUtility.FormatUIString("UIPnlShopWine_FreeRecruit_CountDown", GameDefines.txCountDownColor + GameUtility.Time2String(endTime - nowTime));
			}
		}
	}
}

