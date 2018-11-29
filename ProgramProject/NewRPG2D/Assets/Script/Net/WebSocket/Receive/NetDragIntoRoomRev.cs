using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetDragIntoRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_DragIntoRoom dragIntoRoom = Extensible.GetValue<RS_DragIntoRoom>(data, id);
            if (dragIntoRoom.ret != 0)
            {
                DebugHelper.Log("角色加入房间失败=========");
                return;
            }
            DebugHelper.Log("角色加入房间成功" + dragIntoRoom.residentInfos.Count);
        }
    }
}
