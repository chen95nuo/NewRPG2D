/// <summary>
/// 攻击时，有x%概率使敌人眩晕y秒
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class DizzyBuff : AlwayTriggerBuff
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
                RoleBase role = FindRandomTarget(EnemysRoleList);
                if (role == null)
                {
                    return false;
                }
                role.BuffMoment.AddBuff(BuffTypeEnum.Dizzy, duration);

                return true;
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
