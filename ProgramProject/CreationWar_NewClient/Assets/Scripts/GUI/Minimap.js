#pragma strict
class	Minimap	extends	Song
{
	var	photonView	:	PhotonView;
	private var MonsterSp :MonsterSpawn[];
	private var npcs : npcAI[];
	private var MonsterSpt : Transform[];
	private var Monsters : GameObject[];
	private var teamPlayers : Transform[];
	private var BossSp : MonsterSpawn[];

	private var NPC: GameObject[];
	private var Portal: TriggerLoadLevel[];
	private var ExitPortal:TriggerExit;

	private var Monsterdot : Transform[];
	var reddot : Transform;
	var greendot : Transform;
	var bluedot : Transform;
	var selfdot : Transform;
	var ePortal :Transform;
	var mPortal :Transform;
	var taskdot : Transform;
	var taskdot1 : Transform;
	var taskdot2 : Transform;
	var boosDot : Transform;
	private var mcamera :MapCamera;
	private var offsetx = 0.0;
	private var offsety = 0.0;
	//private var size = 110;
	var LabelZhuobiao : UILabel;
	function	Awake()
	{
		otherp		= new Array();
		MonsterSp	= new Array();
		npcs		= new Array();
		BossSp		= new Array();
		MonsterSpt	= new Array();
		Monsters	= new Array();
		teamPlayers	= new Array();
		NPC			= new Array();
		Portal		= new Array();
		Monsterdot	= new Array();
		TaskTrans	= new Array();
		PlayerSpt	= new Array();
		gos			= new Array();

		Destroy(photonView);
		gameObject.AddComponent(PhotonView);
		remotenum = PlayerPrefs.GetInt("PlayerNum", 5);
		AllManage.mmp = this;
	}
	function	Start () 
	{
		for( var trans : Transform in transform )
		{
			if(	trans.name.IndexOf("(Clone)") > 0	)
			{
				Destroy(trans.gameObject);
			}
		}
		yield;
		photonView	=	GetComponent(	PhotonView	);
		photonView.viewID	=	889;
		photonView.observed	=	this;	
		Positiontable		=	new	Hashtable();
		ppIDlist			=	new	Array( 9 );
		for( var i : int = 0; i < 9; i++ )
		{ 	
			ppIDlist[i]	=	"0000";
		}
		mcamera = FindObjectOfType(MapCamera);
		offsetx = mcamera.transform.position.x;
		offsety = mcamera.transform.position.z;

		reddot.localPosition.y	=	300;
		selfdot.localPosition.y	=	300;
		yield;

		InvokeRepeating(	"SeePlayer",	0,	1.5	);
		InvokeRepeating(	"UpSelfdot",	2,	0.3	);
		yield	WaitForSeconds(2.5);
		upMonsterdots();
		yield	WaitForSeconds(1.5);
		UpdateNPCposition();
		yield;
		UpdatePortalosition();
		yield;
		CreatTeamDot();
		yield;
		CreateTaskDot();
		yield;
		CreateBossDot();
		yield;
		InvokeRepeating("Updateteamposition", 2, 1.5);
		yield;
		InvokeRepeating("UpdateTaskPosition", 2, 1.5);
		yield;
		InvokeRepeating("UpdateBossPosition", 2, 1.5);
	}

	private var BossTrans			: Transform[];
	private var BossSptArray		: Array = new Array();
	private var BossSptTaskArray	: Array = new Array();
	private var useTaskTrans		: Transform;
	private var useTask				: MonsterSpawn;
	function	CreateBossDot()
	{
		var intLength : int = 0;
		var i : int = 0;
		BossSptArray = new Array();
		BossSptTaskArray = new Array();
		BossSp =  FindObjectsOfType(MonsterSpawn);
		for(i=0; i<BossSp.length; i++)
		{
			if(	BossSp[i].spType == SpawnPointType.boss1 || BossSp[i].spType == SpawnPointType.boss2	)
			{
				BossSptArray.Add(BossSp[i].transform);
				BossSptTaskArray.Add(BossSp[i]);
			}
		}
		if(	BossTrans && BossTrans.length > 0	)
		{
			for(	i=0; i < BossTrans.length; i++)
			{
				if(	BossTrans[i] != null	)
				{
					Destroy(BossTrans[i].gameObject);
				}
			}
		}
		BossTrans = new Array(BossSptArray.Count);
		for( i = 0; i < BossTrans.length; i++ )
		{
			useTask = BossSptTaskArray[i];
			useTaskTrans = boosDot;
			BossTrans[i] = Instantiate (useTaskTrans);
			BossTrans[i].parent = this.transform;
			BossTrans[i].localScale = useTaskTrans.localScale;
			BossTrans[i].localPosition.y =300; 	
			BossTrans[i].localPosition.z = 0; 	
		}
	}

