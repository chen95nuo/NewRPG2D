using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlGuildPrivateShop : UIModule
{
	public SpriteText guildLvLabel;
	public UIScrollList shopList;
	public GameObjectPool shopPool;

	private bool waitControl;
	private float deltaTime = 1f;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		// Set PanelTab's Button Status.
		SysUIEnv.Instance.GetUIModule<UIPnlGuildShopTab>().ChangeTabButtons(_UIType.UIPnlGuildPrivateShop);

		// Set Guild Level.
		guildLvLabel.Text = GameUtility.FormatUIString(
			"UIPnlGuildIntroInfo_GuildLv",
			GameDefines.textColorBtnYellow,
			GameDefines.textColorWhite,
			GameUtility.FormatUIString("UIPnlGuildApplyInfo_Level", SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildLevel));

		RequestMgr.Inst.Request(new QueryGuildPrivateShopReq());

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillShopList");
		shopList.ClearList(false);
		shopList.ScrollPosition = 0f;

		this.waitControl = false;
	}

	private void Update()
	{
		if (this.IsShown == false || this.IsOverlayed || this.waitControl)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1f)
		{
			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			for (int index = 0; index < shopList.Count; index++)
			{
				var item = shopList.GetItem(index).Data as UIElemGuildShopItem;
				if (item.NextRefreshTime < nowTime)
				{
					QueryGoodsList();
					break;
				}
			}
		}
	}

	private void QueryGoodsList()
	{
		this.waitControl = true;
		RequestMgr.Inst.Request(new QueryGuildPublicShopReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillShopList(List<KodGames.ClientClass.GuildPrivateGoods> guildPrivateGoods)
	{
		yield return null;

		foreach (var shop in guildPrivateGoods)
		{
			var container = shopPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemGuildShopItem>();

			container.Data = item;
			item.SetData(shop);

			shopList.AddItem(container);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLevelInfoButton(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuideDetail, ConfigDatabase.DefaultCfg.GuildConfig.GetMainTypeByGuideType(GuildConfig._GuideType.PrivateShopGuide));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickShopIcon(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		var good = assetIcon.Data as KodGames.ClientClass.GuildPrivateGoods;
		var goodCfg = ConfigDatabase.DefaultCfg.GuildPrivateShopConfig.GetGoodsById(good.GooodsId);

		if (goodCfg.GoodsIconId != IDSeg.InvalidId)
			GameUtility.ShowAssetInfoUI(goodCfg.GoodsIconId);
		else
		{
			if (goodCfg.Rewards.Count == 1)
			{
				if (GameUtility.ShowAssetInfoUI(goodCfg.Rewards[0].id) == false)
					SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, goodCfg.GoodsId);
			}
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, good.GooodsId);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBuy(UIButton btn)
	{
		var goods = btn.Data as KodGames.ClientClass.GuildPrivateGoods;
		var goodsCfg = ConfigDatabase.DefaultCfg.GuildPrivateShopConfig.GetGoodsById(goods.GooodsId);

		if (!goodsCfg.BatchPurchase)
		{
			RequestMgr.Inst.Request(new BuyGuildPrivateGoodsReq(goods.GooodsId, 1));
			return;
		}

		UIDlgGuildPrivateBuyTips.ShowData showData = new UIDlgGuildPrivateBuyTips.ShowData();

		int cout = goodsCfg.Costinfos.Count - 1;

		if (goods.BuyCount >= cout)
			showData.unitPrice = goodsCfg.Costinfos[cout].Costs[0].count;
		else
			showData.unitPrice = goodsCfg.Costinfos[goods.BuyCount].Costs[0].count;

		showData.totalCount = 1;
		showData.maxCount = goodsCfg.BuyLimitCountPerCircle - goods.BuyCount;
		showData.goods = goods;
		showData.okCallback = OnDialogBuyBtnClick;

		SysUIEnv.Instance.GetUIModule<UIDlgGuildPrivateBuyTips>().ShowDialog(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private bool OnDialogBuyBtnClick(int goodsId, int totalCount, object obj)
	{
		if (totalCount > 0)
			RequestMgr.Inst.Request(new BuyGuildPrivateGoodsReq(goodsId, totalCount));

		return true;
	}

	public void OnQueryShopListSuccess(List<KodGames.ClientClass.GuildPrivateGoods> guildPrivateGoods)
	{
		ClearData();
		StartCoroutine("FillShopList", guildPrivateGoods);
	}

	public void OnBuyGoodsFinish(bool success, KodGames.ClientClass.GuildPrivateGoods guildPrivateGood, KodGames.ClientClass.Reward reward)
	{
		// Refresh Item.
		for (int index = 0; index < shopList.Count; index++)
		{
			var item = shopList.GetItem(index).Data as UIElemGuildShopItem;
			if (item.GoodId == guildPrivateGood.GooodsId)
			{
				item.SetData(guildPrivateGood);
				break;
			}
		}

		// Show Reward Message.
		if (success)
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
				okCallback.Callback = (data) =>
				{
					SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlPackageItemTab);
					return true;
				};

				MainMenuItem cancelCallback = new MainMenuItem();
				cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Shopping");
				showData.SetData(title, message, okCallback, cancelCallback);
				uiShopBuy.ShowDlg(showData);
			}
		}
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildPrivateShop_ItemCountError"));
	}
}
