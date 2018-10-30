using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Tools;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class RolePropertyData : ItemBaseData
    {
        //public string Name;
        public string PrefabName;
        public HurtTypeEnum HurtType;
        public WeaponProfessionEnum Profession;
        public int EquipmentId;
        public float AttackSpeed;
        public string SpriteName;
        public float Level;
        public float DamageMin;
        public float DamageMax;
        public float HP;
        public float MagicArmor;
        public float PhysicArmor;
        public float Critial;
        public float AvoidHurt;
        public float HitEnemy;
        public float MagicDamage;
        public SpecialPropertyData[] SpecialPropertyDatas = new SpecialPropertyData[2];

        public override XmlName ItemXmlName
        {
            get { return XmlName.RolePropertyData; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
         
            ReadXmlDataMgr.StrParse(node, "Description");
            //ItemName = ReadXmlDataMgr.StrParse(node, "Name");
            Description = ReadXmlDataMgr.StrParse(node, "Description");
            PrefabName = ReadXmlDataMgr.StrParse(node, "PrefabName");
             Profession = (WeaponProfessionEnum)ReadXmlDataMgr.IntParse(node, "Profession");
            HurtType = (HurtTypeEnum)ReadXmlDataMgr.IntParse(node, "HurtType");
            EquipmentId = ReadXmlDataMgr.IntParse(node, "EquipmentId");
            SpriteName = ReadXmlDataMgr.StrParse(node, "SpriteName");
            AttackSpeed = ReadXmlDataMgr.FloatParse(node, "AttackSpeed");
           // Level = ReadXmlDataMgr.FloatParse(node, "Level");
            StringHelper.instance.GetRange(ReadXmlDataMgr.StrParse(node, "Damage"), out DamageMin, out DamageMax);
            HP = ReadXmlDataMgr.FloatParse(node, "Hp");
            MagicArmor = ReadXmlDataMgr.FloatParse(node, "MagicArmor");
            Critial = ReadXmlDataMgr.FloatParse(node, "Critial");
            PhysicArmor = ReadXmlDataMgr.FloatParse(node, "PhysicArmor");
            AvoidHurt = ReadXmlDataMgr.FloatParse(node, "AvoidHurt");
            HitEnemy = ReadXmlDataMgr.FloatParse(node, "HitEnemy");
          //  MagicDamage = ReadXmlDataMgr.FloatParse(node, "MagicDamage");

            for (int i = 0; i < SpecialPropertyDatas.Length; i++)
            {
                SpecialPropertyDatas[i] = GetSpecialPropertyData(node, i + 1);
            }
            return base.GetXmlDataAttribute(node);
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
