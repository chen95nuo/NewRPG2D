using UnityEngine;
using System.Collections;
//==云点友游==//
public class SDK_CPYY_manager : MonoBehaviour {
	
	public const int LoginId=1;
	public const int RegisterId=2;
	public const int GetRechargeLogId=3;
	public const int ModifyPassId=4;
	public const int BuyId=5;
	public const int IsLoginId=6;
	public const int LogoutId=7;
	public const int GetPayLogId=8;
	public const int GetUserInfoId=9;
	public const int GameNotifyId=10;
	public const int AddScoreId=11;
	
	private const string pay_notify_url="http://114.215.183.102:8080/card_server_pay/pay.htm?action=pay_cypp";
	
	//public static string msg="msg:";
	// Use this for initialization
	void Start () {
		Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
//	void OnGUI()
//	{
//		GUI.Label(new Rect(100,200,400,400),msg);
//	}
	
	public static void sdk_call(int id)
	{
		
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
			using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
			{
				using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
				{
				jo.Call("sdk_call",new string[]{id+""});
				}
			}
		#endif	
		
	}
	
	public static void sdk_call_pay(string amount,string order_number,string extra)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
			using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
			{
				using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
				{
				jo.Call("sdk_call_pay",new string[]{amount,order_number,extra,pay_notify_url});
				}
			}
		#endif	
		
	}
	
	public static void sdk_call_gameNotify(string type,string level,string info)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
			jo.Call("sdk_call_gameNotify",new string[]{type,level,info});
			}
		}
		#endif	
	}
	
	//==install apk==//
	public static void installApk(string fileURL)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
			jo.Call("installApk",new string[]{fileURL});
			}
		}
		#endif	
	}
	
	public void loginCallBack(string json)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		//msg="msg==:"+json;
		CPYYLoginJson lj=JsonMapper.ToObject<CPYYLoginJson>(json);
		LoginUI_new.mInstance.sdk_login(lj.uid,"",lj.nickname);
		#endif	
	}
	
	public void payCallback(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		if(data.Equals("1"))
		{
			ToastWindow.mInstance.showText(TextsData.getData(399).chinese);
			ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ,"ChargePanel") as ChargePanel;
			charge.UpdateChargePanelRequest();
		}
		else
		{
			ToastWindow.mInstance.showText(TextsData.getData(400).chinese);
		}
		#endif	
	}
	
}
