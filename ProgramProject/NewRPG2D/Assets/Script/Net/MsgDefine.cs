public enum NetSendMsg
{
    /// <summary>
    /// RQ_XXX    		客户端->服务端,必有应答的消息               	     消息号范围:101-999
    /// </summary>
    RQ_StartGame = 101,
    RQ_RoleLogin = 103,
    RQ_CreateRoleName = 105,
    RQ_CreateNewRoom = 109,
    RQ_ResearchMagicSkill = 111,
    RQ_MakeMagicSkill = 113,
    RQ_AddBattleInMagicSkill = 115,
    RQ_RoomUpdateLevel = 117,
    RQ_RoomMerger = 119,
    RQ_SaveAllRoom = 123,
    RQ_ConfirmRearKid = 125,
    RQ_ConfirmKidAdult = 127,
    RQ_ConfirmTrain = 129,
    RQ_DragIntoRoom = 131,
    RQ_Reconnect = 133,
    RQ_UnifiedDiamondConsumerEntry = 135,
    /// <summary>
    /// 客户端->服务端,无需应答的消息		消息号范围:1001-1999
    /// </summary>
    Q_CheckResidentRoomState = 1001,
    Q_CheckProductEquipState = 1002,
    Q_RoomState = 1003,
}

public enum NetReceiveMsg
{
    /// <summary>
    /// RS_XXX          服务端->客户端, 应答的消息                         消息号范围:101-999, 必须为请求的消息号+1
    /// </summary>
    RS_StartGame = 102,
    RS_RoleLogin = 104,
    RS_CreateRoleName = 106,
    RS_CreateNewRoom = 110,
    RS_ResearchMagicSkill = 112,
    RS_MakeMagicSkill = 114,
    RS_AddBattleInMagicSkill = 116,
    RS_RoomUpdateLevel = 118,
    RS_RoomMerger = 120,
    RS_SaveAllRoom = 124,
    RS_ConfirmRearKid = 126,
    RS_ConfirmKidAdult = 128,
    RS_ConfirmTrain = 130,
    RS_DragIntoRoom = 132,
    RS_Reconnect = 134,
    RS_UnifiedDiamondConsumerEntry = 136,
    /// <summary>
    /// 服务端->客户端 ,服务端主动推送的消息	消息号范围:2001-2999
    /// </summary>
    A_RoomInfo = 2001,
    A_ProduceRoom = 2002,
    A_StoreRoom = 2003,
    A_ResidentRoom = 2004,
    A_ProduceEquipInfo = 2005,
    A_QiPao = 2006,
    A_HappinessState = 2007,
    A_ErrorMessage = 2008,
    A_SessionToken = 2009,
}
