using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class HallRoleMgr : TSingleton<HallRoleMgr>
{
    Dictionary<HallRoleData, HallRole> dic = new Dictionary<HallRoleData, HallRole>();
    Dictionary<int, RoleTrainHelper> timeAction = new Dictionary<int, RoleTrainHelper>();
    Dictionary<int, RoleBabyData> childrenTime = new Dictionary<int, RoleBabyData>();
    Dictionary<int, RoleLoveHelper> loveData = new Dictionary<int, RoleLoveHelper>();
    private List<HallRole> AllHallRole = new List<HallRole>();
    private int childNeedTime = 360;//小孩所需时间
    private int LoveTime = 10;//恋爱所需时间

    private GameObject roleBoy;//男孩
    private GameObject roleGirl;//女孩
    private GameObject roleMale;//男人
    private GameObject roleFemale;//女人

    private GameObject RoleBoy
    {
        get
        {
            if (roleBoy == null)
            {
                roleBoy = Resources.Load("UIPrefab/Role/RoleBoy") as GameObject;
            }
            return roleBoy;
        }
    }

    private GameObject RoleGirl
    {
        get
        {
            if (roleGirl == null)
            {
                roleGirl = Resources.Load("UIPrefab/Role/RoleGirl") as GameObject;
            }
            return roleGirl;
        }
    }

    private GameObject RoleMale
    {
        get
        {
            if (roleMale == null)
            {
                roleMale = Resources.Load("UIPrefab/Role/RoleMale") as GameObject;
            }
            return roleMale;
        }
    }

    private GameObject RoleFemale
    {
        get
        {
            if (roleFemale == null)
            {
                roleFemale = Resources.Load("UIPrefab/Role/RoleFemale") as GameObject;
            }
            return roleFemale;
        }
    }

    public void AddRole(HallRoleData data, HallRole role)
    {
        dic.Add(data, role);
    }

    public void GetServerRole()
    {

    }

    public void BuildServerRole(HallRoleData data, RoomMgr room)
    {
        int count = MainCastle.instance.NewRolePoint.childCount;
        HallRole role = InstantiateRole(data.sexType, false);
        role.transform.localPosition = Vector3.right * (count + 2 * 2);
        role.UpdateInfo(data);
    }

    public HallRole BuildNewRole(int sex)
    {
        if (sex == 0)
        {
            sex = Random.Range(1, 3);
        }
        int star = Random.Range(1, 4);
        int[] level = new int[6];
        for (int i = 0; i < level.Length; i++)
        {
            level[i] = Random.Range(1, 4);
        }
        HallRoleData data = new HallRoleData(sex, star, level);
        int count = MainCastle.instance.NewRolePoint.childCount - 1;
        HallRole role = InstantiateRole((RoleSexType)sex, false);
        role.transform.localPosition = Vector3.left * count;
        role.UpdateInfo(data);
        return role;
    }

    /// <summary>
    /// 刷出小孩
    /// </summary>
    /// <param name="data"></param>
    public HallRole BuildNewRole(RoleBabyData data)
    {
        HallRole role = InstantiateRole(data.child.sexType, true);
        int count = MainCastle.instance.NewRolePoint.childCount - 1;
        role.UpdateInfo(data);
        return role;
    }

    private HallRole InstantiateRole(RoleSexType sexType, bool isBaby)
    {
        for (int i = 0; i < AllHallRole.Count; i++)
        {
            if (AllHallRole[i].isChildren == true && AllHallRole[i].sex == sexType)
            {
                Debug.Log("对象池中找到相应实例");
                return AllHallRole[i];
            }
        }
        GameObject go = null;
        if (sexType == RoleSexType.Male && isBaby == false)
        {
            go = GameObject.Instantiate(RoleMale, MainCastle.instance.NewRolePoint) as GameObject;
        }
        else if (sexType == RoleSexType.Female && isBaby == false)
        {
            go = GameObject.Instantiate(RoleFemale, MainCastle.instance.NewRolePoint) as GameObject;
        }
        else if (sexType == RoleSexType.Male && isBaby == true)
        {
            go = GameObject.Instantiate(RoleBoy, MainCastle.instance.NewRolePoint) as GameObject;
        }
        else if (sexType == RoleSexType.Female && isBaby == true)
        {
            go = GameObject.Instantiate(RoleGirl, MainCastle.instance.NewRolePoint) as GameObject;
        }
        else
        {
            Debug.LogError("没有找到要生成的角色");
        }
        return go.GetComponent<HallRole>();
    }

    /// <summary>
    /// 生成新宝宝数据
    /// </summary>
    /// <param name="father"></param>
    /// <param name="mather"></param>
    public RoleBabyData BuildNewBaby(HallRoleData father, HallRoleData mather)
    {
        int sex = Random.Range(1, 3);
        int allStar = father.Star + mather.Star;
        int star = 0;
        ChildData childData = ChildDataMgr.instance.GetData(allStar);
        int[] level = new int[6];
        for (int i = 0; i < level.Length; i++)
        {
            int allLevel = father.RoleLevel[i].Level + mather.RoleLevel[i].Level;
            float roll = Random.Range(2, 7) * 0.1f;
            level[i] = (int)((allLevel / 2) * roll) == 0 ? 1 : (int)((allLevel / 2) * roll);
        }
        for (int i = 0; i < childData.StarLevel.Length; i++)
        {
            int roll = Random.Range(0, 101);
            if (childData.StarLevel[i] > roll)
            {
                star = i + 1;
                HallRoleData role = new HallRoleData(sex, star, level);
                RoleBabyData roleHelper = new RoleBabyData(role, father, mather);
                //BuildNewChild(roleHelper);
                return roleHelper;
            }
        }
        star = childData.StandLevel;
        HallRoleData role_1 = new HallRoleData(sex, star, level);
        RoleBabyData roleHelper_1 = new RoleBabyData(role_1, father, mather);
        //BuildNewChild(roleHelper_1);
        return roleHelper_1;
    }
    ///// <summary>
    ///// 生成新宝宝
    ///// </summary>
    ///// <param name="data"></param>
    //private void BuildNewChild(RoleChildrenData data)
    //{
    //    GameObject go = Resources.Load("UIPrefab/Role/Children") as GameObject;
    //    go = GameObject.Instantiate(go, MainCastle.instance.NewRolePoint);
    //    int count = MainCastle.instance.NewRolePoint.childCount - 1;
    //    go.transform.position = GetRole(data.father).transform.position;
    //    HallRole role = go.GetComponent<HallRole>();
    //    role.UpdateInfo(data.child);
    //}

    /// <summary>
    /// 驱逐角色
    /// </summary>
    public void RemoveRole()
    {

    }

    /// <summary>
    /// 获取角色实例
    /// </summary>
    public HallRole GetRole(HallRoleData data)
    {
        return dic[data];
    }

    /// <summary>
    /// 角色切换房间
    /// </summary>
    /// <param name="room"></param>
    public void RoleChangeRoom(HallRole newRole, HallRole oldRole)
    {
        RoomMgr oldRoleRoom = oldRole.RoleData.currentRoom;
        RoomMgr newRoleRoom = newRole.RoleData.currentRoom;
        //如果当前移过来的角色不是漫游状态
        if (newRoleRoom != null)
        {
            newRoleRoom.RemoveRole(newRole);
        }
        oldRoleRoom.RemoveRole(oldRole);
        oldRoleRoom.AddRole(newRole);
        if (newRoleRoom != null)
        {
            newRoleRoom.AddRole(oldRole);
        }
        else
        {
            oldRole.ChangeType(BuildRoomName.Nothing);
        }


    }
    /// <summary>
    /// 角色排序
    /// </summary>
    /// <param name="Atr"></param>
    /// <returns></returns>
    public List<HallRoleData> ScreenRole(RoleAttribute Atr)
    {
        List<HallRoleData> data = new List<HallRoleData>();
        foreach (var item in dic)
        {
            data.Add(item.Key);
        }
        data.Sort((x, y) => (y.Attribute(Atr).CompareTo(x.Attribute(Atr))));
        return data;
    }
    /// <summary>
    /// 角色排序
    /// </summary>
    /// <returns></returns>
    public List<HallRoleData> ScreenRole()
    {
        List<HallRoleData> data = new List<HallRoleData>();
        foreach (var item in dic)
        {
            data.Add(item.Key);
        }
        data.Sort((x, y) => (x.Star.CompareTo(y.Star)));
        return data;
    }

    /// <summary>
    /// 开始训练
    /// </summary>
    public void StartTrain(HallRoleData data, TrainType atr)
    {
        data.TrainType = RoleTrainType.LevelUp;
        int needTime = TrainDataMgr.instance.GetTrainData(atr, data.RoleLevel[(int)atr + 1].Level + 1);
        int index = CTimerManager.instance.AddListener(1f, needTime, ChickTrainTime);
        data.trainIndex = index;
        RoleTrainHelper trainHelper = new RoleTrainHelper(data, atr, needTime);
        if (timeAction.ContainsKey(index))
        {
            Debug.LogError("计时器重复");
        }
        timeAction.Add(index, trainHelper);
    }
    /// <summary>
    /// 训练中监听
    /// </summary>
    /// <param name="index"></param>
    private void ChickTrainTime(int index)
    {
        //如果该角色所在房间正在施工那么 训练终止
        if (timeAction[index].role.currentRoom.ConstructionType)
        {
            StopTrain(timeAction[index].role);
            StopTrain(index);
            return;
        }
        timeAction[index].time--;
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.ChickRoleTrain, index);
        if (timeAction[index].time <= 0)
        {
            CTimerManager.instance.RemoveLister(index);
            CompleteTrain(index);
        }
    }
    public RoleTrainHelper GetTrainRole(int index)
    {
        RoleTrainHelper role = timeAction[index];
        return role;
    }
    /// <summary>
    /// 角色训练中止
    /// </summary>
    public void StopTrain(HallRoleData data)
    {
        foreach (var item in timeAction)
        {
            if (item.Value.role == data)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                timeAction.Remove(item.Key);
                return;
            }
        }
        Debug.LogError("没有找到要删除的角色");
    }
    public void StopTrain(int index)
    {
        CTimerManager.instance.RemoveLister(index);
        timeAction.Remove(index);
    }
    /// <summary>
    /// 训练暂停
    /// </summary>
    /// <param name="index"></param>
    public void PauseTrain(int index)
    {
        CTimerManager.instance.PauseTimer(index, false);
    }
    /// <summary>
    /// 训练继续
    /// </summary>
    /// <param name="index"></param>
    public void ContinueTrain(int index)
    {
        CTimerManager.instance.PauseTimer(index, true);
    }
    /// <summary>
    /// 角色训练完成
    /// </summary>
    public void CompleteTrain(int index)
    {
        //删除这个角色 并且在角色头顶显示图标
        HallRole role = GetRole(timeAction[index].role);
        role.TrainComplete(timeAction[index].atr);
        role.RoleData.TrainType = RoleTrainType.Complete;
    }
    /// <summary>
    /// 升级完成
    /// </summary>
    /// <param name="data">升级的角色</param>
    /// <param name="isTrue">是否是加速完成</param>
    public void LevelComplete(HallRoleData data, bool isTrue)
    {
        foreach (var item in timeAction)
        {
            if (item.Value.role == data)
            {
                string s_Atr = item.Value.atr.ToString();
                RoleAttribute atr = (RoleAttribute)System.Enum.Parse(typeof(RoleAttribute), s_Atr);
                data.LevelUp(atr);
                Debug.Log("升级技能为" + atr);
                data.trainIndex = 0;
                TrainType type = item.Value.atr;
                if (isTrue)
                {
                    UIRoleTipGroup.instance.CloseIcon(item.Value.role);
                    timeAction.Remove(item.Key);
                }
                else
                {
                    data.TrainType = RoleTrainType.Complete;
                    StopTrain(item.Key);
                }
                ChickNextLevelUp(data, type);//检查是否可以继续升级
                return;
            }
        }
        Debug.LogError("没有找到要升级的角色");
    }

    /// <summary>
    /// 恋爱开始
    /// </summary>
    /// <param name="role_1"></param>
    /// <param name="role_2"></param>
    public void LoveStart(HallRoleData role_1, HallRoleData role_2)
    {
        role_1.LoveType = RoleLoveType.Start;
        role_2.LoveType = RoleLoveType.Start;

        int index = CTimerManager.instance.AddListener(1f, LoveTime, LoveCallBack);
        RoleLoveHelper data = new RoleLoveHelper(role_1, role_2, LoveTime);
        loveData.Add(index, data);
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.ChickRoleLove, index);
    }

    public void LoveCallBack(int index)
    {
        loveData[index].time--;
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.ChickRoleLove, index);
        if (loveData[index].time <= 0)
        {
            LoveComplete(index);
        }
    }
    public RoleLoveHelper GetLoveData(int index)
    {
        return loveData[index];
    }

    public void LoveStop(int index)
    {
        CTimerManager.instance.RemoveLister(index);
        loveData[index].role[0].LoveType = RoleLoveType.Nothing;
        loveData[index].role[1].LoveType = RoleLoveType.Nothing;
        loveData.Remove(index);
    }

    /// <summary>
    /// 恋爱结束
    /// </summary>
    public void LoveComplete(int index)
    {
        RoleBabyData babyData = BuildNewBaby(loveData[index].role[0], loveData[index].role[1]);
        HallRole role;
        if (loveData[index].role[0].sexType == RoleSexType.Female)
        {
            loveData[index].role[0].LoveType = RoleLoveType.ChildBirth;
            loveData[index].role[1].LoveType = RoleLoveType.Nothing;
            loveData[index].role[0].babyData = babyData;
            role = GetRole(loveData[index].role[0]);
        }
        else
        {
            loveData[index].role[0].LoveType = RoleLoveType.Nothing;
            loveData[index].role[1].LoveType = RoleLoveType.ChildBirth;
            loveData[index].role[1].babyData = babyData;
            role = GetRole(loveData[index].role[1]);
        }
        role.LoveComplete();//显示气泡
        loveData.Remove(index);
    }

    /// <summary>
    /// 宝宝成长开始
    /// </summary>
    /// <param name="data"></param>
    public void ChildrenStart(RoleBabyData data)
    {
        data.time = childNeedTime;
        int index = CTimerManager.instance.AddListener(1, childNeedTime, ChildrenCallback);
        childrenTime.Add(index, data);
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.ChickChildTime, index);
    }
    /// <summary>
    /// 宝宝成长消息返回
    /// </summary>
    /// <param name="index"></param>
    public void ChildrenCallback(int index)
    {
        childrenTime[index].time--;
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.ChickChildTime, index);
        if (childrenTime[index].time <= 0)
        {
            ChildrenComplete(index);
        }
    }
    /// <summary>
    /// 宝宝成长结束 创建头顶Icon
    /// </summary>
    public void ChildrenComplete(int index)
    {
        CTimerManager.instance.RemoveLister(index);

        //宝宝头顶显示Icon;

        childrenTime.Remove(index);
    }
    /// <summary>
    /// 完成新角色养成 已点击头顶Icon
    /// </summary>
    public void ChildrenEnd() { }

    /// <summary>
    /// 检查下一级升级
    /// </summary>
    public void ChickNextLevelUp(HallRoleData data, TrainType type)
    {
        if (data.currentRoom.BuildingData.Param2 > data.GetAtrLevel(data.currentRoom.BuildingData.RoomName))
        {
            StartTrain(data, type);
            data.TrainType = RoleTrainType.LevelUp;
        }
        else
        {
            data.TrainType = RoleTrainType.MaxLevel;
        }
    }

    public RoleTrainHelper FindTrainRole(HallRoleData data)
    {
        RoleTrainHelper temp;

        foreach (var item in timeAction)
        {
            if (item.Value.role == data)
            {
                temp = item.Value;
                return temp;
            }
        }
        Debug.LogError("没有找到对应角色");
        return null;
    }
    public RoleTrainHelper FindTrainRole(int index)
    {
        return timeAction[index];
    }
}

public class RoleTrainHelper
{
    public HallRoleData role;
    public TrainType atr;
    public float time;
    public HallChildren children;
    public float maxTime;

    public RoleTrainHelper(HallRoleData data, TrainType atr, float time)
    {
        this.role = data;
        this.atr = atr;
        this.time = time;
        this.maxTime = time;
    }
}

public class RoleLoveHelper
{
    public int time;
    public HallRoleData[] role;
    public RoleLoveHelper(HallRoleData role_1, HallRoleData role_2, int time)
    {
        this.time = time;
        this.role = new HallRoleData[2];
        role[0] = role_1;
        role[1] = role_2;
    }
}

public class RoleBabyData
{
    public HallRoleData child;
    public HallRoleData father;
    public HallRoleData mather;
    public int time;

    public RoleBabyData(HallRoleData child, HallRoleData father, HallRoleData mather)
    {
        this.child = child;
        this.father = father;
        this.mather = mather;
    }
}

public class RoleChildData
{
    public HallRoleData child;
    public int time;

    public RoleChildData(HallRoleData child, int time)
    {
        this.child = child;
        this.time = time;
    }
}

