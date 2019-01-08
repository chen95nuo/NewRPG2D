#pragma strict
var walkSpeed = 4.0;
var runSpeed = 10.0;

var inAirControlAcceleration = 2.0;
var gravity =10.0;
var speedSmoothing = 10.0;
var rotateSpeed = 500.0;

private var trotAfterSeconds = 0;
private var jumpRepeatTime = 1.0;
private var groundedTimeout = 0.25;

// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
private var lockCameraTimer = 0.0;

// The current move direction in x-z
private var moveDirection = Vector3.zero;
private var verticalSpeed = 0.0;
private var moveSpeed = 0.0;

// The last collision flags returned from controller.Move
private var collisionFlags : CollisionFlags; 


private var jumping = false;
private var controller : CharacterController;
private var isMoving = false;
private var walkTimeStart = 0.0;
private var lastJumpButtonTime = -10.0;
private var lastJumpTime = -1.0;
private var inAirVelocity = Vector3.zero;
private var lastGroundedTime = 0.0;
private var dun=false;

 var isControllable = true;
 var bing	=false;
 var yun	=false;
 var down	=false;
 var Movespeed	=1.0;
 var Buffspeed	=1.0;
 
private var photonView : PhotonView;
private var Status : PlayerStatus;
private var animationController : ThirdAnimation;

private var lastsynTime = -1.0;

function	Awake ()
{  	
	photonView		=	GetComponent(PhotonView);
    agent_			=	GetComponent.<NavMeshAgent>();
    Status			=	GetComponent(PlayerStatus);
    controller		=	GetComponent(CharacterController);
	moveDirection	=	transform.TransformDirection(Vector3.forward);
	animationController = GetComponent(ThirdAnimation);
	playerBuff = GetComponent(PlayerApplyBuff);
}

function GetsetTime(){
	var setTime = 0.1;
	switch(	UIControl.mapType	)
	{
		case MapType.zhucheng:
			setTime = BtnGameManager.numSeviceMianCity;
			break;
		case MapType.jingjichang:
			setTime = BtnGameManager.numSevicePVP;
			break;
		case MapType.yewai:
			setTime = BtnGameManager.numSeviceDuplicate;
			break;
		case MapType.fuben:
			setTime = BtnGameManager.numSeviceDuplicate;
			break;
	}
	return setTime;
}

function	Start()
{
	isControllable = true;
	Movespeed=1.0;
	Buffspeed =1.0;
	if(	PlayerUtil.isMine(	GetComponent(	PlayerStatus	).instanceID	)	)
		cameraTransform	=	Camera.main.transform;
	
	GameReonline.playerPostion = transform.position;
}

var PlayerCC : PlayerCollisionControl;
private var cameraTransform	:	Transform;
private var grounded	=	true;
private var forward	:	Vector3;
private var right	:	Vector3;
private var targetDirection	:	Vector3;
function	UpdateSmoothedMovementDirection ()
{
	if(	Camera.main == null	)
	{
		return;
	}
	// Forward vector relative to the camera along the x-z plane	
	if(	cameraTransform	==	null	)
		cameraTransform	=	Camera.main.transform;
	forward		=	cameraTransform.TransformDirection(Vector3.forward);
	forward.y	=	0;
	forward		=	forward.normalized;
	right		=	Vector3(forward.z, 0, -forward.x);	
	var	wasMoving	=	isMoving;
	isMoving		=	Mathf.Abs (alljoy.h) > 0.1 || Mathf.Abs (alljoy.v) > 0.1;	
	targetDirection = alljoy.h * right + alljoy.v * forward;
	if(	grounded	)
	{
		// Lock camera for short period when transitioning moving & standing still
		lockCameraTimer += Time.deltaTime;
		if(	isMoving != wasMoving	)
			lockCameraTimer = 0.0;
		if(	targetDirection != Vector3.zero )
		{   
			PlayerCC.DaDuan();
			if(	moveSpeed < walkSpeed * 0.9 && grounded)
			{
				moveDirection = targetDirection.normalized;
			}
			else
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);			
				moveDirection = moveDirection.normalized;
			}
		}
		// Smooth the speed based on the current target direction
		var curSmooth = speedSmoothing * Time.deltaTime;		
		var targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0);	
		if(	Time.time - trotAfterSeconds > walkTimeStart	)
		{   
			if(	Movespeed<=1.0)
				targetSpeed *= runSpeed;
			else
				targetSpeed *= runSpeed*Movespeed;
		}
		else
		{   
			targetSpeed *= walkSpeed;			
		}
		moveSpeed = Mathf.Lerp(	moveSpeed, targetSpeed, curSmooth	);
		if(	moveSpeed < walkSpeed * 0.3	)
			walkTimeStart = Time.time;
	}
	else
	{	// Lock camera while in air
		if(	jumping		)
			lockCameraTimer = 0.0;
		if(	isMoving	)
			inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
	}
		
}

