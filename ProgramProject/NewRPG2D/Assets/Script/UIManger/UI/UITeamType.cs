using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamType : MonoBehaviour
{
    public UIRound round;
    public UIRoundCard[] cardGrids;
    public Button[] btn_menus;
    private CardData[] cardData;
    private int currentTeam = 0;
    private int currentGrid = 0;
    public Button btn_back;

    private void Awake()
    {
        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateRoundEvent, ChickCard);
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateRoundEvent, UpdateCard);

        Init();
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<int>(UIEventDefineEnum.UpdateRoundEvent, ChickCard);
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateRoundEvent, UpdateCard);
    }

    private void Init()
    {
        UpdateCard();
        btn_back.onClick.AddListener(ChickBack);

        for (int i = 0; i < btn_menus.Length; i++)
        {
            btn_menus[i].onClick.AddListener(ChickMenu);
        }
    }
    private void UpdateCard()
    {
        int index = 0;
        cardData = new CardData[4];
        Debug.Log(cardGrids.Length);
        for (int i = 0; i < cardGrids.Length; i++)
        {
            cardGrids[i].isCard = false;
            cardGrids[i].number = i;
            cardGrids[i].UpdateRoundCard();
        }
        List<CardData> data = BagRoleData.Instance.roles;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].TeamType == (TeamType)currentTeam + 1)
            {
                cardGrids[data[i].TeamPos].UpdateRoundCard(data[i]);
                cardData[data[i].TeamPos] = data[i];
                index++;
            }
        }
        btn_menus[currentTeam].interactable = false;
    }
    private void UpdateCard(CardData data)
    {
        Debug.Log(currentGrid);
        cardData[currentGrid] = data;
        data.TeamPos = currentGrid;
        data.TeamType = (TeamType)(currentTeam + 1);
        cardGrids[currentGrid].UpdateRoundCard(data);
    }
    private void ChickCard(int number)
    {
        currentGrid = number;
        TinyTeam.UI.TTUIPage.ShowPage<UICardHousePage>();
        UpdateCardData data = new UpdateCardData(cardData, GridType.Team);
        UIEventManager.instance.SendEvent<UpdateCardData>(UIEventDefineEnum.UpdateRolesEvent, data);
    }
    private void ChickMenu()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        btn_menus[currentTeam].interactable = true;
        for (int i = 0; i < btn_menus.Length; i++)
        {
            if (btn_menus[i].gameObject == go)
            {
                currentTeam = i;
            }
        }
        GetPlayData.Instance.player[0].CurrentTeam = currentTeam + 1;
        UpdateCard();
    }
    private void ChickBack()
    {
        TinyTeam.UI.TTUIPage.ClosePage<UITeamTypePage>();
    }
}
