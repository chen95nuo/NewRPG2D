
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;

public class ZealmSocketConnection
{
    private Thread connectThread;
    private ZealmSocketConnection parent;
    private byte runType; // 0 - Reader, 1 - Writer

    private TcpClient tcpClient;
    private NetworkStream netStream;
    private long lastWriteTime;
    private long lastReadTime;

    protected NetworkStream _in;
    protected NetworkStream _out;
    protected ZealmSocketConnection _reader;
    protected ZealmSocketConnection _writer;
    protected bool _isConnected;

    private static event Action<bool> eventConnection;

    public bool IsConnected
    {
        get
        {
            return _isConnected;
        }
         set
        {
            _isConnected = value;
            if (eventConnection != null)
            {
                eventConnection(_isConnected);
            }
        }
    }

    public static void SetConnectionEvent(Action<bool> mAction)
    {
        eventConnection = mAction;
    }

    protected byte[] HEAD = new byte[2]{
                    (byte)'Z', (byte)'M'
    };

    protected byte[] HEAD_RECV = new byte[2]{
                    (byte)'Z',(byte)'M'
    };


    public static List<ZMNetData> netDataList = new List<ZMNetData>();


    public ZealmSocketConnection(String url,int port)
    {
        /*int l = url.IndexOf("#");
        long serverID = 0;
        
        if(l != -1){
            String sid = url.Substring(l + 1);
            url = url.Substring(0, l);
            serverID = Long.parseLong(sid, 16);
        }*/
        tcpClient = new TcpClient(url, port);
        netStream = tcpClient.GetStream();
        IsConnected = true;
//        Debug.Log("-------------------------IsConnected");
    //    netStream.ReadTimeout = 200;
        //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //socket.Connect(url, 9998);

        //BinaryWriter bw = new BinaryWriter(_out);
        //bw.Write()
        //BinaryReader br = new BinaryReader(_in);
        //br.re

        //_in = new DataInputStream(socket.getInputStream());
        //_out = new DataOutputStream(socket.getOutputStream());
        //_connection = (StreamConnection)Connector.open(url, Connector.READ_WRITE, true);

        //#if StreamMode == strict
        //# _in = new DataInputStream(_connection.openInputStream());
        //# _out = new DataOutputStream(_connection.openOutputStream());
        //#else
        //_in = _connection.openDataInputStream();
        //_out = _connection.openDataOutputStream();
        //#endif

        //_isConnected = true;
        //_segments.removeAllElements();

        //if(serverID != 0){
        //    _out.writeInt((int)(serverID >> 16));
        //    _out.writeShort((short)serverID);
        //    _out.flush();
        //}
    }

    public ZealmSocketConnection(ZealmSocketConnection par, byte rt)
    {
        parent = par;
        runType = rt;
    }

    public void start()
    {
        //RealData = new Thread(new ThreadStart(NetDataManager.rollreadData));
        //RealData.IsBackground = true;
        //RealData.Start();
		connectThread = new Thread(new ThreadStart(this.run));
		connectThread.IsBackground = true; //设置主线程为后台线程，在托管进程中停止后，会关闭所有线程。
        connectThread.Start();

    }

    public void run()
    {
        MonoBehaviour.print("ZealmSocketConnection run()");
        if (parent == null)
        {
            _reader = new ZealmSocketConnection(this, (byte)0);
            new Thread(new ThreadStart(_reader.run)).Start();

            _writer = new ZealmSocketConnection(this, (byte)1);
            new Thread(new ThreadStart(_writer.run)).Start();

        }
        else if (runType == (byte)0)
        {
            runReader();
        }
        else
        {
            runWriter();
        }
    }


    public void CZprocessNetData(byte[] netData)
    {
//		if (netData [0] == 255 && netData [1] == 255 && netData [2] == 255 && (netData [3] == 3 || netData [3] == 2)) 
//		{
			NetDataManager.addNetData (netData);
//			Debug.Log("-----------DDDDDDDDDDDDDDDDDDDDDDDDDDDDDD--------------"+netData.Length);
//		} 
//		else 
//		{
//			Debug.Log("-----------ffffffffffffffffffffffffffffff--------------"+netData.Length);
//		}
    }
    //public void processNetData(ZMNetData netData)
    //{
    //    if (netData.hand[0] == 90 && netData.hand[1] == 77)
    //    {
    //        NetDataManager.addNetData(netData);
    //    }
    //    else
    //    {
    //        Debug.Log("--------数据非法，不做处理---------");
    //    }
    //}

