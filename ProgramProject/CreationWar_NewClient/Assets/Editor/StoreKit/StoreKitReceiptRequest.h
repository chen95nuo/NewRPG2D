//
//  StoreKitReceiptRequest.h
//  PrimeInAppTest
//
//  Created by Mike DeSaro on 8/19/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol StoreKitReceiptRequestDelegate;

@interface StoreKitReceiptRequest : NSObject
{
	id<StoreKitReceiptRequestDelegate> _delegate;
	BOOL _isTest;
	NSMutableData *_responseData;
}

- (id)initWithDelegate:(id<StoreKitReceiptRequestDelegate>)delegate isTest:(BOOL)isTest;

- (void)validateReceipt:(NSString*)base64EncodedTransactionReceipt;

@end



@protocol StoreKitReceiptRequestDelegate
- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request didFailWithError:(NSError*)error;
- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request validatedWithResponse:(NSString*)response;
- (void)storeKitReceiptRequest:(StoreKitReceiptRequest*)request validatedWithStatusCode:(int)statusCode;
@end
