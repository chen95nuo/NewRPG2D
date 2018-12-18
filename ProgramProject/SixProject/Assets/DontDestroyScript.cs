using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization

    void Start()
    {
        //UmengGameAnalytics.instance.Init();
        //AdsController.instance.Init();

        admobdemo.Instance.initAdmob();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDestroy()
    {
        //UmengGameAnalytics.instance.OnDestroy();
        //AdsController.instance.OnDestroy();
    }
}
