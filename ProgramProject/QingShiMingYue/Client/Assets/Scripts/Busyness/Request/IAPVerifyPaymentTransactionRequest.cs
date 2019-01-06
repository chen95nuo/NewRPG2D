using System;
using System.Collections.Generic;

class IAPVerifyPaymentTransactionRequest : Request
{
	public IAPVerifyPaymentTransactionRequest(string transactionID, byte[] receipt)
	{
		Debug.Log(string.Format("Unity IAPVerifyPaymentTransactionRequest {0}", transactionID));

		this.receipt = receipt;
	}

	public override bool WaitingResponse
	{
		get { return false; }
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ApplePurchase(receipt, ID);
	}

	private byte[] receipt;
}

class IAPVerifyPaymentTransactionResponse : Response
{
	private string transactionID;
	private bool alreadyProcessed;
	private int goodsID;
	private int goodsCount;
	private string meg;

	public IAPVerifyPaymentTransactionResponse(int pRqstID, string transactionID, int goodsID, int goodsCount, string meg)
		: base(pRqstID)
	{
		this.transactionID = transactionID;
		this.goodsID = goodsID;
		this.goodsCount = goodsCount;
		this.meg = meg;
	}

	public IAPVerifyPaymentTransactionResponse(int pRqstID, string transactionID, bool alreadyProcessed, string errMsg)
		: base(pRqstID, 0, errMsg)
	{
		Debug.Log(errMsg);
		this.transactionID = transactionID;
		this.alreadyProcessed = alreadyProcessed;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		GameMain.Inst.GetIAPListener().OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.Successed, transactionID, goodsID, goodsCount,meg,null);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (alreadyProcessed)
		{
			GameMain.Inst.GetIAPListener().OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.Successed_AlreadyProcessed, transactionID, goodsID, goodsCount,errMsg,null);
		}
		else
		{
			base.PrcErr(request,errCode, errMsg);
			GameMain.Inst.GetIAPListener().OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.VerifyPayment_Failed, transactionID, 0, 0,errMsg,null);
		}
	}
}