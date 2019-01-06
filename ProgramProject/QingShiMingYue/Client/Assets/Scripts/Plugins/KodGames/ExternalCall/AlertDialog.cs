using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	public class AlertDialog
	{
#if !UNITY_EDITOR
#if UNITY_ANDROID
		private static AndroidJavaClass GetJavaClass()
		{
			return new AndroidJavaClass("com.KodGames.Android.AlertDialog");
		}
#endif
#endif

		public static void Show(string responseMethodName, string title, string message, string okText, string cancelText)
		{
#if !UNITY_EDITOR
#if UNITY_ANDROID
			GetJavaClass().CallStatic("show", responseMethodName,title, message, okText, cancelText);
#endif
#endif
		}
	}

	public static class AlertDialogHelper
	{
		public static bool ParseOnAlertDialogResponse(string message, out bool pressOKNotCancel)
		{
			return new ExternalCallParameterParser(message).Parse(out pressOKNotCancel);
		}
	}

	public interface IAlertDialogListener
	{
		void OnAlertDialogResponse(string message);
	}
}
