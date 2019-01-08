#pragma strict
var instanceID : int = -1;
var photonView : PhotonView;
var Power : String;
var Prestige : String = "0";
var PVPPoint : String="0";
var PlayerID = 0; 
var PlayerName : String;
var ProID =1;
var BranchID : String = "0";
var TeamID = -1;
var GuildID = 0;
var GuildName ="";
var battlemod = false;
var battletime =0;
var ridemod = false;
var rollmod = false;
var dead = false;
private var arenaDead = false;

var Level :String;
var Experience :String;
var Exmult = 1.0;

var VIPLevel : int;
var ServingMoney : int;
var maxServingMoney : int;

var Maxhealth :String;
var Health :String;
var Maxmana:String;
var Mana :String;

var MultiStamina : int = 5;
var Stamina =0;
var Strength =0;
var Agility =0;
var Intellect =0;
var Focus =1000;


var AllStamina		:	String;
var AllStrength		:	String;
var AllAgility		:	String;
var AllIntellect	:	String;
var AllFocus		:	String;

var ATK				=	0;			//自身攻击力，按职业分为物理和魔法
var MaxATK			:	String;		//最大攻击力   = ATK + 武器加成
var Attackspeed		=	1.0;
var Crit 			:	String;		//暴击  max10000

var Armor			:	String;   
var Defense			=	0;			//防御 max10000    =Armor*10000/(Armor +400+80*攻击者LV)+ extraDefense
var extraDefense	=	0;
var Dodge			=	0;			//闪避 max10000 
var Resist			=	0;			//抗性 max10000   =Armor*2000/(Armor +400+60*攻击者LV)+ extraResist
var extraResist		=	0;
var Distortion		=	0;			//偏斜 max10000 
var Retrieval		:	String;

var Hide			=	false;
var Unableattack	=	0;
var Pearl			=	0;

var Bloodstone		:	String;
var Money			:	String;
var Soul			=	0;
var SoulPower		=	0;
var AutoAITime		=	0;

var NonPoint : int	=	5;

var weaponType : PlayerWeaponType	=	PlayerWeaponType.empty;
//var ts : TiShi;
var PVPRank	:	int;
var pvpNum	:	int = 0;
/// <summary>
/// 主角物体	-
/// </summary>
static	var MainCharacter : Transform;
//var MonsterUICL : MonsterUIControl;
var	MainCSsound: AudioClip[]; 
private var	fontPool : Fontpool; 
private var ArenCL : ArenaControl;
var Bei = 1;
var GOWCardValue : int = 1;
var DoubleCardValue : int = 1;
var PVPmyTeam : String;
private var Enemytag = "Enemy";
private var PositionID = "0000";
var myteaminfo = "";
private var title = "";
var EquipItem : String;
var HideHelmet : int;
var selfRide = -1;
var selfProperties : ExitGames.Client.Photon.Hashtable;
var rankInt : int;
private var mmap : Minimap;
var TController : ThirdPersonController; 
private var Animatc:ThirdAnimation; 
private var agent : NavMeshAgent;
private var pInfoInit : PlayerInfoInit;
var Body : Transform;
var isMine : boolean = true;
var rideQuality : int = 0;
var rideMap : int = 0;
var rideLevel : int = 0;
var PVPDuelTeam : String;
//function SetIsMine(bool : boolean){
//	isMine = bool;
//}

function SetInstanceID(myid : int){
	instanceID = myid;
}

function Awake(){
//	for(var i=0; i<50; i++){
//		print(GetExperience(i , 1));
//	}
	if(UIControl.mapType == MapType.jingjichang || Application.loadedLevelName == "Map200"){
    	Bei =5;
	}
	if(Health == "0"){
		Health = "200";
	}
	if(Maxhealth == "0"){
		Maxhealth = "200";
	}
	if(UIControl.mapType != MapType.zhucheng){
		photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;
	}
//	showexp();
thistransform = this.transform;
SendMessage("SetRemote",SendMessageOptions.DontRequireReceiver);       
agent = GetComponent.<NavMeshAgent>();
pInfoInit = GetComponent.<PlayerInfoInit>();
Animatc = GetComponent(ThirdAnimation);
//photonView = GetComponent(PhotonView); 
//TController = GetComponent(ThirdPersonController);	
//	//print(isMine + " == isMine");
	if(PlayerUtil.isMine(instanceID) && isMine){
		AllManage.psStatic = this;
//	if(ts == null){
//		ts = AllManage.tsStatic;
//	}
	try{
		TeamID = parseInt(Loading.TeamID);
	}catch(e){
		 TeamID = -1 ;
	}
	
	MainCharacter = transform;
	SendMessage("SetRemote",SendMessageOptions.DontRequireReceiver);    
	   
       selfProperties = new ExitGames.Client.Photon.Hashtable();
//       print(PlayerName + " ===================1111========" + PlayerID);
       selfProperties.Add("Name",PlayerName);
       selfProperties.Add("ProID",ProID);
       selfProperties.Add("BranchID",BranchID);
       selfProperties.Add("Level",Level);
       selfProperties.Add("VIPLevel",VIPLevel);       
       selfProperties.Add("PlayerID",PlayerID);   
       selfProperties.Add("TeamID",TeamID); 
       selfProperties.Add("GuildName",GuildName);
       selfProperties.Add("myteaminfo",myteaminfo);
       selfProperties.Add("title",title); 
       selfProperties.Add("PVPmyTeam",UIControl.myTeamInfo);   
       selfProperties.Add("EquipItem",EquipItem);           
       selfProperties.Add("HideHelmet",HideHelmet);           
       selfProperties.Add("rankInt",rankInt);           
       selfProperties.Add("selfRide",selfRide);  
       selfProperties.Add("rideMap",rideMap);         
   PhotonNetwork.SetPlayerCustomProperties(selfProperties);	
	}
	buffint = new Array (14);
}


function ShowMyName(){
//	objNameOther = GameObject.Instantiate(objMyName);
//	objNameOther.transform.parent = transform;
//	objNameOther.transform.localPosition = Vector3.zero;
//	var labelMyName : NameControl;
//	labelMyName = objNameOther.GetComponent(NameControl);
//	if(labelMyName){
//		labelMyName.UIName.text = str ;
//	}
	
		if(UIControl.mapType == MapType.jingjichang){
     				AllResources.FontpoolStatic.ezx.SpriteTeam.enabled = true;
     				}else{
     				AllResources.FontpoolStatic.ezx.SpriteTeam.enabled = false;
     				}
     			if(PVPmyTeam == "Red"){
					AllResources.FontpoolStatic.ezx.SpriteTeam.spriteName = "UIM_Captain2";
					}else
				if(PVPmyTeam == "Blue"){
					AllResources.FontpoolStatic.ezx.SpriteTeam.spriteName = "UIM_Captain1";
		
				}
}
var objNameOther : GameObject;

private var playerdead : PlayerDead;
private var uicl : UIControl;
var isTBteam : boolean = false;
function TBteam(){
	while(uicl == null){
		uicl = FindObjectOfType(UIControl);
		yield;
	}
	if(!PlayerUtil.isMine(instanceID)){
	gameObject.tag = "Player";
	Enemytag = "Enemy";
	}
	if((PVPmyTeam != UIControl.myTeamInfo || Application.loadedLevelName == "Map431") && !PlayerUtil.isMine(instanceID) && UIControl.mapType == MapType.jingjichang){
		yield;
		gameObject.tag = "Enemy";
		Enemytag = "Player";
	}
	isTBteam = true;
}

function SetEnemytag(tg : String){
	Enemytag = tg;
}

function SetTeamID(id : int){ 
	TeamID = id;
	var selfDead = new ExitGames.Client.Photon.Hashtable();
    selfDead.Add("TeamID",TeamID);
   	PhotonNetwork.SetPlayerCustomProperties(selfDead);	    
}

function Start () {
//		KDebug.Log(Application.loadedLevelName + "  =============== song =====  1 " + gameObject.name);
    Time.timeScale = 1.0;
	Unableattack = 100;
    mmap = FindObjectOfType(Minimap);

	if(PlayerUtil.isMine(instanceID) && isMine){
	PVPDuelTeam = UIControl.myTeamInfo;
	var selfDead = new ExitGames.Client.Photon.Hashtable();
	dead = false;
	arenaDead = false;
	ridemod = false;
    selfDead.Add("Dead",dead);
   	PhotonNetwork.SetPlayerCustomProperties(selfDead);	 
		if(invcl == null){
			invcl = AllManage.InvclStatic;
		}

	    alljoy.attackButton = false;
		PVPmyTeam = UIControl.myTeamInfo;
     	if(UIControl.mapType == MapType.jingjichang){
    	    Bei =5;
    	    PlayerID = parseInt(InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText); 
    	    PlayerName = InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText;
    	    ProID = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText); 
    	    BranchID = InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText;
			meAddP();
		}
	if(UIControl.myTeamInfo == "Red"){
		AllManage.areCLStatic.isArenaRed = true;
		Fpoint = GameObject.Find("spawn").transform;			
		FalloutDeath();
	}else
	if(UIControl.myTeamInfo == "Blue"){
		AllManage.areCLStatic.isArenaBlue = true;
		Fpoint = GameObject.Find("end").transform;			
		FalloutDeath();
	}else{
		Fpoint = GameObject.Find("spawn").transform;		
	}
	
	if(UIControl.mapType != MapType.zhucheng){
		playerdead = FindObjectOfType(PlayerDead);
		if(playerdead)
		playerdead.Player = transform;	
	}
		KDebug.Log(Application.loadedLevelName + "  =============== song =====  2 " + gameObject.name);

	var mm : boolean = false;
	while(!mm){
		if(InventoryControl.ytLoad){
			if(InventoryControl.yt.Rows.Count > 0){			
   	    		pvpNum = parseInt(InventoryControl.yt.Rows[0]["pvp1Num"].YuanColumnText); 
				PlayerID = parseInt(InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText); 
				ProID = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText); 
				switch(ProID){
					case 1:
						MultiStamina = 6;
						break;
					case 2:
						MultiStamina = 5;
						break;
					case 3:
						MultiStamina = 4;
						break;
				}
				if(! InRoom.GetInRoomInstantiate().isUpdatePlayerLevel){
					Level = InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText;  	
					Experience = InventoryControl.yt.Rows[0]["Exp"].YuanColumnText; 
				}else{
					if(PlayerID == parseInt(InRoom.GetInRoomInstantiate().playerID)){
						Experience = InRoom.GetInRoomInstantiate().playerExp;
						Level = InRoom.GetInRoomInstantiate().playerLevel ;  						
					}else{
						Level = InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText;  	
						Experience = InventoryControl.yt.Rows[0]["Exp"].YuanColumnText; 
					}
				}
				
				PlayerName = InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText;
				expmax = GetExperience(parseInt(Level),expmax);
				Exmult = parseInt(InventoryControl.yt.Rows[0]["Exmult"].YuanColumnText); 
				Power = GetBDInfoInt("Power" , 0).ToString();
				Prestige = GetBDInfoInt("Prestige" , 0).ToString();  
				PVPPoint = GetBDInfoInt("PVPPoint" , 0).ToString();
				VIPLevel = GetBDInfoInt("Serving" , 0);
				maxServingMoney = getNowServingMoney(VIPLevel);
				ServingMoney = GetBDInfoInt("ServingMoney" , 0);
				Stamina = GetBDInfoInt("Stamina" , 0);
				Strength = GetBDInfoInt("Strength" , 0);
				Agility =  GetBDInfoInt("Agility" , 0);
				Intellect = GetBDInfoInt("Intellect" , 0);
				Focus =  GetBDInfoInt("Focus" , 0);
				NonPoint = GetBDInfoInt("NonPoint" , 0);
				Bloodstone = GetBDInfoInt("Bloodstone" , 0).ToString();
				Money = GetBDInfoInt("Money" , 0).ToString();
				Soul = GetBDInfoInt("Soul" , 0);
				SoulPower = GetBDInfoInt("SoulPower" , 0); 
				PVPRank = GetBDInfoInt("Rank" , 0); 
				EquipItem = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText; 
				HideHelmet = GetBDInfoInt("HideHelmet" , 0); 
				AutoAITime = GetBDInfoInt("AutoAITime" , 0); 
				title = InventoryControl.yt.Rows[0]["SelectTitle"].YuanColumnText;
				if(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Length > 2){
					rideMap = parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(5,1));
					rideLevel = parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(2,2));
					rideQuality =  parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(4,1));
				}
				mm = true;
			}
		}
		yield;
	}
		KDebug.Log(Application.loadedLevelName + "  =============== song =====   3" + gameObject.name);
		Loading.PlayerName = PlayerName;
		if(UIControl.mapType != MapType.zhucheng)
        InvokeRepeating("Retrie", 0.5, 2); 
		   rankInt = AllManage.InvclStatic.GetNPCRankLevelAsName( InventoryControl.yt.Rows[0]["SelectTitle"].YuanColumnText);
		   selfProperties["Name"] = PlayerName;
		   selfProperties["PlayerID"] = PlayerID;
    	   selfProperties["Level"] = Level;
    	   selfProperties["VIPLevel"] = VIPLevel;
    	   selfProperties["ProID"] = ProID;
    	   selfProperties["BranchID"] = BranchID;
    	   selfProperties["TeamID"] = TeamID;
    	   selfProperties["GuildName"] = InventoryControl.yt.Rows[0]["GuildName"].YuanColumnText;    	    
    	   selfProperties["title"] = InventoryControl.yt.Rows[0]["SelectTitle"].YuanColumnText;  
    	   selfProperties["myteaminfo"] = PVPDuelTeam; 
    	   selfProperties["PVPmyTeam"] = PVPmyTeam;   
    	   selfProperties["EquipItem"] = EquipItem;   
    	   selfProperties["HideHelmet"] = HideHelmet;   
    	   selfProperties["rankInt"] = rankInt; 
    	   selfProperties["selfRide"] = selfRide;   
    	   selfProperties["rideMap"] = rideMap;   
    	   PhotonNetwork.SetPlayerCustomProperties(selfProperties);
  	    
			InvokeRepeating("MakePositionID", 0, 1);  
			yield WaitForSeconds(0.5);