	private	var	useBossTrans : Transform;
	function	UpdateBossPosition()
	{
		for( var i : int = 0; i < BossSptArray.Count; i++ )
		{	
			if( BossSptArray[i]	!=	null )
			{	
				useBossTrans	=	BossSptArray[i];
				BossTrans[i].localPosition.x	=	(useBossTrans.transform.position.x-offsetx)*80/mcamera.mapsize;
				BossTrans[i].localPosition.y	=	(useBossTrans.transform.position.z-offsety)*80/mcamera.mapsize;
			}
			else
			{
				if(	BossTrans.length - 1 >= i	)
				{
					BossTrans[i].localPosition.y	=	300;    
				}
			}
		}
	}
	//////////////
	//////////////
	private var TaskTrans : Transform[];
	private var TaskSptArray : Array = new Array();
	private var TaskSptAIArray : Array = new Array();
	private var useAITrans : Transform;
	private var useAI : npcAI;
	function	CreateTaskDot()
	{
		var intLength : int = 0;
		var i : int = 0;
		TaskSptArray = new Array();
		TaskSptAIArray = new Array();
		npcs =  FindObjectsOfType(npcAI);
		for( i=0; i<npcs.length; i++ )
		{
			if(npcs[i].canFuhao)
			{
	//			//print(npcs[i].transform);
				TaskSptArray.Add(npcs[i].transform);
				TaskSptAIArray.Add(npcs[i]);
			}
		}
		if(	TaskTrans && TaskTrans.length > 0	)
		{
			for(i=0; i < TaskTrans.length; i++)
			{
				if(TaskTrans[i] != null)
				{
					Destroy(TaskTrans[i].gameObject);
				}
			}
		}
		TaskTrans = new Array(TaskSptArray.Count);
		for( i = 0; i < TaskTrans.length; i++)
		{
			useAI = TaskSptAIArray[i];
			if(	useAI.fuhaoID == 0)
			{
				useAITrans = taskdot;
			}else
			if( useAI.fuhaoID == 1)
			{
				useAITrans = taskdot1;
			}else
			if( useAI.fuhaoID == 2)
			{
				useAITrans = taskdot2;
			}
			TaskTrans[i]	=	Instantiate (useAITrans);
			TaskTrans[i].parent		=	this.transform;
			TaskTrans[i].localScale	=	useAITrans.localScale;
			TaskTrans[i].localPosition.y	=	300; 	
			TaskTrans[i].localPosition.z	=	0; 	
		}
	}

	private var useTrans : Transform;
	function	UpdateTaskPosition()
	{
		for (var i : int = 0; i < TaskSptArray.Count; i++)
		{	
			if(	TaskSptArray[i] != null	)
			{	
				useTrans = TaskSptArray[i];
				TaskTrans[i].localPosition.x = (useTrans.transform.position.x-offsetx)*80/mcamera.mapsize;
				TaskTrans[i].localPosition.y = (useTrans.transform.position.z-offsety)*80/mcamera.mapsize;
			}
			else
			{
				if(TaskTrans.length - 1 >= i)
				{
					TaskTrans[i].localPosition.y =300;    
				}
			}
		}
	}

	function	upMonsterdots()
	{
		UpdateMonster();
		GreatReddot();
		InvokeRepeating("UpdateMonster", 2, 3);
		InvokeRepeating("Updateposition", 2, 1);
	}

	private	var	PlayerSpt : Transform[];	
	function	CreatTeamDot()
	{
		PlayerSpt = new Array (7);
		for( var i : int = 0; i < 7; i++ )
		{
			PlayerSpt[i]	=	Instantiate (bluedot);
			PlayerSpt[i].parent	=	this.transform;
			PlayerSpt[i].localScale	=	bluedot.localScale;
			PlayerSpt[i].localPosition.y	=	300; 	
			PlayerSpt[i].localPosition.z	=	0; 
		}
	}

