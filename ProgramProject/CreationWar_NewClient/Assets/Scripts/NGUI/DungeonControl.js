	#pragma strict
/// <summary>
/// 管理生成怪物 -
/// </summary>
class	DungeonControl extends Song
{
	function	Awake()
	{
		AllManage.dungclStatic = this;
	}
//	function OnLevelWasLoaded (level : int) {
//		if(level != 15 && level != 16){
//			Awake();
//			Start();
//		}
//	}

	static var AllSkiss : GameSkill[];

	static var level : int = 1;
	static var NowMapLevel : int = 1;
	static var ItemStone : String = "";
	static var ItemStoneLevel : String = "";
	static var ItemStoneName : String = "";
	static var ItemStone1 : String = "";
	static var ItemStoneLevel1 : String = "";
	static var ItemStoneName1 : String = "";
	static var ItemYu : String = "";
	static var ItemYuLevel : String = "";
	static var ItemYuName : String = "";
	static var MapName : String;
	var mtw : MainTaskWork; 
	var	MonsterSp	:	MonsterSpawn[];		//当前地图中的怪物生成点//
	var MonsterPrd : SceneTaskObjectMaker[];
	var allClear : boolean = false;
	var DungeonTime : int; 
	static var DungeonJiSha : int;
	var TimesDead : int = 0; 
	var TimesXuePing : int = 0;

	var st : yuan.YuanTimeSpan;
	var yt : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("MapLevel","id");
	var StoreItem : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("GameItem","id");
	var NPCInfo : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("NPCInfo","id");
	var TableGameSkill : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("TableGameSkill","id");
	var BDboss1 : String;
	var BDboss2 : String;
	var BDEnemy1 : String;
	var BDEnemy2 : String;
	var BDEnemy3 : String;
	var BDEnemy4 : String;
	var BDEnemy1Probability : float;
	var BDEnemy2Probability : float;
	var BDEnemy3Probability : float;
	var BDEnemy4Probability : float;
	var mmo : boolean = false;
	var rewardItemStr : String[];
	static var rewardItemStr1 : String[];
	var LabelMap : UILabel;
	private var npccl : NPCControl;
	private var ps : PlayerStatus;
	var RanItem : String = "";
	var RanItemProbability : int = 0;
	static var Kpower : boolean = false;
	var TimeStart : String = "";
	var TimeEnd : String = ""; 
	//static var AssetBundle1 : AssetBundle;

	var yt1	:	yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("" , "");

	function	getMapName(id : String){
		var yyy :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("123" , "1233");
		yyy.Add(yt1.SelectRowEqual("MapID" , id));
		return yyy.Rows[0]["MapName"].YuanColumnText;
	}
	
	function	getMapNameIsNull(id : String) : boolean{
		try{
			var yyy :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("123" , "1233");
			yyy.Add(yt1.SelectRowEqual("MapID" , id));
			return true;
		}catch(e){
			return false;
		}
	}

	function getMapInfo(id : String){
		var yyy :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("123" , "1233");
		yyy.Add(yt1.SelectRowEqual("MapID" , id));
		return yyy.Rows[0]["MapInfo"].YuanColumnText;
	}

	function getMapLevel(id : String , nandu : int){
		var yyy :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("123" , "1233");
		yyy.Add(yt1.SelectRowEqual("MapID" , id));
		return yyy.Rows[0]["MapLevel" + nandu].YuanColumnText;
	}

	function GetMapLevelAsMapID(mapID : String , nandu : int) : int{
		var ytGet = new yuan.YuanMemoryDB.YuanTable("MapLevelytGet","idytGet");
		ytGet.Add(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("MapID" , mapID));
		try
		{
			return parseInt(ytGet.Rows[0]["MapLevel" + nandu].YuanColumnText);			 		
		}
		catch(e)
		{
			return parseInt(ytGet.Rows[0]["MapLevel1"].YuanColumnText);			 					
		}
	}

	var DotaSpawn1 : MonsterSpawn[];
	var DotaSpawn2 : MonsterSpawn[];
	var alreadySetYu : boolean = false;
	static var staticRoomSP : ExitGames.Client.Photon.Hashtable = new ExitGames.Client.Photon.Hashtable();
	var readedRoomSP : boolean = false;
	
//	function OnLevelWasLoaded (level : int) {
//		yt = new yuan.YuanMemoryDB.YuanTable("MapLevel","id");
//	}
	
	var rewardIsDone : boolean = false;
	function Start()
	{
		pickGold = 0;
		pickBlood = 0;
		TimesDead = 0;
		TimesXuePing = 0;
		timeSpawn = new System.TimeSpan();
		DungeonTime = 0;
		isAlready = false;
//		print("1111111");
		DungeonIsDone = false;
		rewardIsDone = false;
//	    NowMapLevel = 1;
		if(Application.loadedLevelName.Substring(3,1) == "1")
		{
			NowMapLevel = 1;
		}else
		if(Application.loadedLevelName == "Map441")
		{
			NowMapLevel = PVPControl.PVPTaskNanDu;
	    }else
		if(Application.loadedLevelName == "Map200")
		{
			NowMapLevel = 1;
		}
		else
		{
			try{
				var roomName : String;
//				if(PhotonNetwork.room)
				roomName = Loading.nowRoomName; //PhotonNetwork.room.name;
				if(roomName.Length > 6)
				{
					////print("NowMapLevel == " + NowMapLevel + "rrrrr"+roomName);
					NowMapLevel = parseInt(roomName.Substring(6,1));
				}
			}
			catch(e)
			{
				NowMapLevel = parseInt(Loading.useNandu);
			}
		}
//		print(PhotonNetwork.room.name + "  LLLLLLLLLLLLLLL "  + NowMapLevel);
		mtwMapID = mtw.thisMapID;
		StartSetAttr();
		
	}

