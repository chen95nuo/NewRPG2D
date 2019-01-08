#pragma strict

var	agent	:	NavMeshAgent;
private var Status	:	PlayerStatus;
private var AcSkill	:	ActiveSkill;
private var opinfo	:	OtherPlayerInfo;
var targetm	: Transform;
var targetp : Transform;
var Enemytag	=	"Enemy";
private var	delayAttackTime	=	1.5;
private var	remoteRange		=	20;
private var lastatktime		=	0.0;
private var	lastsktime		=	0.0;
private var skilltime		=	0.0;
private var	skillCD			=	8;
private var	offset : Vector3;
var	Canremote	=	false;
private var AttackLength	=	3;
private var Starspeed		=	0.0;
private var TController : ThirdPersonController;
function	Start() 
{
	TController	= GetComponent(ThirdPersonController);
	AcSkill		= GetComponent(ActiveSkill);
	opinfo		= GetComponent(OtherPlayerInfo);
	agent		= GetComponent.<NavMeshAgent>();
	Starspeed	= agent.speed;
	Status		= GetComponent(PlayerStatus);
	yield;
	yield;
	if(	this.CompareTag ("Enemy")	)
		Enemytag = "Player";
	switch (	Status.ProID	)
	{ 
		case 1: 
			Canremote = false;
			if(	Status.weaponType==1	)
				AttackLength = 4;
			break; 
	  
		case 2:  									
			if(	Status.weaponType==2	)
				Canremote = true;
			else
				Canremote = false;
			break; 
	  
		case 3:
			Canremote = true;
			break; 
	}
	while (	!Status.dead	) 
	{
		if(	parseInt(Status.Maxhealth)*0.3 > parseInt(Status.Health)	)
		{
			UseXueping();
		}
		if (	CanSeeTarget()	==	2	)
		{
			yield	StartCoroutine("Attack");
		}
		else if(	CanSeeTarget()	==	1	)
		{
			yield	StartCoroutine("FindTarget");
		}
		else 
			yield	Idle();
	}

}

//-------------------------------------------------------------------注册机器人的本地唯一实例ID-------------------------------------------------------------------//
private	var	MyRobotInstanceID		:	int;
function	SetNewRobotInstanceID( InstID:int	)
{
	Status	=	GetComponent(PlayerStatus);
	Status.instanceID	=	InstID;
}
function	OnEnable ()
{
	MyRobotInstanceID	=	PlayerUtil.GetNewLocalInstanceID();
	SetNewRobotInstanceID( MyRobotInstanceID );
	PlayerUtil.RegisterNewLocalObject( MyRobotInstanceID );
}
function	OnDisable ()
{
	PlayerUtil.UnregisterLocalObject( MyRobotInstanceID );
}
//-------------------------------------------------------------------注册机器人的本地唯一实例ID-------------------------------------------------------------------//

function	CanSeeTarget () : int 
{
	if(Application.loadedLevelName == "Map321" && AllManage.areCLStatic && AllManage.areCLStatic.isJueDouWin){
		return 0;	
	}

	targetp = FindClosestEnemy();
	if(!targetp)
		return 0;	
	else
	{
		var dis = Vector3.Distance(transform.position, targetp.position);    
		if (dis > 32)
			return 0;	
		else if (dis > 26)
			return 1;	
		else
		{	  
			Status.battlemod=true;
			Status.battletime =Time.time;
			return 2;
		}
	}
}

function	SetMaster(ttaget: Transform)
{
	targetm	=	ttaget;
} 

   
private	var	dd	=	true;
function	Idle ()
{
	while (CanSeeTarget()==0&&!Status.dead)
	{	
		if(dd)
		{
			dd =false;		
			if(!targetm)
			{
				agent.speed	=	TController.Movespeed*Starspeed*0.3;
				yield	StartCoroutine("Timer",Random.Range(1,5));
			}
			else
			{ 
				agent.speed	=	TController.Movespeed*Starspeed;  
				yield	StartCoroutine("Timer",1);          
				UpateAgentTargets(targetm.position + targetm.right+targetm.right);
			}   
			dd =true;
		}
		yield;
	}
}

private	function	Timer (ttime: float)
{ 
	while (ttime>=0 && CanSeeTarget ()==0&&!Status.dead) 
	{  
			if(	TController.yun||TController.bing||Status.dead || TController.down)
			{
				agent.Stop();
				return;
			}
		yield WaitForSeconds(0.2);  
		ttime -= 0.2;         
	}
	return;
}

