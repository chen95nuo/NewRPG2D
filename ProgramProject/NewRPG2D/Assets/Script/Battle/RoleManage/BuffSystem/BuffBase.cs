using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Vuforia;

namespace Assets.Script.Battle
{

    public class BuffBase
    {
        public virtual BuffEffectTypeEnum BuffType
        {
            get
            {
                return BuffEffectTypeEnum.None;
            }
        }

        private BuffStateEnum buffState;

        private float buffDuration, currentTime;


        public virtual void AddBuff(float duration, params object[] param)
        {
            buffDuration = duration;
            currentTime = 0;
            buffState = BuffStateEnum.Running;
        }

        public virtual void RmoveBuff()
        {
            buffDuration = -1;
            currentTime = 0;
            buffState = BuffStateEnum.Finish;
        }

        public virtual bool Update(float deltaTime)
        {
            if (buffState == BuffStateEnum.Finish)
            {
                return false;
            }

            currentTime += deltaTime;
            if (currentTime >= buffDuration)
            {
                buffState = BuffStateEnum.Finish;
            }

            return true;
        }

    }
}
