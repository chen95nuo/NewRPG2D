using UnityEngine;
using System.Collections;


public class UISceneStateControl : BWUIPanel {
	/**
	 * UI场景的状态机，（动态加载）
	 */
	//要加载的场景的id//
	public enum UI_STATE_TYPE:int
	{
		UI_STATE_MAINMENU 		= 0,		//主界面//
		UI_STATE_CARDGROUP		= 1,		//卡组（阵容）//
		UI_STATE_CGINFO			= 2,		//卡牌详细//
		UI_STATE_PACK			= 3,		//背包//
		UI_STATE_INTENSIFY		= 4,		//吞噬//
		UI_STATE_COMPOSE		= 5,		//合成//
		UI_STATE_RUNE			= 6,		//符文//
		UI_STATE_CHALLENGE		= 7,		//挑战//
		UI_STATE_WROLDBOSS		= 8,		//世界boss//
		UI_STATE_ACTIVECOPY		= 9,		//活动副本//
		UI_STATE_SPRITEWORLD	= 10,		//灵界（冥想）//
		UI_STATE_EXCHANGE		= 11,  		//兑换//
		UI_STATE_WARPSPACE		= 12,		//扭曲空间//
		UI_STATE_MAZE			= 13,		//迷宫棋盘界面//
		UI_STATE_FRIEND			= 14,		//好友//
		UI_STATE_MAP			= 15,		//推图（战斗）//
		UI_STATE_LOT			= 16,		//召唤（抽卡）//
		UI_STATE_BREAKCARD		= 17,		//突破//
		UI_STATE_ACTIVE			= 18,		//活动//
		UI_STATE_ACHIVEMENT		= 19,		//成就//
		UI_STATE_ARENA			= 20,		//竞技场//
		UI_STATE_MAIL			= 21,		//邮件//
		UI_STATE_SIGN			= 22,		//签到//
		UI_STATE_CZ				= 23,		//充值//
		UI_STATE_GIFT			= 24,		//礼包//
		UI_STATE_SWEEP			= 25, 		//扫荡//
//		UI_STATE_GAMEHELP		= 26, 		//GameHelp//
//		UI_STATE_REQUESTUNLOCK	= 27,		//解锁新功能//
		UI_STATE_COPYSELECT		= 28,		//活动副本的难度选择//
		UI_STATE_SCROLLVIEW		= 29,		//scrollView界面//
		UI_STATE_VIEWCARD		= 30, 		//viewCardPanel//
		UI_STATE_HEADSETTING	= 31,		//头像设置区域//
        UI_STATE_SHOP           = 32,       //商城&黑市
		UI_STATE_TASK				= 33,		//任务//
		UI_STATE_GETWAYPANEL	= 34,		//显示合体技获得途径界面//
		UI_STATE_UNITESKILL		= 35,		//合体技界面//
	}
	
	
	
	
	private string mainMenuPath = "Prefabs/UI/MainMenu/MainMenuPanel";
	private string cgPath = "Prefabs/UI/EmbattlePanel/CombinationPanel";
	private string cgInfoPath = "Prefabs/UI/EmbattlePanel/CardInforPanel";
	private string packPath = "Prefabs/UI/UI-pack/UI-pack";
	private string intensifyPath = "Prefabs/UI/IntensifyPanel/IntensifyPanel";
	private string composePath = "Prefabs/UI/ComposePanel/ComposelPanel";
	private string runePath = "Prefabs/UI/UI-rune";
	private string challengePath = "Prefabs/UI/UI-main";
	private string activeCopyPath = "Prefabs/UI/ActiveWrold/UI-ActiveCopyPanel";
	private string spriteWorldPath = "Prefabs/UI/UI-Meditation/UI-MeditationPanel";
	private string exchangePath = "Prefabs/UI/UI-Exchange/UI-ExchangePanel";
	private string warpSpacePath = "Prefabs/UI/UI-Maze/UI-WarpSpacePanel";
	private string mazePath = "Prefabs/UI/UI-Maze/UI-MazePanel";
	private string friendPath = "Prefabs/UI/UI-friend/UI-friend";
	private string mapPath = "Prefabs/UI/UI-mission/UI-misson";
	private string lotCardPath = "Prefabs/UI/UI-LotCards/UI-LotCard";
	private string cardBreakPath = "Prefabs/UI/CardBreakPanel/CardBreakPanel";
	private string activePath = "Prefabs/UI/ActivityPanel/ActivityPanel";
	private string achivementPath = "Prefabs/UI/Achievement/AchievementPanel";
	private string arenaPath = "Prefabs/UI/UI-Arena/UI-ArenaPanel";
	private string mailPath = "Prefabs/UI/UI-mail/UI-mail";
	private string signPath = "Prefabs/UI/UI-sign/UI-sign";
	private string czPath = "Prefabs/UI/ChargePanel/ChargePanel";
    private string giftPath = "Prefabs/UI/UI-Gift/UI-Gift";
	private string sweepPath = "Prefabs/UI/UI-Sweep/UI-SweepPanel";
	private string copySelPath = "Prefabs/UI/ActiveWrold/UI-EventSelectPanel";
	private string scrollViewPath = "Prefabs/UI/ScrollViewPanel/ScrollViewPanel";
	private string viewCardPath = "Prefabs/UI/ViewCardPanel";
	private string headSettingPath = "Prefabs/UI/UI-HeadSettingPanel/UI-HeadSettingPanel";
    private string shopPath = "Prefabs/UI/UI-Shop/ShopPanel";
	private string taskPath = "Prefabs/UI/TaskPanel/TaskPanel";
	private string getWayPath = "Prefabs/UI/GetWayUI/GetWayDataPanel";
	private string UniteSkillPath = "Prefabs/UI/UniteSkill/UI-UniteSkillPanel";
	
	
	public static UISceneStateControl mInstace;
	public GameObject UIParent;

