using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipmentData : ItemBaseData
    {

        public EquipTypeEnum EquipType;
        public QualityTypeEnum QualityType;
        public int NeedLevel;
        public int LevelMin, LevelMax;
        public int DamageMinRange;
        public int DamageMaxRange;
        public int NormalPropertyId1;
        public int NormalPropertyId2;
        public int NormalPropertyId3;
        public int RandomPropertyId1;
        public int RandomPropertyId2;
        public int RandomPropertyId3;
        public int RandomCount;
        public int SpecialPropertyId1;
        public int SpecialPropertyId2;
        public int SlotCount;


        public override XmlName ItemXmlName
        {
            get { return XmlName.Equipment; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ItemName = ReadXmlDataMgr.StrParse(node, "Name");
            Description = ReadXmlDataMgr.StrParse(node, "Description");
            EquipType = (EquipTypeEnum)ReadXmlDataMgr.IntParse(node, "EquipType");
            QualityType = (QualityTypeEnum)ReadXmlDataMgr.IntParse(node, "Quality");

            string levelRange = ReadXmlDataMgr.StrParse(node, "LevelRange");
            string[] levelRangeArray = levelRange.Split(',');
            LevelMin = Int32.Parse(levelRangeArray[0]);
            if (levelRangeArray.Length > 1) LevelMax = Int32.Parse(levelRangeArray[1]);
            NeedLevel = ReadXmlDataMgr.IntParse(node, "NeedLevel");

            DamageMinRange = ReadXmlDataMgr.IntParse(node, "DamageMinRange");
            DamageMaxRange = ReadXmlDataMgr.IntParse(node, "DamageMaxRange");
            NormalPropertyId1 = ReadXmlDataMgr.IntParse(node, "NormalPropertyId1");
            NormalPropertyId2 = ReadXmlDataMgr.IntParse(node, "NormalPropertyId2");
            NormalPropertyId3 = ReadXmlDataMgr.IntParse(node, "NormalPropertyId3");
            RandomPropertyId1 = ReadXmlDataMgr.IntParse(node, "RandomPropertyId1");
            RandomPropertyId2 = ReadXmlDataMgr.IntParse(node, "RandomPropertyId2");
            RandomPropertyId3 = ReadXmlDataMgr.IntParse(node, "RandomPropertyId3");
            RandomCount = ReadXmlDataMgr.IntParse(node, "RandomCount");
            SpecialPropertyId1 = ReadXmlDataMgr.IntParse(node, "SpecialPropertyId1");
            SpecialPropertyId2 = ReadXmlDataMgr.IntParse(node, "SpecialPropertyId2");
            SlotCount = ReadXmlDataMgr.IntParse(node, "SlotCount");


            return base.GetXmlDataAttribute(node);
        }
    }
}
