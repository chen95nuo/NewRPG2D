using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1_0;

namespace Assets.Script.Net
{
    class NetCreateNewRoomSend : ISend
    {
        RQ_createNewRoom mCreateNewRoom;
        public NetCreateNewRoomSend()
        {
            mCreateNewRoom = new RQ_createNewRoom();
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