function	FindTarget()
{
			if(	TController.yun||TController.bing||Status.dead || TController.down)
			{
				agent.Stop();
				return;
			}
	var en : Transform = FindClosestEnemy();
	if(	en	)
		UpateAgentTargets(en.position);
	yield	WaitForSeconds(0.5); 
}

function	UseXueping()
{
//xuepinbutton.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
}

private var snum :int =1;
private var angle = 180.0;
private var skillNum : int = 0;
function	Attack()
{
	if(	TController.yun||TController.bing||Status.dead || TController.down)
	{
		agent.Stop();
		return;
	}
	agent.speed=TController.Movespeed*Starspeed*TController.Buffspeed; 
	if(!targetp)
		return;
	//UpateAgentTargets(targetp.position); 
	offset = transform.position - targetp.position;   
 	if((Canremote && offset.magnitude < remoteRange )||(!Canremote && offset.magnitude < 7.0))
 	{ 
		agent.Stop();
		angle = 180.0;
		while (angle > 5 &&!TController.yun &&!TController.bing &&!Status.dead && targetp)
		{     
			angle = Mathf.Abs(RotateTowardsPosition(targetp.position,200));
			if(	TController.yun||TController.bing||Status.dead || TController.down)
			{
				agent.Stop();
				return;
			}
			yield;
		}

		if(Time.time > 3 + lastsktime &&  ((Canremote && offset.magnitude < remoteRange)||(!Canremote && offset.magnitude < 7.0)))
			{
				if(	TController.yun||TController.bing||Status.dead || TController.down)
				{
					agent.Stop();
					return;
				}
				skillNum += 1;
				if(skillNum > 3){
					skillNum = 0;
				}
				lastsktime = Time.time ;
				if(opinfo.SkillPositions[skillNum] !=""){
					var skillid = parseInt(opinfo.SkillPositions[skillNum])-1;
					AcSkill.Skill(skillid);
				}
			}
			
		if(	Time.time > delayAttackTime*Status.Attackspeed + lastatktime )
			yield	StartCoroutine("Attackn");
	}
 	else
	{
		UpateAgentTargets(targetp.position); 
	}

	if(!targetp)
	{
		agent.Resume();
		return;
	}  
	return;
}

private	var	ht	=	-1;
private	var	hn	=	0;

function	Attackn()
{
	if(	hn	<	AttackLength && Time.time	<	ht	+	2	)
		hn	+=	1;	   
	else
		hn	=	1;
//	print(TController.yun + " ==== " + TController.bing + " =====  " +  TController.down);
			if(	TController.yun||TController.bing||Status.dead || TController.down)
			{
				agent.Stop();
				return;
			}
	SendMessage("DidPunch",hn, SendMessageOptions.DontRequireReceiver);
	if(Status.ProID==3)
	punchTime = 0.8;
	yield	WaitForSeconds(Random.Range(punchTime,punchTime+0.5));
	ht	=	Time.time;
}
private var punchTime = 0.5;

private function RotateTowardsPosition (targetPos : Vector3, rotateSpeed : float) : float
{
	var relative = transform.InverseTransformPoint(targetPos);
	var angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
	var maxRotation = rotateSpeed * Time.deltaTime;
	var clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);
	transform.Rotate(0, clampedAngle, 0);
	return angle;
}

private function FindClosestEnemy () : Transform
 {
   var gos : GameObject[];
  gos = GameObject.FindGameObjectsWithTag(Enemytag);
  var closest : GameObject;
  var distance = Mathf.Infinity;
for (var go : GameObject in gos) {
    var diff = (go.transform.position - transform.position);
    var curDistance = diff.sqrMagnitude;
    if (curDistance < distance) {
       closest = go;
       distance = curDistance;
       }
    }
 if(closest)
return closest.transform;
else
return null;
}

function UpateAgentTargets(targetPosition : Vector3) {
			if(	TController.yun||TController.bing||Status.dead || TController.down)
			{
				agent.Stop();
				return;
			}
 agent.speed=TController.Movespeed*Starspeed*TController.Buffspeed;
        yield; 
        if(agent){
            agent.enabled = true;
	        agent.Resume();
			agent.destination = targetPosition;
        }	
        yield;
        if(agent){
            agent.Resume();
	        agent.destination = targetPosition;
	        SendMessage("Findway", SendMessageOptions.DontRequireReceiver);
	    }
}
