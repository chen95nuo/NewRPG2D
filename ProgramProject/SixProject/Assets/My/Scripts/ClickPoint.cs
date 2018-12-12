using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPoint
{

    private static ClickPoint instance;

    private ClickPoint()
    {

    }

    public static ClickPoint Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ClickPoint();
            }
            return instance;
        }
    }

    public int InterstitialADNumStart = 3;

    public int InterstitialADNumPause = 3;

    public bool isCreatAward = false;

    public bool isStartShowAds = true;

    public bool isShowTop = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
