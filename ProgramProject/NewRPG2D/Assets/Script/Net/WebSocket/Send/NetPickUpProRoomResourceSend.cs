using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetPickUpProRoomResourceSend : ISend
    {

        RQ_PickUpProRoomResource pickUpProRoomResource;

        public NetPickUpProRoomResourceSend()
        {
            pickUpProRoomResource = new RQ_PickUpProRoomResource();
        }
        public IExtensible Send(params object[] param)
        {
            pickUpProRoomResource.roomId = (string)param[0];
            return pickUpProRoomResource;
        }
    }
}
