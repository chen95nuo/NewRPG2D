using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class RoleEquipSpecialBuff
    {
        public virtual TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Always;
            }
        }

        protected RoleBase currentRole;

        public virtual void Init(RoleBase role, float param1, float param2, float param3)
        {
            currentRole = role;
        }

        public virtual bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (tirggerType == TirggerType)
            {
                return true;
            }
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
