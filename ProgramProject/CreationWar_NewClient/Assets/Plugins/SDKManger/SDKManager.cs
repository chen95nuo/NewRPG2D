using UnityEngine;
using System.Collections;

public class SDKManager : MonoBehaviour {
#if SDK_ITOOLS
	private static int appid = 624;// TODO: 设置APPID
	private static string appkey = "C05D9B52C36536807CD0444396034FB0";// TODO: 设置APPKEY
#elif SDK_XY
	private static string appid = "100002364";// TODO: 设置APPID
	private static string appkey = "kCqQYHQH2pYLuC0L9X8QSRofCnNbNOhC";// TODO: 设置APPKEY
#elif SDK_I4
	private static int appid = 341;// TODO: 设置APPID
	private static string appkey = "8148112b941e470f8bcbe72532e82091";// TODO: 设置APPKEY
#endif
	void Start () {

#if UNITY_IPHONE
#if SDK_AZ
#elif SDK_JYIOS
		SdkConector.NdInit ();
		SdkConector.NdSetUnityReceiver (this.transform.name);
#elif SDK_ITOOLS
		if(ItoolsSdkControl.isInit==false){
			ItoolsSdkControl.ItoolSDKInit (appid,appkey);
			ItoolsSdkControl.isInit=true;
		}
		ItoolsSdkControl.ItoolSDKSetUnityReceiver(this.transform.name);
#elif SDK_KUAIYONG
		if(KYSdkControl.isInit==false){
			KYSdkControl.KYSDKInit ();
			KYSdkControl.isInit=true;
		}
		KYSdkControl.KYSDKSetUnityReceiver(this.transform.name);
#elif SDK_XY
		if(XYSDKControl.isInit==false){
			XYSDKControl.XYSDKInit (appid,appkey);
			XYSDKControl.isInit=true;
		}
		XYSDKControl.XYSDKSetUnityReceiver(this.transform.name);
#elif SDK_I4
		ASSDKControl.ASSDKInit (appid, appkey);
		ASSDKControl.ASSDKSetUnityReceiver (this.transform.name);
#elif SDK_ZSY
		if(ZSYSDKControl.isInit == false){
			ZSYSDKControl.isInit = true;
			ZSYSDKControl.ZSYSDKInit ();
		}
		ZSYSDKControl.ZSYSDKSetUnityReceiver (this.transform.name);
#elif SDK_ZSYIOS
		if(ZSYSDKControl.isInit == false){
			ZSYSDKControl.isInit = true;
			ZSYSDKControl.ZSYSDKInit ();
		}
		ZSYSDKControl.ZSYSDKSetUnityReceiver (this.transform.name);
#elif SDK_PP
		if(PPSdkControl.isInit == false){
			PPSdkControl.isInit=true;
			PPSdkControl.PPSdkInit();
		}
		PPSdkControl.PPSdkSetUnityReceiver (this.transform.name);
#elif SDK_TONGBU
		if(TBSdkControl.isInit==false){
			TBSdkControl.isInit=true;
			TBSdkControl.TBSdkInit();
		}
		TBSdkControl.TBSdkSetUnityReceiver (this.transform.name);
#elif SDK_HM
		if(HMSdkControl.isInit==false){
			HMSdkControl.isInit=true;
			HMSdkControl.HMSdkInit();
		}
		HMSdkControl.HMSdkSetUnityReceiver (this.transform.name);
#endif
#endif

	}

	/// <summary>
	/// sdk的登陆接口
	/// </summary>
	public  void LoginSDK(){

#if UNITY_ANDROID
		//TD_info.setLogin();//TD接入进入游戏
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("doLYLogin");
#elif UNITY_IPHONE
//		Debug.Log("Unity--------LoginSDK-------------");
#if SDK_AZ
#elif SDK_JYIOS
		SdkConector.NdLogin();
#elif SDK_ITOOLS
		ItoolsSdkControl.ItoolSDKLogin();
#elif SDK_KUAIYONG
		KYSdkControl.KYSDKLogin ();
#elif SDK_XY
		XYSDKControl.XYSDKLogin ();
#elif SDK_I4
		ASSDKControl.ASSDKLogin ();
#elif SDK_ZSY
		ZSYSDKControl.ZSYSDKLogin ();
#elif SDK_ZSYIOS
		ZSYSDKControl.ZSYSDKLogin ();
#elif SDK_PP 
		PPSdkControl.PPSdkLogin ();
#elif SDK_TONGBU
		TBSdkControl.TBSdkLogin ();
#elif SDK_HM
		HMSdkControl.HMSdkLogin ();
#endif
#endif

	}
		/// <summary>
	/// sdk的注销接口
	/// </summary>
	public static  void LogoutSDK(){
#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("doLYLogout");
#endif
	}

