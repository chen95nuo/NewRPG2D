using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class ExtraDamageBuff : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Always;
            }
        }

        private float duration;
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

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {

                if (Target != null)
                {
                    Target.BuffMoment.AddBuff(BuffTypeEnum.ExtraDamage, buffDuration, constDamage + magicAddtiveDamge * MagicValue);
                }
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
