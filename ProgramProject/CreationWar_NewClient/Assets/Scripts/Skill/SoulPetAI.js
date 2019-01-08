#pragma strict
var agent	:	NavMeshAgent;
var rs		:	RealSoul	=	new	RealSoul();	//魔魂属性//
var targetp :	Transform;
var	targetm :	Transform;
var targetID : int;
private var anim_	:	Animation;
private var offset	:	Vector3;
private var lastatktime	=	0.0;
private var lastskltime	=	0.0;
var attackRange		=	16;
var delayAttackTime	=	2.5;
var damage		=	10;
var beattack	=	false;
var Movespeed	=	1.0;
private var photonView	:	PhotonView;
private var Starspeed	=	0.0;
private var Damagetype	:	Damagestype = 0;
private var players		:	PlayerStatus;
private var Soulskill	:	SoulPetSkill;
var Level	=	1;
var Maxmana =	0;
var mana	=	0;

function	Start () 
{
	photonView	=	GetComponent(PhotonView);
//	PlayerID	=	parseInt(photonView.viewID);
	if(	UIControl.mapType != MapType.zhucheng )
		fxobject	=	Resources.Load("BulletObject1", GameObject);
	anim_		=	GetComponentInChildren.<Animation>();
	anim_.wrapMode		=	WrapMode.Loop;
	anim_["run"].layer	=	-2;
	anim_["idle"].layer	=	-2;
	anim_["attackb1"].wrapMode	=	WrapMode.Once;
	anim_["attacke1"].wrapMode	=	WrapMode.Once;
	anim_.Stop();
	anim_.CrossFade("idle");
    Starspeed	=	agent.speed;
    yield	WaitForSeconds(1);
    
    if(PlayerUtil.isMine(targetm.GetComponent(PlayerStatus).instanceID)){
	//	var targetID : int = targetm.GetComponent(PhotonView).viewID;		/////修改获取实例ID////
		players	=	targetm.GetComponent(PlayerStatus);
		targetID = players.instanceID;		/////修改获取实例ID////
		PlayerID	=	targetID;
		SendMessage("SetPlayerID" , PlayerID , SendMessageOptions.DontRequireReceiver);
		SendMessage(	"findtargetm",	targetID	);
		GetSkill(rs.level,rs.quality,rs.attr,rs.skillLevel);
		yield;
		yield;
		InvokeRepeating("RetrieattrLevel", 2,10);// rs.level*10);
		if(	UIControl.mapType != MapType.zhucheng	)
			InvokeRepeating("Openr", 1, 15);
	}
	yield; 
	var canAttack : boolean = false;
	while(	true	) 
	{
		canAttack = true;
		if(	UIControl.mapType	!=	MapType.zhucheng	&&	CanSeeTarget()	==	2	)
		{
			if( targetm != null ){
				if(players == null){
					players	=	targetm.GetComponent(PlayerStatus);
				}
				if(players != null){
					if(players.dead){
						canAttack = false;
					}else{
						canAttack = true;
					}
				}
			}
			if(canAttack){
				yield	StartCoroutine(	"Attack"	);
			}else{
				yield	Idle();
			}
		}
		else 
			yield	Idle();
	}
}

function	Settargetm(target:Transform)
{
	targetm	=	target;
}

