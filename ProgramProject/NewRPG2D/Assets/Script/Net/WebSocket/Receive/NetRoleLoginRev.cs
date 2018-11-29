using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using proto.SLGV1;

namespace Assets.Script.Net
{
    public class NetRoleLoginRev : IReceive
    {
        public void Receive(IExtensible data, int id)
        {
            RS_RoleLogin mRoleLogin = Extensible.GetValue<RS_RoleLogin>(data, id);
            int ret = mRoleLogin.ret; //0成功 -1失败
            if (ret == 0)
            {
                CharacterInfo characterInfo = mRoleLogin.userInfo;//账号信息
                List<proto.SLGV1.RoomInfo> allRoom = mRoleLogin.allRoom;//所有房间
                List<ResidentInfo> residentInfo = mRoleLogin.allResident;//所有居民
                List<ProduceRoom> allProRoom = mRoleLogin.allProRoom;//生产类房间
                List<StoreRoom> allStoreRoom = mRoleLogin.allStoreRoom;//储存类房间
                List<ResidentRoom> allResidentRoom = mRoleLogin.allResidentRoom;//居民室
                ProudctEquipInfo proEquip = mRoleLogin.proEquip;//后期需要更改
                AllMagicSkillInfo allMagicSkill = mRoleLogin.allMagicSkill;//魔法生产和投入战斗 以及魔法实验室法术最高等级
                Box boxs = mRoleLogin.boxs;//背包
            }
            else DebugHelper.LogError("数据获取失败重新登陆");
        }
    }
}
