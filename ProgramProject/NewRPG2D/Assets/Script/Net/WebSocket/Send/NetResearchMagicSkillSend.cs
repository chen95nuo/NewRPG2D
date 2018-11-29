using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetResearchMagicSkillSend : ISend
    {
        RQ_ResearchMagicSkill mReserchMagicSkill;

        public NetResearchMagicSkillSend()
        {
            mReserchMagicSkill = new RQ_ResearchMagicSkill();
        }

        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("有一个开始升级或者加速====" + param[0].ToString() + " " + param[1].ToString());
            mReserchMagicSkill.type = (int)param[0];//0 普通 1钻石加速
            mReserchMagicSkill.skillId = (int)param[1];//当前等级法术ID
            return mReserchMagicSkill;
        }
    }
}
