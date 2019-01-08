#pragma strict
	@script RequireComponent(NavMeshAgent)
	enum	bulletType
	{ 
	    arrow		=	1,
	    icearrow	=	2,
	    fireball	=	3,
	    Darkbarrow	=	4,
	    Poisonarrow	=	5
	}
	//AI状态//
	enum	E_AIStatus
	{
		NotDecideYet	=	0,
		WaitingDecision	=	1,
		Idle			=	2,
		Battle			=	3,
		Attack			=	4,
		FallBack		=	10,
	}
	//索敌状态//
	enum	E_SearchTargetStatus
	{
		NoTargetWithinAlertRange	=	0,
		TargetWithinAlert			=	1,
		TargetFocused				=	2,
	}
	private	var	DecisionID			:	int		=	0;			//决策ID用于验证当前建议状态//
	var	CurrentAIStatus		:	E_AIStatus;				//正在执行的AI状态//
	private	var	monsterAnimation	:	MonsterAnimation;		//用于切换状态//
	private	var	SearchTargetStatus	:	E_SearchTargetStatus	=	E_SearchTargetStatus.NoTargetWithinAlertRange;	//索敌状态//

	private	var	DefinedFallBackCheckDistance:float	=	8;

	private	var	MNetView			:	MonsterNetView;		//获取自身ID//
	private	var	MNetInit			:	MonsterNetInit;		//执行同步数据的接口//
	var	agent	:	NavMeshAgent;
	private	var	fxobject			:	GameObject;
	private var Originalposition	:	Vector3 ;	//记录原始位置//
	// var MasterID:int;
	 var	targetp	:	Transform;	//锁定的目标//
	 var	targetm	:	Transform;	//宠物的主人的目标//null时为没有主人,不需要跟随//
	private	var	offset	:	Vector3;
	private	var	qrelative:	Vector3;
	private var lastatktime	=	0.0;
	var punchPosition	=	new	Vector3 (0, 0.5, 0.9);
	var	alertRange	=	18;	//警戒范围//
	var	attackRange	=	16;	//进攻范围//
	private var findmRange	=	45;
	private var remoteRange	=	12;	//远程攻击范围//
	var delayAttackTime		=	2.5;

	private	var	Canremote = false;	//能否远程//
	var remoteType : bulletType;
	var punchRadius	=	5;
	var attackEFID	=	0;
	var stun		=	false;	//眩晕//
	var bing		=	false;	//冰冻//
	var	beattack	=	false;	//受击标记，僵直标记//
	var Canattack	=	true;	//能否攻击//
	var Canmove     =   true;
	var Movespeed	=	1.0;	//移动速度//
	private var	selfspeed	=	1.0;
	var Status : MonsterStatus;
	var Mskill : MonsterSkill;
	var	Enemytag	=	"Player";
	private var attackcombo = 0;	//记录有几种攻击动作，记录连击动作个数 //
	private var n =	0;
	private var mideffect:int;
	private var endeffect:int;
	private var Starspeed = 0.0;
	var Damagetype		:	Damagestype = 0;
	private var	anim_	:	Animation;
	
	//____
	private	var	InvokeRepeatTimeFlage	:	float;
	
	private	var IsSpawning : boolean	=	false;
	
	function Awake()
	{
		CurrentAIStatus	=	E_AIStatus.NotDecideYet;
		
		anim_		=	GetComponent.<Animation>();
		setenemytag();	//确认敌人的Tag//
		Status		=	GetComponent(MonsterStatus);
		Mskill		=	GetComponent(MonsterSkill);
		agent		=	GetComponent.<NavMeshAgent>();
		MNetInit	=	GetComponent(MonsterNetInit);
		Starspeed	=	agent.speed;
		
		monsterAnimation	=	GetComponent(MonsterAnimation);
		
		for( var i : int = 1; i < 5; i++ )
		{ 
			if(	anim_["attacka"+i]	)	
				attackcombo	+=	1;	//记录连击动作个数 //
		}	
		if(	anim_["attackb1"]	)
		{
			Canremote	=	true;
			switch(	remoteType	)
			{	 
				case 1: 
					mideffect=55;
					endeffect=3;
					Damagetype = 1;
					break; 
				case 2: 
					mideffect=45;
					endeffect=58;
					Damagetype = 2;
					break;
				case 3: 
					mideffect=73;
					endeffect=59;
					Damagetype = 3;
					break;     
				case 4: 
					mideffect=74;
					endeffect=76;      
					Damagetype = 4;
					break;
				case 5: 
					mideffect=56;
					endeffect=75; 
					Damagetype = 5;
					break;  
			}
		}


		if(	Application.loadedLevelName == "Map911" && gameObject.name.Substring( 0, 5 ) == "57100"	)
		{	//世界boss//
			findmRange	=	5000;
	    }
	    else
		if(	Application.loadedLevelName == "Map912" && gameObject.name.Substring( 0, 5 ) == "58500"	)
		{	//世界boss//
			findmRange	=	5000;
	    }
	}
	function	setenemytag()
	{
		if(	this.CompareTag(	"Enemy"		))
			Enemytag	=	"Player";
		else 
		if(	this.CompareTag(	"Player"	))		
			Enemytag	=	"Enemy";
		else 
		if(	this.CompareTag(	"Neutral"	))	//中立单位的目标是地面//
			Enemytag	=	"ground";
	}
	private	var	bossspeed	=	1.0;		//标准移动速度//
	function	Start() 
	{
		if(	Application.loadedLevelName == "Map200"	)		//引导关卡//
		{
			alertRange	=	200;	//警戒范围//
			PauseAttack(	true	);		//不主动攻击，但是还手//
			if(	Status.getMonsterLevel()	==	1	)
				Status.ATK	=	Status.ATK*2;
		}
		MNetView	=	GetComponent(MonsterNetView);
		Originalposition	=	transform.position;
		if(	anim_["spawn"]	)
		{
			IsSpawning	=	true;
			yield ;
			SendMessage("SyncAnimation","spawn");
			Invoke(	"CloseSpawn",	anim_["spawn"].length	);
			yield	WaitForSeconds(	anim_["spawn"].length	);
		}
		else
		{
			IsSpawning	=	false;
		}
		SendMessage(	"SyncAnimation",	"idle"	);
		if(	Status.getMonsterLevel()	<	3	)		//根据等级初始化数据//
			bossspeed = 0.8;
	}

	function	CloseSpawn()
	{
		IsSpawning	=	false;
	}
	private var Pauseat	= false;
	function	PauseAttack(	DoesFightBack	:	boolean	)
	{
		Pauseat	=	DoesFightBack;
		if(	DoesFightBack	==	false	)
			attackRange	=	1;
	}
	function	ForceAttack()
	{
		alertRange	=	200;
		attackRange	=	180;
		findmRange	=	300;
		Pauseat		=	false;
	}
	function	Settargetm(	target:Transform	)
	{
		targetm		=	target;
		alertRange	=	9;
		attackRange =	9;
		findmRange	=	300;
		Pauseat		=	false;
	}
	function	Settargetp(	target:Transform	)
	{
		targetp	=	target;
	}
	private	var _tDatas:String[] = new	String[2];
	function	OnAcceptLocalDecision( NewDID	: int, Data:String )
	{
		_tDatas[0]	=	""	+	NewDID;
		_tDatas[1]	=	Data;
		OnAcceptDecision( _tDatas );
	}
	function	AbleToTakeAction()	:	boolean
	{
		if(	Status.dead	)	return	false;
		if(	stun||bing	)	return	false;
		return	true;
	}
	var	_OnAcceptData	:	String[];
	var	_NewDecide	:	int;
	///处理收到的决策//
	function	OnAcceptDecision(	Data	:	String	[]	)
	{
		_NewDecide		=	parseInt( Data[0] );				//第一个是消息ID//
		SetCurrentDecisionID(	_NewDecide	);
		_OnAcceptData	=	Data[1].Split(";".ToCharArray());	//第二个是命令内容//
		switch(	parseInt( _OnAcceptData[0] ) 	)				//第1个值为命令类型//
		{
			case	2://E_AIStatus.Idle:
				AcceptIdle( _OnAcceptData );
				break;
			case	3://E_AIStatus.Battle:
				AcceptBattle( _OnAcceptData );
				break;
			case	5://不在状态机内，单独执行指令//:
				AcceptSkillJ( _OnAcceptData );
				break;
			case	6://不在状态机内，单独执行指令//:
				AcceptSkill( _OnAcceptData );
				break;
			case	7://不在状态机内，单独执行指令//:
				AcceptShoot( _OnAcceptData );
				break;
			case	8://不在状态机内，单独执行指令//:
				AcceptCloseAttack( _OnAcceptData );
				break;
			case	10://E_AIStatus.FallBack:
				AcceptFallBack( _OnAcceptData );
				break;
		}
	}
	//----------------------------------------------------------闲逛----------------------------------------------------------//
	//决策建议闲逛//
	private	var	RealNavSpeed:float;	//实际闲逛速度//
	private	var	FWStr		:String;//这次闲逛目标状态//
	private	var	IdleTime	:float;	//这次闲逛多久//
	function	DecideIdle()
	{
		if(	targetm	==	null	) 	//如果没有召唤者（不需要跟随）//
		{
			RealNavSpeed	=	Movespeed	*	Starspeed	*	selfspeed* 0.3f;
			FWStr			=	GetFindWayString(	Originalposition	+	Vector3(	Random.Range(-3,3),	1,	Random.Range(-15,15)	)	);
			IdleTime		=	Random.Range(3,8);
		}
		else
		{
			RealNavSpeed	=	Movespeed * Starspeed * selfspeed * 1.1f;
			FWStr			=	GetFindWayString(	targetm.position	+	targetm.right*2.0f ); 
			IdleTime		=	1;
		}
		CurrentAIStatus	=	E_AIStatus.WaitingDecision;
		if(	!ObjectAccessor.IsSingleCarbon()	)
		{
			MonsterServerRequest.RequestIdle(	MNetView.MonsterID,	GetCurrentDecisionID(),	2,	RealNavSpeed,	FWStr,	IdleTime	);
		}
		else
		{	
			OnAcceptLocalDecision(	GetCurrentDecisionID(),	MonsterServerRequest.IdleAIData(	2,	RealNavSpeed,	FWStr,	IdleTime	)	);
		}
		
	}
	var	AcceptedIdleTime:float	=	0.0f;	///接受的闲逛时间//
	function	AcceptIdle(	AcceptData	:String[])
	{
		//Debug.Log("K________OnAccetpIdle: " + DecisionID );
		SwitchAIStatus(	E_AIStatus.Idle	);
		SendMessage("SynMonsterspeed", parseFloat(	AcceptData[1]	) );
		SendMessage("SynMonsterMovement",	GetFindWayIntArray( AcceptData[2] )	);
		AcceptedIdleTime	=	parseFloat(	AcceptData[3]	);					//进入Idle时间//
	}
	function	UpdateIdle()
	{
		AcceptedIdleTime	-=	Time.deltaTime;
		if(	AcceptedIdleTime	<	0.0f	)
		{
			//KDebug.Log( "IdleFinished", transform, Color.magenta );
			CurrentAIStatus	=	E_AIStatus.NotDecideYet;
		}
	}
	function	OnExitIdle()
	{
		selfspeed	=	bossspeed;
		SendMessage( "PlayAudio", 1	);
	}
	//----------------------------------------------------------闲逛----------------------------------------------------------//
	//这正常角色决定自己该干什么并建议//
	function	MakeDecision()
	{
		if(	SearchTargetStatus	==	E_SearchTargetStatus.NoTargetWithinAlertRange	)	//没有敌人时//
		{
			if(	CurrentAIStatus	==	E_AIStatus.NotDecideYet	||	CurrentAIStatus	==	E_AIStatus.Battle	)
			{
				DecideIdle();	//没有敌人时统一闲逛//
			}
			if(	CurrentAIStatus	==	E_AIStatus.Attack	)
			{
				
				DecideFallBack(false);	//没有敌人时统一闲逛//
			}
		}else
		if(	SearchTargetStatus	==	E_SearchTargetStatus.TargetWithinAlert	)			//警戒时//
		{
			if(	( CurrentAIStatus	==	E_AIStatus.NotDecideYet	||	CurrentAIStatus	==	E_AIStatus.Idle	)	&&	CurrentAIStatus	!=	E_AIStatus.Battle	)
			{
				DecideBattle();
			}
		}else
		if(	SearchTargetStatus	==	E_SearchTargetStatus.TargetFocused	)				//锁定目标时//
		{
			if(	CurrentAIStatus	!=	E_AIStatus.FallBack	)
				SwitchAIStatus(	E_AIStatus.Attack	);
		}
	}
	
	function	CannotAttackMakeDecision()	//不能攻击的角色决策//
	{
		if(	Pauseat	)	//非凝视状态全都转化成凝视//
		{
			if(	CurrentAIStatus !=	E_AIStatus.Battle	)
				DecideBattle();
		}
		else
		{
			if(	CurrentAIStatus ==	E_AIStatus.Battle	)
				DecideIdle();
		}
		if(	CurrentAIStatus	==	E_AIStatus.NotDecideYet	)
		{
			DecideIdle();	//没有敌人时统一闲逛//
		}
		else
		if(	CurrentAIStatus	==	E_AIStatus.Idle	&&	beattack	)	//闲逛时被攻击
		{
			DecideIdle();	//没有敌人时统一闲逛//被攻击了就换个方向跑呗//
		}
	}
	function	PauseatMakeDecision()	//停止攻击的角色决策//
	{
		if(	CurrentAIStatus !=	E_AIStatus.Battle	)	//非凝视状态全都转化成凝视//
		{
			DecideBattle();
		}
	}
	///索敌每0.3秒进行一次//
	function	SearchTarget()
	{
		if(	CurrentAIStatus ==	E_AIStatus.FallBack	)
			return;
		if(	!Canattack	)	//不能攻击的角色不索敌//
		{
			CannotAttackMakeDecision();
			return	;
		}
		//----------------------------------------- 索 敌 -----------------------------------------//
		targetp	=	Status.FindHatestEnemy(	Enemytag	);	//最高仇恨//
		if(	targetp	!=	null	)
		{	
			//KDebug.Log( "最高仇恨", transform, Color.red );
			SearchTargetStatus	=	E_SearchTargetStatus.TargetFocused;					//222有敌人//
		}
		else
		{
			targetp	=	FindClosestEnemy();			//找到最近的敌人//
			if(	targetp	!=	null	)				//如果有最近的敌人//
			{
				var dis	=	Vector3.Distance(transform.position, targetp.position);    
				if (	dis > alertRange	)		//警戒范围外//
				{
					SearchTargetStatus	=	E_SearchTargetStatus.NoTargetWithinAlertRange;	//000没有目标//
				}
				else if (	dis	>	attackRange	)	//进攻范围外//
				{
					SearchTargetStatus	=	E_SearchTargetStatus.TargetWithinAlert;			//111警戒//
				}
				else
				{	  
					var hit : RaycastHit;	
					if(	Physics.Linecast (		//从自己位置到目标位置做射线判断是否遮挡//
							Vector3( transform.position.x,transform.position.y+3,transform.position.z ), 
							Vector3( targetp.position.x,targetp.position.y+3,targetp.position.z), hit )	)
					{
						//KDebug.Log( "最近距离", transform, Color.blue );
						SearchTargetStatus	=	E_SearchTargetStatus.TargetFocused;			//222有敌人//
				 	}
				 	else
				 	{
				 		SearchTargetStatus	=	E_SearchTargetStatus.TargetWithinAlert;			//111警戒//
				 	}
				}
			}
			else
			{
				SearchTargetStatus	=	E_SearchTargetStatus.NoTargetWithinAlertRange;		//000没有目标//
			}
		}
		if( Pauseat )	//停止攻击的角色直接保持凝视//
		{
			PauseatMakeDecision();
		}
		else			//没停止攻击的角色//
		{
			MakeDecision();
		}
	}
	///玩家死亡之后调用，对AI作相应的处理//
	function	OnTargetDead()
	{
		SwitchAIStatus(	E_AIStatus.NotDecideYet	);
		targetp	=	null;
	}

	//-------------------------------------------------决定盯着某个目标--------------------------------------------------//
	private	var LastBattle	:	float	=	-1;
	///决定锁定某个敌人//
	function	DecideBattle()
	{	//Battle是3//
		if(	Time.time	>	LastBattle	+	AttckDecisionTimeInterval	)
		{
			LastBattle	=	Time.time;
			if(	!ObjectAccessor.IsSingleCarbon()	)
			{
				MonsterServerRequest.RequestBattle(	MNetView.MonsterID,	GetCurrentDecisionID(),	3	);
			}
			else
			{	
				OnAcceptLocalDecision(	GetCurrentDecisionID(),	MonsterServerRequest.BattleAIData(	3	)	);
			}
		}
	}
	///接收到盯着某人看的消息//
	function	AcceptBattle(	Data : String[]	)
	{
		SwitchAIStatus(	E_AIStatus.Battle	);
		monsterAnimation.SyncAnimation("battle");
//		angle	=	180.0;
//		angleo	=	transform.eulerAngles.y;
	}
	//始终朝向//
	function	UpdateBattle()
	{
		if(targetp == null)
			return;
	   	qrelative	=	transform.InverseTransformPoint(targetp.position);
	        angle =	Mathf.Atan2 (qrelative.x, qrelative.z)	*	Mathf.Rad2Deg;
		if(	Mathf.Abs(angle)	>	15	&&	AbleToTakeAction()	&&	targetp	)
		{     
			angle	=	Mathf.Abs(RotateTowardsPosition(targetp.position,200));
		}
	}
	//离开Battle状态的后续工作//
	function	OnExitBattle()
	{
		if(	Math.Abs(	transform.eulerAngles.y	-	angleo	)	>	10	)
			SendMessage( "SyncRot", transform.eulerAngles.y	);
			//MNetInit.SyncRotation(transform.eulerAngles.y);
		if(	agent	)
			agent.enabled = true;
	}
	//-------------------------------------------------决定盯着某个目标--------------------------------------------------//
	//AI状态机 出口 和 入口 //
	function	SwitchAIStatus(	NewStatus	:	E_AIStatus	)
	{
		if(	CurrentAIStatus	==	E_AIStatus.Battle	)
		{	//结束Battle状态的后续工作//
			OnExitBattle();
		}
		else
		if(	CurrentAIStatus	==	E_AIStatus.Idle	)
		{	//结束Idle状态的后续工作//
			OnExitIdle();
		}
		CurrentAIStatus		=	NewStatus;
		if(	CurrentAIStatus	==	E_AIStatus.Attack	)
		{
			OnEnterAttack();
		}
		if(	CurrentAIStatus	==	E_AIStatus.NotDecideYet	)
		{
			targetp	=	null;
		}
	}
	private	var _LatsTimef	:	float	=	-1;	//记录请求超时的警戒时间点//
	function	GetCurrentDecisionID()	:	int
	{
		if(	_LatsTimef	< Time.time	)
		{
			if( CurrentAIStatus == E_AIStatus.Attack )
			{
				DecisionID	++;
				_LatsTimef	=	Time.time + 3;
			}
		}
		return	DecisionID;	
	}
	
	function	SetCurrentDecisionID(	NewID	: int	)
	{	
		_LatsTimef	=	Time.time	+	5;
		DecisionID	=	NewID;
	}
	
	private var AttackTime : float = 0;
	function	Update()
	{
		if(	IsSpawning	)
			return;
		InvokeRepeatTimeFlage	+=	Time.deltaTime;
		if(	InvokeRepeatTimeFlage	>	0.5	)
		{
			InvokeRepeatTimeFlage	-=	0.5;
			SearchTarget();
		}
		
		if(	!Status.dead	)
		{
			switch(	CurrentAIStatus	)
			{
				case	E_AIStatus.Idle:
					UpdateIdle();
					break;
				case	E_AIStatus.Battle:
					UpdateBattle();
					break;
				case	E_AIStatus.Attack:
					UpdateAttack();
					break;
				case	E_AIStatus.FallBack:
					UpdateFallBack();
					break;
			}
		}
	}

	private var snum	:	int =1;
	private var damageT :	int;
	private var angle	=	180.0;
	private var angleo	=	180.0;
	
	private	var	IsRunAttacking	: int	=	0;
	///Old进攻逻辑//
