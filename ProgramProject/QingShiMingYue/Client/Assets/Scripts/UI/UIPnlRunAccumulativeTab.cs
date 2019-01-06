using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlRunAccumulativeTab : UIModule
{
	public SpriteText desc, activeLabel, moneyLabel;
	public UIScrollList activityList;
	public GameObjectPool activityPool;
	public UIButton rechargeBtn;

	private int activityId;
	private bool waitForResponse;
	private float deltaTime;
	private long lastRefreshTime;
	private long nextRefreshTime;
	private ActivityRun ActivityData
	{
		get
		{
			if (ActivityManager.Instance != null)
				return ActivityManager.Instance.GetActivityInRunActivity(activityId) as ActivityRun;
			else
				return null;
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.activityId = (int)userDatas[0];

		//Set ActivityIcon Stage.
		SysUIEnv.Instance.GetUIModule<UIPnlRunActivityTab>().SetActivityIconBackLight(this.activityId);

		QueryAccumulateInfo(null);

		return true;
	}

	public override void OnHide()
	{
		ClearData();

		base.OnHide();
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		//被Overlay了强制刷新
		QueryAccumulateInfo(null);
	}

	private void ClearData()
	{
		StopCoroutine("FillData");
		activityList.ClearList(false);
		activityList.ScrollPosition = 0f;

		this.lastRefreshTime = 0;
		this.nextRefreshTime = 0;
	}

	private void QueryAccumulateInfo(System.Func<bool> del)
	{
		this.waitForResponse = true;
		RequestMgr.Inst.Request(new OperationActivityQueryReq(del));
	}

	private void InitView()
	{
		// Set Message Title.
		var config = ConfigDatabase.DefaultCfg.OperationConfig.getOperationInfoByIndex(activityId, ActivityData.AccumulateIndex);
		if (config != null)
			desc.Text = config.OperationDesc;

		// Set Recharge Money Label.
		moneyLabel.Text = GameUtility.FormatUIString("UIPnlRunAccumulativeTab_Money",
								ItemInfoUtility.GetItemCountStr(ActivityData.AccumulateMoney / (float)ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower, ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType),
								ItemInfoUtility.GetCurrencyNameByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType));

		ClearData();
		SetDynamicView();

		StartCoroutine("FillData");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		var operationCfg = ConfigDatabase.DefaultCfg.OperationConfig.getOperationInfoByIndex(activityId, ActivityData.AccumulateIndex);
		if (operationCfg == null)
		{
			Debug.Log(string.Format("OperationConfig Not found {0}", activityId.ToString("X")));
			yield return null;
		}

		operationCfg.OperationItems.Sort((t1, t2) =>
		{
			int money1 = t1.CompareValue;
			int money2 = t2.CompareValue;
			return money1 - money2;
		});

		for (int index = 0; index < operationCfg.OperationItems.Count; index++)
		{
			var operation = ActivityData.GetOperationItemById(operationCfg.OperationItems[index].ItemId);
			if (operation != null)
			{
				var container = activityPool.AllocateItem().GetComponent<UIListItemContainer>();
				var item = container.GetComponent<UIElemRunAccumlativeItem>();

				container.Data = item;
				item.SetData(operationCfg.OperationItems[index], operation.isEverPurchase, operation.couldPickCounts, ActivityData.AccumulateMoney);
				activityList.AddItem(item.gameObject);
			}
		}
	}

	public void OnQueryInfoSuccess(int activityId)
	{
		this.activityId = activityId;
		this.waitForResponse = false;
		InitView();

		lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		nextRefreshTime = ActivityData.NextRefreshTime(lastRefreshTime);
	}

	public void OnQueryInfoFailed()
	{
		waitForResponse = false;
		lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		nextRefreshTime = ActivityData.NextRefreshTime(lastRefreshTime);
	}

	public void OnGetRewardSuccess(com.kodgames.corgi.protocol.OperationActivityItem operationActivityItem, KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync)
	{
		// Refresh List item.
		var operationCfg = ConfigDatabase.DefaultCfg.OperationConfig.getOperationItemById(operationActivityItem.itemId);
		for (int index = 0; index < activityList.Count; index++)
		{
			UIElemRunAccumlativeItem item = activityList.GetItem(index).Data as UIElemRunAccumlativeItem;

			if ((int)item.btn.Data == operationCfg.ItemId)
			{
				item.SetData(operationCfg, operationActivityItem.isEverPurchase, operationActivityItem.couldPickCounts, ActivityData.AccumulateMoney);
				break;
			}
		}

		// Show Reward.
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgRunActivityRewards), costAndRewardSync.Reward);
	}

	public void ResetWaitForResValue()
	{
		waitForResponse = false;
		nextRefreshTime = ActivityData.NextRefreshTime(lastRefreshTime);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRecharge(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRecharge);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetReward(UIButton btn)
	{
		int operationId = (int)btn.Data;
		var operation = ActivityData.GetOperationItemById(operationId);

		if (operation.isEverPurchase == true && operation.couldPickCounts > 0)
			RequestMgr.Inst.Request(new OperationActivityPickRewardReq(activityId, operationId));
		else if (operation.isEverPurchase == true && operation.couldPickCounts == 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlRunAccumulativeTab_Tisp2"));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlRunAccumulativeTab_Tisp1"));
	}

	private void Update()
	{
		if (waitForResponse || ActivityData == null)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1.0f)
		{
			deltaTime = 0f;

			if (nextRefreshTime > 0 && SysLocalDataBase.Inst.LoginInfo.NowTime >= nextRefreshTime)
			{
				this.waitForResponse = true;

				QueryAccumulateInfo(() =>
				{
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlRunAccumulativeTab_Activity"));
					return true;
				});
			}

			SetDynamicView();
		}
	}

	private void SetDynamicView()
	{
		// Set Recharge Button.
		bool enableRechargeBtn = (ActivityData.RechargeStart <= SysLocalDataBase.Inst.LoginInfo.NowTime) && (ActivityData.RechargeEnd >= SysLocalDataBase.Inst.LoginInfo.NowTime);
		rechargeBtn.Hide(!enableRechargeBtn);

		// Set EndTime Label.
		UIUtility.UpdateUIText(activeLabel, GameUtility.FormatUIString("UIPnlRunAccumulativeTab_Time_01", GameUtility.Time2String((nextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime) < 0 ?
			0 : (nextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime))));
	}
}