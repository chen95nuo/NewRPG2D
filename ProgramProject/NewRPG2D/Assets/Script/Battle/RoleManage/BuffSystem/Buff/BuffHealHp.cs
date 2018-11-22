using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class BuffHealHp : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }

        private float healHp;

        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            healHp = (float)param[1] / (int)buffDuration;
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
        }

        public override bool Update(float deltaTime)
        {
            if (base.Update(deltaTime) == false)
            {
                return false;
            }
            CurrentRole.RolePropertyValue.SetHp(-healHp);
            return true;
        }

    }
}
