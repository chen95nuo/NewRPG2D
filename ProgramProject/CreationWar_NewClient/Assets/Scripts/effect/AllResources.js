#pragma strict
static var EffectGamepoolStatic : EffectGamepool;
private var Effectpool : EffectGamepool;
static var BuffefmanageStatic : Buffefmanage;
private var Buffefpool : Buffefmanage;
static var FontpoolStatic : Fontpool;
private var Fontpools : Fontpool;
static var FuhaopoolStatic : ReGamePool;
private var Fuhaopool :ReGamePool;
static var PickpoolStatic : PickupGamepool;
private var Pickpool : PickupGamepool;
private var invmaker : Inventorymaker;
static var InvmakerStatic : Inventorymaker;
static var ar : AllResources;
static var arObj : GameObject;
private var LT : LootTable;
static var staticLT : LootTable;
var BMC : AudioClip[];
static var staticBMC : AudioClip[];
static var StoneTypeInfo : String[];
static var StoneValueInfo : String[];
var ssStoneTypeInfo : String[];
var ssStoneValueInfo : String[];

//static var camMain :  Camera;
var camUI : Camera;
static var SongGUI : GameObject;
function Awake () {
	GameObject.DontDestroyOnLoad(gameObject);
	
	StoneTypeInfo = ssStoneTypeInfo;
	StoneValueInfo = ssStoneValueInfo;
//	EffectGamepoolStatic = Effectpool;
//	BuffefmanageStatic = Buffefpool;
//	FontpoolStatic = Fontpools;
//	FuhaopoolStatic = Fuhaopool;
//	PickpoolStatic = Pickpool;
//	InvmakerStatic = invmaker;
	staticLT = LT;
	staticBMC = BMC;
	ar = this;
	arObj = gameObject;
	
//	Localization.StaticLoc.FixBrokenWord();
}

//function OnLevelWasLoaded (level : int) {
//	if(level == 0 || level == 1){
//		isLoadGUI = false;
//		camUI.depth = -10;
//		if(PlayerStore.me){
//			Destroy(PlayerStore.me.gameObject);
//		}
//	}
//}

private var useBool : boolean = false;
var ObjLoading : UIPanel;
static var isLoadGUI : boolean = false;
function LoadGUI(load : GameObject){
	if(!useBool){
		useBool = true;
	}
	while(load != null){
//		print(load + " == load");
		yield;
	}
		var preSkill = Resources.Load("Song", GameObject);
//		print(preSkill + " == preSkill");
		GameObject.Instantiate(preSkill);
}

function LoadUI(){
	var preSkill = Resources.Load("Song", GameObject);
//	print(preSkill + " == preSkill");
	GameObject.Instantiate(preSkill);
}

var LabelLoading : UILabel;
var XueTiao : UIFilledSprite;
var ff : float; 
var async : AsyncOperation ;
var progress : int = 0;
private var ptime : int = 0;
function Update(){
	if(async){
		progress =  async.progress *100;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("info719");
			AllManage.AllMge.Keys.Add((50 + progress  / 2) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelLoading);
	//	LabelLoading.text ="正在努力加载..." +progress.ToString();
		ff = progress + 50;
		XueTiao.fillAmount = ff/100;
//		//print(progress);	
	}
	if(Time.time > ptime && Time.timeScale != 1.0){
		Time.timeScale = 1.0;
	}
}
	
function Playtimeslow(){
	if ( Time.timeScale == 1.0){
		ptime = Time.time + 2;
		Time.timeScale = 0.4;
	}
}

