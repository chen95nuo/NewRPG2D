using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlTowerNormalShop : UIModule
{
	public SpriteText countDownLabel;
	public UIScrollList itemList;
	public GameObjectPool itemPool;
	public SpriteText resetCostLabel;
	public UIElemAssetIcon resetIcon;

	private bool waitForQueryList = false;
	private bool mannuallyRefreshResponsed = false;

	private System.DateTime NextResetRefreshCountTimeNormal
	{
		get
		{
			if (SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor) == null)
				return System.DateTime.MaxValue;

			return
			KodGames.TimeEx.GetTimeAfterTime(
					ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).ResetDateTime,
					KodGames.TimeEx.ToUTCDateTime(SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).LastRefreshTime),
						KodGames.TimeEx._TimeDurationType.Day);
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		MysteryShopConfig.Refresh refreshSet =  ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).refreshSet[0];
		
		resetCostLabel.Text = refreshSet.cost.count.ToString();
		resetIcon.SetData(refreshSet.cost.id);

		SysUIEnv.Instance.GetUIModule<UIPnlTowerShop>().SetSelectedBtn(_UIType.UIPnlTowerNormalShop);
		if (SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor) == null)
		{
			waitForQueryList = true;
			RequestMgr.Inst.Request(new QueryMysteryShopInfoReq(MysteryShopConfig._ShopType.MelaleucaFloor));
		}
		else
			SetControl();

		return true;
	}

	private void ClearData()
	{
		// Clear List.
		StopCoroutine("FillList");
		itemList.ClearList(false);
		itemList.ScrollPosition = 0f;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		if (mannuallyRefreshResponsed)
		{
			mannuallyRefreshResponsed = false;
			SetControl();
		}
	}

	private void Update()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor) != null && SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).NextRefreshTime >= SysLocalDataBase.Inst.LoginInfo.NowTime)
			RefreshCountDownUI();
		else if (!waitForQueryList)
		{
			waitForQueryList = true;
			RequestMgr.Inst.Request(new QueryMysteryShopInfoReq(MysteryShopConfig._ShopType.MelaleucaFloor));
		}

		if (NextResetRefreshCountTimeNormal < SysLocalDataBase.Inst.LoginInfo.NowDateTime)
		{
			SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).PlayerRefreshTimes = 0;
			SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).LastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		}
	}

	private void SetControl()
	{
		RefreshCountDownUI();

		ClearData();
		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		for (int index = 0; index < SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods.Count; index++)
		{
			UIListItemContainer container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemShopMysteryItem item = container.gameObject.GetComponent<UIElemShopMysteryItem>();
			item.SetData(SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[index]);
			container.Data = item;
			itemList.AddItem(container);
		}
	}

	public void OnQuerynNormalShopInfoSuccess()
	{
		waitForQueryList = false;
		SetControl();
	}

	public void OnQueryMysteryShopInfoFailed()
	{
		waitForQueryList = false;
	}

	public void OnRefreshNormalShopInfoManuallySuccess()
	{
		mannuallyRefreshResponsed = true;
	}

	private void RefreshCountDownUI()
	{
		if (!GameUtility.EqualsFormatTimeString(countDownLabel.Text, SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).NextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime))
			countDownLabel.Text = GameUtility.Time2String(SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).NextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	private bool CheckPackageCapacity(int goodId)
	{
		List<ClientServerCommon.Reward> rewardparam = new List<ClientServerCommon.Reward>();
		rewardparam.Add(ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).GetGoodsById(goodId).reward);
		return GameUtility.CheckPackageCapacity(rewardparam);
	}

	private void ResetGoodsStateToBought(int goodsId, int goodsIndex)
	{
		for (int index = 0; index < itemList.Count; index++)
		{
			UIListItemContainer container = itemList.GetItem(index) as UIListItemContainer;
			UIElemShopMysteryItem item = container.Data as UIElemShopMysteryItem;

			if (item.GoodsId == goodsId && item.GoodsIndex == goodsIndex)
			{
				item.ResetGoodsBuyStates(true);
				return;
			}
		}
	}

	public void OnBuyGoodSuccess(int goodsId, int goodsIndex)
	{
		ResetGoodsStateToBought(goodsId, goodsIndex);
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlShopMystery_Tip_BuySuccess"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChangeClick(UIButton btn)
	{
		var myShopCfg = ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.MelaleucaFloor);

		RequestMgr.Inst.Request(new ChangeMysteryShopInfoReq(MysteryShopConfig._ShopType.MelaleucaFloor, myShopCfg.refreshSet[0].refreshId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGoodIconClick(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnBuyGoodClick(UIButton btn)
	{
		MysteryGoodInfo goodsInfo = btn.Data as MysteryGoodInfo;
		if (CheckPackageCapacity(goodsInfo.GoodId) == false)
			return;

		RequestMgr.Inst.Request(new BuyMysteryGoodReq(MysteryShopConfig._ShopType.MelaleucaFloor, goodsInfo.GoodId, goodsInfo.GoodIndex));
	}
}
