using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoomUpdateLevelSend : ISend
    {
        RQ_RoomUpdateLevel roomUpdateLevel;
        public NetRoomUpdateLevelSend()
        {
            roomUpdateLevel = new RQ_RoomUpdateLevel();
        }
        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("提交房间升级+=====" + param[0].ToString() + " 类型: " + param[1].ToString());
            roomUpdateLevel.roomId = (string)param[0]; //房间ID 01 02 04
            roomUpdateLevel.type = (int)param[1];//操作类型 1普通升级 2加速升级 0取消升级
            return roomUpdateLevel;
        }
    }
}
