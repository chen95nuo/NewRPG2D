using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto.SLGV1;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetConfirmRearKidSend : ISend
    {
        RQ_ConfirmRearKid confirmRearKid;
        public NetConfirmRearKidSend()
        {
            confirmRearKid = new RQ_ConfirmRearKid();
        }
        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("发起确认养育小孩=======");
            confirmRearKid.id = (int)param[0];
            return confirmRearKid;
        }
    }
}
