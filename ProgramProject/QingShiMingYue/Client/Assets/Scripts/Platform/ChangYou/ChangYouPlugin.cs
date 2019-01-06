using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall.Platform
{
	public static class ChangYouPlugin
	{
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_Initialize();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_CheckUpdate();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_UpdateClient();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_Login(bool autoLogin);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_Logout();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_SetUserId(string userId);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_Buy(string roleId, string groupId, string pushInfo);

		//get GoodsList
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_GoodsList();
	
		//new pay way
		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_Pay(string goods_describe, string goods_icon, string goods_id, string goods_name,
		                                            string goods_price, string goods_number, string goods_register_id, int type,
		                                            string group_id, string role_id, string role_name, string pushInfo);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_ShowToolBar(bool show);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_UserCenter();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_CallCenter();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_MBI_SetAccountId(string accountId);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_MBI_SetServerId(string serverId);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_MBI_SetRoleName(string roleName);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_MBI_SetRoleId(string roleId);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_MBI_SetLevel(int level);

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_MBI_SetLoginLog();

		[DllImport ("__Internal")]
		private static extern void UnityCall_Cy_SetEvent (string eventID);

#elif UNITY_ANDROID
		private static AndroidJavaClass javaClass = null;
		private static AndroidJavaClass GetJavaClass()
		{
			return javaClass != null ? javaClass : javaClass = new AndroidJavaClass("com.KodGames.Platform.CY.CYSDK");
		}
#endif

		public static void Initialize()
		{
#if UNITY_IPHONE
			UnityCall_Cy_Initialize();
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("initialize");
#endif
		}

		public static void Destroy()
		{
#if UNITY_IPHONE
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("destroy");
#endif
		}

		public static void CheckUpdate()
		{
#if UNITY_IPHONE
			UnityCall_Cy_CheckUpdate();	
#elif UNITY_ANDROID
#endif
		}

		public static void UpdateClient()
		{
#if UNITY_IPHONE
			UnityCall_Cy_UpdateClient();
#elif UNITY_ANDROID
#endif
		}

		public static void Login(bool autoLogin)
		{
#if UNITY_IPHONE
			UnityCall_Cy_Login(autoLogin);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("login", autoLogin);
#endif
		}

		public static void Logout()
		{
#if UNITY_IPHONE
			UnityCall_Cy_Logout();
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("logout");
#endif
		}

		public static void QueryProductList()
		{
#if UNITY_IPHONE
			UnityCall_Cy_GoodsList();
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("QueryProductList");
#endif
		}

		public static void Buy(string uid, int level, string roleId, string groupId, string pushInfo, int platformId,
								string goodsName, string productId, int price, int goodsNum, string goodsDescribe, string playerName, string AccessToken, string Oid, string areaName)
		{
#if UNITY_IPHONE
//			UnityCall_Cy_Buy(roleId, groupId, pushInfo);


			UnityCall_Cy_Pay(goodsDescribe, "imageurl", platformId.ToString(), goodsName,
			                 price.ToString(), goodsNum.ToString(), productId, 0,
			                 groupId, roleId, playerName, pushInfo);

#elif UNITY_ANDROID
			GetJavaClass().CallStatic("showPayView", uid, level, roleId, groupId, pushInfo, platformId, goodsName, productId, price, goodsNum, goodsDescribe, AccessToken, Oid, playerName, areaName);
#endif
		}

		public static void ShowUserCenter()
		{
#if UNITY_IPHONE
			UnityCall_Cy_UserCenter();
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("showUserCenter");
#endif
		}

		public static void ShowToolBar(bool show)
		{
#if UNITY_IPHONE
			UnityCall_Cy_ShowToolBar(show);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("ShowToolBar", show);
#endif
		}

		public static void ShowCallCenter(string uId, string roleId, string areaId)
		{
#if UNITY_IPHONE
			UnityCall_Cy_CallCenter();
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("showCallCenter", uId, roleId, areaId);
#endif
		}

		public static void SetUserId(string userId)
		{
#if UNITY_IPHONE
			UnityCall_Cy_SetUserId(userId);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("setUserId", userId);
#endif
		}

		public static void SetAccountId(string accountId)
		{
#if UNITY_IPHONE
			UnityCall_Cy_MBI_SetAccountId(accountId);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("setAccountId", accountId);
#endif
		}

		public static void SetServerId(string serverId)
		{
#if UNITY_IPHONE
			UnityCall_Cy_MBI_SetServerId(serverId);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("setServerId", serverId);
#endif
		}

		public static void SetRoleName(string roleName)
		{
#if UNITY_IPHONE
			UnityCall_Cy_MBI_SetRoleName(roleName);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("setRoleName", roleName);
#endif
		}

		public static void SetRoleId(string roleId)
		{
#if UNITY_IPHONE
			UnityCall_Cy_MBI_SetRoleId(roleId);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("setRoleId", roleId);
#endif
		}

		public static void SetLevel(int level)
		{
#if UNITY_IPHONE
			UnityCall_Cy_MBI_SetLevel(level);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("setLevel", level);
#endif
		}

		public static string GetUDID()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return KodGames.ExternalCall.DevicePlugin.GetUDID();
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				return GetJavaClass().CallStatic<string>("getUDID");
#endif
			return KodGames.ExternalCall.DevicePlugin.GetUDID();
		}

		public static void Quit()
		{
#if UNITY_IPHONE
			
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				GetJavaClass().CallStatic("Quit");
#endif
		}

		public static void SetEvent(string eventID)
		{
#if UNITY_IPHONE
			UnityCall_Cy_SetEvent(eventID);
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("SetEvent", eventID);
#endif
		}

		public static void SetLoginLog()
		{
#if UNITY_IPHONE
			UnityCall_Cy_MBI_SetLoginLog();
#endif
		}

		public static void UploadGameData(int playerId, string playerName, int playerLevel, int areaId, string areaName, bool isCreatRole)
		{
#if UNITY_IPHONE
			
#elif UNITY_ANDROID
			GetJavaClass().CallStatic("UploadGameData", playerId, playerName, playerLevel, areaId, areaName, isCreatRole);
#endif
		}

	}

	public static class ChangYouHelper
	{
		public static bool ParseLoginResponse(string message, out string data, out string channelid, out string opcode, out string promoteid, out string displayName)
		{
			ExternalCallParameterParser parse = new ExternalCallParameterParser(message);

			bool result = true;
			result &= parse.Parse(out data);
			result &= parse.Parse(out channelid);
			result &= parse.Parse(out opcode);
			result &= parse.Parse(out promoteid);
			result &= parse.Parse(out displayName);

			return result;
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

	public interface IChangYouPlatformListener
	{
		void CyInitializeResponse(string message);
		void CyCheckUpdataResponse(string message);
		void CyLoginResponse(string message);
		void CyLogoutResponse(string message);
		void CyPurchaseResponse(string message);
	}
}
