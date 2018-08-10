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


    private void OnEnable()
    {
        slider_BuyNumber.value = 1;
    }
    private void Awake()
    {
        Init();

        UIEventManager.instance.AddListener<UIBagGrid>(UIEventDefineEnum.UpdateBuyItem, BuyItem);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<UIBagGrid>(UIEventDefineEnum.UpdateBuyItem, BuyItem);
    }

    private void Init()
    {
        playerData = GetPlayData.Instance.player[0];
        goldCoin = GetSpriteAtlas.insatnce.GetIcon("GoldImage");
        diamonds = GetSpriteAtlas.insatnce.GetIcon("DiamondsImage");

        btn_Buy.onClick.AddListener(ChickBuy);
        btn_back.onClick.AddListener(ChickBack);
        addNumber.onClick.AddListener(ChickAdd);
        reduceNumber.onClick.AddListener(ChickReduce);

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
                itemImage = data.eggGrid.eggImage;
                ItemQuality = data.eggGrid.eggBG;
                slider_BuyNumber.maxValue = data.eggData.ItemNumber;
                itemPrice = data.eggData.Price;
                break;
            case ItemType.Prop:
                itemName.text = data.propData.Name;
                itemImage = data.otherGrid.otherImage;
                ItemQuality = data.otherGrid.otherBG;
                slider_BuyNumber.maxValue = data.propData.Number;
                itemPrice = data.propData.BuyPrice;
                switch (data.propData.StorePropType)
                {
                    case StorePropType.Nothing:
                        break;
                    case StorePropType.GoldCoin:
                        currencyType.sprite = goldCoin;
                        break;
                    case StorePropType.Diamonds:
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
        TinyTeam.UI.TTUIPage.ClosePage<UIBusinessTipPage>();
    }
    private void ChickBack()
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
        if (currentPrice > playerData.GoldCoin)
        {
            btn_Buy.interactable = false;
            text_currentPrice.text = "<color=#FF0000>" + currentPrice + "</color>";
        }
        else
        {
            btn_Buy.interactable = true;
            if (uiBagGrid == null)
            {
                return;
            }
            switch (uiBagGrid.propData.StorePropType)
            {
                case StorePropType.Nothing:
                    break;
                case StorePropType.GoldCoin:
                    text_currentPrice.text = "<color=#E7BE2F>" + currentPrice + "</color>";
                    break;
                case StorePropType.Diamonds:
                    text_currentPrice.text = "<color=#79D2FF>" + currentPrice + "</color>";
                    break;
                default:
                    break;
            }
        }
    }
}
