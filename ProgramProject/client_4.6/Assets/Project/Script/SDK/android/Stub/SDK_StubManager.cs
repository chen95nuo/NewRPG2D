using System.Collections.Generic;
using UnityEngine;



public class SDK_StubManager : MonoBehaviour
{
    public static SDK_StubManager g_inst = null;

    private void Start()
    {
        if (g_inst != null)
        {
            Destroy(gameObject);
            return;
        }
        g_inst = this;
        DontDestroyOnLoad(gameObject);

        SDK_Google_Manager.ins.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAq1n1N8ZFe+B7PhdobZNVHc2Ve9AhuvftbD74YODh6V0xeZFGkNE42gZXbsel7rKQaOoF8BXJj4CPZ2HZKxuD4lZJ4q8pe170TNabJThC3yaUCWqmg0JJkRpM8uPVhrg/+4LW10MbJE33KbT9tVnDBLdhQJ2/1gsVFZDFZrKdNu/5OMZelBoVIG42NZmjEeLzIn5eqIOyMA7iDj+YYpB58MwCbCnEAJYPE7l65XLut1Uk7+todaSJVhTvLMxFeYlNV57a90Bj+AAcS0kzFa7ko1ErmuARvEXvWT/Mi1mDRu0qzQVCAUwQhvIDmTbkbDCv1UlDvRGLX7Ja8ZsyczUuWwIDAQAB");
    }

    public static void MsyRevicer(string json)
    {
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
                        //if (iabPurchaseCB != null)
                        //{
                        //    iabPurchaseCB(new object[3] { true, (string)cache["desc"], (string)cache["sign"] });
                        //}

                    }
                    else
                    {
                        //不可使用
                        //if (iabPurchaseCB != null)
                        //{
                        //    iabPurchaseCB(new object[3] { false, "", "" });
                        //}

                    }
                }
                break;

            case 3: //消耗成功
                {
                    //OnConsumeFinishedListener
                    if (cache.ret)
                    {
                        //可使用
                        //if (iabConsumeCB != null)
                        //{
                        //    iabConsumeCB(new object[3] { true, (string)cache["desc"], (string)cache["sign"] });
                        //}

                    }
                    else
                    {
                        //不可使用
                        //if (iabConsumeCB != null)
                        //{
                        //    iabConsumeCB(new object[3] { false, "", "" });
                        //}
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
}
