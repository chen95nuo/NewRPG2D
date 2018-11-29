using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetConfirmRearKidRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_ConfirmRearKid confirmRearKid = Extensible.GetValue<RS_ConfirmRearKid>(data, id);
            if (confirmRearKid.ret != 0)
            {
                DebugHelper.Log("生小孩失败======");
                return;
            }
            DebugHelper.Log("小孩生出来啦========");
        }
    }
}
