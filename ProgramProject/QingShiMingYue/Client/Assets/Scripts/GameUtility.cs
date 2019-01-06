using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using ClientServerCommon;

public static class GameUtility
{
	public static KodGames.ClientClass.DeviceInfo GetDeviceInfo()
	{
		KodGames.ClientClass.DeviceInfo deviceInfo = new KodGames.ClientClass.DeviceInfo();
#if UNITY_IPHONE		
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			deviceInfo.DeviceType = _DeviceType.iPhone;
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
			deviceInfo.DeviceType = _DeviceType.Android;
#endif
		deviceInfo.OsType = KodGames.ExternalCall.DevicePlugin.GetSystemName();
		deviceInfo.OsVersion = KodGames.ExternalCall.DevicePlugin.GetSystemVersion();
		deviceInfo.Udid = Platform.Instance.GetUDID();
		deviceInfo.DeviceName = KodGames.ExternalCall.DevicePlugin.GetDeviceName();

		return deviceInfo;
	}

	#region 字符串处理
	public static string AppendString(string old, string pending, bool newLine)
	{
		if (string.IsNullOrEmpty(pending))
			return old;

		if (string.IsNullOrEmpty(old))
		{
			old = pending;
		}
		else
		{
			if (newLine)
				old += "\n";
			old += pending;
		}

		return old;
	}

	public static string FormatStringOnlyWithParams(string strToFormat, object[] canToStringObjs)
	{
		if (string.IsNullOrEmpty(strToFormat) || canToStringObjs == null || canToStringObjs.Length == 0)
			return strToFormat;

		string strUsedToReplace = "{0}";
		for (int paramIndex = 0; paramIndex < canToStringObjs.Length; paramIndex++)
		{
			strUsedToReplace = strUsedToReplace.Replace(strUsedToReplace.Substring(1, strUsedToReplace.Length - 2), paramIndex.ToString());

			if (strToFormat.Contains(strUsedToReplace))
				strToFormat = strToFormat.Replace(strUsedToReplace, canToStringObjs[paramIndex].ToString());
			else break;
		}
		return strToFormat;
	}

#if UNITY_EDITOR || UNITY_IPHONE
	public static CalendarUnit TimeDurationType2CalendarUnit(int timeDurationType)
	{
		switch (timeDurationType)
		{
			case _TimeDurationType.Era: return CalendarUnit.Era;
			case _TimeDurationType.Year: return CalendarUnit.Year;
			case _TimeDurationType.Month: return CalendarUnit.Month;
			case _TimeDurationType.Day: return CalendarUnit.Day;
			case _TimeDurationType.Hour: return CalendarUnit.Hour;
			case _TimeDurationType.Minute: return CalendarUnit.Minute;
			case _TimeDurationType.Second: return CalendarUnit.Second;
			case _TimeDurationType.Week: return CalendarUnit.Week;
			default: return CalendarUnit.Day;
		}
	}
#endif

	public static string Time2String(long time)
	{
		long second = time / 1000 % 60;
		long minute = time / 1000 / 60 % 60;
		long hour = time / 1000 / 60 / 60 % 24;
		long day = time / 1000 / 60 / 60 / 24;

		if (day != 0)
			return FormatUIString("UITimeFormat_Day", day, hour);
		else
			return FormatUIString("UITimeFormat_Hour", hour, minute, second);
	}

	public static bool IsTime2DownZero(long time)
	{
		long second = time / 1000 % 60;
		long minute = time / 1000 / 60 % 60;
		long hour = time / 1000 / 60 / 60 % 24;
		long day = time / 1000 / 60 / 60 / 24;
		if (day == 0 && hour == 0 && minute == 0 && second == 0)
			return true;
		return false;
	}

	public static bool EqualsFormatTimeString(string compareTo, long time)
	{
		long second = time / 1000 % 60;
		long minute = time / 1000 / 60 % 60;
		long hour = time / 1000 / 60 / 60 % 24;
		long day = time / 1000 / 60 / 60 / 24;

		if (day != 0)
			return EqualsFormatString(compareTo, GetUIString("UITimeFormat_Day"), day, hour);
		else
			return EqualsFormatString(compareTo, GetUIString("UITimeFormat_Hour"), hour, minute, second);
	}

	public static string Time2StringWithoutSecond(long time)
	{
		long minute = time / 1000 / 60 % 60;
		long hour = time / 1000 / 60 / 60;

		return string.Format("{0:D2}{2}{1:D2}{3}", hour, minute,
			ConfigDatabase.DefaultCfg.StringsConfig.GetString("UI", "UIPnlSlavePage_Time_Hour"),
			ConfigDatabase.DefaultCfg.StringsConfig.GetString("UI", "UIPnlSlavePage_Time_Minute"));
	}

	public static string TimeOfDay2StringWithoutSecond(long time)
	{
		long minute = time / 1000 / 60 % 60;
		long hour = time / 1000 / 60 / 60 % 24;

		return string.Format("{0:D2}:{1:D2}", hour, minute);
	}

	public static string EqualsFormatTimeStringWithoutThree(long time)
	{
		long hour = time / 1000 / 60 / 60;
		long minute = time / 1000 / 60 % 60;
		long second = time / 1000 % 60;

		return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
	}

