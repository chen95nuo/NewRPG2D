using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class RolePropertyData : ItemBaseData
    {
        public string Name;
        public PropertyData RoleBaseData;
        public override XmlName ItemXmlName
        {
            get { return XmlName.RolePropertyData; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            RoleBaseData = new PropertyData();
           // Name = ReadXmlDataMgr.StrParse(node, "Name");
            ReadXmlDataMgr.StrParse(node, "Description");
            RoleBaseData.RoleProperty = (RolePropertyEnum)ReadXmlDataMgr.IntParse(node, "RoleProperty");
            RoleBaseData.AttackProperty = (RolePropertyEnum)ReadXmlDataMgr.IntParse(node, "AttackProperty");
            RoleBaseData.DefenseProperty = (RolePropertyEnum)ReadXmlDataMgr.IntParse(node, "DefenseProperty");
            float baseValue = ReadXmlDataMgr.IntParse(node, "BaseHP");
            float growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowHPMin");
            float growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowHPMax");
            RoleBaseData.Hp = new PropertyAddtion(baseValue, growValueMin, growValueMax);
            baseValue = ReadXmlDataMgr.IntParse(node, "BaseAttack");
            growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowAttackMin");
            growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowAttackMax");
            RoleBaseData.Attack = new PropertyAddtion(baseValue, growValueMin, growValueMax);
            baseValue = ReadXmlDataMgr.IntParse(node, "BaseDefense");
            growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowDefenseMin");
            growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowDefenseMax");
            RoleBaseData.Defense = new PropertyAddtion(baseValue, growValueMin, growValueMax);
            baseValue = ReadXmlDataMgr.FloatParse(node, "BasePrompt");
            growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowPromptMin");
            growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowPromptMax");
            RoleBaseData.Prompt = new PropertyAddtion(baseValue, growValueMin, growValueMax);
            RoleBaseData.AttackSpeed = ReadXmlDataMgr.FloatParse(node, "AttackSpeed");
            RoleBaseData.MoveSpeed = ReadXmlDataMgr.FloatParse(node, "BaseMoveSpeed");
            RoleBaseData.DizzyChance = ReadXmlDataMgr.FloatParse(node, "Dizzy");
            RoleBaseData.HitEnemyHeal = ReadXmlDataMgr.FloatParse(node, "DrainHP");
            RoleBaseData.ReduceCD = ReadXmlDataMgr.FloatParse(node, "ReduceCDTime");
            RoleBaseData.HealTeam = ReadXmlDataMgr.FloatParse(node, "Heal");
            RoleBaseData.HealPerSecond = ReadXmlDataMgr.FloatParse(node, "HealPerSecond");
            RoleBaseData.FireHurt = ReadXmlDataMgr.FloatParse(node, "BurnHurt");
            RoleBaseData.ReflectHurt = ReadXmlDataMgr.FloatParse(node, "ReflectHurt");
            return base.GetXmlDataAttribute(node);
        }
    }
}
