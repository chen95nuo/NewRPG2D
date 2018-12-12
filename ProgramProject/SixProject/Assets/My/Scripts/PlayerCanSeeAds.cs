using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanSeeAds
{
    private static PlayerCanSeeAds instance;

    private PlayerCanSeeAds()
    {

    }

    public static PlayerCanSeeAds Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCanSeeAds();
            }
            return instance;
        }
    }

    private bool isCanWatchMove = true;
    public bool IsCanWatchMove {
        get {
            return isCanWatchMove;
        }
        set { 
            isCanWatchMove = value;
        }
    }

    public bool isHaveAdsController = false;


    void Start () {
		
	}
	

	void Update () {
		
	}
}
