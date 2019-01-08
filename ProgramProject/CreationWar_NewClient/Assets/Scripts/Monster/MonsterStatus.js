#pragma strict
var	monsterLevel : MonsterLEVEL;   //		RAID = 0,BIGBOSS = 1,BOSS = 2,Elite = 3,Monster = 4,PET = 5
var	monsterType :MonsterType;//		pro = 0,strong = 1,rogue = 2,magic = 3
var monsterMapID:int;       //01yuanshi 2,3
//var canControl = false;
var	PlayerID	=	0;	//实例ID,现在使用PhotonViewID//
var Name	=	"Pet";
var Level = 1;
var ProID = 1;
var ATK = 20;
var Attackspeed =1.0;
var Defense = 0;
var Resist = 0;
var Health = "0";
var Maxhealth ="200";
var Mana = 0;
var MaxMana =0;
var Pettime =0;
var skillCD = 10;
var expTexture : Texture[];

var beginSound	: AudioClip;
var attackSound	: AudioClip;
var hitSound	: AudioClip;
var deadSound	: AudioClip;
var BossVoice	: AudioClip;
var CanBreak = 0;
var dead = false;
var Unableattack=0;

private var target : Transform;
private var relativePlayerPosition	=	Vector3(0,0,0);
private var targetDirection			=	Vector3(0,0,-1);
private var pnumbers	: int[];	//副本玩家实例ID数组，用于记录当前的仇恨列表，最多5人//
private var hatreds		: int[];	//仇恨列表//
private var Enemytag	=	"Player";
//private var photonView	: PhotonView;
private var damageT : int;
private var m_Mat		:	Material;

private	var	MNetView	:	MonsterNetView;

function	Awake ()
{
	if(	Application.loadedLevelName	==	"Map911"	&&	gameObject.name.Substring(0,5) == "57100"	)
	{
		gameObject.AddComponent(BattlefieldCityItem);
	}
	if(	Application.loadedLevelName	==	"Map912"	&&	gameObject.name.Substring(0,5) == "58500"	)
	{
		gameObject.AddComponent(BattlefieldCityItem);
	}
	Unableattack	=	100;	//可吸收伤害次数//
	if(	this.CompareTag ("Enemy"))
		Enemytag = "Player";
	else if(this.CompareTag ("Player"))
		Enemytag = "Enemy";
	else if(this.CompareTag ("Neutral"))
		Enemytag = "Player";
	pnumbers	=	new Array (5);
	hatreds		=	new Array (5);								
	//photonView	=	GetComponent.<PhotonView> ();
	Character	=	GetComponent(CharacterController);
	var bodymesh=	GetComponentInChildren (SkinnedMeshRenderer);
	m_Mat		=	bodymesh.renderer.material;
}
///如果是召唤兽的话，则相应召唤时服务器的修改//
function	AcceptSummon(	SData : String	)
{
//	KDebug.Log(	"~ ~ ~ 收到骷髅信息 ~ ~ ~ 收到骷髅信息 ~ ~ ~", transform, Color.red	);
	MNetView		=	gameObject.GetComponent( MonsterNetView );
	if(	MNetView	==	null	)
		MNetView	=	gameObject.AddComponent( MonsterNetView );
	if(	MNetView	!=	null	)
	{
		if(	MNetView.MonsterID	==	0	)
		{
			MNetView.MonsterID	=	parseInt( SData );
			MonsterHandler.GetInstance().RegisterNewSkull(	MNetView	);
		}
	}
	PlayerID						=	MNetView.MonsterID;
	var	Summanner	:	GameObject	=	ObjectFinder.FindTargetGameObject( MNetView.SummonerID );
	if( Summanner	!=	null )
	{
		tag	=	Summanner.tag;
//		if(!PlayerUtil.isMine(MNetView.SummonerID)){
//			Summanner.SendMessage("AddMyPet" , gameObject , SendMessageOptions.DontRequireReceiver);	
//		}
	}
	var _MAI : MonsterAI = GetComponent( MonsterAI );
	if( _MAI != null )
	{
//		KDebug.Log(	"最终~ ~ ~ 收到骷髅信息 ~ ~ ~ 收到骷髅信息 ~ ~ ~最终", transform, Color.red	);
		_MAI.setenemytag();
	}
	//setenemytag
}

function	Start ()
{
	MNetView	=	GetComponent(MonsterNetView);
	if( UIControl.mapType == MapType.zhucheng )
	{
		Destroy(gameObject);
	}

	//怪物实例ID//
	PlayerID	=	MNetView.MonsterID;	//
	Canattack();
	cleanHatreda();
    temppower = DungeonControl.level;
	yield	WaitForSeconds( 1 );
	if(	Pettime	!=	0	)
	{
		yield	WaitForSeconds( Pettime - 3 );
		if(PlayerUtil.isMine(GetComponent(Skillobject).PlayerID)){
			Die2();
			MonsterServerRequest.SkullRemove(GetComponent(MonsterNetView).MonsterID);
		}
	}
//	else
//	{
//		yield	WaitForSeconds(1);
//		if(	Maxhealth == "200"	)	///怪物创建时，如果不是主客户端，则先同步数据//
//			photonView.RPC("Readnowattr",PhotonTargets.All);	
//	}
	if(	Application.loadedLevelName == "Map411" && ( gameObject.name.Substring(0,5) == "56700" || gameObject.name.Substring(0,5) == "56800" ) )
	{	//欧诺城boss//
		CurrentMonsterType	=	E_MonsterType.PVPBoss;
	}
	else
	if(	Application.loadedLevelName == "Map911" && gameObject.name.Substring( 0, 5 ) == "57100"	)
	{	//世界boss//
		CurrentMonsterType	=	E_MonsterType.WorldBoss;
	}
	else
	if(	Application.loadedLevelName == "Map912" && gameObject.name.Substring( 0, 5 ) == "58500"	)
	{	//世界boss//
		CurrentMonsterType	=	E_MonsterType.WorldBoss;
	}

}

enum	E_MonsterType
{
	Normal		=	0,
	PVPBoss		=	1,
	WorldBoss	=	2,
}
private	var	CurrentMonsterType	:	E_MonsterType	=	E_MonsterType.Normal;


