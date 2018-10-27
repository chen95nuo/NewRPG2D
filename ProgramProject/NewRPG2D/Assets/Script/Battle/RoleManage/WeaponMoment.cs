using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Battle.BattleData;
using UnityEngine.Analytics;
using UnityScript.Macros;

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
            PropertyData data = currentHeroRole.RolePropertyValue.GetPropertyData();
            data.HurtType = equipment.HurtType;
            data.AttackRange = equipment.AttackRange;
            data.AttackSpeed = equipment.AttackSpeed;
            data.WeaponType = equipment.WeaponType;
            data.ProfessionNeed = equipment.ProfessionNeed;
            SetPropertyValue(equipment.RoleProperty, ref data);
        }

        private void SetPropertyValue(Dictionary<RoleAttribute, float> roleProperty, ref PropertyData data)
        {
            data.RoleHp += roleProperty[RoleAttribute.HP];
            data.Damage += (roleProperty[RoleAttribute.DPS] / data.AttackRange);
            data.MagicAttack += roleProperty[RoleAttribute.INT];
            data.MagicArmor += roleProperty[RoleAttribute.MArmor];
            data.PhysicArmor += roleProperty[RoleAttribute.PArmor];
            data.HitPercent += roleProperty[RoleAttribute.HIT];
            data.AviodHurtPercent += roleProperty[RoleAttribute.Dodge];
            data.CriticalPercent += roleProperty[RoleAttribute.Crt];

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

        public void TriggerBuff(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            foreach (var buff in specialBuffs)
            {
                buff.Trigger(tirggerType, ref info);
            }
        }

        private RoleEquipSpecialBuff GetRealBuff(SpecialPropertyData buffData)
        {
            RoleEquipSpecialBuff buff = null;
            switch (buffData.SpecialPropertyType)
            {
                case SpecialPropertyEnum.RebornFriend:
                    buff = new RebornFriendBuff();
                    break;
                case SpecialPropertyEnum.ExtraDamage:
                    buff = new ExtraDamageBuff();
                    break;
                case SpecialPropertyEnum.IncreaseAttackWhenReborn:
                    buff = new IncreaseAttackWhenReborn();
                    break;
                case SpecialPropertyEnum.HurtAllEnemy:
                    buff = new HurtAllEnemy();
                    break;
                case SpecialPropertyEnum.Dizzy:
                    buff = new DizzyBuff();
                    break;
                case SpecialPropertyEnum.ContinueDamage:
                    buff = new ContinueDamage();
                    break;
                case SpecialPropertyEnum.IncreaseCritial:
                    buff = new IncreaseCritial();
                    break;
                case SpecialPropertyEnum.IncreaseDamageWhenTargetDead:
                    buff = new IncreaseDamageWhenTargetDead();
                    break;
                case SpecialPropertyEnum.IncreanseDamageWhenAttack:
                    buff = new IncreanseDamageWhenAttack();
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
