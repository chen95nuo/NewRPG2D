//#define MONTHCARD_LOG
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;

public class UIElemMonthCardItem : MonoBehaviour
{
	public SpriteText costMoney;
	public SpriteText leftUseCount;
	public SpriteText cardName;
	public SpriteText tenTimeDesc;

	public UIElemAssetIcon cardIcon;
	public UIElemAssetIcon buyRewardIcon;
	public UIElemAssetIcon dailyRewardIcon;
	public UIElemAssetIcon tenTimeRewardIcon;

	public UIButton takeBtn, buyBtn;
	public UIElemProgressItem tenTimeProgress;
	public UIBox cardTypeStateBox;
	public UIBox tasteTex;//体验

	public UIBox notifyIcon;//有奖励可领时

	private const int maxCount = 5;
	private MonthCardConfig.MonthCard monthCardCfg;
	public com.kodgames.corgi.protocol.OneMonthCardInfo myMonthCardInfo;

	public void SetData(com.kodgames.corgi.protocol.OneMonthCardInfo monthcardInfo)
	{
		monthCardCfg = ConfigDatabase.DefaultCfg.MonthCardConfig.GetMonthCardById(monthcardInfo.monthCardId);
		var appleGoodConfig = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(monthCardCfg.goodsId, GameUtility.GetDeviceInfo().DeviceType);
		if (appleGoodConfig == null)
			appleGoodConfig = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(monthCardCfg.goodsId, _DeviceType.Unknown);

		// Set Assistant Data.
		takeBtn.gameObject.GetComponent<UIElemAssistantBase>().assistantData = monthCardCfg.id;

		Init();

		tenTimeDesc.Text = GameUtility.GetUIString("UIElemMonthCardItem_TenTimeDesc");//十次奖励进度

		myMonthCardInfo = monthcardInfo;

		cardIcon.SetData(monthCardCfg.monthCardIconId);

		buyRewardIcon.SetData(monthCardCfg.buyRewardAndIcon.iconId);
		dailyRewardIcon.SetData(monthCardCfg.dailyRewardAndIcon.iconId);
		tenTimeRewardIcon.SetData(monthCardCfg.tenRewardAndIcon.iconId);

		float rmb = appleGoodConfig.costRMB / (float)ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower;
		costMoney.Text = rmb.ToString(ItemInfoUtility.GetDecimalMedianByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType));

		leftUseCount.Text = GameUtility.FormatUIString("UIPnlActivityMonthCardInfo_TotalRemainCount", GameDefines.textColorBtnYellow, GameDefines.txColorWhite, monthcardInfo.remainDates, GameDefines.textColorBtnYellow);
		cardName.Text = monthCardCfg.name;

		//体验可以领，但优先领取购买奖励和10次奖励
		if (monthcardInfo.remainFreeCounts > 0 && !HaveTenOrBuyReward())
		{
			tasteTex.gameObject.SetActive(true);
			takeBtn.Text = GameUtility.FormatUIString("UIPnlActivityMonthCardInfo_GetRewardCout", monthcardInfo.remainFreeCounts);
		}
		else
		{
			tasteTex.gameObject.SetActive(false);
			takeBtn.Text = GameUtility.GetUIString("UIElemMonthCardItem_Get");
		}

#if MONTHCARD_LOG
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append(string.Format(" cardType:{0} \nmonthcardInfo.remainFreeCounts {1}", MonthCardType.GetNameByType(monthCardCfg.type), monthcardInfo.remainFreeCounts)).Append("\n");
		sb.Append(string.Format("buyRewardCount:{0}\n", monthcardInfo.buyRewardCount));
		sb.Append(string.Format("pickCounter {0}\n", monthcardInfo.pickCounter));
		sb.Append(string.Format("LastPickRewardDate:{0} remainDates:{1}", SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(myMonthCardInfo.lastPickTime).Date, myMonthCardInfo.remainDates));
		sb.Append("HasRewardToGet:" + HasRewardToGet() + '\n');
		sb.Append("myMonthCardInfo.isCouldPickDailyReward " + myMonthCardInfo.isCouldPickDailyReward);
		Debug.Log(sb.ToString());
#endif

		tenTimeProgress.SetMax(10);
		tenTimeProgress.Value = monthcardInfo.pickCounter;

		//目前只有两种月卡
		SetSuperScriptIcon(cardTypeStateBox, monthCardCfg.type);

		//没有奖励可领，disable按钮
		if (!HasRewardToGet())
			UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(takeBtn, true);
		else
			UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(takeBtn, false);

		notifyIcon.Hide(!HasRewardToGet());
	}

	public void SetSuperScriptIcon(AutoSpriteControlBase baseIcon, int cardType)
	{
		switch (cardType)
		{
			case MonthCardType.MONTH_CARD_TYPE_1:
				UIUtility.CopyIcon(baseIcon, UIElemTemplate.Inst.iconBorderTemplate.monthCard_type_1_Box);
				break;
			case MonthCardType.MONTH_CARD_TYPE_2:
				UIUtility.CopyIcon(baseIcon, UIElemTemplate.Inst.iconBorderTemplate.monthCard_type_2_Box);
				break;

			default:
				Debug.LogError(string.Format("Unsupported MonthCard Type: {0}", MonthCardType.GetNameByType(cardType)));
				break;
		}
	}

	//十次或购买奖励可以领
	bool HaveTenOrBuyReward()
	{
		//十次奖励可领
		if (myMonthCardInfo.pickCounter >= 10)
			return true;

		//购买奖励可领取
		if (myMonthCardInfo.buyRewardCount > 0)
			return true;

		return false;
	}

	//是否有奖励可以领取
	bool HasRewardToGet()
	{
		if (HaveTenOrBuyReward())
			return true;

		return myMonthCardInfo.isCouldPickDailyReward;
	}

	private void Init()
	{
		buyRewardIcon.Data = MonthCardRewardType.BuyReward;
		dailyRewardIcon.Data = MonthCardRewardType.DailyReward;
		tenTimeRewardIcon.Data = MonthCardRewardType.TenTimesReward;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardIconClick(UIButton btn)
	{
		UIElemAssetIcon icon = btn.data as UIElemAssetIcon;

		UnityEngine.MonoBehaviour.print("btn :" + icon.AssetId);
		SysUIEnv.Instance.ShowUIModule<UIPnlMonthCardDetail>(monthCardCfg, myMonthCardInfo);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBuyRewardInfoClick(UIButton btn)
	{
		UIElemAssetIcon icon = btn.Data as UIElemAssetIcon;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgMonthCardView), monthCardCfg, myMonthCardInfo, (int)icon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRewardInfoClick(UIButton btn)
	{
		if (!HasRewardToGet())
		{
			//没有奖励可领。按钮为Disable样式但是可以点击，点击后弹tips
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIElemMonthCardItem_Tip_NoRewardToGet"));
			return;
		}

		RequestMgr.Inst.Request(new MonthCardPickRewardReq(myMonthCardInfo.monthCardId, MonthCardRewardType.OneByOne));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBuyMonthCardClick(UIButton btn)
	{
		if (myMonthCardInfo.remainDates + monthCardCfg.durationDay > monthCardCfg.remainDaysLimit)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityMonthCardInfo_NotBuy"));
			return;
		}

		Platform.Instance.Pay(monthCardCfg.goodsId, monthCardCfg.id.ToString());
	}
}
