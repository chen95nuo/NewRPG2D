using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class HealFriendHp : HealFriendHpBase
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Always;
            }
        }

        private float duration;
        private HurtInfo mHurtInfo = default(HurtInfo);
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param1;
        }

        private float intervalTime = 0;

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            intervalTime += deltaTime;
            if (intervalTime > duration)
            {
                intervalTime = 0;
                Trigger(TirggerType, ref mHurtInfo);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