private var playernum = 0;		//怪物刚刚创建时的玩家数量//
private var addhealth = 100;	//改变怪物血量的参数，用于计算最终经验获得//
//Old初始化怪物的属性//
//@RPC	
//function	SetMonsterStatus(	att : int[],	name:String	)
//{
//    monsterLevel	=	att[0];
//    monsterType		=	att[1];
//    monsterMapID	=	att[2];
//	Name			=	name;
//	var	yy	=	transform.localScale.y;
//	if(	PhotonNetwork.room.playerCount	)
//		playernum	=	PhotonNetwork.room.playerCount;
//	Level	=	DungeonControl.level + att[3]-2;   
//	if(	Level < 1	)
//		Level = 1;
// 
//	switch (monsterLevel)
//	{   
//		case 0: 
//			transform.localScale =Vector3(yy*2,yy*2,yy*2);
//			addhealth = 4000;
//			break; 
//		case 1:  
//			transform.localScale =Vector3(yy*1.8,yy*1.8,yy*1.8);
//			addhealth = 2000;
//			break; 
//		case 2: 
//			transform.localScale =Vector3(yy*1.6,yy*1.6,yy*1.6);
//			if(Level<=20)
//				addhealth = 500+Level*20;
//			else if(Level<=30)
//				addhealth = 1000;
//			else
//				addhealth = 1200;
//			break;
//		case 3: 
//			transform.localScale =Vector3(yy*1.3,yy*1.3,yy*1.3);   
//			addhealth = 200;
//			break;
//		case 5: 
//			transform.localScale =Vector3(yy*0.8,yy*0.8,yy*0.8);
//			addhealth=60;
//			break; 
//	}
//	addhealth	*=	playernum*BtnGameManager.dicClientParms["ClientParms3"]*0.01;
//	var	addDamege	=	(playernum*0.2+0.6)*BtnGameManager.dicClientParms["ClientParms1"]*0.01;
//	if( DungeonControl.NowMapLevel == 5)
//	{
//		addhealth *= 4;
//		addDamege *= 3;
//	}
//	var	mhealth = 100;
//	//var getExpint = addhealth*(Level*8*(6 - getMonsterLevel())+ 6*Level)/(playernum+2)*BtnGameManager.dicClientParms["ClientParms2"]*0.01;
//	switch (	monsterType	)
//	{
//		case 0: 
//		case 1:  
//			mhealth =	addhealth*( Level*348 - Level*getMonsterLevel()*50+50)*0.01;	
//			ATK = addDamege*(Level*18 - getMonsterLevel()*Level*3 + Random.Range(0,Level));
//			Defense = 1200*(6 -getMonsterLevel() );
//			Resist = 300*(6 -getMonsterLevel() );
//			MaxMana = 100;
//			Mana = 30;
//			skillCD = 6;
//			ProID = 1;
//			break; 
//      
//		case 2: 
//			mhealth =	addhealth*( Level*308 - Level*getMonsterLevel()*45+20)*0.01;
//			ATK = addDamege*(Level*24 +2 - getMonsterLevel()*Level*4+ Random.Range(0,Level));
//			Defense = 200*(6 -getMonsterLevel() );
//			Resist = 500*(6 -getMonsterLevel() );
//			MaxMana = 100;
//			Mana = 0;
//			skillCD = 6;
//			InvokeRepeating("Retriemana", 1, 2);
//			ProID = 2;
//			break;
//     
//		case 3: 
//			mhealth =	addhealth*( Level*282 - Level*getMonsterLevel()*45+20)*0.01;
//			ATK = addDamege*(Level*23 +2 - getMonsterLevel()*Level*4+ Random.Range(0,Level));
//			Defense = 200*(6 -getMonsterLevel() );
//			Resist = 1200*(6 -getMonsterLevel() );
//			MaxMana = Level*202 - getMonsterLevel()*35;
//			Mana = MaxMana;
//			skillCD = 8;
//			InvokeRepeating("Retriemana", 1, 5);
//			ProID = 3;
//			break;        
//	} 
//	ChangeTexture(	monsterMapID	);
//	if(	getMonsterLevel()	<	3	)
//		m_Mat.SetFloat(	"_RimPower",	1.5	);
//	Maxhealth	=	mhealth.ToString();
//	Health		=	Maxhealth;
//}

//Used初始化数值//
function	SetMonsterStatus(	att : int[],	name:String,	_PlayerNum	:int, RandLevel:int	)
{
    monsterLevel	=	att[0];
    monsterType		=	att[1];
    monsterMapID	=	att[2];
	Name			=	name;
	var	yy	=	transform.localScale.y;
	Level	=	DungeonControl.level + att[3]-2;   
	if(	Level < 1	)
		Level = 1;
	playernum	=	_PlayerNum;
	var	addDamege = 1.0;
	switch (monsterLevel)
	{   
		case 0: 
			transform.localScale =Vector3(yy*2,yy*2,yy*2);
			addhealth = 4000;
			break; 
		case 1:  
			transform.localScale =Vector3(yy*1.8,yy*1.8,yy*1.8);
			addhealth = 2500;
		    if(	Application.loadedLevelName == "Map200"	)	
			addhealth = 1200;
			break; 
		case 2: 
			transform.localScale =Vector3(yy*1.6,yy*1.6,yy*1.6);
			if(Level<=20)
				addhealth = 700+Level*15;
			else if(Level<=30)
				addhealth = 1200;
			else if(Level<=40)
				addhealth = 1500;
			else
				addhealth = 1800;
			if(	Application.loadedLevelName == "Map200"	)	
				addhealth = 800;
			break;
		case 3: 
			transform.localScale =Vector3(yy*1.3,yy*1.3,yy*1.3);   
			addhealth = 200;
			break; 
		case 4: 
			if(Level<=20)
				addhealth = 100;
			else if(Level<=30)
				addhealth = 120;
			else if(Level<=40)
				addhealth = 130;
			else
				addhealth = 150;
			if(	Application.loadedLevelName == "Map200"	)	
				addhealth = 80;
			break; 
		case 5: 
			transform.localScale =Vector3(yy*0.8,yy*0.8,yy*0.8);
			addhealth=60;
			break; 
	}
				if(Level<=12)
				addDamege = 1.0;
			else if(Level<=25)
				addDamege = 1.4;
			else if(Level<=40)
				addDamege = 1.8;
			else if(Level<=60)
				addDamege = 2.2;
			else if(Level<=80)
				addDamege = 2.5;				
			if(	Application.loadedLevelName == "Map200"	)	
				addDamege = 1.0;
				
	addhealth *= _PlayerNum*BtnGameManager.dicClientParms["ClientParms3"]*0.01;
	addDamege *= (_PlayerNum*0.2+0.6)*BtnGameManager.dicClientParms["ClientParms1"]*0.01;
	if( DungeonControl.NowMapLevel == 5)
	{
		addhealth *= 4;
		addDamege *= 2;
	}
	var	mhealth = 100;
	switch (	monsterType	)
	{
		case 0: 
		case 1:  
			mhealth =	addhealth*( Level*348 - Level*getMonsterLevel()*50+50)*0.01;	
			ATK = addDamege*(Level*18 - getMonsterLevel()*Level*3 + RandLevel * Level * 0.1 +4 )+6;
			Defense = 1200*(6 -getMonsterLevel() );
			Resist = 300*(6 -getMonsterLevel() );
			MaxMana = 100;
			Mana = 30;
			skillCD = 6;
			ProID = 1;
			break; 
		case 2: 
			mhealth =	addhealth*( Level*308 - Level*getMonsterLevel()*45+20)*0.01;
			ATK = addDamege*(Level*24 +2 - getMonsterLevel()*Level*4+ RandLevel * Level * 0.1 +8 )+12;
			Defense = 200*(6 -getMonsterLevel() );
			Resist = 500*(6 -getMonsterLevel() );
			MaxMana = 100;
			Mana = 0;
			skillCD = 6;
			InvokeRepeating("Retriemana", 1, 2);
			ProID = 2;
			break;
		case 3: 
			mhealth =	addhealth*( Level*282 - Level*getMonsterLevel()*45+20)*0.01;
			ATK = addDamege*(Level*23 +2 - getMonsterLevel()*Level*4+ RandLevel * Level * 0.1 +4)+8;
			Defense = 200*(6 -getMonsterLevel() );
			Resist = 1200*(6 -getMonsterLevel() );
			MaxMana = Level*202 - getMonsterLevel()*35;
			Mana = MaxMana;
			skillCD = 8;
			InvokeRepeating("Retriemana", 1, 5);
			ProID = 3;
			break;        
	} 
	ChangeTexture(	monsterMapID	);
	if(	getMonsterLevel()	<	3	)
		m_Mat.SetFloat(	"_RimPower",	1.0	);
	if(gameObject.name.Length > 5 && gameObject.name.Substring(0,5) == "50200"){
		return;
	}
	if(gameObject.name.Length > 5 && ( gameObject.name.Substring(0,5) == "56700" || gameObject.name.Substring(0,5) == "56800" )){
		Maxhealth	=	"200000";
		Health		=	Maxhealth;
		return;
	}
	Maxhealth	=	mhealth.ToString();
	Health		=	Maxhealth;
}

