using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITypeTip : MonoBehaviour
{
    public Button btn_CardType;
    public UIBagGrid[] grids;
    public Button btn_start;
    public Button btn_back;
    public UIRound round;

    private int currentTeam;
    private void Awake()
    {
        btn_start.onClick.AddListener(ChickStart);
        btn_CardType.onClick.AddListener(ChickCardType);
        btn_back.onClick.AddListener(ChickBack);
    }
    public void UpdateGrids(LessonFightData data)
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].gridType = GridType.Explore;
            grids[i].itemType = ItemType.Prop;
            if (i < data.DropID.Length)
            {
                grids[i].gameObject.SetActive(true);
                ItemData propData = GamePropData.Instance.QueryItem((int)data.DropID[i]);
                DroppingData new_data = new DroppingData(propData.Id, propData.SpriteName, propData.Quality);
                grids[i].UpdateItem(new_data);
            }
            else
            {
                grids[i].gameObject.SetActive(false);
            }
        }
    }
    private void ChickStart()
    {
        int cardNumber = 0;
        currentTeam = GetPlayData.Instance.player[0].CurrentTeam;
        Debug.Log((TeamType)currentTeam);
        List<CardData> cardDatas = BagRoleData.Instance.roles;
        for (int i = 0; i < cardDatas.Count; i++)
        {
            if (cardDatas[i].TeamType == (TeamType)currentTeam)
            {
                cardNumber++;
            }
        }

        if (cardNumber > 0)
        {
            //进入战斗
            CardData[] cardData = new CardData[cardNumber];
            int index = 0;
            for (int i = 0; i < cardDatas.Count; i++)
            {
                if (cardDatas[i].TeamType == (TeamType)currentTeam)
                {
                    cardData[index] = cardDatas[i];
                    index++;
                }
            }
            LessonDropData currentLesson = new LessonDropData(round.currentFight.Id, round.currentFight.DropBoxID);
            int level = GetPlayData.Instance.player[0].Level;
            FightData fightData = new FightData(SceneManager.GetActiveScene().name, cardData, currentLesson, level);

            GoFightMgr.instance.FightMessage(fightData);
            TinyTeam.UI.TTUIPage.ClosePage<UIRoundPage>();
            SceneManager.LoadScene("SceneLoad");
            //SceneManager.LoadScene("Scene_Test");
            Debug.Log("进入战斗场景");
        }
        else
        {
            TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
            GameMessageType Message = new GameMessageType();
            Message.message = "当前战阵人数不足";
            UIEventManager.instance.SendEvent<GameMessageType>(UIEventDefineEnum.UpdateMessageTipEvent, Message);
        }
    }
    private void ChickCardType()
    {
        TinyTeam.UI.TTUIPage.ShowPage<UITeamTypePage>();
    }
    public void ChickBack()
    {
        this.gameObject.SetActive(false);
    }
}