function myLoadLevel(level : int){
		yield;
		Application.LoadLevel(level);
}
	var playerPrefabs : Transform[];
	var respawn : GameObject ;
	function CreatePlayer(i : int , str : String , pName : String){
		var obj : GameObject;
//		if(str == "1"){
//			respawn = GameObject.FindWithTag("end");
//		}else{
//			respawn = GameObject.FindWithTag("spawn");
//		}
		var newObj : GameObject = Resources.Load(this.playerPrefabs[i].name, GameObject);
//		print(PlayerUtil.myID +  " ==========================set CreatePlayer");
		newObj.GetComponent(PlayerStatus).instanceID = PlayerUtil.myID;
		var Fpoint : Transform;
	if(str == "Red"){
		Fpoint = GameObject.Find("spawn").transform;			
	}else
	if(str == "Blue"){
		Fpoint = GameObject.Find("end").transform;			
	}else{
		Fpoint = GameObject.Find("spawn").transform;		
	}
		if(GameManager.reLevel == Application.loadedLevelName && pName == GameManager.rePlayerName && GameManager.rePosition != new Vector3(0,0,0)){
			obj = PhotonNetwork.Instantiate(this.playerPrefabs[i].name , GameManager.rePosition , Fpoint.transform.rotation, 0);
//			obj.SendMessage("SetInstanceID" , PlayerUtil.myID , SendMessageOptions.DontRequireReceiver);
			GameManager.rePosition = new Vector3(0,0,0);
			GameManager.reLevel = "";
			GameManager.rePlayerName = "";
		}else{
  			obj = PhotonNetwork.Instantiate(this.playerPrefabs[i].name, Fpoint.position, Fpoint.transform.rotation, 0);	
//  			obj.SendMessage("SetInstanceID" , PlayerUtil.myID , SendMessageOptions.DontRequireReceiver);	
		}
//		camMain.enabled = true;
//		camUI.enabled = true;
		yield;
		camUI.depth = -10;
		async = null;
		progress = 0;
}
	
	function SetPlayerInstanceID(instanceID : int){
		if(PlayerStatus.MainCharacter != null){
			PlayerStatus.MainCharacter.gameObject.SendMessage("SetInstanceID" , instanceID , SendMessageOptions.DontRequireReceiver);
		}
	}
	
	var playerPrefabsTiShen : Transform[];
	function CreatePlayerFuBen(i : int , str : String , pName : String , pos : Vector3) : GameObject{
		if(str == "Blue"){
			respawn = GameObject.FindWithTag("end");
		}else{
			respawn = GameObject.FindWithTag("spawn");
		}
		var obj : GameObject;
		obj = PhotonNetwork.Instantiate(this.playerPrefabsTiShen[i].name , pos + Vector3(Random.Range(-3,3) , 0 ,Random.Range(-4,-1)) , respawn.transform.rotation, 0);
		AllResources.EffectGamepoolStatic.SpawnEffect(166,obj.transform);
		return obj;
	}
	
	function ClearAll(){
		EffectGamepoolStatic.Clear();
			yield;
		if(BuffefmanageStatic)
			BuffefmanageStatic.Clear();
			yield;
		if(FontpoolStatic)
			FontpoolStatic.Clear();
			yield;
		if(FuhaopoolStatic)
			FuhaopoolStatic.Clear();
			yield;
		if(PickpoolStatic)
			PickpoolStatic.Clear();
	}
	
	function SongLoadOut(){
//		print("uicontrol ======= " + this.name);
		if(alljoy)
		alljoy.DontJump = true;
		myLoadLevel(0);
		AllResources.isLoadGUI = false;
//		if(PlayerStore.me)
//			Destroy(PlayerStore.me);
	}

	function PlayerprefabInit(prefabInit : Object[]){
		var prefabName = prefabInit[0];
		var instanceID = prefabInit[1];
		var newObj : GameObject = Resources.Load(prefabName , GameObject);
//		print(instanceID +  " ==========================set PlayerprefabInit");
		newObj.GetComponent(PlayerStatus).instanceID = instanceID;
	}

	function AllResSynHealth(curHP : int){
//		print(PlayerStatus.MainCharacter);
//		print(PlayerStatus.MainCharacter.gameObject);
//		print(curHP);
		if(UIControl.mapType != MapType.zhucheng && PlayerStatus.MainCharacter){
			PlayerStatus.MainCharacter.gameObject.SendMessage("SynHealth" , curHP.ToString() , SendMessageOptions.DontRequireReceiver);
		}
	}

	function ClientAddToMap(point :  Vector3){
		if(UIControl.nowSelectChannel != 0){
			ServerRequest.requestAddToMap(UIControl.nowSelectChannel , point);
			UIControl.nowSelectChannel = 0;
		}else
		if(UIControl.tempTeammapInstensID != 0){
			ServerRequest.requestAddToMap(UIControl.tempTeammapInstensID , point);
			UIControl.tempTeammapInstensID = 0;
		}else
		if(UIControl.mTeamMapInsId != 0){
			ServerRequest.requestAddToMap(UIControl.mTeamMapInsId , point);
			UIControl.mTeamMapInsId = 0;
		}else
		if(UIControl.InviteGoPVEID != 0){
			ServerRequest.requestAddToMap(UIControl.InviteGoPVEID , point);
			UIControl.InviteGoPVEID = 0;
		}else
		if(UIControl.mapInstanceID != 0){
			ServerRequest.requestAddToMap(UIControl.mapInstanceID , point);
			UIControl.mapInstanceID = 0;
		}else
		if(UIControl.BattlefieldinstanceID != ""){
			ServerRequest.requestAddToMap(parseInt(UIControl.BattlefieldinstanceID) , point);
			UIControl.BattlefieldinstanceID = "";
		}else{
			ServerRequest.requestAddToMap(Application.loadedLevelName.Substring(3,3) , point);
		}
		AllManage.UICLStatic.DuiZHangGoFB();
		ServerRequest.isAddToMap = true;
	}

