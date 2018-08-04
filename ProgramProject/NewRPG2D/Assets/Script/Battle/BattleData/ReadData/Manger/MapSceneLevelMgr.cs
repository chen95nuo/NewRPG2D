using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class MapSceneLevelMgr : ItemDataBaseMgr<MapSceneLevelMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.MapSceneLevel; }
        }
    }
}