function SkullMaxHp(hp : int){
		if(gameObject.name.Length > 5 && gameObject.name.Substring(0,5) == "50200"){
			Maxhealth = hp.ToString();
			Health = Maxhealth;
		}
}

///怪物创建时，如果不是主客户端，则先同步数据//
//@RPC
//function	Readnowattr()
//{
//	if(	PhotonNetwork.isMasterClient	)
//	{
//		photonView.RPC("tongbushuxing",name,Level,addhealth,Maxhealth,Health,Defense,Resist,MaxMana,Mana,ProID,monsterLevel);
//	}
//}

//同步时积雪量//
//@RPC
//function	tongbushuxing(	a1:String,	a2:int,	a3:int,	a4:String,	a5:String,	a6:int,	a7:int,	a8:int,	a9:int,	a0:int,	a11:int	)
//{
//	if(!PhotonNetwork.isMasterClient)
//	{
//		name = a1;
//		Level = a2;
//		addhealth =a3;
//		if(Application.loadedLevelName != "Map411" && (gameObject.name.Substring(0,5) != "56700" || gameObject.name.Substring(0,5) != "56800"))
//		{
//
//		}
//		Maxhealth		= a4;
//		Health			= a5;
//		Defense			= a6;
//		Resist			= a7;
//		MaxMana			= a8;
//		Mana			= a9;
//		ProID			= a0;
//		monsterLevel	= a11;
//	}
//}

///持续回蓝//
function	Retriemana()
{
    addmana( 10	); 
}

function	getMonsterLevel() : int
{
	return	monsterLevel;
}

function	Canattack()
{
	yield	WaitForSeconds(1.5);
    Unableattack = 0;	
}
 
private var Character	: CharacterController;	
private var pnumber		: int;
private var damage		: int;
private var Hatred		= 0;
private var Damagetype	= 0;    //0为默认物理伤害，1~4为魔法，1是技能（奥术），2是冰?是火?是暗影，5是毒（自然）  
private var attackerLV	= 1;  //攻击者LV
private var BattleItem	: BattlefieldCityItem;

function	ApplyDamage (info : int[])
{	
	ApplyDamage_(info);
		return ;
//	if(	dead	)
//		return;
//	Unableattack	-=	1;
//	if(	Unableattack	>=	0	)
//	{
//		AllResources.FontpoolStatic.SpawnEffect(11,transform,"吸收!",Character.height*0.5);
//		return;
//	}
//	pnumber		=	info[0];
//    damage		=	info[1];
//    Hatred		=	info[2];
//    Damagetype	=	info[3];
//    attackerLV	=	info[4];      
//	if((Damagetype ==0||Damagetype ==6) && Random.Range(0,100)<5)
//	{
//		AllResources.FontpoolStatic.SpawnEffect(	10,transform,"闪避!",Character.height	);
//		return;
//	}
//	else if((Damagetype !=0||Damagetype !=6)&& Random.Range(0,100)<5)
//	{
//		AllResources.FontpoolStatic.SpawnEffect(9,transform,"偏斜!",Character.height);
//		return;
//	}
//  
//	saveHatred(	info	);		//同步仇恨//
//	
//	target = FindWithID(	pnumber,	Enemytag	); 
//	if(	target	)	//有攻击者//
//	{
//		relativePlayerPosition = transform.InverseTransformPoint(target.position);
//		targetDirection = transform.position - target.position;
//		targetDirection = targetDirection.normalized;
//		photonView.RPC("setDirection", targetDirection);
//		
//		//通知技能击中//
//		target.SendMessage("Hit",transform,SendMessageOptions.DontRequireReceiver);
//		
//		///通知自己被击//
//		SendMessage("Behit",target,SendMessageOptions.DontRequireReceiver);
//		Enemytag = target.tag;
//	}
//    if(	Damagetype==0||Damagetype ==6||Damagetype==1||Damagetype ==7	)
//    {
//		if(Damagetype==0||Damagetype ==6)
//			photonView.RPC("PlayEffect",2);
//		else
//			photonView.RPC("PlayEffect",3);    
//
//		if(	relativePlayerPosition.z < 0 && Random.Range(0,100)<30	)
//		{
//			damageT = damage*(10000-Defense*0.1)/10000;
//			var setArray1 = new int[4];
//            setArray1[0]= pnumber;
//            setArray1[1]= 10;            
//            setArray1[2]= 50; 
//            setArray1[3]= 4;                                						
//			SendMessage("ApplyBuff",setArray1,SendMessageOptions.DontRequireReceiver );
//		}
//		else
//			damageT = damage*(10000-Defense)/10000; 
//    }
//    else
//    {
//		photonView.RPC("PlayEffect",120);
//		damageT = damage*(10000-Resist)/10000;   
//    }
//    //----------------------------------------------------世界boss逻辑----------------------------------------------------//
//	if	(Application.loadedLevelName == "Map411" && (gameObject.name.Substring(0,5) == "56700" || gameObject.name.Substring(0,5) == "56800"))
//	{
//		if(	BattleItem == null)
//		{
//			BattleItem = GetComponent(BattlefieldCityItem);
//		}
//		InRoom.GetInRoomInstantiate().BattlefieldHitBoss(BattleItem.myTeam , damageT);  
//    	return;
//    }
//    else
//	if(	Application.loadedLevelName == "Map718" && gameObject.name.Substring(0,5) == "57100"	)
//	{
//		InRoom.GetInRoomInstantiate().ActivityHitBoss(damageT);  
//    	return;
//    }
//    //----------------------------------------------------世界boss逻辑----------------------------------------------------//
//   
//    photonView.RPC("PlayEffect",0);     
//    
//  
//	Health = (parseInt(Health)- damageT).ToString();
//	
//	if(Damagetype>5)
//	{
//		AllResources.FontpoolStatic.SpawnEffect(12,transform,"暴击!",Character.height);    
//		AllResources.FontpoolStatic.SpawnEffect(4,transform,damageT+"",Character.height);
//	}
//	else
//		AllResources.FontpoolStatic.SpawnEffect(0,transform,damageT+"",Character.height);
//		
//	photonView.RPC("SynHealth",Health);	
//    if(!cbusy && cooldown)
//		photonView.RPC("Changecolor",PhotonTargets.All);
// 
//	if(monsterType == MonsterType.strong)
//		addmana(damageT/parseInt(Maxhealth)*100 +1);     
//	photonView.RPC("hitanimation",PhotonTargets.All);
//	if(	Random.Range(0,10)<5)
//		photonView.RPC("PlayAudio",3);
//	
//	if (	!dead	&&	parseInt(Health) <= 0	)	//没死并且血小于0//
//	{   
//		if(target)
//		{    
//			var setArray = new int[4];
//            setArray[0]= PlayerID;
//            setArray[1]= Level;            
//            setArray[2]= ProID+3; 
//            setArray[3]= 5 - getMonsterLevel(); 
//			target.SendMessage("Kills",setArray, SendMessageOptions.DontRequireReceiver);
//			if(	DungeonControl.Kpower && Application.loadedLevelName != "Map200" )
//			{
//				DungeonControl.Kpower = false;
//				target.SendMessage("AddPower" , Mathf.Clamp(DungeonControl.level, 8 , 20) , SendMessageOptions.DontRequireReceiver);
//			}
//		}
//  		
//  		//Debug.Log("K_________________________Die!!!!!!RPCRequest!");
//    	photonView.RPC("Die",Random.Range(0,100));
//    	UIFindCamera.MonsterDie = true;
//		return;
//	}
}
var	_DamageStr	:	String;
private		var useDam1 : long;
private		var useDam2 : long;
private		var useDam : long;