	private	var	playernum = 0;
	function	Updateteamposition()
	{
		if(	UIControl.mapType	==	MapType.jingjichang	)
			teamPlayers = FindPVPPlayers();	
		else
			teamPlayers = FindFBPlayers();
		var n =0;
		for(	var i : int = 0; i < teamPlayers.Length; i++)
		{	
			if(	teamPlayers[i] != null	)
			{	
				if(	PlayerSpt.length - 1 >= i)
				{
					PlayerSpt[i].localPosition.x = (teamPlayers[i].position.x-offsetx)*80/mcamera.mapsize;
					PlayerSpt[i].localPosition.y = (teamPlayers[i].position.z-offsety)*80/mcamera.mapsize;
				}
			}
			else
			{
				if(	PlayerSpt.length - 1 >= i	)
				{
					PlayerSpt[i].localPosition.y	=	300;
				}
			}
		}
	}

	private var gos	:	PlayerStatus[];
	private	function	FindPVPPlayers () : Transform[]
	{ 
		var	Players	:	Transform[];
		Players = new Array (7);
		var i = 0;
		if(	PlayerStatus.MainCharacter	)
			var selfTeamID = PlayerStatus.MainCharacter.GetComponent(PlayerStatus).PVPmyTeam;
		if(	selfTeamID !=""	)
		{
			var diff:String;
			gos = FindObjectsOfType(PlayerStatus);
			for( var go : PlayerStatus in gos )
			{
				if(	go.transform	!=	PlayerStatus.MainCharacter	)
					diff	=	go.PVPmyTeam; 
				if(	diff	==	selfTeamID	)
				{
					Players[i]	=	go.transform;
					i +=1;
				}
			}
		}
		return Players;
	}

	private	function	FindFBPlayers ()	:	Transform[]
	{ 
		var Players : Transform[];
		Players = new Array (4);
		var i = 0;
		if(	PlayerStatus.MainCharacter	)
			var selfTeamID	=	PlayerStatus.MainCharacter.GetComponent(PlayerStatus).TeamID;
		if(	selfTeamID != -1	)
		{
			var diff:int;
			gos	=	FindObjectsOfType(PlayerStatus);
			for( var go : PlayerStatus in gos )
			{
				if(	go.transform	!=	PlayerStatus.MainCharacter	)
					diff = go.TeamID; 
				if (diff== selfTeamID)
				{
					Players[i] = go.transform;
					i +=1;
					if(i>3)
						break;
				}
			}
	   }
	   return	Players;
	}

	static	var ppIDlist : String[];
	private var selfppID : String = "0000";
	private var otherp : PhotonPlayer[];
	private var Pos : Vector3 = Vector3(0,0,0);
	private var dd =false;
	private var remotenum = 10;

	function	ChangeRemoteNum(i : int)
	{
		remotenum	=	i;
	}
//!john.updateposition
	var Positiontable : Hashtable;
	function	UpDatePositiontable(	PlayerID:int,PositionID:String	)
	{
		selfppID	=	PositionID;
		photonView.RPC("SynPositiontable",PhotonTargets.All ,PlayerID,PositionID);
	}

	@RPC
	function	SynPositiontable(	PlayerID:int,PositionID:String)
	{
		if(	Positiontable.ContainsKey(PlayerID)	)
			Positiontable[PlayerID] = PositionID;
		else
			Positiontable.Add(PlayerID,PositionID);
	}

	function	SeePlayer()
	{
		for(	var i : int = 0; i < 3; i++	)
		{ 
			for(	var k : int = 0; k < 3; k++	)
			{ 
				ppIDlist [i*3+k] =(parseInt(selfppID.Substring(0,2))-1+i).ToString() + (parseInt(selfppID.Substring(2,2))-1+k).ToString();   	 
			}
		}
		otherp	=	PhotonNetwork.otherPlayers;
		if(	UIControl.mapType	!=	MapType.zhucheng	)
		{
			for( var go : PhotonPlayer in otherp )
			{
				PhotonNetwork.DoInstantiateYuan(	go,	remotenum	);
	        }
		}
		else
		{
			for( var go : PhotonPlayer in otherp )
			{
				dd = false;     
			    for( var n : int = 0; n < 9; n++ )
			    { 
					if(	go && Positiontable.ContainsKey(go.ID))
					{
						if(	Positiontable[go.ID].ToString()	==	ppIDlist[n]	)
						{
							dd = true;
						}
					}
				}
				if(	dd ) 
				{
					PhotonNetwork.DoInstantiateYuan(go,remotenum);
				}
				else
				{
					PhotonNetwork.DestroyPlayerObjectsYuan(go.ID);
				}
			} 
		}
	}
       
