//提示钻石不足
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUP_5 : TTUIPage
{
    public Text txt_Tip;
    public Text txt_Message;
    public Text txt_enter;
    public Text txt_cancel;
    public Button btn_enter;
    public Button btn_cancel;

    private void Awake()
    {
        txt_Tip.text = LanguageDataMgr.instance.GetUIString("zuanshibuzu");
        btn_enter.onClick.AddListener(CheckEnter);
        btn_cancel.onClick.AddListener(ClosePage);
        txt_enter.text = LanguageDataMgr.instance.GetUIString("goumai");
        txt_cancel.text = LanguageDataMgr.instance.GetUIString("quxiao");
    }

    private void CheckEnter()
    {
        UIMain.instance.ShowMarket(5);
    }
    public override void Show(object mData)
    {
        base.Show(mData);
        int num = Convert.ToInt32(mData);
        UpdateInfo(num);
    }

    public void UpdateInfo(int num)
    {
        string icon = GameHelper.instance.TextAddIcon("Diamonds");
        txt_Message.text = num.ToString() + icon;
    }
}
