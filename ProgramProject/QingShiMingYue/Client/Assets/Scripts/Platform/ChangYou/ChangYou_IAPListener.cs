using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ClientServerCommon;
using KodGames.ExternalCall;
using KodGames.ExternalCall.Platform;
using LitJson;

public class ChangYou_IAPListener : IAPListener
{
	public class ProductInfo
	{
		public int goodsId;
		public int goodsNumber;
		public int goodsPrice;
		public int type;
		public string goodsRegisterId;
		public string goodsName;
		public string goodsIcon;
		public string goodsDescribe;
	}

	private int goodsID;
	private int callback;
	private string additionalData;
	private List<ProductInfo> productInfos;

	private ProductInfo GetProductIdByGoodId(int goodId)
	{
		if (productInfos == null || productInfos.Count <= 0)
			return null;

		var subGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodId, GameUtility.GetDeviceInfo().DeviceType);
		if (subGoodCfg == null)
			subGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodId, _DeviceType.Unknown);

		var goodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetGoodInfoById(goodId);
		foreach (var product in productInfos)
			if (product.goodsPrice == subGoodCfg.costRMB && product.type == goodCfg.type)
				return product;

		return null;
	}

	private void QueryProductList()
	{
		// 现在每次请求，可以修改为判定productInfos 是否有数据，有数据则不重新请求
		ChangYouPlugin.QueryProductList();
		RequestMgr.Inst.RetainBusy();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnQueryProductListCallback(string productsValue)
	{
		RequestMgr.Inst.ReleaseBusy();
		// Parse Product Info.
		productInfos = new List<ProductInfo>();

		try
		{
			// 解析参数
			string data, state;
			ChangYouHelper.ParseIAPListenerResponse(productsValue, out state, out data);

			if (Int32.Parse(state) == 0)
			{
				//获取商品列表失败
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgRecharge_ProductList_Error"));
			}
			else if (Int32.Parse(state) == 1)
			{
				// 解析结果
				var jsonData = JsonMapper.ToObject<JsonData[]>(data);

				foreach (JsonData json in jsonData)
				{
					ProductInfo productInfo = new ProductInfo();
#if !UNITY_EDITOR
#if UNITY_ANDROID
					productInfo.goodsId = Int32.Parse(json["goodsId"].ToString());
					productInfo.goodsIcon = json["goodsIcon"].ToString();				
					productInfo.goodsNumber = Int32.Parse(json["goodsNumber"].ToString());					
					productInfo.goodsDescribe = json["goodsDescribe"].ToString();					
					var price = Single.Parse(json["goodsPrice"].ToString());
					productInfo.goodsPrice = (int)price;
					productInfo.type = Int32.Parse(json["type"].ToString());
					productInfo.goodsRegisterId = json["goodsRegisterId"].ToString();
					productInfo.goodsName = json["goodsName"].ToString();
#elif UNITY_IPHONE
					productInfo.goodsId = Int32.Parse(json["goods_id"].ToString());
					productInfo.goodsIcon = json["goods_icon"].ToString();
					productInfo.goodsNumber = Int32.Parse(json["goods_number"].ToString());
					productInfo.goodsDescribe = json["goods_describe"].ToString();
					var price = Single.Parse(json["goods_price"].ToString());
					productInfo.goodsPrice = (int)price;
					productInfo.type = Int32.Parse(json["type"].ToString());
					productInfo.goodsRegisterId = json["goods_register_id"].ToString();
					productInfo.goodsName = json["goods_name"].ToString();
#endif
#else
					productInfo.goodsId = Int32.Parse(json["goodsId"].ToString());
					productInfo.goodsIcon = json["goodsIcon"].ToString();
					productInfo.goodsNumber = Int32.Parse(json["goodsNumber"].ToString());
					productInfo.goodsDescribe = json["goodsDescribe"].ToString();
					var price = Single.Parse(json["goodsPrice"].ToString());
					productInfo.goodsPrice = (int)price;
					productInfo.type = Int32.Parse(json["type"].ToString());
					productInfo.goodsRegisterId = json["goodsRegisterId"].ToString();
					productInfo.goodsName = json["goodsName"].ToString();
#endif
					productInfos.Add(productInfo);
				}

				PayProduct();
			}
			else // 服务器处理失败
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgRecharge_ServerProgress_Error"));
		}
		catch (System.Exception exception)
		{
			//解析失败
			Debug.Log("GetProduct Parse Exception : " + exception.StackTrace);
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgRecharge_Analytical_Error"));
		}
	}

	private void PayProduct()
	{
		var productInfo = GetProductIdByGoodId(goodsID);
		if (productInfo == null)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITIP_QueryProductError"));
			return;
		}

		var appSubGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodsID, GameUtility.GetDeviceInfo().DeviceType);
		if (appSubGoodCfg == null)
			appSubGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodsID, _DeviceType.Unknown);

		ChangYouPlugin.Buy(Platform.Instance.ChannelMessage.ChannelUniqueId, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level,
			SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString(), SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID.ToString(),
			GetPartnerOrderId(productInfo.goodsRegisterId, additionalData),
			productInfo.goodsId,
			productInfo.goodsName,
			productInfo.goodsRegisterId,
			appSubGoodCfg.costRMB,
			appSubGoodCfg.realMoneyCount,
			appSubGoodCfg.desc,
			SysLocalDataBase.Inst.LocalPlayer.Name,
			KodPlatform.Instance.ChannelMessage.AccessToken,
			KodPlatform.Instance.ChannelMessage.Oid,
			SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaName);

		payProductCallback = callback;
	}

	public override bool PayProduct(string productID, int goodsID, int count, int callback, string additionalData)
	{
		this.goodsID = goodsID;
		this.callback = callback;
		this.additionalData = additionalData;

		QueryProductList();

		return true;
	}
}