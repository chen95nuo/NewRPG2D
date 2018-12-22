using UnityEngine;
using System.Collections;

public class SDK_GCStubManager : MonoBehaviour{
	
	// Use this for initialization
	void Start () {
		if(PlayerInfo.getInstance().isLogout == true)
		{
			Destroy(gameObject);
			return;
		}
		Object.DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}
	
//	void OnGUI()
//	{
//		GUI.Label(new Rect(100,100,400,400),msg);
//		GUI.Label(new Rect(100,200,400,400),msg2);
//	}
	
	/**
	 * 设置注销回调
	 * lt@2014-6-13 上午10:34:08
	 */
	public static void sdk_setLogoutCallback()
	{
		
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_setLogoutCallback");
			}
		}
		#endif	
		
	}
	/**
	 * 设置账户切换回调
	 * lt@2014-6-13 上午10:35:09
	 */
	public static void sdk_setAccountSwitchCallback()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_setAccountSwitchCallback");
			}
		}
		#endif	
	}
	/**
	 * 登录
	 * lt@2014-6-13 上午10:37:04
	 */
	public static void sdk_startLogin()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_startLogin");
			}
		}
		#endif	
	}
	/**
	 * 登录统计
	 * lt@2014-6-13 上午10:38:20
	 * @param serverMark 服务器标识
	 */
	public static void sdk_startGameServerLogin(string serverMark)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
			jo.Call("sdk_startGameServerLogin",new string[]{serverMark});
			}
		}
		#endif	
	}
	/**
	 * 提交扩展数据(游戏中为用户成功登录游戏角色时，提交该“游戏登录角色”数据)
	 * lt@2014-6-13 上午11:10:44
	 * @param type loginGameRole
	 * @param data json格式
	 * {
		"roleId":"string", //必填
		"roleName":"string", //必填
		"roleLevel":"string", //必填
		"zoneId":int, //必填
		"zoneName":"string", //可选
		}
	 */
	public static void sdk_submitExtendData(string type,string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
			jo.Call("sdk_submitExtendData",new string[]{type,data});
			}
		}
		#endif	
	}
	/**
	 * 支付
	 * @param amount 消费金额
	 * @param product 商品名称
	 * @param serverMark 服务器标识
	 * @param extra 附加参数
	 */
	public static void sdk_startPayment(int amount,string product,string serverMark,string extra)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
			jo.Call("sdk_startPayment",new string[]{amount+"",product,serverMark,extra});
			}
		}
		#endif	
	}
	/**
	 * 显示悬浮窗
	 * lt@2014-6-13 上午10:53:04
	 */
	public static void sdk_showFloatingView()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_showFloatingView");
			}
		}
		#endif	
	}
	/**
	 * 隐藏悬浮窗
	 * lt@2014-6-13 上午10:53:38
	 */
	public static void sdk_hideFloatingView()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_hideFloatingView");
			}
		}
		#endif	
	}
	/**
	 * 打开论坛
	 * lt@2014-6-13 上午10:55:20
	 */
	public void sdk_openForum()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_openForum");
			}
		}
		#endif	
	}
	/**
	 * 打开用户中心
	 * lt@2014-6-13 上午10:55:50
	 */
	public static void sdk_openMemberCenter()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_openMemberCenter");
			}
		}
		#endif	
	}
	/**
	 * 打开退出界面
	 * lt@2014-6-13 上午10:56:53
	 */
	public static void sdk_openExitPopup()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_openExitPopup");
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
	
	public static void sdk_startUpdate(string v)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
			jo.Call("sdk_startUpdate",new string[]{v});
			}
		}
		#endif	
	}
	
	
	//=====================================CallBack=============================================//
	
	public static void sdk_getVersionName()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_getVersionName");
			}
		}
		#endif	
	}
	
	public void loginCallBack(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		Debug.Log(data);
		GCLoginJson gclj=JsonMapper.ToObject<GCLoginJson>(data);
		if(TalkingDataManager.channelId.Equals("870"))
		{
			LoginUI_new.mInstance.sdk_tangguo_login(gclj.userId,"",gclj.userName,gclj.tangguoserver);
		}
		else
		{
			LoginUI_new.mInstance.sdk_login(gclj.userId,"",gclj.userName);
		}
		#endif	
	}
	
	public void payCallback(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		if(data.Equals("1"))
		{
			//ToastWindow.mInstance.showText(TextsData.getData(399).chinese);
			ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ,"ChargePanel") as ChargePanel;
			charge.UpdateChargePanelRequest();
		}
		else
		{
			ToastWindow.mInstance.showText(TextsData.getData(400).chinese);
		}
		#endif	
	}
	
	public void getVersionNameCallBack(string version)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		LoadingControl.instance.setVersion(version);
		#endif	
	}
	
	public void updateCallback(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		if(string.IsNullOrEmpty(data))
		{
			return;
		}
		UpdateJson uj=JsonMapper.ToObject<UpdateJson>(data);
		//LoadingControl.instance.downloadPackage(uj);
		if(uj.flag!=0)
		{
			Application.OpenURL(uj.url);
		}
		#endif	
	}
	
	public void logoutCallback(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		SwitchAccountManager.mInstance.logout();
		#endif
	}
		
	public void accountSwitchCallback(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		//loginCallBack(data);
			
		if(string.IsNullOrEmpty(data))
		{
			return;
		}
		GCLoginJson gclj=JsonMapper.ToObject<GCLoginJson>(data);
		string username="GC_"+gclj.userId;
		string password="";
		string nickname=gclj.userName;
		string platform=Constant.OS_ANDROID;
		SwitchAccountManager.mInstance.switchAccount(username,password,platform,nickname);
		#endif
	}
	
	public void exitProgram(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		Application.Quit();
		#endif	
	}
	
}
