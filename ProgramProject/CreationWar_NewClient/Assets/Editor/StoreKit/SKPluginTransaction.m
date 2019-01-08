//
//  SKPluginTransaction.m
//  PrimeInAppTest
//
//  Created by Mike DeSaro on 8/23/10.
//  Copyright 2010 FreedomVOICE. All rights reserved.
//

#import "SKPluginTransaction.h"

#define kArchiveFile @"storeKitReceipts.archive"

@implementation SKPluginTransaction

@synthesize base64EncodedReceipt = _base64EncodedReceipt, productIdentifier = _productIdentifier, quantity = _quantity;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Class methods

+ (NSString*)documentsPathForFile:(NSString*)filename
{
    NSArray *thePathForSavedFile = NSSearchPathForDirectoriesInDomains( NSDocumentDirectory, NSUserDomainMask, YES );
    return [[thePathForSavedFile objectAtIndex:0] stringByAppendingPathComponent:filename];
}


+ (NSMutableArray*)savedTransactions
{
	NSMutableArray *transactions = [[NSKeyedUnarchiver unarchiveObjectWithFile:[SKPluginTransaction documentsPathForFile:kArchiveFile]] mutableCopy];
	if( !transactions )
		transactions = [NSMutableArray arrayWithCapacity:1];
	return transactions;
}


+ (void)saveTransaction:(SKPluginTransaction*)transaction
{
	// grab the transacitons from disk and add this one to it
	NSMutableArray *transactions = [SKPluginTransaction savedTransactions];
	[transactions addObject:transaction];
	
	// save to disk
	NSString *filePath = [self documentsPathForFile:kArchiveFile];
	[NSKeyedArchiver archiveRootObject:transactions toFile:filePath];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

- (void)dealloc
{
	[_base64EncodedReceipt release];
	[_productIdentifier release];
	
	[super dealloc];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSCoding

- (id)initWithCoder:(NSCoder*)coder
{
	if( ( self = [self init] ) )
	{
		_base64EncodedReceipt = [[coder decodeObjectForKey:@"base64EncodedReceipt"] retain];
		_productIdentifier = [[coder decodeObjectForKey:@"productIdentifier"] retain];
		_quantity = [coder decodeIntForKey:@"quantity"];
	}
	return self;
}


- (void)encodeWithCoder:(NSCoder*)coder
{
	[coder encodeObject:_base64EncodedReceipt forKey:@"base64EncodedReceipt"];
	[coder encodeObject:_productIdentifier forKey:@"productIdentifier"];
	[coder encodeInt:_quantity forKey:@"quantity"];
}

@end
