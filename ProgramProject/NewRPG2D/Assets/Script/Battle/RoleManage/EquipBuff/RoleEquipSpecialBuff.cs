using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class RoleEquipSpecialBuff
    {
        public virtual void Init(RoleBase role, float param1, float param2, float param3)
        {

        }

        public virtual bool Trigger()
        {
            return false;
        }

        public virtual void UpdateLogic(float deltaTime)
        {
        }

        public virtual void Dispose()
        {
        }

    }
}
