using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetAddbattleInMagicSkillSend : ISend
    {
        RQ_AddBattleInMagicSkill addMagicInBattle;
        public NetAddbattleInMagicSkillSend()
        {
            addMagicInBattle = new RQ_AddBattleInMagicSkill();
        }
        public IExtensible Send(params object[] param)
        {
            DebugHelper.Log("将一个法术添加到战斗中去====" + (int)param[1]);
            addMagicInBattle.positions = (int)param[0];
            addMagicInBattle.magicId = (int)param[1];
            return addMagicInBattle;
        }
    }
}
