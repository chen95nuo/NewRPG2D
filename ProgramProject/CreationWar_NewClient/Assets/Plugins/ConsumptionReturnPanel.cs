/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsumptionReturnPanel : MonoBehaviour 
{
    public UILabel activityContents;
    public UILabel oneTitle;
    public UILabel oneBlood;
    public UILabel twoTitle;
    public UILabel twoBlood;
    public UILabel threeTitle;
    public UILabel threeBlood;


    // Use this for initialization
    void Start()
    {

    }

    void SetInfo()
    {
        Dictionary<string, Dictionary<string, object>> info = DynamicActivity.instance.GetNoticeInfo();
        Dictionary<string, object> dic = null;
        if (info.TryGetValue("Consumption", out dic))
        {
            activityContents.text = (string)dic["description"];

            Dictionary<string, string> one = ((Dictionary<object, object>)dic["one"]).DicObjTo<string, string>();
            Dictionary<string, string> two = ((Dictionary<object, object>)dic["two"]).DicObjTo<string, string>();
            Dictionary<string, string> three = ((Dictionary<object, object>)dic["three"]).DicObjTo<string, string>();

            oneTitle.text = one["title"];
            oneBlood.text = one["blood"];

            twoTitle.text = two["title"];
            twoBlood.text = two["blood"];

            threeTitle.text = three["title"];
            threeBlood.text = three["blood"];
        }
    }
}
