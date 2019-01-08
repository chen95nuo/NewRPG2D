#pragma strict
	var photonView	:	PhotonView;
	var subskillmove= 0.0;
	var skillID		:	int;
	var skilltype	:	int;
	var target		:	Transform;
	var targetp		:	Vector3;
	var PlayerID	:	int;
	var Damagetype	=	0;
	var attackerLV	=	1;
	var mideffectID	:	int;
	var endeffectID	:	int;
	var Size		=	1.0;
	var damage		:	int;   //被计算的数值				///Needed///
	var Hatred		:	int;
	var buffID		:	int;							///Needed///
	var buffvalue	:	int;							///buffvalue//
	var bufftime	:	int;							///Needed///
	var Objlife		:	float	=		0.5;			///生存时间//
	var rehitime	:	float	=		2.0;
	var addbuffvalue:	int;
	var selftag		:	String	=	"Player";			///Needed///
	var targetID	:	int = 0;
	var Status : PlayerStatus;

private var objsTimeout : Object[] = new Object[4];
function	timeout()
{ 
	yield;
	objsTimeout[0] = skillID;
	objsTimeout[1] = subskillmove;
	objsTimeout[2] = Size;
	objsTimeout[3] = skilltype;
	SendMessage( "IDsy" , objsTimeout , SendMessageOptions.DontRequireReceiver );
//	photonView.RPC(	"IDsy",	skillID,	subskillmove,	Size,	skilltype	);
	yield;
	if(	target	)
	{
		var	targetID : int = ObjectFinder.GetInstanceID(	target.gameObject	);
		SendMessage("findtarget" , targetID);
	}
	SendMessage("findtargetp" , targetp);
//	photonView.RPC("findtargetp",targetp);
	yield;
 
	if(	skillID == 302 || skillID == 115	)//召唤骷髅 和 召唤血刃//
		PetStatus();

	if(	mideffectID	!=	0 )
	{
       PlayskillEffect(mideffectID );
	}
	yield	WaitForSeconds( Objlife );
	if(	skillID == 302 || skillID == 115	){
		if(PlayerUtil.isMine(PlayerID)){
			DestroyOb( mideffectID );
			MonsterServerRequest.SkullRemove(GetComponent(MonsterNetView).MonsterID);
		}
	}else{	
		DestroyOb( mideffectID );
	}
}
private var hittime			=	0.0;
private var tempbuffID		=	0;
private var tempbuffvalue	=	0;
enum	Tagettype
{   //物体发射方式
    None	= 0,
	Enemy	= 1,   //一个
	Firend	= 2	
}
	
function	Enemytaget(col : Collider) : Tagettype
{
	if(	selftag == "Player" &&(col.CompareTag ("Enemy")||col.CompareTag ("Neutral")))
		return	Tagettype.Enemy;
	else if(selftag == "Player" && col.CompareTag ("Player"))
		return	Tagettype.Firend;
	else if(selftag == "Enemy" &&(col.CompareTag ("Enemy")||col.CompareTag ("Neutral")))
		return	Tagettype.Firend;
	else if(selftag == "Enemy" && col.CompareTag ("Player"))
		return	Tagettype.Enemy;
	return	Tagettype.None;
}

function	OnTriggerEnter(col : Collider)
{
	if(	skilltype !=	7	&&	(Enemytaget(col)==Tagettype.Enemy||Enemytaget(col)==Tagettype.Firend)	)
	{
		hittime	=	Time.time;
		SkillFX(col);
	}
	else	
	if(	skillID==214 && Enemytaget(col)==Tagettype.Enemy	)
	{
		bigBoom(PlayerID,damage,buffvalue);
		if(	endeffectID!=0)
		{
         PlayskillEffect(endeffectID );
		}
		DestroyOb(mideffectID);
	} 
}

function	OnTriggerStay	(col : Collider)	
{
	if(	skilltype	!=	7	&&	(	Enemytaget(col)	==	Tagettype.Enemy	||	Enemytaget(col)	==	Tagettype.Firend	)	)
	{
		yield;
		if(	hittime + rehitime < Time.time )
		{
			hittime=Time.time;
			SkillFX(col);
		}
	}    
}

