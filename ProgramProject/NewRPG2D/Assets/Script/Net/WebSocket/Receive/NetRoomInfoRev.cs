using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomInfoRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_RoomInfo roomInfo = Extensible.GetValue<A_RoomInfo>(data, id);
            DebugHelper.Log("获取到房间升级完成消息 NetRoomInfoRev");
        }
    }
}
