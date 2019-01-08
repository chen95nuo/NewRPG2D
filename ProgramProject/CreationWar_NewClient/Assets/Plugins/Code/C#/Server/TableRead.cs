using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.IO;

public class TableRead : MonoBehaviour
{
    public static TableRead my;
    public bool canReadTable = false;
    public bool isReadingTable = false;
    public bool isReadTimeOutEnd = false;
    public string applicationPath;
    public UISlider sliderTable;
    public UISlider sliderStart;
    public UILabel lblOnline;
    public UILabel lblGameVersion;
    public GameObject btnReonline;
    public GameObject objBtns;
    public GameObject online;
    public GameObject sdklogin;
    [HideInInspector]
    public bool isReadEnd = false;
    public float readNum;
    public float maxNum;
    public wirtelog objWirtelog;
    public bool isOnlineFiled = false;
    public bool isNeedUpdate = false;
    public bool isPlayerMax = false;
	public bool isSerializationFail=false;
	public bool isConnectFail=false;

    public string strInfo;

    public string StrInfo
    {
        get { return strInfo; }
        set
        {
            strInfo = value;
            if (lblOnline != null)
            {
                lblOnline.text = StrInfo;
            }
        }
    }

    WWW download;
    System.Threading.Thread dataTherad;
    byte[] myByte;


    private Dictionary<string, string> LocalResVersion;
    private Dictionary<string, string> ServerResVersion;
    private List<string> NeedDownFiles;
    private bool NeedUpdateLocalVersionFile = false;
    public static readonly string VERSION_FILE = "version.txt";
    
    public static readonly string LOCAL_RES_URL = "file:///" + Application.persistentDataPath + "/";
    public static readonly string LOCAL_RES_PATH = Application.persistentDataPath + "/";
//    public static readonly string SERVER_RES_URL = "http://192.168.1.136:8080/";
//    public static readonly string SERVER_RES_URL = "http://221.229.162.251:8080/AppleDB/";
//	public static readonly string SERVER_RES_URL = "http://221.229.162.251:8080/ZealmDBTest/";
#if UNITY_IOS
	public static readonly string SERVER_RES_URL = "http://221.229.162.251:8080/cp/1.2.9/Data/";

//	public static readonly string SERVER_RES_URL = "http://221.229.162.251:8080/100Test/";
#elif UNITY_ANDROID
	public static readonly string SERVER_RES_URL = "http://221.229.162.251:8080/cp/1.2.9Android/Data/";
#else
	public static readonly string SERVER_RES_URL = "http://221.229.162.251:8080/100Test/";
#endif

	public static readonly string DATA_NAME = "GameDateZealmTest.ydat";
	public static readonly string CONFIG_NAME = "YuanServerRoom.xml";
//   public static readonly string DATA_NAME = "GameDate.ydat";

   
    void Awake()
    {
        my = this;
        applicationPath = Application.persistentDataPath;

        string configFilePath = LOCAL_RES_PATH + CONFIG_NAME;
		//判断目标目录下是否存在文件
        //if (!File.Exists(configFilePath))
        //{
		//	StartCoroutine(this.DownLoad(SERVER_RES_URL + CONFIG_NAME, delegate(WWW localData)
		//	{
		//		ReplaceLocalRes1(CONFIG_NAME, localData.bytes); 
		//	}));
            /*StartCoroutine(DownLoad(@"file:///" + Application.dataPath + "/StreamingAssets/" + CONFIG_NAME, delegate(WWW localData)
            {
                ReplaceLocalRes1(CONFIG_NAME, localData.bytes);
            }));*/
        //}
        //else
        //{
//            Debug.Log("cccccccccccccccccccccccccccccccccccccccccccccc");
        //    ReadDicBenefitsInfo.ReadYuanServerRoom(LOCAL_RES_PATH + CONFIG_NAME);
        //}
      
       
    }

    // Use this for initialization
    void Start()
    {
        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().tableRead = this;
        if (lblGameVersion != null)
        {
            lblGameVersion.text = YuanUnityPhoton.GameVersion.ToString();
        }
    }

