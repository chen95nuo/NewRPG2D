using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIFurnace : MonoBehaviour
{


    public DrawFurnaceMenu[] furnacesMenu;

    public Button addFurnace;
    public Button[] popUp;
    public GameObject typeTime;
    public Text time;
    public Slider timeSlider;

    public GameObject typeStart;
    public Text goldCoin;
    public Text needTime;
    public Button start;

    public Button typeEnd;

    public Button rawMaterial;
    public RawMaterials[] rawMaterials;

    private PlayerData playerData;

    private int currentMenu = 0;
    private int currentButton = 0;

    private int chickCoin = 0;
    private int chickTime = 0;

    public GameObject TipGO;
    public Button isTrue;
    public Button isFalse;
    public Text Tip;
    private FurnaceTipType tipType;

    public GameObject equipTipGO;
    public UIBagGrid equipGrid;
    public Button equipIsTrue;
    public Text equipTip;

    public UIFurnacePopUp furnacePopUp;
    public FurnaceData temporaryData;

    private Image timeSliderImage;

    private void Awake()
    {
        Init();//初始化

        UIEventManager.instance.AddListener<ItemData>(UIEventDefineEnum.UpdatePropsEvent, AddMaterial);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateFurnaceEvent, RemoveMaterial);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateFurnaceMenuEvent, UpdateFurnaceMenu);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<ItemData>(UIEventDefineEnum.UpdatePropsEvent, AddMaterial);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateFurnaceEvent, RemoveMaterial);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateFurnaceMenuEvent, UpdateFurnaceMenu);
    }

    private void Update()
    {
        for (int i = 0; i < playerData.Furnace.Count; i++)
        {
            if (playerData.Furnace[i].FurnaceType == FurnaceType.Run)
            {
                TimeSpan sp = DateTime.FromFileTime(playerData.Furnace[i].EndTime).Subtract(SystemTime.insatnce.GetTime());
                TimeSerialization(sp.Seconds, furnacesMenu[i].menuTime);
                float index = (playerData.Furnace[i].NeedTime - sp.Seconds) / (float)playerData.Furnace[i].NeedTime;
                furnacesMenu[i].menuSlider.value = index;
                if (sp.Seconds <= 0)
                {
                    TimeSerialization(0, furnacesMenu[i].menuTime);
                    playerData.Furnace[i].FurnaceType = FurnaceType.End;
                    if (currentMenu == i)
                    {
                        UpdateFurnace();
                    }
                }
                furnacesMenu[i].menuSlider.fillRect.GetComponent<Image>().color = Color.white;
            }
            else if (playerData.Furnace[i].FurnaceType == FurnaceType.End)
            {
                //进度条满值修改颜色
                furnacesMenu[i].menuSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
        }

        if (playerData.Furnace[currentMenu].FurnaceType == FurnaceType.Run)
        {
            time.text = furnacesMenu[currentMenu].menuTime.text;
            timeSlider.value = furnacesMenu[currentMenu].menuSlider.value;
            timeSliderImage.color = Color.white;
        }
        else if (playerData.Furnace[currentMenu].FurnaceType == FurnaceType.End)
        {
            //重新同步时间和进度条
            timeSlider.value = timeSlider.maxValue;
            time.text = furnacesMenu[currentMenu].menuTime.text;
            //进度条满值修改颜色
            timeSliderImage.color = Color.green;
        }
    }

    public void Init()
    {
        temporaryData = new FurnaceData(new ItemData[4], new FurnacePopUpMaterial[6]);

        timeSliderImage = timeSlider.fillRect.GetComponent<Image>();

        for (int i = 0; i < furnacesMenu.Length; i++)
        {
            furnacesMenu[i].menu.onClick.AddListener(ChickMenu);
        }

        TipGO.SetActive(false);
        equipTipGO.SetActive(false);
        start.interactable = false;
        start.onClick.AddListener(RunStart);
        isTrue.onClick.AddListener(TipTure);
        isFalse.onClick.AddListener(TipFalse);
        typeEnd.onClick.AddListener(GetFurnaceItem);
        equipIsTrue.onClick.AddListener(TipTure);

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
    /// 刷新熔炉菜单栏
    /// </summary>
    public void UpdateFurnaceMenu()
    {
        if (playerData.Furnace.Count == furnacesMenu.Length)
        {
            addFurnace.gameObject.SetActive(false);
        }

        addFurnace.onClick.AddListener(AddFurnace);

        for (int i = 0; i < furnacesMenu.Length; i++)
        {
            if (i < playerData.Furnace.Count)
            {
                furnacesMenu[i].menu.gameObject.SetActive(true);
                furnacesMenu[i].menuNumber.text = (playerData.Furnace[i].Id + 1).ToString();

                switch (playerData.Furnace[i].FurnaceType)
                {
                    case FurnaceType.Nothing:
                        furnacesMenu[i].menuNull.SetActive(true);
                        furnacesMenu[i].something.SetActive(false);
                        break;
                    case FurnaceType.Run:
                        furnacesMenu[i].something.SetActive(true);
                        furnacesMenu[i].menuNull.SetActive(false);
                        furnacesMenu[i].menuTime.text = playerData.Furnace[i].NeedTime.ToString();
                        //Silder
                        break;
                    case FurnaceType.End:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                furnacesMenu[i].menu.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 更新炉子内容 如开始按钮 获取按钮 时间 进度条
    /// </summary>
    public void UpdateFurnace()
    {
        //如果当前熔炉正在运行 或 运行完毕 
        if (playerData.Furnace[currentMenu].FurnaceType == FurnaceType.Run || playerData.Furnace[currentMenu].FurnaceType == FurnaceType.End)
        {
            //刷新熔炉里的四个格子 获取格子中的物品
            for (int i = 0; i < rawMaterials.Length; i++)
            {
                rawMaterials[i].noPorp.gameObject.SetActive(false);
                rawMaterials[i].propQuality.gameObject.SetActive(true);
                rawMaterials[i].btn_rawMaterials.interactable = false;
                rawMaterials[i].propImage.sprite = IconMgr.Instance.GetIcon(playerData.Furnace[currentMenu].Material[i].SpriteName);
                rawMaterials[i].propQuality.sprite = IconMgr.Instance.GetIcon("quality_" + playerData.Furnace[currentMenu].Material[i].Quality);
            }
            switch (playerData.Furnace[currentMenu].FurnaceType)
            {
                case FurnaceType.Run:
                    typeTime.SetActive(true);
                    typeStart.SetActive(false);
                    typeEnd.gameObject.SetActive(false);
                    break;
                case FurnaceType.End:
                    typeTime.SetActive(false);
                    typeStart.SetActive(false);
                    typeEnd.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }

            furnacePopUp.UpdatePopUp(playerData.Furnace[currentMenu]);
        }
        //如当前熔炉未运行 则
        else
        {
            for (int i = 0; i < rawMaterials.Length; i++)
            {
                rawMaterials[i].propQuality.gameObject.SetActive(false);
                rawMaterials[i].noPorp.gameObject.SetActive(true);
                rawMaterials[i].btn_rawMaterials.interactable = true;
            }
            typeTime.SetActive(false);
            typeStart.SetActive(true);
            typeEnd.gameObject.SetActive(false);
            ChickGoldCoin();
            furnacePopUp.UpdatePopUp(temporaryData);
            furnacePopUp.Restart();
        }
        ChickMaterial();
    }
    //检查菜单
    public void ChickMenu()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < furnacesMenu.Length; i++)
        {
            furnacesMenu[i].menu.interactable = true;
            if (go == furnacesMenu[i].menu.gameObject && go != furnacesMenu[currentMenu].menu.gameObject)
            {
                //如果当前选择的熔炉不是正在使用的熔炉,如果上面没有材料,那么清空材料列表
                Debug.Log("切换熔炉");
                RemoveMaterial();
                //先将材料返还在重置数据
                temporaryData = new FurnaceData(new ItemData[4], new FurnacePopUpMaterial[6]);

                ChickMaterial();
                currentMenu = i;
                furnacesMenu[i].menu.interactable = false;

                UpdateFurnace();
            }
        }
    }
    //检查材料栏
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
        for (int i = 0; i < temporaryData.Material.Length; i++)
        {
            if (temporaryData.Material[i] != null)
            {
                if (temporaryData.Material[i].ItemType != ItemType.Nothing)
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

    public void ChickGoldCoin()
    {
        chickCoin = 0;
        chickTime = 0;
        for (int i = 0; i < temporaryData.Material.Length; i++)
        {
            if (temporaryData.Material[i] != null)
            {
                chickCoin += temporaryData.Material[i].UsePrice;
                chickTime += (temporaryData.Material[i].Quality);
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

        if (temporaryData.Material[currentButton] == null || temporaryData.Material[currentButton].ItemType == ItemType.Nothing)
        {
            Debug.Log("该栏物品为空,直接添加");
            data.Number--;
            rawMaterials[currentButton].noPorp.gameObject.SetActive(false);
            rawMaterials[currentButton].propQuality.gameObject.SetActive(true);
            rawMaterials[currentButton].propImage.sprite = IconMgr.Instance.GetIcon( data.SpriteName);
            rawMaterials[currentButton].propQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
            ItemData newData = new ItemData(GamePropData.Instance.GetItem(data.Id));
            newData.Number = 1;
            temporaryData.Material[currentButton] = newData;
        }
        else
        {
            //如果该材料栏已有物品 那么 将该物品返还背包 将当前点击的物品移动上来
            Debug.Log("该栏有物品" + temporaryData.Material[currentButton].Name);
            Debug.Log("切换物品,返还道具");
            List<ItemData> Temp = BagItemData.Instance.GetItems(temporaryData.Material[currentButton].Id);
            int index = 0;
            //检查有多少个相同的道具
            for (int j = 0; j < Temp.Count; j++)
            {
                //如果背包里有该相同道具，且数量不满99 则该组道具加一
                if (Temp[j].Number + 1 <= 99)
                {
                    Temp[j].Number++;
                    temporaryData.Material[currentButton] = new ItemData(ItemType.Nothing);
                    break;
                }
                else
                {
                    index++;
                }
            }
            //如果这个背包中所有的该道具的总数都超过99或者背包中已经没有这个道具了 新建一个这个道具
            if (index >= Temp.Count)
            {
                index = 0;
                ItemData newData = new ItemData(temporaryData.Material[currentButton]);
                BagItemData.Instance.AddItem(newData);
                newData.Number = 1;
            }
            //改变道具
            data.Number--;
            rawMaterials[currentButton].noPorp.gameObject.SetActive(false);
            rawMaterials[currentButton].propQuality.gameObject.SetActive(true);
            rawMaterials[currentButton].propImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
            rawMaterials[currentButton].propQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
            ItemData newData_1 = new ItemData(GamePropData.Instance.GetItem(data.Id));
            newData_1.Number = 1;
            temporaryData.Material[currentButton] = newData_1;
        }
        ChickGoldCoin();
        ChickMaterial();
        furnacePopUp.UpdatePopUp(temporaryData);

    }

    /// <summary>
    /// 返回时将材料列表的材料释放
    /// </summary>
    public void RemoveMaterial()
    {
        ItemData[] items = temporaryData.Material;
        Debug.Log(items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            Debug.Log(items[i].ItemType);
            if (items[i].ItemType != ItemType.Nothing)//如果当前位置不是空的
            {
                //如果在运行 那么只是清除当前格子上的物品
                if (playerData.Furnace[currentMenu].FurnaceType == FurnaceType.Run || playerData.Furnace[currentMenu].FurnaceType == FurnaceType.End)
                {
                }
                //如果不在运行，那么返回改熔炉的道具
                else
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
                    //刷新熔炉材料栏
                    UpdateFurnace();

                }
            }
            else
            {
                Debug.Log(items[i].ItemType);
            }
        }
    }

    public void RunStart()
    {
        tipType = FurnaceTipType.UseFurnace;
        TipType();
    }

    public void AddFurnace()
    {
        tipType = FurnaceTipType.addFurnace;
        TipType();
    }
    //弹出提示框
    public void TipType()
    {
        switch (tipType)
        {
            case FurnaceTipType.addFurnace:
                TipGO.SetActive(true);
                if (playerData.GoldCoin - (2000 * playerData.Furnace.Count) >= 0)
                {
                    Tip.text = "<color=#FFFFFF>是否花费 " + (2000 * playerData.Furnace.Count) + " 金币开启新的熔炉</color>";
                    isTrue.interactable = true;
                }
                else
                {
                    Tip.text = "<color=#FF0000>还需要" + ((2000 * playerData.Furnace.Count) - playerData.GoldCoin) + "金币才可使用</color>";
                    isTrue.interactable = false;
                }
                break;
            case FurnaceTipType.UseFurnace:
                TipGO.SetActive(true);
                if (playerData.GoldCoin - chickCoin >= 0)
                {
                    Tip.text = "<color=#FFFFFF>是否花费 " + chickCoin + " 金币开始熔炼</color>";
                    isTrue.interactable = true;
                }
                else
                {
                    Tip.text = "<color=#FF0000>还需要" + (chickCoin - playerData.GoldCoin) + "金币才可使用</color>";
                    isTrue.interactable = false;
                }
                break;
            case FurnaceTipType.getEquip:
                Debug.Log("打开奖励面板");
                equipTipGO.SetActive(true);
                switch (equipGrid.equipData.Quality)
                {
                    case 1:
                        equipTip.text = "恭喜获得" + "\"<color=#FFFFFF>" + equipGrid.equipData.Name + "</color>\"";
                        break;
                    case 2:
                        equipTip.text = "恭喜获得" + "\"<color=#B1694E>" + equipGrid.equipData.Name + "</color>\"";
                        break;
                    case 3:
                        equipTip.text = "恭喜获得" + "\"<color=#82BDC8>" + equipGrid.equipData.Name + "</color>\"";
                        break;
                    case 4:
                        equipTip.text = "恭喜获得" + "\"<color=#FF7000>" + equipGrid.equipData.Name + "</color>\"";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
    //点击确认
    public void TipTure()
    {
        switch (tipType)
        {
            case FurnaceTipType.addFurnace:
                playerData.GoldCoin -= (2000 * playerData.Furnace.Count);
                playerData.Furnace.Add(new FurnaceData(playerData.Furnace.Count, new ItemData[4], new FurnacePopUpMaterial[6]));
                UpdateFurnaceMenu();
                break;
            case FurnaceTipType.UseFurnace:
                playerData.GoldCoin -= chickCoin;
                //开启熔炼
                StartFurnace();
                //更新道具栏
                UpdateFurnace();

                break;
            case FurnaceTipType.getEquip:
                //点击确认将装备添加到装备栏
                BagEquipData.Instance.AddItem(equipGrid.equipData);
                playerData.Furnace[currentMenu] = new FurnaceData(currentMenu, new ItemData[4], new FurnacePopUpMaterial[6]);
                UpdateFurnace();
                UpdateFurnaceMenu();
                break;
            default:
                break;
        }
        TipGO.SetActive(false);
        equipTipGO.SetActive(false);
    }
    public void TipFalse()
    {
        TipGO.SetActive(false);
    }

    /// <summary>
    /// 开始熔炼
    /// </summary>
    public void StartFurnace()
    {
        //保存气泡的位置
        furnacePopUp.SavePopUpPoint(temporaryData);
        //记录当前时间和结束时间
        temporaryData.StartTime = SystemTime.insatnce.GetTime().ToFileTime();
        temporaryData.EndTime = SystemTime.insatnce.GetTime().AddSeconds(chickTime).ToFileTime();
        //将当前信息赋予改熔炉数据
        playerData.Furnace[currentMenu] = new FurnaceData(currentMenu, temporaryData);
        temporaryData = new FurnaceData(new ItemData[4], new FurnacePopUpMaterial[6]);
        //当前熔炉开始运行
        playerData.Furnace[currentMenu].FurnaceType = FurnaceType.Run;

        UpdateFurnace();
    }

    /// <summary>
    /// 获取熔炉内的装备
    /// </summary>
    public void GetFurnaceItem()
    {
        int[] data = new int[9];
        //匹配材料
        for (int i = 0; i < playerData.Furnace[currentMenu].PopPoint.Length; i++)
        {
            switch (playerData.Furnace[currentMenu].PopPoint[i].materialType)
            {
                case ItemMaterialType.Nothing:
                    data[0] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Iron:
                    data[1] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Wood:
                    data[2] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Leatherwear:
                    data[3] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Cloth:
                    data[4] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Magic:
                    data[5] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Diamonds:
                    data[6] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Stone:
                    data[7] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                case ItemMaterialType.Rubber:
                    data[8] += playerData.Furnace[currentMenu].PopPoint[i].materialNumber;
                    break;
                default:
                    break;
            }
        }
        int[] data_2 = new int[4];
        //匹配必要道具
        for (int i = 0; i < playerData.Furnace[currentMenu].Material.Length; i++)
        {
            data_2[i] = playerData.Furnace[currentMenu].Material[i].Id;
        }
        List<ComposedTableData> tempData = GameComposedTableData.Instance.GetTables(new ComposedTableData(data, data_2));
        Debug.Log(tempData.Count);
        //匹配权重
        if (tempData.Count > 0)
        {
            int index = 0;
            for (int i = 0; i < tempData.Count; i++)
            {
                index += tempData[i].Weight;
            }
            int roll = UnityEngine.Random.Range(0, 100);
            float[] temp = new float[tempData.Count];
            //记录装备总权重
            for (int i = 0; i < tempData.Count; i++)
            {
                float temp_2 = (tempData[i].Weight / (float)index) * 100;
                if (i <= 0)
                {
                    temp[i] = temp_2;
                }
                else
                {
                    temp[i] = temp_2 + temp[i - 1];
                }
            }
            //获取达标的权重
            for (int i = 0; i < temp.Length; i++)
            {
                //得出装备
                if (temp[i] > roll)
                {
                    //打开奖励面板 将该物品添加进背包
                    equipGrid.UpdateItem(GameEquipData.Instance.GetItem(tempData[i].Id));
                    tipType = FurnaceTipType.getEquip;
                    TipType();
                    break;
                }
            }
        }
        else
        {
            //打开奖励面板 将该物品添加进背包
            equipGrid.UpdateItem(GameEquipData.Instance.GetItem(1));
            tipType = FurnaceTipType.getEquip;
            TipType();
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
    UseFurnace,
    getEquip
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
