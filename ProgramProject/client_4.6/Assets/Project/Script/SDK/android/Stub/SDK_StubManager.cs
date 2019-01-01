using System.Collections.Generic;
using UnityEngine;

public class SDK_StubManager : MonoBehaviour, ProcessResponse
{
    public static SDK_StubManager g_inst = null;
    Queue<GooglePayBackJson> orderList = new Queue<GooglePayBackJson>();
    private void Start()
    {
        if (g_inst != null)
        {
            Destroy(gameObject);
            return;
        }
        g_inst = this;
        DontDestroyOnLoad(gameObject);
        SDK_Google_Manager.ins.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgFfZHmpnWJY3N3HFcNKDxjnkWHOoMFbxEni+W5pDgMiDECXgo59zXAD0bwc1R/uodqhPJkKMBe9rGTfKbfxiy8xx0XxB1IRC2pb6sC/bMwOJw7Q+i/e8nje1FabOF0pKEm6cOcyL8ertPAT5kxmGj22GxS77PbI7T+0+QWWdv5a6SZUd4rNVDpGGau00XvU6n2HuJSCVHhZTirqzAikUfVi9v0WTIzq1zcjgs8HD8C1ipKZQzrDnss2djHlHJ2yzDZf4mzHgJy1lC4gfGjCOKEde1ROwyhJYxzi+c89sProkRgebYNBTOnbXO77Z+E4m/tFgLAhCwc7zWgehUW5cWwIDAQAB");
        // TalkingDataManager.ins.initTalkingData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //GooglePayJson payjson = new GooglePayJson("1", "1", "1", "1", "1");
            //PlayerInfo.getInstance().sendRequest(payjson, this);
        }
    }

    public void MsgReceiver(string json)
    {
        Debug.LogError(" json :" + json);
        GooglePayBackJson cache = JsonMapper.ToObject<GooglePayBackJson>(json);
        switch (cache.code)
        {
            case 0:
                {
                    //unknown
                    Debug.Log("Unity-iabWrappe :cannot parse cache[code]");

                }
                break;

            case 1: // init
                {
                    //OnIabSetupFinishedListener
                    if (cache.ret == true)
                    {
                        Debug.Log("iab successfully initialized");

                    }
                    else
                    {
                        Debug.LogError("failed to initialize iab");
                    }
                }
                break;

            case 2: //购买成功
                {

                    if (cache.ret)
                    {
                        //可使用
                        // SendOrder(cache);
                        Debug.LogError(" 支付成功 == " + cache.mOriginalJson);
                    }
                    else
                    {
                        Debug.LogError(" 支付失败 == " + cache.mOriginalJson);

                    }
                }
                break;

            case 3: //消耗成功
                {
                    //OnConsumeFinishedListener
                    if (cache.ret)
                    {
                        //可使用
                        Debug.LogError(" 消耗成功 == " + cache.mOriginalJson);
                        SendOrder(cache);

                    }
                    else
                    {
                        //不可使用
                        Debug.LogError(" 消耗失败 == " + cache.mOriginalJson);
                    }
                }
                break;
        }
    }

    public void payCallback(bool succeed)
    {

#if ((UNITY_ANDROID && !UNITY_EDITOR))
			
		if(succeed)
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

    private void SendOrder(GooglePayBackJson cache)
    {
        orderList.Enqueue(cache);
        GooglePayJson payjson = new GooglePayJson(cache.mOrderId, cache.mOriginalJson, cache.mSignature, cache.mPackageName, cache.mItemType);
        PlayerInfo.getInstance().sendRequest(payjson, this);
    }

    public void receiveResponse(string json)
    {
        PlayerInfo.getInstance().isShowConnectObj = false;
        Debug.Log("receiveResponse == " + json);
        if (orderList.Count > 0)
        {
            GooglePayBackJson cache = orderList.Dequeue();
            if (json.Contains("SUCCESS"))
            {
                ToastWindow.mInstance.showText(TextsData.getData(399).chinese);
                ChargePanel charge = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CZ, "ChargePanel") as ChargePanel;
                charge.UpdateChargePanelRequest();
            }
            else
            {
                SendOrder(cache);
                // ToastWindow.mInstance.showText(TextsData.getData(400).chinese);
            }
        }
    }
}
