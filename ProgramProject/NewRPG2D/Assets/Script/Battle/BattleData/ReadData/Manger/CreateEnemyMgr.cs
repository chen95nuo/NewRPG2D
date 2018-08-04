using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class CreateEnemyMgr : ItemDataBaseMgr<CreateEnemyMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.CreateEnemyData; }
        }
    }
}
