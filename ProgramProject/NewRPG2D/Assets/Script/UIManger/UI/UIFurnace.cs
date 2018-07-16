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

    private int currentMenu;
    private int currentButton;

    private void Awake()
    {
        AwakeInitialization();//初始化

        UIEventManager.instance.AddListener<ItemData>(UIEventDefineEnum.UpdatePropsEvent, AddMaterial);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateFurnaceEvent, RemoveMaterial);
    }

    private void Start()
    {


    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<ItemData>(UIEventDefineEnum.UpdatePropsEvent, AddMaterial);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateFurnaceEvent, RemoveMaterial);

    }

    public void AwakeInitialization()
    {
        for (int i = 0; i < rawMaterials.Length; i++)
        {
            rawMaterials[i].btn_rawMaterials.onClick.AddListener(ChickButton);
            rawMaterials[i].propQuality.gameObject.SetActive(false);
            rawMaterials[i].noPorp.gameObject.SetActive(true);
        }
        playerData = GetPlayData.Instance.GetPlayerData(0);
        TimeSerialization(0, needTime);
        goldCoin.text = "0";
        if (playerData.Furnace.Count == furnaces.Length)
        {
            addFurnace.gameObject.SetActive(false);
        }
        for (int i = 0; i < furnaces.Length; i++)
        {
            furnaces[i].menu.onClick.AddListener(ChickMenu);
            if (i < playerData.Furnace.Count)
            {
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
    public void ChickMenu()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < furnaces.Length; i++)
        {
            furnaces[i].menu.interactable = true;
            if (go == furnaces[i].menu.gameObject)
            {
                currentMenu = i;
                furnaces[i].menu.interactable = false;
                //关闭当前按钮的功能,返还当前材料栏中的物品,清空材料栏，显示当前熔炉的信息

            }
        }

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

    /// <summary>
    /// 将材料添加到当前点击的位置上
    /// </summary>
    /// <param name="data">材料信息</param>
    public void AddMaterial(ItemData data)
    {
        rawMaterials[currentButton].noPorp.gameObject.SetActive(false);
        rawMaterials[currentButton].propQuality.gameObject.SetActive(true);
        rawMaterials[currentButton].propImage.sprite = Resources.Load<Sprite>("UITexture/Icon/prop/" + data.SpriteName);
        rawMaterials[currentButton].propQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        playerData.Furnace[currentMenu].Material[currentButton] = data;
    }

    /// <summary>
    /// 返回时将次材料列表的材料释放
    /// </summary>
    public void RemoveMaterial()
    {
        AwakeInitialization();

        ItemData[] items = playerData.Furnace[currentMenu].Material;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Id > 0)
            {
                for (int j = 0; j < BagItemData.Instance.GetItems(items[i].Id).Count; j++)
                {
                    if (BagItemData.Instance.GetItems(items[i].Id)[j].Number + 1 < 99)
                    {
                        BagItemData.Instance.GetItems(items[i].Id)[j].Number += 1;
                    }
                    else
                    {
                        if (BagItemData.Instance.GetItems(items[i].Id)[j + 1] == null)
                        {
                            BagItemData.Instance.AddItem(items[i]);
                        }
                    }
                }

            }
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
