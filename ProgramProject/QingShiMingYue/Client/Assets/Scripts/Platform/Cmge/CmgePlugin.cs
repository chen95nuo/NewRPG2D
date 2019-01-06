using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall.Platform
{
	public static class CmgePlugin
	{
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cmge_Initialize();
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cmge_Login();
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cmge_Logout();
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cmge_Pay(string roleName, string roleId, string serverId, string serverName, string callBackInfo, string feePointId);
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cmge_ShowToolBar(bool show);

		//facebook 分享
		[DllImport ("__Internal")]
		private static extern void UnityCall_Facebook_Share (string facebookShareName, string facebookShareCaption, string facebookShareDesc
		                                                    , string facebookShareLink, string facebookSharePictureUrl);
		
#elif UNITY_ANDROID

		private static AndroidJavaClass javaClass = null;
		private static AndroidJavaClass GetJavaClass()
		{
            return javaClass != null ? javaClass : javaClass = new AndroidJavaClass("com.KodGames.Platform.Cmge.CMGESDK");
		}

#endif

		public static void Initialize()
		{
#if UNITY_IPHONE
			UnityCall_Cmge_Initialize();
#elif UNITY_ANDROID
            GetJavaClass().CallStatic("initialize");
#endif
		}

		public static void Login()
		{
#if UNITY_IPHONE
			UnityCall_Cmge_Login();
#elif UNITY_ANDROID
            GetJavaClass().CallStatic("login");
#endif
		}

		public static void Logout()
		{
#if UNITY_IPHONE
			UnityCall_Cmge_Logout();
#elif UNITY_ANDROID
            GetJavaClass().CallStatic("logout");
#endif
		}

        public static void Quit(String responseMethodName, String title, String message, String okText, String cancelText)
        {
#if UNITY_IPHONE
			
#elif UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android)
                GetJavaClass().CallStatic("Quit", responseMethodName, title, message, okText, cancelText);
#endif
        }

		public static void JoinGameArea(int level)
		{
#if UNITY_IPHONE

#elif UNITY_ANDROID
			GetJavaClass().CallStatic("JoinGameArea", level);
#endif		
		}

		public static void Pay(string roleName, string roleId, string serverId, string serverName, string callBackInfo, string feePointId, int price)
		{
#if UNITY_IPHONE
			UnityCall_Cmge_Pay(roleName, roleId, serverId, serverName, callBackInfo, feePointId);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("callSDKPayInteface", roleName, roleId, serverId, serverName, callBackInfo, feePointId, price);
#endif
		}

		public static void ShowToolBar(bool show)
		{
#if UNITY_IPHONE
			UnityCall_Cmge_ShowToolBar(show);
#elif UNITY_ANDROID
#endif
		}

		public static void FacebookShare(string facebookShareName, string facebookShareCaption, string facebookShareDesc
										, string facebookShareLink, string facebookSharePictureUrl)
		{
#if UNITY_IPHONE
			UnityCall_Facebook_Share(facebookShareName, facebookShareCaption, facebookShareDesc, facebookShareLink, facebookSharePictureUrl);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("FacebookShare", facebookShareName, facebookShareCaption, facebookShareDesc, facebookShareLink, facebookSharePictureUrl);
#endif
		}

		public static string GetUDID()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return KodGames.ExternalCall.DevicePlugin.GetUDID();
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
                return KodGames.ExternalCall.DevicePlugin.GetUDID();
#endif
			return KodGames.ExternalCall.DevicePlugin.GetUDID();
		}

	}

	public static class CmgeHelper
	{
		public static bool ParseLoginResponse(string message, out string cmgeSign, out string channelid, out string cmgeUserId, out string cmgeIsVIP, out string cmgeUserAccount, out string cmgeTimestamp)
		{													
			ExternalCallParameterParser parse = new ExternalCallParameterParser(message);

			bool result = true;
			result &= parse.Parse(out cmgeSign);
			result &= parse.Parse(out channelid);
			result &= parse.Parse(out cmgeUserId);
			result &= parse.Parse(out cmgeIsVIP);
			result &= parse.Parse(out cmgeUserAccount);
			result &= parse.Parse(out cmgeTimestamp);

			return result;
		}

		public static bool ParseFacebookShareCallBackResponse(string message, out int code, out string msg, out string result)
		{
			ExternalCallParameterParser parse = new ExternalCallParameterParser(message);

			bool resResult = true;
			resResult &= parse.Parse(out code);
			resResult &= parse.Parse(out msg);
			resResult &= parse.Parse(out result);

			return resResult;
		}

		public static bool ParseIAPListenerResponse(string message, out string data, out string state)
		{
			ExternalCallParameterParser parse = new ExternalCallParameterParser(message);

			bool result = true;
			result &= parse.Parse(out data);
			result &= parse.Parse(out state);

			return result;
		}
	}

	public interface ICmgePlatformListener
	{
		void CmgeInitializeResponse(string message);
		void CmgeLoginResponse(string message);
		void CmgeLogoutResponse(string message);
		void CmgePurchaseResponse(string messge);
		void CmgeFacebookShareCallBackResponse(string message);
	}
}
