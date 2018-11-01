using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUpTips_1 : TTUIPage
{
    public Text txt_Name;
    public Text txt_Tip;
    public Image icon;
    public Transform tipTs;
    public Button btn_Close;

    private void Awake()
    {
        tipTs.position = Vector3.one * 20000;
        btn_Close.onClick.AddListener(ClosePage);
    }


    public override void Show(object mData)
    {
        base.Show(mData);
        RoleInfoTipHelper helperData = mData as RoleInfoTipHelper;
        UpdateInfo(helperData);
    }

    private void UpdateInfo(RoleInfoTipHelper data)
    {
        string[] st = data.st.Split(',');
        txt_Name.text = st[0];
        string str = "";
        for (int i = 1; i < st.Length; i++)
        {
            str += string.Format(st[i], "<color=#b4e254>", "</color>");
        }
        txt_Tip.text = str;
        icon.enabled = data.needIcon;
        if (data.needIcon)
        {
            icon.sprite = GetSpriteAtlas.insatnce.GetLevelIconToAtr(data.atr);
        }
        tipTs.position = data.ts.position;
    }






    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
        tipTs.position = Vector3.one * 20000;
    }
}