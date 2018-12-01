using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;


namespace Assets.Script.Net
{
    public class NetCheckCreateNewRoomSend : ISend
    {
        RQ_CheckCreateNewRoom checkCreateNewRoom;
        public NetCheckCreateNewRoomSend()
        {
            checkCreateNewRoom = new RQ_CheckCreateNewRoom();
        }
        public IExtensible Send(params object[] param)
        {
            checkCreateNewRoom.roomId = (int)param[0];
            return checkCreateNewRoom;
        }
    }
}