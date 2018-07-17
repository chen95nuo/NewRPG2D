using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIFurnace : MonoBehaviour
{


    public DrawFurnaceMenu[] furnaces;

    public Button addFurnace;
    public Button[] popUp;
    public GameObject typeTime;
    public Text time;
    public Slider timeSlider;

    public Text goldCoin;
    public Text needTime;
    public Button start;

    public Button rawMaterial;
    public RawMaterials[] rawMaterials;

    public PlayerData playerData;

    private int currentMenu = 0;
    private int currentButton = 0;

    private int chickCoin = 0;
    private int chickTime = 0;

    public GameObject TipGO;
    public Button isTrue;
    public Button isFalse;
    public Text Tip;
    private FurnaceTipType tipType;

    public UIFurnacePopUp furnacePopUp;

    private void Awake()
    {
        AwakeInitialization();//初始化

        UIEventManager.instance.AddListener<ItemData>(UIEventDefineEnum.UpdatePropsEvent, AddMaterial);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateFurnaceEvent, RemoveMaterial);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<ItemData>(UIEventDefineEnum.UpdatePropsEvent, AddMaterial);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateFurnaceEvent, RemoveMaterial);

    }

    public void AwakeInitialization()
    {
        for (int i = 0; i < furnaces.Length; i++)
        {
            furnaces[i].menu.onClick.AddListener(ChickMenu);
        }

        TipGO.SetActive(false);
        start.interactable = false;
        start.onClick.AddListener(RunStart);
        isTrue.onClick.AddListener(TipTure);
        isFalse.onClick.AddListener(TipFalse);

        for (int i = 0; i < rawMaterials.Length; i++)
        {
            rawMaterials[i].btn_rawMaterials.onClick.AddListener(ChickButton);
            rawMaterials[i].propQuality.gameObject.SetActive(false);
            rawMaterials[i].noPorp.gameObject.SetActive(true);
        }
        playerData = GetPlayData.Instance.GetPlayerData(0);
        TimeSerialization(0, needTime);
        goldCoin.text = "0";

        ChickMenu();
        UpdateFurnaceMenu();
        UpdateFurnace();
    }
    /// <summary>
    /// 刷新熔炉Menu
    /// </summary>
    public void UpdateFurnaceMenu()
    {
        if (playerData.Furnace.Count == furnaces.Length)
        {
            addFurnace.gameObject.SetActive(false);
        }
        addFurnace.onClick.AddListener(AddFurnace);
        for (int i = 0; i < furnaces.Length; i++)
        {
            if (i < playerData.Furnace.Count)
            {
                furnaces[i].menu.gameObject.SetActive(true);
                furnaces[i].menuNumber.text = (playerData.Furnace[i].Id + 1).ToString();
                if (playerData.Furnace[i].Time == 0)
                {
                    furnaces[i].menuNull.SetActive(true);
                    furnaces[i].something.SetActive(false);
                }
                else
                {
                    furnaces[i].something.SetActive(true);
                    furnaces[i].menuNull.SetActive(false);
                    furnaces[i].menuTime.text = playerData.Furnace[i].Time.ToString();
                }
            }
            else
            {
                furnaces[i].menu.gameObject.SetActive(false);
            }

        }

    }

    public void UpdateFurnace()
    {
        for (int i = 0; i < rawMaterials.Length; i++)
        {
            rawMaterials[i].propQuality.gameObject.SetActive(false);
            rawMaterials[i].noPorp.gameObject.SetActive(true);
        }
        ChickGoldCoin();
        ChickMaterial();
        furnacePopUp.Restart();
        furnacePopUp.UpdatePopUp(playerData.Furnace[currentMenu]);
    }

    public void ChickMenu()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < furnaces.Length; i++)
        {
            furnaces[i].menu.interactable = true;
            if (go == furnaces[i].menu.gameObject && go != furnaces[currentMenu].menu.gameObject)
            {
                //如果当前选择的熔炉不是正在使用的熔炉,如果上面没有材料,那么清空材料列表
                Debug.Log("切换熔炉");
                ChickMaterial();
                if (playerData.Furnace[i].Material == null)
                {
                    playerData.Furnace[i].Material = new ItemData[4];
                }
                RemoveMaterial();
                currentMenu = i;
                furnaces[i].menu.interactable = false;
            }
        }
        furnacePopUp.UpdatePopUp(playerData.Furnace[currentMenu]);
    }

    public void ChickButton()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < rawMaterials.Length; i++)
        {
            if (go == rawMaterials[i].btn_rawMaterials.gameObject)
            {
                currentButton = i;
                TinyTeam.UI.TTUIPage.ShowPage<UIUseItemBagPage>();
                UIEventManager.instance.SendEvent<PropMeltingType>(UIEventDefineEnum.UpdatePropsEvent, PropMeltingType.isTrue);
            }
        }
    }

    public void ChickMaterial()
    {
        int index = 0;
        for (int i = 0; i < playerData.Furnace[currentMenu].Material.Length; i++)
        {
            if (playerData.Furnace[currentMenu].Material[i] != null)
            {
                if (playerData.Furnace[currentMenu].Material[i].ItemType != ItemType.Nothing)
                {
                    index++;
                }
            }
            if (index >= 4)
            {
                start.interactable = true;
            }
            else
            {
                start.interactable = false;
            }
        }
    }
    public void RunStart()
    {

    }

    public void ChickGoldCoin()
    {
        chickCoin = 0;
        chickTime = 0;

        for (int i = 0; i < playerData.Furnace[currentMenu].Material.Length; i++)
        {
            if (playerData.Furnace[currentMenu].Material[i] != null)
            {
                chickCoin += playerData.Furnace[currentMenu].Material[i].UsePrice;
                chickTime += (playerData.Furnace[currentMenu].Material[i].Quality);
            }
        }
        goldCoin.text = chickCoin.ToString();
        TimeSerialization(chickTime, needTime);
        //如果使用的金币超过我拥有的金币数量 则文字显示红色
        if (chickCoin > playerData.GoldCoin)
        {
            goldCoin.color = Color.red;
        }
        else
        {
            goldCoin.color = Color.white;
        }
    }

    /// <summary>
    /// 将材料添加到当前点击的位置上
    /// </summary>
    /// <param name="data">材料信息</param>
    public void AddMaterial(ItemData data)
    {
        if (playerData.Furnace[currentMenu].Material.Length < 4)
        {
            playerData.Furnace[currentMenu].Material = new ItemData[4];
        }
        if (playerData.Furnace[currentMenu].Material[currentButton] == null || playerData.Furnace[currentMenu].Material[currentButton].ItemType == ItemType.Nothing)
        {
            data.Number--;
            rawMaterials[currentButton].noPorp.gameObject.SetActive(false);
            rawMaterials[currentButton].propQuality.gameObject.SetActive(true);
            rawMaterials[currentButton].propImage.sprite = Resources.Load<Sprite>("UITexture/Icon/prop/" + data.SpriteName);
            rawMaterials[currentButton].propQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
            ItemData newData = new ItemData(GamePropData.Instance.GetItem(data.Id));
            newData.Number = 1;
            playerData.Furnace[currentMenu].Material[currentButton] = newData;
        }
        else
        {

        }
        ChickGoldCoin();
        ChickMaterial();
        furnacePopUp.UpdatePopUp(playerData.Furnace[currentMenu]);

    }

    /// <summary>
    /// 返回时将材料列表的材料释放
    /// </summary>
    public void RemoveMaterial()
    {
        ItemData[] items = playerData.Furnace[currentMenu].Material;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                break;
            }
            if (items[i].ItemType != ItemType.Nothing)//如果当前位置不是空的
            {
                List<ItemData> data = BagItemData.Instance.GetItems(items[i].Id);
                int index = 0;
                //检查有多少个相同的道具
                for (int j = 0; j < data.Count; j++)
                {
                    //如果背包里有该相同道具，且数量不满99 则该组道具加一
                    if (data[j].Number + 1 <= 99)
                    {
                        data[j].Number++;
                        items[i] = new ItemData(ItemType.Nothing);
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
                //如果这个背包中所有的该道具的总数都超过99或者背包中已经没有这个道具了 新建一个这个道具
                if (index >= data.Count)
                {
                    index = 0;
                    ItemData newData = new ItemData(items[i]);
                    BagItemData.Instance.AddItem(newData);
                    newData.Number = 1;
                    //清空当前位置的道具
                    items[i] = new ItemData(ItemType.Nothing);
                }
            }
        }
        UpdateFurnace();
    }

    public void AddFurnace()
    {
        tipType = FurnaceTipType.addFurnace;
        TipType();
    }

    public void TipTure()
    {
        switch (tipType)
        {
            case FurnaceTipType.addFurnace:
                playerData.AddGlodCoin -= (2000 * playerData.Furnace.Count);
                playerData.Furnace.Add(new FurnaceData(playerData.Furnace.Count));
                UpdateFurnaceMenu();
                break;
            case FurnaceTipType.UseFurnace:
                playerData.AddGlodCoin -= chickCoin;
                break;
            default:
                break;
        }
        TipGO.SetActive(false);
    }
    public void TipFalse()
    {

        TipGO.SetActive(false);
    }

    public void TipType()
    {
        switch (tipType)
        {
            case FurnaceTipType.addFurnace:
                TipGO.SetActive(true);
                Tip.text = "是否花费 " + (2000 * playerData.Furnace.Count) + " 金币开启新的熔炉";
                break;
            case FurnaceTipType.UseFurnace:
                break;
            default:
                break;
        }
    }













    void TimeSerialization(int time, Text text)
    {
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);

        text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
    }

}
public enum FurnaceTipType
{
    addFurnace,
    UseFurnace
}

[System.Serializable]
public class RawMaterials
{
    [SerializeField]
    public Button btn_rawMaterials;
    [SerializeField]
    public Image propImage;
    [SerializeField]
    public Image propQuality;
    [SerializeField]
    public Image noPorp;
}

[System.Serializable]
public class DrawFurnaceMenu
{
    [SerializeField]
    public Button menu;
    [SerializeField]
    public Text menuNumber;
    [SerializeField]
    public GameObject menuNull;
    [SerializeField]
    public GameObject something;
    [SerializeField]
    public Text menuTime;
    [SerializeField]
    public Slider menuSlider;
}
