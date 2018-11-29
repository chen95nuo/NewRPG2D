using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomSplitRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_RoomSplit roomSplit = Extensible.GetValue<RS_RoomSplit>(data, id);
            if (roomSplit.ret != 0)
            {
                DebugHelper.Log("房间拆分失败=======");
                return;
            }
            DebugHelper.Log("房间拆分成功=======");
        }
    }
}
