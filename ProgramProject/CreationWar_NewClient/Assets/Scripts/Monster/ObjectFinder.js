#pragma strict

	static	function	GetInstanceID( Go:GameObject )	:	int
	{
		var	PS:PlayerStatus	=	Go.GetComponent(	PlayerStatus	);
		if(	PS	!=	null	)
		{
			return	PS.instanceID;
		}
		else
		{
			var	MNV	:	MonsterNetView	=	Go.GetComponent(MonsterNetView);
			if(	MNV	!=	null	)
			{
				return	MNV.MonsterID;
			}
		}
		return	0;
	}
	
	static	function	FindTargetTransform( ID : int )	:	Transform
	{
		var	go : GameObject;	
		if(	ID	==	PlayerUtil.myID	)
		{
			go	=	PlayerStatus.MainCharacter.gameObject;
		}
		else
		{
			go	=	ObjectAccessor.getAOIObject(ID);			
		}
		if(	go	==	null	)
		{
			go	=	MonsterHandler.GetInstance().FindMonsterByMonsterID(ID);			
		}
		if(	go	!=	null	)
		{
			return	go.transform;	
		}
		return	null;
	}
	
	static	function	FindTargetGameObject( ID : int )	:	GameObject
	{
		var	go : GameObject;	
		if(	ID	==	PlayerUtil.myID	)
		{
			go	=	PlayerStatus.MainCharacter.gameObject;
		}
		else
		{
			go	=	ObjectAccessor.getAOIObject(ID);
		}
		if(	go	==	null	)
		{
			go	=	MonsterHandler.GetInstance().FindMonsterByMonsterID(ID);
		}
		return	go;
	}