	var isAlready : boolean = false;
	var mtwMapID : String;
	//设置关卡内的相关数据//
	function	StartSetAttr () 
	{
	//	if(staticRoomSP)
	//		//print(staticRoomSP.Count);
	//		//print(staticRoomSP);
		TimeStart	=	InRoom.GetInRoomInstantiate().serverTime.ToString();
		Kpower = true; 
		st = new yuan.YuanTimeSpan(); 
		allClear = false;
		rewardItemStr = new Array(3);
		rewardItemStr1 = new Array(3);
		
		yt1 = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel;
		
		if(yt1 == null)
		{
			return;
		}
		if(yt.Count != 0)
		{
			yt = new yuan.YuanMemoryDB.YuanTable("MapLevel","id");
		}
		if(yt.Count == 0)
		{
			yt.Add(yt1.SelectRowEqual("MapID" , mtwMapID));
		}
		StoreItem =  YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem;
		NPCInfo =  YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytNPCInfo;
		TableGameSkill =  YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameSkill;
		if(	yt.Count	>	0	&&	StoreItem.Count	>	0	&&	NPCInfo.Count	>	0	)
		{
			MapName = yt.Rows[0]["MapName"].YuanColumnText;
			try
			{
				level = parseInt(yt.Rows[0]["MapLevel" + NowMapLevel].YuanColumnText);			 		
			}
			catch(e)
			{
				level = parseInt(yt.Rows[0]["MapLevel1"].YuanColumnText);			 					
			}
			BDboss1 = (yt.Rows[0]["Boss1"].YuanColumnText);		 
			BDboss2 = (yt.Rows[0]["Boss2"].YuanColumnText);		 
//			print(BDboss1 + " ===  " + BDboss2);
			BDEnemy1 = (yt.Rows[0]["Enemy1"].YuanColumnText);		 
			BDEnemy2 = (yt.Rows[0]["Enemy2"].YuanColumnText);		 
			BDEnemy3 = (yt.Rows[0]["Enemy3"].YuanColumnText);		 
			BDEnemy4 = (yt.Rows[0]["Enemy4"].YuanColumnText);		
	//		print(yt.Rows[0]["MapName"].YuanColumnText  +" - "+yt.Rows[0]["Enemy1"].YuanColumnText + " - " + yt.Rows[0]["Enemy2"].YuanColumnText + " - " +yt.Rows[0]["Enemy3"].YuanColumnText + " - " +yt.Rows[0]["Enemy4"].YuanColumnText + " - ") ; 
			BDEnemy1Probability = GetBDInfoInt("Enemy1Probability" , 0);
			BDEnemy2Probability = GetBDInfoInt("Enemy2Probability" , 0); 
			BDEnemy3Probability = GetBDInfoInt("Enemy3Probability" , 0); 
			BDEnemy4Probability = GetBDInfoInt("Enemy4Probability" , 0); 
			
//	function isTowerMap() : boolean{
//		if(Application.loadedLevelName == "Map721"){
//			return true;
//		}
//		return false;
//	}
//
//	function GetTowerTable(towerNum : int) : yuan.YuanMemoryDB.YuanRow{
//		return YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBosstower.SelectRowEqual("id" , towerNum.ToString());
//	}
			
			
//			if(Application.loadedLevelName == "Map721"){
//				ytTower = new yuan.YuanMemoryDB.YuanTable("bosstower1","id");
//				ytTower.add
//			}
			
			RanItem = yt.Rows[0]["RanItem"].YuanColumnText;
			RanItemProbability = GetBDInfoInt("RanItemProbability" , 0); 
			if(NowMapLevel == 5)
			{
				RanItem = yt.Rows[0]["ArcRanItem"].YuanColumnText;
				RanItemProbability *= 5;
			}
			mmo = true;
		}
	//	print(level + " == level == " + NowMapLevel + " == mtwMapID" + mtwMapID);
		if(!isAlready)
		{
			ItemStone = GetItemIDAsLevelAndType((Mathf.Clamp(10 * level / 80 + 1,1,10)).ToString() , Random.Range(3,5).ToString());
			if(ItemStone.Length > 3)
			{
				ItemStone = ItemStone.Substring(0,1) + "4" + ItemStone.Substring(2,2);
				ItemStone1 = ItemStone.Substring(0,1) + "5" + ItemStone.Substring(2,2);
				ItemStoneName = GetItemNameAsID(ItemStone);
				ItemStoneLevel = GetItemLevelAsID(ItemStone);
				ItemStoneName1 = GetItemNameAsID(ItemStone1);
				ItemStoneLevel1 = GetItemLevelAsID(ItemStone1);
			
				ItemYu = GetItemIDAsLevelAndType((Mathf.Clamp(10 * level / 80 + 1,1,10)).ToString() , "1");
				ItemYuName = GetItemNameAsID(ItemYu);
				ItemYuLevel = GetItemLevelAsID(ItemYu);
			}
		}
		alreadySetYu = true;
		if(parseInt(BDEnemy1)>0 && GetNPCIDAsID(BDEnemy1).Length > 2)
			Monsterprefab1 = Resources.Load("5" +GetNPCIDAsID(BDEnemy1).Substring(1,2) + "00", GameObject);
		if(parseInt(BDEnemy2)>0 && GetNPCIDAsID(BDEnemy2).Length > 2)	
			Monsterprefab2 = Resources.Load("5" +GetNPCIDAsID(BDEnemy2).Substring(1,2) + "00", GameObject);
		if(parseInt(BDEnemy3)>0 && GetNPCIDAsID(BDEnemy3).Length > 2)
			Monsterprefab3 = Resources.Load("5" +GetNPCIDAsID(BDEnemy3).Substring(1,2) + "00",GameObject);
		if(parseInt(BDEnemy4)>0 && GetNPCIDAsID(BDEnemy4).Length > 2)
			Monsterprefab4 = Resources.Load("5" +GetNPCIDAsID(BDEnemy4).Substring(1,2) + "00", GameObject);
		if(parseInt(BDboss1)>0 && GetNPCIDAsID(BDboss1).Length > 2)
			Monsterprefab5 = Resources.Load("5" +GetNPCIDAsID(BDboss1).Substring(1,2) + "00",GameObject);
		if(parseInt(BDboss2)>0 && GetNPCIDAsID(BDboss2).Length > 2)
			Monsterprefab6 = Resources.Load("5" +GetNPCIDAsID(BDboss2).Substring(1,2) + "00", GameObject);          
		AddAllSkiss();
		LabelMap.text = MapName;
		AllManage.tsStatic.Show1(MapName);
		var float1 : float;
		var float2 : float;
		var float3 : float;
		var float4 : float;
		var f2 : int;
		var f3 : int;
		var f4 : int;
		f2 = BDEnemy1Probability + BDEnemy2Probability;
		f3 = BDEnemy1Probability + BDEnemy2Probability + BDEnemy3Probability;
		f4 = BDEnemy1Probability + BDEnemy2Probability + BDEnemy3Probability + BDEnemy4Probability;	
		
		//--------------------------------------------------------生成怪物信息初始化--------------------------------------------------------//
		
		MonsterSp	=	FindObjectsOfType(MonsterSpawn);	//获取所有的怪物生成点//
		
		var i : int = 0;
		var useInt : int = 0;
		var useSpStr : String = "";
		var strSP1 : String = "";
		var strSP2 : String = "";
		for(	i=0; i<MonsterSp.length;	i++	)
		{		
			if(	PhotonNetwork.room	)
			{
				useSpStr	=	PhotonNetwork.room.customProperties[ "sp" + MonsterSp[i].viewID	]; 
			}
			else
			{
				useSpStr	=	null;
			}
			if(	staticRoomSP	!=	null	&&	useSpStr	==	null	)
				useSpStr = staticRoomSP["sp" + MonsterSp[i].viewID];  
			if(useSpStr != null && useSpStr.Length > 1){
				strSP1 = useSpStr.Substring(0,1);
				strSP2 = useSpStr.Substring(1,1); 					
//				if(strSP1 == "0")
//					MonsterSp[i].pp = false;
//				else
//					MonsterSp[i].pp = true;
//				if(strSP2 == "0")
//					MonsterSp[i].clear = false;
//				else
//					MonsterSp[i].clear = true;
				if( !MonsterSp[i].IsCleared())
				{
					//MonsterSp[i].pp = true;
					staticRoomSP["sp" + MonsterSp[i].viewID] = "10";		
				}		
			}
			else
			{
	//			var RoomSP = new ExitGames.Client.Photon.Hashtable();
	//		    RoomSP.Add("sp" + MonsterSp[i].viewID ,"10");
	//		    PhotonNetwork.room.SetCustomProperties(RoomSP);
	//		   	PhotonNetwork.SetPlayerCustomProperties(RoomSP);	    		
				staticRoomSP.Add("sp" + MonsterSp[i].viewID ,"10");
			}
		}

		if(PhotonNetwork.connected && PhotonNetwork.isMasterClient)
		PhotonNetwork.room.SetCustomProperties(staticRoomSP);
		
		if(	Application.loadedLevelName	==	"Map441"	)
		{
			useInt = 0;
			for(i=0; i<MonsterSp.length; i++){
				if(MonsterSp[i].DotaTeam == 0){
					DotaSpawn1[useInt] = MonsterSp[i];
					if(useInt <= DotaSpawn1.length / 2){
						DotaSpawn1[useInt].SetMonsterAsID("xiaoguai" , Monsterprefab1 , GetNPCNameAsID(BDEnemy1) , GetNMRowAsID(BDEnemy1));
					}else{
						DotaSpawn1[useInt].SetMonsterAsID("xiaoguai" , Monsterprefab2 , GetNPCNameAsID(BDEnemy2) , GetNMRowAsID(BDEnemy2));				
					}
					useInt += 1;
				}
			}
			useInt = 0;
			for(i=0; i<MonsterSp.length; i++){
				if(MonsterSp[i].DotaTeam == 1){
					DotaSpawn2[useInt] = MonsterSp[i];
					if(useInt <= DotaSpawn2.length / 2){
						DotaSpawn2[useInt].SetMonsterAsID("xiaoguai" , Monsterprefab3 , GetNPCNameAsID(BDEnemy3) , GetNMRowAsID(BDEnemy3));
					}else{
						DotaSpawn2[useInt].SetMonsterAsID("xiaoguai" , Monsterprefab4 , GetNPCNameAsID(BDEnemy4) , GetNMRowAsID(BDEnemy4));				
					}
					useInt += 1;
				}
			}
		}
		else	//MapName != "Map441"//不是竞技场——
		{
			for(	i=0; i<MonsterSp.length; i++)
			{ 
				float1	=	i;   
				float2	=	MonsterSp.length; 
				float3	=	float1	/	float2; 
				float4	=	BDEnemy1Probability + BDEnemy2Probability + BDEnemy3Probability + BDEnemy4Probability;
				if(MonsterSp[i].spType == SpawnPointType.Enemy)
				{
					if(float3 < BDEnemy1Probability / float4)
					{
						if(BDEnemy1Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab1 , GetNPCNameAsID(BDEnemy1) , GetNMRowAsID(BDEnemy1));
					}else
					if(float3 < f2 / float4)
					{
						if(BDEnemy2Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab2 , GetNPCNameAsID(BDEnemy2) , GetNMRowAsID(BDEnemy2));
					}else 
					if(float3 < f3 / float4)
					{
						if(BDEnemy3Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab3 , GetNPCNameAsID(BDEnemy3) , GetNMRowAsID(BDEnemy3));			
					}else
					if(float3 <= f4 / float4)
					{
						if(BDEnemy4Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab4 , GetNPCNameAsID(BDEnemy4) , GetNMRowAsID(BDEnemy4));		
					}
				}else
				if(MonsterSp[i].spType == SpawnPointType.boss1)
				{
						MonsterSp[i].SetMonsterAsID("boss" , Monsterprefab5  , GetNPCNameAsID(BDboss1) , GetNMRowAsID(BDboss1));			
				}else
				if(MonsterSp[i].spType == SpawnPointType.boss2)
				{
						MonsterSp[i].SetMonsterAsID("boss" , Monsterprefab6  , GetNPCNameAsID(BDboss2) , GetNMRowAsID(BDboss2));			
				}
			}
		}
		
		//--------------------------------------------------------生成怪物信息初始化--------------------------------------------------------//
		
		var useRan : int = 0;
		rewardItemStr[0] = GetRewardItemIDAsLevel((Mathf.Clamp(17 * level / 80,1,17)).ToString());
		rewardItemStr[1] = GetRewardItemIDAsLevel((Mathf.Clamp(17 * level / 80 - 1,1,17)).ToString());
		rewardItemStr[2] = GetRewardItemIDAsLevel((Mathf.Clamp(17 * level / 80 + 1,1,17)).ToString());
		useRan = Random.Range(0,100);
		if(useRan > 98 && rewardItemStr[0].Length > 1){
			rewardItemStr[0] = rewardItemStr[0].Substring(0,2) + Random.Range(18,25);
		}
		useRan = Random.Range(0,100);
		if(useRan > 98 && rewardItemStr[1].Length > 1){
			rewardItemStr[1] = rewardItemStr[1].Substring(0,2) + Random.Range(18,25);
		}
		useRan = Random.Range(0,100);
		if(useRan > 98 && rewardItemStr[2].Length > 1){
			rewardItemStr[2] = rewardItemStr[2].Substring(0,2) + Random.Range(18,25);
		}
		rewardItemStr1[0] = rewardItemStr[0] + ",0" + Random.Range(1,3);
		rewardItemStr1[1] = rewardItemStr[1] + ",0" + Random.Range(1,3);
		rewardItemStr1[2] = rewardItemStr[2] + ",0" + Random.Range(1,3);
		yield;	
		npccl = FindObjectOfType(NPCControl);
		if(npccl){
			npccl.SetNPCName(NPCInfo);	
		}
		yield;
		MonsterPrd = FindObjectsOfType(SceneTaskObjectMaker);
		if(Application.loadedLevelName == "Map441"){
			PVPControl.PVPTaskNanDu = 1;
		}
	//	//print("NowMapLevel ==== " + NowMapLevel);
		
		if(PhotonNetwork.room){
			useSpStr = PhotonNetwork.room.customProperties["DungeonDone"]; 
		}else{
			useSpStr = null;
		}
		if(staticRoomSP != null && useSpStr == null)
			useSpStr = staticRoomSP["DungeonDone"];  

		if(useSpStr != null){
			if(useSpStr == "1"){
				wanchengFuBen();
			}
		}else{
			try{
				staticRoomSP.Add("DungeonDone" ,"0");
			}catch(e){
			
			}
		}
		if(staticRoomSP != null && useSpStr == null)
			useSpStr = staticRoomSP["DungeonDone"];  
		if(useSpStr == "1"){
				wanchengFuBen();
		}
		
		if(PhotonNetwork.connected && PhotonNetwork.isMasterClient)
		PhotonNetwork.room.SetCustomProperties(staticRoomSP);
		yield;
		yield;
		yield;
		yield;
		yield WaitForSeconds(2);
		readedRoomSP = true;
		isAlready = true;
	}

	function YieldStartSetAttr(){
		yield	StartSetAttr () ;
		AllManage.mtwStatic.ClearDungeon();
	}

