/// <summary>
/// 普通攻击控制、逻辑、伤害计算、同步 -
/// </summary>
#pragma strict
@System.Serializable
enum	fxType
{	//	附带特效	-
	Non		=0,		//无	-
	Jitui	=1,		//击退-
	Jifei	=2,		//击飞-
	Jidao	=3,		//击倒-
	Slow	=4		//减速-
}
/// <summary>
/// 攻击相关信息 -
/// </summary>
class	ComboAttack 
{
	var animation	:	AnimationClip;
	var impact		:	AudioClip;
	var HitPoint	=	1.0;
	var qianjin		=	0;
	var fxobject	:	GameObject;		//发射的子弹，为空时，攻击为近战攻击//
	var fxtype		:	fxType;			//附带效果类型//
	var EffectID	:	int;			//攻击特效ID//
}
/// <summary>
/// 普通攻击时，是否单杀，false群杀 -
/// </summary>
private var	Single	=	false;
var leftSwipe	:	MeleeWeaponTrail;
var rightSwipe	:	MeleeWeaponTrail;
//三种武器的相关战斗数据//
var CAttack0	:	ComboAttack[];	
var CAttack1	:	ComboAttack[];
var CAttack2	:	ComboAttack[];

private var CAttack			:	ComboAttack[];
private var damage			:	int;
private var Damagetype		:	Damagestype;	//0为默认物理伤害，1~4为魔法，1是技能（奥术），2是冰，3是火，4是暗影，5是毒（自然）//
private var Hatred			=	1;				//仇恨值//
private var punchHitTime	=	0.4;
private var punchPosition	:	Vector3;
private var punchRadius		=	4;
private var Playrobot		:	PlayerRobot;

//---平砍---
private var hn			=	0;		//攻击连招阶段记录//
private var ht			=	-1;		//攻击行为时间记录//
private var hittime		=	0.0;	//“攻击操作间隔0.2s后允许下次操作”的行为时间记录//
//---平砍---

private var busy		=	false;	//公共冷却标签机//

private var move = Vector3.zero;
private var photonView : PhotonView;
private var controller : CharacterController;
private var playerController : ThirdPersonController; 
private var actv : ActiveSkill;
private var Status : PlayerStatus;
private var Enemytag = "Enemy";
private var cc : GameObject;		//创建发射物的临时变量，减少GC//
private var cameraTransform : Transform;
private var controllerhigh = 2.0;
private var controllery = Vector3.zero;
function	Awake () 
{
	Playrobot			= GetComponent(PlayerRobot);
   	photonView			= GetComponent(PhotonView);
	controller			= GetComponent(CharacterController);	
	playerController	= GetComponent(ThirdPersonController);
	actv                = GetComponent(ActiveSkill);
	Status				= GetComponent(PlayerStatus);
	Damagetype			= Damagestype.PhysicalSharps;
    CAttack				= CAttack0;
    controllerhigh = controller.height;
    controllery = controller.center ;
}
function	Start()
{
	if(	Status.ProID	==	1	)	//战士仇恨2倍//
		Hatred	=	2;	
	cameraTransform	=	Camera.main.transform;
}

private var wipe =false;

/// <summary>
/// 更换不同武器攻击时带来的效果 -
/// </summary>
@RPC
function	ChangeWeapon(	i	:	int	)
{
	if(	i	==	0	)
	{
		CAttack	= CAttack0;
		Single	= true;
		wipe	= false;
		Damagetype	= 1;
		punchHitTime = 0.2;	
	}
	else if(	i	==	1	)
	{
		CAttack = CAttack1;
		punchHitTime = 0.1;
		Single = false;
		wipe = true;
		Damagetype = 0;
		if(Status.ProID ==1)
		{
			Hatred = 5;
		}
		else if(Status.ProID ==3)
		{
			punchHitTime	= 0.4;
			Damagetype		= 3;
			wipe = false;
		}
	}
	else
	{
		CAttack = CAttack2;
		punchHitTime = 0.3;
		Single = false;
		wipe = true;
		Damagetype = 0;
		if(Status.ProID ==2)
		{
			Damagetype = 1;
			Hatred = 0.8;
			wipe = false;
		}
		else if(Status.ProID ==3)
		{
			punchHitTime = 0.4;
			Damagetype = 5;
			wipe = false;
		}
	}
}

