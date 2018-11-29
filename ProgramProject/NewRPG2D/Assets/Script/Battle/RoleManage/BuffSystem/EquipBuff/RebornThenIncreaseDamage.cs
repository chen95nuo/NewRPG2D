/// <summary>
/// 以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
/// </summary>


namespace Assets.Script.Battle
{
    public class RebornThenIncreaseDamage : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Reborn;
            }
        }

        private float duration;
        private float increaseDamage;
        private bool bTrigger;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param1;
            increaseDamage = param2 * role.RolePropertyValue.Damage;
        }
        
        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            bTrigger = base.Trigger(tirggerType, ref info);
            if (bTrigger)
            {
                currentRole.BuffMoment.AddBuff(BuffTypeEnum.IncreaseDamage, duration, increaseDamage);
            }
            return bTrigger;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
