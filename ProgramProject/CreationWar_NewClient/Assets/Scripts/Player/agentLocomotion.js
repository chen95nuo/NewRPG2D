#pragma strict
@script	RequireComponent(	NavMeshAgent	)

	private var agent_ 	:	NavMeshAgent;
	private var Animatc	:	ThirdAnimation; 
	private var Status	:	PlayerStatus;
	function	Start() 
	{
		agent_	=	GetComponent.<	NavMeshAgent	>();
		Animatc	=	GetComponent(	ThirdAnimation	);
		Status	=	GetComponent(	PlayerStatus	);
		Animatc.RideAnimation(	"idle"	);
		//agent_.baseOffset	=	-0.1;
	}

	var FindTimes : int = 0;
	///进行寻路时的动作切换//
	function	Findway()
	{	
		if(	agent_	){
			FindTimes += 1;
		}else{
			agent_	=	GetComponent.<	NavMeshAgent	>();
		}
		var myTimes : int = 0;
		myTimes	=	FindTimes;
		while(	agent_.enabled	==	true	&&	myTimes	==	FindTimes	)
		{
			yield	StartCoroutine(	"UpdateAnimationBlend"	);
		}
	}

	private var walkAnimationSpeed : float = 5.0;
	private var runAnimationSpeed : float = 8.0;
	private var velocityXZ : Vector3;
	private var speed : float;
	private var aa	=	true;
	//更新动作融合//
	private	function	UpdateAnimationBlend()
	{
		if(	aa	)
		{
			aa	=	false;
			velocityXZ = Vector3(agent_.velocity.x, 0.0f , agent_.velocity.z);
			speed = velocityXZ.magnitude;
			if(	speed > walkAnimationSpeed + 2 )
			{
				Animatc.RideAnimation("run");
				Animatc.AniSpeedScale = speed / runAnimationSpeed;	   
			}
			else 
			if(	speed > 0.1	) 
			{
				Animatc.RideAnimation("walk");
				Animatc.AniSpeedScale = speed / walkAnimationSpeed;	   
			}
			else
			{
				if(	Status.battlemod	)
					Animatc.RideAnimation("battle");
				else
					Animatc.RideAnimation("idle");
				Animatc.AniSpeedScale =1;	   
			}
			yield;
			yield;
			aa	=	true;
		}
		yield;	
	}
