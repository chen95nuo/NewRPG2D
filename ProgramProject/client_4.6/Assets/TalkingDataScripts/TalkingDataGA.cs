// SDK Version: 2.1.0.42

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Threading;

public class TalkingDataGA {
	
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void _tdgaOnStart(string appId, string channelId);
	
	[DllImport ("__Internal")]
	private static extern void _tdgaOnEvent(string eventId, string []keys, string []stringValues, double []intValues, int count);
	
	[DllImport ("__Internal")]
	private static extern string _tdgaGetDeviceId();
	
	[DllImport ("__Internal")]
	private static extern void _tdgaSetSdkType(int type);
	
	[DllImport ("__Internal")]
	private static extern void _tdgaSetVerboseLogDisabled();
#elif UNITY_ANDROID
	//init static class --save memory/space
	private static AndroidJavaClass agent;
	private static AndroidJavaClass unityClass;
	
	private static string JAVA_CLASS = "com.tendcloud.tenddata.TalkingDataGA";
	private static string UNTIFY_CLASS = "com.unity3d.player.UnityPlayer";
#endif
	
#if UNITY_ANDROID
	public static void AttachCurrentThread() {
		AndroidJNI.AttachCurrentThread();
	}
	
	public static void DetachCurrentThread() {
		AndroidJNI.DetachCurrentThread();
	}
#endif
	
	public static string GetDeviceId() {
		//if the platform is real device
		string ret = null;
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			ret = _tdgaGetDeviceId();
#elif UNITY_ANDROID
			if (agent != null) {
				AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
				ret = agent.CallStatic<string>("getDeviceId", activity);
			}
#endif
		}
		return ret;
	}

	public static void OnStart(string appID, string channelId) {
		//if the platform is real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetSdkType(2);
			_tdgaOnStart(appID, channelId);
#elif UNITY_ANDROID
			if (agent == null) {
				agent = new AndroidJavaClass(JAVA_CLASS);
			}
			agent.SetStatic<int>("sPlatformType", 2);
	    	unityClass = new AndroidJavaClass(UNTIFY_CLASS);
			AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			agent.CallStatic("init", activity, appID, channelId);
			agent.CallStatic("onResume", activity);
#endif
		}
	}
	
	public static void OnEnd() {
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_ANDROID
			if (agent != null) {
				AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
				agent.CallStatic("onPause", activity);
				agent = null;
				unityClass = null;
			}
#endif
		}
	}
	
	public static void OnKill() {
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_ANDROID
			if (agent != null) {
				AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
				agent.CallStatic("onKill", activity);
				agent = null;
				unityClass = null;
			}
#endif
		}
	}
	
	public static void OnEvent(string actionId, Dictionary<string, object> parameters) {
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
			if (parameters != null && parameters.Count > 0 && parameters.Count <= 10) {
#if UNITY_IPHONE
				int count = parameters.Count;
				string []keys = new string[count];
				string []stringValues = new string[count];
				double []numberValues = new double[count];
				int index = 0;
				foreach (KeyValuePair<string, object> kvp in parameters) {
					if (kvp.Value is string) {
						keys[index] = kvp.Key;
						stringValues[index] = (string)kvp.Value;
					} else {
						try {
						  	double tmp = System.Convert.ToDouble(kvp.Value);
						  	numberValues[index] = tmp;
							keys[index] = kvp.Key;
						} catch(System.Exception) {
							count--;
						  	continue;
						}
					}
					index++;
				}
				
				_tdgaOnEvent(actionId, keys, stringValues, numberValues, count);
#elif UNITY_ANDROID
				int count = parameters.Count;
				AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap", count);
				
				IntPtr method_Put = AndroidJNIHelper.GetMethodID(map.GetRawClass(), 
					"put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				
				object[] args = new object[2];
				foreach (KeyValuePair<string, object> kvp in parameters) {
					args[0] = new AndroidJavaObject("java.lang.String", kvp.Key);
					if (typeof(System.String).IsInstanceOfType(kvp.Value)) {
						args[1] = new AndroidJavaObject("java.lang.String", kvp.Value);
					} else {
						args[1] = new AndroidJavaObject("java.lang.Double", ""+kvp.Value);
					}
					AndroidJNI.CallObjectMethod(map.GetRawObject(), method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
				}
				if (agent != null) {
					AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
					agent.CallStatic("onEvent", activity, actionId, map);
				}
#endif
			}
		}
    }
	
	public static void SetVerboseLogDisabled() {
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaSetVerboseLogDisabled();
#elif UNITY_ANDROID
			if (agent == null) {
				agent = new AndroidJavaClass(JAVA_CLASS);
			}
			agent.CallStatic("setVerboseLogDisabled");
#endif
		}
	}
}
