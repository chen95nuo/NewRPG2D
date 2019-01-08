#pragma strict
var target			: Transform;
var targetp			: Vector3;
var PlayerID		: int = 0;
var Damagetype		= 0;
var attackerLV		= 1;
var mideffectID 	: int;
var endeffectID 	: int;
var damage			: int;   
var Enemytag		: String;
var fxtype:fxType	= fxType.Non;
var chuangtou		= false;
var targetID		: int = 0;
var Status : PlayerStatus;
private	var BNetInit	: BulletNetInit;
function	Awake()
{
	BNetInit	=	GetComponent(BulletNetInit);
}
function	Start()
{
	yield;
	if(	PlayerID	==	0	)
	{
		this.collider.enabled	=	false;
		return;
	}
	if(	chuangtou	==	false	)
		target	=	null;
	if(	target	)
	{
		
		var	targetID : int = ObjectFinder.GetInstanceID(	target.gameObject	);
		SendMessage(	"findtarget" ,	targetID ,	SendMessageOptions.DontRequireReceiver	);
	}
	else
	{
		BNetInit.findtargetp(targetp);
	}
		//SendMessage(	"findtargetp" , targetp , SendMessageOptions.DontRequireReceiver);

	if(	mideffectID	!=	0	)
	{
		PlayEffect(mideffectID);
//		photonView.RPC("PlayEffect",mideffectID);
	}
	yield	WaitForSeconds(0.8);
	DestroyOb(	mideffectID	);
}
function	OnTriggerEnter( col : Collider )
{
//	if(	col.CompareTag ( Enemytag )	&& (PlayerUtil.isMine(PlayerID) || (Status && !Status.isMine) ))
	if(	col.CompareTag ( Enemytag )	&& (PlayerUtil.isMine(PlayerID) || (Status && !Status.isMine) || (!Status && col.GetComponent(PlayerStatus) && PlayerUtil.isMine( col.GetComponent(PlayerStatus).instanceID))))
	{
		if(	fxtype != fxType.Non	)
			col.SendMessage( "ApplyBuff", FxArray( fxtype ), SendMessageOptions.DontRequireReceiver );
		var settingsArray	= new int[5];
			settingsArray[0] = PlayerID;
			settingsArray[1] = damage;
			settingsArray[2] = damage;
			settingsArray[3] = Damagetype;
			settingsArray[4] = attackerLV;          						
		col.SendMessage( "ApplyDamage", settingsArray, SendMessageOptions.DontRequireReceiver );	
		PlayEffect( endeffectID );
//      photonView.RPC("PlayEffect",endeffectID);	
		if(	chuangtou	==	false	)
			DestroyOb(mideffectID);
	}
}
function	DestroyOb(	mideffectID	:	int	)
{
	if(	mideffectID	!=	0	)
	{
		UnEffect(	mideffectID	);
	}
	yield ;
	try
	{
		PhotonNetwork.Destroy(	gameObject	);      
	}
	catch( e ){}
} 
//@RPC
function	PlayEffect( i : int )
{
    AllResources.EffectGamepoolStatic.SpawnEffect( i, transform );
}
//@RPC
function	UnEffect(i:int)
{
	gameObject.BroadcastMessage ("Unspawn", i,SendMessageOptions.DontRequireReceiver);
}
function	FxArray(Type : fxType):int[]
{	
	var setArray	=	new int[4];
	setArray[0]		=	PlayerID;
	switch (Type)
	{				
		case 1:
			setArray[1] = 4;
			setArray[2] = 8;
			setArray[3] = 1;
			break;
		case 2:
			setArray[1] = 5;
			setArray[2] = 8;
			setArray[3] = 1;
			break;
		case 3:
			setArray[1] = 6;
			setArray[2] = 10;
			setArray[3] = 3;
			break;
		case 4:
			setArray[1] = 10;
			setArray[2] = 50;
			setArray[3] = 3; 
			break;
	}
	return  setArray;
}