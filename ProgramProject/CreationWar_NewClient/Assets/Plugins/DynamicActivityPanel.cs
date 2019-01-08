/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DynamicActivityType
{
    Growup = 0,//成长福利
    Continuous,//连续充值奖励
    Leveling,//冲级奖励
    Recharge,//充值奖励
    Consumption,//消费返额
    Returnmoney,//充值反额
}
public class DynamicActivityPanel : MonoBehaviour 
{
    public DynamicActivityType type;
    public UILabel activityContents;
    public UILabel oneTitle;
    public UILabel oneInfo;
    public UILabel oneBlood;
    public UILabel twoTitle;
    public UILabel twoInfo;
    public UILabel twoBlood;
    public UILabel threeTitle;
    public UILabel threeInfo;
    public UILabel threeBlood;


    // Use this for initialization
    void Start()
    {

    }

    public void SetInfo()
    {
        Dictionary<string, Dictionary<string, object>> info = DynamicActivity.instance.GetNoticeInfo();
        Dictionary<string, object> dic = null;
        if (info.TryGetValue(type.ToString(), out dic))
        {
            if (null != activityContents)
            {
                activityContents.text = (string)dic["description"];
            }

            Dictionary<string, string> one = ((Dictionary<object, object>)dic["one"]).DicObjTo<string, string>();
            Dictionary<string, string> two = ((Dictionary<object, object>)dic["two"]).DicObjTo<string, string>();
            Dictionary<string, string> three = ((Dictionary<object, object>)dic["three"]).DicObjTo<string, string>();

            if (null != oneTitle)
            {
                oneTitle.text = one["title"];
            }
            if (null != oneInfo)
            {
                oneInfo.text = one["info"].Replace('@', '\n');
            }
            if (null != oneBlood)
            { 
                oneBlood.text = one["blood"]; 
            }

            if (null != twoTitle)
            {
                twoTitle.text = two["title"];
            }
            if (null != twoInfo)
            {
                twoInfo.text = two["info"].Replace('@', '\n');
            }
            if (null != twoBlood)
            {
                twoBlood.text = two["blood"];
            }

            if (null != threeTitle)
            {
                threeTitle.text = three["title"];
            }
            if (null != threeInfo)
            {
                threeInfo.text = three["info"].Replace('@', '\n');
            }
            if (null != threeBlood)
            {
                threeBlood.text = three["blood"];
            }
        }
    }
}
