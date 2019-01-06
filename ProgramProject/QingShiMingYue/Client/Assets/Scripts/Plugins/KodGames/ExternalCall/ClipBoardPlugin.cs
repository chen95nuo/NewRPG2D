//using UnityEngine;
//using System;
//using System.Collections;
//using System.Runtime.InteropServices;
//
//namespace KodGames.ExternalCall
//{
//	public class ClipBoardPlugin
//	{
//#if UNITY_IPHONE	
//	[DllImport ("__Internal")]
//	private static extern void UnityCall_ClipBoard_SetString(string data);
//	
//	[DllImport ("__Internal")]
//	private static extern IntPtr UnityCall_ClipBoard_GetString();
//#endif
//
//		public static void SetString(string data)
//		{
//#if UNITY_IPHONE
//		if (Application.platform == RuntimePlatform.IPhonePlayer)
//			UnityCall_ClipBoard_SetString(data);
//#elif UNITY_ANDROID
//		if (Application.platform == RuntimePlatform.Android)
//			new AndroidJavaClass("com.KodGames.Android.ClipBoard").CallStatic("setString", data);
//#endif
//		}
//
//		public static string GetString()
//		{
//#if UNITY_IPHONE		
//		if (Application.platform == RuntimePlatform.IPhonePlayer)
//			return Marshal.PtrToStringAnsi(UnityCall_ClipBoard_GetString());
//#elif UNITY_ANDROID
//		if (Application.platform == RuntimePlatform.Android)
//			return new AndroidJavaClass("com.KodGames.Android.ClipBoard").CallStatic<string>("getString");
//#endif
//
//			return "";
//		}
//	}
//}
