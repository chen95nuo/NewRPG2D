using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILoading : TTUIPage
{
    public static UILoading instance = null;

    public Slider slider;
    public Text txt_LoadNum;
    public bool GetMessageDown { get { return GameHelper.instance.ServerInfo; } }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (slider.value < 98)
        {
            slider.value += Time.deltaTime * 80;
        }
        else if (GetMessageDown)
        {
            slider.value += Time.deltaTime * 5;
        }
        txt_LoadNum.text = slider.value.ToString("#0") + "%";
        if (slider.value == slider.maxValue)
        {
            ClosePage();
            BuildingManager.instance.ResetUIProduce();
            UIPanelManager.instance.ShowPage<UIMain>();
        }
    }

    public override void Hide(bool needAnim)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
