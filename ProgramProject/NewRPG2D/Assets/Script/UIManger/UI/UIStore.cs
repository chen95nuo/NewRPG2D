using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : MonoBehaviour
{

    public int maxItem = 9;

    public Text time;
    public GameObject buyTip;
    public UIBagGrid currentItem;
    public UIBagItem bagItem;
    public Slider buySlider;
    public Text buyNumber;
    public Button buyNumberAdd;
    public Button buyNumberReduce;
    public Image priceImage;
    public Text priceNumber;
    public Button btn_true;
    public Button btn_false;

    private ItemData currentData;
    private ItemData[] toDayItem;
    private PlayerData playerData;

    void Awake()
    {
        Init();

        UIEventManager.instance.AddListener<ItemData>(UIEventDefineEnum.UpdateStoreEvent, DrwTipPage);
    }

    private void Init()
    {
        buyTip.SetActive(false);
        buyNumberAdd.onClick.AddListener(BuyNumberAdd);
        buyNumberReduce.onClick.AddListener(BuyNumberReduce);
        btn_true.onClick.AddListener(IsTrue);
        btn_false.onClick.AddListener(IsFalse);
        playerData = GetPlayData.Instance.player[0];
    }

    private void Update()
    {
        System.DateTime nextDay = SystemTime.insatnce.GetTime().AddDays(1).Date;
        System.TimeSpan ts = nextDay.Subtract(SystemTime.insatnce.GetTime());
        string nowTime = string.Format("{0:D2}", (int)ts.TotalHours) + ":" + string.Format("{0:D2}", ts.Minutes) + ":" + string.Format("{0:D2}", ts.Seconds);
        time.text = nowTime;
    }

    private void Start()
    {
        RefreshStore();
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<ItemData>(UIEventDefineEnum.UpdateStoreEvent, DrwTipPage);

    }

    private void DrwTipPage(ItemData data)
    {
        currentData = data;
        buyTip.SetActive(true);
        currentItem.UpdateItem(data);
        buySlider.value = 0;
        buySlider.maxValue = data.Number;
        buyNumber.text = buySlider.value.ToString();
    }

    private void RefreshStore()
    {
        //GameStoreData.Instance.items
        List<StoreData> items = GameStoreData.Instance.items;
        int index = items.Count;
        List<StoreData> newData = new List<StoreData>();
        do
        {
            int roll = Random.Range(0, index);
            newData.Add(items[roll]);
        } while (newData.Count < maxItem);

        toDayItem = new ItemData[newData.Count];

        for (int i = 0; i < newData.Count; i++)
        {
            int roll = Random.Range(1, newData[i].PropNumber);
            ItemData temp = GamePropData.Instance.GetItem(newData[i].PropId, roll);
            toDayItem[i] = temp;
        }

        bagItem.UpdateStore(toDayItem);
    }
    public void SliderValueChange()
    {
        buyNumber.text = buySlider.value.ToString();
        float currentPrice = currentData.BuyPrice * buySlider.value;
        priceNumber.text = currentPrice.ToString();
        if (currentPrice > playerData.GoldCoin)
        {
            btn_true.interactable = false;
            priceNumber.text = "<color=#FF0000>" + currentPrice + "</color>";
        }
        else
        {
            btn_true.interactable = true;
            //priceNumber.text = "<color=#E7BE2F>" + currentPrice + "</color>";
            switch (currentData.StorePropType)
            {
                case StorePropType.Nothing:
                    break;
                case StorePropType.GoldCoin:
                    priceNumber.text = "<color=#E7BE2F>" + currentPrice + "</color>";
                    break;
                case StorePropType.Diamonds:
                    priceNumber.text = "<color=#79D2FF>" + currentPrice + "</color>";
                    break;
                default:
                    break;
            }
        }
    }

    public void BuyNumberAdd()
    {
        buySlider.value++;
    }

    public void BuyNumberReduce()
    {
        buySlider.value--;
    }

    public void IsTrue()
    {

    }
    public void IsFalse()
    {
        buyTip.SetActive(false);
    }

}