	private var k	=	0;
	function	UpdateMonster()
	{
		MonsterSp	=	FindObjectsOfType(	MonsterSpawn	);
		Monsters	=	GameObject.FindGameObjectsWithTag(	"Enemy"	);
	    MonsterSpt	=	new	Array(	MonsterSp.length	);
		k	=	0;
		for( var i : int = 0; i < MonsterSp.length; i++ )
		{
			if( MonsterSp[i].IsAbleToSpawn() && !MonsterSp[i].IsCleared() )
			{	
		    	MonsterSpt[k]	=	MonsterSp[i].transform;
				k	+=	1;
		    }
	    }
		if(	k	==	MonsterSp.length	)
			return;
		for( var n : int = 0; n < Monsters.length; n++ )
		{ 
			if(	k	<	MonsterSpt.length && MonsterSpt[k]	==	null	&&	Monsters[n]	)
			{	
		    	MonsterSpt[k]	=	Monsters[n].transform;
		    	k	+=	1;
		    }  
	    }
	}

	function	UpdateNPCposition()
	{
		NPC		=	GameObject.FindGameObjectsWithTag("NPC");
		var	NPCdot	:	Transform[];	
		NPCdot	=	new	Array( NPC.length );
		for( var i : int = 0; i < NPC.length; i++)
		{
			NPCdot[i] = Instantiate (greendot) ;
			NPCdot[i].parent = this.transform;
			NPCdot[i].localScale = greendot.localScale;	
		    NPCdot[i].localPosition.x = (NPC[i].transform.position.x-offsetx)*80/mcamera.mapsize;
		    NPCdot[i].localPosition.y = (NPC[i].transform.position.z-offsety)*80/mcamera.mapsize;
		    NPCdot[i].localPosition.z =	0;
		}
	}

	function	UpdatePortalosition()
	{
		Portal	=	FindObjectsOfType(TriggerLoadLevel);
		ExitPortal	=	FindObjectOfType(TriggerExit); 
		var Portalp : Transform;
		var Portalh : Transform;
		if(	Portal.length	>=	1	)
			Portalp = Portal[0].transform;
		else 
		if(	ExitPortal	)
			Portalp = ExitPortal.transform;
		if(	Portalp	)
		{
			ePortal.localPosition.x = (Portalp.position.x-offsetx)*80/mcamera.mapsize;
			ePortal.localPosition.y = (Portalp.position.z-offsety)*80/mcamera.mapsize;
		}
		if(	Portal.length>=2	)
			Portalh = Portal[1].transform;
		if(	Portalh	)
		{
			mPortal.localPosition.x = (Portalh.position.x-offsetx)*80/mcamera.mapsize;
			mPortal.localPosition.y = (Portalh.position.z-offsety)*80/mcamera.mapsize;
		}
	}

	function	GreatReddot()
	{
		Monsterdot	=	new Array (MonsterSpt.length);
		for( var i : int = 0; i < MonsterSpt.length; i++ )
		{ 
			Monsterdot[i]					=	Instantiate (reddot);
			Monsterdot[i].parent			=	this.transform;
			Monsterdot[i].localScale		=	reddot.localScale;
			Monsterdot[i].localPosition.z	=	0;
	    }
	}

	function	Updateposition()
	{
		try
		{
			for (var i : int = 0; i < Monsterdot.length; i++)
			{
				if(	MonsterSpt[i]	)
				{	
					Monsterdot[i].localPosition.x	=	(MonsterSpt[i].position.x-offsetx)*80/mcamera.mapsize;
					Monsterdot[i].localPosition.y	=	(MonsterSpt[i].position.z-offsety)*80/mcamera.mapsize;
			    }
			    else
					Monsterdot[i].localPosition.y	=	200;
		    }
	    }
	    catch(e)
	    {}
	}
	
	private var zuobiaox : int;
	private var zuobiaoy : int;
	function	UpSelfdot()
	{
		if(	PlayerStatus.MainCharacter	)
		{
			zuobiaox 	=	PlayerStatus.MainCharacter.position.x;
			zuobiaoy	=	PlayerStatus.MainCharacter.position.z;
			selfdot.localPosition.x	=	(zuobiaox-offsetx)*80/mcamera.mapsize;
			selfdot.localPosition.y	=	(zuobiaoy-offsety)*80/mcamera.mapsize;
			selfdot.eulerAngles.z	=	-PlayerStatus.MainCharacter.eulerAngles.y;
			LabelZhuobiao.text		=	zuobiaox +"," + zuobiaoy;
		}
	}
}