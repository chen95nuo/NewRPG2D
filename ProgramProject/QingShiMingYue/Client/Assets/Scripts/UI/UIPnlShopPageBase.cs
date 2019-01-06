using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public abstract class UIPnlShopPageBase : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool goodsItemPool;

	private float lastUpdateDelta = 0f;

	public override void OnHide()
	{
		ClearData();
		lastUpdateDelta = 0f;
		base.OnHide();
	}

	protected void ClearData()
	{
		// Clear Assistant Data.
		for (int i = 0; i < scrollList.Count; i++)
		{
			var item = scrollList.GetItem(i).Data as UIElemShopPropItem;
			if (item == null)
				continue;

			item.ClearData();
		}

		StopCoroutine("FillData");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
	}

	protected virtual void Update()
	{
		lastUpdateDelta += Time.deltaTime;
		if (lastUpdateDelta > 1f)
		{
			lastUpdateDelta -= 1f;
			UpdateCalulateCountDown();
		}
	}

	private void UpdateCalulateCountDown()
	{
		for (int index = 0; index < scrollList.Count; index++)
		{
			UIElemShopPropItem item = scrollList.GetItem(index).Data as UIElemShopPropItem;
			item.UpdateData();
		}
	}

	private UIElemShopPropItem GetShopListItemByGoodId(int goodsId)
	{
		for (int i = 0; i < scrollList.Count; ++i)
		{
			UIElemShopPropItem item = scrollList.GetItem(i).Data as UIElemShopPropItem;
			if (item.Goods.GoodsID == goodsId)
				return item;
		}

		return null;
	}

	// Will be invoke when click the good icon.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnGoodIconBtnClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		KodGames.ClientClass.Goods good = assetIcon.Data as KodGames.ClientClass.Goods;

		GoodConfig.Good goodsConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(good.GoodsID);

		if (goodsConfig.assetIconId != IDSeg.InvalidId)
			GameUtility.ShowAssetInfoUI(goodsConfig.assetIconId);
		else
		{
			if (goodsConfig.rewards.Count == 1)
			{
				// 如果只包含一个物品, 显示这个物品的信息
				if (GameUtility.ShowAssetInfoUI(goodsConfig.rewards[0].id) == false)
					// False 表示不支持该id物品的显示, 直接显示商品描述
					SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, good.GoodsID);
			}
			else
			{
				// 有多个物品, 显示商品信息
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, good.GoodsID);
			}
		}
	}

	// Will be invoke when click the button 'buy' of the item.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnGoodBuyBtnClick(UIButton btn)
	{
		// Get the click goods item.
		UIElemShopPropItem shopPropItem = btn.data as UIElemShopPropItem;

		int goodId = shopPropItem.Goods.GoodsID;

		// 如果有未达到限制或者有购买次数限制直接向服务器获取错误信息, 
		if (shopPropItem.HasLimit() || shopPropItem.Goods.MaxBuyCount > 0)
		{
			OnDialogBuyBtnClick(goodId, 1, null);
		}
		else
		{
			// Show UIDlgShopBuyTips
			UIDlgShopBuyTips.ShowData showData = new UIDlgShopBuyTips.ShowData();
			showData.goodsId = goodId;
			showData.unitPrice = SysLocalDataBase.Inst.GetGoodsPriceAfterDiscount(goodId);
			showData.totalCount = 1;
			showData.maxCount = shopPropItem.Goods.RemainBuyCount;
			showData.okCallback = OnDialogBuyBtnClick;

			SysUIEnv.Instance.GetUIModule<UIDlgShopBuyTips>().ShowDialog(showData);
		}
	}

	// Will be invoke when click the button 'buy' of the dialog.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private bool OnDialogBuyBtnClick(int goodsId, int totalCount, object obj)
	{
		if (totalCount > 0)
		{
			var goodsData = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(goodsId);
			if (goodsData != null)
				RequestMgr.Inst.Request(new BuyGoodsReq(goodsId, totalCount, goodsData.StatusIndex, OnGoodBuySuccess, OnBuyGoodsFailed_UpdateStatusDel));
		}

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGoodBuySuccess(int goodsId, int amount, KodGames.ClientClass.Reward reward, List<KodGames.ClientClass.Cost> costs)
	{
		if (reward == null || reward.Consumable == null)
		{
			Debug.LogError("Reward is null or Reward Consumable is null");
			return;
		}

		if (IDSeg.ToAssetType(SysLocalDataBase.GetFirstAssetId(reward)) == IDSeg._AssetType.Special)
		{
			string message = GameUtility.GetUIString("UIPnlShop_BuyGoods_Success_SpecialItem_Message");
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), message);
		}
		else
		{
			// Show the success message
			SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
			UIDlgMessage uiShopBuy = uiEnv.GetUIModule<UIDlgMessage>();

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

			string messageFormat = GameUtility.GetUIString("UIPnlShop_BuyGoods_Success_Message");
			string messageValue = ItemInfoUtility.GetAssetName(SysLocalDataBase.GetFirstAssetId(reward));

			string title = GameUtility.GetUIString("UIDlgMessage_Title_Purchase_Succeeded");
			string message = string.Format(messageFormat, messageValue);

			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Check");
			okCallback.Callback = OnCheckTheGood;

			MainMenuItem cancelCallback = new MainMenuItem();
			cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Shopping");
			showData.SetData(title, message, okCallback, cancelCallback);
			uiShopBuy.ShowDlg(showData);
		}
	
		// Refresh goods own count.
		UIElemShopPropItem shopPropItem = GetShopListItemByGoodId(goodsId);
		if (shopPropItem != null)
			shopPropItem.UpdateData();
	}

	private void OnBuyGoodsFailed_UpdateStatusDel(int goodsId)
	{
		// 购买失败, 更新商品状态
		var shopPropItem = GetShopListItemByGoodId(goodsId);
		if (shopPropItem == null)
			return;

		if (shopPropItem.Goods.Status == _GoodsStatusType.Closed)
			// 删除商品
			scrollList.RemoveItem(shopPropItem.uiContainer, false);
		else
			// 更新状态
			shopPropItem.SetData(shopPropItem.Goods);
	}

	// Check the goods which just bought.
	private bool OnCheckTheGood(object obj)
	{
		//在千机楼场景中进入主场景
		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower || SysGameStateMachine.Instance.CurrentState is GameState_WolfSmoke)
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlPackageItemTab));

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlPackageItemTab);
		return true;
	}
}