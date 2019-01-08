#pragma strict
var monsterInfo	:	GameNMInfo;
var useALevel	:	int	=	0;

/// <summary>
/// Old初始化，记录DB中怪物的相关数据 -
/// </summary>
function	SetMonsterInfo(rows : yuan.YuanMemoryDB.YuanRow)
{
	SetMonsterInfo(	rows,	Random.Range(-1,2),	Random.Range( 0, 10 ),	1	);
		return;
//	monsterInfo					=	new	GameNMInfo();
//	monsterInfo.name			=	rows["NPCName"].YuanColumnText;
//	monsterInfo.npcType			=	GetSKInfoInt("NPCType" , 0 , rows);
//	monsterInfo.NPCID			=	rows["NPCID"].YuanColumnText;
//	monsterInfo.MapID			=	rows["MapID"].YuanColumnText;
//	monsterInfo.isTaskNPC		=	GetSKInfoInt("isTaskNPC" , 0 , rows);
//	monsterInfo.npcTitle		=	rows["NPCTitle"].YuanColumnText;
//	monsterInfo.npcTalk			=	rows["NPCTalk"].YuanColumnText;
//	monsterInfo.monsterParent	=	rows["MosterParent"].YuanColumnText;
//	monsterInfo.monsterParentID	=	rows["MosterParentID"].YuanColumnText;
//	monsterInfo.monsterTitle	=	rows["MosterTitle"].YuanColumnText;          //chenghao	
////	    GetComponent(MonsterStatus).Name = monsterInfo.name;
//	monsterInfo.monsterMapID	=	GetSKInfoInt("MosterMapID" , 0 , rows);         //tietu
//	monsterInfo.monsterLevel	=	GetSKInfoInt("MosterLevel" , 0 , rows);    //jinying,boss
//	monsterInfo.monsterType		=	GetSKInfoInt("MosterType" , 0 , rows);       //qiangzhuang,jiaozha,mofa
//	var setArray = new int[4];
//	setArray[0]	=	monsterInfo.monsterLevel;
//	setArray[1]	=	monsterInfo.monsterType;
//	setArray[2]	=	monsterInfo.monsterMapID;
//	setArray[3]	=	Random.Range(-1,2) + useALevel;
//	photonView.RPC("SetMonsterStatus",PhotonTargets.AllBuffered,setArray,monsterInfo.name);	
//	if(	monsterInfo.monsterLevel	<	3	)
//	{
//		monsterInfo.monsterSkill1 = GetSKInfoInt("MosterSkill0", 0 , rows);
//		monsterInfo.monsterSkill2 = GetSKInfoInt("MosterSkill1", 0 , rows);
//		monsterInfo.monsterSkill3 = GetSKInfoInt("MosterSkill2", 0 , rows);
//	    	var settingsArray = new int[3];
//			settingsArray[0]=monsterInfo.monsterSkill1;
//			settingsArray[1]=monsterInfo.monsterSkill2;
//			settingsArray[2]=monsterInfo.monsterSkill3;
//			photonView.RPC("ReadyMosterSkill",PhotonTargets.AllBuffered,settingsArray);
//	}
}

