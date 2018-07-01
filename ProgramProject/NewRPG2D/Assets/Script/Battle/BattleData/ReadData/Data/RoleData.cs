using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class RoleData : ItemBaseData
    {

        public string Name;
        public AttackTypeEnum AttackType;
        public ActorTypeEnum ActorType;
        public int SellCoin;
        public int ExpValue;
        public string PrefabPathName;
        public int NormalAttackId;
        public int SkillDataId01;
        public int SkillDataId02;
        public int PropertyId;


        public override XmlName ItemXmlName
        {
            get { return XmlName.RoleData; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            Name = ReadXmlDataMgr.StrParse(node, "Name");
            ReadXmlDataMgr.StrParse(node, "Description");
            AttackType = (AttackTypeEnum) ReadXmlDataMgr.IntParse(node, "AttackType");
            ActorType = (ActorTypeEnum)ReadXmlDataMgr.IntParse(node, "Type");
            SellCoin = ReadXmlDataMgr.IntParse(node, "SellCoin");
            ExpValue = ReadXmlDataMgr.IntParse(node, "ExpValue");
            PrefabPathName = ReadXmlDataMgr.StrParse(node, "Prefab");
            NormalAttackId = ReadXmlDataMgr.IntParse(node, "NormalAttackId");
            SkillDataId01 = ReadXmlDataMgr.IntParse(node, "SkillDataId01");
            SkillDataId02 = ReadXmlDataMgr.IntParse(node, "SkillDataId02");
            PropertyId = ReadXmlDataMgr.IntParse(node, "PropertyId");

            return base.GetXmlDataAttribute(node);
        }
    }
}
