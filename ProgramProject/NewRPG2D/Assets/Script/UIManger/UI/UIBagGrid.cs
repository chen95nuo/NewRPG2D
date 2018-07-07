using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagGrid : MonoBehaviour
{

    public int itemID = -1;
    public ItemType itemType;

    public GameObject type;



    #region 获取各种信息......

    public GameObject egg;
    public Image eggBG;//边框
    public Image eggImage;//图片
    public Image eggStars;//星星
    public Image eggAttribute;//属性
    public Text eggNumber;//数量
    public Text eggNeedTime;//孵化时间
    public GameObject other;
    public Image otherBG;//边框
    public Image otherImage;//图片
    public Text otherNumber;//数量
    public int hatchingTime;//孵化时间
    public int stars;//星星数量
    public int quality;//稀有度
    public int isUse;//可否使用
    public EquipType equipType;//武器类型

    #endregion

    private Button chickButton;

    // Use this for initialization
    void Awake()
    {
        UpdateItem(-1, ItemType.Nothing);
        chickButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateItem(int itemID, ItemType type)
    {
        this.itemID = itemID;
        this.type.gameObject.SetActive(false);
        itemType = type;
        hatchingTime = 0;
        stars = 0;
        quality = 0;
        isUse = 0;
        equipType = 0;
    }

    public void UpdateItem(EggData data)
    {
        itemID = data.Id;
        itemType = data.ItemType;
        type.gameObject.SetActive(true);

        egg.SetActive(true);
        other.SetActive(false);
        eggBG.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        eggAttribute.sprite = Resources.Load<Sprite>("UITexture/Icon/egg/attribute/" + data.Attribute);
        eggNumber.text = data.ItemNumber.ToString();
        eggImage.sprite = Resources.Load<Sprite>("UITexture/Icon/egg/" + data.Id + "/" + data.Name);
        eggStars.sprite = Resources.Load<Sprite>("UITexture/Icon/stars/" + data.StarsLevel);
        HatchTime((int)data.HatchingTime, eggNeedTime);

        hatchingTime = (int)data.HatchingTime;
        stars = data.StarsLevel;
    }

    public void UpdateItem(ItemData data)
    {
        itemID = data.Id;
        itemType = data.ItemType;
        type.gameObject.SetActive(true);

        egg.SetActive(false);
        other.SetActive(true);
        otherBG.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        otherImage.sprite = Resources.Load<Sprite>("UITexture/Icon/prop/" + data.SpriteName);
        otherNumber.gameObject.SetActive(true);
        otherNumber.text = data.Number.ToString();

        quality = data.Quality;
        isUse = data.PropType;
    }
    public void UpdateItem(EquipData data)
    {
        itemID = data.Id;
        itemType = data.ItemType;
        type.gameObject.SetActive(true);

        egg.SetActive(false);
        other.SetActive(true);
        otherBG.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Quality);
        otherImage.sprite = Resources.Load<Sprite>("UITexture/Icon/equip/" + data.SpriteName);
        otherNumber.gameObject.SetActive(false);

        quality = data.Quality;
        equipType = data.EquipType;
    }


    void HatchTime(int time, Text text)
    {
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);

        text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
    }

}
