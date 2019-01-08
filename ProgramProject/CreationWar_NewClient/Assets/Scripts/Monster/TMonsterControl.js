#pragma strict
var MonsterSp : MonsterSpawn[];
var MonsterSpBoss1 : MonsterSpawn;
var MonsterSpBoss2 : MonsterSpawn;
private	var attacktime = 0;
private var photonView : PhotonView;
//var ts : TiShi;
private	var	MaxHealth	:int;
private	var	ShowLightBallHP	:ShowMonsterHP;
function	Start ()
{
	photonView	=	GetComponent.<PhotonView> ();
	yield	WaitForSeconds(5);
	yield;
	yield;
	yield;
	yield;
	Health		=	DungeonControl.level * 1000;
	
	//if(	PhotonNetwork.isMasterClient	)
	//{
	while(	!PlayerStatus.MainCharacter	)
	{
		yield;
	}
	ShowLightBallHP	=	FindObjectOfType(	ShowMonsterHP	);
	SetMaxHP(	Health	);
//	yield;
//	yield;
//	InvokeRepeating("Callattack",5,45);
	//}
	yield	WaitForSeconds(5);
	CallStart();
	InvokeRepeating("Settarget",5,2);
	InvokeRepeating("SendDamage",5,0.2f);
}

private	var	IsStarted	:	boolean	=	false;
private	var	TimeFlage	:	float	=	0.0f;
private var ptime : float = 0;
var AllMonster : MonsterStatus[];
var alFinish : boolean = false;
private	function	Update()
{
	if(	IsStarted	)
	{
		TimeFlage	+=	Time.deltaTime;
		if(	TimeFlage	>	45.0f	)
		{
			attacktime	++;
			TimeFlage	-=	45.0f;
			Callattack();
		}
	}
	if(canFinish){
		if(Time.time > ptime && !alFinish){
			ptime = Time.time + 3;
			AllMonster =  FindObjectsOfType(MonsterStatus);
			if(AllMonster == null || AllMonster.length == 0){
				alFinish = true;
				ServerRequest.requestDefneceFinish();
			}
		}
	}
//	else
//	{
//		TimeFlage	+=	Time.deltaTime;
//		if(	TimeFlage	>	5.0f	)
//		{
//			CallStart();
//		}
//	}
}

//发送游戏开始//
private	function	CallStart()
{
	TMonsterHandler.GetInstance(	gameObject	);
	//KDebug.Log(" KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK 发送请求开始黑风防御 " );
	InRoom.GetInRoomInstantiate().DefenceBattleStart(MaxHealth);
}

//接受游戏开始//
function	OnStart(	_TimeFlage	:	String	)
{
	//KDebug.Log("OnStart 服务器回复时间 ： =" + _TimeFlage);
	IsStarted	=	true;
	TimeFlage	=	parseFloat(_TimeFlage);
	attacktime	=	1;
	ComputeAttackTime();
	Callattack();
}

private	function	ComputeAttackTime()
{
	while(	TimeFlage	>	45.0f	)
	{
		attacktime	++;
		TimeFlage	-=	45.0f;
	}
}

private	function	SetMaxHP(	hp	:	int	)
{
	MaxHealth	=	Health;
	if(	ShowLightBallHP	==	null	)
		ShowLightBallHP	=	FindObjectOfType(	ShowMonsterHP	);
	if(	ShowLightBallHP	!=	null	)
		ShowLightBallHP.ShowHaloHp(	Health, MaxHealth	);
}

private	function	SetHP(	hp	:	int	)
{
	Health	=	hp;
	if(	ShowLightBallHP	==	null	)
		ShowLightBallHP	=	FindObjectOfType(	ShowMonsterHP	);
	if(	ShowLightBallHP	!=	null	)
		ShowLightBallHP.ShowHaloHp(	Health, MaxHealth	);
}

function	SendDamage()
{
	if(	Damages	>	0)
	{
		//Debug.Log( "发送伤害"+Damages );
		InRoom.GetInRoomInstantiate().BallApplyDamage(Damages);
		Damages	=	0;
	}
}

var canFinish : boolean = false;
function	Callattack()
{
	if(	attacktime	>	9	)
		return;
	else
	{
		//attacktime	+=	1;
		//KDebug.Log(	"attacktime	=	"+attacktime);
		if(	attacktime ==	3	||	attacktime	==	6	)
		{
			SendMessage(	"showtishi",	"Boss即将到来"	);
			MonsterSpBoss1.CallMonster(	attacktime);
		}
		else 
		if(	attacktime	==	9	)
		{
			SendMessage(	"showtishi",	"最后一波！"	);  
			MonsterSpBoss2.CallMonster(	attacktime);
			canFinish = true;
		}
		else
		{
			SendMessage(	"showtishi",	"第 " + attacktime + " 波攻击即将开始！");
			ShowLightBallHP.ShowNumber(attacktime);     
		}
		for( var Mp : MonsterSpawn in MonsterSp )
		{
			Mp.CallMonster(	attacktime);
		}
		//Settarget();
	}
}

