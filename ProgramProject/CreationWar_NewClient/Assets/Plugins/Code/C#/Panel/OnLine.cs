using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using yuan.YuanPhoton;
using System.IO;
using System.Text;
public class OnLine : MonoBehaviour {

	public static OnLine my;
    public MainMenuManage mmManage;
    public TableRead tableRead;
	public string serverAddress=string.Empty;
	public string serverAddress2=string.Empty;
	public List<GameObject> listBtnServer=new List<GameObject>();
	
	public static bool StartGame=true;

	void Awake()
	{
		my=this;
	}

	void Start()
	{
		Localization.StaticLoc.FixBrokenWord();
		Debug.Log ("------"+SystemInfo.deviceUniqueIdentifier);
		if(PlayerPrefs.GetString ("Language")=="")
		{
			
		}
		else
		{
			YuanUnityPhoton.LanguageVersion=PlayerPrefs.GetString ("Language","CH");
		}
		//CMGE ();
		//ConChina();
		Debug.Log("ready connect server");
		if(IsConnectByFile())
		{
			return;
		}	
		tableRead.TimeOut();
		//ConHK222 ();
       // Htest();
        //ConZhangTian();
	//Htest ();
        //ConWeiwei();
                              //ConZeamlTest ();
		ConMy ();
		//Linux ();
//		ConZheng();
//		StartCoroutine (ConHttp());
	}
	
	bool IsConnectByFile()
	{

        if (File.Exists("udp"))
        {



            PhotonHandler.SetUpdMode();


        }
		

		if(File.Exists("johnconnect.txt"))
		{
			
			StreamReader t_objReader = new StreamReader("johnconnect.txt");
			serverAddress=t_objReader.ReadLine();
			PhotonHandler.ShowLog(string.Format("file exits:{0}",serverAddress));
			StartCoroutine(Connect());
			
			
			return true;
		}


   
		
		Debug.Log ("file not exist");
		
		return false;
	}

	public IEnumerator ConHttp()
	{
		tableRead.strInfo=StaticLoc.Loc.Get("meg0094");
		WWW www=null;
		try
		{
#if UNITY_IOS
#if SDK_ZSYIOS
			www = new WWW(@"http://221.229.162.251:8080/cp/AppleServerIP.html");
#else
			www = new WWW(@"http://221.229.162.251:8080/cp/1.2.6/ServerIP.html");
#endif
			//			www = new WWW(@"http://221.229.162.251:8080/cp/ServerIP.html");
#elif UNITY_ANDROID
			www = new WWW(@"http://221.229.162.251:8080/cp/1.2.6Android/ServerIP.html");
#else
			www = new WWW(@"http://221.229.162.251:8080/cp/1.2.6/ServerIP.html");
#endif
		}
		catch(System.Exception ex)
		{
			
			tableRead.strInfo=StaticLoc.Loc.Get("meg0102");
			//ShowConnectFail(StaticLoc.Loc.Get("meg0102")+StaticLoc.Loc.Get("meg0103"));

			TableRead.my.ReadTimeOut (StaticLoc.Loc.Get("meg0102")+StaticLoc.Loc.Get("meg0103"));
			Debug.LogError (ex.ToString());
		}
			yield return www;
			//Debug.Log ("---------------------------"+www.text);
		try
		{
			string[] ips=www.text.Split(',');
			serverAddress = ips[0].Trim ();
			serverAddress2=ips[1].Trim();

			Encoding utf8=System.Text.Encoding.UTF8;
			Encoding unicode=System.Text.Encoding.Unicode;

			byte[] bytes=utf8.GetBytes(serverAddress);
			byte[] encoding=System.Text.Encoding.Convert(utf8,unicode,bytes);
			serverAddress=unicode.GetString(encoding);

			bytes=utf8.GetBytes(serverAddress2);
			encoding=System.Text.Encoding.Convert(utf8,unicode,bytes);
			serverAddress2=unicode.GetString(encoding);

			Debug.Log ("---------------------serverAddress:"+serverAddress);
			Debug.Log ("---------------------serverAddress2:"+serverAddress2);
			StartCoroutine(Connect());
		}
		catch(System.Exception ex)
		{
			
			tableRead.strInfo=StaticLoc.Loc.Get("meg0102");
			//ShowConnectFail(StaticLoc.Loc.Get("meg0102")+StaticLoc.Loc.Get("meg0103"));
			TableRead.my.ReadTimeOut (StaticLoc.Loc.Get("meg0102")+StaticLoc.Loc.Get("meg0103"));
			Debug.LogError (ex.ToString());
		}


	}