function	ApplyGravity ()
{   	
	if(	jumping	)
		verticalSpeed	-=	gravity	*	12	*	Time.deltaTime;
	else
		verticalSpeed	=	-9.8;	
}

function	SyncDidJump(strs : String[])
{
	if(	strs.length >= 5	)
	{
		var	intJumps	:	int[];
		intJumps	=	new	int[strs.length];
		for(var i=0; i<intJumps.length; i++)
		{
			intJumps[i]	=	parseInt(strs[i]);
		}
		DidJump(	intJumps	);
	}
}

//yuanchengtiaoyue
function	DidJump (Syns : int[])
{
	remoteIsMove = true;
	if(	!PlayerUtil.isMine(	GetComponent(PlayerStatus).instanceID	)	)
	{ 
			Remontemovement = newRemontemovement;
		var move : Vector3;
		usefloat = Syns[0];
		move.x = usefloat / 100.0;
		
		usefloat = Syns[1];
		move.y = usefloat / 100.0;
		
		usefloat = Syns[2];
		move.z = usefloat / 100.0;
		
		
		usefloat = Syns[3];
		Remontepos.x = usefloat / 100.0;
		
		usefloat = Syns[4];
		Remontepos.y = usefloat / 100.0+1;
		
		usefloat = Syns[5];
		Remontepos.z = usefloat / 100.0;
      newRemontemovement = move;  
	}
    dun	=	true;
	jumping	=	true;      //远程跳跃
	animationController.RideAnimation("jumpstart");
	yield	WaitForSeconds(0.25); 
	dun	=	false;
	verticalSpeed	=	45;
	lastJumpTime	=	Time.time;
	lastJumpButtonTime	=	-10;
	animationController.RideAnimation("jump");
}

function	DidLand()
{
	jumping	=	false;
}

private	var	relativePos	=	Vector3.zero;

