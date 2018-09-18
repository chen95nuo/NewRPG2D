using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class BuildingDataMgr : ItemDataBaseMgr<BuildingDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.BuildingData; }
    }
}
