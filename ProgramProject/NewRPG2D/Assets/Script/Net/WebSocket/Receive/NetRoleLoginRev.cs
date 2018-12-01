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
                GetPlayerData.Instance.SetData(mRoleLogin.userInfo);//获取账号信息
                HallRoleMgr.instance.GetServerAllRole(mRoleLogin.allResident);//获取所有居民
                BuildingManager.instance.GetAllBuildingData(mRoleLogin.allRoom);//获取所有房间
                BuildingManager.instance.GetAllProduceRoom(mRoleLogin.allProRoom);//获取生产房间数据
                BuildingManager.instance.GetAllStoreRoom(mRoleLogin.allStoreRoom);//获取储存类房间数据
                BuildingManager.instance.GetAllResidentRoom(mRoleLogin.allResidentRoom);//获取居民室数据
                List<ProduceEquipInfo> proEquip = mRoleLogin.proEquip;//获取装备房间数据
                BuildingManager.instance.GetAllMagicSkillInfo(mRoleLogin.allMagicSkill);//获取魔法数据
                Box boxs = mRoleLogin.boxs;//背包
                DebugHelper.Log("获取到账号数据" + mRoleLogin.userInfo.userId);

                GameHelper.instance.ServerInfo = true;//让进度条继续
            }
            else DebugHelper.LogError("数据获取失败重新登陆 ret=" + ret);
        }
    }
}