private var SoulHangUpExp : int = 0;
private var intSoulPlus : int[];
function	RetrieattrLevel()
{
	if(parseInt(players.Level)< rs.level)
		return;
//	rs.attrLevel += 1;
//	if(rs.attrLevel>99)
//	{
//		rs.level +=1;
//		if(rs.level  > 99){
//			rs.level = 99;
//		}
//		rs.attrLevel = 0;
//		SendMessage( "PlayEffect",	4	);
//		ServerRequest.syncAct(targetID , CommonDefine.ACT_SoulPlayEffect, "4");
//		AllResources.FontpoolStatic.SpawnEffect(4,transform,AllManage.AllMge.Loc.Get("info681"),2);
//	}
	if(InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText == ""){
		InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText = "0";
	}
	SoulHangUpExp = parseInt(InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText);
	InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText = parseInt(SoulHangUpExp + 10).ToString();
	switch(rs.quality){
		case 1:
			if(SoulHangUpExp > BtnGameManager.dicClientParms["SoulHangUpTime1"] * 60){
				intSoulPlus = AllManage.AllMge.SoulAddExp(rs.quality , rs.level , rs.attrLevel , BtnGameManager.dicClientParms["SoulHangUpExp1"] );
				ShowSoulUpTips(intSoulPlus , rs.level , rs.quality , rs.attrLevel);
				rs.level = intSoulPlus[1];
				rs.quality = intSoulPlus[0];
				rs.attrLevel = intSoulPlus[2];			
				InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText = "0";
				AllManage.InvclStatic.UpdateSoulPet();
			}
			break;
		case 2:
			if(SoulHangUpExp > BtnGameManager.dicClientParms["SoulHangUpTime2"] * 60){
				intSoulPlus = AllManage.AllMge.SoulAddExp(rs.quality , rs.level , rs.attrLevel , BtnGameManager.dicClientParms["SoulHangUpExp2"] );				
				ShowSoulUpTips(intSoulPlus , rs.level , rs.quality , rs.attrLevel);
				rs.level = intSoulPlus[1];
				rs.quality = intSoulPlus[0];
				rs.attrLevel = intSoulPlus[2];			
				InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText = "0";
				AllManage.InvclStatic.UpdateSoulPet();
			}
			break;
		case 3:
			if(SoulHangUpExp > BtnGameManager.dicClientParms["SoulHangUpTime3"] * 60){
				intSoulPlus = AllManage.AllMge.SoulAddExp(rs.quality , rs.level , rs.attrLevel , BtnGameManager.dicClientParms["SoulHangUpExp3"] );				
				ShowSoulUpTips(intSoulPlus , rs.level , rs.quality , rs.attrLevel);
				rs.level = intSoulPlus[1];
				rs.quality = intSoulPlus[0];
				rs.attrLevel = intSoulPlus[2];			
				InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText = "0";
				AllManage.InvclStatic.UpdateSoulPet();
			}
			break;
		case 4:
			if(SoulHangUpExp > BtnGameManager.dicClientParms["SoulHangUpTime4"] * 60){
				intSoulPlus = AllManage.AllMge.SoulAddExp(rs.quality , rs.level , rs.attrLevel , BtnGameManager.dicClientParms["SoulHangUpExp4"] );				
				ShowSoulUpTips(intSoulPlus , rs.level , rs.quality , rs.attrLevel);
				rs.level = intSoulPlus[1];
				rs.quality = intSoulPlus[0];
				rs.attrLevel = intSoulPlus[2];			
				InventoryControl.yt.Rows[0]["SoulHangUpExp"].YuanColumnText = "0";
				AllManage.InvclStatic.UpdateSoulPet();
			}
			break;
	}
}

function ShowSoulUpTips(ints : int[] , sLevel : int , sQual : int , sExp : int){
	if(ints[0] > sQual){
		AllManage.AllMge.TipsUpQual(ints[0] - sQual , transform);
	}
	if(ints[1] > sLevel){
			AllManage.AllMge.TipsUpLevel(ints[1] - sLevel , transform);
			if(AllManage.SoulCLStatic){
				AllManage.SoulCLStatic.UpShowLevel();
			}
	}
	if(ints[2] > sExp){
		AllManage.AllMge.TipsUpExp(ints[2] - sExp);
	}
}

