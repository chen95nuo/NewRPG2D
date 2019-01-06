using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace KodGames.ExternalCall
{
	public class DevicePlugin
	{
#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_Device_GetUDID();
		
		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_Device_GetName();

		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_Device_GetDeviceName();

		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_Device_GetSystemName();

		[DllImport ("__Internal")]
		private static extern IntPtr UnityCall_Device_GetSystemVersion();
		
		[DllImport ("__Internal")]
		private static extern bool UnityCall_Device_GetIdleTimerDisabled();
		
		[DllImport ("__Internal")]
		private static extern void UnityCall_Device_SetIdleTimerDisabled(bool disabled);
#endif

#if UNITY_ANDROID
		private static AndroidJavaClass GetJavaClass()
		{
			return new AndroidJavaClass("com.KodGames.Android.Device");
		}
#endif
		public static string GetPlatformName()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return "IOS";
#elif UNITY_ANDROID
			return "Android";
#endif
#else
			return "Unity";
#endif
		}

		public static string GetUDID()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return Marshal.PtrToStringAnsi(UnityCall_Device_GetUDID());
#elif UNITY_ANDROID
			return SystemInfo.deviceUniqueIdentifier;
#endif
#else
			// Show not be empty
			return "DefaultUDID";
#endif
		}

		public static string GetGUID()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE
			return Marshal.PtrToStringAnsi(UnityCall_Device_GetUDID());
#elif UNITY_ANDROID
			return GetJavaClass().CallStatic<string>("getGuid");
#endif
#else
			// Show not be empty
			//return "DefaultUDID";
			return GameUtility.GetUIString("UIGUID");
#endif
		}

		public static string GetName()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return Marshal.PtrToStringAnsi(UnityCall_Device_GetName());
#endif
			return "";
		}

		public static string GetDeviceName()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return Marshal.PtrToStringAnsi(UnityCall_Device_GetDeviceName());
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				return GetJavaClass().CallStatic<string>("getDevcieName");
#endif
			return "";
		}

		public static void CreateDeskShortCut()
		{
#if !UNITY_EDITOR
#if UNITY_IPHONE

#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				GetJavaClass().CallStatic("createDeskShortCut");
#endif
#endif
		}

		public static string GetSystemName()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return Marshal.PtrToStringAnsi(UnityCall_Device_GetSystemName());
