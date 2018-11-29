using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetMakeMagicSkillRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_MakeMagicSkill rsMakeSkill = Extensible.GetValue<RS_MakeMagicSkill>(data, id);
            if (rsMakeSkill.ret == 0)
            {
                DebugHelper.Log("无法制造一个新的法术====");
                return;
            }
            //获取所有法术
            DebugHelper.Log("开始制作一个新的法术====" + rsMakeSkill.magicSkillings.Count);
        }
    }
}
