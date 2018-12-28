using UnityEngine;
using System.Collections;

public class SDK_Google_Manager
{
    public const int LoginId = 1;
    private const string pay_notify_url = "http://118.25.209.26:8080/card_server_pay/pay.htm?action=googelinpay";

    private AndroidJavaObject mIABHelperObj = null;
    private static SDK_Google_Manager g_inst;
    public static SDK_Google_Manager ins
    {
        get
        {
            if (g_inst == null)
            {
                g_inst = new SDK_Google_Manager();
            }

            return g_inst;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void init(string base64EncodedPublicKey)
    {
#if ((UNITY_ANDROID && !UNITY_EDITOR))
        dispose();
        mIABHelperObj = new AndroidJavaObject("com.blingstorm.arpg.iabWrapper", new object[2] { base64EncodedPublicKey, "StubSDK" });
#endif
    }

    public void dispose()
    {
        if (mIABHelperObj != null)
        {
            mIABHelperObj.Call("dispose");
            mIABHelperObj.Dispose();
            mIABHelperObj = null;
        }
    }

    public void sdk_call_pay(string strSKU, string reqCode, string payload)
    {
        if (mIABHelperObj != null)
        {
            mIABHelperObj.Call("purchase", new object[3] { strSKU, reqCode, payload });
        }
    }

    public void consume_inapp(string strPurchaseJsonInfo, string strSignature)
    {
        if (mIABHelperObj != null)
        {
            mIABHelperObj.Call("consume", new object[3] { "inapp", strPurchaseJsonInfo, strSignature });
        }
    }



}