#endif
			return "";
		}

		public static string GetSystemVersion()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return Marshal.PtrToStringAnsi(UnityCall_Device_GetSystemVersion());
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				return GetJavaClass().CallStatic<string>("getSystemVersion");
#endif

			return "";
		}

		public static bool GetIdleTimerDisabled()
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				return UnityCall_Device_GetIdleTimerDisabled();
#endif
			return false;
		}

		public static void SetIdleTimerDisabled(bool disabled)
		{
#if UNITY_IPHONE
			if (Application.platform == RuntimePlatform.IPhonePlayer)
				UnityCall_Device_SetIdleTimerDisabled(disabled);
#endif
		}

		public static bool IsRunningOnVirtualDevice()
		{
			return false;
		}

		public static string GetCpuAbi()
		{
#if UNITY_IPHONE

#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				return GetJavaClass().CallStatic<string>("getCpuAbi");
#endif
			return "";
		}

		public static string GetCpuAbi2()
		{
#if UNITY_IPHONE
			
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				return GetJavaClass().CallStatic<string>("getCpuAbi2");
#endif
			return "";
		}

		public static string GetSystemInfoString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendLine("");
			sb.AppendLine("OperatingSystem : " + SystemInfo.operatingSystem);
			sb.AppendLine("ProcessorType : " + SystemInfo.processorType);
			sb.AppendLine("ProcessorCount : " + SystemInfo.processorCount);
			sb.AppendLine("SystemMemorySize : " + SystemInfo.systemMemorySize);
			sb.AppendLine("GraphicsMemorySize : " + SystemInfo.graphicsMemorySize);
			sb.AppendLine("GraphicsDeviceName : " + SystemInfo.graphicsDeviceName);
			sb.AppendLine("GraphicsDeviceVendor : " + SystemInfo.graphicsDeviceVendor);
			sb.AppendLine("GraphicsDeviceID : " + SystemInfo.graphicsDeviceID);
			sb.AppendLine("GraphicsDeviceVendorID : " + SystemInfo.graphicsDeviceVendorID);
			sb.AppendLine("GraphicsDeviceVersion : " + SystemInfo.graphicsDeviceVersion);
			sb.AppendLine("GraphicsShaderLevel : " + SystemInfo.graphicsShaderLevel);
			sb.AppendLine("GraphicsPixelFillrate : " + SystemInfo.graphicsPixelFillrate);
			sb.AppendLine("SupportsShadows : " + SystemInfo.supportsShadows);
			sb.AppendLine("SupportsRenderTextures : " + SystemInfo.supportsRenderTextures);
			sb.AppendLine("SupportsImageEffects : " + SystemInfo.supportsImageEffects);
			sb.AppendLine("SupportedRenderTargetCount : " + SystemInfo.supportedRenderTargetCount);
			sb.AppendLine("DeviceUniqueIdentifier : " + SystemInfo.deviceUniqueIdentifier);
			sb.AppendLine("DeviceName : " + SystemInfo.deviceName);
			sb.AppendLine("SupportsAccelerometer : " + SystemInfo.supportsAccelerometer);
			sb.AppendLine("SupportsGyroscope : " + SystemInfo.supportsGyroscope);
			sb.AppendLine("SupportsLocationService : " + SystemInfo.supportsLocationService);
			sb.AppendLine("SupportsVibration : " + SystemInfo.supportsVibration);
			sb.AppendLine("DeviceType : " + SystemInfo.deviceType);
			return sb.ToString();
		}

		public static void ResetGame()
		{
#if UNITY_IPHONE
			
#elif UNITY_ANDROID
			if (Application.platform == RuntimePlatform.Android)
				GetJavaClass().CallStatic("restart");
#endif
		}
	}
}


//namespace DeviceUtils.Utility
//{
//    public static class DeviceUtils
//    {
//        private static IphoneModel _model = DeviceUtils.IphoneModel.Unknown;
//        private static string _modelString = string.Empty;

//        // Iphone Model
//        public enum IphoneModel
//        {
//            iPhone1,
//            iPhone3G,
//            iPhone3GS,
//            iPhone4,
//            iPhone4S,
//            iPhone5,
//            iPhone5C,
//            iPhone5S,
//            iPhoneUnknown,

//            iPod1Gen,
//            iPod2Gen,
//            iPod3Gen,
//            iPod4Gen,
//            iPod5Gen,
//            iPodUnknown,

//            iPad1,
//            iPad2,
//            iPad3, // The New iPad
//            iPad4, // iPad Retina
//            iPadAir,
//            iPadMini1Gen,
//            iPadMini2Gen, // iPad Mini Retina
//            iPadUnknown,

//            Unknown
//        }

//#if UNITY_IPHONE
//    [DllImport ("__Internal")]
//    private static extern string GetIPhoneHWMachine();
//#else
//        private static string GetIPhoneHWMachine() { return string.Empty; }
//#endif

//        public static string GetIphoneModelString()
//        {
//            if (string.IsNullOrEmpty(_modelString) == true)
//            {
//                _modelString = GetIPhoneHWMachine();
//            }

//            return _modelString;
//        }

//        // Identifiers:
//        // http://theiphonewiki.com/wiki/Models
//        public static IphoneModel GetIphoneModel()
//        {
//            if (_model == DeviceUtils.IphoneModel.Unknown)
//            {
//                string deviceHWMachine = GetIphoneModelString();

//                if (deviceHWMachine.StartsWith("iPhone"))
//                {
//                    _model = GetIphoneModel(deviceHWMachine);
//                }
//                else if (deviceHWMachine.StartsWith("iPod"))
//                {
//                    _model = GetIpodModel(deviceHWMachine);
//                }
//                else if (deviceHWMachine.StartsWith("iPad"))
//                {
//                    _model = GetIpadModel(deviceHWMachine);
//                }
//            }

