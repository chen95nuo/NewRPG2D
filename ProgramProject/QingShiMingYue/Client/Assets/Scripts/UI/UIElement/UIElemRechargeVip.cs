using UnityEngine;
using System;
using ClientServerCommon;
using System.Collections.Generic;

public class UIElemRechargeVip : MonoBehaviour
{
	public SpriteText vipTitleLabel;
	public SpriteText vipPrivilegeDetail;

	public UIBox bgBox;

	public GameObject goodArea;

	public UIElemAssetIcon goodIcon;
	public SpriteText goodNameText;

	public UIElemAssetIcon goodNormalPriceIcon;
	public SpriteText goodNormalPrice;
	public SpriteText goodNormalPriceText;

	public UIElemAssetIcon goodSpecialPriceIcon;
	public SpriteText goodSpecialPrice;
	public SpriteText goodSpecialPriceText;

	public AutoSpriteControlBase goodPriceUnderLine;

	public UIButton buyBtn;
	public UIBox alreadyGot;

	private const float ENPTY_BG_HEIGHT = 45f;
	private const float GOOD_AREA_HEIGHT = 60f;
	private const float HEIGHT_WITH_GOOD = 40f;
	private const float GOOD_DEFAULT_HEIGHT = 63f;

	private VipConfig.Vip vipCfg;

	public void SetData(VipConfig.Vip vip)
	{
		vipCfg = vip;
		// Set title
		vipTitleLabel.Text = GameUtility.FormatUIString("UIDlgRechargeVIp_BGTitle", vipCfg.level);
		// Set content

		var privilegeStr = new System.Text.StringBuilder();
		foreach (VipConfig.Privilege privilege in vipCfg.privileges)
		{
			privilegeStr.AppendFormat("{0}.{1}", vipCfg.privileges.IndexOf(privilege) + 1, privilege.desc);
			privilegeStr.AppendLine();
		}

		vipPrivilegeDetail.Text = privilegeStr.ToString();

		SetGoodData(vipCfg.goodId, vipCfg.level, vipCfg.privileges.Count);

		//ResetBG();

		goodIcon.border.Data = vipCfg.goodId;
	}

	//到达VIP等级之后高亮
	//public void ResetBG()
	//{
	//    UIElemTemplate.Inst.listItemBgTemplate.SetListItemBg(bgBox, SysLocalDataBase.Inst.LocalPlayer.VipLevel >= vipCfg.level);
	//}

	private void SetGoodData(int goodId, int vipLevel, int privilegeCount)
	{
		if (goodId != IDSeg.InvalidId)
		{
			var goodsCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodId);
			int assetId = goodId;

			// good icon
			if (goodsCfg != null && goodsCfg.assetIconId != IDSeg.InvalidId)
			{
				assetId = goodsCfg.assetIconId;
				goodIcon.SetData(assetId);
			}
			else
				goodIcon.SetData(goodId);

			// good name
			goodNameText.Text = ItemInfoUtility.GetAssetName(assetId);

			var goods = GetGoodByGoodId(goodId);
			if (goods == null)
				goodArea.SetActive(false);
			else
			{
				goodArea.SetActive(true);

				buyBtn.Hide(goods.RemainBuyCount <= 0);
				alreadyGot.Hide(goods.RemainBuyCount > 0);
				
				buyBtn.Data = goods;
				
				// normal price
				int goodPrice = goodsCfg.costs[0].count;
				int costId = goodsCfg.costs[0].id;
				
				// sell price
				goodSpecialPriceIcon.SetData(costId);
				goodSpecialPriceText.Text = (goods.Discount != 0 ? goods.Discount : goodPrice).ToString();

				// discount price
				bool hasDiscount = goods.Discount != 0;
				goodNormalPrice.Hide(!hasDiscount);
				goodNormalPriceIcon.Hide(!hasDiscount);
				goodPriceUnderLine.Hide(!hasDiscount);
				
				// If has discount , show the original price and under line
				if (hasDiscount)
				{
					goodNormalPriceIcon.SetData(costId);
					goodNormalPriceText.Text = goodPrice.ToString();
					goodPriceUnderLine.width = goodNormalPriceText.GetWidth(goodNormalPriceText.Text);
				}
			}
		}
		else
			goodArea.SetActive(false);

		bgBox.SetSize(bgBox.width, ENPTY_BG_HEIGHT + vipPrivilegeDetail.GetLineHeight() * privilegeCount + (goodArea.activeSelf ? GOOD_AREA_HEIGHT : 0f));
		goodArea.transform.localPosition = new Vector3(goodArea.transform.localPosition.x, HEIGHT_WITH_GOOD - bgBox.height, goodArea.transform.localPosition.z);
	}

	private KodGames.ClientClass.Goods GetGoodByGoodId(int goodId)
	{
		foreach (var goods in SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods)
		{
			if (goods.GoodsID == goodId)
				return goods;
		}

		return null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnGoodIconBtnClick(UIButton btn)
	{
		int goodId = (int)btn.Data;
		GoodConfig.Good goodsConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodId);

		if (goodsConfig.assetIconId != IDSeg.InvalidId)
			GameUtility.ShowAssetInfoUI(goodsConfig.assetIconId);
		else
		{
			if (goodsConfig.rewards.Count == 1)
			{
				if (GameUtility.ShowAssetInfoUI(goodsConfig.rewards[0].id) == false)
					SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, goodId);
			}
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, goodId);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnGoodBuyBtnClick(UIButton btn)
	{
		int goodId = (btn.Data as KodGames.ClientClass.Goods).GoodsID;

		var goodsData = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(goodId);
		if (goodsData != null)
			RequestMgr.Inst.Request(new BuyGoodsReq(goodId, 1, (btn.Data as KodGames.ClientClass.Goods).StatusIndex, OnGoodBuySuccess, OnGoodBuyFail));
	}

	public void OnGoodBuySuccess(int goodsId, int amount, KodGames.ClientClass.Reward reward, List<KodGames.ClientClass.Cost> costs)
	{
		buyBtn.Hide(true);
		alreadyGot.Hide(false);

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
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	public void OnGoodBuyFail(int goodsId)
	{
		;
	}

	private bool OnCheckTheGood(object obj)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlPackageItemTab);
		return true;
	}
}


