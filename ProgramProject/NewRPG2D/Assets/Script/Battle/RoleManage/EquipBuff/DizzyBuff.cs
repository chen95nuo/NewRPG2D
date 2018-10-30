/// <summary>
/// 攻击时，有x%概率使敌人眩晕y秒
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class DizzyBuff : RoleEquipSpecialBuff
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

        private bool canTrigger;
        private float addTime;

        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
            triggerChange = param1;
            duration = param2;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            if (canTrigger)
            {
                addTime += deltaTime;
                if (addTime < duration)
                {
                    for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
                    {
                        RoleBase role = GameRoleMgr.instance.RolesList[i];
                        if (role.TeamId != currentRole.TeamId)
                        {
                            role.IsCanControl = false;
                        }
                    }
                }
                else
                {
                    canTrigger = false;
                    for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
                    {
                        RoleBase role = GameRoleMgr.instance.RolesList[i];
                        if (role.TeamId != currentRole.TeamId)
                        {
                            role.IsCanControl = true;
                        }
                    }
                }
            }
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 100f) < triggerChange)
            {
                canTrigger = true;
                addTime = 0;
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
