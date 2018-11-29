

namespace Assets.Script.Battle
{
    public class AlwayTriggerBuff : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Always;
            }
        }

        protected float duration;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param1;
        }

        private float intervalTime = 0;

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            intervalTime += deltaTime;
            if (intervalTime > duration * currentRole.RolePropertyValue.CDTimePercent)
            {
                if(Trigger(TirggerType, ref mHurtInfo))
                {
                    intervalTime = 0;
                    //currentRole.BuffMoment.RemoveBuff(BuffTypeEnum.IncreaseCDTime);
                    //currentRole.BuffMoment.RemoveBuff(BuffTypeEnum.ReduceCDTime);
                }
            }
        }
    }
}