function	ApplyDamage_ (info : int[])
{	
	//Debug.Log( "击中了，还没计算伤害。" );
	if(	dead	)
	{
		//Debug.Log( "击中了，还没计算伤害。怪已经死了。" );
		return;
	}
	Unableattack	-=1	;
	if(	Unableattack >=	0	)
	{
		AllResources.FontpoolStatic.SpawnEffect(11,transform,"吸收!",Character.height*0.5);
		return;
	}
	pnumber		=	info[0];	//Needed//
    damage		=	info[1];
    Hatred		=	info[2];	//Needed//
    Damagetype	=	info[3];	//Needed//
    attackerLV	=	info[4];
    
    if(	!PlayerUtil.IsLocalObject(pnumber)	)	//只发当前客户端玩家造成的伤害//
    {
    	//Debug.Log( "玩家ID传的不对啊亲！" );
    	//return;
    }
    //闪避、偏侧的话，自己知道就行了//
	if(	(Damagetype ==0||Damagetype ==6) && Random.Range(0,100)<5	)	
	{
		AllResources.FontpoolStatic.SpawnEffect(10,transform,"闪避!",Character.height);	//-------------结果闪避//
		return;
	}
	else if(	(Damagetype !=0||Damagetype !=6)&& Random.Range(0,100)<5	)
	{
		AllResources.FontpoolStatic.SpawnEffect(9,transform,"偏斜!",Character.height);	//-------------结果偏斜//
		return;
	}
	//target	=	ObjectAccessor.getAOIObject(pnumber).transform;
	target	=	FindWithID(	pnumber,	Enemytag	); 
	//target	=	PlayerStatus.MainCharacter;
	if(	target	)	//有攻击者//
	{
		relativePlayerPosition	=	transform.InverseTransformPoint(target.position);
		targetDirection		=	transform.position - target.position;
		targetDirection		=	targetDirection.normalized;
		
		if(	PlayerUtil.IsLocalObject(pnumber)	)
		{
			SendMessage(	"Behit",target,		SendMessageOptions.DontRequireReceiver	);
			target.SendMessage(	"Hit",	transform,	SendMessageOptions.DontRequireReceiver	);
		}
	}
	else
	{
//		Debug.Log("K______________没有找到攻击者！");
	}
	//------------------------------------------------确认命中------------------------------------------------//	
	
	var	IsRand30	:	int	= 0;
	
	if(	Damagetype==0||Damagetype ==6||Damagetype==1||Damagetype ==7	)
    {
    	if(	PlayerUtil.IsLocalObject(pnumber)	)
		{
			if(	Damagetype==0	||	Damagetype ==6	)
				SendMessage("PlayEffect",2);
			else
				SendMessage("PlayEffect",3);    	
		}
	    if(	relativePlayerPosition.z < 0 && Random.Range(0,100)<30	)
		{
			useDam1 = damage;
			useDam2 = Defense;
			useDam = useDam1*(10000-useDam2*0.1);
			damageT	=	useDam/10000;
			IsRand30	=	1;
		}
		else
		{
			useDam1 = damage;
			useDam2 = Defense;
			useDam = useDam1*(10000-useDam2);
			damageT = useDam/10000; 
			IsRand30	=	0;
		}
    }
    else
    {
    	if(	PlayerUtil.IsLocalObject(pnumber)	)
		{
			SendMessage(	"PlayEffect",	120	);
		}
		useDam1 = damage;
		useDam2 = Resist;
		useDam = useDam1*(10000-useDam2);
    	damageT = useDam/10000;
    }
	if(	PlayerUtil.IsLocalObject(pnumber)	)
	{
		SendMessage(	"PlayEffect",	0	);
		if(	Damagetype	>	5	)	//是否暴击显示//
		{
			AllResources.FontpoolStatic.SpawnEffect(12,transform,"暴击!",Character.height);    
			AllResources.FontpoolStatic.SpawnEffect(4,transform,"-"+damageT,Character.height);
			PlayAudio(	3	);
		}
		else
		{
			AllResources.FontpoolStatic.SpawnEffect(0,transform,"-"+damageT,Character.height);
		}
		if(	!cbusy )
			Changecolor();
		hitanimation();

	}
    //----------------------------------------------------世界boss逻辑----------------------------------------------------//
	if(	Application.loadedLevelName == "Map411" && ( gameObject.name.Substring(0,5) == "56700" || gameObject.name.Substring(0,5) == "56800" ) )
	{	//欧诺城boss//
		if(	BattleItem	==	null	)
		{
			BattleItem	=	GetComponent( BattlefieldCityItem );
		}
		InRoom.GetInRoomInstantiate().BattlefieldHitBoss( BattleItem.myTeam , damageT );
		_DamageStr	=	"";
	  	_DamageStr	+=	pnumber;		//i
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	Hatred;			//i
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	Damagetype;		//i
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	IsRand30;		//i
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	0;		//f战场boss不传伤害//
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	0;		//i  = rttf战场boss不爆东西、、//
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	0;		//i	 = RandRewardf战场boss不爆东西//
	  	_DamageStr	+=	",";
	  	_DamageStr	+=	Enemytag;			//
	  	InRoom.GetInRoomInstantiate().MonsterHateList(PlayerID , pnumber , Hatred);
		//KDebug.Log("~ " + damageT + "~~~~~~~~~~~~~~~~~~~~~~~~~~", transform, Color.red );
		MonsterServerRequest.MonsterDamaged(	MNetView.MonsterID,	damageT,	_DamageStr	);
    	return;
    }
    else
	if(	Application.loadedLevelName == "Map911" && gameObject.name.Substring( 0, 5 ) == "57100"	)
	{	//世界boss//
		InRoom.GetInRoomInstantiate().ActivityHitBoss(	damageT	);
		WorldBossAcceptDamage(	pnumber, Hatred, Damagetype, IsRand30 );
    	return;
    }
    else
	if(	Application.loadedLevelName == "Map912" && gameObject.name.Substring( 0, 5 ) == "58500"	)
	{	//世界boss//
		InRoom.GetInRoomInstantiate().ActivityHitBoss(	damageT	);
		WorldBossAcceptDamage(	pnumber, Hatred, Damagetype, IsRand30 );
    	return;
    }
    //----------------------------------------------------世界boss逻辑----------------------------------------------------//
    ///----
  	_DamageStr	=	"";
  	_DamageStr	+=	pnumber;		//i
  	_DamageStr	+=	",";
  	_DamageStr	+=	Hatred;			//i
  	_DamageStr	+=	",";
  	_DamageStr	+=	Damagetype;		//i
  	_DamageStr	+=	",";
  	_DamageStr	+=	IsRand30;		//i
  	_DamageStr	+=	",";
  	_DamageStr	+=	damageT;		//f
  	_DamageStr	+=	",";
  	_DamageStr	+=	Random.Range(0,100);//i  = rtt
  	_DamageStr	+=	",";
  	_DamageStr	+=	Random.Range(0,3);	//i	 = RandReward
  	_DamageStr	+=	",";
  	_DamageStr	+=	Enemytag;			//
//	KDebug.Log("~ " + damageT + "~~~~~~~~~~~~~~~~~~~~~~~~~~MNetView.MonsterID = " + MNetView.MonsterID, transform, Color.red );
	MonsterServerRequest.MonsterDamaged(	MNetView.MonsterID,	damageT,	_DamageStr	);
}
var	Acc_DamageStr	:	String[];
//收到伤害结果//
function	OnAcceptDamage(	DamageData:String[]	)
{
	//Debug.Log( "K___________________________收到伤害消息" );PVPBoss
	if(	CurrentMonsterType	==	E_MonsterType.WorldBoss	)
		return;
//	KDebug.Log("~~~~~~~~~~HP = " + Health + "~~~~~~~~~~~~~~~~~~ + ID = " + MNetView.MonsterID, transform, Color.blue );
    //Debug.Log("K_______________________NewHealth = " + Health);
    Health	=	DamageData[0];	///生命同步//
	if(DamageData[1]=="")
	{
	  return;
	}
	
	Acc_DamageStr	=	DamageData[1].Split(	",".ToCharArray()	);
	//;			//_DamageStr数据//
	var _pnumber:int		=	parseInt(Acc_DamageStr[0]);
	var _Hatred	:int		=	parseInt(Acc_DamageStr[1]);
	var _Damagetype:int		=	parseInt(Acc_DamageStr[2]);
	var IsRand30:int		=	parseInt(Acc_DamageStr[3]);
	var _damageT:int		=	parseFloat(Acc_DamageStr[4]);
	var _rtt 	: int		=	parseInt(Acc_DamageStr[5]);
	var _RandReward:int		=	parseInt(Acc_DamageStr[6]);
	var	_EnermyTag:String	=	Acc_DamageStr[7];
	
	if(	CurrentMonsterType	==	E_MonsterType.PVPBoss	)
	{
		PVPBossAcceptDamage(	_pnumber,	_Hatred,	_Damagetype,	IsRand30	);
		return;
	}
	
	
	target	=	FindWithID(	_pnumber,	_EnermyTag	);
	if(	target	)	//有攻击者//
	{
		//PlayerUtil.isMain(pnumber)
		targetDirection		=	transform.position - target.position;
		targetDirection		=	targetDirection.normalized;
		//setDirection(	targetDirection	);
		//通知技能击中//
		SaveHatred(	_pnumber,	_Hatred	);
		if(	!PlayerUtil.IsLocalObject(_pnumber)	)
		{
			SendMessage(	"Behit",target,		SendMessageOptions.DontRequireReceiver	);
			target.SendMessage(	"Hit",	transform,	SendMessageOptions.DontRequireReceiver	);
		}
		Enemytag	=	target.tag;
	}
	//else
	//	Debug.Log("KKK______________________没找到攻击者");
	
	if(	_Damagetype==0||_Damagetype ==6||_Damagetype==1||_Damagetype ==7	)
    {
    	if(	!PlayerUtil.IsLocalObject(_pnumber)	)
		{
			if(	_Damagetype==0	||	_Damagetype ==6	)
				SendMessage("PlayEffect",2);
			else
				SendMessage("PlayEffect",3);    	
		}
		
		if( IsRand30	==	1	)
		{	
			var setArray1 = new int[4];
            setArray1[0]= _pnumber;
            setArray1[1]= 10;            //BuffID
            setArray1[2]= 50; 			//Value
            setArray1[3]= 4;			//Time             						
			SendMessage("ApplyBuff",setArray1,SendMessageOptions.DontRequireReceiver );
		}
    }
    else
    {
    	if(	!PlayerUtil.IsLocalObject(_pnumber)	)
		{
			SendMessage(	"PlayEffect",	120	);
		}
    }
	
	////伤害显示//
	if(	!PlayerUtil.IsLocalObject(_pnumber)	)
	{
		SendMessage(	"PlayEffect",	0	);
		if(	_Damagetype	>	5	)	//是否暴击显示//
		{
			AllResources.FontpoolStatic.SpawnEffect(12,transform,"暴击!",Character.height);    
			AllResources.FontpoolStatic.SpawnEffect(4,transform,"-"+_damageT,Character.height);
		}
		else
		{
			AllResources.FontpoolStatic.SpawnEffect(0,transform,"-"+_damageT,Character.height);
		}
		if(	!cbusy	)
			Changecolor();
		hitanimation();
		if(	Random.Range( 0, 10 )	<	5	)
			SendMessage( "PlayAudio", 3 );
	}
	if(	monsterType == MonsterType.strong	)	//强类似怪物受攻击之后
		addmana(damageT/parseInt(Maxhealth)*100 +1);     
	//-------------------------------------------------------------判定死亡-------------------------------------------------------------//
	if ( !dead	&&	parseInt(Health) <= 0	)	//没死并且血小于0//
	{   
		if(target)
		{    
			var setArray = new int[4];
            setArray[0]= PlayerID;
            setArray[1]= Level;            
            setArray[2]= ProID+3; 
            setArray[3]= 5 - getMonsterLevel(); 
			target.SendMessage("Kills",setArray, SendMessageOptions.DontRequireReceiver);
			if(DungeonControl.Kpower && Application.loadedLevelName != "Map200")
			{
				DungeonControl.Kpower = false;
				if(UIControl.mapType != MapType.jingjichang && Application.loadedLevelName != "Map721"){
					if(DungeonControl.NowMapLevel == 5){
						if( Application.loadedLevelName.Length > 5 && Application.loadedLevelName.Substring(5,1) == "1" )
							InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.EliteDungeon , temppower , 0 , "");
//							target.SendMessage("AddPower" , Mathf.Clamp(parseInt(temppower*0.5+30), 30 , 60) , SendMessageOptions.DontRequireReceiver);					
					}else{
						InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.NormalDungeon  , temppower , 0 , "");
//						target.SendMessage("AddPower" , Mathf.Clamp(parseInt(temppower*0.34+10), 15 , 30) , SendMessageOptions.DontRequireReceiver);		
					}
				}
			}
		}
		//---------------------------------------死亡必要参数----------------------------------//
    	Die( _rtt, _RandReward );
    	UIFindCamera.MonsterDie = true;
		return;
	}
}
private var temppower = 1.0;

