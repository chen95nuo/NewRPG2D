using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleTipGroup : TTUIPage
{
    public static UIRoleTipGroup instance;

    public GameObject Trintip;
    public GameObject ChildTip;
    public GameObject BabyTip;
    public Transform tipGridPoint;
    public Transform childTipPoint;
    public Transform babyTipPoint;
    private Canvas canvas;
    private List<UIRoleTrainGrid> trainGrid = new List<UIRoleTrainGrid>();
    private List<UIRoleChildGrid> ChildGrid = new List<UIRoleChildGrid>();
    private List<UIRoleBabyTip> babyGrid = new List<UIRoleBabyTip>();
    private void Awake()
    {
        instance = this;
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
        transform.SetSiblingIndex(0);
    }

    public void ShowIcon(HallRole role, TrainType type)
    {
        Transform ts = role.TipPoint.transform;
        for (int i = 0; i < trainGrid.Count; i++)
        {
            if (trainGrid[i].IsUse == false)
            {
                trainGrid[i].GetInfo(ts, canvas, role,type);
                return;
            }
        }
        GameObject go = Instantiate(Trintip, tipGridPoint) as GameObject;
        UIRoleTrainGrid data = go.GetComponent<UIRoleTrainGrid>();
        trainGrid.Add(data);
        data.GetInfo(ts, canvas, role,type);
    }

    public void ShowChildIcon(HallRole role)
    {
        Transform ts = role.TipPoint.transform;
        for (int i = 0; i < ChildGrid.Count; i++)
        {
            if (ChildGrid[i].IsUse == false)
            {
                ChildGrid[i].GetInfo(ts, canvas, role);
                return;
            }
        }
        GameObject go = Instantiate(ChildTip, childTipPoint) as GameObject;
        UIRoleChildGrid data = go.GetComponent<UIRoleChildGrid>();
        ChildGrid.Add(data);
        data.GetInfo(ts, canvas, role);
    }

    public void ShowBabyIcon(HallRole role)
    {
        Transform ts = role.TipPoint.transform;
        for (int i = 0; i < babyGrid.Count; i++)
        {
            if (babyGrid[i].IsUse == false)
            {
                babyGrid[i].GetInfo(ts, canvas, role.currentBaby);
                return;
            }
        }
        GameObject go = Instantiate(BabyTip, babyTipPoint) as GameObject;
        UIRoleBabyTip data = go.GetComponent<UIRoleBabyTip>();
        babyGrid.Add(data);
        data.GetInfo(ts, canvas, role.currentBaby);
    }

    public void CloseIcon(int roleID)
    {
        for (int i = 0; i < trainGrid.Count; i++)
        {
            if (trainGrid[i].role.RoleData.id == roleID)
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
        for (int i = 0; i < ChildGrid.Count; i++)
        {
            if (ChildGrid[i].role.RoleData == role)
            {
                Debug.Log("找到了对应的怀孕图标 删除");
                ChildGrid[i].RemoveInfo();
                return;
            }
        }
        Debug.LogError("没有找到对应的怀孕图标");
    }

    public void CloseBabyIcon(RoleBabyData role)
    {
        for (int i = 0; i < babyGrid.Count; i++)
        {
            if (babyGrid[i].babyData.child.id == role.child.id)
            {
                Debug.Log("找到了对应的宝宝图标 删除");
                babyGrid[i].RemoveInfo();
                return;
            }
        }
        Debug.LogError("没有找到对应的宝宝图标");
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
