using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class HealBuff : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }

        public override void AddBuff(params object[] param)
        {
            base.AddBuff(param);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
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