	public void ConZhangTian()
	{
        serverAddress = "192.168.1.58:9998";
        serverAddress2 = "192.168.1.58:9998";
		StartCoroutine(Connect());
	}

	public void ConZeamlTest()
	{
		serverAddress = "221.229.162.251:5059";
		StartCoroutine(Connect());
	}
	
	public void ConZheng (){
		serverAddress = "192.168.1.82:9998";
		serverAddress2="192.168.1.82:9998";
		StartCoroutine(Connect());
	}
	
	public void ConWeiwei (){
        serverAddress = "192.168.1.48:9998";
        serverAddress2 = "192.168.1.48:9998";
		StartCoroutine(Connect());
	}
	
	public void Htest (){
		serverAddress = "211.155.91.188:5059";
		StartCoroutine(Connect());
	}

	public void CMGE(){
		serverAddress = "211.155.91.187:5059";
		StartCoroutine(Connect());
	}

	public void ConLocal(){
		serverAddress = "192.168.1.101:5059";
		StartCoroutine(Connect());
	}

    public void ConTest()
    {
        serverAddress = "211.155.91.188:5059";
        StartCoroutine(Connect());
    }	

    public void ConMy()
    {
        serverAddress = "115.29.36.226:9998";
        serverAddress2 = "115.29.36.226:9998";
        StartCoroutine(Connect());
    }

	public void Linux()
	{
		
		serverAddress = "58.218.201.34:9999";
		//serverAddress = "122.192.154.34:9998";
		//serverAddress2="122.192.154.34:9998";
		serverAddress2="58.218.201.34:9999";
		StartCoroutine(Connect());
	}
	
	public void ConChina()
	{   
		serverAddress="117.131.207.219:5059";
		StartCoroutine (Connect ());
	}
	
	public void ConHK()
	{   
		serverAddress="203.169.186.164:5057";
		StartCoroutine (Connect ());
	}
	
	public void ConHK222()
	{
		serverAddress="124.248.228.222:5059";
		StartCoroutine (Connect ());
	}
	
	public void ConChina86()
	{
		serverAddress="58.218.200.86:5059";
		StartCoroutine (Connect ());
	}

	public void ReOnline()
	{
		Application.LoadLevel(0);
	}

