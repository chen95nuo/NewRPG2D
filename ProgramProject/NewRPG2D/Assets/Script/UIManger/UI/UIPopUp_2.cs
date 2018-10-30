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
        transform.SetSiblingIndex(transform.parent.childCount - 1);
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
                anim(PopUps[i], st);
                break;
            }
            if (i == PopUps.Count - 1)
            {
                GameObject go = Instantiate(popUp_2, transform) as GameObject;
                Text txt = go.GetComponent<Text>();
                PopUps.Add(new PopUp_2Helper(txt));
            }
        }
    }

    private void anim(PopUp_2Helper popUp, string st)
    {
        Sequence mSequence = DOTween.Sequence();
        popUp.txt.rectTransform.DOAnchorPos(Vector2.up * 700, 5).OnComplete(() => popUp.IsUse = false);
        popUp.txt.transform.localScale = Vector3.zero;
        mSequence.Append(popUp.txt.transform.DOScale(1.2f, 0.25f));
        mSequence.Append(popUp.txt.transform.DOScale(1f, 0.25f));
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