function	Update ()
{  
	//	连招设定 	-
		//------------------------------------平砍------------------------------------//
	if(	UIControl.mapType != MapType.zhucheng &&!Status.dead && Status.isMine && playerController.isControllable && !busy && hittime+0.2<Time.time && alljoy.attackButton )
	{	//	不在主城		&&	没死		&&	更新个人		&&	可控		&&	不在公共冷却		&&	没有受击		&&	点了攻击		//
		Status.ridemod	=	false;
		busy	=	true;
		if(	playerController.IsJumping()	)
	  		hn = 0;
	  	else
	  	{
			if(	hn<CAttack.Length-1 && Time.time<ht+2	)
				hn	+=	1;	   
			else
 				hn	=	1;
		}
		DidPunch(hn);	    	
		hittime	=	Time.time;
		ht		=	Time.time;
	}	//------------------------------------平砍------------------------------------//
	if(	!Status.dead && Status.isMine && playerController.isControllable &&(alljoy.rollButton ||	Input.GetKeyDown (KeyCode.U)))
	{
		ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_roll, "");
		actroll();
		Status.ridemod=false;
		actv.busy = true;
		rolldis = 8.0;
		alljoy.rollButton = false;
		timeout();
	}
  	if(	!Status.dead && Status.isMine && playerController.isControllable &&(alljoy.rideButton ||	Input.GetKeyDown (KeyCode.R)))
  	{
  		if(	Status.ridemod	==	false	)
  		{
  			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_RideOn, String.Format("{0};{1}", (Status.rideLevel - 1).ToString() , Status.rideMap) );
  			
			photonView.RPC("rideOn",Status.rideLevel - 1 , Status.rideMap);
		}
		else
		{
			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_RideOff, "");
			
			SendMessage("rideOff" , SendMessageOptions.DontRequireReceiver);
//			photonView.RPC("rideOff",PhotonTargets.All);
		}
		alljoy.rideButton	=	false;
		SendMessage(	"InitlastAnimation",	SendMessageOptions.DontRequireReceiver	);
	}

}
/// <summary>
/// 执行技能的公共冷却		-
/// </summary>
function	timeout()
{
	yield	WaitForSeconds(0.5);
	actv.busy	=	false;
}

function actroll(){
Status.rollmod = true;
controller.center = Vector3(0,0.5,0);
controller.height = 1.0;
		SendMessage("PlayEffect",162 , SendMessageOptions.DontRequireReceiver);
		SendMessage("PlayAnimat", "roll" , SendMessageOptions.DontRequireReceiver);
yield	WaitForSeconds(1);
Status.rollmod = false;
controller.center = controllery;
controller.height = controllerhigh;
}

function ReturnRoll(){
Status.rollmod = true;
controller.center = Vector3(0,0.5,0);
controller.height = 1.0;
		SendMessage("PlayEffect",162 , SendMessageOptions.DontRequireReceiver);
		SendMessage("PlayAnimat", "roll" , SendMessageOptions.DontRequireReceiver);
yield	WaitForSeconds(1);
Status.rollmod = false;
controller.center = controllery;
controller.height = controllerhigh;
}

var responseTargetp : Vector3;
var responseInstanceID : int = 0;
function ResponsePunch(objs : Object[]){
	var skillType : int;
	var skillid : int;
	skillType = objs[0];
	skillid = objs[1];
	responseTargetp = objs[2];
	responseInstanceID = objs[3];
	DidPunch(skillid - 100);
}
private var targetPS : PlayerStatus;
private var targetMS : MonsterStatus;