//			photonView.RPC("PlayEffect",166);
			SendMessage("PlayEffect" , 166 , SendMessageOptions.DontRequireReceiver);
			ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "166");
		var colStr : String;
		if(VIPLevel == 9){
			colStr = "[ff8400]";
		}else{
			colStr = getNameColor(Level);	
		}
		if(invcl == null){
			invcl = AllManage.InvclStatic;
		}
//			print(PlayerPrefs.GetInt("ShowNickNameSelf", 0) + " =============ShowNickNameSelf");
			if( PlayerPrefs.GetInt("ShowNickNameSelf", 1) == 1){
		AllResources.FontpoolStatic.SpawnEffect(7 , transform , invcl.yanseLevel[ rankInt / 2 ] + title + "\n" + colStr+PlayerName + "\n" + GuildName , 5 , UIControl.myTeamInfo , this , rankInt);	
//				objNameMe = GameObject.Instantiate(objMyName);
//				objNameMe.transform.parent = transform;
//				objNameMe.transform.localPosition = Vector3.zero;
//				var labelMyName : NameControl;
//				labelMyName = objNameMe.GetComponent(NameControl);
//				if(labelMyName){
//					labelMyName.UIName.text = invcl.yanseLevel[ rankInt / 2 ] + title + "\n" + colStr+PlayerName + "\n" + GuildName ;
//				}
//				
//				if(UIControl.mapType == MapType.jingjichang){
//     				labelMyName.UITeam.enabled = true;
//     				}else{
//     				labelMyName.UITeam.enabled = false;
//     				}
//     			if(UIControl.myTeamInfo == "Red"){
//					labelMyName.UITeam.spriteName = "UIM_Captain2";
//					}else
//				if(UIControl.myTeamInfo == "Blue"){
//					labelMyName.UITeam.spriteName = "UIM_Captain1";
//		
//				}
			}
//	yield WaitForSeconds(1);
//	ServerRequest.requestAddToMap(Application.loadedLevelName.Substring(3,3) , Fpoint.position);
	for(var objDes :GameObject  in ObjectAccessor.aoiObject.Values)
		{
			if(objDes != null)
			{
				objDes.SendMessage("ClearPetArray" , SendMessageOptions.DontRequireReceiver);
				Destroy(objDes);
				//ObjectAccessor.removeAOIObject(instanceID);
			}
		}
	ObjectAccessor.clearAOIObject();
	
	
	
		
		KDebug.Log(Application.loadedLevelName + "  =============== song =====   4");
	AllResources.ar.ClientAddToMap(Fpoint.position);
//	NGUIDebug.Log(	instanceID + " ======FINISHED_CLIENT====== " + gameObject.name);
//	if(Application.loadedLevelName != "Map200")
//	{
	
	    var nd:ZMNetData  = new ZMNetData(parseInt(OpCode.LOADING_FINISHED_CLIENT));
	    KDebug.WriteLog("=======================LOADING_FINISHED_CLIENT,LOADING_FINISHED_CLIENT======================");
	    ZealmConnector.sendRequest(nd);
//	}
		ServerRequest.isAddToMap = true;
	}else{
		InvokeRepeating("PlayerBackPosition", 2, 2); 
		if(Application.loadedLevelName == "Map321" && ! isMine){
			InRoom.GetInRoomInstantiate().Playerdueld(PlayerID);
		}
	}
	yield WaitForSeconds(1);
	if(AllManage.UICLStatic)
		AllManage.UICLStatic.SetTeamList("");
		Unableattack = 0;
	//Localization.StaticLoc.FixBrokenWord();	
//	yield WaitForSeconds(5);
//	ShowSkillCanUpdate();
}

function PlayerBackPosition(){
	if(transform.position.y < -50){
		transform.position.y = 10;
	}
}

var objNameMe : GameObject;
var objMyName : GameObject;

function meAddP(){
	yield WaitForSeconds(1);
	var pss : PlayerStatus[];
	while(pss == null || pss.length < 2){
		pss = FindObjectsOfType(PlayerStatus);
//		print(pss.length + " == psslength");
		if(pss.length > 1){
			photonView.RPC("ServerAddP" , PhotonTargets.AllBuffered , PlayerID.ToString() ,UIControl.myTeamInfo , PlayerName);
			return;
		}
		yield WaitForSeconds(1);
	}
}

@RPC
function ServerAddP(PlayerID : String , myTeamInfo : String , PlayerName : String){
//	print("111111111111111111111111111111111 " + gameObject.name);
	ArenCL = FindObjectOfType(ArenaControl);
	ArenCL.AddP(PlayerID , myTeamInfo , PlayerName);
	ArenCL.refArenaPlayer();
}

function OnDestroy(){
	if(!PlayerUtil.isMine(instanceID)){
		InRoom.GetInRoomInstantiate().RemoveLookPlayer(PlayerID.ToString());
	}
//	if(UIControl.mapType == MapType.fuben){
//		AllManage.UICLStatic.SetTeamList("");
//	}
//	if(AllManage.UICLStatic){
//		AllManage.UICLStatic.SetTeamList("");	
//	}
}

private var px : int;
private var py : int;
private var lastPositionx = 0.0;
private var thistransform : Transform;
function MakePositionID(){
px = thistransform.position.x*0.03125 +11;
py = thistransform.position.z*0.03125 +11;
PositionID = px +""+ py;
if (PhotonNetwork.room && lastPositionx != thistransform.position.x){
	lastPositionx = thistransform.position.x;
	mmap.UpDatePositiontable(PhotonNetwork.player.ID, PositionID);
   }
}

var LabelTitle : GameObject = null;
function SetAttr(photonPlayer : PhotonPlayer){
if(!PlayerUtil.isMine(instanceID))
  Showbody(false);
//		print(PlayerName + " =1= " + PlayerID);
		PlayerName = photonPlayer.allProperties["Name"];
		PlayerID = photonPlayer.allProperties["PlayerID"];
		PVPDuelTeam = photonPlayer.allProperties["myteaminfo"];
//		print(PlayerName + " =2= " + PlayerID);
		
    	Level = photonPlayer.allProperties["Level"];
    	VIPLevel = photonPlayer.allProperties["VIPLevel"];
    	ProID = photonPlayer.allProperties["ProID"];
    	BranchID = photonPlayer.allProperties["BranchID"];
    	TeamID = photonPlayer.allProperties["TeamID"];
    	GuildName = photonPlayer.allProperties["GuildName"];
    	myteaminfo = photonPlayer.allProperties["myteaminfo"];
    	title = photonPlayer.allProperties["title"];
    	PVPmyTeam = photonPlayer.allProperties["PVPmyTeam"];
    	EquipItem = photonPlayer.allProperties["EquipItem"];
    	rankInt = photonPlayer.allProperties["rankInt"];
    	selfRide = photonPlayer.allProperties["selfRide"];
    	rideMap = photonPlayer.allProperties["rideMap"];
    	try{
			HideHelmet = photonPlayer.allProperties["HideHelmet"];    	
    	}catch(e){
    		HideHelmet = 0;
    	}
    	if(selfRide>=0)
    	rideOn(selfRide , rideMap); 
    	if(UIControl.mapType == MapType.jingjichang)
    	  TBteam();
    	  else if(Application.loadedLevelName == "Map712"||Application.loadedLevelName == "Map713")
    	  SetEnemy(); 
    	  if(!PlayerUtil.isMine(instanceID))
		showWeaponInv(EquipItem , HideHelmet);
	if(LabelTitle != null){
		AllResources.FontpoolStatic.UnspawnEffect(7 , LabelTitle);
	}
    	while(!fontPool){
    	fontPool = FindObjectOfType(Fontpool);
    	yield;
    	}
	var colStr : String;
	if(VIPLevel == 9){
		colStr = "[ff8400]";
	}else{
		colStr = getNameColor(Level);	
	}
	if(invcl == null){
		invcl = AllManage.InvclStatic;
	}
	  photonView.RPC("moverremont",PhotonTargets.All);
	yield WaitForSeconds(0.5);
	if(PlayerID == parseInt(InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText)){
		AllResources.FontpoolStatic.SpawnEffect(7 , transform , invcl.yanseLevel[ rankInt / 2 ] + title + "\n" + colStr+PlayerName + "\n" + GuildName , 5 , UIControl.myTeamInfo , this , rankInt);	
	}else{
		if( PlayerPrefs.GetInt("ShowNickNamePlayers", 1) == 1){
			AllResources.FontpoolStatic.SpawnEffect(7 , transform , invcl.yanseLevel[ rankInt / 2 ] + title + "\n" + colStr+PlayerName + "\n" + GuildName , 5 , PVPmyTeam , this , rankInt);			
//			var objName = GameObject.Instantiate(objMyName);
//				objName.transform.parent = transform;
//				objName.transform.localPosition = Vector3.zero;
//				var labelMyName : NameControl;
//				labelMyName = objName.GetComponent(NameControl);
//				if(labelMyName){
//					labelMyName.UIName.text = invcl.yanseLevel[ rankInt / 2 ] + title + "\n" + colStr+PlayerName + "\n" + GuildName ;
//				}
		}
	}
if(!PlayerUtil.isMine(instanceID)){
  Showbody(true);	
  	InRoom.GetInRoomInstantiate().AddLookPlayer(PlayerID.ToString() , photonView);
  }
	yield WaitForSeconds(1);
	//Localization.StaticLoc.FixBrokenWord();
}

@RPC
function moverremont(){
if(PlayerUtil.isMine(instanceID)){
 photonView.RPC("mover2",PhotonTargets.AllBuffered,this.transform.position);
 }
}


