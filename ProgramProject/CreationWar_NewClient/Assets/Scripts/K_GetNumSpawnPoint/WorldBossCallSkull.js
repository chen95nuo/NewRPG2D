#pragma strict

private	var	SpawnPointsArray	:	MonsterSpawn[];
private	var	StartCounting		:	boolean	=	false;
private	var	TimeFlage			:	float	=	0.0;

private	var	MaxSkullCount		:	int		=	6;

private	var	SpawnedWave			:	int		=	0;

function	Awake()
{
	SpawnPointsArray	=	FindObjectsOfType(MonsterSpawn);
	for( var	i = 0;	i	<	SpawnPointsArray.length;	i++	)
	{
		if(	SpawnPointsArray[i].spType	==	SpawnPointType.Enemy	)
		{
			SpawnPointsArray[i].LimitMonsterCountTo( this );
		}
	}
}

function	Start()
{
	//InvokeRepeating(	"CallSpawnMonster",	10,	10	);
	StartCounting	=	true;
}

function	Update() 
{
	if(	StartCounting	)
	{
		TimeFlage	+=	Time.deltaTime;
		if(	TimeFlage	>	10.0f	)
		{
			TimeFlage	=	0;
			CallSpawnMonster();
		}
	}
}

///出怪//
private		function	CallSpawnMonster()
{
//	KDebug.Log( "~出~怪~指~令~" );
	for( var i = 0;	i < SpawnPointsArray.length; i++ )
	{
		if(	SpawnPointsArray[i].spType	==	SpawnPointType.Enemy	)
		{
			SpawnPointsArray[i].CallWorldBossSkull( SpawnedWave );
		}
	}
}

///告诉刚刚生成了一波//
function	WaveSpawned( WaveID	:	int	)
{
//	KDebug.Log("出怪完成同步 + " + WaveID );
	TimeFlage	=	0.0;
	SpawnedWave	=	WaveID;
}

///根据数量判断当前状态能否生成怪物//
function	AbleToSpawn()	:	boolean	
{
	var	Counts	:	int	=	0;
	for( var i = 0;	i < SpawnPointsArray.length; i++ )
	{
		if(	SpawnPointsArray[i].spType	==	SpawnPointType.Enemy	)
		{
			Counts	+=	SpawnPointsArray[i].GetCountSpawnedSkullCount();
		}
	}
	
	if(	MaxSkullCount	>	Counts	)
		return	true;
	return	false;
}

