using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SDKManager
{
	private static SDKManager instance = null;
	
	bool normal;
	
	bool useSDK91;
	bool useSDKPP;
	bool useSDKTB;
	//==蜂巢sdk母包(android)==//
	bool useSDKGC;
	//==云点友游sdk(android)==//
	bool useSDKCPYY;
	//==酷派==//
	bool useCoolpad;
	//==百度多酷sdk(android)==//
	bool useSDKBDDK;
	//kuaiyong//
	bool useSDKKY;
	
	SDKManager()
	{
		useSDK91 = false;
		useSDKPP = false;
		useSDKTB = false;
		useSDKGC = false;
		useSDKCPYY=false;
		useCoolpad=false;
		useSDKBDDK=false;
		useSDKKY=false;

		//==所有sdk标记需要加入到下边的list,用来判断是否需要显示切换账号按钮(无sdk的时候显示)==//
		List<bool> sdks=new List<bool>();
		sdks.Add(useSDK91);
		sdks.Add(useSDKPP);
		sdks.Add(useSDKTB);
		sdks.Add(useSDKGC);
		sdks.Add(useSDKCPYY);
		sdks.Add(useCoolpad);
		sdks.Add(useSDKBDDK);
		sdks.Add(useSDKKY);
		
		normal=true;
		foreach(bool useSdk in sdks)
		{
			if(useSdk)
			{ 
				normal=false;
				break;
			}
		}
	}
	
	public static SDKManager getInstance()
	{
		if(instance == null)
		{
			instance = new SDKManager();
		}
		return instance;
	}
	//==正常,没有使用sdk==//
	public bool isNormal()
	{
		return normal;
	}
	
	public bool isUseBreakSDK()
	{
		if(useSDK91 || useSDKPP || useSDKTB)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public bool isSDK91Using()
	{
		return useSDK91;
	}
	
	public bool isSDKTBUsing()
	{
		return useSDKTB;
	}
	
	public bool isSDKGCUsing()
	{
		return useSDKGC;
	}
	
	public bool isSDKCPYYUsing()
	{
		return useSDKCPYY;
	}

	public bool isSDKCoolpadUsing()
	{
		return useCoolpad;
	}
	
	public bool isSDKBDDKUsing()
	{
		return useSDKBDDK;
	}
	
	public bool isSDKKYUsing()
	{
		return useSDKKY;
	}
}


