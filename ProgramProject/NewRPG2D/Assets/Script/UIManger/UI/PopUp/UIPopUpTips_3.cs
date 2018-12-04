//主界面人口提示框

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUpTips_3 : TTUIPage
{
    public static UIPopUpTips_3 instance;

    public Text txt_Tip;
    public Text txt_nowPopulation;
    public Text txt_amount;

    public Transform tipTs;

    private void Awake()
    {
        txt_Tip.text = LanguageDataMgr.instance.GetUIString("renkou");
        txt_nowPopulation.text = LanguageDataMgr.instance.GetUIString("dangqianrenkou");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (go == null || go != tipTs.gameObject)
            {
                ClosePage();
            }
        }
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        List<object> data = mData as List<object>;

        tipTs = data[0] as Transform;
        string st = data[1].ToString();
        txt_amount.text = st;
    }

    public override void Hide(bool needAnim = false)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = false)
    {
        base.Active(needAnim = false);
    }
}
