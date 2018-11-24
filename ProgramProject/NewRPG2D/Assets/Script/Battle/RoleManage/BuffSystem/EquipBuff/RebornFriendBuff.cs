/// <summary>
/// 以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
/// </summary>

namespace Assets.Script.Battle
{
    public class RebornFriendBuff : AlwayTriggerBuff
    {
        
        private float constHealHp;
        private float magicAddtive;
        private float healHp;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            constHealHp = param2;
            magicAddtive = param3;
        }
        
        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if(base.Trigger(triggerType, ref info))
            {
                return RebornRole(constHealHp + magicAddtive * MagicValue);
            }

            return false;

        }
    }
}
