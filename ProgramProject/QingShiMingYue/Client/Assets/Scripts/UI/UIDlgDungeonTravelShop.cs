using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDungeonTravelShop : UIModule
{
	public SpriteText titleLabel;
	public UIScrollList travelGoodList;
	public GameObjectPool travelGoodPool;

	private float deltaTime;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		StartCoroutine("InitView", (int)userDatas[0]);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		// Clear List.
		StopCoroutine("InitView");

		for (int i = 0; i < travelGoodList.Count; i++)
		{
			var item = travelGoodList.GetItem(i).Data as UIElemDungeonTravelShopItem;

			if (item != null)
				item.ClearData();
		}
		travelGoodList.ClearList(false);
		travelGoodList.ScrollPosition = 0f;

		// Clear Data.
		deltaTime = 0f;
	}

	public void Update()
	{
		if (travelGoodList.Count <= 0)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime >= 1.0f)
		{
			deltaTime = 0f;
			for (int index = 0; index < travelGoodList.Count; index++)
				(travelGoodList.GetItem(index).Data as UIElemDungeonTravelShopItem).UpdateView();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator InitView(int dungeonId)
	{
		yield return null;

		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		var dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);
		var travelCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetTravelTradeByDungeonId(dungeonId);
		var travelRecrod = SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonTravelDataByDungeonId(dungeonId);

		// Set title Label.
		titleLabel.Text = GameUtility.FormatUIString("UIPnlCampaign_TravelShop_Title", ItemInfoUtility.GetLevelCN(travelCfg.openNeedStars), ItemInfoUtility.GetAssetName(dungeonId));

		// Sort Good Data.
		travelCfg.canBuyGoodsIds.Sort((d1, d2) =>
			{
				int fix1 = travelRecrod.AlreadyBuyGoodIds.Contains(d1) ? 1 : 0;
				int fix2 = travelRecrod.AlreadyBuyGoodIds.Contains(d2) ? 1 : 0;

				if (fix1 != fix2)
					return fix1 - fix2;
				else
					return d1 - d2;
			});

		// Fill List.
		for (int index = 0; index < travelCfg.canBuyGoodsIds.Count; index++)
		{
			var container = travelGoodPool.AllocateItem().GetComponent<UIListItemContainer>();
			var travelItem = container.gameObject.GetComponent<UIElemDungeonTravelShopItem>();
			container.Data = travelItem;
			travelItem.SetData(dungeonId, travelCfg.openNeedStars, ConfigDatabase.DefaultCfg.CampaignConfig.GetTravelGoodById(travelCfg.canBuyGoodsIds[index]), dungeonRecord);

			travelGoodList.AddItem(container);
		}
	}

	public void OnResponseBuyTravel(int goodId)
	{
		for (int index = 0; index < travelGoodList.Count; index++)
		{
			var item = (travelGoodList.GetItem(index).Data as UIElemDungeonTravelShopItem);

			if (item.TravelGood.goodId == goodId)
			{
				item.UpdateView();
				break;
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTravelBuy(UIButton btn)
	{
		var shopItem = btn.Data as UIElemDungeonTravelShopItem;

		RequestMgr.Inst.Request(new BuyTravelReq(shopItem.DungeonId, shopItem.TravelGood.goodId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}
}
