using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;
using System.Collections;

public class UIPnlAvatarIllusion : UIModule
{
	public static string TimeToIllusionLeftTime(long time)
	{
		if (time <= 0)
			return GameUtility.FormatUIString("IllusionLeftTime_Second", 0);

		string result = string.Empty;
		long second = time / 1000 % 60;
		long minute = time / 1000 / 60 % 60;
		long hour = time / 1000 / 60 / 60 % 24;
		long day = time / 1000 / 60 / 60 / 24;

		if (day >= 100)
			result = GameUtility.FormatUIString("IllusionLeftTime_LongDay", day);
		else if (day >= 1)
			result = GameUtility.FormatUIString("IllusionLeftTime_DayHour", day, hour);
		else if (hour >= 1)
			result = GameUtility.FormatUIString("IllusionLeftTime_HourMinute", hour, minute);
		else if (minute >= 1)
			result = GameUtility.FormatUIString("IllusionLeftTime_MinuteSecond", minute, second);
		else
			result = GameUtility.FormatUIString("IllusionLeftTime_Second", second);

		return result;
	}

	com.kodgames.corgi.protocol.IllusionAvatar curIllusionAvatar = null;
	com.kodgames.corgi.protocol.Illusion curIllusion = null;

	public UIAvatarRenderCard renderCard;

	//IllusionList
	public UIScrollList illusionList;
	private List<UIElemEquipIllusionItem> cachedIllusionList = new List<UIElemEquipIllusionItem>();
	public GameObjectPool illusionItemPool;

	//Illusion Info
	public SpriteText illusionNameLabel;

	//Button Layout
	public UIButton buyButton;
	public UIButton attrUpBtn;
	public UIButton attrDownBtn;
	public UIButton surfaceUpBtn;
	public UIButton surfaceDownBtn;
	public UIButton activeBtn;

	public UIElemAssetIcon buyCostIcon;
	public SpriteText realMoneyCostLabel;
	public SpriteText realMoneyCostCountLabel;
	public SpriteText itemCostInfoLabel;

	public UIElemAssetIcon activitybuyCostIcon;
	public SpriteText activityrealMoneyCostLabel;
	public SpriteText activityrealMoneyCostCountLabel;
	public SpriteText activityitemCostInfoLabel;

	public SpriteText cantBuyPromptLabel;

	public UIScrollList arriList;
	public GameObjectPool arriItemPool;
	public GameObject btnRoot;

	//最近一次查询全部角色幻化信息的时间
	public static long lastResponseTime = 0;

	private int avatarRecourseId;
	private float avatarPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		ClearData();

		//int avatarResId = 0x03000003;//盖聂
		int avatarResId = (int)userDatas[0];
		avatarRecourseId = (int)userDatas[0];

		curIllusionAvatar = SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars.Find(n => n.recourseId == avatarResId);
		renderCard.ResetAngles();
		renderCard.Initialize(avatarResId, AvatarAction._Type.IllusionUIIdle, AvatarAction._Type.IllusionUISelectRole);

		realMoneyCostLabel.Text = GameUtility.GetUIString("UIPnlAvatarIllusion_RealMoneyCostLabel");

		curIllusion = null;
		//get using illusion.
		foreach (var illusion in curIllusionAvatar.illusions)
		{
			if (illusion.useStatus != IllusionConfig._UseStatus.UnUse)
			{
				curIllusion = illusion;
				break;
			}
		}

		//如果没有使用中的，但是有已经购买的幻化，默认选中该幻化。
		if (curIllusion == null)
			foreach (var illusion in curIllusionAvatar.illusions)
			{
				if (illusion.endTime == -1 || illusion.endTime > SysLocalDataBase.Inst.LoginInfo.NowTime)
				{
					curIllusion = illusion;
					break;
				}
			}

		//如果没有正在使用的幻化，默认显示第一个幻化
		if (curIllusion == null)
			curIllusion = curIllusionAvatar.illusions[0];

