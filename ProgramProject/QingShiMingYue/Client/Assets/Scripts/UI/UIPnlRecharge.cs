using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ExternalCall;

public class UIPnlRecharge : UIModule
{
	public SpriteText vipPrivilegeLabel;

	public SpriteText myVipLevel;

	public UIProgressBar levelBar;

	public SpriteText levelBar_text;

	public UIChildLayoutControl childLayoutCtrl;

	public UIBox firstRechargeRewardBg;
	public UIScrollList firstRechargeRewardList;
	public GameObjectPool firstRechargeRewardObjPool;

	public UIScrollList rechargeList;
	public GameObjectPool rechargeObjPool;

	public AutoSpriteControlBase backBtn;

	private const int ORIGIN_SCREEN_WIDTH = 320;
	private const int ORIGIN_SCREEN_HEIGHT = 480;
	private float ORG_LIST_HEIGHT = 215f;
	private const float ORG_FR_HEIGHT = 94f;

	private int lastFillVIPLevel = -1;
	private bool needFillList = false;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// 不在主城中 显示背景板
		backBtn.Hide(SysGameStateMachine.Instance.CurrentState is GameState_CentralCity);

		if (GameUtility.CheckFuncOpened(_OpenFunctionType.MonthCardFeedback, false, true))
			RequestMgr.Inst.Request(new QueryMonthCardInfoReq());

		SetData();
		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	public void OnRechargeSuccess(/*List<int> payStatus*/)
	{
		//判断月卡功能，如果月卡功能开启，那么就请求数据缓存起来
		if (GameUtility.CheckFuncOpened(_OpenFunctionType.MonthCardFeedback, false, true))
			RequestMgr.Inst.Request(new QueryMonthCardInfoReq());

		SetVIPInformationUI();
		SetUILayout();
		ClearList();
		StopCoroutine("FillRechargeList");

		if (IsOverlayed)
			needFillList = true;
		else
			StartCoroutine("FillRechargeList");
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		if (needFillList)
		{
			StartCoroutine("FillRechargeList");
			needFillList = false;
		}
	}

	private void SetData()
	{
		SetVIPInformationUI();
		SetUILayout();
		FillRechargeAndRewardList();

	}

