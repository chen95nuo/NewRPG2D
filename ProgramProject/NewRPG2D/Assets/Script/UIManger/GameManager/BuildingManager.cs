/*
 * 提供所有建筑信息
 * 建筑数据中有建筑 则建造数据中的建筑
 * 若没有建筑 则等待服务器提供建筑信息
*/

using Assets.Script.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using proto.SLGV1;
using UnityEngine.UI;

public class BuildingManager : TSingleton<BuildingManager>
{
    private Dictionary<BuildRoomName, List<LocalBuildingData>> allBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
    private Dictionary<int, LocalBuildingData> proBuilding = new Dictionary<int, LocalBuildingData>();
    private Dictionary<BuildRoomName, LocalBuildingData> stockBuilding = new Dictionary<BuildRoomName, LocalBuildingData>();

    private List<LocalBuildingData> levelUpBuild = new List<LocalBuildingData>();

    private int produceInterval = 30;

    public Dictionary<BuildRoomName, List<LocalBuildingData>> AllBuildingData
    {
        get { return allBuilding; }
    }

    public List<LocalBuildingData> LevelUpBuild
    {
        get
        {
            return levelUpBuild;
        }
    }
    #region 房间建设
    /// <summary>
    /// 第一次启动通过服务器或的房间
    /// </summary>
    /// <param name="s_AllRoom"></param>
    public void GetAllBuildingData(List<proto.SLGV1.RoomInfo> s_AllRoom)
    {
        allBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
        for (int i = 0; i < s_AllRoom.Count; i++)
        {
            AddNewRoom(s_AllRoom[i]);
        }
    }

    /// <summary>
    /// 添加一个新房间,如有重复认为修改房间消息
    /// </summary>
    /// <param name="roomInfo"></param>
    public LocalBuildingData AddNewRoom(proto.SLGV1.RoomInfo roomInfo)
    {
        int[] id = TypeOfRoomId(roomInfo.roomId);
        BuildRoomName name = (BuildRoomName)id[0];

        //寻找是否有重复房间
        if (!allBuilding.ContainsKey(name))
        {
            allBuilding.Add(name, new List<LocalBuildingData>());
        }
        LocalBuildingData buildingData = new LocalBuildingData(roomInfo, id[1]);
        allBuilding[name].Add(buildingData);
        SetAllBuildingData(buildingData);
        if (buildingData.leftTime > 0)
        {
            LevelUpBuild.Add(buildingData);
        }
        return buildingData;
    }

    /// <summary>
    /// 写入主房间
    /// </summary>
    public void InitMainHallRoom()
    {
        GetPlayerData.Instance.SetThroneRoom(allBuilding[BuildRoomName.ThroneRoom][0]);
        GetPlayerData.Instance.SetBarracks(allBuilding[BuildRoomName.Barracks][0]);
    }

    /// <summary>
    /// 切场景回来后重新放置房间
    /// </summary>
    public void InitBuildingData()
    {
        foreach (var roomData in allBuilding)
        {
            for (int i = 0; i < roomData.Value.Count; i++)
            {
                SetAllBuildingData(roomData.Value[i]);
            }
        }
    }

    /// <summary>
    /// 房间合并
    /// </summary>
    /// <param name="mergeData"></param>
    public void RoomMerge(RS_RoomMerger mergeData)
    {
        int[] id1 = TypeOfRoomId(mergeData.reqRoomIds[0]);
        LocalBuildingData room_1 = SearchRoomData((BuildRoomName)id1[0], id1[1]);
        int[] id2 = TypeOfRoomId(mergeData.reqRoomIds[1]);
        LocalBuildingData room_2 = SearchRoomData((BuildRoomName)id2[0], id2[1]);
        RemoveRoom(room_1);
        RemoveRoom(room_2);

        AddNewRoom(mergeData.newroomInfo);
    }
    /// <summary>
    /// 合并删除
    /// </summary>
    /// <param name="room"></param>
    public void RemoveRoom(LocalBuildingData room)
    {
        if (CheckProduction(room))
        {
            RemoveProduce(room);
        }
        bool isTrue = allBuilding[room.buildingData.RoomName].Remove(room);
        room.currentRoom.RemoveBuilding();
        if (!isTrue)
            Debug.Log("房间删除 失败");
    }

