using UnityEngine;
using System;
using System.Runtime.InteropServices;
using ClientServerCommon;

namespace KodGames.ExternalCall
{
	public class BundlePlugin
	{
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern IntPtr UnityCall_Bundle_GetMainBundleVersion();
#endif

		public static string GetMainBundleVersion()
		{
#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			return Marshal.PtrToStringAnsi(UnityCall_Bundle_GetMainBundleVersion());
#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
			return new AndroidJavaClass("com.KodGames.Android.Bundle").CallStatic<string>("getMainBundleVersion");
#endif

			return ConfigDatabase.DefaultCfg.ClientConfig.version;
		}
	}
}
