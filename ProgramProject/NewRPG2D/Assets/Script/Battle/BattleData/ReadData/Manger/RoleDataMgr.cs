using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class RoleDataMgr : ItemDataBaseMgr<RoleDataMgr>
    {
        public override XmlName CurrentXmlName
        {
            get { return XmlName.SkillData; }
        }
    }
}
