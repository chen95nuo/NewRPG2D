using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMagicWorkShop : TTUIPage
{
    public static UIMagicWorkShop instance;
    public UIMagicUseGrid[] useGrid;
    public GameObject grid;
    public Transform readGridPoint;
    public List<UIMagicUseGrid> readGrids = new List<UIMagicUseGrid>();
    public List<UIMagicUseGrid> workGrids = new List<UIMagicUseGrid>();

    public UIMagicTip tip;
    private int empty = 0;

    private void Awake()
    {

    }

    public override void Show(object mData)
    {
        tip.RemovePage();

        base.Show(mData);
        UpdateInfo();
    }

    private void UpdateInfo()
    {

        MagicWorkShopHelper allMagic = MagicDataMgr.instance.AllMagicData;
        for (int i = 0; i < allMagic.useMagic.Length; i++)
        {
            if (allMagic.useMagic[i] != null)
            {
                useGrid[i].UpdateInfo(allMagic.useMagic[i], 1);
            }
            else
            {
                empty++;
                useGrid[i].UpdateInfo();
            }
        }
        int type = 0;
        if (empty == 0)
        {
            type = 2;
        }
        for (int i = 0; i < allMagic.readyMagic.Count; i++)
        {
            if (readGrids.Count == i)
            {
                InstanceGrid(readGrids);
            }
            readGrids[i].gameObject.SetActive(true);
            readGrids[i].UpdateInfo(allMagic.useMagic[i], type);
        }
        for (int i = allMagic.readyMagic.Count; i < readGrids.Count; i++)
        {
            readGrids[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < allMagic.workQueue.Length; i++)
        {
            if (workGrids.Count == i)
            {
                InstanceGrid(workGrids);
            }
            workGrids[i].gameObject.SetActive(true);
            workGrids[i].UpdateInfo(allMagic.workQueue[i], type);
        }
        for (int i = allMagic.readyMagic.Count; i < workGrids.Count; i++)
        {
            workGrids[i].gameObject.SetActive(false);
        }
    }

    public void ChickUseBtn(RealMagic grid)
    {
        tip.ShowPage(1, grid);
    }

    public void ShowInfo()
    {

    }

    public void InstanceGrid(List<UIMagicUseGrid> grids)
    {
        GameObject go = Instantiate(grid, readGridPoint) as GameObject;
        UIMagicUseGrid data = go.GetComponent<UIMagicUseGrid>();
        grids.Add(data);
    }
}