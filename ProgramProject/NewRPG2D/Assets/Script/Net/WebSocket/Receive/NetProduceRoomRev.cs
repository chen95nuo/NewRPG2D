using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetProduceRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_ProduceRoom produceRoom = Extensible.GetValue<A_ProduceRoom>(data, id);
            DebugHelper.Log("生产类房间库存 == " + produceRoom.proRoom);
        }
    }
}
