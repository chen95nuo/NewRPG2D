#pragma	strict

//var ptime : int = 0;
var myTeam		:	String	=	"5";
var Status		:	MonsterStatus;
private	var	MAI	:	MonsterAI;
var myPosLu		:	int;
var viewID		:	int;
function	Awake()
{
	photonView	=	GetComponent(PhotonView);
	viewID		=	photonView.viewID;
}

function	Start()
{
	Status	=	GetComponent(	MonsterStatus	);
	if(	Application.loadedLevelName == "Map911"	)
	{
		Status.Maxhealth = UIControl.ActivityBossHPMax.ToString();	
	}
	else
	if(	Application.loadedLevelName == "Map912"	)
	{
		Status.Maxhealth = UIControl.ActivityBossHPMax.ToString();	
	}
	else
	{
		Status.Maxhealth = "200000";			
	}
	MAI		=	GetComponent(	MonsterAI	);
	while(	!PlayerStatus.MainCharacter	)
	{
		yield;
	}
	yield;

	if(	UIControl.myTeamInfo != myTeam	)
	{
		this.tag	=	"Enemy";
		if(	MAI	)
			MAI.Enemytag	=	"Player";
	}
	else
	{
		this.tag	=	"Player";
		if(	MAI	)
			MAI.Enemytag = "Enemy";
	}
	InvokeRepeating("UpdateHp", 2, 1);
}

var dotamcl : DOTMonsterControl;
function	UpdateHp()
{
	//if(	PhotonNetwork.isMasterClient	)
	//{
		if(	dotamcl != null	)
		{
			photonView.RPC("RPCSetHealthAsID",PhotonTargets.AllBuffered,dotamcl.Health);		
		}
		else
		{
			photonView.RPC("RPCSetHealthAsID",PhotonTargets.AllBuffered,Status.Health);		 	
		}
	//}
}

var health : int = 100;
private var photonView : PhotonView;

@RPC
function	RPCSetHealthAsID(hp : int)
{
	//if(PhotonNetwork.connected && PhotonNetwork.isMasterClient && !PhotonNetwork.offlineMode){}	
	health = hp;
}

function	SetServerBossHP(hp : int)
{
	//Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~传入HP = " + hp );
	Status.Health = hp.ToString();
	if(	Application.loadedLevelName == "Map911"	)
	{
		Status.Maxhealth = UIControl.ActivityBossHPMax.ToString();	
	}
	else
	if(	Application.loadedLevelName == "Map912"	)
	{
		Status.Maxhealth = UIControl.ActivityBossHPMax.ToString();	
	}
	else
	{
		Status.Maxhealth = "200000";			
	}
	if(	parseInt(	Status.Health	) <= 0 && ! Status.dead	)
	{
		Status.RPCDie();
	}
}