function	WorldBossAcceptDamage(	_pnumber	:int,	_Hatred		:int,	_Damagetype	:int,	IsRand30	:int	)
{
	target	=	FindWithID(	_pnumber,	Enemytag	);
	if(	target	)	//有攻击者//
	{
		targetDirection		=	transform.position - target.position;
		targetDirection		=	targetDirection.normalized;
		//通知技能击中//
		SaveHatred(	_pnumber,	_Hatred	);
		Enemytag	=	target.tag;
	}
	if(	_Damagetype==0||_Damagetype ==6||_Damagetype==1||_Damagetype ==7	)
    {
    	if(	!PlayerUtil.IsLocalObject(_pnumber)	)
		{
			if(	_Damagetype==0	||	_Damagetype ==6	)
				SendMessage("PlayEffect",2);
			else
				SendMessage("PlayEffect",3);    	
		}
		if( IsRand30	==	1	)
		{	
			var setArray1 = new int[4];
            setArray1[0]= _pnumber;
            setArray1[1]= 10;            //BuffID
            setArray1[2]= 50; 			//Value
            setArray1[3]= 4;			//Time             						
			SendMessage("ApplyBuff",setArray1,SendMessageOptions.DontRequireReceiver );
		}
    }
}
function	PVPBossAcceptDamage(	_pnumber	:int,	_Hatred		:int,	_Damagetype	:int,	IsRand30	:int	)
{
	target	=	FindWithID(	_pnumber,	Enemytag	);
	if(	target	)	//有攻击者//
	{
		targetDirection		=	transform.position - target.position;
		targetDirection		=	targetDirection.normalized;
		//通知技能击中//
		SaveHatred(	_pnumber,	_Hatred	);
		Enemytag	=	target.tag;
	}
	if(	_Damagetype==0||_Damagetype ==6||_Damagetype==1||_Damagetype ==7	)
    {
    	if(	!PlayerUtil.IsLocalObject(_pnumber)	)
		{
			if(	_Damagetype==0	||	_Damagetype ==6	)
				SendMessage("PlayEffect",2);
			else
				SendMessage("PlayEffect",3);    	
		}
		if( IsRand30	==	1	)
		{	
			var setArray1 = new int[4];
            setArray1[0]= _pnumber;
            setArray1[1]= 10;            //BuffID
            setArray1[2]= 50; 			//Value
            setArray1[3]= 4;			//Time             						
			SendMessage("ApplyBuff",setArray1,SendMessageOptions.DontRequireReceiver );
		}
    }
}

