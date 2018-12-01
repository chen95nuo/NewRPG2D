using Assets.Script.Timer;
using Assets.Script.UIManger;
using Assets.Script.Utility;
using System.Collections;
using UnityEngine;

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
    }

    private void Start()
    {
        UIPanelManager.instance.ShowPage<UILoading>();
        StartCoroutine(ChickInfo());
    }

    private void Init()
    {
        BuildingManager.instance.SetMainHallRoom();//写入大厅等级和战斗室等级
        MainCastle.instance.Init();//建造墙体
        //EditCastle.instance.Init();
        HallRoleMgr.instance.ResetRoleData();//刷新所有角色
        BuildingManager.instance.ResetBuildingData();//刷新房间并安排角色
        Debug.Log(MainCastle.instance.buildPoint);
    }

    private IEnumerator ChickInfo()
    {
        yield return new WaitUntil(() => GameHelper.instance.ServerInfo);
        Debug.Log("获取到服务器数据了");
        Init();
    }
}
