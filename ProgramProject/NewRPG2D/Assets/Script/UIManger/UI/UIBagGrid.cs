using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBagGrid : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
{

    public int itemID = -1;
    public ItemType itemType;
    public GridType gridType;

    public GameObject type;

    public EggData eggData;
    public ItemData propData;
    public EquipData equipData;
    public CardData roleData;

    #region 获取各种信息......

    public BagEggGrid eggGrid;
    public BagOtherGrid otherGrid;
    public BagRoleGrid roleGrid;

    [System.NonSerialized]
    public int hatchingTime;//孵化时间
    [System.NonSerialized]
    public int stars;//星星数量
    [System.NonSerialized]
    public int quality;//稀有度
    [System.NonSerialized]
    public int propType;//可否使用
    [System.NonSerialized]
    public EquipType equipType;//武器类型
    [System.NonSerialized]
    public int grow;//成长
    [System.NonSerialized]
    public int level;//等级
    [System.NonSerialized]
    public int goodFeeling;//好感度

    #endregion

    public StoreGrid storeGrid;

    private Button chickButton;
    private bool isPointerDown = false;

    // Use this for initialization
    void Awake()
    {
        chickButton = GetComponent<Button>();
        if (gridType == GridType.NoClick)
        {
            chickButton.interactable = false;
        }
        chickButton.onClick.AddListener(ShowItemMessage);

        if (gridType == GridType.Store)
        {
            storeGrid.btn_But.onClick.AddListener(ShowBuyPage);
        }
    }
    void Start()
    {
        if (itemType == ItemType.Role)
        {
            chickButton.onClick.AddListener(ShowRolePage);
            return;
        }
        else if (itemType == ItemType.Nothing)
        {
            UpdateItem(-1, ItemType.Nothing);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ShowItemMessage()
    {
        if (itemID > 0 && itemType != ItemType.Role && itemType != ItemType.Prop && itemType != ItemType.Egg)
        {
            TTUIPage.ShowPage<UIBagItemMessage>();
        }
        else if (itemType == ItemType.Prop)
        {
            switch (gridType)
            {
                case GridType.Nothing:
                    TTUIPage.ShowPage<UIBagItemMessage>();
                    break;
                case GridType.Use:
                    UseProp();
                    break;
                case GridType.Store:
                    BuyProp();
                    break;
                default:
                    break;
            }
        }
        else if (itemType == ItemType.Role && gridType == GridType.Explore)
        {
            BackExplore();
        }
        else if (itemType == ItemType.Egg && gridType == GridType.Use)
        {
            UseEgg();
        }

    }
    public void ShowRolePage()
    {
        switch (gridType)
        {
            case GridType.Nothing:
                TTUIPage.ShowPage<UIRolePage>();
                UIEventManager.instance.SendEvent<bool>(UIEventDefineEnum.UpdateRolesEvent, false);
                RoundCardMessage data = new RoundCardMessage(roleData, GridType.Nothing);
                UIEventManager.instance.SendEvent<RoundCardMessage>(UIEventDefineEnum.UpdateCardMessageEvent, data);
                break;
            case GridType.Use:
                if (!roleData.Fighting)
                {
                    TTUIPage.ClosePage<UICardHousePage>();
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMaterialEvent, roleData);
                }
                break;
            case GridType.Store:
                break;
            case GridType.Explore:
                BackExplore();
                break;
            case GridType.Team:
                Debug.Log("进入小队菜单");
                //选中的
                TTUIPage.ClosePage<UICardHousePage>();
                UIEventManager.instance.SendEvent<CardData>(UIEventDefineEnum.UpdateRoundEvent, roleData);
                break;
            default:
                break;
        }
    }

    public void ShowBuyPage()
    {
        TTUIPage.ShowPage<UIBusinessTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateBuyItem, this);
        UIStore store = GetComponentInParent<UIStore>();
        //store.OutPage();
        UIEventManager.instance.SendEvent<GameObject>(UIEventDefineEnum.UpdateStoreEvent, store.gameObject);
    }
    public void UseProp()
    {
        if (itemType != ItemType.Nothing)
        {
            if (propData.Number > 0)
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent, propData);
            }
            TTUIPage.ClosePage<UIUseItemBagPage>();
            UIEventManager.instance.SendEvent<bool>(UIEventDefineEnum.UpdateUsePage, false);
        }
    }
    public void BuyProp()
    {
        Debug.Log("这是一件商品");
    }

    /// <summary>
    /// 孵化蛋
    /// </summary>
    public void UseEgg()
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateHatcheryEvent, eggData);
        TTUIPage.ClosePage<UIUseItemBagPage>();
        Debug.Log("Close");

        UIEventManager.instance.SendEvent<bool>(UIEventDefineEnum.UpdateUsePage, false);

    }

    public void BackExplore()
    {
        UIEventManager.instance.SendEvent<CardData>(UIEventDefineEnum.UpdateExploreEvent, roleData);
        TTUIPage.ClosePage<UICardHousePage>();
    }

    public void UpdateItem(int itemID, ItemType type)
    {
        this.itemID = itemID;
        this.type.gameObject.SetActive(false);
        itemType = type;
        hatchingTime = 0;
        stars = 0;
        quality = 0;
        propType = 0;
        equipType = 0;
    }

    public void UpdateItem(EggData data)
    {
        eggData = data;
        itemID = data.Id;
        itemType = data.ItemType;
        type.gameObject.SetActive(true);

        eggGrid.egg.SetActive(true);
        otherGrid.other.SetActive(false);
        eggGrid.eggBG.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        eggGrid.eggAttribute.sprite = IconMgr.Instance.GetIcon(data.Attribute);
        eggGrid.eggNumber.text = data.ItemNumber.ToString();
        eggGrid.eggImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        eggGrid.eggImage.SetNativeSize();
        eggGrid.eggStars.sprite = IconMgr.Instance.GetIcon("Stars_" + data.StarsLevel);
        HatchTime((int)data.HatchingTime, eggGrid.eggNeedTime);

        hatchingTime = (int)data.HatchingTime;
        stars = data.StarsLevel;
    }

    public void UpdateItem(ItemData data)
    {
        type.gameObject.SetActive(true);
        propData = data;
        itemID = data.Id;
        itemType = data.ItemType;
        eggGrid.egg.SetActive(false);
        otherGrid.other.SetActive(true);
        if (otherGrid.otherBG != null)
        {
            otherGrid.otherBG.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        }
        otherGrid.otherImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        //otherGrid.otherImage.SetNativeSize();
        otherGrid.otherNumber.gameObject.SetActive(true);
        otherGrid.otherNumber.text = data.Number.ToString();
        if (otherGrid.otherNumberBG != null)
        {
            otherGrid.otherNumberBG.gameObject.SetActive(true);
        }
        quality = data.Quality;
        propType = (int)data.PropType;

        if (storeGrid.priceImage != null)
        {
            switch (data.StorePropType)
            {
                case CurrencyType.Nothing:
                    break;
                case CurrencyType.GoldCoin:
                    storeGrid.price.text = "<color=#E7BE2F>" + data.BuyPrice + "</color>";
                    storeGrid.priceImage.sprite = GetSpriteAtlas.insatnce.GetIcon("Cry_Icon_GoldImage");
                    break;
                case CurrencyType.Diamonds:
                    storeGrid.price.text = "<color=#79D2FF>" + data.BuyPrice + "</color>";
                    storeGrid.priceImage.sprite = GetSpriteAtlas.insatnce.GetIcon("Cry_Icon_DiamondsImage");
                    break;
                default:
                    break;
            }
            storeGrid.name.text = data.Name;
            storeGrid.ItemDescribe.text = data.Describe;
        }
    }
    public void UpdateItem(EquipData data)
    {
        type.gameObject.SetActive(true);
        equipData = data;
        itemID = data.Id;
        itemType = data.ItemType;

        eggGrid.egg.SetActive(false);
        otherGrid.other.SetActive(true);
        if (otherGrid.otherBG != null)
        {
            otherGrid.otherBG.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        }
        otherGrid.otherImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        //otherGrid.otherImage.SetNativeSize();
        if (otherGrid.otherNumberBG != null)
        {
            otherGrid.otherNumberBG.gameObject.SetActive(false);
        }
        otherGrid.otherNumber.gameObject.SetActive(false);

        quality = data.Quality;
        equipType = data.EquipType;
    }
    public void UpdateItem(CardData data)
    {
        roleData = data;
        itemID = data.Id;
        itemType = data.ItemType;

        roleGrid.roleBG.sprite = IconMgr.Instance.GetIcon("roleQuality_" + data.Quality);
        roleGrid.roleImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        roleGrid.roleImage.SetNativeSize();
        roleGrid.roleStars.sprite = IconMgr.Instance.GetIcon("Stars_" + data.Stars);
        roleGrid.roleAttribute.sprite = IconMgr.Instance.GetIcon("Att_" + data.Attribute);
        if (data.TeamType == TeamType.Nothing)
            roleGrid.roleTypeBG.gameObject.SetActive(false);
        else
        {
            roleGrid.roleTypeBG.gameObject.SetActive(true);
            roleGrid.roleType.text = data.TeamType.ToString();
        }
        if (data.Fighting)
        {
            roleGrid.roleTypeBG.gameObject.SetActive(true);
            roleGrid.roleType.text = "探险中...";
        }
        roleGrid.roleLevel.text = "LV." + data.Level.ToString();

        level = data.Level;
        stars = data.Stars;
        grow = data.Quality;
        goodFeeling = data.GoodFeeling;
    }
    public void UpdateItem(DroppingData data)
    {
        propData = GamePropData.Instance.GetItem(data.Id);
        otherGrid.otherImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        otherGrid.otherBG.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        type.SetActive(true);
    }


    void HatchTime(int time, Text text)
    {
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);

        text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (itemType == ItemType.Prop)
        {
            switch (gridType)
            {
                case GridType.Nothing:
                    break;
                case GridType.Use:
                    break;
                case GridType.Store:
                    break;
                case GridType.Explore:
                    Debug.Log("显示小气泡");
                    TTUIPage.ShowPage<UILittleTipPage>();
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, propData.Name);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

                    break;
                case GridType.Team:
                    Debug.Log("显示小气泡");
                    TTUIPage.ShowPage<UILittleTipPage>();
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, propData.Name);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

                    break;
                default:
                    break;
            }
        }
        if (itemType == ItemType.Egg && gridType != GridType.Use)
        {
            TTUIPage.ShowPage<UILittleTipPage>();
            if (eggData.IsKnown)
            {

                UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, eggData.Name);
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

            }
            else
            {
                UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, "不知名的蛋");
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, false);
        if (itemType == ItemType.Prop && Input.GetMouseButton(0))
        {
            switch (gridType)
            {
                case GridType.Nothing:
                    break;
                case GridType.Use:
                    break;
                case GridType.Store:
                    break;
                case GridType.Explore:
                    Debug.Log("关闭小气泡");
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent);
                    break;
                case GridType.Team:
                    Debug.Log("关闭小气泡");
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent);
                    break;
                default:
                    break;
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            if (itemType == ItemType.Prop)
            {
                switch (gridType)
                {
                    case GridType.Nothing:
                        break;
                    case GridType.Use:
                        break;
                    case GridType.Store:
                        break;
                    case GridType.Explore:
                        Debug.Log("刷新小气泡");
                        TTUIPage.ShowPage<UILittleTipPage>();
                        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, propData.Name);
                        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

                        break;
                    case GridType.Team:
                        Debug.Log("刷新小气泡");
                        TTUIPage.ShowPage<UILittleTipPage>();
                        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, propData.Name);
                        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

                        break;
                    default:
                        break;
                }
            }
            if (itemType == ItemType.Egg && gridType != GridType.Use)
            {
                TTUIPage.ShowPage<UILittleTipPage>();
                if (eggData.IsKnown)
                {
                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, eggData.Name);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);

                }
                else
                {
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, "不知名的蛋");
                }
            }
        }
    }


    [System.Serializable]
    public class BagEggGrid
    {
        public GameObject egg;
        public Image eggBG;//边框
        public Image eggImage;//图片
        public Image eggStars;//星星
        public Image eggAttribute;//属性
        public Text eggNumber;//数量
        public Text eggNeedTime;//孵化时间
    }
    [System.Serializable]
    public class BagOtherGrid
    {
        public GameObject other;
        public Image otherBG;//边框
        public Image otherImage;//图片
        public Text otherNumber;//数量
        public Image otherNumberBG;//数量背景
    }
    [System.Serializable]
    public class BagRoleGrid
    {
        public Image roleBG;//边框
        public Image roleImage;//图片
        public Image roleStars;//星星
        public Text roleType;//当前状态
        public Image roleTypeBG;//状态背景
        public Text roleLevel;//等级
        public Image roleAttribute;//属性
    }
    [System.Serializable]
    public class StoreGrid
    {
        public Text name;//道具名称
        public Text price;//道具价格
        public Image priceImage;//价格图片
        public Button btn_But;//购买按钮
        public Text ItemDescribe;//道具介绍
    }
}
