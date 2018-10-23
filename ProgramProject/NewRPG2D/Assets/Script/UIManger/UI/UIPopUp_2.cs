using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using DG.Tweening;


public class UIPopUp_2 : TTUIPage
{
    public GameObject popUp_2;
    private List<PopUp_2Helper> PopUps = new List<PopUp_2Helper>();

    public override void Show(object mData)
    {
        base.Show(mData);
        string st = mData as string;
        Debug.Log("St =" + st);
        UpdateInfo(st);
    }

    public void UpdateInfo(string st)
    {
        if (PopUps.Count <= 0)
        {
            GameObject go = Instantiate(popUp_2, transform) as GameObject;
            Text txt = go.GetComponent<Text>();
            PopUps.Add(new PopUp_2Helper(txt));
        }
        for (int i = 0; i < PopUps.Count; i++)
        {
            if (PopUps[i].IsUse == false)
            {
                PopUps[i].IsUse = true;
                PopUps[i].txt.text = st;
                PopUps[i].txt.rectTransform.DOAnchorPos(Vector2.up * 700, 3).OnComplete(() => PopUps[i].IsUse = false);
                return;
            }
            if (i == PopUps.Count - 1)
            {
                GameObject go = Instantiate(popUp_2, transform) as GameObject;
                Text txt = go.GetComponent<Text>();
                PopUps.Add(new PopUp_2Helper(txt));
            }
        }
    }
}

public class PopUp_2Helper
{
    public Text txt;
    private bool isUse;

    public PopUp_2Helper(Text text)
    {
        txt = text;
        IsUse = false;
    }

    public bool IsUse
    {
        get
        {
            return isUse;
        }

        set
        {
            isUse = value;
            if (IsUse == false)
            {
                txt.text = "";
                txt.rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}
