using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public enum Gender {
	UNKNOW = 0,
	MALE = 1,
	FEMALE = 2
}

public enum AccountType {
	ANONYMOUS = 0,
	REGISTERED = 1,
	SINA_WEIBO = 2,
	QQ = 3,
	QQ_WEIBO = 4,
	ND91 = 5,
	TYPE1 = 11,
	PP = 12,
	TYPE3 = 13,
	TYPE4 = 14,
	TYPE5 = 15,
	TYPE6 = 16,
	TYPE7 = 17,
	TYPE8 = 18,
	TYPE9 = 19,
	TYPE10 = 20 
}

public class TDGAAccount {
	
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void _tdgaSetAccount(string accountId);
	
	[DllImport ("__Internal")]   
    private static extern void _tdgaSetAccountName(string accountName);
    
	[DllImport ("__Internal")]   
    private static extern void _tdgaSetAccountType(int accountType);
    
	[DllImport ("__Internal")]   
    private static extern void _tdgaSetLevel(int level) ;
    
	[DllImport ("__Internal")]   
    private static extern void _tdgaSetGender(int gender);
    
	[DllImport ("__Internal")]   
    private static extern void _tdgaSetAge(int age);
    
	[DllImport ("__Internal")]   
    private static extern void _tdgaSetGameServer(string ameServer);
	
#elif UNITY_ANDROID
	//init static class --save memory/space
	
	//init static class --save memory/space
	private static AndroidJavaClass agent = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount");
	
	private AndroidJavaObject mAccount;
	
	public void setAccountObject(AndroidJavaObject account) {
		mAccount = account;
	}
#endif
	
	public static TDGAAccount SetAccount(string accountId) {
        return null;
        TDGAAccount account = new TDGAAccount();
		//if the platform is real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetAccount(accountId);
#elif UNITY_ANDROID
			AndroidJavaObject jobj = agent.CallStatic<AndroidJavaObject>("setAccount", accountId);
			account.setAccountObject(jobj);
#endif
		}
		
		return account;
	}
	
	public TDGAAccount() {
		
	}
	
	public void SetGameServer(string gameServer) {
        return;
        //if the platform is real device
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetGameServer(gameServer);
#elif UNITY_ANDROID
			if (mAccount != null) {
				mAccount.Call("setGameServer", gameServer);
			}
#endif
		}		
	}
	
	public void SetAccountName(string accountName) {
        return;
        //if the platform is real device
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetAccountName(accountName);
#elif UNITY_ANDROID
			if (mAccount != null) {
				mAccount.Call("setAccountName", accountName);
			}
#endif
		}		
	}
	
	public void SetLevel(int level) {
        return;
        //if the platform is real device
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetLevel(level);
#elif UNITY_ANDROID
			if (mAccount != null) {
				mAccount.Call("setLevel", level);
			}
#endif
		}		
	}
	
	public void SetAge(int age) {
        return;
        //if the platform is real device
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetAge(age);
#elif UNITY_ANDROID
			if (mAccount != null) {
				mAccount.Call("setAge", age);
			}
#endif
		}		
	}
	
	public void SetAccountType(AccountType type) {
        return;
        //if the platform is real device
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetAccountType((int)type);
#elif UNITY_ANDROID
			if (mAccount != null) {
				AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount$AccountType");
				AndroidJavaObject obj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
				mAccount.Call("setAccountType", obj);
			}
#endif
		}		
	}
	
	public void SetGender(Gender type) {
        return;
        //if the platform is real device
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetGender((int)type);
#elif UNITY_ANDROID
			if (mAccount != null) {
				AndroidJavaClass enumClass = new AndroidJavaClass("com.tendcloud.tenddata.TDGAAccount$Gender");
				AndroidJavaObject obj = enumClass.CallStatic<AndroidJavaObject>("valueOf", type.ToString());
				mAccount.Call("setGender", obj);
			}
#endif
		}		
	}
}