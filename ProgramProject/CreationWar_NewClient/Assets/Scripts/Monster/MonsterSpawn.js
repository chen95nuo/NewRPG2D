
#pragma strict 
enum	MonsterLevel
{
	pet			=1 , 
	normal		=2 , 
	elite		=3 , 
	petBoss		=4 , 
	normalBoss	=5 , 
	eliteBoss	=6
}
enum	SpawnPointType
{	
	boss1	=1 , 
	boss2	=2 , 
	Enemy	=3
}
//var	CurrentMonsterSpawnPointID	:	int;	//记录当前这生成点在当前这个场景中唯一索引公的ID//
private	var	SPNetView	:	SpawnPointNetView;
private	var	MonsterHP	:	int;

private	var	WaveID		:	int		=	-1;	//在塔防模式下一个点不只只有一个怪，此处记录是生成过几次怪，用于与服务器请求同步//
//private	var	SpawnedWave	:	int		=	-1;	//已经生成的波数//

private	var	MonsterPrefab	:	GameObject;	//Needed怪预设，用于怪物初始化//
private	var	pp				:	boolean		=	true;				//是否离开了生成点区域的标记//

function	IsAbleToSpawn()	:	boolean
{
	return	pp;
}
function	IsCleared()	:	boolean
{
	return	clear;
}

function SetClear(bool : boolean){
	clear = bool;
}

private var currentEnemy	: GameObject;	//接受实例化出怪物的变量，记录了这个点里出了什么怪//
private var CurrentCollider		:	SphereCollider ;				//自身的触发范围//
private var clear		:	boolean		=	false; 
private	var	Candel		:	boolean = true;		//能否删除，在竞技场中一般标记为不能//
var spType		:	SpawnPointType = SpawnPointType.Enemy;
var MonsterName :	String;					//Needed怪物预设、怪物名称，用于在小地图中显示名字//
var photonView	:	PhotonView;
var	viewID		:	int;	//Photon的ViewID已经不用了哟！不用了哟！，现在生成点的唯一实例ID是SPNetView.CurrentSpawnPointID//

var RoomSP	=	new	ExitGames.Client.Photon.Hashtable();
function	Awake()
{
	if(	!photonView	)
		photonView	=	gameObject.GetComponent( PhotonView );
	if(	SPNetView	==	null	)
		SPNetView	=	gameObject.GetComponent( SpawnPointNetView );
	if(	CurrentCollider	==	null	)	
		CurrentCollider	=	GetComponent( SphereCollider );
	viewID	=	photonView.viewID;
}

function	Start()
{
	CurrentCollider.radius	=	35;
	move();
}
//调整Y轴位置//
function	move()
{
	var hit : RaycastHit;
	if(	Physics.Raycast (transform.position, -Vector3.up, hit, 200.0)	)
	{ 
		if(	hit.collider.CompareTag ("Ground")	)   
			transform.position	=	hit.point + Vector3(0,0.5,0) ;	//重新设限确认自身位置在地表上方0.5处//
    }
}

private	var	rows1	:	yuan.YuanMemoryDB.YuanRow;	//Needed怪物属性数据,用于初始化怪物战斗属性//
//外部（DungeonControl - Start中）调用设置生成怪的数据//
function	SetMonsterAsID(	typeStr : String , prefab : GameObject , name : String , rows : yuan.YuanMemoryDB.YuanRow)
{
	rows1					=	rows;	//Needed怪物属性数据,用于初始化怪物战斗属性//
	MonsterName				=	name;	//Needed怪物预设、怪物名称，用于在小地图中显示名字//
	MonsterPrefab			=	prefab;	//Needed怪预设，用于怪物初始化//
	if(	UIControl.mapType	==	MapType.jingjichang	)
		CallMonster(	0	);	//竞技场直接刷新怪物//
}