function	GetSkill(	Lv:int,	Mlv:int,	ProID:int,	skilllevel:int	)	
{
	Soulskill	=	GetComponent(	SoulPetSkill	);
	Level		=	Lv;
	Maxmana		=	Lv*2+Lv*6*Mlv +120;
	mana		=	Maxmana;
	damage		=	Lv*6*Mlv;
	delayAttackTime	=	4 - Mlv*0.5;
	Soulskill.ReadyPetSkill(ProID);
	mideffect	=	132;
	switch (ProID)
	{
		case 1: 
//      mideffect=132;
			endeffect=59;
			Damagetype = 3;
			break; 
		case 2: 
//      mideffect=45;
			endeffect=58;
			Damagetype = 2;
			break;
		case 3: 
//      mideffect=73;
			endeffect=75;
			Damagetype = 5;
			break;
		case 4: 
//      mideffect=74;
			endeffect=76;      
			Damagetype = 4;
			break;
		case 5: 
//      mideffect=56;
			endeffect=3; 
			Damagetype = 1;
			break;  
	}
}

function	Openr()
{
	if(	rs.quality	>	3	)
	{
		Soulskill.OpenRing(1);
	}
}

private	function	Timer (ttime: float)
{ 
	while(	ttime>=0 && !beattack && CanSeeTarget ()==0)
	{  
		yield	WaitForSeconds(0.2);  
		ttime	-=	0.2;         
	}
	return;
}

function	CanSeeTarget () : int 
{
	targetp	=	FindClosestEnemy();
	if(	!targetp	)
		return 0;      
	else if(	Vector3.Distance(	transform.position,	targetp.position	)	>	attackRange	)
		return 0;		
	var hit : RaycastHit;	
	if (	Physics.Linecast (Vector3(transform.position.x,transform.position.y+3,transform.position.z), Vector3(targetp.position.x,targetp.position.y+3,targetp.position.z), hit))
		return 2;
	return 0;
}

function Idle ()
{		 
	agent.speed	=	Movespeed*Starspeed;  
	if(	UIControl.mapType != MapType.zhucheng	)
		yield	StartCoroutine("Timer",1);
	else
		yield	WaitForSeconds(2);
	if(beattack)
	{
		return;
	} 
	while(	targetm	== null	)
	{
		yield;    	
	}
     
	Findway(	targetm.position	+	targetm.right	);
	if(	(transform.position -targetm.position).magnitude > attackRange+10)
	{  
		agent.enabled	=	false;
		transform.position = targetm.position + targetm.right;
	}       
} 

function	Attack()
{
	agent.speed	=	Movespeed*Starspeed;
	var damageT	=	damage	-	damage	*	0.1	*	Random.Range(	3,	7	);
	if(	!targetp	)
		return;
	offset = transform.position - targetp.position; 
	if(	offset.magnitude	<	attackRange	)
	{ 
		anim_.CrossFade("idle");
		agent.enabled = false;
		var angle = 180.0;
		while (angle > 5 && targetp)
		{     
			angle = Mathf.Abs(RotateTowardsPosition(targetp.position,200));
			yield;
			yield;
		}
		if(	PlayerUtil.isMine(targetID) && rs.quality>2 && Time.time > Soulskill.SkillP[0].CoolDown + lastskltime && Soulskill.SkillP[0].Cost <= mana)
			yield	StartCoroutine("SoulSK");
		else	if(	PlayerUtil.isMine(targetID) && rs.quality>1&& Time.time > delayAttackTime + lastatktime )
			yield	StartCoroutine("Shoot",damageT);
	}
	if(!targetp)
		return;
	if((transform.position -targetm.position).magnitude > attackRange)
	{  
		agent.speed=2*Starspeed;
		Findway(targetm.position + targetm.right); 
		yield WaitForSeconds(2); 
		beattack = false;     
		return;	
	}
	return;
}

function	SoulSK()
{
	lastskltime	=	Time.time;
	Soulskill.Skill(0);
	yield	WaitForSeconds(0.5);
	mana	-=	Soulskill.SkillP[0].Cost;
	return;
}

