using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;
using Assets.Script.Utility.Tools;
using UnityEngine;

namespace Assets.Script.Battle.BattleData
{
    public class SkillData : ItemBaseData
    {
        public int SkillId;
        public int SkillLevel;
        public int SkillMaxLevel;
        public int MP;
        public int CD;
        public SkillTypeEnum SkillType;
        public float AttackRange;
        public int TargetCount;
        public Sprite Icon;
        public int EffectId1;
        public int EffectId2;
        public int EffectId3;
        public int EffectId4;

        public override XmlName ItemXmlName
        {
            get { return XmlName.SkillData; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ReadXmlDataMgr.StrParse(node, "Description");
            //SkillId = ReadXmlDataMgr.IntParse(node, "SkillID");
            //SkillLevel = ReadXmlDataMgr.IntParse(node, "SkillLevel");
            //SkillMaxLevel = ReadXmlDataMgr.IntParse(node, "MaxLevel");
            MP = ReadXmlDataMgr.IntParse(node, "MP");
            CD = ReadXmlDataMgr.IntParse(node, "CD");
            SkillType = (SkillTypeEnum)ReadXmlDataMgr.IntParse(node, "SkillType");
            AttackRange = ReadXmlDataMgr.FloatParse(node, "AttackRange");
            TargetCount = ReadXmlDataMgr.IntParse(node, "TargetCount");
            Icon = SpriteHelper.CreateSprite(ReadXmlDataMgr.StrParse(node, "Icon"));
            EffectId1 = ReadXmlDataMgr.IntParse(node, "EffectId1");
            EffectId2 = ReadXmlDataMgr.IntParse(node, "EffectId2");
            EffectId3 = ReadXmlDataMgr.IntParse(node, "EffectId3");
            EffectId4 = ReadXmlDataMgr.IntParse(node, "EffectId4");
            return base.GetXmlDataAttribute(node);
        }
    }
}
