using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StoreKitEventListener : MonoBehaviour
{
	void Start()
	{
		// Listens to all the StoreKit events.  All event listeners MUST be removed before this object is disposed!
		StoreKitManager.purchaseSuccessful += purchaseSuccessful;
		StoreKitManager.purchaseCancelled += purchaseCancelled;
		StoreKitManager.purchaseFailed += purchaseFailed;
		StoreKitManager.receiptValidationFailed += receiptValidationFailed;
		StoreKitManager.receiptValidationRawResponseReceived += receiptValidationRawResponseReceived;
		StoreKitManager.receiptValidationSuccessful += receiptValidationSuccessful;
		StoreKitManager.productListReceived += productListReceived;
		StoreKitManager.productListRequestFailed += productListRequestFailed;
		StoreKitManager.restoreTransactionsFailed += restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinished += restoreTransactionsFinished;
	}
	
	
	void OnDisable()
	{
		// Remove all the event handlers
		StoreKitManager.purchaseSuccessful -= purchaseSuccessful;
		StoreKitManager.purchaseCancelled -= purchaseCancelled;
		StoreKitManager.purchaseFailed -= purchaseFailed;
		StoreKitManager.receiptValidationFailed -= receiptValidationFailed;
		StoreKitManager.receiptValidationRawResponseReceived -= receiptValidationRawResponseReceived;
		StoreKitManager.receiptValidationSuccessful -= receiptValidationSuccessful;
		StoreKitManager.productListReceived -= productListReceived;
		StoreKitManager.productListRequestFailed -= productListRequestFailed;
		StoreKitManager.restoreTransactionsFailed -= restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinished -= restoreTransactionsFinished;
	}
	
	
	void productListReceived( List<StoreKitProduct> productList )
	{
		Debug.Log( "total productsReceived: " + productList.Count );
		
		// Do something more useful with the products than printing them to the console
		foreach( StoreKitProduct product in productList )
			Debug.Log( product.ToString() + "\n" );
	}
	
	
	void productListRequestFailed( string error )
	{
		Debug.Log( "productListRequestFailed: " + error );
	}
	
	
	void receiptValidationSuccessful()
	{
		Debug.Log( "receipt validation successful" );
	}
	
	
	void receiptValidationFailed( string error )
	{
		Debug.Log( "receipt validation failed with error: " + error );
	}
	
	
	void receiptValidationRawResponseReceived( string response )
	{
		Debug.Log( "receipt validation raw response: " + response );
	}
	

	void purchaseFailed( string error )
	{
		Debug.Log( "purchase failed with error: " + error );
	}
	

	void purchaseCancelled( string error )
	{
		Debug.Log( "purchase cancelled with error: " + error );
	}
	
//	public GameObject obj;
	void purchaseSuccessful( string productIdentifier, string receipt, int quantity )
	{
//		InRoom.GetInRoomInstantiate().GetBuyLanWei(receipt , "");
//		NGUIDebug.Log(productIdentifier + " ===== " + receipt + " ==== " + quantity);
//		obj.SendMessage("Jia" , productIdentifier , SendMessageOptions.DontRequireReceiver);
		Debug.Log( "purchased product: " + productIdentifier + ", quantity: " + quantity );
	}
	
	
	void restoreTransactionsFailed( string error )
	{
		Debug.Log( "restoreTransactionsFailed: " + error );
	}
	
	
	void restoreTransactionsFinished()
	{
		Debug.Log( "restoreTransactionsFinished" );
	}

}
