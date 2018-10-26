using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using DG.Tweening;

public class UILockRoomTip : TTUIPage
{
    public Text txt_Name;
    public Text txt_Level;
    public Button btn_Message;
    public Button btn_LevelUp;
    public Button btn_Cancel;
    public Button btn_SpeedUp;
    public Button btn_CastleEditor;
    public Button btn_CastleMod;
    public Button btn_Train;
    public Button btn_TrainSpeedUp_1;
    public Button btn_TrainSpeedUp_2;
    public Button btn_Craft;
    public Button btn_CraftItem;
    public Button btn_Research;

    private UILockSpeedUp uiSpeedUp;
    private UILockTrainSpeed uiTrainSpeed_1;
    private UILockTrainSpeed uiTrainSpeed_2;

    private RoomMgr roomData;
    private bool isOpen = false;
    private RectTransform rt;
    private void Awake()
    {
        Init();
        rt = GetComponent<RectTransform>();
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        RoomMgr data = mData as RoomMgr;
        rt.anchoredPosition = Vector3.down * 540;
        rt.DOAnchorPos(Vector3.zero, 0.15f);
        LockRoomData(data);
        LockRoomData(data);
    }

    private void Init()
    {
        ClostAllBtn(false);
        uiSpeedUp = btn_SpeedUp.GetComponent<UILockSpeedUp>();
        uiTrainSpeed_1 = btn_TrainSpeedUp_1.GetComponent<UILockTrainSpeed>();
        uiTrainSpeed_2 = btn_TrainSpeedUp_2.GetComponent<UILockTrainSpeed>();

        btn_Message.onClick.AddListener(ChickMessage);
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
        btn_Cancel.onClick.AddListener(ChickCancel);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
        btn_CastleEditor.onClick.AddListener(ChickCastleEddit);
        btn_CastleMod.onClick.AddListener(ChickCastleMod);
        btn_Train.onClick.AddListener(ChickTrain);
        btn_TrainSpeedUp_1.onClick.AddListener(ChickTranSpeedUP_1);
        btn_TrainSpeedUp_2.onClick.AddListener(ChickTranSpeedUP_2);
        btn_Craft.onClick.AddListener(ChickCraft);
        btn_CraftItem.onClick.AddListener(ChickCraftItem);
        btn_Research.onClick.AddListener(ChickResearch);
    }

    private void LockRoomData(RoomMgr data)
    {
        roomData = data;
        txt_Name.text = roomData.RoomName.ToString();
        txt_Level.text = roomData.BuildingData.Level.ToString();
        switch (data.BuildingData.RoomType)
        {
            case RoomType.Nothing:
                break;
            case RoomType.Production:
                ProductionRoom();
                break;
            case RoomType.Training:
                TrainingRoom();
                break;
            case RoomType.Support:
                SupportRoom();
                break;
            case RoomType.Max:
                break;
            default:
                break;
        }
        isOpen = !isOpen;
        if (isOpen)
        {
            Debug.Log("开启");
        }
        else
        {
            Debug.Log("关闭");
        }
    }
    /// <summary>
    /// 生产类房间
    /// </summary>
    private void ProductionRoom()
    {
        ClostAllBtn(false);
        if (roomData.ConstructionType == true)
        {
            btn_Message.gameObject.SetActive(isOpen);
            RoomLevelUp(true);
            return;
        }
        RoomLevelUp(false);
        btn_Message.gameObject.SetActive(isOpen);
        btn_LevelUp.gameObject.SetActive(isOpen);
    }

    /// <summary>
    /// 训练类房间
    /// </summary>
    private void TrainingRoom()
    {
        ClostAllBtn(false);
        if (roomData.ConstructionType == true)
        {
            RoomLevelUp(true);
            return;
        }
        RoomLevelUp(false);
        btn_LevelUp.gameObject.SetActive(isOpen);
        btn_Train.gameObject.SetActive(isOpen);
        //这边还需要判断有多少个角色对应角色出现提示框
    }

