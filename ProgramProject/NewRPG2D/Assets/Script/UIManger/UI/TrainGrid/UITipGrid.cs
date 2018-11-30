using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UITipGrid : MonoBehaviour
{

    protected bool isUse;
    protected Canvas canvas;
    protected Image icon;
    protected Vector3 point;
    protected RectTransform rt;
    protected Button btn_Click;
    protected HallRoleData currentData;

    public bool IsUse
    {
        get
        {
            return isUse;
        }

        set
        {
            isUse = value;
            gameObject.SetActive(isUse);
        }
    }

    protected void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, UpdatePos);
        rt = transform.transform as RectTransform;
        btn_Click = GetComponent<Button>();
        btn_Click.onClick.AddListener(ChickEnter);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, UpdatePos);
    }

    public void UpdateInfo(Canvas canvas, RoleTipHelper tipData)
    {
        currentData = HallRoleMgr.instance.GetRoleData(tipData.roleID);
        UpdatePos();
    }

    protected void UpdatePos()
    {
        if (currentData.instance == null)
        {
            return;
        }
        Vector3 ts = currentData.instance.TipPoint.position;
        rt.anchoredPosition = GameHelper.instance.GetPoint(canvas, ts);
    }

    protected virtual void ChickEnter()
    {
        RemoveInfo();
    }

    public void RemoveInfo()
    {
        IsUse = false;
        transform.position = Vector3.up * 1000;
        currentData = null;
    }
}
