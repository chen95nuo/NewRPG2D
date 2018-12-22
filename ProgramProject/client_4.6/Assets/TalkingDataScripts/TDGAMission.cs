using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class TDGAMission {
	
	private static string JAVA_CLASS = "com.tendcloud.tenddata.TDGAMission";
	
#if UNITY_IPHONE
	[DllImport ("__Internal")]   
    private static extern void _tdgaOnBegin(string missionId);
    
	[DllImport ("__Internal")]
    private static extern void _tdgaOnCompleted(string missionId);
    
	[DllImport ("__Internal")]
    private static extern void _tdgaOnFailed(string missionId, string failedCause);
#elif UNITY_ANDROID
	static AndroidJavaClass agent = new AndroidJavaClass(JAVA_CLASS);
#endif
	
	public static void OnBegin(string missionId) {
		//if the platform is real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaOnBegin(missionId);
#elif UNITY_ANDROID
			agent.CallStatic("onBegin", missionId);
#endif
		}		
	}
	
	public static void OnCompleted(string missionId) {
		//if the platform is real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaOnCompleted(missionId);
#elif UNITY_ANDROID
			agent.CallStatic("onCompleted", missionId);
#endif
		}		
	}
	
	public static void OnFailed(string missionId, string failedCause) {
		//if the platform is real device
		if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor){
#if UNITY_IPHONE
			_tdgaOnFailed(missionId, failedCause);
#elif UNITY_ANDROID
			agent.CallStatic("onFailed", missionId, failedCause);
#endif
		}		
	}
}