//	function	Attack()	
//	{
//		if(	!PhotonNetwork.room	||	beattack	)
//		{
//			return;
//		}
//		if(	IsRunAttacking	>	0	)
//			return;
//			
//		IsRunAttacking	++;
//		
//		photonView.RPC("SynMonsterspeed",Movespeed*Starspeed*selfspeed);
//		//agent.speed=Movespeed*Starspeed*selfspeed;
//		damageT	=	Status.ATK + Status.ATK*0.1*Random.Range(0,5);
//		if(	!targetp	)		//没有目标//
//		{
//			IsRunAttacking	--;
//			return;
//		}
//		offset	=	transform.position	-	targetp.position;   
//	
//		if(	Status.getMonsterLevel()<3 && 			//怪物难度等级//
//			Status.Level>10	&&						//属性等级大于10//
//			parseInt(Status.Health) < parseInt(Status.Maxhealth)*0.5 && 
//			juexin>0 && 
//			skilltime + Status.skillCD < Time.time && 
//			offset.magnitude < remoteRange	)
//		{
//			photonView.RPC("SyncAnimation","battle");
//			agent.enabled = false;
//			yield	StartCoroutine("SkillJ");
//		}
//		else 
//		if(	Status.getMonsterLevel()<3 && 
//			parseInt(Status.Health) < parseInt(Status.Maxhealth)*0.8 && 
//			Status.Mana>70 && 
//			Mskill && 
//			!Mskill.busy && 
//			skilltime + Status.skillCD < Time.time && 
//			Mskill.SkillP.Length>snum &&
//			(( Mskill.SkillP[snum].skillType==0 && offset.magnitude < 5)||( Mskill.SkillP[snum].skillType!=0 &&offset.magnitude < remoteRange))	)
//		{
//			photonView.RPC("SyncAnimation","battle");
//			agent.enabled = false;
//			angle = 180.0;
//			angleo = transform.eulerAngles.y;
//			while (angle > 5 &&!stun &&!bing &&!Status.dead && targetp)
//			{     
//				angle = Mathf.Abs(RotateTowardsPosition(targetp.position,200));
//				yield;
//			}
//			if(	Math.Abs(transform.eulerAngles.y - angleo)	>	10	)
//				photonView.RPC("SyncRot",transform.eulerAngles.y);
//			yield StartCoroutine("Skills",snum);
//		}
//		else if((Canremote && offset.magnitude < remoteRange)||(!Canremote && offset.magnitude < 5))
//		{ 
//			photonView.RPC("SyncAnimation","battle");
//			agent.enabled = false;
//			angle = 180.0;
//			angleo = transform.eulerAngles.y;
//			while (angle > 5 &&!stun &&!bing &&!Status.dead && targetp)
//			{     
//				angle = Mathf.Abs(RotateTowardsPosition(targetp.position,200));
//				yield;
//			}
//			if(	Math.Abs(transform.eulerAngles.y - angleo)	>	10	)
//				photonView.RPC("SyncRot",transform.eulerAngles.y);
//			if(	Canremote && Time.time > delayAttackTime*Status.Attackspeed + lastatktime	)
//				yield	StartCoroutine("Shoot");
//			else 
//			if(	!Canremote && Time.time > delayAttackTime*Status.Attackspeed + lastatktime	)
//				yield	StartCoroutine("CloseAttack");
//		}
//		else
//		{  
//			if(	stun||bing||Status.dead	)
//			{
//				agent.enabled = false;
//				IsRunAttacking--;
//				return;
//			}
//			else 
//			if(	!targetp) 
//			{
//				agent.enabled = true;
//				IsRunAttacking--;
//				return;
//			}  
//			else
//			{
//				Findway(targetp.position); 
//				yield;
//			}
//		}
//		snum	+=	1;
//		if(	snum	>	3	)   
//	        snum	=	1;
//		if(	targetp && targetp.tag	!=	Enemytag	)
//		{
//			agent.enabled = true;
//	//            Status.removeHatred(FindID(targetp));
//			IsRunAttacking--;
//			return;
//		}
//		if(	targetm	) 
//			Originalposition = targetm.position; 
//		if((transform.position -Originalposition).magnitude > findmRange)
//		{  
//	//        Status.removeHatred(FindID(targetp));
//			juexin = 5;
//			photonView.RPC("SynMonsterspeed",2*Starspeed);
//	//        agent.speed=2*Starspeed;
//			Findway(Originalposition); 
//	        yield	WaitForSeconds(5); 
//			if(!PhotonNetwork.room)
//			{
//				IsRunAttacking--;
//				return;
//			}
//			photonView.RPC("cleanHatreda",PhotonTargets.All);
//			Status.AddHealth(parseInt(Status.Maxhealth));   
//	  	}
//	  	IsRunAttacking--;
//	}
	//---------------------------------------------------------------------Attack---------------------------------------------------------------------//
	function	OnEnterAttack()
	{	//攻击模式的入口 //
		SendMessage(	"SyncAnimation",	"battle"	);
	}
	//作攻击的 -
	function	UpdateAttack()	
	{
		if( beattack	)
		{
			return;
		}
		if(	!targetp	)		//没有目标//
		{
			return;
		}
		if(	IsRunAttacking	>	0	)
		{
			return;
		}
		IsRunAttacking++;
		
		SendMessage( "SynMonsterspeed", Movespeed * Starspeed * selfspeed );		//TA!同步速度//
		damageT	=	Status.ATK	+	Status.ATK	*	0.1	*	Random.Range(0,5);	//TA!计算实际伤害//
		
		offset	=	transform.position	-	targetp.position;   //从怪物指向自己//
					qrelative	=	transform.InverseTransformPoint(targetp.position);
	        angle =	Mathf.Atan2 (qrelative.x, qrelative.z)	*	Mathf.Rad2Deg;
	        angleo	=	transform.eulerAngles.y;
		if(	Status.getMonsterLevel()	<	3	&& 						//怪物难度等级小于3//
			Status.Level				>	10	&&						//属性等级大于10//
			parseInt(Status.Health)	<	parseInt(Status.Maxhealth)*0.5 &&	//血量	低于50%//
			juexin	>	0 && 									
			skilltime + Status.skillCD < Time.time && 
			offset.magnitude < remoteRange	)
		{		
			DecideSkillJ();	//<1>决定爆发技//
		}
		else 
		if(	Status.getMonsterLevel()<	3	&& 
			parseInt(Status.Health)	<	parseInt(Status.Maxhealth)*0.8 && 
			Mskill			&& 
			!Mskill.busy	&& 
			skilltime + Status.skillCD	<	Time.time	&& 
			Mskill.SkillP.Length	>	snum	&&
			(	(	Mskill.SkillP[snum].skillType	==0 && offset.magnitude < punchRadius	)	||	
				(	Mskill.SkillP[snum].skillType	!=0 && offset.magnitude < remoteRange)	)	)
		{   
			agent.enabled	=	false;
			if(	Math.Abs(angle) > 15	&&	AbleToTakeAction()	&&	targetp	)
			{     
				angle	=	Mathf.Abs(RotateTowardsPosition(targetp.position,200));
			}
			DecideSkill(	snum	);						//<2>决定使用技能//
		}
		else if( ( Canremote && offset.magnitude < remoteRange ) || ( !Canremote && offset.magnitude < punchRadius ) )
		{	//近战或者远程//
		    
			agent.enabled	=	false;
			if(	Math.Abs(angle) > 15	&&	AbleToTakeAction()	&&	targetp	)
			{     
				angle	=	Mathf.Abs(	RotateTowardsPosition(	targetp.position,	200	)	);
			}
			if(	Canremote && Time.time > delayAttackTime	*	Status.Attackspeed	+	lastatktime	){
			    Status.SaveHatred(	FindID(targetp),1);
				DecideShoot();	
				}							//<3>决定远程攻击//
			else 
			if(	!Canremote && Time.time > delayAttackTime	*	Status.Attackspeed	+	lastatktime	){
				DecideCloseAttack();	
				Status.SaveHatred(	FindID(targetp),1);
				}					//<4>决定近战攻击//
		}
		else	//------------------------------------------//<5>决定接近目标//以下为--------------------------------------//
		{  
			if(	!AbleToTakeAction()	)
			{
				agent.enabled	=	false;
				IsRunAttacking--;
				return;
			}
			else 
			if(	!targetp	) 
			{
				agent.enabled	=	true;
				IsRunAttacking--;
				return;
			}  
			else
			{
				if(	CurrentAIStatus	!=	E_AIStatus.FallBack	&& Canmove)
					Findway(	targetp.position	); 
			}
		}
		snum	+=	1;
		if(	snum	>	3	)   	//技能使用索引的轮换//
	        snum	=	1;
		if(	targetp	!=	null	&&	targetp.tag	!=	Enemytag	)
		{	//敌人的tag切换了//
			Status.removeHatred(	FindID(targetp)	);
			targetp	=	null;
			agent.enabled	=	true;
			SwitchAIStatus( E_AIStatus.NotDecideYet );
			IsRunAttacking	--;
			return;
		}
		///-------------------------------------------------------FallBack!-------------------------------------------------------///
	  	if(	targetm	)
			Originalposition	=	targetm.position; 
		if(	( transform.position - Originalposition ).magnitude > findmRange )
		{  	
			if(	targetp	!=	null)
		  {	
			Status.removeHatred(	FindID(targetp)	);
			targetp	=	null;
	    	}
			DecideFallBack(false);
	  	}
	  	IsRunAttacking--;
	}
	//---------------------------------------------------------------------Attack---------------------------------------------------------------------//
	
	private	var	AttckDecisionTimeInterval	:	float	=	1.0;
	//----------------------------------5-----------------------------------SkillJ---------------------------------------------------------------------//
	///决定爆发 5 //
	private	var	LastSkillJ	:	float	=	-1;
	function	DecideSkillJ()
	{
		if(	Pauseat	)
			return;
		if(	Time.time	>	LastSkillJ	+	AttckDecisionTimeInterval	)
		{
			LastSkillJ	=	Time.time;
			if(	!ObjectAccessor.IsSingleCarbon()	)
			{
				MonsterServerRequest.RequestSkillJ(	MNetView.MonsterID,	GetCurrentDecisionID(),	5	);
			}
			else
			{	
				OnAcceptLocalDecision(	GetCurrentDecisionID(),	MonsterServerRequest.SkillJAIData(	5	)	);
			}
		}
	}
	private	var	_IsSkillJ:boolean	=	false;
	//接受技能J//
	function	AcceptSkillJ(	Data:String	[]	)
	{
		if(	CurrentAIStatus	!=	E_AIStatus.Attack	)
			return;
		if(	_IsSkillJ	)
			return;
		_IsSkillJ	=	true;
		
		IsRunAttacking++;
		SendMessage(	"SyncAnimation"	,	"battle"	);
		agent.enabled	=	false;
		if(	!Mskill	)	//没有技能//
		{
			juexin	=	0;
		}
		else if(	!Mskill.busy	)
		{
			if(juexin>1)
			{
				Mskill.Skill(5);	//baonu
				juexin		-=	1;
				skilltime	=	Time.time;
			} 
			else
			{
				Mskill.Skill(0);	//juexing
				juexin		=	0;
			}  
			yield	WaitForSeconds(0.4);
			Status.huandon( 2,	transform.position	);
		}
		IsRunAttacking--;
		_IsSkillJ	=	false;
	}
	//---------------------------------------------------------------------SkillJ---------------------------------------------------------------------//
	
	
	//---------------------------------------------------------------------Skills---------------------------------------------------------------------//
	///决定使用技能 6 //
	private	var	LastSkill	:	float	=	-1;
	function	DecideSkill(	index	:	int	)
	{
		if(	Pauseat	)
			return;
		if(	!AbleToTakeAction()	||	!targetp	||	(	!Status.BTMode && beattack	) )
			return;
		if(	Time.time	>	LastSkill	+	AttckDecisionTimeInterval	)
		{
			LastSkill	=	Time.time;
			if(	!ObjectAccessor.IsSingleCarbon()	)
			{
				MonsterServerRequest.RequestSkill(	MNetView.MonsterID,	GetCurrentDecisionID(),	6, index	);
			}
			else
			{	
				OnAcceptLocalDecision(	GetCurrentDecisionID(),	MonsterServerRequest.SkillAIData(	6, index	)	);
			}
		}
	}
	private	var	_IsSkill:boolean	=	false;
	private	var	UsedSKillID:int;
	//接受技能//
	function	AcceptSkill( Data:String[] )
	{
		if(	CurrentAIStatus	!=	E_AIStatus.Attack	)
			return;
		if(	_IsSkill	)
			return;
		_IsSkill	=	true;
		IsRunAttacking++;
		if(	!AbleToTakeAction()||!targetp||(!Status.BTMode && beattack) )
		{
			IsRunAttacking	--	;
			_IsSkill	=	false;
			return;
		}
		UsedSKillID	=	parseInt(	Data[1]	);
		Mskill.Skill(	UsedSKillID	);
		skilltime	=	Time.time;
		yield ;
		while(	Mskill.busy	)
		{
			yield	WaitForSeconds(0.1);
		}
		Status.addmana(	- Status.MaxMana * 0.01 * Mskill.SkillP[ UsedSKillID ].Cost );
		IsRunAttacking--;
		_IsSkill	=	false;
	}
	//---------------------------------------------------------------------Skills---------------------------------------------------------------------//
	
	
	//---------------------------------------------------------------------Shoot---------------------------------------------------------------------//
	//7
	private	var	LastShoot	:	float	=	-1;
	function	DecideShoot()
	{
		if(	Time.time	>	LastShoot	+	AttckDecisionTimeInterval	)
		{
			if(	!Pauseat	)
			{
				LastShoot	=	Time.time;
				if(	!ObjectAccessor.IsSingleCarbon()	)
				{
					MonsterServerRequest.RequestShoot(	MNetView.MonsterID,	GetCurrentDecisionID(),	7	);
				}
				else
				{	
					OnAcceptLocalDecision(	GetCurrentDecisionID(),	MonsterServerRequest.ShootAIData(	7	)	);
				}
			}
		}
	}
	private	var	_IsShoot	:	boolean	=	false;
	function	AcceptShoot( Data:String[] )
	{
		if(	CurrentAIStatus	!=	E_AIStatus.Attack	)
			return;
		if(	_IsShoot	)
			return;
		_IsShoot	=	true;
		
		IsRunAttacking	++;
		if(	!AbleToTakeAction()	||	!targetp	||	(!Status.BTMode && beattack)	)
		{
			IsRunAttacking	--;
			_IsShoot	=	false;
			return;
		}
		if(	anim_["ready"]!= null)
		{
			SendMessage(	"SyncAnimation","ready" );
		}
		else
			SendMessage(	"SyncAnimation","battle" );
		if(	fxobject	==	null	)
			fxobject	=	Resources.Load("BulletObject1", GameObject);
		yield	WaitForSeconds(	delayAttackTime	-	1	);
		if(	beattack	)
		{
			IsRunAttacking--;
			_IsShoot	=	false;
			return;
		}
		SendMessage(	"SyncAnimation",	"attackb1"	);
		yield	WaitForSeconds(	0.3	);
		SendMessage(	"PlayAudio",	2	);
		if(	beattack	)
		{
			IsRunAttacking--;
			_IsShoot	=	false;
			return;
		}
		///------创建发射物体------RPC!!!!!!//
		var cc	=	PhotonNetwork.Instantiate( fxobject.name, transform.position	+	transform.up*2	+	transform.forward*2	, transform.rotation, 0 );
		var skillobject : Bulletobject	=	cc.GetComponent(	Bulletobject	); 
		skillobject.targetp		=	transform.position + transform.up*2 + transform.forward*40;
		skillobject.PlayerID	=	Status.PlayerID;
		skillobject.Damagetype	=	Damagetype;
		skillobject.attackerLV	=	Status.Level; 
		skillobject.mideffectID	=	mideffect;    
		skillobject.endeffectID	=	endeffect; 
		skillobject.damage		=	damageT;
		skillobject.Enemytag	=	Enemytag; 
		///------创建发射物体------//
		yield	WaitForSeconds( 0.7 );
		if(	Status.monsterType == MonsterType.strong	)
			Status.addmana( 2 );  
		lastatktime	=	Time.time;
		_IsShoot	=	false;
		IsRunAttacking	--;
	}
	//---------------------------------------------------------------------Shoot---------------------------------------------------------------------//
	
	
	//---------------------------------------------------------------------CloseAttack---------------------------------------------------------------------//
	//8
	
	private	var	LastCloseAttack	:	float	=	-1;
	
	function	DecideCloseAttack()
	{
		if(	Time.time	>	LastCloseAttack	+	AttckDecisionTimeInterval	)
		{
			if(	!Pauseat	)
			{	
				//KDebug.Log( "怪 =》 " + MNetView.MonsterID +"请求攻击！", transform );
				LastCloseAttack	=	Time.time;
				if(	!ObjectAccessor.IsSingleCarbon()	)
				{
					MonsterServerRequest.RequestCloseAttack(	MNetView.MonsterID,	GetCurrentDecisionID(),	8	);
				}
				else
				{	
					OnAcceptLocalDecision(	GetCurrentDecisionID(),	MonsterServerRequest.CloseAttackAIData(	8 )	);
				}
			}
		}
	}
	private	var	_IsCloseAttack	:	boolean	=	false;
	function	AcceptCloseAttack( Data:String[] )
	{
		if(	CurrentAIStatus	!=	E_AIStatus.Attack	)
			return;
		if(	_IsCloseAttack	)
		{
			//Debug.Log("ReFuse __  AcceptCloseAttack");
			return;
		}
		_IsCloseAttack	=	true;
		IsRunAttacking++;
		if(	!AbleToTakeAction()	||	!targetp	||	(!Status.BTMode && beattack)	)
		{
			IsRunAttacking--;
			_IsCloseAttack	=	false;
			return;
		}
		///轮换连击动作//
		if(	n < attackcombo &&	Time.time - lastatktime <3.0)	
			n	+=	1;
		else
			n	=	1;
		SendMessage(	"SyncAnimation","attacka" + n	);
		if(	attackEFID	!=	0	)
			SendMessage(	"PlayEffect",attackEFID);
		SendMessage(	"PlayAudio",	2	);
		yield	WaitForSeconds(0.3);
		if( beattack )
		{
			IsRunAttacking--;
			_IsCloseAttack	=	false;
			return;
		}
	//	try
	//	{
			var settingsArray		=	new int[5];
				settingsArray[0]	=	Status.PlayerID;
				settingsArray[1]	=	damageT;
				settingsArray[2]	=	damageT;
				settingsArray[3]	=	Damagetype;
				settingsArray[4]	=	Status.Level; 
			var pos	=	transform.TransformPoint(	punchPosition	);
			if(	targetp &&(	pos	-	targetp.position	).magnitude < punchRadius	)
			{	
				targetp.SendMessage(	"ApplyDamage",	settingsArray,	SendMessageOptions.DontRequireReceiver	);
				if( Status.getMonsterLevel()<3)
					Status.huandon( 1,	transform.position	);
				if(Status.monsterType == MonsterType.strong)
					Status.addmana(	2	); 
				lastatktime = Time.time;
		    }
//		}
//		catch( e )
//		{
//			_IsCloseAttack	=	false;
//			IsRunAttacking--;
//		}
		_IsCloseAttack	=	false;
		IsRunAttacking--;
	}
	//---------------------------------------------------------------------CloseAttack---------------------------------------------------------------------//
	
	//---------------------------------------------------------------------FallBack---------------------------------------------------------------------//
	//10
	private	var	LastFallBack	:	float	=	-1;
	function	DecideFallBack( huixue : boolean)
	{
		if(	Time.time	>	LastFallBack	+	AttckDecisionTimeInterval	)
		{
			LastFallBack	=	Time.time;
			
			if(huixue)
			MonsterServerRequest.RequestFallBack(	MNetView.MonsterID,	GetCurrentDecisionID(),	10,	1);
			else
			MonsterServerRequest.RequestFallBack(	MNetView.MonsterID,	GetCurrentDecisionID(),	10,	0);
		}
	}
	///记录这个怪撤退时是否回复血量//
	private	var	RecoverOnFallback	:	boolean	=	true;
	function	AcceptFallBack( Data:String[] )
	{
		SwitchAIStatus(	E_AIStatus.FallBack	);
		juexin = 5;
		SendMessage(	"SynMonsterspeed",	2	*	Starspeed	);
		Findway(	Originalposition	); 
		//yield	WaitForSeconds(5); 
		
	}
	
	private	var	_DisToOri	:	float;	//记录回退中离目的地的距离//
	
	function	UpdateFallBack()
	{
//	Debug.Log("1111111111111111111111111111");
		if(	targetm	) 
		{		//跟随召唤者//
			Originalposition	=	targetm.position;
			_DisToOri	=	(	transform.position	-	Originalposition	).magnitude;
			if(	_DisToOri	>	DefinedFallBackCheckDistance	)	//还没回去//
			{  
				Findway(	Originalposition	); 
		  	}
		  	else
		  	{
		  		SwitchAIStatus(	E_AIStatus.NotDecideYet	);
		  	}
	  	}
	  	else	//回到出生点//
	  	{
	  		_DisToOri	=	(	transform.position	-	Originalposition	).magnitude;
//	  		Findway(	Originalposition	); 
	  		if(	_DisToOri	>	DefinedFallBackCheckDistance	)	//还没回去//
			{  
					Findway(	Originalposition	); 
		  	}
		  	else
		  	{   if(CurrentAIStatus == E_AIStatus.Attack)
		  	       return;
				SwitchAIStatus(	E_AIStatus.NotDecideYet	); 
				if(	SearchTargetStatus	==	E_SearchTargetStatus.TargetFocused	)				//锁定目标时//
				  SwitchAIStatus(	E_AIStatus.Attack	);
				else
				DecideFallBack( true);
		  	}
	  	}
	}
	
	//---------------------------------------------------------------------FallBack---------------------------------------------------------------------//
	private	var _tps:PlayerStatus;
	private	var _tms:MonsterStatus;
	function	FindID(target:Transform) : int
	{
		if(	target == null	)
		{
			return 0;
		}
		var	diff :	int	=	0;
		_tps	=	target.GetComponent(PlayerStatus);
		if( _tps	==	null	)
		{
			_tms	=	target.GetComponent(MonsterStatus);
			if(	_tms	!=	null	)
			{
				diff	=	_tms.PlayerID;
			}
		}
		else
		{
			diff	=	_tps.instanceID;
		}
		return	diff; 
	}

	//近战攻击//
