using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using DG.Tweening;

public class UIPopUP_3 : TTUIPage
{
    public GameObject txt_Tip;
    private Canvas canvas;
    private List<UIPopUp_3Grid> grids = new List<UIPopUp_3Grid>();
    private void Awake()
    {
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        UIPopUp_3Helper data = mData as UIPopUp_3Helper;
        UpdateInfo(data);
    }

    private void UpdateInfo(UIPopUp_3Helper data)
    {
        for (int i = 0; i < grids.Count; i++)
        {
            if (grids[i].IsTrue == false)
            {
                grids[i].UpdateInfo(canvas, data);
                return;
            }
        }
        GameObject go = Instantiate(txt_Tip, transform) as GameObject;
        UIPopUp_3Grid grid = go.GetComponent<UIPopUp_3Grid>();
        grid.UpdateInfo(canvas, data);
        grids.Add(grid);
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


public class UIPopUp_3Helper
{
    public BuildRoomName name;
    public Vector3 ts;
    public int number;
    public UIPopUp_3Helper(BuildRoomName name, int number, Vector3 ts)
    {
        this.name = name;
        this.number = number;
        this.ts = ts;
    }
}
