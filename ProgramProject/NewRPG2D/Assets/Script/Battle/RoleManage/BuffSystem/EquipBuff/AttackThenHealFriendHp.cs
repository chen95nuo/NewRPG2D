using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class AttackThenHealFriendHp : HealFriendHpBase
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }

        private float trigerChange;
        private HurtInfo mHurtInfo = default(HurtInfo);
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            trigerChange = param1 * 0.01f;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (RandomValue < trigerChange && base.Trigger(tirggerType, ref info))
            {
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
