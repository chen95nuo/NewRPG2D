using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Test_IAPListener : IAPListener 
{
	public override bool PayProduct(string productID, int goodsID, int count, int callback, string additionalData)
	{ 
		// Call notice server
		bool tf = RequestMgr.Inst.Request(new IAPPaymentTestRequest(goodsID, count, additionalData));
		
		// If success save callback for response
		if (tf)
			payProductCallback = callback;

		return tf;
	}
}
