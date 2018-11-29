using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetPickupProCompleteEquiSend : ISend
    {
        RQ_PickupProCompleteEquip completeEquip;

        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("一个房间完成了生产==== " + (string)param[0]);
            completeEquip.roomId = (string)param[0];
            return completeEquip;
        }
    }
}
