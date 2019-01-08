using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

public class ZealmConnector {
	/** 建立的连接 */
	public static ZealmSocketConnection connection = null;
	
	/**
     * 建立连接。
     * 
     * @throws Exception
     */
	public static ZealmSocketConnection createConnection(String url)  
	{
		// 建立Socket连接。
		if(connection == null){
			try
			{
				string[] strAdd=url.Split(':');
				string urlAdd=strAdd[0];
				int port =int.Parse(strAdd[1]);
				connection = new ZealmSocketConnection(urlAdd,port);
				connection.start();

				if(!TableRead.my.Equals(null))
				{
					TableRead.my.strInfo=StaticLoc.Loc.Get("meg0099");
				}
			}
			catch (Exception e)
			{
				if(!TableRead.my.Equals(null))
				{
					TableRead.my.strInfo=StaticLoc.Loc.Get("meg0098");
					TableRead.my.isConnectFail=true;
				}
				MonoBehaviour.print(string.Format("IP:{0},{1}",url,e.ToString()));
			}
		}
		 
		return connection;
	}

    /**
     * 尝试重连。
     */
    public static void tryReconnect()
    {
         closeConnection();
    }

    /**
    * 关闭连接。
    */
    public static void closeConnection()
    {
        string stackInfo = new StackTrace().ToString();
//        KDebug.WriteLog("=========================closeConnection========================" + stackInfo);
        if (connection != null)
        {
            try
            {
                ZealmSocketConnection temp = connection;
                connection = null;
				temp.IsConnected=false;
                temp.close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        System.GC.Collect();
    }

	/**
     * 向服务器发送请求。
     * 
     * @param segment 发送的包。
     */
	public static void sendRequest(ZMNetData netData){
		if(connection != null){
			ZealmSocketConnection.writeNetData(netData);
		}else{
			MonoBehaviour.print("==============warning: connection is null. request not sent");
		}
	}
}
