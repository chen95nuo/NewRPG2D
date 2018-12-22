using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>
/// PVE scene control.
/// cuixl @2013-08-16
/// </summary>
public class PVESceneControl : MonoBehaviour ,ProcessResponse
{
	public static PVESceneControl mInstance;
	//计时器//
	public float curTime;
	//0是己方，1是敌方//
	public Player[] players;
	public UnitSkillData[] unitSkills;
	
	// guide
	public UnitSkillData[] demoUnitSkillDatas = new 	UnitSkillData[2];
	public int demoPlayerAUnitSkillEnergy = 0;
	
	public int[] demoUnitSkillEnergys = new int[2];
	public int lastDemoUnitSkillIndex = -1;
	public UnitSkillData demoPlayerBUnitSkillData;
	public int demoPlayerBUnitSkillEnergy;
	
	//合体技队列//
	public Queue unitSkillQueue;
	//当前行动方//
	private int curSide;
	private Card curCard;
	/**标示回合暂停或继续**/
	private bool running;
	/**标示战斗是否结束**/
	bool isBattleOver=false;
	/**正在使用合体技**/
	private UnitSkill currentUnitSkill;
	private UnitSkill curTempUnitSkill;
	//小回合数//
	private int round=-1;
	
	bool isWaitForSetUnitSkill = false;
	bool isPlayBounes = false;
	bool unitKill=false;
	bool havePlayed=false;

	bool needRecoverSpeed = false;
	
	bool isShowKOTip = false;
	
	
	public GameObject hitNumParent;
	public UniteBtnManager uniteManager;
	public UIInterfaceManager uiManager;
	public EnergyManager energyManager;
	public GameObject BloodParent;
	public GameObject UIInterface;
	public bool[] canUnitSkill;
	public int battleResult ; //0 失败，1成功//
	
	private BattleLogJson blj;
	//迷宫战斗的战报//
	private MazeBattleLogJson mblj;
	//pvp战斗向服务器发送的战报 //
	private PkBattleLogJson pblj;
	//活动副本战斗的战报//
	private EventBattleLogJson eblj;
	
	//战斗日志//
	private List<string> battleDetails=new List<string>();
	
	/**发送数据相关**/
	private bool haveSended;
	private bool receiveData;
	private float delayTime=1f;
	private float allCardOverTime;
	
	public bool isDoComboAction = false;
	ArrayList aliveEnemyList = new ArrayList();
	//延迟2秒自动开始战斗//
	private bool begin;
	
	public GameObject curSceneObj;
	public GameObject curSkyBox;
	
	public bool needShowWinCameraEffect = false;
	
	string sceneName = string.Empty;
	int starNum = 3;
	//如果有bounes的话，bounes掉落物品领取标识，0未领取， 1已领取//
	int bounesDropGet ;
	//当前的missionId//
	int missionId;

	List<int> dialogBeginIDList = new List<int>();
	bool needShowBeginDialog = false;
	public bool finishShowBeginDialog = false;
	
	List<int> dialogWinIDList = new List<int>();
	bool needShowWinDialog = false;
	public bool finishShowWinDialog = false;
	
	List<int> dialogLoseIDList = new List<int>();
	bool needShowLoseDialog = false;
	public bool finishShowLoseDialog = false;
	
	List<int> dialogUnitSkillIDList = new List<int>();
	int unitSkillDialogIndex = 0;
	public bool needShowUnitSkillDialog = false; 
	public bool finishShowUnitSkillDialog = true;
	
	// to litao,control is can show dialog
	bool canShowDialog = true;
	
	bool needBornCard = false;
	bool bornAllEffectOk = false;
	bool startWaitBornCard = false;
	float bornCardTime = 0.2f;
	float bornEffectTime = 0.3f;
	int curBornCardIndex = -1;
	int curBornEffectIndex = -1;
	float curBornEffectTime = 0.0f;
	float curBornCardTime = 0.0f;
	
	public bool finishStartFightCameraShow = false;
	
	public bool waitStartFightUIEffect = false;
	
	CardJson[] bornCardJsons;
	int playerAUnitSkillID = 0;
	int bornTeam = 0;
	string bornRuneld = string.Empty;
	
	private bool haveHideFriend;
	private bool friendAppear;
	private float friendAppearTime;
	
	public bool guidePause = false;
	
	//设置合体技释放之前速率//
	float lastSpeedScale;
	
	private int cardNum;
	
	public Hashtable loadEffects=new Hashtable();
	
	bool needShowDemoBlack = false;
	
	public int requestType;
	
	bool needShowPlayerCards = false;
	
	int errorCode = 0;
	
	int maxEnergy=0;
	int initEnergy=0;
	
	bool waitForSetNextActionCardInfo = false;
	
	public GameObject sheepParentNode ;
	public List<Card> needRemoveSheeps;

    bool isEvent;

    public int hurt;
	
	void Awake()
	{
		mInstance = this;
	}

