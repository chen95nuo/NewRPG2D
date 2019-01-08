class	PetNetInit	extends	Photon.MonoBehaviour
{
	private var Animatc		:	MonsterAnimation; 
	private var agent		:	NavMeshAgent;
	private var Status		:	MonsterStatus;
	private var subskillmove=	0.0;
	private var Size		=	1.0;
	private var skillID		:	int;
	private var target		:	Transform;
	private var targetp		:	Vector3;
	private var skillTime	=	0.5;
	private var _SkillObject:	Skillobject;
	private var usefloat	:	float;
	
	private	var	MNetView	:	MonsterNetView;
	
	function	Awake()
	{
		_SkillObject	=	GetComponent(Skillobject);
		Animatc			=	GetComponent(MonsterAnimation);
		Status			=	GetComponent(MonsterStatus);
		agent			=	GetComponent.<NavMeshAgent>();
	
	
		MNetView		=	gameObject.GetComponent( MonsterNetView );
		if(	MNetView	==	null	)
			MNetView	=	gameObject.AddComponent( MonsterNetView );
		if(	MNetView	!=	null	)
			MonsterHandler.GetInstance().RegisterNewSkull(	MNetView	);
//		else
//			KDebug.Log("AddComponent这函数居然.......返回Null?");
	}
	
	function	Start()
	{
		MNetView.SummonerID	=	_SkillObject.PlayerID;	//赋值召唤者ID//
		if(	PlayerUtil.IsLocalObject(	MNetView.SummonerID	)	)
		{
//			KDebug.Log(	"~ ！ ~ ！ ~ 发发发发发发发发发消息 ~ ！ ~ ！ ~ ", transform, Color.blue	);
			MonsterServerRequest.SummonSkull( MNetView.SummonerID, _SkillObject.GetPetMaxHealth(), String.Format("{0};{1};{2};{3};{4}", _SkillObject.damage , _SkillObject.buffID , _SkillObject.buffvalue , _SkillObject.bufftime , _SkillObject.Objlife) );
		}
	}

	function	OnPhotonInstantiate( msg : PhotonMessageInfo )
	{
		_SkillObject.enabled	=	true;
		_SkillObject.timeout();	//初始化宠物的数据//
		GetComponent(MonsterAI).enabled = true;
	}
	function	IDsy(	objs	:	Object[]	)
	{
		skillID					=	objs[0];
		subskillmove			=	objs[1];
		Size					=	objs[2];
		transform.localScale	=	Vector3( 2*Size, 2*Size, 2*Size );
	}
	function	findtarget(	ID	:	int	)
	{
		target	=	ObjectFinder.FindTargetTransform( ID );
	}

	@RPC
	function	findtargetp(pp:Vector3)
	{
		targetp	=	pp;
	}

	@RPC
	function	SynMonsterspeed(speeds : float)
	{
		agent.speed	=	speeds;
	}

	@RPC
	function	SynMonsterMovement(Syns : int[])
	{
		var pos		: Vector3;
		var rota	: float;
		usefloat	= Syns[0];
		pos.x		= usefloat / 100.0;
			
		usefloat	= Syns[1];
		pos.y		= usefloat / 100.0;
			
		usefloat	= Syns[2];
		pos.z		= usefloat / 100.0;

		usefloat	= Syns[3];
		rota		= usefloat / 10.0;
		Findwaym( pos );
		if(	Math.Abs(transform.eulerAngles.y - rota)>1	)
			smoothrota(rota);
	}

	@RPC
	function	SyncRot(rota:float)
	{
		if(	Math.Abs( transform.eulerAngles.y - rota ) > 1 )
			smoothrota(rota);
	}

	function	smoothrota( Anglesy : float )
	{
		var step =12;
		while( step > 0 )
		{     
			transform.eulerAngles.y	=	Mathf.Lerp(	transform.eulerAngles.y , Anglesy , Time.deltaTime * 8	);
			step	-=	1;
			yield;
		}
	}

	var FindTimes : int = 0;

	function	Findwaym(	targetPosition : Vector3	)
	{
		agent.enabled	=	true;
			yield;
		if(	!Status.dead	&&	agent.enabled	==	true	)
		{
	        agent.Resume();
			agent.destination	=	targetPosition;
		}
		FindTimes += 1;
		var myTimes : int = 0;
		myTimes = FindTimes;
		while( ( agent || agent.enabled == true ) && myTimes == FindTimes ) 
		{
			yield	StartCoroutine("UpdateAnimationBlend");
			yield;
			yield;
		}
	}

	private var walkAnimationSpeed	:	float = 5.0;
	private var runAnimationSpeed	:	float = 10.0;
	private	var cc : boolean = true;
	private	function	UpdateAnimationBlend()
	{
		if(	cc	)
		{
			cc = false;
			var velocityXZ : Vector3 = Vector3(agent.velocity.x, 0.0f , agent.velocity.z);
			var speed : float = velocityXZ.magnitude;
			if(	speed > walkAnimationSpeed+1) 
			{
				Animatc.anim_.CrossFade("run");
			    Animatc.AniSpeedScale = speed / runAnimationSpeed;
			}
			else if( speed > 0.1 )
			{
				Animatc.anim_.CrossFade("walk");
			    Animatc.AniSpeedScale = speed / walkAnimationSpeed;		
			}
			else
			{
				Animatc.anim_.CrossFade("idle");
				Animatc.AniSpeedScale = 1;
			}
			yield;
			yield;
			cc	=	true;
		}
		yield;
	}
}
