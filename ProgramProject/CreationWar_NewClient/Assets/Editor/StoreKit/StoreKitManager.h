//
//  StoreKitManager.h
//  StoreKit
//
//  Created by Mike DeSaro on 8/18/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "StoreKitReceiptRequest.h"


@interface StoreKitManager : NSObject <SKProductsRequestDelegate, SKPaymentTransactionObserver, StoreKitReceiptRequestDelegate>
{

}

+ (StoreKitManager*)sharedManager;


- (BOOL)canMakePayments;

- (void)restoreCompletedTransactions;

- (void)requestProductData:(NSSet*)productIdentifiers;

- (void)purchaseProduct:(NSString*)productIdentifier quantity:(int)quantity;

- (void)validateReceipt:(NSString*)transactionReceipt isTestReceipt:(BOOL)isTest;

- (NSString*)getAllSavedTransactions;

@end