private var mtw			:	MainTaskWork;
var 		task		:	MainTask		=	null;
var 		produced	:	ProducedObject	=	null; 		//任务生成的个体数据（预设、类型）//
var			isTask		:	boolean			=	false;		//是否有任务/，是不是任务/
private var MonsterID	:	int;							//怪物的ID为PhotonView的即时ID,不同客户端之间用于索引确认目标单位//
var	Taskrows			:	yuan.YuanMemoryDB.YuanRow;
//有任务时外部设置是不是任务//
function	SetTaskObj(	tsk : MainTask , p : ProducedObject	)
{ 
	try
	{
		mtw					=	FindObjectOfType(MainTaskWork);
		produced			=	p; 
		produced.spawnObj	=	gameObject;
		task				=	tsk; 
		isTask				=	true;
		Taskrows			=	AllManage.dungclStatic.GetNMRowAsID(parseInt(task.doneType.Substring(0,4)).ToString());
	}catch(e){}
}

//初始化怪物的属性,因为要上传服务器怪的属性，所以临时写//
static	function	GetMonsterHP(	_monsterLevel:int,	_monsterType:int,	playernum:int	,UsedlevelChanged:int)	:	int
{
	var _Level	=	DungeonControl.level -	2	+	UsedlevelChanged;   
	if(	_Level < 1	)
		_Level = 1;
 	var	addhealth	=	0;
	switch (	_monsterLevel	)
	{   
		case 0: 
			addhealth = 4000;
			break; 
		case 1:  
			addhealth = 2500;
		    if(	Application.loadedLevelName == "Map200"	)	
			addhealth = 1200;
			break; 
		case 2: 
			if(_Level<=20)
				addhealth = 700+_Level*15;
			else if(_Level<=30)
				addhealth = 1200;
			else if(_Level<=40)
				addhealth = 1500;
			else
				addhealth = 1800;
			if(	Application.loadedLevelName == "Map200"	)	
				addhealth = 800;
			break;
			
		case 3:  
			addhealth = 200;
			break; 
		case 4: 
			if(_Level<=20)
				addhealth = 100;
			else if(_Level<=30)
				addhealth = 120;
			else if(_Level<=40)
				addhealth = 130;
			else
				addhealth = 150;
			if(	Application.loadedLevelName == "Map200"	)	
				addhealth = 80;
			break; 
		case 5: 
			addhealth=60;
			break; 
	}
	addhealth	*=	playernum*BtnGameManager.dicClientParms["ClientParms3"]*0.01;
	if( DungeonControl.NowMapLevel == 5)
	{
		addhealth *= 4;

	}
	var	mhealth	=	100;
	switch (	_monsterType	)
	{
		case 0: 
		case 1:  
			mhealth	=	addhealth*(	_Level	*	348	-	_Level	*	_monsterLevel*50+50)*0.01;	
			break; 
		case 2: 
			mhealth =	addhealth*( _Level	*	308	-	_Level	*	_monsterLevel*45+20)*0.01;
			break;
		case 3: 
			mhealth =	addhealth*( _Level	*	282	-	_Level	*	_monsterLevel*45+20)*0.01;
			break;        
	} 
	return	mhealth;
}
private var	pttime	=	0;	//抵制重复申请!//
/// <summary>
/// 主角进入触发器时创建怪物 -
/// </summary>
function	OnTriggerEnter (	other	:	Collider	)
{
	if(	Application.loadedLevelName	==	"Map411"	)
		return	;
//	KDebug.Log(	"	Application.loadedLevelName	=	"	+	Application.loadedLevelName	);
	//KDebug.WriteLog(	"进入碰撞点 = " + SPNetView.CurrentSpawnPointID + " pp = " + pp.ToString()	);
	//Debug.Log(	"OnTriggerEnter!@#$%^&*()$%^$&^$^&%$^&%$^&$&^$&^$^&$%$&%% + "+other.tag + "  asdasd  "+(pttime <= Time.time).ToString() + " Level =  " +	Application.loadedLevelName	);
	if(	other.CompareTag ( "Player" ) && pp && pttime <= Time.time && Application.loadedLevelName	!=	"Map411" )
	{	//	玩家	&&	pp =》 这个玩家没有在区域内	&&	没有重复1秒内重复开启//
		pttime	=	Time.time	+	1;
		var	UsedLevelChange	: int	=	Random.Range(-1, 2	);	//UsedLevel的随机改变值,//
		var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
		
		if(	Application.loadedLevelName == "Map911"	)	//世界boss//
		{	//第一个为false表示boss死了
			if(	spType	==	SpawnPointType.boss1	||	spType	==	SpawnPointType.boss2	)
			{
				SpawnWorldBoss(	true,	UsedLevelChange, RandLevel,	SPNetView.GetNumPlayer()	);
			}
			else
			{
				//小怪	不作处理，等待出怪的管理器进行出怪调用 -
			}
		}
		else
		if(	Application.loadedLevelName == "Map912"	)	//世界boss//
		{	//第一个为false表示boss死了
			if(	spType	==	SpawnPointType.boss1	||	spType	==	SpawnPointType.boss2	)
			{
				SpawnWorldBoss(	true,	UsedLevelChange, RandLevel,	SPNetView.GetNumPlayer()	);
			}
			else
			{
				//小怪	不作处理，等待出怪的管理器进行出怪调用 -
			}
		}
		else
		{
			MonsterHP	=	GetMonsterHP(	GetSKInfoInt("MosterLevel" , 0 , rows1),	GetSKInfoInt("MosterType" , 0 , rows1),	SPNetView.GetNumPlayer(),	UsedLevelChange	);
			var	Str	:String	=	""+UsedLevelChange+","+RandLevel+","+SPNetView.GetNumPlayer();
			MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	-1,	MonsterHP,	Str	);
		}	
	}
}
//读取数据用的//
function	GetSKInfoInt(bd : String , it : int , rows : yuan.YuanMemoryDB.YuanRow) : int
{  
	var iii : int = 0;
	try
	{ 
		iii	=	parseInt(rows[bd].YuanColumnText);
		return  iii;
	}
	catch(e)
	{
		return	it;	
	}
}
///处理发来的消息//
function	OnMonsterSpawnPointHitted(	Data	:	String[]	)
{
	//Debug.Log(	"OnMonsterSpawnPointHitted!@#$%^&*()$%^$&^$^&%$^&%$^&$&^$&^$^&$%$&%%"	);
	MonsterID		=	int.Parse(	Data[0]	);
	var _Status		=	int.Parse(	Data[1]	);
	var	_MaxHP		=	int.Parse(	Data[2]	);
	var	_DataList:String[]	=	Data[3].Split(",".ToCharArray());
	var	UsedLevelChange	: int	=	int.Parse(	_DataList[0]	);	//UsedLevel的随机改变值//
	var	RandLevel		: int	=	int.Parse(	_DataList[1]	);	//用于取从0到level//
	var	RealNumPlayer	: int	=	int.Parse(	_DataList[2]	);	//创建时玩家数量//
	//Debug.Log(	"K________OnMonsterSpawnPointHitted + MonsterID " + MonsterID + " _Status " + _Status + " _MaxHP " + _MaxHP + " Data3 = "+Data[3]	);
	if(	_Status	==	0	)
	{	//服务器记录这个怪已经死了//
		pp		=	false;
		clear	=	true;
//		KDebug.Log("怪物已经死了");
	}
	else
	{	//服务器记录这个怪还没死//
		if(	pp	&&	currentEnemy	==	null	&&	MonsterPrefab	!=	null	&&	Application.loadedLevelName	!=	"Map441"	&&	(!clear)	)
		{	//主客户端	&&	pp	&&	当前创建的敌人还没有	&&	这个点有要刷新的怪物预设	&&	地图不是441	//
			if(	!isTask	)	//不是任务//
			{
				currentEnemy	=	MonsterHandler.CreateMonster(	MonsterPrefab, transform.position, Quaternion.identity,	MonsterID	);
				//随机朝向//
				currentEnemy.transform.eulerAngles	=	Vector3(0,Random.Range(-90,90),0);		
				currentEnemy.GetComponent(MonsterStatus).InitOnSpawn( this );
				//初始化怪物属性//
				currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1,UsedLevelChange,RandLevel,RealNumPlayer);   			
			}
			else			//是任务//
			{
				currentEnemy	=	mtw.CreateMonster(task , produced , transform.position,MonsterID	);
				if(	currentEnemy	)
				{
					currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
					currentEnemy.GetComponent(MonsterStatus).InitOnSpawn( this );
				}
			}
			pp	=	false;
			AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "00";
			AllManage.dungclStatic.SetMonsterStaticRoom();
			CurrentCollider.radius	=	60;		
		}
	}
}