    public void runReader()
    {
        while (parent.IsConnected)
        {
          //  Thread.Sleep(500);
            try
            {
                byte[] CZnetData = parent.CZreadNetData();
                if (CZnetData == null)
                {
                    continue;
                }
                parent.CZprocessNetData(CZnetData);//==================================================================
            }
            catch (Exception ex)
            {
//				KDebug.WriteLog("%%%%%%%%%%%%%%%%%%%%runReader%%%%%%%%%%%%%%%%%%%%%%%" + ex.Message);
              //  MonoBehaviour.print("%%%%%%%%%%%%%%%%%%%%runReader%%%%%%%%%%%%%%%%%%%%%%%" + ex.Message);
				Debug.Log("%%%%%%%%%%%%%%%%%%%%runReader%%%%%%%%%%%%%%%%%%%%%%%" + ex.Message);

                try
                {
                    if (parent.IsConnected)
                    {
                        ZealmConnector.closeConnection();
                        parent.tryReconnect();
                        //EventManager.addEvent(Const.EVENT_DISCONNECTED, 0);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    Debug.LogError(ex.ToString());
                    throw new Exception(ex.Message);
                }
                break;
            }
        }
    }

    protected void write(ZMNetData netData)
    {
        netData.flush();
        netStream.Write(HEAD, 0, HEAD.Length);

        //  MonoBehaviour.print("==============" + HEAD.Length + netData.data.Length);
        byte[] dataLength = BitConverter.GetBytes(HEAD.Length + netData.data.Length + 4);
        netStream.Write(dataLength, 0, dataLength.Length);

        netStream.Write(netData.data, 0, netData.data.Length);
        netStream.Flush();
    }


    public void runWriter()
    {
        while (parent.IsConnected)
        {
            try
            {
                while (ZealmSocketConnection.netDataList.Count != 0)
                {
                 // ZMNetData zmNetData = (ZMNetData)ZealmSocketConnection.netDataList[0];
                 // ZealmSocketConnection.netDataList.RemoveAt(0);
                 // try
                 // {
                 //     parent.write(zmNetData);
                 //       parent.lastWriteTime = Utils.DateTimeExtensions.currentTimeMillis();
                 // }
                 // catch (Exception ex)
                 // {
                 //     KDebug.WriteLog("WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW=========WWWWWWWWWW----------------"+ zmNetData.type +"==========" +ex.Message);
                 //     break;
                 // }
                }
            }
            catch (Exception ex)
            {
//				KDebug.WriteLog("%%%%%%%%%%%%%%%%%%%%runWriter%%%%%%%%%%%%%%%%%%%%%%%" + ex.Message);
                MonoBehaviour.print("#############runWriter#############" + ex.Message);
                try
                {
                    if (parent.IsConnected)
                    {
                        ZealmConnector.closeConnection();
                        parent.tryReconnect();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(ex.Message);
                }
                break;
            }
          finally
          {
              try
              {
                  Thread.Sleep(50);
              }
              catch (Exception e)
              {
              }
          }
        }
    }

    public static void writeNetData(ZMNetData netData)
    {
        ZealmConnector.connection.write(netData);
        //netDataList.Add(netData);
    }

  //  /// GZip解压函数
  //  public byte[] GZipDecompress(byte[] data)
  //  {
  //      try
  //      {
  //          MemoryStream stream = new MemoryStream();
  //          Stream gZipStream = new GZipInputStream(new MemoryStream(data));
  //          byte[] bytes = new byte[102400];
  //          int count = 0;
  //          while ((count = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
  //          {
  //              stream.Write(bytes, 0, count);
  //          }
  //          gZipStream.Close();

  //          byte[] re = stream.ToArray();
  //          stream.Close();

  //          byte[] retu = new byte[re.Length - 27];
  //          Array.Copy(re, 27, retu, 0, retu.Length);
  //          bytes = new byte[0];
  //          return retu;
  //      }
  //      catch (Exception ex){
  ////          KDebug.Log("GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGzIP----------IS-----------ERROR--------"+ex);
  //          return null;
  //      }
  //  }
    public byte[] CZreadNetData()
    {
        if (netStream.CanRead)
        {
            int len = tcpClient.ReceiveBufferSize;
            if (len > 8192)
            {
                len = 1448;
            }
		//	len = 50;
            int netlen = 0;
            int templen = 0;
            byte[] bytes = new byte[len];
            templen = netStream.Read(bytes, 0, bytes.Length);
            netlen = templen;
            while (netStream.DataAvailable && templen == len)
            {
                byte[] tempbytes = new byte[len];
				byte[] temp = new byte[0];
                templen = netStream.Read(tempbytes, 0, tempbytes.Length);
                netlen += templen;
				temp = new byte[templen];
				Array.Copy(tempbytes,0,temp,0,temp.Length);
                bytes = bytes.Add(temp);
			//	Debug.Log("1111111111111");
                tempbytes = new byte[0];
            }
            if (netlen != 0)
            {
                byte[] ret = new byte[netlen];
                Array.Copy(bytes, 0, ret, 0, netlen);
                return ret;
            }
            return null;
        }
        else
        {
            netStream.Close();
            tcpClient.Close();
            return null;
        }
    }



  //  public ZMNetData readNetData()
  //  {
  //      if (netStream.CanRead)
  //      {
  //          try
  //          {
  //          // if (!netStream.DataAvailable)
  //          //     return null;
  //              int len = tcpClient.ReceiveBufferSize;
  //              if (len > 8192)
  //              {
  //                  len = 1448;
  //              }
  //              int netlen = 0;
  //              byte[] bytes = new byte[len];
  //              netlen = netStream.Read(bytes, 0, bytes.Length);
  //              while (netStream.DataAvailable && netlen == len)
  //              {
  //                  byte[] tempbytes = new byte[len];
  //                  netlen = netStream.Read(tempbytes, 0, tempbytes.Length);
  //                  bytes = bytes.Add(tempbytes);
  //                  tempbytes = new byte[0];
  //              }
  //              if (netlen != 0)
  //              {
  //                  byte[] ttt = GZipDecompress(bytes);
  //                  if (ttt != null)
  //                  {
  //                      ZMNetData ret = new ZMNetData(ttt);
  ////                      Debug.Log("数据长度（real）----------------------------------------len：" + ttt.Length + "---------压缩长度-----len：" + netlen);
  //                      ttt = new byte[0];
  //                      bytes = new byte[0];
  //                      return ret;
  //                  }
  //                  else
  //                  {
  //                      ttt = new byte[0];
  //                      bytes = new byte[0];
  //                      return null;
  //                  }
  //              }
  //              else {
  //                  return null;
  //              }
  //          }
  //          catch (Exception ex)
  //          {
  //              Debug.Log("CZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZCZ-------------------------"+ex);
  //              return null;
  //          }
  //      }
  //      else {
  //          netStream.Close();
  //          tcpClient.Close();
  //          return null;
  //      }
  //  }
           //    if (!netStream.DataAvailable)
           //        return null;
           //    int len = 512;
           //    int dataAdd = 0;
           //    byte[] bytes = new byte[len];
           //    int netlen = 0;
           //    byte[] templen = new byte[4];
           //    int temp  = 0;
           //    try
           //    {
           //        temp = netStream.Read(templen, 0, templen.Length);
           //    }
           //    catch (Exception ex){
           //        Debug.Log("CZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZCCCCCCCCCCCCCCCC=====================================LEN====" + ex);
           //        KDebug.WriteLog("CZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZCCCCCCCCCCCCCCCC=====================================LEN====" + ex);
           //    }
           //    int datalen = (int)getNumber(templen, 0, 4);
           // //   Debug.Log("data---len-------------------------------------"+datalen);
           //  if (datalen > 8192 || datalen <= 0)
           //  {
           //      Debug.Log("数据长度读取错误，抛弃这条数据-------------------------------------------"+datalen);
           //      KDebug.WriteLog("数据长度读取错误，抛弃这条数据-------------------------------------------"+datalen);
           //      templen = new byte[0];
           //      return null;
           //  }
           //
           //
           //    if (datalen <= len)
           //    {
           //        try
           //        {
           //            netlen = netStream.Read(bytes, 0, datalen);
           //        }
           //        catch (Exception ex)
           //         {
           //            Debug.Log("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY=====================================FFF====" + ex);
           //            KDebug.WriteLog("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY=====================================FFF====" + ex);
           //        }
           //    }
           //    else
           //    {
           //        dataAdd = datalen / len;
           //        try
           //        {
           //            netlen += netStream.Read(bytes, 0, len);
           //        }
           //        catch (Exception ex)
           //        {
           //            Debug.Log("pingjiepingjiepingjiepingjiepingjiepingjie=====================================one====" + ex);
           //            KDebug.WriteLog("pingjiepingjiepingjiepingjiepingjiepingjie=====================================one====" + ex);
           //        }
           //        /*            //经过测试，while执行效率与下部分的for循环的效率相差无几，所以哪个都行              
           //                      int num = 1;
           //                      while(dataAdd > 0)
           //                      {
           //                          byte[] tempByte = new byte[len];
           //                          if (dataAdd == 1)
           //                          {
           //                              netlen += netStream.Read(tempByte, 0, datalen - num * len);
           //                          }
           //                          else {
           //                              netlen += netStream.Read(tempByte, 0, len);
           //                          }
           //                          bytes = bytes.Add(tempByte);
           //                          tempByte = new byte[0];
           //                          num++;
           //                          dataAdd--;
           //                      }
           //      */
           //
           //        for (int i = 1; i <= dataAdd; i++)
           //        {
           //            byte[] tempbytes = new byte[len];
           //            if (i == dataAdd)
           //            {
           //                try
           //                {
           //                    netlen += netStream.Read(tempbytes, 0, datalen - i * len);
           //                }
           //                catch (Exception ex)
           //                {
           //                    Debug.Log("pingjiepingjiepingjiepingjiepingjiepingjie=====================================LAST====" + ex);
           //                    KDebug.WriteLog("pingjiepingjiepingjiepingjiepingjiepingjie=====================================LAST====" + ex);
           //                }
           //            }
           //            else
           //            {
           //                try
           //                {
           //                    netlen += netStream.Read(tempbytes, 0, len);
           //                }
           //                catch (Exception ex)
           //                {
           //                    Debug.Log("pingjiepingjiepingjiepingjiepingjiepingjie=====================================NEXT====" + ex);
           //                    KDebug.WriteLog("pingjiepingjiepingjiepingjiepingjiepingjie=====================================Next====" + ex);
           //                }
           //            }
           //            bytes = bytes.Add(tempbytes);
           //            tempbytes = new byte[0];
           //        }
           //
           //    }
           //     byte[] ttt = GZipDecompress(bytes);
           //     if (ttt != null)
           //     {
           //         ZMNetData ret = new ZMNetData(ttt);
           //         bytes = new byte[0];
           //         templen = new byte[0];
           //         return ret;
           //     }
           //     else
           //     {
           //         bytes = new byte[0];
           //         templen = new byte[0];
           //         return null;
           //     }
  //     }
  //     else {
  //         netStream.Close();
  //         tcpClient.Close();
  //         return null;
  //     }
  // }
        //    short opcode = (short)getNumber(bytes, 6, 2);
        //    Debug.Log("----------------------OPCODE:" + opcode);
           // while ((netlen = netStream.Read(bytes, 0, datalen)) != 0)
           // {
           //     Debug.Log("------------------len:"+netlen);
             //   byte[] ttt = GZipDecompress(bytes);
              //  short opcode = (short)getNumber(ttt, 6, 2);
                //    if (opcode == 184 || opcode == 165)
                //    {
            //    Debug.Log("收到未解压数据长度----------------" + netlen + "--------------解压后数据长度--------------" + ttt.Length + "----------------------OPCODE:" + opcode);
                //    }
          //  return new ZMNetData(ttt);
           // }
            /*
            while(netlen == 1448 && tcpClient.ReceiveBufferSize != 8192)
            {
                byte[] tempbytes = new byte[1448];
                netlen = netStream.Read(tempbytes, 0, tempbytes.Length);
                bytes = bytes.Add(tempbytes);
                tempbytes = new byte[0];
            }
             * */
        //    return null;
        // byte[] ttt = GZipDecompress(bytes);
        // short opcode = (short)getNumber(ttt, 6, 2);
        // //    if (opcode == 184 || opcode == 165)
        // //    {
        // Debug.Log("收到未解压数据长度----------------" + netlen + "--------------解压后数据长度--------------" + ttt.Length + "----------------------OPCODE:" + opcode);
        // //    }
        // return new ZMNetData(ttt);
   //     }
   //     else
   //     {
   //         netStream.Close();
   //         tcpClient.Close();
   //         return null;
   //     }
   // }
    /**********************************/
    /*
                if (tcpClient.ReceiveBufferSize == 8192)
                {
                    int len = tcpClient.ReceiveBufferSize;
                    byte[] bytes = new byte[len];

                    int netlen = netStream.Read(bytes, 0, bytes.Length);
                    // Debug.Log("len-------------------------------------------------------------------------------" + netlen);
                    byte[] headtest = new byte[2];
                    headtest = ZMNetData.readHead(bytes);
                    int count = (int)ZMNetData.getNumber(bytes, 2, 4);
                    if (headtest[0] == 90 && headtest[1] == 77 && count <= len)
                    {
                        MonoBehaviour.print("######8192#######ZMHEAD 成功！！！!!!#############");
                        ZMNetData data = new ZMNetData(bytes);
                        bytes = new byte[0];
                        return data;
                    }
                    else if (headtest[0] == 90 && headtest[1] == 77 && count > len)
                    {
                        int num = count / len;
                        int lenl;
                        for (int i = 0; i < num; i++)
                        {

                            byte[] tempbytes = new byte[len];

                            if (i == num - 1)
                            {
                                lenl = netStream.Read(tempbytes, 0, count - num * len);
                            }
                            else
                            {

                                lenl = netStream.Read(tempbytes, 0, len);
                            }
                            Debug.Log("to be contue--------------------------------------------------------------------" + lenl);
                            bytes = bytes.Add(tempbytes);
                        }
                        ZMNetData data = new ZMNetData(bytes);
                        bytes = new byte[0];
                        return data;
                    }
                    else
                    {
                        MonoBehaviour.print("######8192#######ZMHEAD Error!!!#############");
                        netStream.Close();
                        tcpClient.Close();

                        return null;
                    }
                }
                else
                {
                    int len = 1448;
                    byte[] bytes = new byte[len];

                    int netlen = netStream.Read(bytes, 0, bytes.Length);
                    byte[] headtest = new byte[2];
                    headtest = ZMNetData.readHead(bytes);
                    int count = (int)ZMNetData.getNumber(bytes, 2, 4);
                    if (headtest[0] == 90 && headtest[1] == 77 && count <= len)
                    {
                        MonoBehaviour.print("######1448#######ZMHEAD 成功！！！!!!#############");
                        ZMNetData data = new ZMNetData(bytes);
                        bytes = new byte[0];
                        return data;
                    }
                    else if (headtest[0] == 90 && headtest[1] == 77 && count > len)
                    {
                        int num = count / len;
                        int lenl;
                        for (int i = 0; i < num; i++)
                        {

                            byte[] tempbytes = new byte[len];

                            if (i == num - 1)
                            {
                                lenl = netStream.Read(tempbytes, 0, count - num * len);
                            }
                            else
                            {

                                lenl = netStream.Read(tempbytes, 0, len);
                            }
                            Debug.Log("to be contue--------------------------------------------------------------------" + lenl);
                            bytes = bytes.Add(tempbytes);
                        }
                        ZMNetData data = new ZMNetData(bytes);
                        bytes = new byte[0];
                        return data;
                    }
                    else
                    {
                        MonoBehaviour.print("#####1448########ZMHEAD Error!!!#############");
                        netStream.Close();
                        tcpClient.Close();

                        return null;
                    }
                }
       */

    /*
    byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
    netStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);
 //   ZMNetData data = new ZMNetData(bytes);
    byte[] headtest = new byte[2];
    headtest = ZMNetData.readHead(bytes);
    int count = (int)ZMNetData.getNumber(bytes, 2, 4);
    if (headtest[0] == 90 && headtest[1] == 77 && count <= tcpClient.ReceiveBufferSize)
    {
        Debug.Log(tcpClient.ReceiveBufferSize + "+++++++++++++++++++++++++++++");
        MonoBehaviour.print("#############ZMHEAD 成功！！！!!!#############");
        return new ZMNetData(bytes);
    }
    else if (headtest[0] == 90 && headtest[1] == 77 && count > tcpClient.ReceiveBufferSize)
    {
        int num = count / tcpClient.ReceiveBufferSize;

        Debug.Log("num======================" + num);
        for (int i = 0; i < num; i++)
        {
            byte[] tempbytes = new byte[tcpClient.ReceiveBufferSize];
            netStream.Read(tempbytes, 0, (int)tcpClient.ReceiveBufferSize);
            bytes = bytes.Add(tempbytes);
        }
        return new ZMNetData(bytes);
    }
    else
    {
     //   MonoBehaviour.print("#############ZMHEAD Error!!!#############");
        netStream.Close();
        tcpClient.Close();
                
        return null;
    }
            

}
else
{
    // MonoBehaviour.print("#############isNOTREAD#############");
    netStream.Close();
    tcpClient.Close();

    return null;
}
 * */


    /*byte[] head = new byte[2];
    if (readFull(_in, head) != head.Length)
    {
        return null;
    }
    for (int i = 0; i < HEAD_RECV.Length; i++)
    {
        if (HEAD[i] != head[i])
        {
            throw new Exception("Wrong protocol");
        }
    }
    byte[] lenInHead = null;
        
    switch(head[1]){
        case (byte)'A':
            lenInHead = new byte[4];
            break;
        case (byte)'B':
            lenInHead = new byte[2];
            break;
        case (byte)'C':
            lenInHead = new byte[1];
            break;
        default:
            throw new IOException("Wrong protocol");
    }
        
    if(readFull(_in, lenInHead) != lenInHead.Length){
        return null;
    }

    int len = 0;
        
    switch(head[1]){
        case (byte)'A':
            len = (int)getNumber(lenInHead, 0, 4);
            break;
        case (byte)'B':
            len = (int)(getNumber(lenInHead, 0, 2) & 0xFFFF);
            break;
        case (byte)'C':
            len = (int)(getNumber(lenInHead, 0, 1) & 0xFF);
            break;
    }

    len -= head.Length + lenInHead.Length;
    byte[] buf = null;

    if(len > 0){
        buf = new byte[len];

        if(readFull(_in, buf) != len){
            throw new Exception("Not enough input");
        }   
    }

    ZMNetData data = new ZMNetData(buf);
    return data;*/
    //   }

    //public static int getNumber(byte[] buf, int off, int len)
    //{
    //    byte[] bytes = new byte[4];
    //    Array.Copy(buf, off, bytes, 0, len);

    //    byte[] temp = new byte[4];
    //    temp[0] = bytes[3];
    //    temp[1] = bytes[2];
    //    temp[2] = bytes[1];
    //    temp[3] = bytes[0];
    //    int ret = System.BitConverter.ToInt32(temp, 0);
    //    bytes = new byte[0];
    //    temp = new byte[0];
    //    //   Debug.Log("--------------------------------------getnumber:"+ret);
    //    return ret;
    //}
    public static long getNumber(byte[] buf, int off, int len)
    {
        long l = 0;

        for (int i = 0; i < len; i++)
        {
            l <<= 8;
            l |= (buf[off + i]) & 0xff;
        }

        return l;
    }

    protected static int readFull(NetworkStream inputStream, byte[] buf)
    {
        int len = 0;

        try
        {
            while (len < buf.Length)
            {
                int l = inputStream.Read(buf, len, buf.Length - len);

                if (l < 0)
                {
                    throw new Exception("Wrong protocol");
                }

                len += l;
            }
        }
        finally
        {
            for (int i = len; i < buf.Length; i++)
            {
                buf[i] = 0;
            }
        }

        return len;
    }

    public void tryReconnect()
    {
        // 试图重新登录
        ZealmConnector.tryReconnect();
    }

    public void close()
    {
        IsConnected = false;

        if (_reader != null)
        {
            try
            {
                _reader.close();
            }
            catch (IOException ex2)
            {
            }
        }

        if (_writer != null)
        {
            try
            {
                _writer.close();
            }
            catch (IOException ex3)
            {
            }
        }



        _reader = null;
        _writer = null;
        if (netStream != null)
            netStream.Close();
        if (tcpClient != null)
            tcpClient.Close();

    }

}
