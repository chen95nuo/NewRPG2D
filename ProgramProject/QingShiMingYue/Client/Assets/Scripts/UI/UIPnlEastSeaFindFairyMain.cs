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

	//�������
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

	//�ܿ��عرշ��غ���
	public void OnQueryEastSeaZentiaError()
	{
		SetCloseActivity(true);
	}

	//����ѯ���غ���
	public void OnQueryEastSeaZentiaSuccess(List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges
		, KodGames.ClientClass.Cost refreshCost, string refreshDesc, string zentiaDesc, List<string> flowMessages)
	{
		ClearData();
		this.zentiaExchanges = zentiaExchanges;
		this.zentiaDesc = zentiaDesc;
		this.flowMessages = flowMessages;

		//Set UI
		//ʣ����Եֵ
		crrentEastSeaLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();
		//�����
		explainLabel.Text = refreshDesc;
		//ˢ������
		refreshNumLable.Text = refreshCost.Count.ToString();
		//ˢ������ͼ��
		refreshIcon.SetData(refreshCost.Id);

		//Set Data
		SetCloseActivity(false);
		SetListData();
		CancelInvoke("PlayFlowMessages");
		InvokeRepeating("PlayFlowMessages", 0, playSpeed);
	}

	//�ֶ�ˢ�£��Զ�ˢ�·��غ���
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

	//��ȡ��Ϣ���غ���
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

	//����Ե��ť�ص�
	public void OnEastSeaExchangeZentiaItemSuccess(List<Pair<int, int>> pairs)
	{
		//�������
		xunButton.SetControlState(UIButton.CONTROL_STATE.DISABLED);
		esffItem.zentiaExchange.IsAlreadyExchanged = true;
		esffItem.ClearData(true);
		esffItem.SetData(esffItem.zentiaExchange);

		crrentEastSeaLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();

		//��Ч��tips
		GameObject animationObject = Instantiate(UIFX_Q_XianYuanUp, animationInitPoint, Quaternion.identity) as GameObject;
		foreach (Pair<int, int> pair in pairs)
			if (IDSeg._SpecialId.Zentia == pair.first)
				animationObject.GetComponentInChildren<SpriteText>().Text = string.Format("+{0}", pair.second);
		animationObject.transform.parent = transform;

		DestroyObject(animationObject, 2f);
		preventClick.SetActive(true);
		Invoke("PlayRewardTips", 2f);
	}

	//��ʱ����tips ����Ե���Ŷ�����
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void PlayRewardTips()
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(esffItem.zentiaExchange.Reward, true, false));
		preventClick.SetActive(false);
		isOpenMessageWindow = false;
		RequestMgr.Inst.Request(new EastSeaQueryZentiaFlowMessageReq());
	}

	//������Ϣ
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

	//����ϢList���ϲ���message
	private void InsertListItem(int index)
	{
		var container = pmdPool.AllocateItem().GetComponent<UIListItemContainer>();
		var item = container.GetComponent<UIElemEastSeaPMDItem>();
		item.SetData(flowMessages[index]);
		pmdList.InsertItem(container, pmdList.Count, true, string.Empty, false);
	}

	//��������message����
	private void RestartPlayMessage()
	{
		CancelInvoke("PlayFlowMessages");
		InvokeRepeating("PlayFlowMessages", 0, 2f);
	}

	//ȡ����������message������ͬʱ�����������
	private void RestartRefreshData()
	{
		CancelInvoke("PlayFlowMessages");
		ClearData();
	}

	//�����������
	private void ClearData()
	{
		if (flowMessages != null)
			flowMessages.Clear();

		foreach (var reward in eastSeaRewardList)
			reward.ClearData(true);
	}

	//��ܿ����Ƿ�ر�
	private void SetCloseActivity(bool isClose)
	{
		successObject.SetActive(!isClose);
		errorObject.SetActive(isClose);
	}

	//˵������¼�
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetailedBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIDlgEastSeaExplain>(zentiaDesc);
	}

	//��Ե�һ���ť
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEastSeaExchangeBtn(UIButton btn)
	{
		RestartRefreshData();

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEastSeaElementItem));

		OnHide();
	}

	//ˢ�°�ť
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefreshBtn(UIButton btn)
	{
		animationInitPoint = btn.transform.position;
		ClearData();
		RequestMgr.Inst.Request(new EastSeaRefreshZentiaReq());

	}

	//�������ϸ��ť
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetailedEntertainingDiversionsBtn(UIButton btn)
	{
		isOpenMessageWindow = true;
		RequestMgr.Inst.Request(new EastSeaQueryZentiaFlowMessageReq());
	}

	//Ѱ��Ե��ť
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


