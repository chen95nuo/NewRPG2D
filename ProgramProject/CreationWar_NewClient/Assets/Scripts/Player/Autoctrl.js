#pragma strict
static	var	Wayfinding	=	false;
private var aa	=	true;
private var agent : NavMeshAgent;
private var agentmotion : agentLocomotion;
private var Controller :ThirdPersonController;
private var TAnimation :ThirdPersonPlayerAnimation;
function	Start ()
{
	agent		= GetComponent(NavMeshAgent);	
	agentmotion	= GetComponent(agentLocomotion);
	Controller	= GetComponent(ThirdPersonController);
	TAnimation	= GetComponent(ThirdPersonPlayerAnimation);
	agent.enabled	= false;
    agentmotion.enabled	= false;
    AllManage.tsStatic.closeAutoMove();
	yield	WaitForSeconds(1);
	aa	=	false;
}
//showAutoMove
private var copyPos : Vector3;
function	Update ()
{
	if(	Wayfinding	&&	aa	)
	{
		//Controller.enabled = false;
		TAnimation.enabled	=	false;
		agent.enabled		=	true;
		agentmotion.enabled	=	true;
		if(	!PlayerAI.AutoAI	)
    		AllManage.tsStatic.showAutoMove();
		aa					=	false;
	}
	if(	!Wayfinding	&&	!aa	)
	{
		agent.enabled		=	false;
		agentmotion.enabled	=	false;
		AllManage.tsStatic.closeAutoMove();
		//Controller.enabled = true;
		TAnimation.enabled	=	true;
		aa					=	true;
	}
	if(	bool && Time.time > ptime	)
	{
		ptime	=	Time.time + 0.5;
		try
		{
			copyPos		=	transform.position;
			copyPos.y	=	pos.y;
			if(	Vector3.Distance(copyPos , pos) < 3	)
			{
				bool	=	false;
//				AllManage.tsStatic.Show("tips062");	

				agent.enabled		=	false;
				agentmotion.enabled =	false;
				AllManage.UICLStatic.OneNPCTalkNoClick();
				AllManage.tsStatic.closeAutoMove();
//				Controller.enabled	=	true;
				TAnimation.enabled	=	true;
				aa					=	true;
				Wayfinding			=	false;
			}
		}
		catch(e){}
	}
}

var ptime : float;
function	DaoDa()
{
}

var bool : boolean = false;
var pos : Vector3 = Vector3(0,0,0);
function	UdateAgentTargets(targetPosition : Vector3) 
{
	if(	(	targetPosition-transform.position	).sqrMagnitude	<	0.00001f	|| alljoy.h != 0 || alljoy.v != 0)
	{
		//KDebug.Log( "原地寻路",transform,Color.red );
		return;
	}
	bool = false;
	pos = targetPosition;
	Wayfinding = true;
	//if(!ts)
	//	ts = FindObjectOfType(TiShi);
	//AllManage.tsStatic.Show("tips063");	
	SendMessage(	"DaDuan",	SendMessageOptions.DontRequireReceiver	);
	yield; 
	if(	agent	)
	{
		if(AllManage.psStatic.ridemod){
			Controller.Movespeed = AllResources.ar.rideSpeedMove[AllManage.psStatic.rideQuality];
		}else{
			Controller.Movespeed = 1;
		}
		
		Controller.Buffspeed = 1;
		agent.speed = Controller.Movespeed	* AllManage.pAIStatic.GetStarspeed() *  Controller.Buffspeed;
		agent.enabled		=	true;
		agentmotion.enabled	=	true;
		if(	!PlayerAI.AutoAI	)
			AllManage.tsStatic.showAutoMove();
	    agent.SetDestination(targetPosition);
	    agentmotion.Findway(); 
//		agent.destination	=	targetPosition;
	}
//	yield;
//	if(	agent	)
//	{
//	    agent.SetDestination(targetPosition);
////	    agent.destination	=	targetPosition;
//	}
	bool	=	true;
}

function	UdateAgentTargetsNoTs(	targetPosition	:	Vector3	)
{
	if(	(	targetPosition-transform.position	).sqrMagnitude	<	0.00001f	)
	{
		//KDebug.Log( "原地寻路",transform,Color.red );
		return;
	}
	Wayfinding = true;
	yield; 
	if(	agent	)
	{
		if(AllManage.psStatic.ridemod){
			Controller.Movespeed = AllResources.ar.rideSpeedMove[AllManage.psStatic.rideQuality];
		}else{
			Controller.Movespeed = 1;
		}
				Controller.Buffspeed = 1;
				agent.speed = Controller.Movespeed	* AllManage.pAIStatic.GetStarspeed() *  Controller.Buffspeed;
		agent.enabled = true;
		agentmotion.enabled = true;
		if(	!PlayerAI.AutoAI	)
			AllManage.tsStatic.showAutoMove();
		agent.SetDestination(targetPosition);
		agentmotion.Findway(); 
//		agent.destination	=	targetPosition;
	}
	//if(!ts)
	//	ts = FindObjectOfType(TiShi);
	//AllManage.tsStatic.Show("tips063");	
//	yield;
//	if(	agent	)
//	{
//		agent.SetDestination(targetPosition);
////		agent.destination = targetPosition;
//	}
}
