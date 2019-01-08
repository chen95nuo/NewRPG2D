using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class StoreKitBinding
{
    [DllImport("__Internal")]
    private static extern bool _canMakePayments();
 
    public static bool canMakePayments()
    {
        // Call plugin only when running on real device
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			return _canMakePayments();
		return false;
    }


    [DllImport("__Internal")]
    private static extern void _requestProductData( string productIdentifier );
 
	// Accepts comma-delimited set of product identifiers
    public static void requestProductData( string productIdentifier )
    {
        // Call plugin only when running on real device
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_requestProductData( productIdentifier );
    }


    [DllImport("__Internal")]
    private static extern void _purchaseProduct( string productIdentifier, int quantity );
 
    public static void purchaseProduct( string productIdentifier, int quantity )
    {
        // Call plugin only when running on real device
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_purchaseProduct( productIdentifier, quantity );
    }


    [DllImport("__Internal")]
    private static extern void _restoreCompletedTransactions();
 
    public static void restoreCompletedTransactions()
    {
        // Call plugin only when running on real device
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_restoreCompletedTransactions();
    }


    [DllImport("__Internal")]
    private static extern void _validateReceipt( string base64EncodedTransactionReceipt, bool isTest );
 
    public static void validateReceipt( string base64EncodedTransactionReceipt, bool isTest )
    {
        // Call plugin only when running on real device
        if( Application.platform == RuntimePlatform.IPhonePlayer )
			_validateReceipt( base64EncodedTransactionReceipt, isTest );
    }
	
	
    [DllImport("__Internal")]
    private static extern string _getAllSavedTransactions();
 
	// Returns a list of all the transactions that occured on this device.  They are stored in the Document directory.
    public static List<StoreKitTransaction> getAllSavedTransactions()
    {
		List<StoreKitTransaction> transactionList = new List<StoreKitTransaction>();
		
        // Call plugin only when running on real device
        if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			// Grab the transactions and parse them out
			string allTransactions = _getAllSavedTransactions();
	
			// parse out the products
	        string[] transactionParts = allTransactions.Split( new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries );
	        for( int i = 0; i < transactionParts.Length; i++ )
	            transactionList.Add( StoreKitTransaction.transactionFromString( transactionParts[i] ) );
		}
		
		return transactionList;
    }
	
}