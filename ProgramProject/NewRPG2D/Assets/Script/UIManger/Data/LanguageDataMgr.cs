using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class LanguageDataMgr : ItemDataBaseMgr<LanguageDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.LanguageData; }
    }

    public LanguageData GetString(string message)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            LanguageData data = CurrentItemData[i] as LanguageData;
            if (data.GetName == message)
            {
                return data;
            }
        }
        return null;
    }
}
