using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopTip : MonoBehaviour
{
    private Text message_1;
    private Text message_2;
    private Image priceImage;

    private void Awake()
    {
        message_1 = this.transform.Find("BG/Message_1").GetComponent<Text>();
        message_2 = this.transform.Find("BG/Message_2").GetComponent<Text>();
        priceImage = this.transform.Find("BG/PriceImage").GetComponent<Image>();

        UIEventManager.instance.AddListener<string>(UIEventDefineEnum.UpdateMessagePopTipEvent, UpdateMessage);
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<string>(UIEventDefineEnum.UpdateMessagePopTipEvent, UpdateMessage);
    }

    private void UpdateMessage(string st)
    {
        message_2.text = "+" + st;
        Debug.Log("运行了");
        Invoke("CloseTipPage", 2.0f);
    }

    private void CloseTipPage()
    {
        TinyTeam.UI.TTUIPage.ClosePage<UIPopTipPage>();
    }

}
