using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipPropertyData : ItemBaseData
    {

        public TirggerTypeEnum TirggerType;
        public PropertyTypeEnum PropertyType;
        public float TriggerChance;
        public int BufferDataId01;
        public int BufferDataId02;
        public int BufferDataId03;


        public override XmlName ItemXmlName
        {
            get { return XmlName.Equipment; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ItemName = ReadXmlDataMgr.StrParse(node, "Name");
            Description = ReadXmlDataMgr.StrParse(node, "Description");
            TirggerType = (TirggerTypeEnum) ReadXmlDataMgr.IntParse(node, "TirggerType");
            PropertyType = (PropertyTypeEnum)ReadXmlDataMgr.IntParse(node, "PropertyType");
            TriggerChance = ReadXmlDataMgr.FloatParse(node, "TriggerChance");
            BufferDataId01 = ReadXmlDataMgr.IntParse(node, "BufferDataId01");
            BufferDataId02 = ReadXmlDataMgr.IntParse(node, "BufferDataId02");
            BufferDataId03 = ReadXmlDataMgr.IntParse(node, "BufferDataId03");
          
            return base.GetXmlDataAttribute(node);
        }
    }
}