function	OnAcceptDeath(	RewardData:int[]	)
{
	//KDebug.Log("$缓伤死亡￥消息!Getted!",transform,Color.yellow);
	Die( RewardData[0], RewardData[1] );
}
//玩家获得经验//
//获得经验int1: AddHealth 血量加成; int2: Level 属性等级; int3:MonsterLevel 怪物难度级别; int4: playernum创建时人数//
function	SendExp(int1:int,int2:int,int3:int,int4:int)
{
	var	colliders : Collider[]	=	Physics.OverlapSphere ( transform.position, 30);
	var setArray = new int[4];
	setArray[0]	=	int1;
	setArray[1]	=	int2;            
	setArray[2]	=	int3;
	setArray[3]	=	int4;	
	for(	var	hit	in	colliders	)
	{
		if(	hit.CompareTag ("Player"))
    		hit.SendMessage("AddExperienceF", setArray,SendMessageOptions.DontRequireReceiver);
	}
}

var showtask	:	boolean = false;
var task	:	MainTask = null;
var mtw		:	MainTaskWork;
function	SetTask(g : GameValue)
{
	showtask	=	true;
	task		=	g.task;
	mtw			=	g.mtw;
}

//used显示任务，打开//
function	isShowTask()
{
	showtask = true;
}
//怪物出生点//
private	var	MonsterSp	:	MonsterSpawn;
//Old
//function SetMonsterSp(ID : int)
//{
//	photonView.RPC("SetRemote",PhotonTargets.AllBuffered);
//	photonView.RPC("SetMonsterSpp",PhotonTargets.AllBuffered , ID);
//}
///替代SetMonsterSp在初始化时赋值//
function	InitOnSpawn(	_monsterSpawn	:	MonsterSpawn	)
{
	GetComponent(MonsterAI).enabled		=	true;
	GetComponent(NavMeshAgent).enabled	=	false;
	MonsterSp							=	_monsterSpawn;
}
 
//@RPC
//function	SetMonsterSpp(ID : int)
//{
//	var pv	=	photonView.Find(ID);
//	if(pv)
//		MonsterSp =pv.GetComponent(MonsterSpawn);
//}

var MonsterPrd : GameObject;	//任务怪出生点//
//used任务中设置出生点//
function	SetMonsterPrd(prd : GameObject)
{
	MonsterPrd	=	prd;
}

////Old死亡效果结果//
//@RPC
//function	Die (rtt:int)
//{	
//	Debug.Log(	"K_________________Die__RPC Act!!!"	);
//	if(	Application.loadedLevelName != "Map200"	)
//	{	//获得经验//
//		SendExp(addhealth,Level,getMonsterLevel(),playernum);	
//	}
//	BTMode = false;
//	
//	//--------如果这个怪有任务--------//
//	if(	mtw	)	
//	{
//		mtw.TaskInfoValue(task,TaskValueType.monster);
//	}
//	if(	MonsterSp	)
//		MonsterSp.TBC();
//	if(	MonsterPrd != null)
//	{
//		MonsterPrd.SendMessage("MonsterDie",SendMessageOptions.DontRequireReceiver);
//	}
//	//--------如果这个怪有任务--------//
//    dead	=	true;
//    Health="0";
//    if(PhotonNetwork.isMasterClient)
//    {
//		photonView.RPC("PlayEffect",107);
//		photonView.RPC("PlayEffect",121);    
//		photonView.RPC("PlayEffect",1);	
//		photonView.RPC("PlayAudio",4);
//
//		GetComponent(NavMeshAgent).enabled = false; 
//	}
//	//解除锁定//
//	if(	qiuai.objs == this.transform	)
//		qiuai.objs = null;
//	animation.Play("die",PlayMode.StopAll);
//	
//	if(	rtt>50	)	//爆战利品----------随机数判定----------//
//		AllResources.PickpoolStatic.SpawnPickup( Random.Range(0,3), transform );
// //   if(getMonsterLevel()<3 && UIControl.mapType == MapType.fuben && Application.loadedLevelName != "Map200")
// //   AllResources.PickpoolStatic.SpawnPickup(4,transform);
// 
// 	//通知敌人（怪物的敌人是玩家）怪死了，产生一些技能的效果//
//	var	gos : GameObject[];
//	gos	= GameObject.FindGameObjectsWithTag(Enemytag); 
//	for( var go : GameObject in gos )
//	{
//		go.SendMessage("MonsterDie", SendMessageOptions.DontRequireReceiver);    
//	}
//	
//	//消除存在感......//
//	this.tag	=	"Ground";
//	
//	if(!Canrespawn && gameObject && PhotonNetwork.isMasterClient)
//	{
//		if(	CanBreak	>	0 && rtt	<=	50	)
//		{
//			photonView.RPC(	"PlayEffect",121+CanBreak	); 
//			try
//			{  
//					PhotonNetwork.Destroy(gameObject);      
//			}
//			catch(e)
//			{    }         
//		}   
//		else
//		{
//			yield	WaitForSeconds(3);
//			SendMessage( "Burn" ); 
//		}
//	}
//}