	public Hashtable stateHash = new Hashtable();
	
	
	void Awake()
	{
		mInstace = this;
		_MyObj = mInstace.gameObject;
		
		stateHash.Clear();
		//判断主城界面是否存在在场景中//
		for(int i = 0 ;i < UIParent.transform.childCount;i ++)
		{
			Transform trans = UIParent.transform.GetChild(i);
			MainMenuManager main = trans.GetComponent<MainMenuManager>();
			if(main!=null && !stateHash.ContainsKey(UI_STATE_TYPE.UI_STATE_MAINMENU))
			{
				stateHash.Add( UI_STATE_TYPE.UI_STATE_MAINMENU, trans.gameObject);
				trans.gameObject.SetActive(false);
			}
			
			GetWayPanelManager getWay = trans.GetComponent<GetWayPanelManager>();
			if(getWay!=null && !stateHash.ContainsKey(UI_STATE_TYPE.UI_STATE_GETWAYPANEL))
			{
				stateHash.Add( UI_STATE_TYPE.UI_STATE_GETWAYPANEL, trans.gameObject);
				trans.gameObject.SetActive(false);
			}
		}
		 
	}
	
	

	// Use this for initialization
	void Start () {

        Debug.Log("---------------------- PlayerInfo.getInstance().BattleOverBackType:" + PlayerInfo.getInstance().BattleOverBackType);
		if(PlayerInfo.getInstance().BattleOverBackType == 0 )		//当前界面是主场景界面//
		{
			ChangeState(UI_STATE_TYPE.UI_STATE_MAINMENU);
		}
		else if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_MAP)
		{
			ChangeState(UI_STATE_TYPE.UI_STATE_MAP);
		}
		else if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_MAZE)
		{
			ChangeState(UI_STATE_TYPE.UI_STATE_MAZE);
		}
		else if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_WRAPSPACE)
		{
			ChangeState(UI_STATE_TYPE.UI_STATE_WARPSPACE);
		}
		else if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_PVP)
		{
			ChangeState(UI_STATE_TYPE.UI_STATE_ARENA);
		}
		else if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_EVENT)
		{
            if (PlayerInfo.getInstance().isEvent)
                ChangeState(UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
            else
                ChangeState(UI_STATE_TYPE.UI_STATE_COPYSELECT);
		}
		else if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_QH)
		{
			ChangeState(UI_STATE_TYPE.UI_STATE_INTENSIFY);
		}
        else if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_ZH)
        {
            ChangeState(UI_STATE_TYPE.UI_STATE_LOT);
        }
	}
	
	public void ChangeState(UI_STATE_TYPE state)
	{
		if(stateHash.ContainsKey(state))
		{
			ShowObjByType(state);
			return;
		}
		GameObject prefab = null;
		GameObject obj = null;
		switch(state)
		{
		case UI_STATE_TYPE.UI_STATE_MAINMENU:
			prefab = Resources.Load(mainMenuPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
			
		case UI_STATE_TYPE.UI_STATE_CARDGROUP:
			prefab = Resources.Load(cgPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_CGINFO:
			prefab = Resources.Load(cgInfoPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_PACK:
			prefab = Resources.Load(packPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_INTENSIFY:
			prefab = Resources.Load(intensifyPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_COMPOSE:
			prefab = Resources.Load(composePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;	
		case UI_STATE_TYPE.UI_STATE_RUNE:
			prefab = Resources.Load(runePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_CHALLENGE:
			prefab = Resources.Load(challengePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_ACTIVECOPY:
			prefab = Resources.Load(activeCopyPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_SPRITEWORLD:
			prefab = Resources.Load(spriteWorldPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_EXCHANGE:
			prefab = Resources.Load(exchangePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
			
		case UI_STATE_TYPE.UI_STATE_WARPSPACE:
			prefab = Resources.Load(warpSpacePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_MAZE:
			prefab = Resources.Load(mazePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_FRIEND:
			prefab = Resources.Load(friendPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_MAP:
			prefab = Resources.Load(mapPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_LOT:
			prefab = Resources.Load(lotCardPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_BREAKCARD:
			prefab = Resources.Load(cardBreakPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_ACTIVE:
			prefab = Resources.Load(activePath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_ACHIVEMENT:
			prefab = Resources.Load(achivementPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_ARENA:
			prefab = Resources.Load(arenaPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_MAIL:
			prefab = Resources.Load(mailPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_SIGN:
			prefab = Resources.Load(signPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_CZ:
			prefab = Resources.Load(czPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			
			break;
		case UI_STATE_TYPE.UI_STATE_GIFT:
			prefab = Resources.Load(giftPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_SWEEP:
			prefab = Resources.Load(sweepPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_COPYSELECT:
			prefab = Resources.Load(copySelPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_SCROLLVIEW:
			prefab = Resources.Load(scrollViewPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_VIEWCARD:
			prefab = Resources.Load(viewCardPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_HEADSETTING:
			prefab = Resources.Load(headSettingPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
        case UI_STATE_TYPE.UI_STATE_SHOP:
            prefab = Resources.Load(shopPath) as GameObject;
            obj = Instantiate(prefab) as GameObject;
            break;
		case UI_STATE_TYPE.UI_STATE_TASK:
			prefab = Resources.Load(taskPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_GETWAYPANEL:
			prefab = Resources.Load(getWayPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		case UI_STATE_TYPE.UI_STATE_UNITESKILL:
			prefab = Resources.Load(UniteSkillPath) as GameObject;
			obj = Instantiate(prefab)as GameObject;
			break;
		}
		
		if(obj != null)
		{
			GameObjectUtil.gameObjectAttachToParent(obj, UIParent );
			obj.transform.localPosition= Vector3.zero;
			stateHash.Add(state, obj);
		}
		Resources.UnloadUnusedAssets();
	}
	
	public GameObject GetObjByType(UI_STATE_TYPE type)
	{
		GameObject obj = (GameObject)stateHash[type];
		return obj;
	}
	
	public Object GetComponentByType(UI_STATE_TYPE type, string name)
	{
		GameObject obj = GetObjByType(type);
		Object comp = null;
		if(obj!=null)
		{
			comp = obj.GetComponent(name);
			
		}
		return comp;
	}
	
	public void DestoryObj(UI_STATE_TYPE type)
	{
		GameObject obj = (GameObject)stateHash[type];
		stateHash.Remove(type);
		GameObject.Destroy(obj);
		Resources.UnloadUnusedAssets();
	}
	
	public void ShowObjByType(UI_STATE_TYPE type)
	{
		GameObject obj = (GameObject)stateHash[type];
		if(obj.activeSelf)
			return;
		obj.SetActive(true);
		obj.transform.localPosition = Vector3.zero;
	}
	
	
	public void HideObj(UI_STATE_TYPE type)
	{
		if(stateHash.ContainsKey(type))
		{
			
			GameObject obj = (GameObject)stateHash[type];
			if(obj!=null)
			{
				
				if(type == UI_STATE_TYPE.UI_STATE_MAINMENU)
				{
					MainMenuManager main = GetComponentByType(UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
					if(main != null)
					{
						main.hide();
					}
					if(Main3dCameraControl.mInstance != null)
					{
						Main3dCameraControl.mInstance.hide(); 
					}
				}
				obj.SetActive(false);
			}
		}
		
		gc();
	}
	
	void OnDestroy()
	{
		CleanData();
	}
	
	public void CleanData()
	{
		
		foreach(DictionaryEntry de in stateHash)
		{
			if((UI_STATE_TYPE)de.Key != UI_STATE_TYPE.UI_STATE_MAINMENU)
			{
				GameObject obj = (GameObject)de.Value;
				GameObject.Destroy(obj);
//				GameObject.DestroyImmediate(obj);
			}
			else 
			{
				MainMenuManager main = GetComponentByType(UI_STATE_TYPE.UI_STATE_MAINMENU , "MainMenuManager") as MainMenuManager;
				main.hide();
			}
		}
		stateHash.Clear();
		
		gc();
		mInstace = null;
	}
	
	
	public void gc()
	{
		//==释放资源==//

		Resources.UnloadUnusedAssets();
	}
		
}
