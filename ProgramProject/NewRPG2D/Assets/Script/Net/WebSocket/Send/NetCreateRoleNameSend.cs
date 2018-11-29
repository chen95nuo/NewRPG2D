using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetCreateRoleNameSend : ISend
    {
        RQ_CreateRoleName mCreateRoleName;
        public NetCreateRoleNameSend()
        {
            mCreateRoleName = new RQ_CreateRoleName();
        }
        public IExtensible Send(params object[] param)
        {
            mCreateRoleName.roleName = (string)param[0];
            return mCreateRoleName;
        }
    }
}
