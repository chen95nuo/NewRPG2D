using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void Update () {  
		
		if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Home))  
		{  
#if UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");  
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");  
			jo.Call("doExit");
#endif
		}  
	} 
	
	public static void ExitSDK(){
#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");  
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");  
			jo.Call("doExit");
#endif
	}
}
