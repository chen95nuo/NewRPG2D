//using UnityEngine;
//using System.Runtime.InteropServices;
//
//namespace KodGames.ExternalCall
//{
//	public static class CrittercismPlugin
//	{
//		/// <summary>
//		/// Show debug and log messaged in the console in release mode.
//		/// If true CrittercismPlugin logs will not appear in the console.
//		/// </summary>
//		const bool _ShowDebugOnOnRelease = false;
//
//		private static bool sHandleUnityExceptions = false;
//
//#if UNITY_IPHONE
//		const string _INTERNAL	= "__Internal";
//
//		[DllImport(_INTERNAL)]
//		private static extern bool UnityCall_Crittercism_IsInited();
//
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_NewException(string name, string reason, string stack);
//		[DllImport(_INTERNAL)]
//		private static extern bool UnityCall_Crittercism_LogHandledException();
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_LogUnhandledException();
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_LogUnhandledExceptionWillCrash();
//		
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_SetAsyncBreadcrumbMode(bool writeAsync);
//	    [DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_LeaveBreadcrumb(string breadcrumb);
//
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_SetUsername(string key);
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_SetValue(string value, string key);
//		
//		[DllImport(_INTERNAL)]
//		private static extern void UnityCall_Crittercism_SetOptOutStatus(bool status);
//		[DllImport(_INTERNAL)]
//		private static extern bool UnityCall_Crittercism_GetOptOutStatus();
//
//#elif UNITY_ANDROID
//		private static AndroidJavaClass GetJavaClass()
//		{
//			return new AndroidJavaClass("com.KodGames.Crittercism.CrittercismPlugin");
//		}
//#endif
//
//		/// <summary>
//		/// Description:
//		/// Start Crittercism for Unity, will start crittercism for ios if it is not already active.
//		/// Parameters:
//		/// appID: Crittercisms Provided App ID for this application
//		/// key: Crittercisms Provided Key for this application
//		/// secret: Crittercisms Provided Secret for this application
//		/// loadFromResources: Attempt to load the appID, key, and secret from the CrittercismIDs.text file.
//		/// handleUnityExceptions: Allow crittercisms to recieve unity handled exceptions.
//		/// </summary>
//		public static void SetupExceptionHandler(bool handleUnityExceptions)
//		{
//			if (sHandleUnityExceptions == handleUnityExceptions)
//				return;
//
//			sHandleUnityExceptions = handleUnityExceptions;
//
//#if!UNITY_EDITOR
//#if UNITY_IPHONE
//			if (UnityCall_Crittercism_IsInited() == false)
//				return;
//#elif UNITY_ANDROID
//			if (GetJavaClass().CallStatic<bool>("isInited") == false)
//				return;
//#endif
//#endif
//			if (sHandleUnityExceptions)
//			{
//				System.AppDomain.CurrentDomain.UnhandledException += _OnUnresolvedExceptionHandler;
//				Application.RegisterLogCallback(_OnDebugLogCallbackHandler);
//				Debug.Log("CrittercismPlugin: SetupExceptionHandler");
//			}
//			else
//			{
//				System.AppDomain.CurrentDomain.UnhandledException -= _OnUnresolvedExceptionHandler;
//				Application.RegisterLogCallback(null);
//				Debug.Log("CrittercismPlugin: RemoveExceptionHandler");
//			}
//		}
//
//		/// <summary>
//		/// Log an exception that has been handled in code.
//		/// This exception will be reported to the Crittercism portal.
//		/// </summary>
//		public static void LogHandledException(System.Exception e)
//		{
//			if (e == null)
//				return;
//
//			string name = _EscapeString(e.ToString());
//			string message = _EscapeString(e.Message);
//			string stack = _EscapeString(e.StackTrace);
//
//#if!UNITY_EDITOR
//#if UNITY_IPHONE
//			UnityCall_Crittercism_NewException(name, message, stack);
//			UnityCall_Crittercism_LogHandledException();
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("newException", name, message, stack);
//			GetJavaClass().CallStatic("logUnhandledException");
//#endif
//#endif
//
//			//#else
//			string logMessage = name + "\n" + message + "\n" + stack;
//			if (Debug.isDebugBuild == true || _ShowDebugOnOnRelease == true)
//				Debug.LogWarning(logMessage);
//			//#endif
//		}
//
//		/// <summary>
//		/// Retrieve whether the user is opting out of Crittercism.
//		/// </summary>
//		public static bool GetOptOut()
//		{
//#if !UNITY_EDITOR
//#if UNITY_IPHONE 
//			return UnityCall_Crittercism_GetOptOutStatus();	
//#elif UNITY_ANDROID
//			return GetJavaClass().CallStatic<bool>("getOptOutStatus");
//#endif
//#else 
//			return false;
//#endif
//		}
//
//		/// <summary>
//		/// Set if whether the user is opting to use crittercism
//		/// </summary></param>
//		public static void SetOptOut(bool s)
//		{
//#if !UNITY_EDITOR
//#if UNITY_IPHONE 
//			UnityCall_Crittercism_SetOptOutStatus(s);
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("setOptOutStatus",s);
//#endif
//#endif
//		}
//
//		/// <summary>
//		/// Set the Username of the user
//		/// This will be reported in the Crittercism Meta.
//		/// </summary>
//		public static void SetUsername(string username)
//		{
//#if !UNITY_EDITOR
//#if UNITY_IPHONE 
//			UnityCall_Crittercism_SetUsername(_EscapeString(username));	
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("setUsername",_EscapeString(username));
//#endif
//#endif
//		}
//
//		/// <summary>
//		/// Add a custom value to the Crittercism Meta.
//		/// </summary>
//		public static void SetValue(string v, string key)
//		{
//#if !UNITY_EDITOR
//#if UNITY_IPHONE 
//			UnityCall_Crittercism_SetValue(_EscapeString(v), _EscapeString(key)); 
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("setValue",_EscapeString(v), _EscapeString(key));
//#endif
//#endif
//		}
//
//		/// <summary>
//		/// Leave a breadcrumb for tracking.
//		/// </summary>
//		public static void LeaveBreadcrumb(string l)
//		{
//#if !UNITY_EDITOR
//#if UNITY_IPHONE 
//			UnityCall_Crittercism_LeaveBreadcrumb(_EscapeString(l));
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("leaveBreadcrumb",_EscapeString(l));
//#endif
//#endif
//		}
//
//		private static string _EscapeString(string enter)
//		{
//			if (string.IsNullOrEmpty(enter))
//				enter = "";
//			else
//				enter = WWW.EscapeURL(enter);
//
//			return enter;
//		}
//
//		private static void _OnUnresolvedExceptionHandler(object sender, System.UnhandledExceptionEventArgs args)
//		{
//			if (args == null || args.ExceptionObject == null)
//				return;
//
//			try
//			{
//				System.Type type = args.ExceptionObject.GetType();
//				if (type == typeof(System.Exception))
//				{
//#if!UNITY_EDITOR
//					System.Exception e = (System.Exception)args.ExceptionObject;
//					string name = _EscapeString(e.ToString());
//					string message = _EscapeString(e.Message);
//					string stack = _EscapeString(e.StackTrace);
//#if UNITY_IPHONE
//					UnityCall_Crittercism_NewException(name, message, stack);
//					UnityCall_Crittercism_LogUnhandledException();
//					UnityCall_Crittercism_LogUnhandledExceptionWillCrash();
//#elif UNITY_ANDROID
//					GetJavaClass().CallStatic("newException", name, message, stack);
//					GetJavaClass().CallStatic("logUnhandledException");
//					GetJavaClass().CallStatic("logUnhandledExceptionWillCrash");
//#else
//					string logMessage	= name + "\n" + message + "\n" + stack;
//					if(args.IsTerminating)
//					{
//						if (Debug.isDebugBuild == true || _ShowDebugOnOnRelease == true)
//							Debug.LogError("CrittercismPlugin: Terminal Exception: " + logMessage);
//					}
//					else
//					{
//						if (Debug.isDebugBuild == true || _ShowDebugOnOnRelease == true)
//							Debug.LogWarning(logMessage);
//					}
//#endif
//#endif
//				}
//				else
//				{
//					if (Debug.isDebugBuild == true || _ShowDebugOnOnRelease == true)
//						Debug.Log("CrittercismPlugin: Unknown Exception Type: " + args.ExceptionObject.ToString());
//				}
//			}
//			catch
//			{
//				if (Debug.isDebugBuild == true || _ShowDebugOnOnRelease == true)
//					Debug.Log("CrittercismPlugin: Failed to resolve exception");
//			}
//		}
//
//		private static void _OnDebugLogCallbackHandler(string name, string stack, LogType type)
//		{
//			if (!sHandleUnityExceptions)
//				return;
//
//			if (LogType.Exception == type || LogType.Assert == type)
//			{
//				name = _EscapeString(name);
//				stack = _EscapeString(stack);
//
//#if!UNITY_EDITOR
//#if UNITY_IPHONE
//				UnityCall_Crittercism_NewException(name, name, stack);
//				UnityCall_Crittercism_LogUnhandledException();
//#elif UNITY_ANDROID
//			GetJavaClass().CallStatic("newException", name, name, stack);
//				GetJavaClass().CallStatic("logUnhandledException");
//#endif
//#endif
//			}
//		}
//	}
//}