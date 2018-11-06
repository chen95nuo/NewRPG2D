using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleTip : TTUIPage
{

    public GameObject tip;
    private Canvas canvas;
    public Transform TipPoint;
    private List<UIRoleTipGrid> tipGrids = new List<UIRoleTipGrid>();

    private void Awake()
    {
        canvas = TTUIRoot.Instance.uiCanvas;
        InstantiateGrid();
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        RoleTipHelper data = mData as RoleTipHelper;
    }

    public void UpdateInfo(RoleTipHelper data)
    {
        for (int i = 0; i < tipGrids.Count; i++)
        {
            if (i + 1 == tipGrids.Count)
            {
                InstantiateGrid();
            }
            if (tipGrids[i].IsUse == false)
            {
                tipGrids[i].IsUse = true;
                tipGrids[i].UpdateInfo(canvas, data);
            }
        }
    }

    private void InstantiateGrid()
    {
        GameObject go = Instantiate(tip, TipPoint) as GameObject;
        UIRoleTipGrid grid = go.GetComponent<UIRoleTipGrid>();
        tipGrids.Add(grid);
        grid.IsUse = false;
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }
}

public class RoleTipHelper
{
    public int roleID;
    public string spriteName;
    public RoleTipHelper(int roleID, string st = "")
    {
        this.roleID = roleID;
        this.spriteName = st;
    }
}