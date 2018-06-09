using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Battle
{
    public class BattleStaticAndEnum
    {
        public const float RolePropertyAttackAddtion = 1.15f; //属性克制攻击伤害加成
        public const float RoleCriticalAttackAddtion = 1.5f;  //暴击攻击伤害加成
        public const int RoleBaseCriticalPercent = 2;  //暴击率（100%）
        public const int RolePromptCalculateCritical = 120;  //敏捷计算暴击时的分母
        public const int RolePromptCalculate = 4;  //敏捷计算暴击时插值的分母
        public const float RoleHurtRangePercentMin = 0.8f;  //伤害浮动最小值
        public const float RoleHurtRangePercentMax = 1.2f;  //伤害浮动最大值
        public const int RolePromptOffsetMax = 80;   //敏捷插值最大值
        public const float RolePromptOffsetTime = 0.3f;   //敏捷插值最大值
        public const float RoleHurtPercent = 0.95f;   //攻击伤害比
        public const float RoleDefensePercent = 0.25f;   //防御格挡比
        public const float RoleDefenseRangePercentMin = 0.75f;  //防御浮动最小值
        public const float RoleDefenseRangePercentMax = 1.0f;  //防御浮动最大值
    }

    public struct PropertyData
    {
        public RolePropertyEnum RoleProperty;  //自己的属性
        public RolePropertyEnum AttackProperty; //克制的属性
        public RolePropertyEnum DefenseProperty; // 被克制的属性
        public float Hp;
        public float Mp;
        public float Attack;
        public float Defense;
        public float Prompt;
        public float AttackSpeed;
        public float AttackCritial;
        public float MoveSpeed;
        public float HitEnemyHeal;
        public float ReduceCD;
        public float FireHurt;
        public float DizzyChance;
        public float ReflectHurt;
        public float HealTeam;
        public float BloodHurt;
    }

    public struct HurtInfo
    {
        public RoleBase AttackRole;
        public RoleBase TargeRole;
        public HurtTypeEnum HurtType;
        public float HurtValue;
    }

    #region Enum
    public enum HurtTypeEnum
    {
        Physic,
        Magic,
        Real,
        Heal,
    }

    public enum RolePropertyEnum
    {

    }

    #endregion
}