	function SetTowerMonsters(towerNum : int){
		if(isTowerMap()){
			BDboss1 = "0";
			BDboss2 = "0";
			BDEnemy1 = "0";
			BDEnemy2 = "0";
			BDEnemy3 = "0";
			BDEnemy4 = "0";
			allClear = false;
			var rows : yuan.YuanMemoryDB.YuanRow;
			rows = GetTowerTable(towerNum);
			
			var monsterList : String = rows["monsterlist"].YuanColumnText;
			var bossid : String = rows["bossid"].YuanColumnText;
			var towerMonsters : String[];
			var towerMonsterNum : float = 0;
			level = parseInt(rows["level"].YuanColumnText);
			towerMonsters = monsterList.Split(";"[0]);
			NowMapLevel = 5;
			if(towerMonsters.length > 0){
				towerMonsterNum += 1;
				BDEnemy1 = towerMonsters[0];
			}
			if(towerMonsters.length > 1){
				towerMonsterNum += 1;
				BDEnemy2 = towerMonsters[1];
			}
			if(towerMonsters.length > 2){
				towerMonsterNum += 1;
				BDEnemy3 = towerMonsters[2];
			}
			if(towerMonsters.length > 3){
				towerMonsterNum += 1;
				BDEnemy4 = towerMonsters[3];
			}
			if(bossid != "")
				BDboss1 = bossid;
			
			BDEnemy1Probability = 100.0 / towerMonsterNum;
			BDEnemy2Probability = 100.0 / towerMonsterNum;
			BDEnemy3Probability = 100.0 / towerMonsterNum;
			BDEnemy4Probability = 100.0 / towerMonsterNum;

			if(parseInt(BDEnemy1)>0 && GetNPCIDAsID(BDEnemy1).Length > 2)
				Monsterprefab1 = Resources.Load("5" +GetNPCIDAsID(BDEnemy1).Substring(1,2) + "00", GameObject);
			if(parseInt(BDEnemy2)>0 && GetNPCIDAsID(BDEnemy2).Length > 2)	
				Monsterprefab2 = Resources.Load("5" +GetNPCIDAsID(BDEnemy2).Substring(1,2) + "00", GameObject);
			if(parseInt(BDEnemy3)>0 && GetNPCIDAsID(BDEnemy3).Length > 2)
				Monsterprefab3 = Resources.Load("5" +GetNPCIDAsID(BDEnemy3).Substring(1,2) + "00",GameObject);
			if(parseInt(BDEnemy4)>0 && GetNPCIDAsID(BDEnemy4).Length > 2)
				Monsterprefab4 = Resources.Load("5" +GetNPCIDAsID(BDEnemy4).Substring(1,2) + "00", GameObject);
			if(parseInt(BDboss1)>0 && GetNPCIDAsID(BDboss1).Length > 2)
				Monsterprefab5 = Resources.Load("5" +GetNPCIDAsID(BDboss1).Substring(1,2) + "00",GameObject);
			if(parseInt(BDboss2)>0 && GetNPCIDAsID(BDboss2).Length > 2)
				Monsterprefab6 = Resources.Load("5" +GetNPCIDAsID(BDboss2).Substring(1,2) + "00", GameObject);          

			var float1 : float;
			var float2 : float;
			var float3 : float;
			var float4 : float;
			var f2 : int;
			var f3 : int;
			var f4 : int;
			f2 = BDEnemy1Probability + BDEnemy2Probability;
			f3 = BDEnemy1Probability + BDEnemy2Probability + BDEnemy3Probability;
			f4 = BDEnemy1Probability + BDEnemy2Probability + BDEnemy3Probability + BDEnemy4Probability;	
					
			MonsterSp	=	FindObjectsOfType(MonsterSpawn);	//获取所有的怪物生成点//

				
			for(var i=0; i<MonsterSp.length; i++)
			{ 
				float1	=	i;   
				float2	=	MonsterSp.length; 
				float3	=	float1	/	float2; 
				float4	=	BDEnemy1Probability + BDEnemy2Probability + BDEnemy3Probability + BDEnemy4Probability;
				if(MonsterSp[i].spType == SpawnPointType.Enemy)
				{
					if(float3 < BDEnemy1Probability / float4)
					{
						if(BDEnemy1Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab1 , GetNPCNameAsID(BDEnemy1) , GetNMRowAsID(BDEnemy1));
					}else
					if(float3 < f2 / float4)
					{
						if(BDEnemy2Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab2 , GetNPCNameAsID(BDEnemy2) , GetNMRowAsID(BDEnemy2));
					}else 
					if(float3 < f3 / float4)
					{
						if(BDEnemy3Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab3 , GetNPCNameAsID(BDEnemy3) , GetNMRowAsID(BDEnemy3));			
					}else
					if(float3 <= f4 / float4)
					{
						if(BDEnemy4Probability > 0)
							MonsterSp[i].SetMonsterAsID("xiaoguai" , Monsterprefab4 , GetNPCNameAsID(BDEnemy4) , GetNMRowAsID(BDEnemy4));		
					}
				}else
				if(MonsterSp[i].spType == SpawnPointType.boss1)
				{
						MonsterSp[i].SetMonsterAsID("boss" , Monsterprefab5  , GetNPCNameAsID(BDboss1) , GetNMRowAsID(BDboss1));			
				}else
				if(MonsterSp[i].spType == SpawnPointType.boss2)
				{
						MonsterSp[i].SetMonsterAsID("boss" , Monsterprefab6  , GetNPCNameAsID(BDboss2) , GetNMRowAsID(BDboss2));			
				}
			}
		}
	}

	function isTowerMap() : boolean{
		if(Application.loadedLevelName == "Map721"){
			return true;
		}
		return false;
	}

	function GetTowerTable(towerNum : int) : yuan.YuanMemoryDB.YuanRow{
		return YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBosstower.SelectRowEqual("id" , towerNum.ToString());
	}

	function SetMonsterStaticRoom(){
		PhotonNetwork.room.SetCustomProperties(staticRoomSP);
	//	staticRoomSP["sp" + viewID] = str;
	}

	private var Monsterprefab1:GameObject;
	private var Monsterprefab2:GameObject;
	private var Monsterprefab3:GameObject;
	private var Monsterprefab4:GameObject;
	private var Monsterprefab5:GameObject;
	private var Monsterprefab6:GameObject;

	private var fbxPrefab1 :GameObject;
	private var fbxPrefab2 :GameObject;
	function AddAllSkiss(){
		AllSkiss = new Array();
		fbxPrefab1 = Resources.Load("MonsterSkillO1", GameObject);
		fbxPrefab2 = Resources.Load("MonsterSkillO2", GameObject);	
		for(var rows : yuan.YuanMemoryDB.YuanRow in TableGameSkill.Rows){
			AddAS(rows);
		}

	}

	function AddAS(rows : yuan.YuanMemoryDB.YuanRow){
		var useArray : GameSkill[];
		useArray = AllSkiss;
		AllSkiss = new Array(useArray.length + 1);
		for(var i=0; i<useArray.length; i++){
			AllSkiss[i] = useArray[i];
		}
		AllSkiss[useArray.length] = new GameSkill();
		AllSkiss[useArray.length].sName = rows["Name"].YuanColumnText;
		AllSkiss[useArray.length].skillType = GetSKInfoInt(rows,"Type" , 0 );
		AllSkiss[useArray.length].startID = GetSKInfoInt(rows,"StartID" , 0 );
		AllSkiss[useArray.length].middleID = GetSKInfoInt(rows,"MiddleID" , 0 );
		AllSkiss[useArray.length].endID = GetSKInfoInt(rows,"EndID" , 0 );
		AllSkiss[useArray.length].scope = GetSKInfoInt(rows,"Scope" , 0 );
		AllSkiss[useArray.length].damageValue = GetSKInfoInt(rows,"DamageValue" , 0 );
		AllSkiss[useArray.length].damageType = GetSKInfoInt(rows,"DamageType" , 0 );
		AllSkiss[useArray.length].buffID = GetSKInfoInt(rows,"BuffID" , 0 );
		AllSkiss[useArray.length].buffValue = GetSKInfoInt(rows,"BuffValue" , 0 );
		AllSkiss[useArray.length].buffTime = GetSKInfoInt(rows,"BuffTime" , 0 );
		AllSkiss[useArray.length].sType = GetSKInfoInt(rows,"SkillType" , 0 );
		AllSkiss[useArray.length].info = rows["Info"].YuanColumnText;
		AllSkiss[useArray.length].batterNum = GetSKInfoInt(rows,"BatterNum" , 0 );
		AllSkiss[useArray.length].CoolDown = GetSKInfoInt(rows,"CoolDown" , 0 );
		AllSkiss[useArray.length].Cost = GetSKInfoInt(rows,"Expend" , 0 );
		AllSkiss[useArray.length].skillTime = GetSKInfoInt(rows,"skillTime" , 0 );
		  switch (AllSkiss[useArray.length].scope)
	 {				
	 case 0:
	 case 1:
	 case 2:
	 case 3: 
	 	AllSkiss[useArray.length].fxobject = fbxPrefab1;
	 break;	

	 case 4:
	 	AllSkiss[useArray.length].fxobject = fbxPrefab2;
	 break;
	  }
	}


	function GetSKInfoInt(rows : yuan.YuanMemoryDB.YuanRow,bd : String , it : int) : int{  
		var iii : int = 0;
		try{ 
			iii = parseInt(rows[bd].YuanColumnText);
			return  iii;
		}catch(e){
			return it;	
		}
	}

	function GetNMRowAsID(id : String) : yuan.YuanMemoryDB.YuanRow{
		for(var rows : yuan.YuanMemoryDB.YuanRow in NPCInfo.Rows){
			if(rows["id"].YuanColumnText == id){
				return rows;
			}
		}
		return null;	
	}

