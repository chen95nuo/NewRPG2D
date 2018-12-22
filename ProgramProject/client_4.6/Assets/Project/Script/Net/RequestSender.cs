using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System;
using System.Threading;
using System.IO;
using LitJson;

/**发送请求器**/
public class RequestSender {
	
	private static RequestSender instance;
	/**密钥长度**/
	private const int KeyLength=24;
	private string key;
	private WebClient client;
	private WebClient clientSpecial;
	/**每次请求的最大请求次数**/
	private const int MaxRequestTimes=2;
	/**已经进行的请求次数**/
	private int requestTimes;
	/**请求明细**/
	private string url;
	private string param;
	public static string serverIp; 
	public static string serverPort;
	/**本次请求开始时间**/
	private long startTime;
	/**超时时长(毫秒)**/
	private const long TimeOut=10*1000;
	private static bool threadRunning=true;
	/**等待服务器应答的对象**/
	private ProcessResponse pr;
	private ProcessResponse prSpecial;
	private bool needEncriptAndCompress;
	private bool lastRequestOver=true;
	
	// Use this for initialization
	private RequestSender () 
	{
		client=new WebClient();
		client.Encoding=Encoding.GetEncoding("UTF-8");
		client.Credentials = CredentialCache.DefaultCredentials;
		client.UploadStringCompleted+=new UploadStringCompletedEventHandler(uploadStringCompleted);
		/**判断超时线程**/
		ThreadStart ts=new ThreadStart(checkTime);
		Thread t=new Thread(ts);
		t.Start();
		
		clientSpecial=new WebClient();
		clientSpecial.Encoding=Encoding.GetEncoding("UTF-8");
		clientSpecial.Credentials = CredentialCache.DefaultCredentials;
		clientSpecial.UploadStringCompleted+=new UploadStringCompletedEventHandler(uploadStringCompletedSpecial);
	}
	
	public static RequestSender getInstance()
	{
		if(instance==null)
		{
			instance=new RequestSender();
		}
		return instance;
	}
	
	/**判断超时**/
	public void checkTime()
	{
		while(threadRunning)
		{
			if(client.IsBusy && GameObjectUtil.getCurTime()-startTime>=TimeOut)
			{
				client.CancelAsync();
				Debug.Log("result:请求超时");
			}
			Thread.Sleep(100);
		}
	}
	
	public bool request(int commandId,string param,bool needEncriptAndCompress,ProcessResponse pr)
	{
		while(!lastRequestOver)
		{
			Thread.Sleep(100);
		}
		if(client.IsBusy)
		{
			Debug.Log("正在请求,请等待:"+commandId+",pr:"+pr+",param:"+param);
			return false;
		}
		CommandData cd=CommandData.getData(commandId);
		if(cd==null)
		{
			Debug.Log("command null");
			pr.receiveResponse(null);
			return false;
		}
		//重置//
		requestTimes=0;
		this.needEncriptAndCompress=needEncriptAndCompress;
		this.pr=pr;
		//设置请求时间//
		startTime=GameObjectUtil.getCurTime();
		//设置请求明细//
		this.url=(cd.urlPrefix+cd.urlSuffix).Replace("localhost",serverIp).Replace("8080",serverPort);
		//Debug.Log("url:"+url);
		this.param=param;
		requestAsync();
		return true;
	}
	
	//无需加密解密的请求//
	public bool request(int commandId,string param,ProcessResponse prSpecial)
	{
		
		CommandData cd=CommandData.getData(commandId);
		if(cd==null)
		{
			Debug.Log("command null");
			pr.receiveResponse(null);
			return false;
		}
		//重置//
		requestTimes=0;
		this.prSpecial=prSpecial;
		//设置请求时间//
		startTime=GameObjectUtil.getCurTime();
		//设置请求明细//
		this.url=(cd.urlPrefix+cd.urlSuffix).Replace("118.25.209.26",serverIp).Replace("8080",serverPort);
		//Debug.Log("url:"+url);
		this.param=param;
		requestAsyncSpecial();
		return true;
	}
	
