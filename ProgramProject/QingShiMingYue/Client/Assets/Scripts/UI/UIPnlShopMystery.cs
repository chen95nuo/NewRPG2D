using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlShopMystery : UIModule
{
	public List<UIElemShopMysteryItem1> goodItems;
	public SpriteText resetTimeLabel;
	public SpriteText activityTimeLabel;

	public SpriteText avatarTimeLabel;
	public SpriteText equipmentTimeLabel;
	public UIElemAssetIcon avatarCostIcon;
	public UIElemAssetIcon equipmentCostIcon;
	public SpriteText avatarCostNumber;
	public SpriteText equipmentCostNumber;
	public SpriteText avatarCostStaticLabel;
	public SpriteText equipmentCostStaticLabel;

	public List<UIButton> resetBtns;

	private bool waitControl;

	private float deltaTime;
	private int deleteItemId;

	// Refresh Time.
	private long lastRefreshTime;
	private long nextRefreshTime;

	private ActivityMysteryer ActivityData
	{
		get
		{
			if (ActivityManager.Instance != null)
				return ActivityManager.Instance.GetActivity<ActivityMysteryer>();

			return null;
		}
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		resetBtns[0].Data = MysteryerConfig._RefreshType.Avatar;
		resetBtns[1].Data = MysteryerConfig._RefreshType.Equip;

		for (int index = 0; index < goodItems.Count; index++)
			goodItems[index].InitData();

		avatarCostIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		equipmentCostIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		avatarCostIcon.Hide(true);
		equipmentCostIcon.Hide(true);

		avatarCostStaticLabel.Text = string.Empty;
		equipmentCostStaticLabel.Text = string.Empty;

		//初始化这个时间，主要是用来第一次进行更新函数时能尽快渲出倒计时
		deltaTime = 1.1f;

		// 注册活动按钮绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Activity, UpdateActivityNotify);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlTarven>().SetSelectedBtn(_UIType.UIPnlShopMystery);
		SysUIEnv.Instance.GetUIModule<UIPnlTarven>().activityNotify.Hide(true);

		QueryAndReq();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消活动按钮绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Activity, UpdateActivityNotify);
	}

	private void ClearData()
	{
		this.lastRefreshTime = 0;
		this.nextRefreshTime = 0;
	}

	//填充三个物品
	private void InitGoodUI(List<com.kodgames.corgi.protocol.MysteryerGood> goods)
	{
		goods.Sort((g1, g2) =>
			{
				return g1.place - g2.place;
			});

		for (int index = 0; index < Mathf.Min(goods.Count, goodItems.Count); index++)
			goodItems[index].SetData(goods[index]);
	}

	private void InitResetUI(List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh)
	{
		int strAvatar = 0;
		int strEquipment = 0;
		int iconAvatar = 0;
		int iconEquipment = 0;
		int costAvatar = 0;
		int costEquipment = 0;

		for (int index = 0; index < refresh.Count; index++)
		{
			if (refresh[index].type == MysteryerConfig._RefreshType.Avatar)
			{
				strAvatar = refresh[index].remainTimes;
				iconAvatar = refresh[index].cost.id;
				costAvatar = refresh[index].cost.count;
			}
			if (refresh[index].type == MysteryerConfig._RefreshType.Equip)
			{
				strEquipment = refresh[index].remainTimes;
				iconEquipment = refresh[index].cost.id;
				costEquipment = refresh[index].cost.count;
			}
		}

		if (strAvatar > 0)
			avatarTimeLabel.Text = GameUtility.FormatUIString("UIPnlShopMystery_AvatarReset", strAvatar);
		else
			avatarTimeLabel.Text = string.Empty;
		if (strEquipment > 0)
			equipmentTimeLabel.Text = GameUtility.FormatUIString("UIPnlShopMystery_EquipmentReset", strEquipment);
		else
			equipmentTimeLabel.Text = string.Empty;


		avatarCostStaticLabel.Text = GameUtility.FormatUIString("UIPnlShopMystery_Cost2", ItemInfoUtility.GetAssetName(iconAvatar));
		equipmentCostStaticLabel.Text = GameUtility.FormatUIString("UIPnlShopMystery_Cost2", ItemInfoUtility.GetAssetName(iconEquipment));

		avatarCostIcon.Hide(false);
		equipmentCostIcon.Hide(false);
		avatarCostIcon.SetData(iconAvatar);
		equipmentCostIcon.SetData(iconEquipment);

		avatarCostNumber.Hide(false);
		equipmentCostNumber.Hide(false);
		avatarCostNumber.Text = costAvatar.ToString();
		equipmentCostNumber.Text = costEquipment.ToString();
	}

	private void SetResetTimeLabel()
	{
		if (ActivityData != null)
		{
			long time = nextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime;
			time = time < 0 ? 0 : time;
			string strTime = GameUtility.EqualsFormatTimeStringWithoutThree(time);
			string str = "";
			if (nextRefreshTime > 0)
				str = GameUtility.FormatUIString("UIPnlShopMystery_ResetTimeLabel", GameDefines.textColorBtnYellow.ToString(), GameDefines.txCountDownColor.ToString(), strTime);

			UIUtility.UpdateUIText(resetTimeLabel, str);
		}
	}

	private void SetActivityTimeLabel()
	{
		if (ActivityData != null)
		{
			long time = 0;
			long day = 0;
			if (nextRefreshTime > 0)
			{
				time = ActivityData.ActivityInfo.CloseTime - SysLocalDataBase.Inst.LoginInfo.NowTime;
				day = time / 1000 / 60 / 60 / 24;
			}

			string str = "";

			if (deleteItemId != 0)
				str = GameUtility.FormatUIString("UIPnlShopMystery_ActivityTimeLabel",
								GameDefines.textColorBtnYellow.ToString(),
								GameDefines.txCountDownColor.ToString(),
								day,
								GameDefines.textColorBtnYellow.ToString(),
								GameDefines.textColorWhite.ToString(),
								ItemInfoUtility.GetAssetName(deleteItemId),
								GameDefines.textColorBtnBlue_font.ToString());
			else
				str = GameUtility.FormatUIString("UIPnlShopMystery_ActivityTimeNoDeleteLabel",
								GameDefines.textColorBtnYellow.ToString(),
								GameDefines.txCountDownColor.ToString(),
								day,
								GameDefines.textColorBtnYellow.ToString());

			UIUtility.UpdateUIText(activityTimeLabel, str);
		}
	}

	private void Update()
	{
		if (!waitControl && ActivityData != null)
		{
			deltaTime += Time.deltaTime;
			if (deltaTime > 1.0f)
			{
				deltaTime = 0f;

				if (nextRefreshTime > 0 && SysLocalDataBase.Inst.LoginInfo.NowTime > nextRefreshTime)
					QueryAndReq();

				SetResetTimeLabel();
				SetActivityTimeLabel();
			}
		}
	}

	public void UpdateActivityNotify()
	{
		if (this.IsShown && ActivityData.IsOpen)
			waitControl = false;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGoodIconClick(UIButton btn)
	{
		UIElemAssetIcon item = (btn.Data) as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(item.AssetId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnBuyGoodClick(UIButton btn)
	{
		int place = (int)(btn.IndexData);
		int id = (int)(btn.Data);
		RequestMgr.Inst.Request(new BuyMysteryerReq(place, id));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoWine(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlShopWine);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnResetGood(UIButton btn)
	{
		int type = (int)(btn.Data);
		RequestMgr.Inst.Request(new RefreshMysteryerReq(type));
	}

	private void QueryAndReq()
	{
		if (!waitControl)
		{
			waitControl = true;
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlShopMystery)))
				RequestMgr.Inst.Request(new QueryMysteryerReq());
		}
	}

	public void QueryAndRefreshMysteryerResSuccess(List<com.kodgames.corgi.protocol.MysteryerGood> goods, List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh, int deleteItemId)
	{
		if (ActivityData != null)
		{
			//Fill UI.
			this.deleteItemId = deleteItemId;
			InitGoodUI(goods);
			InitResetUI(refresh);
			waitControl = false;

			// Reset Refresh Time.
			lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			nextRefreshTime = ActivityData.NextRefreshTime(lastRefreshTime);
		}
	}

	public void RefreshMysteryerResSuccess(List<com.kodgames.corgi.protocol.MysteryerGood> goods, List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh)
	{
		if (ActivityData != null)
		{
			InitGoodUI(goods);
			InitResetUI(refresh);
			waitControl = false;
		}
	}

	public void QueryRefreshMysterResFailed()
	{
		waitControl = false;
		lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		nextRefreshTime = ActivityData.NextRefreshTime(lastRefreshTime);
	}

	public void BuyMysteryerReqSuccess(com.kodgames.corgi.protocol.MysteryerGood goods)
	{
		for (int index = 0; index < goodItems.Count; index++)
		{
			if ((int)(goodItems[index].buyBtn.IndexData) == goods.place)
				goodItems[index].SelectBuyBtnToggle(true);
		}

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlShopMystery_Tip_BuySuccess"));
	}

	public void ResetWaitControlValue()
	{
		waitControl = false;
		nextRefreshTime = ActivityData.NextRefreshTime(lastRefreshTime);
	}
}
