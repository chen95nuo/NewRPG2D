/// <summary>
///攻击时，有x%概率使一名敌人的护甲降低y%，持续z秒
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class ReduceTargetArmor : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }

        private float triggerChange;
        private float duration;
        private float reduceArmorPercent;

        private bool canTrigger;
        private Dictionary<RoleBase, float> targetArmor = new Dictionary<RoleBase, float>();
        private Dictionary<RoleBase, float> armorTime = new Dictionary<RoleBase, float>();
        // private int firstTargetId;

        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
            triggerChange = param1;
            reduceArmorPercent = param2;
            duration = param3;
            targetArmor.Clear();
            armorTime.Clear();
        }

        private float intervalTime = 0;

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            if (canTrigger)
            {
                foreach (var addTime in armorTime)
                {
                    float tempTime = addTime.Value;
                    tempTime += deltaTime;
                    if (tempTime > duration)
                    {
                        addTime.Key.RolePropertyValue.SetPhysicArmor(-targetArmor[addTime.Key]);
                    }
                }
            }
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 1f) < triggerChange)
            {
                canTrigger = true;
                armorTime[info.TargeRole] = 0;
                targetArmor[info.TargeRole] = info.TargeRole.RolePropertyValue.PhysicArmor*reduceArmorPercent;
                info.TargeRole.RolePropertyValue.SetPhysicArmor(targetArmor[info.TargeRole]);
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