	public bool request(string url,string param,bool needEncriptAndCompress,ProcessResponse pr)
	{
		while(!lastRequestOver)
		{
			Thread.Sleep(100);
		}
		if(client.IsBusy)
		{
			Debug.Log("正在请求,请等待:"+url+",pr:"+pr+",param:"+param);
			return false;
		}
		//重置//
		requestTimes=0;
		this.needEncriptAndCompress=needEncriptAndCompress;
		this.pr=pr;
		//设置请求时间//
		startTime=GameObjectUtil.getCurTime();
		//设置请求明细//
		this.url=url.Replace("118.25.209.26",serverIp).Replace("8080",serverPort);
		//Debug.Log("url:"+url);
		this.param=param;
		requestAsync();
		return true;
	}
	
	/**异步请求**/
	private void requestAsync()
	{
		try
		{
			requestTimes++;
			string msg="";
			if(needEncriptAndCompress)
			{
				//压缩//
				string info=StringUtil.compressByGZIP(param);
				//加密//
				string cipher=EncryptAndDecipher.encrypt2(key, info);
				msg=cipher;
			}
			else
			{
				msg=param;
			}
			//请求//
            //Debug.Log("mess request:" + param);
			client.UploadStringAsync(new Uri(url),"POST",msg);
			//记录流量//
			PlayerInfo.getInstance().recordFlowInfo(1,msg);
		}
		catch(Exception e)
		{
			Debug.Log("requestAsync error:"+e.Message);
			parseResponseJson(null);
		}
		finally
		{
			client.Dispose();
		}
	}
	
	/**无需加密解密的异步请求**/
	private void requestAsyncSpecial()
	{
		try
		{
			requestTimes++;
			string msg="";
			//压缩//
			string info=StringUtil.compressByGZIP(param);
			msg=info;
			
			//请求//
            Debug.Log("mess special request:" + param);
			clientSpecial.UploadStringAsync(new Uri(url),"POST",msg);
			//记录流量//
			PlayerInfo.getInstance().recordFlowInfo(1,msg);
		}
		catch(Exception e)
		{
			Debug.Log("requestAsync error:"+e.Message);
			parseResponseJsonSpecial(null);
		}
		finally
		{
			clientSpecial.Dispose();
		}
	}
	
	private void uploadStringCompleted(object sender,UploadStringCompletedEventArgs args)
	{
		if(args.Error==null)
		{
			try
			{
				//获取返回信息//
				string responseBody=args.Result;
				//记录流量//
				PlayerInfo.getInstance().recordFlowInfo(2,responseBody);
				setCookie();
				//处理//
				string json="";
				if(needEncriptAndCompress)
				{
					json=parseResponseBody(responseBody);
				}
				else
				{
					json=responseBody;
				}
				//解析//
				if(json!=null && json!="")
				{
					parseResponseJson(json);
				}
				else
				{
					if(requestTimes<MaxRequestTimes)
					{
						requestAsync();
					}
					else
					{
						parseResponseJson(null);
					}
				}
			}
			catch(Exception e)
			{
				Debug.Log("error:"+e.StackTrace);
				parseResponseJson(null);
			}
		}
		else
		{
			Debug.Log("error:"+args.Error.Message);
			parseResponseJson(null);
		}
	}
	
	private void uploadStringCompletedSpecial(object sender,UploadStringCompletedEventArgs args)
	{
		if(args.Error==null)
		{
			try
			{
				//获取返回信息//
				string responseBody=args.Result;
				//记录流量//
				PlayerInfo.getInstance().recordFlowInfo(2,responseBody);
				//处理//
				string json="";
				json=parseResponseBodySpecial(responseBody);
				//json=responseBody;
				//解析//
				if(json!=null && json!="")
				{
					parseResponseJsonSpecial(json);
				}
				else
				{
					if(requestTimes<MaxRequestTimes)
					{
						requestAsyncSpecial();
					}
					else
					{
						parseResponseJsonSpecial(null);
					}
				}
			}
			catch(Exception e)
			{
				Debug.Log("error:"+e.StackTrace);
				parseResponseJsonSpecial(null);
			}
		}
		else
		{
			Debug.Log("error:"+args.Error.Message);
			parseResponseJsonSpecial(null);
		}
	}
	
