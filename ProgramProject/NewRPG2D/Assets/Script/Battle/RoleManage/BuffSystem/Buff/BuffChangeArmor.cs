using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class BuffChangeArmor : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }

        private float armorValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            armorValue = (float) param[1] * currentRole.RolePropertyValue.PhysicArmor;
            currentRole.RolePropertyValue.SetPhysicArmor(armorValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetPhysicArmor(-armorValue);
        }

        public override bool Update(float deltaTime)
        {
            if(base.Update(deltaTime) == false)
            {
                return false;
            }

            return true;
        }
        
    }
}