    private void versionCompare()
    {
        readNativeFile ();
        
        LocalResVersion = new Dictionary<string, string>();
        ServerResVersion = new Dictionary<string, string>();
        NeedDownFiles = new List<string>();
        //LOCAL_RES_URL + VERSION_FILE
        //NGUIDebug.Log("===========download loacl file =========" + LOCAL_RES_PATH + VERSION_FILE);
        StartCoroutine(DownLoad(@"file:///" + LOCAL_RES_PATH + VERSION_FILE, delegate(WWW localVersion)
        {
            //NGUIDebug.Log("===========download loacl file =========" + localVersion.text); 
            ParseVersionFile(localVersion.text, LocalResVersion);
            
            //加载服务端version配置  
            //NGUIDebug.Log("===========download  server file =========" + SERVER_RES_URL + VERSION_FILE);
            StartCoroutine(this.DownLoad(SERVER_RES_URL + VERSION_FILE, delegate(WWW serverVersion)
            {
                //保存服务端version  
                ParseVersionFile(serverVersion.text, ServerResVersion);
                //计算出需要重新加载的资源  
               
                CompareVersion();
                //加载需要更新的资源  
                DownLoadRes();
            }));
        }

        )
        );
    }

    private void DownLoadRes()
    {
        if (NeedDownFiles.Count == 0)
        {
            UpdateLocalVersionFile();
            
			ReadDicBenefitsInfo.ReadYuanServerRoom(LOCAL_RES_PATH + CONFIG_NAME);
            StartCoroutine(ReadHttp(LOCAL_RES_URL + DATA_NAME));

            return;
        }

        string file = NeedDownFiles[0];
        NeedDownFiles.RemoveAt(0);
        StartCoroutine(ReadHttpServer(SERVER_RES_URL, file)); 
    }

	public void TimeOut()
	{
		Invoke("ReadTimeOut", 90);
	}

    IEnumerator ReadHttpServer(string serverUrl, string file)
    {
        download = new WWW(serverUrl + file);
        //Invoke("ReadTimeOut", 40);
        yield return download;

		//将下载的资源替换本地就的资源  
        ReplaceLocalRes(file, download.bytes);
        DownLoadRes();
    }