private var Fstr : String = ";";
var tPersonW : ThirdPersonWeapon;
function showWeaponInv(equStr : String , head : int){ 
	var i : int = 0;
	var useInvID : String[];
	useInvID = equStr.Split(Fstr.ToCharArray());
	if(useInvID.length < 2){
		return;
	}
	for(i=0; i<12; i++){	 
		if(useInvID[i] != ""){  
//			//print("sdlfjlsdfjlsdkfjlsdjkf == " + useInvID[i]);
			tPersonW.RPCShowWeapon(useInvID[i] , AllManage.InvclStatic.equepmentIDs[i] , head);
		}
	}
}

function reTitle(){ 
	rankInt = invcl.GetNPCRankLevelAsName( InventoryControl.yt.Rows[0]["SelectTitle"].YuanColumnText);
	selfProperties["rankInt"] = rankInt;
	selfProperties["title"] = InventoryControl.yt.Rows[0]["SelectTitle"].YuanColumnText;  
	PhotonNetwork.SetPlayerCustomProperties(selfProperties);
//	SetAttr(PhotonNetwork.player);
}

function getNameColor(str : String) : String{
	var useInt : int;
	useInt = parseInt(str);
	if(useInt < 20){
		return "[ffffff]";
	}else
	if(useInt < 40){
		return "[00ff00]";
	}else
	if(useInt < 60){
		return "[0096ff]";
	}else
	if(useInt < 80){
		return "[b400ff]";
	}
	return "";
}

function SetEnemy(){
if(!PlayerUtil.isMine(instanceID)){
gameObject.tag = "Enemy";
Enemytag = "Player";
}
}

private var oldHE : int = 0;
function Retrie(){
var MH = parseInt(Maxhealth);
var HE = parseInt(Health);
var RE = parseInt(Retrieval);
var MM = parseInt(Maxmana);
var MA = parseInt(Mana);
 	if(!dead  )
	{
//		print(" =============== " + gameObject); 
		if(PlayerUtil.isMine(instanceID)){
		
			switch (ProID){ 
			  case 1: 
					if( MA>0&&!battlemod){
						MA -=1;	
						}
					if( !battlemod ){
						HE+=RE*0.03;
						if(PlayerUtil.isMine(instanceID) && parseInt(Health) < parseInt(Maxhealth)){
							ServerRequest.requestAddHp(RE*0.03);	
						}
					}
			  break; 
			  
			  case 2:  									
					MA += 20;
					if( !battlemod ){
						HE+=RE*0.03;	
						if(PlayerUtil.isMine(instanceID) && parseInt(Health) < parseInt(Maxhealth)){
							ServerRequest.requestAddHp(RE*0.03);	
						}
					}
			  break; 
			  
			  case 3:
					MA += RE*0.02;
					if (!battlemod ){
						HE+=RE*0.02;
						if(PlayerUtil.isMine(instanceID) && parseInt(Health) < parseInt(Maxhealth)){
							ServerRequest.requestAddHp(RE*0.02);	
						}
					}
			  break; 
				}
				if(MA>MM)
				  MA = MM;
				if(HE>MH)
				 HE = MH;
			     Health = HE.ToString();
			     Mana = MA.ToString();
//			     print(Health + " =============== " + gameObject); 
			if (PhotonNetwork.connected && UIControl.mapType != MapType.zhucheng){
				 photonView.RPC("SynHealth",Health);
//				 photonView.RPC("SynMana",Mana);	
				 	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_SynMana, Mana);
				 }
		}
//	   if(battlemod) 	
	 	 AddBuff();
	}
}

function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}

function JianNonPoint(i : int){
	NonPoint -= i;
	if(NonPoint <0){
		NonPoint = 0;
	}
	InventoryControl.yt.Rows[0]["NonPoint"].YuanColumnText = NonPoint.ToString(); 
}

var myPes : int[];
function SetXunLian(its : int[] , pes : int[]){
	GetMaxPoint();
//   photonView.RPC("PlayEffect",161);
	SendMessage("PlayEffect" , 161 , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "161");
	PlayerInfoInit();
	Stamina = Mathf.Clamp(Stamina + its[0] , 0,99999);
	Strength = Mathf.Clamp(Strength + its[1] , 0,99999);
	Agility = Mathf.Clamp(Agility + its[2] , 0,99999);
	Intellect = Mathf.Clamp(Intellect  + its[3], 0,99999);
	Focus = Mathf.Clamp(Focus + its[4] , 0,1000+99999);
	InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText = Stamina.ToString();
	InventoryControl.yt.Rows[0]["Strength"].YuanColumnText = Strength.ToString();
	InventoryControl.yt.Rows[0]["Agility"].YuanColumnText = Agility.ToString();
	InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText = Intellect.ToString();
	InventoryControl.yt.Rows[0]["Focus"].YuanColumnText = Focus.ToString();
   
}

function useSetXunLian(){
	NonPoint = GetBDInfoInt("NonPoint" , 0);
	GetMaxPoint();
//   photonView.RPC("PlayEffect",161);
	SendMessage("PlayEffect" , 161 , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "161");
	PlayerInfoInit();
	SetEquepInfo(invcl.EquipStatus);		
}

var maxPoint : int = 0;
function GetMaxPoint(){
	maxPoint = 0;
	for(var i=0; i <=parseInt(Level); i++){
		maxPoint += parseInt(i*2) + 12;
	}
}

var Combat : int = 0;
var oldCom : int = 0;
function GetCombat() : int{
	oldCom = Combat;
	Combat = 0;
	Combat = parseInt(AllStamina) + parseInt(AllStrength)*0.5 + parseInt(AllAgility)*1.5 + parseInt(AllIntellect)*0.5 + parseInt(AllFocus)*2 + parseInt(Armor)*0.2 + parseInt(Crit)*0.8 + parseInt(Level)*60;
	for(var i=0; i<AllManage.InvclStatic.PES.length; i++){
		if(AllManage.InvclStatic.PES[i].inv != null){
			Combat += AllManage.InvclStatic.PES[i].iValue;
		}
	}
	if(Combat > oldCom && oldCom != 0){
		ServerRequest.ChangeForceValue(Combat);
		AllManage.tipclStatic.ShowCombat(Combat.ToString() , true);
	}else
	if(Combat < oldCom){
		ServerRequest.ChangeForceValue(Combat);
		AllManage.tipclStatic.ShowCombat(Combat.ToString() , false);
	}
	return Combat;
}


function SetEquepInfo(pes : int[]){
//	//print("sd'flsd;'fl'sdlf");
	myPes = new Array(pes.length);
	for(var i =0; i<pes.length; i++){
		myPes[i] = pes[i];
	}
	PlayerInfoInit();
	sArmor = pes[1];
	sDefense = pes[2];
	Focus += pes[3];
//	sCrit += pes[4];
	Stamina += pes[5];
	Strength += pes[6];
	Agility += pes[7];
	Intellect += pes[8]; 
switch (ProID){
  case 1:
  ATK +=((pes[6]*2+ pes[7])*0.2+pes[0])*(GOWCardValue+100)*0.01;
  sCrit +=pes[4]+pes[6]*0.4;
   Maxmana = "100"; 
  break;
  
  case 2:
  ATK +=((pes[7]*2+ pes[6])*0.2+pes[0])*(GOWCardValue+100)*0.01;
   sCrit +=pes[4]+pes[7]*0.4;
   Maxmana = "100"; 
  break;
  
  case 3:
  ATK +=(pes[8]*0.25+pes[0])*(GOWCardValue+100)*0.01;
   sCrit +=pes[4]+pes[8]*0.4;
  Maxmana =( parseInt(Maxmana) + pes[8]*6+pes[12]).ToString(); 
  break;    
 }		
	sRetrieval += pes[9];
	sResist += pes[11];
    sDodge += Agility*0.5;  
    sDistortion += Intellect*0.3 ; 
//	if(PhotonNetwork.room)
	Synattr(ATK,Maxmana,sArmor,sDefense,Focus,sCrit,Stamina,Strength,Agility,Intellect,sDodge,sDistortion);
	var useStr : String = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",ATK,Maxmana,sArmor,sDefense,Focus,sCrit,Stamina,Strength,Agility,Intellect,sDodge,sDistortion);
	if(syncActACT_PlaySynAttr != useStr || Time.time > ATime){
		ATime = Time.time + 10;
		syncActACT_PlaySynAttr = useStr;
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySynAttr, syncActACT_PlaySynAttr);
	}
	
//photonView.RPC("Synattr",ATK,Maxmana,sArmor,sDefense,Focus,sCrit,Stamina,Strength,Agility,Intellect,sDodge,sDistortion);		
   AddBuff();
//	//print(ATK + " == atk");
}

private var syncActACT_PlaySynAttr : String = "";
function OtherSetEquepInfo(pes : int[]){
//	return;
	OtherPlayerInfoInit();
	OthersArmor = pes[1];
	OthersDefense = pes[2];
	OtherFocus += pes[3];
//	sCrit += pes[4];
	OtherStamina += pes[5];
	OtherStrength += pes[6];
	OtherAgility += pes[7];
	OtherIntellect += pes[8]; 
switch (ProID){
  case 1:
  OtherATK +=((pes[6]*2+ pes[7])*0.2+pes[0])*(GOWCardValue+100)*0.01;
  OthersCrit +=pes[4]+pes[6]*0.4;
   OtherMaxmana = 100; 
  break;
  
  case 2:
  OtherATK +=((pes[7]*2+ pes[6])*0.2+pes[0])*(GOWCardValue+100)*0.01;
   OthersCrit +=pes[4]+pes[7]*0.4;
   OtherMaxmana = 100; 
  break;
  
  case 3:
  OtherATK +=(pes[8]*0.25+pes[0])*(GOWCardValue+100)*0.01;
   OthersCrit +=pes[4]+pes[8]*0.4;
  OtherMaxmana += pes[8]*6+pes[12]; 
  break;    
 }		
	OthersRetrieval += pes[9];
	OthersResist += pes[11];
    OthersDodge += Agility*0.5;  
    OthersDistortion += Intellect*0.3 ; 
    OtherMaxhealth = ((OtherStamina*MultiStamina)*Bei).ToString();
}

function ReturnSynattr(strs : String[]){
	Synattr(parseInt(strs[0]) , strs[1] , parseInt(strs[2]) , parseInt(strs[3]) , parseInt(strs[4]) , parseInt(strs[5]) , parseInt(strs[6]) , parseInt(strs[7]) , parseInt(strs[8]) , parseInt(strs[9]) , parseInt(strs[10]) , parseInt(strs[11]));
}

//@RPC
function Synattr(a1:int,a2:String,a3:int,a4:int,a5:int,a6:int,a7:int,a8:int,a9:int,a10:int,a11:int,a12:int){
ATK = a1;
Maxmana = a2;
sArmor = a3;
sDefense = a4;
Focus = a5;
sCrit = a6;
Stamina = a7;
Strength = a8;
Agility =a9;
Intellect =a10;
sDodge = a11;
sDistortion = a12;
}

function PlayerInfoInit(){
	if(gameObject.GetComponent(OtherPlayerInfo) == null){
		if(PlayerUtil.isMine(instanceID)){
			Stamina = GetBDInfoInt("Stamina" ,0 );
			Strength =  GetBDInfoInt("Strength" ,0 ); 
			Agility = GetBDInfoInt("Agility" ,0 ); 
			Intellect = GetBDInfoInt("Intellect" ,0 ); 
			Focus = GetBDInfoInt("Focus" ,0 ); 
		}else{
			Stamina = pInfoInit.Stamina;
			Strength = pInfoInit.Strength; 
			Agility = pInfoInit.Agility; 
			Intellect = pInfoInit.Intellect; 
			Focus = pInfoInit.Focus; 
			ProID = pInfoInit.proID; 
			PlayerName = pInfoInit.playerName; 
		}
	}
switch (ProID){
  case 1:
  MultiStamina = 6;
  ATK =(Strength*2+ Agility)*0.1;
   Maxmana = "100"; 
  break;
  
  case 2:
  MultiStamina = 5;
  ATK =(Agility*2+ Strength)*0.1;
   Maxmana = "100"; 
  break;
  
  case 3:
  MultiStamina = 4;
  ATK =Intellect*0.15;
  Maxmana = (Intellect*6).ToString(); 
  break;    
 }
 sCrit = 500;
 sArmor= 0;
 sDefense=0;
 sDodge= 500;  
 sResist =0;   
 sDistortion =500 ; 
 sRetrieval= 500; 
 sMaxhealth = Stamina*MultiStamina;  
}