	/// <summary>
	/// Zzsdk_passes 服务器回传给sdk所需要的参数authCode ；loginName  ；sdkUserId  ；access_token ；access_token )
	/// </summary>
	public static void zzsdk_passArguments(string  mValue)
	{
#if UNITY_ANDROID
//		Debug.Log ("----------------------zzsdk_passArguments:"+mValue);
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("onLoginInfoReceivd",mValue);
#endif
	}
	
	
	/// <summary>
	/// Zzsdk支付接口，调用需要传递参数,第一个参数CONFIG_GAME_SERVER_ID:游戏服务器ID，见前面的服务ID(SERVER_ID).第二个参数：自定义消息，交易成功后该消息将原样传递给游戏服务
	///器。可用于记录交易信息，如玩家角色名、所在区服等。长度上限为100 字符。第三个参数：交易金额，单位为分，如300 表示3 元； ０表示玩家可以充值任意金额

	/// </summary>
//	public static void zzsdk_pay(){
	public static void zzsdk_pay(string payInfo){
//		Debug.Log("Unity--------zzsdk_pay function is called!-------------" + payInfo);
#if UNITY_ANDROID
	AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	
	AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("doLYPay",payInfo);
#elif UNITY_IPHONE
#if SDK_JYIOS
		//订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串
		string[] payInfos = payInfo.Split(';');
//		float productPrice = float.Parse(payInfos[3]);
//		int productCount = int.Parse(payInfos[4]);
//		SdkConector.NdUniPayAsyn(payInfos[0], payInfos[1],payInfos[2],payInfos[3],payInfos[4],payInfos[5]);
#elif SDK_ITOOLS
		string[] itoolsPay = payInfo.Split(';');
		float propAmount = float.Parse (itoolsPay[1]);
//		ItoolsSdkControl.ItoolSDKpay (itoolsPay[0],propAmount,itoolsPay[2]);
#endif
#endif
}

	/// <summary>
	/// Zzsdk 切换账号接口
	/// </summary>
	public static void zzsdk_changge(){
		// <summary>
		/// Zzsdk的登陆接口，s等于“0”的时候为横屏其余为竖屏
		/// </summary>
		/// <param name="s">S.</param>
#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("changge_sdk");
#endif
	}
	
	/// <summary>
	///接受到安卓传过来的AuthorizationCode；
	/// </summary>
	/// <param name="s">S.</param>
	 void getAuthCode(string s){
		
		LoginSDKManager.CanSDKLogin=true;
		LoginSDKManager.SdkID=s;
		LoginSDKManager.SdkToken="123";
		if(TableRead.canSDKLogin)
		{
			LoginSDKManager.CanSDKLogin=false;
			TableRead.canSDKLogin=false;


			//YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginZSY (LoginSDKManager.SdkID,true);
//			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginLianYun(LoginSDKManager.SdkID,TableRead.strPageName,true),null));

#if SDK_UC
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginUC(LoginSDKManager.SdkID,true),null));
//			Debug.Log("uid=" + LoginSDKManager.SdkID);
#elif SDK_CMGE
			//登陆中手游sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginZSY(LoginSDKManager.SdkID,true),null));

#elif SDK_DOWN
			string[] codes = LoginSDKManager.SdkID.Split(';');
			string mid = codes[0];
			string token = codes[1];
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginDL(mid,token,true),null));

#elif SDK_QH
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginTSZ(LoginSDKManager.SdkID,LoginSDKManager.SdkToken,true),null));
#elif SDK_LENOVO
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginLenovo(LoginSDKManager.SdkID,true),null));

#else
			//登陆联运sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginLianYun(LoginSDKManager.SdkID,TableRead.strPageName,true),null));

#endif

		}
		
	}

	public static void getLoginResult(string result){
#if UNITY_IPHONE	
		Debug.Log ("-----getLoginResult-----"+result);
		LoginSDKManager.CanSDKLogin=true;
		LoginSDKManager.SdkID = result;
		LoginSDKManager.SdkToken="123";
		if (TableRead.canSDKLogin) {
			LoginSDKManager.CanSDKLogin = false;
			TableRead.canSDKLogin = false;
#if SDK_JYIOS
//		Debug.Log("login result = " + result);
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin91(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_ITOOLS
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginItools(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_KUAIYONG
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginKYSDK(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_XY
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginXY(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_I4
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginAS(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_ZSY
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginZSYIos(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_ZSYIOS
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginCMGE(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_PP
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginPP(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_TONGBU
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginTB(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_HM
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginHM(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#endif
		}
#endif
	}

/// <summary>
	///若sdk登陆失败，或者未输入账号,点击返回键时，会传入一个字符串“fail”；
	/// </summary>
	/// <param name="s">S.</param>
	void getFaiure(string s){
		
		
	}
	
	/// <summary>
	/// Gets the key info.
	/// 获取当前安装包的签名证书信息
	/// 将传过来的参数发送给服务器，用来验证正版
	/// </summary>
	/// <param name='code'>
	/// Code.
	/// code 传过来的证书信息
	/// </param>
	void getKeyInfo(string code){
		//游戏端在这里获取证书信息
//		YuanUnityPhoton.keyStore=code;
		StartCoroutine (RunKeyStore (code));
	}
	
	IEnumerator RunKeyStore( string mKeyCode)
	{
		while(true)
		{
			if(YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ServerConnected==true)
			{
				break;
			}
			yield return new WaitForSeconds(0.1f);
		}
		YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().LoginValidation (mKeyCode);
	}


}