    /// <summary>
    /// 房间升级
    /// </summary>
    /// <param name="roomUpdateLevel"></param>
    public void RoomUpdateLevelUp(RS_RoomUpdateLevel roomUpdateLevel)
    {
        int[] id = TypeOfRoomId(roomUpdateLevel.roomInfo.roomId);
        BuildRoomName name = (BuildRoomName)id[0];
        LocalBuildingData data = SearchRoomData(name, id[1]);
        data.leftTime = roomUpdateLevel.roomInfo.leftTime;
        if (data.leftTime > 0)
        {
            data.ConstructionType = true;
            LevelUpBuild.Add(data);
        }
        else
        {
            LevelUpBuild.Remove(data);
        }
    }

    /// <summary>
    /// 将房间数据实例化
    /// </summary>
    /// <param name="roomData"></param>
    public void SetAllBuildingData(LocalBuildingData roomData)
    {
        if (MainCastle.instance == null)
        {
            return;
        }
        MainCastle.instance.InstanceRoom(roomData);
        if (roomData.leftTime > 0) //如果该房间在升级
        {
            //发送消息给升级标签
            BuildingRoomLevelUp(roomData);
        }
        for (int i = 0; i < roomData.roleData.Length; i++)
        {
            if (roomData.roleData[i] != null)
            {
                HallRole data = roomData.roleData[i].instance;
                data.roleRoomIndex = i;
                data.transform.parent = roomData.currentRoom.RolePoint;
                data.transform.localPosition = Vector3.zero;
                Vector2 endPoint = roomData.buildingData.RolePoint[i];
                data.RoleMove(endPoint);
            }
        }
    }

    /// <summary>
    /// 房间升级
    /// </summary>
    public void BuildingRoomLevelUp(LocalBuildingData buildingData)
    {
        //如果该房间没有实例 判断不在主场景
        if (buildingData.currentRoom != null)
        {
            //如果该房间正在升级 或升级完成
            if (buildingData.leftTime >= 0)
            {
                buildingData.ConstructionType = true;
            }
        }
    }
    #endregion

    #region 房间功能

    #region 生产类
    /// <summary>
    /// 全部生产类房间
    /// </summary>
    /// <param name="proData"></param>
    public void GetAllProduceRoom(List<proto.SLGV1.ProduceRoom> proData)
    {
        proBuilding = new Dictionary<int, LocalBuildingData>();
        for (int i = 0; i < proData.Count; i++)
        {
            GetProduceRoom(proData[i]);
        }
    }
    /// <summary>
    /// 给生产房间添加时间事件
    /// </summary>
    /// <param name="proData"></param>
    private void GetProduceRoom(proto.SLGV1.ProduceRoom proData)
    {
        string[] id = proData.roomId.Split('_');
        LocalBuildingData data = SearchRoomData((BuildRoomName)int.Parse(id[0]), int.Parse(id[1]));
        data.Stock = proData.toBeCollected;
        data.Yield = proData.speedProd;
        if (proBuilding.ContainsValue(data))
        {
            Debug.Log("重复的生产房间 或者有房间生产参数需要更改,暂时return");
            return;
        }
        int index = CTimerManager.instance.AddListener(produceInterval, 0, ProduceCallBack);
        proBuilding.Add(index, data);
    }
    /// <summary>
    /// 生产事件计时回调
    /// </summary>
    /// <param name="index"></param>
    private void ProduceCallBack(int index)
    {
        LocalBuildingData roomData = proBuilding[index];
        roomData.Stock += (roomData.Yield * produceInterval);
        //必要的情况下通知UI
    }
    private void RemoveProduce(LocalBuildingData data)
    {

    }
    #endregion

    #region 储存类
    /// <summary>
    /// 获取全部储存类房间
    /// </summary>
    /// <param name="stoData"></param>
    public void GetAllStoreRoom(List<proto.SLGV1.StoreRoom> stoData)
    {
        for (int i = 0; i < stoData.Count; i++)
        {
            GetStoreRoom(stoData[i]);
        }
    }

    /// <summary>
    /// 添加储存类房间数据
    /// </summary>
    /// <param name="stoData"></param>
    public void GetStoreRoom(proto.SLGV1.StoreRoom stoData)
    {
        int[] id = TypeOfRoomId(stoData.roomId);
        LocalBuildingData data = SearchRoomData((BuildRoomName)id[0], id[1]);
        data.Stock = stoData.depot;
    }
    #endregion

    #region 居民室

    public void GetAllResidentRoom(List<proto.SLGV1.ResidentRoom> resRoom)
    {
        for (int i = 0; i < resRoom.Count; i++)
        {
            GetResidentRoom(resRoom[i]);
        }
    }

