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
        Init();
    }

    private void Init()
    {
        UIPanelManager.instance.ShowPage<UILogin>();

        Invoke("test", 0.5f);
    }

    private void test()
    {
        ReadXmlNewMgr.instance.ReadXmlByType(XmlName.BuildingData, XmlName.Hall);
        ReadXmlNewMgr.instance.ReadXmlByType(XmlName.CreateEnemyData, XmlName.RolePropertyData, XmlTypeEnum.Battle);
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