var buffint:int[];

private var sCrit = 500;
var sArmor= 0;
var sDefense=0;
private var sDodge= 500;  
private var sResist =0;   
private var sDistortion =500; 
private var sRetrieval= 500; 
private var sMaxhealth = 0; 
private var oldMaxhealth = "";
function AddBuff(){
var tempATK = 0; 
switch (ProID){
  case 1:
  tempATK =(buffint[10]*2+ buffint[11])*0.08;
  break;
  
  case 2:
  tempATK =(buffint[11]*2+ buffint[10])*0.15;
  break;
  
  case 3:
  tempATK =buffint[12]*0.15;
  break;    
 }
// print(MaxATK + " == " + gameObject);
	MaxATK =(ATK + buffint[0] + tempATK).ToString();     //鏀瑰彉涓虹櫨鍒嗘瘮
//	//print(MaxATK + " == " + buffint[0] + " == " + tempATK);
	Crit = (sCrit + buffint[1]).ToString();    //鏆村嚮 max10000  鏀瑰彉涓虹櫨鍒嗘瘮
	Armor= (sArmor + buffint[2]).ToString();    //鏀瑰彉涓虹櫨鍒嗘瘮
	extraDefense= sDefense + buffint[3]; //闃插尽 max10000  鏀瑰彉涓虹櫨鍒嗘瘮
    Dodge= sDodge + buffint[4]+  buffint[11]*0.5;  //闂伩 max10000   鏀瑰彉涓虹櫨鍒嗘瘮
    extraResist = sResist + buffint[5];   //鎶楁€?max10000   鏀瑰彉涓虹櫨鍒嗘瘮
    Distortion =sDistortion + buffint[6] + buffint[12]*0.3; //鍋忔枩 max10000  鏀瑰彉涓虹櫨鍒嗘瘮
    Retrieval= (sRetrieval + buffint[7]).ToString(); //鍥炲閫熷害 榛樿5 鏀瑰彉涓虹櫨鍒嗘瘮
	AllStamina = (Stamina + buffint[9]).ToString();
	
	var boolInitMaxHealth : boolean = false;
	Maxhealth = ((parseInt(AllStamina)*MultiStamina + buffint[8])*Bei).ToString();
	
	if(PlayerUtil.isMine(instanceID) && oldMaxhealth != Maxhealth && AllManage.InvclStatic.sendACT_PlaySynAttr){
		if(oldMaxhealth == ""){
			boolInitMaxHealth = true;
		}
		ServerRequest.requestSetMaxHP(parseInt(Maxhealth) , boolInitMaxHealth , parseInt(Maxmana));
		oldMaxhealth = Maxhealth;
//		print(Health + " ==========================asdasdasdasdasdasdasdasdasdasdasdasdasd====" + Maxhealth);
	}
	
	AllStrength = (Strength + buffint[10]).ToString(); 
	AllAgility = (Agility + buffint[11]).ToString();
	AllIntellect = (Intellect + buffint[12]).ToString();
	AllFocus = (Focus + buffint[13]).ToString();
//	if(PhotonNetwork.room)
//	print(Maxhealth + " ========================= Maxhealth");
	SynMax(Maxhealth,MaxATK,Crit,Armor,extraDefense,Dodge,Resist,Distortion,Retrieval,AllStamina,AllStrength,AllAgility,AllIntellect,AllFocus,Maxmana);
	var useStr : String = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14}",Maxhealth,MaxATK,Crit,Armor,extraDefense,Dodge,Resist,Distortion,Retrieval,AllStamina,AllStrength,AllAgility,AllIntellect,AllFocus,Maxmana);
	if(syncActACT_PlaySynMax != useStr){
		syncActACT_PlaySynMax = useStr;
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySynMax, syncActACT_PlaySynMax);
	}

//photonView.RPC("SynMax",Maxhealth,MaxATK,Crit,Armor,extraDefense,Dodge,Resist,Distortion,Retrieval,AllStamina,AllStrength,AllAgility,AllIntellect,AllFocus);	
}

private var syncActACT_PlaySynMax : String = "";
function ReturnSynMax(strs : String[]){
	SynMax(strs[0] , strs[1] , strs[2] , strs[3] , parseInt(strs[4]) , parseInt(strs[5]) , parseInt(strs[6]) , parseInt(strs[7]) , strs[8] , strs[9] , strs[10] , strs[11] , strs[12] , strs[13] , strs[14]);
}

//@RPC
function SynMax(a1:String,a2:String,a3:String,a4:String,a5:int,a6:int,a7:int,a8:int,a9:String,a10:String,a11:String,a12:String,a13:String,a14:String,a15:String){
Maxhealth = a1;
MaxATK = a2;
Crit = a3;
Armor = a4;
extraDefense = a5;
Dodge = a6;
Resist = a7;
Distortion = a8;
Retrieval =a9;
AllStamina =a10;
AllStrength = a11;
AllAgility = a12;
AllIntellect =a13;
AllFocus = a14;
Maxmana = a15;
}


private var lastadd =0;
private var aa = false;
private var bb = true;
private var cc = true;
var oob : boolean = false;
private var ATime : int = 0;
function Update () {
	  if(battlemod&& bb){
	  	oob = true;
		SendMessage("OnFight" , oob , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayOnFight, "0");
	   bb = false;	   
	   }
	else if(battlemod && !bb && Time.time>=battletime+5){
	   battlemod=false;
	   oob = false;
		SendMessage("OnFight" , oob , SendMessageOptions.DontRequireReceiver);	
	   ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayOnFight, "1");
	   bb = true;
	   }

	if(Hide==true && !aa ){
		Showbody(false);
		if(PlayerUtil.isMine(instanceID))
			TController.Movespeed =1.5; 
		aa=true;
	}
	else if(Hide==false&&aa){
		Showbody(true);
		if(PlayerUtil.isMine(instanceID) &&TController.Movespeed>1)
			TController.Movespeed =1; 
		aa=false;		
	}
	if(ridemod&& cc){
	cc = false;
	if(PlayerUtil.isMine(instanceID)){
	TController.Movespeed = AllResources.ar.rideSpeedMove[rideQuality]; 
	 agent.speed=AllResources.ar.rideSpeedAutoMove[rideQuality];
	}	
	}
	else if(!ridemod && !cc){
	cc = true;
	rideClose();
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_rideClose, "");
	if(PlayerUtil.isMine(instanceID) &&TController.Movespeed>1){
	TController.Movespeed =1; 
	agent.speed=9;
	}	
	}
}

var nowBuffStr : String = "";
function SetNowBuff(buff : int[]){
	nowBuffStr = buff[0] + ";" + buff[1] + ";" + buff[2] + ";" + buff[3];
}



private var responseBuff : int[];
private var responseDam : int = 0;
function ResponseDamage(objs : Object[]){
	nowBuffStr = objs[4];
	var strs : String[] = nowBuffStr.Split(FStr.ToCharArray());
//	print(strs.length + " ===== responseBuff.length");
	if(strs.length >= 4){
		responseBuff = new int[strs.length];
		for(var i=1; i<responseBuff.length; i++){
			responseBuff[i] = parseInt(strs[i]);
		}
//		photonView.RPC("leachBuff",responseBuff); 
	}
	ridemod	=	false;
	responseDam = objs[2];
	AllResources.FontpoolStatic.SpawnEffect(3,transform,responseDam+"",2);
}

//----------------------------------------伤害临时数据减少GC----------------------------------------
private var qq : PlayerStatus;
private var pnumber	:	int;	//攻击者ID PlayerID//
private var damage :int;
private var Hatred=0;
private var Damagetype=0;    //0为默认物理伤害，1~4为魔法，1是技能（奥术），2是冰，3是火，4是暗影，5是毒（自然）  
private var attackerLV = 1;  //攻击者LV
private var target :Transform;
private var relativePlayerPosition	=	Vector3(0,0,0);		//相对玩家自身的相对坐标//
private var targetDirection =Vector3(0,0,-1);
private var damageT : int;
//----------------------------------------伤害临时数据减少GC----------------------------------------
/// <summary>
/// 主角的伤害函数	-
/// </summary>
function	ApplyDamage (	info	:	int[]	)
{ 
	//return;
	if(	dead	)	//不鞭尸//
		return;
	if(rollmod){
		AllResources.FontpoolStatic.SpawnEffect(10,transform,"闪避！",1.5);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_FontEffect, "10");
		return;	
	}
	battlemod		=	true;
	battletime		=	Time.time; 
	Unableattack	-=	1;
	if(	Unableattack >=	0	)
	{
		AllResources.FontpoolStatic.SpawnEffect(11,transform,"吸收！",1);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_FontEffect, "11");
		return;
	}
	pnumber		=	info[0];	//攻击者ID instanceID//
    damage		=	info[1];
    Hatred		=	info[2];
    Damagetype	=	info[3];
    attackerLV	=	info[4];      

	if((Damagetype==0||Damagetype ==6||Damagetype==1||Damagetype ==7) && Random.Range(0,10000)<Dodge)
	{
		AllResources.FontpoolStatic.SpawnEffect(10,transform,"闪避！",1.5);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_FontEffect, "10");
		return;
	}
	else if( Application.loadedLevelName != "Map200" && Random.Range(0,10000)<Distortion	)
	{
		AllResources.FontpoolStatic.SpawnEffect(9,transform,"偏斜！",2);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_FontEffect, "9");
   		return;
   	}
	target	=	FindWithID(pnumber,Enemytag);
	
	if(	target	)
	{
		relativePlayerPosition	=	transform.InverseTransformPoint(target.position);	//相对于玩家的坐标//
		targetDirection			=	transform.position	-	target.position;
//		photonView.RPC("setDirection", targetDirection.normalized);	
		SendMessage("setDirection" , targetDirection.normalized , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_SetDirection, AllResources.Vector3ToString(targetDirection.normalized));
		target.SendMessage(	"Hit",	transform,	SendMessageOptions.DontRequireReceiver	);
//		photonView.RPC("Behit",PhotonTargets.All);	
		SendMessage("Behit" , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_Behit, "");
		
	}
	var ARM	=	parseInt(	Armor	);
    if(Damagetype==0||Damagetype ==6||Damagetype==1||Damagetype ==7)	//物理、奥术//
    {
    	//攻击特效-
		if(	Damagetype	==	0	||	Damagetype	==	6	)
		{
//			photonView.RPC("PlayEffect",2);
			SendMessage("PlayEffect" , 2 , SendMessageOptions.DontRequireReceiver);
			ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "2");
		}
		else
		{
//			photonView.RPC("PlayEffect",3);
			SendMessage("PlayEffect" , 3 , SendMessageOptions.DontRequireReceiver);
			ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "3");
		}
//		photonView.RPC("PlayEffect",0);		
		SendMessage("PlayEffect" , 0 , SendMessageOptions.DontRequireReceiver);			
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "0");

		Defense	=	ARM*10000/(ARM +400+80*attackerLV)+ extraDefense;
		if (	relativePlayerPosition.z	<	0 && Random.Range(0,100)<30	)
		{
			damageT = damage*(10000-Defense*0.1)*0.0001;
			var setArray1 = new int[4];
            setArray1[0]= pnumber;
            setArray1[1]= 10;            
            setArray1[2]= 50; 
            setArray1[3]= 4;                                						
			SendMessage("ApplyBuff",setArray1,SendMessageOptions.DontRequireReceiver );
		}
    	else
			damageT	=	damage*(10000-Defense)*0.0001;	
	}
    else
    {
//		photonView.RPC("PlayEffect",120);
		SendMessage("PlayEffect" , 120 , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "120");
		Resist = ARM*4000/(ARM +400+60*attackerLV)+ extraResist;
		damageT = damage*(10000-Resist)*0.0001;    
    }
	var HE = parseInt(Health);
	HE -= damageT;
	if(UIControl.mapType == MapType.yewai && HE < 1){
		HE = 1;
	}
	Health = HE.ToString();
