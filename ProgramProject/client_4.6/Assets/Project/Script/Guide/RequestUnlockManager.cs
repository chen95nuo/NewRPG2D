using UnityEngine;
using System.Collections;

public class RequestUnlockManager : BWUIPanel,ProcessResponse
{
	public enum MODELID : int
	{
		E_Intensify = 3,
		E_Compose = 4,
		E_Rune = 5,
		E_ActiveCopy = 8,
		E_PVP = 9,
		E_Spirit = 10,
		E_WarpSpace = 11,
		E_LotCard = 16,
		E_Break = 17,
		E_Achievement = 20,
		E_Sign = 23,
	}
	public static RequestUnlockManager mInstance = null;
	public GameObject lockObj = null;
	public GameObject unlockPanel = null;
	public UISprite icon;
	public UILabel desc;
	int unlockID;
	LockResultJson lrj;
	
	public bool finishRequestUnLockMsg = true;
	bool receiveData;
	
	int requestType = 0;
	bool wairRequestPlayerInfo = false;
	
	int errorCode = 0;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
		wairRequestPlayerInfo = false;
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(receiveData)
		{
			receiveData = false;
			if(errorCode == -3)
				return;
			if(requestType == 1)
			{
				finishRequestUnLockMsg = true;
				if(lrj != null && errorCode == 0)
				{
					if(lrj.type == 0)
						return;
					showUnlockPanel(lrj.modeId);
					switch(lrj.modeId)
					{
					case (int)MODELID.E_Intensify:
					{
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_Compose:
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_UnlockCompose);
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_Rune:
					{
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_ActiveCopy:
					{
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_PVP:
					{
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_Spirit:
					{
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_WarpSpace:
					{
						GuideManager.getInstance().runGuide();
					}break;
					case (int)MODELID.E_Break:
					{
						GuideManager.getInstance().runGuide();	
					}break;
					case (int)MODELID.E_Achievement:
					{
						GuideManager.getInstance().runGuide();
					}break;
					}
					requestType = 2;
					wairRequestPlayerInfo = true;
					PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
				}
			}
			else if(requestType == 2)
			{
				if(errorCode == 0)
				{
					wairRequestPlayerInfo = false;
					if(HeadUI.mInstance != null)
					{
						HeadUI.mInstance.refreshPlayerInfo();
					}
				}
				
				
			}
			
		}
	}
	
	public override void init()
	{
		base.init();
		unlockPanel.transform.localPosition = new Vector3(0,0,0);
	}
	
	public override void show ()
	{
		unlockPanel.SetActive(true);
	}
	
	public override void hide()
	{
		lockObj.SetActive(false);
		unlockPanel.SetActive(false);
		unlockID = -1;
		lrj=null;
		wairRequestPlayerInfo = false;
	}
	
	public void showUnlockPanel(int id)
	{
		UnlockData ud = UnlockData.getData(id);
		if(ud == null)
			return;
		if(ud.showup == 0)
			return;
		unlockID = id;
		show();
		icon.spriteName = ud.icon;
		desc.text = ud.unlockdes;
		if(ToastWindow.mInstance != null && ToastWindow.mInstance.isVisible())
		{
			ToastWindow.mInstance.hide();
		}
	}
	
	public void onClickCheckBtn()
	{
		if(wairRequestPlayerInfo)
			return;
		lockObj.SetActive(false);
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_UnlockBreak))
		{
			GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_UnlockBreak);
		}
		
		UnlockData ud = UnlockData.getData(unlockID);
		if(ud != null)
		{
			//if(ud.type != 1)
			{
//				MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
				//返回主城界面//
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
				if(main!= null)
				{
					main.SetData(STATE.ENTER_MAINMENU_BACK);
				}
				
				MissionUI mission1 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI") as MissionUI;
				
				
				if(mission1 != null && mission1.gameObject.activeSelf)
				{
					mission1.hide();
				}
				WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager") as WarpSpaceUIManager;
				if(warpSpace != null && warpSpace.gameObject.activeSelf)
				{
					warpSpace.hide();
				}
				
				ActiveWroldSelManager activeSel = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COPYSELECT, 
					"ActiveWroldSelManager")as ActiveWroldSelManager;
				if(activeSel != null && activeSel.gameObject.activeSelf)
				{
//					ActiveWroldSelManager.mInstance.hide();
					activeSel.hide();
				}
				
				SweepUIManager sweep = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP, "SweepUIManager") as SweepUIManager;
				if(sweep != null && sweep.gameObject.activeSelf)
				{
					sweep.hide();
				}
				
				ActivityPanel active = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVE,"ActivityPanel") as ActivityPanel;
				if(active != null && active.gameObject.activeSelf)
				{
					active.hide();
				}
			}

		}
		hide();
	}
	
	public void requestUnlockMsg()
	{
		requestType = 1;
		lockObj.SetActive(true);
		UIJson uiJson = new UIJson(STATE.UI_Unlock);
		PlayerInfo.getInstance().sendRequest(uiJson,this);
		finishRequestUnLockMsg = false;
	}
	
	public void receiveResponse(string json)
	{
		if(json != null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			if(requestType == 1)
			{
				receiveData = true;
				lrj = JsonMapper.ToObject<LockResultJson>(json);
				errorCode = lrj.errorCode;
			}
			else if(requestType  == 2)
			{
				PlayerResultJson pj=JsonMapper.ToObject<PlayerResultJson>(json);
				errorCode = pj.errorCode;
				if(pj!=null && pj.list!=null && pj.list.Count>0)
				{
					string[] str = pj.s;
					PlayerInfo.getInstance().SetUnLockData(str);
					PlayerInfo.getInstance().player = pj.list[0];
					receiveData=true;
				}
			}

		}
	}
	
	public int getRequestLevel(int modelID)
	{
		int level = 0;
		UnlockData ud = UnlockData.getData(modelID);
		if(ud == null)
			return 1000;
		level = ud.method;
		return level;
	}
	
	public bool isCanSendUnlockRequestMsg(int modelID)
	{
		if(	PlayerInfo.getInstance().player.level >= getRequestLevel(modelID))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public bool isCanUnlockFunctionByFinishMissionID(int modelID)
	{
		int missionID = PlayerInfo.getInstance().player.missionId;
		UnlockData ud = UnlockData.getData(modelID);
		if(ud == null)
			return false;
		if(ud.type != 1)
			return false;
		if(missionID >= ud.method)
			return true;
		return false;
	}
}
