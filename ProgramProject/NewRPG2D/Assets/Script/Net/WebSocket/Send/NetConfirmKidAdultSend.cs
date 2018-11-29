using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetConfirmKidAdultSend : ISend
    {
        RQ_ConfirmKidAdult confirmKidAdult;
        public NetConfirmKidAdultSend()
        {
            confirmKidAdult = new RQ_ConfirmKidAdult();
        }
        public IExtensible Send(params object[] param)
        {
            confirmKidAdult.id = (int)param[0];
            DebugHelper.Log("发起小孩成人图标=======");
            return confirmKidAdult;
        }
    }
}
