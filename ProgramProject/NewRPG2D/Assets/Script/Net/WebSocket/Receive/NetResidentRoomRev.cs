using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetResidentRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_ResidentRoom residentRoom = Extensible.GetValue<A_ResidentRoom>(data, id);
            DebugHelper.Log("居民室居民 == " + residentRoom.residentRoom);
        }
    }
}