//	print("damageT ------------------------ == " + damageT);
	if(ObjectAccessor.getAOIObject(pnumber) == null){
		ridemod	=	false;
//		print(gameObject.tag + " tag ==================== ");
 		ServerRequest.requestDamage(instanceID , damageT , 0 , nowBuffStr);
 		nowBuffStr = "";
		if(Damagetype>5)	//暴击//
		{
			AllResources.FontpoolStatic.SpawnEffect(12,transform,"暴击！",1.5); 
			ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_FontEffect, "12");   
			AllResources.FontpoolStatic.SpawnEffect(4,transform,"-"+damageT,2);
		}
		else
			AllResources.FontpoolStatic.SpawnEffect(3,transform,"-"+damageT,2);
   }
	Hide=false;
//	photonView.RPC("hitanimation",PhotonTargets.All);
	SendMessage("hitanimation" , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_HitAnimation, "");
	SynHealth(Health);
//	photonView.RPC("SynHealth",Health);---no
	if(	Random.Range(0,10)<5){
		var intPlaySelfAudio : int = Random.Range(0,3);
//		photonView.RPC("PlaySelfAudio",Random.Range(0,3)); 		
		SendMessage("PlaySelfAudio" , intPlaySelfAudio , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySelfAudio, intPlaySelfAudio.ToString());
	}
	
	if (!arenaDead &&HE <= 0)
	{
		arenaDead = true;
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Death , "");
		if(target)
		{
			var setArray = new int[4];
			setArray[0]= PlayerID;
			setArray[1]= parseInt(Level);            
			setArray[2]= ProID; 
			setArray[3]= PVPRank; 
			target.SendMessage("Kills",setArray, SendMessageOptions.DontRequireReceiver);
		}
		if(PlayerUtil.isMine(instanceID))
			AllManage.tsStatic.LiaoTian(AllManage.AllMge.Loc.Get("info277"), Color.red);        
		return;
	}
}
 
 function PlayerCleanHatred(){
	if( BtnGameManager.isPlayerSelectEnamy){
		var colliders : Collider[] = Physics.OverlapSphere ( transform.position, 31);
		for (var hit in colliders) {
			if(hit.CompareTag ("Enemy"))
				hit.SendMessage("removeHatred" , PlayerID ,SendMessageOptions.DontRequireReceiver);
		}
	}
}
 
  var invcl : InventoryControl;
 function Map200GoOn(){
 	invcl = FindObjectOfType(InventoryControl);
 	var Story :Storycamera;
	Story = FindObjectOfType(Storycamera); 
    Story.Playani (3,transform.position); 
	yield WaitForSeconds(8);
	//TD_info.successTraining();
 	invcl.RetrunYT();
 }
 
function Kills(are : int[]){
if(PlayerUtil.isMine(instanceID) && isMine){
	DungeonControl.DungeonJiSha += 1;
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.KillEnemy , are[0].ToString());
	AllManage.mtwStatic.DoneKiller();
if(are[2]>3){
	if(parseInt(Level) >= 12&&Random.Range(0,10)>6){
		UseSoul((-1)*are[3]);
		 if(are[3]>2)
		 UseSoul((-1)*(are[3]-2)*10);
	}
	if(ArenaControl.areType == ArenaType.jingjichang && UIControl.mapType == MapType.jingjichang){
		AddPVP8Info(PVP8InfoType.Boss , 20);
		AddPVPPoint(20 * (-1));
	}
}
else{
   if( ArenCL == null){
  		ArenCL = FindObjectOfType(ArenaControl);
   }
//   print(" dao le zhe li");
   var useInt : int = 0; 
   var lv : int = 0;
   lv = parseInt(Level);
   if(are[1] > lv + 2){
		useInt = 15;
   }else
   if(are[1] < lv - 2){
		useInt = 5;   
   	}else{
   		useInt = 10;
   	}
//    
	AddPVPPoint(useInt * (-1));
	switch(are[2]){
		case 1:
			InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.WinSoldierNum , 1);			
			break;
		case 2:
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.WinRobberNum , 1);			
			break;
		case 3:
			InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.WinMasterNum , 1);			
			break;
	}
	var float1 : float;
	var float2 : float;
	float1 = parseInt(Health);
	float2 = parseInt(Maxhealth);
	if(Health == Maxhealth){
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.PerfectVS , 1);				
	}else
	if((float1 / float2) < 0.1){
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.NoseVS , 1);			
	}
	ArenCL.AddKill(are);
	}
	if(ArenaControl.areType == ArenaType.jingjichang && UIControl.mapType == MapType.jingjichang){
		AddPVP8Info(PVP8InfoType.Kill , useInt);
	}
  }
}

private var FStr : String = ";";
private var sb : StringBuilder;
function AddPVP8Info(type : PVP8InfoType , point : int){
	var PVP8info : String = "";
	var infos : String[]; 
	var intLength : int = 0;
	var useStr : String;
	
	PVP8info = InventoryControl.yt.Rows[0]["PVP8Info"].YuanColumnText;
	infos = PVP8info.Split(FStr.ToCharArray());
	if(infos.Length >= 10){
		intLength =  infos.length - 10;
	}else{
		intLength = 0;
	}
	
	useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + "," + parseInt(type) + "," + point + ";";
	
	PVP8info = "";
	sb = new StringBuilder();
	for(var i=intLength; i<infos.length; i++){
		sb.Append(infos[i]);
		sb.Append(";");
//		PVP8info += infos[i];
	}
	sb.Append(useStr);
	PVP8info = sb.ToString();
//	PVP8info += useStr;
	InventoryControl.yt.Rows[0]["PVP8Info"].YuanColumnText = PVP8info;
}

@RPC
function hitanimation(){
 Animatc.anim_.CrossFade("hit",0.1);
}

function ZHanChangFuHuo(){
}

function Miaosha(){
photonView.RPC("Die",PhotonTargets.All);

}

@RPC
function	Die()
{
	MonsterHandler.OnPlayerDead(	instanceID	);
	Health	=	"0";
	dead	=	true;
	ridemod	=	false;	
	Animatc.anim_["die"].layer			=	8;
	Animatc.anim_["die"].normalizedTime	=	0;
	Animatc.anim_.Play("die",PlayMode.StopAll);
	if(	temptag == ""	)
	{
		temptag = this.tag;
	}
	this.tag = "Ground";
	if(	PlayerUtil.isMine(instanceID) )
	{
		if(	ArenaControl.areType == ArenaType.juedou && AllManage.areCLStatic && Application.loadedLevelName != "Map321")
		{
			AllManage.areCLStatic.JueDouWin(false);
		}
		
		if(	isMine	)
		{ 
			var selfDead = new ExitGames.Client.Photon.Hashtable();
		    selfDead.Add("Dead",dead);
		   	PhotonNetwork.SetPlayerCustomProperties(selfDead);	    
		    Autoctrl.Wayfinding	=	false;		//关闭寻路//
	        TController.isControllable = false;
//		    photonView.RPC("PlaySelfAudio",3); 
			SendMessage("PlaySelfAudio" , 3 , SendMessageOptions.DontRequireReceiver);
		    ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySelfAudio, "3");
		    AllManage.UICLStatic.MakePreDead();
			yield WaitForSeconds(1.5);
//		    photonView.RPC("PlaySelfAudio",4);
			SendMessage("PlaySelfAudio" , 4 , SendMessageOptions.DontRequireReceiver);
			ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySelfAudio, "4");
			yield WaitForSeconds(3);
			playerdead = FindObjectOfType(PlayerDead);
			if(	playerdead)
		    	playerdead.Dead(parseInt(Level));
			if(Application.loadedLevelName == "Map200")
			{
				var TC : TalkControl;
				yield;
				while(!TC)
				{
					TC = FindObjectOfType(TalkControl);
					yield;
				}
				if(	TC	)
				{
					TC.LevelID = 2;
					TC.step = 0;
					TC.ShowTalkAsStep(gameObject , "Map200GoOn");
				}
				var Story :Storycamera;
				yield;
				Story = FindObjectOfType(Storycamera); 
				if(	Story	)
					Story.Playani (2,transform.position);
			}
	        if(	Application.loadedLevelName == "Map321" && AllManage.areCLStatic	)
	        {
				AllManage.areCLStatic.JueDouWin(false);	        
	        }
		}
		else
		{
//		    photonView.RPC("PlaySelfAudio",3); 
			SendMessage("PlaySelfAudio" , 3 , SendMessageOptions.DontRequireReceiver);
		    ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySelfAudio, "3");
			yield WaitForSeconds(1.5);
//		    photonView.RPC("PlaySelfAudio",4);
			SendMessage("PlaySelfAudio" , 4 , SendMessageOptions.DontRequireReceiver);
		    ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlaySelfAudio, "4");
			yield WaitForSeconds(5);
			UIControl.isYtFuben = false;
			PhotonNetwork.Destroy(gameObject);
			if(	Application.loadedLevelName == "Map321" && AllManage.areCLStatic	)
			{
				AllManage.areCLStatic.JueDouWin(true);
			}
			return;
		}
	}
	else
	{
		if(	ArenaControl.areType == ArenaType.juedou && AllManage.areCLStatic && (Application.loadedLevelName == "Map311" || Application.loadedLevelName == "Map321"))
		{
			AllManage.areCLStatic.JueDouWin(true);
		}
	}
	agent.enabled	=	false;
	agent.speed		=	0;
	try
	{
		if(	qiuai && qiuai.objs == this.transform)
    		qiuai.objs = null;
    }catch(e){}
}


private var temptag:String = "";
private var Fpoint : Transform;
function Respawn(i:int){
//     photonView.RPC("PlayEffect",106);  
		SendMessage("PlayEffect" , 106 , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "106");
		yield WaitForSeconds(2);     
     
switch (i){
  case 0:
	spawn();
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayerSpawn, "");
//  photonView.RPC("spawn",PhotonTargets.All); 
//  photonView.RPC("PlayEffect",114);  
	SendMessage("PlayEffect" , 114 , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "114");
  break;
  
  case 1:
  
  ServerRequest.syncAct(instanceID , CommonDefine.ACT_Mover, AllResources.ar.Vector3ToString(Fpoint.position));
  mover(Fpoint.position);
//  photonView.RPC("mover",Fpoint.position);
  Camera.main.SendMessage("Movespawn" , SendMessageOptions.DontRequireReceiver);
  spawn();
  ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayerSpawn, "");
//  photonView.RPC("spawn",PhotonTargets.All); 
  break;
  
  case 2:
  spawn();
  ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayerSpawn, "");
//  photonView.RPC("spawn",PhotonTargets.All); 
//  photonView.RPC("PlayEffect",114);  
	SendMessage("PlayEffect" , 114 , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "114");
     photonView.RPC("bigBoom",parseInt(MaxATK),"Enemy",10);
     ServerRequest.syncAct(instanceID , CommonDefine.ACT_BigBoom, String.Format("{0};{1};{2}", parseInt(MaxATK),"Enemy",10) );
  break;
  
  case 3:
  spawn();
  ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayerSpawn, "");
//  photonView.RPC("spawn",PhotonTargets.All);
//  photonView.RPC("PlayEffect",114); 
	SendMessage("PlayEffect" , 114 , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "114");
     photonView.RPC("bigBoom",parseInt(MaxATK),"Enemy",10);
     ServerRequest.syncAct(instanceID , CommonDefine.ACT_BigBoom, String.Format("{0};{1};{2}", parseInt(MaxATK),"Enemy",10) );
  addbuff();   
  break; 
  }
