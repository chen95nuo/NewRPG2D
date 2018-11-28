using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class LocalServer : TSingleton<LocalServer>
{
    private Dictionary<BuildRoomName, LocalBuildingData> production;

    public Dictionary<BuildRoomName, LocalBuildingData> Production
    {
        get
        {
            if (production == null)
            {
                production = new Dictionary<BuildRoomName, LocalBuildingData>();
            }
            return production;
        }
    }

    public List<ServerBuildData> saveRoomData;
    public List<ServerHallRoleData> saveRoleData;
    public List<RoleBabyData> saveBabydata = new List<RoleBabyData>();

    public void StartInit()
    {
        if (saveRoomData == null)
        {
            TestRoom();
        }
        ChickPlayerInfo.instance.ChickBuildDic(saveRoomData);
        if (saveRoleData == null)
        {
            HallRoleData data_1 = HallRoleMgr.instance.BuildNewRole(1);
            HallRoleData data_2 = HallRoleMgr.instance.BuildNewRole(1);
            //HallRoleData data_3 = HallRoleMgr.instance.BuildNewRole(1);
            //HallRoleData data_4 = HallRoleMgr.instance.BuildNewRole(1);
            saveRoleData = new List<ServerHallRoleData>();
            saveRoleData.Add(new ServerHallRoleData(9, data_1));
            saveRoleData.Add(new ServerHallRoleData(9, data_2));
            //saveRoleData.Add(new ServerHallRoleData(4, data_3));
            //saveRoleData.Add(new ServerHallRoleData(4, data_4));
        }
        HallRoleMgr.instance.ChickRoleDic(saveRoleData);
        HallRoleMgr.instance.ChickBabyDic(saveBabydata);
        MagicLevel();
    }

    public void RoleChangeRoom(int roleId, LocalBuildingData roomData, int index = 0)
    {
        HallRole hallRole = HallRoleMgr.instance.GetRole(roleId);
        if (roomData == null)
        {
            hallRole.RoleWorldRoam();
        }
        HallEventManager.instance.SendEvent(HallEventDefineEnum.CameraMove);
        for (int i = 0; i < saveRoleData.Count; i++)
        {
            if (saveRoleData[i].role.id == roleId)
            {
                saveRoleData[i].RoomId = roomData.id;
            }
        }
        hallRole.transform.parent = roomData.currentRoom.RolePoint;
        hallRole.currentBuildIndex = index;
        Vector2 roomPoint = roomData.currentRoom.RolePoint.transform.position;
        Vector2 rolePoint = roomData.buildingData.RolePoint[index];
        Vector2 endPoint = roomPoint + rolePoint;
        hallRole.RoleMove(endPoint);
    }

    /// <summary>
    /// 初始刷新房间
    /// </summary>
    private void TestRoom()
    {
        saveRoomData = new List<ServerBuildData>();
        ServerBuildData s_1 = new ServerBuildData(1, 10035, new Vector2(6, 0), 0, 0, 0);
        saveRoomData.Add(s_1);
        ServerBuildData s_2 = new ServerBuildData(2, 10035, new Vector2(6, 1), 0, 0, 0);
        saveRoomData.Add(s_2);
        ServerBuildData s_3 = new ServerBuildData(3, 10035, new Vector2(6, 2), 0, 0, 0);
        saveRoomData.Add(s_3);
        ServerBuildData s_4 = new ServerBuildData(4, 10010, new Vector2(7, 1), 0, 0, 0);
        saveRoomData.Add(s_4);
        ServerBuildData s_8 = new ServerBuildData(8, 10036, new Vector2(10, 2), 0, 0, 0);
        saveRoomData.Add(s_8);
        ServerBuildData s_9 = new ServerBuildData(9, 10001, new Vector2(13, 2), 0, 0, 0);
        saveRoomData.Add(s_9);
        ServerBuildData s_5 = new ServerBuildData(5, 10013, new Vector2(7, 0), 0, 0, 0);
        saveRoomData.Add(s_5);
        ServerBuildData s_6 = new ServerBuildData(6, 10031, new Vector2(7, 2), 0, 0, 0);
        saveRoomData.Add(s_6);
        ServerBuildData s_7 = new ServerBuildData(7, 10016, new Vector2(3, 0), 0, 0, 0);
        saveRoomData.Add(s_7);
    }

    /// <summary>
    /// 魔法技能等级
    /// </summary>
    public void MagicLevel()
    {
        Dictionary<MagicName, MagicData> dic = new Dictionary<MagicName, MagicData>();
        for (int i = 0; i < (int)MagicName.Max; i++)
        {
            MagicData data = MagicDataMgr.instance.GetMagic((MagicName)i, 1);
            dic.Add((MagicName)i, data);
        }
        MagicDataMgr.instance.SetMagicLevel(dic);
        MagicData magicData = MagicDataMgr.instance.GetMagic(MagicName.Fireball, 1);
        RealMagic realMagic = new RealMagic(MagicDataMgr.instance.InstanceMagicID, magicData);
        MagicDataMgr.instance.AllMagicData.useMagic[0] = realMagic;
    }

}
