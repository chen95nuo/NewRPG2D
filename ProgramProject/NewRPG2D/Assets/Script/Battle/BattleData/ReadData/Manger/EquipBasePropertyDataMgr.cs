using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipBasePropertyDataMgr : ItemDataBaseMgr<EquipBasePropertyDataMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.EquipBaseProperty; }
        }
    }
}
