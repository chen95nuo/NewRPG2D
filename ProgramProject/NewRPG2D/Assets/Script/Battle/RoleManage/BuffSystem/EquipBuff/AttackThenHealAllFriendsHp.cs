using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class AttackThenHealAllFriendsHp : AttackTriggerBuff
    {
        private float damage;
        private float damageAddtiveMagic;
        private float healHpPercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            triggerChange = 100;
            damage = param1;
            damageAddtiveMagic = param2;
            healHpPercent = param3;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if ( base.Trigger(triggerType, ref info))
            {
                float realDamage = damage + damageAddtiveMagic*MagicValue;
                info.HurtValue = realDamage + info.HurtValue;
                float lastHp = Target.RolePropertyValue.RoleHp;
                Target.RoleDamageMoment.HurtDamage(ref info);
                float loseHp = lastHp - Target.RolePropertyValue.RoleHp;
                for (int i = 0; i < FriendsRoleList.Count; i++)
                {
                    FriendsRoleList[i].RolePropertyValue.SetHp(-(loseHp*healHpPercent));
                }

                return true;
            }
            return false;
        }
    }
}