//	function	CloseAttack()
//	{
//		if(	stun	||	bing	||	Status.dead	||	!targetp	||	(!Status.BTMode && beattack)	)
//			return;
//		///轮换连击动作//
//		if(	n < attackcombo &&	Time.time - lastatktime <3.0)	
//			n	+=	1;
//		else
//			n	=	1;
//		photonView.RPC("SyncAnimation","attacka" + n);
//		if(	attackEFID	!=0	)
//			photonView.RPC("PlayEffect",attackEFID);
//		photonView.RPC("PlayAudio",2);
//		yield	WaitForSeconds(0.3);
//		if( beattack )
//			return;
//		try
//		{
//			var settingsArray = new int[5];
//				settingsArray[0]=Status.PlayerID;
//				settingsArray[1]=damageT;
//				settingsArray[2]=damageT;
//				settingsArray[3]= Damagetype;
//				settingsArray[4]= Status.Level; 
//			var pos = transform.TransformPoint(punchPosition);
//			if(targetp &&(pos -targetp.position).magnitude < punchRadius)
//			{	
//				targetp.SendMessage("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );
//				if( Status.getMonsterLevel()<3)
//					photonView.RPC("huandon",1,transform.position);
//				if(Status.monsterType == MonsterType.strong)
//					Status.addmana(2); 
//				lastatktime = Time.time;
//		    }
//		}
//		catch(e)
//		{
//		    
//		}
//		return;
//	}
	//远程攻击//
