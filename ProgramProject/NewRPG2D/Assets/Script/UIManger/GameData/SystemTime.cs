using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemTime : MonoBehaviour
{
    public static SystemTime insatnce = null;


    public DateTime startTime;
    public float onlineTime;

    private float currentTime;

    private void Awake()
    {
        insatnce = this;
        startTime = DateTime.Now;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 1)
        {
            currentTime = 0;
            onlineTime += 1;
        }
    }

    public DateTime GetTime()
    {
        return startTime.AddSeconds(onlineTime);
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
