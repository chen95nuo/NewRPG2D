using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle;
using Assets.Script.Battle.BattleData;
using System;

public class UILessonInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_needNumb;
    public Text txt_Tip_1;

    public GameObject EnemyOBJ;
    public Transform enemyGridPoint;

    public UILessonRole[] roles;
    private List<UILessonEnemy> enemyGrids = new List<UILessonEnemy>();

    public GameObject itemGrid;
    public Transform itemPoint;
    public List<UILessonItemGrid> itemGrids = new List<UILessonItemGrid>();


    public Button btn_Role;
    public Button btn_Magic;
    public Button btn_Type;
    public Button btn_Start;
    public Button btn_Close;
    public Button btn_Back;

    public LessonRolePoint[] rolePoint = new LessonRolePoint[9];
    private HallRoleData[] rolePointData = new HallRoleData[9];
    private List<RoleDetailData> FightRoleData = new List<RoleDetailData>();
    private MapLevelData currentLesson;
    private void Awake()
    {
        btn_Role.onClick.AddListener(ChickRole);
        btn_Magic.onClick.AddListener(ChickMagic);
        btn_Type.onClick.AddListener(ChickType);
        btn_Start.onClick.AddListener(ChickStart);
        txt_Tip_1.text = "攻击";
        btn_Role.interactable = false;
        btn_Close.onClick.AddListener(ClosePage);
        btn_Back.onClick.AddListener(ClosePage);
    }

    private void ChickStart()
    {
        if (FightRoleData.Count > 0)
        {
            ChickPlayerInfo.instance.UpLoadAllRoom();
            HallRoleMgr.instance.SaveBabyData();
            LocalStartFight.instance.UpdateInfo(FightRoleData, currentLesson);
            return;
        }
        object st = "参与战斗的角色数量不足";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
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
        currentLesson = data;
        Debug.Log(currentLesson.Lesson);
        UpdateInfo(data);
    }

    private void UpdateInfo(MapLevelData data)
    {
        txt_Name.text = data.Name;
        txt_needNumb.text = data.NeedNum.ToString("#0");

        currentLesson = data;
        PlayerData playerData = GetPlayerData.Instance.GetData();



        ChickRolesGrids();
        ChickEnemysGrids(data);
        ChickRolePoint();
        ChickItemGrids(data);
    }

    private void ChickRolesGrids()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        LocalBuildingData barracksData = playerData.BarracksData;
        for (int i = 0; i < roles.Length; i++)
        {
            if (i < barracksData.buildingData.Param2)
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
                Debug.Log("角色" + data.CreateEnemyIds[i]);
                enemyGrids[index].gameObject.SetActive(true);
                enemyGrids[index].UpdateInfo(data.CreateEnemyIds[i]);
                index++;
            }
        }
        for (int i = index; i < enemyGrids.Count; i++)
        {
            enemyGrids[i].gameObject.SetActive(false);
        }
    }

    private void ChickItemGrids(MapLevelData data)
    {
        int index = 0;
        for (int i = 0; i < data.AwardItem.Length; i++)
        {
            if (itemGrids.Count <= index)
            {
                GameObject go = Instantiate(itemGrid, itemPoint) as GameObject;
                UILessonItemGrid gridData = go.GetComponent<UILessonItemGrid>();
                itemGrids.Add(gridData);
                go.SetActive(false);
            }
            if (data.AwardItem[i].ItemId != 0)
            {
                itemGrids[index].gameObject.SetActive(true);
                itemGrids[index].UpdateInfo(data.AwardItem[i]);
                index++;
            }
        }
        for (int i = 0; i < data.TreasureBoxIds.Length; i++)
        {
            if (itemGrids.Count <= index)
            {
                GameObject go = Instantiate(itemGrid, itemPoint) as GameObject;
                UILessonItemGrid gridData = go.GetComponent<UILessonItemGrid>();
                itemGrids.Add(gridData);
                go.SetActive(false);
            }
            if (data.TreasureBoxIds[i] != 0)
            {
                itemGrids[index].gameObject.SetActive(true);
                TreasureBox boxData = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(data.TreasureBoxIds[i]);
                itemGrids[index].UpdateInfo(boxData);
            }
        }
    }

    private void ChickRolePoint()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        LocalBuildingData barracksData = playerData.BarracksData;
        for (int i = 0; i < barracksData.roleData.Length; i++)
        {
            if (barracksData.roleData[i] != null)
            {
                ChickRolePointHelp(barracksData.roleData[i]);
            }
        }
    }
    private void ChickRolePointHelp(HallRoleData roleData)
    {
        int index = 9;
        int nowPoint = -1;
        RoleDetailData data = new RoleDetailData();
        for (int i = 0; i < rolePoint.Length; i++)
        {
            if (index > rolePoint[i].GetType(roleData.ProfessionType) && rolePointData[i] == null)
            {
                if (nowPoint != -1)
                    rolePointData[nowPoint] = null;
                index = rolePoint[i].GetType(roleData.ProfessionType);
                nowPoint = i;
                rolePointData[i] = roleData;
            }
        }
        data.Name = roleData.Name;
        data.Level = roleData.FightLevel;
        data.IconName = roleData.IconName;
        data.Profession = roleData.ProfessionType;
        data.sexType = roleData.sexType;
        data.EquipIdList = roleData.Equip;
        data.BornPositionType = (BornPositionTypeEnum)nowPoint;
        Debug.Log("位置 :" + nowPoint);
        Debug.Log("职业 :" + data.Profession);
        FightRoleData.Add(data);
    }

    public override void ClosePage()
    {
        for (int i = 0; i < rolePointData.Length; i++)
        {
            rolePointData[i] = null;
        }
        base.ClosePage();
    }
}