//	function	Shoot ()
//	{
//		if(stun||bing||Status.dead||!targetp||(!Status.BTMode && beattack))
//			return;
//		if(anim_["ready"]!= null)
//		{
//			photonView.RPC("SyncAnimation","ready");
//		}
//		else
//			photonView.RPC("SyncAnimation","battle");
//		if(	fxobject	==	null	)
//			fxobject	=	Resources.Load("BulletObject1", GameObject);
//		yield	WaitForSeconds(delayAttackTime-1);
//		if(	!PhotonNetwork.room	||	beattack	)
//			return;
//		photonView.RPC("SyncAnimation","attackb1");
//		yield	WaitForSeconds(0.3);
//		photonView.RPC("PlayAudio",2);
//		if(!PhotonNetwork.room|| beattack)
//			return;
//		var cc = PhotonNetwork.Instantiate( fxobject.name, transform.position+transform.up*2+transform.forward*2,transform.rotation,0);
//		var skillobject : Bulletobject = cc.GetComponent(Bulletobject); 
//		skillobject.targetp = transform.position + transform.up*2 + transform.forward*40;
//		skillobject.PlayerID = Status.PlayerID;
//		skillobject.Damagetype = Damagetype;
//		skillobject.attackerLV = Status.Level; 
//		skillobject.mideffectID = mideffect;    
//		skillobject.endeffectID = endeffect; 
//		skillobject.damage = damageT;
//		skillobject.Enemytag = Enemytag;  
//		yield	WaitForSeconds(0.7);
//		if(!PhotonNetwork.room)
//			return;
//		if(Status.monsterType == MonsterType.strong)
//			Status.addmana(2);  
//		lastatktime = Time.time;
//		return;
//	}

