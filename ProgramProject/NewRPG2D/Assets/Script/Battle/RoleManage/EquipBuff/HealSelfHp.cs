/// <summary>
///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class HealSelfHp : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }

        private float triggerChange;
        private float healHpPercent;


        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
            triggerChange = param1 * 0.01f;
            healHpPercent = param2 * 0.01f;
        }

        private float intervalTime = 0;

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 1f) < triggerChange)
            {
                currentRole.RolePropertyValue.SetHp(-currentRole.RolePropertyValue.MaxHp * healHpPercent);
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
