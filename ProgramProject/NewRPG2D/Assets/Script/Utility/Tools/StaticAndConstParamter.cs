using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

public class StaticAndConstParamter
{
    public const float FORWARD_THRESHOLD = 0.05f;
    public const float JOYSTICK_TIME = 0.01f;
    public const float MOVE_SPEED_MIN_THRESHOLD = 0.2f;
    public const float MOVE_SPEED_MAX_THRESHOLD = 1.0f;
    public const float GRAVITY = -9.8f;
    public const float CheckFallHeight = 1.5f;
    public const int MaxBagSlot = 20;
    public const float DeltaTime = 0.01f; //UnityEngine.Time.deltaTime;
    public const int MIN_ROTATE_ANGLE = 50;
    public const int VIRTUAL_POSITION_FRAME = 10;
    public const float SEND_POSITION_TIMES = 100f;
    public const float SEND_TIME_TIMES = 1000f;
    public const float SEND_JOYSTICK_TIMES = 10000f;
    public const string GROUND_BAND_NAME = "Ground";
    public const string ITEM_PATH_NAME = "Prefab/";
    public const int MIN_DISTANCE_GROUND = 10;
    public const float MAX_MOVE_X = 23f;
    public const float MAX_MOVE_Y = 10f;
    public const float MAX_MOVE_Z = 11f;

    public const string HeroRootPath = "HeroRoot";
    public const string MonterRootPath = "MonsterRoot";
    public const string ItemRootPath = "ItemRoot";
    public const float AttackSpeed = 1;

}

public enum ItemTypeEnum
{

}

public enum MonstersTypeEnum
{

}

public enum CameraStateEnum
{
    None,
    LockTarget,
    FreeState,
}

public enum EquipTypeEnum
{
    Sword,
    Armor,

}

public enum BodyTypeEnum
{
    Hair_1,//头发
    Hair_2,//胡须
    Beard//身体
}

public enum QualityTypeEnum
{
    White,
    Green,
    Blue,
    Purple,
    Orange,
}

public enum WeaponTypeEnum
{
    Life,                //生活
    Arrow,               //弓箭
    Wand,                //魔杖
    Knife,               //匕首
    Sword,               //剑
}

public enum WeaponProfessionEnum
{
    Fighter,             //战士
    Shooter,             //射手
    Magic,               //法师
}

public enum ProfessionNeedEnum
{
    Fighter,             //战士
    Shooter,             //射手
    Magic,               //法师

    Accounting,          //会计
}

public enum TirggerTypeEnum
{
    Always,
    Death,
    Attack,
    Hurt,
    Reborn,
}

public enum PropertyTypeEnum
{
    Normal,
    Random,
    Special,
}

public enum RolePropertyTypeEnum
{
    Normal,
    Random,
    Special,
}


public enum SexTypeEnum : byte
{
    Man = 1,
    Woman,
}

public enum WeaponName
{
}

public enum RoomStateEnum
{
    FreeTime,
    Gaming,
}


public enum RoomMemberStateEnum
{
    NoPrepare,
    Prepare,
}

public enum RoomReadyStateEnum
{
    Cancel = 0,   //取消准备
    Prepare = 1,  //已经准备
    Leave = 2,    //离开
    Reject = 3,   //踢出房间
    PlayGame = 9, //开始游戏
}

public enum ErrorId
{
    EM_SUCCESS = 0,     //成功,
    EM_ERROR_SERVER = 1,        //服务器错误，需返回登陆,
    EM_CLIENT_DATA_ERROR = 8,       //玩家数据异常,
    EM_ERROR_SERVER_LOGIC = 11,     //服务器内部逻辑错误,
    EM_ERROR_CONFIG_ERR = 12,       //配置表数据错误,
    EM_ERROR_PARAM_FAILD = 13,      //参数错误
    EM_RESOURCE_NOT_ENOUGH = 15,        //资源不足
    EM_NOT_FIND_GAMESERVER = 16,        //找不到GameServer

    EM_LOGIN_ROLE_ERROR = 20,       //角色错误,
    EM_LOGIN_ACCOUNT_ERROR = 21,        //帐号错误,
    EM_LOGIN_PWD_ERROR = 22,        //密码错误,
    EM_REGISTER_ACCOUNT_EXITS = 23,     //帐号已存在,
    EM_LOGIN_WATIING = 25,      //服务器满，请稍候再试,
    EM_LOGIN_ACCOUNT_LOCK = 26,     //帐号被封停,

    EM_CHECK_LOGIN_NOT_FIND_PLAYER = 27,        //登陆玩家未找到,
    EM_CHECK_LOGIN_INVALID_KEY = 28,        //无效登陆key,
    EM_LOGIN_NEED_CREATE_ROLE = 29,     //没有角色（需要创角）,

    EM_CREATEROLE_IS_EXIST = 60,        //角色已存在,
    EM_CREATEROLE_LOGINID_ERROR = 61,		//登陆号错误;
}

#region Struct
public struct AnimatorName
{
    public const string SwitchAnimation = "SwitchAnimationName";
}

public struct PlayerInfo
{
    public long roleId;
    public string roleName;
    public uint roleLevel;
    public uint roleExpCur;
    public uint roleExpTop;
    public uint roleSC;
    public uint roleSCBind;
    public uint roleGold;
    public uint createRoleTime;
    public bool isAnti;
}

public struct RoomInfo
{
    public int RoomId;
    public string RoomName;
    public string RoomPassword;
    public int PlayerCount;
    public RoomStateEnum RoomState;
}

public struct RoomMemberInfo
{
    public long RoleId;
    public string RoleName;
    public bool IsOwner; //房主
    public SexTypeEnum RoleSex;
    public RoomMemberStateEnum MemberState; //准备状态
}

#endregion

#region command

public struct PageMainCommand
{
    public const string SET_JOYSTICK = "SetupJoystick";
}

public struct RoomPageCommad
{
    public const string CreateRoom = "CreateRoom";
    public const string JoinRoom = "JoinRoom";
    public const string LeaveRoom = "LeaveRoom";
    public const string GetRoomList = "GetRoomList";
}

public struct CommonCommand
{
    public const string EnablePage = "Enable";
}

#endregion


