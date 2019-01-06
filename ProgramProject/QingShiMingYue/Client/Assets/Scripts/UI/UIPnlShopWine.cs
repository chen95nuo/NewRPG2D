using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlShopWine : UIModule
{
	public UIScrollList tavernScrollList;
	public GameObjectPool tavernPool;
	public UIElemShopWineRewardItem item;

	private float lastUpdateDelta = 0f;

	public UIChildLayoutControl layoutCoutrol;

	private float LIST_WITH = 316.0f;
	private float LIST_HIEGHT_ONE = 335.0f;
	private float LIST_HIEGHT_TWO = 245.0f;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Set the tab button
		SysUIEnv.Instance.GetUIModule<UIPnlTarven>().SetSelectedBtn(_UIType.UIPnlShopWine);

		SysUIEnv.Instance.GetUIModule<UIPnlTarven>().UpdateActivityNotify();

		RequestMgr.Inst.Request(new QueryTavernInfoReq());
		return true;
	}

	public override void OnHide()
	{
		ClearData();

		base.OnHide();
	}

	private void Update()
	{
		lastUpdateDelta += Time.deltaTime;
		if (lastUpdateDelta > 1f)
		{
			lastUpdateDelta -= 1f;
			for (int index = 0; index < tavernScrollList.Count; index++)
			{
				if (tavernScrollList.GetItem(index).Data is UIElemShopWineItem)
				{
					UIElemShopWineItem item = tavernScrollList.GetItem(index).Data as UIElemShopWineItem;
					item.UpdateFreeRecruitCountDownMessage();
				}
			}
		}
	}

	private void SetLYC(params bool[] hideF)
	{
		for (int index = 0; index < Mathf.Min(hideF.Length, layoutCoutrol.childLayoutControls.Length); index++)
			layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, hideF[index]);
	}

	public void OnQueryTavernInfoSuccess(List<int> rewardIds)
	{
		ClearData();
		if (rewardIds != null && rewardIds.Count > 0 && ActivityManager.Instance != null && ActivityManager.Instance.GetActivity<ActivityMysteryer>().IsOpen)
		{
			SetLYC(false, false);
			tavernScrollList.SetViewableArea(LIST_WITH, LIST_HIEGHT_TWO);
			FillRewardItems(rewardIds);
		}
		else
		{
			SetLYC(true, false);
			tavernScrollList.SetViewableArea(LIST_WITH, LIST_HIEGHT_ONE);
			StartCoroutine("FillList");
		}
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		tavernScrollList.ClearList(false);
		tavernScrollList.ScrollPosition = 0f;
		lastUpdateDelta = 0f;
	}

	private void FillRewardItems(List<int> rewardIds)
	{
		item.SetData(rewardIds);

		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		foreach (var tavern in SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos)
		{
			var tavernConfig = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(tavern.id);
			if (!tavernConfig.IsOpen)
				continue;
			UIListItemContainer uiContainer = tavernPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemShopWineItem item = uiContainer.GetComponent<UIElemShopWineItem>();
			uiContainer.data = item;
			
			item.SetData(tavernConfig.Id, tavern.sepicalRewardIds,tavern);
			tavernScrollList.AddItem(item.gameObject);
		}
	}

	private UIElemShopWineItem GetElemShopWineItem(int tavernId)
	{
		for (int index = 0; index < tavernScrollList.Count; index++)
		{
			if (tavernScrollList.GetItem(index).Data is UIElemShopWineItem)
			{
				UIElemShopWineItem item = tavernScrollList.GetItem(index).Data as UIElemShopWineItem;
				if (item.tavernId == tavernId)
					return item;
			}
		}

		return null;
	}
	
	private void SetAccidentRewardBtnShow(com.kodgames.corgi.protocol.TavernInfo tempTavernInfo)
	{
		for (int i = 0; i < Mathf.Min(SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos.Count, tavernScrollList.Count); i++)
		{
			var info=SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos[i];
			if (tavernScrollList.GetItem(i).Data is UIElemShopWineItem && (tempTavernInfo.id==info.id || tempTavernInfo.id.Equals(info.id)))
			{
				UIElemShopWineItem item = tavernScrollList.GetItem(i).Data as UIElemShopWineItem;
				var tavernConfig = ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(info.id);
				if (info.sepicalRewardIds != null && info.sepicalRewardIds.Count > 0)
				{
					item.accidentRewardBtn.gameObject.SetActive(true);
					item.accidentRewardBtn.Data = new object[] { info.sepicalRewardIds, tavernConfig.SepicalRewardContext };
				}
				else
					item.accidentRewardBtn.gameObject.SetActive(false);
			}
		}
	}

	// Reward Show
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardShowClick(UIButton btn)
	{
		TavernConfig.Tavern tavern = btn.data as TavernConfig.Tavern;
		// Show UI
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlRecruitShow), tavern);
	}

	// Recruit Hero
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRecruitIconClick(UIButton btn)
	{
		TavernConfig.Tavern tavern = btn.data as TavernConfig.Tavern;
		RequestMgr.Inst.Request(new TavernBuyReq(tavern.Id, TavernConfig._TavernType.NormalTavern));
	}

	// Recruit Ten Hero
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRecruitTenClick(UIButton btn)
	{
		TavernConfig.Tavern tavern = btn.data as TavernConfig.Tavern;
		RequestMgr.Inst.Request(new TavernBuyReq(tavern.Id, TavernConfig._TavernType.TenTavern));
	}

	public void OnTavernBuySuccess(KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, com.kodgames.corgi.protocol.TavernInfo tempTavernInfo)
	{
		// Refresh UI.
		GetElemShopWineItem(tempTavernInfo.id).RefreshDynamicUI(tempTavernInfo);
		SetAccidentRewardBtnShow(tempTavernInfo);
		SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(typeof(UIEffectLottery), costAndRewardAndSync.ViewFixReward, costAndRewardAndSync.ViewRandomReward));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSpecialRewardIcon(UIButton btn)
	{
		UIElemAssetIcon itemIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(itemIcon.AssetId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoMystery(UIButton btn)
	{
		if (ActivityManager.Instance == null || ActivityManager.Instance.GetActivity<ActivityMysteryer>() == null || !ActivityManager.Instance.GetActivity<ActivityMysteryer>().IsOpen)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlShopMystery_Tip_NoActivity"));
		else
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlShopMystery);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAccidentRewardBtn(UIButton btn)
	{
		object[] datas = btn.Data as object[];
		List<int> sepicalRewardIds = datas[0] as List<int>;
		string sepicalRewardContext = datas[1].ToString();
		List<Reward> rewards = new List<Reward>();
		foreach (int id in sepicalRewardIds)
		{
			Reward reward = new Reward();
			reward.id = id;
			reward.count = -1;
			reward.level = 1;
			reward.breakthoughtLevel = 0;
			rewards.Add(reward);
		}
		UIDlgShopGiftPreview.ShowData showdata = new UIDlgShopGiftPreview.ShowData();
		var rewardData = new UIDlgShopGiftPreview.RewardData();
		rewardData.rewards = null;
		rewardData.title = string.Empty;
		rewardData.rewards = rewards;
		showdata.rewardDatas.Add(rewardData);
		showdata.showCountInName = false;
		showdata.message = sepicalRewardContext;
		showdata.title = GameUtility.GetUIString("UIPnlShopWine_SepicalReward_Message");
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgShopGiftPreview), _UILayer.Invalid, showdata);

	}
}