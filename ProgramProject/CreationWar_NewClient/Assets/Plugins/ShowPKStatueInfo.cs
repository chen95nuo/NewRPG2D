/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class ShowPKStatueInfo : MonoBehaviour 
{
    public static ShowPKStatueInfo instance;
    public GameObject[] characterModels;// 角色模型数组，第一个为战士，第二个为游侠，第三个为法师
    public UILabel lblRoleInfo;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(false);
        }

        InRoom.GetInRoomInstantiate().getfirstForceValue();
    }

    public void ShowRoleInfo(string roleInfo)
    {
        if (string.IsNullOrEmpty(roleInfo))
        {
            lblRoleInfo.text = StaticLoc.Loc.Get("meg0144");
        }
        else
        {
            string[] info = roleInfo.Split(',');
            string name = info[0];
            string level = info[1];
            string guildName = info[2];
            string proID = info[3];
            string title = info[4];
            //lblRoleInfo.text = string.Format("{0}{1}\n{2}{3}\n{4}{5}\n{6}{7}", StaticLoc.Loc.Get("meg0145"), title, StaticLoc.Loc.Get("meg0146"), name, StaticLoc.Loc.Get("meg0147"), level, StaticLoc.Loc.Get("meg0148"), guildName);
            lblRoleInfo.text = string.Format("{0}{1}\n{2}{3}\n{4}{5}", StaticLoc.Loc.Get("meg0146"), name, StaticLoc.Loc.Get("meg0147"), level, StaticLoc.Loc.Get("meg0148"), guildName);

            characterModels[int.Parse(proID) - 1].SetActive(true);
        }
    }
}
