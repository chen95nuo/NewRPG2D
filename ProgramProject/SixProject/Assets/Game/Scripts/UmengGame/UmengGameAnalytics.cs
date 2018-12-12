using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Umeng;

public class UmengGameAnalytics
{
    private const string AppKey = "594f808b677baa0cb5001eaa";
    private const string ChannelId = "GooglePlay";
    private const string ConfigKey = "";
    // Use this for initialization
    private static UmengGameAnalytics _instance;

    public static UmengGameAnalytics instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UmengGameAnalytics();
            }
            return _instance;
        }
    }

    public void Init()
    {
        GA.StartWithAppKeyAndChannelId(AppKey, ChannelId);
        GA.EnableActivityDurationTrack(true);
        GA.SetLogEnabled(true);
        GA.SetLogEncryptEnabled(false);
        GA.PageBegin("mainGame");
        //GA.UpdateOnlineConfig();
        //GA.GetConfigParamForKey(ConfigKey);
    }

    public void UpdataEvent(string EventName)
    {
        GA.Event(EventName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PayNoADs()
    {
        GA.Pay(10,GA.PaySource.Paypal, 100);
        GA.Use("NoAds",1,100);
    }

    public void OnDestroy()
    {
        GA.PageEnd("mainGame");
    }
}
