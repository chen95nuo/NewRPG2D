using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall.Platform
{
	public static class EfunPlugin
	{
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_Initialize();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_Login();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_Logout();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_Pay(string productId, string playerId, string serverCode,
		                                              string roleLevel, string roleName, string remark);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_ShowToolBar(bool show);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_UserCenter();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_CallCenter();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_ShowCircleUserCenter(string serverCode, string roleName, string roleId, string roleLevel, string remark, string creditId);

		//facebook 分享
		[DllImport ("__Internal")]
		private static extern void UnityCall_Efun_Share (string facebookShareName, string facebookShareCaption, string facebookShareDesc
		                                                     , string facebookShareLink, string facebookSharePictureUrl);

#elif UNITY_ANDROID

#endif

		public static void Initialize()
		{
#if UNITY_IPHONE
			UnityCall_Efun_Initialize();
#elif UNITY_ANDROID
#endif
		}

		public static void Login()
		{
#if UNITY_IPHONE
			UnityCall_Efun_Login();
#elif UNITY_ANDROID
#endif
		}

		public static void Logout()
		{
#if UNITY_IPHONE
			UnityCall_Efun_Logout();
#elif UNITY_ANDROID
#endif
		}

		public static void Pay(string productId, string playerId, string serverCode,
		                       string roleLevel, string roleName, string remark)
		{
			#if UNITY_IPHONE
			UnityCall_Efun_Pay(productId, playerId, serverCode, roleLevel, roleName, remark);
#elif UNITY_ANDROID
#endif
		}

		public static void ShowToolBar(bool show)
		{
#if UNITY_IPHONE
			UnityCall_Efun_ShowToolBar(show);
#elif UNITY_ANDROID
#endif
		}

		public static void UserCenter()
		{
#if UNITY_IPHONE
			UnityCall_Efun_UserCenter();
#elif UNITY_ANDROID
#endif
		}

		public static void CallCenter()
		{
#if UNITY_IPHONE
			UnityCall_Efun_CallCenter();
#elif UNITY_ANDROID
#endif
		}

		public static void ShowCircleUserCenter(string serverCode, string roleName, string roleId, string roleLevel, string remark, string creditId)
		{
#if UNITY_IPHONE
			UnityCall_Efun_ShowCircleUserCenter(serverCode, roleName, roleId, roleLevel, remark, creditId);
#elif UNITY_ANDROID
#endif
		}

		public static void FacebookShare(string facebookShareName, string facebookShareCaption, string facebookShareDesc
		                                 , string facebookShareLink, string facebookSharePictureUrl)
		{
#if UNITY_IPHONE
			UnityCall_Efun_Share(facebookShareName, facebookShareCaption, facebookShareDesc, facebookShareLink, facebookSharePictureUrl);
#elif UNITY_ANDROID
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


	public static class EfunHelper
	{
		public static bool ParseLoginResponse(string message, out string efuncode, out string efunmessage, out string efunsign, out string efunUserAccount, out string efunTimestamp, out string efunuserid)
		{
			ExternalCallParameterParser parse = new ExternalCallParameterParser(message);

			bool result = true;
			result &= parse.Parse(out efuncode);
			result &= parse.Parse(out efunmessage);
			result &= parse.Parse(out efunsign);
			result &= parse.Parse(out efunUserAccount);
			result &= parse.Parse(out efunTimestamp);
			result &= parse.Parse(out efunuserid);

			return result;
		}

		public static bool ParseFacebookShareCallBackResponse(string message, out int FBcode, out string FBmessage)
		{
			ExternalCallParameterParser parse = new ExternalCallParameterParser(message);

			bool resResult = true;
			resResult &= parse.Parse(out FBcode);
			resResult &= parse.Parse(out FBmessage);

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

	public interface IEfunPlatformListener
	{
		void EfunInitializeResponse(string message);
		void EfunLoginResponse(string message);
		void EfunLogoutResponse(string message);
		void EfunPurchaseResponse(string messge);
		void EfunFacebookShareCallBackResponse(string message);
	}
}