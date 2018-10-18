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
        public const string Death = "death";
    }

    public struct PropertyData
    {
        public HurtTypeEnum HurtType;
        public WeaponTypeEnum WeaponType;
        public ProfessionNeedEnum ProfessionNeed;
        public float RoleHp;
        public float RoleMp;
        public float Damage;
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


    public enum SpecialPropertyEnum
    {
        None,
        RebornFriend = 1,               ///以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
        ExtraDamage,                    ///攻击时，有x%概率在y秒内造成z%点分裂伤害。
        IncreaseAttackWhenReborn,       ///以任何形式复活后，伤害提升x%。持续y秒。
        HurtAllEnemy,                   ///攻击时，有x%概率对全体敌人造成y%点伤害
        Dizzy,                          ///攻击时，有x%概率使敌人眩晕y秒
        ContinueDamage,                 ///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
        IncreaseCritial,                ///攻击时，有x%概率提高自身y%的暴击。持续z秒。
        IncreaseDamageWhenTargetDead,   ///当前攻击目标被击杀时，x秒内造成的伤害提高y%。
        IncreanseDamageWhenAttack,      ///每次攻击都提升x%点伤害，当前目标死亡时，损失所有伤害提升。
        ReduceTargetArmor,              ///攻击时，有x%概率使一名敌人的护甲降低y%，持续z秒
        IncreasePhysicDamage,           ///攻击时，有x%概率使物理伤害提高y%，持续z秒
        IncreaseMagicDamage,            ///攻击时，有x%概率使魔法伤害提高y%，持续z秒
        HealSelfHp,                     ///攻击时，有x概率使自身回复y%生命值
        HealFriendHp,                   ///恢复一名受伤程度最重的友方x点生命值。Y秒冷却
        ReduceEnemyDamage,              ///攻击时，有x%的概率降低敌人伤害的y%。持续z秒
        IncreaseArmor,                  ///受到伤害时，有x%概率使自身护甲增加y点，持续z秒
        BornSmallMonster,               ///每隔x秒，召唤两个y生命值，z伤害的骷髅帮助作战。
        GodWhenHurt,                    ///受到伤害时，有X%概率对自己使用干涉，回复Y点生命值。在干涉期间将不会受到攻击，也无法移动或攻击。持续Z秒
        HurtAllEnemyWhenDead,           ///死亡时，对敌人造成x点范围伤害。
        DizzyAllEnemyWhenDead,          ///死亡时，眩晕范围内敌人x秒
        IncreaseAvoidHurtChanceWhenHurt,///受到攻击时，有x%概率提升x%闪躲。
        BornSmallMonsterWhenDead,       ///死亡时，召唤两个x生命值，y伤害的骷髅帮助作战。
        HealHpWhenHurt,                 ///受到伤害时，有x%概率为自己恢复x%点生命值
        HealHpPerSecondWhenHurt,        ///受到伤害时，有x%概率在y秒内为自己恢复x%点生命值
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