function	SpawnWorldBoss(	_Status : boolean,	UsedLevelChange	:	int, RandLevel		:	int,	RealNumPlayer	:	int	)
{
	//Debug.Log(	"OnMonsterSpawnPointHitted!@#$%^&*()$%^$&^$^&%$^&%$^&$&^$&^$^&$%$&%%"	);
	MonsterID	=	PlayerUtil.WorldBossInstance;
	//Debug.Log(	"K________OnMonsterSpawnPointHitted + MonsterID " + MonsterID + " _Status " + _Status + " _MaxHP " + _MaxHP + " Data3 = "+Data[3]	);
	if(	!_Status	)
	{	//服务器记录这个怪已经死了//
		pp		=	false;
		clear	=	true;
//		KDebug.Log("怪物已经死了");
	}
	else
	{	//服务器记录这个怪还没死//
		if(	pp	&&	currentEnemy	==	null	&&	MonsterPrefab	!=	null	&&	(!clear)	)
		{	//主客户端	&&	pp	&&	当前创建的敌人还没有	&&	这个点有要刷新的怪物预设	&&	地图不是441	//
			//if(	!isTask	)	//不是任务//
			{
				currentEnemy	=	MonsterHandler.CreateMonster(	MonsterPrefab, transform.position, Quaternion.identity,	MonsterID	);
				//随机朝向//
				currentEnemy.transform.eulerAngles	=	Vector3(0,Random.Range(-90,90),0);		
				currentEnemy.GetComponent(MonsterStatus).InitOnSpawn( this );
				//初始化怪物属性//
				currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1,UsedLevelChange,RandLevel,RealNumPlayer);  	
			}
//			else			//是任务//
//			{
//				currentEnemy	=	mtw.CreateMonster(task , produced , transform.position,MonsterID	);
//				if(	currentEnemy	)
//				{
//					currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
//					currentEnemy.GetComponent(MonsterStatus).InitOnSpawn( this );
//				}
//			}
			pp	=	false;
			AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "00";
			AllManage.dungclStatic.SetMonsterStaticRoom();
			CurrentCollider.radius	=	60;		
		}
	}
}
/// <summary>
/// Old同步主角进入某个触发器时创建怪物 -
/// </summary>
//function	CallOnTrigger () 
//{
//	if(	PhotonNetwork.isMasterClient && pp && currentEnemy == null && MonsterPrefab != null && Application.loadedLevelName != "Map441"	)
//	{	//主客户端	&&	pp	&&	当前创建的敌人还没有	&&	这个点有要刷新的怪物预设	&&	地图不是441	//
//		if(	!isTask	)	//不是任务//
//		{
//			currentEnemy	=	PhotonNetwork.InstantiateSceneObject(	MonsterPrefab.name,	transform.position, Quaternion.identity, 0 , null);
//			currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0);		//随机朝向//
//			currentEnemy.SendMessage(	"SetMonsterSp" ,	photonView.viewID ,	SendMessageOptions.DontRequireReceiver	);	//初始化网络职务分配//
//			currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1);   			//初始化怪物属性//
//		}
//		else			//是任务//
//		{
//			//currentEnemy	=	mtw.CreateMonster(task , produced , transform.position);
//			if(currentEnemy)
//			{
//				currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
//				currentEnemy.SendMessage("SetMonsterSp" , photonView.viewID , SendMessageOptions.DontRequireReceiver); 
//			}
//		}
//		pp	=	false;
//		AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "00";
//		AllManage.dungclStatic.SetMonsterStaticRoom();		   		
//		photonView.RPC("TongBupp",PhotonTargets.AllBuffered, false,60);        	
// 		MonsterID	=	currentEnemy.GetComponent(PhotonView).viewID;       	
//		photonView.RPC("TongBumonster",PhotonTargets.AllBuffered,MonsterID );		
//	}
//}

