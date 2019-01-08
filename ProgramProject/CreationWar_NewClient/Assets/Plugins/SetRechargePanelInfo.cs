/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class SetRechargePanelInfo : MonoBehaviour 
{
    public static SetRechargePanelInfo instance;

    public UILabel lbl100;
    public UILabel lbl200;
    public UILabel lbl500;
    public UILabel lbl1200;
    public UILabel lblInfo1;
    public UILabel lblInfo2;
    public UILabel lblNum;

    void Awake()
    {
        if(null == instance)
        {
            instance = this;
        }
    }

    public void SetInfo(string str1, string str2, string str3, string str4, string str5, string str6, string str7)
    {
        lbl100.text = str1.Replace("\\n", "\n"); ;
        lbl200.text = str2.Replace("\\n", "\n");
        lbl500.text = str3.Replace("\\n", "\n");
        lbl1200.text = str4.Replace("\\n", "\n"); ;
        lblInfo1.text = str5;
        lblInfo2.text = str6;
        lblNum.text = str7;
    }
}