AllResources.FontpoolStatic.SpawnEffect(4,transform,"Respawn!",2); 
AllManage.AttackButtonStatic.ReSetAnXia(); 
}

@RPC
function mover(pposition : Vector3){ 
  transform.position = pposition;
}

@RPC
function spawn(){
	if(temptag != "")
 	   this.tag = temptag;
    if(UIControl.mapType == MapType.jingjichang)
    	TBteam();
	Animatc.anim_["die"].layer = -1;
	Animatc.anim_.Stop("die");
	Health = Maxhealth;
	if(PlayerUtil.isMine(instanceID))
		ServerRequest.requestAddHp(parseInt(Health));
	agent.speed=9;
	if(ProID==1)
		Mana = "30";
		else if(ProID==2)
		Mana = "0";
		else
		Mana = Maxmana;	
	dead = false;
	arenaDead = false;
	if(PlayerUtil.isMine(instanceID) && isMine){
        TController.isControllable = true;
        TController.down = false;
        TController.bing = false;
        TController.Movespeed = 1;
        TController.Buffspeed = 1;
        alljoy.attackButton = false;
        Attackspeed =1;
        Camera.main.SendMessage("Flash" , SendMessageOptions.DontRequireReceiver);
        SendMessage("resetSkill" , SendMessageOptions.DontRequireReceiver);
		if(ASkill == null){
			ASkill = GetComponent(ActiveSkill);
		}
        ASkill.busy = false;
		var selfDead = new ExitGames.Client.Photon.Hashtable();
	    selfDead.Add("Dead",dead);
	   	PhotonNetwork.SetPlayerCustomProperties(selfDead);	    
    }
   TController.yun = false;
   Animatc.anim_["down"].layer = -1;
   Animatc.anim_.Stop("down");
   SendMessage("RideAnimation", "idle",SendMessageOptions.DontRequireReceiver);	

}



function addbuff(){
       	  	var setArray = new int[4];
            setArray[0]= PlayerID;
            setArray[1]= 33;           
            setArray[2]= 0;
            setArray[3]= 15;            						
	   SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver ); 
	   yield;
            setArray[1]= 21;           
            setArray[2]= 100;
            setArray[3]= 15;            						
	   SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver ); 
}

function hitDirection() : Vector3 {
return targetDirection;
}

@RPC
function setDirection(Direction : Vector3){
	targetDirection=Direction;
}

//@RPC
function SynHealth(P : String){
	if(!dead)
	{
		var Phealth : int = 0;
		Phealth = parseInt(P);
		if( Phealth <= 0&&!dead){
		    photonView.RPC("Die",PhotonTargets.All);
			P = "0";
		}
		Health = P;
	//	//print(gameObject + " == " + Phealth + " == " + dead);
	}
}

@RPC
function SynMana(P : String){
Mana = P;
}

function ReturnShowbody(str : String){
	if(str == "0"){
		Showbody(true);
	}else
	if(str == "0"){
		Showbody(false);
	}
}

var bodybone : GameObject;
@RPC
function Showbody (dd : boolean ){
bodybone.SetActiveRecursively(dd);
}

/// <summary>
/// i为诸客户端统一的个体ID，Tag为玩家还是怪，返回在本地的怪物预设位置 -
/// </summary>
function	FindWithID (i:int,	tag:String) : Transform
{
	var gos : GameObject[];
	gos	= GameObject.FindGameObjectsWithTag(tag);
	var diff = -1;
	for(	var go : GameObject in gos)
	{
		if(	go.GetComponent(PlayerStatus)	)
			diff = go.GetComponent(PlayerStatus).instanceID;
		else if(go.GetComponent(MonsterStatus))
			diff = go.GetComponent(MonsterStatus).PlayerID; 
		if (	diff	== i)
			return go.transform;
    }
    return null;
}

function AddServingMoney(exp : int){
	ServingMoney += exp;
	maxServingMoney = getNowServingMoney(VIPLevel);
	while(ServingMoney > maxServingMoney && VIPLevel < 9){
		AddVIP(-1);
		maxServingMoney = getNowServingMoney(VIPLevel);
		yield;
	}
	InventoryControl.yt.Rows[0]["ServingMoney"].YuanColumnText = ServingMoney.ToString();
}

function AddVIP(i : int) : boolean{
	if(VIPLevel >= i){
		VIPLevel -= i; 
		if(VIPLevel > 9){
			 VIPLevel = 9;
		}
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.ServerON1VIP, VIPLevel);
		InRoom.GetInRoomInstantiate().SetTitle(yuan.YuanPhoton.TitleType.VIP, VIPLevel);
		InventoryControl.yt.Rows[0]["Serving"].YuanColumnText = VIPLevel.ToString(); 
		return true;
	}else{
//		ts.Show("当前体力不足"); 
		return false;
	}
}

function AddAutoAITime(i : int) : boolean{
	if(AutoAITime >= i){
		AutoAITime -= i; 
		InventoryControl.yt.Rows[0]["AutoAITime"].YuanColumnText = AutoAITime.ToString(); 
		return true;
	}else{
//		ts.Show("当前体力不足"); 
		return false;
	}
}

function getNowServingMoney(lv : int) : float{
	var max : int = 0;
	switch(lv){
		case 0 : max = 10; break;
		case 1 : max = 50; break;
		case 2 : max = 150; break;
		case 3 : max = 500; break;
		case 4 : max = 1000; break;
		case 5 : max = 2500; break;
		case 6 : max = 5000; break;
		case 7 : max = 10000; break;
		case 8 : max = 20000; break;			
	}
	return max;
} 

//function AddExperience(e : int){
//	print("sldjflsdjflsdjflsjdflksdjf EXP" + e);
//	InRoom.GetInRoomInstantiate().AddExperience(e , DoubleCardValue);
//	return;
//}

function ReturnAddExperienceF(ex : int){
	photonView.RPC("ReturnAddExperience",ex);
}

@RPC
function ReturnAddExperience(ex : int){
	if(PlayerUtil.isMine(instanceID) && isMine){
		Experience =  InRoom.GetInRoomInstantiate().playerExp;
		AllResources.FontpoolStatic.SpawnEffect(2,transform, "+" + ex.ToString() + " EXP",1.5);
	}
}

function ReturnFontEffect(i:int){
AllResources.FontpoolStatic.SpawnEffect(i,transform,"",1.5);
}

function ReturnUpDateLevelF(){
	photonView.RPC("ReturnUpDateLevel");
}

@RPC
function ReturnUpDateLevel(){
	if(PlayerUtil.isMine(instanceID) && isMine){
//		AddPower(-10);
		AllManage.isUpgrade = true;
		var LvPlus : int = 0;
		LvPlus = parseInt(InRoom.GetInRoomInstantiate().playerLevel) - parseInt(Level);
		Level = InRoom.GetInRoomInstantiate().playerLevel;
		if(UIControl.mapType == MapType.zhucheng){
			AllManage.tipclStatic.ShowUpgrade(Level);
			AllManage.isUpgrade = false;
			if(! AllManage.mtwStatic.MainPS.LookTaskIsDone("504")){
				AllManage.tipclStatic.ShowTextButton(AllManage.AllMge.Loc.Get( "info278" ) , "1" , AllManage.AllMge.Loc.Get( "info887" )  ,  AllManage.AllMge.Loc.Get( "info890" ) , AllManage.UICLStatic.gameObject , "SkillMoveOn");
			}
		}
		var lv = parseInt(Level);
		NonPoint = GetBDInfoInt("NonPoint" , 0);
//		NonPoint += lv*2 + 12; 		
//		InventoryControl.yt.Rows[0]["NonPoint"].YuanColumnText = NonPoint.ToString();
		 
		expmax = GetExperience(lv,expmax);
		shengji();
		if(AllManage.SkillCLStatic){
			AllManage.SkillCLStatic.useSkillPoint(-1 * LvPlus);
			AllManage.SkillCLStatic.TSSkillButton();	
		}else{
			try{
				InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText) + LvPlus).ToString();		
			}catch(e){
				InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText = "1";
			}
		}
		AllManage.tsStatic.particLevelUp.Emit();
		AllManage.UICLStatic.UpDateLevelShowZhuButtons();
		SendMessage("PrivateStatusInit" , SendMessageOptions.DontRequireReceiver);
		if(UIControl.mapType == MapType.zhucheng){
//			if(! AllManage.mtwStatic.MainPS.LookTaskIsDone("61")){
				ShowSkillCanUpdate();
//			}
		}	
	}
}

private var ASkill : ActiveSkill;
private var PSkill : PassiveSkill;

public var NewSkillName : String ;
public var NewSkillStr : String ;

function ShowSkillCanUpdate(){
	AllManage.UIALLPCStatic.showSkill32();
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	var i : int = 0;
	var useSKstr : String[] = InventoryControl.yt.Rows[0]["Skill"].YuanColumnText.Split(Fstr.ToCharArray());
	var useInt : int = 0;
	var useSkillObj : Skillclass; 
	var usePassSkillobj : Skillpassive;
	if(ASkill == null){
		ASkill = GetComponent(ActiveSkill);
	}
	if(PSkill == null){
		PSkill = GetComponent(PassiveSkill);
	}
	var useInt1 : int = 0;
	var bool : boolean = false;
//	try{
		for(i=0; i<23; i++){
			bool = true;
			if((i+1 >= 4 && i+1 <= 9) || (i+1 >= 18 && i+1 <= 20)){
				useInt1 = 1;
			}else
			if((i+1 >= 10 && i+1 <= 15) || (i+1 >= 21 && i+1 <= 23)){
				useInt1 = 2;	
			}
			if(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "0" && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "3"){
				if(useInt1.ToString() != InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText){
					bool = false;
				}
			}
			var o : int;
			if(i < 15){
				o = AllManage.AllMge.useSkillcl.personSkills[i].level[Mathf.Clamp(parseInt(ASkill.SkillP[i].level.ToString().Substring(0,1)),0,2)];		
			}else{
				o = AllManage.AllMge.useSkillcl.personSkills[i].level[Mathf.Clamp(parseInt(PSkill.SkillP[i-15].level.ToString().Substring(0,1)),0,2)];	
			}
			useInt = parseInt(useSKstr[i].Substring(0,1));
			if(parseInt(Level) >= o && useInt < 3){
				if(i < 15){
					useSkillObj = ASkill.GetSkillInfoWithI(i);
//					if(bool)
//						AllManage.tipclStatic.ShowNewSkills(AllManage.AllMge.Loc.Get(useSkillObj.name) , "ProID_" + ProID.ToString() + useSkillObj.ID.ToString().Substring(1,2));
						NewSkillName = AllManage.AllMge.Loc.Get(useSkillObj.name);
						NewSkillStr = "ProID_" + ProID.ToString() + useSkillObj.ID.ToString().Substring(1,2);
						AllManage.tipclStatic.ShowNewSkills(NewSkillName , NewSkillStr);
//						print(NewSkillName + " === " + NewSkillStr);
				}else{
//					usePassSkillobj = PSkill.GetSkillInfoAsID(i + 1); 
//					if(bool)
//						AllManage.tipclStatic.ShowNewSkills(AllManage.AllMge.Loc.Get(usePassSkillobj.name) , "ProID_" + ProID.ToString() + ( parseInt(usePassSkillobj.ID.ToString().Substring(1,2)) - 15).ToString());
//						NewSkillName = AllManage.AllMge.Loc.Get(usePassSkillobj.name);
//						NewSkillStr = "ProID_" + ProID.ToString() + ( parseInt(usePassSkillobj.ID.ToString().Substring(1,2)) - 15).ToString();
				}
			}
		}
//	}catch(e){
//	
//	}
}


