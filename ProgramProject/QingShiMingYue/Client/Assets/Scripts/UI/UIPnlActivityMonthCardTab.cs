using System;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;
using KodGames;

public class UIPnlActivityMonthCardTab : UIModule
{
	public SpriteText titleLabel;
	public SpriteText explainLabel;
	public GameObjectPool monthCardItemPool;
	public UIScrollList cardScrollList;
	public bool shouldQueryWhenRemoveOverlay = false;

	public List<com.kodgames.corgi.protocol.OneMonthCardInfo> myMonthCardInfos
	{
		get { return ActivityManager.Instance.GetActivity<ActivityMonthCard>().MonthCardInfos; }
	}

	private bool waitForQueryList;
	private long serverLastResetTime = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlActivityTab>().SetActiveButton(_UIType.UIPnlActivityMonthCardTab);

		QueryMothCardInfo();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		cardScrollList.ClearList(false);
		cardScrollList.ScrollListTo(0);
		waitForQueryList = false;
	}

	private void SetData()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	public override void RemoveOverlay()
	{
		if (shouldQueryWhenRemoveOverlay)
		{
			shouldQueryWhenRemoveOverlay = false;
			QueryMothCardInfo();
		}

		base.RemoveOverlay();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		titleLabel.Text = GameUtility.GetUIString("UIPnlActivityMonthCardTab_Title");

		explainLabel.Text = ConfigDatabase.DefaultCfg.MonthCardConfig.explain;

		myMonthCardInfos.Sort((m, n) =>
		{
			return m.monthCardType - n.monthCardType;
		});

		for (int i = 0; i < myMonthCardInfos.Count; i++)
		{
			UIElemMonthCardItem monthCardItem = monthCardItemPool.AllocateItem().GetComponent<UIElemMonthCardItem>();
			monthCardItem.SetData(myMonthCardInfos[i]);
			cardScrollList.AddItem(monthCardItem.gameObject);
		}
	}

	void Update()
	{
		if (waitForQueryList)
			return;

		if (TimeEx.GetTimeAfterTime(
			TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.MonthCardConfig.resetTime),
			SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(serverLastResetTime),
			_TimeDurationType.Day) <= SysLocalDataBase.Inst.LoginInfo.NowDateTime)
		{
			QueryMothCardInfo();
		}
	}

	#region Response
	private void QueryMothCardInfo()
	{
		this.waitForQueryList = true;

		RequestMgr.Inst.Request(new QueryMonthCardInfoReq());
	}

	//query回调
	public void OnResponseQuerySuccess(long lastResetTime)
	{
		SetData();

		this.waitForQueryList = false;
		this.serverLastResetTime = lastResetTime;
	}

	//领取奖励回调
	public void OnResponseQuerySuccess()
	{

		//不清空列表，防止滚动列表重置，玩家再次领取时还要拖动列表
		foreach (var info in myMonthCardInfos)
		{
			for (int i = 0; i < cardScrollList.Count; i++)
			{
				var temp = cardScrollList.GetItem(i).gameObject.GetComponent<UIElemMonthCardItem>();
				if (temp.myMonthCardInfo.monthCardId == info.monthCardId)
					temp.SetData(info);
			}
		}

		this.waitForQueryList = false;
	}
	#endregion
}
