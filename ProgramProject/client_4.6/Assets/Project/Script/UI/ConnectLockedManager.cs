using UnityEngine;
using System.Collections;

public class ConnectLockedManager : BWUIPanel ,ProcessResponse{

	public GameObject conLockedObj;
	private GameObject child;
	private float frameCount;
	private float offsetForRequest;
	private const float ReTime=10f;
	
	private int requestType;
	private bool receiveData;
	private bool canLoadScene;
	private PlayerResultJson pj;
	
	void Awake(){
		_MyObj = gameObject;
		init();
	}
	
	void Start(){
		if(conLockedObj == null){
			conLockedObj = GameObject.Find("ConnectLocked");
		}
		if(conLockedObj != null )
		{
			child = conLockedObj.transform.FindChild("Child").gameObject;
			child.SetActive(false);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerInfo.getInstance() != null)
		{
			//显示联网界面//	
			if(PlayerInfo.getInstance().isShowConnectObj )
			{
				if(_MyObj.transform.localPosition.x!=Vector3.zero.x || _MyObj.transform.localPosition.y!=Vector3.zero.y || _MyObj.transform.localPosition.z!=Vector3.zero.z)
				{
					_MyObj.transform.localPosition=Vector3.zero;
				}
				frameCount += Time.deltaTime;
				if(!conLockedObj.activeSelf)
				{
					//Debug.Log("show Loading panel****************");
					conLockedObj.SetActive(true);
					conLockedObj.transform.position = Vector3.zero;
				}
				//当联网时间超过1.5秒后，显示图片//
				else if(frameCount > 1.5f && !child.activeSelf)
				{
					child.SetActive(true);
				}
				
				if(PlayerInfo.getInstance().timeout && frameCount>=ReTime)
				{
					offsetForRequest+=Time.deltaTime;
					if(offsetForRequest>=1f)
					{
						offsetForRequest=0;
						PlayerInfo.getInstance().timeout=false;
						//==发送登录请求==//
						string username=PlayerPrefs.GetString("username");
						string password=PlayerPrefs.GetString("password");
						string platform="";
						if(Application.platform==RuntimePlatform.Android)
						{
							platform=Constant.OS_ANDROID;
						}
						else if(Application.platform==RuntimePlatform.IPhonePlayer)
						{
							platform=Constant.OS_IOS;
						}
						else
						{
							platform=Constant.OS_PC;
						}
						string json=JsonMapper.ToJson(new LoginJson(username,password,1,platform,null));
						requestType=1;
						RequestSender.getInstance().clearCookie();
						RequestSender.getInstance().request(1,json,true,this);
						//times++;
						//msg="第"+times+"次重连";
					}
				}
			}
			
			//隐藏联网界面//
			if(!PlayerInfo.getInstance().isShowConnectObj && conLockedObj.activeSelf){
				//Debug.Log("hide Loading panel _________________");
				child.SetActive(false);
				conLockedObj.SetActive(false);
				frameCount = 0;
			}
		}
		if(canLoadScene)
		{
			canLoadScene=false;
			PlayerInfo.getInstance().isShowConnectObj=false;
			PlayerInfo.getInstance().BattleOverBackType=0;
			PlayerInfo.isFirstLogin = true;
			GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
		}
		if(receiveData)
		{
			receiveData=false;
			switch(requestType)
			{
			case 1:
				if(pj.errorCode==0)
				{
					//==0不需重新登录,1需要重新登录==//
					if(pj.mark==0)
					{
						PlayerInfo.getInstance().reSendRequest();
					}
					else
					{
						canLoadScene=true;
					}
				}
				break;
			}
		}
	}
	
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//==此处特殊,收到消息但不处理isShowConnectObj标识==//
			switch(requestType)
			{
			case 1:
				pj=JsonMapper.ToObject<PlayerResultJson>(json);
				receiveData=true;
				break;
			}
		}
	}
	
//	int times;
//	string msg;
//	public void OnGUI()
//	{
//		if(msg!=null)
//		{
//			GUI.Label(new Rect(100,100,300,60), "msg:" + msg);
//		}
//	}
}
