using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleTrainGroup : TTUIPage
{
    public static UIRoleTrainGroup instance;

    public GameObject tip;
    public Transform tipGridPoint;
    public Canvas canvas;
    private List<UIRoleTrainGrid> trainGrid = new List<UIRoleTrainGrid>();

    private void Awake()
    {
        instance = this;
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
    }

    public void ShowIcon(HallRole role, TrainType type)
    {
        Transform ts = role.transform;
        for (int i = 0; i < trainGrid.Count; i++)
        {
            if (trainGrid[i].IsUse == false)
            {
                trainGrid[i].GetInfo(ts, canvas, type, role);
            }
        }
        GameObject go = Instantiate(tip, tipGridPoint) as GameObject;
        UIRoleTrainGrid data = go.GetComponent<UIRoleTrainGrid>();
        data.GetInfo(ts, canvas, type, role);
    }



}
