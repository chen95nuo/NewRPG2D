using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Button[] btn_rounds;
    private List<RoundData> rounds;
    private List<RoundData> playerRounds;
    private CardData[] cardData;

    private void Awake()
    {

        Init();

    }

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
                data.LessonData[j].UnLock = true;
                data.LessonData[j].DifficultyType = playerRounds[i].LessonData[j].DifficultyType;
            }
        }
    }

    private void UpdateLesson()
    {
        lessonType.MainTip.text = rounds[currentRound].Name;
        for (int i = 0; i < rounds[currentRound].LessonData.Length; i++)
        {
            lessonType.lesson[i].onClick.AddListener(ChickLesson);
            if (!rounds[currentRound].LessonData[i].UnLock)
            {
                lessonType.lesson[i].interactable = false;
            }
        }
    }

    private void UpdateDifficulty()
    {
        for (int i = 0; i < teamType.btn_difficuly.Length; i++)
        {
            teamType.btn_difficuly[i].onClick.AddListener(ChickDifficuly);
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
            teamType.cardGrids[i].number = i;
            teamType.cardGrids[i].UpdateRoundCard();
        }
        for (int i = 0; i < BagRoleData.Instance.roles.Count; i++)
        {
            if (BagRoleData.Instance.roles[i].TeamType == (TeamType)currentTeam + 1)
            {
                teamType.cardGrids[BagRoleData.Instance.roles[i].TeamPos].UpdateRoundCard(BagRoleData.Instance.roles[i]);
                cardData[index] = BagRoleData.Instance.roles[i];
                index++;
            }
        }
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

    }
}

[System.Serializable]
public class LessonType
{
    public Text MainTip;
    public Button[] lesson;
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
}
