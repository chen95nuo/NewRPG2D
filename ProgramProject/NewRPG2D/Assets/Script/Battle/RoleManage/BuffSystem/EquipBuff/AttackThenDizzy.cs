/// <summary>
/// 攻击时，有x%概率使敌人眩晕y秒
/// </summary>
namespace Assets.Script.Battle
{
    public class AttackThenDizzy : AttackTriggerBuff
    {
        private float duration;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param2;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                DizzyAllEnemy(duration);
                return true;
            }
            return false;
        }
    }
}
