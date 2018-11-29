/// <summary>
///当前攻击目标被击杀时，x秒内造成的伤害提高y%
/// </summary>

namespace Assets.Script.Battle
{
    public class TargetDeadTrigger : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.TargetDeath;
            }
        }
    }
}
