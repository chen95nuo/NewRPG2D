using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetAddbattleInMagicSkillRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_AddBattleInMagicSkill addBattleInMagicSkill = Extensible.GetValue<RS_AddBattleInMagicSkill>(data, id);

            if (addBattleInMagicSkill.ret != 0)
            {
                DebugHelper.Log("添加法术到战斗中失败=====");
                return;
            }
            DebugHelper.Log("添加到法术中成功 ====== " + addBattleInMagicSkill.battleInUseMagicSkill.Count);
            List<MagicSkillInfo> info = addBattleInMagicSkill.battleInUseMagicSkill;

        }
    }
}
