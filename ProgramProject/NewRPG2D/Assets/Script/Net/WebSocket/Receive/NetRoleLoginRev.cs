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
                GetPlayerData.Instance.SetData(mRoleLogin.userInfo);
                List<ResidentInfo> residentInfo = mRoleLogin.allResident;//所有居民
                HallRoleMgr.instance.GetServerAllRole(mRoleLogin.allResident);
                List<proto.SLGV1.RoomInfo> allRoom = mRoleLogin.allRoom;//所有房间
                BuildingManager.instance.GetAllBuildingData(mRoleLogin.allRoom);
                List<ProduceRoom> allProRoom = mRoleLogin.allProRoom;//生产类房间
                BuildingManager.instance.GetAllProduceRoom(mRoleLogin.allProRoom);
                List<StoreRoom> allStoreRoom = mRoleLogin.allStoreRoom;//储存类房间
                BuildingManager.instance.GetAllStoreRoom(mRoleLogin.allStoreRoom);
                List<ResidentRoom> allResidentRoom = mRoleLogin.allResidentRoom;//居民室
                BuildingManager.instance.GetAllResidentRoom(mRoleLogin.allResidentRoom);
                ProudctEquipInfo proEquip = mRoleLogin.proEquip;//后期需要更改
                AllMagicSkillInfo allMagicSkill = mRoleLogin.allMagicSkill;//魔法生产和投入战斗 以及魔法实验室法术最高等级
                BuildingManager.instance.GetAllMagicSkillInfo(mRoleLogin.allMagicSkill);
                Box boxs = mRoleLogin.boxs;//背包
                DebugHelper.Log("获取到账号数据" + characterInfo.userId);

                GameHelper.instance.ServerInfo = true;//让进度条继续
            }
            else DebugHelper.LogError("数据获取失败重新登陆 ret=" + ret);
        }
    }
}
