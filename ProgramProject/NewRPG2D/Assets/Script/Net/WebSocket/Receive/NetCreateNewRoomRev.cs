using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
   public class NetCreateNewRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_CreateNewRoom mCreateNewRoom = Extensible.GetValue<RS_CreateNewRoom>(data, id);
            int ret = mCreateNewRoom.ret;

            if (ret != 0)
            {
                DebugHelper.Log("新房间获取失败");
                return;
            }
            proto.SLGV1.RoomInfo baseRoomInfo = mCreateNewRoom.baseRoomInfo;//新建的房间基本信息
        }
    }
}
