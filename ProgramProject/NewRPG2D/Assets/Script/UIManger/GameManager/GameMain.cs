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
    }

    private void Start()
    {
        UIPanelManager.instance.ShowPage<UIMain>();
        ReadXmlNewMgr.instance.ReadXmlByType(XmlName.BuildingData, XmlName.Hall);
        Invoke("test", 1f);
    }

    private void test()
    {
        ChickPlayerInfo.instance.ChickBuilding();
    }
}
