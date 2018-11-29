using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetSaveAllRoomSend : ISend
    {
        RQ_SaveAllRoom saveAllRoom;
        public NetSaveAllRoomSend()
        {
            saveAllRoom = new RQ_SaveAllRoom();
        }
        public IExtensible Send(params object[] param)
        {
            for (int i = 0; i < param.Length; i++)
            {
                saveAllRoom.room.Add((proto.SLGV1.RoomInfo)param[i]);
            }
            return saveAllRoom;
        }
    }
}