function	SkillFX(col : Collider)
{
	if(	col	==	null	)
		return;
	tempbuffID =buffID;
	tempbuffvalue =buffvalue;  
         
	if(	Enemytaget(col)==Tagettype.Firend	&&	buffID==37	)
		tempbuffID = 18;	
	else if(	Enemytaget(col)==Tagettype.Firend &&(buffID==35||buffID==36||buffID==38)	)
		tempbuffID = 17;
  
	if(	buffID==17	||	buffID==18	)
		tempbuffvalue = buffvalue*Hatred*0.01;
	else if(	(buffID>=11 && buffID<=16)||(buffID>=35&&buffID<=38)	)
		tempbuffvalue = buffvalue*damage*0.01; 
	        
	if(	((	Enemytaget(col)==Tagettype.Enemy && (( buffID>0 && buffID<=16)||(buffID<=38 && buffID>=34)))||(Enemytaget(col)==Tagettype.Firend && buffID>=17)) && (PlayerUtil.isMine(PlayerID) || !Status.isMine	))
	{          
		var setArray	= new int[4];
		setArray[0]		= PlayerID;
		setArray[1]		= tempbuffID;            
		setArray[2]		= tempbuffvalue; 
		setArray[3]		= bufftime;                                						
		col.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver );
	}
	if(	(Enemytaget(col)==Tagettype.Enemy && (skillID==304||skillID==305) && addbuffvalue != 0) && (PlayerUtil.isMine(PlayerID) || !Status.isMine ))
	{
		var setArray2 = new int[4];
            setArray2[0]= PlayerID;
            setArray2[1]= 11;           
            setArray2[2]= addbuffvalue;
            setArray2[3]= 6;            						
		col.SendMessage("ApplyBuff",setArray2,SendMessageOptions.DontRequireReceiver );		  
	}
	if(	(Enemytaget(col)	==	Tagettype.Enemy && damage >0) && (PlayerUtil.isMine(PlayerID)	 || !Status.isMine ))
	{
		var settingsArray = new int[5];
		settingsArray[0]= PlayerID;
		settingsArray[1]= damage;
		settingsArray[2]= Hatred;
		settingsArray[3]= Damagetype;
		settingsArray[4]= attackerLV;          						
		col.SendMessage("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );	
	}
	if(	skillID==310||skillID==312||skillID==314)
		allHatred(tempbuffvalue); 
	if(endeffectID	!=	0 && ((	Enemytaget(col)==Tagettype.Enemy && (( buffID>=0 && buffID<=16)||(buffID<=38 && buffID>=34)))||(Enemytaget(col)==Tagettype.Firend &&( buffID>=17&& buffID>=34))) && (PlayerUtil.isMine(PlayerID) || !Status.isMine	))
	{ 
		PlayskillEffect(endeffectID );
	}
    if(skillID ==104||skillID==105||skillID==110||skillID==115||skillID==202||skillID==208||skillID==211||skillID==305||skillID==314)
    {
    	huandong(1,transform.position ); 
    }
	if(	skilltype	==	6	)
		DestroyOb(mideffectID);
}

function ServerReturnDesMe(){
	DestroyOb(mideffectID);
}

function	huandong(ID:int,position:Vector3)
{
	newCamera.huandong(ID,position);
}

function	DestroyOb(mideffectID:int)
{
	if(mideffectID!=0){
		UnEffect(mideffectID);
//		photonView.RPC("UnEffect",mideffectID);	
	}
	yield ;
	yield ; 
	if(	skillID != 302 || skillID != 115)
	{
		try
		{
            PhotonNetwork.Destroy(gameObject);      
    	}
    	catch(e)
    	{    
    	}
    }
} 
function	GetPetMaxHealth():int
{
	return	2*damage + 400;
}
function	PetStatus()
{
	var Status	=	GetComponent(MonsterStatus);
	var	MAI		=	GetComponent(MonsterAI);  
	this.tag	=	selftag;
//	KDebug.Log(	" Pet Status Tag = "	+	selftag,	transform,	Color.red	);
	
	Status.ATK			=	damage	*	0.8;
	Status.Maxhealth	=	GetPetMaxHealth().ToString();
	Status.Health		=	Status.Maxhealth;
	Status.Pettime		=	Objlife	-	3;
	while(	!MAI.targetm	)
	{
		MAI.targetm	=	Status.FindWithID(	PlayerID,	selftag	); 
		yield;
	}
	MAI.setenemytag();
	
	if(	(skillID	==	302 || skillID == 115	)&&	buffID	!=	0	)
	{
		var petff		=	GetComponent(Petdebuff);
		petff.buffID	=	buffID;
		petff.buffvalue	=	buffvalue;
		petff.bufftime	=	bufftime;
	}
}

function	Settag(PlayerviewID:int)
{
	var targetpp	=	ObjectAccessor.GetObjectByInstanceID(PlayerviewID);
	if(	targetpp	)
		this.tag	=	targetpp.tag;
//	KDebug.Log( "Tag = " + this.tag, transform, Color.green );
}

function	bigBoom( PlayerID : int, damage:int,buffvalue : int )
{
	huandong( 3, transform.position );
	tempbuffID		=	buffID;
	tempbuffvalue	=	buffvalue; 
 	var	colliders	:	Collider[]	=	Physics.OverlapSphere(	transform.position,	8	);
	for(var	hit	in	colliders	)
	{
		var closestPoint = hit.ClosestPointOnBounds(transform.position);
		var distance = Vector3.Distance(closestPoint, transform.position);
		var hitPoints = 1.0 - Mathf.Clamp01(distance / 6);
		hitPoints *= damage;
		if(	(Enemytaget(hit)	==	Tagettype.Enemy)	 && (PlayerUtil.isMine(PlayerID) || !Status.isMine ))
		{
			var setArray = new int[4];
            setArray[0]= PlayerID;
            setArray[1]= tempbuffID;            
            setArray[2]= tempbuffvalue; 
            setArray[3]= bufftime;                                						
			hit.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver );

			var settingsArray = new int[5];
			settingsArray[0]=PlayerID;
			settingsArray[1]=hitPoints;
			settingsArray[2]=hitPoints;
			settingsArray[3]= 3;
			settingsArray[4]= attackerLV; 
			hit.SendMessageUpwards("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );			
		}
	}  
}

private		function	allHatred(	buffvalue:int	)
{
 	var colliders	:	Collider[]	=	Physics.OverlapSphere (	transform.position,	25	);
	var settingsArray	=	new int[3];
	settingsArray[0]	=	PlayerID;
	settingsArray[1]	=	0;
	settingsArray[2]	=	buffvalue;
	for( var hit in colliders )
	{
		if(	Enemytaget(hit)	==	Tagettype.Enemy	)
			hit.SendMessageUpwards(	"saveHatred",	settingsArray,	SendMessageOptions.DontRequireReceiver	);			
	}  
}

function	PlayskillEffect(	i:int	)
{
	if(	AllResources.EffectGamepoolStatic	)
	{
	    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
	}
}

function	UnEffect(i:int)
{
	gameObject.BroadcastMessage (	"Unspawn",	i,	SendMessageOptions.DontRequireReceiver	);
}