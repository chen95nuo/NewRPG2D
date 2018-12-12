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
            mCreateNewRoom.roomId = (int)param[0];
            List<int> point = (List<int>)param[1];
            mCreateNewRoom.xFloorOriginOffset.Clear();
            for (int i = 0; i < point.Count; i++)
            {
                mCreateNewRoom.xFloorOriginOffset.Add(point[i]);
            }
            foreach (var item in mCreateNewRoom.xFloorOriginOffset)
            {
                DebugHelper.Log("参数为: " + item);
            }
            return mCreateNewRoom;
        }
    }
}