//new死亡效果结果rtt(0,100),RewardRand(0,3)//
function	Die (	rtt:int,	RewardRand: int )
{	
	if(	Application.loadedLevelName != "Map200"	)
	{	//获得经验//
		SendExp(addhealth,Level,getMonsterLevel(),playernum);	
	}
	BTMode	=	false;
	//--------如果这个怪有任务--------//
	if(	mtw	)	
	{
		mtw.TaskInfoValue(task,TaskValueType.monster);
	}
	if(	MonsterSp	)
		MonsterSp.TBC(transform);	//告诉生成点，怪物死了//
	if(	MonsterPrd != null)
	{
		MonsterPrd.SendMessage("MonsterDie",SendMessageOptions.DontRequireReceiver);
	}
	//--------如果这个怪有任务--------//
    dead	=	true;
    Health	=	"0";
	SendMessage(	"PlayEffect",	107	);
	SendMessage(	"PlayEffect",	121	);    
	SendMessage(	"PlayEffect",	1	);	
	PlayAudio(	4	);
	GetComponent(NavMeshAgent).enabled = false; 
	//解除锁定//
	if(	qiuai.objs	==	this.transform	)
		qiuai.objs	=	null;
	animation.Play("die",PlayMode.StopAll);
	if (getMonsterLevel()	<	3)  //慢动作！//
		AllResources.ar.Playtimeslow();
	if(	rtt	>	50	)	//爆战利品----------随机数判定----------//
		AllResources.PickpoolStatic.SpawnPickup( RewardRand, transform );
 
 	//通知敌人（怪物的敌人是玩家）怪死了，产生一些技能的效果//
 	if(	PlayerStatus.MainCharacter	!=	null	&&	PlayerStatus.MainCharacter.tag	==	Enemytag	)
 	{
 		PlayerStatus.MainCharacter.SendMessage(	"MonsterDie",	SendMessageOptions.DontRequireReceiver	); 
 	}
	//消除存在感......//
	
	if(GetComponent(PetNetInit) != null){
		MonsterServerRequest.SkullRemove(GetComponent(MonsterNetView).MonsterID);
	}
	this.tag	=	"Ground";
	if(	!Canrespawn	&&	gameObject	)
	{
		if(	CanBreak	>	0	&&	rtt	<=	50	)
		{
			SendMessage(	"PlayEffect",	121	+	CanBreak	); 
			try
			{  
				Destroy(gameObject);      
			}
			catch(e)
			{    }         
		}   
		else
		{
			yield	WaitForSeconds(3);
			Burn(); 
		}
	}
}

private var Canrespawn = false;
//@RPC
//function	Respawn()
//{
//	if(	Enemytag	==	"Player"	)
//		this.tag	=	"Enemy";
//	else	if(	Enemytag	==	"Enemy"	)
//		this.tag	=	"Player";
//
//	GetComponent(NavMeshAgent).enabled = true; 
//	dead	=	false;
//	Health	=	Maxhealth;
//	animation.Play(	"idle",	PlayMode.StopAll	);
//}

//@RPC
function Die2 ()
{	
    dead	=	true;
    this.tag	=	"Ground";
 
	PlayAudio(	4	);
	GetComponent(NavMeshAgent).enabled	=	false; 
	if(	qiuai.objs	==	this.transform	)
		qiuai.objs	=	null;
	animation.Play(	"die",	PlayMode.StopAll	);
	yield	WaitForSeconds( 3 );
	try
	{
		Destroy( gameObject );      
    }
    catch( e ){}
}

//怪物燃烧然后消失//
//@RPC
function	Burn()
{
	m_Mat.shader	=	Shader.Find("Dissolve/Dissolve_TexturCoords");
	m_Mat.SetColor("_DissColor",Color(1, 0.2, 0, 1));
	m_Mat.SetTexture ("_DissolveSrc",AllManage.NoisemapStatic);
	var m_fTime = 0.0;
	SendMessage("PlayEffect",164); 
	while( m_fTime<=1.0 )
	{
		m_Mat.SetFloat("_Amount", m_fTime);
		m_fTime += Time.deltaTime;
		yield;
	}
	try
	{
		Destroy(gameObject);      
    }
    catch(e){}	 
}

//治疗//
//function	AddHealth( i : int)
//{
//	if(	i	>	0	)	//加血//
//		AllResources.FontpoolStatic.SpawnEffect(1,transform,"+"+i,Character.height*0.5);
//	else		//减血//
//		AllResources.FontpoolStatic.SpawnEffect(0,transform,-i +"",Character.height*0.5);
//	var	healths	:	int	=	parseInt(Health);
//	Health	=	(healths + i).ToString();
//	if(	parseInt(Health)	>	parseInt(Maxhealth)	)
//		Health = Maxhealth;
//	else if(	parseInt(Health)<=0	&&	!dead	)
//	{
//		Health = "0";
		//KDebug.Log( "~缓伤死亡~", transform, Color.red );
		//Die (	Random.Range( 0, 100 ),	Random.Range( 0, 3 ) );
//	}
//	photonView.RPC("SynHealth",Health);
//}

//@RPC
//function	SynHealth(P : String)
//{
//	Health	=	P;
//}

private var tcolor : Color;
private var tpower =0.0;
private var cbusy = false;

//更改颜色//
//@RPC
function	Changecolor()
{
	if(	cbusy	)
		return;
	cbusy = true;
	if(	m_Mat.HasProperty("_RimColor")	)
	{
		tcolor = m_Mat.GetColor("_RimColor");
		tpower = m_Mat.GetFloat("_RimPower");
		m_Mat.SetColor("_RimColor",Color(0.3, 0.7, 1, 1));  
		m_Mat.SetFloat("_RimPower",3.0);
		yield;
		yield;
 		m_Mat.SetColor("_RimColor",tcolor);
 		m_Mat.SetFloat("_RimPower",tpower);
	}
	cbusy = false;
	if( Level >= 10 && getMonsterLevel() < 3 && !dead &&parseInt(Health) <= parseInt(Maxhealth)*0.5 && cooldown	)
		StartBTmode();	
}

var BTMode = false;
private var cooldown = true;
var modetime = 0;
function	StartBTmode()
{
	BTMode = true;
	cooldown = false;
	var m_fTime = 15.0;
	var ttint =0.0;
	PlayAudio(5);
	if(m_Mat.HasProperty("_RimColor"))
	{
		var tempcolor = m_Mat.GetColor("_RimColor");
		var temppower = m_Mat.GetFloat("_RimPower");
	}
	var	tempATK	=	ATK;
	switch (modetime)
	{   
		case 0: 
			AllManage.tsStatic.Show("info755");
			m_Mat.SetColor("_RimColor",Color(0.3, 0.7, 1, 1));
			ATK = tempATK + tempATK*0.2;
			break; 
		case 1: 
			AllManage.tsStatic.Show("info756");
			m_Mat.SetColor("_RimColor",Color(1, 0.4, 0, 1));
			ATK = tempATK + tempATK*0.4;
			break;
		case 2: 
			AllManage.tsStatic.Show("info757");
			m_Mat.SetColor("_RimColor",Color(1, 0, 0, 1));
			ATK = tempATK + tempATK*0.5;
			break;
	}
	SendMessage( "PlayEffect", 165 ); 
	while( m_fTime >= 0 )
	{
		ttint  = parseInt(Time.time) +0.5;
		m_Mat.SetFloat("_RimPower", Mathf.Abs(Time.time - ttint)*2+0.5);
		m_fTime -= Time.deltaTime;
		yield;
	}
	BTMode = false;
	if(	m_Mat.HasProperty("_RimColor")	)
	{
		m_Mat.SetColor("_RimColor",tempcolor);
		m_Mat.SetFloat("_RimPower",temppower);	
	} 
	ATK = tempATK;
	modetime += 1;
	if(	modetime >	2	)
		modetime =	0;
	yield	WaitForSeconds(15);

	cooldown = true;
}


function	addmana(P:int)
{
	Mana += P;   
	if(	Mana	>=	MaxMana	)
	{
		Mana =	MaxMana;
	}
	else if(	Mana	<	0	)
	Mana	=	0;
	//photonView.RPC("SynMana",Mana);
}
    
//@RPC
//function	SynMana(P : int){
//Mana = P;
//}

//@RPC
//function SynLevel(P1 : int,P2: int,P3 : String,P4 : int,P5 : int,P6 : int){
//Level = P1;
//addhealth =P2;
//Maxhealth = P3;
//ATK = P4;
//MaxMana = P5;
//Mana= P6;	
//}
private	var _tMS : MonsterStatus;
private	var _tPS : PlayerStatus;

