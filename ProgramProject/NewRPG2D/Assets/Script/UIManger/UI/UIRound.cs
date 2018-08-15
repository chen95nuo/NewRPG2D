using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIRound : MonoBehaviour
{
    public GameObject MainPage;
    public GameObject LessonPage;
    public GameObject DifficultyPage;
    public Button btn_round;
    public LessonType lessonType;
    public TeamDifficulty teamType;

    private int currentRound = 0;
    private int currentLesson = 0;
    private int currentDifficulty = 0;
    private int currentTeam = 0;
    private int currentGrid = 0;
    private Button[] btn_rounds;
    private List<RoundData> rounds;
    private List<RoundData> playerRounds;
    private CardData[] cardData = new CardData[4];

    private void Awake()
    {

        Init();

        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateRoundEvent, UpdateGrid);
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateRoundEvent, UpdateCard);

    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<int>(UIEventDefineEnum.UpdateRoundEvent, UpdateGrid);
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateRoundEvent, UpdateCard);


    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        MainPage.SetActive(true);
        LessonPage.SetActive(false);
        DifficultyPage.SetActive(false);

        rounds = GameRoundData.Instance.items;
        btn_rounds = new Button[rounds.Count];
        btn_round.gameObject.SetActive(false);
        for (int i = 0; i < btn_rounds.Length; i++)
        {
            GameObject go = Instantiate(btn_round.gameObject, btn_round.transform.parent.transform) as GameObject;
            go.SetActive(true);
            btn_rounds[i] = go.GetComponent<Button>();
            btn_rounds[i].interactable = false;
            //图片暂无 btn_rounds[i].Image
            btn_rounds[i].onClick.AddListener(ChickRounds);
            btn_rounds[i].GetComponentInChildren<Text>().text = rounds[i].Name;

        }
        UpdateMainLesson();//刷新列表

        lessonType.btn_lessonBack.onClick.AddListener(ChickLessonBack);
        teamType.btn_teamBack.onClick.AddListener(ChickTeamBack);
        teamType.btn_start.onClick.AddListener(ChickStartButton);

        for (int i = 0; i < teamType.btn_team.Length; i++)
        {
            teamType.btn_team[i].onClick.AddListener(ChickTeam);
        }
        for (int i = 0; i < rounds[currentRound].LessonData.Length; i++)
        {
            lessonType.lesson[i].onClick.AddListener(ChickLesson);
        }
        for (int i = 0; i < teamType.btn_difficuly.Length; i++)
        {
            teamType.btn_difficuly[i].onClick.AddListener(ChickDifficuly);
        }
    }

    private void UpdateMainLesson()
    {
        for (int i = 0; i < btn_rounds.Length; i++)
        {
            btn_rounds[i].interactable = false;
        }
        playerRounds = PlayerRoundData.Instance.items;
        RoundData data = new RoundData();
        for (int i = 0; i < playerRounds.Count; i++)
        {
            data = GameRoundData.Instance.GetItem(playerRounds[i].Id);
            data.UnLock = true;
            btn_rounds[playerRounds[i].Id - 1].interactable = true;
            for (int j = 0; j < playerRounds[i].LessonData.Length; j++)
            {
                if (playerRounds[i].LessonData[j].DifficultyType != DifficultyType.Nothing)
                {
                    data.LessonData[j].UnLock = true;
                    data.LessonData[j].DifficultyType = playerRounds[i].LessonData[j].DifficultyType;
                }
            }
        }
    }

    private void UpdateLesson()
    {
        lessonType.MainTip.text = rounds[currentRound].Name;
        for (int i = 0; i < rounds[currentRound].LessonData.Length; i++)
        {
            lessonType.lesson[i].interactable = false;
            if (rounds[currentRound].LessonData[i].UnLock)
            {
                lessonType.lesson[i].interactable = true;
            }
        }
    }

    private void UpdateDifficulty()
    {
        for (int i = 0; i < teamType.btn_difficuly.Length; i++)
        {
            if (i < (int)rounds[currentRound].LessonData[currentLesson].DifficultyType)
            {
                teamType.btn_difficuly[i].interactable = true;
            }
            else
                teamType.btn_difficuly[i].interactable = false;
        }
        for (int i = 0; i < teamType.propGrids.Length; i++)
        {
            if (i < rounds[currentRound].LessonData[currentLesson].LessonDrop[currentDifficulty].DropPropId.Length)
            {
                int index = rounds[currentRound].LessonData[currentLesson].LessonDrop[currentDifficulty].DropPropId[i];
                teamType.propGrids[i].gameObject.SetActive(true);
                teamType.propGrids[i].UpdateItem(GamePropData.Instance.QueryItem(index));
            }
            else
            {
                teamType.propGrids[i].gameObject.SetActive(false);
            }
        }
        teamType.text_fatigue.text = rounds[currentRound].LessonData[currentLesson].NeedFatigue.ToString();

    }

    private void UpdateCard()
    {
        int index = 0;
        cardData = new CardData[4];
        for (int i = 0; i < teamType.cardGrids.Length; i++)
        {
            teamType.cardGrids[i].isCard = false;
            teamType.cardGrids[i].number = i;
            teamType.cardGrids[i].UpdateRoundCard();
        }
        for (int i = 0; i < BagRoleData.Instance.roles.Count; i++)
        {
            if (BagRoleData.Instance.roles[i].TeamType == (TeamType)currentTeam + 1)
            {
                teamType.cardGrids[BagRoleData.Instance.roles[i].TeamPos].UpdateRoundCard(BagRoleData.Instance.roles[i]);
                cardData[BagRoleData.Instance.roles[i].TeamPos] = BagRoleData.Instance.roles[i];
                index++;
            }
        }
        teamType.btn_team[currentTeam].interactable = false;
    }
    private void UpdateCard(CardData data)
    {
        Debug.Log(currentGrid);
        cardData[currentGrid] = data;
        data.TeamPos = currentGrid;
        data.TeamType = (TeamType)(currentTeam + 1);
        teamType.cardGrids[currentGrid].UpdateRoundCard(data);
    }
    private void UpdateGrid(int gridID)
    {
        currentGrid = gridID;
        TinyTeam.UI.TTUIPage.ShowPage<UICardHousePage>();
        UIEventManager.instance.SendEvent<GridType>(UIEventDefineEnum.UpdateRolesEvent, GridType.Team);
        UIEventManager.instance.SendEvent<CardData[]>(UIEventDefineEnum.UpdateRolesEvent, cardData);
    }

    private void ChickRounds()
    {
        Debug.Log("检查区域按钮");
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_rounds.Length; i++)
        {
            if (btn_rounds[i].gameObject == go)
            {
                currentRound = i;
                break;
            }
        }
        if (rounds[currentRound].UnLock)
        {
            LessonPage.SetActive(true);
            MainPage.SetActive(false);
            DifficultyPage.SetActive(false);
            UpdateLesson();
        }
    }
    private void ChickLesson()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < lessonType.lesson.Length; i++)
        {
            if (lessonType.lesson[i].gameObject == go)
            {
                currentLesson = i;
                break;
            }
        }
        if (rounds[currentRound].LessonData[currentLesson].UnLock)
        {
            MainPage.SetActive(false);
            LessonPage.SetActive(false);
            DifficultyPage.SetActive(true);
            UpdateDifficulty();
            UpdateCard();
        }
    }
    private void ChickDifficuly()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < lessonType.lesson.Length; i++)
        {
            if (teamType.btn_difficuly[i].gameObject == go)
            {
                currentDifficulty = i;
                break;
            }
        }
        UpdateDifficulty();
    }
    private void ChickTeam()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < teamType.btn_team.Length; i++)
        {
            teamType.btn_team[currentTeam].interactable = true;
            if (teamType.btn_team[i].gameObject == go)
            {
                currentTeam = i;
            }

        }
        UpdateCard();
    }
    /// <summary>
    /// 开始按钮
    /// </summary>
    private void ChickStartButton()
    {
        int cardNumber = 0;
        //判断位置上是否有卡牌
        for (int i = 0; i < teamType.cardGrids.Length; i++)
        {
            if (teamType.cardGrids[i].isCard)
            {
                cardNumber++;
                if (teamType.cardGrids[i].cardData.Fighting)
                {
                    //提示有角色在战斗中
                    TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateMessageTipEvent, "有角色在探险中无法开启");
                    return;
                }
            }
        }
        //如果有角色那么进入战斗
        if (cardNumber != 0)
        {
            //进入战斗
            Debug.Log("进入战斗");
            CardData[] cardData = new CardData[cardNumber];
            int index = 0;
            for (int i = 0; i < teamType.cardGrids.Length; i++)
            {
                if (teamType.cardGrids[i].isCard)
                {
                    cardData[index] = teamType.cardGrids[i].cardData;
                    index++;
                }
            }
            LessonDropData currentLesson = rounds[currentRound].LessonData[this.currentLesson].LessonDrop[currentDifficulty];
            int level = GetPlayData.Instance.player[0].Level;
            FightData fightData = new FightData(SceneManager.GetActiveScene().name, cardData, currentLesson, level);

            UIEventManager.instance.SendEvent(UIEventDefineEnum.FightMessage, fightData);
            TinyTeam.UI.TTUIPage.ClosePage<UIRoundPage>();
            //Globe.nextSceneName = "Scene_Test";
            //if (SceneManager.GetActiveScene().name == )
            //{

            //}
            SceneManager.LoadScene("Scene_1");

        }
        else
        {
            //提示没有角色无法开始
            TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
            UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateMessageTipEvent, "没有角色无法开始");
        }

        /*开启关卡
        if (playerRounds[currentRound].LessonData[currentLesson].DifficultyType < DifficultyType.Difficult
            && currentDifficulty + 1 == (int)playerRounds[currentRound].LessonData[currentLesson].DifficultyType)
        {
            playerRounds[currentRound].LessonData[currentLesson].DifficultyType++;
        }
        if ((currentLesson + 1) < playerRounds[currentRound].LessonData.Length && playerRounds[currentRound].LessonData[currentLesson + 1].DifficultyType == DifficultyType.Nothing)
        {
            playerRounds[currentRound].LessonData[currentLesson + 1] = new LessonData(DifficultyType.Easy);
        }
        else if ((currentLesson + 1) >= rounds[currentRound].LessonData.Length && (currentRound + 1) < rounds.Count)
        {
            RoundData data = new RoundData(playerRounds[currentRound].Id + 1, new LessonData[4]);
            for (int i = 0; i < data.LessonData.Length; i++)
            {
                data.LessonData[i] = new LessonData(DifficultyType.Nothing);
            }
            data.LessonData[0] = new LessonData(DifficultyType.Easy);
            playerRounds.Add(data);
        }*/

        UpdateMainLesson();
        UpdateDifficulty();
    }
    private void ChickLessonBack()
    {
        MainPage.SetActive(true);
        UpdateMainLesson();
        LessonPage.SetActive(false);
        DifficultyPage.SetActive(false);
    }
    private void ChickTeamBack()
    {
        MainPage.SetActive(false);
        LessonPage.SetActive(true);
        UpdateLesson();
        DifficultyPage.SetActive(false);
    }
}

[System.Serializable]
public class LessonType
{
    public Text MainTip;
    public Button[] lesson;
    public Button btn_lessonBack;
}

[System.Serializable]
public class TeamDifficulty
{
    public Button[] btn_difficuly;
    public Button[] btn_team;
    public UIRoundCard[] cardGrids;
    public UIBagGrid[] propGrids;
    public Text text_fatigue;
    public Button btn_start;
    public Button btn_teamBack;
}
