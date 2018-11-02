using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMarket : MonoBehaviour
{
    public Button btn_back;
    public Button[] btn_AllType;
    public Transform gridTrans;
    public GameObject grid;
    public List<UIMarketGrid> grids;

    private Dictionary<RoomType, List<BuildingData>> dic;
    private int currentType = 1;


    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.ChickBuild, UpdateType);

        Init();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ChickBuild, UpdateType);
    }

    private void Init()
    {
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBtnType);
        }

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
    }

    public void ChickBtnType()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            if (btn_AllType[i].gameObject == go)
            {
                currentType = i + 1;
            }
        }
        UpdateType();
    }

    public void ClosePage()
    {
        UIMain.instance.CloseSomeUI(true);
        this.gameObject.SetActive(false);
    }

    private void UpdateType()
    {
        RoomType type = (RoomType)currentType;
        List<UIMarketGrid> fullGrids = new List<UIMarketGrid>();
        for (int i = 0; i < dic[type].Count; i++)
        {
            if (grids.Count <= i)//如果格子数量不足 新建
            {
                GameObject go = Instantiate(grid, gridTrans) as GameObject;
                grids.Add(go.GetComponent<UIMarketGrid>());
            }
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
        for (int i = 0; i < fullGrids.Count; i++)
        {
            fullGrids[i].transform.SetSiblingIndex(fullGrids[i].transform.parent.transform.childCount - 1);
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
