using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgAffordCost : UIModule
{
	public UIElemAssetIcon itemIcon;
	public SpriteText titleLabel;
	public SpriteText itemNameLabel;
	public SpriteText itemDescLabel;
	public SpriteText itemUseDetailLabel;
	public SpriteText itemCountLabel;
	public SpriteText itemCostLabel;
	public UIButton useItemBtn;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SetUICtrls((int)userDatas[0]);

		return true;
	}

	public void SetUICtrls(int assetId)
	{
		if (assetId == IDSeg._SpecialId.Stamina || assetId == IDSeg._SpecialId.Energy)
		{
			HideSelf();
			//体力、精力购买有独有的逻辑
			ProcessBuyStaminaLogic(assetId);
			return;
		}

		// Set Title.
		titleLabel.Text = GameUtility.FormatUIString("UIDlgAffordCost_Title_NotEnough", ItemInfoUtility.GetAssetName(assetId));

		int goodsId = ConfigDatabase.DefaultCfg.GameConfig.recoveryConfig.GetGoodsIdByAssetId(assetId);
		GoodConfig.Good goodCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodsId);
		if (goodCfg == null)
		{
			Debug.LogError("Invalid goods id : " + goodsId.ToString("X"));
			return;
		}

		Reward reward = goodCfg.rewards[0];
		if (reward == null)
		{
			Debug.LogError("Invalid Recovery Item");
			return;
		}

		itemNameLabel.Text = ItemInfoUtility.GetAssetName(reward.id);
		itemDescLabel.Text = ItemInfoUtility.GetAssetDesc(reward.id);
		itemIcon.SetData(reward.id);
		itemUseDetailLabel.Text = string.Empty;

		//当前：{0}个
		// Set current owner.
		int itemCount = ItemInfoUtility.GetGameItemCount(reward.id);
		itemCountLabel.Text = GameUtility.FormatUIString("UIDlgAffordCost_Label_ItemCount", itemCount);

		//价格：{0}元宝
		// Set Cost Label.
		int costCount = SysLocalDataBase.Inst.GetGoodsPriceAfterDiscount(goodsId);
		itemCostLabel.Hide(itemCount != 0);
		itemCostLabel.Text = GameUtility.FormatUIString("UIDlgAffordCost_Label_ItemBuyCost", costCount);

		// Set UseButton.
		bool buyConsumableNoUse = IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Item && !ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(assetId).HasConsumeReward();
		var datas = new List<object>();
		datas.Add(assetId);
		datas.Add(goodsId);
		datas.Add(itemCount == 0);
		datas.Add(buyConsumableNoUse);

		useItemBtn.Hide(itemCount > 0 && buyConsumableNoUse);
		useItemBtn.data = datas;
		useItemBtn.Text = GameUtility.GetUIString(itemCount == 0 ? (buyConsumableNoUse ? "UIDlgAffordCost_Label_Buy" : "UIDlgAffordCost_Label_BuyAndUse") : "UIDlgAffordCost_Label_Use");
	}

	private void ProcessBuyStaminaLogic(int assetId)
	{
		ItemConfig.Item item = null;
		SpecialGoodsConfig.SpecialGood itemSpecialGood = ConfigDatabase.DefaultCfg.SpecialGoodsConfig.GetGoodByRewardId(assetId);
		//体力
		if (assetId == IDSeg._SpecialId.Stamina)
		{
			if (itemSpecialGood == null || !itemSpecialGood.IsOpen)
			{
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgAffordCost_Stamina_Title_NotEnough"));
				return;
			}	
			item = ConfigDatabase.DefaultCfg.ItemConfig.GetItemByType(ItemConfig._Type.AddStamina);		
		}
		
		//精力
		if (assetId == IDSeg._SpecialId.Energy)
		{
			if(itemSpecialGood == null || !itemSpecialGood.IsOpen)
			{
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgAffordCost_Title_NotEnough", ItemInfoUtility.GetAssetName(assetId)));
				return;
			}	
			item = ConfigDatabase.DefaultCfg.ItemConfig.GetItemByType(ItemConfig._Type.AddEnergy);
		}
		
		KodGames.ClientClass.Consumable consumable = null;
		if(item != null)
			consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(item.id);

		//如果没有回复体力道具（烤山鸡）
		if (consumable == null || consumable.Amount <= 0)
		{
			//体力购买次数是否到达上限
			int useCount = 0;
			int maxUseCount = 0;

			//体力
			if (assetId == IDSeg._SpecialId.Stamina)
			{
				useCount = SysLocalDataBase.Inst.LocalPlayer.StaminaBuyCount;
				maxUseCount = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.BuyStaminaCount);
			}
			if (assetId == IDSeg._SpecialId.Energy)
			{			
				useCount = SysLocalDataBase.Inst.LocalPlayer.EnergyBuyCount;
				maxUseCount = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.BuyEnergyCount);
			}								

			UIDlgMessage.ShowData sData = new UIDlgMessage.ShowData();
			MainMenuItem cancleMenu = new MainMenuItem();
			cancleMenu.ControlText = GameUtility.GetUIString("UICtrl_Btn_Cancle");

			MainMenuItem okMenu = new MainMenuItem();
			okMenu.ControlText = GameUtility.GetUIString("UICtrl_Btn_OK");

			string dlgTitle;
			string dlgContent;
			string leftBuyCount = GameUtility.FormatUIString("UIDlg_BuyStamina_LeftBuyCount", Mathf.Max(maxUseCount - useCount, 0), maxUseCount);

			//达到购买体力次数上限，不能够购买了
			if (useCount >= maxUseCount)
			{
				//（今日剩余购买次数{0}/{1}）
				dlgContent = GameUtility.GetUIString("UIDlg_BuyStamina_BuyCountNotEnough");//今日已经没有剩余的购买次数了哦
				dlgContent += "\n";
				dlgContent += leftBuyCount;

				dlgTitle = string.Format(GameUtility.GetUIString("UIDlgRecoverStamina_UseItem_Title"), ItemInfoUtility.GetAssetName(assetId));
				sData.SetData(dlgTitle, dlgContent, okMenu);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(sData);
			}
			else//没有达到使用上限提示购买体力
			{
				//获取回复体力的specialGood，目的是为了获取多少元宝买多少体力，必须配置一个SpecialGood，内容是消耗元宝获取体力。
				SpecialGoodsConfig.SpecialGood specialGood = ConfigDatabase.DefaultCfg.SpecialGoodsConfig.GetGoodByRewardId(assetId);
				if (specialGood == null)
				{
					Debug.LogError("BuyStaminaFailed. Can't find in SpeicalGoodConfig by Reward " + assetId.ToString("X"));
					return;
				}

				int realMoneyCost = 0;
				foreach (var cost in specialGood.Costs)
				{
					if (cost.id == IDSeg._SpecialId.RealMoney)
					{
						realMoneyCost = cost.count;
						break;
					}
				}

				//Title："购买体力"
				dlgTitle = GameUtility.FormatUIString("UIDlg_BuyStamina_Title", ItemInfoUtility.GetAssetName(assetId));

				//是否花费{xx}{元宝}购买{xx}点{体力}？
				dlgContent = GameUtility.FormatUIString("UIDlg_BuyStamina_BuyStamina", realMoneyCost, ItemInfoUtility.GetAssetName(IDSeg._SpecialId.RealMoney), specialGood.GoodReward.count, ItemInfoUtility.GetAssetName(assetId));
				dlgContent += "\n";
				dlgContent += leftBuyCount;

				okMenu.Callback = (data) =>
				{
					//如果元宝不足，弹出提示对话框
					if (SysLocalDataBase.Inst.LocalPlayer.RealMoney < realMoneyCost)
					{
						UIDlgMessage.ShowData subShowData = new UIDlgMessage.ShowData();
						MainMenuItem subCancleMenu = new MainMenuItem();
						subCancleMenu.ControlText = GameUtility.GetUIString("UICtrl_Btn_Cancle");

						MainMenuItem subOkMenu = new MainMenuItem();
						subOkMenu.ControlText = GameUtility.GetUIString("UIPnlShop_Recharge");//充值

						string subDlgContent = GameUtility.GetUIString("UIPnlLevelUpTab_IngotNotAfford");//元宝不足啦，请先充值
						subOkMenu.Callback = (data1) =>
						{
							//单击“充值”，跳转到充值页面
							SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlRecharge), _UILayer.Top);
							return true;
						};

						subShowData.SetData(subOkMenu.ControlText, subDlgContent, subCancleMenu, subOkMenu);

						SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(subShowData);
						return true;
					}

					//购买体力
					if (assetId == IDSeg._SpecialId.Stamina)
						RequestMgr.Inst.Request(new BuySpecialGoodsReq(specialGood.GoodId, specialGood.GoodReward.count, OnBuyStaminaSuccess));
					if (assetId == IDSeg._SpecialId.Energy)
						RequestMgr.Inst.Request(new BuySpecialGoodsReq(specialGood.GoodId, specialGood.GoodReward.count, OnBuyEnergySuccess));
					
					return true;
				};

				sData.SetData(dlgTitle, dlgContent, cancleMenu, okMenu);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(sData);
			}
		}
		else
		{
			string promptStr = GameUtility.FormatUIString("UIDlgRecoverStamina_UseItem_PromptString", consumable.Amount, ItemInfoUtility.GetAssetName(item.id), ItemInfoUtility.GetAssetName(assetId));
			SysUIEnv.Instance.ShowUIModule<UIDlgRecoverStamina_UseItem>(item.id, promptStr, new System.Action(() =>
			{
				//使用一个烤山鸡
				RequestMgr.Inst.Request(new ConsumeItemReq(item.id, 1, 0, "", true));
			}),assetId);
		}
	}

	//使用烤山鸡获得体力成功后 ConsumeItemRes回调
	public void OnUseItemRecoverStaminaSuccess()
	{
		SpecialGoodsConfig.SpecialGood sg = ConfigDatabase.DefaultCfg.SpecialGoodsConfig.GetGoodByRewardId(IDSeg._SpecialId.Stamina);

		//使用成功，回复{0}【{1}】"
		string message = GameUtility.FormatUIString("UIDlg_BuyStamina_UseSuccess_TipPnlTex", ItemInfoUtility.GetAssetName(IDSeg._SpecialId.Stamina), sg.GoodReward.count);

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(message, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//购买体力成功后弹提示面板
	//BuySpecialGoodsRes回调
	void OnBuyStaminaSuccess(int recoverCount)
	{
		//购买{0}成功，回复{1}【{2}】
		string pnlContent = GameUtility.FormatUIString("UIDlg_BuyStamina_OnSuccess_TipPnlTex", ItemInfoUtility.GetAssetName(IDSeg._SpecialId.Stamina), ItemInfoUtility.GetAssetName(IDSeg._SpecialId.Stamina), recoverCount);

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(pnlContent, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	void OnBuyEnergySuccess(int recoverCount)
	{
		//购买{0}成功，回复{1}【{2}】
		string pnlContent = GameUtility.FormatUIString("UIDlg_BuyStamina_OnSuccess_TipPnlTex", ItemInfoUtility.GetAssetName(IDSeg._SpecialId.Energy), ItemInfoUtility.GetAssetName(IDSeg._SpecialId.Energy), recoverCount);

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(pnlContent, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnUseItemBtnClick(UIButton btn)
	{
		var datas = btn.data as List<object>;
		int assetId = (int)datas[0];
		int goodsId = (int)datas[1];
		bool isBuy = (bool)datas[2];
		bool buyConsumableNoUse = (bool)datas[3];

		if (isBuy)
		{
			var goodsData = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(goodsId);
			if (goodsData == null)
			{
				Debug.LogError(string.Format("Good Id {0} Not Found.", goodsId.ToString("X")));
				return;
			}

			if (buyConsumableNoUse)
				RequestMgr.Inst.Request(new BuyGoodsReq(goodsId, 1, goodsData.StatusIndex, (data1, data2, data3) =>
					{
						CloseAndShowResultPnl(buyConsumableNoUse, assetId, null);
					}));
			else
				RequestMgr.Inst.Request(new BuyAndUseReq(goodsData.GoodsID, goodsData.StatusIndex, (reward) =>
				{
					CloseAndShowResultPnl(buyConsumableNoUse, assetId, reward);
				}));
		}
		else
		{
			Reward costItem = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodsId).rewards[0];
			RequestMgr.Inst.Request(new ConsumeItemReq(costItem.id, 1, 0, "", true));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	//关闭快捷购买，显示结果面板，主要显示买到了什么或使用后获得了什么
	private void CloseAndShowResultPnl(bool buyConsumableNoUse, int assetId, KodGames.ClientClass.Reward reward)
	{
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		if (buyConsumableNoUse)//直接购买，不使用
		{
			//购买成功
			string pnlContent = GameUtility.GetUIString("UIDlgAffordCost_ResultText_Buy_Success") + '\n';
			//获得{0}x1
			pnlContent += GameUtility.FormatUIString("UIDlgAffordCost_ResultText_Buy_ObtainedItem", ItemInfoUtility.GetAssetName(assetId), 1);

			showData.SetData(pnlContent, true, true);
		}
		else
		{
			//购买并使用成功
			string pnlContent = string.Empty;
			switch (assetId)
			{
				case IDSeg._SpecialId.ArenaChallengeTimes:
					pnlContent = GameUtility.GetUIString("UIDlgAffordCost_ResultText_BuyAndUse_Success");
					break;

				default:
					pnlContent = GameUtility.GetUIString("UIDlgAffordCost_ResultText_Use_Success") + '\n';
					//获得{0}x1
					pnlContent += GameUtility.FormatUIString("UIDlgAffordCost_ResultText_Buy_ObtainedItem", ItemInfoUtility.GetAssetName(assetId), 1);
					break;
			}

			showData.SetData(pnlContent, true, true);
		}

		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
		HideSelf();
	}
}
