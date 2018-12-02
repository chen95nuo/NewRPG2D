using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomStateSend : ISend
    {
        Q_RoomState roomState;

        public NetRoomStateSend()
        {
            roomState = new Q_RoomState();
        }

        public IExtensible Send(params object[] param)
        {
            roomState.roomId = (string)param[0];
            return roomState;
        }
    }
}
