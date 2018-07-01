using Assets.Script.Utility;
using System;
using System.Collections.Generic;

namespace Assets.Script.Battle.BattleData.ReadData
{
    public class RolePropertyDataMgr : ItemDataBaseMgr<RolePropertyDataMgr>
    {
        public override XmlName CurrentXmlName
        {
            get { return XmlName.RolePropertyData; }
        }
    }
}
