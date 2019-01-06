using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class UIPnlEastSeaFindFairyMain : UIPnlEastSeaFindFairyTimer
{
	public SpriteText explainLabel;
	public SpriteText crrentEastSeaLable;
	public SpriteText refreshNumLable;
	public UIElemAssetIcon refreshIcon;
	public List<UIElemEastSeaFindFairyItem> eastSeaRewardList;
	public UIScrollList pmdList;
	public GameObjectPool pmdPool;
	public GameObject successObject;
	public GameObject errorObject;
	public GameObject preventClick;
	public GameObject UIFX_Q_XianYuanUp;
	public UIBox notifIconBox;

	public float flowSpeed = 1f;
	public EZAnimation.EASING_TYPE easingType = EZAnimation.EASING_TYPE.Default;

	private float playSpeed = 6f;
	private string zentiaDesc;
	private int index = 1;
	private const int MAX_MESSAGE_ITEM_COUNT = 3;
	private const int MAX_MESSAGE_COUNT = 20;
	private List<string> flowMessages;
	private List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges;
	private Vector3 animationInitPoint = Vector3.zero;
	private UIElemEastSeaFindFairyItem esffItem;
	private UIButton xunButton;
	private bool isOpenMessageWindow = false;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		successObject.SetActive(false);
		errorObject.SetActive(false);
		preventClick.SetActive(false);

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void UpdataActivityIconNotify()
	{
		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.FunctionStates.Count; i++)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.FunctionStates[i].id == GreenPointType.ZentiaServerReward)
			{
				notifIconBox.Hide(!SysLocalDataBase.Inst.LocalPlayer.FunctionStates[i].isOpen);
				break;
			}
		}
	}

	public override void Update()
	{
		base.Update();

		UpdataActivityIconNotify();
	}

	//填充数据
	private void SetListData()
	{
		for (int i = 0; i < eastSeaRewardList.Count; i++)
		{
			if (i >= zentiaExchanges.Count && i < eastSeaRewardList.Count)
				eastSeaRewardList[i].gameObject.SetActive(false);
			else
			{
				eastSeaRewardList[i].gameObject.SetActive(true);
				eastSeaRewardList[i].SetData(zentiaExchanges[i]);
			}
		}
	}

	//总开关关闭返回函数
	public void OnQueryEastSeaZentiaError()
	{
		SetCloseActivity(true);
	}

	//主查询返回函数
	public void OnQueryEastSeaZentiaSuccess(List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges
		, KodGames.ClientClass.Cost refreshCost, string refreshDesc, string zentiaDesc, List<string> flowMessages)
	{
		ClearData();
		this.zentiaExchanges = zentiaExchanges;
		this.zentiaDesc = zentiaDesc;
		this.flowMessages = flowMessages;

		//Set UI
		//剩余仙缘值
		crrentEastSeaLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();
		//活动描述
		explainLabel.Text = refreshDesc;
		//刷新消耗
		refreshNumLable.Text = refreshCost.Count.ToString();
		//刷新消耗图标
		refreshIcon.SetData(refreshCost.Id);

		//Set Data
		SetCloseActivity(false);
		SetListData();
		CancelInvoke("PlayFlowMessages");
		InvokeRepeating("PlayFlowMessages", 0, playSpeed);
	}

	//手动刷新，自动刷新返回函数
	public void OnQueryEastSeaZentiaSuccess(List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges, int count)
	{
		ClearData();
		this.zentiaExchanges = zentiaExchanges;
		crrentEastSeaLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();
		SetListData();

		GameObject animationObject = Instantiate(UIFX_Q_XianYuanUp, animationInitPoint, Quaternion.identity) as GameObject;
		animationObject.GetComponentInChildren<SpriteText>().Text = string.Format("+{0}", count);
		DestroyObject(animationObject, 2f);
		animationObject.transform.parent = transform;

		isOpenMessageWindow = false;
		RequestMgr.Inst.Request(new EastSeaQueryZentiaFlowMessageReq());
	}

	//获取消息返回函数
	public void OnEastSeaQueryZentiaFlowMessageSuccess(List<string> flowMessages)
	{
		if (this.flowMessages != null)
			this.flowMessages.Clear();
		if (flowMessages != null && flowMessages.Count > MAX_MESSAGE_COUNT)
			this.flowMessages = flowMessages.GetRange(0, MAX_MESSAGE_COUNT);
		else
			this.flowMessages = flowMessages;
		if (isOpenMessageWindow)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgEastSeaMessages), flowMessages);
	}

	//得仙缘按钮回调
	public void OnEastSeaExchangeZentiaItemSuccess(List<Pair<int, int>> pairs)
	{
		//处理更新
		xunButton.SetControlState(UIButton.CONTROL_STATE.DISABLED);
		esffItem.zentiaExchange.IsAlreadyExchanged = true;
		esffItem.ClearData(true);
		esffItem.SetData(esffItem.zentiaExchange);

		crrentEastSeaLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();

		//特效及tips
		GameObject animationObject = Instantiate(UIFX_Q_XianYuanUp, animationInitPoint, Quaternion.identity) as GameObject;
		foreach (Pair<int, int> pair in pairs)
			if (IDSeg._SpecialId.Zentia == pair.first)
				animationObject.GetComponentInChildren<SpriteText>().Text = string.Format("+{0}", pair.second);
		animationObject.transform.parent = transform;

		DestroyObject(animationObject, 2f);
		preventClick.SetActive(true);
		Invoke("PlayRewardTips", 2f);
	}

	//延时播放tips 得仙缘播放动画，
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void PlayRewardTips()
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(esffItem.zentiaExchange.Reward, true, false));
		preventClick.SetActive(false);
		isOpenMessageWindow = false;
		RequestMgr.Inst.Request(new EastSeaQueryZentiaFlowMessageReq());
	}

	//播放消息
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void PlayFlowMessages()
	{
		if (flowMessages != null && flowMessages.Count > 0)
		{
			if (pmdList.Count > MAX_MESSAGE_ITEM_COUNT)
				pmdList.RemoveItem(0, false, true, false);
			index = index + 1 >= flowMessages.Count ? 0 : index + 1;
			InsertListItem(index);
			pmdList.ScrollToItem(pmdList.Count - 1, flowSpeed, easingType);
		}
	}

	//给消息List集合插入message
	private void InsertListItem(int index)
	{
		var container = pmdPool.AllocateItem().GetComponent<UIListItemContainer>();
		var item = container.GetComponent<UIElemEastSeaPMDItem>();
		item.SetData(flowMessages[index]);
		pmdList.InsertItem(container, pmdList.Count, true, string.Empty, false);
	}

	//重启播放message滚动
	private void RestartPlayMessage()
	{
		CancelInvoke("PlayFlowMessages");
		InvokeRepeating("PlayFlowMessages", 0, 2f);
	}

	//取消播放所有message滚动，同时清除所有数据
	private void RestartRefreshData()
	{
		CancelInvoke("PlayFlowMessages");
		ClearData();
	}

	//清除所有数据
	private void ClearData()
	{
		if (flowMessages != null)
			flowMessages.Clear();

		foreach (var reward in eastSeaRewardList)
			reward.ClearData(true);
	}

	//活动总开关是否关闭
	private void SetCloseActivity(bool isClose)
	{
		successObject.SetActive(!isClose);
		errorObject.SetActive(isClose);
	}

	//说明点击事件
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetailedBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIDlgEastSeaExplain>(zentiaDesc);
	}

	//仙缘兑换按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEastSeaExchangeBtn(UIButton btn)
	{
		RestartRefreshData();

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEastSeaElementItem));

		OnHide();
	}

	//刷新按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefreshBtn(UIButton btn)
	{
		animationInitPoint = btn.transform.position;
		ClearData();
		RequestMgr.Inst.Request(new EastSeaRefreshZentiaReq());

	}

	//跑马灯详细按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetailedEntertainingDiversionsBtn(UIButton btn)
	{
		isOpenMessageWindow = true;
		RequestMgr.Inst.Request(new EastSeaQueryZentiaFlowMessageReq());
	}

	//寻仙缘按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFindEastSeaBtn(UIButton btn)
	{
		xunButton = btn;
		animationInitPoint = btn.transform.position;
		esffItem = btn.Data as UIElemEastSeaFindFairyItem;
		esffItem.OnClickFindEastSeaBtn();

		if (esffItem.costs != null && esffItem.costs.Count >= esffItem.GetIconCount())
			RequestMgr.Inst.Request(new EastSeaExchangeZentiaItemReq(esffItem.zentiaExchange.ExchangeId, esffItem.zentiaExchange.Index, esffItem.costs));
		else
		{
			SysUIEnv.Instance.ShowUIModule<UIPnlTipFlow>(GameUtility.GetUIString("UIPnlEastSeaFindFairyMain_PleaseContext"));
		}
	}

}