var nowSkillMonsterName : String = "";
function CallMonsterSkillMonster(CalledWave: int , MonsterHP : int , skillName : String){
//	if(	MonsterPrefab	!=	null	)
//	{ 
//		if(	Application.loadedLevelName	==	"Map411"	&&	(	spType	==	SpawnPointType.boss1	||	spType	==	SpawnPointType.boss2	)	)
//		{
//			KDebug.Log(	"过滤已经出的波" + CalledWave	);
//			yield	WaitForSeconds(5);	//欧诺城等5秒再发送
//		}
//		if(	CalledWave	<	WaveID	)
//		{	
//			KDebug.Log(	"过滤已经出的波" + CalledWave	);
//			return;
//		}
//		//////////////
//		var	UsedLevelChange	: int	=	Random.Range(-1, 2	);	//UsedLevel的随机改变值,//
//		var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
//		MonsterHP	=	GetMonsterHP(	GetSKInfoInt("MosterLevel" , 0 , rows1),	GetSKInfoInt("MosterType" , 0 , rows1),	SPNetView.GetNumPlayer(),	UsedLevelChange	);
//		var	Str	:String	=	""+UsedLevelChange+","+RandLevel+","+SPNetView.GetNumPlayer() +	",0";
//		KDebug.Log(	"申请出怪 " + SPNetView.CurrentSpawnPointID + " CalledWave = " + CalledWave + " Str = " + Str );
//		MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	CalledWave,	MonsterHP,	Str	);
//	}
	var	UsedLevelChange	: int	=	Random.Range(-1, 2	);	//UsedLevel的随机改变值,//
	var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
	var	Str	:String	=	""+UsedLevelChange+","+RandLevel+","+SPNetView.GetNumPlayer() +	",0";
	MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	CalledWave,	MonsterHP,	Str	);
	nowSkillMonsterName = skillName;
}