    //更新本地的version配置  
    private void UpdateLocalVersionFile()
    {
        if (NeedUpdateLocalVersionFile)
        {
            
            StringBuilder versions = new StringBuilder();
            foreach (var item in ServerResVersion)
            {
                versions.Append(item.Key).Append(",").Append(item.Value).Append("\n");
            }

            FileStream stream = new FileStream(LOCAL_RES_PATH + VERSION_FILE, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            //NGUIDebug.Log("==================UpdateLocalVersionFile ===========================" + LOCAL_RES_PATH + VERSION_FILE + data.Length);
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }
        //加载显示对象  
        //StartCoroutine(Show());
    }

    private void ReplaceLocalRes(string fileName, byte[] data)
    {
        string filePath = LOCAL_RES_PATH + fileName;
        FileStream stream = new FileStream(LOCAL_RES_PATH + fileName, FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }

    private void ReplaceLocalRes1(string fileName, byte[] data)
    {
        string filePath = LOCAL_RES_PATH + fileName;
        FileStream stream = new FileStream(LOCAL_RES_PATH + fileName, FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
        ReadDicBenefitsInfo.ReadYuanServerRoom(LOCAL_RES_PATH + CONFIG_NAME);
//        Debug.Log("11111111111111111111111111111111111111111111111111111111111");
    }

    private void ParseVersionFile(string content, Dictionary<string, string> dict)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        string[] items = content.Split(new char[] { '\n' });
        foreach (string item in items)
        {
            string[] info = item.Split(new char[] { ',' });
            if (info != null && info.Length == 2)
            {
                dict.Add(info[0], info[1]);
            }
        }

    }

    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        yield return www;
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
    }

    public delegate void HandleFinishDownload(WWW www);

    private void CompareVersion()
    {
        foreach (var version in ServerResVersion)
        {
            string fileName = version.Key;
            string serverMd5 = version.Value;
            //新增的资源  
            if (!LocalResVersion.ContainsKey(fileName))
            {
                NeedDownFiles.Add(fileName);
            }
            else
            {
                //需要替换的资源  
                string localMd5;
                LocalResVersion.TryGetValue(fileName, out localMd5);
                if (!serverMd5.Equals(localMd5))
                {
                    NeedDownFiles.Add(fileName);
                }
            }
        }
        //本次有更新，同时更新本地的version.txt  
        NeedUpdateLocalVersionFile = NeedDownFiles.Count > 0;
    }


    IEnumerator ReadHttp(string url)
    {
        //NGUIDebug.Log("==============ReadHttp=================" + url);
		try
		{
        	download = new WWW(url);
		}
		catch(System.Exception e)
		{
			ReadTimeOut (StaticLoc.Loc.Get("meg0217"));
			Debug.LogError (e.ToString ());
		}
        //Invoke("ReadTimeOut", 40);
        yield return download;
        //NGUIDebug.Log("==============ReadHttp  download=================" + download.size);
        
        myByte = download.bytes;
        //KDebug.WriteLog("=================================read file length==========" + myByte.Length);
        dataTherad = new System.Threading.Thread(new System.Threading.ThreadStart(SerializationData));
        dataTherad.Start();

    }

	public void SerializationFail()
	{
//		MainMenuManage.my.warnings.warningAllEnter.btnEnter.target=online;
//		MainMenuManage.my.warnings.warningAllEnter.btnEnter.functionName="ReOnline";
//		MainMenuManage.my.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("meg0093"));
		ReadTimeOut (StaticLoc.Loc.Get("meg0093"));

	}

    void SerializationData()
    {
        try 
		{ 
					
					strInfo=StaticLoc.Loc.Get("meg0092");
						//KDebug.WriteLog(string.Format("-----------------------------------开始解压"));
			        Dictionary<string, yuan.YuanMemoryDB.YuanTable> dicGet = yuan.YuanSerializationDataSet.SerializationDataSet.YuanDeserializeForByte<Dictionary<string, yuan.YuanMemoryDB.YuanTable>>(myByte);
						//KDebug.WriteLog(string.Format("-----------------------------------解压完成"));
					int num=0;
			        foreach (KeyValuePair<string, yuan.YuanMemoryDB.YuanTable> item in YuanUnityPhoton.dicGetYT)
			        {
			                item.Value.Rows = dicGet[item.Key].Rows;
								//KDebug.WriteLog(string.Format("--------------------------------------------------读表 {0} 完成",item.Value.TableName));
							num++;
			        }
						//KDebug.WriteLog (string.Format("---------------------所有读表完成:{0}",num));
			        isReadEnd = true;
        }
        catch (System.Exception ex)
        {
			isSerializationFail=true;
			//SerializationFail();
            Debug.LogError(string.Format(ex.ToString()));

        }
		finally
		{
			dataTherad.Abort();
		}
    }

    void ReadTimeOut()
    {
        if (!isReadTimeOutEnd)
        {
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().isTimerTableRead = false;
            strInfo = StaticLoc.Loc.Get("info546");
            isOnlineFiled = true;
			if(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().timerTableRead!=null)
			{
	            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().timerTableRead.Dispose();
	            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().timerTableRead = null;
			}
        }
    }

	public void ReadTimeOut(string mInfo)
	{
		if (!isReadTimeOutEnd)
		{

			strInfo = StaticLoc.Loc.Get(mInfo);
			
			isOnlineFiled = true;
			CancelInvoke("ReadTimeOut");
			if(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().timerTableRead!=null)
			{
				YuanUnityPhoton.GetYuanUnityPhotonInstantiate().timerTableRead.Dispose();
				YuanUnityPhoton.GetYuanUnityPhotonInstantiate().timerTableRead = null;
			}
		}
	}

    private bool isUpdating = false;
    private bool onceOpen = false;
    void Update()
    {
        if (isUpdating) return;

        if (isNeedUpdate)
        {
            isNeedUpdate = false;
            StartCoroutine(UpdatePanel());
            isUpdating = true;
            return;
        }

        if (lblOnline != null)
        {
            lblOnline.text = strInfo;
        }

        if (lblOnline != null && isOnlineFiled && null != objProgressbar && objProgressbar.active)
        {
            //lblOnline.text = "链接失败，您的网络不给力";
            btnReonline.SetActiveRecursively(true);


        }
        //if (sliderStart != null && sliderStart.sliderValue > 0f && !onceOpen)
        if (sliderStart != null && !onceOpen)
        {
            OpenAnnouncementPanel();    //当开始加载进度时，打开公告面板
            onceOpen = true;
            //TD_info.setStartGame(); // TD接入进入游戏统计
        }

        // if (sliderTable != null)
        // {
        //     sliderTable.sliderValue = (maxNum - readNum) / maxNum;
        // }
        if (sliderStart != null && isReadingTable && download != null)
        {
            sliderStart.sliderValue = download.progress;
        }

        if (isPlayerMax)
        {
            isPlayerMax = false;
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info655"));
        }

        if (isReadEnd)
        {
            if (TD_info.isTDStartGame)
            {
                //TD_info.startSucccess();// TD接入读条成功
            }

            if (online != null && online.active)
            {
                online.SetActiveRecursively(false);
            }


			isReadTimeOutEnd = true;
            if (isAnnouncementPanelClosed)
            {
				isReadEnd = false;
                ReadTableEnd();
            }
        }

        if (canReadTable)
        {
            canReadTable = false;
            isReadingTable = true;
            versionCompare();
            //StartCoroutine (ReadHttp (@"http://221.229.162.251:8080/GameDate.ydat")); 
            //			StartCoroutine (ReadHttp (@"http://192.168.0.116:8080/GameDateZealmTest.ydat")); 
            //			StartCoroutine (ReadHttp (@"http://221.229.162.251:8080/GameDateZealmTest.ydat")); 
            //			StartCoroutine (ReadHttp (@"http://192.168.1.123:8080/GameDateZealmTest.ydat")); 
            //StartCoroutine(ReadHttp(@"http://192.168.1.136:8080/GameData/GameDateZealmTest.ydat")); 

        }

		if(isSerializationFail)
		{
			isSerializationFail=false;
			SerializationFail();
		}
		if(isConnectFail)
		{
			isConnectFail=false;

			TableRead.my.ReadTimeOut (StaticLoc.Loc.Get("meg0098")+StaticLoc.Loc.Get("meg0103"));
			//OnLine.my.ShowConnectFail(StaticLoc.Loc.Get("meg0098")+StaticLoc.Loc.Get("meg0103"));
		}
    }

    public GameObject objNeedUpdate;
    public GameObject objProgressbar;
    public UILabel lbNeedUpdate;
    /// <summary>
    /// 需要更新程序时，关闭不必要的面板并显示更新面板
    /// </summary>
    private IEnumerator UpdatePanel()
    {
        if (null != objNeedUpdate && null != objProgressbar)
        {
            //            if (logoObj != null && !logoObj.active)
            //            {
            //                logoObj.SetActiveRecursively(true);
            //            }

            if (AnnouncementPanel != null && AnnouncementPanel.active)
            {
                AnnouncementPanel.SetActiveRecursively(false);
                isAnnouncementPanelClosed = true;
            }

            objProgressbar.SetActiveRecursively(false);
            objNeedUpdate.SetActiveRecursively(true);
            WWW www = new WWW(@"http://221.229.162.251:8080/cp/Update_Throne.html");
            yield return www;
            lbNeedUpdate.text = www.text;
        }
    }

    public void NeedUpdate()
    {
        StartCoroutine(GetNewPage());
        //Application.OpenURL ("http://down.joygame.cn/joygame/heianzhirengengxin.apk");
    }
#if UNITY_ANDROID
#if SDK_AZ
    public static string strPageName = "az";
#elif SDK_CMGE	
	public static string strPageName="cmge";
#elif SDK_DOWN	
	public static string strPageName="down";
#elif SDK_DUOKU	
	public static string strPageName="duoku";
#elif SDK_HUAWEI	
	public static string strPageName="huawei";
#elif SDK_ITOOLS	
	public static string strPageName="itools";
#elif SDK_JYIOS	
	public static string strPageName="jyios";
#elif SDK_JY
	public static string strPageName="jy";
#elif SDK_KUAIYONG	
	public static string strPageName="ky";
#elif SDK_LENOVO
	public static string strPageName="lenovo";
#elif SDK_MI
	public static string strPageName="mi";
#elif SDK_MUZI
	public static string strPageName="muzi";
#elif SDK_OPPO
	public static string strPageName="oppo";
#elif SDK_PEASECOD
	public static string strPageName="peasecod";
#elif SDK_QH
	public static string strPageName="qh";
#elif SDK_UC
	public static string strPageName="uc";
#elif SDK_VIVO
	public static string strPageName="vivo";
#else
	public static string strPageName="zealm";
#endif
#elif UNITY_IOS
#if SDK_JYIOS	
	public static string strPageName="jyios";
#elif SDK_ITOOLS
	public static string strPageName="itools";
#elif SDK_KUAIYONG
	public static string strPageName="ky";
#elif SDK_XY
	public static string strPageName="xy";
#elif SDK_I4
	public static string strPageName="i4";
#elif SDK_ZSY
	public static string strPageName="cmgeios";
#elif SDK_ZSYIOS
	public static string strPageName="zsyios";
#elif SDK_PP
	public static string strPageName="pp";
#elif SDK_TONGBU
	public static string strPageName="tbt";
#elif SDK_HM
	public static string strPageName="hm";
#else
	public static string strPageName="zealm";
#endif
#else
	public static string strPageName="zealm";
#endif
    public IEnumerator GetNewPage()
    {
        //WWW www=new WWW(@"http://221.229.162.251:8080/geturl/geturl.php?type="+strPageName);
        WWW www = new WWW(@"http://221.229.162.251:8080/cp/geturl/geturl.php?type=" + strPageName);
        yield return www;

        try
        {
            if (!string.IsNullOrEmpty(www.text))
            {
                Application.OpenURL(www.text);
            }
            else
            {
                YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info727"));
            }
        }
        catch
        {
            YuanUnityPhoton.GetYuanUnityPhotonInstantiate().MMManage.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info727"));
        }
    }

    //public GameObject  mm;
    public static bool canSDKLogin = false;
    public void ReadTableEnd()
    {
        objWirtelog.Register();

        canSDKLogin = true;
        if (LoginSDKManager.CanSDKLogin)
        {
			//修改上传冲突
            //登陆misdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
            //StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginMI(LoginSDKManager.SdkID,LoginSDKManager.sdkToken,"ZealmPass","UserInfo",true)));
            //登陆opposdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
            //StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginOPPO(LoginSDKManager.SdkID,LoginSDKManager.sdkToken,"ZealmPass","UserInfo",true)));


            //登陆中手游sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
            //	StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginZSY(LoginSDKManager.SdkID,"ZealmPass","UserInfo",true)));
            //登陆联运sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；

            //登陆360sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
            //				StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginTSZ(LoginSDKManager.SdkID,LoginSDKManager.sdkToken,"ZealmPass","UserInfo",true)));
#if UNITY_IOS
#if SDK_JYIOS
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLogin91(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_ITOOLS
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginItools(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_KUAIYONG
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginKYSDK(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_XY
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginXY(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_I4
			BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginAS(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_ZSY
			BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginZSYIos(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_ZSYIOS
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginCMGE(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_PP
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginPP(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_TONGBU
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginTB(LoginSDKManager.SdkID,TableRead.strPageName,true),null);
#elif SDK_HM
		BtnManager.my.RunBeginTimeOut(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginHM(LoginSDKManager.SdkID,TableRead.strPageName,true),null);

#endif
#endif
#if UNITY_ANDROID
#if SDK_UC
            StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10, 2, BtnManager.my.ConnectYuanUnity, () => YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginUC(LoginSDKManager.SdkID, true), null));
#elif SDK_CMGE
			//登陆中手游sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginZSY(LoginSDKManager.SdkID,true),null));	
#elif SDK_DOWN
			string[] codes = LoginSDKManager.SdkID.Split(';');
			string mid = codes[0];
			string token = codes[1];
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().PlayerLoginDL(mid,token,true),null));
			Debug.Log("mid=" + mid + " token=" + token);
#elif SDK_QH
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginTSZ(LoginSDKManager.SdkID,LoginSDKManager.SdkToken,true),null));
			Debug.Log("uid=" + LoginSDKManager.SdkID);
#elif SDK_LENOVO
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginLenovo(LoginSDKManager.SdkID,true),null));
#else
			//登陆联运sdk,只需要修正strPageName的值（186行）,与当前要出的sdk对应,其余方法注释；
			StartCoroutine(BtnManager.my.BeginTimeOutNoRe(10,2,BtnManager.my.ConnectYuanUnity ,()=>YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginLianYun(LoginSDKManager.SdkID,strPageName,true),null));
#endif
#endif
            //YuanUnityPhoton.GetYuanUnityPhotonInstantiate().PlayerLoginLianYun(LoginSDKManager.SdkID,channelName,true);
            canSDKLogin = false;
            LoginSDKManager.CanSDKLogin = false;
        }

        if (objBtns != null && !objBtns.active)
        {
            objBtns.SetActiveRecursively(true);
            sdklogin.SetActiveRecursively(true);
        }

        isLoadingEnd = true;

        //if (online != null)
        //{
        //    online.SetActiveRecursively(false);
        //}
    }

