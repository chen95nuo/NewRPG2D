using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetPickupProCompleteEquiRev : IReceive
    {

        public void Receive(IExtensible data, int id)
        {
            RQ_PickupProCompleteEquip pickupProCompleteEquip = Extensible.GetValue<RQ_PickupProCompleteEquip>(data, id);
            string roomId = pickupProCompleteEquip.roomId;
            DebugHelper.Log("装备完成制作的房间ID ===" + roomId);
        }
    }
}
