using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;
using Assets.Script.Net;
using proto.SLGV1;

public class UIMain : TTUIPage
{
    public static UIMain instance;

    public GameObject rightDown;
    public GameObject leftDown;
    public GameObject leftUp;


    public UIMarket market;

    public Button[] btn_Produces;
    public Button btn_diamonds;

    public Text txt_gold;
    public Text txt_food;
    public Text txt_mana;
    public Text txt_wood;
    public Text txt_iron;
    public Text txt_grailNum;//圣杯数量
    public Text txt_diamonds;
    public Text txt_playerName;
    public Text txt_ClanName;//工会信息
    public Text txt_Population;//人数
    public Text txt_builderNum;//工程队
    public Text txt_happinessState;

    public Image goldSlider;
    public Image foodSlider;
    public Image manaSlider;
    public Image woodSlider;
    public Image ironSlider;

    public Image[] Icons;

    private PlayerData playerData;

    private ServerBuildData[] spaceStock = new ServerBuildData[5];

    public SpaceNumJump gold;
    public SpaceNumJump food;
    public SpaceNumJump mana;
    public SpaceNumJump wood;
    public SpaceNumJump iron;
    public SpaceNumJump diamonds;

    public Canvas[] hightIcons;

    public Button btn_UIMarket;
    public Button btn_UIChat;
    public Button btn_UIRank;
    public Button btn_UISetting;
    public Button btn_UIBag;
    public Button btn_UIMap;
    public Button btn_Cancel;
    public Button btn_People;//人口提示 popUpTips_3
    public Button btn_Worker;//建筑工人提示 popUpTips_4
    public Button btn_Happiness;//幸福度提示 popUpTips_5
    public Button btn_Info;
    public Button btn_Rank;
    public Button btn_SetUp;
    public Button btn_MaliBox;
    public Button btn_Achievement;
    public Button btn_Task;

    private BuildingData currentBuildData;

