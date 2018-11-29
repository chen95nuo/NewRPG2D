public enum NetSendMsg
{
    /// <summary>
    /// RQ_XXX    		客户端->服务端,必有应答的消息               	     消息号范围:101-999
    /// </summary>
    RQ_StartGame = 101,
    RQ_RoleLogin = 103,
    RQ_CreateRoleName = 105,
    RQ_CreateNewRoom = 107,
    RQ_ResearchMagicSkill = 109,
    RQ_MakeMagicSkill = 111,
    RQ_AddBattleInMagicSkill = 113,
    RQ_RoomUpdateLevel = 115,
    RQ_RoomMerger = 117,
    RQ_RoomSplit = 119,
    RQ_SaveAllRoom = 121,
    RQ_ConfirmRearKid = 123,
    RQ_ConfirmKidAdult = 125,
    RQ_ConfirmTrain = 127,
    RQ_DragIntoRoom = 129,
    /// <summary>
    /// 客户端->服务端,无需应答的消息		消息号范围:1001-1999
    /// </summary>
}

public enum NetReceiveMsg
{
    /// <summary>
    /// RS_XXX          服务端->客户端, 应答的消息                         消息号范围:101-999, 必须为请求的消息号+1
    /// </summary>
    RS_StartGame = 102,
    RS_RoleLogin = 104,
    RS_CreateRoleName = 106,
    RS_CreateNewRoom = 108,
    RS_ResearchMagicSkill = 110,
    RS_MakeMagicSkill = 112,
    RS_AddBattleInMagicSkill = 114,
    RS_RoomUpdateLevel = 116,
    RS_RoomMerger = 118,
    RS_RoomSplit = 120,
    RS_SaveAllRoom = 122,
    RS_ConfirmRearKid = 124,
    RS_ConfirmKidAdult = 126,
    RS_ConfirmTrain = 128,
    RS_DragIntoRoom = 130,
    /// <summary>
    /// 服务端->客户端 ,服务端主动推送的消息	消息号范围:2001-2999
    /// </summary>
    A_ErrorMessage = 2000,
    A_ProduceRoom = 2001,
    A_StoreRoom = 2002,
    A_ResidentRoom = 2003,
    A_QiPao = 2004,
    A_HappinessState = 2005,
    A_SessionToken = 2006,
}
