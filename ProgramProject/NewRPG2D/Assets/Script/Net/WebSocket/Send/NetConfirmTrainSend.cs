using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetConfirmTrainSend : ISend
    {
        RQ_ConfirmTrain confirmTrain;
        public NetConfirmTrainSend()
        {
            confirmTrain = new RQ_ConfirmTrain();
        }
        public IExtensible Send(params object[] param)
        {
            confirmTrain.id = (int)param[0];
            DebugHelper.Log("发起训练完成======");
            return confirmTrain;
        }
    }
}
