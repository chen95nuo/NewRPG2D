using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class IncreaseFriendsHpBuff : AlwayTriggerBuff
    {
        private float buffDuration;
        private float maxHpPrecent;


        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            maxHpPrecent = param2 * 0.01f;
            buffDuration = param3;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                currentRole.BuffMoment.AddBuff(BuffTypeEnum.ChangeMaxHp, buffDuration, maxHpPrecent);
                return true;
            }

            return false;
        }

    }
}
