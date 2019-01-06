using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlGuildPublicShop : UIModule
{
	public SpriteText guildLvLabel;
	public SpriteText refreshLabel;
	public UIScrollList shopList;
	public GameObjectPool shopPool;

	private long nextRefreshTime;
	private bool waitControl;
	private float deltaTime = 1f;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		// Set PanelTab's Button Status.
		SysUIEnv.Instance.GetUIModule<UIPnlGuildShopTab>().ChangeTabButtons(_UIType.UIPnlGuildPublicShop);

		// Set Guild Level.
		guildLvLabel.Text = GameUtility.FormatUIString(
			"UIPnlGuildIntroInfo_GuildLv",
			GameDefines.textColorBtnYellow,
			GameDefines.textColorWhite,
			GameUtility.FormatUIString("UIPnlGuildApplyInfo_Level", SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildLevel));

		RequestMgr.Inst.Request(new QueryGuildPublicShopReq());

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

		nextRefreshTime = 0;
		deltaTime = 1f;
		waitControl = false;
	}

	private void Update()
	{
		if (this.IsShown == false || this.IsOverlayed || this.waitControl)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1f)
		{
			if (nextRefreshTime > 0 && SysLocalDataBase.Inst.LoginInfo.NowTime > nextRefreshTime)
				QueryGoodsList();

			SetRefreshTimeLabel();
		}
	}

	private void QueryGoodsList()
	{
		this.waitControl = true;
		RequestMgr.Inst.Request(new QueryGuildPublicShopReq());
	}

	private void SetRefreshTimeLabel()
	{
		var str = string.Empty;
		if (nextRefreshTime > 0)
			str = GameUtility.FormatUIString("UIPnlGuildPublicShop_RefreshLabel", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, GameUtility.EqualsFormatTimeStringWithoutThree(nextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime));

		UIUtility.UpdateUIText(refreshLabel, str);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillShopList(List<KodGames.ClientClass.GuildPublicGoods> guildPublicGoods)
	{
		yield return null;

		foreach (var shop in guildPublicGoods)
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
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuideDetail, ConfigDatabase.DefaultCfg.GuildConfig.GetMainTypeByGuideType(GuildConfig._GuideType.PublicShopGuide));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickShopIcon(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		var good = assetIcon.Data as KodGames.ClientClass.GuildPublicGoods;
		var goodCfg = ConfigDatabase.DefaultCfg.GuildPublicShopConfig.GetGoodsById(good.GooodsId);

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
		RequestMgr.Inst.Request(new BuyGuildPublicGoodsReq((btn.Data as KodGames.ClientClass.GuildPublicGoods).GooodsId));
	}

	public void OnQueryShopListSuccess(List<KodGames.ClientClass.GuildPublicGoods> guildPublicGoods, long nextRefreshTime)
	{
		ClearData();

		this.nextRefreshTime = nextRefreshTime;
		StartCoroutine("FillShopList", guildPublicGoods);
	}

	public void OnBuyGoodsFinish(bool success, KodGames.ClientClass.GuildPublicGoods guildPublicGood, KodGames.ClientClass.Reward reward)
	{
		// Refresh Item.
		if (guildPublicGood != null)
		{
			for (int index = 0; index < shopList.Count; index++)
			{
				var item = shopList.GetItem(index).Data as UIElemGuildShopItem;
				if (item.GoodId == guildPublicGood.GooodsId)
				{
					item.SetData(guildPublicGood);
					break;
				}
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
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildPublicShop_ItemCountError"));
	}
}