using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetResearchMagicSkillRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_ResearchMagicSkill magicSkill = Extensible.GetValue<RS_ResearchMagicSkill>(data, id);
            if (magicSkill.ret != 0)
            {
                DebugHelper.Log("法术升级失败");
                return;
            }
            DebugHelper.Log("一个新的法术开始升级  === " + magicSkill.magicSkill.skillId);
        }
    }
}