function AddExperienceF(ExpArray : int[]){
	if(PlayerUtil.isMine(instanceID) && isMine){
		InRoom.GetInRoomInstantiate().AddExperience(ExpArray , DoubleCardValue);
	}
	return;
}



function getNowExp() : float{
	var f1 : float;
	var f2 : float;
	var ii : int;
	ii = parseInt(Level) - 1;
	if(ii < 0)
		ii = 0;
	if( Level != "1"){
		f1 = parseInt(Experience) -  GetExperience(ii,f2); 
	}else{
		f1 = parseInt(Experience);
	}
	if(f1 < 0)
		f1 = 0;
	return f1;
}

function getNextExp() : float{
	var f1 : float;
	var f2 : float; 
	var f3 : float;
	if( Level != "1"){
		f1 = GetExperience(parseInt(Level),f2) - GetExperience(parseInt(Level) - 1,f3); 
	}else{
		f1 = GetExperience(parseInt(Level),f2); 
	}
	return f1;
}

var strexp : String = "";
function showexp(){
	for(var i=1; i<100; i++){
		strexp += ("Level " + i + " = " + (GetExperience(i+1,1) - GetExperience(i,1)) + "\n");
	}
	//print(strexp);
}

private var levelo : float;
var expmax : int;
private var addition : float;
private var ratio : float;
function GetExperience(lv : int,ex : int){
	if(lv <= 15){
		ex = 50;
		addition = 500;
		levelo = 0;
		ratio = 100;
		while(levelo < lv){
			ex += addition;
			ratio += ratio*6.0/100.0;
			addition += (ratio/100.0)*100.0;
			levelo++;
		}
	}else
	{
			 ex = 18400;
             addition = 3000;
            for (var lvli = 16; lvli <= lv; lvli++)
            {
                if (lvli <= 40)
                {
                    addition =  Convert.ToInt32(addition * 1.19);
                }
                else
                {
                    addition = addition + 65000 + ((((lvli - 40) / 3)) * (((lvli - 40) / 3)) * (((lvli - 40) / 3)) * 800);
                }
                ex += addition;
                //Console.WriteLine(i + "级升下一级的经验为：" + (int)(addition * 2) + "，全部经验为:"+ex*2);
            }
		
		}
	return ex*2;
	}

function cube(x : float) : float{
	return x*x*x;
}

function isSoul(i : int) : boolean{
	if(i == 0){
		return true;
	}
	if(Soul >= i){
		return true;        
	}
	AllManage.tsStatic.Show("tips064");
	return false;
}

function UseSoul(sl : int) : boolean{
if(PlayerUtil.isMine(instanceID) && isMine){
	var showStr : String = ""; 
	if(sl == 0){
		return true;
	}
	if(Soul >= sl){
		Soul -= sl;
		InventoryControl.yt.Rows[0]["Soul"].YuanColumnText = Soul.ToString();
		if(sl != 0){
			if(sl > 0){
				showStr += AllManage.AllMge.Loc.Get("info552") + sl + AllManage.AllMge.Loc.Get("info553") + "\n";
				
			}
	        else{
				showStr += AllManage.AllMge.Loc.Get("info554") + (-sl) + AllManage.AllMge.Loc.Get("info553") + "\n";
	       		ForTheSoul += 1;
//				Debug.Log("JR-------------------------------------------------------------"+ForTheSoul.ToString());
	         }
//			AllManage.tsStatic.Show1(showStr);	
		}
		return true;        
	}
	AllManage.tsStatic.Show("tips064");
	return false;
	}
}

function isSoulPower(i : int) : boolean{
	if(i == 0){
		return true;
	}
	if(SoulPower >= i){
		return true;        
	}
	AllManage.tsStatic.Show("tips065");
	return false;
}

var ForTheSoul : int = 0;

function UseSoulPower(sl : int) : boolean{
if(PlayerUtil.isMine(instanceID) && isMine){
	var showStr : String = ""; 
	if(sl == 0){
		return true;
	}
	if(SoulPower >= sl){
		SoulPower -= sl;
		InventoryControl.yt.Rows[0]["SoulPower"].YuanColumnText = SoulPower.ToString();
		if(sl != 0){
			if(sl > 0){
				showStr += AllManage.AllMge.Loc.Get("info552") + sl +AllManage.AllMge.Loc.Get("info555") + "\n";
				AllManage.tsStatic.Show1(showStr);
			}
	        else{
				showStr += AllManage.AllMge.Loc.Get("info554") + (-sl) + AllManage.AllMge.Loc.Get("info555") ;
				EquipEnhance.instance.ShowMyItem("",showStr);
	        }
			
		}
		return true;        
	}
	AllManage.tsStatic.Show("tips065");
	return false;
	}
}

function isBlood(i : int) : boolean{
	if(i == 0){
		return true;
	}
	var BS = parseInt(Bloodstone);
	if(BS >= i){
		return true;        
	}
    //AllManage.tsStatic.Show("tips060");
	BtnGameManagerBack.my.SwitchToStore();
	return false;
}

function isMoney( i : int) : boolean{
	if(i == 0){
		return true;
	}
var MY = parseInt(Money);
	if(MY >= i){
		return true;
	}
	AllManage.tsStatic.Show("tips061");
	return false;
}

function isBrokenMaterial(mNums : int[]) : boolean{
	if(mNums[0] == 0 && mNums[1] == 0){
		return true;
	}
	var mIron = parseInt(InventoryControl.yt.Rows[0]["MarrowIron"].YuanColumnText);
	var mGold = parseInt(InventoryControl.yt.Rows[0]["MarrowGold"].YuanColumnText);
	
	if(mIron >= mNums[0] && mGold >= mNums[1]){
		return true;
	}
	
	if(mIron < mNums[0] && mGold < mNums[1]){
		AllManage.tsStatic.Show("meg0162");
	}else
	if(mIron < mNums[0]){
		AllManage.tsStatic.Show("info880");
	}else
	if(mGold < mNums[1]){
		AllManage.tsStatic.Show("info881");
	}
		
	return false;
}

function UseMoney( mon : int , blo : int) : boolean{
if(PlayerUtil.isMine(instanceID) && isMine){
	if(mon == 0 && blo == 0){
		return true;
	}
var MY = parseInt(Money);
var BS = parseInt(Bloodstone);
var showStr : String = ""; 
var showBuGou : String = "";
	if(MY >= mon && BS >= blo){
		MY -= mon;
		Money = MY.ToString();
		InventoryControl.yt.Rows[0]["Money"].YuanColumnText = MY.ToString();
		if(mon != 0){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Gold , (-1 * mon).ToString());
			if(mon > 0){
				showStr += AllManage.AllMge.Loc.Get("info552") + mon +AllManage.AllMge.Loc.Get("info335") + "\n";
				AllManage.tsStatic.Show1(showStr);
			}
	        else{
				showStr += AllManage.AllMge.Loc.Get("info554") + (-mon) + AllManage.AllMge.Loc.Get("info335")+ "\n";
				EquipEnhance.instance.ShowMyItem("",showStr);
	        }
		}
 		BS -= blo;
		Bloodstone = BS.ToString();
		InventoryControl.yt.Rows[0]["Bloodstone"].YuanColumnText = BS.ToString(); 
		if(blo != 0){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Blood , (-1 * blo).ToString());
			if(blo > 0){
				showStr += AllManage.AllMge.Loc.Get("info552")  +  blo + AllManage.AllMge.Loc.Get("messages053");
				AllManage.tsStatic.Show1(showStr);
			}
	        else{
				showStr += AllManage.AllMge.Loc.Get("info554") +  (-blo) + AllManage.AllMge.Loc.Get("messages053");
				EquipEnhance.instance.ShowMyItem("",showStr);
	        }
		}
//		if(ts == null){
//			ts = FindObjectOfType(TiShi);
//		}
			
		return true;        
	}else{
		if(MY < mon){
			showBuGou += AllManage.AllMge.Loc.Get("info491")  + "\n";
		}
		if(BS < blo){
			showBuGou += AllManage.AllMge.Loc.Get("info490");		
		}
	}
	AllManage.tsStatic.Show1(showBuGou);
	return false;
	}
}
var StrID : String;
function AddPower( i : int) : boolean{  
var POW : int;
POW = parseInt(Power);
	if(POW >= i){
		POW -= i; 
		if(POW > InventoryControl.xingdongzhi){
			 POW = InventoryControl.xingdongzhi;
		}
		Power = POW.ToString();
	    //InventoryControl.yt.Rows[0]["Power"].YuanColumnText = Power; 
		InRoom.GetInRoomInstantiate().AddPower(POW);
		AllManage.UICLStatic.CharBarTextPower(i);
		return true;
	}else{
		  				var Istrue : boolean = false;
						var StrItem = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
						var str : String[] = StrItem.Split(";"[0]); 
						for(var j= 0 ; j<str.Length ; j++)
						{
						if(str[j]!="")
							
							if(str[j].Substring(0,3).Equals("884")){
							StrID = str[j];
							Istrue = true;
							AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
							}
					if(!Istrue){
							AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
					}
					}
		
	}
}

function UseDaoju()
{
		AllManage.InvclStatic.UseDaojuAsID(StrID);
}
function UseShangdian()
	{
		AllManage.UICLStatic.StoreOpenMoveOn();
	}

function AddPrestige(i : int) : boolean{
var PRE : int;
PRE = parseInt(Prestige);
	if(PRE > i){
		PRE -= i; 
		Prestige = PRE.ToString();
		InventoryControl.yt.Rows[0]["Prestige"].YuanColumnText = Prestige; 
		
		InRoom.GetInRoomInstantiate().SetTitle(yuan.YuanPhoton.TitleType.PVEPoint , PRE );
		
		return true;
	}else{
		AllManage.tsStatic.Show("tips066"); 
		return false;
	}
}


function AddPVPPoint(i : int) : boolean{
var PRO : int;
PRO = parseInt(PVPPoint);
	if(PRO > i){
		PRO -= i; 
		PVPPoint = PRO.ToString();
		InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText = PVPPoint.ToString(); 
		InRoom.GetInRoomInstantiate().SetTitle(yuan.YuanPhoton.TitleType.LegionPoint , parseInt(PVPPoint)); 
//		
		var  pointmax : int;
		pointmax = GetExperience(PVPRank,pointmax);
		while(parseInt(PVPPoint) >=pointmax){	
			PVPRank += 1;
			pointmax = GetExperience(PVPRank,pointmax); 
			//yield;
		}
		InventoryControl.yt.Rows[0]["Rank"].YuanColumnText = PVPRank.ToString(); 
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.ServerON1Rank, PVPRank);
		return true;
	}else{
		AllManage.tsStatic.Show("tips067"); 
		return false;
	}
}

function CanAddHealth(){
//	photonView.RPC("RPCCanAddHealth",PhotonTargets.AllBuffered);	
}

//@RPC
//function RPCCanAddHealth(){
//	canHealth = true;
//	print(canHealth + " == " + gameObject.name);
//}

var canHealth : boolean = false;
function AddHealth( i : int){
//	if(!canHealth)
//		return;
var HE = parseInt(Health);
HE += i;
if(i>0)
AllResources.FontpoolStatic.SpawnEffect(1,transform,"+"+i,1);
else
AllResources.FontpoolStatic.SpawnEffect(3,transform,-i +"",1);
	if(HE>parseInt(Maxhealth))
	 HE = parseInt(Maxhealth);
	 else if(HE<=0)
	 HE = 0;
//	 	if(!canHealth)
//			return;
	if(PlayerUtil.isMine(instanceID)){
//		print("requestAddHp =========== " + i);
		ServerRequest.requestAddHp(i);
	}
  photonView.RPC("SynHealth",HE.ToString());	
}

function AddMana( i : int){
var MA = parseInt(Mana);
MA += i;
AllResources.FontpoolStatic.SpawnEffect(5,transform,i +"",1.5);
   if(MA>parseInt(Maxmana))
	  MA = parseInt(Maxmana);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_SynMana, MA.ToString());
	SynMana(MA.ToString());
