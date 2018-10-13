﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{

    public struct RandomPropertyData
    {
        public RoleAttribute AttributeType;
        public RangeData ValueRange;
    }

    public struct SpecialPropertyData
    {
        public SpecialPropertyEnum SpecialPropertyType;
        public float param1;
        public float param2;
        public float param3;
    }

    public struct RangeData
    {
        public float Min, Max;
    }

    public class EquipmentData : ItemBaseData
    {

        public EquipTypeEnum EquipType;
        public QualityTypeEnum QualityType;
        public RangeData LevelRange = new RangeData();
        public RangeData PhysicDamageMinRange = new RangeData();
        public RangeData PyhsicDamageMaxRange = new RangeData();
        public RangeData HPRange = new RangeData();
        public RangeData MagicArmorRange = new RangeData();
        public RangeData PhysicArmorRange = new RangeData();
        public RangeData CritialRange = new RangeData();
        public RangeData AvoidHurtRange = new RangeData();
        public RangeData HitEnemyRange = new RangeData();
        public RangeData MagicDamageRange = new RangeData();
        public RandomPropertyData[] RandomPropertyDatas = new RandomPropertyData[3];
        public int RandomCount;
        public SpecialPropertyData[] SpecialPropertyDatas = new SpecialPropertyData[2];


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
            GetRange(levelRange, out LevelRange.Min, out LevelRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "PhysicDamageMinRange"), out PhysicDamageMinRange.Min, out PhysicDamageMinRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "PyhsicDamageMaxRange"), out PyhsicDamageMaxRange.Min, out PyhsicDamageMaxRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "HPRange"), out HPRange.Min, out HPRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "MagicArmorRange"), out MagicArmorRange.Min, out MagicArmorRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "PhysicArmorRange"), out PhysicArmorRange.Min, out PhysicArmorRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "DropEquipRange"), out CritialRange.Min, out CritialRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "AvoidHurtRange"), out AvoidHurtRange.Min, out AvoidHurtRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "HitEnemyRange"), out HitEnemyRange.Min, out HitEnemyRange.Max);
            GetRange(ReadXmlDataMgr.StrParse(node, "MagicDamageRange"), out MagicDamageRange.Min, out MagicDamageRange.Max);
            for (int i = 0; i < RandomPropertyDatas.Length; i++)
            {
                RandomPropertyDatas[i] = GetRandomPropertyData(node, i + 1);
            }

            RandomCount = ReadXmlDataMgr.IntParse(node, "RandomCount");

            for (int i = 0; i < SpecialPropertyDatas.Length; i++)
            {
                SpecialPropertyDatas[i] = GetSpecialPropertyData(node, i + 1);
            }

            return base.GetXmlDataAttribute(node);
        }

        private void GetRange(string range, out float min, out float max)
        {
            string[] rangeArray = range.Split(',');
            min = float.Parse(rangeArray[0]);
            if (rangeArray.Length > 1)
            {
                max = float.Parse(rangeArray[1]);
            }
            else
            {
                max = min;
            }
        }

        private RandomPropertyData GetRandomPropertyData(XmlNode node, int index)
        {
            RandomPropertyData data = new RandomPropertyData();
            data.AttributeType = (RoleAttribute)ReadXmlDataMgr.IntParse(node, string.Format("RandomProperty{0}Type", index));
            GetRange(ReadXmlDataMgr.StrParse(node, string.Format("RandomProperty{0}Range", index)), out data.ValueRange.Min, out data.ValueRange.Max);
            return data;
        }

        private SpecialPropertyData GetSpecialPropertyData(XmlNode node, int index)
        {
            SpecialPropertyData data = new SpecialPropertyData();
            data.SpecialPropertyType = (SpecialPropertyEnum)ReadXmlDataMgr.IntParse(node, string.Format("SpecialPropertyType{0}", index));
            data.param1 = ReadXmlDataMgr.IntParse(node, string.Format("SpecialProperty{0}Param1", index));
            data.param2 = ReadXmlDataMgr.IntParse(node, string.Format("SpecialProperty{0}Param2", index));
            data.param3 = ReadXmlDataMgr.IntParse(node, string.Format("SpecialProperty{0}Param3", index));

            return data;
        }

    }
}
