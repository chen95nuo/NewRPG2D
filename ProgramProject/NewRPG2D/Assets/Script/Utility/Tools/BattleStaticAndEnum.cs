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


        #region for test

        public static bool isGod;

        #endregion
    }

    public struct RoleAnimationName
    {
        public const string Move = "run";
        public const string Idle = "stand";
        public const string NormalAttack = "attack";
        public const string Skill1 = "attack1";
        public const string Skill2 = "attack1";
        public const string Hit = "hit";
        public const string Death = "dead";
        public const string AttackArrow = "attack_arrow";
        public const string AttackCut = "attack_cut";
        public const string AttackHand = "attack_hand";
        public const string Win = "joy";
    }

    public struct PropertyData
    {
        public HurtTypeEnum HurtType;
        public WeaponTypeEnum WeaponType;
        public WeaponProfessionEnum ProfessionNeed;
        public float RoleHp;
        public float RoleMp;
        public float MaxDamage;
        public float MinDamage;
        public float MagicAttack;
        public float MagicArmor;
        public float PhysicArmor;
        public float CriticalPercent;
        public float AviodHurtPercent;
        public float HitPercent;
        public float AttackSpeed;
        public float MoveSpeed;
        public int AttackRange;
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
        Point01 = 0,
        Point02,
        Point03,
        Point04,
        Point05,
        Point06,
        Point07,
        Point08,
        Point09,
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
        Win,
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
        None,
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