private var sendSyns : int[] = new int[7];
var useMovement : Vector3;
private var jumpMovement : Vector3;
function	Update()
{
	grounded	=	IsGrounded();
	if(	PlayerUtil.isMine(	Status.instanceID	) && Status.isMine	)
	{
		if (!isControllable)
		{
			Input.ResetInputAxes();
			return;
		}

		alljoy.v	=	alljoy.v*Movespeed*Buffspeed;
		alljoy.h	=	alljoy.h*Movespeed*Buffspeed;
		
		if ( alljoy.jumpButton )
		{
			lastJumpButtonTime = Time.time;
			if(	PlayerStatus.MainCharacter == transform	)
				PlayerCC.DaDuan();
			alljoy.jumpButton = false;
		}
		UpdateSmoothedMovementDirection();		//平滑转摄像机//
		movement = moveDirection * moveSpeed + inAirVelocity;
		if(	lastJumpTime + jumpRepeatTime <= Time.time && IsGrounded()&& !jumping && Time.time < lastJumpButtonTime + 0.3 )
		{		
	 		sendSyns[0] = movement.x * 100;
	 		sendSyns[1] = movement.y * 100;
	 		sendSyns[2] = movement.z * 100;
	 		sendSyns[3] = transform.position.x * 100;
	 		sendSyns[4] = transform.position.y * 100;
	 		sendSyns[5] = transform.position.z * 100;
			DidJump(sendSyns);
			if( ServerRequest.isAddToMap ){		
				ServerRequest.syncAct(PlayerUtil.myID , CommonDefine.ACT_JUMP, 
				String.Format("{0};{1};{2};{3};{4};{5}", sendSyns[0] , sendSyns[1] , sendSyns[2] , sendSyns[3] , sendSyns[4] , sendSyns[5] ));
			}
			lastsynTime = Time.time+0.7;
			dun=true;
			jumping = true;	  //本地跳跃
			animationController.RideAnimation("jumpstart");	
		}			
	}
	ApplyGravity ();
	if(	PlayerUtil.isMine(	Status.instanceID	)	)
	{
			//处理导航
			if(	agent_.enabled == true	)
				movement = agent_.velocity;	
		//处理完movement并且传递。
			if(	Time.time > lastsynTime + GetsetTime() && (
				Vector3.Distance(lastposition , transform.position) > 0.1 ||
				Vector3Contrast(lastmovent , movement) ))
			{
				//(Vector3Contrast(lastposition , transform.position))){// || Vector3Contrast(lastmovent , movement))){
//				print(Vector3Contrast(lastposition , transform.position) + " --- " + lastposition + " ===== " + transform.position);
				lastposition = transform.position;
				lastmovent = movement;
				lastsynTime = Time.time;
			if( ServerRequest.isAddToMap ){
				ServerRequest.requestMove(	Vector3(movement.x , movement.y , movement.z), 
										Vector3(transform.position.x , transform.position.y , transform.position.z));
			}
			
			}
			GameReonline.playerPostion = transform.position;

	}
	// 处理移动
	if(	PlayerUtil.isMine(	Status.instanceID	) && Status.isMine	)
	{
		if(	isControllable	&&	!yun	&&	!bing	&&	!down	)
		{
			movement += Vector3 (0, verticalSpeed, 0);
			movement *= Time.deltaTime;
			if(	agent_.enabled	==	false	)
				collisionFlags	=	controller.Move(	movement	);
		}
		//处理转向
		if (IsGrounded())
		{
			if(agent_.enabled == false && qiuai.objs &&( alljoy.attackButton||alljoy.skillButton))
			{
			   relativePos = qiuai.objs.position - transform.position;
			   relativePos.y=0;		
			   moveDirection = relativePos*0.001;
			}
			if(agent_.enabled == true)
			{
			    tempmoveDirection = agent_.velocity;
			    tempmoveDirection.y=0;
			    moveDirection = Vector3.Lerp(moveDirection, tempmoveDirection, Time.deltaTime * 4);
	        }
	        if(PlayerAI.AutoAI == false && moveDirection != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(moveDirection);
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
		}	
		else
		{    
			if(	PlayerAI.AutoAI == false	)
			{
				var xzMove = movement;
				xzMove.y = 0;
				if(	xzMove.sqrMagnitude > 0.001)
				{
					transform.rotation = Quaternion.LookRotation(xzMove);
				}
			}
		}
	}
	else
	{   if (IsGrounded())
	       lastGroundedTime = Time.time;
		if(Status.isMine && !playerBuff.busy && remoteIsMove)
		{			
				moveToPosition(Remontepos);	
		}
	}
	//处理落地
	if(	IsGrounded() && jumping && !dun )
		SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
		
	if(animationController == null){
		animationController = GetComponent(ThirdAnimation);
	}
//	if(yun)
//		animationController.RideAnimation("stun");
	if(Status.isMine){
	if(PlayerUtil.isMine(	Status.instanceID	)){
	   if (jumping)
		animationController.RideAnimation("jump");	
	   else if ( !IsGroundedWithTimeout())
		animationController.RideAnimation("jump");	
		}
	 else{
	 	if (jumping)
		animationController.RideAnimation("jump");	
		}
	}
}

function GetanimationController() : ThirdAnimation{
	if(animationController == null){
		animationController = GetComponent(ThirdAnimation);
	}
	return animationController;
}

function moveToPosition(position :Vector3)
{
	
	if(!jumping )
	transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 0.3/GetsetTime());
	Remontemovement	= Vector3.Lerp(Remontemovement, newRemontemovement, Time.deltaTime * 0.3/GetsetTime());		
    useMovement = Remontemovement;
	useMovement += Vector3 (0, verticalSpeed, 0);
	useMovement *= Time.deltaTime;
	collisionFlags	= controller.Move(useMovement);
	RemontexzMove	=	useMovement;
	RemontexzMove.y	= 0;
	if(	RemontexzMove.sqrMagnitude > 0.0001){
		transform.rotation = Quaternion.LookRotation(RemontexzMove);
		if(yun)
			animationController.RideAnimation("stun");
		else if(RemontexzMove.sqrMagnitude > 0.01)	
		animationController.RideAnimation("run");	
		else if(RemontexzMove.sqrMagnitude > 0.001)	
		animationController.RideAnimation("walk");
		else
		animationController.RideAnimation("idle");	
		}
		else{
		if(yun)
			animationController.RideAnimation("stun");
		else if(Status.battlemod==true)
		    animationController.RideAnimation("battle");
		else
		    animationController.RideAnimation("idle");
		
			}	
	//同步坐标与替身实际坐标距离小于2时，关闭同步开关
	if(Vector3.Distance(position , transform.position) < 1 && !jumping)
	{
		remoteIsMove = false;
		//todo 人物停止移动二种情况 休闲，战斗准备
		if(yun)
			animationController.RideAnimation("stun");
		else if(Status.battlemod==true)
		    animationController.RideAnimation("battle");
		else
		    animationController.RideAnimation("idle");
	}
}

