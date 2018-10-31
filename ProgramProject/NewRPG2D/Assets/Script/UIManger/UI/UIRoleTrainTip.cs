using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleTrainTip : TTUIPage
{
    public static UIRoleTrainTip instance;

    Dictionary<int, int> tipDic = new Dictionary<int, int>();

    public GameObject roleTrainTipGrid;
    private List<UIRoleTrainTipGrid> trainGrids = new List<UIRoleTrainTipGrid>();

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

    public void UpdateInfo(RoleTrainHelper index)
    {
        if (tipDic.ContainsKey(index.roleID))
        {
            trainGrids[tipDic[index.roleID]].UpdateInfo(index, canvas);
        }
        else
        {
            for (int i = 0; i < trainGrids.Count; i++)
            {
                if (trainGrids[i].isUse == false)
                {
                    trainGrids[i].UpdateInfo(index, canvas);
                    tipDic.Add(index.roleID, i);
                }
            }
        }
    }

    public void RemoveDic(int roleID)
    {
        trainGrids[tipDic[roleID]].isUse = false;
        if (tipDic.ContainsKey(roleID))
        {
            tipDic.Remove(roleID);
        }
    }
}
