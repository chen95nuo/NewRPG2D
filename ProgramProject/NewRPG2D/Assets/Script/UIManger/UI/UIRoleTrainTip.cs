using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleTrainTip : TTUIPage
{
    public static UIRoleTrainTip instance;

    Dictionary<int, UIRoleTrainTipGrid> tipDic = new Dictionary<int, UIRoleTrainTipGrid>();

    List<UIRoleTrainTipGrid> tipPool = new List<UIRoleTrainTipGrid>();

    public GameObject roleTrainTipGrid;

    private Canvas canvas;

    private void Awake()
    {
        instance = this;
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        RoleTrainHelper index = mData as RoleTrainHelper;
        UpdateInfo(index);
    }

    public void AddLister(int roleID)
    {
        if (tipPool.Count <= 0)
        {
            GameObject go = Instantiate(roleTrainTipGrid, transform) as GameObject;
            UIRoleTrainTipGrid grid = go.GetComponent<UIRoleTrainTipGrid>();
            tipDic.Add(roleID, grid);
        }
        else
        {
            tipDic.Add(roleID, tipPool[0]);
            tipPool[0].gameObject.SetActive(true);
            tipPool.RemoveAt(0);
        }
    }

    public void UpdateInfo(RoleTrainHelper trainData)
    {
        if (tipDic.ContainsKey(trainData.roleID))
        {
            tipDic[trainData.roleID].UpdateInfo(trainData, canvas);
        }
        else
        {
            AddLister(trainData.roleID);
            tipDic[trainData.roleID].UpdateInfo(trainData, canvas);
        }

    }

    public void RemoveDic(int roleID)
    {
        if (tipDic.ContainsKey(roleID))
        {
            tipDic[roleID].gameObject.SetActive(false);
            tipPool.Add(tipDic[roleID]);
            tipDic.Remove(roleID);
        }
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
