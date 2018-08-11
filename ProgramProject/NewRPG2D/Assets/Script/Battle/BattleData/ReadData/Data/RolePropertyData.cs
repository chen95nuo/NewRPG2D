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
        public PropertyData RoleOtherData;
        public PropertyBaseData RoleBaseData;
        public override XmlName ItemXmlName
        {
            get { return XmlName.RolePropertyData; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            RoleBaseData = new PropertyBaseData();
            RoleOtherData = new PropertyData();
           // Name = ReadXmlDataMgr.StrParse(node, "Name");
            ReadXmlDataMgr.StrParse(node, "Description");
            //RoleBaseData.RoleProperty = (RolePropertyEnum)ReadXmlDataMgr.IntParse(node, "RoleProperty");
            RoleOtherData.AttackProperty = (RolePropertyEnum)ReadXmlDataMgr.IntParse(node, "AttackProperty");
            RoleOtherData.DefenseProperty = (RolePropertyEnum)ReadXmlDataMgr.IntParse(node, "DefenseProperty");

            //float baseValue = ReadXmlDataMgr.IntParse(node, "BaseHP");
            //float growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowHPMin");
            //float growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowHPMax");
            //RoleBaseData.Hp = new PropertyAddtion(baseValue, growValueMin, growValueMax);

            //baseValue = ReadXmlDataMgr.IntParse(node, "BaseAttack");
            //growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowAttackMin");
            //growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowAttackMax");
            //RoleBaseData.Attack = new PropertyAddtion(baseValue, growValueMin, growValueMax);

            //baseValue = ReadXmlDataMgr.IntParse(node, "BaseDefense");
            //growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowDefenseMin");
            //growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowDefenseMax");
            //RoleBaseData.Defense = new PropertyAddtion(baseValue, growValueMin, growValueMax);

            //baseValue = ReadXmlDataMgr.FloatParse(node, "BasePrompt");
            //growValueMin = ReadXmlDataMgr.FloatParse(node, "GrowPromptMin");
            //growValueMax = ReadXmlDataMgr.FloatParse(node, "GrowPromptMax");
            //RoleBaseData.Prompt = new PropertyAddtion(baseValue, growValueMin, growValueMax);

            RoleOtherData.AttackSpeed = ReadXmlDataMgr.FloatParse(node, "AttackSpeed");
            RoleOtherData.MoveSpeed = ReadXmlDataMgr.FloatParse(node, "BaseMoveSpeed");
            RoleOtherData.DizzyChance = ReadXmlDataMgr.FloatParse(node, "Dizzy");
            RoleOtherData.HitEnemyHeal = ReadXmlDataMgr.FloatParse(node, "DrainHP");
            RoleOtherData.ReduceCD = ReadXmlDataMgr.FloatParse(node, "ReduceCDTime");
            RoleOtherData.HealTeam = ReadXmlDataMgr.FloatParse(node, "Heal");
            RoleOtherData.HealPerSecond = ReadXmlDataMgr.FloatParse(node, "HealPerSecond");
            RoleOtherData.FireHurt = ReadXmlDataMgr.FloatParse(node, "BurnHurt");
            RoleOtherData.ReflectHurt = ReadXmlDataMgr.FloatParse(node, "ReflectHurt");
            return base.GetXmlDataAttribute(node);
        }
    }
}