private var settime	= 0.0;
function	OnTriggerEnter (other : Collider)
{
	if(	other.CompareTag ("Enemy")	&&	settime	<	Time.time+3	)
	{
		SendMessage("PlayEffect",133);  
		settime = Time.time;
		other.SendMessage("Settargetp",transform,SendMessageOptions.DontRequireReceiver );
	}
}


private var gos : GameObject[];
function	Settarget()
{
	//yield;
	//yield;
	//KDebug.Log(	"attacktime	=	"+attacktime);
	gos	=	GameObject.FindGameObjectsWithTag("Enemy"); 
	//var MAI	:	MonsterAI;
	for( var go : GameObject in gos )
	{
		go.SendMessage("Settargetm",transform,SendMessageOptions.DontRequireReceiver);   
 	}
}

function	AllStop()
{
	//yield;
	//yield;
	//KDebug.Log(	"attacktime	=	"+attacktime);
	gos	=	GameObject.FindGameObjectsWithTag("Enemy"); 
	//var MAI	:	MonsterAI;
	for( var go : GameObject in gos )
	{
		go.SendMessage( "PauseAttack", true );   
 	}
}


private var pnumber :int;
private var damage :int;
private var Damagetype=0;    //0为默认物理伤害，1~4为魔法，1是技能（奥术），2是冰?是火?是暗影，5是毒（自然）  
var Health	=	10;
var dead	=	false;
var	Damages:int=0;
function	ApplyDamage(	info	:	int[]	)
{	////print("hit me!");
	if(	dead	)
		return;
	pnumber		=	info[0];
	damage		=	info[1];
	Damagetype	=	info[3];   
	SendMessage(	"PlayEffect",	3	);
	Damages	+=	damage;
	//Health -= damage;
	//SetHP(	Health	);
	if(	Damagetype	>	5	)
	{
		AllResources.FontpoolStatic.SpawnEffect(6,transform,"暴击!",2);    
		AllResources.FontpoolStatic.SpawnEffect(4,transform,damage+"",2.5);
	}
	else
		AllResources.FontpoolStatic.SpawnEffect(0,transform,damage+"",2.5);
    //SendMessage("SynHealth",Health);	     
	SendMessage("hitanimation");
	SendMessage("PlayAudio",1);
	
}

function	OnAcceptHP( Data:String )
{
	//KDebug.Log("收到了！收到了！恭喜！收到球血同步了！    "	 +	Data	);
	Health	=	parseInt(Data);
	//Health -= damage;
	SetHP(	Health	);
	if (!dead &&Health <= 0)
	{
		SetHP(	0	);
    	SendMessage("PlayAudio",2);
    	SendMessage("Die");
	}
}

@RPC
function	Die()
{  
	dead		=	true;
	this.tag	=	"Ground";
	AllStop();
	InvokeRepeating("AllStop",0.2,0.2);
	GameOver();
}

//var qr : QueRen;
function	GameOver()
{
//if( !qr){
//	qr = AllManage.qrStatic;
//}
	////print("jie shu le pk");
	AllManage.qrStatic.ShowQueRen(gameObject , "ReturnLevel" , "messages001");  	
}

function	ReturnLevel()
{
	Loading.Level	=	DungeonControl.ReLevel;
	InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	AllManage.UICLStatic.RemoveAllTeam();
	alljoy.DontJump	=	true;
	yield;
	PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

}

@RPC
function	SynHealth(P : int)
{
	Health	=	P;
}

@RPC
function	hitanimation()
{
	if(	animation	&&	animation["hit"]	!=	null	)
		animation.CrossFade("hit",0.1);
}

@RPC
function	PlayEffect(i:int)
{
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}

@RPC
function	showtishi(	Str	:	String	)
{
//  if(!ts)
//  ts = FindObjectOfType(TiShi);
    AllManage.tsStatic.ShowBlue(Str);
}

var	hitSound	:	AudioClip;
var	deadSound	:	AudioClip;
@RPC
function	PlayAudio(	i	:	int	)
{
	switch (i)
	{				
		case	1:
			if(	hitSound	)	
				audio.PlayOneShot(hitSound);
			break;
		case 2:
			if(	deadSound	)	
				audio.PlayOneShot(deadSound);
			break;
	}
}