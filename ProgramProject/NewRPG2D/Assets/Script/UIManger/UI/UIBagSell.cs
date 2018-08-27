using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagSell : MonoBehaviour
{

    public Text propName;
    public Slider numberSlider;
    public Text numberText;
    public Button addNumber;
    public Button reduceNumber;
    public Text coin;
    public Button btn_back;
    public Button btn_isTrue;

    private ItemData itemData;
    public UIBagPopUp popUp;

    private void Awake()
    {
        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickOBJ);
        addNumber.onClick.AddListener(ChickAddNumber);
        reduceNumber.onClick.AddListener(ChickReduceNumber);
        btn_back.onClick.AddListener(ChickBack);
        btn_isTrue.onClick.AddListener(ChickIsTrue);
        numberSlider.onValueChanged.AddListener(SliderValueChange);

    }

    private void SliderValueChange(float value)
    {
        numberText.text = value.ToString();
        coin.text = (numberSlider.value * itemData.SellPrice).ToString();
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickOBJ);
    }

    public void UpdateMessage(ItemData data)
    {
        itemData = data;
        propName.text = data.Name;
        numberSlider.maxValue = data.Number;
        numberSlider.value = 1;
        coin.text = (numberSlider.value * data.SellPrice).ToString();
    }

    private void ChickAddNumber()
    {
        if (numberSlider.value < numberSlider.maxValue)
        {
            numberSlider.value++;
        }
    }

    private void ChickReduceNumber()
    {
        if (numberSlider.value > numberSlider.minValue)
        {
            numberSlider.value--;
        }
    }

    private void ChickIsTrue()
    {
        string st = "是否出售 " + itemData.Name + "*" + numberSlider.value + "\n" + "将获得 : " + (numberSlider.value * itemData.SellPrice) + "金币";
        TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
        GameMessageType data = new GameMessageType();
        data.message = st;
        data.gameOBJ = this.gameObject;
        UIEventManager.instance.SendEvent<GameMessageType>(UIEventDefineEnum.UpdateMessageTipEvent, data);
    }
    private void ChickOBJ(GameObject obj)
    {
        Debug.Log("运行了");
        if (obj == this.gameObject)
        {
            GetPlayData.Instance.player[0].GoldCoin += itemData.SellPrice;
            BagItemData.Instance.ReduceItems(itemData.Id, (int)numberSlider.value);
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessagePopTipEvent, itemData.SellPrice.ToString());
            Debug.Log("运行了");
            this.gameObject.SetActive(false);
            TinyTeam.UI.TTUIPage.ShowPage<UIPopTipPage>();
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessagePopTipEvent, (numberSlider.value * itemData.SellPrice).ToString());
            TinyTeam.UI.TTUIPage.ClosePage<UIBagItemMessage>();
        }
    }

    private void ChickBack()
    {
        this.gameObject.SetActive(false);
    }


}
