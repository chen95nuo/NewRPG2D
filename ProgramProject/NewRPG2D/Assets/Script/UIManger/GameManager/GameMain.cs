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

        Test();
    }
    private void Test()
    {

    }

    private void Init()
    {
        BuildingManager.instance.InitMainHallRoom();//写入大厅等级和战斗室等级
        MainCastle.instance.Init();//建造墙体
        //EditCastle.instance.Init();
        HallRoleMgr.instance.InitRoleData();//刷新所有角色
        BuildingManager.instance.InitBuildingData();//刷新房间并安排角色
                                                    //ItemInfoManager.instance
    }

    private IEnumerator ChickInfo()
    {
        yield return new WaitUntil(() => GameHelper.instance.ServerInfo);
        Debug.Log("获取到服务器数据了");
        Init();
    }
}