    private void Awake()
    {
        instance = this;

        btn_UIMarket.onClick.AddListener(ShowMarket);
        btn_UIBag.onClick.AddListener(CheckBag);
        btn_UIMap.onClick.AddListener(CheckMap);
        btn_Cancel.onClick.AddListener(CheckCancel);
        btn_People.onClick.AddListener(CheckPeople);
        btn_Worker.onClick.AddListener(CheckWorkr);
        btn_Happiness.onClick.AddListener(CheckHappiness);
        btn_Info.onClick.AddListener(CheckInfo);
        btn_UIRank.onClick.AddListener(CheckRank);
        btn_SetUp.onClick.AddListener(CheckSetUp);
        btn_UIChat.onClick.AddListener(CheckChat);
        btn_MaliBox.onClick.AddListener(CheckMail);
        btn_UISetting.onClick.AddListener(CheckSetting);
        btn_Achievement.onClick.AddListener(CheckAchie);
        btn_Task.onClick.AddListener(CheckTask);

        for (int i = 0; i < btn_Produces.Length; i++)
        {
            btn_Produces[i].onClick.AddListener(CheckProduceTips);
        }
        market.gameObject.SetActive(false);

        HallEventManager.instance.AddListener(HallEventDefineEnum.diamondsSpace, CheckDiamonds);
        HallEventManager.instance.AddListener<BuildRoomName>(HallEventDefineEnum.CheckStock, GetSpace);
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.UiMainHight, UIMainHight);
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.CheckHappiness, HappinessChange);
        UIMainHight(-1);

        gold = new SpaceNumJump(txt_gold);
        food = new SpaceNumJump(txt_food);
        mana = new SpaceNumJump(txt_mana);
        wood = new SpaceNumJump(txt_wood);
        iron = new SpaceNumJump(txt_iron);
        diamonds = new SpaceNumJump(txt_diamonds);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.diamondsSpace, CheckDiamonds);
        HallEventManager.instance.RemoveListener<BuildRoomName>(HallEventDefineEnum.CheckStock, GetSpace);
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.UiMainHight, UIMainHight);
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.CheckHappiness, HappinessChange);
    }

    private void CheckTask()
    {
        object st = "任务系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckAchie()
    {
        object st = "成就系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckSetting()
    {
        object st = "设置系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckMail()
    {
        object st = "邮箱系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckChat()
    {
        object st = "聊天系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckSetUp()
    {
        object st = "入侵系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckRank()
    {
        object st = "排行系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckInfo()
    {
        object st = "玩家数据系统暂未开放";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }

    private void CheckHappiness()
    {
        UIPanelManager.instance.ShowPage<UIPopUpTips_5>(btn_Happiness.transform);
    }

    private void CheckWorkr()
    {
        UIPanelManager.instance.ShowPage<UIPopUpTips_4>();
    }

    private void CheckPeople()
    {
        List<object> obj = new List<object>();
        string st = HallRoleMgr.instance.GetAllRoleNum + "/" + BuildingManager.instance.SearchRoleSpace();
        obj.Add(btn_People.transform);
        obj.Add(st);
        UIPanelManager.instance.ShowPage<UIPopUpTips_3>(obj);
    }

    /// <summary>
    /// 右上材料点击方法
    /// </summary>
    private void CheckProduceTips()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_Produces.Length; i++)
        {
            if (btn_Produces[i].gameObject == go)
            {
                BuildRoomName name = BuildRoomName.Nothing;
                switch (i)
                {
                    case 0: name = BuildRoomName.Gold; break;
                    case 1: name = BuildRoomName.Food; break;
                    case 2: name = BuildRoomName.Mana; break;
                    case 3: name = BuildRoomName.Wood; break;
                    case 4: name = BuildRoomName.Iron; break;
                    default:
                        break;
                }
                UIPanelManager.instance.ShowPage<UIPopUpTips_2>();
                UIPopUpTips_2.instance.UpdateInfo(name, btn_Produces[i].transform);
                return;
            }
        }
    }

    private void Start()
    {
        Init();//初始化UI
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Init()
    {
        RefreshText();
        playerData = GetPlayerData.Instance.GetData();
        int level = playerData.MainHallLevel;
        if (level > 0)
        {
            btn_Produces[0].gameObject.SetActive(true);
            btn_Produces[1].gameObject.SetActive(true);
            GetSpace(BuildRoomName.GoldSpace);
            GetSpace(BuildRoomName.FoodSpace);
        }
        if (level >= 4)
        {
            btn_Produces[2].gameObject.SetActive(true);
            GetSpace(BuildRoomName.ManaSpace);
        }
        if (level >= 6)
        {
            btn_Produces[3].gameObject.SetActive(true);
            GetSpace(BuildRoomName.WoodSpace);
        }
        if (level >= 9)
        {
            btn_Produces[4].gameObject.SetActive(true);
            GetSpace(BuildRoomName.IronSpace);
        }
        txt_playerName.text = playerData.Name;
        txt_ClanName.text = "";
        txt_diamonds.text = playerData.Diamonds.ToString();
        txt_grailNum.text = playerData.GrailNum.ToString();
        txt_builderNum.text = playerData.BuilderNum + "/" + HallConfigDataMgr.instance.GetValue(HallConfigEnum.builder);
        CheckDiamonds();
        CheckResident();
        HappinessChange(playerData.Happiness);

    }

    /// <summary>
    /// 幸福度系统
    /// </summary>
    /// <param name="number"></param>
    private void HappinessChange(int number)
    {
        number = HallConfigDataMgr.instance.GetNowHappiness(number);
        txt_happinessState.text = "+" + number.ToString() + "%";
    }

    /// <summary>
    /// 检查居民数和上限
    /// </summary>
    private void CheckResident()
    {
        txt_Population.text = HallRoleMgr.instance.GetAllRoleNum + "/" + BuildingManager.instance.SearchRoleSpace();
    }

    #region Market
    /// <summary>
    /// UIMarket
    /// </summary>
    private void ShowMarket()
    {
        CloseSomeUI(false);
        market.gameObject.SetActive(true);
        market.CheckBtnType();
    }
    public void ShowMarket(int page = 1)
    {
        CloseSomeUI(false);
        market.gameObject.SetActive(true);
        market.currentType = page;
        market.CheckBtnType();
    }
    public void CloseMarket()
    {
        CloseSomeUI(true);
        market.gameObject.SetActive(false);
    }
    #endregion

    public void ShowBack(bool isTrue)
    {
        btn_Cancel.gameObject.SetActive(isTrue);
    }

    public void CloseSomeUI(bool isTrue)
    {
        leftUp.SetActive(isTrue);
        rightDown.SetActive(isTrue);
        leftDown.SetActive(isTrue);
        btn_UIChat.gameObject.SetActive(isTrue);
        btn_UIRank.gameObject.SetActive(isTrue);
        btn_UISetting.gameObject.SetActive(isTrue);
    }

    /// <summary>
    /// 通过消息更新当前某资源容量
    /// </summary>
    /// <param name="newStock"></param>
    private void GetSpace(BuildRoomName name)
    {
        Image slider = null;
        float allSpace = BuildingManager.instance.SearchRoomSpace(name);
        float allStock = BuildingManager.instance.SearchRoomStock(name);
        switch (name)
        {
            case BuildRoomName.GoldSpace:
                slider = goldSlider;
                txt_gold.text = allStock.ToString();
                break;
            case BuildRoomName.FoodSpace:
                slider = foodSlider;
                txt_food.text = allStock.ToString();
                break;
            case BuildRoomName.ManaSpace:
                slider = manaSlider;
                txt_mana.text = allStock.ToString();
                break;
            case BuildRoomName.WoodSpace:
                slider = woodSlider;
                txt_wood.text = allStock.ToString();
                break;
            case BuildRoomName.IronSpace:
                slider = ironSlider;
                txt_iron.text = allStock.ToString();
                break;
            default:
                break;
        }
        slider.fillAmount = (allStock / allSpace);
    }

    private void CheckDiamonds()
    {
        txt_diamonds.text = playerData.Diamonds.ToString();
    }

    public IEnumerator JumpNumber(SpaceNumJump data)
    {
        while (data.num < data.maxNum)
        {
            data.txt.text = data.num.ToString();
            float temp = (data.maxNum - data.num) * 0.1f;
            temp = temp <= 1f ? 1f : temp;
            data.num += (int)temp;
            yield return new WaitForSeconds(0.05f);
        }
        data.num = data.maxNum;
        data.txt.text = data.num.ToString();
    }

    public void CheckBag()
    {
        UIPanelManager.instance.ShowPage<UIBag>();
    }
    public void CheckMap()
    {
        UIPanelManager.instance.ShowPage<UIWorldMap>();
    }
    public void CheckCancel()
    {
        ShowBack(false);
        MapControl.instance.ResetRoomTip();
        ShowMarket();
    }

    public void GetStock()
    {

    }
    public void CheckAllStock(BuildRoomName name, Image slider)
    {
        //float all = ChickPlayerInfo.instance.GetAllStock(name);
        //float allStock = ChickPlayerInfo.instance.GetAllStockSpace(name);
        //slider.fillAmount = (all / allStock);
    }

    public void RefreshText()
    {
        btn_Produces[2].gameObject.SetActive(false);
        btn_Produces[3].gameObject.SetActive(false);
        btn_Produces[4].gameObject.SetActive(false);
    }

    public void UIMainHight(int index)
    {
        for (int i = 0; i < hightIcons.Length; i++)
        {
            hightIcons[i].sortingOrder = index;
        }
    }

    /// <summary>
    /// 向服务器发送建造验证
    /// </summary>
    /// <param name="buildingData"></param>
    public void RSCheckCreateNewRoom(BuildingData buildingData)
    {
        //发送建造请求
        WebSocketManger.instance.Send(NetSendMsg.RQ_CheckCreateNewRoom, buildingData.ItemId);
        currentBuildData = buildingData;
    }

    /// <summary>
    /// 接收服务器建造验证结果
    /// </summary>
    /// <param name="checkCreateRoom"></param>
    public void RQCheckCreateNewRoom(RS_CheckCreateNewRoom checkCreateRoom)
    {

        if (checkCreateRoom.ret == -1)//库满
        {
            UIPanelManager.instance.ShowPage<UIPopUp_4>(checkCreateRoom.neeedItem[0]);
        }
        else if (checkCreateRoom.ret == -2)//资源不足
        {
            Dictionary<MaterialName, int> needStock = new Dictionary<MaterialName, int>();
            for (int i = 0; i < checkCreateRoom.neeedItem.Count; i++)
            {
                needStock.Add((MaterialName)checkCreateRoom.neeedItem[i].produceType, checkCreateRoom.neeedItem[i].needNum);
            }

            UIPanelManager.instance.ShowPage<UIPopUp_1>(needStock);
            return;
        }
        ShowBack(true);
        market.gameObject.SetActive(false);
        MainCastle.instance.BuildRoomTip(currentBuildData);
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

}
public class SpaceNumJump
{
    public int num;
    public int maxNum;
    public Text txt;

    public SpaceNumJump(Text txt)
    {
        num = 0;
        this.maxNum = 0;
        this.txt = txt;
    }
}
public class UIMainCheckStock
{
    public BuildRoomName name;
    public int stock;

    public UIMainCheckStock(BuildRoomName name, int stock)
    {
        this.name = name;
        this.stock = stock;
    }
}