private	var	skilltime = -300.0;	//记录的上次释放技能的时间//
///使用技能//
//function	Skills(i:int)
//{
//	if(	stun||bing||Status.dead||!targetp||(!Status.BTMode && beattack))
//		return;
//	Mskill.Skill(i);
//	skilltime = Time.time;
//	yield ;
//	if(!PhotonNetwork.room)
//	return;
//	while(Mskill.busy)
//	{
//		yield WaitForSeconds(0.1);
//		if(!PhotonNetwork.room)
//		return;
//	}
//	Status.addmana(-Status.MaxMana*0.01*Mskill.SkillP[i].Cost);
//	return;
//}

private	var	juexin	=	5;
//爆发觉醒技能//
//function	SkillJ()
//{
//	if(	!Mskill	)	//没有技能//
//	{
//		juexin = 0;
//		return;
//	}
//	else if(	!Mskill.busy	)
//	{
//		if(juexin>1)
//		{
//			Mskill.Skill(5);//baonu
//			juexin -= 1;
//			skilltime = Time.time;
//		} 
//		else
//		{
//			Mskill.Skill(0);//juexing
//			juexin = 0;
//		}  
//		yield WaitForSeconds(0.4);
//		if(!PhotonNetwork.room)
//			return;
//		photonView.RPC("huandon",2,transform.position);
//		return;
//	}
//}

