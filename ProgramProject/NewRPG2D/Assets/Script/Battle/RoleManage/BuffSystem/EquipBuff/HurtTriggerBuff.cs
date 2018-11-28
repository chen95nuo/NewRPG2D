
using UnityEngine;

namespace Assets.Script.Battle
{
    public class HurtTriggerBuff : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Hurt;
            }
        }
        protected float triggerChange;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            triggerChange = param1;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info) && Random.Range(0, 100f) < triggerChange)
            {
                return true;
            }
            return false;
        }
    }
}
