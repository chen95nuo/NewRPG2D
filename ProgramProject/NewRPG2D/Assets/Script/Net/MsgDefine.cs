public enum NetSendMsg
{
    /// <summary>
    /// RQ_XXX    		客户端->服务端,必有应答的消息               	     消息号范围:101-999
    /// </summary>
    RQ_StartGame       = 101,
    /// <summary>
    /// 客户端->服务端,无需应答的消息		消息号范围:1001-1999
    /// </summary>
    Q_HG_RefreshTili = 1003,
 
}

public enum NetReceiveMsg
{
    /// <summary>
    /// RS_XXX          服务端->客户端, 应答的消息                         消息号范围:101-999, 必须为请求的消息号+1
    /// </summary>
    RS_StartGame = 102,

    /// <summary>
    /// 服务端->客户端 ,服务端主动推送的消息	消息号范围:2001-2999
    /// </summary>
    A_ErrorMessage = 2001,
    A_HG_Tili = 2047,
}
