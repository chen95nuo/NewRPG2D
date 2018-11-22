using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class ExtraAllEnemyDamage : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Always;
            }
        }

        private float duration;
        private float constDamage;
        private float magicAddtiveDamge;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param1;
            constDamage = param2;
            magicAddtiveDamge = param3;
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
                float damage = constDamage + magicAddtiveDamge*MagicValue;
                HurtEnemy(damage);
                List<RoleBase> enemys = FindEnemys(Target);
                if (enemys != null)
                {
                    for (int i = 0; i < enemys.Count; i++)
                    {
                        HurtEnemy(enemys[i], damage);
                    }
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
