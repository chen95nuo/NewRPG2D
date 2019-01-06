using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	/// <summary>
	/// 用于从外部(Android,iOS)获取相关配置
	/// </summary>
	public class KodConfigPlugin
	{
#if !UNITY_EDITOR
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern int UnityCall_KodConfig_GetChannelId();

		[DllImport ("__Internal")]
		private static extern int UnityCall_KodConfig_GetSubChannelId();

		[DllImport ("__Internal")]
		private static extern int UnityCall_KodConfig_GetPublisher();

		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_KodConfig_GetServerIP();

		[DllImport ("__Internal")]
		private static extern int UnityCall_KodConfig_GetServerPort();

		[DllImport ("__Internal")]
		private static extern int UnityCall_KodConfig_GetRevision();

		[DllImport ("__Internal")]
		private static extern bool UnityCall_KodConfig_IsAppStore();

		[DllImport ("__Internal")]
		private static extern bool UnityCall_KodConfig_HasAdditionalSplashScreen();

		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_KodConfig_GetAdditionalSplashPath();
		
		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_KodConfig_GetStringValue(String value);

		[DllImport ("__Internal")]
		private static extern int UnityCall_KodConfig_GetIntValue(String value);

		[DllImport ("__Internal")]
		private static extern bool UnityCall_KodConfig_GetBoolValue(String value);

		[DllImport ("__Internal")]
		private static extern void UnityCall_KodConfig_Pasteboard(String pasteStr);

#elif UNITY_ANDROID
		private static AndroidJavaClass javaClass = null;
		private static AndroidJavaClass GetJavaClass()
		{
			return javaClass == null ? javaClass = new AndroidJavaClass("com.KodGames.Android.KodConfig") : javaClass;
		}
#endif
#endif

		public static int GetPublisher()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_GetPublisher();
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<int>("getPublisher");
#endif
#else
			return 0;
#endif
		}

		public static string GetServerIP()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return Marshal.PtrToStringAnsi(UnityCall_KodConfig_GetServerIP());
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<string>("getServerIP");
#endif
#else
			return "";
#endif
		}

		public static int GetServerPort()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_GetServerPort();
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<int>("getServerPort");
#endif
#else
			return 0;
#endif
		}

		public static int GetRevision()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_GetRevision();
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<int>("getRevision");
#endif
#else
			return 0;
#endif
		}

		public static int GetChannelId()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_GetChannelId();
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<int>("getChannelId");
#endif
#else
			return 0;
#endif
		}

		public static int GetSubChannelId()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_GetSubChannelId();
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<int>("getSubChannelId");
#endif
#else
			return 0;
#endif
		}

		public static bool IsAppStore()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_IsAppStore();
#elif UNITY_ANDROID
			return false;
#endif
#else
			return false;
#endif
		}

		public static bool HasAdditionalSplashScreen()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return UnityCall_KodConfig_HasAdditionalSplashScreen();
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<bool>("hasAdditionalSplashScreen");
#endif
#else
			return false;
#endif

		}

		public static string GetAdditionalSplashPath()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return Marshal.PtrToStringAnsi(UnityCall_KodConfig_GetAdditionalSplashPath());
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<string>("getAdditionalSplashPath");
#endif
#else
			return "";
#endif
		}

		public static string GetPromoteId()
		{
			string promoteId = "";
#if !UNITY_EDITOR
#if UNITY_IPHONE
			promoteId = GetStringValue("CyPromoteId");
#elif UNITY_ANDROID
			promoteId = GetStringValue("CMBIChannelId");
#endif
#endif
			return promoteId;
		}

		private static Dictionary<string, int> intValues = new Dictionary<string, int>();
		public static int GetIntValue(string key)
		{
			if (intValues.ContainsKey(key))
				return intValues[key];

			int value = 0;
#if !UNITY_EDITOR
#if UNITY_IPHONE
			value = UnityCall_KodConfig_GetIntValue(key);
#elif UNITY_ANDROID
            value = GetJavaClass().CallStatic<int>("getIntValue", key);
#endif
#endif
			intValues.Add(key, value);
			return value;
		}

		private static Dictionary<string, string> stringValues = new Dictionary<string, string>();
		public static string GetStringValue(string key)
		{
			if (stringValues.ContainsKey(key))
				return stringValues[key];

			string value = "";
#if !UNITY_EDITOR
#if UNITY_IPHONE
			value = Marshal.PtrToStringAnsi(UnityCall_KodConfig_GetStringValue(key));
#elif UNITY_ANDROID
            value = GetJavaClass().CallStatic<string>("getStringValue", key);
#endif
#endif
			stringValues.Add(key, value);
			return value;
		}

		private static Dictionary<string, bool> boolValues = new Dictionary<string, bool>();
		public static bool GetBoolValue(string key)
		{
			if (boolValues.ContainsKey(key))
				return boolValues[key];

			bool value = false;
#if !UNITY_EDITOR
#if UNITY_IPHONE
			value = UnityCall_KodConfig_GetBoolValue(key);
#elif UNITY_ANDROID
            value = GetJavaClass().CallStatic<bool>("getBoolValue", key);
#endif
#endif
			boolValues.Add(key, value);
			return value;
		}
		
		public static void Pasteboard(String pasteStr)
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			UnityCall_KodConfig_Pasteboard(pasteStr);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("Pasteboard", pasteStr);
#endif
#endif
		}
	}
}