		StartCoroutine("FillList");

		RefreshCurrentIllusionInfo();

		return true;
	}

	private void AddArriDescItem(string desc)
	{
		UIListItemContainer itemContainer = arriItemPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemIllusionArriDescItem item = itemContainer.gameObject.GetComponent<UIElemIllusionArriDescItem>();
		item.SetData(desc);
		// Save item in container
		itemContainer.Data = item;

		// Add to list
		arriList.AddItem(itemContainer);
	}

	private void RefreshCurrentIllusionInfo()
	{
		arriList.ClearList(false);
		arriList.ScrollToItem(0, 0);
		var illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(curIllusion.illusionId);

		if (illusionCfg == null)
		{
			Debug.LogError(string.Format("Invalid IllusionID {0}", curIllusion.illusionId.ToString("X8")));
			return;
		}

		long continueLongtime = (long)illusionCfg.ContinueTime * 3600000L;
		string continueTimeDesc = string.Empty;

		if (continueLongtime > 0)
		{
			long day = continueLongtime / 1000 / 60 / 60 / 24;
			long hour = continueLongtime / 1000 / 60 / 60 % 24;

			if (day >= 1)
			{
				if (hour >= 1)
					continueTimeDesc = GameUtility.FormatUIString("IllusionLeftTime_DescLabelDayHour", day, hour);
				else
					continueTimeDesc = GameUtility.FormatUIString("IllusionLeftTime_DescLabelDay", day);
			}
			else
			{
				if (hour >= 1)
					continueTimeDesc = GameUtility.FormatUIString("IllusionLeftTime_DescLabelHour", day);
			}
		}

		//show curIllusionCfg info
		illusionNameLabel.Text = ItemInfoUtility.GetAssetName(illusionCfg.Id);
		string descLable = string.Empty;
		descLable = GameUtility.FormatUIString("UIPnlAvatarIllusion_IllusionDescLabel", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetAssetDesc(illusionCfg.Id));
		if (illusionCfg.ContinueTime == -1)
			descLable += GameUtility.GetUIString("UIPnlAvatarIllusion_IllusionDescLabel_Forever");
		else if (illusionCfg.ContinueTime > 0 && !string.IsNullOrEmpty(continueTimeDesc))
			descLable += GameUtility.FormatUIString("UIPnlAvatarIllusion_IllusionDescLabel_WithTime", continueTimeDesc);

		AddArriDescItem(descLable);

		AddArriDescItem(GameUtility.FormatUIString("UIPnlAvatarIllusion_IllusionModifierLabel", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetIllusionDesc(illusionCfg.Id)));

		cantBuyPromptLabel.Text = "";
		//btnLayout.HideChildObj(cantBuyPromptLabel.gameObject, true);

		//如果装备中，默认显示该幻化武器。如果未激活，也默认显示该幻化武器
		if ((curIllusion.useStatus != IllusionConfig._UseStatus.UnUse && curIllusion.useStatus != IllusionConfig._UseStatus.Unknow) || curIllusion.endTime == 0 || (curIllusion.endTime < SysLocalDataBase.Inst.LoginInfo.NowTime && curIllusion.endTime != -1))
			renderCard.AvatarChangeIllusionWeapon(curIllusion.illusionId);
		else
			//如果可以装备但是未装备，角色默认使用默认武器
			renderCard.AvatarUseDefaultWeapons();

		if (curIllusion.endTime != -1 && curIllusion.endTime < SysLocalDataBase.Inst.LoginInfo.NowTime)
		{
			activeBtn.gameObject.SetActive(false);
			btnRoot.gameObject.SetActive(true);

			switch (IDSeg.ToAssetType(illusionCfg.ActivateCost.id))
			{
				case IDSeg._AssetType.Special://元宝激活

					activityrealMoneyCostLabel.Text = "";
					activityitemCostInfoLabel.Text = "";
					activitybuyCostIcon.Hide(false);
					activityrealMoneyCostCountLabel.Hide(false);
					activitybuyCostIcon.SetData(illusionCfg.ActivateCost.id);
					activityrealMoneyCostCountLabel.Text = illusionCfg.ActivateCost.count.ToString();
					break;

				case IDSeg._AssetType.Item:
					bool costEnough = ItemInfoUtility.CheckCostEnough(illusionCfg.ActivateCost.id, illusionCfg.ActivateCost.count);

					//激活道具不够，也不能够快捷购买（无法购买或续费）
					if (!costEnough && !SellInNormalShop(illusionCfg.ActivateCost.id))
					{
						btnRoot.gameObject.SetActive(false);
						activeBtn.gameObject.SetActive(false);
						//需要{0}个{1}才能激活{2}，您当前有{3}个，获取途径：{4}
						cantBuyPromptLabel.Text = GameUtility.FormatUIString("UIPnlAvatarIllusion_CanNotBuyPromptLabel",
						illusionCfg.ActivateCost.count,
						ItemInfoUtility.GetAssetName(illusionCfg.ActivateCost.id),
						 ItemInfoUtility.GetAssetName(illusionCfg.Id),
						ItemInfoUtility.GetGameItemCount(illusionCfg.ActivateCost.id)
						);

						//获取途径拼在这里
						if (!string.IsNullOrEmpty(illusionCfg.GetWay))
							cantBuyPromptLabel.Text += illusionCfg.GetWay;
					}
					else
					{
						btnRoot.gameObject.SetActive(false);
						activeBtn.gameObject.SetActive(true);

						activityrealMoneyCostLabel.Text = "";
						activitybuyCostIcon.Hide(true);
						activityrealMoneyCostCountLabel.Hide(true);
						activityitemCostInfoLabel.Hide(false);

						//[color][道具名称: ][color][当前拥有数量]/[需要数量]
						activityitemCostInfoLabel.Text = GameUtility.FormatUIString("UIPnlAvatarIllusion_ItemCostInfoLabel",
						GameDefines.textColorBtnYellow,
						ItemInfoUtility.GetAssetName(illusionCfg.ActivateCost.id),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetGameItemCount(illusionCfg.ActivateCost.id),
						illusionCfg.ActivateCost.count
						);
					}
					break;

				default:
					Debug.LogError("UIPnlAvatarIllusion illusion activate CostID Config ERROR. costId=" + illusionCfg.ActivateCost.id.ToString("0xX8"));
					break;
			}
		}
		else
		{
			btnRoot.gameObject.SetActive(true);
			activeBtn.gameObject.SetActive(false);

			//manage buttons
			switch (curIllusion.useStatus)
			{
				case IllusionConfig._UseStatus.UseAttr:
					attrDownBtn.gameObject.SetActive(true);
					surfaceUpBtn.gameObject.SetActive(true);
					attrUpBtn.gameObject.SetActive(false);
					surfaceDownBtn.gameObject.SetActive(false);
					break;

				case IllusionConfig._UseStatus.UseFacade:
					attrDownBtn.gameObject.SetActive(false);
					surfaceUpBtn.gameObject.SetActive(false);
					attrUpBtn.gameObject.SetActive(true);
					surfaceDownBtn.gameObject.SetActive(true);
					break;

				case IllusionConfig._UseStatus.UseAll:
					attrDownBtn.gameObject.SetActive(true);
					surfaceUpBtn.gameObject.SetActive(false);
					attrUpBtn.gameObject.SetActive(false);
					surfaceDownBtn.gameObject.SetActive(true);
					break;

				case IllusionConfig._UseStatus.UnUse:
					attrDownBtn.gameObject.SetActive(false);
					surfaceUpBtn.gameObject.SetActive(true);
					attrUpBtn.gameObject.SetActive(true);
					surfaceDownBtn.gameObject.SetActive(false);
					break;
			}

			if (curIllusion.endTime != -1)
			{
				buyButton.gameObject.SetActive(true);
				switch (IDSeg.ToAssetType(illusionCfg.ActivateCost.id))
				{
					case IDSeg._AssetType.Special://元宝激活

						realMoneyCostLabel.Text = "";
						itemCostInfoLabel.Text = "";
						buyCostIcon.Hide(false);
						realMoneyCostCountLabel.Hide(false);

						buyCostIcon.SetData(illusionCfg.ActivateCost.id);
						realMoneyCostCountLabel.Text = illusionCfg.ActivateCost.count.ToString();
						break;

					case IDSeg._AssetType.Item:
						realMoneyCostLabel.Hide(true);
						buyCostIcon.Hide(true);
						realMoneyCostCountLabel.Hide(true);
						itemCostInfoLabel.Hide(false);
						//[color][道具名称: ][color][当前拥有数量]/[需要数量]
						itemCostInfoLabel.Text = GameUtility.FormatUIString("UIPnlAvatarIllusion_ItemCostInfoLabel",
						GameDefines.textColorBtnYellow,
						ItemInfoUtility.GetAssetName(illusionCfg.ActivateCost.id),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetGameItemCount(illusionCfg.ActivateCost.id),
						illusionCfg.ActivateCost.count
						 );
						break;

					default:
						Debug.LogError("UIPnlAvatarIllusion illusion activate CostID Config ERROR. costId=" + illusionCfg.ActivateCost.id.ToString("0xX8"));
						break;
				}
			}
			else buyButton.gameObject.SetActive(false);

		}
	}

	//是否正在商城出售（能够快捷购买的条件）
	public static bool SellInNormalShop(int costId)
	{
		bool sellInNormalShop = false;
		int goodId = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodIdByAssetId(costId);
		if (goodId != IDSeg.InvalidId)
		{
			KodGames.ClientClass.Goods goodsInfo = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(goodId);
			GoodConfig.Good goodRewardTheItem = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodId);

			if (goodRewardTheItem != null && goodsInfo != null
				&& goodRewardTheItem.goodsType == _Goods.NormalGoods
				&& goodRewardTheItem.GetGoodItemType() != ItemConfig._Type.Package
				&& goodsInfo.ShowVipLevel <= SysLocalDataBase.Inst.LocalPlayer.VipLevel
				&& goodsInfo.ShowPlayerLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
				sellInNormalShop = true;
		}
		return sellInNormalShop;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		renderCard.PlayIdleAnimation();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		cachedIllusionList.Clear();
		illusionList.ClearList(false);
		illusionList.ScrollListTo(0);
		arriList.ClearList(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	IEnumerator FillList()
	{
		yield return null;

		//long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		curIllusionAvatar.illusions.Sort((m, n) =>
		{
			var mCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(m.illusionId);
			var nCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(n.illusionId);

			return nCfg.SortIndex - mCfg.SortIndex;
			//int mActivated = m.endTime == -1 || m.endTime > nowTime ? 0 : 1;
			//int nActivated = n.endTime == -1 || n.endTime > nowTime ? 0 : 1;

			//var mCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(m.illusionId);
			//var nCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(n.illusionId);

			//if (mActivated != nActivated)
			//    return mActivated - nActivated;
			//else
			//    return nCfg.SortIndex - mCfg.SortIndex;
		});

		int index = 0;

		for (int i = 0; i < curIllusionAvatar.illusions.Count; i++)
		{
			UIElemEquipIllusionItem illItem = illusionItemPool.AllocateItem().GetComponent<UIElemEquipIllusionItem>();

			illItem.SetData(curIllusionAvatar.illusions[i]);

			if (curIllusionAvatar.illusions[i].illusionId == curIllusion.illusionId)
				index = i;

			illusionList.AddItem(illItem.gameObject);
			cachedIllusionList.Add(illItem);
		}

		illusionList.ScrollToItem(index, 0f);
		SetIllusionItemLight(curIllusion.illusionId);
	}

	long nextRefreshTime = 0;
	bool waitForQuery = false;
	//统一管理倒计时或发出协议
	void Update()
	{
		if (SysLocalDataBase.Inst.LoginInfo.NowTime < nextRefreshTime)
			return;

		if (waitForQuery)//when responsed list may be refilled.
			return;

		nextRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime + 1000;

		for (int i = 0; i < cachedIllusionList.Count; i++)
		{
			var illusionItem = cachedIllusionList[i];

			if (illusionItem.Illusion.endTime <= 0)//forever or not activated.
				continue;

			long leftTime = illusionItem.Illusion.endTime - SysLocalDataBase.Inst.LoginInfo.NowTime;

			if (leftTime <= 0 && !waitForQuery && lastResponseTime < illusionItem.Illusion.endTime)
			{
				waitForQuery = true;
				avatarPower = CalculatePower();
				RequestMgr.Inst.Request(new QueryIllusionReq(OnQueryIllusionRes));
				break;
			}
			//Update CountDown SpriteText
			illusionItem.SetCountDownLabelStr(TimeToIllusionLeftTime(leftTime));
		}
	}

	private void SetIllusionItemLight(int illusionId)
	{
		for (int i = 0; i < cachedIllusionList.Count; i++)
			cachedIllusionList[i].SetLight(cachedIllusionList[i].Illusion.illusionId == illusionId);
	}

	//计算战力
	private float CalculatePower()
	{
		if (curIllusionAvatar != null)
		{
			foreach (var illusion in curIllusionAvatar.illusions)
			{
				if (illusion.useStatus == IllusionConfig._UseStatus.UseAll || illusion.useStatus == IllusionConfig._UseStatus.UseAttr)
				{
					var ill = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(illusion.illusionId);
					if (ill != null)
						return ill.IllusionPower;
				}
			}
		}

		return 0f;
	}

	//快捷购买成功时。
	public void OnBuyItemSuccess()
	{
		ReFillUI();
	}

	private void OnQueryIllusionRes(bool success)
	{
		OnIllusionOperateResponse(success);
	}

	private void OnIllusionOperateResponse(bool success)
	{
		if (!success)
		{
			waitForQuery = false;
			return;
		}

		ReFillUI();

		waitForQuery = false;
	}

	private void ReFillUI()
	{
		ClearData();
		if (curIllusionAvatar != null)
			curIllusionAvatar = SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars.Find(n => n.recourseId == curIllusionAvatar.recourseId);

		if (curIllusion != null)
			curIllusion = curIllusionAvatar.illusions.Find(n => n.illusionId == curIllusion.illusionId);

		//Show Power Tips
		float tempAvatarPower = CalculatePower();
		if (tempAvatarPower > avatarPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_Illusion_Up", ItemInfoUtility.GetAssetName(avatarRecourseId), (int)(tempAvatarPower - avatarPower)));
		else if (tempAvatarPower < avatarPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_Illusion_Down", ItemInfoUtility.GetAssetName(avatarRecourseId), (int)(avatarPower - tempAvatarPower)));

		StartCoroutine("FillList");

		RefreshCurrentIllusionInfo();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIllusionIcon(UIButton btn)
	{
		UIElemEquipIllusionItem item = btn.Data as UIElemEquipIllusionItem;
		SetIllusionItemLight(item.Illusion.illusionId);

		curIllusion = item.Illusion;
		RefreshCurrentIllusionInfo();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBuyBtn(UIButton btn)
	{
		avatarPower = CalculatePower();

		if (curIllusion.endTime == -1)
		{
			Debug.LogError("UIPnlAvatarIllusion OnClickBuyBtn curIllusion.endTime == -1");
			return;
		}

		if (waitForQuery)
			return;

		var illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(curIllusion.illusionId);

		if (ItemInfoUtility.CheckCostEnough(illusionCfg.ActivateCost.id, illusionCfg.ActivateCost.count))
		{
			if (curIllusion.endTime <= SysLocalDataBase.Inst.LoginInfo.NowTime)//not activated or overdue
			{
				RequestMgr.Inst.Request(new ActivateAndIllusionReq(curIllusionAvatar.recourseId, curIllusion.illusionId, curIllusion.useStatus, OnIllusionOperateResponse));
				waitForQuery = true;
			}
			else
			{
				RequestMgr.Inst.Request(new ActivateIllusionReq(curIllusionAvatar.recourseId, curIllusion.illusionId, IllusionConfig._ActivateType.Renew, OnIllusionOperateResponse));
				waitForQuery = true;
			}
		}
		else
		{
			switch (IDSeg.ToAssetType(illusionCfg.ActivateCost.id))
			{
				case IDSeg._AssetType.Special://元宝不足
					if (illusionCfg.ActivateCost.id == IDSeg._SpecialId.RealMoney)
					{
						UIDlgMessage.ShowData subShowData = new UIDlgMessage.ShowData();
						MainMenuItem subCancleMenu = new MainMenuItem();
						subCancleMenu.ControlText = GameUtility.GetUIString("UICtrl_Btn_Cancle");

						MainMenuItem subOkMenu = new MainMenuItem();
						subOkMenu.ControlText = GameUtility.GetUIString("UIPnlAvatarIllusion_Recharge");//充值

						string subDlgContent = GameUtility.GetUIString("UIPnlAvatarIllusion_IngotNotAfford");//元宝不足啦，请先充值
						subOkMenu.Callback = (data1) =>
						{
							//弹出充值界面
							SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlRecharge), _UILayer.Top);
							return true;
						};
						subShowData.SetData(subOkMenu.ControlText, subDlgContent, subCancleMenu, subOkMenu);
						SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(subShowData);
					}
					else
					{
						SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlAvatarIllusion_CostNotAfford", ItemInfoUtility.GetAssetName(illusionCfg.ActivateCost.id)));
					}
					break;

				case IDSeg._AssetType.Item:
					GameUtility.ShowNotEnoughtAssetUI(illusionCfg.ActivateCost.id, illusionCfg.ActivateCost.count);
					break;
			}
		}
	}

	//属性幻化
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAttrUpBtn(UIButton btn)
	{
		avatarPower = CalculatePower();

		RequestMgr.Inst.Request(new IllusionReq(curIllusionAvatar.recourseId, curIllusion.illusionId, IllusionConfig._UseStatus.UseAttr, IllusionConfig._IllusionType.Illusion, OnIllusionOperateResponse));
	}
	//外观幻化
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSurfaceUpBtn(UIButton btn)
	{
		avatarPower = CalculatePower();

		RequestMgr.Inst.Request(new IllusionReq(curIllusionAvatar.recourseId, curIllusion.illusionId, IllusionConfig._UseStatus.UseFacade, IllusionConfig._IllusionType.Illusion, OnIllusionOperateResponse));
	}

	//属性卸载
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAttrDownBtn(UIButton btn)
	{
		avatarPower = CalculatePower();

		RequestMgr.Inst.Request(new IllusionReq(curIllusionAvatar.recourseId, curIllusion.illusionId, IllusionConfig._UseStatus.UseAttr, IllusionConfig._IllusionType.CancelIllusion, OnIllusionOperateResponse));
	}

	//属性卸载
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSurfaceDownBtn(UIButton btn)
	{
		avatarPower = CalculatePower();

		RequestMgr.Inst.Request(new IllusionReq(curIllusionAvatar.recourseId, curIllusion.illusionId, IllusionConfig._UseStatus.UseFacade, IllusionConfig._IllusionType.CancelIllusion, OnIllusionOperateResponse));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuideBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlIllusionGuide);
	}
}
