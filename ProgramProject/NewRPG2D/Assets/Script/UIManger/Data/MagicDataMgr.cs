using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class MagicDataMgr : ItemDataBaseMgr<MagicDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.MagicData; }
    }

    public MagicData GetMagic(MagicName name, int level)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            MagicData temp = CurrentItemData[i] as MagicData;
            if (temp.magicName == name && temp.level == level)
            {
                return temp;
            }
        }
        return null;
    }
}
