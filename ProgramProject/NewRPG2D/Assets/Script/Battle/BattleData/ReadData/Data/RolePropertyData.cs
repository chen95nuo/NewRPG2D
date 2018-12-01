using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Tools;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class RolePropertyData : ItemBaseCsvData
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

        public override CsvEChartsType ItemCsvName
        {
            get { return CsvEChartsType.RolePropertyData; }
        }

        public override bool AnalySis(string[] data)
        {
            if (base.AnalySis(data))
            {
                Description = StrParse(data, 1);
                EquipmentId = IntParse(data, 2);
                PrefabName = StrParse(data, 3);
                SpriteName = StrParse(data, 4);
                Profession = (WeaponProfessionEnum)IntParse(data, 5);
                HurtType = (HurtTypeEnum)IntParse(data, 6);
                AttackSpeed = FloatParse(data, 7);
                HP = FloatParse(data, 8);
                PhysicArmor = FloatParse(data, 9);
                MagicArmor = FloatParse(data, 10);
                AvoidHurt = FloatParse(data, 11);
                Critial = FloatParse(data, 12);
                HitEnemy = FloatParse(data, 13);
                List<float> listValue = ListParse(data, 14);
                if (listValue != null && listValue.Count >0 )
                {
                    DamageMin = listValue[0];
                    DamageMax = listValue.Count > 1 ? listValue[1] : DamageMin;
                }

                for (int i = 0; i < SpecialPropertyDatas.Length; i++)
                {
                    SpecialPropertyDatas[i] = GetSpecialPropertyData(data, 4 * i + 15);
                }

                return true;
            }
            return false;
        }

        private SpecialPropertyData GetSpecialPropertyData(string[] Csvdata, int index)
        {
            SpecialPropertyData data = new SpecialPropertyData();
            data.SpecialPropertyType = (SpecialPropertyEnum)IntParse(Csvdata, index);
            data.param1 = IntParse(Csvdata, index + 1);
            data.param2 = IntParse(Csvdata, index + 2);
            data.param3 = IntParse(Csvdata, index + 3);
            data.param4 = 0;
            return data;
        }
    }
}