var	tagetp	:	Transform;
/// <summary>
/// 攻击行为，传入连击阶段 -
/// </summary>
function	DidPunch (h : int )
{
	if(	!PhotonNetwork.room	)
	{
		busy	=	false;
		return;
	}
	if(	PlayerUtil.isMine(	Status.instanceID	)	)
	{
		if(	tagetp	)
		{
			targetPS	=	tagetp.gameObject.GetComponent(	PlayerStatus	);
			if(	targetPS	)
			{
				ServerRequest.requestUseSkill(	1 , 100 + h , transform.position + transform.up*2.5 + transform.forward*40 , targetPS.instanceID);		
			}
			else
			{
				targetMS = tagetp.gameObject.GetComponent(MonsterStatus);
				if(	targetMS	)
				{
					ServerRequest.requestUseSkill(	1 , 100 + h , transform.position + transform.up*2.5 + transform.forward*40 , targetMS.PlayerID);		
				}
			}
		}
		else
		{
			ServerRequest.requestUseSkill(1 , 100 + h , transform.position + transform.up*2.5 + transform.forward*40 , 0);		
		}
	}
	SendMessage(	"DaDuan",	SendMessageOptions.DontRequireReceiver	);
	if(	this.CompareTag ("Enemy")	)
		Enemytag	=	"Player";
	var temptype	=	0;
	//伤害公式//
	damage	=	parseInt(	Status.MaxATK	)	*	CAttack[h].HitPoint*BtnGameManager.dicClientParms["ClientParms4"]*0.01;
//	print(damage + " == " + Status.MaxATK + " == " + CAttack[h].HitPoint + " == " + BtnGameManager.dicClientParms["ClientParms4"]+ " == " + "" + "");
	//伤害公式浮动20%伤害//
	damage	-=	Random.Range(	0,	damage	*	0.2	);
	if(	Random.Range(	0,	10000	)	<=	parseInt(	Status.Crit	)	)
	{
		damage		*=	2;//bj//
		temptype	=	6;	//攻击属性改为物理暴击//
		if(Random.Range(1,4) > 2)
			SendMessage(	"PlaySelfAudio" ,	5 ,	SendMessageOptions.DontRequireReceiver	);
	}
	SendMessage(	"PlayAnimat" ,	CAttack[h].animation.name ,	SendMessageOptions.DontRequireReceiver	);
    if(	CAttack[h].EffectID	!=	0	)
    {
    	SendMessage("PlayEffect" , CAttack[h].EffectID , SendMessageOptions.DontRequireReceiver);
    }
	if(	h	==	0	)	//阶段//
	{		//跳跃//
		punchPosition	=	Vector3(0,-1,1);
		punchRadius		=	5;
		SendMessage("PlaySelfAudio" , 6 , SendMessageOptions.DontRequireReceiver);
	}
	else	//二阶砍有前进值//
	{
		punchPosition	=	Vector3(0,1,1.5);
		punchRadius		=	4;   
		qianjing(h);
		playerController.Buffspeed	=	0.01;   
	} 
	yield	WaitForSeconds(	CAttack[h].animation.length	*	0.2	/	Status.Attackspeed	);
	if(	!PhotonNetwork.room	)
	{
		busy = false;
		return;
	}
	if(	Status.ProID	==	1	&&	Status.weaponType	==	2	&&	h	==	3	)
	{
		punchRadius		=	7; 
		Allattack(	transform.TransformPoint(	punchPosition	),	temptype,	h,	null	);
    }
	yield	WaitForSeconds(	CAttack[h].animation.length	*	0.3	/	Status.Attackspeed	);
	if(	!PhotonNetwork.room	)
	{
		busy	=	false;
		return;
	}
	PlayAudio(	h	);
	if(	Status.isMine && qiuai.objs && (qiuai.objs.CompareTag ("Enemy")||qiuai.objs.CompareTag ("Neutral"))  && responseInstanceID == 0)
	{
		Enemytag	=	qiuai.objs.tag;
		tagetp	=	qiuai.objs;
    }	
	else
	if(	!Status.isMine && Playrobot && Playrobot.targetp)
	{
		tagetp	=	Playrobot.targetp;      
    }
	if(	CAttack[h].fxobject	!=	null	)
	{	//投射//
		cc	=	PhotonNetwork.Instantiate(	this.CAttack[h].fxobject.name,	transform.position+transform.up*2+transform.forward*3,	transform.rotation,	0	);
		Physics.IgnoreCollision(	cc.collider,	collider	);   
		var Bullet : Bulletobject = cc.GetComponent(Bulletobject);  
		
		if(	PlayerUtil.isMine(	Status.instanceID	)	)
		{
			Bullet.target		=	tagetp;      
			Bullet.targetp		=	transform.position + transform.up*2.5 + transform.forward*40;
//			KDebug.Log(	"本地射击赋值射击地点", transform, Color.cyan	);
		}
		else
		{
			var go : GameObject;
			if(	responseInstanceID == PlayerUtil.myID	)
			{
				go = PlayerStatus.MainCharacter.gameObject;
			}
			else
			{
				go = ObjectAccessor.getAOIObject(responseInstanceID);			
			}
			if(go == null)
			{
				go = MonsterHandler.GetInstance().FindMonsterByMonsterID(responseInstanceID);			
			}
			if(	go != null	)
			{
				Bullet.target	=	go.transform;
				Bullet.targetID	=	responseInstanceID;
			}
//			KDebug.Log(	"本地射击赋值射击地点", transform, Color.magenta	);
			Bullet.targetp	=	responseTargetp;		
		}
		Bullet.PlayerID		=	Status.instanceID;
		Bullet.Damagetype	=	Damagetype+temptype;
		Bullet.attackerLV	=	parseInt( Status.Level);
		Bullet.damage		=	damage;
		Bullet.Enemytag		=	Enemytag;
		Bullet.fxtype		=	CAttack[h].fxtype;
		Bullet.Status = Status;
	}
	else	
	{	//平砍//       
		var	pos	=	transform.TransformPoint(punchPosition);     
		if(	tagetp	&&  ( tagetp.tag == "Enemy"  || tagetp.tag == "Neutral" || tagetp.tag == Enemytag) &&Vector3.Distance(	tagetp.position	+	Vector3(0,1,0),	pos	)	<	punchRadius	&& (PlayerUtil.isMine(Status.instanceID) || !Status.isMine))
		{	//目标在攻击范围内//
			var	setArray	=	new int[5];
			setArray[0]		=	Status.instanceID;
			setArray[1]		=	damage;
			setArray[2]		=	Hatred*damage;
			setArray[3]		=	Damagetype+temptype;
			setArray[4]		=	parseInt(Status.Level);
			if(	tagetp &&CAttack[h].fxtype	!=	fxType.Non	)	//加Buff//
				tagetp.SendMessage("ApplyBuff",FxArray(CAttack[h].fxtype),SendMessageOptions.DontRequireReceiver );
			tagetp.SendMessage(	"ApplyDamage",	setArray,	SendMessageOptions.DontRequireReceiver	);
			if(	Status.ProID	==	1	)						//战士平砍加2怒气//
				Status.AddMana(2);
		}
		if(	!Single	)
		{
			Allattack( pos, temptype, h, tagetp );
		}
	}
	yield	WaitForSeconds(	punchHitTime	/	Status.Attackspeed	);	//等待攻击时间之后解除攻击冷却//

	playerController.Buffspeed	=	1; 
    Status.Hide					=	false;
	busy						=	false;
}

