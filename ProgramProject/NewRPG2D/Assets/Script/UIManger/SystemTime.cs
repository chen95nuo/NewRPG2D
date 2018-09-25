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

    public void TimeNormalized(int time, Text text)
    {
        if (time < 0)
        {
            time = 0;
        }
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);
        text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
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
