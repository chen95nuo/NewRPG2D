using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Assets.Script.Battle
{
    public class ValueProperty
    {
        public RolePropertyEnum RoleProperty;  //自己的属性
        public RolePropertyEnum AttackProperty; //克制的属性
        public RolePropertyEnum DefenseProperty; // 被克制的属性
        public HurtTypeEnum HurtType { get; private set; } // 伤害类型
        public float RoleHp { get; private set; }
        public float RoleMp { get; private set; }
        public float MaxHp { get; private set; }
        public float Damage
        {
            get { return UnityEngine.Random.Range(MinDamage, MaxDamage); }
        }

        public float MagicAttack { get; private set; }
        public float MagicArmor { get; private set; }
        public float PhysicArmor { get; private set; }
        public float CriticalPercent { get; private set; }  //暴击
        public float AviodHurtPercent { get; private set; } //闪避
        public float HitPercent { get; private set; }       //命中 

        public float MoveSpeed { get; private set; }
        public float AttackSpeed { get; private set; }
        public int AttackRange;

        public RoleBase AttackRole { get; private set; }
        private float MaxDamage, MinDamage;
        private RoleBase currenRole;
        private bl_HUDText HUDRoot;
        private HUDTextInfo HUDTextInfoinfo;
        private Dictionary<WeaponProfessionEnum, int> attackRangeDictionary = new Dictionary<WeaponProfessionEnum, int>()
        {
            {WeaponProfessionEnum.None, 2},
            {WeaponProfessionEnum.Fighter, 2},
            {WeaponProfessionEnum.Shooter, 20},
            {WeaponProfessionEnum.Magic, 18},
        };

        public void SetCurrentRole(RoleBase mRole)
        {
            currenRole = mRole;
            HUDRoot = bl_UHTUtils.GetHUDText;
            HUDTextInfoinfo = new HUDTextInfo(currenRole.RoleTransform, "");
            HUDTextInfoinfo.Color = Color.white;
            HUDTextInfoinfo.Size = 150;
            HUDTextInfoinfo.Speed = 10;
            HUDTextInfoinfo.VerticalAceleration = -2;
            HUDTextInfoinfo.VerticalFactorScale = 0.1f;
            HUDTextInfoinfo.VerticalPositionOffset = 2.5f;
            HUDTextInfoinfo.Side = bl_Guidance.Down;
        }

        public void InitBaseRoleValue(PropertyData rolePropertyData)
        {
            RoleHp = rolePropertyData.RoleHp;
            MinDamage = rolePropertyData.MinDamage;
            MaxDamage = rolePropertyData.MaxDamage;
            MagicAttack = rolePropertyData.MagicAttack;
            MagicArmor = rolePropertyData.MagicArmor;
            PhysicArmor = rolePropertyData.PhysicArmor;
            CriticalPercent = rolePropertyData.CriticalPercent;
            AviodHurtPercent = rolePropertyData.AviodHurtPercent;
            HitPercent = rolePropertyData.HitPercent;
            MoveSpeed = 5;
            AttackSpeed = rolePropertyData.AttackSpeed;
            HurtType = rolePropertyData.HurtType;
            if (attackRangeDictionary.TryGetValue(rolePropertyData.ProfessionNeed, out AttackRange) == false)
            {
                AttackRange = 2;
            }
            MaxHp = RoleHp;
        }

        public PropertyData GetPropertyData()
        {
            PropertyData data = default(PropertyData);
            data.RoleHp = RoleHp;
            data.MinDamage = MinDamage;
            data.MaxDamage = MaxDamage;
            data.MagicAttack = MagicAttack;
            data.MagicArmor = MagicArmor;
            data.PhysicArmor = PhysicArmor;
            data.CriticalPercent = CriticalPercent;
            data.AviodHurtPercent = AviodHurtPercent;
            data.HitPercent = HitPercent;
            data.AttackSpeed = AttackSpeed;
            data.HurtType = HurtType;
            data.AttackRange = AttackRange;
            return data;
        }


        public void SetMoveSeed(float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }

        public bool SetHp(float HpChange, RoleBase attackRole)
        {
            AttackRole = attackRole;
            return SetHp(HpChange);
        }

        private HpChangeParam HpParam= new HpChangeParam();
        public bool SetHp(float HpChange)
        {
            if (currenRole.IsDead)
            {
                return false;
            }
            if (BattleStaticAndEnum.isGod == false)
            {
                RoleHp -= HpChange;
            }
            HpParam.role = currenRole;
            HpParam.changeValue = HpChange;
            EventManager.instance.SendEvent(EventDefineEnum.HpChange, HpParam);
            if (HpChange < 0)
            {
                HUDTextInfoinfo.Color = Color.green;
            }
            else
            {
                HUDTextInfoinfo.Color = Color.red;
            }
            HUDTextInfoinfo.Text = ((int)HpChange).ToString();
            HUDRoot.NewText(HUDTextInfoinfo);
            return RoleHp > 0;
        }

        public bool SetMp(float MpChange)
        {
            RoleMp -= MpChange;
            return RoleMp > 0;
        }

        public void SetDamage(float attackChange)
        {
            MinDamage += attackChange;
            MaxDamage += attackChange;
        }

        public void SetMagicAttack(float attackChange)
        {
            MagicAttack += attackChange;
        }

        public void SetPhysicArmor(float defenseChange)
        {
            PhysicArmor += defenseChange;
        }

        public void SetMagicArmor(float defenseChange)
        {
            MagicArmor += defenseChange;
        }

        public void SetCriticalPercent(float criticalPercentChange)
        {
            CriticalPercent += criticalPercentChange;
        }

        public void SeAviodHurtPercent(float aviodHurtPercentChange)
        {
            AviodHurtPercent += aviodHurtPercentChange;
        }

        public void SetHitPercent(float hitPercentChange)
        {
            HitPercent += hitPercentChange;
        }
    }
}