function	Allattack(	pos:	Vector3,	temptype:	int,	h:	int,	tichu :	Transform	)
{
	var	enemies	:	GameObject[]	=	GameObject.FindGameObjectsWithTag(Enemytag);	
	for(	var	go	:	GameObject	in	enemies	)
	{
		if (go.transform !=tichu && Vector3.Distance(go.transform.position+Vector3(0,2,0), pos) < punchRadius && PlayerUtil.isMine(Status.instanceID))
		{         
            var settingsArray	=	new int[5];
			settingsArray[0]	=	Status.instanceID;
			settingsArray[1]	=	damage*0.6;
			settingsArray[2]	=	Hatred*damage*0.6;
			settingsArray[3]	=	Damagetype+temptype;
			settingsArray[4]	=	parseInt(Status.Level);
	        if( go && CAttack[h].fxtype	!=	fxType.Non	)
				go.SendMessage(	"ApplyBuff",	FxArray(CAttack[h].fxtype),	SendMessageOptions.DontRequireReceiver	);
	        go.SendMessage(	"ApplyDamage",	settingsArray,	SendMessageOptions.DontRequireReceiver	);
			if(	Status.ProID	==	1	&&	Status.weaponType == 2	)
				Status.AddMana(1);	//战士的ID = 2武器群杀每砍到一个人多给1点怒气//
		}	
	}
}