	function GetItemIDAsLevel(level : String) : String{
		for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
			if(rows["LevelID"].YuanColumnText == level){
				return rows["ItemID"].YuanColumnText;
			}
		}
		return "";
	}

	function GetRewardItemIDAsLevel(level : String) : String{
		for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
			if(rows["ItemType"].YuanColumnText == "5"){
				if(rows["LevelID"].YuanColumnText == level){
					return rows["ItemID"].YuanColumnText;
				}		
			}
		}
		return "";
	}

	function GetItemIDAsLevelAndType(level : String , Type : String) : String{
	//	//print(level + " =====================lasjkfdlsajkdflas");
		for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
			if(rows["LevelID"].YuanColumnText == level && rows["ItemType"].YuanColumnText == Type){
				return rows["ItemID"].YuanColumnText;
			}
		}
		return "";
	}

	function GetItemNameAsID(ID : String){
	//	//print(ID + " ====aksjdflasjdflajsdf");
		for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
			if(rows["ItemID"].YuanColumnText == ID.Substring(0,4)){
	//			//print("rows[LevelID].YuanColumnText == " + rows["LevelID"].YuanColumnText);
				return rows["Name"].YuanColumnText;
			}
		}
		return "";

	}

	function GetItemLevelAsID(ID : String){
	//	//print(ID + " ====aksjdflasjdflajsdf");
		for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
			if(rows["ItemID"].YuanColumnText == ID){
				return rows["LevelID"].YuanColumnText;
			}
		}
		return "";
	}

	function GetItemValueAsID(ID : String) : String{
	//	//print(ID + " ====aksjdflasjdflajsdf");
		for(var rows : yuan.YuanMemoryDB.YuanRow in StoreItem.Rows){
			if(ID.Length > 3 && rows["ItemID"].YuanColumnText == ID.Substring(0,4)){
				return rows["ItemValue"].YuanColumnText;
			}
		}
		return "";
	}

	function GetNPCNameAsID(id : String) : String{
	//	//print(id + " ===== id ");
		for(var rows : yuan.YuanMemoryDB.YuanRow in NPCInfo.Rows){
			if(rows["id"].YuanColumnText == id){
				return rows["NPCName"].YuanColumnText;
			}
		}
		return "";
	}

	function GetNPCNameAsbianhao(id : String) : String{
	//	//print(id + " ===== id ");
		for(var rows : yuan.YuanMemoryDB.YuanRow in NPCInfo.Rows){
			if(rows["NPCID"].YuanColumnText == id){
	//			//print(rows["NPCName"].YuanColumnText);
				return rows["NPCName"].YuanColumnText;
			}
		}
		return "";
	}

	function GetNPCIDAsID(id : String) : String{
	//	//print(id + " ===== id ");
		for(var rows : yuan.YuanMemoryDB.YuanRow in NPCInfo.Rows){
			if(rows["id"].YuanColumnText == id){
				return rows["NPCID"].YuanColumnText;
			}
		}
		return "";
	}

	function GetBDInfoInt(bd : String , it : int) : int{  
		var iii : int = 0;
		try{ 
			iii = parseInt(yt.Rows[0][bd].YuanColumnText);
			return  iii;
		}catch(e){
			return it;	
		}
	}

	function isDungeonClear() : boolean{
		var isC : boolean;
		var i : int = 0;
		if(MonsterSp.length > 0 || MonsterPrd.length > 0){
			isC = true;
		}
		for(i=0; i<MonsterSp.length; i++){
			if(!MonsterSp[i].IsCleared() && (MonsterSp[i].spType == SpawnPointType.boss1 || MonsterSp[i].spType == SpawnPointType.boss2)){
				isC = false;
			}
		}
		for(i=0; i<MonsterPrd.length; i++){
			if(!MonsterPrd[i].clear){
				isC = false;
			}
		}
		return isC;
	}

	function isAutoAttackDungeonClear() : boolean{
		var isC : boolean;
		var i : int = 0;
		if(MonsterSp.length > 0 || MonsterPrd.length > 0){
			isC = true;
		}
		for(i=0; i<MonsterSp.length; i++){
			if(!MonsterSp[i].IsCleared()){
				isC = false;
			}
		}
		for(i=0; i<MonsterPrd.length; i++){
			if(!MonsterPrd[i].clear){
				isC = false;
			}
		}
		return isC;
	}

	function canBackTown(){
		if(isDungeonClear()){
			return false;
		}else{
			return true;
		}
	}

	var useNextMap : String;
	var useNextTrigger : GameObject;
	var useEnd : GameObject;
	function LookBoss () 
	{
		allClear = isDungeonClear();
		if(allClear && Application.loadedLevelName != "Map200" && !alreadyRewards){
			if(Application.loadedLevelName != "Map911" && Application.loadedLevelName != "Map912"){
				wanchengFuBen();
				ServerRequest.requestChangeMapState(1);
			}else{
				AllManage.UIALLPCStatic.show48();
			}
		}
	}

	function LookTowerMonster(){
		allClear = isAutoAttackDungeonClear();
		if(allClear){
			yield Showwin();
			ServerRequest.requestTowerFloorFinish(UIControl.towerNum);
			AllManage.UICLStatic.TowerFloorFinish();
		}
	}

	var DungeonIsDone : boolean = false;
	///副本完成时调用//
	function	wanchengFuBen()
	{	
//		AllManage.pAIStatic.StopAutoAttack();
//		AllManage.pAIStatic.AutoAIType = CommonDefine.AutoNON;
		Debug.Log(	"K__________wanchengFuBen"	);
		DungeonIsDone	=	true;
		var	MAI :MonsterAI[]	=	FindObjectsOfType(MonsterAI)as MonsterAI[];
		for(	var go : MonsterAI in MAI	)
		{
			go.gameObject.SendMessage(	"PauseAttack",	true,	SendMessageOptions.DontRequireReceiver	); 
		}
		if(AllManage.pAIStatic)
			AllManage.pAIStatic.StopAutoAttack();
//		var gos : GameObject[];
//		gos	=	GameObject.FindGameObjectsWithTag("Enemy"); 
//		for(	var go : GameObject in gos	)
//		{
//			go.SendMessage("PauseAttack", true,SendMessageOptions.DontRequireReceiver); 
//		}
		if(NowMapLevel == 5)
		{
				useNextMap = (parseInt(mtw.thisMapID) + 1).ToString();
				if(yt1.SelectRowEqual("MapID" , useNextMap) != null){
					yield WaitForSeconds(3);
					useEnd =  GameObject.Find("end");
					var obj : GameObject;
					obj = GameObject.Instantiate(useNextTrigger);
					obj.transform.position = useEnd.transform.position;
					AllManage.timeDJStatic.Show(gameObject , "lookIsNextMapLevel" , "LaterDone" , 5 , "messages025" , "messages026" , "messages027" , false);
					isHeroDungeonGo = true;
				}else{
					ShowTanKuang();
				}
			}else{
				ShowTanKuang();			
			}
	    	AllManage.dungclStatic.staticRoomSP["DungeonDone"] = "1";
	    	AllManage.dungclStatic.SetMonsterStaticRoom();		   			
	}

	function lookIsNextMapLevel() : boolean{
		var gos : GameObject[];
		gos = GameObject.FindGameObjectsWithTag("Enemy"); 
		for (var go : GameObject in gos) {
			go.SendMessage("PauseAttack", true,SendMessageOptions.DontRequireReceiver); 
		}
		if(useEnd){
			AllManage.mtwStatic.FindWay(useEnd.transform.position);
		}
	}

	var fsNU : UIFilledSprite;
	var selectLater : boolean = false;
	var parentTanKuang : GameObject;
	//private var timeDJ : TimeDaoJi;
	var WinBordUI :UIPanel;
	var WinBord : TweenScale;
	var TweenWin : TweenScale;
	var TweenDun : TweenPosition;
	var emits :  ParticleEmitter[];
	static var alreadyRewards : boolean = false;
	function ShowTanKuang(){
		if(! alreadyRewards){
			DungeonIsDone	=	true;
			alreadyRewards = true;
			yield WaitForSeconds(1);
		    yield Showwin();
		//    Autoctrl.Wayfinding = false;
			mtw.DoneDungeon(mtw.thisMapID , NowMapLevel);
			NowDone();
		}else{
			yield WaitForSeconds(1);
			FanHui();
		}
	}

	function FanHui(){
		if(UIControl.mapType != MapType.zhucheng){
			show0();
			AllManage.timeDJStatic.Show(gameObject , "DungeonClose" , "LaterDone" , 20 , "messages028" , "messages029" , "messages030" , true);
			AllManage.UICLStatic.showDeadYield = false;		}
	}

	function Showwin(){
		AllManage.UICLStatic.showDeadYield = true;
		AllManage.UICLStatic.MakePreWin();
		yield;
		yield;
	    WinBordUI.enabled = true;
		WinBordUI.gameObject.SetActiveRecursively(true);
		TweenWin.gameObject.SetActiveRecursively(false);
		WinBord.Play(true);
		TweenDun.Play(true);
		yield WaitForSeconds(0.3);
		if(WinBord)
			WinBord.audio.Play();
		yield WaitForSeconds(0.5);	
		if(TweenWin)
			TweenWin.Play(true);
		if(TweenWin)
			TweenWin.gameObject.SetActiveRecursively(true);
		yield WaitForSeconds(0.3);	
		if(emits){
			for (var i : int = 0; i < emits.length; i++){
			if(emits[i]){
	      emits[i].emit=true;
	    	}
	    	}
	    }
	    TweenWin.audio.Play();
	     yield WaitForSeconds(0.1);
		if(emits){
		for (var t : int = 0; t < emits.length; t++){
			if(emits[t])
	      emits[t].emit=false;
	    }  
	    }
	    yield WaitForSeconds(1); 
		if(WinBordUI)
	    WinBordUI.gameObject.audio.Play();
//		yield WaitForSeconds(3);
		if(WinBordUI)
		WinBordUI.gameObject.SetActiveRecursively(false);
	}

	var Rtime : int = 0;
	var nowWillDoneCard : boolean = false;
	var Gtime : int = 0;
	var isHeroDungeonGo : boolean = false;
	function Update(){
//		if(Time.time > Rtime && nowWillDoneCard){
//			Rtime = Time.time + 3;
//			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().DoneCard(TimesDead , TimesXuePing , level , FenTime , UseXXStr , pss.length , NowMapLevel , RanItem , pickGold , pickBlood , enemyClear , mtw.MapID));	
//		}
		if(Time.time > Gtime && isHeroDungeonGo){
			Gtime = Time.time + 5;
			AllManage.timeDJStatic.Show(gameObject , "lookIsNextMapLevel" , "LaterDone" , 5 , "messages025" , "messages026" , "messages027" , false);
		}
	}

	var pickGold : int = 0;
	var pickBlood : int;
		var pss : PlayerStatus[];
		var enemyClear : float = 0;
	function DoneCard(){
		if(AllManage.UICLStatic.GroundLiaoTian){
			AllManage.UICLStatic.GroundLiaoTian.SetActiveRecursively(false);
		}
		AllManage.UIALLPCStatic.show30();
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages054");
				AllManage.AllMge.Keys.Add(AllManage.mtwStatic.SaoDangTimes + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelTimesSaoDang);
	//	LabelTimesSaoDang.text = "设定次数剩余：" + AllManage.mtwStatic.SaoDangTimes;
		if(UIControl.mapType == MapType.zhucheng){
			DoneButtons1.localPosition.y = 3000;
			DoneButtons2.localPosition.y = 3000;
		}
		ParentDungeonUse.localPosition.y = -22;
		for(var o=0; o<xingxing; o++){
			TweenXings[o].gameObject.transform.localScale = Vector3(120,120,1);
			TweenXingsIcon[o].enabled = false;
		}
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		UIControl.isYtFuben = false;
		var gos : GameObject[];
	  gos = GameObject.FindGameObjectsWithTag("Enemy"); 
	  for (var go : GameObject in gos) {
	   go.SendMessage("PauseAttack", true,SendMessageOptions.DontRequireReceiver);    
	   }
		timeSpawn = InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(TimeStart);
		FenXiaohao = Mathf.Clamp(1000 - TimesDead*500 - TimesXuePing*100 , 0,1000);
		if(TimesDead != 0 || TimesXuePing != 0){
			AllManage.isDie = true;
		}
		useInt = (level - parseInt(AllManage.psStatic.Level)) * 100 + 500;
		useInt = Mathf.Clamp(useInt , 0 , 1000);
		FenTime = Mathf.Clamp(DungeonTime - timeSpawn.TotalSeconds , -480,480);
		Fzongfen = 500 + FenTime + FenXiaohao + useInt;
		pss = FindObjectsOfType(PlayerStatus);
		
		var numEnemy : float = 0;
		var numClear : float = 0;
		for(var i=0; i<MonsterSp.length; i++){
			if(MonsterSp[i].spType == SpawnPointType.Enemy){
				numEnemy += 1;
				if(MonsterSp[i].IsCleared()){
					numClear += 1;
				}
			}
		}
		enemyClear = numClear / numEnemy;
	//	print(enemyClear + " == " + numClear + "/" + numEnemy);
//		AllManage.tsStatic.RefreshBaffleOn();
		if(NowMapLevel ==1){
//			print("putong == " + LookThisMapIsDone(mtw.MapID + NowMapLevel , UseXXStr));
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText;
			UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
	//		InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText = UseXXStr;
		}
		else if(NowMapLevel ==2){
//			print("jingying == " + LookThisMapIsDone(mtw.MapID + NowMapLevel , UseXXStr));
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText;		
			UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
	//		InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText = UseXXStr;
		}
		else{
//			print("jingying == " + LookThisMapIsDone(mtw.MapID + NowMapLevel , UseXXStr));
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText;		
			UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
	//		InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText = UseXXStr;
		}
//		print(UseXXStr + " == " + NowMapLevel + " == "  + mtw.MapID);
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().DoneCard(TimesDead , TimesXuePing , level , FenTime , UseXXStr , pss.length , NowMapLevel , RanItem , pickGold , pickBlood , enemyClear , mtwMapID));
		nowWillDoneCard = true;
