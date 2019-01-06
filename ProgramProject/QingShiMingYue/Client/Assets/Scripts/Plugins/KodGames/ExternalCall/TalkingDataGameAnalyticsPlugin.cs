using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
namespace KodGames.ExternalCall
{
	public class TalkingDataGameAnalyticsPlugin
	{
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void UnityCall_TDGA_SetAccount(string accountId,int accountType,string accountName,int level,string server);
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_TDGA_OnChargeRequst(string orderId,string iapId,double currencyAmount,string currencyType,double virtualCurrencyAmount,string paymentType);
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_TDGA_OnChargeSuccess(string orderId);
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_TDGA_OnReward(double virtualCurrencyAmount, string reason);
		
		[DllImport ("__Internal")]
    	private static extern void UnityCall_TDGA_OnPurchase(string item,int number,double price);
		
		[DllImport ("__Internal")]
    	private static extern void UnityCall_TDGA_OnUse(string item,int number);
    
    	[DllImport ("__Internal")]
    	private static extern void UnityCall_TDGA_OnBegin(string missionId);
		
		[DllImport ("__Internal")]
    	private static extern void UnityCall_TDGA_OnCompleted(string missionId);
		
		[DllImport ("__Internal")]
    	private static extern void UnityCall_TDGA_OnFailed(string missionId,string cause);

		[DllImport ("__Internal")]
		private static extern void  UnityCall_TDGA_SetEvent(string eventId, String[] key, String[] value, int size);

#elif UNITY_ANDROID
		private static AndroidJavaClass GetJavaClass()
		{
			return new AndroidJavaClass("com.KodGames.TalkingGame.TalkingGameSDK");
		}
#endif

		public static void SetAccount(string accountId, int accountType, string accountName, int level, string server)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_SetAccount(accountId,accountType,accountName,level,server);
#elif UNITY_ANDROID && !UNITY_EDITOR
			GetJavaClass().CallStatic("setAccount",new object[]{ accountId, accountName, level, server});
#endif
		}

		public static void OnReward(double virtualCurrencyAmount, string reason)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_OnReward(virtualCurrencyAmount,reason);
#elif UNITY_ANDROID && !UNITY_EDITOR
			GetJavaClass().CallStatic("onReward",new object[]{ virtualCurrencyAmount, reason});
#endif
		}

		public static void OnPurchase(string item, int number, double price)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_OnPurchase(item,number,price);
#elif UNITY_ANDROID && !UNITY_EDITOR
			GetJavaClass().CallStatic("onPurchase",new object[]{ item, number,price});
#endif
		}

		public static void OnUse(string item, int number)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_OnUse(item,number);
#endif
		}

		public static void OnBegin(string missionId)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_OnBegin(missionId);
#elif UNITY_ANDROID && !UNITY_EDITOR
			GetJavaClass().CallStatic("onBegin",new object[]{ missionId});
#endif
		}

		public static void OnCompleted(string missionId)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_OnCompleted(missionId);
#elif UNITY_ANDROID && !UNITY_EDITOR
			GetJavaClass().CallStatic("onCompleted",new object[]{ missionId});
#endif
		}

		public static void OnFailed(string missionId, string cause)
		{
#if UNITY_IPHONE		
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_TDGA_OnFailed(missionId,cause);
#endif
		}

		public static void OnEvent(string eventId, Dictionary<string, object> parameters)
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				var keys = new List<string>();
				var values = new List<string>();
				foreach (var kvp in parameters)
				{
					keys.Add(kvp.Key);
					values.Add(kvp.Value.ToString());
				}
				
				UnityCall_TDGA_SetEvent(eventId, keys.ToArray(), values.ToArray(), parameters.Count);
			}

#elif UNITY_ANDROID && !UNITY_EDITOR
			if (Application.platform == RuntimePlatform.Android)
			{
				int count = parameters.Count;
				AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap", count);
				
				IntPtr method_Put = AndroidJNIHelper.GetMethodID(map.GetRawClass(), 
				                                                 "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				
				object[] args = new object[2];
				foreach (KeyValuePair<string, object> kvp in parameters) {
					args[0] = new AndroidJavaObject("java.lang.String", kvp.Key);
					if (typeof(System.String).IsInstanceOfType(kvp.Value)) {
						args[1] = new AndroidJavaObject("java.lang.String", kvp.Value);
					} else {
						args[1] = new AndroidJavaObject("java.lang.Double", ""+kvp.Value);
					}
					AndroidJNI.CallObjectMethod(map.GetRawObject(), method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
				}
				
				GetJavaClass().CallStatic("onEvent",new object[]{ eventId,map});
			}
#endif
		}
	}
}
