using System;


public class StoreKitProduct
{
    public string productIdentifier;
    public string title;
    public string description;
    public string price;
	public string currencySymbol;


    public static StoreKitProduct productFromString( string productString )
    {
        StoreKitProduct product = new StoreKitProduct();

        string[] productParts = productString.Split( new string[] { "|||" }, StringSplitOptions.None );
        if( productParts.Length == 5 )
        {
            product.productIdentifier = productParts[0];
            product.title = productParts[1];
            product.description = productParts[2];
            product.price = productParts[3];
			product.currencySymbol = productParts[4];
        }

        return product;
    }
	
	
	public override string ToString()
	{
		return String.Format( "<StoreKitProduct>\nID: {0}\nTitle: {1}\nDescription: {2}\nPrice: {3}\nCurrency Symbol: {4}",
			productIdentifier, title, description, price, currencySymbol );
	}
}
