using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class PropDataMgr : ItemDataBaseMgr<PropDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.PropData; }
    }
}