//		;	
	//												 DoneCard( timesDead, timesXuePing, mapLevel, fenTime, useXXStr, 																					 psLength,		 nowMapLevel, ranItem, needGold, needBlood, enemyClear)
	//	InRoom.GetInRoomInstantiate().DoneCard(TimesDead , TimesXuePing , level , FenTime , InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText , pss.length , NowMapLevel , RanItem , pickGold , pickBlood);	
		return; 
	}
	var donecardinvs : InventoryItem[];
	function returnDoneCard(objs : Object[]){
		AllManage.CardCLStatic.ObjBtns.SetActive(false);
		if(nowWillDoneCard){
			nowWillDoneCard = false;		
		}else{
			return;
		}
		AllManage.tsStatic.CloseFinger();
		GetBlood = objs[7];
		
		TeamGold = objs[9];
		TeamExp = objs[10];
		if(GetBlood > 0){
			LabelBlood.text = AllManage.AllMge.Loc.Get("info758") + GetBlood;
			if(SpriteBlood){
			SpriteBlood.enabled = true;
			}
		}else{
		if(SpriteBlood){
			SpriteBlood.enabled = false;
		}
		}
		var HeroGroup : int ;
		HeroGroup = objs[8];
		if(HeroGroup<=0){
			ObjHeroGroup.SetActiveRecursively(false);
		}else{
			ObjHeroGroup.SetActiveRecursively(true);
		LabelHoreStone.text = "+" + objs[8];
		}
		
		if(ps.ForTheSoul<=0){
			ObjSoulGroup.SetActiveRecursively(false);
		}else{
			ObjSoulGroup.SetActiveRecursively(true);
		LabelSoul.text = "+ " + ps.ForTheSoul.ToString();
		}
		
		//TD_info.setOverInstance(MapName);
	//	print("shi zle le");
		rewardIsDone = true;
		Fzongfen = objs[4];
		bili= Fzongfen / float1;
		xingxing = objs[3];
		if(NowMapLevel ==1){
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText;
			UseXXStr = DoSetThisMapXXAsID(xingxing * 2, UseXXStr);
			InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText = UseXXStr;
		}
		else if(NowMapLevel ==2){
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText;		
			UseXXStr = DoSetThisMapXXAsID(xingxing * 2, UseXXStr);
			InventoryControl.yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText = UseXXStr;
		}
		else{
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText;		
			UseXXStr = DoSetThisMapXXAsID(xingxing * 2, UseXXStr);
			InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText = UseXXStr;
		}
		var i=0;
		for(i=0; i<xingxing; i++){
			TweenXingsIcon[i].enabled = true;
	//		TweenXings[i].Play(true);
			TweenXings[i].gameObject.SendMessage("Play" , true , SendMessageOptions.DontRequireReceiver);
			yield WaitForSeconds(0.25);
			ParticleXingXings[i].Emit();
			ParticleXingXings[i].audio.Play();
			yield WaitForSeconds(0.1);
			ParticleXingXings[i].emit = false;
		}
	//	if(NowMapLevel < 5){
	//		UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText;
	//		UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
	//		InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText = UseXXStr;
	//	}else{
	//		UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText;		
	//		UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
	//		InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText = UseXXStr;
	//	}
		var useBiliInt : int;
		useBiliInt = bili * 10;
		InventoryControl.yt.Rows[0]["DuplicatePoint"].YuanColumnText = (invcl.GetBDInfoInt("DuplicatePoint" , 0) + useBiliInt).ToString();
		FenExp = objs[6];
	//	GetGold = xingxing * 2 * level * 20;
		GetGold = objs[0];
		
		AllManage.CardCLStatic.ObjBtns.SetActive(true);
		AllManage.CardCLStatic.ObjBtns.transform.localPosition.y = -193;
		
		var ids : String[] = new String[5];

	//	invs = GetThisMapInventoryItems();
		donecardinvs = new Array(5);
		var useinvs : String[];
		for(i=0; i<donecardinvs.length; i++){
			useinvs = objs[2];
			ids[i] = useinvs[i];
	//		print(ids[i] + " == ids[i]");
			donecardinvs[i] = AllResources.InvmakerStatic.GetItemInfo(ids[i], donecardinvs[i]);
	//		print(invs[i]);
		}
//		AllManage.CardCLStatic.GoShowCards(invs , 1);	

//		btnShowEndCard();
		
//		yield WaitForSeconds(1.5);
		useBool = false;
		while( ! useBool ){
			showZongFen = ShowOneValuePlus(LabelZongFen , showZongFen , Fzongfen , Random.Range(11,100) , "", 0);
			yield;
		}
		var TeamGoldStr : String = "";
		useBool = false;
		while( !useBool  ){
			showGold = ShowOneValuePlus(LabelGold , showGold , GetGold - pickGold , Random.Range(11,100) , "+", 0);
			TeamGoldStr = LabelGold.text;
			yield;
		}
		if(pickGold > 0)
			LabelGold.text += "+ " + pickGold; 
		//组队金币
		useBool = false;
		while( !useBool  && TeamGold>0){
			showTeamGold = ShowOneValuePlus(LabelGold , showTeamGold , TeamGold , Random.Range(11,100) , "+", 0);
			
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(TeamGoldStr + "+");
			AllManage.AllMge.Keys.Add(TeamGold.ToString());
			AllManage.AllMge.Keys.Add("info1254");
			AllManage.AllMge.SetLabelLanguageAsID(LabelGold);
			yield;
		}
		
		var TeamExpStr : String = "";
		useBool = false;
		while( !useBool  ){
			showExp = ShowOneValuePlus(LabelExp , showExp , FenExp , Random.Range(11,100) , "+", 0);
			TeamExpStr = LabelExp.text;
			yield;
		}
		
		//组队经验
		useBool = false;
		while( !useBool && TeamExp>0){
			ShowTeamExp = ShowOneValuePlus(LabelExp , ShowTeamExp , TeamExp , Random.Range(11,100) , "+", 0);
			
			AllManage.AllMge.Keys.Clear();
			
			AllManage.AllMge.Keys.Add(TeamExpStr + "+");
			AllManage.AllMge.Keys.Add(TeamExp.ToString());
			AllManage.AllMge.Keys.Add("info1254");
			AllManage.AllMge.SetLabelLanguageAsID(LabelExp);
			yield;
		}
		useBool = false;
		//print(useBiliInt + " == useBiliInt");
		while( !useBool  ){
			showPVE = ShowOneValuePlus(LabelPVEPoint , showPVE , objs[5] , Random.Range(11,100) , "+", 0);
			yield;
		}
		
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(NowMapLevel < 5){
			ps.AddPrestige( -1*NowMapLevel*5);
		}else{
			InventoryControl.yt.Rows[0]["AimHeroMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimHeroMission"].YuanColumnText) + 1).ToString();
			ps.AddPrestige(-30);		
		}
		
	//	ObjDangBan.localPosition.y = 5000;
	//	if(ps == null && PlayerStatus.MainCharacter){
	//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	//	}
	//	ps.AddExperience(xingxing * level * 200);
	//	if(NowMapLevel < 5){
	//		ps.AddPrestige( -1*NowMapLevel*5);
	//	}else{
	//		InventoryControl.yt.Rows[0]["AimHeroMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimHeroMission"].YuanColumnText) + 1).ToString();
	//		ps.AddPrestige(-30);		
	//	}
	//	InventoryControl.yt.Rows[0]["AimFinshMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimFinshMission"].YuanColumnText) + 1).ToString();
		
	//	if(pss.length > 1){
	//		InventoryControl.yt.Rows[0]["AimTeamMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimTeamMission"].YuanColumnText) + 1).ToString();
	//	}
	//	AllManage.AllMge.UseMoney((GetGold + pickGold ) * (-1), pickBlood * (-1) , UseMoneyType.doneCard , gameObject , "");
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	yield;
	//	yield;
	//	yield;
	}
	var EndAudio : AudioSource;
	
	function btnShowEndCard(){
		AllManage.CardCLStatic.GoShowCards(donecardinvs , 1);
		AllManage.CardCLStatic.ObjParent.SetActive(false);
		if(UIControl.mapType == MapType.zhucheng){
		DoneButtons2.localPosition.y = 0;
		DoneButtons1.localPosition.y = 3000;
	}
	if(EndAudio){
		EndAudio.Play();
	}
	}

	function GetThisMapInventoryItems() : InventoryItem[]{
		if(rewardItemStr[0] != "")
		rewardItemStr[0] += ",0" + Mathf.Clamp(xingxing + Random.Range(-1,2) , 1, 3);
		if(rewardItemStr[1] != "")
		rewardItemStr[1] += ",0" + Mathf.Clamp(xingxing + Random.Range(-1,2) , 1, 3);
		if(rewardItemStr[2] != "")
		rewardItemStr[2] += ",0" + Mathf.Clamp(xingxing + Random.Range(-1,2) , 1, 3);	
		
		var invs : InventoryItem[];
		invs = new Array(5);
		var str : String = "";
		if(NowMapLevel == 5){
			str = AllResources.staticLT.MakeItemID1(str, Random.Range(2,4));
			invs[0] = AllResources.InvmakerStatic.GetItemInfo(str, invs[0]);
			
			str = rewardItemStr[Random.Range(0,3)];
			str = RanItem;
			if(str == ""){
				str = AllResources.staticLT.MakeItemID1(str, Random.Range(2,4));		
			}
			invs[1] = AllResources.InvmakerStatic.GetItemInfo(str, invs[1]);
			
			str = AllResources.staticLT.MakeItemID1(str, Random.Range(3,5));
			invs[2] = AllResources.InvmakerStatic.GetItemInfo(str, invs[2]);
			
			str = AllResources.staticLT.MakeItemID1(str, Random.Range(4,6));
			invs[3] = AllResources.InvmakerStatic.GetItemInfo(str, invs[3]);
			
			str = RanItem;
			if(str == ""){
				str = AllResources.staticLT.MakeItemID1(str, Random.Range(4,6));		
			}
			invs[4] = AllResources.InvmakerStatic.GetItemInfo(str, invs[4]);
		}else{
			str = AllResources.staticLT.MakeItemID1(str, Random.Range(1,3));
			invs[0] = AllResources.InvmakerStatic.GetItemInfo(str, invs[0]);
			
			str = rewardItemStr[Random.Range(0,3)];
			if(str == ""){
				str = AllResources.staticLT.MakeItemID1(str, Random.Range(1,3));		
			}
			invs[1] = AllResources.InvmakerStatic.GetItemInfo(str, invs[1]);
			
			str = AllResources.staticLT.MakeItemID1(str, Random.Range(2,4));
			invs[2] = AllResources.InvmakerStatic.GetItemInfo(str, invs[2]);
			
			str = AllResources.staticLT.MakeItemID1(str, Random.Range(3,5));
			invs[3] = AllResources.InvmakerStatic.GetItemInfo(str, invs[3]);
			
			str = RanItem;
			if(str == ""){
				str = AllResources.staticLT.MakeItemID1(str, Random.Range(3,5));		
			}
			invs[4] = AllResources.InvmakerStatic.GetItemInfo(str, invs[4]);	
		}
		return invs;
	}

	function NowDone(){
	//	parentTanKuang.SetActiveRecursively(false);
	//	selectLater = true;
	//	DoneDungeon();
		DoneCard();
	}

	function LaterDone(){
	//	parentTanKuang.SetActiveRecursively(false);
	//	if(!selectLater){
	//		selectLater = true;
	//		yield WaitForSeconds(15);
	//		ShowTanKuang();
	//	}
	}
	function OneXuePing(){
		TimesXuePing += 1;
		AllManage.TimeXuepingNum = TimesXuePing;
	}

	function OneDead(){
		TimesDead += 1;
	}

	var LabelJiSha : UILabel;
	var LabelTime1 : UILabel;
	var LabelTime2 : UILabel;
	var LabelTime3 : UILabel;
	var LabelXiaoHao : UILabel;
	var LabelJiQiao : UILabel;
	var LabelZongFen : UILabel; 
	var LabelHoreStone : UILabel;
	var LabelExp : UILabel;
	var LabelGold : UILabel;
	var LabelBlood : UILabel;
	var SpriteBlood : UISprite;
	var LabelSoul : UILabel;
	var ObjSoulGroup : GameObject;
	var ObjHeroGroup : GameObject;
	var TweenDC : TweenPosition;
	var SpriteStars : UISprite[];
	//var UIALLC : UIAllPanelControl; 
	var GetGold : int;
	var GetBlood : int;
	var JiangLiRandom : int;
	var timeSpawn : System.TimeSpan;
	var ObjDangBan : Transform;
	
	var TeamGold : int ;
	var TeamExp : int ;
	var ParticleXingXings : ParticleEmitter[];
		var useInt : int;
		var FenXiaohao : int;
		var Fzongfen : float;
		var FenTime : int;
		var float1 : float = 2000;
		var bili : float;
		var xingxing : int = 0;
		var FenExp : int;

	private var Fstr : String = ";";
	private var Dstr : String = ",";
	var UseXXStr : String;
	function DoSetThisMapXXAsID(XX : int , NowStr : String) : String{ 
		var bool : boolean = false;
		var useStrs : String[];
		var useMapStrs : String[];
		var i : int = 0;
		useStrs = NowStr.Split(Fstr.ToCharArray());
		for(i=0; i<useStrs.length; i++){
			if(useStrs[i].Length > 2){
				useMapStrs = useStrs[i].Split(Dstr.ToCharArray());
				if(useMapStrs[0] == (mtw.MapID + NowMapLevel)){
					if(XX > parseInt(useMapStrs[1])){
						useMapStrs[1] = XX.ToString(); 
						useStrs[i] = useMapStrs[0] + "," + useMapStrs[1];
					}
					bool = true;
				}
			}
		}
		NowStr = "";
		for(i=0; i<useStrs.length; i++){
			NowStr += useStrs[i] + ";";
		}
		if(!bool){
			NowStr += (mtw.MapID + NowMapLevel) + "," + XX + ";";
		}
		return NowStr;
	}

	function LookThisMapIsDone(nowMap : String , NowStr : String){
		var useStrs : String[];
		var useMapStrs : String[];
		var i : int = 0;
		useStrs = NowStr.Split(Fstr.ToCharArray());
		for(i=0; i<useStrs.length; i++){
			if(useStrs[i].Length > 2){
				useMapStrs = useStrs[i].Split(Dstr.ToCharArray());
				if(useMapStrs[0] == nowMap){
					return false;
				}
			}
		}
		return true;
	}

	function DoneDungeon(){ 
		AllManage.UIALLPCStatic.show26();
		yield;
		yield;
		yield;
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages054");
				AllManage.AllMge.Keys.Add(AllManage.mtwStatic.SaoDangTimes + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelTimesSaoDang);
	//	LabelTimesSaoDang.text = "设定次数剩余：" + AllManage.mtwStatic.SaoDangTimes;
		if(UIControl.mapType == MapType.zhucheng){
			DoneButtons1.localPosition.y = 3000;
			DoneButtons2.localPosition.y = 0;
		}
		for(var o=0; o<xingxing; o++){
			TweenXings[o].gameObject.transform.localScale = Vector3(120,120,1);
			TweenXingsIcon[o].enabled = false;
		}
		DungOpenB.trans1.localPosition.x = 1000;
		DungOpenB.trans2.localPosition.x = 1000;
		DungOpenB.trans3.localPosition.x = 1000;
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		UIControl.isYtFuben = false;
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages055");
				AllManage.AllMge.Keys.Add(Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) + "");
				AllManage.AllMge.Keys.Add("messages053");
				AllManage.AllMge.SetLabelLanguageAsID(LabelBoxCostBlood2);
	//	LabelBoxCostBlood2.text = "点击开启,花费" + Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) + "血石";
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages055");
				AllManage.AllMge.Keys.Add(Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) * 2 + "");
				AllManage.AllMge.Keys.Add("messages053");
				AllManage.AllMge.SetLabelLanguageAsID(LabelBoxCostBlood3);
	//	LabelBoxCostBlood3.text = "点击开启,花费" + Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) * 2 + "血石";

		var gos : GameObject[];
	  gos = GameObject.FindGameObjectsWithTag("Enemy"); 
	  for (var go : GameObject in gos) {
	   go.SendMessage("PauseAttack", true,SendMessageOptions.DontRequireReceiver);    
	   }
		TweenDC.Play(true);
	//	UIALLC.showThisPanel("fuben");
	//	for(var o=0; o<MonsterSp.length; o++){
	//		if(MonsterSp[o].clear){
	//			DungeonJiSha += 1;
	//		}
	//	}
	//LabelJiSha.text = DungeonJiSha + "";
		timeSpawn = InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(TimeStart);
	//LabelTime.text = timeSpawn.Hours.ToString() + ":" + timeSpawn.Minutes.ToString() + ":" + timeSpawn.Seconds.ToString();
		FenXiaohao = Mathf.Clamp(1000 - TimesDead*500 - TimesXuePing*100 , 0,1000);
	//LabelXiaoHao.text = FenXiaohao.ToString();
		useInt = (level - parseInt(AllManage.psStatic.Level)) * 100 + 500;
		useInt = Mathf.Clamp(useInt , 0 , 1000);
	//LabelJiQiao.text = useInt.ToString();
		FenTime = Mathf.Clamp(DungeonTime - timeSpawn.TotalSeconds , -20,20);
		Fzongfen = 500 + FenTime + FenXiaohao + useInt;
	//LabelZongFen.text = Fzongfen.ToString(); 
		bili= Fzongfen / float1;
		if(bili < 0.25){
			xingxing =1;
		}else
		if(bili < 0.5){
			xingxing =2;
		}else
		if(bili < 0.75){
			xingxing =3;
		}else
		if(bili < 1){
			xingxing =4;
		}else{
			xingxing =5;
		}
		if(NowMapLevel ==1){
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText;
			UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
			InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText = UseXXStr;
		}
		else if(NowMapLevel ==2){
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText;		
			UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
			InventoryControl.yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText = UseXXStr;
		}
		else{
			UseXXStr = InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText;		
			UseXXStr = DoSetThisMapXXAsID(bili * 10, UseXXStr);
			InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText = UseXXStr;
		}
		var useBiliInt : int;
		useBiliInt = bili * 10;
		InventoryControl.yt.Rows[0]["DuplicatePoint"].YuanColumnText = (invcl.GetBDInfoInt("DuplicatePoint" , 0) + useBiliInt).ToString();
		FenExp = xingxing * level * 200;
	//LabelExp.text = "+" + (xingxing * level * 200).ToString();
	//	//print(xingxing + " == xing xing");
	//	GetGold = 500 + FenTime + Mathf.Clamp(1000 - TimesDead*500 , 0,1000) + useInt;
		GetGold = xingxing * 2 * level * 20;
	//	GetBlood = Random.Range(0,6);
		
	//	JiangLiRandom = Random.Range(1,4);
	//	//print(JiangLiRandom + " == JiangLiRandom");
	//	//print(JiangLiRandom + " == JiangLiRandom");
		if(rewardItemStr[0] != "")
		rewardItemStr[0] += ",0" + Mathf.Clamp(xingxing + Random.Range(-1,2) , 1, 3);
		if(rewardItemStr[1] != "")
		rewardItemStr[1] += ",0" + Mathf.Clamp(xingxing + Random.Range(-1,2) , 1, 3);
		if(rewardItemStr[2] != "")
		rewardItemStr[2] += ",0" + Mathf.Clamp(xingxing + Random.Range(-1,2) , 1, 3);	
		
		yield WaitForSeconds(1.5);
		useBool = false;
		while( ! useBool){
			showJiSha = ShowOneValuePlus(LabelJiSha , showJiSha , DungeonJiSha , 1 , "" , 0);
			yield;
		}
		useBool = false;
		while( !  useBool){
			showTime3 = ShowOneValuePlus(LabelTime3 , showTime3 , timeSpawn.Seconds , Random.Range(1,4) , ":", 1);
			yield;
		}
		useBool = false;
		while( ! useBool ){
			showTime2 = ShowOneValuePlus(LabelTime2 , showTime2 , timeSpawn.Minutes , Random.Range(1,4) , ":", 1);
			yield;
		}
		useBool = false;
		while( !  useBool){
			showTime1 = ShowOneValuePlus(LabelTime1 , showTime1 , timeSpawn.Hours , Random.Range(1,4) , "", 1);
			yield;
		}
		useBool = false;
		while( ! useBool ){
			showXiaoHao = ShowOneValuePlus(LabelXiaoHao , showXiaoHao , FenXiaohao , Random.Range(11,40) , "", 0);
			yield;
		}
		useBool = false;
		while( !  useBool){
			showJiQiao = ShowOneValuePlus(LabelJiQiao , showJiQiao , useInt , Random.Range(11,100) , "", 0);
			yield;
		}
		useBool = false;
		while( ! useBool ){
			showZongFen = ShowOneValuePlus(LabelZongFen , showZongFen , Fzongfen , Random.Range(11,100) , "", 0);
			yield;
		}
		useBool = false;
		while( !useBool  ){
			showExp = ShowOneValuePlus(LabelExp , showExp , FenExp , Random.Range(11,100) , "+", 0);
			yield;
		}
		
		for(var i=0; i<xingxing; i++){
	//		SpriteStars[i].spriteName = "xingxingquan"; 
			TweenXingsIcon[i].enabled = true;
			TweenXings[i].Play(true);
			yield WaitForSeconds(0.25);
			ParticleXingXings[i].Emit();
			ParticleXingXings[i].audio.Play();
			yield WaitForSeconds(0.1);
			ParticleXingXings[i].emit = false;
		}
		
		ObjDangBan.localPosition.y = 5000;
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
//		ps.AddExperience(xingxing * level * 200);
		if(NowMapLevel < 5){
			ps.AddPrestige( -1*NowMapLevel*5);
		}else{
			InventoryControl.yt.Rows[0]["AimHeroMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimHeroMission"].YuanColumnText) + 1).ToString();
			ps.AddPrestige(-30);		
		}
		InventoryControl.yt.Rows[0]["AimFinshMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimFinshMission"].YuanColumnText) + 1).ToString();
		
		var pss : PlayerStatus[];
		pss = FindObjectsOfType(PlayerStatus);
		if(pss.length > 1){
			InventoryControl.yt.Rows[0]["AimTeamMission"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimTeamMission"].YuanColumnText) + 1).ToString();
		}
	}

	function EndNow(){
			showJiSha = DungeonJiSha;
			showTime3 = timeSpawn.Seconds;
			showTime2 = timeSpawn.Minutes;
			showTime1 = timeSpawn.Hours;
			showXiaoHao = FenXiaohao;
			showJiQiao = useInt;
			showZongFen = Fzongfen;
			showExp = FenExp;
			showGold = GetGold;
			
			showTeamGold = TeamGold;
			ShowTeamExp =  TeamExp;
	}

	private var useBool : boolean;
	function ShowOneValuePlus(label : UILabel ,iStart : int, iEnd : int , iPlus : int , otherStr : String , Lth : int) : int{
		if(iStart < iEnd){
			iStart += iPlus;
		}
		if(iStart >= iEnd){
			iStart = iEnd;
			useBool = true;		
		}
		if(Lth == 1){
			if(iStart.ToString().Length < 2){
				label.text = otherStr + "0" + iStart.ToString();		
			}else{
				label.text = otherStr + iStart.ToString();					
			}
		}else{
			label.text = otherStr + iStart.ToString();
		}
		return iStart;
	}

	var TweenXings : TweenScale[];
	var TweenXingsIcon : UISprite[];
	var showJiSha : int = 0;
	var showTime1 : int = 0;
	var showTime2 : int = 0;
	var showTime3 : int = 0;
	var showXiaoHao : int = 0;
	var showJiQiao : int = 0;
	var showZongFen : int = 0;
	var showExp : int = 0;
	var showGold : int = 0;
	var showPVE : int = 0;
	
	var showTeamGold : int = 0;
	var ShowTeamExp : int = 0;
	function OnApplicationExit(){
		 YuanUnityPhoton.GetYuanUnityPhotonInstantiate().peer.Disconnect();
	} 

	function DungeonReLoad(){
	//	CreateOtherInvsSet();
	//	yield WaitForSeconds(3);
	 	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
//	function TipsNewPower(type : yuan.YuanPhoton.CostPowerType , num1 : int , num2 : int , itemID : String , obj : GameObject , functionName : String){
		if(NowMapLevel == 5){
			AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.EliteDungeon , NowMapLevel , 0 , "" , gameObject , "canDungeonReLoad");		
		}else{
			AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.NormalDungeon , NowMapLevel , 0 , "" , gameObject , "canDungeonReLoad");					
		}
