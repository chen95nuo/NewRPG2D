using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPopUp_3Grid : MonoBehaviour
{
    public Text txt_Tip;
    public bool isTrue;
    private RectTransform rect;

    public void UpdateInfo(Canvas canvas, UIPopUp_3Helper data)
    {
        rect = txt_Tip.transform.transform as RectTransform;
        Vector2 pos = GameHelper.instance.GetPoint(canvas, data.ts);
        rect.anchoredPosition = pos;
        string st = LanguageDataMgr.instance.GetRoomTxtColor(data.name);
        txt_Tip.text = st + "+" + data.number + "</color>";
        Vector2 v2 = pos + (Vector2.up * 100);
        rect.DOAnchorPos(v2, 1.0f).OnComplete(() => gameObject.SetActive(false));
    }
}
