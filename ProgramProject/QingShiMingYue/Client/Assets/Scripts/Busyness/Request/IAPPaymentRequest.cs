using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using System.Collections;

// Only for testing
class IAPPaymentTestRequest : Request
{
	private int goodsID;
	private int count;
	private string additionalData;

	public IAPPaymentTestRequest(int goodsID, int count, string additionalData)
	{
		this.goodsID = goodsID;
		this.count = count;
		this.additionalData = additionalData;
	}

	public override bool WaitingResponse
	{
		get { return false; }
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		// Use goods ID as callback data
		return bsn.ApplePurchaseTest(goodsID, count, ID, additionalData);
	}
}

class IAPPaymentRequest : Request
{
	private string productID;
	private int goodsID;
	private int count;
	private string additionalData;

	public override bool CheckTimeout { get { return false; } }

	public IAPPaymentRequest(string productID, int goodsID, int count, string additionalData)
	{
		this.productID = productID;
		this.goodsID = goodsID;
		this.count = count;
		this.additionalData = additionalData;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return GameMain.Inst.GetIAPListener().PayProduct(productID, goodsID, count, ID, additionalData);
	}
}

class IAPPaymentResponse : Response
{
	private int goodsID;
	private int count;
	private string meg;
	private List<int> payStatus;

	public IAPPaymentResponse(int pRqstID, int goodID, int goodCount, string meg, List<int> payStatus)
		: base(pRqstID)
	{
		Debug.Log(string.Format("Unity IAPPaymentResponse {0},{1},{2}", pRqstID, goodsID, count));
		this.goodsID = goodID;
		this.count = goodCount;
		this.meg = meg;
		this.payStatus = payStatus;
	}

	public IAPPaymentResponse(int pRqstID, string errMsg)
		: base(pRqstID, 0, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.AppleGoodIds = this.payStatus;
		// Add the goods to local data
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlRecharge)))
			SysUIEnv.Instance.GetUIModule<UIPnlRecharge>().OnRechargeSuccess(/*payStatus*/);
		if ((SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityMonthCardTab) && !SysUIEnv.Instance.GetUIModule(_UIType.UIPnlActivityMonthCardTab).IsOverlayed)
		|| SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMonthCardDetail))
			RequestMgr.Inst.Request(new QueryMonthCardInfoReq());

		// 为了苹果审核，暂时取消对话框
		//if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgRechargeFirst)))
		//{
		//    MainMenuItem gotoPackageMenu = new MainMenuItem();
		//    gotoPackageMenu.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Get");
		//    gotoPackageMenu.Callback = (data) =>
		//    {
		//        SysUIEnv.Instance.HideUIModule(typeof(UIDlgRechargeFirst));
		//        SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEmail), _EmailDisplayType.System, true);
		//        return true;
		//    };

		//    MainMenuItem cancelMenu = new MainMenuItem();
		//    cancelMenu.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");
		//    cancelMenu.Callback = (data) =>
		//    {
		//        SysUIEnv.Instance.HideUIModule(typeof(UIDlgRechargeFirst));
		//        return true;
		//    };

		//    UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		//    showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title"), GameUtility.GetUIString("UIDlgRecharge_Tip_Success"), cancelMenu, gotoPackageMenu);
		//    SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
		//}

		// TODO:临时代码，评审结束，删除代码
		#region  评审
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), meg);
		#endregion

		//if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityProcessRechargeTab))
		//    SysUIEnv.Instance.GetUIModule<UIPnlActivityProcessRechargeTab>().RequestInfo();
		//else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityProcessSpendTab))
		//    SysUIEnv.Instance.GetUIModule<UIPnlActivityProcessSpendTab>().RequestInfo();

		return true;
	}
}

