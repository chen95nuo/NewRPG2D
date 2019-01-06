//using UnityEngine;
//using System;
//using System.Collections;
//using System.Runtime.InteropServices;
//
//namespace KodGames.ExternalCall
//{
//	public class MZMonitorPlugin
//	{
//#if UNITY_IPHONE
//		[DllImport ("__Internal")]
//		private static extern void UnityCall_MZMonitor_AdTrack();
//#endif
//
//		public static void AdTrack()
//		{
//#if UNITY_IPHONE		
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//				UnityCall_MZMonitor_AdTrack();
//#elif UNITY_ANDROID
//#endif
//		}
//	}
//}