var DotaTeam	:	int = 9;
var myPosLu		:	int = 0;
var myTower		:	DotTower;
function	CallMonster(	CalledWave: int	)
{
	yield;
	//KDebug.Log(	"CallMonster!@#$%^&*()$%^$&^$^&%$^&%$^&$&^$&^$^&$%$&%%  WaveID  =" + WaveID+"         SPNetView.CurrentSpawnPointID     " + SPNetView.CurrentSpawnPointID	);
	if(	MonsterPrefab	!=	null	)
	{ 
		if(	Application.loadedLevelName	==	"Map411"	&&	(	spType	==	SpawnPointType.boss1	||	spType	==	SpawnPointType.boss2	)	)
		{
//			KDebug.Log(	"过滤已经出的波" + CalledWave	);
			yield	WaitForSeconds(5);	//欧诺城等5秒再发送
		}
		if(	CalledWave	<	WaveID	)
		{	
//			KDebug.Log(	"过滤已经出的波" + CalledWave	);
			return;
		}
		//////////////
		if(AllManage.dungclStatic.isTowerMap()){
			clear = false;
		}
		var	UsedLevelChange	: int	=	Random.Range(-1, 2	);	//UsedLevel的随机改变值,//
		var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
		MonsterHP	=	GetMonsterHP(	GetSKInfoInt("MosterLevel" , 0 , rows1),	GetSKInfoInt("MosterType" , 0 , rows1),	SPNetView.GetNumPlayer(),	UsedLevelChange	);
		var	Str	:String	=	""+UsedLevelChange+","+RandLevel+","+SPNetView.GetNumPlayer() +	",0";
//		KDebug.Log(	"申请出怪 " + SPNetView.CurrentSpawnPointID + " CalledWave = " + CalledWave + " Str = " + Str );
		MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	CalledWave,	MonsterHP,	Str	);
		//////////////
//		currentEnemy = PhotonNetwork.InstantiateSceneObject(MonsterPrefab.name, transform.position, Quaternion.identity, 0,null);
//		
//	    currentEnemy.GetComponent(MonsterInfo).tongbuGuai(DotaTeam.ToString() , myPosLu);
//	    currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
//	    currentEnemy.SendMessage("SetMonsterSp" , photonView.viewID, SendMessageOptions.DontRequireReceiver);
//	    currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1);  
//	    
//	    pp = false;  			
//	    AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "00";
//	    AllManage.dungclStatic.SetMonsterStaticRoom();
//
//	    	photonView.RPC("TongBupp",PhotonTargets.AllBuffered, pp,60);        	
//	    	MonsterID = currentEnemy.GetComponent(PhotonView).viewID;       	
//			photonView.RPC("TongBumonster",PhotonTargets.AllBuffered,MonsterID );	
//			Candel = false;	 
	}
}

