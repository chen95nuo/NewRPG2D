using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetConfirmKidAdultRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_ConfirmKidAdult confirmKidAdult = Extensible.GetValue<RS_ConfirmKidAdult>(data, id);
            if (confirmKidAdult.ret != 0)
            {
                DebugHelper.Log("小孩成人失败=======");
                return;
            }
            DebugHelper.Log("小孩成人成功=======");
        }
    }
}