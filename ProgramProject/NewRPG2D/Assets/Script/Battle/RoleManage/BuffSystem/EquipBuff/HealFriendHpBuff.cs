using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class HealFriendHpBuff : AlwayTriggerBuff
    {
        private float buffDuration;
        private float constHealHp;
        private float magicAddHeal;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            buffDuration = param2;
            constHealHp = param3;
            magicAddHeal = param4;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
               currentRole.BuffMoment.AddBuff(BuffTypeEnum.HealHp, buffDuration, constHealHp + magicAddHeal * MagicValue);
                return true;
            }

            return false;
        }
    }
}
