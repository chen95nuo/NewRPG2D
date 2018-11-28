using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto.SLGV1_0;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetRoleloginSend : ISend
    {
        RQ_RoleLogin mRoleLogin;

        public NetRoleloginSend()
        {
            mRoleLogin = new RQ_RoleLogin();
        }

        public IExtensible Send(params object[] param)
        {
            return mRoleLogin;
        }
    }
}
