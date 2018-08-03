using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIMessageTipPage : TTUIPage
{
    private Text text_message;

    public UIMessageTipPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UIMessageTip";
    }

    public override void Awake(GameObject go)
    {
        UIEventManager.instance.AddListener<string>(UIEventDefineEnum.UpdateMissageTipEvent, UpdateMessage);
        this.transform.Find("MainBG/btn_enter").GetComponent<Button>().onClick.AddListener(ClosePage<UIMessageTipPage>);
        text_message = this.transform.Find("MainBG/message").GetComponent<Text>();
    }

    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }


    public void UpdateMessage(string message)
    {
        text_message.text = message;
    }
}