	/**
	 * 设置新密钥,获取服务器返回数据
	 * lt@2013-8-21 上午10:39:05
	 * @param responseBody
	 * @return
	 */
	private string parseResponseBody(string responseBody)
	{
		if(responseBody==null)
		{
			return null;
		}
		if(responseBody.Length==KeyLength)
		{
			key=responseBody;
			return null;
		}
		else if(responseBody.Length<KeyLength)
		{
			Debug.Log("response error:"+responseBody);
			return null;
		}
		string newkey=responseBody.Substring(responseBody.Length-KeyLength);
		string msg=responseBody.Substring(0,responseBody.Length-KeyLength);
		//旧密钥解密//
		msg=EncryptAndDecipher.decipher2(key, msg);
		//解压缩//
		msg=StringUtil.uncompressByGZIP(msg);
        //Debug.Log("mess receive:" + msg);
		key=newkey;
		return msg;
	}
	
	//对无需加密解密的返回做解压缩//
	private string parseResponseBodySpecial(string responseBody)
	{
		if(responseBody==null)
		{
			return null;
		}
		
		//解压缩//
		string msg=StringUtil.uncompressByGZIP(responseBody);
        Debug.Log("mess special receive:" + msg);
		return msg;
	}
	
	private void setCookie()
	{
		if(client.Headers.GetValues("Cookie")==null)
		{
			string cookie=client.ResponseHeaders.Get("Set-Cookie");
			client.Headers.Add("Cookie", cookie);
		}
	}
	
	public void clearCookie()
	{
		client.Headers.Remove("Cookie");
	}
	
	private void parseResponseJson(string json)
	{
		lastRequestOver=false;
		try
		{
			if(json==null)
			{
				PlayerInfo.getInstance().timeout=true;
				//==进入游戏特殊处理,要有弹窗提示==//
				if(pr.GetType().Equals(typeof(LoadingControl)))
				{
					((LoadingControl)pr).netFail(1);
				}
			}
			else
			{
				//Debug.Log("json  111 : "  + json);
				int errorCodeNum = 0;
				string[] ss = json.Split(',');
				for(int i = 0; i < ss.Length; ++i)
				{
					if(ss[i].Contains("errorCode"))
					{
						string[] ss1 = ss[i].Split(':');
						ss1[1] = ss1[1].Replace("}","");
						//Debug.Log("ss1[1] :" + ss1[1]);
						errorCodeNum = StringUtil.getInt(ss1[1]);
					}
				}
				if(errorCodeNum == -3)
				{
					ToastWindow.closeType = -3;
					ToastWindow.needShow = true;
				}
			
				pr.receiveResponse(json);
				if(!pr.GetType().Equals(typeof(ConnectLockedManager)))
				{
					PlayerInfo.getInstance().CleanRequest();
				}
				pr = null;
			}
		}
		catch(Exception e)
		{
			Debug.Log(e.Message);
			Debug.Log(e.StackTrace);
			PlayerInfo.getInstance().timeout=true;
		}
		lastRequestOver=true;
	}
	
	private void parseResponseJsonSpecial(string json)
	{
		lastRequestOver=false;
		try
		{
			if(json==null)
			{
				PlayerInfo.getInstance().timeout=true;
			}
			else
			{
				prSpecial.receiveResponse(json);
				if(!prSpecial.GetType().Equals(typeof(ConnectLockedManager)))
				{
					PlayerInfo.getInstance().CleanRequest();
				}
				prSpecial = null;
			}
		}
		catch(Exception e)
		{
			Debug.Log(e.Message);
			Debug.Log(e.StackTrace);
			PlayerInfo.getInstance().timeout=true;
		}
		lastRequestOver=true;
	}
	
//	private void writeCommand(string url,string param)
//	{
//		string directory="D:/unityLog";
//		string path="D:/unityLog/command_"+System.DateTime.Now.ToString("yyyy-MM-dd")+".log";
//		if(!Directory.Exists(directory))
//		{
//			Directory.CreateDirectory(directory);
//		}
//		if(!File.Exists(path))
//		{
//			using(StreamWriter sw=File.CreateText(path))
//			{
//				//sw.Encoding=System.Text.Encoding.UTF8;
//				//sw.WriteLine("commands:");
//			}
//		}
//		using(StreamWriter sw=File.AppendText(path))
//		{
//			Debug.Log("url:"+url);
//			sw.WriteLine(url);
//			Debug.Log("param:"+param);
//			sw.WriteLine(param);
//		}
//	}
}
