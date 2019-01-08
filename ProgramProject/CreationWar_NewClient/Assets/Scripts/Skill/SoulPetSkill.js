#pragma strict
var SkillP	:	GameSkill[];
var busy	=	false;
private var PlayerID	:	int;
private var targetp		:	Vector3;
private var cc			:	GameObject;
private var photonView	:	PhotonView;
private var Enemytag	:	String;
private var MAI			:	SoulPetAI;
private var anim_		:	Animation;
private var CanCombat	=	1;
private var CanChange	=	1;
private var CanRefes	=	1;
private var CanSelf		=	1;
private	var	MNetView	:	MonsterNetView;
function	Awake()
{
	Enemytag	=	"Enemy";
	photonView	=	GetComponent.<	PhotonView	> ();
	MAI			=	GetComponent.<	SoulPetAI	> ();
//	PlayerID	=	parseInt(	photonView.viewID	);
	anim_		=	GetComponentInChildren.<	Animation	>();
}

function SetPlayerID(id : int){
	PlayerID = id;
}

function	ReadyPetSkill(	ProID : int	)
{
	SkillP		=	new	Array (2);
	SkillP[0]	=	new	GameSkill();
	SkillP[1]	=	new	GameSkill();
	SkillP[0]	=	DungeonControl.AllSkiss[	ProID	+	46	];
	SkillP[1]	=	DungeonControl.AllSkiss[	ProID	+	51	];
	SkillP[0].animation	=	anim_[	"attacke1"	].clip; 
}
private var combo	=	1;
function	Skill( i : int )
{
	if(	SkillP[i]	)
	{
		busy = true;
		switch( SkillP[i].sType )
		{				
			case 0:
				yield	ActiveSkill(i);
				break;
			case 1:
				OpenRing(i);
				yield	WaitForSeconds(0.5);
				break;
		}
		busy = false;
	}
}
function	AcceptSummon(	SData	:	String	)
{
//	KDebug.Log(	"~ ~ ~ 收到骷髅信息 ~ ~ ~ 收到骷髅信息 ~ ~ ~", transform, Color.red	);
	MNetView		=	gameObject.GetComponent( MonsterNetView );
	if(	MNetView	==	null	)
		MNetView	=	gameObject.AddComponent( MonsterNetView );
	if(	MNetView	!=	null	)
	{
		if(	MNetView.MonsterID	==	0	)
		{
			MNetView.MonsterID	=	parseInt( SData[0] );
			MonsterHandler.GetInstance().RegisterNewSkull(	MNetView	);
		}
	}
	PlayerID						=	MNetView.MonsterID;
	var	Summanner	:	GameObject	=	ObjectFinder.FindTargetGameObject( MNetView.SummonerID );
	if( Summanner	!=	null )
	{
		tag	=	Summanner.tag;
	}
	var _MAI : MonsterAI = GetComponent( MonsterAI );
	if( _MAI != null )
	{
//		KDebug.Log(	"最终~ ~ ~ 收到骷髅信息 ~ ~ ~ 收到骷髅信息 ~ ~ ~最终", transform, Color.red	);
		_MAI.setenemytag();
	}
}
function	ActiveSkill(	i	:	int	)
{
	if(	MAI.targetp	)
		targetp	=	MAI.targetp.transform.position + transform.forward*20 +transform.up*2;
	else
		targetp	=	transform.position + transform.up*2 + transform.forward*40;
	yield	WaitForSeconds(0.2);
	if(	SkillP[i].startID	!=	0	){
		SendMessage(	"PlayEffect",	SkillP[i].startID	);
		ServerRequest.syncAct(MAI.targetID , CommonDefine.ACT_SoulPlayEffect, SkillP[i].startID.ToString());
	}
	combo	=	SkillP[i].batterNum;
    while(	combo	>	0	)
    {
		if(	SkillP[i].animation	!=	null	){
			SendMessage(	"SyncAnimation",	SkillP[i].animation.name	);
			ServerRequest.syncAct(MAI.targetID , CommonDefine.ACT_SoulSyncAnimation, SkillP[i].animation.name);
		}
     	yield	WaitForSeconds(	SkillP[i].animation.length	-	0.5	);       
        CallObject(	i	);
        yield	WaitForSeconds(	0.5	);
        combo	-=1;
	}   
	return;
}
function	CallObject(	i	:	int	)
{
	cc	=	PhotonNetwork.Instantiate(	this.SkillP[i].fxobject.name,	ObjectPosition(SkillP[i].skillType),	transform.rotation,	0	);
//	ServerRequest.syncAct(PlayerID , CommonDefine.ACT_SoulShoot, String.Format("{0};{1}",  this.SkillP[i].fxobject.name , AllResources.Vector3ToString(ObjectPosition(SkillP[i].skillType))));
	var	skillobject	:	MonsterSkillobject	=	cc.GetComponent(MonsterSkillobject); 
	skillobject.targetp		=	targetp;
	skillobject.PlayerID	=	PlayerID;
	skillobject.Damagetype	=	SkillP[i].damageType;
	skillobject.attackerLV	=	MAI.Level;
	skillobject.skilltype	=	SkillP[i].skillType;  
	skillobject.scope		=	SkillP[i].scope; 
	skillobject.mideffectID	=	SkillP[i].middleID;    
	skillobject.endeffectID	=	SkillP[i].endID;
	skillobject.damage		=	SkillP[i].damageValue*MAI.damage*0.01;
	skillobject.buffID		=	SkillP[i].buffID;
	skillobject.buffvalue	=	SkillP[i].buffValue ;//百分比
	skillobject.bufftime	=	SkillP[i].buffTime ; 
	skillobject.selftag		=	"Player" ; 
	if(	SkillP[i].skillType	<=	3	)
		cc.transform.parent	=	this.transform;
}