//            return _model;
//        }

//        private static IphoneModel GetIphoneModel(string iPhoneHWMachine)
//        {
//            if (GetIphoneModelString().StartsWith("iPhone1,1") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone1;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone1,2") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone3G;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone2") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone3GS;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone3") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone4;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone4") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone4S;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone5,1") == true ||
//                 GetIphoneModelString().StartsWith("iPhone5,2") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone5;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone5,3") == true ||
//                 GetIphoneModelString().StartsWith("iPhone5,4") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone5C;
//            }
//            else if (GetIphoneModelString().StartsWith("iPhone6") == true)
//            {
//                return DeviceUtils.IphoneModel.iPhone5S;
//            }

//            return DeviceUtils.IphoneModel.iPhoneUnknown;
//        }

//        private static IphoneModel GetIpodModel(string iPhoneHWMachine)
//        {
//            if (GetIphoneModelString().StartsWith("iPod1") == true)
//            {
//                return DeviceUtils.IphoneModel.iPod1Gen;
//            }
//            else if (GetIphoneModelString().StartsWith("iPod2") == true)
//            {
//                return DeviceUtils.IphoneModel.iPod2Gen;
//            }
//            else if (GetIphoneModelString().StartsWith("iPod3") == true)
//            {
//                return DeviceUtils.IphoneModel.iPod3Gen;
//            }
//            else if (GetIphoneModelString().StartsWith("iPod4") == true)
//            {
//                return DeviceUtils.IphoneModel.iPod4Gen;
//            }
//            else if (GetIphoneModelString().StartsWith("iPod5") == true)
//            {
//                return DeviceUtils.IphoneModel.iPod5Gen;
//            }

//            return DeviceUtils.IphoneModel.iPodUnknown;
//        }

//        private static IphoneModel GetIpadModel(string iPhoneHWMachine)
//        {
//            if (GetIphoneModelString().StartsWith("iPad1") == true)
//            {
//                return DeviceUtils.IphoneModel.iPad1;
//            }
//            else if (GetIphoneModelString().StartsWith("iPad2,1") == true ||
//                GetIphoneModelString().StartsWith("iPad2,2") == true ||
//                GetIphoneModelString().StartsWith("iPad2,3") == true ||
//                GetIphoneModelString().StartsWith("iPad2,4") == true)
//            {
//                return DeviceUtils.IphoneModel.iPad2;
//            }
//            else if (GetIphoneModelString().StartsWith("iPad2,5") == true ||
//                 GetIphoneModelString().StartsWith("iPad2,6") == true ||
//                 GetIphoneModelString().StartsWith("iPad2,7") == true)
//            {
//                return DeviceUtils.IphoneModel.iPadMini1Gen;
//            }
//            else if (GetIphoneModelString().StartsWith("iPad3,1") == true ||
//                GetIphoneModelString().StartsWith("iPad3,2") == true ||
//                GetIphoneModelString().StartsWith("iPad3,3") == true)
//            {
//                return DeviceUtils.IphoneModel.iPad3;
//            }
//            else if (GetIphoneModelString().StartsWith("iPad3,4") == true ||
//                GetIphoneModelString().StartsWith("iPad3,5") == true ||
//                GetIphoneModelString().StartsWith("iPad3,6") == true)
//            {
//                return DeviceUtils.IphoneModel.iPad4;
//            }
//            else if (GetIphoneModelString().StartsWith("iPad4,1") == true ||
//                GetIphoneModelString().StartsWith("iPad4,2") == true ||
//                GetIphoneModelString().StartsWith("iPad4,3") == true)
//            {
//                return DeviceUtils.IphoneModel.iPadAir;
//            }
//            else if (GetIphoneModelString().StartsWith("iPad4,4") == true ||
//                GetIphoneModelString().StartsWith("iPad4,5") == true)
//            {
//                return DeviceUtils.IphoneModel.iPadAir;
//            }			

//            return DeviceUtils.IphoneModel.iPadUnknown;
//        }
//    }
//}