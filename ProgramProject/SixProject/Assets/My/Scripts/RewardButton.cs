using MadFireOn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardButton : MonoBehaviour
{

    public static RewardButton instance;

    public int AddplayerScore;

    public int ShowAddScore;


    //public bool isMoveDown = true;


    public bool isButton = true;
    private void Awake()
    {
        instance = this;
        Debug.Log("isButton: " + isButton);
    }

    public void Rewaed()
    {
        ClickPoint.Instance.isCreatAward = true;


        isButton = true;

        try
        {
            //GameObject.Find("AnalyticsTools").GetComponent<TestUnityAds>().HandleShowButtonClick();
            admobdemo.Instance.ShowRewarded();

        }
        catch (System.Exception)
        {

            Debug.Log("初始化广告失败");
        }

        ClickPoint.Instance.isShowTop = false;


        try
        {
            GameObject.Find("RewardButton").gameObject.SetActive(false);

        }
        catch (System.Exception)
        {

            Debug.Log("Can not see Ads");
        }


        try
        {
            GameObject.Find("HandTap").gameObject.SetActive(false);

        }
        catch (System.Exception)
        {

            Debug.Log("HandTape false");
        }

    }

    public void AddPlayerScore()
    {
        try
        {
            UmengGameAnalytics.instance.UpdataEvent("reward_Click");
        }
        catch (System.Exception)
        {
            Debug.Log("reward_Click");
        }

        AddplayerScore = Random.Range(45, 50) * 5;
        Debug.Log("Down");

        GameManager.instance.AddScore(AddplayerScore);
    }




}
