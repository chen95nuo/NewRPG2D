using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipmentDataMgr : ItemDataBaseMgr<EquipmentDataMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.Equipment; }
        }
    }
}
