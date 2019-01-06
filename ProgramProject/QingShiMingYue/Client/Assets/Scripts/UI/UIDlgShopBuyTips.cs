using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgShopBuyTips : UIModule
{
	/// <summary>
	/// Show data.
	/// title,unitPrice,itemIconName,priceIconName,totalCount should not be null
	/// </summary>
	public class ShowData
	{
		public delegate bool Calback(int assetId, int totalCount, object data);

		public int goodsId;
		public int unitPrice;
		public int totalCount;
		public int maxCount;

		public Calback okCallback = null;
		public object okCallbackData = null;
	}

	// Use to layout button
	public SpriteText titleText;
	public SpriteText itemCountField;
	public UIButton increaseBtn;
	public UIButton decreaseBtn;
	public UIButton button1;
	public UIButton button2;
	public UIElemAssetIcon itemIcon;
	public UIElemAssetIcon priceIcon;

	//¹ºÂòµÄÌáÊ¾
	public SpriteText buyTitleText;

	public UIChildLayoutControl layOut;

	protected ShowData showData;

	public EZAnimation.EASING_TYPE easingType;

	public bool useAcc;
	public float minDelta;
	public float deltaDuration;
	public int deltaCount;
	protected float maxDelta;

	public int minAcc;
	public int acc;
	public int accCount;
	protected int maxAcc;

	protected int currentAcc;

	protected EZAnimation.Interpolator interpolator;
	protected float pressTimer;
	protected float lastChangeTime;
	protected bool increaseBtnPressed = false;
	protected bool decreaseBtnPressed = false;

	protected int affordCount = int.MaxValue;
	protected readonly Color TXT_AFFORD = GameDefines.txColorWhite;
	protected readonly Color TXT_NOT_AFFORD = GameDefines.txColorRed;

	protected const int MAX_SHOW_COUNT = 999;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		increaseBtn.SetInputDelegate(IncreaseInputDel);
		decreaseBtn.SetInputDelegate(DecreaseInputDel);

		return true;
	}

	public void ShowDialog(ShowData showData)
	{
		if (ShowSelf() == false)
			return;

		this.showData = showData;
		this.increaseBtnPressed = false;
		this.decreaseBtnPressed = false;

		affordCount = GetAffordCount();

		maxDelta = minDelta + ((float)deltaCount) * deltaDuration;
		maxAcc = minAcc + acc * accCount;
		currentAcc = minAcc;

		GoodConfig.Good goodConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(showData.goodsId);
		int showIconId = goodConfig.assetIconId == IDSeg.InvalidId ? goodConfig.id : goodConfig.assetIconId;

		// Title
		titleText.Text = ItemInfoUtility.GetAssetName(showIconId);
		buyTitleText.Text = GameUtility.FormatUIString("UIDlgShop_BuyGoods_Tips", ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(showData.goodsId).name);


		// Count of the goods you want to buy
		itemCountField.Text = showData.totalCount.ToString();

		// Icon
		itemIcon.SetData(showIconId);

		// Price icon
		int costId = goodConfig.costs[0].id;
		priceIcon.SetData(costId);

		// Count
		SetGoodsCount(showData.totalCount);
	}

	protected void Update()
	{
		// Update goods count
		if (increaseBtnPressed || decreaseBtnPressed)
		{
			pressTimer += Time.deltaTime;
			float deltaPerSecond = interpolator(Mathf.Min(pressTimer, deltaDuration), minDelta, maxDelta - minDelta, deltaDuration);

			if (deltaPerSecond != 0 && pressTimer - lastChangeTime > (1 / deltaPerSecond))
			{
				if (currentAcc < maxAcc && useAcc)
					currentAcc += acc;

				lastChangeTime += (1 / deltaPerSecond);

				if (increaseBtnPressed)
					SetGoodsCount(showData.totalCount + (useAcc ? currentAcc : 1));
				else if (decreaseBtnPressed)
					SetGoodsCount(showData.totalCount - (useAcc ? currentAcc : 1));
			}
		}
	}

	protected void SetGoodsCount(int count)
	{
		showData.totalCount = GetDisplayGoodsCount(count);
		itemCountField.Text = showData.totalCount.ToString();

		string totalCostCount = (showData.totalCount * showData.unitPrice).ToString();

		if (showData.totalCount > affordCount)
			priceIcon.border.Text = string.Format("{0}{1}", TXT_NOT_AFFORD.ToString(), totalCostCount);
		else
			priceIcon.border.Text = string.Format("{0}{1}", TXT_AFFORD.ToString(), totalCostCount);
	}

	protected int GetDisplayGoodsCount(int count)
	{
		if (count <= 0)
			return 1;
		else if (count > MAX_SHOW_COUNT)
			return MAX_SHOW_COUNT;
		else if (showData.maxCount > 0 && count > showData.maxCount)
			return showData.maxCount;

		return count;
	}

	protected int GetAffordCount()
	{
		List<Cost> costs = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(showData.goodsId).costs;

		if (costs == null)
			return int.MaxValue;

		if (ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(showData.goodsId).costs[0].id == IDSeg._SpecialId.GameMoney)
			return SysLocalDataBase.Inst.LocalPlayer.GameMoney / showData.unitPrice;

		if (ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(showData.goodsId).costs[0].id == IDSeg._SpecialId.RealMoney)
			return SysLocalDataBase.Inst.LocalPlayer.RealMoney / showData.unitPrice;

		return int.MaxValue;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickIncreaseBtn(UIButton btn)
	{
		SetGoodsCount(showData.totalCount + 1);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickDecreaseBtn(UIButton btn)
	{
		SetGoodsCount(showData.totalCount - 1);
	}

	protected void IncreaseInputDel(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				increaseBtnPressed = true;
				decreaseBtnPressed = false;
				currentAcc = minAcc;
				StartPressEasing();
				break;

			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				increaseBtnPressed = false;
				decreaseBtnPressed = false;
				break;
		}
	}

	protected void DecreaseInputDel(ref POINTER_INFO ptr)
	{
		switch (ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
				increaseBtnPressed = false;
				decreaseBtnPressed = true;
				currentAcc = minAcc;
				StartPressEasing();
				break;

			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				increaseBtnPressed = false;
				decreaseBtnPressed = false;
				break;
		}
	}

	protected void StartPressEasing()
	{
		pressTimer = 0;
		lastChangeTime = 0;
		interpolator = EZAnimation.GetInterpolator(easingType);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClose(UIButton btn)
	{
		HideSelf();
	}

	// button1 click
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickButton1(UIButton btn)
	{
		HideSelf();
	}

	// button2 click
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickButton2(UIButton btn)
	{
		int goodRemainCount = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(showData.goodsId).RemainBuyCount;
		if (goodRemainCount > 0 && showData.totalCount > goodRemainCount)
		{
			string errorMsg = string.Format(GameUtility.GetUIString("UIPnlShop_BuyGoods_Max_Count"), SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(showData.goodsId).RemainBuyCount);
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errorMsg);
			return;
		}

		if (showData != null && showData.okCallback != null)
			if (showData.okCallback(showData.goodsId, showData.totalCount, showData.okCallbackData) == false)
				return;

		HideSelf();
	}
}
