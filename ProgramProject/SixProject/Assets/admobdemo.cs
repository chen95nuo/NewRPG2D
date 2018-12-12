using UnityEngine;
using admob;
public class admobdemo
{
    private static admobdemo instance;
    public static admobdemo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new admobdemo();
                instance.initAdmob();
            }
            return instance;
        }
    }

    Admob ad;
    string appID = "";
    string bannerID = "";
    string interstitialID = "";
    string rewardedID = "";
    string nativeBannerID = "";

    void initAdmob()
    {
        Debug.Log("初始化 Admob-------------");
#if UNITY_IOS
        		 appID="ca-app-pub-3940256099942544~1458002511";
				 bannerID="ca-app-pub-3940256099942544/2934735716";
				 interstitialID="ca-app-pub-3940256099942544/4411468910";
				 videoID="ca-app-pub-3940256099942544/1712485313";
				 nativeBannerID = "ca-app-pub-3940256099942544/3986624511";
#elif UNITY_ANDROID
        appID = "ca-app-pub-2785934198997173~4354683412";
        //bannerID = "ca-app-pub-3940256099942544/6300978111";
        //interstitialID = "ca-app-pub-3940256099942544/1033173712";
        rewardedID = "ca-app-pub-3940256099942544/5224354917";
        //nativeBannerID = "ca-app-pub-3940256099942544/2247696110";
#endif
        AdProperties adProperties = new AdProperties();
        adProperties.isTesting = true;

        ad = Admob.Instance();
        ad.bannerEventHandler += onBannerEvent;
        ad.interstitialEventHandler += onInterstitialEvent;
        ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        ad.nativeBannerEventHandler += onNativeBannerEvent;
        ad.initSDK(appID, adProperties);//reqired,adProperties can been null

        ad.loadRewardedVideo(rewardedID);
    }

    public void ShowRewarded()
    {
        Debug.Log("开始播放奖励视频");
        if (ad.isRewardedVideoReady())
        {
            Debug.Log("加载完成 播放");
            ad.showRewardedVideo();
        }
        else
        {
            Debug.Log("加载未完成");
            ad.loadRewardedVideo(rewardedID);
        }
    }
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(120, 0, 100, 60), "showInterstitial"))
    //    {
    //        Debug.Log("touch 间质 inst button -------------");
    //        if (ad.isInterstitialReady())
    //        {
    //            ad.showInterstitial();
    //        }
    //        else
    //        {
    //            ad.loadInterstitial(interstitialID);
    //        }
    //    }
    //    if (GUI.Button(new Rect(240, 0, 100, 60), "showRewardVideo"))
    //    {
    //        Debug.Log("touch 奖励视频 video button -------------");
    //        if (ad.isRewardedVideoReady())
    //        {
    //            ad.showRewardedVideo();
    //        }
    //        else
    //        {
    //            ad.loadRewardedVideo(videoID);
    //        }
    //    }
    //    if (GUI.Button(new Rect(0, 100, 100, 60), "showbanner"))
    //    {
    //        Debug.Log("横幅");
    //        Admob.Instance().showBannerRelative(bannerID, AdSize.SMART_BANNER, AdPosition.BOTTOM_CENTER);
    //    }
    //    if (GUI.Button(new Rect(120, 100, 100, 60), "showbannerABS"))
    //    {
    //        Debug.Log("横幅ABS");
    //        Admob.Instance().showBannerAbsolute(bannerID, AdSize.BANNER, 20, 220, "mybanner");
    //    }
    //    if (GUI.Button(new Rect(240, 100, 100, 60), "removebanner"))
    //    {
    //        Debug.Log("删除横幅");
    //        Admob.Instance().removeBanner();
    //        Admob.Instance().removeBanner("mybanner");
    //    }
    //}
    void onInterstitialEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
        if (eventName == AdmobEvent.onAdLoaded)
        {
            Admob.Instance().showInterstitial();
        }
    }
    void onBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
    }
    void onRewardedVideoEvent(string eventName, string msg)
    {
        Debug.Log("handler onRewardedVideoEvent---" + eventName + "  rewarded: " + msg);
    }
    void onNativeBannerEvent(string eventName, string msg)
    {
        Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
    }
}
