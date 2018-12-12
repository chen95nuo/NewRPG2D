using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAds : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
        if (ClickPoint.Instance.isStartShowAds)
        {
            try
            {
                //GameObject.Find("AdsController").GetComponent<TestInterstitialAd>().HandleShowAdButtonClick();
                ClickPoint.Instance.isStartShowAds = false;
            }
            catch (System.Exception)
            {


            }

        }
    }
	
}
