using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetDragIntoRoomSend : ISend
    {
        RQ_DragIntoRoom dragIntoRoom;
        public NetDragIntoRoomSend()
        {
            dragIntoRoom = new RQ_DragIntoRoom();
        }
        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("发起一个居民移动到房间========");
            dragIntoRoom.id = (int)param[0];
            dragIntoRoom.roomId = (string)param[1];
            return dragIntoRoom;
        }
    }
}
