//using UnityEngine;
//using System;
//using System.Collections;
//using System.Runtime.InteropServices;
//
//namespace KodGames.ExternalCall
//{
//	public class PartytrackPlugin
//	{
//#if UNITY_IPHONE
//		[DllImport ("__Internal")]
//		private static extern void Partytrack_SendPayment(float itemPrice,string itemName);
//#elif UNITY_ANDROID
//		private static AndroidJavaClass GetJavaClass()
//		{
//			return new AndroidJavaClass("com.KodGames.Android.PartytrackSDK");
//		}
//#endif
//
//		public static void SendPayment(float itemPrice,string itemName)
//		{
//#if UNITY_IPHONE
//			Partytrack_SendPayment(itemPrice,itemName);
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("Partytrack_SendPayment",new object[]{ itemPrice, itemName});
//#endif
//		}
//	}
//}
//
