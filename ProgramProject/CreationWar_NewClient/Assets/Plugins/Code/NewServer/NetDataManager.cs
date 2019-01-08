using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;


    class NetDataManager
    {
        public static List<ZMNetData> netDatas = new List<ZMNetData>();
        public static ZMNetData nextPacket;
        public static List<byte[]> bytenetDatas = new List<byte[]>();
        public static TcpClient testclient = new TcpClient();

		public static List<short> tmpList = new List<short>();
        /**
        * 保存服务器回传的包。
        * 
        * @param segment 服务器回传的包。
        */
        public static void addNetData(byte[] netData)
        {
            bytenetDatas.Add(netData);
        }
		public static void rollreadData()
		{
		int count = bytenetDatas.Count;
		while (count-- > 0)
		{
			try
			{
				byte[] bytes = bytenetDatas[0];
				bytenetDatas.RemoveAt(0);
				int integer = 4;
				int len = 0;
				int Experimental = 0;
				int datalen = 0;
				ZMNetData netData;
				byte[] ret = new byte[0];
				do
				{
					Experimental = (int)getNumber(bytes, len, integer);
					len += integer;
					datalen = (int)getNumber(bytes, len, integer);
					len += integer;
					byte[] data;
					if (((Experimental == -253 || Experimental == -254) && datalen > bytes.Length - len))
					{
						int roll = bytes.Length - len;
						if(bytenetDatas.Count==0)
						{
					//		Debug.Log("sleep-----------------------------------");
							System.Threading.Thread.Sleep(100);
						}
						while (bytenetDatas.Count > 0 && datalen != roll)
						{
							byte[] temps = bytenetDatas[0];
							roll += temps.Length;
							if (roll > datalen)
							{
								byte[] temp = bytenetDatas[0];
								int lennext = datalen - (bytes.Length - len);
								byte[] ttt = new byte[lennext];
								Array.Copy(temp, 0, ttt, 0, lennext);
								byte[] next = new byte[temp.Length - lennext];
								Array.Copy(temp, lennext, next, 0, next.Length);
								bytenetDatas[0] = next;
								bytes = bytes.Add(ttt);
						//		Debug.Log("22222222222222");

								roll = bytes.Length - len;
							}
							else
							{
								bytes = bytes.Add(temps);
						//		Debug.Log("33333333333333");

								bytenetDatas.RemoveAt(0);
								count--;
							}
							if(bytenetDatas.Count==0&&datalen != roll)
							{
						//		Debug.Log("sleep-------------sleep----------------------");
								System.Threading.Thread.Sleep(100);
							}
						}
					}
					if (Experimental == -253)
					{
						data = new byte[datalen];
						Array.Copy(bytes, len, data, 0, datalen);
						ret = new byte[data.Length];
						ret = data;
					}
					else if (Experimental == -254)
					{
						data = new byte[datalen];
						Array.Copy(bytes, len, data, 0, datalen);
						ret = GZipDecompress(data);
					}
					if (ret != null && ret[0] == 90 && ret[1] == 77)
					{
						netData = new ZMNetData(ret);
						if(netData.type == (short)73)
						{
							Debug.Log("1111111111111111111111111111111          "+ System.DateTime.Now.ToString());
						}
						netDatas.Add(netData);
					}
					len += datalen;
					if (bytes.Length - 8 >= len)
					{
						Experimental = (int)getNumber(bytes, len, integer);
					}
					else
					{
						Experimental = 0;
					}
				} while (Experimental == -253 || Experimental == -254);
				//                    bytenetDatas.RemoveAt(0);
			}
			catch (Exception ex)
			{
				//    bytenetDatas.RemoveAt(0);
				//    System.Windows.Forms.MessageBox.Show(ex.ToString());
			}
		}
	}

	
	
	public static void update()
	{
        tmpList.Clear();
		DateTime t1 = DateTime.Now;
		int count = netDatas.Count;
		for (int i = 0; i < count; i++)
		{

			try
			{
				ZMNetData netData = (ZMNetData)netDatas[0];
				tmpList.Add(netData.type);
				netDatas.RemoveAt(0);
				handleNetData(netData);
			}
			catch (Exception e)
			{
				//              Debug.Log(e + "------------------------------------------------------------------------------------NetDataManager-");
			}
		}
		DateTime t2 = DateTime.Now;
		TimeSpan ts1=t2-t1;

		if(ts1.Milliseconds > 5)
		{
			String s = "";
			for (int i = 0; i < tmpList.Count; i++) {
				s = s +tmpList[i]+",";
			}
            //if(s != "")
            //{
                //Debug.Log("--------------------ClientSessionUpdate too LONG-------------------" + ts1.Milliseconds + " opcode :" + s);
//                KDebug.WriteLog("--------------------ClientSessionUpdate too LONG-------------------" + ts1.Milliseconds + " opcode :" + s);
            //}
			

		}

	}
	
	public static event Action<Zealm.OperationResponse> DataHandlePhoton;
	public static event Action<Zealm.OperationResponse> InRoomHandle;
	public static event Action<ZMNetData> DataHandle;
	
	public static void SetInRoomHandle(Action<Zealm.OperationResponse> op)
	{
		InRoomHandle=op;
	}
	/**
     * 处理一个网络包。
     * @param segment
     */
	public static void handleNetData(ZMNetData netData)
	{
		//     Debug.Log("opcode  handle -----------------------------------------------------"+netData.type);
		try
		{
			ZMNetData cloneDt=netData.Clone () as ZMNetData;
			if(netData.type == (short)73)
			{
				Debug.Log("2222222222222222222222222222222222          "+ System.DateTime.Now.ToString());
			}
			if(netData.isnot==2)
			{
				if(DataHandle!=null)
				{
					DataHandle(cloneDt);
				}
			}
			if(netData.isnot==1)
			{
				if(DataHandlePhoton!=null)
				{
					
					int tryGet=netData.readInt();
					if(tryGet==-255)
					{
						short mRetrunCode=netData.readShort ();
						string mDebugMessage=netData.readString ();
						Dictionary<short,object> dicParms=netData.getMapBO ();
						Zealm.OperationResponse op=new Zealm.OperationResponse(netData.type,mRetrunCode,dicParms,mDebugMessage);
						//            Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++"+op.OperationCode);
						DataHandlePhoton(op);
						if(InRoomHandle!=null)
						{
							InRoomHandle(op);
						}
					}
				}
				}
			}
			catch(System.Exception ex)
		    {
			    Debug.LogError(ex.ToString ());
		    }
            nextPacket = netData;

            try{
                if (!netData.handled)
                {
                    netData.reset();
//                    world.processPacket();
                }

            }catch(Exception e){
        	//Log.exception(e);
            }finally{
                nextPacket = null;
            }
        }

        //public static int byteToInt(byte[] b) 
        //{  
        //    int s = 0;   
        //    for (int i = 0; i < 3; i++) 
        //    {     
        //        if (b >= 0)
        //            s = s + b;
        //        else          
        //            s = s + 256 + b;     
        //        s = s * 256;  
        //    }   
        //    if (b[3] >= 0)    //最后一个之所以不乘，是因为可能会溢出     
        //        s = s + b[3];  
        //    else      
        //        s = s + 256 + b[3];  
        //    return s;     
        //}  

        //public static int getNumber(byte[] buf, int off, int len)
        //{
        //    byte[] bytes = new byte[4];
        //    Array.Copy(buf,off,bytes,0,len);

        //    byte[] temp = new byte[4];
        //    temp[0] = bytes[3];
        //    temp[1] = bytes[2];
        //    temp[2] = bytes[1];
        //    temp[3] = bytes[0];
        //    int ret = System.BitConverter.ToInt32(temp, 0);
        //    bytes = new byte[0];
        //    temp = new byte[0];
        // //   Debug.Log("--------------------------------------getnumber:"+ret);
        //    return ret;
        //}

        public static long getNumber(byte[] buf, int off, int len)
        {
            long longVal = 0;

            for (int i = 0; i < len; i++)
            {
                longVal <<= 8;
                longVal |= (buf[off + i]) & 0xff;
            }

            return longVal;
        }

        /// GZip解压函数
        public static byte[] GZipDecompress(byte[] data)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                Stream gZipStream = new GZipInputStream(new MemoryStream(data));
                byte[] bytes = new byte[10240];
                int count = 0;
                while ((count = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    stream.Write(bytes, 0, count);
                }
                gZipStream.Close();

                byte[] re = stream.ToArray();
                stream.Close();

                byte[] retu = new byte[re.Length - 27];
                Array.Copy(re, 27, retu, 0, retu.Length);
                bytes = new byte[0];
                return retu;
            }
            catch (Exception ex)
            {
                //          KDebug.Log("GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGzIP----------IS-----------ERROR--------"+ex);
                return null;
            }
        }
    }