private var fxobject : GameObject;
private var staeffect:int;
private var mideffect:int;
private var endeffect:int;
private var PlayerID : int;
private var shootDirection : Vector3;
function	Shoot	(damageT:int)
{
	SendMessage("SyncAnimation","attackb1");
	ServerRequest.syncAct(targetID , CommonDefine.ACT_SoulSyncAnimation, "attackb1");
	yield WaitForSeconds(0.3);
	shootDirection = transform.position+transform.up*2+transform.forward*2;
	var cc = PhotonNetwork.Instantiate( fxobject.name, shootDirection ,transform.rotation,0);
//	ServerRequest.syncAct(targetID , CommonDefine.ACT_SoulShoot, String.Format("{0};{1}",  fxobject.name , AllResources.Vector3ToString(shootDirection)));
	
	var skillobject : Bulletobject = cc.GetComponent(Bulletobject); 
	if(targetp)
		skillobject.targetp = targetp.position + transform.up*2 + transform.forward*20;
	else   
		skillobject.targetp = transform.position + transform.up*2 + transform.forward*40;
	skillobject.PlayerID = PlayerID;
	skillobject.Damagetype = Damagetype;
	skillobject.attackerLV = Level; 
	skillobject.mideffectID = mideffect;    
	skillobject.endeffectID = endeffect; 
	skillobject.damage = damageT;
	skillobject.Enemytag = "Enemy";  
	yield	WaitForSeconds(0.7);
	lastatktime	=	Time.time;
}

function RPCShoot(strs : String[]){
	var cc = PhotonNetwork.Instantiate( strs[0] , Vector3(parseFloat(strs[1]) , parseFloat(strs[2]) , parseFloat(strs[3])) ,transform.rotation,0);
}

function	Findway(targetPosition : Vector3)
{
	agent.enabled = true;
	yield;
	if( agent.enabled == true)
	{
		agent.Resume();
		agent.destination = targetPosition;
	}
	while(agent.enabled == true)
	{
		yield	StartCoroutine("UpdateAnimationBlend");
		yield;
	}
}


private	function	UpdateAnimationBlend() 
{
	var velocityXZ	:	Vector3	=	Vector3(	agent.velocity.x,	0.0f,	agent.velocity.z	);
	var speed		:	float	=	velocityXZ.magnitude;
	if(	speed	>	0.1	) 
	{
		anim_.CrossFade("run");	
	}
	else
	{
		anim_.CrossFade("idle");
	}
	yield;
	yield;
	yield;
}

private	function	RotateTowardsPosition(	targetPos : Vector3,	rotateSpeed : float ) : float
{
	var relative = transform.InverseTransformPoint(targetPos);
	var angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
	var maxRotation = rotateSpeed * Time.deltaTime;
	var clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);
	transform.Rotate(0, clampedAngle, 0);
	return angle;
}

private	function	FindClosestEnemy () : Transform
{     
	var gos : GameObject[];
	gos	=	GameObject.FindGameObjectsWithTag("Enemy");
	var closest : GameObject;
	var distance = Mathf.Infinity;
	for( var	go	:	GameObject	in gos)
	{
		var diff	=	(	go.transform.position	-	transform.position	);
		var curDistance	=	diff.sqrMagnitude;
		if(	curDistance	<	distance	)
		{
			closest		=	go;
			distance	=	curDistance;
		}
	}
	if(	closest	)
		return	closest.transform;
	else
		return	null;
}

@RPC
function	PlayEffect(	i	:	int	)
{
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}  
      
@RPC
function	SyncAnimation(str :String)
{
	anim_.CrossFade(str);
}

@RPC
function	findtargetm(ID:int)
{
	while(	targetm	==	null	)
	{
		var trans : GameObject;
		trans	=	ObjectFinder.FindTargetGameObject( ID );
		if(	trans	!=	null	)
		{
			targetm	=	trans.transform;
		}
		yield;    	
	}
}