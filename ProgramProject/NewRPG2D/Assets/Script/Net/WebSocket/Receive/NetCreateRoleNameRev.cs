using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto.SLGV1_0;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetCreateRoleNameRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_CreateRoleName mCreateRoleName = Extensible.GetValue<RS_CreateRoleName>(data, id);
            if (mCreateRoleName.ret != 0)
            {
                DebugHelper.Log("改名失败");
                return;
            }
            CharacterInfo userInfo = mCreateRoleName.userInfo;//新的角色信息
        }
    }
}
