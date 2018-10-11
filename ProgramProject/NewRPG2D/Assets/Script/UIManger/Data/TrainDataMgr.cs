using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class TrainDataMgr : ItemDataBaseMgr<TrainDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.TrainData; }
    }

    public int GetTrainData(TrainType type, int level)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            TrainData data = CurrentItemData[i] as TrainData;
            if (data.trainType == type && (data.level[0] < level && level < data.level[1]))
            {
                return data.needTime * 5;
            }
        }
        Debug.LogError("没有找到需要的训练类Data");
        return 0;
    }
}
