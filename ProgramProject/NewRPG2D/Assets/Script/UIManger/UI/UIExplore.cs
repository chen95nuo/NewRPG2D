using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIExplore : MonoBehaviour
{
    public UIExpMapGrid mapGrid;
    public UIExpMenuGrid[] menuGrids;
    public UIExpCardGrid[] cardGrids;
    public Button addMenu;
    public ScrollRect sr;
    public Button btn_back;
    public UIBagGrid[] bagGrids;
    public GameObject waitScene;
    public GameObject fightScene;
    public UIExploreMapControl fightmapGrid;

    private List<UIExpMapGrid> mapGrids = new List<UIExpMapGrid>();
    private UIExpMapGrid currentMap;
    private UIExpMenuGrid currentMenu;
    private UIExpCardGrid currentCard;
    private CardData[] cardData;
    private SpriteAtlas mapSprite;

    private int currentCardId;
    private bool chickStartBtn;


    private void Awake()
    {
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateExploreEvent, UpdateCardData);
        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickAddMenu);
        Init();
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateExploreEvent, UpdateCardData);
        UIEventManager.instance.RemoveListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickAddMenu);
    }
    private void Init()
    {
        mapSprite = Resources.Load<SpriteAtlas>("UISpriteAtlas/ExploreMap");
        btn_back.onClick.AddListener(ChickBtnBack);
        addMenu.onClick.AddListener(ChickAddMenu);

        ChickMenu();
        ChickMap();
        ChickCard();
    }
    #region 初始化
    private void ChickMenu()
    {
        ExpeditionData menus = PlayerExpeditionData.Instance.items;
        int index = 0;
        for (int i = 0; i < menuGrids.Length; i++)
        {
            if (i < menus.ExpeditionTeam.Count)
            {
                menuGrids[i].UpdateMenu(menus.ExpeditionTeam[i]);
                index++;
            }
            else
            {
                //没有这个按钮 就隐藏
                menuGrids[i].UpdateMenu();
            }
        }
        if (index >= 3)
        {
            addMenu.gameObject.SetActive(false);
        }
        currentMenu = menuGrids[0];
        currentMenu.ChickMenu(false);

    }

    private void ChickCard()
    {
        ExpeditionTeam expData = currentMenu.expedData;
        switch (expData.ExploreType)
        {
            case ExploreType.Nothing:
                cardData = new CardData[3];
                cardGrids[0].gameObject.SetActive(false);
                cardGrids[1].UpdateCard(currentMap.exploreData);
                cardGrids[2].gameObject.SetActive(false);
                waitScene.SetActive(true);
                fightScene.SetActive(false);
                break;
            case ExploreType.Run:
                for (int i = 0; i < expData.CardsData.Length; i++)
                {
                    cardData = expData.CardsData;
                }
                waitScene.SetActive(false);
                fightScene.SetActive(true);
                Sprite sp = mapSprite.GetSprite(currentMap.exploreData.SpriteName);
                fightmapGrid.UpdateMap(expData, sp);
                break;
            case ExploreType.End:
                waitScene.SetActive(false);
                fightScene.SetActive(true);
                Sprite sp1 = mapSprite.GetSprite(currentMap.exploreData.SpriteName);
                fightmapGrid.UpdateMap(expData, sp1);
                break;
            default:
                break;
        }
    }

    private void ChickMap()
    {
        List<int> maps = PlayerExpeditionData.Instance.items.UnLockMap;
        int number = 0;
        if (maps.Count <= 2)
        {
            number = 3;
            GridsControl(number);
        }
        else
        {
            GridsControl(maps.Count + 1);
        }
        for (int i = 0; i < maps.Count; i++)
        {
            ExploreData data = GameExploreData.Instance.GetItem(maps[i]);
            Sprite sp = mapSprite.GetSprite(data.IconName);
            mapGrids[i].UpdateMapGrid(data, sp);
        }
        if (number > 0)
        {
            for (int i = maps.Count; i < number; i++)
            {
                mapGrids[i].UpdateMapGrid();
            }
        }
        else
        {
            mapGrids[maps.Count].UpdateMapGrid();
        }
        UpdateMap(mapGrids[0]);
        currentMap.ChickMap(true);
    }
    #endregion

    private void GridsControl(int number)
    {
        mapGrid.gameObject.SetActive(false);
        Transform ts = mapGrid.transform.parent.transform;
        if (mapGrids.Count < number)
        {
            for (int i = mapGrids.Count; i < number; i++)
            {
                GameObject go = Instantiate(mapGrid.gameObject, ts) as GameObject;
                go.SetActive(true);
                UIExpMapGrid gr = go.GetComponent<UIExpMapGrid>();
                mapGrids.Add(gr);
            }
        }
        else
        {
            for (int i = 0; i < mapGrids.Count; i++)
            {
                if (i < number)
                {
                    mapGrids[i].gameObject.SetActive(true);
                }
                else
                {
                    mapGrids[i].gameObject.SetActive(false);
                }
            }
        }

    }


    #region 触发事件
    /// <summary>
    /// 触发队伍按钮
    /// </summary>
    /// <param name="menuData"></param>
    public void UpdateMenu(UIExpMenuGrid menuData)
    {
        if (currentMenu == null || currentMenu != menuData)
        {
            currentMenu.ChickMenu(true);
            menuData.ChickMenu(false);
            currentMenu = menuData;
            chickStartBtn = false;
            ChickCard();
            ChickMap();
        }
    }
    /// <summary>
    /// 触发地图按钮
    /// </summary>
    /// <param name="mapData"></param>
    public void UpdateMap(UIExpMapGrid mapData)
    {
        if (currentMap == null || currentMap != mapData)
        {
            if (currentMap != null)
            {
                currentMap.ChickMap(false);
            }
            else
            {
                currentMap = mapData;
            }
            if (chickStartBtn)
            {
                mapData.btn_Start.gameObject.SetActive(true);
                currentMap.btn_Start.gameObject.SetActive(false);
            }
            else
            {
                mapData.btn_Start.gameObject.SetActive(false);
                currentMap.btn_Start.gameObject.SetActive(false);
            }
            mapData.ChickMap(true);
            currentMap = mapData;

            int[] droppingId = mapData.exploreData.DroppingId;
            //下方显示该地图掉落信息
            Debug.Log("运行了");
            for (int i = 0; i < bagGrids.Length; i++)
            {
                bagGrids[i].gridType = GridType.Explore;
                bagGrids[i].itemType = ItemType.Prop;
                if (i < droppingId.Length)
                {
                    bagGrids[i].gameObject.SetActive(true);
                    ItemData data = GamePropData.Instance.QueryItem(droppingId[i]);
                    Debug.Log(data.SpriteName);
                    DroppingData new_data = new DroppingData(data.Id, data.SpriteName, data.Quality);
                    bagGrids[i].UpdateItem(new_data);
                }
                else
                {
                    bagGrids[i].gameObject.SetActive(false);
                }
            }

        }
    }
    /// <summary>
    /// 触发角色按钮
    /// </summary>
    /// <param name="cardGridData"></param>
    public void UpdateCard(UIExpCardGrid cardGridData)
    {
        for (int i = 0; i < cardGrids.Length; i++)
        {
            if (cardGrids[i] == cardGridData)
            {
                currentCardId = i;
                cardData[i] = cardGridData.cardData;
            }
        }
        int level = 0;
        UpdateCardData data = new UpdateCardData();
        if (currentCardId == 1)
        {
            level = currentMap.exploreData.CaptainLevel;
            data = new UpdateCardData(cardData, GridType.Explore, level, true);
        }
        else
        {
            level = cardData[1].Level;
            data = new UpdateCardData(cardData, GridType.Explore, level, false);
        }
        currentCard = cardGridData;
        TinyTeam.UI.TTUIPage.ShowPage<UICardHousePage>();
        UIEventManager.instance.SendEvent<UpdateCardData>(UIEventDefineEnum.UpdateRolesEvent, data);
    }
    private void UpdateCardData(CardData data)
    {
        currentCard.UpdateCard(data);
        cardData[currentCardId] = data;
        if (currentCardId == 1)
        {
            cardGrids[0].UpdateCard();
            cardGrids[2].UpdateCard();
        }

        //检查人数是否足够
        ChickCardNumber();
    }
    #endregion

    #region 本地事件

    private void ChickCardNumber()
    {
        for (int i = 0; i < cardData.Length; i++)
        {
            if (cardData[i] == null)
            {
                chickStartBtn = false;

                return;
            }
        }
        chickStartBtn = true;
        currentMap.btn_Start.gameObject.SetActive(true);
    }

    private void ChickBtnBack()
    {
        TinyTeam.UI.TTUIPage.ClosePage<UIExplorePage>();
    }

    private void ChickAddMenu()
    {
        TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
        GameMessageType data = new GameMessageType();
        data.currencyType = CurrencyType.Diamonds;
        data.gameOBJ = this.gameObject;
        data.message = "是否增加一个探险队？";
        data.messageType = UIMessageType.BuyOrSell;
        data.number = 2000;
        UIEventManager.instance.SendEvent<GameMessageType>(UIEventDefineEnum.UpdateMessageTipEvent, data);
    }
    private void ChickAddMenu(GameObject go)
    {
        if (go != this.gameObject)
        {
            return;
        }
        int index = PlayerExpeditionData.Instance.items.ExpeditionTeam.Count;
        PlayerExpeditionData.Instance.items.ExpeditionTeam.Add(new ExpeditionTeam(index + 1));
        ChickMenu();
    }

    public void ChickStart()
    {
        //获取小队信息
        ExpeditionTeam teamData = PlayerExpeditionData.Instance.items.ExpeditionTeam[currentMenu.expedData.Id - 1];

        for (int i = 0; i < cardData.Length; i++)
        {
            cardData[i].Fighting = true;
        }
        teamData.CardsData = cardData;
        cardData = new CardData[3];
        teamData.CurrentMap = currentMap.exploreData;
        int needTime = currentMap.exploreData.NeedTime;
        teamData.EndTime = needTime;
        teamData.ExploreType = ExploreType.Run;
        currentMenu.UpdateMenu(teamData);
        ChickCard();
    }

    public void ResetPage()
    {
        ChickMenu();
        ChickMap();
        ChickCard();
    }

    #endregion
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