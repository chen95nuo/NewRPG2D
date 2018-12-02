using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicLabLevelUp : UILevelUp
{
    public Text txt_Tip;
    public Text txt_DownTip;
    public Transform gridPoint;
    public GameObject grid;
    public List<UIMagicGrid> magicGrids = new List<UIMagicGrid>();

    protected override void Init(LocalBuildingData room)
    {
        base.Init(room);
        int nextID = room.buildingData.NextLevelID;
        BuildingData data = BuildingDataMgr.instance.GetDataByItemId<BuildingData>(nextID);
        int nowUnLock = (int)room.buildingData.Param1 - 1;
        int nextUnlock = (int)data.Param1;
        for (int i = nowUnLock; i < nextUnlock; i++)
        {
            if (magicGrids.Count <= i)
            {
                InstantiateGrids();
            }
            magicGrids[i].gameObject.SetActive(true);
            MagicData magicData = MagicDataMgr.instance.GetMagic((MagicName)i, 1);
            magicGrids[i].UpdateInfo(magicData, MagicGridType.Nothing);
        }
        for (int i = nextUnlock; i < magicGrids.Count; i++)
        {
            magicGrids[i].gameObject.SetActive(false);
        }
    }

    private void InstantiateGrids()
    {
        GameObject go = Instantiate(grid, gridPoint) as GameObject;
        UIMagicGrid magic = go.GetComponent<UIMagicGrid>();
        magicGrids.Add(magic);
    }
}