	public static string Time2SortString(long time)
	{
		// Set total work time

		if (time >= 1000 * 60 * 60 * 24 * 7)
		{
			// week
			return GameUtility.GetUIString("UITimeLeftLabel_Week");
		}
		if (time >= 1000 * 60 * 60 * 24)
		{
			// Hour
			return ((int)(time / (1000 * 60 * 60 * 24))).ToString() + GameUtility.GetUIString("UITimeLabel_Day");
		}
		if (time >= 1000 * 60 * 60)
		{
			// Hour
			return ((int)(time / (1000 * 60 * 60))).ToString() + GameUtility.GetUIString("UITimeLabel_Hour");
		}
		else if (time >= 1000 * 60)
		{
			// Minutes
			return ((int)(time / (1000 * 60))).ToString() + GameUtility.GetUIString("UITimeLabel_Minute");
		}
		else
		{
			// Second
			return (time / (1000)).ToString() + GameUtility.GetUIString("UITimeLabel_Second");
		}
	}

	public static float FloatToPercentageValue(float value)
	{
		return KodGames.Math.RoundToInt(value * 1000) / 10f;
	}

	public static int FloatToPercentageInteger(float value)
	{
		return KodGames.Math.RoundToInt(value * 100);
	}

	public static string FloatToPercentage(float value)
	{
		return FloatToPercentage(value, true);
	}

	public static string FloatToPercentage(float value, bool forceAddSign)
	{
		float _value = FloatToPercentageValue(value);
		//if (_value >= 0)
		//    return forceAddSign ? string.Format("+{0}%", _value) : string.Format("{0}%", _value);
		//else
		return string.Format("{0}%", _value);
	}

	public static string ToMemorySizeString(int size)
	{
		string sign = size >= 0 ? "" : "-";
		float absSize = Mathf.Abs(size);
		if (absSize < 1024)
			return string.Format("{0}{1:F0}B", sign, absSize);

		absSize /= 1024;
		if (absSize < 1024)
			return string.Format("{0}{1:F0}K", sign, absSize);

		absSize /= 1024;
		if (absSize < 1024)
			return string.Format("{0}{1:F1}M", sign, absSize);

		absSize /= 1024;
		return string.Format("{0}{1:F1}G", sign, absSize);
	}

	public static string GetUIString(string key)
	{
		return ConfigDatabase.DefaultCfg.StringsConfig.GetString(GameDefines.strBlkUI, key);
	}

	public static string FormatUIString(string key, params object[] datas)
	{
		return string.Format(GetUIString(key), datas);
	}

	private static System.Text.StringBuilder sourceSB = new System.Text.StringBuilder();
	private static System.Text.StringBuilder targetSB = new System.Text.StringBuilder();

	public static bool EqualsFormatString(string compareTo, string format, params object[] datas)
	{
		sourceSB.Length = 0;
		sourceSB.AppendFormat(format, datas);
		targetSB.Length = 0;
		targetSB.Append(compareTo);

		var sourceLength = sourceSB.Length;
		var targetLength = targetSB.Length;
		if (sourceLength != targetLength)
			return false;

		for (int i = 0; i < sourceLength; ++i)
			if (sourceSB[i] != targetSB[i])
				return false;

		return true;
	}
	#endregion

	#region UI Logic

	#region 协议返回消耗品不足时的处理
	public static void ShowNotEnoughtAssetUI(int assetId, int requireCount)
	{
		switch (assetId)
		{
			//元宝不足
			case IDSeg._SpecialId.RealMoney:
				MainMenuItem rechargeMenu = new MainMenuItem();
				rechargeMenu.ControlText = GameUtility.GetUIString("UIDlgMessage_Ctrl_Recharge");
				rechargeMenu.Callback = (data) =>
				{
					if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgDailyReward)))
						SysUIEnv.Instance.HideUIModule(typeof(UIDlgDailyReward));

