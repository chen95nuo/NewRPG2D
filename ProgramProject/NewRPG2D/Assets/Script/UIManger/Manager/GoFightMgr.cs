using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFightMgr : TSingleton<GoFightMgr>
{
    public override void Init()
    {

        UIEventManager.instance.AddListener<FightData>(UIEventDefineEnum.FightMessage, FightMessage);
        UIEventManager.instance.AddListener(UIEventDefineEnum.MissionComplete, MissionComplete);
        UIEventManager.instance.AddListener(UIEventDefineEnum.MissionFailed, MissionFailed);

    }

    public LessonDropData currentLesson;
    public CardData[] cardData;
    public string mainScene;

    public void FightMessage(FightData fightData)
    {
        Debug.Log("确认信息");
        cardData = fightData.CardData;

        currentLesson = fightData.CurrentLesson;
        //currentLesson = fightData.CurrentLesson;
        mainScene = fightData.CurrentMap;
    }

    public void MissionComplete()
    {
        //掉落包，角色
        Debug.Log("收到提示");
        //将道具添加到背包并获取道具信息
        DropBagData dropData = GameDropBagData.Instance.GetItem(currentLesson.DropBoxId);
        int addExp = dropData.AddExp;
        int playExp = dropData.AddPlayerExp;
        GainData[] data = GameDropBagData.Instance.GetGains(currentLesson.DropBoxId);
        CardGainData[] cardGainData = GameDropBagData.Instance.GetCards(cardData, addExp, playExp);
        //弹出道具奖励菜单
        TinyTeam.UI.TTUIPage.ShowPage<UIGainTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateGainTipEvent, data);
        //将经验加到角色身上并获取角色信息
        //显示经验奖励面板
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateGainTipEvent, cardGainData);
    }
    public void MissionFailed()
    {

    }
}
