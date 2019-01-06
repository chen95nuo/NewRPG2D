using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using KodGames.ExternalCall;

public class IOS_IAPListener : IAPListener
{
	private void Awake()
	{
		// Initialize IAP system
		IAPPlugin.InitializeIAP();
	}

	/// <summary>
	/// Call by unity script to check payment
	/// </summary>
	public override bool CanMakePayment()
	{
		return IAPPlugin.CanMakePayment();
	}

	/// <summary>
	/// Call by unity script to make product payment
	/// </summary>
	public override bool PayProduct(string productID, int goodsID, int count, int callback, string additionalData)
	{
		Debug.Log(string.Format("PayProduct {0},{1},{2},{3}", productID, goodsID, count, callback));
		if (WaitingforPreviousPayment())
			return false;

		// Call plugin method
		bool tf = IAPPlugin.PayProduct(productID, count);

		// If success save callback for response
		if (tf)
			payProductCallback = callback;

		return tf;
	}

	/// <summary>
	/// Called by IOS when product information received
	/// </summary>
	private void OnRequestProductResponse(string message)
	{

	}

	/// <summary>
	/// Called by IOS when
	/// 1. After initialized, process unverifyed transaction.
	/// 2. After payment response receivec.
	/// </summary>
	private void OnPaymentResponse(string message)
	{
		Debug.Log(string.Format("Unity OnPaymentResponse {0}", message));

		bool success;
		string productID;
		int productQuantity;
		string transactionID;

		if (IAPHelper.ParsePaymentResponse(message, out success, out productID, out productQuantity, out transactionID))
		{
			if (success)
			{
				// Get transaction receipt
				byte[] receiptBuffer = IAPPlugin.FetchPaymentTransactionReciept(transactionID);

				// Send verify request, when response received. Send payment response
				RequestMgr.Inst.Request(new IAPVerifyPaymentTransactionRequest(transactionID, receiptBuffer));
				return;
			}
			else
			{
				OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.Payment_Failed, "", 0, 0,"",null);
				return;
			}
		}

		Debug.LogError("Parse parameter failed");
		// Send response to release screen lock.
		OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.Payment_Failed, "", 0, 0,"",null);
		return;
	}

	// Call when
	public override void OnVerifyPaymentTransaction(VerifyPaymentResult result, string transactionID, int goodsID, int count, string errMeg, List<int> payStatus)
	{
		Debug.Log(string.Format("Unity OnPaymentResponse {0},{1},{2},{3}", result, transactionID, goodsID, count));

		switch (result)
		{
			case VerifyPaymentResult.Successed:
				// Verify successed remove the payment from payment queue.
				IAPPlugin.FinishTransition(transactionID);
				RequestMgr.Inst.Response(new IAPPaymentResponse(payProductCallback, goodsID, count,errMeg,payStatus));
				payProductCallback = Request.InvalidID;
				break;

			case VerifyPaymentResult.Successed_AlreadyProcessed:
				// This transaction has been processed, remove the payment from payment queue.
				IAPPlugin.FinishTransition(transactionID);
				break;

			case VerifyPaymentResult.Payment_Failed:
			case VerifyPaymentResult.VerifyPayment_Failed:
				RequestMgr.Inst.Response(new IAPPaymentResponse(payProductCallback, ""));
				payProductCallback = Request.InvalidID;
				break;
		}
	}
}
