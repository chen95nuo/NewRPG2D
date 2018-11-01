using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUP_3 : TTUIPage
{
    public Text txt_Tip;
    private Canvas canvas;
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
        RectTransform rect = txt_Tip.transform.transform as RectTransform;
        Vector2 pos = GameHelper.instance.GetPoint(canvas, data.ts);
        rect.anchoredPosition = pos;

        string st = LanguageDataMgr.instance.GetRoomTxtColor(data.name);
        txt_Tip.text = st + "+" + data.number + "</color>";
        Invoke("ClosePage", 1.0f);
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
