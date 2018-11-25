
using System.Collections.Generic;
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

        public void SetEquipSlot(int equipId, SexTypeEnum sexType)
        {
            EquipmentRealProperty equipment = EquipmentMgr.instance.GetEquipmentByEquipId(equipId);
            if (equipment == null)
            {
                return;
            }
            currentHeroRole.MonoRoleRender.CurrentRoleEquip.ChangeEquip(equipment.EquipType, equipment.EquipName, sexType);
            for (int i = 0; i < equipment.SpecialProperty.Count; i++)
            {
                AddSpecialBuffs(equipment.SpecialProperty[i]);
            }
            PropertyData data = currentHeroRole.RolePropertyValue.GetPropertyData();
            data.HurtType = equipment.HurtType;
            data.AttackRange = equipment.AttackRange;
            data.AttackSpeed = equipment.AttackSpeed;
            data.WeaponType = equipment.WeaponType;
            if (equipment.EquipType == EquipTypeEnum.Sword)
            {
                data.ProfessionNeed = equipment.WeaponProfession;
            }
            SetPropertyValue(equipment.RoleProperty, ref data);
        }

        public void AddSpecialBuffs(SpecialPropertyData data)
        {
            specialBuffs.Add(GetRealBuff(data));
        }

        private void SetPropertyValue(Dictionary<RoleAttribute, float> roleProperty, ref PropertyData data)
        {
            data.RoleHp += roleProperty[RoleAttribute.HP];
            data.MinDamage += (roleProperty[RoleAttribute.MinDamage]);
            data.MaxDamage += (roleProperty[RoleAttribute.MaxDamage]);
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
                case SpecialPropertyEnum.HealFriendHp:
                    buff = new HealFriendHp();
                    break;
                case SpecialPropertyEnum.HealFriendHpBuff:
                    buff = new HealFriendHpBuff();
                    break;
                case SpecialPropertyEnum.ExtraDamage:
                    buff = new ExtraDamage();
                    break;
                case SpecialPropertyEnum.ExtraDamageBuff:
                    buff = new ExtraDamageBuff();
                    break;
                case SpecialPropertyEnum.ExtraAllEnemyDamage:
                    buff = new ExtraAllEnemyDamage();
                    break;
                case SpecialPropertyEnum.ExtraAllEnemyDamageBuff:
                    buff = new ExtraAllEnemyDamageBuff();
                    break;
                case SpecialPropertyEnum.IncreaseFriendsArmorBuff:
                    buff = new IncreaseFriendsArmorBuff();
                    break;
                case SpecialPropertyEnum.IncreaseFriendsHpBuff:
                    buff = new IncreaseFriendsHpBuff();
                    break;
                case SpecialPropertyEnum.DizzyBuff:
                    buff = new DizzyBuff();
                    break;
                case SpecialPropertyEnum.RebornThenIncreaseDamage:
                    buff = new RebornThenIncreaseDamage();
                    break;
                case SpecialPropertyEnum.AttackThenExtraAllEnemyDamage:
                    buff = new AttackThenExtraAllEnemyDamage();
                    break;
                case SpecialPropertyEnum.AttackThenDizzy:
                      buff = new AttackThenDizzy();
                    break;
                case SpecialPropertyEnum.AttackThenExtraAllEnemyDamageBuff:
                    buff = new AttackThenExtraAllEnemyDamageBuff();
                    break;
                case SpecialPropertyEnum.AttackThenIncreaseCritialBuff:
                    buff = new AttackThenIncreaseCritial();
                    break;
                case SpecialPropertyEnum.AttackThenReduceTargetArmorBuff:
                    buff = new AttackThenReduceTargetArmorBuff();
                    break;
                case SpecialPropertyEnum.AttackThenIncreaseHpBuff:
                    buff = new AttackThenIncreaseHpBuff();
                    break;
                case SpecialPropertyEnum.AttackThenIncreaseAvoidBuff:
                    buff = new AttackThenIncreaseAvoidBuff();
                    break;
                case SpecialPropertyEnum.AttackThenIncreaseSelfArmor:
                    buff = new AttackThenIncreaseSelfArmor();
                    break;
                case SpecialPropertyEnum.AttackThenIncreaseMagicArmor:
                    buff = new AttackThenIncreaseMagicArmor();
                    break;
                case SpecialPropertyEnum.TargetDeadThenIncreaseDamage:
                    buff = new TargetDeadThenIncreaseDamage();
                    break;
                case SpecialPropertyEnum.AttackThenIncreaseDamageBuff:
                    buff = new AttackThenIncreaseDamageBuff();
                    break;
                case SpecialPropertyEnum.AttackThenIcreaseAFriendDamageBuff:
                    buff = new AttackThenIcreaseAFriendDamageBuff();
                    break;
                case SpecialPropertyEnum.HurtThenHealHp:
                    buff = new HurtThenHealHp();
                    break;
                case SpecialPropertyEnum.AttackThenHealFriendHp:
                    buff = new AttackThenHealFriendHp();
                    break;
                default:
                    buff = new RoleEquipSpecialBuff();
                    break;
            }

            buff.Init(currentHeroRole, buffData.param1, buffData.param2, buffData.param3, buffData.param4);

            return buff;
        }
    }
}
