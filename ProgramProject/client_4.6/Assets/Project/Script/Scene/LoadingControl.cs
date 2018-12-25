using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class LoadingControl : MonoBehaviour ,ProcessResponse{
	
	public static LoadingControl instance;
	//==内网公告服==//
	private string InnerServerIp="118.25.209.26";
	private string InnerServerPort="8080";
	//==外网测试公告服==//
	private string OuterServerIp="118.25.209.26";
	private string OutServerPort="8080";
	//==外网公告服==//
	//private string OuterServerIp="112.124.25.230";
	//private string OutServerPort="8080";
	
	//==苹果公告服==//
	//private string OuterServerIp="121.40.204.200";
	//private string OutServerPort="8080";
	
	public static string ServerCenterIp;
	public static string ServerCenterPort;
	
	private GameObject _myObj;
	private LoadRes lr;
	private GameObject processBar;
	//private UILabel processLabel;
	private UISprite processSpr;
	private int requestType;
	private bool receiveData;
	private string announce;
	
	//==内外网==//
	public bool outerNet;
	public string version="1.0.0";
	
	//public UILabel copyResource;
	//public UILabel dowFileForComparing;
	//public UILabel comparing;
	//public UILabel downBinFiles;
	//public UILabel loadBinFiles;
	//public UILabel downPackage;
	public UILabel versionWrong;
	//public UILabel installPackage;
	//public UILabel enterGame;
	public GameObject downError;
	
	public GameObject gcLogoPanel;
	
	//==-1拷贝资源文件,0读取音乐比对包版本,1下载资源比对文件,2根据比对结果下载资源文件,3加载资源文件,4显示进入游戏界面==//
	private int step=-1;
	private bool canNext;
	private List<string> downloadList;
	private int downloadIndex;
	//private float sliderValue;
	public static string localPath;
	//==bin文件远程路径==//
	private string binPath;
	//==0版本不符,1版本ok==//
	private int mark;
	//==是否可安装==//
	private bool canInstall;
	//==当前www==//
	//private WWW curWWW;
	
	private const string ResManagerTemp="res_manager_temp.bin";
	private const string ResManager="res_manager.bin";
	//==下载到本地的包名==//
	private string packageName;
	private const string VersionUrl="http://118.25.209.26:8080/card_server_center/gift.htm?action=serverVersion";
	
	private bool receiveEnterGameMsg = false;
	
	public string curDownRemotePath;
	public string curDownLocalPath;
	private float curProcessValue;
	private int curRequestCommand;
	private string curRequestUrl;
	private string curRequestParams;
	private bool needShowRetry;
	private int retryType;//==1request失败,2下载失败==//
	
	void Awake()
	{
		_myObj=gameObject;
		instance=this;
		//设置不让屏幕变暗//
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	// Use this for initialization
	void Start ()
	{
		if(outerNet)
		{
			RequestSender.serverIp=OuterServerIp;
			RequestSender.serverPort=OutServerPort;
		}
		else
		{
			RequestSender.serverIp=InnerServerIp;
			RequestSender.serverPort=InnerServerPort;
		}
		ServerCenterIp=RequestSender.serverIp;
		ServerCenterPort=RequestSender.serverPort;
		
		localPath= Application.persistentDataPath+"//";
		Debug.Log("localPath ============ " + localPath);
		
		lr=_myObj.GetComponent<LoadRes>();
		processBar=_myObj.transform.FindChild("Progress Bar").gameObject;
		//processLabel = processBar.transform.FindChild("Label").GetComponent<UILabel>();
		processSpr = processBar.transform.FindChild("Foreground").GetComponent<UISprite>() ;
		
		downError.SetActive(false);
		if(PlayerInfo.getInstance().isLogout)
		{
			step=5;
			curProcessValue=1f;
		}
		
		canNext=true;
		
		GcLogoInit();
	}
	
	public void setVersion(string v)
	{
		version=v;
		canNext=true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(curProcessValue<1)
		{
			curProcessValue += Time.deltaTime/4;
			processSpr.fillAmount = curProcessValue;
		}
		
		if(needShowRetry)
		{
			needShowRetry=false;
			downError.SetActive(true);
		}
			
		if(canNext)
		{
			canNext=false;
			switch(step)
			{
			case -1:
				//==如果没有拷贝bin变量,则从StreamingAssets拷贝一份==//
				if(PlayerPrefs.GetInt("StreamingAssets")!=1)
				{
					//processLabel.text=copyResource.text;
					StartCoroutine(copyResourceFiles());
				}
				else
				{
					step=0;
					canNext=true;
				}
				break;
			case 0:
				//读取音量//
			 	PlayerInfo.getInstance().musicBgVolume = PlayerPrefs.GetFloat(STATE.SAVE_KEY_MUSICBG, 1);
				PlayerInfo.getInstance().soundEffVolume = PlayerPrefs.GetFloat(STATE.SAVE_KEY_SOUNDEFF, 1);
				//==请求游戏版本,如果版本不符,不能进入游戏(android自动下载最新版本并运行)==//
				requestType=1;
				string url=VersionUrl;
				string platform="other";
				if(Application.platform==RuntimePlatform.Android)
				{
					platform="android";
				}
				else if(Application.platform==RuntimePlatform.IPhonePlayer)
				{
					platform="ios";
				}
				VersionJson vj=new VersionJson(version,TalkingDataManager.channelId,platform);
				string param=JsonMapper.ToJson(vj);
				curRequestUrl=url;
				curRequestParams=param;
				RequestSender.getInstance().request(url,param,false,this);
				break;
			case 1://==下载资源比对文件==//
				//==设置进度文本==//
				//sliderValue=0.2f;
				//processLabel.text=dowFileForComparing.text;
				//==异步下载比对文件==//
				StartCoroutine(BinReader2.downLoadFile(binPath+ResManager,localPath+ResManagerTemp,this,false));
				break;
			case 2://==比对==//
				//sliderValue=0.4f;
				//processLabel.text=comparing.text;
				StartCoroutine(compareResManager());
				break;
			case 3://==根据比对结果下载资源文件==//
				//if(downloadList.Count>0)
				//{
					//processLabel.text=downBinFiles.text+","+downloadIndex+"/"+downloadList.Count;
					//sliderValue=0.4f+0.6f*downloadIndex/downloadList.Count;
				//}
				if(downloadIndex>=downloadList.Count)
				{
					//==覆盖旧的==//
					File.Delete(localPath+ResManager);
					File.Move(localPath+ResManagerTemp,localPath+ResManager);
					step=4;
					canNext=true;
				}
				else
				{
					string fileName=downloadList[downloadIndex];
					downloadIndex++;
					StartCoroutine(BinReader2.downLoadFile(binPath+fileName,localPath+fileName,this,false));
				}
				break;
			case 4://==加载资源文件==//
				//sliderValue=1f;
				//processLabel.text=loadBinFiles.text;
				lr.loadBins();
				break;
			case 5://==显示进入游戏界面==//
				//processLabel.text=enterGame.text+"...";
				readPlayerPrefs();
				requestType=2;
				curRequestCommand=33;
				RequestSender.getInstance().request(33,"",false,this);
				StopAllCoroutines();
				break;
			}
		}
		
		if(receiveData)
		{
			receiveData=false;
			switch(requestType)
			{
			case 1:
				Debug.Log("binPath:"+binPath);
				if(mark==0)
				{
					ToastWindow.mInstance.showText(versionWrong.text);
					if(!string.IsNullOrEmpty(binPath))
					{
						if(Application.platform==RuntimePlatform.Android)
						{
							Application.OpenURL(binPath);
						}
					}
				}
				else if(mark==-1)
				{
					NoticeUIManager.mInstance.SetData(binPath);
				}
				else
				{
					step=1;
					canNext=true;
				}
				break;
			case 2:
				receiveEnterGameMsg = true;
				break;
			}
		}
		//==1.进度条走满,2.收到进入游戏信息==//
		if(curProcessValue>=1 && receiveEnterGameMsg)
		{
			_myObj.SetActive(false);
			if(Application.platform  == RuntimePlatform.IPhonePlayer)
			{
				if(SDKManager.getInstance().isSDK91Using())
				{
					//SDK91ConectorHelper.doInitSDK();
					if(!PlayerInfo.getInstance().isLogout)
					SDKPlatform91.SdkInit();
				}
				else if(SDKManager.getInstance().isSDKTBUsing())
				{
					//SDK.TBInit();
				}
			}
			Debug.Log("announce:"+announce);
			if(announce != null && announce != "" && announce.Length > 0)
			{
				NoticeUIManager.mInstance.SetData(announce);
			}
			LoginUI_new.mInstance.show();
		}
		//==游戏包下载中==//
		//if(curWWW!=null)
		//{
			//sliderValue=curWWW.progress;
			//processLabel.text=downPackage.text+(int)(sliderValue*100)+"%";
		//}
		//==游戏包下载完成,安装游戏包==//
		if(canInstall)
		{
			canInstall=false;
			if(Application.platform==RuntimePlatform.Android)
			{
				//processLabel.text=installPackage.text+"...";
				if(SDKManager.getInstance().isSDKGCUsing())
				{
					//==调用android方法==//
					SDK_StubManager.installApk(localPath+packageName);
				}
				else if(SDKManager.getInstance().isSDKCPYYUsing())
				{
					SDK_StubManager.installApk(localPath+packageName);
				}
			}
		}
		
//		processBar.GetComponent<UISlider>().value=sliderValue;
		
	}
	
	private IEnumerator copyResourceFiles()
	{
		List<string> copyFiles=new List<string>();
		copyFiles.AddRange(GetComponent<LoadRes>().binNames);
		copyFiles.Add(ResManager);
		//==获取StreamingAssets路径==//
		string streamingAssetsPath = Application.streamingAssetsPath + "/";
		if(Application.platform == RuntimePlatform.Android)
		{
			BinReader2.copyFolderForAndroid(streamingAssetsPath,copyFiles,localPath);
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			BinReader2.copyFolder(streamingAssetsPath,copyFiles,localPath);
		}
		else
		{
			BinReader2.copyFolder(streamingAssetsPath,copyFiles,localPath);
		}
		PlayerPrefs.SetInt("StreamingAssets",1);
		step=0;
		canNext=true;
		yield return null;
	}
	
	//==读取本地信息==//
	private void readPlayerPrefs()
	{
		LoginUI_new.mInstance.lastServerId=PlayerPrefs.GetInt("lastServerId");
		LoginUI_new.mInstance.username=PlayerPrefs.GetString("username");
		LoginUI_new.mInstance.password=PlayerPrefs.GetString("password");
	}
	
	public void downloadPackage(UpdateJson uj)
	{
		//==异步下载游戏包==//
		//processLabel.text=downPackage.text+"...";
		string url=uj.url;
		Debug.LogError(url);
		int index=url.LastIndexOf("/");
		packageName=url.Substring(index+1);
		StartCoroutine(BinReader2.downLoadFile(url,localPath+packageName,this,true));
	}
	
	public void setWWW(WWW www)
	{
		//curWWW=www;
	}
	
	//==下载完成==//
	public void downOver(bool mark)
	{
		//curWWW=null;
		if(!mark)
		{
			netFail(2);
			return;	
		}
		switch(step)
		{
		case 0:
			canInstall=true;
			break;
		case 1:
			step=2;
			canNext=true;
			break;
		case 3:
			canNext=true;
			break;
		}
	}
	
	//==比对==//
	public IEnumerator compareResManager()
	{
		List<string> result=new List<string>();
		
		List<string> resmanagersNew=BinReader2.readResManager(localPath+ResManagerTemp);
		if(resmanagersNew.Count==0)
		{
			Debug.Log("download resmanager error");
			yield return false;
		}
		List<string> fileNameNew=new List<string>();
		List<int> fileRivisionNew=new List<int>();
		List<int> fileSizeNew=new List<int>();
		for(int k=0;k<resmanagersNew.Count;k++)
		{
			string[] ss=resmanagersNew[k].Split('-');
			string fileName=StringUtil.getString(ss[0]);
			int rivision=StringUtil.getInt(ss[1]);
			int size=StringUtil.getInt(ss[2]);
			fileNameNew.Add(fileName);
			fileRivisionNew.Add(rivision);
			fileSizeNew.Add(size);
		}
		
		downloadList=new List<string>();
		
		List<string> resmanagersOld=BinReader2.readResManager(localPath+ResManager);
		//==开始比对==//
		if(resmanagersOld.Count==0)
		{
			result.AddRange(fileNameNew);
		}
		else
		{
			Hashtable fileNameOld=new Hashtable();
			for(int k=0;k<resmanagersOld.Count;k++)
			{
				string[] ss=resmanagersOld[k].Split('-');
				string fileName=StringUtil.getString(ss[0]);
				int rivision=StringUtil.getInt(ss[1]);
				int size=StringUtil.getInt(ss[2]);
				fileNameOld.Add(fileName,rivision+"-"+size);
			}
			for(int k=0;k<fileNameNew.Count;k++)
			{
				string fileName=fileNameNew[k];
				if(!fileNameOld.ContainsKey(fileName))
				{
					result.Add(fileName);
				}
				else
				{
					int rivisionNew=fileRivisionNew[k];
					int sizeNew=fileSizeNew[k];
					
					string info=(string)fileNameOld[fileName];
					string[] ss=info.Split('-');
					int rivisionOld=StringUtil.getInt(ss[0]);
					long sizeOld= BinReader2.getFileSize(localPath+fileName);
					if(rivisionNew!=rivisionOld || sizeNew!=sizeOld)
					{
						result.Add(fileName);
					}
				}
			}
		}
		//==如果不在加载列表里,则无需下载==//
		List<string> binNames=GetComponent<LoadRes>().binNames;
		foreach(string fileName in result)
		{
			if(binNames.Contains(fileName))
			{
				downloadList.Add(fileName);
			}
		}
		downloadIndex=0;
		step=3;
		canNext=true;
		yield return null;
	}
	
	//==加载bin文件完成==//
	public void loadBinsOver()
	{
		step=5;
		canNext=true;
		//Application.LoadLevel ("a");
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
				VersionResultJson vrj=JsonMapper.ToObject<VersionResultJson>(json);
				binPath=vrj.binPath;
				mark=vrj.mark;
				receiveData=true;
				break;
			case 2:
				ServerListJson slj=JsonMapper.ToObject<ServerListJson>(json);
				LoginUI_new.mInstance.servers=slj.list;
				announce=slj.announce;
				receiveData=true;
				break;
			}
		}
	}
	
	void GcLogoInit()
	{
#if UNITY_EDITOR
#elif PLAT_KY
		if(!PlayerInfo.getInstance().gcLogoHadShow)
		{
			PlayerInfo.getInstance().gcLogoHadShow = true;
			gcLogoPanel.SetActive(true);
			Invoke("GcLogoDisable", 2.0f);
			canNext = false;
		}
#endif
	}
	
	void GcLogoDisable()
	{
		canNext = true;
		gcLogoPanel.SetActive(false);
	}
	
	public void onClickRetry()
	{
		downError.SetActive(false);
		if(retryType==1)
		{
			//==重新请求==//
			if(requestType==1 && !string.IsNullOrEmpty(curRequestUrl) && !string.IsNullOrEmpty(curRequestParams))
			{
				RequestSender.getInstance().request(curRequestUrl,curRequestParams,false,this);
			}
			if(requestType==2 && curRequestCommand!=0)
			{
				RequestSender.getInstance().request(curRequestCommand,"",false,this);
			}
		}
		else
		{
			//==重新下载文件==//
			if(!string.IsNullOrEmpty(curDownRemotePath) && !string.IsNullOrEmpty(curDownLocalPath))
			{
				StartCoroutine(BinReader2.downLoadFile(curDownRemotePath,curDownLocalPath,this,false));
			}
		}
	}
	
	public void netFail(int type)
	{
		retryType=type;
		needShowRetry=true;
	}
}
