using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetSaveAllRoomRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_SaveAllRoom saveAllRoom = Extensible.GetValue<RS_SaveAllRoom>(data, id);

            if (saveAllRoom.ret != 0)
            {
                DebugHelper.Log("编辑模式保存失败=======");
                return;
            }
            DebugHelper.Log("编辑模式保存成功=======");
        }
    }
}
