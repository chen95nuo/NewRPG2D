using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageTip : MonoBehaviour
{

    private Text text_message;
    private Button btn_isTrue;
    private Button btn_back;
    public Image currencyImage;
    public Text text_message_2;
    public Text text_number;

    private GameObject go;

    private void Awake()
    {
        UIEventManager.instance.AddListener<GameMessageType>(UIEventDefineEnum.UpdateMessageTipEvent, ChickMessage);
        btn_isTrue = this.transform.Find("MainBG/BtnGroup/btn_isTrue").GetComponent<Button>();
        btn_back = this.transform.Find("MainBG/BtnGroup/btn_back").GetComponent<Button>();
        btn_isTrue.onClick.AddListener(ChickIsTrue);
        btn_back.onClick.AddListener(TinyTeam.UI.TTUIPage.ClosePage<UIMessageTipPage>);
        text_message = this.transform.Find("MainBG/Cry_InsideBG/message").GetComponent<Text>();
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<GameMessageType>(UIEventDefineEnum.UpdateMessageTipEvent, ChickMessage);
    }

    public void ChickIsTrue()
    {
        if (go != null)
            UIEventManager.instance.SendEvent<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, go);
        go = null;
        TinyTeam.UI.TTUIPage.ClosePage<UIMessageTipPage>();
    }
    public void ChickMessage(GameMessageType message)
    {
        this.go = message.gameOBJ;
        text_message.text = message.message + "\n";
        switch (message.messageType)
        {
            case UIMessageType.OnlyTip:
                currencyImage.gameObject.SetActive(false);
                text_message_2.gameObject.SetActive(false);
                text_number.gameObject.SetActive(false);
                break;
            case UIMessageType.BuyOrSell:
                currencyImage.gameObject.SetActive(true);
                text_message_2.gameObject.SetActive(true);
                text_number.gameObject.SetActive(true);
                text_number.text = message.number.ToString();

                switch (message.currencyType)
                {
                    case CurrencyType.GoldCoin:
                        currencyImage.sprite = GetSpriteAtlas.insatnce.GetIcon("Cry_Icon_GoldImage");
                        break;
                    case CurrencyType.Diamonds:
                        currencyImage.sprite = GetSpriteAtlas.insatnce.GetIcon("Cry_Icon_DiamondsImage");
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
