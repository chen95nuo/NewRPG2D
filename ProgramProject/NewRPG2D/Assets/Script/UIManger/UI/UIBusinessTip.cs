using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBusinessTip : MonoBehaviour
{
    public Text itemName; //物品名字
    public Image itemImage;//物品图片
    public Image ItemQuality;//物品品质
    public Text buyNumber;//当前数量
    public Slider slider_BuyNumber;//购买数量
    public Button addNumber;//添加数量
    public Button reduceNumber;//减少数量
    public Text text_currentPrice;//当前价格
    public Image currencyType;//货币类型
    public Button btn_Buy;//购买按钮
    public Button btn_back;//后退

    private float itemPrice;//物品价格

    private Sprite goldCoin;
    private Sprite diamonds;
    private float currentPrice;//当前价格

    private UIBagGrid uiBagGrid;

    private PlayerData playerData;

    public Image BG;
    public Animation anim;

    private void OnEnable()
    {
        slider_BuyNumber.value = 1;
        //UIAnimTools.Instance.GetBG(BG, false, .8f);
        //UIAnimTools.Instance.PlayAnim(anim, "UIBusinessTipMain_in", false);
    }
    private void Awake()
    {
        Init();

        UIEventManager.instance.AddListener<UIBagGrid>(UIEventDefineEnum.UpdateBuyItem, BuyItem);
        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickMessage);
        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateStoreEvent, RestartOBJ);
    }
    GameObject go;
    private void RestartOBJ(GameObject go)
    {
        this.go = go;
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<UIBagGrid>(UIEventDefineEnum.UpdateBuyItem, BuyItem);
        UIEventManager.instance.RemoveListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickMessage);
        UIEventManager.instance.RemoveListener<GameObject>(UIEventDefineEnum.UpdateStoreEvent, RestartOBJ);

    }

    private void Init()
    {
        playerData = GetPlayData.Instance.player[0];
        goldCoin = GetSpriteAtlas.insatnce.GetIcon("Cry_Icon_GoldImage");
        diamonds = GetSpriteAtlas.insatnce.GetIcon("Cry_Icon_DiamondsImage");

        btn_Buy.onClick.AddListener(ChickBuy);
        btn_back.onClick.AddListener(ChickBack);
        addNumber.onClick.AddListener(ChickAdd);
        reduceNumber.onClick.AddListener(ChickReduce);

    }

    private void ChickMessage(GameObject go)
    {
        Debug.Log("Message");
        if (go = this.gameObject)
        {
            //如果点击确认 获取当前道具信息 道具价格 为背包中的道具添加数量;
            playerData.GoldCoin -= (int)currentPrice;//扣钱
            switch (uiBagGrid.itemType)
            {
                case ItemType.Nothing:
                    break;
                case ItemType.Egg:
                    break;
                case ItemType.Prop:
                    uiBagGrid.propData.Number -= (int)slider_BuyNumber.value;//扣道具
                    BagItemData.Instance.AddItems(uiBagGrid.propData, (int)slider_BuyNumber.value);//背包添加道具
                    break;
                case ItemType.Equip:
                    break;
                case ItemType.Role:
                    break;
                default:
                    break;
            }
            UIEventManager.instance.SendEvent<bool>(UIEventDefineEnum.UpdateBuyItem, true);

            ChickBack();
        }
    }

    private void BuyItem(UIBagGrid data)
    {
        uiBagGrid = data;
        Debug.Log("获取到道具了");
        switch (data.itemType)
        {
            case ItemType.Nothing:
                break;
            case ItemType.Egg:
                itemName.text = data.eggData.Name;
                itemImage.sprite = data.eggGrid.eggImage.sprite;
                ItemQuality.sprite = data.eggGrid.eggBG.sprite;
                slider_BuyNumber.maxValue = data.eggData.ItemNumber;
                itemPrice = data.eggData.Price;
                break;
            case ItemType.Prop:
                itemName.text = data.propData.Name;
                itemImage.sprite = data.otherGrid.otherImage.sprite;
                ItemQuality.sprite = data.otherGrid.otherBG.sprite;
                slider_BuyNumber.maxValue = data.propData.Number;
                itemPrice = data.propData.BuyPrice;
                switch (data.propData.StorePropType)
                {
                    case CurrencyType.Nothing:
                        break;
                    case CurrencyType.GoldCoin:
                        currencyType.sprite = goldCoin;
                        break;
                    case CurrencyType.Diamonds:
                        currencyType.sprite = diamonds;
                        break;
                }
                break;
            case ItemType.Equip:
                break;
            case ItemType.Role:
                break;
            default:
                break;
        }

        SliderValueChange();//更新当前价格和数量

    }

    private void ChickBuy()
    {
        if (slider_BuyNumber.value == 0)
        {
            return;
        }
        Debug.Log("确认购买");
        string message = "是否使用" + currentPrice + "购买" + uiBagGrid.propData.Name + "*" + slider_BuyNumber.value;
        TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
        //UIAnimTools.Instance.GetBG(BG, true);
        //UIAnimTools.Instance.PlayAnim(anim, "UIBusinessTipMain_out");
        UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateMessageTipEvent, message);
        UIEventManager.instance.SendEvent<GameObject>(UIEventDefineEnum.UpdateMessageTipGoEvent, this.gameObject);
        ////如果点击确认 获取当前道具信息 道具价格 为背包中的道具添加数量;

    }
    private void ChickBack()
    {
        //UIAnimTools.Instance.GetBG(BG, true);
        //UIAnimTools.Instance.PlayAnim(anim, "UIBusinessTipMain_out");

        //if (go != null)
        //{
        //    go.SetActive(false);
        //    go.SetActive(true);
        //    go = null;
        //}

        //Invoke("ClosePage", .8f);
        ClosePage();
    }

    private void ClosePage()
    {
        TinyTeam.UI.TTUIPage.ClosePage<UIBusinessTipPage>();
    }
    private void ChickAdd()
    {
        slider_BuyNumber.value++;
    }
    private void ChickReduce()
    {
        slider_BuyNumber.value--;
    }

    public void SliderValueChange()
    {
        buyNumber.text = slider_BuyNumber.value.ToString();
        currentPrice = itemPrice * slider_BuyNumber.value;
        text_currentPrice.text = currentPrice.ToString();
        Debug.Log(currentPrice);
        Debug.Log(playerData.GoldCoin);
        if (uiBagGrid == null)
        {
            return;
        }
        switch (uiBagGrid.propData.StorePropType)
        {
            case CurrencyType.Nothing:
                break;
            case CurrencyType.GoldCoin:
                if (currentPrice > playerData.GoldCoin)
                {
                    btn_Buy.interactable = false;
                    text_currentPrice.text = "<color=#FF0000>" + currentPrice + "</color>";
                }
                else
                {
                    state();
                }
                break;
            case CurrencyType.Diamonds:
                if (currentPrice > playerData.Diamonds)
                {
                    btn_Buy.interactable = false;
                    text_currentPrice.text = "<color=#FF0000>" + currentPrice + "</color>";
                }
                else
                {
                    state();
                }
                break;
            default:
                break;
        }



    }

    private void state()
    {
        {
            btn_Buy.interactable = true;
            if (uiBagGrid == null)
            {
                return;
            }
            switch (uiBagGrid.propData.StorePropType)
            {
                case CurrencyType.Nothing:
                    break;
                case CurrencyType.GoldCoin:
                    text_currentPrice.text = "<color=#E7BE2F>" + currentPrice + "</color>";
                    break;
                case CurrencyType.Diamonds:
                    text_currentPrice.text = "<color=#79D2FF>" + currentPrice + "</color>";
                    break;
                default:
                    break;
            }
        }
    }
}
