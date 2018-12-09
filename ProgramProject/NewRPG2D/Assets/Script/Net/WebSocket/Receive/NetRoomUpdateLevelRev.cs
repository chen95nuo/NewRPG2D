using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomUpdateLevelRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_RoomUpdateLevel roomUpdateLevel = Extensible.GetValue<RS_RoomUpdateLevel>(data, id);

            if (roomUpdateLevel.ret != 0)
            {
                DebugHelper.Log("房间升级验证失败+=========");
                return;
            }
            BuildingManager.instance.RoomUpdateLevelUp(roomUpdateLevel);
            DebugHelper.Log("房间开始升级扣除材料 ====== " + roomUpdateLevel.roomInfo.roomId);
        }
    }
}
