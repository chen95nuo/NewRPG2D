/// <summary>
/// 以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
/// </summary>

using System.Runtime.Hosting;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class HurtAllEnemy : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }

        private float triggerChange, damagePercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            triggerChange = param1;
            damagePercent = param2;
        }


        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 100f) < triggerChange)
            {
                for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
                {
                    RoleBase role = GameRoleMgr.instance.RolesList[i];
                    if (role.TeamId != currentRole.TeamId)
                    {
                        HurtInfo hurtInfo = new HurtInfo();
                        hurtInfo.TargeRole = role;
                        hurtInfo.AttackRole = currentRole;
                        hurtInfo.HurtType = currentRole.RolePropertyValue.HurtType;
                        hurtInfo.HurtValue = currentRole.RolePropertyValue.Damage * damagePercent * 0.01f;

                        currentRole.RoleDamageMoment.HurtDamage(ref hurtInfo);
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
