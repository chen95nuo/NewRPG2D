//
//  StoreKitManager.m
//  StoreKit
//
//  Created by Mike DeSaro on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "StoreKitManager.h"
#import "SKPluginTransaction.h"
#import "NSDataBase64.h"


void UnitySendMessage( const char * className, const char * methodName, const char * param );


@implementation StoreKitManager

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (StoreKitManager*)sharedManager
{
	static StoreKitManager *sharedManager = nil;
	
	if( !sharedManager )
		sharedManager = [[StoreKitManager alloc] init];
	
	return sharedManager;
}


- (id)init
{
	if( ( self = [super init] ) )
	{
		// Listen to transaction changes
		[[SKPaymentQueue defaultQueue] addTransactionObserver:self];
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (void)completeAndRecordTransaction:(SKPaymentTransaction*)transaction
{
	NSLog( @"StoreKit: transaction completed: %@", transaction );
	
	// record the transaction after normalizing restored transactions
	if( transaction.originalTransaction )
		transaction = transaction.originalTransaction;
	
	// extract the transaction details
	NSString *productIdentifier = transaction.payment.productIdentifier;
	NSString *base64EncodedReceipt = [transaction.transactionReceipt base64Encoding];
	
	// create a pluginTransaction to save to disk
	SKPluginTransaction *pluginTransaction = [[SKPluginTransaction alloc] init];
	pluginTransaction.base64EncodedReceipt = base64EncodedReceipt;
	pluginTransaction.productIdentifier = productIdentifier;
	pluginTransaction.quantity = transaction.payment.quantity;
	[SKPluginTransaction saveTransaction:pluginTransaction];
	[pluginTransaction release];
	
	// complete the transaction
	[[SKPaymentQueue defaultQueue] finishTransaction:transaction];
	
	// notify Unity
	NSString *returnValue = [NSString stringWithFormat:@"%@|||%@|||%i", productIdentifier, base64EncodedReceipt, transaction.payment.quantity];
	UnitySendMessage( "StoreKitManager", "productPurchased", [returnValue UTF8String] );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)requestProductData:(NSSet*)productIdentifiers
{
	SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:productIdentifiers];
	request.delegate = self;
	[request start];
}


- (BOOL)canMakePayments
{
	return [SKPaymentQueue canMakePayments];
}


- (void)restoreCompletedTransactions
{
	[[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
}


- (void)purchaseProduct:(NSString*)productIdentifier quantity:(int)quantity
{
	SKMutablePayment *payment = [SKMutablePayment paymentWithProductIdentifier:productIdentifier];
	payment.quantity = quantity;
	[[SKPaymentQueue defaultQueue] addPayment:payment];
}


- (void)validateReceipt:(NSString*)transactionReceipt isTestReceipt:(BOOL)isTest
{
	// Create our request and send it off.  It will be released in its delegate callback
	StoreKitReceiptRequest *request = [[StoreKitReceiptRequest alloc] initWithDelegate:self isTest:isTest];
	[request validateReceipt:transactionReceipt];
}


- (NSString*)getAllSavedTransactions
{
	NSMutableArray *transactions = [SKPluginTransaction savedTransactions];
	if( !transactions.count )
		return @"";
	
	NSMutableString *transactionString = [NSMutableString string];
	for( SKPluginTransaction *trans in transactions )
	{
		// extract all the relevant data from the saved transactions
		[transactionString appendFormat:@"%@|||%@|||%i||||", trans.productIdentifier, trans.base64EncodedReceipt, trans.quantity];
	}
	
	// Remove the last 4 chars ONLY if we have enough characters!
	if( transactionString.length >= 4 )
		[transactionString deleteCharactersInRange:NSMakeRange( transactionString.length - 4, 4 )];
	
	return transactionString;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark SKProductsRequestDelegate

- (void)productsRequest:(SKProductsRequest*)request didReceiveResponse:(SKProductsResponse*)response
{
	NSMutableString *productString = [NSMutableString string];
	for( SKProduct *product in response.products )
	{
		[productString appendFormat:@"%@|||%@|||%@|||%@|||%@||||", product.productIdentifier, product.localizedTitle, product.localizedDescription, product.price, [product.priceLocale objectForKey:NSLocaleCurrencySymbol]];
	}
	
	// Remove the last 4 chars
	if( productString.length >= 4 )
		[productString deleteCharactersInRange:NSMakeRange( productString.length - 4, 4 )];
	
	for( NSString *invalidId in response.invalidProductIdentifiers )
		NSLog( @"StoreKit: invalid productIdentifier: %@", invalidId );
	
	[request autorelease];
	
	// Send the info back to Unity
	UnitySendMessage( "StoreKitManager", "productsReceived", [productString UTF8String] );
}


- (void)request:(SKRequest*)request didFailWithError:(NSError*)error
{
    UnitySendMessage( "StoreKitManager", "productsRequestDidFail", [[error localizedDescription] UTF8String] );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark SKPaymentTransactionObserver

- (void)paymentQueue:(SKPaymentQueue*)queue updatedTransactions:(NSArray*)transactions
{
	for( SKPaymentTransaction *transaction in transactions )
	{
		switch( transaction.transactionState )
		{
			case SKPaymentTransactionStatePurchasing:
			{
				NSLog( @"StoreKit: in the process of purchasing" );
				return;
				break;
			}
			case SKPaymentTransactionStateFailed:
			{
				if( transaction.error.code == SKErrorPaymentCancelled )
				{
					UnitySendMessage( "StoreKitManager", "productPurchaseCancelled", [[transaction.error localizedDescription] UTF8String] );
					NSLog( @"StoreKit: cancelled transaction: %@", [transaction.error localizedDescription] );
				}
				else
				{
					UnitySendMessage( "StoreKitManager", "productPurchaseFailed", [[transaction.error localizedDescription] UTF8String] );
					NSLog( @"StoreKit: error: %@", [transaction.error localizedDescription] );
				}
				// complete the transaction
				[[SKPaymentQueue defaultQueue] finishTransaction:transaction];
				break;
			}
			case SKPaymentTransactionStatePurchased:
			case SKPaymentTransactionStateRestored:
			{
				[self completeAndRecordTransaction:transaction];
				break;
			}
		} // end switch
	} // end for
}


- (void)failedTransaction:(SKPaymentTransaction*)transaction
{
	NSLog( @"StoreKit: ---------Doubt this will ever get called.  API is incorrect in docs.----------" );
}


// Sent when an error is encountered while adding transactions from the user's purchase history back to the queue.
- (void)paymentQueue:(SKPaymentQueue*)queue restoreCompletedTransactionsFailedWithError:(NSError*)error
{
	UnitySendMessage( "StoreKitManager", "restoreCompletedTransactionsFailed", [[error localizedDescription] UTF8String] );
	NSLog( @"restoreCompletedTransactionsFailedWithError: %@", [error localizedDescription] );
}


// Sent when all transactions from the user's purchase history have successfully been added back to the queue.
- (void)paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue*)queue
{
	UnitySendMessage( "StoreKitManager", "restoreCompletedTransactionsFinished", "" );
	NSLog( @"paymentQueueRestoreCompletedTransactionsFinished" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark StoreKitReceiptRequestDelegate

- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request didFailWithError:(NSError*)error
{
	[request release];
	
	UnitySendMessage( "StoreKitManager", "validateReceiptFailed", [[error localizedDescription] UTF8String] );
}


- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request validatedWithResponse:(NSString*)response
{
	UnitySendMessage( "StoreKitManager", "validateReceiptRawResponse", [response UTF8String] );
}


- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request validatedWithStatusCode:(int)statusCode
{
	[request release];

	UnitySendMessage( "StoreKitManager", "validateReceiptFinished", [[NSString stringWithFormat:@"%i", statusCode] UTF8String] );
}

@end
