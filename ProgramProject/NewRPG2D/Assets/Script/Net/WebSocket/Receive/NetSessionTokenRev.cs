using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetSessionTokenRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_SessionToken sessionToken = Extensible.GetValue<A_SessionToken>(data, id);
            DebugHelper.Log("断线重连");
        }
    }
}
