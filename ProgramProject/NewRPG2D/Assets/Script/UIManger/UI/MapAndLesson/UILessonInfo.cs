using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

public class UILessonInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_needNumb;
    public Text txt_Tip_1;
    public CreateEnemyData currentLesson;

    public GameObject roleInfo;
    public GameObject EnemyInfo;

    public override void Show(object mData)
    {
        base.Show(mData);
        CreateEnemyData data = mData as CreateEnemyData;
        UpdateInfo(data);
    }

    private void UpdateInfo(CreateEnemyData data)
    {
        currentLesson = data;

    }


}
