using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;
using System;

public class UILessonInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_needNumb;
    public Text txt_Tip_1;
    public MapLevelData currentLesson;

    public GameObject EnemyOBJ;
    public Transform enemyGridPoint;

    public UILessonRole[] roles;
    public List<UILessonEnemy> enemyGrids = new List<UILessonEnemy>();

    public Button btn_Role;
    public Button btn_Magic;
    public Button btn_Type;
    public Button btn_Start;

    private void Awake()
    {
        btn_Role.onClick.AddListener(ChickRole);
        btn_Magic.onClick.AddListener(ChickMagic);
        btn_Type.onClick.AddListener(ChickType);
        btn_Start.onClick.AddListener(ChickStart);
    }

    private void ChickStart()
    {

    }

    private void ChickType()
    {

    }

    private void ChickMagic()
    {
        object str = "技能系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(str);
    }

    private void ChickRole()
    {
        object str = "布阵系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(str);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        MapLevelData data = mData as MapLevelData;
        UpdateInfo(data);
    }

    private void UpdateInfo(MapLevelData data)
    {
        currentLesson = data;
        PlayerData playerData = GetPlayerData.Instance.GetData();



        ChickRolesGrids();
        ChickEnemysGrids(data);
    }

    private void ChickRolesGrids()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        LocalBuildingData barracksData = playerData.BarracksData;
        for (int i = 0; i < roles.Length; i++)
        {
            if (i <= barracksData.buildingData.Param2)
            {
                if (barracksData.roleData[i] != null)
                {
                    roles[i].UpdateInfo(barracksData.roleData[i]);
                }
                else
                {
                    roles[i].UpdateInfo();
                }
            }
            else
            {
                roles[i].UpdateLocked(i + 1);
            }
        }
    }

    private void ChickEnemysGrids(MapLevelData data)
    {
        int index = 0;
        for (int i = 0; i < data.CreateEnemyIds.Length; i++)
        {
            if (enemyGrids.Count <= index)
            {
                GameObject go = Instantiate(EnemyOBJ, enemyGridPoint) as GameObject;
                UILessonEnemy enemyGridData = go.GetComponent<UILessonEnemy>();
                enemyGrids.Add(enemyGridData);
                go.SetActive(false);
            }
            if (data.CreateEnemyIds[i] != 0)
            {
                enemyGrids[i].gameObject.SetActive(true);
                enemyGrids[i].UpdateInfo(data.CreateEnemyIds[i]);
            }
        }
    }

    private void ChickItemGrids(MapLevelData data)
    {
        for (int i = 0; i < data.AwardItem.Length; i++)
        {

        }
        for (int i = 0; i < data.TreasureBoxIds.Length; i++)
        {

        }
    }


}
