using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomSplitSend : ISend
    {
        RQ_RoomSplit roomSplit;
        public NetRoomSplitSend()
        {
            roomSplit = new RQ_RoomSplit();
        }
        public IExtensible Send(params object[] param)
        {
            roomSplit.roomId = (int)param[0];
            DebugHelper.Log("发起了房屋拆分======" + roomSplit.roomId);
            return roomSplit;
        }
    }
}
