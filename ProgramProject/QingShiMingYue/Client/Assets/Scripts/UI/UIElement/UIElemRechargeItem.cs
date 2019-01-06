using UnityEngine;
using System;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemRechargeItem : MonoBehaviour
{
	public UIBox normalCardBox;
	public UIBox monthCardBox;
	public SpriteText rechargeRMBLabel;
	public SpriteText rechargeRealMoneyLabel;
	public SpriteText rechargeExtraRealMoneyLabel;
	public SpriteText monthcardContxt;
	public SpriteText monthcardRMBLable;
	public UIElemAssetIcon icon;
	public SpriteText rechargeExtraRealMoneyTitleLabel;
	public UIBox rechargeExtraRealMoneyBox;	//控制不显示奖励时隐藏的空间
	public UIButton rechargeBtn;
	public UIButton monthCardBtn;

	private float monthCardHeight;
	public void Awake()
	{
		monthCardHeight = monthCardBox.height;
	}

	public void SetData(MonthCardConfig.MonthCard mothCardCfg, float costRMB, string nameCon, int currencyType)
	{
		SetCardUI(true);

		// Set Icon.
		icon.SetData(mothCardCfg.monthCardIconId);

		// Set Name and Money Label.
		monthcardRMBLable.Text = costRMB.ToString(ItemInfoUtility.GetDecimalMedianByType(currencyType));
		monthcardContxt.Text = nameCon;

		// Set Button Data.
		monthCardBtn.data = mothCardCfg;
	}

	public void SetData(int realMoneyCount, int realMoneyCountExtra, float costRMB, int goodId, int rewardForFirst, int firstMultiple, int currencyType)
	{
		SetCardUI(false);

		// Init Hide.
		rechargeExtraRealMoneyLabel.Hide(true);
		rechargeExtraRealMoneyBox.Hide(true);
		rechargeExtraRealMoneyTitleLabel.Hide(true);

		// Set Name and Money Label.
		rechargeRMBLabel.Text = costRMB.ToString(ItemInfoUtility.GetDecimalMedianByType(currencyType));
		rechargeRealMoneyLabel.Text = GameUtility.FormatUIString("UIDlgRecharge_ShowMoney", realMoneyCount);

		if (realMoneyCountExtra > 0)
		{
			rechargeExtraRealMoneyLabel.Hide(false);
			rechargeExtraRealMoneyBox.Hide(false);
			rechargeExtraRealMoneyTitleLabel.Hide(false);
			rechargeExtraRealMoneyLabel.Text = GameUtility.FormatUIString("UIDlgRecharge_ShowMoney", realMoneyCountExtra);
		}

		if (rewardForFirst > 0)
		{
			rechargeExtraRealMoneyLabel.Hide(false);
			rechargeExtraRealMoneyBox.Hide(false);
			rechargeExtraRealMoneyTitleLabel.Hide(false);
			rechargeExtraRealMoneyLabel.Text = GameUtility.FormatUIString("UIDlgRecharge_Add_toOne", rewardForFirst + realMoneyCountExtra * firstMultiple);
		}

		// Set Button Data.
		rechargeBtn.data = goodId;
	}

	private void SetCardUI(bool isMonthCard)
	{
		monthCardBox.gameObject.SetActive(isMonthCard);
		normalCardBox.gameObject.SetActive(!isMonthCard);

		// Set MonthCard Size.
		monthCardBox.SetSize(monthCardBox.width, isMonthCard ? monthCardHeight : normalCardBox.height);
	}
}
