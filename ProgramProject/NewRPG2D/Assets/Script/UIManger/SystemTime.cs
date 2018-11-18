using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemTime : TSingleton<SystemTime>
{
    private DateTime startTime;
    private float onlineTime;

    private float currentTime;

    //private void Awake()
    //{
    //    insatnce = this;
    //    startTime = DateTime.Now;
    //}
    //private void Update()
    //{
    //    currentTime += Time.deltaTime;
    //    if (currentTime >= 1)
    //    {
    //        currentTime = 0;
    //        onlineTime += 1;
    //    }
    //}

    public DateTime GetTime()
    {
        return startTime.AddSeconds(onlineTime);
    }

    public string TimeNormalized(int time)
    {
        if (time < 0)
        {
            time = 0;
        }
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
    }

    public string TimeNormalizedOf(float Second, bool isMin = true)
    {
        int index = 0;
        int HowManySecond = (int)Second;
        if (HowManySecond == 0)
        {
            return "0";
        }
        if (isMin)
        {
            HowManySecond *= 60;
        }
        string ShowStr = "";
        if (HowManySecond >= (24 * 3600))
        {
            ShowStr += (HowManySecond / (24 * 3600)) + " 天 ";
            HowManySecond %= (24 * 3600);
            index++;
        }
        if (HowManySecond >= 3600)
        {
            ShowStr += (HowManySecond / 3600) + " 小时 ";
            HowManySecond %= 3600;
            index++;
        }
        if (HowManySecond >= 60 && index < 2)
        {
            ShowStr += (HowManySecond / 60) + " 分钟 ";
            index++;
        }
        if (HowManySecond > 0 && index < 2)
        {
            ShowStr = (HowManySecond % 60) == 0 ? ShowStr : ShowStr += (HowManySecond % 60) + "秒";
        }
        return ShowStr;
    }

    void OnApplicationPause(bool isPause)
    {

        if (isPause)
        {

        }
        else
        {
            currentTime = 0;
            onlineTime = 0;
            startTime = DateTime.Now;
        }
    }
}
