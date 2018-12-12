using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayMoneyButton : MonoBehaviour {

    public int isPayNoAds = 0;

    public void PaySucceed()
    {
        if (PlayerPrefs.GetInt("isPay") == 1)
        {
            isPayNoAds = PlayerPrefs.GetInt("isPay");
        }
        else
        {
            isPayNoAds = 1;
            PlayerPrefs.SetInt("isPay", 1);
        }

        try
        {
            //UmengGameAnalytics.instance.UpdataEvent("cost_Click");
        }
        catch (System.Exception)
        {
            Debug.Log("cost_Click");
        }
    }

    public void PayDefeat()
    {
        if (PlayerPrefs.GetInt("isPay") != 1)
        {
            isPayNoAds = 0;
        }
    }
}
