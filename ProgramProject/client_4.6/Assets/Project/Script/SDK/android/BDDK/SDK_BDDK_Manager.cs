using UnityEngine;
using System.Collections;

public class SDK_BDDK_Manager : MonoBehaviour {
	public const int LoginId=1;
	
	void Awake()
	{
		if(PlayerInfo.getInstance().isLogout == true)
		{
			Destroy(gameObject);
			return;
		}
	}

	// Use this for initialization
	void Start () {
		Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void sdk_call(int id)
	{
		
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
			using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.baidu.LauncherActivity"))
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
			
			using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.baidu.LauncherActivity"))
			{
				using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
				{
				jo.Call("sdk_call_pay",new string[]{amount,order_number,extra});
				}
			}
		#endif	
		
	}
	
	public static void sdk_exit()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
			using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.baidu.LauncherActivity"))
			{
				using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
				{
				jo.Call("sdk_exit");
				}
			}
		#endif	
		
	}
	
	public void loginCallBack(string json)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		Debug.Log("11111111111111111111111111111111111");
			
		//msg="msg==:"+json;
		BDDKLoginJson bdj = JsonMapper.ToObject<BDDKLoginJson>(json);
		LoginUI_new.mInstance.sdk_login(bdj.userId,"",bdj.userName);
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
	
	public void changeUID()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		SwitchAccountManager.mInstance.logout();
		#endif
	}
	
	
}
