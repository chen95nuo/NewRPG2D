using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetStoreRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_StoreRoom storeRoom = Extensible.GetValue<A_StoreRoom>(data, id);
            DebugHelper.Log("储存类房间容量 ==" + storeRoom);
        }
    }
}
