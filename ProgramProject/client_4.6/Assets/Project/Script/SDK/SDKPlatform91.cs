using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class SDKPlatform91 : MonoBehaviour {

    const int PLAT_91_APPID = 115608;
    const string PLAT_91_APPKEY = "bb53adbd3c3523f71288fd5f64b1e5591f984a6d3eebba26";
	
	const string PLAT_KY_APPID = "5987";
	const string PLAT_KY_MD5KEY = "O3Og75Qy4uskL21lP6RkGlmrcZEGYidM";
	
#if UNITY_EDITOR
#elif PLAT_91
    [DllImport("__Internal")]
    private static extern void SDK91NdInit(int appId, string appKey, bool testMode);
    [DllImport("__Internal")]
    private static extern void SDK91NdLogin();
    [DllImport("__Internal")]
    private static extern void SDK91NdLoginEx();
    [DllImport("__Internal")]
    private static extern void SDK91NdLogout();
    [DllImport("__Internal")]
    private static extern void SDK91NdLogSwitch();
    [DllImport("__Internal")]
    private static extern void SDK91NdPaySyn(string orderId, string productId, string productName, float productPrice, int productCount, string payDesc);
    [DllImport("__Internal")]
    private static extern void SDK91NdPayAsyn(string orderId, string productId, string productName, float productPrice, int productCount, string payDesc);
    [DllImport("__Internal")]
    private static extern void SetCallback(string objName, string funcName);
#elif PLAT_KY
	[DllImport("__Internal")]
    private static extern void SDKKyLogin();
	[DllImport("__Internal")]
    private static extern void SDKKyLoginDefault();
	[DllImport("__Internal")]
    private static extern void SDKKyPay(string orderId, string productName, string productPrice, string serverId, string appId, string md5Key);
	[DllImport("__Internal")]
    private static extern void SDKKySet();
	[DllImport("__Internal")]
    private static extern void SetCallback(string objName, string funcName);
#endif

    public static void SdkInit()
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdInit(PLAT_91_APPID, PLAT_91_APPKEY, false);
#endif
    }

    public static void SdkLogin()
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdLogin();
#elif PLAT_KY
		SDKKyLogin();
#endif
    }

    public static void SdkLoginEx()
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdLoginEx();
#endif
    }

    public static void SdkLogout()
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdLogout();
#endif
    }

    public static void SdkLogSwitch()
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdLogSwitch();
#endif
    }

    public static void SdkPaySyn(string orderId, string productId, string productName, float productPrice, int productCount, string payDesc)
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdPaySyn(orderId, productId, productName, productPrice, productCount, payDesc);
#endif
    }

    public static void SdkPayAsyn(string orderId, string productId, string productName, float productPrice, int productCount, string payDesc)
    {
#if UNITY_EDITOR
#elif PLAT_91
        SDK91NdPayAsyn(orderId, productId, productName, productPrice, productCount, payDesc);
#endif
    }
	
	public static void SdkPayKy(string orderId, string productName, string productPrice)
	{
#if UNITY_EDITOR
#elif PLAT_KY
        SDKKyPay(orderId, productName, productPrice, "", PLAT_KY_APPID, PLAT_KY_MD5KEY);
#endif
	}
	
	public static void SdkSet()
	{
#if UNITY_EDITOR
#elif PLAT_KY
        SDKKySet();
#endif
	}

    public static void SdkCallback(string objName, string funcName)
    {
#if UNITY_EDITOR
#elif PLAT_91
        SetCallback(objName, funcName);
#elif PLAT_KY
		SetCallback(objName, funcName);
#endif
    }


    void ExitCallback(string param)
    {
		Debug.Log("----- SDKPlatform91 ExitCallback param:"+param);
		//SwitchAccountManager.mInstance.logout();
		PlayerInfo.getInstance().isLogout=true;
		Application.LoadLevel("loading");
        //GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_LOADING);
    }
	
	void LoginCallback(string param)
	{
		Debug.Log("----- SDKPlatform91 LoginCallback");
		LoginUI_new.mInstance.LoginCallback91(param);
	}
	
	void CloseCallback(string param)
	{
		Debug.Log("----- SDKPlatform91 CloseCallback");
	}
	
	//ui but
	void OnSetKy()
	{
		SdkSet();
	}

    public static Dictionary<string, string> string2Dic(string str)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] ss = str.Split('&');
        foreach (string s in ss)
        {
            int temp = s.IndexOf('=');
            if (temp > 0 && temp < s.Length - 1)
                dic.Add(s.Substring(0, temp), s.Substring(temp + 1, s.Length - temp - 1));
        }
        return dic;
    }
}
