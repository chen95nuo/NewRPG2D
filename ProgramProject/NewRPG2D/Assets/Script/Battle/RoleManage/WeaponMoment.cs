using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Battle
{
    public class WeaponMoment
    {
        private RoleBase currentHeroRole;

        private List<RoleEquipSpecialBuff> specialBuffs = new List<RoleEquipSpecialBuff>();

        public void SetCurrentRole(RoleBase mRole)
        {
            currentHeroRole = mRole;
        }

        public void SetEquipSlot(EquipTypeEnum equipType, int equipId)
        {
            EquipmentRealProperty equipment = EquipmentMgr.instance.GetEquipmentByEquipId(equipId);
            for (int i = 0; i < equipment.SpecialProperty.Count; i++)
            {
                specialBuffs.Add(GetRealBuff(equipment.SpecialProperty[i]));
            }
            SetPropertyValue(equipment.RoleProperty, equipment.HurtType, equipment.AttackRange, equipment.AttackSpeed);
        }

        private void SetPropertyValue(Dictionary<RoleAttribute, float> roleProperty, HurtTypeEnum hurtType, int attackRange, float attackSpeed)
        {
            PropertyData data = default(PropertyData);
            data.RoleHp += roleProperty[RoleAttribute.HP];
            data.Damage += (roleProperty[RoleAttribute.DPS] / attackSpeed);
            data.MagicAttack += roleProperty[RoleAttribute.INT];
            data.MagicArmor += roleProperty[RoleAttribute.MArmor];
            data.PhysicArmor += roleProperty[RoleAttribute.PArmor];
            data.HitPercent += roleProperty[RoleAttribute.HIT];
            data.AviodHurtPercent += roleProperty[RoleAttribute.Dodge];
            data.CriticalPercent += roleProperty[RoleAttribute.Crt];
            data.HurtType = hurtType;
            data.AttackRange = attackRange;
            data.AttackSpeed = attackSpeed;
            currentHeroRole.RolePropertyValue.InitBaseRoleValue(data);
        }

        public void UpdateLogic(float deltaTime)
        {
            foreach (var buff in specialBuffs)
            {
                buff.UpdateLogic(deltaTime);
            }
        }

        public void Dispose()
        {
            foreach (var buff in specialBuffs)
            {
                buff.Dispose();
            }
        }


        private RoleEquipSpecialBuff GetRealBuff(SpecialPropertyData buffData)
        {
            RoleEquipSpecialBuff buff = null;
            switch (buffData.SpecialPropertyType)
            {
                    case SpecialPropertyEnum.RebornFriend:
                    buff = new RoleEquipSpecialBuff();
                    break;

                default:
                    buff = new RoleEquipSpecialBuff();
                    break;
            }

            buff.Init(currentHeroRole, buffData.param1, buffData.param2, buffData.param3);

            return buff;
        }
    }
}