function	CallMonster( ATimes : int, CalledWave : int )
{
	yield;
	if(	WaveID	>	CalledWave	)
		return	;		//这一波已经出过了//
	//KDebug.Log(	"CallMonster( ATimes : int ) !@#$%^&* ( ) $%^$&^$^&%$^&%$^&$&^$&^$^&$%$&%% "	);
	if(	MonsterPrefab != null && myTower != null && !myTower.dead	)
	{ 
		//////////////
		var	UsedLevelChange	: int	=	Random.Range(-1, 2	);	//UsedLevel的随机改变值,//
		var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
		MonsterHP	=	GetMonsterHP(	GetSKInfoInt("MosterLevel" , 0 , rows1),	GetSKInfoInt("MosterType" , 0 , rows1),	SPNetView.GetNumPlayer(),	UsedLevelChange	);
		var	Str	:String	=	""+UsedLevelChange+","+RandLevel+","+SPNetView.GetNumPlayer() +","+ATimes;
		MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	CalledWave,	MonsterHP,	Str	);
		//////////////
//	     	currentEnemy = PhotonNetwork.InstantiateSceneObject(MonsterPrefab.name, transform.position, Quaternion.identity, 0,null);
//	    	currentEnemy.GetComponent(MonsterInfo).tongbuGuai(DotaTeam.ToString() , myPosLu);
//	    	currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
//	        currentEnemy.SendMessage("SetMonsterSp" , photonView.viewID, SendMessageOptions.DontRequireReceiver);
//	        //特殊改变-
//	        var useInt : int = 0;
//	        useInt =  ATimes / 3 ;
//	        currentEnemy.GetComponent(MonsterInfo).useALevel = useInt * 4;
//	         //特殊改变-
//	        
//	    	currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1);  
//	    	
//	    	pp = false;   
//	    	AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "00";
//	    	AllManage.dungclStatic.SetMonsterStaticRoom();
//		   				
//	    	photonView.RPC("TongBupp",PhotonTargets.AllBuffered, pp,60);        	
//	    	MonsterID = currentEnemy.GetComponent(PhotonView).viewID;       	
//			photonView.RPC("TongBumonster",PhotonTargets.AllBuffered,MonsterID );	
//			Candel = false;	 
	}
}

