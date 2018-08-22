using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExplore : MonoBehaviour
{
    public Button menuBtn;
    public Button[] menus = new Button[4];//小队选项
    public Button addMenu;
    public Button enter;
    public Button expBtn;
    public Text time;
    public ExRolesBtn[] roleBtn;
    public GameObject map;
    private int currentMenu = 0;
    private int currentRoleBtn = 0;
    private ExploreData currentExplore;//当前地图
    private CardData[] cards = new CardData[3];//临时卡牌数据
    private UIExpBtnGrid[] expGrids;
    private List<ExpeditionData> expData = new List<ExpeditionData>();//所有探险小队

    private void Awake()
    {
        Init();

        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateExploreEvent, ShowRole);
        UIEventManager.instance.AddListener<bool>(UIEventDefineEnum.UpdateExploreTipEvent, IsTrue);
    }
    private void Update()
    {
        DateTime nowtime = SystemTime.insatnce.GetTime();
        switch (expData[currentMenu].ExploreType)
        {
            case ExploreType.Nothing:
                time.text = "00:00:00";
                map.SetActive(true);
                break;
            case ExploreType.Run:
                SystemTime.insatnce.TimeNormalized(expData[currentMenu].NowTime, time);
                map.SetActive(false);
                if (expData[currentMenu].NowTime <= 0)
                {
                    expData[currentMenu].ExploreType = ExploreType.End;
                }
                break;
            case ExploreType.End:
                enter.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateExploreEvent, ShowRole);
        UIEventManager.instance.RemoveListener<bool>(UIEventDefineEnum.UpdateExploreTipEvent, IsTrue);
    }
    private void Init()
    {
        UpdateMenu();
        enter.onClick.AddListener(RoleIsGo);
        enter.gameObject.SetActive(false);
        currentExplore = GameExploreData.Instance.items[0];
        for (int i = 0; i < roleBtn.Length; i++)
        {
            if (i == 0)
            {
                roleBtn[i].tipTop.text = "小队长";
                roleBtn[i].mainTip.text = "要求等级:" + currentExplore.CaptainLevel.ToString();
            }
            else
            {
                roleBtn[i].tipTop.text = "队员";
                roleBtn[i].mainTip.text = "要求等级:" + currentExplore.OtherLevel.ToString();
            }
            roleBtn[i].role.gameObject.SetActive(false);
            roleBtn[i].addRole.gameObject.SetActive(true);
            roleBtn[i].roleBtn.onClick.AddListener(UpdateRoleGrid);
        }
        UpdateRoles();//刷新角色
        UpdateExploreMap();//刷新地图
    }

    /// <summary>
    /// 添加选项
    /// </summary>
    private void AddMenu()
    {
        TinyTeam.UI.TTUIPage.ShowPage<UITipPage>();
        int needPrice = PlayerExpeditionData.Instance.items.Count * 2000; //价格
        int index = GetPlayData.Instance.player[0].GoldCoin - (PlayerExpeditionData.Instance.items.Count * 2000);
        if (index >= 0)
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateExploreTipEvent, needPrice);
        else
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateExploreTipEvent, -1);
    }
    /// <summary>
    /// 点击确定购买按钮
    /// </summary>
    /// <param name="isTrue"></param>
    private void IsTrue(bool isTrue)
    {
        int index = PlayerExpeditionData.Instance.items.Count + 1;
        PlayerExpeditionData.Instance.items.Add(new ExpeditionData(index));
        UpdateMenu();
    }
    /// <summary>
    /// 开始按钮触发 保存当前位置的角色信息 若结束时发布奖励
    /// </summary>
    private void RoleIsGo()
    {
        if (expData[currentMenu].ExploreType == ExploreType.Nothing)
        {
            enter.gameObject.SetActive(false);
            for (int i = 0; i < expData[currentMenu].CardsData.Length; i++)
            {
                expData[currentMenu].CardsData[i] = roleBtn[i].role.roleData;
                expData[currentMenu].CardsData[i].Fighting = true;
            }
            long index = SystemTime.insatnce.GetTime().AddSeconds(currentExplore.NeedTime).ToFileTime();
            expData[currentMenu].EndTime = index;
            expData[currentMenu].ExploreType = ExploreType.Run;
            UpdateRoles();
        }
        else if (expData[currentMenu].ExploreType == ExploreType.End)
        {
            //将道具添加到背包并获取道具信息
            DropBagData dropData = GameDropBagData.Instance.GetItem(currentExplore.DroppingBoxId);
            int addExp = dropData.AddExp;
            int playExp = dropData.AddPlayerExp;
            GainData[] data = GameDropBagData.Instance.GetGains(currentExplore.DroppingBoxId);
            //弹出道具奖励菜单
            TinyTeam.UI.TTUIPage.ShowPage<UIGainTipPage>();
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateGainTipEvent, data);
            CardGainData[] cardGainData = GameDropBagData.Instance.GetCards(expData[currentMenu].CardsData, addExp, playExp);
            //将经验加到角色身上并获取角色信息
            //显示经验奖励面板
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateGainTipEvent, cardGainData);
            ResetRole();//重置角色
        }
    }
    private void ResetRole()
    {
        for (int i = 0; i < expData[currentMenu].CardsData.Length; i++)
        {
            expData[currentMenu].CardsData[i].Fighting = false;
        }
        expData[currentMenu].EndTime = 0;
        expData[currentMenu].ExploreType = ExploreType.Nothing;
        UpdateRoles();
        ChickEnterBtn();
    }
    /// <summary>
    /// 检查按钮
    /// </summary>
    private void ChickMenu()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i] != null)
            {
                menus[i].interactable = true;
                if (menus[i].gameObject == go)
                {
                    currentMenu = i;
                    menus[i].interactable = false;
                }
            }
        }
        Debug.Log("CurrentMenu" + currentMenu);
        UpdateRoles();
        ChickEnterBtn();
    }
    /// <summary>
    /// 刷新按钮
    /// </summary>
    private void UpdateMenu()
    {
        menuBtn.gameObject.SetActive(false);
        Transform ts = menuBtn.GetComponentInParent<Transform>();
        int index = PlayerExpeditionData.Instance.items.Count;
        for (int i = 0; i < index; i++)
        {
            if (menus[i] == null)
            {
                GameObject go = Instantiate(menuBtn.gameObject, ts) as GameObject;
                go.SetActive(true);
                menus[i] = go.GetComponent<Button>();
                menus[i].onClick.AddListener(ChickMenu);
            }
        }
        menus[currentMenu].interactable = false;
        addMenu.transform.SetSiblingIndex(ts.childCount - 1);
        if (index >= 4)
            addMenu.gameObject.SetActive(false);
        else
            addMenu.gameObject.SetActive(true);
        addMenu.onClick.AddListener(AddMenu);
    }
    /// <summary>
    /// 刷新所有角色
    /// </summary>
    private void UpdateRoles()
    {
        expData = PlayerExpeditionData.Instance.items;
        if (expData[currentMenu].CardsData == null || expData[currentMenu].CardsData.Length <= 0)
        {
            expData[currentMenu].CardsData = new CardData[3];
        }
        for (int i = 0; i < expData[currentMenu].CardsData.Length; i++)
        {
            if (expData[currentMenu].ExploreType == ExploreType.Run)
            {
                ShowRole(expData[currentMenu].CardsData[i], i);
                roleBtn[i].role.UpdateItem(expData[currentMenu].CardsData[i]);
            }
            else
            {
                if (i == 0)
                {
                    roleBtn[i].tipTop.text = "小队长";
                    roleBtn[i].mainTip.text = "要求等级:" + currentExplore.CaptainLevel.ToString();
                }
                else
                {
                    roleBtn[i].tipTop.text = "队员";
                    roleBtn[i].mainTip.text = "要求等级:" + currentExplore.OtherLevel.ToString();
                }
                roleBtn[i].role.roleData = null;
                roleBtn[i].role.gameObject.SetActive(false);
                roleBtn[i].addRole.gameObject.SetActive(true);
                roleBtn[i].roleBtn.onClick.AddListener(UpdateRoleGrid);
            }
        }
    }
    /// <summary>
    /// 刷新当前位置的角色信息
    /// </summary>
    private void UpdateRoleGrid()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        TinyTeam.UI.TTUIPage.ShowPage<UICardHousePage>();
        for (int i = 0; i < roleBtn.Length; i++)
        {
            cards[i] = roleBtn[i].role.roleData;
            if (roleBtn[i].roleBtn.gameObject == go.gameObject)
            {
                currentRoleBtn = i;
                if (currentRoleBtn == 0)
                    UIEventManager.instance.SendEvent<int>(UIEventDefineEnum.UpdateRolesEvent, currentExplore.CaptainLevel);
                else
                    UIEventManager.instance.SendEvent<int>(UIEventDefineEnum.UpdateRolesEvent, currentExplore.OtherLevel);
            }
        }
        UpdateCardData data = new UpdateCardData(cards, GridType.Explore);
        UIEventManager.instance.SendEvent<UpdateCardData>(UIEventDefineEnum.UpdateRolesEvent, data);
    }

    /// <summary>
    /// 刷新探险地图
    /// </summary>
    private void UpdateExploreMap()
    {
        expGrids = new UIExpBtnGrid[GameExploreData.Instance.items.Count];
        Transform ts = expBtn.transform.parent.transform;
        expBtn.gameObject.SetActive(false);
        for (int i = 0; i < expGrids.Length; i++)
        {
            GameObject go = Instantiate(expBtn.gameObject, ts) as GameObject;
            expGrids[i] = go.GetComponent<UIExpBtnGrid>();
            expGrids[i].gameObject.SetActive(true);
            expGrids[i].UpdateExpBtnGrid(GameExploreData.Instance.items[i]);
            expGrids[i].expBtn.onClick.AddListener(ChickMapBtn);
        }

    }
    /// <summary>
    /// 检查当前地图按钮
    /// </summary>
    private void ChickMapBtn()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < expGrids.Length; i++)
        {
            if (expGrids[i].expBtn.gameObject == go)
            {
                currentExplore = expGrids[i].expData;
            }
        }
    }

    /// <summary>
    /// 选择角色后刷新角色图片
    /// </summary>
    /// <param name="data">角色数据</param>
    private void ShowRole(CardData data)
    {
        roleBtn[currentRoleBtn].role.UpdateItem(data);
        roleBtn[currentRoleBtn].role.gameObject.SetActive(true);
        roleBtn[currentRoleBtn].addRole.gameObject.SetActive(false);
        roleBtn[currentRoleBtn].mainTip.text = "LV." + data.Level;
        ChickEnterBtn();
    }
    private void ShowRole(CardData data, int currentRole)
    {
        roleBtn[currentRole].role.UpdateItem(data);
        roleBtn[currentRole].role.gameObject.SetActive(true);
        roleBtn[currentRole].addRole.gameObject.SetActive(false);
        roleBtn[currentRole].mainTip.text = "LV." + data.Level;
        ChickEnterBtn();
    }
    /// <summary>
    /// 检查确定按钮
    /// </summary>
    private void ChickEnterBtn()
    {
        if (expData[currentMenu].ExploreType == ExploreType.Run)
        {
            enter.gameObject.SetActive(false);
            map.SetActive(false);
            return;
        }
        int index = 0;
        for (int i = 0; i < roleBtn.Length; i++)
        {
            if (roleBtn[i].role.roleData != null)
            {
                index++;
            }
        }
        if (index >= 4)
            enter.gameObject.SetActive(true);
        else
            enter.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class ExRolesBtn
{
    [SerializeField]
    public Button roleBtn;
    [SerializeField]
    public Text tipTop;
    [SerializeField]
    public UIBagGrid role;
    [SerializeField]
    public Text mainTip;
    [SerializeField]
    public Image addRole;

}