function	stunself()
{
	stun	=	true;
	agent.enabled = false;
	if(anim_["stun"])
	{   anim_["stun"].layer = 3;
		SendMessage("SyncAnimation","stun");
	}		
}

function	huiself()
{
	stun	=	false;
	agent.enabled	=	true;
	if(anim_["stun"])
	{ 
	SendMessage("StopAnimation","stun");
	anim_["stun"].layer = -2;	
	}
}

function	bingself( ice : boolean )
{
	bing	=	true;
	agent.enabled = false;
	if(	ice	)
		SendMessage( "synanispeed", 0 );
}

//解除冰冻//
function	huaself()
{
	bing			=	false;
	agent.enabled	=	true;
	SendMessage( "synanispeed", 1 );
}

private var sendSyns	:	int[]	=	new int[4];
//used
function	Findway( targetPosition : Vector3 )
{
	sendSyns[0]	=	targetPosition.x * 100;
	sendSyns[1]	=	targetPosition.y * 100;
	sendSyns[2]	=	targetPosition.z * 100;
	sendSyns[3]	=	transform.eulerAngles.y	*	10;
	SendMessage(	"SynMonsterMovement",	sendSyns	);
}
//返回用于同步的位置数据串//
function	GetFindWayString(	targetPosition : Vector3	):String
{
	return	"" + Mathf.FloorToInt(targetPosition.x * 100) + "," + Mathf.FloorToInt(targetPosition.y * 100) + "," + Mathf.FloorToInt(targetPosition.z * 100) + "," + Mathf.FloorToInt(	transform.eulerAngles.y	* 10 );
}