    public void CloseOnlineBtn()
    {
        if (online != null)
        {
            online.SetActiveRecursively(false);
        }
    }

    private bool isAnnouncementPanelClosed = false;
    //    public GameObject logoObj;
    public GameObject btnInfo;
    private bool isLoadingEnd = false;
    /// <summary>
    /// 打开公告面板，当游戏开始加载进度条的瞬间调用此方法
    /// </summary>
    void OpenAnnouncementPanel()
    {
        if (AnnouncementPanel != null && !AnnouncementPanel.active)
        {
            AnnouncementPanel.SetActiveRecursively(true);
            isAnnouncementPanelClosed = false;
        }
    }
    /// <summary>
    /// 关闭公告面板，当公告面板上的关闭按钮被点击时才调用此方法
    /// </summary>
    void CloseAnnouncementPanel()
    {
        //TD_info.NoticeSuccess();//TD接入关闭公告
        if (AnnouncementPanel != null && AnnouncementPanel.active)
        {
            AnnouncementPanel.SetActiveRecursively(false);
            isAnnouncementPanelClosed = true;
        }

        //        if (logoObj != null && !logoObj.active)
        //        {
        //            logoObj.SetActiveRecursively(true);
        //        }

        if (btnInfo != null && !btnInfo.active)
        {
            btnInfo.SetActiveRecursively(true);
        }

        if (objBtns != null && !objBtns.active && isLoadingEnd)
        {
            objBtns.SetActiveRecursively(true);
            sdklogin.SetActiveRecursively(true);
        }
    }

