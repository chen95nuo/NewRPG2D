//
//  SKPluginTransaction.h
//  PrimeInAppTest
//
//  Created by Mike DeSaro on 8/23/10.
//  Copyright 2010 FreedomVOICE. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface SKPluginTransaction : NSObject <NSCoding>
{
	NSString *_base64EncodedReceipt;
	NSString *_productIdentifier;
	int _quantity;
}
@property (nonatomic, retain) NSString *base64EncodedReceipt;
@property (nonatomic, retain) NSString *productIdentifier;
@property (nonatomic) int quantity;


// retrieve and saved to disk transactions for archival
+ (NSString*)documentsPathForFile:(NSString*)filename;

+ (NSMutableArray*)savedTransactions;

+ (void)saveTransaction:(SKPluginTransaction*)transaction;


@end
