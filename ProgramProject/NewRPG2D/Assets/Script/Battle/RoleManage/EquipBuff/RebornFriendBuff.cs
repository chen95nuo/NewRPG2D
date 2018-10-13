using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Battle
{
    public class RebornFriendBuff : RoleEquipSpecialBuff
    {
        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }

        public override bool Trigger()
        {
            return base.Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
