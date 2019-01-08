using UnityEngine;
using System;
using System.Collections.Generic;


public class StoreKitManager : MonoBehaviour
{
	// Events and delegates
	public delegate void ProductPurchasedEventHandler( string productIdentifier, string receipt, int quantity );	
	public delegate void ProductListReceivedEventHandler( List<StoreKitProduct> productList );
	public delegate void StoreKitErrorEventHandler( string error );
	public delegate void StoreKitEmptyEventHandler();
	public delegate void StoreKitStringEventHandler( string response );
	public delegate void ValidateReceiptSuccessfulEventHandler();
	
	// Fired when a product is successfully paid for.  returnValue will hold the productIdentifer and receipt of the purchased product.
	public static event ProductPurchasedEventHandler purchaseSuccessful;
	
	// Fired when the product list your required returns.  Automatically serializes the productString into StoreKitProduct's.
	public static event ProductListReceivedEventHandler productListReceived;
	
	// Fired when requesting product data fails
	public static event StoreKitErrorEventHandler productListRequestFailed;
	
	// Fired when a product purchase fails
	public static event StoreKitErrorEventHandler purchaseFailed;
	
	// Fired when a product purchase is cancelled by the user or system
	public static event StoreKitErrorEventHandler purchaseCancelled;
	
	// Fired when the validateReceipt call fails
	public static event StoreKitErrorEventHandler receiptValidationFailed;
	
	// Fired when receive validation completes and returns the raw receipt data
	public static event StoreKitStringEventHandler receiptValidationRawResponseReceived;
	
	// Fired when the validateReceipt method finishes.  It does not automatically mean success.
	public static event ValidateReceiptSuccessfulEventHandler receiptValidationSuccessful;
	
	// Fired when an error is encountered while adding transactions from the user's purchase history back to the queue
	public static event StoreKitErrorEventHandler restoreTransactionsFailed;
	
	// Fired when all transactions from the user's purchase history have successfully been added back to the queue
	public static event StoreKitEmptyEventHandler restoreTransactionsFinished;
	
	
    void Awake()
    {
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
    }
	
	public int repeatTimes = 3;
	public void productPurchased( string returnValue )
	{
		// split up into useful data
		string[] receiptParts = returnValue.Split( new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries );
		if( receiptParts.Length != 3 )
		{
			if( purchaseFailed != null )
				purchaseFailed( "Could not parse receipt information: " + returnValue );
			return;
		}


		string productIdentifier = receiptParts[0];
		string receipt = receiptParts[1];
		int quantity = int.Parse( receiptParts[2] );

		PlayerPrefs.SetString("productPurchased" , PlayerPrefs.GetString("productPurchased" , "") + ";," + receipt);
		InRoom.GetInRoomInstantiate().VerifyToServer(PlayerPrefs.GetString("NumTitleS1" , "Empty") + "," + receipt + "," + PlayerPrefs.GetString("GameUserID", ""));
//		Send(productIdentifier, receipt);
		repeatTimes = 3;
		if( purchaseSuccessful != null )
			purchaseSuccessful( productIdentifier, receipt, quantity );
	}

	private float pTime = 0f;
	void Update(){
		if( Time.time > pTime && repeatTimes > 0){
			pTime = Time.time + CommonDefine.Pay_Interval_Time;
			RequestPay();
		}
//		if( Input.GetKeyDown(KeyCode.J) ){
//			PlayerPrefs.SetString("productPurchased" , PlayerPrefs.GetString("productPurchased" , "") + ";,laskjdlakjsdlkajsdlkjlsakjd");
//		}
//		if( Input.GetKeyDown(KeyCode.M) ){
//			RemoveOnePay(",laskjdlakjsdlkajsdlkjlsakjd");
//		}
	}

	public void RequestPay()
	{
		string preservationOrder = "";
		preservationOrder = PlayerPrefs.GetString("productPurchased" , "");
		string[] parseParts = preservationOrder.Split( new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries );
		if( parseParts.Length > 0)
		{
			repeatTimes -= 1;
			for(int i=0; i<parseParts.Length; i++){
				string[] sendParts = parseParts[i].Split( new string[] { "," }, StringSplitOptions.RemoveEmptyEntries );
				if( sendParts.Length > 0){
					InRoom.GetInRoomInstantiate().VerifyToServer(PlayerPrefs.GetString("NumTitleS1" , "Empty") + "," + sendParts[0] + "," + PlayerPrefs.GetString("GameUserID", ""));
//					Send(sendParts[1] , sendParts[2]);
				}
			}
		}else{
			repeatTimes = 0;
		}
	}

	public static void RemoveOnePay(string receipt)
	{
		string strs = "";
		string strs1 = "";
		strs = PlayerPrefs.GetString("productPurchased" , "");
		strs1 = ";" + receipt;
		PlayerPrefs.SetString("productPurchased" , strs.Replace(strs1 , ""));
	}

	public void productPurchaseFailed( string error )
	{
		if( purchaseFailed != null )
			purchaseFailed( error );
	}
	
		
	public void productPurchaseCancelled( string error )
	{
		if( purchaseCancelled != null )
			purchaseCancelled( error );
	}
	
	
	public void productsReceived( string productString )
	{
        List<StoreKitProduct> productList = new List<StoreKitProduct>();

		// parse out the products
        string[] productParts = productString.Split( new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries );
        for( int i = 0; i < productParts.Length; i++ )
            productList.Add( StoreKitProduct.productFromString( productParts[i] ) );
		
		if( productListReceived != null )
			productListReceived( productList );
	}
	
	
	public void productsRequestDidFail( string error )
	{
		if( productListRequestFailed != null )
			productListRequestFailed( error );
	}
	
	
	public void validateReceiptFailed( string error )
	{
		if( receiptValidationFailed != null )
			receiptValidationFailed( error );
	}
	
	
	public void validateReceiptRawResponse( string response )
	{
		if( receiptValidationRawResponseReceived != null )
			receiptValidationRawResponseReceived( response );
	}
	
	
	public void validateReceiptFinished( string statusCode )
	{
		if( statusCode == "0" )
		{
			if( receiptValidationSuccessful != null )
				receiptValidationSuccessful();
		}
		else
		{
			if( receiptValidationFailed != null )
				receiptValidationFailed( "Receipt validation failed with statusCode: " + statusCode );
		}
	}
	
	
	public void restoreCompletedTransactionsFailed( string error )
	{
		if( restoreTransactionsFailed != null )
			restoreTransactionsFailed( error );
	}
	
	
	public void restoreCompletedTransactionsFinished( string empty )
	{
		if( restoreTransactionsFinished != null )
			restoreTransactionsFinished();
	}

}

