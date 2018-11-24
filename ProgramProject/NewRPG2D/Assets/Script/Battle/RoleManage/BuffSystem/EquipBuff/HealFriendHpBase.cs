using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class HealFriendHpBase : AlwayTriggerBuff
    {

        private float healHp;
        private float healHpMagic;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            healHpMagic = param2;
            healHpMagic = param3;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                RoleBase loseHpRole = FindLoseMaxHpRole();
                loseHpRole.RolePropertyValue.SetHp(-(healHp + MagicValue * healHpMagic));
                return true;
            }
            return false;
        }
    }
}
