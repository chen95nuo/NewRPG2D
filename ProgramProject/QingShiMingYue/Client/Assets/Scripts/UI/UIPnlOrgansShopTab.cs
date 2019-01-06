using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlOrgansShopTab : UIModule
{
	public UIScrollList shopItemList;
	public GameObjectPool itemPool;
	public SpriteText refreshTimerLable;
	public SpriteText consumptionCoin;
	public UIElemAssetIcon coinIcon;

	public UIScrollList timerList;
	public GameObjectPool timeItemPool;

	public GameObject refreshTimeTitle;
	public GameObject todayTimeTitle;
	public UIButton refreshBtn;
	public GameObject refreshDataBG;

	private long nextRefreshTime = 0;
	private float deltaTime = 0;
	private List<BeastConfig.RefreshInfo> refreshTimerInfos = new List<BeastConfig.RefreshInfo>();
	private UIElemBeastExchageItem esffItem;
	
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlOrganTab))
			SysUIEnv.Instance.GetUIModule<UIPnlOrganTab>().ChangeTabButtons(_UIType.UIPnlOrgansShopTab);
		UIHide(false);
		refreshTimerInfos = ConfigDatabase.DefaultCfg.BeastConfig.BeastPartShops.RefreshInfos;
		RequestMgr.Inst.Request(new QueryBeastExchangeShopReq());
		return true;
	}

	private void UIHide(bool isShow)
	{
		refreshTimeTitle.SetActive(isShow);
		todayTimeTitle.SetActive(isShow);
		refreshBtn.Hide(!isShow);
		refreshDataBG.SetActive(isShow);
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	//主查询协议正确返回
	public void OnQueryBeastExchangeShopSuccess(List<ZentiaExchange> beastExchanges, KodGames.ClientClass.Cost cost, long nextRefreshTime)
	{
		UIHide(true);
		SetData(beastExchanges);
		this.nextRefreshTime = nextRefreshTime;
		coinIcon.SetData(cost.Id);
		consumptionCoin.Text = cost.Count.ToString();
	}


	public void OnRefreshBeastExchangeShopSuccess(List<ZentiaExchange> beastExchanges)
	{
		UIHide(true);
		SetData(beastExchanges);
	}

	private void SetData(List<ZentiaExchange> beastExchanges)
	{
		
		shopItemList.ClearList(false);
		foreach (ZentiaExchange exchage in beastExchanges)
		{
			var container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemBeastExchageItem>();
			item.ClearData(true);
			item.SetData(exchage);
			shopItemList.AddItem(container.gameObject);
		}

		SetToDayTimes();
	}

	private void ClearData()
	{
		shopItemList.ClearList(false);
		timerList.ClearList(false);
	}

	private void SetToDayTimes()
	{
		timerList.ClearList(false);
		int index = IsTimerPeriod();
		foreach (var rtInfo in refreshTimerInfos)
		{
			var container = timeItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<SpriteText>();
			System.DateTime dateTime = ConfigDatabase.DefaultCfg.BeastConfig.toDateTime(rtInfo.RefreshTime);
			item.Text = string.Format("{0}{1}:{2}:{3}", rtInfo.Index == index ? GameDefines.OrganExchangeToDayTimer2 : GameDefines.OrganExchangeToDayTimer1, GetIntToStr(dateTime.Hour), GetIntToStr(dateTime.Minute), GetIntToStr(dateTime.Second));
			timerList.AddItem(container.gameObject);
		}
	}

	private string GetIntToStr(int num)
	{
		if (num < 10)
			return string.Format("0{0}", num);
		else
			return num.ToString();
	}

	private int IsTimerPeriod()
	{
		int hour = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Hour;
		int minute = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Minute;
		int second = SysLocalDataBase.Inst.LoginInfo.NowDateTime.Second;
		int index = 0;
		foreach (var rtInfo in refreshTimerInfos)
		{
			System.DateTime dateTime = ConfigDatabase.DefaultCfg.BeastConfig.toDateTime(rtInfo.RefreshTime);
			if (dateTime.Hour > hour || (dateTime.Hour == hour && dateTime.Minute > minute) || (dateTime.Hour == hour && dateTime.Minute == minute && dateTime.Second > second))
				return rtInfo.Index;
			if (index == refreshTimerInfos.Count - 1 && (dateTime.Hour < hour || (dateTime.Hour == hour && dateTime.Minute < minute) || (dateTime.Hour == hour && dateTime.Minute == minute && dateTime.Second < second)))
				return refreshTimerInfos[0].Index;
			index++;
		}
		
		return 0;
	}

	private void Update()
	{
		if (IsShown == false || nextRefreshTime == 0)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1.0f)
		{
			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

			// Set Refresh Label.
			if (refreshTimerLable != null)
				UIUtility.UpdateUIText(refreshTimerLable, nextRefreshTime <= 0 ? "00:00:00" : GameUtility.Time2String(nextRefreshTime - nowTime));

			if (nextRefreshTime <= nowTime)
				RequestMgr.Inst.Request(new QueryBeastExchangeShopReq());

			deltaTime = 0f;
		}
	}

	//刷新按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefreshBtn(UIButton btn)
	{
		RequestMgr.Inst.Request(new RefreshBeastExchangeShopReq());
	}
}