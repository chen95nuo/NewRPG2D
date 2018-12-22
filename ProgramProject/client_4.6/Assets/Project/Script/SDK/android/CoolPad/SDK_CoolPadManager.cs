using UnityEngine;
using System.Collections;

public class SDK_CoolPadManager : MonoBehaviour {
	
	private static string notifyurl="http://112.124.25.230:8080/card_server_pay/pay.htm?action=pay_coolpad";
	
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
	
	public static void sdk_login()
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
			using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
			{
				using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
				{
					jo.Call("sdk_login");
				}
			}
		#endif	
	}
	
	/**调用SDK进行支付
	 * @param waresid 商品编码
	 * @param price 单位为分
	 * @param exorderno 外部订单号
	 * @param extra 商户私有信息在做交易结果同步的时候会回传给开发者
	 */
	public static void sdk_startPay(string waresid,string price,string exorderno,string extra)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		using(AndroidJavaClass jc=new AndroidJavaClass("com.Begamer.Card.LauncherActivity"))
		{
			using(AndroidJavaObject jo=jc.GetStatic<AndroidJavaObject>("launcher"))
			{
				jo.Call("sdk_startPay",new string[]{waresid,price,exorderno,extra,notifyurl});
			}
		}
		#endif	
	}
	
	public void loginCallBack(string data)
	{
		#if ((UNITY_ANDROID && !UNITY_EDITOR))
		CoolpadLoginJson cj=JsonMapper.ToObject<CoolpadLoginJson>(data);
		LoginUI_new.mInstance.sdk_login(cj.userId,"",cj.userName);
		#endif	
	}
	
	public void payCallBack(string data)
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
