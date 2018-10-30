using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class GameMain : MonoBehaviour
{

    public static GameMain Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Init();
    }

    public void Init()
    {
        UIPanelManager.instance.ShowPage<UILoading>();
        Invoke("test", 0.5f);
    }

    private void test()
    {
        ReadXmlNewMgr.instance.ReadXmlByType(XmlName.BuildingData, XmlName.Hall);
        Invoke("test_1", 0.5f);
        Invoke("test_2", 1.0f);
    }

    private void test_1()
    {
        ChickPlayerInfo.instance.ChickBuilding();
    }

    private void test_2()
    {
        LocalServer.instance.StartInit();
        for (int i = 1; i < 26; i++)
        {
            int index = 10000 + i;
            EquipmentMgr.instance.CreateNewEquipment(index);
        }
    }
}
