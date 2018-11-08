using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class WorkShopInfoData
{
    public BuildingData data;
    public int index;
    public WorkShopInfoData(BuildingData data, int index)
    {
        this.data = data;
        this.index = index;
    }
}

public class UIWorkShop : TTUIPage
{
    public UIWorkShopTypeGrid[] TypeGrid;
    public BuildingData currentData;

    private void Awake()
    {
        for (int i = 0; i < TypeGrid.Length; i++)
        {
            TypeGrid[i].btn_Type.onClick.AddListener(ChickClick);
        }
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        BuildingData data = mData as BuildingData;
        UpdateInfo(data);
    }

    private void UpdateInfo(BuildingData data)
    {
        currentData = data;
        if (data.RoomName == BuildRoomName.JewelleryWorkShop)
            TypeGrid[2].gameObject.SetActive(false);
        else
            TypeGrid[2].gameObject.SetActive(true);

        for (int i = 0; i < TypeGrid.Length; i++)
        {
            TypeGrid[i].UpdateInfo(data.RoomName, i);
        }
    }

    private void ChickClick()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < TypeGrid.Length; i++)
        {
            if (TypeGrid[i].gameObject == go)
            {
                WorkShopInfoData data = new WorkShopInfoData(currentData, i);
                UIPanelManager.instance.ShowPage<UIWorkShopInfo>(data);
            }
        }

    }
}
