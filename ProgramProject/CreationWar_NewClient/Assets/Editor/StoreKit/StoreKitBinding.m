//
//  StoreKitBinding.m
//  StoreKit
//
//  Created by Mike DeSaro on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "StoreKitManager.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]



bool _canMakePayments()
{
	return ( [[StoreKitManager sharedManager] canMakePayments] == 1 );
}


// Accepts comma-delimited set of product identifiers
void _requestProductData( const char *productIdentifiers )
{
	// grab the product list and split it on commas
	NSString *identifiers = GetStringParam( productIdentifiers );
	NSArray *parts = [identifiers componentsSeparatedByString:@","];
	NSMutableSet *set = [NSMutableSet set];
	
	// add all the products to the set
	for( NSString *product in parts )
		[set addObject:[product stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]]];
	
	[[StoreKitManager sharedManager] requestProductData:set];
}


void _purchaseProduct( const char *product, int quantity )
{
	NSString *productId = GetStringParam( product );
	[[StoreKitManager sharedManager] purchaseProduct:productId quantity:quantity];
}


void _restoreCompletedTransactions()
{
	[[StoreKitManager sharedManager] restoreCompletedTransactions];
}


void _validateReceipt( const char *base64EncodedTransactionReceipt, bool isTest )
{
	NSString *receipt = GetStringParam( base64EncodedTransactionReceipt );
	[[StoreKitManager sharedManager] validateReceipt:receipt isTestReceipt:isTest];
}


const char * _getAllSavedTransactions()
{
	NSString *transactions = [[StoreKitManager sharedManager] getAllSavedTransactions];
	if( transactions )
		return MakeStringCopy( transactions );
	return MakeStringCopy( @"" );
}
