using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHatcheryPool : MonoBehaviour
{

    public UIHatcheryGrid[] grids;
    public int currentGrids = 0;
    public PlayerData playerdata;

    public GameObject AddPoolOBJ;
    public HatchAddPoolPage addPool;
    public GameObject AddSpeedOBJ;
    public HatchAddSpeedPage addSpeed;

    private List<HatcheryData> items;
    private int propId = 7; //减少时间道具的ID
    private int speedAdd = 10;//减少的时间
    private int propNumber = 0;//减少时间的道具数量


    private void Awake()
    {

        Init();

        UIEventManager.instance.AddListener<EggData>(UIEventDefineEnum.UpdateHatcheryEvent, UpdateGrid);

        playerdata = GetPlayData.Instance.player[0];
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        AddPoolOBJ.SetActive(false);
        AddSpeedOBJ.SetActive(false);
        addPool.btn_back.onClick.AddListener(AddPoolBack);
        addSpeed.btn_back.onClick.AddListener(AddSpeedBack);
        addPool.btn_Gold.onClick.AddListener(AddPoolForGold);
        addPool.btn_Diamons.onClick.AddListener(AddPoolForDiamons);
        addSpeed.btn_enter.onClick.AddListener(ChickAddSpeedEnter);

        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].UpdateGrid();
            grids[i].btn_hatchery.onClick.AddListener(ChickGrid);
            grids[i].btn_UnLock.onClick.AddListener(ChickUnLoad);
            grids[i].btn_speed.onClick.AddListener(ChickAddSpeed);
        }

        items = PlayerHatcheryData.Instance.items;

        UpdateHatchery();
    }
    /// <summary>
    /// 刷新有效的孵化池
    /// </summary>
    private void UpdateHatchery()
    {
        for (int i = 0; i < items.Count; i++)
        {
            grids[items[i].Id].UpdateGrid(items[i]);
        }
    }
    /// <summary>
    /// 启动孵化池
    /// </summary>
    /// <param name="data"></param>
    private void UpdateHatchery(EggData data)
    {
        grids[currentGrids].UpdateGrid(data);
    }
    /// <summary>
    /// 检查当前格子
    /// </summary>
    public void ChickGrid()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < grids.Length; i++)
        {
            if (grids[i].gameObject == go)
            {
                currentGrids = i;
                break;
            }
        }
        TinyTeam.UI.TTUIPage.ShowPage<UIUseItemBagPage>();

        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateUsePage, ItemType.Egg);
    }
    /// <summary>
    /// 更新格子
    /// </summary>
    /// <param name="data"></param>
    public void UpdateGrid(EggData data)
    {
        grids[currentGrids].UpdateGrid(data);
    }
    /// <summary>
    /// 检查解锁按钮
    /// </summary>
    public void ChickUnLoad()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < grids.Length; i++)
        {
            if (grids[i].btn_UnLock.gameObject == go)
            {
                currentGrids = i;
                break;
            }
        }
        //弹出解锁对话框
        AddPoolOBJ.SetActive(true);
        addPool.diamonLevel.text = "等级：" + "1";
        addPool.diamonNumber.text = "第" + playerdata.DiamondHatch.ToString() + "次扩建";
        if (playerdata.Diamonds - ((playerdata.DiamondHatch + 1) * 2000) > 0)
        {
            addPool.btn_Diamons.interactable = true;
            addPool.diamonPrice.text = "造价：" + ((playerdata.DiamondHatch + 1) * 2000).ToString();
        }
        else
        {
            addPool.btn_Diamons.interactable = false;
            addPool.diamonPrice.text = "造价：" + ((playerdata.DiamondHatch + 1) * 2000).ToString();
        }
        addPool.goldLevel.text = "等级：" + "1";
        addPool.goldNumber.text = "第" + playerdata.GoldHatch.ToString() + "次扩建";
        if (playerdata.GoldCoin - ((playerdata.GoldHatch + 1) * 2000) > 0)
        {
            addPool.btn_Gold.interactable = true;
            addPool.goldPrice.text = "造价：" + ((playerdata.GoldHatch + 1) * 2000).ToString();
        }
        else
        {
            addPool.btn_Gold.interactable = false;
            addPool.goldPrice.text = "造价：" + ((playerdata.GoldHatch + 1) * 2000).ToString();
        }
    }
    public void AddPoolForGold()
    {
        playerdata.GoldCoin -= ((playerdata.GoldHatch + 1) * 2000);
        playerdata.GoldHatch++;
        items.Add(new HatcheryData(currentGrids, (HatcheryType)1));
        UpdateHatchery();
        AddPoolOBJ.SetActive(false);
    }
    public void AddPoolForDiamons()
    {
        playerdata.Diamonds -= ((playerdata.DiamondHatch + 1) * 2000);
        playerdata.DiamondHatch++;
        items.Add(new HatcheryData(currentGrids, (HatcheryType)1));
        UpdateHatchery();
        AddPoolOBJ.SetActive(false);
    }
    public void ChickAddSpeed()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < grids.Length; i++)
        {
            if (grids[i].btn_speed.gameObject == go)
            {
                currentGrids = i;
                break;
            }
        }
        List<ItemData> data = BagItemData.Instance.GetItems(propId);
        for (int i = 0; i < data.Count; i++)
        {
            propNumber += data[i].Number;
        }
        AddSpeedOBJ.SetActive(true);
        addSpeed.propImage.sprite = IconMgr.Instance.GetIcon(data[0].SpriteName);
        addSpeed.propSlider.maxValue = propNumber;
        addSpeed.propNumber.text = addSpeed.propSlider.value.ToString();
        addSpeed.propName.text = GamePropData.Instance.QueryItem(propId).Name.ToString();
        addSpeed.addSpeedNumber.text = "将减少**" + (addSpeed.propSlider.value * speedAdd).ToString() + "**秒";
        if (addSpeed.propSlider.value == 0)
            addSpeed.btn_enter.interactable = false;
        else
            addSpeed.btn_enter.interactable = true;
    }
    public void ChickValueChange()
    {
        addSpeed.propNumber.text = addSpeed.propSlider.value.ToString();
        addSpeed.propName.text = GamePropData.Instance.QueryItem(propId).Name.ToString();
        addSpeed.addSpeedNumber.text = "将减少**" + (addSpeed.propSlider.value * speedAdd).ToString() + "**秒";
        if (addSpeed.propSlider.value == 0)
            addSpeed.btn_enter.interactable = false;
        else
            addSpeed.btn_enter.interactable = true;
    }
    public void ChickAddSpeedEnter()
    {
        long endTime = items[currentGrids].EndTime;
        endTime = DateTime.FromFileTime(endTime).AddSeconds(-(addSpeed.propSlider.value * speedAdd)).ToFileTime();
        items[currentGrids].EndTime = endTime;
        BagItemData.Instance.ReduceItems(propId, (int)addSpeed.propSlider.value);
        AddSpeedOBJ.SetActive(false);
    }
    #region 后退按钮
    public void AddPoolBack()
    {
        AddPoolOBJ.SetActive(false);
    }
    public void AddSpeedBack()
    {
        AddSpeedOBJ.SetActive(false);
    }
    #endregion
}







[System.Serializable]
public class HatchAddPoolPage
{
    public Button btn_Gold;
    public Text goldNumber;
    public Text goldLevel;
    public Text goldPrice;

    public Button btn_Diamons;
    public Text diamonNumber;
    public Text diamonLevel;
    public Text diamonPrice;

    public Button btn_back;
}
[System.Serializable]
public class HatchAddSpeedPage
{
    public Image propImage;
    public Text propName;
    public Text propNumber;
    public Slider propSlider;
    public Text addSpeedNumber;
    public Button btn_enter;
    public Button btn_back;
}

