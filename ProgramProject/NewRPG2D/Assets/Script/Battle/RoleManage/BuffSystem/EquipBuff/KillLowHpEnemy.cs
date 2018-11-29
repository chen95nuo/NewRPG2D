/// <summary>
/// 以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
/// </summary>


namespace Assets.Script.Battle
{
    public class KillLowHpEnemy : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }

        private float triggerChance;
        private float hpPercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            hpPercent = param1 * 0.01f;
            triggerChance = param2 * 0.01f;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && RandomValue < triggerChance)
            {
                if (info.TargeRole.RolePropertyValue.RoleHp < info.TargeRole.RolePropertyValue.MaxHp * hpPercent)
                {
                    info.HurtValue = info.TargeRole.RolePropertyValue.RoleHp;
                    info.TargeRole.RolePropertyValue.SetHp(info.HurtValue, info.AttackRole);
                    info.TargeRole.IsCanInterrput = true;
                    info.AttackRole.RoleWeapon.TriggerBuff(TirggerTypeEnum.TargetDeath, ref info);
                    info.TargeRole.SetRoleActionState(ActorStateEnum.Death);

                    return true;
                }
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
