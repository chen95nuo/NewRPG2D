#pragma strict
var TrapDamagetype : Damagestype = 0;
var TrapdamageT1 : int = 0;
var TrapdamageT2 : int = 0; 
var TrapLevel : int = 1; 
var TrapID	:	int = 999999;	//ApplyDamage用的实例ID//
var TrapffectID : int;

//var photonView : PhotonView;
private var ptime : float = 0;

function	Start()
{
	TrapdamageT1 = AllManage.dungclStatic.level * 20 * BtnGameManager.dicClientParms["ClientParms1"]*0.01;
	TrapdamageT2 = AllManage.dungclStatic.level * 20 * BtnGameManager.dicClientParms["ClientParms1"]*0.01;
	if(	isZiDan	)
	{
		yield	WaitForSeconds(1.5);
		Destroy(	gameObject	);
	}
}

function	OnTriggerStay(Other : Collider)
{
	if(	Time.time > ptime && trapType != TrapType.qiangci	)
	{
		if(Other.tag == "Player" || Other.tag == "Enemy")
		{
			ptime = Time.time + 2;
       		var settingsArray = new int[5];
			settingsArray[0]= TrapID;
			settingsArray[1]=	TrapdamageT1;
			settingsArray[2]=	TrapdamageT2;
			settingsArray[3]= TrapDamagetype;
			settingsArray[4]= TrapLevel; 
			if(Other)
			{
				if(TrapffectID != 0)
				 	PlayEffect( TrapffectID );
					//photonView.RPC("PlayEffect",TrapffectID);
				if(! AllManage.dungclStatic.DungeonIsDone){
					Other.SendMessage("ApplyDamage",settingsArray , SendMessageOptions.DontRequireReceiver);				
				}
			}
		}
	}
}

function	OnTriggerEnter(Other : Collider)
{
	if(	trapType == TrapType.qiangci	)	//墙刺//
	{
		if(Other.tag == "Player" || Other.tag == "Enemy")
		{
       		var settingsArray = new int[5];
			settingsArray[0]= TrapID;
			settingsArray[1]= TrapdamageT1;	//伤害.//
			settingsArray[2]= TrapdamageT2;	//仇恨//
			settingsArray[3]= TrapDamagetype;
			settingsArray[4]= TrapLevel; 
			if(	Other	)
			{
				if(	TrapffectID != 0	)
					PlayEffect( TrapffectID );
					//photonView.RPC("PlayEffect",TrapffectID);
				if(! AllManage.dungclStatic.DungeonIsDone){
					Other.SendMessage("ApplyDamage",settingsArray , SendMessageOptions.DontRequireReceiver);
				}
			}
		}	
	}
}

var trapType : TrapType;
var moveVec : Vector3;
var isZiDan : boolean = false;
var speed	: float = 0;
function	Update()
{
	if(	isZiDan	)
	{
		if(	trapType	==	TrapType.qiangci	)
		{
			transform.localPosition += transform.right * speed * Time.deltaTime;
			//transform.position = Vector3.Lerp (transform.position, moveVec , speed );
		}
	}
}

//@RPC
function	PlayEffect(i:int)
{
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}