private var skillMonster : GameObject;
//竞技场出怪//
function	OnCallMonster(	Data:String[]	)
{
	MonsterID		=	int.Parse(	Data[0]	);
	var _Status		=	int.Parse(	Data[1]	);
	var	_MaxHP		=	int.Parse(	Data[2]	);
	var	_DataList	:	String[]	=	Data[3].Split(",".ToCharArray());
	var	UsedLevelChange	: int	=	int.Parse(	_DataList[0]	);	//UsedLevel的随机改变值//
	var	RandLevel		: int	=	int.Parse(	_DataList[1]	);	//用于取从0到level//
	var	RealNumPlayer	: int	=	int.Parse(	_DataList[2]	);	//创建时玩家数量//
	var	ATimes			: int	=	parseInt(	_DataList[3]	);	//随着时间刷怪属性增幅参数//
	var NewWaveID		: int	=	parseInt(	Data[4]	);
	if(	IsLimitCount	)
	{
		WorldBossMonsterSpawnControl.WaveSpawned( NewWaveID );
		if(	!WorldBossMonsterSpawnControl.AbleToSpawn()	)
			return;
	}
	
	if(	!SPNetView.IsSpawned( MonsterID ) )
	{
		if(	WaveID	>	NewWaveID	)
		{
			return;
		}
		WaveID	=	NewWaveID;
		SPNetView.AddMonster( MonsterID );
		
		if(nowSkillMonsterName != ""){
			skillMonster = Resources.Load( nowSkillMonsterName , GameObject);
			currentEnemy	=	MonsterHandler.CreateMonster(	skillMonster , transform.position, Quaternion.identity,	MonsterID	);		
			currentEnemy.GetComponent(	MonsterInfo	).tongbuGuai2(	DotaTeam.ToString(),	myPosLu	);
		}else{
			currentEnemy	=	MonsterHandler.CreateMonster(	MonsterPrefab, transform.position, Quaternion.identity,	MonsterID	);		
			currentEnemy.GetComponent(	MonsterInfo	).tongbuGuai(	DotaTeam.ToString(),	myPosLu	);
		}
		if(	IsLimitCount	)
		{
			SPNetView.AddWorldBossSpawnedMonster( currentEnemy );
		}	
		//currentEnemy	=	PhotonNetwork.InstantiateSceneObject(	MonsterPrefab.name, transform.position, Quaternion.identity, 0,null);
//		currentEnemy.GetComponent(	MonsterInfo	).tongbuGuai(	DotaTeam.ToString(),	myPosLu	);
		currentEnemy.transform.eulerAngles	=	Vector3(	0,	Random.Range(-90,90),	0	); 
		currentEnemy.GetComponent(	MonsterStatus	).InitOnSpawn( this );
		        //特殊改变	-	//随着时间刷怪属性增幅参数//
		        var useInt : int = 0;
		        useInt	=  ATimes / 3 ;
		        currentEnemy.GetComponent(MonsterInfo).useALevel = useInt * 4;
				//特殊改变	-	//随着时间刷怪属性增幅参数//
		//currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1);  
		currentEnemy.GetComponent(MonsterInfo).SetMonsterInfo(rows1,UsedLevelChange,RandLevel,RealNumPlayer);   
		currentEnemy.GetComponent(MonsterStatus).SkullMaxHp(_MaxHP);
		if(Application.loadedLevelName == "Map411"){
			currentEnemy.SendMessage("SetSpawnObject" , this , SendMessageOptions.DontRequireReceiver);
			currentEnemy.SendMessage("SetWaveSpawned" , WaveID , SendMessageOptions.DontRequireReceiver);
		}
		pp	=	false;   
		AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "00";
		AllManage.dungclStatic.SetMonsterStaticRoom();
			   				
		CurrentCollider.radius	=	60;     	
		//MonsterID = currentEnemy.GetComponent(PhotonView).viewID;
		Candel	=	false;	 
	}
}

//Old
//@RPC
//function	TongBupp( p:boolean,ccr:int)
//{
//	pp			=	p;
//	CurrentCollider.radius	=	ccr;
//}

//Old
//@RPC
//function	TongBumonster(q:int)
//{
//	MonsterID	=	q;
//}

//怪物死了之后执行，通知任务完成//
var transMonster : Transform;
function	TBC(trasn : Transform)
{
	//KDebug.Log("TBCTBCTBCTBCTBCTBC");
	if(	!clear	&&	(	spType	==	SpawnPointType.boss2	||	spType	==	SpawnPointType.boss1	)	)
		MonsterServerRequest.RequestBoss2WasDie(	SPNetView.CurrentSpawnPointID	);
	transMonster = trasn;
	CommitKillMonster();
}

