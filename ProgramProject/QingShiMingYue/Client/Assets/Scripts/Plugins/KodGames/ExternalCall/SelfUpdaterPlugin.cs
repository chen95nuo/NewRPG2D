using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	public class SelfUpdaterPlugin
	{
#if UNITY_ANDROID
		private static AndroidJavaClass GetJavaClass()
		{
			return new AndroidJavaClass("com.KodGames.Android.UpdateClient");
		}
#endif

		public static void Update(string apkUrl, string appUpdateDesc, bool forceUpdate)
		{
#if !UNITY_EDITOR
#if UNITY_ANDROID
			GetJavaClass().CallStatic("update",
				apkUrl, 
				forceUpdate,
				GameUtility.GetUIString("UpdateClient_NoStorageMsg"),
				appUpdateDesc,
				GameUtility.GetUIString("UpdateClient_UpdateDesc"),
				GameUtility.GetUIString("UpdateClient_ErrorMsg"),
				GameUtility.GetUIString("UpdateClient_UpdateingMsg"),
				GameUtility.GetUIString("UpdateClient_RetryText"),
				GameUtility.GetUIString("UpdateClient_UpdateText"),
				GameUtility.GetUIString("UpdateClient_CancelText"),
				GameUtility.GetUIString("UpdateClient_InstallText"),
				GameUtility.GetUIString("UpdateClient_InstallMsg"),
				GameUtility.GetUIString("UpdateClient_ReUpdateMsg"),
				GameUtility.GetUIString("UpdateClient_ReUpdateOkText"),
				GameUtility.GetUIString("UpdateClient_ReUpdateCancelText"),
				GameUtility.GetUIString("UpdateClient_ApkSize"),
				GameUtility.GetUIString("UpdateClient_ConnectType"),
				GameUtility.GetUIString("UpdateClient_ConnectTypeShowMsg"));
#else
			
#endif
#endif
		}

		public static void DeleteUpdateFile()
		{
#if !UNITY_EDITOR
#if UNITY_ANDROID
			GetJavaClass().CallStatic("deleteApkFileAfterUpdate");
#else
			
#endif
#endif
		}
	}
}