private var	buffer:String[];
//解析用于同步的位置数据串//
function	GetFindWayIntArray(	Buffer: String	)
{
	buffer		=	Buffer.Split(",".ToCharArray());
	sendSyns[0]	=	parseInt(	buffer[0] );
	sendSyns[1]	=	parseInt(	buffer[1] );
	sendSyns[2]	=	parseInt(	buffer[2] );
	sendSyns[3]	=	parseInt(	buffer[3] );
	return	sendSyns;
}

//玩家死了，传过来的消息//
function	Kills(info : int[])
{
	//Debug.Log("K________________________玩家死了");
	Status.removeHatred(info[0]);
	targetp = null;
	//Debug.Log( " K_____________----____玩家死了__PlayerDead = " + instanceID );
	//removeHatred(	instanceID	);
	//var _MAI : MonsterAI = GetComponent( MonsterAI );
	OnTargetDead();
}

///响应角色被攻击//
function	Behit(	target	:	Transform	)
{
	Pauseat		=	false;	
	attackRange	=	18;
	Behita(	target.tag	);
}
//僵直了//
function	Behita(	targettag : String	)
{
	if(	Application.loadedLevelName == "Map200" && Status.getMonsterLevel() == 1 && parseInt(Status.Health) < parseInt(Status.Maxhealth) * 0.4 )
	{   
		BossWasDie();	//Boss死在这里判定....？
		return;
	}
	if(	this.CompareTag( "Neutral" )	)		//中立//
		this.tag	=	"Enemy";
	Enemytag	=	targettag;
	if(	!beattack	)
	{
		beattack	=	true;
		if(	Status.getMonsterLevel()<3	)
			yield WaitForSeconds(0.1);
		else
			yield WaitForSeconds(0.2);
		beattack	=	false; 
	}
}


