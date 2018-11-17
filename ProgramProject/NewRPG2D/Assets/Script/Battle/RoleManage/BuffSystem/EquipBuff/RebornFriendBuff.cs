/// <summary>
/// 以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
/// </summary>

namespace Assets.Script.Battle
{
    public class RebornFriendBuff : RoleEquipSpecialBuff
    {
        private float addTime;
        private float CDTime;
        private float healHp;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            addTime = CDTime = param3;
            healHp = param1 + role.RolePropertyValue.MagicAttack * param2;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            addTime += deltaTime;
            if (addTime >= CDTime)
            {
                for (int i = 0; i < GameRoleMgr.instance.RolesList.Count; i++)
                {
                    RoleBase role = GameRoleMgr.instance.RolesList[i];
                    if (role.TeamId == currentRole.TeamId && role.IsDead)
                    {
                        addTime = 0;
                        role.Reborn();
                        role.RolePropertyValue.SetHp(healHp);
                    }
                }
            }
        }
        
        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            return (base.Trigger(tirggerType, ref info));
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