	// Use this for initialization
	void Start ()
	{
		needShowDemoBlack = false;
		begin = false;
		guidePause = false;
		isPlayBounes = false;
		allCardOverTime = 0;
		//load scene
		SceneInfo.getInstance().load();
		sceneName = string.Empty;
		//==================================设置双方数据 start===================================//
		
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			initNormalBattle();
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			initMazeBattle();
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
		{
			initPVPBattle();
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_EVENT)
		{
			initEventBattle();
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_DEMO)
		{
			initDemoBattle();
		}
		
		//born playerA card
		players[0].setBornCardParam(bornCardJsons,bornTeam,bornRuneld,maxEnergy,initEnergy);	
		players[0].initUnitSkillShowCards(bornCardJsons,0,playerAUnitSkillID);
		needBornCard = true;
		curBornEffectTime = 0.0f;
		curBornCardTime = 0.0f;
		curBornEffectIndex = 0;
		startWaitBornCard = false;
		players[0].bornEffectByIndex(curBornEffectIndex);
		ViewControl.mInstacne.playStartFightCameraPath();
		
		if(sceneName != string.Empty)
		{
			string scenePathName = "Prefabs/Scene/" + sceneName;
			GameObject sceneObj = Resources.Load(scenePathName) as GameObject;
			curSceneObj = GameObject.Instantiate(sceneObj) as GameObject;
			curSkyBox = GameObjectUtil.findGameObjectByName(curSceneObj,sceneName+"skybox");
			
			//播放声音//
			string musicName = "";
			if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
			{
				musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_PVP).music;
			}
			else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
			{
				musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MAZE).music;
			}
			else 
			{
				musicName = MusicData.getIdBySceneName(sceneName).music;
			}
			MusicManager.playBgMusic(musicName);
			showFog();
		}
		//开启日志//
		BattleLog.getInstance().logStart();
		//初始化怒气槽//
		energyManager.init(players[0].getEnergy(), players[0].getMaxEnergy());
		//开启战斗//
		running=false;
		needRecoverSpeed = true;
	}
	
	void initNormalBattle()
	{
		BattleResultJson brj=PlayerInfo.getInstance().brj;
		maxEnergy=brj.mes[0];
		initEnergy=brj.initE;
		//记录bounes领取标识//
		bounesDropGet = brj.bs;
		//创建玩家A//
		Player playerA=new Player();
		//设置角色卡//
		bornCardJsons = brj.cs0;
		bornTeam = 0;
		bornRuneld = PlayerInfo.getInstance().player.runeId;
		//设置合体技//
		UnitSkillData[] us0=new UnitSkillData[3];
		if(brj.us0!=null)
		{
			for(int k=0;k<3;k++)
			{
				us0[k]=UnitSkillData.getData(brj.us0[k]);
			}
		}
		playerA.raceAtts=brj.raceAtts;
		playerA.setUnitSkills(us0);
		playerAUnitSkillID = brj.us0[0];
		//创建玩家B//
		Player playerB=new Player();
		playerB.init(brj.cs1,1,null,brj.mes[1],0);
		UnitSkillData[] us1=new UnitSkillData[3];
		if(brj.us1!=null)
		{
			for(int k=0;k<3;k++)
			{
				us1[k]=UnitSkillData.getData(brj.us1[k]);
			}
		}
		playerB.setUnitSkills(us1);
		//==================================设置双方数据 end=====================================//
		//初始化合体技队列
		Queue queue=new Queue();
		unitSkillQueue=Queue.Synchronized(queue);
		//初始化屏幕合体技,合体技可释放标记,合体技按钮控制,怒气槽//
		unitSkills=us0;
		canUnitSkill=new bool[3];
		//==设置好友卡牌,释放好友合体技时出来,打完就消失==//
		
		missionId = brj.md;
		if(missionId < STATE.BATTEL_WITH_FRIEDN_ID && GuideManager.getInstance().getCurrentGuideID()<=(int)GuideManager.GuideType.E_Battle3_Friend)
		{
			players=new Player[]{playerA,playerB};
			uniteManager.init();
			EnergyManager.mInstance.init();				
		}
		else
		{
			Player friend=new Player();
			friend.initFriend(brj.fs,brj.fr);
			friend.initUnitSkillShowCards(brj.fs,0,brj.us0[2]);
			players=new Player[]{playerA,playerB,friend};
			uniteManager.init();
			EnergyManager.mInstance.init();
		}

		//初始化验证json//
		blj=new BattleLogJson();
		blj.bNum=brj.bNum;
		blj.gm=1+"";
		//将该标识初始化，//
		blj.bonus = 0;
		MissionData missionData = MissionData.getData(brj.md);
		if(missionData != null)
		{
			sceneName = missionData.scene;
		}
		canShowDialog = brj.t==1;
		if(canShowDialog)
		{
			dialogBeginIDList = TaskData.getTaskIDArray(brj.md,(int)DialogPanel.DIALOGTYPE.E_Begin);
			if(dialogBeginIDList.Count > 0)
			{
				needShowBeginDialog = true;
				finishShowBeginDialog = false;
			}
			else
			{
				finishShowBeginDialog = true;
			}
			dialogWinIDList = TaskData.getTaskIDArray(brj.md,(int)DialogPanel.DIALOGTYPE.E_Win);
			if(dialogWinIDList.Count > 0)
			{
				needShowWinDialog = true;
			}
			dialogLoseIDList = TaskData.getTaskIDArray(brj.md,(int)DialogPanel.DIALOGTYPE.E_Lose);
			if(dialogLoseIDList.Count > 0)
			{
				needShowLoseDialog = true;
			}
			finishShowWinDialog = true;
			finishShowLoseDialog = true;
			if(needShowWinDialog || needShowLoseDialog)
			{
				finishShowWinDialog = false;
				finishShowLoseDialog = false;
			}
		}
		else
		{
			needShowBeginDialog = false;
			needShowWinDialog = false;
			needShowLoseDialog = false;
			finishShowBeginDialog = true;
			finishShowWinDialog = true;
			finishShowLoseDialog = true;
		}
		
	}
	
	void initPVPBattle()
	{
		PkBattleResultJson pbrj=PlayerInfo.getInstance().pbrj;
		maxEnergy=pbrj.mes[0];
		initEnergy=pbrj.initEs[0];
		//创建玩家A//
		Player playerA=new Player();
		//设置角色卡//
		bornCardJsons = pbrj.cs0;
		bornTeam = 0;
		bornRuneld = PlayerInfo.getInstance().player.runeId;
		//设置合体技//
		UnitSkillData[] us0=new UnitSkillData[3];
		if(pbrj.us0!=null)
		{
			for(int k=0;k<3;k++)
			{
				us0[k]=UnitSkillData.getData(pbrj.us0[k]);
			}
		}
		playerA.raceAtts=pbrj.raceAtts1;
		playerA.setUnitSkills(us0);
		playerAUnitSkillID = pbrj.us0[0];
		//创建玩家B//
		Player playerB=new Player();
		playerB.init(pbrj.cs1,1,pbrj.runes[1],pbrj.mes[1],pbrj.initEs[1]);
		playerB.initUnitSkillShowCards(pbrj.cs1,1,pbrj.us1[0]);
		UnitSkillData[] us1=new UnitSkillData[3];
		if(pbrj.us1!=null)
		{
			for(int k=0;k<3;k++)
			{
				us1[k]=UnitSkillData.getData(pbrj.us1[k]);
			}
		}
		playerB.raceAtts=pbrj.raceAtts2;
		playerB.setUnitSkills(us1);
		players=new Player[]{playerA,playerB};
		//==================================设置双方数据 end=====================================//
		//初始化合体技队列
		Queue queue=new Queue();
		unitSkillQueue=Queue.Synchronized(queue);
		//初始化屏幕合体技,合体技可释放标记,合体技按钮控制,怒气槽//
		unitSkills=us0;
		canUnitSkill=new bool[3];
		uniteManager.init();
		EnergyManager.mInstance.init();
		//初始化验证json//
		pblj=new PkBattleLogJson();
		pblj.bNum=pbrj.bNum;
		sceneName = "c03";
		notShowDialog();
		//==战力大的一方先出手==//
		int[] battlePowers=pbrj.bps;
		if(battlePowers[0]>=battlePowers[1])
		{
			curSide=0;
		}
		else
		{
			curSide=1;
		}
		StartFightPanel.mInstance.setPVPInfo(pbrj.ns[0],pbrj.lvs[0],pbrj.bps[0],pbrj.ns[1],pbrj.lvs[1],pbrj.bps[1]);
	}
	
	void initMazeBattle()
	{
		MazeBattleResultJson mbrj=PlayerInfo.getInstance().mbrj;
		maxEnergy=mbrj.mes[0];
		initEnergy=mbrj.initE;
		//创建玩家A//
		Player playerA=new Player();
		//设置角色卡//
		bornCardJsons = mbrj.cs0;
		bornTeam = 0;
		bornRuneld = PlayerInfo.getInstance().player.runeId;
		//设置合体技//
		UnitSkillData[] us0=new UnitSkillData[3];
		if(mbrj.us0!=null)
		{
			for(int k=0;k<3;k++)
			{
				us0[k]=UnitSkillData.getData(mbrj.us0[k]);
			}
		}
		playerA.raceAtts=mbrj.raceAtts;
		playerA.setUnitSkills(us0);
		playerAUnitSkillID = mbrj.us0[0];
		//创建玩家B//
		Player playerB=new Player();
		playerB.init(mbrj.cs1,1,null,mbrj.mes[1],0);
		UnitSkillData[] us1=new UnitSkillData[3];
		if(mbrj.us1!=null)
		{
			for(int k=0;k<3;k++)
			{
				us1[k]=UnitSkillData.getData(mbrj.us1[k]);
			}
		}
		playerB.setUnitSkills(us1);
		players=new Player[]{playerA,playerB};
		//==================================设置双方数据 end=====================================//
		//初始化合体技队列
		Queue queue=new Queue();
		unitSkillQueue=Queue.Synchronized(queue);
		//初始化屏幕合体技,合体技可释放标记,合体技按钮控制,怒气槽//
		unitSkills=us0;
		canUnitSkill=new bool[3];
		uniteManager.init();
		EnergyManager.mInstance.init();
		//初始化验证json//
		mblj=new MazeBattleLogJson();
		mblj.bNum=mbrj.bNum;
		mblj.gm=1+"";
		mblj.type = PlayerInfo.getInstance().mazeBattleType;
		mblj.map = PlayerInfo.getInstance().curMazeId;
		MazeData md = MazeData.getData(mbrj.td);
		if(md != null)
		{
			sceneName = md.scene;
		}
		notShowDialog();
	}
	
	void initDemoBattle()
	{
		needShowDemoBlack = true;
		NewPlayerBattleResultJson npbrj=PlayerInfo.getInstance().npbrj;
		maxEnergy=npbrj.mes[0];
		initEnergy=npbrj.initE;
		//创建玩家A//
		Player playerA=new Player();
		//设置角色卡//
		bornCardJsons = npbrj.cs0;
		bornTeam = 0;
		bornRuneld = string.Empty;
		
		UnitSkillData[] us0Array = new UnitSkillData[3];
		if(npbrj.us0 != null)
		{
			for(int i = 0; i < npbrj.us0.Count; ++i)
			{
				if(i >= 1)
					continue;
				string unitString = npbrj.us0[i];
				string[] tempStr = unitString.Split('-');
				int unitSkillID = StringUtil.getInt(tempStr[0]);
				UnitSkillData usd = UnitSkillData.getData(unitSkillID);
				if(usd == null)
				{
					continue;
				}
				us0Array[i] = usd;
				demoPlayerAUnitSkillEnergy = StringUtil.getInt(tempStr[1]);
			}
		}
		playerA.setUnitSkills(us0Array);
		unitSkills = us0Array;
		//创建玩家B//
		Player playerB=new Player();
		playerB.init(npbrj.cs1,1,null,npbrj.mes[1],0);
		
		List<DemoBattleUnitSkillDataInfo> demoList1= new List<DemoBattleUnitSkillDataInfo>();
		if(npbrj.us1 != null)
		{
			for(int i = 0; i < npbrj.us1.Count; ++i)
			{
				DemoBattleUnitSkillDataInfo info = new DemoBattleUnitSkillDataInfo();
				string unitString = npbrj.us1[i];
				string[] tempStr = unitString.Split('-');
				int unitSkillID = StringUtil.getInt(tempStr[0]);
				UnitSkillData usd = UnitSkillData.getData(unitSkillID);
				if(usd == null)
				{
					continue;
				}
				info.usd = usd;
				info.energy = StringUtil.getInt(tempStr[1]);
				demoList1.Add(info);
			}
		}
		playerB.setDemoUnitSkills(demoList1);

		players=new Player[]{playerA,playerB};
		uniteManager.init();
		EnergyManager.mInstance.init();
		UIInterfaceManager.mInstance.skipBtn.SetActive(true);
		//==================================设置双方数据 end=====================================//
		//初始化合体技队列//
		Queue queue=new Queue();
		unitSkillQueue=Queue.Synchronized(queue);

		demoPlayerBUnitSkillData = demoList1[0].usd;
		demoPlayerBUnitSkillEnergy = demoList1[0].energy;

		canUnitSkill=new bool[3];
		// demo dialog
		{
			dialogBeginIDList = TaskData.getTaskIDArray(100000,(int)DialogPanel.DIALOGTYPE.E_Begin);
			if(dialogBeginIDList.Count > 0)
			{
				needShowBeginDialog = true;
				finishShowBeginDialog = false;
			}
			else
			{
				finishShowBeginDialog = true;
			}
			dialogLoseIDList = TaskData.getTaskIDArray(100000,(int)DialogPanel.DIALOGTYPE.E_Lose);
			if(dialogLoseIDList.Count > 0)
			{
				needShowLoseDialog = true;
			}
			finishShowWinDialog = true;
			finishShowLoseDialog = true;
			if(needShowWinDialog || needShowLoseDialog)
			{
				finishShowWinDialog = false;
				finishShowLoseDialog = false;
			}
			dialogUnitSkillIDList = TaskData.getTaskIDArray(100000,(int)DialogPanel.DIALOGTYPE.E_UnitSkill);
		}
		
		blj=new BattleLogJson();
		blj.r = 3;// demo battle flag
		uiManager.addSpeedCtrl.SetActive(false);
		sceneName = "c03";
	}
	
	void initEventBattle()
	{
		EventBattleResultJson ebrj=PlayerInfo.getInstance().ebrj;
		maxEnergy=ebrj.mes[0];
		initEnergy=ebrj.initE;
		//创建玩家A//
		Player playerA=new Player();
		//设置角色卡//
		bornCardJsons = ebrj.cs0;
		bornTeam = 0;
		bornRuneld = PlayerInfo.getInstance().player.runeId;
		//设置合体技//
		UnitSkillData[] us0=new UnitSkillData[3];
		if(ebrj.us0!=null)
		{
			for(int k=0;k<3;k++)
			{
				us0[k]=UnitSkillData.getData(ebrj.us0[k]);
			}
		}
		playerA.raceAtts=ebrj.raceAtts;
		playerA.setUnitSkills(us0);
		playerAUnitSkillID = ebrj.us0[0];
		//创建玩家B//
		Player playerB=new Player();
		playerB.init(ebrj.cs1,1,null,ebrj.mes[1],0);
		UnitSkillData[] us1=new UnitSkillData[3];
		if(ebrj.us1!=null)
		{
			for(int k=0;k<3;k++)
			{
				us1[k]=UnitSkillData.getData(ebrj.us1[k]);
				if(us1[k]!=null)
				{
					Debug.Log("us1:"+us1[k].name);
				}
			}
		}
		playerB.setUnitSkills(us1);
		players=new Player[]{playerA,playerB};
		//==================================设置双方数据 end=====================================//
		//初始化合体技队列//
		Queue queue=new Queue();
		unitSkillQueue=Queue.Synchronized(queue);
		//初始化屏幕合体技,合体技可释放标记,合体技按钮控制,怒气槽//
		unitSkills=us0;
		canUnitSkill=new bool[3];
		uniteManager.init();
		EnergyManager.mInstance.init();
		//初始化验证json//
		eblj=new EventBattleLogJson();
		eblj.bNum=ebrj.bNum;
		FBeventData fbed = FBeventData.getData(ebrj.id);
        if (fbed.leveltype == 1)
        {
            isEvent = true;
            eblj.t = hurt;
        }
		if(fbed != null)
		{
			sceneName = fbed.scene;
		}
		notShowDialog();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			bool busy = false;
			if(!isBattleOver)
			{
				if(players==null)
				{
					return;
				}
				foreach(Player p in players)
				{
					if(p.getCards()==null)
					{
						continue;
					}
					foreach(Card c in p.getCards())
					{
						if(c!=null)
						{
							if(c.isRunning())
							{
								busy = true;
								break;
							}
						}
					}
				}
			}
			
			if(!busy)
			{
				if(needShowUnitSkillDialog)
				{
					needShowUnitSkillDialog = false;
					List<int> usIDList = new List<int>();
					int dID = dialogUnitSkillIDList[unitSkillDialogIndex];
					unitSkillDialogIndex++;
					usIDList.Add(dID);
					DialogPanel.mInstance.setDialogIDList(usIDList,DialogPanel.DIALOGTYPE.E_UnitSkill);
					DialogPanel.mInstance.show();
					if(ViewControl.mInstacne.isRunNormalCamareMove())
					{
						ViewControl.mInstacne.stopNormalCamareMove();
					}
					return;
				}
				else
				{
					if(!finishShowUnitSkillDialog)
					{
						return;
					}
					else
					{
						hitNumParent.SetActive(true);
						UIInterface.SetActive(true);
						BloodParent.SetActive(true);
					}
				}
				if(running)
				{
					if(canUnitSkill[0] && players[0].getEnergy() >= demoPlayerAUnitSkillEnergy)
					{
						bool isPointed = BattleGuidePointUnitSkill.mInstance.getIsPointed();
						if(!isPointed)
						{
							if(ViewControl.mInstacne.isRunNormalCamareMove())
							{
								ViewControl.mInstacne.stopNormalCamareMove();
							}
							BattleGuidePointUnitSkill.mInstance.showStep(0);	
							guidePause = true;
						}
					}
				}
			}
			
		}
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill)
				||GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
		{
			if(running)
			{
				if(canUnitSkill[0] && players[0].getEnergy() >= unitSkills[0].cost)
				{
					bool isPointed = BattleGuidePointUnitSkill.mInstance.getIsPointed();
					if(!isPointed)
					{
						if(ViewControl.mInstacne.isRunNormalCamareMove())
						{
							ViewControl.mInstacne.stopNormalCamareMove();
						}
						BattleGuidePointUnitSkill.mInstance.showStep(0);	
						guidePause = true;
					}
				}
			}
		}
		
		if(guidePause )
		{
			return;
		}
		
		if(!haveHideFriend)
		{
			haveHideFriend=true;
			//==隐藏好友==//
			if(players.Length==3 && players[2]!=null)
			{
				players[2].hideCards();
			}
		}
		
		if(receiveData && !isPlayBounes)
		{
			bool isShowQHBtn = true;
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:
			{
				//储存战斗结束时血量数据//
				requestType = 2;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PLAYER),this);
			}break;
			case 2:
			{
				//隐藏上下ui//
				if(uiManager != null)
				{
					uiManager.UpContrl(false);
					uiManager.DownContrl(false);
				}
				
				if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
				{
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Demo);
					PlayerInfo.isFirstLogin = true;
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_UI);
					return;
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill))
				{
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Battle1_UnitSkill);
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
				{
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Battle2_Bounes);
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle3_Friend))
				{
					GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Battle3_Friend);
				}
				else if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
				{
					isShowQHBtn = false;
					
					if(mblj.r == 2)
					{
						GuideManager.getInstance().isMazeFail = true;
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_WarpSpace);
						// run equip skill guide
						GuideManager.getInstance().runGuide();
					}
					else if(GuideManager.getInstance().isMazeBoss )
					{
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_WarpSpace);
						// run equip skill guide
						GuideManager.getInstance().runGuide();
					}
					
					
				}
				bool isShowKO = false;
				if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
				{
					float mul = StringUtil.getFloat(blj.gm);
					if(mul == 3.5f)
					{
						isShowKO = true;
					}
					else 
					{
						isShowKO = false;
					}
				}
				if(PlayerInfo.getInstance().battleType != STATE.BATTLE_TYPE_DEMO)
				{

                    ResultTipManager.mInstance.setData(starNum, Battle_Player_Info.GetInstance().BattleResult, isShowKO, isShowQHBtn, isEvent ? true : false);
					//清除合体技按钮上的特效//
					if(UniteBtnManager.mInstance!=null)
					{
						UniteBtnManager.mInstance.RemovAllEff();
					}
				}
			}break;
			}
		}
		
		/**等待卡牌出生**/
		if(needBornCard)
		{
			if(curBornEffectTime >= bornEffectTime)
			{
				startWaitBornCard = true;
				curBornEffectTime = 0;
				curBornCardTime = 0;
				players[0].bornEffectByIndex(curBornEffectIndex);
				curBornCardIndex = curBornEffectIndex;
				curBornEffectIndex++;
				if(!players[0].checkIsCanGoOnBornCard(curBornEffectIndex))
				{
					bornAllEffectOk = true;
				}
			}
			else
			{
				curBornEffectTime += Time.deltaTime;
			}
			if(startWaitBornCard)
			{
				if(curBornCardTime >= bornCardTime)
				{
					startWaitBornCard = false;
					curBornCardTime = 0;
					players[0].bornCardByIndex(curBornCardIndex);
				}
				else
				{
					curBornCardTime += Time.deltaTime;
				}
			}
			if(bornAllEffectOk && !startWaitBornCard)
			{
				needBornCard = false;
				players[0].clearBornEffect();
				players[0].setCardAttibute();
			}
			return;
		}
		/**等待开场摄像机效果**/
		if(!finishStartFightCameraShow)
		{
			return;	
		}
		
		/**等待开场剧情**/
		if(needShowBeginDialog )
		{
			needShowBeginDialog = false;
			DialogPanel.mInstance.setDialogIDList(dialogBeginIDList,DialogPanel.DIALOGTYPE.E_Begin);
			DialogPanel.mInstance.show();
		}
		else
		{
			if(finishShowBeginDialog)
			{
				/**等待开场UI效果**/
				if(waitStartFightUIEffect)
				{
					waitStartFightUIEffect = false;
					if(!StartFightPanel.mInstance.isVisible())
					{
						StartFightPanel.mInstance.show();
						//播放音效//
						MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_STARTFIGHT);
					}
				}
				//延迟2秒自动战斗//
				curTime+=Time.deltaTime;
				if(!begin && curTime>=2f)
				{
					begin=true;
					running=true;
					ViewControl.mInstacne.startWaitNormalCamareMove();
					return;
				}
			}
		}
		
		//战斗结束//
		if(isBattleOver )
		{
			//==等待所有动作结束==//
			if(currentUnitSkill!=null)
			{
				return;
			}
			if(curCard!=null && curCard.isRunning())
			{
				return;
			}
			if(needRecoverSpeed)
			{
				needRecoverSpeed = false;
//				UIInterfaceManager.mInstance.AddSpeedBtn(1);
				UIInterfaceManager.mInstance.doSpeedChange(STATE.SPEED_NORMAL);
			}
			//最后一击是用合体技击杀，并且没有进入过bonus时，才会出现bounes, 只有在pve战斗中才会出现//
			if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL
				&& blj!=null
				&& blj.r==1
				&& !havePlayed
				&& (!isShowKOTip && UIInterfaceManager.mInstance.GetKOTipType() == -1)			//没有出现ko提示界面的情况下//
				&& players!=null
				&& !GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo)
				&& !GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle1_UnitSkill)
				)
			{
				int missionId=PlayerInfo.getInstance().brj.md;
				isPlayBounes=Statics.checkKO(missionId,GetBounsDropMark(),unitKill,players[0],getRound());
				MissionData md = MissionData.getData(missionId);
				if(md.addtasktype>0 && GetBounsDropMark()==0)		//有ko条件//
				{
					isShowKOTip = true;
					if(isPlayBounes)			//达成ko条件//
					{
						UIInterfaceManager.mInstance.showKOTip(1);
						
					}
					else 
					{
						UIInterfaceManager.mInstance.showKOTip(0);
					}
				}
				if(isPlayBounes)
				{
					havePlayed=true;
				}
			}
			
			if(isShowKOTip)
			{
				return ;
			}

			if(isPlayBounes && !BattleBounesPanel.mInstance.gameObject.activeSelf)
			{
				if(ViewControl.mInstacne.isRunNormalCamareMove())
				{
					ViewControl.mInstacne.stopNormalCamareMove();
				}
				string modelName = "";
				if(GetAliveEnemyList().Count>0){
					Card ca = (Card)GetAliveEnemyList()[0];
					modelName = ca.getCardData().cardmodel;
				}
				int bounesMark = PVESceneControl.mInstance.GetBounsDropMark();
				BattleBounesPanel.mInstance.SetData(modelName, bounesMark);
				//只要出现过bonus就不再出现//
				blj.bonus = 1;
				//						BattleBounesPanel.mInstance.show();
				//清除合体技按钮上的特效//
				if(UniteBtnManager.mInstance!=null)
				{
					UniteBtnManager.mInstance.RemovAllEff();
				}
				return;
			}
			if(isPlayBounes)
			{
				return;
			}
			if(!haveSended)
			{
				foreach(Player p in players)
				{
					if(p.getCards()==null)
					{
						continue;
					}
					foreach(Card c in p.getCards())
					{
						if(c!=null)
						{
							if(c.isRunning())
							{
								return;
							}
						}
					}
				}
				if(ViewControl.mInstacne.isRunNormalCamareMove())
				{
					ViewControl.mInstacne.stopNormalCamareMove();
				}
				/**等待结束剧情**/
				if(needShowWinDialog || needShowLoseDialog)
				{
					finishShowWinDialog = true;
					finishShowLoseDialog = true;
					
					if(needShowWinDialog && needShowWinCameraEffect)
					{
						finishShowWinDialog = false;
						DialogPanel.mInstance.setDialogIDList(dialogWinIDList,DialogPanel.DIALOGTYPE.E_Win,true);
						DialogPanel.mInstance.show();
					}
					if(needShowLoseDialog && !needShowWinCameraEffect)
					{
						finishShowLoseDialog = false;
						DialogPanel.mInstance.setDialogIDList(dialogLoseIDList,DialogPanel.DIALOGTYPE.E_Lose);
						DialogPanel.mInstance.show();
					}
					needShowWinDialog = false;
					needShowLoseDialog = false;
				}
				else
				{
					if(finishShowWinDialog&& finishShowLoseDialog)
					{
						//隐藏上下ui//
						if(uiManager != null)
						{
							uiManager.UpContrl(false);
							uiManager.DownContrl(false);
						}
						if(needShowWinCameraEffect )
						{
							//播放胜利音效//
							string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_WIN).music;
							MusicManager.playBgMusic(musicName);
							
							//删除我方卡牌身上的所有特效//
							Card[] cards = players[0].getCards();
							for(int i = 0;i<cards.Length;i++){
								Card cd = cards[i];
								if(cd != null){
									cd.CleanEffect();
								}
							}
							players[0].playWinAction();
							needShowWinCameraEffect = false;
							ViewControl.mInstacne.playWinCameraPath();
						}
						if(needShowDemoBlack)
						{
							if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
							{
								uiManager.showDemoBattleTopBlack();
							}
							needShowDemoBlack =false;
						}
						if(allCardOverTime<delayTime)
						{
							allCardOverTime+=Time.deltaTime;
						}
						else
						{
							/**播放完动画后发送**/
							haveSended=true;
							//设置星级//
							SetBattleStarNum();
							sendBattleResult();
						}
					}
				}
			}
			return;
		}
		if(running)
		{
			running=false;
			//第一回合之前要把所有的T激活//
			if(round<0)
			{
				for(int k=0;k<players.Length;k++)
				{
					if(k>=2)
					{
						continue;
					}
					Player p=players[k];
					for(int i=0;i<p.getCards().Length;i++)
					{
						Card c=p.getCards()[i];
						if(c!=null && c.getSkill().type==2)
						{
							c.setActive(1);
							c.ignoreT=true;
							List<int> targetIndexs2=Statics.getTargetIndexs(i,p.getCards(),null);
							processTarget(i,p,null,targetIndexs2);
						}
					}
				}
				changeRound(0);
				return;
			}
			//检查电脑人的合体技//
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
			{
				DemoBattleUnitSkillDataInfo info = players[1].getDemoUnitSkillList()[0];
				if(players[1].getEnergy() >= demoPlayerBUnitSkillEnergy)
				{
					players[1].removeEnergy(demoPlayerBUnitSkillEnergy);
					unitSkillQueue.Enqueue(new UnitSkill(1,info.usd,this,false));
					needShowUnitSkillDialog = true;
					finishShowUnitSkillDialog = false;
				}
			}
			else
			{
				UnitSkillData[] us1=players[1].getUnitSkills();
				if(us1!=null && us1.Length>0)
				{
					UnitSkillData usd=us1[0];
					if(usd!=null)
					{
						if(players[1].getEnergy()>=usd.cost)
						{
							players[1].removeEnergy(usd.cost);
							//==pvp对方可以放合体技==//
							if(PlayerInfo.getInstance().battleType==3)
							{
								unitSkillQueue.Enqueue(new UnitSkill(1,usd,this,false));
							}
						}
					}
				}
			}
	
			if(needShowUnitSkillDialog)
			{
				needShowUnitSkillDialog = false;
				List<int> usIDList = new List<int>();
				int dID = dialogUnitSkillIDList[unitSkillDialogIndex];
				unitSkillDialogIndex++;
				usIDList.Add(dID);
				DialogPanel.mInstance.setDialogIDList(usIDList,DialogPanel.DIALOGTYPE.E_UnitSkill);
				DialogPanel.mInstance.show();
				if(ViewControl.mInstacne.isRunNormalCamareMove())
				{
					ViewControl.mInstacne.stopNormalCamareMove();
				}
				return;
			}
			else
			{
				if(!finishShowUnitSkillDialog)
				{
					return;
				}
			}
			
			if(currentUnitSkill!=null  )
			{
				return;
			}
			else
			{
				//==如果curCard=null合体技会卡住,此处设置curCard只为防止合体技卡住,无其他意图==//
				if(curCard==null)
				{
					curCard=players[0].getOneLiveCard();
				}
				
				//==这里只把对面的当做敌人==//
				recordEnemys(players[1]);
				//合体技开始//
				if(unitSkillQueue.Count>0&& !isWaitForSetUnitSkill && !isDoComboAction)
				{
					UnitSkill unitSkill=(UnitSkill)unitSkillQueue.Dequeue();
					UnitSkillData unitSkillData=unitSkill.getUnitSkill();
				
					//复活//
					if(unitSkillData.type==5 && !players[unitSkill.getTeam()].haveDeadCard())
					{
						return;
					}
					if(players[unitSkill.getOtherTeam()].isDead())
						return;
					
					lastSpeedScale = UIInterfaceManager.mInstance.getCurScale();
					NewBattleUnitePanel.mInstance.SetData(1,unitSkillData.index);
					isWaitForSetUnitSkill = true;
					curTempUnitSkill=unitSkill;
					
					//将时间流速改成1,合体技释放的过程中不要加速//
					UIInterfaceManager.mInstance.doSpeedChange(STATE.SPEED_BATTLE_NORMAL);
					//隐藏ui以及血条//
					UIInterface.SetActive(false);
					BloodParent.SetActive(false);
					hitNumParent.SetActive(false);
					if(unitSkill.isHelpSkill())
					{
						UniteBtnManager.mInstance.hideFriendSkillBtn();
					}
				}
				if(isWaitForSetUnitSkill)
				{
					if(!NewBattleUnitePanel.mInstance.gameObject.activeSelf)
					{
						UnitSkill unitSkill=curTempUnitSkill;
						if(unitSkill.isHelpSkill())
						{
							//==好友合体技开始==//
							if(!friendAppear)
							{
								friendAppear=true;
								//==自己的人消失,出现特效==//
								players[0].friendStart();
								players[2].showUnitSkillCards();
							}
							//==好友出现等待==//
							if(friendAppear)
							{
								friendAppearTime+=Time.deltaTime;
								if(friendAppearTime<0.5f)
								{
									return;
								}
							}
						}
						needShowPlayerCards = true;
						isWaitForSetUnitSkill = false;
						UnitSkillData unitSkillData=unitSkill.getUnitSkill();
						if(true)
						{
							if(unitSkillData.cardtype == 1)
							{
							
							}
							else
							{
								//初始化合体技//
								Player srcP=players[unitSkill.getTeam()];
								Player ownP = null;
								if(unitSkill.isHelpSkill())
								{
									srcP = players[2];
									ownP = players[0];
								}
								Player targetP=players[(unitSkill.getTeam()+1)%2];
								List<Card> targetCards=Statics.getUnitSkillTargets(unitSkillData.aim,srcP,targetP,ownP);
								List<Card> unitSkillShowCards = new List<Card>();
								if((unitSkillData.index == UnitSkillData.defaultUnitSkillId1) || (unitSkillData.index == UnitSkillData.defaultUnitSkillId2))
								{
									isDoComboAction = true;
									List<int> srcIDList = new List<int>();
									UnitSkillData comboUSD = UnitSkillData.getData(unitSkillData.index);
									if(comboUSD == null)
									{
										return;
									}
									for(int i = 0; i < comboUSD.cards.Length;++i)
									{
										if( comboUSD.cards[i] != 0)
										{
											srcIDList.Add(comboUSD.cards[i]);
										}
									}
									if(unitSkill.isHelpSkill())
									{
										srcP=players[2];
									}
									
									List<int> targetIDList = new List<int>();
									for(int i = 0; i < targetCards.Count;++i)
									{
										Card c= (Card)targetCards[i];
										int id = 0;
										if(c != null)
										{
											id = c.getCardData().id;
										}
										targetIDList.Add(id);
									}
									ActionCameraControl.COMBOTYPE cType = ActionCameraControl.COMBOTYPE.E_Combo3;
									if(unitSkillData.index == UnitSkillData.defaultUnitSkillId1)
									{
										if(unitSkill.getTeam() == 0)
										{
											cType = ActionCameraControl.COMBOTYPE.E_Combo3;
											ActionCameraControl.mInstance.showComboAction(srcIDList,targetIDList,cType);
										}
										else if(unitSkill.getTeam() == 1)
										{
											cType = ActionCameraControl.COMBOTYPE.E_Combo3_Oppsite;
											ActionCameraControl.mInstance.showComboAction(targetIDList,srcIDList,cType);
										}
									}
									else if(unitSkillData.index == UnitSkillData.defaultUnitSkillId2)
									{
										if(unitSkill.getTeam() == 0)
										{
											cType = ActionCameraControl.COMBOTYPE.E_Combo6;
											ActionCameraControl.mInstance.showComboAction(srcIDList,targetIDList,cType);
										}
										else if(unitSkill.getTeam() == 1)
										{
											cType = ActionCameraControl.COMBOTYPE.E_Combo6_Oppsite;
											ActionCameraControl.mInstance.showComboAction(targetIDList,srcIDList,cType);
										}
									}
									return;
								}
								if(!GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
								{
									hideNotCastUnitSkillCards(srcP.getCards());
									List<Card> tempCardList = new List<Card>();
									for(int i =0 ; i < srcP.unitSkillShowCards.Length;++i )
									{
										if(srcP.unitSkillShowCards[i] != null)
										{
											tempCardList.Add(srcP.unitSkillShowCards[i]);
										}
									}
									unitSkillShowCards =tempCardList;
									
								}
								if(unitSkill.isHelpSkill() && unitSkillData.aim >= 3 && unitSkillData.aim <= 5)
								{
									unitSkill.init(targetCards,4,ownP,targetP,unitSkillShowCards);
								}
								else
								{
									unitSkill.init(targetCards,4,srcP,targetP,unitSkillShowCards);
								}
								currentUnitSkill=unitSkill;
								unitSkill.nextStage();
								//每次合体技后判断一次是否有一方死亡//
								if(players[0].isDead())
								{
									setPlayerLoseInfo();
									isBattleOver=true;
								}
								if(players[1].isDead())
								{
									needShowWinCameraEffect = true;
									setPlayerWinInfo();
									isBattleOver=true;
									unitKill=true;
								}
							}
						}
					}
					return;
				}
				if(isDoComboAction)
				{
					if(ActionCameraControl.mInstance.isFinish())
					{
						isDoComboAction = false;
						UnitSkill unitSkill=curTempUnitSkill;
						//UnitSkill unitSkill = BattleUnitSkillPanel.mInstance.getUnitSkill();
						UnitSkillData unitSkillData=unitSkill.getUnitSkill();
						if(unitSkillData == null)
							return;
						//List<Card> comboShowCard = new List<Card>();
						//if(unitSkill.isHelpSkill())
						//{
						//	comboShowCard = players[2].getUnitSkillShowCardList();
						//}
						//else
						//{
						//	comboShowCard = players[unitSkill.getTeam()].getUnitSkillShowCardList();
						//}
						if(true)
						{
							//初始化合体技//
							Player srcP=players[unitSkill.getTeam()];
							if(unitSkill.isHelpSkill())
							{
								srcP=players[2];
							}
							
							Player targetP=players[(unitSkill.getTeam()+1)%2];
							List<Card> targetCards=Statics.getUnitSkillTargets(unitSkillData.aim,srcP,targetP);
							
							unitSkill.init(targetCards,4,srcP,targetP,null);
							players[0].hideUnitSkillShowCards();
							currentUnitSkill=unitSkill;
							currentUnitSkill.setStage(4);
							if(currentUnitSkill.isHelpSkill() )
							{
								players[0].friendOver();
								players[2].destoryCards();
							}
							currentUnitSkill.playEnd();
							//每次合体技后判断一次是否有一方死亡//
							if(players[0].isDead())
							{
								setPlayerLoseInfo();
								isBattleOver=true;
							}
							if(players[1].isDead())
							{
								needShowWinCameraEffect = true;
								setPlayerWinInfo();
								isBattleOver=true;
								unitKill=true;
							}
						}
					}
					return;
				}
			}
			changeRound(round + 1);
			//==这里只把对面的当做敌人==//
			recordEnemys(players[1]);
			
			
			if(getRound()>Constant.MaxRound)
			{
				int num0=0;
				foreach(Card c in players[0].getCards())
				{
					if(c!=null && c.getCurHp()<=0)
					{
						num0++;
					}
				}
				int num1=0;
				foreach(Card c in players[1].getCards())
				{
					if(c!=null && c.getCurHp()<=0)
					{
						num1++;
					}
				}
				if(num0<num1)
				{
					needShowWinCameraEffect = true;
					setPlayerWinInfo();
				}
				else
				{
					setPlayerLoseInfo();
				}
				isBattleOver=true;
				return;
			}
			
			waitForSetNextActionCardInfo = true;
			
			players[0].clearNewForbitCardList();
			players[1].clearNewForbitCardList();
			//==天赋:特殊,每经过一个回合，自动回复一定量的HP==//
            if (isEvent && round == 60)
            {
                setPlayerWinInfo();
                isBattleOver = true;
                return;
            }
			if(round%12==1 && getRound()>1)
			{
				foreach(Player p in players)
				{
					if(p.getCards() == null)
						continue;
					foreach(Card c in p.getCards())
					{
						if(c!=null && c.getCurHp()>0)
						{
							int addHp=0;
							TalentData td=TalentData.getData(c.talent1);
							if(td!=null && td.type==7 && td.class1==10)
							{
								addHp+=(int)(c.getMaxHp()*td.number);
							}
							TalentData td2=TalentData.getData(c.talent2);
							if(td2!=null && td2.type==7 && td2.class1==10)
							{
								addHp+=(int)(c.getMaxHp()*td2.number);
							}
							TalentData td3=TalentData.getData(c.talent3);
							if(td3!=null && td3.type==7 && td3.class1==10)
							{
								addHp+=(int)(c.getMaxHp()*td3.number);
							}
							
							if(addHp>0)
							{
								c.setCurHp(c.getCurHp()+addHp);
								if(c.getCurHp()>c.getMaxHp())
								{
									c.setCurHp(c.getMaxHp());
								}
								c.createHitNumObject(addHp,4);
							}
						}
					}
				}
				
				if(players[0].isCanResumeAttackCard(getRound()) || players[1].isCanResumeAttackCard(getRound()))
				{
					// TODO Zay
					Invoke("checkIsCanDoPSEffect",1.0f);
				}
				else
				{
					checkIsCanDoPSEffect();
				}
			}
			else
			{
				checkIsCanDoPSEffect();
			}
		}
		else
		{
			if(currentUnitSkill!=null)
			{
				return;
			}
			if(waitForSetNextActionCardInfo)
				return;
			if(curCard!=null && !curCard.isRunning())
			{
				curCard.resetTargets();
				running=true;
			}
			if(round==0)
			{
				if(players==null)
				{
					return;
				}
				foreach(Player p in players)
				{
					if(p.getCards()==null)
					{
						continue;
					}
					foreach(Card c in p.getCards())
					{
						if(c!=null && c.getSkill().type==2)
						{
							if(c.isRunning())
							{
								return;
							}
							else
							{
								c.ignoreT=false;
								c.resetTargets();
							}
						}
					}
				}
				running=true;
			}
		}
	}
	
	void  checkIsCanDoPSEffect()
	{
		Player curPlayer=players[curSide];
		Player targetPlayer=players[(curSide+1)%2];
		//get actor 
		int tempIdex =curPlayer.getCurActionIndex()+ 1;
		tempIdex=tempIdex%6;
		Card tempCard = curPlayer.getCard(tempIdex);
		if(tempCard!=null && tempCard.getCurHp()>0 && !tempCard.isForbitAttack())
		{
			List<int> aliveTargetIndex = new List<int>();
			for(int i = 0;i < targetPlayer.getCards().Length;++i)
			{
				Card cc = targetPlayer.getCard(i);
				if(cc != null && cc.getCurHp()>0)
				{
					aliveTargetIndex.Add(i);
				}
			}
			if(tempCard.canForbitAttack())
			{
				Statics.doForbitAttack(tempCard,targetPlayer,aliveTargetIndex,getRound());
				Invoke("setNextActionCardInfo",1.0f);
				return;
			}
			if(tempCard.isCanReduceEnergy())
			{
				Statics.doReduceEnergy(tempCard,targetPlayer);
				Invoke("setNextActionCardInfo",1.5f);
				return;
			}
			aliveTargetIndex.Clear();
			aliveTargetIndex = null;
		}
		setNextActionCardInfo();
	}
	
	void setNextActionCardInfo()
	{
		waitForSetNextActionCardInfo = false;
		//battle 
		Player curPlayer=players[curSide];
		Player targetPlayer=players[(curSide+1)%2];
		//get actor 
		int curIndex=curPlayer.addCurActionIndex();
		curCard=curPlayer.getCard(curIndex);
		if(curCard==null || curCard.getCurHp()<=0 || curCard.isForbitAttack())
		{
			curSide=(curSide+1)%2;
			running=true;
			return;
		}
		//get target
		List<int> targetIndexs=Statics.getTargetIndexs(curIndex,curPlayer.getCards(),targetPlayer.getCards());
		processTarget(curIndex,curPlayer,targetPlayer,targetIndexs);
		//每次普通攻击后判断一次是否有一方死亡//
		if(players[0].isDead())
		{
			setPlayerLoseInfo();
			isBattleOver=true;
			battleResult = 0;
			return;
		}
		if(players[1].isDead())
		{
			needShowWinCameraEffect = true;
			setPlayerWinInfo();
			isBattleOver=true;
			battleResult = 1;
			return;
		}
		//set side//
		curSide=(curSide+1)%2;
	}
	
	public void setPlayerWinInfo()
	{
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			blj.r=1;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			mblj.r = 1;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
		{
			pblj.r = 1;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_EVENT)
		{
			eblj.r = 1;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_DEMO)
		{
			needShowWinCameraEffect =false;
		}
	}
	
	public void setPlayerLoseInfo()
	{
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			blj.r=2;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			mblj.r = 2;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
		{
			pblj.r = 2;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_EVENT)
		{
			eblj.r = 2;
		}
	}
	
	public void skipFirstBattle()
	{
		isBattleOver=true;
		battleResult = 0;
	}
	
	private void recordEnemys(Player enemy)
	{
		//==每一小回合开始的时候记录下活着的敌人==//
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			aliveEnemyList.Clear();
			for(int i = 0;i < enemy.getCards().Length;i++)
			{
				Card cc = enemy.getCard(i);
				if(cc != null && cc.getCurHp()>0)
				{
					aliveEnemyList.Add(cc);
				}
			}	
		}
	}
	
	public void showFog()
	{
		MapSceneData msd = MapSceneData.getData(sceneName);
		if(msd != null)
		{
			float[] fogColor = msd.fogs;
			if(fogColor.Length == 4)
			{
				Color c = new Color(fogColor[0],fogColor[1],fogColor[2],fogColor[3]);
				GameObjectUtil.showFog(c);
			}
		}
	}
	
	//检查合体技是否可以释放//
	private void checkUnitSkill()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
		{
			if(demoUnitSkillDatas==null)
			{
				return;
			}
			for(int i = 0; i < demoUnitSkillDatas.Length;++i)
			{
				canUnitSkill[i]=true;
			}
		}
		else
		{
			for(int i=0;unitSkills!=null && i<unitSkills.Length;i++)
			{
				UnitSkillData usd=unitSkills[i];
				if(usd==null)
				{
					continue;
				}
				
				if((i<2 && (usd!=null && players !=null )) 
					|| (i==2 && (usd!=null && players !=null)))
				{
					switch(usd.type)
					{
					case 1://攻击//
					case 2://防护//
					case 3://回复//
					case 4://BUFF
					case 6://献祭//
					case 7://变身//
					case 8://定身//
						canUnitSkill[i]=true;
						break;
					case 5://复活//
						if(players[0].haveDeadCard())
						{
							canUnitSkill[i]=true;
						}
						else
						{
							canUnitSkill[i]=false;
						}
						break;
					}
				}
				else
				{
					if(canUnitSkill != null)
					{
						canUnitSkill[i]=false;
					}
				}
			}
		}
	}
	
	private int frameNum;
	void LateUpdate()
	{
		//检查合体技是否可以释放//
		frameNum++;
		if(frameNum>=20)
		{
			frameNum=0;
			checkUnitSkill();
		}
		if(currentUnitSkill!=null && !isWaitForSetUnitSkill)
		{
			if(currentUnitSkill.isCanCountFrame){
				currentUnitSkill.frameCount+=Time.deltaTime;
				if(currentUnitSkill.unitSkill != null && currentUnitSkill.frameCount >= (currentUnitSkill.unitSkill.screenShakeDelay + 0.5)){
					currentUnitSkill.ShakeScreen();
					currentUnitSkill.isCanCountFrame = false;
				}
			}
			//延时产生的特效//
			if(currentUnitSkill.uniteDelayEffectList.Count>0)
			{
				for(int i=currentUnitSkill.uniteDelayEffectList.Count-1;i>=0;i--)
				{
					Effect e=(Effect)(currentUnitSkill.uniteDelayEffectList[i]);
					if(e.isDelayOver(curTime))
					{
						currentUnitSkill.uniteDelayEffectList.RemoveAt(i);
						e.create();
						currentUnitSkill.uniteEffectList.Add(e);
					}
				}
			}
			//转换阶段//
			if(currentUnitSkill.canRunNextStage())
			{
				if(currentUnitSkill.stage == 2)
				{
					if(needShowPlayerCards)
					{
						if(currentUnitSkill.isHelpSkill() )
						{
							players[0].friendOver();
							//==释放好友角色==//
							players[2].hideUnitSkillShowCards();
							players[2].destoryCards();
						}
						else
						{
							if(currentUnitSkill.getTeam() == 0)
							{
								players[0].hideUnitSkillShowCards();
								players[0].showCards();
								
							}
							else
							{
								if(!GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Demo))
								{
									players[1].hideUnitSkillShowCards();
								}
								players[1].showCards();
								
							}
						}
						needShowPlayerCards = false;
					}
				}
				
				currentUnitSkill.nextStage();
				if(currentUnitSkill.stageOver())
				{
					//清除召唤兽//
					if(currentUnitSkill.callUpObject != null){
						Destroy(currentUnitSkill.callUpObject);
					}
					//清除特效 合体技结束//
					TickUniteSkill();
					currentUnitSkill.clear();
					UIInterfaceManager.mInstance.doSpeedChange(uiManager.getCurScale());
					
					currentUnitSkill=null;
					//显示ui//
					if(!UIInterface.activeSelf )
					{
						UIInterface.SetActive(true);
					}
					UIInterfaceManager.mInstance.doSpeedChange(lastSpeedScale);
					ViewControl.mInstacne.reset();
					ViewControl.mInstacne.startWaitNormalCamareMove();
				}
			}
		}
	}
	
	public void TickUniteSkill()
	{
		if(currentUnitSkill.uniteEffectList!= null)
		{
			for(int i = 0;i < currentUnitSkill.uniteEffectList.Count;)
			{
				Effect eff = (Effect)currentUnitSkill.uniteEffectList[i];
				currentUnitSkill.uniteEffectList.RemoveAt(i);
				Destroy(eff.getEffectObj());
			}
			currentUnitSkill.uniteEffectList=null;
		}
		if(currentUnitSkill.uniteDelayEffectList!=null)
		{
			currentUnitSkill.uniteDelayEffectList.Clear();
			currentUnitSkill.uniteDelayEffectList=null;
		}
	}
	
	//bool isDebug = false;
	
	public bool isCastingUnitSkill()
	{
		return (currentUnitSkill != null);	
	}
	
	private void processTarget(int curActionIndex,Player curPlayer,Player targetPlayer,List<int> targetIndexs)
	{
		// find is crit
		//bool isCrit = false;
		List<Card> targets=new List<Card>();
		List<Card> tanks=null;
		
		Card actionCard=curPlayer.getCard(curActionIndex);	
		if(actionCard.getSkill().type==1)
		{

			tanks=new List<Card>();
			
			List<int> tankIndexs=Statics.getTankIndex(targetPlayer.getCards(),actionCard.getSkill().atkTarget,targetIndexs);
			foreach(int tankIndex in tankIndexs)
			{
				Card tank=targetPlayer.getCards()[tankIndex];
				tank.setBehurtSkill(curCard.getSkill());
				//获取被T的索引//
				List<int> beTankIndexs=Statics.getBeTankIndexs(tankIndex,targetPlayer.getCards());
				//如果T是单体T,查看T是否被攻击,如果没有被攻击,查看被攻击的人是否有跟T在同一横排的,如果受伤的人跟T在同一横排,T可为它抵挡//
				if(tank.getSkill().atkTarget==0 && !targetIndexs.Contains(tankIndex))
				{
					beTankIndexs.Clear();
					int p=0;
					if(tankIndex>=curPlayer.getCards().Length/2)
					{
						p=1;
					}
					foreach(int i in targetIndexs)
					{
						int pTemp=0;
						if(i>=curPlayer.getCards().Length/2)
						{
							pTemp=1;
						}
						if(pTemp==p)
						{
							beTankIndexs.Add(i);
							break;
						}
					}
				}
				//如果是单体攻击,并且受伤目标的同一横排有竖排防御,此竖排防御可以为此受伤目标抵挡//
				if(actionCard.getSkill().atkTarget==0 && tank.getSkill().defTarget==1)
				{
					int tempTargetIndex=(int)targetIndexs[0];
					if(
						(tempTargetIndex<targetPlayer.getCards().Length/2 && tankIndex<targetPlayer.getCards().Length/2)
						||
						(tempTargetIndex>=targetPlayer.getCards().Length/2 && tankIndex>=targetPlayer.getCards().Length/2)
						)
					{
						beTankIndexs.Clear();
						beTankIndexs.Add(tempTargetIndex);
					}
				}
				//如果是竖排攻击,并且前排受伤目标的同一横排有竖排防御,此竖排防御可为前排受伤目标抵挡//
				if(actionCard.getSkill().atkTarget==1 && tank.getSkill().defTarget==1)
				{
					int tempIndex=(int)targetIndexs[0];
					if(tempIndex!=tankIndex 
						&& 
						(
						(tempIndex<targetPlayer.getCards().Length/2 && tankIndex<targetPlayer.getCards().Length/2)
						|| 
						(tempIndex>=targetPlayer.getCards().Length/2 && tankIndex>=targetPlayer.getCards().Length/2)
						)
						)
					{
						beTankIndexs.Clear();
						beTankIndexs.Add(tempIndex);
					}
				}
				//去掉受伤目标之外的被T者//
				for(int i=beTankIndexs.Count-1;i>=0;i--)
				{
					int index=(int)beTankIndexs[i];
					if(!targetIndexs.Contains(index))
					{
						beTankIndexs.RemoveAt(i);
					}
				}
				List<Card> beTanks=new List<Card>();
				foreach(int index in beTankIndexs)
				{
					Card beTank=targetPlayer.getCard(index);
					beTank.setBeTankSkill(tank.getSkill());
					beTanks.Add(beTank);
				}
				tank.setBeTanks(beTanks);
				tanks.Add(tank);
			}
			
			//==天赋:3对士气槽==//
			float energyMul=1f;
			float energyMulTemp=0;
			TalentData td=TalentData.getData(curCard.talent1);
			if(td!=null && td.type==3 && td.class1==1 && td.effect==1)
			{
				energyMulTemp+=td.number;
			}
			TalentData td2=TalentData.getData(curCard.talent2);
			if(td2!=null && td2.type==3 && td2.class1==1 && td2.effect==1)
			{
				energyMulTemp+=td2.number;
			}
			TalentData td3=TalentData.getData(curCard.talent3);
			if(td3!=null && td3.type==3 && td3.class1==1 && td3.effect==1)
			{
				energyMulTemp+=td3.number;
			}
			energyMul+=energyMulTemp;
			//计算伤害//
			foreach(int index in targetIndexs)
			{
				Card target=targetPlayer.getCard(index);
				target.setBehurtSkill(curCard.getSkill());
				targets.Add(target);
			}
			Statics.calculateHurt_step0(curCard,targets,tanks,1,uiManager);
			foreach(Card target in targets)
			{
				//在目标里的T计算伤害//
				if(tanks.Contains(target))
				{
					if(targetIndexs.Count==1)
					{
						Statics.calculateHurt_step1(getRound(),actionCard,target,curPlayer,targetPlayer,0,1,0,curSide==0);
						//if(target.getHurtIsCrit())
						//{
							//isCrit = true;
						//}
					}
					else 
					{
						if(target.beTanks.Count==1)
						{
							Statics.calculateHurt_step1(getRound(),actionCard,target,curPlayer,targetPlayer,0,1,0,curSide==0);
							//if(target.getHurtIsCrit())
							//{
								//isCrit = true;
							//}
						}
						else
						{
							Statics.calculateHurt_step1(getRound(),actionCard,target,curPlayer,targetPlayer,0,3,target.beTanks.Count-1,curSide==0);
							//if(target.getHurtIsCrit())
							//{
								//isCrit = true;
							//}
						}
					}
				}
				else
				{
					float tankX=0;
					foreach(Card tank in tanks)
					{
						if(tank.beTanks.Contains(target))
						{
							tankX=tank.getSkill().numberX;
							break;
						}
					}
					Statics.calculateHurt_step1(getRound(),actionCard,target,curPlayer,targetPlayer,tankX,0,0,curSide==0);
					//if(target.getHurtIsCrit())
					//{
					//	isCrit = true;
					//}
				}
				
				//==T计算伤害是计算天赋对士气的影响==//
				float energyMulTar=1f;
				float energyMulTarTemp=0;
				//==天赋:3对士气槽==//
				TalentData tdTar=TalentData.getData(target.talent1);
				if(tdTar!=null && tdTar.type==3 && tdTar.class1==2 && tdTar.effect==1)
				{
					energyMulTarTemp+=tdTar.number;
				}
				TalentData tdTar2=TalentData.getData(target.talent2);
				if(tdTar2!=null && tdTar2.type==3 && tdTar2.class1==2 && tdTar2.effect==1)
				{
					energyMulTarTemp+=tdTar2.number;
				}
				TalentData tdTar3=TalentData.getData(target.talent3);
				if(tdTar3!=null && tdTar3.type==3 && tdTar3.class1==2 && tdTar3.effect==1)
				{
					energyMulTarTemp+=tdTar3.number;
				}
				energyMulTar+=energyMulTarTemp;
				
				//回复士气值//
				switch(actionCard.getSkill().atkTarget)
				{
				case 0:
					curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeSingle)*energyMul);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeSingle)*energyMulTar);
					break;
				case 1:
					curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeSwing)*energyMul);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeSwing)*energyMulTar);
					break;
				case 2:
					curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeLine)*energyMul);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeLine)*energyMulTar);
					break;
				case 3:
					curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeAll)*energyMul);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeAll)*energyMulTar);
					break;
				}
			}
			//不在目标里的T计算伤害//
			foreach(Card tank in tanks)
			{
				if(!targets.Contains(tank))
				{
					
					//==T计算伤害是计算天赋对士气的影响==//
					float energyMulTar=1f;
					float energyMulTarTemp=0;
					//==天赋:3对士气槽==//
					TalentData tdTar=TalentData.getData(tank.talent1);
					if(tdTar!=null && tdTar.type==3 && tdTar.class1==2 && tdTar.effect==1)
					{
						energyMulTarTemp+=tdTar.number;
					}
					TalentData tdTar2=TalentData.getData(tank.talent2);
					if(tdTar2!=null && tdTar2.type==3 && tdTar2.class1==2 && tdTar2.effect==1)
					{
						energyMulTarTemp+=tdTar2.number;
					}
					TalentData tdTar3=TalentData.getData(tank.talent3);
					if(tdTar3!=null && tdTar3.type==3 && tdTar3.class1==2 && tdTar3.effect==1)
					{
						energyMulTarTemp+=tdTar3.number;
					}
					energyMulTar+=energyMulTarTemp;
					
					Statics.calculateHurt_step1(getRound(),actionCard,tank,curPlayer,targetPlayer,0,2,tank.beTanks.Count,curSide==0);
					//if(tank.getHurtIsCrit())
					//{
					//	isCrit = true;
					//}
					//回复士气值//
					switch(actionCard.getSkill().atkTarget)
					{
					case 0:
						curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeSingle)*energyMul);
						targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeSingle)*energyMulTar);
						break;
					case 1:
						curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeSwing)*energyMul);
						targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeSwing)*energyMulTar);
						break;
					case 2:
						curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeLine)*energyMul);
						targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeLine)*energyMulTar);
						break;
					case 3:
						curPlayer.addTempEnergy(EnergyData.getEnergy(1,EnergyData.ScopeAll)*energyMul);
						targetPlayer.addTempEnergy(EnergyData.getEnergy(2,EnergyData.ScopeAll)*energyMulTar);
						break;
					}
				}
			}
		}
		else if(actionCard.getSkill().type==2)
		{
			foreach(int index in targetIndexs)
			{
				if(index!=curActionIndex)
				{
					targets.Add(curPlayer.getCard(index));
				}
			}
		}
		else
		{
			//==天赋:3对士气槽==//
			float energyAdd=0;
			TalentData td=TalentData.getData(curCard.talent1);
			if(td!=null && td.type==3 && td.class1==3 && td.effect==2)
			{
				energyAdd+=td.number;
			}
			TalentData td2=TalentData.getData(curCard.talent2);
			if(td2!=null && td2.type==3 && td2.class1==3 && td2.effect==2)
			{
				energyAdd+=td2.number;
			}
			TalentData td3=TalentData.getData(curCard.talent3);
			if(td3!=null && td3.type==3 && td3.class1==3 && td3.effect==2)
			{
				energyAdd+=td3.number;
			}
			//对每个目标的回复量//
			int resumeNum=0;
			foreach(int index in targetIndexs)
			{
				curPlayer.getCard(index).setResumeSkill(curCard.getSkill());
				targets.Add(curPlayer.getCard(index));
				resumeNum=Statics.resume(getRound(),actionCard,curPlayer.getCard(index),curPlayer,curSide==0);
				//==天赋:3对士气槽==//
				float energyAddTar=0;
				TalentData tdTar=TalentData.getData(curPlayer.getCard(index).talent1);
				if(tdTar!=null && tdTar.type==3 && tdTar.class1==4 && tdTar.effect==2)
				{
					energyAddTar+=tdTar.number;
				}
				TalentData tdTar2=TalentData.getData(curPlayer.getCard(index).talent2);
				if(tdTar2!=null && tdTar2.type==3 && tdTar2.class1==4 && tdTar2.effect==2)
				{
					energyAddTar+=tdTar2.number;
				}
				TalentData tdTar3=TalentData.getData(curPlayer.getCard(index).talent3);
				if(tdTar3!=null && tdTar3.type==3 && tdTar3.class1==4 && tdTar3.effect==2)
				{
					energyAddTar+=tdTar3.number;
				}
				
				//回复士气值//
				switch(actionCard.getSkill().healTarget)
				{
				case 0:
					curPlayer.addTempEnergy(EnergyData.getEnergy(4,EnergyData.ScopeSingle)+energyAdd+energyAddTar);
					break;
				case 1:
					curPlayer.addTempEnergy(EnergyData.getEnergy(4,EnergyData.ScopeSwing)+energyAdd+energyAddTar);
					break;
				case 2:
					curPlayer.addTempEnergy(EnergyData.getEnergy(4,EnergyData.ScopeLine)+energyAdd+energyAddTar);
					break;
				case 3:
					curPlayer.addTempEnergy(EnergyData.getEnergy(4,EnergyData.ScopeAll)+energyAdd+energyAddTar);
					break;
				}
			}
			//回复士气值//
			switch(actionCard.getSkill().healTarget)
			{
			case 0:
				curPlayer.addTempEnergy(EnergyData.getEnergy(3,EnergyData.ScopeSingle));
				break;
			case 1:
				curPlayer.addTempEnergy(EnergyData.getEnergy(3,EnergyData.ScopeSwing));
				break;
			case 2:
				curPlayer.addTempEnergy(EnergyData.getEnergy(3,EnergyData.ScopeLine));
				break;
			case 3:
				curPlayer.addTempEnergy(EnergyData.getEnergy(3,EnergyData.ScopeAll));
				break;
			}
			
			int addHp=0;
			//==天赋:特殊,对其他单位使用回复技能时本身也会回复一定比例的HP数值==//
			if(td!=null && td.type==7 && td.class1==9 && !targetIndexs.Contains(curActionIndex))
			{
				addHp+=(int)(resumeNum*td.number);
			}
			if(td2!=null && td2.type==7 && td2.class1==9 && !targetIndexs.Contains(curActionIndex))
			{
				addHp+=(int)(resumeNum*td2.number);
			}
			if(addHp>0)
			{
				curCard.setCurHp(curCard.getCurHp()+addHp);
				if(curCard.getCurHp()>curCard.getMaxHp())
				{
					curCard.setCurHp(curCard.getMaxHp());
				}
				curCard.createHitNumObject(addHp,4);
			}
		}
		
		//if(actionCard.team != 0 || actionCard.getSkill().type != 1)
		//{
			//isCrit = false;	
		//}
		//else
		//{
		//	if(GameHelper.isTestCritCameraShow)
		//	{
				//isCrit = true;
		//	}
		//}
		
		if(actionCard.team == 0)
		{
			ViewControl.CameraShowType cst = ViewControl.CameraShowType.E_Null;
			SkillData skill = actionCard.getSkill();
			int skillCST = skill.view;
			float viewNum = skill.viewNum;
			
			if(skillCST == 0)
			{
				cst = ViewControl.CameraShowType.E_Null;
			}
			else
			{
				int r = UnityEngine.Random.Range(1,101);
				float num =  (float)((float)r / 100.0f);
				if(num <= viewNum)
				{
					if(skillCST == 1)
					{
						cst = ViewControl.CameraShowType.E_One;
					}
					else if(skillCST == 2)
					{
						cst = ViewControl.CameraShowType.E_Mul;
					}
					else if(skillCST == 3)
					{
						cst = ViewControl.CameraShowType.E_CST3;
					}
				}
			}
			if(cst != ViewControl.CameraShowType.E_Null)
			{
				hideCardsForCritCameraShow(actionCard,targets,tanks);
				ViewControl.mInstacne.doCameraShow(actionCard,targets,tanks, cst);
			}
		}
		//for play//
		actionCard.setTargets(targets,tanks);
		actionCard.setDirect(0);
	}
	
	public static GameObject findPrefab(string name)
	{
		return GameObject.Find(name);
	}
	
	//获取回合数//
	public int getRound()
	{
		if(round%12==0)
		{
			return round/12;
		}
		return round/12+1;
	}
	
	public void changeRound(int num)
	{
		round = num;
		
        if (isEvent)
        {
            uiManager.ChangeRoundNum(getRound(), 1);
        }
        else
        {
            uiManager.ChangeRoundNum(getRound());
        }
		
		
		if(cardNum==0)
		{
			for(int k=0;k<2;k++)
			{
				Player p=players[k];
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						cardNum++;
					}
				}
			}
		}
		
		if(cardNum<=6)
		{
			if(round%12==0)
			{
				Resources.UnloadUnusedAssets();
			}
		}
		else
		{
			if(round%6==0)
			{
				Resources.UnloadUnusedAssets();
			}
		}
	}
	
	public void addBattleLog(string s)
	{
		if(!string.IsNullOrEmpty(s) && battleDetails!=null)
		{
			battleDetails.Add(s);
		}
	}
	
	/**发送战斗结果**/
	public void sendBattleResult()
	{
		//==普通技能日志选2条==//
		requestType = 1;
		//发送战斗日志//
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			blj.sNum = starNum;
			blj.bs=battleDetails;
			blj.round=getRound();
			PlayerInfo.getInstance().sendRequest(blj,this);
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			mblj.bs=battleDetails;
			if(mblj.r == 1)			//胜利//
			{
				//胜利的话保持原有返回数据//
			}	
			else if(mblj.r == 2)	//失败//
			{
				PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_MAZE ;
			}
			string curCardHp = "";
			Card[] curCardTeam = players[0].getCards();
			for(int i = 0;i<curCardTeam.Length;i++)
			{
				if(curCardTeam[i] == null)
				{
					continue;
				}
				int curHp = curCardTeam[i].curHp;
				if(curHp<=0)
				{
					PlayerInfo.getInstance().curMazeBattleCardHp[curCardTeam[i].cardData.id] = 1;
					curHp = 1;
				}
				else
				{
					PlayerInfo.getInstance().curMazeBattleCardHp[curCardTeam[i].cardData.id] = curCardTeam[i].curHp;
					curHp = curCardTeam[i].curHp;
				}
				if(i<(curCardTeam.Length-1))
				{
					curCardHp+=curCardTeam[i].cardData.id+"-"+curHp+"&";
				}
				else
				{
					curCardHp+=curCardTeam[i].cardData.id+"-"+curHp;
				}
			}
			mblj.cb = curCardHp;
			PlayerInfo.getInstance().sendRequest(mblj,this);
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
		{
			pblj.bs=battleDetails;
			PlayerInfo.getInstance().sendRequest(pblj,this);
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_EVENT)
		{
			eblj.bs=battleDetails;

            if (isEvent)
            {
                eblj.t = 1;
                eblj.bv = hurt;
            }
 
			PlayerInfo.getInstance().sendRequest(eblj,this);
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_DEMO)
		{
			blj.bs=battleDetails;
			blj.r = 3;
			PlayerInfo.getInstance().sendRequest(blj,this);
		}
		
		battleDetails=null;
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
			{
				//将服务器返回的数据付给我得脚本中，该脚本里面的值在切换场景之后就为空了//
				Battle_Player_Info.GetInstance().SetResponesData(json);
				errorCode = Battle_Player_Info.GetInstance().errorCode;
				receiveData=true;
			}break;
			case 2:
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
			}break;
			}
		}
	}
	
	public ArrayList GetAliveEnemyList()
	{
		return aliveEnemyList;
	}
	 
	public void SetBounesGlodMul(float goldM)
	{
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			blj.gm = goldM.ToString();
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			mblj.gm = goldM.ToString();
		}
	}
	
	//设置bounes的标志//
	public void SetIsPlayBounes(bool isPlay)
	{
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			isPlayBounes = isPlay;
		}
	}
	
	//设置关闭ko提示界面//
	public void SetIsShowKOTip(bool isShow)
	{
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			isShowKOTip = isShow;
		}
	}
	
	public void hideNotCastUnitSkillCards(Card[] cardList)
	{
		//List<Card> resultCardList = new List<Card>();
		for(int i = 0 ; i < cardList.Length;++i)
		{
			if(cardList[i] == null)
				continue;
			cardList[i].hideObj();
		}
	}
	
	public void hideCardsForCritCameraShow(Card actionCard,List<Card> targets,List<Card> tanks)
	{
		for(int i = 0; i < 2;++i)
		{
			Player player = players[i];
			if(player.getCards() != null)
			{
				Card[] cards = player.getCards();
				for(int j = 0; j < cards.Length;++j)
				{
					Card c = cards[j];
					if(c != null)
					{
						if(c == actionCard)
						{
							continue;
						}
						if(targets != null && targets.Contains(c))
						{
							continue;
						}
						if(tanks != null && tanks.Contains(c))
						{
							continue;
						}
						if(c.getCurHp() <= 0)
						{
							continue;
						}
						//隐藏卡牌及内容//
						c.hideObj();
					}
				}
			}
		}
	}
	
	public void showCardsForFinishCritCameraShow()
	{
		for(int i = 0; i < 2; ++i)
		{
			Player player = players[i];
			if(player.getCards() != null)
			{
				Card[] cards = player.getCards();
				for(int j = 0; j < cards.Length;++j)
				{
					Card c = cards[j];
					if(c == null)
						continue;
					if(c.getCurHp() > 0)
					{
						c.recoverShowObj();
					}
				}
			}
		}
	}
	
	//设置战斗的星级//
	public void SetBattleStarNum()
	{
		//判断星级//
		if(players[0].getDeadCardNum() ==0)
		{
			starNum = 3;
		}
		else if(players[0].getDeadCardNum() == 1)
		{
			starNum = 2;
		}
		else
		{
			starNum = 1;
		}
	}
	
	//获取bouns掉落物品领取标识0未领取， 1已领取//
	public int GetBounsDropMark()
	{
		return bounesDropGet;
	}
	
	public int GetCurMissionId()
	{
		return missionId;
	}
	
	public void notShowDialog()
	{
		needShowBeginDialog = false;
		finishShowBeginDialog = true;
		needShowWinDialog = false;
		finishShowWinDialog = true;
		needShowLoseDialog = false;
		finishShowLoseDialog = true;
	}
	
	public bool GetBattleOver()
	{
		return isBattleOver;
	}
	
	public void doRemoveSheep()
	{
		Invoke("removeSheep",0.1f);
	}
	
	void removeSheep()
	{
		for(int i = 0 ; i < needRemoveSheeps.Count; ++i)
		{
			needRemoveSheeps[i].gc();
			needRemoveSheeps[i] = null;
		}
		needRemoveSheeps.Clear();
	}
	
	public void setBattleOver(bool b)
	{
		isBattleOver = b;
	}
	
	public void setBattleResult(int r)
	{
		battleResult = r;
	}
	
	public void gc()
	{
		foreach(Player p in players)
		{
			if(p!=null)
			{
				p.gc();
			}
		}
		players=null;
		demoUnitSkillDatas=null;
		demoUnitSkillEnergys=null;
		demoPlayerBUnitSkillData=null;
		if(unitSkillQueue!=null)
		{
			unitSkillQueue.Clear();
			unitSkillQueue=null;
		}
		curCard=null;
		if(currentUnitSkill!=null)
		{
			currentUnitSkill.gc();
			currentUnitSkill=null;
		}
		if(curTempUnitSkill!=null)
		{
			curTempUnitSkill.gc();
			curTempUnitSkill=null;
		}
		uniteManager.gc();
		uniteManager=null;
		uiManager.gc();
		uiManager=null;
		energyManager.gc();
		energyManager=null;
		canUnitSkill=null;
		blj=null;
		mblj=null;
		pblj=null;
		eblj=null;
		battleDetails=null;
		aliveEnemyList.Clear();
		aliveEnemyList=null;
		curSceneObj=null;
		curSkyBox=null;
		dialogBeginIDList.Clear();
		dialogBeginIDList=null;
		dialogWinIDList.Clear();
		dialogWinIDList=null;
		dialogLoseIDList.Clear();
		dialogLoseIDList=null;
		dialogUnitSkillIDList.Clear();
		dialogUnitSkillIDList=null;
		bornCardJsons=null;
		ActionCameraControl.mInstance.gc();
		ViewControl.mInstacne.gc();
		if(BattleBounesPanel.mInstance!=null)
		{
			BattleBounesPanel.mInstance.gc();
		}
		BattleGuidePointUnitSkill.mInstance.gc();
		DialogPanel.mInstance.gc();
		NewBattleUnitePanel.mInstance.gc();
		ResultTipManager.mInstance.gc();
		StartFightPanel.mInstance.gc();
		Battle_Player_Info.instance.gc();
		loadEffects.Clear();
		loadEffects=null;
		mInstance = null;
		
		Resources.UnloadUnusedAssets();
	}
}