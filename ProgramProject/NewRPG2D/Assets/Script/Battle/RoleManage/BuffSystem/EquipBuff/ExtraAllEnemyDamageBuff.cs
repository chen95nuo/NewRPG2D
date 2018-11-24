using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class ExtraAllEnemyDamageBuff : AlwayTriggerBuff
    {
        private float buffDuration;
        private float constDamage;
        private float magicAddtiveDamge;
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param1;
            buffDuration = param2;
            constDamage = param3;
            magicAddtiveDamge = param4;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                float damage = constDamage + magicAddtiveDamge*MagicValue;
                HurtAllEnemyBuff(damage, buffDuration);
                return true;
            }

            return false;
        }
    }
}
