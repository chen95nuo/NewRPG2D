using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script
{
    class BattleBuffEnum
    {
    }

    public enum BuffEffectTypeEnum
    {
        None,
        Buff,                 //增益
        Debuff,               //减益
    }

    public enum BuffStateEnum
    {
        Running,
        Finish,
    }

    public enum BuffTypeEnum
    {
        None,
        HealHp,
        ExtraDamage,
        ChangeArmor,
        ChangeMaxHp,
        Dizzy,
        IncreaseDamage,
        ChangeCritial,
        ReduceArmor,
        IncreaseAvoid,
        IncreaseMagicArmor,
        ReduceDamage,
    }

    public enum SpecialPropertyEnum
    {
        None,                            
        HealFriendHp,                    ///每过x秒，为一名受伤程度最重的友方回复y+z点生命值。  Z=当前角色法术强度*P
        HealFriendHpBuff,                ///每过x秒，在y秒内为一位友方回复Z+A点生命值。 A=当前角色法术强度*P
        ExtraDamage,                     ///每过x秒，对敌人造成y+z点伤害。       Z=当前角色法术强度*P 
        ExtraDamageBuff,                 ///每过x秒，在y秒内对敌人造成z+A伤害。   A=当前角色法术强度*P
        ExtraAllEnemyDamage,             ///每过x秒，造成y+z分裂伤害。       Z=当前角色法术强度*P 
        ExtraAllEnemyDamageBuff,         ///每过x秒，在 y秒内造成z+A分裂伤害。  A=当前角色法术强度*P 
        RebornFriend,                    ///每过x秒，复活一位友方并为其回复y+z生命值。  z=当前角色法术强度*P
        BornSmallMonster,                ///每过x秒，召唤一只{怪物名称}加入战场（生命值为x，伤害为 y）。
        IncreaseFriendsArmorBuff,        ///每过x秒，为一位友方增加y%护甲。持续z秒。
        IncreaseFriendsHpBuff,           ///每过x秒，为一位友方增加x%最大生命值。持续z秒。
        DizzyBuff,                       ///每过x秒，眩晕一名敌人 y 秒。 
        AttackThenHealFriendHp,          ///攻击时，有x%概率为一名受伤程度最重的友方回复y+z点生命值。  Z=当前角色法术强度*P
        AttackThenExtraDamage,           ///攻击时，有x%概率对一名敌人造成y+z伤害。  Z=当前角色法术强度*P
        AttackThenIcreaseAFriendDamageBuff, ///攻击时，有x%概率将一位友方所造成的伤害提升y%。持续z秒。
        AttackThenHealSlefHp,            ///攻击时，有x%概率为自己回复y%生命值。
        AttackThenExtraAllEnemyDamage,   ///攻击时，有x%概率造成y%分裂伤害。
        AttackThenDizzy,                 ///攻击时，有x%概率使敌人眩晕y秒
        AttackThenExtraAllEnemyDamageBuff,///攻击时，有x% 概率使敌人流血，在y秒内造成z%伤害 
        AttackThenIncreaseCritialBuff,   ///攻击时，有x%概率提高自身y%的暴击。持续z秒。
        AttackThenIncreaseDamageBuff,    ///攻击时，有x%概率将伤害提高y%。持续z秒。
        AttackThenReduceTargetArmorBuff, ///攻击时，有x%概率使一名敌人的护甲降低y%，持续z秒
        AttackThenIncreaseAvoidBuff,     ///攻击时，有x%概率提高自身y%的闪避。持续z秒。
        AttackThenIncreaseHpBuff,        ///攻击时，有x%概率提高自身y%的最大生命值。持续z秒。
        AttackThenIncreaseSelfArmor,     ///攻击时，有x%概率提高自身y%的护甲。持续z秒。
        AttackThenIncreaseMagicArmor,    ///攻击时，有x%概率使魔法护甲提高y%，持续z秒
        AttackThenReduceEnemyDamage,     ///攻击时，有x%的概率降低敌人伤害的y%。持续z秒
        HurtThenHealHp,                  ///受到伤害时，有x%概率为自己恢复x%点生命值
        HurtThenHealHpBuff,              ///受到伤害时，有x%概率为自己恢复x%点生命值 持续z秒。
        HurtThenIncreaseArmor,           ///受到攻击时，有x%概率在y秒内回复自身y%的生命值。
        HurtThenReduceTargetMagicArmorBuff, ///受到攻击时，有x%概率降低敌人y%的魔法护甲。持续z秒。
        AttackThenIncreaseDamage,       ///每次攻击都提升x%点伤害。转换当前攻击目标时，效果重置
        AttackThenHealHp,                ///攻击命中敌人时，造成x+y伤害，并为友军回复等同于伤害值z%的生命值。  y=法术强度*P
        AttackThenEnemyExtraDamageBuff,  ///攻击时，x%概率使被击中的敌人所受的伤害提高x%。效果持续y秒。
        DeadThenHurtAllEnemy,           ///死亡时，对敌人造成x点范围伤害。
        DeadThenDizzyAllEnemy,          ///死亡时，眩晕范围内敌人x秒
        LowHpBornSmallMonster,          ///生命值低于x%时，每隔y秒召唤两个z生命值、a伤害的骷髅。 
        HurtThenRelectEnemy,            ///受到伤害时， 有x%概率对攻击者造成y%反弹伤害。
        HurtThenGod,                    ///受到伤害时，有X%概率对自己使用干涉，回复Y点生命值。在干涉期间将不会受到攻击，也无法移动或攻击。持续Z秒
        DeadThenHealAnother,            ///死亡时，对一名受伤程度最重的友方回复x%生命值。每场战斗仅可启动 1 次。
        RebornThenIncreaseDamage,         ///以任何形式复活后，伤害提升x%。持续y秒。
        KillLowHpEnemy,                 ///目标生命值少于x%时，y%概率斩杀敌人。
        TargetDeadThenIncreaseDamage,    ///当前攻击目标被击杀时，x秒内造成的伤害提高y%。

    }
}