					if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlRecharge))
						SysUIEnv.Instance.HideUIModule(_UIType.UIPnlRecharge);

					if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgAffordCost)))
						SysUIEnv.Instance.HideUIModule(typeof(UIDlgAffordCost));

					if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgBeforeBattleLineUp)))
						SysUIEnv.Instance.HideUIModule(typeof(UIDlgBeforeBattleLineUp));

					if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgDungeonTravelShop)))
						SysUIEnv.Instance.HideUIModule(typeof(UIDlgDungeonTravelShop));

					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlRecharge));
					return true;
				};

				MainMenuItem cancelMenu = new MainMenuItem();
				cancelMenu.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_Recharge"), GameUtility.GetUIString("UIDlgMessage_Msg_Recharge"), cancelMenu, rechargeMenu);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
				break;

			//元神不足
			case IDSeg._SpecialId.Soul:
				MainMenuItem sellBtn = new MainMenuItem();
				sellBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_Ctrl_SellAvatar");
				sellBtn.Callback = (data) =>
				{
					if (SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState is GameState_CentralCity)
					{
						if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageAvatarTab))
							SysUIEnv.Instance.HideUIModule(_UIType.UIPnlPackageAvatarTab);

						SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlPackageAvatarTab);
					}
					else
						SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlPackageAvatarTab));

					return true;
				};

				MainMenuItem cancelBtn = new MainMenuItem();
				cancelBtn.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

				UIDlgMessage.ShowData msgData = new UIDlgMessage.ShowData();
				msgData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_SellAvatar"), GameUtility.GetUIString("UIDlgMessage_Msg_SellAvatar"), cancelBtn, sellBtn);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(msgData);
				break;

			//陨铁不足
			case IDSeg._SpecialId.Iron:
				MainMenuItem sellBtn_Iron = new MainMenuItem();
				sellBtn_Iron.ControlText = GameUtility.GetUIString("UIDlgMessage_Ctrl_SellAvatar");
				sellBtn_Iron.Callback = (data) =>
				{
					if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageEquipTab))
						SysUIEnv.Instance.HideUIModule(_UIType.UIPnlPackageEquipTab);

					SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlPackageEquipTab);
					return true;
				};

				MainMenuItem cancelBtn_Iron = new MainMenuItem();
				cancelBtn_Iron.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

				UIDlgMessage.ShowData msgData_Iron = new UIDlgMessage.ShowData();
				msgData_Iron.SetData(GameUtility.GetUIString("UIDlgMessage_Title_SellEquip"), GameUtility.GetUIString("UIDlgMessage_Msg_SellEquip"), cancelBtn_Iron, sellBtn_Iron);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(msgData_Iron);

				break;

			case IDSeg._SpecialId.Stamina:
			case IDSeg._SpecialId.Energy:
			case IDSeg._SpecialId.ArenaChallengeTimes:
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAffordCost), assetId);
				break;

			//增加体力
			case IDSeg._SpecialId.UseItemCount_AddStamina:
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UI_CONSUME_ITEM_FAILED_USE_TIME_NOT_ENOUGH"));
				break;
			case IDSeg._SpecialId.Zentia:
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlEastSeaElementItem_COSTS_NOT_ENOUGH"));
				break;
			default:
				if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.worldChatItemId)
				{
					UIDlgMessage.ShowData sData = new UIDlgMessage.ShowData();

					MainMenuItem cancleMenu = new MainMenuItem();
					cancleMenu.ControlText = GameUtility.GetUIString("UICtrl_Btn_Cancle");

					MainMenuItem okMenu = new MainMenuItem();
					okMenu.Callback =
						(data) =>
						{
							SysUIEnv.Instance.ShowUIModule(typeof(UIPnlShopProp));
							return true;
						};
					okMenu.ControlText = GameUtility.GetUIString("UICtrl_Btn_OK");

					sData.SetData(GameUtility.GetUIString("UIPnlChatTab_Dlg_Title"), GameUtility.GetUIString("UIPnlChatTab_Dlg_BuyConsumable"), true, cancleMenu, okMenu);
					SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(sData);
				}
				else if (IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Item)
				{
					switch (ItemConfig._Type.ToItemType(assetId))
					{
						case ItemConfig._Type.SecertKey:
							SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAffordCost), assetId);
							break;

						default:
							if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.nurseMeridianItemId ||
								assetId == ConfigDatabase.DefaultCfg.ItemConfig.changeMeridianItemId ||
								ItemConfig._Type.ToItemType(assetId) == ItemConfig._Type.KeyItem ||
								ItemConfig._Type.ToItemType(assetId) == ItemConfig._Type.Gacha
								)
							{
								int goodId = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodIdByAssetId(assetId);
								if (goodId != IDSeg.InvalidId)
								{
									UIDlgShopBuyTips.ShowData buyData = new UIDlgShopBuyTips.ShowData();
									buyData.goodsId = goodId;
									buyData.unitPrice = SysLocalDataBase.Inst.GetGoodsPriceAfterDiscount(goodId);
									buyData.maxCount = -1;
									buyData.totalCount = requireCount - (SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(assetId) == null ? 0 : SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(assetId).Amount);
									buyData.okCallback = OnDialogBuyBtnClick;

									SysUIEnv.Instance.GetUIModule<UIDlgShopBuyTips>().ShowDialog(buyData);
									return;
								}
								else
									SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("TipFlow_ItemNotEnough", ItemInfoUtility.GetAssetName(assetId)));
							}
							else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.illusionStoneId || ItemConfig._Type.ToItemType(assetId) == ItemConfig._Type.IllusionCostItem)
							{
								int goodId = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodIdByAssetId(assetId);
								if (goodId != IDSeg.InvalidId)
								{
									GoodConfig.Good goodConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodId);

									UIDlgShopBuyTips.ShowData buyData = new UIDlgShopBuyTips.ShowData();
									buyData.goodsId = goodId;
									buyData.unitPrice = SysLocalDataBase.Inst.GetGoodsPriceAfterDiscount(goodId);
									buyData.maxCount = -1;
									buyData.totalCount = requireCount - (SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(assetId) == null ? 0 : SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(assetId).Amount);

									int moneyCount = ItemInfoUtility.GetGameItemCount(goodConfig.costs[0].id);

									int canBuyCount = Mathf.Max(moneyCount / buyData.unitPrice, 1);

									if (canBuyCount < buyData.totalCount)
										buyData.totalCount = canBuyCount;

									buyData.okCallback = OnDialogBuyBtnClick;

									SysUIEnv.Instance.GetUIModule<UIDlgShopBuyTips>().ShowDialog(buyData);
									return;
								}
								else
									SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("TipFlow_ItemNotEnough", ItemInfoUtility.GetAssetName(assetId)));
							}
							else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.exploreItem)
							{
								SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGuildPointNotEnough));
							}
							else
							{
								//突破丹
								if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.avatarBreakThroughItemId)
									SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_AvatarBreakThroughItem"));
								//精炼石
								else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.equipmentBreakThroughItemId)
									SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_EquipmentBreakThroughItem"));
								//霸气单
								else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.domineerItemId)
									SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_DomineerItem"));
								else
									SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("TipFlow_ItemNotEnough", ItemInfoUtility.GetAssetName(assetId)));
							}
							break;
					}
				}
				else//某种消耗品不足
				{
					//突破丹
					if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.avatarBreakThroughItemId)
						SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_AvatarBreakThroughItem"));
					//精炼石
					else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.equipmentBreakThroughItemId)
						SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_EquipmentBreakThroughItem"));
					//霸气单
					else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.domineerItemId)
						SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgMessage_DomineerItem"));
					else
						SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("TipFlow_ItemNotEnough", ItemInfoUtility.GetAssetName(assetId)));
				}

				break;
		}
	}

	private static bool OnDialogBuyBtnClick(int goodsId, int totalCount, object obj)
	{
		if (totalCount > 0)
		{
			var goodsData = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(goodsId);
			if (goodsData != null)
				RequestMgr.Inst.Request(new BuyGoodsReq(goodsId, totalCount, goodsData.StatusIndex, OnBuySuccess));
		}

		return true;
	}

	private static void OnBuySuccess(int goodsId, int amount, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
		string assetName = ItemInfoUtility.GetAssetName(ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodsId).id);

		//购买成功
		string message = GetUIString("UIDlgAffordCost_ResultText_Buy_Success") + '\n';
		message += FormatUIString("UIDlgAffordCost_ResultText_Buy_ObtainedItem", assetName, amount);

		//改为不弹tips，弹面板。"购买物品成功\n获得了{0}x{1}"
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(message, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageItemTab))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageItemTab>().RefreshView(costAndRewardAndSync.Reward, costAndRewardAndSync.Costs);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarIllusion))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarIllusion>().OnBuyItemSuccess();
	}

	public static void ShowDilogForVipNotEnough()
	{
		string title = GameUtility.GetUIString("UIDlgMessage_Title_Vip_NotEnough");
		string message = GameUtility.GetUIString("UIDlgMessage_Message_VIPLevelNotMatch");
		string okMessage = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_BuyRealMoney");
		ShowDialog(title, message, okMessage, (data) =>
		{
			if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRecharge);
			else
				SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlRecharge));

			return true;
		});
	}

	private static void ShowDialog(string title, string message, string okBtn, MainMenuItem.OnCallback okCallBack)
	{
		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = okBtn;
		okCallback.Callback = okCallBack;

		MainMenuItem cancelCallback = new MainMenuItem();
		cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

		showData.SetData(title, message, cancelCallback, okCallback);

		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	#endregion

	#region 角色、装备、技能（书籍）、道具信息界面显示
	public static bool ShowAssetInfoUI(int assetId)
	{
		return ShowAssetInfoUI(assetId, _UILayer.Invalid);
	}

	public static bool ShowAssetInfoUI(Reward assetReward)
	{
		return ShowAssetInfoUI(assetReward, _UILayer.Invalid);
	}

	/*
	* 注意：非内丹系统显示内丹与其他奖励混合礼包的奖励页面需要使用本重构方法
	* 非混合礼包使用UIpnlDanInfo
	* 目前支持的混合奖励界面：炼丹宝箱预览
	*/
	public static bool ShowAssetInfoUI(com.kodgames.corgi.protocol.ShowReward assetShowReward, _UILayer layer)
	{
		if (IDSeg.ToAssetType(assetShowReward.id) == IDSeg._AssetType.Dan)
		{
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlDanInfo), layer, assetShowReward);
			return true;
		}
		else
		{
			Reward assetReward = new Reward();
			assetReward.id = assetShowReward.id;
			assetReward.level = assetShowReward.level;
			assetReward.breakthoughtLevel = assetShowReward.breakthought;
			return ShowAssetInfoUI(assetReward, layer);
		}
	}

	public static bool ShowAssetInfoUI(int assetId, _UILayer layer)
	{
		Reward assetReward = new Reward();
		assetReward.id = assetId;
		assetReward.level = 1;

		return ShowAssetInfoUI(assetReward, layer);
	}

	public static bool ShowAssetInfoUI(Reward assetReward, _UILayer layer)
	{
		if (assetReward == null)
			return false;

		switch (IDSeg.ToAssetType(assetReward.id))
		{
			case IDSeg._AssetType.Item:
				switch (ItemConfig._Type.ToItemType(assetReward.id))
				{
					case ItemConfig._Type.Package:
						ItemConfig.Item item = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(assetReward.id);
						UIDlgShopGiftPreview.ShowData showdata = new UIDlgShopGiftPreview.ShowData();

						var rewardData = new UIDlgShopGiftPreview.RewardData();
						rewardData.rewards = item.fixRewardPreviews;
						rewardData.title = GameUtility.GetUIString("UIPnlShop_FixedReward");
						showdata.rewardDatas.Add(rewardData);

						if (item.randRewardPreviews != null && item.randRewardPreviews.Count > 0)
						{
							rewardData = new UIDlgShopGiftPreview.RewardData();
							rewardData.rewards = item.randRewardPreviews;
							rewardData.title = GameUtility.GetUIString("UIPnlPVERodomReward_Title_Random");
							showdata.rewardDatas.Add(rewardData);
						}

						showdata.title = GameUtility.GetUIString("UIDlgShopGiftPreview_Title");

						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgShopGiftPreview), layer, showdata);
						return true;

					case ItemConfig._Type.AvatarScorll:
						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlAvatarInfo), layer, assetReward.id);
						return true;

					case ItemConfig._Type.EquipScroll:
						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlEquipmentInfo), layer, assetReward.id);
						return true;

					case ItemConfig._Type.SkillScroll:
						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlSkillInfo), layer, assetReward.id);
						return true;										

					case ItemConfig._Type.BeastScroll:

						BeastConfig.BaseInfo baseInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastFragmentId(assetReward.id);
						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlOrganInfo), layer, baseInfo.Id, true);
						return true;

					case ItemConfig._Type.BeastPart:
						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlOrganChipInfo), layer, assetReward.id);
						return true;

					default:
						SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlConsumableInfo), layer, assetReward.id);
						return true;
				}

			case IDSeg._AssetType.Avatar:

				var kd_avatar = new KodGames.ClientClass.Avatar();
				kd_avatar.ResourceId = assetReward.id;
				kd_avatar.LevelAttrib.Level = assetReward.level;
				kd_avatar.BreakthoughtLevel = assetReward.breakthoughtLevel;
				kd_avatar.IsAvatar = true;

				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlAvatarInfo), layer, kd_avatar, false, true, false, false, null);
				return true;

			case IDSeg._AssetType.Equipment:

				var kd_equipment = new KodGames.ClientClass.Equipment();
				kd_equipment.ResourceId = assetReward.id;
				kd_equipment.LevelAttrib.Level = assetReward.level;
				kd_equipment.BreakthoughtLevel = assetReward.breakthoughtLevel;

				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlEquipmentInfo), layer, kd_equipment, false, true, false, false, null, false);
				return true;

			case IDSeg._AssetType.CombatTurn:

				var kd_skill = new KodGames.ClientClass.Skill();
				kd_skill.ResourceId = assetReward.id;
				kd_skill.LevelAttrib.Level = assetReward.level;

				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlSkillInfo), layer, kd_skill, false, true, false, false, null, false);
				return true;

			case IDSeg._AssetType.Beast:

				var beastBase = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastFragmentId(assetReward.id);
				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlOrganInfo), layer, beastBase.Id, true);
				return true;

			case IDSeg._AssetType.Dan:

				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlDanInfo), layer, assetReward);
				return true;

			default:
				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlConsumableInfo), layer, assetReward.id);
				return false;
		}
	}
	#endregion

	#region 包裹上限
	public static bool CanItemShowInPackage(int itemId)
	{
		if (IDSeg.ToAssetType(itemId) == IDSeg._AssetType.Item)
		{
			switch (ItemConfig._Type.ToItemType(itemId))
			{
				case ItemConfig._Type.SkillScroll:
				case ItemConfig._Type.EquipScroll:
				case ItemConfig._Type.AvatarScorll:
				case ItemConfig._Type.DanLevelUpMaterial:
				case ItemConfig._Type.DanBreakthoughtMaterial:
				case ItemConfig._Type.DanAttributeRefreshMaterial:
				case ItemConfig._Type.BeastPart:
				case ItemConfig._Type.BeastScroll:
					return false;
			}
		}
		else
			return false;

		ItemConfig.Item itemConfig = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(itemId);
		if (itemConfig != null && itemConfig.hideInPackage)
			return false;

		return true;
	}

	public static int GetConsumableCounts()
	{
		int consumableCounts = 0;
		foreach (var consumable in SysLocalDataBase.Inst.LocalPlayer.Consumables)
		{
			if (consumable.Amount > 0)
			{
				if (CanItemShowInPackage(consumable.Id))
					consumableCounts++;
				else
					continue;
			}
		}

		return consumableCounts;
	}

	public static int GetPackageItemCount()
	{
		KodGames.ClientClass.Player localDB = SysLocalDataBase.Inst.LocalPlayer;

		// Skip the superSkill.
		int totalCount = 0;

		for (int index = 0; index < localDB.Skills.Count; index++)
		{
			SkillConfig.Skill skillConfig = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(localDB.Skills[index].ResourceId);
			if (skillConfig == null)
			{
				Debug.LogError("Skill " + localDB.Skills[index].ResourceId.ToString("X") + " in SkillConfig is not found.");
				continue;
			}
			else if (skillConfig.type == ClientServerCommon.CombatTurn._Type.PassiveSkill)
				totalCount++;
		}

		// Skip Recruit Avatar.
		for (int index = 0; index < localDB.Avatars.Count; index++)
			if (localDB.Avatars[index].IsAvatar)
				totalCount++;

		return totalCount + GetConsumableCounts() + localDB.Equipments.Count;
	}

	public static bool CheckPackageCapacity(List<ClientServerCommon.Reward> rewards)
	{
		// Skip the superSkill.
		int totalCountInPackage = GetPackageItemCount();

		if (rewards != null)
		{
			ItemConfig.Item itemTemp;
			for (int i = 0; i < rewards.Count; i++)
			{
				if (IDSeg.ToAssetType(rewards[i].id) == IDSeg._AssetType.Avatar
				|| IDSeg.ToAssetType(rewards[i].id) == IDSeg._AssetType.Equipment
				|| IDSeg.ToAssetType(rewards[i].id) == IDSeg._AssetType.CombatTurn)
					totalCountInPackage++;
				else
				{
					itemTemp = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(rewards[i].id);
					if (itemTemp == null)
						continue;

					if (!itemTemp.hideInPackage)
					{
						var cons = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(rewards[i].id);
						if (cons == null)
							totalCountInPackage++;
					}
				}
			}
		}

		int packageMaxCapacity = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.PackageCapacity);
		if (totalCountInPackage > packageMaxCapacity)
		{
			string title = GameUtility.GetUIString("UIDlgMessage_Title_Tips");
			string message = GameUtility.GetUIString("UIDlgMessage_Message_PackageCapacity");
			string okStr = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_CleanPackage");
			ShowDialog(title, message, okStr, (data) =>
			{
				if (SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState is GameState_CentralCity)
					SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlPackageAvatarTab);
				else
					SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlPackageAvatarTab));
				return true;
			});
			return false;
		}
		return true;
	}

	public static bool CheckPackageCapacity()
	{
		return CheckPackageCapacity(null);
	}
	#endregion

	// 通用UI跳转功能
	public static void JumpUIPanel(int uiType, params object[] userDatas)
	{
		switch (uiType)
		{
			case _UIType.UI_ActivityDungeon:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.Secret, true, true))
					CampaignData.OpenCampaignView(uiType, userDatas);
				break;

			case _UIType.UI_Dungeon:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.Dungeon, true, true))
					CampaignData.OpenCampaignView(uiType, userDatas);
				break;

			case _UIType.UI_Tower:
				if (CheckFuncOpened(_OpenFunctionType.MelaleucaFloor, true, true))
					SysGameStateMachine.Instance.EnterState<GameState_Tower>();
				break;

			case _UIType.UI_Adventrue:
				if (CheckFuncOpened(_OpenFunctionType.MarvellousAdventure, true, true))
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAdventureMain));
				break;

			case _UIType.UIPnlAdventureScene:
				RequestMgr.Inst.Request(new MarvellousQueryReq());
				break;

			case _UIType.UIPnlTowerWeekReward:
				if (CheckFuncOpened(_OpenFunctionType.MelaleucaFloor, true, true))
					SysGameStateMachine.Instance.EnterState<GameState_Tower>(new UserData_ShowUI(_UIType.UIPnlTowerWeekReward));
				break;

			case _UIType.UIPnlArena:
				if (CheckFuncOpened(_OpenFunctionType.Arena, true, true))
					if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
						SysUIEnv.Instance.ShowUIModule(typeof(UIPnlArena));
					else
						SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlArena));
				break;

			case _UIType.UIPnlOrgansBeastTab:
				if (CheckFuncOpened(_OpenFunctionType.Beast, true, true))
					if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
						SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrgansBeastTab));
					else
						SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlOrgansBeastTab));
				break;

			case _UIType.UIPnlOrgansShopTab:
				if (CheckFuncOpened(_OpenFunctionType.Beast, true, true))
					if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
						SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrgansShopTab));
					else
						SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlOrgansShopTab));
				break;

			case _UIType.UI_WolfSmoke:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.WolfSmoke, true, true))
					RequestMgr.Inst.Request(new QueryWolfSmoke());
				break;

			case _UIType.UIPnlFriendTab:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.Friend, true, true))
					SysUIEnv.Instance.ShowUIModule(uiType, userDatas);
				break;

			case _UIType.UIPnlAdventureMain:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.MarvellousAdventure, true, true))
					SysUIEnv.Instance.ShowUIModule(uiType, userDatas);
				break;

			case _UIType.UIPnlFriendStart:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.FriendCampaign, true, true))
					RequestMgr.Inst.Request(new QueryFriendCampaignReq());
				break;

			case _UIType.UIPnlIllusion:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.Illusion, true, true))
					SysUIEnv.Instance.ShowUIModule(uiType, userDatas);
				break;

			case _UIType.UIPnlActivityInvite:
				if (ConfigDatabase.DefaultCfg.GameConfig.isInviteCode &&
					ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.InviteCode) <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level &&
					SysLocalDataBase.Inst.LocalPlayer.Function.ShowInviteCode)
					RequestMgr.Inst.Request(new QueryInviteCodeInfoReq());
				else
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityInvite_Level_Count"));
				break;			

			case _UIType.UIPnlDanMain:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.DanHome, true, true))
					RequestMgr.Inst.Request(new QueryDanHomeReq());
				break;

			case _UIType.UIPnlDanFurnace:
				RequestMgr.Inst.Request(new QueryAlchemyReq());
				break;

			case _UIType.UIPnlDanDecompose:
				RequestMgr.Inst.Request(new QueryDanDecomposeReq());
				break;

			case _UIType.UI_Guild:
			case _UIType.UIPnlGuildTab:
			case _UIType.UI_GuildPoint:
			case _UIType.UIPnlGuildPrivateShop:
			case _UIType.UIPnlGuildPublicShop:
			case _UIType.UIPnlGuildShopActivity:
			case _UIType.UIPnlGuildConstruct:
				if (GameUtility.CheckFuncOpened(_OpenFunctionType.Guild, true, true))
					JumpGuildUI(uiType);
				break;

			default:
				SysUIEnv.Instance.ShowUIModule(uiType, userDatas);
				break;
		}
	}

	// 门派外部跳转
	private static void JumpGuildUI(int uiType)
	{
		RequestMgr.Inst.Request(new GuildQueryReq(() =>
		{
			if (CheckUIAccess(_UIType.UI_Guild, true) == false)
				return true;

			switch (uiType)
			{
				case _UIType.UI_Guild:
					if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo != null && SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildId > 0)
						SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
					break;

				case _UIType.UIPnlGuildTab:
				case _UIType.UIPnlGuildPrivateShop:
				case _UIType.UIPnlGuildPublicShop:
				case _UIType.UIPnlGuildShopActivity:
				case _UIType.UIPnlGuildConstruct:

					if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo != null && SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildId > 0)
						SysUIEnv.Instance.ShowUIModule(uiType);

					break;

				case _UIType.UI_GuildPoint:
					RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Now));
					break;

			}

			return true;
		}));
	}

	// 查看阵容信息:Npc 和 玩家
	public static void ShowViewAvatarUI(KodGames.ClientClass.Player player, int rankLevel)
	{
		if (player == null)
			return;

		if (player.PlayerId < 0)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAvatarLineUpGuide), player);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlViewAvatar), player, rankLevel);

	}

	// 邀请好友
	public static void InviteFriend(int currentFriendCount, int targetPlayerId)
	{
		// Check if has reach max count of friend
		if (currentFriendCount >= ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.MaxFriendCount))
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFriendMsg_Msg_MaxFriendNum"));
		else
		{
			// Show invite dialog
			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

			MainMenuItem okMenu = new MainMenuItem();
			okMenu.ControlText = GameUtility.GetUIString("UIPnlFriendTab_Ctrl_SendInvite");
			okMenu.CallbackData = targetPlayerId;
			okMenu.Callback = (data) =>
				{
					RequestMgr.Inst.Request(new InviteFriendReq((int)data, string.Empty));
					return true;
				};

			showData.SetData(
				GameUtility.GetUIString("UIDlgFriendMsg_Title_InviteFriend"),
				GameUtility.GetUIString("UIDlgFriendMsg_Msg_InviteFriend"),
				okMenu);

			SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIDlgMessage>().ShowDlg(showData);
		}
	}
	#endregion

	#region UI功能开启判定
	public static bool ShowUIFilter(System.Type uiType)
	{
		return CheckUIAccess(uiType, true);
	}

	public static bool CheckUIAccess(System.Type uiType, bool showTip)
	{
		return CheckUIAccess(SysUIEnv.Instance.GetUITypeByClass(uiType), showTip);
	}

	public static bool CheckUIAccess(int uiType, bool showTip)
	{
		switch (uiType)
		{
			case _UIType.UIPnlArena:
				return CheckFuncOpened(_OpenFunctionType.Arena, showTip, true);

			case _UIType.UIPnlAvatarLevelUp:
				return CheckFuncOpened(_OpenFunctionType.AvatarLevelUp, showTip, true);

			case _UIType.UIPnlAvatarBreakThrough:
				return CheckFuncOpened(_OpenFunctionType.AvatarBreakThrough, showTip, true);

			case _UIType.UIPnlAvatarMeridianTab:
				return CheckFuncOpened(_OpenFunctionType.Meridian, showTip, true);

			case _UIType.UIPnlAvatarDomineerTab:
				return CheckFuncOpened(_OpenFunctionType.Domineer, showTip, true);

			case _UIType.UIPnlAvatarDiner:
				return CheckFuncOpened(_OpenFunctionType.Diner, showTip, true);

			case _UIType.UIPnlEquipmentLevelup:
				return CheckFuncOpened(_OpenFunctionType.EquipmentLevelUp, showTip, true);

			case _UIType.UIPnlEquipmentRefine:
				return CheckFuncOpened(_OpenFunctionType.EquipmentRefine, showTip, true);

			case _UIType.UIPnlShopWine:
				return CheckFuncOpened(_OpenFunctionType.Tavern, showTip, true);

			case _UIType.UIPnlShopMystery:
				return CheckFuncOpened(_OpenFunctionType.MysteryShop, showTip, true);

			case _UIType.UIPnlHandBook:
				return CheckFuncOpened(_OpenFunctionType.CardPicture, showTip, true);

			case _UIType.UIDlgDailyReward:
				return CheckFuncOpened(_OpenFunctionType.DailyReward, showTip, true);

			case _UIType.UIPnlLevelRewardTab:
				return CheckFuncOpened(_OpenFunctionType.LevelRewardActivity, showTip, true);

			case _UIType.UIPnlAssistant:
				return CheckFuncOpened(_OpenFunctionType.Assistant, showTip, true);

			case _UIType.UIPnlShopProp:
				return CheckFuncOpened(_OpenFunctionType.GoodsShop, showTip, true);

			case _UIType.UIPnlChatTab:
				return CheckFuncOpened(_OpenFunctionType.Chat, showTip, true);

			case _UIType.UIDlgItemGetWay:
				return CheckFuncOpened(_OpenFunctionType.ItemGetWay, showTip, true);

			case _UIType.UIPnlTravelShopGuid:
				return CheckFuncOpened(_OpenFunctionType.TravelGuid, showTip, true);

			case _UIType.UIPnlIllusion:
			case _UIType.UIPnlAvatarIllusion:
			case _UIType.UIPnlIllusionGuide:
			case _UIType.UIPnlIllusionGuideDetail:
				return CheckFuncOpened(_OpenFunctionType.Illusion, showTip, true);

			//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			case _UIType.UI_Tower:
				return (CheckFuncOpened(_OpenFunctionType.MelaleucaFloor, showTip, true));

			case _UIType.UI_WolfSmoke:
				return GameUtility.CheckFuncOpened(_OpenFunctionType.WolfSmoke, showTip, true);

			case _UIType.UI_ActivityDungeon:
				return GameUtility.CheckFuncOpened(_OpenFunctionType.Secret, showTip, true);

			case _UIType.UI_Dungeon:
				return GameUtility.CheckFuncOpened(_OpenFunctionType.Dungeon, showTip, true);

			case _UIType.UI_Guild:
				return GameUtility.CheckFuncOpened(_OpenFunctionType.Guild, showTip, true);
			
			case _UIType.UIPnlOrgansBeastTab:
				return GameUtility.CheckFuncOpened(_OpenFunctionType.Beast, showTip, true);

			default:
				return true;
		}
	}

	public static bool CheckFuncOpened(int func, bool showTip, bool checkMinLevel)
	{
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(func))
		{
			if (showTip)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_CLOSE_Function_MSG", _OpenFunctionType.GetDisplayNameByType(func, ConfigDatabase.DefaultCfg)));

			return false;
		}

		if (checkMinLevel)
		{
			int limitLevel = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(func);
			if (limitLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			{
				if (showTip)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_NOT_OPEN", limitLevel));

				return false;
			}

			int vipLevel = ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(func);
			if (vipLevel > SysLocalDataBase.Inst.LocalPlayer.VipLevel)
			{
				if (showTip)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_NOT_OPEN", vipLevel));
				return false;
			}
		}
		else
		{
			int limitLevel = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(func);
			if (limitLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			{
				if (showTip)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_ALREADY_CLOSED", limitLevel));

				return false;
			}

			int vipLevel = ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(func);
			if (vipLevel <= SysLocalDataBase.Inst.LocalPlayer.VipLevel)
			{
				if (showTip)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_ALREADY_CLOSED", vipLevel));
				return false;
			}
		}

		return true;
	}
	#endregion

	#region 战斗加速

	// 根据当前vip等级，判定战斗加速x是否开启
	public static bool CheckFuncOpenedByPlayerLevel(int func, bool showTip, bool checkMinLevel)
	{
		if (checkMinLevel)
		{
			int limitLevel = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(func);
			if (limitLevel == 0 || limitLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			{
				if (showTip)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_NOT_OPEN", limitLevel));

				return false;
			}
		}
		else
		{
			int limitLevel = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(func);
			if (limitLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			{
				if (showTip)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_ALREADY_CLOSED", limitLevel));

				return false;
			}
		}

		return true;
	}

	// 根据当前vip等级，判定战斗加速x是否开启
	public static bool CheckFuncOpenedByVIPLevel(int func, bool showTip, bool checkMinLevel)
	{
		if (checkMinLevel)
		{
			int vipLevel = ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(func);
			//如果没有配置vipLevel，返回-1
			if (vipLevel == -1 || vipLevel > SysLocalDataBase.Inst.LocalPlayer.VipLevel)
			{
				return false;
			}
		}
		else
		{
			int vipLevel = ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(func);
			if (vipLevel <= SysLocalDataBase.Inst.LocalPlayer.VipLevel)
			{
				return false;
			}
		}

		return true;
	}
	#endregion

	#region 道具使用次数上限
	public static Dictionary<int, int> ConsumeCosts(int id)
	{
		Dictionary<int, int> result = new Dictionary<int, int>();

		foreach (var cost in ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(id).consumeCost)
		{
			if (result.ContainsKey(cost.id))
				result[cost.id] += cost.count;
			else
				result.Add(cost.id, cost.count);
		}

		return result;
	}

	public static int CalMaxConsumeAmount(int id)
	{
		int amount = int.MaxValue;

		Dictionary<int, int> consumeCosts = ConsumeCosts(id);
		foreach (var c in SysLocalDataBase.Inst.LocalPlayer.Consumables)
		{
			if (c.Id == id && amount > c.Amount)
				amount = c.Amount;

			if (consumeCosts.ContainsKey(c.Id) && amount > (c.Amount / consumeCosts[c.Id]))
				amount = c.Amount / consumeCosts[c.Id];
		}

		if (amount > ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(id).maxUseCount)
			return ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(id).maxUseCount;
		else
			return amount;
	}
	#endregion
}