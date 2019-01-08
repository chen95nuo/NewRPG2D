class	MonsterNetInit	extends	Photon.MonoBehaviour
{	//Done
	private var Animatc	:	MonsterAnimation; 
	private var Status	:	MonsterStatus; 
	private var agent 	:	NavMeshAgent;
	private var MAI   : MonsterAI;
	private var usefloat:	float;
	function	Awake()
	{
		Animatc	=	GetComponent(MonsterAnimation);
		Status	=	GetComponent(MonsterStatus);
		agent	=	GetComponent.<NavMeshAgent>();
		MAI     =   GetComponent(MonsterAI);
	}
	
	//@RPC
	function	SetRemote()
	{
//		if(	PhotonNetwork.isMasterClient	)
//	    {	//主客户端开启AI和寻路，发起同步位置//
		GetComponent( MonsterAI ).enabled	=	true;
		agent.enabled = false; 
//	        //photonView.RPC("Synposition",transform.position); 
//	    }
//	    else
//	    {
//	        GetComponent(MonsterAI).enabled = false;
//	    	agent.enabled = false; 	
//		}
	}

	//@RPC
	//function	Synposition(pposition : Vector3)
	//{ 
	//	transform.position = pposition;
	//}

	function	OnMasterClientSwitched( bla : PhotonPlayer)
	{
//	    if(	PhotonNetwork.isMasterClient	)
//	    {
		GetComponent(MonsterAI).enabled = true;
		agent.enabled = true; 
//	    }
//	    else
//	    {
//	        GetComponent(MonsterAI).enabled = false;
//	    	agent.enabled = false; 	
//	    }
	} 

	//Used
	//@RPC
	function	SynMonsterspeed(speeds : float)
	{
		agent.speed	=	speeds;
	}

	//Used
	function	SynMonsterMovement(Syns : int[])
	{
		var pos		:	Vector3;
		var rota	:	float;
		usefloat	=	Syns[0];
		pos.x		=	usefloat / 100.0;
			
		usefloat	=	Syns[1];
		pos.y		=	usefloat / 100.0;
			
		usefloat	=	Syns[2];
		pos.z		=	usefloat / 100.0;

		usefloat	=	Syns[3];
		rota		=	usefloat / 10.0;
		Findwaym(	pos	);
		if(	Math.Abs( transform.eulerAngles.y - rota )	>	1	)
			smoothrota( rota );
	}

//	//new同步移动,取代上面那个函数........还没完全取代//
//	function	SyncMovement( Syns : int[] )
//	{
//		var pos		: Vector3;
//		var rota	: float;
//		usefloat	=	Syns[0];
//		pos.x		=	usefloat / 100.0;
//			
//		usefloat	=	Syns[1];
//		pos.y		=	usefloat / 100.0;
//			
//		usefloat	=	Syns[2];
//		pos.z		=	usefloat / 100.0;
//
//		usefloat	=	Syns[3];
//		rota		=	usefloat / 10.0;
//		Findwaym(pos);
//		if(	Math.Abs(transform.eulerAngles.y - rota)	>	1	)
//			smoothrota(rota);
//	}

	//Used//
	//@RPC
	function	SyncRot(rota:float)
	{
		if( Math.Abs(transform.eulerAngles.y - rota) > 1 )
			smoothrota(rota);
	}

	//new//
	function	SyncRotation(	rota	:	float	)
	{
		if(	Math.Abs(transform.eulerAngles.y - rota)	>	1	)
			smoothrota(rota);
	}

	//12帧平滑过渡朝向//
	function	smoothrota(Anglesy:float)
	{
		var	step	=	12;
		while(	step	>	0	)
		{     
			transform.eulerAngles.y	=	Mathf.Lerp(transform.eulerAngles.y , Anglesy , Time.deltaTime * 8);
			step	-=	1;
			yield;
		}
	}

	var FindTimes : int = 0;

	//used平滑过渡位置//
	function	Findwaym(	targetPosition	:	Vector3	)
	{
		agent.enabled	=	true;
		yield;
		if(	!Status.dead && agent.enabled == true)
		{
			agent.Resume();
			agent.destination = targetPosition;
		}
		FindTimes	+=	1;
		var myTimes : int = 0;
		myTimes		=	FindTimes;
		while(	(agent || agent.enabled == true) && myTimes == FindTimes	)
		{
			yield StartCoroutine("UpdateAnimationBlend");
			yield;
			yield;
		}
	}

	private var walkAnimationSpeed	:	float = 5.0;
	private var runAnimationSpeed	:	float = 10.0;
	private	var cc	:	boolean	=	true;
	//切换动作//
	private	function	UpdateAnimationBlend()
	{
		if(	cc	)
		{
			cc	=	false;
			var	velocityXZ : Vector3 = Vector3(agent.velocity.x, 0.0f , agent.velocity.z);
			var speed : float = velocityXZ.magnitude;
			if(	speed > walkAnimationSpeed + 1 )
			{
				Animatc.anim_.CrossFade("run");
			    Animatc.anim_["run"].speed = speed / runAnimationSpeed;
			}
			else if(speed > 0.1) 
			{
				Animatc.anim_.CrossFade("walk");
				Animatc.anim_["walk"].speed	=	speed / walkAnimationSpeed ;		
			}
			else
			{    if(MAI.CurrentAIStatus == E_AIStatus.Battle ||MAI.CurrentAIStatus == E_AIStatus.Attack )
			     Animatc.anim_.CrossFade("battle");
			     else
				 Animatc.anim_.CrossFade("idle");
			}
			yield;
			yield;
			cc = true;
		}
	}

}