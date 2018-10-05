using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using Assets.Script.Utility;

public class GameMain : MonoBehaviour
{

    public static GameMain Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Init();
    }

    private void Init()
    {
        UIPanelManager.instance.ShowPage<UIMain>();
        Invoke("test", 0.5f);
    }

    private void test()
    {
        ReadXmlNewMgr.instance.ReadXmlByType(XmlName.BuildingData, XmlName.Hall);
        Invoke("test_1", 0.5f);
    }

    private void test_1()
    {
        ChickPlayerInfo.instance.ChickBuilding();
    }
}