//	photonView.RPC("SynMana",MA.ToString());
}	

function ChangeWeapons(i:int){
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_ChangeWeapon, i.ToString());
	photonView.RPC("ChangeWeapon",i);  
}

function shengji(){
//photonView.RPC("PlayEffect",4);
SendMessage("PlayEffect" , 4 , SendMessageOptions.DontRequireReceiver);
ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "4");
AllResources.FontpoolStatic.SpawnEffect(4,transform,AllManage.AllMge.Loc.Get("info681"),2);
//AddHealth(parseInt(Maxhealth));
//AddMana(parseInt(Maxmana));
}

//@RPC
function	PlaySelfAudio(i:int)
{
	if(MainCSsound[i])
		audio.PlayOneShot(MainCSsound[i],0.8);
}

function CompletTask(){
//photonView.RPC("PlayEffect",108);
	SendMessage("PlayEffect" , 108 , SendMessageOptions.DontRequireReceiver);
	ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, "108");
}

function FalloutDeath ()
{
	Respawn(1);
	return;
}

function SetNowMonster(monsterS : MonsterStatus){ 
	if(AllManage.MonsterUICLStatic){
		AllManage.MonsterUICLStatic.getMonster(monsterS);	
	}
}

function SetNowMonster(monsterS : PlayerStatus){ 
	if(AllManage.MonsterUICLStatic){
		AllManage.MonsterUICLStatic.getMonster(monsterS);	
	}
}

function SetNowMonster(){ 
	if(AllManage.MonsterUICLStatic){
		AllManage.MonsterUICLStatic.getMonster();	
	}
}

function UseXuePing(lv : int){
  var info = new int[4];
            info[0]= 0;
            info[1]= 18;            
            info[2]= 0;
            info[3]= 2;  
  photonView.RPC("PlaybuffEffect",info);
  ServerRequest.syncAct(instanceID , CommonDefine.ACT_PlaybuffEffect, String.Format("{0};{1};{2};{3}", info[0] , info[1] , info[2] , info[3]));
	AddHealth(parseInt(Maxhealth) * lv / parseInt(Level)*8);
	AddMana(parseInt(Maxmana) * lv / parseInt(Level)*8);
}

function HealthFull(){
  var info = new int[4];
            info[0]= 0;
            info[1]= 18;            
            info[2]= 0;
            info[3]= 2;  
  photonView.RPC("PlaybuffEffect",info);
  ServerRequest.syncAct(instanceID , CommonDefine.ACT_PlaybuffEffect, String.Format("{0};{1};{2};{3}", info[0] , info[1] , info[2] , info[3]));
	AddHealth(parseInt(Maxhealth));
	AddMana(parseInt(Maxmana));
}

function showTS(str : String){
	AllManage.tsStatic.Show1(str);
}

private var dd = true;
var Eating : boolean = false;
function PlayerAction(itemID : String){
	if(itemID.Substring(1,1)=="3"){
		if(!battlemod){
			if(itemID.Substring(0,1)!="0"){
				AllManage.WakCLStatic.StartEatFood();
			}
			Eating = true;
			var lv = parseInt(itemID.Substring(2,2));
			var stime = 12.0;
			var info = new int[4];
			info[0]= PlayerID;
			info[1]= 40;            
			info[2]= lv*10;
			info[3]= 3;   
			var itemValue : String = "";
			itemValue = AllManage.dungclStatic.GetItemValueAsID(itemID);
			//   AllManage.WakCLStatic.FallFood();
			if(itemValue != "" && parseInt(itemValue) > 0 && lv <= 10 && (lv % 2) == 0){
	 	 		InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.EatFish , parseInt(itemValue) , 0 , "");				
//				AddPower(parseInt(itemValue)  * -1);
			}
			if(AllManage.newUseItemCLStatic && AllManage.newUseItemCLStatic.gameObject.active){
				AllManage.newUseItemCLStatic.resetList();
			}

			while( Eating && stime>=0){
				if(dd){
					dd = false;                          						
					SendMessage("ApplyBuff",info,SendMessageOptions.DontRequireReceiver );
					yield WaitForSeconds(3);
					stime -=3.0;
					dd = true;
				}
				yield;
			}
			Eating = false;
			//   if(lv>10){
			//   AddPower(-5);
			//   }
		}else{
			AllManage.tsStatic.Show("tips068"); 
		}
	}
	else 
	if(itemID.Substring(1,1)=="8"){
		var lei = parseInt(itemID.Substring(2,1));
		//photonView.RPC("PlayEffect",lei+155);
		SendMessage("PlayEffect" , lei+155 , SendMessageOptions.DontRequireReceiver);
		ServerRequest.syncAct(instanceID , 	CommonDefine.ACT_PlayEffect, lei+"155");
	}

}

function StopEat(){
	//print("==============" + Eating);
	Eating = false;
}

function SaveAnotherPos(pos : Vector3){
	GameManager.reLevel = Application.loadedLevelName;
	GameManager.rePlayerName = PlayerName;
	GameManager.rePosition = pos;
}

function DoRetrie(){
	InvokeRepeating("Retrie", 0.5, 2); 
}

function ReturnRideOn(strs : String[]){
	rideOn(parseInt(strs[0]) , parseInt(strs[1]));
}

//var ridename : GameObject[];
private var ridemode : GameObject;
@RPC
function rideOn(i:int , map : int){
	rideMap = map;
    selfRide = i;
    if(i>=0){
	if(isMine && ridemode){
		Body.parent = this.transform;
		Body.localPosition = Vector3(0,0,0);
		Body.rotation = this.transform.rotation;
		Destroy(ridemode); 
    }
    ridemode = Instantiate(AllResources.ar.ridename[i], transform.position,transform.rotation);
    ridemode.transform.parent = this.transform;
    ridemode.transform.localPosition = Vector3(0,0,0);
    var ridebody : ride = GetComponentInChildren(ride); 
    
    ridebody.ChangeTexture(rideMap - 1);
    Animatc.rideanim_ = ridebody.rideanim_;
    Body.parent = ridebody.rideroot;
    Body.localPosition = Vector3(0,-15,0);
    if(PlayerUtil.isMine(instanceID)){
			AllResources.ridemod = true;
         var selfRideh = new ExitGames.Client.Photon.Hashtable();
         selfRideh.Add("selfRide",i);    
     PhotonNetwork.SetPlayerCustomProperties(selfRideh);  
     }  
     ridemod = true;
     if(objNameOther){
     objNameOther.transform.localPosition.y = 1;
     }
     if(objNameMe){
		objNameMe.transform.localPosition.y = 1;     
     }
     Animatc.anim_.CrossFade("ride"); 
     }
     if(PlayerUtil.isMine(instanceID)){
     	ServerRequest.requestRide(true);
     }
 }
 
@RPC
function rideOff(){
 selfRide = -1;
   ridemod = false;
   if(objNameMe){
  	 objNameMe.transform.localPosition.y = 0;
   }
     if(objNameOther){
     objNameOther.transform.localPosition.y = 0;
     }
   Body.parent = this.transform;
   Body.localPosition = Vector3(0,0,0);
   Body.rotation = this.transform.rotation;
   if( isMine && ridemode)
   Destroy(ridemode); 
    if(PlayerUtil.isMine(instanceID)){
  	 	AllResources.ridemod = false;
         var selfRideh = new ExitGames.Client.Photon.Hashtable();
         selfRideh.Add("selfRide",-1);    
     PhotonNetwork.SetPlayerCustomProperties(selfRideh); 
     } 
    if(PlayerUtil.isMine(instanceID)){
		ServerRequest.requestRide(false);
	}
     Animatc.RideAnimation("jumpend") ;
}

@RPC
function rideClose(){
selfRide = -1;
   Body.parent = this.transform;
   Body.localPosition = Vector3(0,0,0);
   Body.rotation = this.transform.rotation;
   if( isMine && ridemode)
   Destroy(ridemode); 
    if(PlayerUtil.isMine(instanceID)){
  	 	AllResources.ridemod = false;
         var selfRideh = new ExitGames.Client.Photon.Hashtable();
         selfRideh.Add("selfRide",-1);    
     PhotonNetwork.SetPlayerCustomProperties(selfRideh);  
     } 
     Animatc.anim_.Stop("ride");  
    if(PlayerUtil.isMine(instanceID)){
		ServerRequest.requestRide(false);
	}
}


var OthersCrit = 500;
var OthersArmor= 0;
var OthersDefense=0;
var OthersDodge= 500;  
var OthersResist =0;   
var OthersDistortion =500; 
var OthersRetrieval= 500; 
var OthersMaxhealth = 0; 
var OtherMaxmana=100;
var OtherMana =100;
var OtherStamina =0;
var OtherStrength =0;
var OtherAgility =0;
var OtherIntellect =0;
var OtherFocus =1000;
var OtherATK=0;    //自身攻击力，按职业分为物理和魔法
var OtherMaxATK :String = "0";  //最大攻击力   = ATK + 武器加成
var OtherMaxhealth :String = "0";
function OtherPlayerInfoInit(){
	OtherStamina = GetBDInfoInt("Stamina" ,0 );
	OtherStrength =  GetBDInfoInt("Strength" ,0 ); 
	OtherAgility = GetBDInfoInt("Agility" ,0 ); 
	OtherIntellect = GetBDInfoInt("Intellect" ,0 ); 
	OtherFocus = GetBDInfoInt("Focus" ,0 ); 
switch (ProID){
  case 1:
  OtherATK =(OtherStrength*2+ OtherAgility)*0.08;
   OtherMaxmana = 100; 
  break;
  
  case 2:
  OtherATK =(OtherAgility*2+ OtherStrength)*0.15;
   OtherMaxmana = 100; 
  break;
  
  case 3:
  OtherATK =OtherIntellect*0.15;
  OtherMaxmana = OtherIntellect*6; 
  break;    
 }
 OthersCrit = 500;
 OthersArmor= 0;
 OthersDefense=0;
 OthersDodge= 500;  
 OthersResist =0;   
 OthersDistortion =500 ; 
 OthersRetrieval= 500; 
 OthersMaxhealth = Stamina*MultiStamina;  
}

function closeNPCTalk(bool : boolean){
	if(PlayerUtil.isMine(instanceID) && isMine){
		if(bool){
			//AllManage.UICLStatic.buttonNpcTalk.transform.localScale = Vector3(0.5018393,0.5018393,0.5018393);	
			AllManage.UICLStatic.buttonNpcTalk.transform.localScale = Vector3.one;		
		}else{
			AllManage.UICLStatic.buttonNpcTalk.transform.localScale = Vector3.zero;					
		}
	}
}

function closeNpcTalkYes(id : int){
	if(PlayerUtil.isMine(instanceID) && isMine){
		AllManage.UICLStatic.buttonNpcTalk.transform.localScale = Vector3.one;	
		if(id == 0 || id == 2){
			AllManage.UICLStatic.npcTalkShan.enabled = true;
		}else{
			AllManage.UICLStatic.npcTalkShan.enabled = false;
		}
	}
}

function closeNpcTalkNo(id : int){
	if(PlayerUtil.isMine(instanceID) && isMine){
		AllManage.UICLStatic.buttonNpcTalk.transform.localScale = Vector3.zero;	
	}
}

function RefreshSoul(){
	Soul = GetBDInfoInt("Soul" , 0);
}

//function RotateToPosition(vec : Vector3){
//	var angle = 180.0;
//	//print(angle + "  == " + vec);
//	while (angle > 5 ){ 
//		//print(angle + " ===");
//		angle = Mathf.Abs(RotateTowardsPosition(vec));	
//		yield;
//	}
//}
//
//
//
//private function RotateTowardsPosition (targetPos : Vector3) : float
//{
//	var relative = transform.InverseTransformPoint(targetPos);
//	var angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
//	var maxRotation = 200 * Time.deltaTime;
//	var clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);
//	transform.Rotate(0, clampedAngle, 0);
//	return angle;
//}