//		var costPower : int = 0;
//		costPower = AllResources.GetMapCostPower(mtwMapID , NowMapLevel);
//		
//		if(parseInt(ps.Power) >= costPower){
//			if(NowMapLevel == 5){
//				Loading.Level = "Map" + Application.loadedLevelName.Substring(3,2) + "1";
//			}else{
//				Loading.Level = Application.loadedLevelName;		
//			}
//			Loading.AgainTimes += 1;
//			Loading.YaoQingStr = PlayerInfo.mapName + "" +  Loading.AgainTimes;
//			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
//			alljoy.DontJump = true;
//			yield;
//			PhotonNetwork.LeaveRoom();
//			InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;				AllResources.ar.AllLoadLevel("Loading 1");
//			AllResources.ar.AllLoadLevel("Loading 1");
//		}
	}

	function canDungeonReLoad(isBool : boolean){
		if(isBool){
			if(NowMapLevel == 5){
				Loading.Level = "Map" + Application.loadedLevelName.Substring(3,2) + "1";
			}else{
				Loading.Level = Application.loadedLevelName;		
			}
			Loading.AgainTimes += 1;
			Loading.YaoQingStr = PlayerInfo.mapName + "" +  Loading.AgainTimes;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
			alljoy.DontJump = true;
			yield;
			PhotonNetwork.LeaveRoom();
			InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;				AllResources.ar.AllLoadLevel("Loading 1");
			AllResources.ar.AllLoadLevel("Loading 1");			
		}
	}

	function DungeonNextLoad(){
		InRoom.GetInRoomInstantiate().IsTeamHead();
	}

	function TeamHeadNextLoad(){
		Loading.Level = "Map" + useNextMap;
		Loading.AgainTimes += 1;
		Loading.YaoQingStr = PlayerInfo.mapName + "" +  Loading.AgainTimes;
		Loading.YaoQingStr = Loading.Level + "" + NowMapLevel + Loading.YaoQingStr.Substring(6 , Loading.YaoQingStr.Length - 6);
		AllManage.UICLStatic.teamHeadMapName = Loading.Level + NowMapLevel;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
			PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");


	}
	
	public function desP(){
		while(PhotonNetwork.isMasterClient){
			yield;
		}
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
	}
	 
	static var ReLevel : String = "";
	function DungeonClose(){
	//	CreateOtherInvsSet();
	//	yield WaitForSeconds(3);
		InRoom.GetInRoomInstantiate().RemoveTempTeam();
		Loading.Level = ReLevel;
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
		AllManage.UICLStatic.RemoveAllTeam();
		alljoy.DontJump = true;
		yield;
			PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}

	function CreateOtherInvsSet(){
		var inv1 : InventoryItem = null;
		var inv2 : InventoryItem = null;
		var inv3 : InventoryItem = null;
		var inv4 : InventoryItem = null; 
		var invStr : String; 
		var ranrum : int;

		if(DungOpenB.trans1.localPosition.x != 0){
			newRanInt = Random.Range(0,100);
			if(newRanInt > 80){
				inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1);
			}else{
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 		
				}else{
					invStr = AllResources.staticLT.MakeItemID1(invStr, 1); 
				}
				inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
			}
			ranrum = Random.Range(0,10000);
			if(ranrum > (7000 - RanItemProbability/2) && RanItem != ""){
				invStr = RanItem;
				inv4 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv4);
			}
			DungOpenB.ShowWeaponAsID(inv1 , inv2 , inv3 , inv4 , 1);
			ObjBaoXiang[0].SetActiveRecursively(false);
		}
		
		if(DungOpenB.trans2.localPosition.x != 0){
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 70){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 3); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
				}
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv2 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[1] , inv2); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 30){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 1); 
					}
					inv2 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv2);
				}
				ranrum = Random.Range(0,10000);
				if(ranrum > (7000 - RanItemProbability) && RanItem != ""){
					invStr = RanItem;
					inv4 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv4);
				}
				DungOpenB.ShowWeaponAsID(inv1 , inv2 , inv3 , inv4 , 2);
				ObjBaoXiang[1].SetActiveRecursively(false);
		}
		
		if(DungOpenB.trans3.localPosition.x != 0){
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 30){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 3); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
				}
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv2 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[1] , inv2); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 70){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 4); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv2 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv2);
				}
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv3 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[2] , inv3); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 40){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 3); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv3 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv3);
				}
				ranrum = Random.Range(0,10000);
				if(ranrum > (7000 - RanItemProbability/2) && RanItem != ""){
					invStr = RanItem;
					inv4 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv4);
				}
			DungeonJiSha = 0;
			DungOpenB.ShowWeaponAsID(inv1 , inv2 , inv3 , inv4 , 3);
			ObjBaoXiang[2].SetActiveRecursively(false);
		}
	}

	var GroundReward : GameObject;
	//var qr : QueRen; 
	//var ts : TiShi;
	//var invmaker : Inventorymaker;
	var ObjBaoXiang : GameObject[];
	//var LT : LootTable;
	var invcl : InventoryControl;
	var OpenB : OpenBox;
	function open1(){ 
		QRopen1();
	} 
	var newRanInt : int = 0;
	function QRopen1(){	
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		var inv1 : InventoryItem = null;
		var inv2 : InventoryItem = null;
		var inv3 : InventoryItem = null;
		var inv4 : InventoryItem = null; 
		var invStr : String; 
		var useOpenInvs : InventoryItem[] ;
		useOpenInvs = new Array(4);
		JiangLiRandom = Random.Range(1,3);
		////print(JiangLiRandom + " == JiangLiRandom");
		
		if(rewardItemStr[0] != "" && JiangLiRandom > 0){
			newRanInt = Random.Range(0,100);
			if(newRanInt > 80){
				inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1);
			}else{
				newRanInt = Random.Range(0,100);
				if(newRanInt > 80){
					invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 		
				}else{
						if(NowMapLevel == 5){
							invStr = GetGemItem(1);
						}else{				
							invStr = AllResources.staticLT.MakeItemID1(invStr, 1); 
						}
				}
				inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
			}
			invcl.AddBagItem(inv1);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv1.itemID);
			useOpenInvs[0] = inv1;
		}
	//	if(rewardItemStr[1] != "" && JiangLiRandom > 1) { 
	//		newRanInt = Random.Range(0,100);
	//		if(newRanInt > 80){
	//			inv2 = invmaker.GetItemInfo(rewardItemStr[1] , inv2); 
	//		}else{
	//			invStr = LT.MakeItemID1(invStr, 1); 
	//			inv2 = invmaker.GetItemInfo(invStr, inv2);
	//		}
	//		invcl.AddBagItem(inv2);
	//		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv2.itemID);
	//	}
	//	if(rewardItemStr[2] != "" && JiangLiRandom > 2){
	//		newRanInt = Random.Range(0,100);
	//		if(newRanInt > 80){
	//			inv3 = invmaker.GetItemInfo(rewardItemStr[2] , inv3);
	//		}else{
	//			invStr = LT.MakeItemID1(invStr, Random.Range(1,3)); 
	//			inv3 = invmaker.GetItemInfo(invStr, inv3);
	//		}
	//		invcl.AddBagItem(inv3);
	//		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv3.itemID);
	//	}
		var ranrum : int;
		ranrum = Random.Range(0,10000);
		////print(" ranrum == " + ranrum);
		if(ranrum > (10000 - RanItemProbability/2) && RanItem != ""){
			invStr = RanItem;
			inv4 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv4);
			invcl.AddBagItem(inv4);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv4.itemID);
			useOpenInvs[3] = inv4;
		}
		DungOpenB.ShowWeaponAsID(useOpenInvs[0] , useOpenInvs[1] , useOpenInvs[2] , useOpenInvs[3] , 1);
	//	ps.UseBloodStone(GetBlood * (-1));
	//	AllManage.AllMge.UseMoney(GetGold * (-1) , GetBlood * (-1) , UseMoneyType.rewardDungeon1 , gameObject , "");
	//	ps.UseMoney(GetGold * (-1) , GetBlood * (-1));
	//	useStr += inv4.itemName + "x" + inv4.consumablesNum;
		OpenB.open(1,GetGold,GetBlood,inv1,inv2,inv3,inv4);
	//	ts.Show(useStr);
		ObjBaoXiang[0].SetActiveRecursively(false);
	}

	function open2(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
			AllManage.qrStatic.ShowBuyQueRen1(gameObject , "use2" ,  "" , AllManage.AllMge.Loc.Get("info289") + Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22)+ AllManage.AllMge.Loc.Get("info290")  );
		else
			use2();
	}

	function use2(){
		if(ps.isBlood( Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22))){
			QRopen2();
		}
	}

	function QRopen2(){
	//	AllManage.AllMge.UseMoney(0 , Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) , UseMoneyType.QRopen2 , gameObject , "realQRopen2");
	}

	function realQRopen2(){
		var bool : boolean = true;
	//	bool = ps.UseMoney(0, Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) );
		if(bool){
			var inv1 : InventoryItem = null;
			var inv2 : InventoryItem = null;
			var inv3 : InventoryItem = null;
			var inv4 : InventoryItem = null; 
			var invStr : String; 
		var useOpenInvs : InventoryItem[] ;
		useOpenInvs = new Array(4);
		JiangLiRandom = Random.Range(3,4);
		////print(JiangLiRandom + " == JiangLiRandom");
			if(rewardItemStr[0] != "" && JiangLiRandom > 0){
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 80){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 3); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
				}
				invcl.AddBagItem(inv1);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv1.itemID);
				useOpenInvs[0] = inv1;
			}
			if(rewardItemStr[1] != "" && JiangLiRandom > 1) { 
				newRanInt = Random.Range(0,100);
				if(newRanInt > 50){
					inv2 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[1] , inv2); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 80){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 		
					}else{
						if(NowMapLevel == 5){
							invStr = GetGemItem(2);
						}else{				
							invStr = AllResources.staticLT.MakeItemID1(invStr, 1); 
						}
					}
					inv2 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv2);
				}
				invcl.AddBagItem(inv2);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv2.itemID);
				useOpenInvs[1] = inv2;
			}
	//		if(rewardItemStr[2] != "" && JiangLiRandom > 2){
	//			newRanInt = Random.Range(0,100);
	//			if(newRanInt > 80){
	//				inv3 = invmaker.GetItemInfo(rewardItemStr[2] , inv3);
	//			}else{
	//				invStr = LT.MakeItemID1(invStr, Random.Range(1,3)); 
	//				inv3 = invmaker.GetItemInfo(invStr, inv3);
	//			}
	//			invcl.AddBagItem(inv3);
	//			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv3.itemID);
	//		}
		var ranrum : int;
		ranrum = Random.Range(0,10000);
		////print(" ranrum == " + ranrum);
		if(ranrum > (10000 - RanItemProbability) && RanItem != ""){
			invStr = RanItem;
			inv4 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv4);
			invcl.AddBagItem(inv4);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv4.itemID);
			useOpenInvs[3] = inv4;
		}
		DungOpenB.ShowWeaponAsID(useOpenInvs[0] , useOpenInvs[1] , useOpenInvs[2] , useOpenInvs[3] , 2);
	//	ps.UseBloodStone(0 , GetBlood * (-1));
		ps.UseMoney(GetGold * (-1)* 1.5 , GetBlood * (-1));
	//		useStr += inv4.itemName + "x" + inv4.consumablesNum;
			OpenB.open(2,GetGold* 1.5,GetBlood,inv1,inv2,inv3,inv4);
		//	ts.Show(useStr);
			ObjBaoXiang[1].SetActiveRecursively(false);
		}
	}

	function open3(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
			AllManage.qrStatic.ShowBuyQueRen1(gameObject , "use3" , "" , AllManage.AllMge.Loc.Get("info291") + Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22)*2 +AllManage.AllMge.Loc.Get("info292") );
		else
			use3();
	}

	function use3(){
		if(ps.isBlood( Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22) * 2 )){
			QRopen3();
		}
	}

	function QRopen3(){
		var bool : boolean = false;
		bool = ps.UseMoney(0, Mathf.Clamp(parseInt(ps.Level) / 5 , 2 , 22)*2 );
		if(bool){
			var inv1 : InventoryItem = null;
			var inv2 : InventoryItem = null;
			var inv3 : InventoryItem = null;
			var inv4 : InventoryItem = null; 
			var invStr : String; 
		var useOpenInvs : InventoryItem[] ;
		useOpenInvs = new Array(4);
		JiangLiRandom = Random.Range(3,4);
		////print(JiangLiRandom + " == JiangLiRandom");
			if(rewardItemStr[0] != "" && JiangLiRandom > 0){
				newRanInt = Random.Range(0,100);
				if(newRanInt > 80){
					inv1 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[0] , inv1); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 80){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 3); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv1 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv1);
				}
				invcl.AddBagItem(inv1);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv1.itemID);
				useOpenInvs[0] = inv1;
			}
			if(rewardItemStr[1] != "" && JiangLiRandom > 1) { 
				newRanInt = Random.Range(0,100);
				if(newRanInt > 80){
					inv2 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[1] , inv2); 
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 90){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 4); 		
					}else{
						invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
					}
					inv2 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv2);
				}
				invcl.AddBagItem(inv2);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv2.itemID);
				useOpenInvs[1] = inv2;
			}
			if(rewardItemStr[2] != "" && JiangLiRandom > 2){
				newRanInt = Random.Range(0,100);
				if(newRanInt > 80){
						inv3 = AllResources.InvmakerStatic.GetItemInfo(rewardItemStr[2] , inv3); 		
				}else{
					newRanInt = Random.Range(0,100);
					if(newRanInt > 90){
						invStr = AllResources.staticLT.MakeItemID1(invStr, 3); 		
					}else{
						if(NowMapLevel == 5){
							invStr = GetGemItem(3);
						}else{				
							invStr = AllResources.staticLT.MakeItemID1(invStr, 2); 
						}
					}
					inv3 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv3);
					useOpenInvs[2] = inv3;
			}
				invcl.AddBagItem(inv3);
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv3.itemID);
			}
		var ranrum : int;
		ranrum = Random.Range(0,10000);
		////print(" ranrum == " + ranrum);
		if(ranrum > (10000 - RanItemProbability*2) && RanItem != ""){
			invStr = RanItem;
			inv4 = AllResources.InvmakerStatic.GetItemInfo(invStr, inv4);
			invcl.AddBagItem(inv4);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , inv4.itemID);
				useOpenInvs[3] = inv4;
		}
	//	ps.UseMoney();
			DungOpenB.ShowWeaponAsID(useOpenInvs[0] , useOpenInvs[1] , useOpenInvs[2] , useOpenInvs[3] , 3);
			ps.UseMoney(GetGold * (-1) *2 , GetBlood * (-1));
	//		useStr += inv4.itemName + "x" + inv4.consumablesNum;
			OpenB.open(3,GetGold * 2,GetBlood,inv1,inv2,inv3,inv4);
		//	ts.Show(useStr);
			ObjBaoXiang[2].SetActiveRecursively(false);
		}
	}

	function GetGemItem(BOXID : int){
		var str : String;
		switch(BOXID){
			case 1:
				str = "81" + Random.Range(1,6) + Mathf.Clamp(level / 20 , 1 , 9) + ",01";
				break;
			case 2:
				str = "81" + Random.Range(1,6) + Mathf.Clamp(level / 15 , 1 , 9) + ",01";
				break;
			case 3:
				str = "81" + Random.Range(1,6) + Mathf.Clamp(level / 10 , 1 , 9) + ",01";
				break;
		}
		return str;
	}

	private var DungOpenB : DungeonOpenBox;
	private var LabelBoxCostBlood1 : UILabel;
	private var LabelBoxCostBlood2 : UILabel;
	private var LabelBoxCostBlood3 : UILabel;
	var DoneButtons1 : Transform;
	var DoneButtons2 : Transform;
	var LabelTimesSaoDang : UILabel;
	var ParentDungeonUse : Transform;
	var LabelPVEPoint : UILabel;
	function SetObj(AS : AwakeSet){
	//	LabelJiSha = AS.LabelJiSha ;
	//	LabelTime1 = AS.LabelTime1 ;
	//	LabelTime2 = AS.LabelTime2 ;
	//	LabelTime3 = AS.LabelTime3 ;
	//	LabelXiaoHao = AS.LabelXiaoHao ;
	//	LabelJiQiao = AS.LabelJiQiao ;
	//	LabelZongFen = AS.LabelZongFen ; 
	//	LabelExp = AS.LabelExp ;
	//	TweenDC = AS.TweenDC ;
	//	SpriteStars = AS.SpriteStars ;
	//	ObjDangBan = AS.ObjDangBan ;
	//	ParticleXingXings = AS.ParticleXingXings ;
	//	TweenXings = AS.TweenXings ;
	//	TweenXingsIcon = AS.TweenXingsIcon ;
	//	ObjBaoXiang = AS.ObjBaoXiang ;
	//	DungOpenB =  AS.DungOpenB ;
	//	LabelBoxCostBlood2 =  AS.LabelBoxCostBlood2 ;
	//	LabelBoxCostBlood3 =  AS.LabelBoxCostBlood3 ;
	//	DoneButtons1 =  AS.DoneButtons1 ;
	//	DoneButtons2 =  AS.DoneButtons2 ;
	//	LabelTimesSaoDang = AS.LabelTimesSaoDang;

		LabelZongFen = AS.LabelZongFen ; 
		LabelExp = AS.LabelExp ;
		SpriteStars = AS.SpriteStars ;
		ParticleXingXings = AS.ParticleXingXings ;
		TweenXings = AS.TweenXings ;
		TweenXingsIcon = AS.TweenXingsIcon ;
		DoneButtons1 =  AS.DoneButtons1 ;
		DoneButtons2 =  AS.DoneButtons2 ;
		LabelTimesSaoDang = AS.LabelTimesSaoDang;
		LabelGold = AS.LabelGold;
		LabelSoul = AS.LabelSoul;
		ObjSoulGroup = AS.ObjSoulGroup;
		ObjHeroGroup = AS.ObjHeroGroup;
		ParentDungeonUse =  AS.ParentDungeonUse;
		LabelPVEPoint = AS.LabelPVEPoint;
		LabelBlood = AS.LabelBlood;
		SpriteBlood = AS.SpriteBlood;
		LabelHoreStone = AS.LabelHoreStone;
	}

	function SetObjWin(AS : AwakeSet){
		WinBordUI = AS.WinBordUI ;
		WinBord = AS.WinBord ;
		TweenWin = AS.TweenWin ;
		TweenDun = AS.TweenDun ;
		emits = AS.emits ;
	} 

	function show0(){
		AllManage.UIALLPCStatic.show0();
	}

	function JiXuSaoDang(){
		show0();
		AllManage.mtwStatic.SaoDangStart();
	}
	
	function AttackGuide(){
	var preShengchan = Resources.Load("Anchor - helproot", GameObject);
	GameObject.Instantiate( preShengchan );
	}
	
	function CloseDoneCardButton(){
		ObjSoulGroup.SetActiveRecursively(false);
		ObjHeroGroup.SetActiveRecursively(false);
		SpriteBlood.enabled = false;
	}
	
	function BossDileStoneBox(trans : Transform){
		var i : int = 0;
		var canBox : boolean = false;
		var clearNum : int = 0;
		var bossNum : int = 0;
		for(i=0; i<MonsterSp.length; i++){
			if(MonsterSp[i].spType == SpawnPointType.boss1 || MonsterSp[i].spType == SpawnPointType.boss2){
				bossNum += 1;
			}
		}
		for(i=0; i<MonsterSp.length; i++){
			if(MonsterSp[i].IsCleared() && (MonsterSp[i].spType == SpawnPointType.boss1 || MonsterSp[i].spType == SpawnPointType.boss2)){
				clearNum += 1;
			}
		}
		
		if(bossNum > 1 && clearNum < bossNum && clearNum > 0){
			canBox = true;
		}
		
		if(canBox){
			AllResources.PickpoolStatic.SpawnPickup( 4, trans );
		}
	}
	
}