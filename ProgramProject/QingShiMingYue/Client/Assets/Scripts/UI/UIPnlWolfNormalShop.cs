using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlWolfNormalShop : UIModule
{
	public SpriteText countDownLabel;
	public UIScrollList itemList;
	public GameObjectPool itemPool;
	public SpriteText resetCostLabel;
	public UIElemAssetIcon resetIcon;

	private List<KodGames.ClientClass.WolfSmokeGoodsInfo> wolfSmokeGoods;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;


		var resetCost = ConfigDatabase.DefaultCfg.WolfSmokeConfig.Shop.RefreshCost[0];
		resetIcon.SetData(resetCost.id);
		resetCostLabel.Text = resetCost.count.ToString();

		SysUIEnv.Instance.GetUIModule<UIPnlWolfShop>().SetSelectedBtn(_UIType.UIPnlWolfNormalShop);
		RequestMgr.Inst.Request(new QueryWolfSmokeShop());

		return true;
	}

	private void ClearData()
	{
		// Clear List.
		StopCoroutine("FillList");
		itemList.ClearList(false);
		itemList.ScrollPosition = 0f;
	}

	private void SetControl()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		for (int index = 0; index < this.wolfSmokeGoods.Count; index++)
		{
			UIListItemContainer container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemShopMysteryItem item = container.gameObject.GetComponent<UIElemShopMysteryItem>();
			item.SetData(wolfSmokeGoods[index]);
			container.Data = item;
			itemList.AddItem(container);
		}
	}

	public void OnQuerynNormalShopInfoSuccess(List<KodGames.ClientClass.WolfSmokeGoodsInfo> wolfSmokeGoods)
	{
		this.wolfSmokeGoods = wolfSmokeGoods;
		SetControl();
	}

	public void OnRefreshNormalShopInfoManuallySuccess()
	{

	}

	private bool CheckPackageCapacity(int goodId)
	{
		List<ClientServerCommon.Reward> rewardparam = new List<ClientServerCommon.Reward>();
		rewardparam.Add(ConfigDatabase.DefaultCfg.WolfSmokeConfig.Shop.GetGoodsById(goodId).reward);

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
		RequestMgr.Inst.Request(new QueryRefreshWolfSmokeShop());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGoodIconClick(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnBuyGoodClick(UIButton btn)
	{

		WolfSmokeGoodsInfo goodsInfo = btn.Data as WolfSmokeGoodsInfo;

		if (CheckPackageCapacity(goodsInfo.GoodsId) == false)
			return;

		RequestMgr.Inst.Request(new QueryBuyWolfSmokeShop(goodsInfo.GoodsId, goodsInfo.GoodsIndex));
	}
}