function	Vector3Contrast(vecA : Vector3 , vecB : Vector3)
{
	if(	parseInt(vecA.x * 10) != parseInt(vecB.x * 10) || parseInt(vecA.y * 10) != parseInt(vecB.y * 10) || parseInt(vecA.z * 10) != parseInt(vecB.z * 10))
	{
		return true;
	}
	return false;
}
private var RemontexzMove:Vector3= Vector3.zero;
private var movement:Vector3= Vector3.zero;
private var Remontemovement = Vector3.zero;
private var newRemontemovement = Vector3.zero;
private var tempmoveDirection = Vector3.zero;
private var lastposition:Vector3= Vector3.zero;
private var lastmovent:Vector3= Vector3.zero;
private var agent_ : NavMeshAgent;
var Remontepos : Vector3;
private var usefloat : float;
var remoteIsMove: boolean = false;
//@RPC
private var playerBuff : PlayerApplyBuff;
function	SynMovement(Syns : int[])
{
//		print(Syns + " ======== ");
	if(!playerBuff.busy){
		
		remoteIsMove = true;
		Remontemovement = newRemontemovement;
		var move	: Vector3;

		usefloat = Syns[0];
		move.x = usefloat;
			
		usefloat = Syns[1];
		move.y = usefloat ;
			
		usefloat = Syns[2];
		move.z = usefloat;
			
		usefloat = Syns[3];
		Remontepos.x = usefloat;
			
		usefloat = Syns[4] / 100 + 1;
		Remontepos.y = usefloat;
			
		usefloat = Syns[5];
		Remontepos.z = usefloat;
		newRemontemovement = move;  
	}
}


function	GetSpeed()
{
	return	moveSpeed;
}

function	IsJumping ()
{
	return	jumping ;
}

function	IsGrounded ()
{
	return	(collisionFlags & CollisionFlags.CollidedBelow) != 0;
}

function	GetDirection() 
{
	return	moveDirection;
}

function	IsGroundedWithTimeout()
{
	return	lastGroundedTime + groundedTimeout > Time.time;
}

function SetRemontepos(pos : Vector3){
	Remontepos = pos;
}

@script RequireComponent(CharacterController)
