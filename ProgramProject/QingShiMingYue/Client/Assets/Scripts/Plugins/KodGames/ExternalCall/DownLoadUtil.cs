using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using ClientServerCommon;

namespace KodGames.ExternalCall
{
	public class DownLoadUtil
	{
#if UNITY_IPHONE

#endif

#if UNITY_ANDROID
		private static AndroidJavaClass javaClass = null;
		private static AndroidJavaClass GetJavaClass()
		{
			return javaClass != null ? javaClass : javaClass = new AndroidJavaClass("com.KodGames.Android.DownLoadUtil");
		}
#endif


		public static void DownLoadGameAsset(string remoteUrl, string localUrl, string localkodNamesFileName)
		{
#if !UNITY_EDITOR
#if UNITY_ANDROID
			GetJavaClass().CallStatic("DownLoadGameAsset",remoteUrl, localUrl, localkodNamesFileName);
#endif
#endif
		}

		public static string[] GetFinishKodName()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return null;
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<string[]>("GetFinishKodName");
#endif
#else
			return null;
#endif
		}

		public static string[] GetFailedKodName()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return null;
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<string[]>("GetFailedKodName");
#endif
#else
			return null;
#endif
		}
	}
}

