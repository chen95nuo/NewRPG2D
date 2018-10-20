using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class TreasureBoxDataMgr : ItemDataBaseMgr<TreasureBoxDataMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.TreasureBox; }
        }
    }
}
