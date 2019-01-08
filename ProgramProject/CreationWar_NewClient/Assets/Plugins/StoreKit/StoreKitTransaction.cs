using System;


public class StoreKitTransaction
{
    public string productIdentifier;
    public string base64EncodedTransactionReceipt;
    public int quantity;


    public static StoreKitTransaction transactionFromString( string transactionString )
    {
        StoreKitTransaction transaction = new StoreKitTransaction();

        string[] transactionParts = transactionString.Split( new string[] { "|||" }, StringSplitOptions.None );
        if( transactionParts.Length == 3 )
        {
            transaction.productIdentifier = transactionParts[0];
            transaction.base64EncodedTransactionReceipt = transactionParts[1];
            transaction.quantity = int.Parse( transactionParts[2] );
        }

        return transaction;
    }
	
	
	public override string ToString()
	{
		return productIdentifier;
//		return string.Format( "<StoreKitTransaction>\nID: {0}\nReceipt: {1}\nQuantity: {2}", productIdentifier, base64EncodedTransactionReceipt, quantity );
	}
}