/// <summary>
/// New初始化，记录DB中怪物的相关数据 -
/// </summary>
///	RandLevel:随机等级用的随机数0-10
/// NumPlayer玩家数量
function	SetMonsterInfo(rows : yuan.YuanMemoryDB.YuanRow,	UsedLevelChange :int,	RandLevel :	int,	NumPlayer	:	int	)
{
	monsterInfo					=	new	GameNMInfo();
	monsterInfo.name			=	rows["NPCName"].YuanColumnText;
	monsterInfo.npcType			=	GetSKInfoInt("NPCType" , 0 , rows);
	monsterInfo.NPCID			=	rows["NPCID"].YuanColumnText;
	monsterInfo.MapID			=	rows["MapID"].YuanColumnText;
	monsterInfo.isTaskNPC		=	GetSKInfoInt("isTaskNPC" , 0 , rows);
	monsterInfo.npcTitle		=	rows["NPCTitle"].YuanColumnText;
	monsterInfo.npcTalk			=	rows["NPCTalk"].YuanColumnText;
	monsterInfo.monsterParent	=	rows["MosterParent"].YuanColumnText;
	monsterInfo.monsterParentID	=	rows["MosterParentID"].YuanColumnText;
	monsterInfo.monsterTitle	=	rows["MosterTitle"].YuanColumnText;          //chenghao	

	monsterInfo.monsterMapID	=	GetSKInfoInt("MosterMapID" , 0 , rows);	//tietu
	monsterInfo.monsterLevel	=	GetSKInfoInt("MosterLevel" , 0 , rows);	//jinying,boss
	monsterInfo.monsterType		=	GetSKInfoInt("MosterType" , 0 , rows);	//qiangzhuang,jiaozha,mofa
	
	var setArray	=	new int[4];
	setArray[0]		=	monsterInfo.monsterLevel;
	setArray[1]		=	monsterInfo.monsterType;
	setArray[2]		=	monsterInfo.monsterMapID;
	setArray[3]		=	UsedLevelChange + useALevel;
	GetComponent(MonsterStatus).SetMonsterStatus( setArray, monsterInfo.name, NumPlayer, RandLevel );
	if(	monsterInfo.monsterLevel	<	3	)
	{
		monsterInfo.monsterSkill1 = GetSKInfoInt("MosterSkill0", 0 , rows);
		monsterInfo.monsterSkill2 = GetSKInfoInt("MosterSkill1", 0 , rows);
		monsterInfo.monsterSkill3 = GetSKInfoInt("MosterSkill2", 0 , rows);
	    	var settingsArray = new int[3];
			settingsArray[0]=monsterInfo.monsterSkill1;
			settingsArray[1]=monsterInfo.monsterSkill2;
			settingsArray[2]=monsterInfo.monsterSkill3;
			GetComponent(MonsterSkill).ReadyMosterSkill( settingsArray );
	}
}


function	GetSKInfoInt(bd : String , it : int , rows : yuan.YuanMemoryDB.YuanRow) : int
{  
	var iii : int = 0;
	try
	{ 
		iii	=	parseInt(	rows[bd].YuanColumnText	);
		return  iii;
	}
	catch(e)
	{
		return	it;	
	}
}

///PVP调用//
function	tongbuGuai(	myTeam : String ,	myPosLu : int	)
{
	//KDebug.Log( "!!!!!!!!!!!!!!!!!!!!!!!设置了怪的阵营 ： " + gameObject.name );
	if(	Application.loadedLevelName	==	"Map411"	)
	{
		if(	transform.position.z	>	200	)
			myTeam	=	"Blue";
		else
			myTeam	=	"Red";
	}
	if(Application.loadedLevelName != "Map911" && Application.loadedLevelName != "Map712" && Application.loadedLevelName != "Map713" && Application.loadedLevelName != "Map912"){	
		RPCtongbuGuai(	myTeam,	myPosLu	);
	}
}

///PVP调用//
function	tongbuGuai2(	myTeam : String ,	myPosLu : int	)
{
	//KDebug.Log( "!!!!!!!!!!!!!!!!!!!!!!!设置了怪的阵营 ： " + gameObject.name );
	if(	Application.loadedLevelName	==	"Map411"	)
	{
		if(	transform.position.z	>	200	)
			myTeam	=	"Blue";
		else
			myTeam	=	"Red";
	}
}


function	RPCtongbuGuai(myTeam : String , myPosLu : int)
{
	if(Application.loadedLevelName == "Map721")
		return;
	var	BfCItem	:	BattlefieldCityItem;
	BfCItem	=	gameObject.AddComponent(BattlefieldCityItem);
	if(	BfCItem	!=	null	)
	{
		BfCItem.myTeam	=	myTeam;
		BfCItem.myPosLu	=	myPosLu;
	}
}