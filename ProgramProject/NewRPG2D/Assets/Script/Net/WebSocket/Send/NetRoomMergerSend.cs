using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{

    public class NetRoomMergerSend : ISend
    {
        RQ_RoomMerger roomMerger;
        public NetRoomMergerSend()
        {
            roomMerger = new RQ_RoomMerger();
        }
        public IExtensible Send(params object[] param)
        {
            for (int i = 0; i < param.Length; i++)
            {
                roomMerger.roomIds.Add((int)param[i]);
            }
            DebugHelper.Log("发起房间合并========");
            return roomMerger;
        }
    }
}
