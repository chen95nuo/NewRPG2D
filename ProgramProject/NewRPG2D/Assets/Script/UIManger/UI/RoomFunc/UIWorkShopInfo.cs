using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;
using DG.Tweening;

public class UIWorkShopInfo : TTUIPage
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_Tip_5;
    public Text txt_Tip_6;

    public Text txt_Name;
    public Text txt_Quality;
    public Text txt_NeedMana;
    public Text txt_NeedPropNum;
    public Text txt_NeedTime;
    public Text txt_Diamonds;
    public Text txt_Time;
    public Image TimeSlider;

    public Text txt_HaveMana;
    public Text[] txt_HaveProp;

    public Image propIcon;
    public Image TypeIcon;

    public Button btn_Next;//下一个
    public Button btn_Previous;//上一个
    public Button btn_Close;//退出
    public Button btn_Start;//启动
    public Button btn_SpeedUp;//加速
    public Button[] btn_Type;//左侧选项
    public Button[] btn_Quality;//品质选项

    //private WorkShopInfoData currentData;
    private WorkShopData[] currentShopData;

    private string whiteText { get { return LanguageDataMgr.instance.whiteText; } }
    private string redText { get { return LanguageDataMgr.instance.redSt; } }

    private int currentType = 0;//当前装备类型
    private int currentIndex = 0;//当前品质类型
    private LocalBuildingData currentData;

    private int[] haveProp = new int[5];
    private int haveMana = 0;

    public RectTransform RightTip;
    public RectTransform LeftTip;
    public Button btn_Mana;
    public Button btn_Prop;
    public UIWorkShopTip popTip;
    public UIWorkShopGrid grid;
    public GameObject GridsGroup;
    public int CurrentIndex
    {
        get
        {
            return currentIndex;
        }
        set
        {
            if (value < 0)
            {
                value = btn_Quality.Length - 1;
            }
            else if (value >= btn_Quality.Length)
            {
                value = 0;
            }
            btn_Quality[currentIndex].interactable = true;
            btn_Quality[currentIndex].transform.DOLocalMoveY(-10, 0.3f);
            currentIndex = value;
            btn_Quality[currentIndex].interactable = false;
            btn_Quality[currentIndex].transform.DOLocalMoveY(10, 0.3f);
            UpdateInfo(currentIndex);
        }
    }

    private void Awake()
    {
        HallEventManager.instance.AddListener<WorkShopHelper>(HallEventDefineEnum.ChickWorkTime, UpdateTime);
        HallEventManager.instance.AddListener(HallEventDefineEnum.CheckStock, UpdateHaveMana);
        HallEventManager.instance.AddListener(HallEventDefineEnum.ChickFragment, UpdateHaveProp);

        txt_Tip_1.text = "所需资源";
        txt_Tip_2.text = "所需碎片";
        txt_Tip_3.text = "物品品质选择";
        txt_Tip_4.text = "制作";
        txt_Tip_5.text = "加速";
        txt_Tip_6.text = "制作时间:";

        btn_Next.onClick.AddListener(ChickNext);
        btn_Previous.onClick.AddListener(ChickPrevious);
        btn_Close.onClick.AddListener(ClosePage);
        btn_Start.onClick.AddListener(ChickStart);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);

        for (int i = 0; i < btn_Type.Length; i++)
        {
            btn_Type[i].onClick.AddListener(ChickLeftType);
        }

        for (int i = 0; i < btn_Quality.Length; i++)
        {
            btn_Quality[i].onClick.AddListener(ChickQuality);
        }

        UpdateHaveMana();
        UpdateHaveProp();
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<WorkShopHelper>(HallEventDefineEnum.ChickWorkTime, UpdateTime);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CheckStock, UpdateHaveMana);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ChickFragment, UpdateHaveProp);
    }

    private void ChickSpeedUp()
    {
        Debug.Log("等待后台消息");
        //WorkShopDataMgr.instance.
    }

    private void ChickQuality()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_Quality.Length; i++)
        {
            if (btn_Quality[i].gameObject == go)
            {
                CurrentIndex = i;
                break;
            }
        }
    }

    private void ChickLeftType()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_Type.Length; i++)
        {
            if (btn_Type[i].gameObject == go)
            {
                currentType = i;
                UpdateInfo(0);
                break;
            }
        }
    }

    private void ChickStart()
    {
        WorkShopDataMgr.instance.AddWork(currentData.id, currentShopData[currentIndex]);
        TipAnim(true);
    }

    private void ChickPrevious()
    {
        CurrentIndex--;
    }

    private void ChickNext()
    {
        CurrentIndex++;
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        RoomMgr room = mData as RoomMgr;
        LocalBuildingData data = room.currentBuildData;
        if (currentData == null || currentData.buildingData.ItemId != data.buildingData.ItemId)
        {
            currentData = data;
            currentType = 0;
            UpdateInfo(data.buildingData);
            CurrentIndex = 0;

            WorkShopHelper timeData = WorkShopDataMgr.instance.GetWorkTime(data.id);
            TipAnim(timeData != null, timeData != null && timeData.time <= 0);
            if (timeData != null)
            {
                UpdateTime(timeData);
            }
        }
    }

    public void UpdateInfo(BuildingData data)
    {
        currentShopData = WorkShopDataMgr.instance.GetWorkData(data.ItemId);
        string nameSt = LanguageDataMgr.instance.GetEquipName(data.RoomName.ToString(), currentType);
        nameSt += string.Format("(<quad name=Fight size=60 width=1 />{0}-{1})", currentShopData[currentIndex].Level[0], currentShopData[currentIndex].Level[1]);
        txt_Name.text = nameSt;
    }

    public void UpdateInfo(int index)
    {
        currentIndex = index;
        txt_Quality.text = LanguageDataMgr.instance.GetString("WorkShop_" + currentShopData[index].Quality.ToString());
        txt_NeedTime.gameObject.SetActive(true);
        txt_NeedTime.text = SystemTime.instance.TimeNormalizedOf(currentShopData[index].NeedTime, false);
        string spriteName = PropDataMgr.instance.GetDataByItemId<PropData>(currentShopData[index].NeedPropId).SpriteName;
        propIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(spriteName);
        grid.UpdateInfo(currentData.buildingData, currentIndex);
        UpdateLeftTip();
        ChickProp();
        ChickMana();
    }

    public void TipAnim(bool isCry = false, bool isEnd = false)
    {
        btn_Start.gameObject.SetActive(!isCry);
        btn_SpeedUp.gameObject.SetActive(isCry);
        if (isCry)
        {
            txt_NeedTime.gameObject.SetActive(false);
            TimeSlider.gameObject.SetActive(true);
            btn_Mana.gameObject.SetActive(false);
            btn_Prop.gameObject.SetActive(false);
            txt_Tip_3.text = isEnd ? "制作完成" : "制作中";
            LeftTip.anchoredPosition = Vector2.left * 500;
        }
        btn_Previous.gameObject.SetActive(!isCry);
        btn_Next.gameObject.SetActive(!isCry);
        GridsGroup.gameObject.SetActive(!isCry);
        txt_Tip_3.text = "物品品质选择";
        RightTip.anchoredPosition = Vector2.right * 500;

        if (!isEnd)
        {
            RightTip.DOAnchorPos(Vector2.zero, 0.5f);
        }
    }

    public void ChickMana()
    {
        if (haveMana >= currentShopData[currentIndex].NeedMana)
        {
            txt_NeedMana.text = string.Format(whiteText, currentShopData[currentIndex].NeedMana);
        }
        else
        {
            txt_NeedMana.text = string.Format(redText, currentShopData[currentIndex].NeedMana);
        }
    }

    public void ChickProp()
    {
        if (haveProp[currentIndex] >= currentShopData[currentIndex].NeedPropNum)
        {
            txt_NeedPropNum.text = string.Format(whiteText, currentShopData[currentIndex].NeedPropNum);
        }
        else
        {
            txt_NeedPropNum.text = string.Format(redText, currentShopData[currentIndex].NeedPropNum);
        }
    }
    public void UpdateHaveMana()
    {
        haveMana = CheckPlayerInfo.instance.GetAllStock(BuildRoomName.Mana);
        txt_HaveMana.text = haveMana.ToString();
    }

    public void UpdateHaveProp()
    {
        int index = 1001;//第一个道具的ID
        for (int i = 0; i < txt_HaveProp.Length; i++)
        {
            int temp = ChickItemInfo.instance.GetItemNum(index + i);
            haveProp[i] = temp;
            txt_HaveProp[i].text = temp.ToString();
        }
    }

    public void UpdateLeftTip()
    {
        int count = currentShopData[currentIndex].EquipType.Length;
        for (int i = 0; i < count; i++)
        {
            btn_Type[i].gameObject.SetActive(true);
            btn_Type[i].image.sprite = GetSpriteAtlas.insatnce.GetWotkType(currentData.buildingData.RoomName.ToString(), i);
        }
        for (int i = count; i < btn_Type.Length; i++)
        {
            btn_Type[i].gameObject.SetActive(false);
        }
    }

    public void UpdateTime(WorkShopHelper timeData)
    {
        if (timeData.roomId == currentData.id)
        {
            txt_Time.text = SystemTime.instance.TimeNormalizedOf(timeData.time, false);
            TimeSlider.fillAmount = (timeData.maxTime - timeData.time) / timeData.maxTime;
        }
    }






    public override void Hide(bool needAnim = true)
    {
        base.Hide(false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(false);
    }
}
