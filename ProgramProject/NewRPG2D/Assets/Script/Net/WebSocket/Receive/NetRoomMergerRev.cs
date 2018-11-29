using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomMergerRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_RoomMerger roomMerger = Extensible.GetValue<RS_RoomMerger>(data, id);
            if (roomMerger.ret != 0)
            {
                DebugHelper.Log("合并失败========");
                return;
            }
            DebugHelper.Log("房间合并成功=======" + roomMerger.newroomInfo.roomId);
        }
    }
}
