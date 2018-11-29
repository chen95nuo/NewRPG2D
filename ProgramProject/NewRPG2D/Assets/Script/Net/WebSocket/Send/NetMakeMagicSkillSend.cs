using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto.SLGV1;
using ProtoBuf;

namespace Assets.Script.Net
{
    public class NetMakeMagicSkillSend : ISend
    {
        RQ_MakeMagicSkill makeMagicSkill;
        public NetMakeMagicSkillSend()
        {
            makeMagicSkill = new RQ_MakeMagicSkill();
        }
        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("发送了一个新的法术制作信息=====" + param[1].ToString());
            makeMagicSkill.type = (int)param[0];//0普通生产，1 加速生产 2取消生产
            makeMagicSkill.skillId = (int)param[1];
            return makeMagicSkill;
        }
    }
}
