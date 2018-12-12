using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
  public  class NetPickUpProRoomResourceRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_PickUpProRoomResource pickUpProRoomResource = Extensible.GetValue<RS_PickUpProRoomResource>(data, id);
            DebugHelper.Log("ProRoomResourece 收获资源");
            BuildingManager.instance.GetProduceResource(pickUpProRoomResource);
        }
    }
}
