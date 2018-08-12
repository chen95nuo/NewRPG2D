using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Battle
{

    public struct BattleStaticAndEnum
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

    public struct RoleAnimationName
    {
        public const string Move = "run";
        public const string Idle = "stand";
        public const string NormalAttack = "attack";
        public const string Skill1 = "attack1";
        public const string Skill2 = "attack1";
        public const string Hit = "hit";
        public const string Death = "death";
    }

    public struct PropertyData
    {
        public RolePropertyEnum AttackProperty; //克制的属性
        public RolePropertyEnum DefenseProperty; // 被克制的属性
        public float AttackSpeed;
        public float AttackCritial;
        public float MoveSpeed;
        public float HitEnemyHeal;
        public float ReduceCD;
        public float FireHurt;
        public float DizzyChance;
        public float ReflectHurt;
        public float HealTeam;
        public float HealPerSecond;
    }

    public struct PropertyBaseData
    {
        public RolePropertyEnum RoleProperty;  //自己的属性
        public PropertyAddtion Hp;
        public PropertyAddtion Attack;
        public PropertyAddtion Defense;
        public PropertyAddtion Prompt;
    }

    public struct PropertyAddtion
    {
        public float BaseValue;
        public float GrowValueMin;
        public float GrowValueMax;

        public PropertyAddtion(float baseValue, float growValueMin, float growValueMax)
        {
            BaseValue = baseValue;
            GrowValueMin = growValueMin;
            GrowValueMax = growValueMax;
        }
    }

    public struct HurtInfo
    {
        public RoleBase AttackRole;
        public RoleBase TargeRole;
        public HurtTypeEnum HurtType;
        public float HurtValue;
    }

    public struct CreateEnemyInfo
    {
        public BornPositionTypeEnum PositionType;
        public int EnemyPointRoleId;
        public int EnemyCount;
        public float FirstEnemyDelayTime;
        public float IntervalTime;
    }

    public struct AIObjectInfo
    {
        public float MoveSpeed;
    }

    [Serializable]
    public class BornPoint
    {
        public BornPositionTypeEnum BornPositionType;
        public Transform Point;
    }


    #region Enum
    public enum BornPositionTypeEnum
    {
        Point01 = 1,
        Point02,
        Point03,
        Point04,
        Point05,
    }

    public enum TeamTypeEnum
    {
        Hero,
        Monster,
        Other,
    }

    public enum ActorStateEnum
    {
        Idle,
        Run,
        Hit,
        Death,
        NormalAttack,
        Skill1,
        Skill2,
    }

    public enum RoleTypeEnum
    {
        Hero,
        Monster,
        Boss,
        Item,
    }


    public enum SkillSlotTypeEnum
    {
        NormalAttack,
        Skill1,
        Skill2,
        Max,
    }

    public enum HurtTypeEnum
    {
        Physic,
        Magic,
        Real,
        Heal,
    }

    public enum RolePropertyEnum
    {
        Frie,
        Light,
        Dark,
        Water,
    }

    public enum AttackTypeEnum
    {
        ShortRange, //近战
        LongRange,  //远程
    }

    public enum ActorTypeEnum
    {
        Actor,
        Monster,
    }

    public enum SkillTypeEnum
    {
        NextAttack,
        ToSelf,
        ToMyTeam,
        ToEnemy,
        ToAllRandom,
        ToMyTeamRandom,
        ToEnemyRandom,
        NoTarget,
    }

    #endregion
}
