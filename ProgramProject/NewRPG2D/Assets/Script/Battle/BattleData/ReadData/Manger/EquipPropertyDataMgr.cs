using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipPropertyDataMgr : ItemDataBaseMgr<EquipPropertyDataMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.EquipProperty; }
        }
    }
}
