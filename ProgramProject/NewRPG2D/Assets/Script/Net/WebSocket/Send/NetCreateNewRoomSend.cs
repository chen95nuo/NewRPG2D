using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetCreateNewRoomSend : ISend
    {
        RQ_CreateNewRoom mCreateNewRoom;
        public NetCreateNewRoomSend()
        {
            mCreateNewRoom = new RQ_CreateNewRoom();
        }

        public IExtensible Send(params object[] param)
        {
            mCreateNewRoom.roomType = (int)param[0];
            List<int> point = (List<int>)param[1];
            for (int i = 0; i < point.Count; i++)
            {
                mCreateNewRoom.xFloorOriginOffset.Add(point[i]);
            }
            return mCreateNewRoom;
        }
    }
}