//通过实例ID获取怪或者玩家，这里不能获取已经死亡的单位//
function	FindWithID(	i:int,	tag:String	) : Transform
 {
	var	gos	:	GameObject[];
	gos	=	GameObject.FindGameObjectsWithTag(tag);
	var	diff	=	-2;	//实例ID//
	for(	var go : GameObject in gos) 
	{
		_tPS		=	go.GetComponent(PlayerStatus);
		if(	_tPS	!=	null )
		{
			diff	=	_tPS.instanceID;
			if(	diff	==	i	&&	!_tPS.dead )
			{
				return	go.transform;
			}
		}
		else 
		{	//不是玩家，是怪//
			_tMS	=	go.GetComponent(MonsterStatus);
			if(	_tMS != null	)
			{
				diff	=	_tMS.PlayerID;
				if(	diff	==	i	&&	!_tMS.dead )
				{
					return	go.transform;
				}
			}
		}
    }
	return	null;
}

///找到最高仇恨的敌人//
function	FindHatestEnemy (Enemytag:String) : Transform
{
	var hatredest	=	0;
	var pppnumber	=	-999;
	var diff		=	-2;
	for(var i : int = 0; i < 5; i++) 
	{
		if(hatreds[i]>hatredest)
		{
			hatredest = hatreds[i];
			pppnumber = pnumbers[i];
		}
	}     
	return	FindWithID(pppnumber,Enemytag);
}

///used同步仇恨//
function	saveHatred(info : int[])
{
	SaveHatred(	info[0],	info[2]	);
	//photonView.RPC("saveHatreda",info); 
}

//Old记录仇恨//
//@RPC
//function	saveHatreda(info : int[])
//{
//	var pnumber =	info[0];	//仇恨源ID//
//	var Hatred	=	info[2];	//仇恨//
//	for (var i : int = 0; i < 5; i++)
//	{ 
//		if(	hatreds[i]	==	0	)
//		{
//			pnumbers[i]	=	pnumber;
//			hatreds[i]	=	Hatred;
//			return;
//		}
//		else if(pnumbers[i]==pnumber)
//		{
//			hatreds[i]+=Hatred;   
//		}
//	}
//}

//New	记录仇恨//
function	SaveHatred(	pnumber:int,	Hatred:int	)
{
	InRoom.GetInRoomInstantiate().MonsterHateList(PlayerID , pnumber , Hatred);
	for( var i : int = 0; i < 5; i++ )
	{ 
		if(	hatreds[i]	==	0	)
		{
			pnumbers[i]	=	pnumber;
			hatreds[i]	=	Hatred;
			return;
		}
		else if(pnumbers[i]==pnumber)
		{
			hatreds[i]+=Hatred;   
		}
	}
}

//@RPC
function	hitanimation()
{
	animation.Blend("hit",0.6,0.01);
}

function	EnemyDead(	instanceID : int	)
{
	//Debug.Log( " K_____________----____玩家死了__PlayerDead = " + instanceID );
	removeHatred(	instanceID	);
	var _MAI : MonsterAI = GetComponent( MonsterAI );
	_MAI.OnTargetDead();
}

function	removeHatred(pnumber : int)
{
	//KDebug.Log( "清理玩家的仇恨 + " + pnumber ,transform );
	removeHatreda( pnumber );
}

//@RPC
function	removeHatreda(pnumber : int)
{
	InRoom.GetInRoomInstantiate().RemoveMosterHate(PlayerID , pnumber);  
//	for( var i : int = 0; i < 5; i++ )
//	{ 
//		if( pnumbers[i] == pnumber )
//		{
//			hatreds[i]	=	-1;
//			pnumbers[i]	=	-999;
//			//KDebug.Log( "~ 清理玩家 " + pnumber + "的仇恨",transform );
//			return;
//		}
//	}
}

function ServerRemoveHatreda(objs : Object[]){
	var strs1 : String[];
	var strs2 : String[];
	strs1 = objs[0];
	strs2 = objs[1];
	pnumbers = new int[strs1.length];
	hatreds = new int[strs2.length];
	for(var i=0; i<strs1.length ; i++){
		pnumbers[i] = parseInt(strs1[i]);
		hatreds[i] = parseInt(strs2[i]);	
	}
}

//function	cleanHatred()
//{
//	photonView.RPC("cleanHatreda",PhotonTargets.All);
//}

//Used清理仇恨//
//@RPC
function	cleanHatreda()
{
	InRoom.GetInRoomInstantiate().ClearMonsterHate(PlayerID);
	//Health	=	Maxhealth;
	for( var i : int = 0; i < 5; i++ )
	{
		pnumbers[i]	=	-999;
		hatreds[i]	=	0;
	}
}

//function	hitDirection() : Vector3 
//{
//	return	targetDirection;
//}
private	var	_target:Transform;
function	hitDirection(	AttackerID:int	) : Vector3 
{	
	_target	=	FindWithID(	AttackerID,	Enemytag	); 
	//target	=	PlayerStatus.MainCharacter;
	if(	_target	!=	null	)	//有攻击者//
	{
		//Debug.Log("K_______________________击退获取实际位置  + " + Enemytag + " _ " + AttackerID );
		return	(transform.position - _target.position).normalized;
	}
	else
	{
		//Debug.Log("K_______________________击退获取实际位置！错误！" + Enemytag + " _ " + AttackerID + " _p= " + PlayerStatus.MainCharacter.GetComponent.<PlayerStatus>().instanceID + " Tag = " + PlayerStatus.MainCharacter.tag );
		return	targetDirection;
	}
}

//@RPC
//function	setDirection(Direction : Vector3)
//{
//	targetDirection	=	Direction;
//}

@RPC
function	PlayEffect(i:int)
{
	AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}

@RPC
function	PlayAudio(i:int)
{
	switch (i)
	{				
		case 1:
			if(beginSound)	
				audio.PlayOneShot(beginSound);
			break;
	 
		case 2:	
			if(attackSound)
				audio.PlayOneShot(attackSound);
			break;
	 
		case 3:
			if(hitSound)	
				audio.PlayOneShot(hitSound);
			break;
	 
		case 4:
			if(deadSound)	
				audio.PlayOneShot(deadSound);
			break;
	 
		case 5:
			if(BossVoice)	
				audio.PlayOneShot(BossVoice);
			break;	
	}
}

function ChangeTexture(MapID:int)
{
	if(	MapID<=1||expTexture.length==0)
		return;
	if(	MapID>expTexture.length+1)
		MapID = expTexture.length+1;
	var	srcbodymeshs = this.transform.GetComponentsInChildren (SkinnedMeshRenderer||MeshRenderer);
	for (var srcbodymesh in srcbodymeshs )
	{
		srcbodymesh.renderer.material.mainTexture = expTexture[MapID-2];
    }
}

@RPC
function	huandon(ID:int,position:Vector3)
{
	newCamera.huandong(ID,position);
}

function	RPCDie()
{
	//var rtt : int	=	Random.Range(0,100);
	Debug.Log("某个PVPboss挂了");
	Die (	Random.Range( 0, 100 ),	Random.Range( 0, 3 ) );
}

function	OnClick()
{
	if(!BtnGameManager.isPlayerSelectEnamy)
		qiuai.objs = this.transform;
	PlayerStatus.MainCharacter.SendMessage("SetNowMonster",this,SendMessageOptions.DontRequireReceiver);
}