    public GameObject AnnouncementPanel;
    /// <summary>
    /// 点击感叹号按钮时的逻辑
    /// </summary>
    void BtnInfoClick()
    {
        if (btnInfo != null && btnInfo.active)
        {
            btnInfo.SetActiveRecursively(false);
        }

        if (AnnouncementPanel != null && !AnnouncementPanel.active)
        {
            AnnouncementPanel.SetActiveRecursively(true);
            isAnnouncementPanelClosed = false;
        }

        if (objBtns != null && objBtns.active)
        {
            objBtns.SetActiveRecursively(false);

        }
        if (sdklogin != null && sdklogin.active)
        {
            sdklogin.SetActiveRecursively(false);
        }
    }

	/// <summary>
	/// Loads the local data.
	/// 复制文件到外部存储
	/// </summary>
	private void loadLocalData()
	{
		StartCoroutine(DownLoad(@"file:///" + Application.dataPath + "/StreamingAssets/"+VERSION_FILE, delegate(WWW localVersion)
		                        {
			ReplaceLocalRes(VERSION_FILE, localVersion.bytes);
		}));
		StartCoroutine(DownLoad(@"file:///" + Application.dataPath + "/StreamingAssets/"+DATA_NAME, delegate(WWW localData)
		                        {
			ReplaceLocalRes(DATA_NAME, localData.bytes);
		}));
		/*StartCoroutine(DownLoad(@"file:///" + Application.dataPath + "/StreamingAssets/"+CONFIG_NAME, delegate(WWW localData)
		                        {
			ReplaceLocalRes(CONFIG_NAME, localData.bytes);
		}));*/
		Debug.Log ("loadLocalData"+ "    复制文件到外部存储");
	}

	/// <summary>
	/// Reads the native file.
	/// 判断目标目录下是否存在文件
	/// </summary>
	 void readNativeFile(){

		string versionFilePath = LOCAL_RES_PATH + VERSION_FILE;
		string dataFilePath = LOCAL_RES_PATH + DATA_NAME;
		string configFilePath = LOCAL_RES_PATH + CONFIG_NAME;
//		Debug.Log (versionFilePath);
		//判断目标目录下是否存在文件
		if (File.Exists (versionFilePath) && File.Exists (dataFilePath) ){//&& File.Exists(configFilePath)) {
			//存在
//			Debug.Log("file is have");
		} else {
			//不存在
			loadLocalData();
		}
	}

}
