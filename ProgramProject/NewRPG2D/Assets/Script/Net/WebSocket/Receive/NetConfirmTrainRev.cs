using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetConfirmTrainRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_ConfirmTrain confirmTrain = Extensible.GetValue<RS_ConfirmTrain>(data, id);
            if (confirmTrain.ret != 0)
            {
                DebugHelper.Log("角色升级失败========");
                return;
            }
            DebugHelper.Log("角色升级成功=======");
        }
    }
}