private	function	CommitKillMonster()
{
	clear	=	true;	//这个点的怪被杀了杀了了//
	//photonView.RPC(	"TongbuC",	PhotonTargets.AllBuffered , true	);
	if(!AllManage.dungclStatic.isTowerMap()){
			if( spType	== SpawnPointType.boss1 || spType == SpawnPointType.boss2 )
			{
				if(	dungCL	==	null)
				{
					dungCL	=	FindObjectOfType(	DungeonControl	);
				}
				dungCL.LookBoss();
				if(transMonster != null && ! AllResources.ar.IsActivity() && UIControl.mapType == MapType.fuben && Application.loadedLevelName != "Map200"){
					dungCL.BossDileStoneBox(transMonster);
				}
				//	photonView.RPC("TongbuCBoss",PhotonTargets.All); 
			}
	}else{
		if(	dungCL	==	null)
		{
			dungCL	=	FindObjectOfType(	DungeonControl	);
		}
		dungCL.LookTowerMonster();
	}
	AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "01";
	AllManage.dungclStatic.SetMonsterStaticRoom();
}
//@RPC
//function	TongbuC(	bool : boolean	)
//{
//	clear	=	bool;
//}

private	var	dungCL	:	DungeonControl;
//@RPC
//function TongbuCBoss()
//{
//	if(	dungCL == null)
//	{
//		dungCL = FindObjectOfType(DungeonControl);
//	}
////	if()
//	dungCL.LookBoss();
//}

//function	OnTriggerExit (other : Collider)
//{ 
//	if(	UIControl.mapType	!=	MapType.jingjichang	&&	Candel	&&	!clear &&other.CompareTag ("Player")	)
//	{	//不是竞技场	&&	
//		yield;
//		var	colliders : Collider[]	= Physics.OverlapSphere ( transform.position, this.CurrentCollider.radius);
//		var	col		: Collider;
//		for( var	hit	in	colliders	) 
//		{
//			if(	hit.CompareTag ("Player")	)
//			    col	=	hit;
//		}
//		//if(	col	==	null	)
//		//	Des();
//	} 
//}

///Old怪物回去然后...//
//private	function	Des()
//{
//	if(	PhotonNetwork.connected && PhotonNetwork.isMasterClient && MonsterID != 0 )
//	{
//		var	PV	:	PhotonView	=	photonView.Find(MonsterID);
//	    if( PV )
//			PhotonNetwork.Destroy(PV);
//		pp	=	true;    
//	    AllManage.dungclStatic.staticRoomSP["sp" + viewID] = "10";
//	    AllManage.dungclStatic.SetMonsterStaticRoom();
//		   	
//		photonView.RPC("TongBupp",PhotonTargets.AllBuffered , pp,35);
//	}
//}

///组队下通知这里已经被杀了//
function	Boss2WasDie()
{
//	KDebug.Log("收到了其他人杀怪信息");
	if(	clear	)
		return;
	CommitKillMonster();
}


private	var IsLimitCount					:	boolean	=	false;
private	var	WorldBossMonsterSpawnControl	:	WorldBossCallSkull;
///限制出怪数量 -
function	LimitMonsterCountTo(	_WorldBossMonsterSpawnControl	:	WorldBossCallSkull	)
{
	WorldBossMonsterSpawnControl	=	_WorldBossMonsterSpawnControl;
	IsLimitCount					=	true;
}

//世界boss召唤骷髅//
function	CallWorldBossSkull(	NewWaveID : int 	)
{
	if(	WorldBossMonsterSpawnControl.AbleToSpawn()	)
	{	//数量允许(总数量小于6)//
		CallMonster( NewWaveID );
	}
}

//获取当前这个点已经生成的怪物数量//
function	GetCountSpawnedSkullCount()	:	int
{
	return	SPNetView.GetWorldBossSpawnedMonsterCount();
}

function GetRow1() : boolean {
	if(rows1 != null){
		return true;
	}
	return false;
}
