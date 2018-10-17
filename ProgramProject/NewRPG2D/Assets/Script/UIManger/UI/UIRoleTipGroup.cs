using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleTipGroup : TTUIPage
{
    public static UIRoleTipGroup instance;

    public GameObject tip;
    public Transform tipGridPoint;
    public Transform childTipPoint;
    private Canvas canvas;
    private List<UIRoleTrainGrid> trainGrid = new List<UIRoleTrainGrid>();
    private List<UIRoleChildGrid> ChildGrid = new List<UIRoleChildGrid>();

    private void Awake()
    {
        instance = this;
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
    }

    public void ShowIcon(HallRole role)
    {
        Transform ts = role.transform;
        for (int i = 0; i < trainGrid.Count; i++)
        {
            if (trainGrid[i].IsUse == false)
            {
                trainGrid[i].GetInfo(ts, canvas, role);
                return;
            }
        }
        GameObject go = Instantiate(tip, tipGridPoint) as GameObject;
        UIRoleTrainGrid data = go.GetComponent<UIRoleTrainGrid>();
        trainGrid.Add(data);
        data.GetInfo(ts, canvas, role);
    }

    public void ShowChildIcon(HallRole role)
    {
        Transform ts = role.transform;
        for (int i = 0; i < ChildGrid.Count; i++)
        {
            if (ChildGrid[i].IsUse == false)
            {
                ChildGrid[i].GetInfo(ts, canvas, role);
                return;
            }
        }
        GameObject go = Instantiate(tip, childTipPoint) as GameObject;
        UIRoleChildGrid data = go.GetComponent<UIRoleChildGrid>();
        ChildGrid.Add(data);
        data.GetInfo(ts, canvas, role);
    }

    public void CloseIcon(HallRoleData role)
    {
        for (int i = 0; i < trainGrid.Count; i++)
        {
            if (trainGrid[i].role.RoleData == role)
            {
                Debug.Log("找到了对应的升级图标 删除");
                trainGrid[i].RemoveInfo();
                return;
            }
        }
        Debug.LogError("没有找到对应的升级图标");
    }

    public void CloseChildIcon(HallRoleData role)
    {
        for (int i = 0; i < trainGrid.Count; i++)
        {
            if (trainGrid[i].role.RoleData == role)
            {
                Debug.Log("找到了对应的怀孕图标 删除");
                trainGrid[i].RemoveInfo();
                return;
            }
        }
        Debug.LogError("没有找到对应的怀孕图标");
    }
}
