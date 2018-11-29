using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetQiPaoRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            A_QiPao qiPao = Extensible.GetValue<A_QiPao>(data, id);
            DebugHelper.Log("添加一个新的气泡" + qiPao.id + " 在:" + qiPao.jumingId);
        }
    }
}
