using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIMessageTipPage : TTUIPage
{
    private Text text_message;
    private Button btn_isTrue;
    private Button btn_back;

    private GameObject go;

    public UIMessageTipPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UIMessageTip";
    }

    public override void Awake(GameObject go)
    {
        UIEventManager.instance.AddListener<string>(UIEventDefineEnum.UpdateMissageTipEvent, UpdateMessage);
        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateMissageTipEvent, ChickGO);
        btn_isTrue = this.transform.Find("MainBG/BtnGroup/btn_isTrue").GetComponent<Button>();
        btn_back = this.transform.Find("MainBG/BtnGroup/btn_back").GetComponent<Button>();
        btn_isTrue.onClick.AddListener(ChickIsTrue);
        btn_back.onClick.AddListener(ClosePage<UIMessageTipPage>);
        text_message = this.transform.Find("MainBG/Cry_InsideBG/message").GetComponent<Text>();
    }

    public override void Refresh()
    {
        go = null;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public void ChickIsTrue()
    {
        if (go != null)
            UIEventManager.instance.SendEvent<GameObject>(UIEventDefineEnum.UpdateMissageTipEvent, go);
        else
            ClosePage<UIMessageTipPage>();

    }
    public void ChickGO(GameObject go)
    {
        this.go = go;
    }
    public void UpdateMessage(string message)
    {
        text_message.text = message;
    }
}