private var	_relative	:	Vector3;
private var	_angle		:	float;
private var	_maxRotation	:	float;
private var	_clampedAngle	:	float;
//调整朝向targetPos：朝向位置，rotateSpeed：旋转限速//
function	RotateTowardsPosition	(	targetPos : Vector3, rotateSpeed : float	) : float
{
	_relative	=	transform.InverseTransformPoint(	targetPos	);
	_angle		=	Mathf.Atan2 (_relative.x, _relative.z)	*	Mathf.Rad2Deg;
	_maxRotation	=	rotateSpeed	*	Time.deltaTime;
	_clampedAngle	=	Mathf.Clamp(_angle, -_maxRotation, _maxRotation);
	transform.Rotate( 0, _clampedAngle, 0 );
	return	_angle;
}
private var	gos		:	GameObject[];
private var	closest	:	GameObject;
private var	distance:	float;
//找到最近的敌人//
private	function	FindClosestEnemy () : Transform
{
	gos			=	GameObject.FindGameObjectsWithTag(Enemytag);
	distance	= 	Mathf.Infinity;
	closest		=	null;
	for (var go : GameObject in gos)
	{
		var diff = (go.transform.position - transform.position);
		var curDistance = diff.sqrMagnitude;
		if (curDistance < distance)
		{
			closest = go;
			distance = curDistance;
		}
	}
	if(	closest	!=	null	)
		return	closest.transform;
	else
		return	null;
}

//尤利西斯死前调用这个方法
private var TC : TalkControl;
function	BossWasDie()
{
	PauseAttack(	true	);	//停止攻击//
	//KDebug.Log("~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ Boss Was Die ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ Boss Was Die", transform, Color.red);
	Status.Unableattack	=	100;
	this.tag			=	"Neutral";
	AllManage.UICLStatic.ObjDontControl.enabled	=	true;
	//暂停wanjia攻击
	//（死亡前）动画
	anim_["baozhashuo"].wrapMode	=	WrapMode.Loop;	
	SendMessage(	"SyncAnimation",	"baozhashuo"	);

	yield	WaitForSeconds(2);
	//审判者：（狂笑声）我已得永生，而你们，终究逃不出审判。（狂笑声）
	var playerS : PlayerStatus;
	if(playerS == null && PlayerStatus.MainCharacter){
		playerS = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if(playerS != null && ! playerS.dead){
		if(	!TC	)
		{
			TC	=	FindObjectOfType( TalkControl );
		}
		TC.LevelID	=	5;
		TC.step		=	0;
		TC.ShowTalkAsStep( gameObject , "bossb" );
	}
	yield	WaitForSeconds(11);
    BossBoom(  );
}

function	BossBoom()
{
	SendMessage(	"SyncAnimation",	"baozha" );
	Mskill.Skill(	4	);//miaosha
	yield	WaitForSeconds(0.5);  
	Mskill.Skill(	4	);//miaosha
	yield	WaitForSeconds(2);
	var colliders : Collider[] = Physics.OverlapSphere ( transform.position, 200 );
	for ( var hit in colliders )
	{
		if( hit.CompareTag ( "Player" ) )
			hit.SendMessage( "Miaosha" , SendMessageOptions.DontRequireReceiver );
	}
	SendMessage( "Burn"	); 
	//杀死玩家 -
}

function	OnDrawGizmosSelected ()
{
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireSphere ( transform.TransformPoint( punchPosition ), punchRadius );
	Gizmos.color = Color.red;
	Gizmos.DrawWireSphere ( transform.position, attackRange );
	Gizmos.color = Color.blue;
	Gizmos.DrawWireSphere (	transform.position, alertRange	);
}