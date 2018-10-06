using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipBasePropertyData : ItemBaseData
    {

        public RoleAttribute RolePropertyType;
        public int BaseValue;


        public override XmlName ItemXmlName
        {
            get { return XmlName.Equipment; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ItemName = ReadXmlDataMgr.StrParse(node, "Name");
            Description = ReadXmlDataMgr.StrParse(node, "Description");
            RolePropertyType = (RoleAttribute)ReadXmlDataMgr.IntParse(node, "RolePropertyType");
            BaseValue = ReadXmlDataMgr.IntParse(node, "BaseValue");
            return base.GetXmlDataAttribute(node);
        }
    }
}
