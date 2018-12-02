using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System;

public class LanguageDataMgr : ItemCsvDataBaseMgr<LanguageDataMgr>
{
    public string redSt = "<color=#ee5151>{0}</color>";
    public string whiteText = "<color=#cccccc>{0}</color>";

    Dictionary<string, LanguageData> dic = new Dictionary<string, LanguageData>();

    protected override CsvEChartsType CurrentCsvName
    {
        get { return CsvEChartsType.LanguageData; }
    }

    public string GetString(string message)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            LanguageData data = CurrentItemData[i] as LanguageData;
            if (data.GetName == message)
            {
                return data.Chinese;
            }
        }
        return "";
    }

    public string GetUIString(string message)
    {
        return GetString("UI_" + message);
    }

    public string GetRoomTxtColor(BuildRoomName name)
    {
        string st = "";
        switch (name)
        {
            case BuildRoomName.Gold:
                st = "<color=#f5d835>";
                break;
            case BuildRoomName.Food:
                st = "<color=#eda160>";
                break;
            case BuildRoomName.Mana:
                st = "<color=#c415d0>";
                break;
            case BuildRoomName.Wood:
                st = "<color=#be7f27>";
                break;
            case BuildRoomName.Iron:
                st = "<color=#9fbdd7>";
                break;
            default:
                break;
        }
        return st;
    }

    public string GetRoomName(string name)
    {
        string st = "Room_" + name;
        st = GetString(st);
        return st;
    }
    public string GetDes(string name)
    {
        string st = "Des_" + name;
        st = GetString(st);
        return st;
    }
    public string GetInfo(string name)
    {
        string st = "Info_" + name;
        st = GetString(st);
        return st;
    }
    public string GetEquipName(string name, int type)
    {
        string st = name + type;
        st = GetString(st);
        return st;
    }
}
