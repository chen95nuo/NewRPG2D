using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIBagGrid : MonoBehaviour
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

    private Button chickButton;

    // Use this for initialization
    void Awake()
    {
        chickButton = GetComponent<Button>();


    }
    void Start()
    {
        if (itemType == ItemType.Role && gridType != GridType.Use)
        {
            chickButton.onClick.AddListener(ShowRolePage);
            return;
        }
        else if (itemType == ItemType.Role && gridType == GridType.Use)
        {
            chickButton.onClick.AddListener(ShowMaterialHouse);
            return;
        }
        else if (itemID >= 0 && itemType != ItemType.Role)
        {
            chickButton.onClick.AddListener(TTUIPage.ShowPage<UIBagItemMessage>);
            return;
        }
        UpdateItem(-1, ItemType.Nothing);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ShowRolePage()
    {
        TTUIPage.ShowPage<UIRolePage>();
    }

    public void ShowMaterialHouse()
    {
        if (!roleData.Fighting)
        {
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMaterialEvent, roleData);
        }
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
        eggGrid.eggBG.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        eggGrid.eggAttribute.sprite = Resources.Load<Sprite>("UITexture/Icon/attribute/" + data.Attribute);
        eggGrid.eggNumber.text = data.ItemNumber.ToString();
        eggGrid.eggImage.sprite = Resources.Load<Sprite>("UITexture/Icon/egg/" + data.Name);
        eggGrid.eggStars.sprite = Resources.Load<Sprite>("UITexture/Icon/stars/" + data.StarsLevel);
        HatchTime((int)data.HatchingTime, eggGrid.eggNeedTime);

        hatchingTime = (int)data.HatchingTime;
        stars = data.StarsLevel;
    }

    public void UpdateItem(ItemData data)
    {
        propData = data;
        itemID = data.Id;
        itemType = data.ItemType;
        type.gameObject.SetActive(true);

        eggGrid.egg.SetActive(false);
        otherGrid.other.SetActive(true);
        otherGrid.otherBG.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        otherGrid.otherImage.sprite = Resources.Load<Sprite>("UITexture/Icon/prop/" + data.SpriteName);
        otherGrid.otherNumber.gameObject.SetActive(true);
        otherGrid.otherNumber.text = data.Number.ToString();

        quality = data.Quality;
        propType = (int)data.PropType;
    }
    public void UpdateItem(EquipData data)
    {
        equipData = data;
        itemID = data.Id;
        itemType = data.ItemType;
        type.gameObject.SetActive(true);

        eggGrid.egg.SetActive(false);
        otherGrid.other.SetActive(true);
        otherGrid.otherBG.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        otherGrid.otherImage.sprite = Resources.Load<Sprite>("UITexture/Icon/equip/" + data.SpriteName);
        otherGrid.otherNumber.gameObject.SetActive(false);

        quality = data.Quality;
        equipType = data.EquipType;
    }
    public void UpdateItem(CardData data)
    {
        roleData = data;
        itemID = data.Id;
        itemType = data.ItemType;

        roleGrid.roleBG.sprite = Resources.Load<Sprite>("UITexture/Icon/roleQuality/" + data.Quality);
        roleGrid.roleImage.sprite = Resources.Load<Sprite>("UITexture/Icon/role/" + data.Name);
        roleGrid.roleStars.sprite = Resources.Load<Sprite>("UITexture/Icon/stars/" + data.Stars);
        roleGrid.roleAttribute.sprite = Resources.Load<Sprite>("UITexture/Icon/attribute/" + data.Attribute);
        if (data.TypeMessage == null)
            roleGrid.roleTypeBG.gameObject.SetActive(false);
        else
        {
            roleGrid.roleTypeBG.gameObject.SetActive(true);
            roleGrid.roleType.text = data.TypeMessage;
        }
        if (data.Fighting)
        {
            roleGrid.roleTypeBG.gameObject.SetActive(true);
            roleGrid.roleType.text = "探险中...";
        }
        roleGrid.roleLevel.text = data.Level.ToString();

        level = data.Level;
        stars = data.Stars;
        grow = data.Quality;
        goodFeeling = data.GoodFeeling;
    }


    void HatchTime(int time, Text text)
    {
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);

        text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
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
}