    public void GetResidentRoom(proto.SLGV1.ResidentRoom resRoom)
    {
        //对应房间添加对应人数再加对应时间
    }

    #endregion

    #region 装备生产工厂
    public void GetAllEquipProRoom(List<proto.SLGV1.ProduceEquipInfo> equipRoom)
    {
        for (int i = 0; i < equipRoom.Count; i++)
        {
            GetEquipProRoom(equipRoom[i]);
        }
    }

    public void GetEquipProRoom(proto.SLGV1.ProduceEquipInfo equipRoom)
    {
        //给UI发送房间信息 让UI追踪房间 该房间施工状态为True;
    }
    #endregion

    #region 魔法工厂
    public void GetAllMagicSkillInfo(proto.SLGV1.AllMagicSkillInfo skillRoom)
    {
        //获取所有法术，获取所有法术等级;
    }
    #endregion

    #endregion

    #region UI刷新
    /// <summary>
    /// 刷新所有房间资源
    /// </summary>
    public void ResetUIProduce()
    {
        UpdateProduce(BuildRoomName.GoldSpace);
        UpdateProduce(BuildRoomName.FoodSpace);
        UpdateProduce(BuildRoomName.ManaSpace);
        UpdateProduce(BuildRoomName.WoodSpace);
        UpdateProduce(BuildRoomName.IronSpace);
    }
    /// <summary>
    /// 刷新房间资源
    /// </summary>
    /// <param name="room"></param>
    public void UpdateProduce(BuildRoomName room)
    {
        HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.CheckStock, room);
    }
    public int[] GetBuildDicInfo(BuildingData data)
    {
        int[] index = new int[2];
        PlayerData player = GetPlayerData.Instance.GetData();
        if (data.RoomName != BuildRoomName.Stairs)
        {
            for (int i = 0; i < data.UnlockLevel.Length; i++)
            {
                if (data.UnlockLevel[i] <= player.MainHallLevel)
                {
                    index[1]++;//获取当前等级的可建造数
                }
            }
        }
        else
        {
            index[1] = 4 + player.MainHallLevel;
        }
        if (!allBuilding.ContainsKey(data.RoomName))
        {
            index[0] = 0;
            return index;
        }
        for (int i = 0; i < allBuilding[data.RoomName].Count; i++)
        {
            if (allBuilding[data.RoomName][i] != null)
            {
                index[0]++;//获取该类建筑已建造数量
                if (allBuilding[data.RoomName][i].buildingData.RoomType == RoomType.Production)
                {
                    switch (allBuilding[data.RoomName][i].buildingData.RoomSize)
                    {
                        case 6: index[0]++; break;
                        case 9: index[0] += 2; break;
                        default:
                            break;
                    }
                }
            }
        }
        return index;
    }
    #endregion

    #region 查找功能
    /// <summary>
    /// 通过类型和ID查找房间
    /// </summary>
    /// <param name="type">房间类型</param>
    /// <param name="id">房间ID</param>
    /// <returns></returns>
    public LocalBuildingData SearchRoomData(BuildRoomName type, int id = 1)
    {
        if (allBuilding.ContainsKey(type))
        {
            List<LocalBuildingData> TypeRoom = allBuilding[type];
            for (int i = 0; i < TypeRoom.Count; i++)
            {
                if (TypeRoom[i].id == id) return TypeRoom[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 通过BuildRoomName查找总容量
    /// </summary>
    /// <returns></returns>
    public int SearchRoomSpace(string name)//非储存类房间名转储存房间名
    {
        name = name + "Space";
        BuildRoomName roomname = (BuildRoomName)System.Enum.Parse(typeof(BuildRoomName), name);
        return SearchRoomSpace(roomname);
    }
    public int SearchRoomSpace(BuildRoomName name)
    {
        int space = 0;
        if (allBuilding.ContainsKey(name))
        {
            space += (int)allBuilding[name][0].buildingData.Param2;
        }
        space += GetPlayerData.Instance.GetData().DefaultSpace;
        return space;
    }

    /// <summary>
    /// 通过BuildRoomName查找剩余空间
    /// </summary>
    /// <returns></returns>
    public int SearchRoomEmpty(BuildRoomName name)
    {
        int empty = 0;
        if (allBuilding.ContainsKey(name))
        {
            LocalBuildingData data = allBuilding[name][0];
            empty += (int)(data.buildingData.Param2 - data.Stock);
        }
        PlayerData playData = GetPlayerData.Instance.GetData();
        empty += (playData.DefaultSpace - playData.GetResSpace(name));
        return empty;
    }

    /// <summary>
    /// 通过BuildRoomName查找总数量
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int SearchRoomStock(BuildRoomName name)
    {
        int stock = 0;
        if (allBuilding.ContainsKey(name))
        {
            stock += (int)allBuilding[name][0].Stock;
        }
        stock += GetPlayerData.Instance.GetData().GetResSpace(name);
        return stock;
    }

    public int SearchRoomYield(BuildRoomName name)
    {
        int yield = 0;
        if (allBuilding.ContainsKey(name))
        {
            foreach (var yeildRoom in allBuilding[name])
            {
                yield += yeildRoom.Yield;
            }
        }
        return yield;
    }

    /// <summary>
    /// 查询总人口空间
    /// </summary>
    /// <returns></returns>
    public int SearchRoleSpace()
    {
        int num = (int)HallConfigDataMgr.instance.GetValue(HallConfigEnum.population);
        if (allBuilding.ContainsKey(BuildRoomName.LivingRoom))
        {
            List<LocalBuildingData> LivingRooms = allBuilding[BuildRoomName.LivingRoom];
            foreach (var room in LivingRooms)
            {
                num += (int)room.buildingData.Param2;
            }
        }
        return num;
    }

    /// <summary>
    /// 通过仓库等级和材料转化钻石
    /// </summary>
    /// <param name="name">房间名称</param>
    /// <param name="amount">所需材料数量</param>
    /// <returns></returns>
    public int SearchRoomStockToDiamonds(BuildRoomName name, int amount)
    {
        if (allBuilding.ContainsKey(name))
        {
            LocalBuildingData L_data = SearchRoomData(name);
            return (int)Mathf.Ceil(amount / L_data.buildingData.Param1);
        }
        BuildingData B_data = BuildingDataMgr.instance.GetbuildingData(name);
        float temp = 400;
        if (B_data != null)
        {
            Debug.Log("房间不足加次判定  后期更改");
            temp = B_data.Param1;
        }
        return (int)Mathf.Ceil(amount / temp);
    }

    /// <summary>
    /// 搜索所有可升级的房间
    /// </summary>
    /// <returns></returns>
    public List<LocalBuildingData> SearchCanUpgradedRoom()
    {
        int mainLevel = GetPlayerData.Instance.GetData().MainHallLevel;
        List<LocalBuildingData> canUpBuild = new List<LocalBuildingData>();
        foreach (var room in allBuilding)
        {
            for (int i = 0; i < room.Value.Count; i++)
            {
                if (room.Value[i].buildingData.NeedLevel <= mainLevel
                    && room.Value[i].buildingData.NextLevelID != 0)
                {
                    canUpBuild.Add(room.Value[i]);
                }
            }
        }
        return canUpBuild;
    }

    /// <summary>
    /// 检查是否是生产类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool CheckProduction(LocalBuildingData data)
    {
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.Gold:
                return true;
            case BuildRoomName.Food:
                return true;
            case BuildRoomName.Mana:
                return true;
            case BuildRoomName.Wood:
                return true;
            case BuildRoomName.Iron:
                return true;
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// 检查是否是储存类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool CheckStorage(LocalBuildingData data)
    {
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.GoldSpace:
                return true;
            case BuildRoomName.FoodSpace:
                return true;

            case BuildRoomName.ManaSpace:
                return true;

            case BuildRoomName.WoodSpace:
                return true;

            case BuildRoomName.IronSpace:
                return true;
            default:
                break;
        }
        return false;
    }
    #endregion

    #region 工具

    /// <summary>
    /// 转换名字
    /// </summary>
    /// <param name="stId"></param>
    /// <returns></returns>
    public int[] TypeOfRoomId(string stId)
    {
        string[] st = stId.Split('_');
        int[] id = new int[2] { int.Parse(st[0]), int.Parse(st[1]) };
        return id;
    }

    #endregion

    #region 帮助类

    public int TimeToDiamonds(BuildingData data)
    {
        return (int)Mathf.Ceil(data.NeedTime * 0.01f);
    }

    /// <summary>
    /// 通过房间需要的材料 修改txt字体颜色 返回材料字典
    /// </summary>
    /// <param name="data"></param>
    /// <param name="txt"></param>
    /// <returns></returns>
    public Dictionary<MaterialName, int> RoomNeedMaterialHelper(BuildingData data, Text[] txt)
    {
        Dictionary<MaterialName, int> dic = new Dictionary<MaterialName, int>();
        int needDimonds = 0;
        for (int i = 0; i < data.needMaterial.Length; i++)
        {
            if (data.needMaterial[i] > 0)
            {
                BuildRoomName name = MaterialNameToBuildRoomName((MaterialName)i + 1);
                int stock = SearchRoomStock(name);
                int temp = stock - data.needMaterial[i];
                if (temp < 0)
                {
                    temp = -temp;
                    dic.Add((MaterialName)i + 1, temp);
                    txt[i].text = string.Format(LanguageDataMgr.instance.redSt, data.needMaterial[i]);
                    needDimonds += SearchRoomStockToDiamonds(name, temp);
                }
                else
                {
                    txt[i].text = string.Format(LanguageDataMgr.instance.whiteText, data.needMaterial[i]);
                }
            }
        }
        dic.Add(MaterialName.Diamonds, needDimonds);
        return dic;
    }
    /// <summary>
    /// 房间需要的材料转成钻石
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int RoomNeedDiamondsHelper(BuildingData data)
    {
        int needDia = 0;
        for (int i = 0; i < data.needMaterial.Length; i++)
        {
            if (data.needMaterial[i] > 0)
            {
                BuildRoomName name = MaterialNameToBuildRoomName((MaterialName)i + 1);
                needDia += SearchRoomStockToDiamonds(name, data.needMaterial[i]);
            }
        }
        return needDia;
    }
    /// <summary>
    /// 判断仓库大小是否足够
    /// </summary>
    public bool RoomNeedSpaceHelper(BuildingData data)
    {
        for (int i = 0; i < data.needMaterial.Length; i++)
        {
            BuildRoomName name = MaterialNameToBuildRoomName((MaterialName)i + 1);
            if (SearchRoomSpace(name) < data.needMaterial[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 通过材料名字转换房间名字
    /// </summary>
    /// <param name="mat"></param>
    /// <returns></returns>
    public BuildRoomName MaterialNameToBuildRoomName(MaterialName mat)
    {
        BuildRoomName name = BuildRoomName.Nothing;
        switch (mat)
        {
            case MaterialName.Diamonds:
                break;
            case MaterialName.Gold:
                name = BuildRoomName.GoldSpace;
                break;
            case MaterialName.Mana:
                name = BuildRoomName.ManaSpace;
                break;
            case MaterialName.Wood:
                name = BuildRoomName.WoodSpace;
                break;
            case MaterialName.Iron:
                name = BuildRoomName.IronSpace;
                break;
            default:
                break;
        }
        return name;
    }
    public RoleAttribute NameToAtr(BuildRoomName RoomName)
    {
        switch (RoomName)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                return RoleAttribute.Gold;
            case BuildRoomName.Food:
                return RoleAttribute.Food;
            case BuildRoomName.Mana:
                return RoleAttribute.Mana;
            case BuildRoomName.Wood:
                return RoleAttribute.Wood;
            case BuildRoomName.Iron:
                return RoleAttribute.Iron;
            case BuildRoomName.FighterRoom:
                return RoleAttribute.Fight;
            case BuildRoomName.Kitchen:
                return RoleAttribute.Food;
            case BuildRoomName.Mint:
                return RoleAttribute.Gold;
            case BuildRoomName.Laboratory:
                return RoleAttribute.Mana;
            case BuildRoomName.Crafting:
                return RoleAttribute.Wood;
            case BuildRoomName.Foundry:
                return RoleAttribute.Fight;
            case BuildRoomName.Hospital:
                return RoleAttribute.HP;
            case BuildRoomName.MagicWorkShop:
                return RoleAttribute.ManaSpeed;
            case BuildRoomName.MagicLab:
                return RoleAttribute.ManaSpeed;
            case BuildRoomName.WeaponsWorkShop:
            case BuildRoomName.ArmorWorkShop:
                return RoleAttribute.ManaSpeed;
            case BuildRoomName.GemWorkShop:
                return RoleAttribute.ManaSpeed;
            case BuildRoomName.Barracks:
                return RoleAttribute.Fight;
            case BuildRoomName.LivingRoom:
                return RoleAttribute.Max;
            case BuildRoomName.MaxRoom:
            default:
                break;
        }
        return RoleAttribute.Nothing;

    }
    #endregion
}
