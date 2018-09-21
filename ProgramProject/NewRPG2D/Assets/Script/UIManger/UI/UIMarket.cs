using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMarket : MonoBehaviour
{
    public Button btn_back;
    public Button btn_production;
    public Button btn_training;
    public Button btn_support;
    public Button btn_treasures;
    public Button btn_miscellaneous;
    public Button btn_diamonds;
    public UIMain main;
    public Transform gridTrans;
    public GameObject grid;
    public List<UIMarketGrid> grids;

    private Dictionary<RoomType, List<BuildingData>> dic;

    private void Awake()
    {
        HallEventManager.instance.AddListener<RoomType>(HallEventDefineEnum.ChickBuild, UpdateType);

        Init();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<RoomType>(HallEventDefineEnum.ChickBuild, UpdateType);
    }

    private void Init()
    {
        btn_production.onClick.AddListener(ChickProduction);
        btn_training.onClick.AddListener(ChickTraining);
        btn_support.onClick.AddListener(ChickSupport);
        btn_treasures.onClick.AddListener(ChickTreasures);
        btn_miscellaneous.onClick.AddListener(ChickMiscellaneous);
        btn_diamonds.onClick.AddListener(ChickDiamonds);

        btn_back.onClick.AddListener(ClosePage);

        grids = new List<UIMarketGrid>();
        GameObject go = Instantiate(grid, gridTrans);
        grids.Add(go.GetComponent<UIMarketGrid>());

        GetDic();
        //Invoke("GetDic", 0.5f);
    }

    private void GetDic()
    {
        dic = BuildingDataMgr.instance.GetBuildingType();
        Debug.Log("获取字典");
        ChickProduction();
    }

    /// <summary>
    /// 筛选生产类
    /// </summary>
    public void ChickProduction()
    {
        UpdateType(RoomType.Production);
    }

    /// <summary>
    /// 筛选训练类
    /// </summary>
    public void ChickTraining()
    {
        UpdateType(RoomType.Training);
    }

    /// <summary>
    /// 筛选功能类
    /// </summary>
    public void ChickSupport()
    {
        UpdateType(RoomType.Support);
    }

    public void ChickTreasures() { }

    public void ChickMiscellaneous() { }

    public void ChickDiamonds() { }

    public void ClosePage()
    {
        main.CloseSomeUI(true);
        this.gameObject.SetActive(false);
    }

    private void UpdateType(RoomType type)
    {
        List<UIMarketGrid> fullGrids = new List<UIMarketGrid>();
        for (int i = 0; i < dic[type].Count; i++)
        {
            if (grids.Count <= i)//如果格子数量不足 新建
            {
                GameObject go = Instantiate(grids[grids.Count - 1].gameObject, gridTrans) as GameObject;
                grids.Add(go.GetComponent<UIMarketGrid>());
            }
            grids[i].market = this;
            grids[i].gameObject.SetActive(true);
            int[] index = ChickPlayerInfo.instance.GetBuildiDicInfo(dic[type][i]);
            bool isTrue = true;
            if (index[0] >= index[1])
            {
                isTrue = false;
                fullGrids.Add(grids[i]);
            }
            grids[i].UpdateBuilding(dic[type][i], index, isTrue);//当前格子更新信息
        }
        ClearGrids(dic[type].Count);//将多余的格子关闭
    }

    private void ClearGrids(int index)
    {
        for (int i = index; i < grids.Count; i++)
        {
            grids[i].gameObject.SetActive(false);
        }
    }
}
