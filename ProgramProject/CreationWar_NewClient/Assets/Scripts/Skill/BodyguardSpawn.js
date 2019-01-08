
//#pragma strict 
var	SPNetView		:	SpawnPointNetView;
var	MonsterPrefab	:	GameObject ;
private	var pp	: boolean =	true;
private	var currentEnemy : GameObject;
private	var photonView : PhotonView;
private var monsterType	:int	=1;
private var monsterMapID:int	=1;
private var monsterLevel:int	=20;
var	monstername	:	String = "";
function	Awake()
{
	if(	SPNetView	==	null	)
		GetComponent(	SpawnPointNetView	);
	if(	photonView	==	null	)
		photonView	=	GetComponent(PhotonView);
}

function	Start () 
{
	yield	WaitForSeconds(2);
	if(	UIControl.GuildLevel	!=	-1	)
		CallMonsterGuildLevel();
}

private	var	rows1	:	yuan.YuanMemoryDB.YuanRow;	//Needed怪物属性数据,用于初始化怪物战斗属性//
function	CallMonster()
{
	if(	pp	&&	MonsterPrefab	!=	null	)
	{ 
		rows1 = AllManage.dungclStatic.GetNMRowAsID("322");
		var	UsedLevelChange	: int	=	Random.Range(-1, 2	);	//UsedLevel的随机改变值,//
		var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
//		var MonsterHP	:int	=	MonsterSpawn.GetMonsterHP(	monsterLevel,	monsterType,	1,	UsedLevelChange	);
		var MonsterHP	:int	=	MonsterSpawn.GetMonsterHP(	GetSKInfoInt("MosterLevel" , 0 , rows1),	GetSKInfoInt("MosterType" , 0 , rows1),	1,	UsedLevelChange	);

		var	Str			:	String	=	""	+	UsedLevelChange	+	","	+	RandLevel	+	","	+	1;
		MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	-1,	MonsterHP,	Str	);
//		monsterLevel	=	UIControl.GuildLevel * 10;
//     	currentEnemy	=	PhotonNetwork.InstantiateSceneObject(MonsterPrefab.name, transform.position, Quaternion.identity, 0,null);
//    	currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
//		var setArray = new int[4];
//		setArray[0]	=	2;
//		setArray[1]	=	monsterType;
//		setArray[2]	=	monsterMapID;
//		setArray[3]	=	monsterLevel;	 
//    	currentEnemy.GetComponent(PhotonView).RPC("SetMonsterStatus",PhotonTargets.AllBuffered,setArray,monstername);
//		pp = false;  			       	       	 
	}
}

function	CallMonsterGuildLevel()
{
	if(	pp	&&	MonsterPrefab	!=	null	)
	{ 
		rows1 = AllManage.dungclStatic.GetNMRowAsID("322");
		var	UsedLevelChange	: int	=	UIControl.GuildLevel * 10;	//UsedLevel的随机改变值,//
		var	RandLevel		: int	=	Random.Range( 0, 10 );	//用于取从0到level取个随机百分比用于初始化攻击力//
//		var MonsterHP	:int	=	MonsterSpawn.GetMonsterHP(	monsterLevel,	monsterType,	1,	UsedLevelChange	);
		var MonsterHP	:int	=	MonsterSpawn.GetMonsterHP(	GetSKInfoInt("MosterLevel" , 0 , rows1),	GetSKInfoInt("MosterType" , 0 , rows1),	1,	UsedLevelChange	);

		var	Str			:	String	=	""	+	UsedLevelChange	+	","	+	RandLevel	+	","	+	1;
		MonsterServerRequest.RequestPlayerEnterMonsterSpawnArea(	SPNetView.CurrentSpawnPointID,	-1,	MonsterHP,	Str	);
//		monsterLevel	=	UIControl.GuildLevel * 10;
//     	currentEnemy	=	PhotonNetwork.InstantiateSceneObject(MonsterPrefab.name, transform.position, Quaternion.identity, 0,null);
//    	currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
//		var setArray = new int[4];
//		setArray[0]	=	2;
//		setArray[1]	=	monsterType;
//		setArray[2]	=	monsterMapID;
//		setArray[3]	=	monsterLevel;	 
//    	currentEnemy.GetComponent(PhotonView).RPC("SetMonsterStatus",PhotonTargets.AllBuffered,setArray,monstername);
//		pp = false;  			       	       	 
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

function	OnMonsterSpawnPointHitted(	Data	:	String[]	)
{
	var MonsterID	=	int.Parse(	Data[0]	);
	var _Status		=	int.Parse(	Data[1]	);
	var	_MaxHP		=	int.Parse(	Data[2]	);
	var	_DataList:String[]	=	Data[3].Split(",".ToCharArray());
	var	UsedLevelChange	: int	=	int.Parse(	_DataList[0]	);	//UsedLevel的随机改变值//
	var	RandLevel		: int	=	int.Parse(	_DataList[1]	);	//用于取从0到level//
	var	RealNumPlayer	: int	=	int.Parse(	_DataList[2]	);	//创建时玩家数量//
	//Debug.Log(	"K________OnMonsterSpawnPointHitted + MonsterID " + MonsterID + " _Status " + _Status + " _MaxHP " + _MaxHP + " Data3 = "+Data[3]	);
	monsterLevel	=	UIControl.GuildLevel * 10;
	currentEnemy	=	MonsterHandler.CreateMonster(	MonsterPrefab, transform.position, Quaternion.identity,	MonsterID	);
	//currentEnemy	=		PhotonNetwork.InstantiateSceneObject(MonsterPrefab.name, transform.position, Quaternion.identity, 0,null);
    currentEnemy.transform.eulerAngles = Vector3(0,Random.Range(-90,90),0); 
	var setArray = new int[4];
	setArray[0]	=	2;
	setArray[1]	=	monsterType;
	setArray[2]	=	monsterMapID;
	setArray[3]	=	monsterLevel;	 
	//currentEnemy.GetComponent(PhotonView).RPC("SetMonsterStatus",PhotonTargets.AllBuffered,setArray,monstername);
	currentEnemy.GetComponent( MonsterStatus ).SetMonsterStatus( setArray, monstername, 1, RandLevel );
	pp = false;  
}