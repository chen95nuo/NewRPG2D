using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections.Generic;


public class UIPnlMonthCardDetail : UIModule
{
	public UIBox roleTex_1, roleTex_2;
	public UIBox titleState, typeState;
	public SpriteText descText, priceText;
	public UIButton buyRewardBtn, dailyRewardBtn, tentimeRewardBtn;

	public UIBox tenTimeRewardNotifyIcon, buyRewardNotifyIcon, dailyRewardNotifyIcon;

	private MonthCardConfig.MonthCard monthCardCfg;
	private com.kodgames.corgi.protocol.OneMonthCardInfo monthCardInfo;

	private void OnInteractiveWithServer()
	{
		//仅在与服务器发生过交互才刷新主列表的内容
		SysUIEnv.Instance.GetUIModule<UIPnlActivityMonthCardTab>().shouldQueryWhenRemoveOverlay = true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		monthCardCfg = userDatas[0] as MonthCardConfig.MonthCard;
		monthCardInfo = userDatas[1] as com.kodgames.corgi.protocol.OneMonthCardInfo;

		InitUI();

		return true;
	}

	void InitUI()
	{
		bool type_1 = monthCardCfg.type == MonthCardType.MONTH_CARD_TYPE_1;
		bool type_2 = monthCardCfg.type == MonthCardType.MONTH_CARD_TYPE_2;

		buyRewardBtn.Data = MonthCardRewardType.BuyReward;
		dailyRewardBtn.Data = MonthCardRewardType.DailyReward;
		tentimeRewardBtn.Data = MonthCardRewardType.TenTimesReward;

		roleTex_1.Hide(!type_1);
		roleTex_2.Hide(!type_2);

		titleState.SetState(type_1 ? 0 : 1);
		typeState.SetState(type_1 ? 0 : 1);

		descText.Text = monthCardCfg.introduce;

		var appleGoodConfig = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(monthCardCfg.goodsId, GameUtility.GetDeviceInfo().DeviceType);
		if (appleGoodConfig == null)
			appleGoodConfig = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(monthCardCfg.goodsId, _DeviceType.Unknown);

		priceText.Text = GameUtility.FormatUIString("UIPnlMonthCardDetail_SellPrice", GameDefines.txColorGreen, GameDefines.txColorYellow,
											(appleGoodConfig.costRMB / (float)ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower).ToString(ItemInfoUtility.GetDecimalMedianByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType)),
											ItemInfoUtility.GetCurrencyNameByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType));

		tenTimeRewardNotifyIcon.Hide(monthCardInfo.pickCounter != 10);
		buyRewardNotifyIcon.Hide(monthCardInfo.buyRewardCount == 0);
		dailyRewardNotifyIcon.Hide(!monthCardInfo.isCouldPickDailyReward);
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardTypeBtnClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgMonthCardView), monthCardCfg, monthCardInfo, (int)btn.Data, new System.Action(OnInteractiveWithServer));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBuyMonthCardClick(UIButton btn)
	{
		if (monthCardInfo.remainDates + monthCardCfg.durationDay > monthCardCfg.remainDaysLimit)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityMonthCardInfo_NotBuy"));
			return;
		}

		Platform.Instance.Pay(monthCardCfg.goodsId, monthCardCfg.id.ToString());
		OnInteractiveWithServer();
	}

	public void OnResponseQuerySuccess(List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos)
	{
		foreach (var temp in monthCardInfos)
		{
			if (this.monthCardInfo.monthCardId == temp.monthCardId)
			{
				this.monthCardInfo = temp;
				this.monthCardCfg = ConfigDatabase.DefaultCfg.MonthCardConfig.GetMonthCardById(temp.monthCardId);
				break;
			}
		}

		OnInteractiveWithServer();

		InitUI();
	}
}