    /// <summary>
    /// 功能类房间
    /// </summary>
    private void SupportRoom()
    {
        ClostAllBtn(false);
        if (roomData.ConstructionType == true)
        {
            RoomLevelUp(true);
            return;
        }
        switch (roomData.BuildingData.RoomName)
        {
            case BuildRoomName.Nothing:
                Debug.LogError("错误房间类型超限");
                break;
            case BuildRoomName.LivingRoom:
                btn_Message.gameObject.SetActive(isOpen);
                btn_LevelUp.gameObject.SetActive(isOpen);
                break;
            case BuildRoomName.TrophyRoom:
                break;
            case BuildRoomName.Hospital:
                break;
            case BuildRoomName.ClanHall:
                break;
            case BuildRoomName.MagicWorkShop:
                break;
            case BuildRoomName.MagicLab:
                btn_Message.gameObject.SetActive(isOpen);
                btn_LevelUp.gameObject.SetActive(isOpen);
                btn_Research.gameObject.SetActive(isOpen);
                break;
            case BuildRoomName.WeaponsWorkShop:
                break;
            case BuildRoomName.ArmorWorkShop:
                break;
            case BuildRoomName.GemWorkSpho:
                break;
            case BuildRoomName.Stairs:
                break;
            case BuildRoomName.ThroneRoom:
                mainHouse();
                break;
            case BuildRoomName.Barracks:
                btn_Message.gameObject.SetActive(isOpen);
                btn_LevelUp.gameObject.SetActive(isOpen);
                break;
            case BuildRoomName.MaxRoom:
                Debug.LogError("错误房间类型超限");
                break;
            default:
                break;
        }
    }

    private void mainHouse()
    {
        if (roomData.ConstructionType == true)
        {
            btn_Message.gameObject.SetActive(isOpen);
            RoomLevelUp(true);
            return;
        }
        RoomLevelUp(false);
        btn_Message.gameObject.SetActive(isOpen);
        btn_LevelUp.gameObject.SetActive(isOpen);
        btn_CastleEditor.gameObject.SetActive(isOpen);
        btn_CastleMod.gameObject.SetActive(isOpen);
    }

    private void RoomLevelUp(bool isTrue)
    {
        btn_Cancel.gameObject.SetActive(isTrue);
        btn_SpeedUp.gameObject.SetActive(isTrue);
    }

