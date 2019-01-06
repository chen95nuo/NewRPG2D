//using UnityEngine;
//using System;
//using System.Collections;
//using System.Runtime.InteropServices;
//
//namespace KodGames.ExternalCall
//{
//	public class MessagePlugin
//	{
//#if UNITY_EDITOR || UNITY_IPHONE
//		[DllImport("__Internal")]
//		private static extern void UnityCall_Message_SendEmail(string toRecipients, string ccRecipients, string bccRecipients, string subject, string body);
//
//		[DllImport("__Internal")]
//		private static extern bool UnityCall_Message_CanSendSMSMessage();
//
//		[DllImport("__Internal")]
//		private static extern void UnityCall_Message_SendSMSMessage(string body);
//
//		[DllImport("__Internal")]
//		private static extern void UnityCall_Message_SetApplicationIconBadgeNumber(int number);
//#endif
//
//		/// <summary>
//		/// Send a mail recipient and be sperated by ","
//		/// </summary>
//		public static void SendEmail(string toRecipients, string ccRecipients, string bccRecipients, string subject, string body)
//		{
//#if UNITY_EDITOR || UNITY_IPHONE
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UnityCall_Message_SendEmail(toRecipients, ccRecipients, bccRecipients, subject, body);
//			}
//#endif
//		}
//
//		public static bool CanSendSMSMessage()
//		{
//#if UNITY_EDITOR || UNITY_IPHONE
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				return UnityCall_Message_CanSendSMSMessage();
//			}
//#endif
//
//			return false;
//		}
//
//		public static void SendSMSMessage(string body)
//		{
//#if UNITY_EDITOR || UNITY_IPHONE
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UnityCall_Message_SendSMSMessage(body);
//			}
//#endif
//		}
//
//		public static void SetApplicationIconBadgeNumber(int number)
//		{
//#if UNITY_EDITOR || UNITY_IPHONE
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UnityCall_Message_SetApplicationIconBadgeNumber(number);
//			}
//#endif
//		}
//	}
//}