function RPCCallObject(strs : String[]){
	var cc = PhotonNetwork.Instantiate( strs[0] , Vector3(parseFloat(strs[1]) , parseFloat(strs[2]) , parseFloat(strs[3])) ,transform.rotation,0);
}

private	function	ObjectPosition(i : int) : Vector3
{  
	var ST :Vector3;
	switch( i )
	{				
		case 0:
		case 1:
		case 5:
		case 7:
			ST = transform.position+transform.up*2+transform.forward*2; 
			break;
		case 2:
			ST = transform.position+transform.up*2+transform.forward*5; 
			break;
		case 3: 
			ST = transform.position+transform.up*2;  
			break;
		case 4:
			if(	MAI.targetp	)
				ST = MAI.targetp.transform.position+transform.up*2;
			else
				ST = transform.position+transform.forward*10+transform.up*2;     
			break;
		case 6:  
			ST = transform.position+transform.forward*4 + transform.right*Random.Range(-5,5) ;
			break;
		case 8:  
			ST = transform.position+transform.up*2 + transform.forward*Random.Range(0,10) + transform.right*Random.Range(-10,10) ;
			break;          
	}  
	return ST;  
}
function	OpenRing(i:int)
{
	if(	SkillP[i].startID	!=	0	){
        SendMessage("PlayEffect",SkillP[i].startID );
        ServerRequest.syncAct(MAI.targetID , CommonDefine.ACT_SoulPlayEffect, SkillP[i].startID.ToString());
	}
	SkillP[i].OpenR = true;  
	if(	SkillP[i].skillType	<=	2	)
		Ring(	SkillP[i].skillType,	i	);            
	yield	WaitForSeconds(14);    
	SkillP[i].OpenR	=	false;        
}
private	var	bb	=	true;
function	Ring(	tpye:int,	i:int	)
{
	while(	SkillP[i].OpenR	&&	tpye==0	)
	{
		if(	bb	)
		{
			bb	=	false;
			allhit(	(SkillP[i].scope+1)*3+2,	SkillP[i].buffValue*MAI.damage*0.01,	SkillP[i].damageType	);
			if(	SkillP[i].middleID	!=	0	){
				SendMessage(	"PlayEffect",	SkillP[i].middleID	);
				ServerRequest.syncAct(MAI.targetID , CommonDefine.ACT_SoulPlayEffect, SkillP[i].middleID.ToString());
			}
			yield	WaitForSeconds(3);
			bb	=	true;
    	}
		yield;
	}
	while(	SkillP[i].OpenR	&&	tpye==1 )
	{
		if(	bb	)
		{
			bb = false;
			allbuff(	(SkillP[i].scope+1)*3+2,	SkillP[i].buffID,	SkillP[i].buffValue,	SkillP[i].buffTime,	Enemytag	);
			if(	SkillP[i].middleID	!=	0	){
				SendMessage("PlayEffect",SkillP[i].middleID);
				ServerRequest.syncAct(MAI.targetID , CommonDefine.ACT_SoulPlayEffect, SkillP[i].middleID.ToString());
			}
			yield	WaitForSeconds(3);
			bb	=	true;
		}
		yield;
	}
	while(	SkillP[i].OpenR && tpye==2 )
	{
		if(	bb	)
		{
			bb	=	false; 
			if(	MAI.targetm	)
				addbuff(	MAI.targetm,	SkillP[i].buffID,	SkillP[i].buffValue,	SkillP[i].buffTime	);
			if(	SkillP[i].middleID	!=	0	){
				SendMessage(	"PlayEffect",	SkillP[i].middleID	);
				ServerRequest.syncAct(MAI.targetID , CommonDefine.ACT_SoulPlayEffect, SkillP[i].middleID.ToString());
			}
			yield	WaitForSeconds(3);
			bb	=	true;
		}
		yield;
	}
}
private	function	addbuff(	target:Transform,	buffID:int,	buffValue:int,	bufftime:int	)
{
	var	settingsArray	=	new	int[4];
	settingsArray[0]	=	PlayerID;
	settingsArray[1]	=	buffID;
	settingsArray[2]	=	buffValue*MAI.damage*0.01;
	settingsArray[3]	=	bufftime;	
	target.SendMessageUpwards(	"ApplyBuff",	settingsArray,	SendMessageOptions.DontRequireReceiver	);			
}
private	function	allhit( radius : int, buffValue : int, damagetype : int )
{
	var	colliders		:	Collider[]	=	Physics.OverlapSphere (	transform.position,	radius	);
	var	settingsArray	=	new	int[5];
	settingsArray[0]	=	PlayerID;
	settingsArray[1]	=	buffValue*MAI.damage*0.01;
	settingsArray[2]	=	buffValue*MAI.damage*0.01;
	settingsArray[3]	=	damagetype;
	settingsArray[4]	=	MAI.Level; 
	for( var hit in colliders )
	{
		if( hit.CompareTag (Enemytag) )
			hit.SendMessageUpwards( "ApplyDamage", settingsArray, SendMessageOptions.DontRequireReceiver );			
	}  
}
private	function	allbuff(	radius:int,	buffID:int,	buffValue:int,	bufftime:int,	tag:String	)
{
	var	colliders		:	Collider[]	=	Physics.OverlapSphere (	transform.position,	radius	);
	var	settingsArray	=	new	int[4];
	settingsArray[0]	=	PlayerID;
	settingsArray[1]	=	buffID;
	settingsArray[2]	=	buffValue*MAI.damage*0.01;
	settingsArray[3]	=	bufftime;	
	for(	var hit in colliders	)
	{
		if(	hit.CompareTag (Enemytag)	)
			hit.SendMessageUpwards("ApplyBuff",settingsArray,SendMessageOptions.DontRequireReceiver );
	}				
}