//		objs[0] = srcInstanceID;
//		objs[1] = targetInstanceID;
//		objs[2] = damage;
//		objs[3] = skillID;
//		objs[4] = effect;
	private var go : GameObject;
	function ResponseDamage(objs : Object[]){
		if(objs[1] == PlayerUtil.myID){
			if(PlayerStatus.MainCharacter){
				go = PlayerStatus.MainCharacter.gameObject;			
			}
		}else{
			go = ObjectAccessor.getAOIObject(objs[1] );
			if(go == null){
				go = MonsterHandler.GetInstance().FindMonsterByMonsterID(objs[1] );
			}
		}
		if(go){
			go.SendMessage("ResponseDamage" , objs , SendMessageOptions.DontRequireReceiver);
		}
	}
	
	function FindGameObjectByInstanceID(instanceID : int) : GameObject
	{
		var go : GameObject = null;
		if(instanceID == PlayerUtil.myID)
		{
			if(PlayerStatus.MainCharacter)
			{
				go = PlayerStatus.MainCharacter.gameObject;			
			}
		}
		else
		{
			go = ObjectAccessor.getAOIObject(instanceID);
			if(go == null)
			{
				go = MonsterHandler.GetInstance().FindMonsterByMonsterID(instanceID);
			}
		}
		return go;
	}

	var ridename : GameObject[];
	var rideSpeedMove : float[];
	var rideSpeedAutoMove : float[];
	var isAddToMap : boolean = false;
	private var Fstr : String = ";";
	function ResponseSyncAct(objs : Object[]){
		try{
			if(objs[0] == PlayerUtil.myID){
				if(PlayerStatus.MainCharacter){
					go = PlayerStatus.MainCharacter.gameObject;			
				}
			}else{
				go = ObjectAccessor.getAOIObject(objs[0] );
				if(go == null){
					go = MonsterHandler.GetInstance().FindMonsterByMonsterID(objs[0] );
				}
			}
			var actType : int = objs[1];
			var param : String = objs[2];
			var strs : String[];
			var ints : int[];
			if(go){
				switch(actType)
				{
					case CommonDefine.ACT_JUMP : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							go.SendMessage("SyncDidJump" , strs , SendMessageOptions.DontRequireReceiver);	
						}
						break;
					case CommonDefine.ACT_SetDirection : 
						strs = param.Split(Fstr.ToCharArray());
						if(strs.length > 2){
							go.SendMessage("setDirection" , Vector3(parseFloat(strs[0]) , parseFloat(strs[1]) , parseFloat(strs[2])) , SendMessageOptions.DontRequireReceiver);
						}
						break;
					case CommonDefine.ACT_Behit : 
						go.SendMessage("Behit" , SendMessageOptions.DontRequireReceiver);
						break;
					case CommonDefine.ACT_PlayEffect : 
	//					if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("PlayEffect" , parseInt(param) , SendMessageOptions.DontRequireReceiver);
	//					}
						break;
					case CommonDefine.ACT_HitAnimation : 
	//					if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("hitanimation" , SendMessageOptions.DontRequireReceiver);
	//					}
						break;
					case CommonDefine.ACT_PlaySelfAudio : 
	//					if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("PlaySelfAudio" ,  parseInt(param) , SendMessageOptions.DontRequireReceiver);
	//					}
						break;
					case CommonDefine.ACT_ApplyBuff : 
						ints = ArrayStringToInt(param.Split(Fstr.ToCharArray()));
						if(ints.length > 3){
							go.SendMessage("leachBuff" ,  ints , SendMessageOptions.DontRequireReceiver);					
						}
						break;
					case CommonDefine.ACT_ApplyBuff_Down : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("down" , SendMessageOptions.DontRequireReceiver);
						}
						break;
					case CommonDefine.ACT_ApplyBuff_Ground : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("Ground" , SendMessageOptions.DontRequireReceiver);
						}
						break;
					case CommonDefine.ACT_ApplyBuff_JiTui : 
	//					ints = ArrayStringToInt(param.Split(Fstr.ToCharArray()));
	//					if(ints.length > 4){
	//						go.SendMessage("ServerResponsejitui" , ints , SendMessageOptions.DontRequireReceiver);
	//					}
						break;
					case CommonDefine.ACT_PlayAnimat : 
	//					if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("PlayAnimat" , param , SendMessageOptions.DontRequireReceiver);
	//					}
						break;
					case CommonDefine.ACT_PlaySynMax : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 10){
								go.SendMessage("ReturnSynMax" , strs , SendMessageOptions.DontRequireReceiver);				
							}
						}
						break;
					case CommonDefine.ACT_PlaySynAttr : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 10){
								go.SendMessage("ReturnSynattr" , strs , SendMessageOptions.DontRequireReceiver);				
							}
						}
						break;
					case CommonDefine.ACT_PlayOnFight : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("RetrunOnFight" , param , SendMessageOptions.DontRequireReceiver);	
						}			
						break;	
					case CommonDefine.ACT_RideOn : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 1){
								go.SendMessage("ReturnRideOn" , strs , SendMessageOptions.DontRequireReceiver);				
							}
						}
						break;	
					case CommonDefine.ACT_RideOff : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("rideOff" , SendMessageOptions.DontRequireReceiver);			
						}	
						break;	
					case CommonDefine.ACT_PlaybuffEffect : 
						if(!PlayerUtil.isMine(objs[0])){
							ints = ArrayStringToInt(param.Split(Fstr.ToCharArray()));
							if(ints.length > 3){
								go.SendMessage("PlaybuffEffect" , ints , SendMessageOptions.DontRequireReceiver);				
							}
						}
						break;
					case CommonDefine.ACT_PlayskillEffect : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("PlayskillEffect" , parseInt(param) , SendMessageOptions.DontRequireReceiver);				
						}
						break;	
					case CommonDefine.ACT_BigBoom : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 2){
								go.SendMessage("ReturnBigBoom" , strs , SendMessageOptions.DontRequireReceiver);				
							}
						}
						break;
					case CommonDefine.ACT_Huandon : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 3){
								go.SendMessage("Retrunhuandon" , strs , SendMessageOptions.DontRequireReceiver);				
							}
						}
						break;
					case CommonDefine.ACT_Showbody : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("ReturnShowbody" , param , SendMessageOptions.DontRequireReceiver);		
						}
						break;	
					case CommonDefine.ACT_rideClose : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("rideClose" , SendMessageOptions.DontRequireReceiver);		
						}		
						break;	
					case CommonDefine.ACT_PlayerSpawn : 
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("spawn" , SendMessageOptions.DontRequireReceiver);		
						}		
						break;	
					case CommonDefine.ACT_Mover : 
						if(!PlayerUtil.isMine(objs[0])){
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 2){					
								go.SendMessage("mover" , Vector3(parseInt(strs[0]) , parseInt(strs[1]) , parseInt(strs[2])) , SendMessageOptions.DontRequireReceiver);			
							}	
						}
						break;	
					case CommonDefine.ACT_ChangeWeapon : 			
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("ChangeWeapon" , parseInt(param) , SendMessageOptions.DontRequireReceiver);	
						}
						break;	
					case CommonDefine.ACT_GetAnimation : 	
						if(!PlayerUtil.isMine(objs[0])){				
							go.SendMessage("GetAnimation" , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_SyyAnimation : 	
						if(!PlayerUtil.isMine(objs[0])){				
							go.SendMessage("ChangeWeapon" , param , SendMessageOptions.DontRequireReceiver);		
						}	
						break;	
					case CommonDefine.ACT_CrossAnimation2 : 		
						if(!PlayerUtil.isMine(objs[0])){
							go.SendMessage("CrossAnimation2" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_CrossAnimation : 				
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("CrossAnimation" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;
					case CommonDefine.ACT_PlayrideAnimation : 				
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("PlayrideAnimation" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_synAspeed : 			
						if(!PlayerUtil.isMine(objs[0])){		
							go.SendMessage("synAspeed" , parseFloat(param) , SendMessageOptions.DontRequireReceiver);		
						}
						break;	
					case CommonDefine.ACT_RPCShowWeaponFirst : 	
						if(!PlayerUtil.isMine(objs[0])){				
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 2){					
								go.SendMessage("ReturnRPCShowWeaponFirst" , Vector3(parseInt(strs[0]) , parseInt(strs[1]) , parseInt(strs[2])) , SendMessageOptions.DontRequireReceiver);			
							}	
						}
						break;	
					case CommonDefine.ACT_RPCRemoveWeapon : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("RPCRemoveWeapon" , parseInt(param) , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_lookTouRPC : 	
						if(!PlayerUtil.isMine(objs[0])){				
							go.SendMessage("lookTouRPC" , parseInt(param) , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_ObjCloseAsType : 				
						if(!PlayerUtil.isMine(objs[0])){	
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 1){					
								go.SendMessage("ReturnObjCloseAsType" , strs , SendMessageOptions.DontRequireReceiver);			
							}
						}
						break;	
					case CommonDefine.ACT_PlayAnimation : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("PlayAnimation" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_CallObjectSoul : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("ReturnCallObjectSoul" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_ReMoveSoul : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("ReturnreMoveSoul" , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_SoulPlayEffect : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("RPCPlayEffect" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_SoulSyncAnimation : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("RPCSyncAnimation" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_SoulShoot : 	
						if(!PlayerUtil.isMine(objs[0])){	
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 1){					
								go.SendMessage("RPCSoulShoot" , strs , SendMessageOptions.DontRequireReceiver);			
							}
						}
						break;	
					case CommonDefine.ACT_SoulSkill : 	
						if(!PlayerUtil.isMine(objs[0])){	
							strs = param.Split(Fstr.ToCharArray());
							if(strs.length > 1){					
								go.SendMessage("RPCSoulSkill" , strs , SendMessageOptions.DontRequireReceiver);			
							}
						}
						break;	
					case CommonDefine.ACT_SynMana : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("SynMana" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_Callloop : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("Callloop" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_changelayer : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("changelayer" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_skillContinue : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("changeskillContinue" , param , SendMessageOptions.DontRequireReceiver);			
						}
						break;	
					case CommonDefine.ACT_roll : 	
						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("ReturnRoll" , SendMessageOptions.DontRequireReceiver);			
						}
					case CommonDefine.ACT_FontEffect : 	
//						if(!PlayerUtil.isMine(objs[0])){	
							go.SendMessage("ReturnFontEffect" , parseInt(param) ,SendMessageOptions.DontRequireReceiver);			
//						}
						break;	
						
				}
			}
		}catch(e){
//			print(e.ToString());
		}
	}

	static function Vector3ToString(vec : Vector3) : String
	{
		return String.Format("{0};{1};{2}", vec.x , vec.y , vec.z);
	}
	
	static function ArrayStringToInt(strs : String[]) : int[]
	{
		var ints : int[] = new int[strs.length];
		for(var i=0; i<strs.length; i++){
			ints[i] = parseInt(strs[i]);
		}
		return ints;
	}

	static function GetMapCostPower(mapID : String , nandu : int) : int{
		var costPower : int = 0;
		try{
			costPower = AllManage.dungclStatic.GetMapLevelAsMapID(mapID , nandu);
		}catch(e){
			costPower = 20;
		}
		costPower = Mathf.Clamp(costPower , 8 , 20);
		return costPower;
	}
	
	private var useRealLevel : String = "";
	function AllLoadLevel(level : String){
		useRealLevel = level;
		
		if(IsActivity()){
			ServerRequest.requestActivityExit();
		}
		
		if(UIControl.boolTeamHeadYes){
			if( CanRealGoLevel1() || CanRealGoLevel2()){
				while (Input.touchCount != 0){
					yield;
				}
				ServerRequest.isAddToMap = false;
				ObjectAccessor.clearAOIObject();
				Application.LoadLevel(level);
			}else{
				AllManage.qrStatic.ShowQueRen1(gameObject , "RealAllLoadLevelYes" , "RealAllLoadLevelNo" , AllManage.AllMge.Loc.Get("info1142"));			
			}
		}else{
				while (Input.touchCount != 0){
					yield;
				}
			ServerRequest.isAddToMap = false;
			ObjectAccessor.clearAOIObject();
			Application.LoadLevel(level);
		}
	}
	
	function IsActivity() : boolean{
		if(Application.loadedLevelName.Substring(3,3) == "712"
			|| Application.loadedLevelName.Substring(3,3) == "713"
			|| Application.loadedLevelName.Substring(3,3) == "911"
			|| Application.loadedLevelName.Substring(3,3) == "912"
			|| Application.loadedLevelName.Substring(3,3) == "411"
			|| Application.loadedLevelName.Substring(3,3) == "421"
		){
			return true;
		}
		return false;
	}
	
	
	function CanRealGoLevel1() : boolean{
		if(UIControl.TeamHeadYesOKmapid.Length > 0 && Loading.Level.Substring(3,3) == UIControl.TeamHeadYesOKmapid.Substring(0,3) && Loading.nandu == UIControl.TeamHeadYesOKmapid.Substring(4,1)){
			return true;
		}
		return false;
	}
	
	function CanRealGoLevel2() : boolean{
		if(UIControl.tempTeamPlayerGoMapID.Length > 0 && Loading.Level.Substring(3,3) == UIControl.tempTeamPlayerGoMapID.Substring(0,3) && Loading.nandu == UIControl.tempTeamPlayerGoMapID.Substring(4,1)){
			return true;
		}
		return false;
	}
	
	function RealAllLoadLevelYes(){
				while (Input.touchCount != 0){
					yield;
				}
		if(AllManage.UICLStatic.boolTeamHeadYes)
			InRoom.GetInRoomInstantiate().RemoveTempTeam();
		else
			InRoom.GetInRoomInstantiate().TempTeamPlayerRemove();
		ServerRequest.isAddToMap = false;
		ObjectAccessor.clearAOIObject();
		Application.LoadLevel(useRealLevel);
	}
	
	function RealAllLoadLevelNo(){
	
	}
	
	function ReciveJitui(objects : Object[])
	{
		if(PlayerStatus.MainCharacter)
		{
			PlayerStatus.MainCharacter.gameObject.SendMessage("ReciveJitui", objects, SendMessageOptions.DontRequireReceiver);			
		}
	}
	
	public function GetTaskRewardIconAsType(type : int) : String
	{
		var str : String = "";
		switch(type){
			case 0 :
				str = "";
				break;
			case 1 :
				str = "EXP";
				break;
			case 2 :
				str = "UIM_Glory";
				break;
			case 3 :
				str = "UIM_Activity_Values";
				break;
			case 4 :
				str = "GHGX_icon";
				break;
			case 5 :
				str = "UIM_Imperial_Crown";
				break;
			case 6 :
				str = "YXhuizhang";
				break;
			case 7 :
				str = "ZFhuizhang";
				break;
		}
		return str;
	}
	
	var atlasBase : UIAtlas;
	var atlasGround : UIAtlas;
	public function GetTaskRewardAtlasAsType(type : int) : UIAtlas
	{
		var atlas : UIAtlas;
		switch(type){
			case 0 :
				atlas = null;
				break;
			case 1 :
				atlas = atlasGround;
				break;
			case 2 :
				atlas = atlasBase;
				break;
			case 3 :
				atlas = atlasBase;
				break;
			case 4 :
				atlas = atlasBase;
				break;
			case 5 :
				atlas = atlasBase;
				break;
			case 6 :
				atlas = atlasGround;
				break;
			case 7 :
				atlas = atlasGround;
				break;
		}
		return atlas;
	}
	
	var atlasRect : Vector2[];
	public function GetTaskRewardRectAsType(type : int) : Vector2
	{
		try{
			return atlasRect[type];
		}catch(e){
			return Vector2.zero;
		}
	}
	
	public static var ridemod : boolean = false;
	
	
	function GetHoleDescription(stoneID : String) : int{ 
		var holeValue : int = 0;
		for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.GameItem.Rows){
			if(rows["ItemID"].YuanColumnText == stoneID){
				holeValue = parseInt(rows["ItemValue"].YuanColumnText);
				return holeValue;
			}
		}
		return 0;
	}

//	function SyncAct(obj : GameObject , actType : int){
//	
//	}
//	
//	