	private void SetVIPInformationUI()
	{
		int level = SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel - 1 ? ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel - 1 : SysLocalDataBase.Inst.LocalPlayer.VipLevel;

		myVipLevel.Text = GameUtility.FormatUIString("UIRankingTab_Txt_MyVipLevel", SysLocalDataBase.Inst.LocalPlayer.VipLevel);
		int levelCostRMB = ConfigDatabase.DefaultCfg.VipConfig.GetVipByLevel(level).costRMB;
		int myVipLevelCostRMB = SysLocalDataBase.Inst.LocalPlayer.RemainingCostRMB >= levelCostRMB ? levelCostRMB : SysLocalDataBase.Inst.LocalPlayer.RemainingCostRMB;
		if (levelCostRMB != 0)
		{
			float val = (float)myVipLevelCostRMB / (float)levelCostRMB;
			levelBar.Value = val;
		}
		else
		{
			levelBar.Value = 0;
		}


		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel)
		{
			vipPrivilegeLabel.Text = GameUtility.FormatUIString("UIRankingTab_Txt_CanNotVipPrivilegeLabel", GameDefines.textColorBtnYellow_font);
			levelBar.Value = 1f;
			myVipLevelCostRMB = levelCostRMB;
		}
		else
		{
			float constRMB = (ClientServerCommon.ConfigDatabase.DefaultCfg.VipConfig.GetVipByLevel(level).costRMB - SysLocalDataBase.Inst.LocalPlayer.RemainingCostRMB) / (float)ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower;

			//{0}再儲值{1}{2}{3}{4}即可享受{5}VIP{6}{7}特權
			vipPrivilegeLabel.Text = GameUtility.FormatUIString("UIRankingTab_Txt_VipPrivilegeLabel_CZ",
				GameDefines.textColorBtnYellow_font,
				GameDefines.textColorBtnBlue_font,
				constRMB.ToString(ItemInfoUtility.GetDecimalMedianByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType)),
				ItemInfoUtility.GetCurrencyNameByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType),
				GameDefines.textColorBtnYellow_font,
				GameDefines.textColorBtnBlue_font,
				level + 1,
				GameDefines.textColorBtnYellow_font);
		}

		levelBar_text.Text = myVipLevelCostRMB + "/" + levelCostRMB;
	}

	private void SetUILayout()
	{
		childLayoutCtrl.HideChildObj(firstRechargeRewardBg.gameObject, true);
		if (SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB.Equals(0))
		{
			childLayoutCtrl.HideChildObj(firstRechargeRewardBg.gameObject, false);

			if (rechargeList.viewableArea.y != ORG_LIST_HEIGHT)
				rechargeList.SetViewableArea(rechargeList.viewableArea.x, ORG_LIST_HEIGHT);
		}
		else
		{
			if (rechargeList.viewableArea.y != ORG_LIST_HEIGHT + ORG_FR_HEIGHT)
				rechargeList.SetViewableArea(rechargeList.viewableArea.x, ORG_LIST_HEIGHT + ORG_FR_HEIGHT);
		}
	}

	private void FillRechargeAndRewardList()
	{
		ClearList();
		if (SysLocalDataBase.Inst.LocalPlayer.TotalCostRMB.Equals(0))
			StartCoroutine("FillFirstRechargeList");
		StartCoroutine("FillRechargeList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillFirstRechargeList()
	{
		yield return null;

		foreach (Reward reward in ConfigDatabase.DefaultCfg.AppleGoodConfig.GetAllFirstReward())
		{
			UIListItemContainer container = firstRechargeRewardObjPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemFirstRechargeItem firstRechargeItem = container.GetComponent<UIElemFirstRechargeItem>();

			container.Data = firstRechargeItem;
			firstRechargeItem.SetData(reward);

			firstRechargeRewardList.AddItem(container);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRechargeList()
	{
		yield return null;

		int type = (int)(GameUtility.GetDeviceInfo().DeviceType);

		if (lastFillVIPLevel != SysLocalDataBase.Inst.LocalPlayer.VipLevel)
		{
			foreach (var goodInfo in ConfigDatabase.DefaultCfg.AppleGoodConfig.goodInfos)
			{
				var subGoodInfo = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodInfo.goodId, type);
				if (subGoodInfo == null)
					subGoodInfo = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodInfo.goodId, _DeviceType.Unknown);

				if (subGoodInfo != null)
				{
					if (subGoodInfo.minDisplayVIPLevel > SysLocalDataBase.Inst.LocalPlayer.VipLevel ||
					(subGoodInfo.hideInApple && KodGames.ExternalCall.KodConfigPlugin.IsAppStore()))
						continue;

					UIListItemContainer container = null;
					UIElemRechargeItem rechargeItem = null;

					if (goodInfo.type == PurchaseType.Normal)
					{
						container = rechargeObjPool.AllocateItem().GetComponent<UIListItemContainer>();
						rechargeItem = container.GetComponent<UIElemRechargeItem>();
						container.Data = rechargeItem;

						int rewardForFirst = subGoodInfo.realMoneyCount * (subGoodInfo.firstMultiple - 1);

						//判断是否已经首冲过该档
						if (SysLocalDataBase.Inst.LocalPlayer.AppleGoodIds.Contains(goodInfo.goodId))
							rewardForFirst = 0;

						float costRMB = subGoodInfo.costRMB / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower);

						rechargeItem.SetData(subGoodInfo.realMoneyCount, subGoodInfo.realMoneyCountExtra, costRMB, goodInfo.goodId, rewardForFirst, subGoodInfo.firstMultiple, ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType);
						rechargeList.AddItem(container);
					}
					else
					{
						if (GameUtility.CheckFuncOpened(_OpenFunctionType.MonthCardFeedback, false, true))
						{
							var monthCardCfg = ConfigDatabase.DefaultCfg.MonthCardConfig.GetMonthCardByGoodId(goodInfo.goodId);
							container = rechargeObjPool.AllocateItem().GetComponent<UIListItemContainer>();
							rechargeItem = container.GetComponent<UIElemRechargeItem>();

							container.Data = rechargeItem;

							float costRMB = subGoodInfo.costRMB / (float)(ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower);
							rechargeItem.SetData(monthCardCfg, costRMB, monthCardCfg.name, ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType);
							rechargeList.AddItem(container);
						}
					}
				}
			}

			lastFillVIPLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;
		}
	}

	private void ClearList()
	{
		lastFillVIPLevel = -1;

		StopCoroutine("FillFirstRechargeList");
		firstRechargeRewardList.ClearList(false);
		firstRechargeRewardList.ScrollListTo(0f);

		StopCoroutine("FillRechargeList");
		rechargeList.ClearList(false);
		rechargeList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
		if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureBuyReward>() != null && SysUIEnv.Instance.GetUIModule<UIPnlAdventureBuyReward>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureBuyReward>().UpdateShowCoinUI();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFirstRewardIcon(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI((int)assetIcon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnViewPrivilegeClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRechargeVip);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnComeToView(UIButton btn)
	{
		if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlActivityMonthCardTab);
		else
			SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlActivityMonthCardTab));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBuyMonthCardClick(UIButton btn)
	{
		var monthCardCfg = btn.data as MonthCardConfig.MonthCard;
		var monthCardInfos = ActivityManager.Instance.GetActivity<ActivityMonthCard>().MonthCardInfos;

		for (int i = 0; i < monthCardInfos.Count; i++)
		{
			if (monthCardCfg.id == monthCardInfos[i].monthCardId)
			{
				if (monthCardInfos[i].remainDates + monthCardCfg.durationDay > monthCardCfg.remainDaysLimit)
				{
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityMonthCardInfo_NotBuy"));
					return;
				}
			}
		}

		Platform.Instance.Pay(monthCardCfg.goodsId, monthCardCfg.id.ToString());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRechargeClick(UIButton btn)
	{
		if (Platform.Instance.IsGuest)//is guest
		{
			MainMenuItem bindMenu = new MainMenuItem();
			bindMenu.Callback = (data) =>
			{
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlSetting));
				return true;
			};
			bindMenu.ControlText = GameUtility.GetUIString("UIDlgRecharge_Ctrl_ToBind");

			MainMenuItem cancelMenu = new MainMenuItem();
			cancelMenu.ControlText = GameUtility.GetUIString("UIDlgRecharge_Ctrl_Cancel");

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(GameUtility.GetUIString("UIDlgRecharge_Title_Recharge"), GameUtility.GetUIString("UIDlgRecharge_Tip_Guest"), bindMenu, cancelMenu);
			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);

			return;
		}

		Platform.Instance.Pay((int)(btn.data));
	}
}