    private void ChickMessage()
    {
        Debug.Log("根据类型弹出信息框");
        switch (roomData.BuildingData.RoomName)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                UIPanelManager.instance.ShowPage<UIProductionInfo>(roomData);
                break;
            case BuildRoomName.GoldSpace:
                UIPanelManager.instance.ShowPage<UIStockInfo>(roomData);
                break;
            case BuildRoomName.Food:
                UIPanelManager.instance.ShowPage<UIProductionInfo>(roomData);
                break;
            case BuildRoomName.FoodSpace:
                UIPanelManager.instance.ShowPage<UIStockInfo>(roomData);
                break;
            case BuildRoomName.Wood:
                UIPanelManager.instance.ShowPage<UIProductionInfo>(roomData);
                break;
            case BuildRoomName.WoodSpace:
                UIPanelManager.instance.ShowPage<UIStockInfo>(roomData);
                break;
            case BuildRoomName.Mana:
                UIPanelManager.instance.ShowPage<UIProductionInfo>(roomData);
                break;
            case BuildRoomName.ManaSpace:
                UIPanelManager.instance.ShowPage<UIStockInfo>(roomData);
                break;
            case BuildRoomName.Iron:
                UIPanelManager.instance.ShowPage<UIProductionInfo>(roomData);
                break;
            case BuildRoomName.IronSpace:
                UIPanelManager.instance.ShowPage<UIStockInfo>(roomData);
                break;
            case BuildRoomName.LivingRoom:
                UIPanelManager.instance.ShowPage<UILivingRoomInfo>(roomData);
                break;
            case BuildRoomName.Barracks:
                UIPanelManager.instance.ShowPage<UIBarracksInfo>(roomData);
                break;
            case BuildRoomName.ThroneRoom:
                UIPanelManager.instance.ShowPage<UIThroneInfo>(roomData);
                break;
            default:
                break;
        }
    }

    private void ChickLevelUp()
    {
        Debug.Log("根据类型弹出升级框");
        switch (roomData.BuildingData.RoomType)
        {
            case RoomType.Nothing:
                Debug.LogError("错误房间类型超限");
                break;
            case RoomType.Production:
                UIPanelManager.instance.ShowPage<UIProdLevelUp>(roomData);
                break;
            case RoomType.Training:
                UIPanelManager.instance.ShowPage<UIProdLevelUp>(roomData);
                break;
            case RoomType.Support:
                SupportType();
                break;
            case RoomType.Max:
                Debug.LogError("错误房间类型超限");
                break;
            default:
                break;
        }
    }

    private void ChickCancel()
    {
        Debug.Log("升级中取消升级");
    }

    private void ChickSpeedUp()
    {
        Debug.Log("升级中进行加速");
        LocalServer.instance.ChickTime(roomData);
    }

    private void ChickCastleEddit()
    {
        //Debug.Log("检查城堡编辑器");
        MapControl.instance.ShowEditMap();
    }

    private void ChickCastleMod()
    {
        Debug.Log("检查城堡背景修改");
        object st = "该功能暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void ChickTrain()
    {
        Debug.Log("根据类型检查训练");
        UIPanelManager.instance.ShowPage<UITrainInfo>(roomData);
    }

    private void ChickTranSpeedUP_1()
    {
        Debug.Log("检查一号训练加速");
    }
    private void ChickTranSpeedUP_2()
    {
        Debug.Log("检查二号训练加速");
    }
    private void ChickCraft()
    {
        Debug.Log("检查制造");
    }
    private void ChickCraftItem()
    {
        Debug.Log("检查物品制造");
    }
    private void ChickResearch()
    {
        Debug.Log("检查魔法升级");
        UIPanelManager.instance.ShowPage<UIMagicLevelUp>(roomData);
    }

    private void ClostAllBtn(bool isTrue)
    {
        btn_Message.gameObject.SetActive(isTrue);
        btn_LevelUp.gameObject.SetActive(isTrue);
        btn_Cancel.gameObject.SetActive(isTrue);
        btn_SpeedUp.gameObject.SetActive(isTrue);
        btn_CastleEditor.gameObject.SetActive(isTrue);
        btn_CastleMod.gameObject.SetActive(isTrue);
        btn_Train.gameObject.SetActive(isTrue);
        btn_TrainSpeedUp_1.gameObject.SetActive(isTrue);
        btn_TrainSpeedUp_2.gameObject.SetActive(isTrue);
        btn_Craft.gameObject.SetActive(isTrue);
        btn_CraftItem.gameObject.SetActive(isTrue);
        btn_Research.gameObject.SetActive(isTrue);
    }

    private void SupportType()
    {
        switch (roomData.BuildingData.RoomName)
        {
            case BuildRoomName.Nothing:
                Debug.LogError("错误房间类型超限");
                break;
            case BuildRoomName.LivingRoom:
                UIPanelManager.instance.ShowPage<UIProdLevelUp>(roomData);
                break;
            case BuildRoomName.TrophyRoom:
                break;
            case BuildRoomName.Hospital:
                break;
            case BuildRoomName.ClanHall:
                break;
            case BuildRoomName.MagicWorkShop:
                break;
            case BuildRoomName.MagicLab:
                break;
            case BuildRoomName.WeaponsWorkShop:
                break;
            case BuildRoomName.ArmorWorkShop:
                break;
            case BuildRoomName.GemWorkSpho:
                break;
            case BuildRoomName.Stairs:
                break;
            case BuildRoomName.ThroneRoom:
                UIPanelManager.instance.ShowPage<UIThroneLevelUp>(roomData);
                break;
            case BuildRoomName.Barracks:
                UIPanelManager.instance.ShowPage<UIProdLevelUp>(roomData);
                break;
            case BuildRoomName.MaxRoom:
                Debug.LogError("错误房间类型超限");
                break;
            default:
                break;
        }
    }

}