	public void ShowConnectFail( string mText)
	{
		MainMenuManage.my.warnings.warningAllEnter.btnEnter.target=this.gameObject;
		MainMenuManage.my.warnings.warningAllEnter.btnEnter.functionName="ReOnline";
		MainMenuManage.my.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), mText);
		StopAllCoroutines();
	}


    IEnumerator Connect()
    {
      //  serverAddress = serverAddress2;
		tableRead.strInfo=StaticLoc.Loc.Get("meg0101");
		Ping ping=new Ping(serverAddress.Split (':')[0]);
		Ping ping2=new Ping(serverAddress2.Split (':')[0]);
		int timeOut=0;

        int p1 = 0;
        int p2 = 0;
		while(true)
		{
			if(ping.isDone||ping2.isDone)
			{
				break;
			}
			if(timeOut>30)
			{
				break;
			}

			timeOut++;
			Debug.Log (string.Format("---------------------Ping:{0},{1};Ping2:{2},{3}",ping.isDone,ping.time,ping2.isDone,ping2.time));
			yield return new WaitForSeconds(0.1f);

		}
        p1 = ping.time;
        p2 = ping2.time;
        Debug.Log(string.Format("---------------------PingEnd:{0},{1};Ping2:{2},{3}", ping.isDone, p1, ping2.isDone, p2));
        if (p1 == -1 && p2 == -1)
		{
			tableRead.strInfo=StaticLoc.Loc.Get("meg0096");
			//ShowConnectFail(StaticLoc.Loc.Get("meg0096")+StaticLoc.Loc.Get("meg0103"));

			tableRead.ReadTimeOut (StaticLoc.Loc.Get("meg0096")+StaticLoc.Loc.Get("meg0103"));
		}
        else if (p2 != -1 && p1 != -1)
        {
            if (p1 > p2)
            {
                serverAddress = serverAddress2;
            }
        }
        else if (p2 != -1)
        {
            serverAddress = serverAddress2;
        }

		ping.DestroyPing();
		ping2.DestroyPing();

		Debug.Log ("===============ConnectIP:"+serverAddress);
		tableRead.strInfo=StaticLoc.Loc.Get("meg0095");
		//NGUIDebug.Log ("--------------------------RadyConnect");
		yuan.YuanClass.SwitchList (listBtnServer,false,true);
//		if(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected)
//		{    
//
//        	//YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Disconnect();
//			ZealmConnector.closeConnection();
//			while(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerConnected)
//			{
//				yield return new WaitForSeconds(0.1f);
//			}		
//		}
//
//			if(InRoom.GetInRoomInstantiate ().ServerConnected)
//			{   
//		    	//InRoom.GetInRoomInstantiate().peer.Disconnect();
//			ZealmConnector.closeConnection();
//				while(InRoom.GetInRoomInstantiate ().ServerConnected)
//				{
//					yield return new WaitForSeconds(0.1f);
//				}				
//			}
		 
			ZealmConnector.closeConnection();
		//try
		//{   
			//NGUIDebug.Log ("-------------------------------IP:"+serverAddress);
			YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = serverAddress;
//	        YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = "117.131.207.219" + ":5059";
//        YuanUnityPhoton.NewYuanUnityPhotonInstantiate().ServerAddress = "192.168.1.100" + ":5059";
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ServerApplication = "YuanPhotonServerRoom";
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage = this.mmManage;
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead = this.tableRead;
			//NGUIDebug.Log ("--------------------------StratConnect");
			
	        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().Connect();
			PhotonHandler.ShowLog("SetConnectionIP:"+serverAddress);
			PlayerPrefs.SetString ("ConnectionIP",serverAddress);
			while(!YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().ServerConnected)
			{
				yield return new WaitForSeconds(0.1f);
			}
			//NGUIDebug.Log ("--------------------------Connected");
			if(StartGame)
			{
				StartGame=false;
				YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SetPlayerBehavior (yuan.YuanPhoton.ConsumptionType.GameSchedule,((int)GameScheduleType.OpenGame).ToString (),SystemInfo.deviceUniqueIdentifier);
			}
//			Debug.Log("Con2222222");
		//}
		//catch(System.Exception ex)
		//{
			//Debug.LogError (ex.ToString ());
		//}
/**************************************************	
	//	if(InRoom.GetInRoomInstantiate ().peer.PeerState==ExitGames.Client.Photon.PeerStateValue.Connected)
	//	{   
    //    	InRoom.GetInRoomInstantiate().peer.Disconnect();
	//		while(InRoom.GetInRoomInstantiate ().peer.PeerState!=ExitGames.Client.Photon.PeerStateValue.Disconnected)
	//		{
	//			yield return new WaitForSeconds(0.1f);
	//		}				
	//	}
*******************************************************/
		try
		{
	    	InRoom.NewInRoomInstantiate();
		}
		catch(System.Exception ex)
		{
			Debug.LogError (ex.ToString ());
		}
		PhotonNetwork.Disconnect();
    }
}
