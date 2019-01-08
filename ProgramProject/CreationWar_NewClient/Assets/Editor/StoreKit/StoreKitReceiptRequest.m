//
//  StoreKitReceiptRequest.m
//  PrimeInAppTest
//
//  Created by Mike DeSaro on 8/19/10.
//  Copyright 2010 Prime31 Studios. All rights reserved.
//

#import "StoreKitReceiptRequest.h"


@implementation StoreKitReceiptRequest

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

- (id)initWithDelegate:(id<StoreKitReceiptRequestDelegate>)delegate isTest:(BOOL)isTest
{
	if( ( self = [super init] ) )
	{
		_delegate = delegate;
		_isTest = isTest;
	}
	
	return self;
}


- (void)dealloc
{
	_delegate = nil;
	[_responseData release];
	
	[super dealloc];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)validateReceipt:(NSString*)base64EncodedTransactionReceipt
{
	// Construct the request
	NSString *urlString = ( _isTest ) ? @"https://sandbox.itunes.apple.com/verifyReceipt" : @"https://buy.itunes.apple.com/verifyReceipt";
	NSMutableURLRequest *request =	[NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString]];
	[request setHTTPMethod:@"POST"];
	[request setValue:@"application/x-www-form-urlencoded" forHTTPHeaderField:@"Content-Type"];
	
	// Construct the post data
	NSString *postData = [NSString stringWithFormat:@"{\"receipt-data\":\"%@\"}", base64EncodedTransactionReceipt];
	[request setHTTPBody:[postData dataUsingEncoding:NSUTF8StringEncoding]];
	[request setValue:[NSString stringWithFormat:@"%d", postData.length] forHTTPHeaderField:@"Content-Length"];
	
	// Initialize the responseData
	_responseData = [[NSMutableData alloc] init];
	
	// kick off the request
    [[[NSURLConnection alloc] initWithRequest:request delegate:self] autorelease];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSURLRequest

- (void)connection:(NSURLConnection*)connection didReceiveResponse:(NSURLResponse*)response
{
    [_responseData setLength:0];
}


- (void)connection:(NSURLConnection*)connection didReceiveData:(NSData*)data
{
    [_responseData appendData:data];
}


- (void)connection:(NSURLConnection*)connection didFailWithError:(NSError*)error
{
    [_delegate storeKitReceiptRequest:self didFailWithError:error];
}


// Once this method is invoked, "_responseData" contains the complete result
- (void)connectionDidFinishLoading:(NSURLConnection*)connection
{
	NSString *response = [NSString stringWithUTF8String:(const char*)_responseData.bytes];
	
	// send the raw response
	[_delegate storeKitReceiptRequest:self validatedWithResponse:response];
	
	NSLog( @"StoreKit: validate receipt response: %@", response );
	
	// See if it is valid
	NSScanner *scanner = [NSScanner scannerWithString:response];
	if( ![scanner scanUpToString:@"\"status\":" intoString:NULL] )
	{
		[_delegate storeKitReceiptRequest:self didFailWithError:[NSError errorWithDomain:@"Unknown" code:212 userInfo:[NSDictionary dictionaryWithObject:@"Could not parse status code" forKey:NSLocalizedDescriptionKey]]];
		return;
	}
	
	[scanner scanString:@"\"status\":" intoString:NULL];
	
	int status = -1;
	[scanner scanInt:&status];
	
	[_delegate storeKitReceiptRequest:self validatedWithStatusCode:status];
}

@end