private var	qian	=	-0.1;
private var rolldis =	-0.1;

private	function	qianjing(i : int)
{
	if(	tagetp	)
		qian	=	Vector3.Distance(tagetp.position, transform.position)-6; 
	if( qian	>=	CAttack[i].qianjin)
	{
		qian	=	CAttack[i].qianjin;
//     photonView.RPC("PlayAnimat","XiaoTiao"); 
	}
}

/// <summary>
/// 根据状态标记对角色位置进行更新 -
/// </summary>
function	FixedUpdate ()
{
	if( qian >0 )
	{
		move=(transform.TransformDirection (0,0,1))*20*Time.deltaTime*playerController.Movespeed;	
		controller.Move(move);
		qian-=20*Time.deltaTime;
	}
	else if(rolldis>0)
	{
		var movement = cameraTransform.TransformDirection (alljoy.h,0,alljoy.v+0.01).normalized;
		movement.y = 0;
		move=movement*30*Time.deltaTime*playerController.Movespeed;	
		controller.Move(move);
		rolldis-=30*Time.deltaTime;
	}
}

function	OnDrawGizmosSelected ()
{	
	Gizmos.color	=	Color.yellow;
	Gizmos.DrawWireSphere(	transform.TransformPoint(punchPosition),	punchRadius	);
}

//@RPC
function	showswipe(	b	:	boolean	)
{
	if(	leftSwipe	)
		leftSwipe.Emit	=	b;
	if(	rightSwipe	)
		rightSwipe.Emit	=	b;
}

//@RPC
function	PlayAudio(	h	:	int	)
{
   if(	CAttack[h].impact	)
		audio.PlayOneShot(CAttack[h].impact,0.8);  
}
	
/// <summary>
/// 施加Buff时获取序列化的Buff数据 -
/// </summary>
function	FxArray(	Type	:	fxType	)	:	int[]
{
	var setArray	=	new int[4];
	setArray[0]		=	Status.instanceID;
	switch (Type)
 	{				
 		case 1:		//击退-
			setArray[1] = 4;
			setArray[2] = 3;
			setArray[3] = 1;
			break;
		case 2:		//击飞-
			setArray[1] = 5;
			setArray[2] = 8;
			setArray[3] = 1;
			break;
		case 3:		//击倒-
			setArray[1] = 6;
			setArray[2] = 8;
			setArray[3] = 3;
			break;
		case 4:		//减速-
			setArray[1] = 10;
			setArray[2] = 50;
			setArray[3] = 2; 
			break;
	}
	return	setArray;
}         						

function	resetSkill()
{
	hittime	=	-2;
	busy	=	false;
} 

function	BreakRide()
{
	if(	Status.ridemod	){
			ServerRequest.syncAct(Status.instanceID , CommonDefine.ACT_RideOff, "");
			SendMessage("rideOff" , SendMessageOptions.DontRequireReceiver);
	
	}
//		photonView.RPC(	"rideOff",	PhotonTargets.All	);
}

function	StopMove()
{
	controller.Move(	Vector3.zero	);
}

function GetBusy() : boolean{
	return busy;
}

