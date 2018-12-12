//
//  UmengManager.cs
//
//  Created by ZhuCong on 1/1/14.
//  Copyright 2014 Umeng.com . All rights reserved.
//  Version 1.31

using UnityEngine;
using System.Collections;
using Umeng;
public class UmengManager : MonoBehaviour
{
	

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
	
	#if UNITY_ANDROID
	
	void OnApplicationPause(bool isPause)
	{
		
		//Debug.Log("Umeng:OnApplicationPause" + isPause);
		if (isPause){
			//Debug.Log("Umeng:----onPause");
			GA.onPause();
		}
		else{
			//Debug.Log("Umeng:----onResume");
			GA.onResume();
		}
		
	}
	
	
	
	void OnApplicationQuit()
	{
		//Debug.Log("Umeng:OnApplicationQuit");
		GA.onKillProcess();
	}
	
	
	
	#endif
	
	
	
	
	
}
