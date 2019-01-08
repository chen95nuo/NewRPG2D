#pragma strict
//class ArenaControl extends XmlControl{ 
var LocLabelGround : UILocalize;
var LocLabelGround1 : UILocalize;
var LocLabelGround2 : UILocalize;
var LocLabelReStart : UILocalize;
var LocLabelLiKai : UILocalize;
enum ArenaType{
	juedou = 0, 
	jingjichang = 1,
	zhanchang = 2
} 

class ArenaSettlement{
	var name : String;
	var icon : String;
	var jisha : String;
	var siwang : String;
	var jiqiao : String;
	var rongyu : String;
//	var PhotonP : PhotonPlayer;
	var at : ArenaItem;
	var pls : PlayerStatus;
} 

//var uiallCL : UIAllPanelControl;
static var ArenaIng : boolean = true;
static var isArenaRed : boolean = false;
static var isArenaBlue : boolean = false;
static var areType : ArenaType;
private var DC :DungeonControl;
private var cardCL : CardControl;
var LabelTitle : UILocalize;
var cardCLobj : GameObject;
var BackGrounds : GameObject[];
function Awake(){  
	isArenaRed = false;
	isArenaBlue = false;
//	transform.parent = null;
	AllManage.areCLStatic = this;
	pDead = AllManage.pDeadStatic;
	DC = AllManage.dungclStatic;
//	uiallCL = AllManage.UIALLPCSbattltatic;
	AllManage.UIALLPCStatic.SetGUI(BackGrounds[0] , "juedou" , 28);
	AllManage.UIALLPCStatic.SetGUI(BackGrounds[1] , "jingjichang" , 27);
	AllManage.UIALLPCStatic.SetGUI(BackGrounds[2] , "zhanchang" , 29);
	AllManage.UIALLPCStatic.SetGUI(BackGrounds[3] , "Newjuedou" , 62);
//	areType = ArenaType.jingjichang;
	ArenaIng = true;
	photonView = GetComponent(PhotonView);
	cardCL = cardCLobj.GetComponent(CardControl);
	cardCLobj.SetActiveRecursively(false);
	if(Application.loadedLevelName == "Map321"){
		InventoryControl.yt.Rows[0]["AimOfflinePlayer"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimOfflinePlayer"].YuanColumnText) + 1).ToString();
	}
//	photonView.viewID = 9999;
}

function OnDisable(){
	try{
		gameObject.active = true;	
	}catch(e){
	
	}
}



var CostTime : int;
var useCostTime : int;
static var Corps : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("Corps","id");
static var staticRoomArean : ExitGames.Client.Photon.Hashtable = new ExitGames.Client.Photon.Hashtable();
function Start () { 
	useCostTime = Time.time;
	var i : int = 0;
	var useInt : int = 0;
	var useSpStr : String = "";
	var strSP1 : String = "";
	var strSP2 : String = "";
	var strs : String[];
	strs = new Array(2);
	strs[0] = "Red";
	strs[1] = "Blue";
	var strTry : int;
//	for(i=0; i<strs.length; i++){		
//		if(PhotonNetwork.room && PhotonNetwork.room.customProperties){
//			try{
//				strTry = PhotonNetwork.room.customProperties["BattlefieldPoint" + strs[i]];
//				////print(strTry);
//				if(strTry)
//					useSpStr = strTry.ToString(); 
//			}catch(e){
//				useSpStr = null;			
//			}
//		}else{
//			useSpStr = null;
//		}
////		//print(useSpStr);
//		if(DungeonControl.staticRoomSP != null && useSpStr == null){
//			try{
//				strTry = DungeonControl.staticRoomSP["BattlefieldPoint" + strs[i]];
//				////print(strTry);
//				if(strTry)
//					useSpStr = strTry.ToString(); 
//			}catch(e){
//				useSpStr = null;			
//			}
//		}
//		
////		//print(useSpStr + " == useSpStr == " +strs[i] + " == ");
//		if(useSpStr != null && useSpStr != ""){
//			if(i==0)
//				BattlefieldPointRed = parseInt(useSpStr);
//			else
//				BattlefieldPointBlue = parseInt(useSpStr);
//		}else{
//			try{
//				DungeonControl.staticRoomSP.Add("BattlefieldPoint" + strs[i] , 0);			
//			}catch(e){			
//			}
//		}
//	}
//	if(PhotonNetwork.connected && PhotonNetwork.isMasterClient)
//	PhotonNetwork.room.SetCustomProperties(DungeonControl.staticRoomSP);

	 
	AllManage.UIALLPCStatic.show0();
	battleobj.SetActiveRecursively(false);
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	yield WaitForSeconds(10);
	var useInt3 : int =0;
//	//print(UIControl.mapType + " == UIControl.mapType");
	if(UIControl.mapType == MapType.jingjichang){
//		//print(areType + " == areType");
		switch(areType){
			case ArenaType.juedou :
				useInt3 = parseInt(InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText) + 1;
				InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText = useInt3.ToString();
				InventoryControl.yt.Rows[0]["AimPVP1"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimPVP1"].YuanColumnText) + 1).ToString();
				break;
			case ArenaType.jingjichang :
				InventoryControl.yt.Rows[0]["AimPVP24"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimPVP24"].YuanColumnText) + 1).ToString();
				break;
			case ArenaType.zhanchang :
//				//print(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText + " == times");
				useInt3 = parseInt(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText) + 1;
				InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText = useInt3.ToString();
//				//print(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText + " == times");
				InventoryControl.yt.Rows[0]["AimPVP8"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimPVP8"].YuanColumnText) + 1).ToString();
				break;
		}
		ptime = Time.time + 10;
//		//print(UIControl.ArenaType  + " == UIControl.ArenaType ");
		if(UIControl.arenaType == "2"){
//			//print( " == 11111 ");
			//InRoom.GetInRoomInstantiate().GetYuanTable("select * from Corps where id = " + InventoryControl.yt.Rows[0]["Corps2v2ID"].YuanColumnText,"DarkSword2",Corps);	
			 InRoom.GetInRoomInstantiate ().GetTableForID (BtnGameManager.yt.Rows[0]["Corps2v2ID"].YuanColumnText,yuan.YuanPhoton.TableType.Corps,Corps);
		}else
		if(UIControl.arenaType == "4"){
//			//print( " == 222222 ");
			//InRoom.GetInRoomInstantiate().GetYuanTable("select * from Corps where id = " + InventoryControl.yt.Rows[0]["Corps4v4ID"].YuanColumnText ,"DarkSword2",Corps);	
			InRoom.GetInRoomInstantiate ().GetTableForID (BtnGameManager.yt.Rows[0]["Corps4v4ID"].YuanColumnText,yuan.YuanPhoton.TableType.Corps,Corps);
		}
	}
}

var isJueDouWin : boolean = false;
var ButtonChongXin : Transform;
function JueDouWin(bool : boolean){
	if(!isJueDouWin){
		isJueDouWin = true;
		if(bool){
		    StartCoroutine(AllManage.dungclStatic.Showwin());		
		}
	    StartCoroutine(JueDouWinNext(bool));
	}
}

var JueDouFanpaiObj : Transform;

var JueDouFanpaiBtnNext : Transform;
function JueDouWinNext(bool : boolean){
		if(PVPTimeControl.PvpT){
			PVPTimeControl.PvpT.TimeStop();	
		}
	    yield WaitForSeconds(4);
		ButtonChongXin.position.y = 3000;
		var useInt : int;
		var useStr : String;
		var psLevel : int=0;
		psLevel = parseInt(AllManage.psStatic.Level); 
		try{
			InRoom.GetInRoomInstantiate().PVP1Fruit(UIControl.ytPVP.Rows[0]["PlayerID"].YuanColumnText , bool);
		}catch(e){
		
		}
		var PVP1info : String = "";
		var infos : String[]; 
		var intLength : int = 0;
		PVP1info = InventoryControl.yt.Rows[0]["PVP1Info"].YuanColumnText;
		infos = PVP1info.Split(FStr.ToCharArray());
		if(infos.length >= 10){
			intLength = infos.length - 10;
		}else{
			intLength = 0;
		}
		if(bool){
			useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + ",1," + otherJueDouName + ";";
			JueDouFanpaiObj.localPosition.y = -200;
			JueDouFanpaiBtnNext.localPosition.y = -200;
			LabelGround2.text = AllManage.AllMge.Loc.Get("messages038");
	//		InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText) + 10).ToString();	 	 	
			useInt = GetBDInfoInt("ColosseumVictoryNum",0) + 1;
			InventoryControl.yt.Rows[0]["ColosseumVictoryNum"].YuanColumnText = useInt.ToString();
	 	 	if(Application.loadedLevelName == "Map321"){
	 	 		InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.ShadowDemonWin , 0 , 0 , "");
//	 	 		ps.AddPower(-10);
	 	 	}else{
//	 	 		InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.ShadowDemonlose , 0 , 0 , "");
	 	 		ps.AddPVPPoint(10 * (-1));
				if(psLevel - otherPlayerLevel < 3){
		    		if(psLevel >= otherPlayerLevel){
						useInt = 50 - (psLevel - otherPlayerLevel)*20;
					}else{
						useInt = 50 + (otherPlayerLevel - psLevel)*20;
					}	
				}else{
					useInt = 0;
				}
				useInt = Mathf.Clamp(useInt , 0 , 100);
				InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText = (GetBDInfoInt("ColosseumPoint",1000) + useInt).ToString();
		 		LabelJuedouJiFen.text = AllManage.AllMge.Loc.Get("messages043")+InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText;
	 	 	}
		 	TransJueDouJiFen.localPosition.y = -16.78711;
			cardCLobj.SetActiveRecursively(true);
		}else{
			useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + ",0," + otherJueDouName + ";";
			LabelGround2.text = AllManage.AllMge.Loc.Get("messages039");
			JueDouFanpaiObj.localPosition.y = 1000;
			JueDouFanpaiBtnNext.localPosition.y = 1000;
			InventoryControl.yt.Rows[0]["ColosseumFailNum"].YuanColumnText =  (GetBDInfoInt("ColosseumFailNum",0) + 1).ToString();
			
			TransJueDouButtons.localPosition.y = 0;
			TransJueDouJiFen.localPosition.y = -16.78711;
			cardCLobj.SetActiveRecursively(true);
			if(Application.loadedLevelName == "Map321"){
//				ps.AddPower(-2);
			}else{
	 	 		ps.AddPVPPoint(5 * (-1));
				if(psLevel - otherPlayerLevel < 3){
		     		if(psLevel >= otherPlayerLevel){
						useInt = -1 * (50 + (psLevel - otherPlayerLevel)*20)/2;
					}else{
						useInt = -1 * (50 - (otherPlayerLevel - psLevel)*20)/2;
					}
				}else{
					useInt = -50;
				}
				useInt = Mathf.Clamp(useInt , -50 , -5); 
				var useInt2 : int = 0;
				useInt2 = GetBDInfoInt("ColosseumPoint",1000) + useInt;
				if(useInt2 < 0){
					useInt2 = 0;
				}
				InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText = useInt2.ToString();
				LabelJuedouJiFen.text = AllManage.AllMge.Loc.Get("messages043")+InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText;
	 	 	}
//	 	 	 PVPType  0为影魔 1为单人 2为双人 3为雪地 4为O诺诚
	 	 	if(Application.loadedLevelName == "Map311"){
	 	 		InRoom.GetInRoomInstantiate().PVPisFall(1);
	 	 	}else
	 	 	if(Application.loadedLevelName == "Map321"){
	 	 		InRoom.GetInRoomInstantiate().PVPisFall(0);
	 	 	}
		}
		PVP1info = "";
		for(var i=intLength; i<infos.length; i++){
			PVP1info += infos[i] + ";";
		}
		PVP1info += useStr;
		InventoryControl.yt.Rows[0]["PVP1Info"].YuanColumnText = PVP1info;
 		LabelJuedouShengLi.text = InventoryControl.yt.Rows[0]["ColosseumVictoryNum"].YuanColumnText;
 		LabelJuedouShiBai.text = InventoryControl.yt.Rows[0]["ColosseumFailNum"].YuanColumnText;
 		LabelJuedouPingJu.text = InventoryControl.yt.Rows[0]["ColosseumDrawNum"].YuanColumnText;
 		LabelJuedouDiaoXian.text = InventoryControl.yt.Rows[0]["ColosseumOutlineNum"].YuanColumnText;
 		LabelJuedouJiFen.text = AllManage.AllMge.Loc.Get("messages043") + InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText;
		LabelJuedouChangCi.text = (GetBDInfoInt("ColosseumVictoryNum",0) + GetBDInfoInt("ColosseumFailNum",0) + GetBDInfoInt("ColosseumDrawNum",0) + GetBDInfoInt("ColosseumOutlineNum",0)).ToString();
		FindPaiming();
		AllManage.UIALLPCStatic.show12();
}

//var pt : Transform; 
private var ptime : float;
function Update () {
	if(Time.time > ptime && UIControl.mapType == MapType.jingjichang){
		ptime = Time.time + 2; 
//		CostTime = Time.time;
		if(areType == ArenaType.juedou){
			FindDuelPlayer();
		}
//		print(isArenaRed + " == " + isArenaBlue + " == " +  ArenaIng );
//		if(isArenaRed && isArenaBlue && ArenaIng && PhotonNetwork.connected && !PhotonNetwork.offlineMode && Application.loadedLevelName != "Map321"){
		if(ArenaIng && PhotonNetwork.connected && !PhotonNetwork.offlineMode && Application.loadedLevelName != "Map321"){
//			if(areType == ArenaType.juedou){
//				LookArena();		
//			}
		}
		if(reStartRed && reStartBlue){
//			Loading.loadstr = UIControl.ArenaID;
//			Loading.Level = Application.loadedLevelName; 
//			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
//			Application.LoadLevel(Application.loadedLevelName);
			Loading.Level = Application.loadedLevelName;		
			Loading.AgainTimes += 1;
			Loading.YaoQingStr = PlayerInfo.mapName + "" +  Loading.AgainTimes;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	        alljoy.DontJump = true;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

		}
	}
	
//	pt.localPosition.y = 0;
}

private var FindOn : boolean = false;
function FindDuelPlayer(){
	if(FindOn){
		return;
	}
	if(FindPlayers && FindPlayers.length < 2){
		FindPlayers = FindObjectsOfType(PlayerStatus);
	}
	var i : int = 0;
	if(FindPlayers && FindPlayers.length == 2){
		for(i=0; i<FindPlayers.length; i++){
			if(FindPlayers[i] == null){
				FindOn = true;
				JueDouWin(true);
			}
		}
	}
}

function LookArena(){
	var jieshu : boolean = true; 
	jieshu = LookTeam(RedTeam , jieshu , "0");
//	//print(jieshu + " == 1" + ArenaIng + " == " + isArenaRed + " == " + isArenaBlue);
	if(jieshu && ArenaIng){ 
		if(Battlefieldtype == "xuedi"){
			WinTeamID = "0"; 
		}else{
			WinTeamID = "1"; 	
		}
		GameOver();
	}
	jieshu = true;
	jieshu = LookTeam(BlueTeam , jieshu , "1");
//	//print(jieshu + " == 2" + ArenaIng + " == " + isArenaRed + " == " + isArenaBlue);
	if(jieshu && ArenaIng){
		if(Battlefieldtype == "xuedi"){
			WinTeamID = "1"; 
		}else{
			WinTeamID = "0"; 	
		}
		GameOver();
	}
}

@RPC
function AddBattlefieldPointAsTeamID(red : int, blue : int  , team : String){
//	//print(PhotonNetwork.connected + " == " + PhotonNetwork.isMasterClient);
	if(PhotonNetwork.connected && PhotonNetwork.isMasterClient && !PhotonNetwork.offlineMode){
		if(team == "0"){
			red += 1;
		}else
		if(team == "1"){
			blue += 1;
		}
		BattlefieldPointRed = red;
		BattlefieldPointBlue = blue;
//    	AllManage.dungclStatic.staticRoomSP["BattlefieldPointRed"] = BattlefieldPointRed;
//    	AllManage.dungclStatic.staticRoomSP["BattlefieldPointBlue"] = BattlefieldPointBlue;
//    	AllManage.dungclStatic.SetMonsterStaticRoom();				
	}
	photonView.RPC("TongBuBattlefieldPoint",PhotonTargets.AllBuffered, BattlefieldPointRed,BattlefieldPointBlue);       
}

@RPC
function TongBuBattlefieldPoint(red : int, blue : int){
	BattlefieldPointRed = red;
	BattlefieldPointBlue = blue;
}

static var Battlefieldtype : String; 
var BattlefieldPointRed : int = 0;
var BattlefieldPointBlue : int = 0;
var BattlefieldHealth : int = 100;
private var bicl : BattlefieldIceControl; 
var battleobj :GameObject;
var LabelBattlefieldPoint : UILabel;
var LabelBattlefieldPoint1 : UILabel;
var BTJinKuangWinName : String;
function LookTeam(team : ArenaSettlement[] , js : boolean , nowTeam : String) : boolean{
	var i : int = 0; 
//	//print(areType + " == areType == " + Battlefieldtype);
	if(areType == ArenaType.zhanchang){
		if(bicl == null){
			bicl = FindObjectOfType(BattlefieldIceControl);
		}
        battleobj.SetActiveRecursively(true);
		if(Battlefieldtype == "xuedi"){
			if(PhotonNetwork.isMasterClient){
				var PointRed = 0;
				var PointBlue = 0;
				for(i=0; i<5; i++){
					if(bicl.Flags[i].myFlagID == "0"){
						photonView.RPC("AddBattlefieldPointAsTeamID",PhotonTargets.AllBuffered,BattlefieldPointRed , BattlefieldPointBlue , "0"); 
//						BattlefieldPointRed += 1;
					}else
					if(bicl.Flags[i].myFlagID == "1"){
						photonView.RPC("AddBattlefieldPointAsTeamID",PhotonTargets.AllBuffered,BattlefieldPointRed , BattlefieldPointBlue , "1"); 
//						BattlefieldPointBlue += 1;
					}
				}
//				photonView.RPC("AddBattlefieldPointAsTeamID",PhotonTargets.AllBuffered,BattlefieldPointRed , BattlefieldPointBlue); 
			}
			var BattlefieldPoint : int = 0;
//			//print(UIControl.myTeamInfo + " == UIControl.myTeamInfo");
			if(nowTeam == "0"){
				BattlefieldPoint = BattlefieldPointRed;
			}else
			if(nowTeam == "1"){
				BattlefieldPoint = BattlefieldPointBlue;
			}
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages046");
//			AllManage.AllMge.Keys.Add(BattlefieldPointRed + "");
//			AllManage.AllMge.Keys.Add("\n");
//			AllManage.AllMge.Keys.Add("messages047");
//			AllManage.AllMge.Keys.Add(BattlefieldPointBlue + "");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelBattlefieldPoint);
//			LabelBattlefieldPoint.text = "红队积分 : " + BattlefieldPointRed + "\n" + "蓝队积分 : " + BattlefieldPointBlue;
			AllManage.AllMge.SetLabelLanguageAsID(LabelBattlefieldPoint1);
//			LabelBattlefieldPoint1.text = "红队积分 : " + BattlefieldPointRed + "\n" + "蓝队积分 : " + BattlefieldPointBlue;
			if(BattlefieldPoint < 1000){
				js = false;
			}
		}else
		if(Application.loadedLevelName == "Map431"){
			js = false;
			for(i=0; i<BlueTeam.length; i++){
				if(parseInt(BlueTeam[i].jisha) >= 20){
					js = true;
					BTJinKuangWinName = BlueTeam[i].name;
					return js;
				}
			}
			for(i=0; i<RedTeam.length; i++){
				if(parseInt(RedTeam[i].jisha) >= 20){
					js = true;
					BTJinKuangWinName = RedTeam[i].name;
					return js;
				}
			}
		}else{
		    if(bicl.shitou.length<1){
		    	bicl.shitou = FindObjectsOfType(BattlefieldCityItem);
		    }
			
//			if(Application.loadedLevelName == "Map441"){
//				var bci : BattlefieldCityItem[];
//				bci = FindObjectsOfType(BattlefieldCityItem);
//				for(var b:BattlefieldCityItem in bci){
//					if(b.gameObject.name == "TaFangTargetA"){
//						bicl.shitou[0] = b;
//					}else
//					if(b.gameObject.name == "TaFangTargetB"){
//						bicl.shitou[1] = b;
//					}
//				}
//			}
//			//print(bicl.shitou[0].myTeam + " = " + bicl.shitou[1].myTeam + " = " + UIControl.myTeamInfo);
     if(bicl.shitou.length>0){
			if(bicl.shitou[0].myTeam == nowTeam){
				BattlefieldHealth = bicl.shitou[0].health;
			}
			if(bicl.shitou[1].myTeam == nowTeam){
				BattlefieldHealth = bicl.shitou[1].health;			
			}
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages048");
//			AllManage.AllMge.Keys.Add(bicl.shitou[0].health + "");
//			AllManage.AllMge.Keys.Add("\n");
//			AllManage.AllMge.Keys.Add("messages049");
//			AllManage.AllMge.Keys.Add(bicl.shitou[1].health + "");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelBattlefieldPoint);
//			AllManage.AllMge.SetLabelLanguageAsID(LabelBattlefieldPoint1);
			
//			LabelBattlefieldPoint.text = "红队水晶血量 : " + bicl.shitou[0].health + "\n" + "蓝队水晶血量 : " + bicl.shitou[1].health;
//			LabelBattlefieldPoint1.text = "红队水晶血量 : " + bicl.shitou[0].health + "\n" + "蓝队水晶血量 : " + bicl.shitou[1].health;
			}
			if(BattlefieldHealth > 0){ 
				js = false;
			}
		}
	}else{
//		for(i=0; i<team.length; i++){
//			if(team[i] != null){
//				if(team[i].pls != null ){
//					if(FindIsHavePlayerAsID(team[i].pls.PlayerID) && ! team[i].pls.dead){
//						 if(js){
//						 	js = false;
//						 }
//					}
//				}
//			}
//		}
	}
	return js;
}

function PlayerDieAsID(PlayerID : int){
	if(PlayerDieAsID.ToString() == InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText){
		JueDouWin(false);
	}else{
		JueDouWin(true);
	}
}

function FindIsHavePlayerAsID(id : int) : boolean{
	FindPlayers = FindObjectsOfType(PlayerStatus);
	var i : int = 0;
	if(PhotonNetwork.room){
		for(i=0; i< FindPlayers.length; i++){
			if(FindPlayers[i].PlayerID.ToString() == id.ToString()){
				return true;
			}
		}
	}
	return false;
}

var WinTeamID : String = "";
var LabelTime : UILabel;
private var bool30 : boolean = false;
function GameOver(){
//	//print("jue dou jie shu");
	 ArenaIng = false;
	 switch(areType){
	 	case ArenaType.juedou :
			OverJuedou();
		 	break;
	 	case ArenaType.jingjichang : 
	 		OverJingJi();
	 		ButtonJingJiJiXu.localPosition.y = 0;
	 		break;
	 	case ArenaType.zhanchang :  
//	 		OverZhanChang();
	 		ButtonZhanChangJiXu.localPosition.y = 35;
	 		break;
	}
	CostTime = Time.time - useCostTime;
}

var Label301 : UILabel;
var Label302 : UILabel;
function Do30Miao(){
	var useInt : int = 30;
	while(useInt > 0){
		useInt -= 1;
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(useInt + "");
//			AllManage.AllMge.Keys.Add("messages050");
//			AllManage.AllMge.SetLabelLanguageAsID(Label302);
//			AllManage.AllMge.SetLabelLanguageAsID(Label301);
//		Label302.text = useInt + "秒后即将传送";
//		Label301.text = useInt + "秒后即将传送";
		yield WaitForSeconds(1);
	}
	likai();
}
private var canShowWin : boolean = false;
function OverDuel(win : boolean){
	if(canShowWin || isJueDouWin){
		return;
	}
//	print("yun xing overduel");
	canShowWin = true;
	 AllManage.UICLStatic.MakePreArena();
	 yield;
	 yield;
	ArenaIng = false;
	CostTime = Time.time - useCostTime;
	
	var useInt : int;
	var useStr : String;
	var psLevel : int=0;
	psLevel = parseInt(AllManage.psStatic.Level); 
	var PVP1info : String = "";
	var infos : String[]; 
	var intLength : int = 0;
	PVP1info = InventoryControl.yt.Rows[0]["PVP1Info"].YuanColumnText;
	infos = PVP1info.Split(FStr.ToCharArray());
	if(infos.Length >= 10){
		intLength = infos.Length - 10;
	}else{
		intLength = 0;
	}
	if(win){
		useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + ",1," + otherJueDouName + ";";
//		LabelTitle.Keys.Clear();
//		LabelTitle.Keys.Add("messages038");
		useInt = GetBDInfoInt("ColosseumVictoryNum",0) + 1;
		InventoryControl.yt.Rows[0]["ColosseumVictoryNum"].YuanColumnText = useInt.ToString();
		if(psLevel >= otherPlayerLevel){
			useInt = 100 - (psLevel - otherPlayerLevel)*20;
		}else{
			useInt = 100 + (otherPlayerLevel - psLevel)*20;
		}
		useInt = Mathf.Clamp(useInt , 20 , 200);
		InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText = (GetBDInfoInt("ColosseumPoint",1000) + useInt).ToString();
//		AllManage.AllMge.Keys.Clear();
//		AllManage.AllMge.Keys.Add("messages043");
//		AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText);
//		AllManage.AllMge.SetLabelLanguageAsID(LabelJuedouJiFen);
 	 	ps.AddPVPPoint(10 * (-1));
        yield AllManage.dungclStatic.Showwin();
	 	TransJueDouJiFen.localPosition.y = -16.78711;
	}else{
		useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + ",0," + otherJueDouName + ";";
		if(psLevel >= otherPlayerLevel){
			useInt = (100 + (psLevel - otherPlayerLevel)*20)/2;
		}else{
			useInt = (100 - (otherPlayerLevel - psLevel)*20)/2;
		}
		useInt = Mathf.Clamp(useInt , 10 , 100); 
		
//		LabelTitle.Keys.Clear();
//		LabelTitle.Keys.Add("messages039");
		InventoryControl.yt.Rows[0]["ColosseumFailNum"].YuanColumnText =  (GetBDInfoInt("ColosseumFailNum",0) + 1).ToString();
		var useInt2 : int = 0;
		 useInt2 = GetBDInfoInt("ColosseumPoint",1000) - useInt;
		 if(useInt2 < 0){
		 	useInt2 = 0;
		 }
		InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText = useInt2.ToString();
//		AllManage.AllMge.Keys.Clear();
//		AllManage.AllMge.Keys.Add("messages043");
//		AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText);
//		AllManage.AllMge.SetLabelLanguageAsID(LabelJuedouJiFen);
		TransJueDouButtons.localPosition.y = 0;
		TransJueDouJiFen.localPosition.y = -16.78711;
	}
	
	for(var i=intLength; i<infos.length; i++){
		PVP1info += infos[i];
	}
	PVP1info += useStr;
	InventoryControl.yt.Rows[0]["PVP1Info"].YuanColumnText = PVP1info;
	cardCLobj.SetActiveRecursively(true);
	LabelJuedouChangCi.text = (GetBDInfoInt("ColosseumVictoryNum",0) + GetBDInfoInt("ColosseumFailNum",0) + GetBDInfoInt("ColosseumDrawNum",0) + GetBDInfoInt("ColosseumOutlineNum",0)).ToString();
	FindPaiming();
//	AllManage.AllMge.Keys.Clear();
//	AllManage.AllMge.Keys.Add("messages044");
//	AllManage.AllMge.Keys.Add(CostTime/60 + "");
//	AllManage.AllMge.Keys.Add("messages045");
//	AllManage.AllMge.SetLabelLanguageAsID(LabelTime);
//	AllManage.UIALLPCStatic.show12();
}

var LabelGround2 : UILabel;
var LabelTime2 : UILabel;
var ButtonZhanChangJiXu : Transform;
var ButtonZhanChangClose : Transform;
function OverZhanChang(objs : Object[]){
	var winTeamStr : String;
	winTeamStr = objs[6];
	AllManage.UICLStatic.MakePreArena();
	yield;
	yield;
	var useInt : int;
	var useStr : String;
	if(Application.loadedLevelName == "Map431"){
		if(UIControl.myTeamInfo == winTeamStr){
			AllManage.mtwStatic.DoneWin();
//			LabelGround2.text = "胜利";
//			LabelTitle.Keys.Clear();
//			LabelTitle.Keys.Add("messages038");
//			InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText) + 10).ToString(); 
			ps.AddPVPPoint(10 * (-1));
	        yield DC.Showwin();
		}else{
//			LabelGround2.text = "失败";
//			LabelTitle.Keys.Clear();
//			LabelTitle.Keys.Add("messages039");
			TransJueDouButtons.localPosition.y = 0;
//			TransJueDouJiFen.localPosition.y = 1000;

		}
	}else{
		
		invClear();
		teamRedScore = objs[0];
		teamBlueScore = objs[1];
		RedTeamFlag = objs[2];
		BlueTeamFlag = objs[3];
		PlayerCount  = objs[4];
		var ListPlayer : System.Collections.Generic.Dictionary.<String, String>[];
		ListPlayer = objs[5];
		var i : int = 0;
		var Obj : GameObject;
		var useAre : ArenaItem; 
		for(i=0; i<ListPlayer.length; i++){
			Obj = Instantiate(arenaIT.gameObject); 
			Obj.name = Obj.name + Time.time.ToString();
			Obj.transform.parent = invParentZhanChang;
			Obj.transform.localScale = Vector3.one;
			useAre = Obj.GetComponent(ArenaItem);
			useAre.SetArenaItem(ListPlayer[i] , 0);
			useAre.areCL = this; 
			addInvItem(useAre);				
		}
//		GID2.repsoitionNow = true;
	
		if(UIControl.myTeamInfo == winTeamStr || BTJinKuangWinName == ps.PlayerName){
			AllManage.mtwStatic.DoneWin();
			LabelGround2.text = AllManage.AllMge.Loc.Get("messages038");
			ps.AddPVPPoint(10 * (-1));
		    StartCoroutine(AllManage.dungclStatic.Showwin());
		    yield WaitForSeconds(3);
		    ButtonZhanChangJiXu.localPosition.y = 35;
		}else{
			LabelGround2.text = AllManage.AllMge.Loc.Get("messages039");
			TransZhanChangButtons.localPosition.y = 0;
	 	 	if(Application.loadedLevelName == "Map421"){
	 	 		InRoom.GetInRoomInstantiate().PVPisFall(3);
	 	 	}else
	 	 	if(Application.loadedLevelName == "Map411"){
	 	 		InRoom.GetInRoomInstantiate().PVPisFall(4);
	 	 	}
		}
	}
	ButtonZhanChangClose.localPosition.y = 3000;
	cardCLobj.SetActiveRecursively(true);
//	AllManage.AllMge.Keys.Clear();
//	AllManage.AllMge.Keys.Add("messages044");
//	AllManage.AllMge.Keys.Add(CostTime/60 + "");
//	AllManage.AllMge.Keys.Add("messages045");
//	AllManage.AllMge.SetLabelLanguageAsID(LabelTime2);
	endPVPTime = parseInt((System.DateTime.Parse(UIControl.BattlefieldEndTime) - InRoom.GetInRoomInstantiate().serverTime).TotalMinutes);
	LabelTime2.text = AllManage.AllMge.Loc.Get("info868") + endPVPTime + AllManage.AllMge.Loc.Get("messages045");
	AllManage.UIALLPCStatic.show14();
	ShowZhanChangUI();

	uiPanelZhanchang.transform.localPosition.y = 0;
	uiPanelZhanchang.clipOffset.y = 0;
	GID2.repositionNow = true;
	UICenterZhanChang.enabled = false;
	yield;
	yield;
	yield;
	UICenterZhanChang.enabled = true;
}

var TransZhanChangButtons : Transform;
var TransZhanChangJiFen : Transform;
var  EixtReward : ParticleEmitter;
function NextZhanChang(){
	if(EixtReward){
	 EixtReward.Emit();
	 }
//PVPType  0为影魔 1为单人 2为双人 3为雪地 4为O诺诚
	var pvpType : int = 0;
	switch(Application.loadedLevelName){
		case "Map321" :
			pvpType = 0;
			break;
		case "Map311" :
			pvpType = 1;
			break;
		case "Map421" :
			pvpType = 3;
			break;
		case "Map411" :
			pvpType = 4;
			break;
	}
//	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().DoneCardPVP(AllManage.dungclStatic.level , AllManage.dungclStatic.NowMapLevel , AllManage.dungclStatic.RanItem , pvpType));
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().DoneCardPVP(1 , pvpType));
}

function returnZhanChangCard(itemIDs : String[]){
	TransZhanChangButtons.localPosition.y = 0;
	TransZhanChangJiFen.localPosition.y = 1000;
	var invs : InventoryItem[] = new InventoryItem[5];
	for(var i=0; i<itemIDs.length; i++){
		invs[i] = AllResources.InvmakerStatic.GetItemInfo(itemIDs[i] , invs[i]);
	}
	cardCL.GoShowCards(invs , 1);
}

var LabelGround : UILabel;
private var FStr : String = ";";
function OverJuedou(){
	if(canShowWin){
		return;
	}
//	print("yunxing over juedou");
	canShowWin = true;
	 AllManage.UICLStatic.MakePreArena();
	 yield;
	 yield;
	var useInt : int;
	var useStr : String;
	var psLevel : int=0;
	psLevel = parseInt(AllManage.psStatic.Level); 
	var PVP1info : String = "";
	var infos : String[]; 
	var intLength : int = 0;
	PVP1info = InventoryControl.yt.Rows[0]["PVP1Info"].YuanColumnText;
	infos = PVP1info.Split(FStr.ToCharArray());
	if(infos.Length >= 10){
		intLength = 1;
	}else{
		intLength = 0;
	}
	if(WinTeamID == UIControl.myTeamInfo){
		useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + ",1," + otherJueDouName + ";";
		LabelTitle.Keys.Clear();
		LabelTitle.Keys.Add("messages038");
		useInt = GetBDInfoInt("ColosseumVictoryNum",0) + 1;
		InventoryControl.yt.Rows[0]["ColosseumVictoryNum"].YuanColumnText = useInt.ToString();
		if(psLevel >= otherPlayerLevel){
			useInt = 100 - (psLevel - otherPlayerLevel)*20;
		}else{
			useInt = 100 + (otherPlayerLevel - psLevel)*20;
		}
		useInt = Mathf.Clamp(useInt , 20 , 200);
		InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText = (GetBDInfoInt("ColosseumPoint",1000) + useInt).ToString();
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages043");
			AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText);
			AllManage.AllMge.SetLabelLanguageAsID(LabelJuedouJiFen);
	 	 	ps.AddPVPPoint(10 * (-1));
        yield DC.Showwin();
	 	
	}else{
		useStr = InRoom.GetInRoomInstantiate().serverTime.ToString() + ",0," + otherJueDouName + ";";
		if(psLevel >= otherPlayerLevel){
			useInt = (100 + (psLevel - otherPlayerLevel)*20)/2;
		}else{
			useInt = (100 - (otherPlayerLevel - psLevel)*20)/2;
		}
		useInt = Mathf.Clamp(useInt , 10 , 100); 
		
		LabelTitle.Keys.Clear();
		LabelTitle.Keys.Add("messages039");
		InventoryControl.yt.Rows[0]["ColosseumFailNum"].YuanColumnText =  (GetBDInfoInt("ColosseumFailNum",0) + 1).ToString();
		var useInt2 : int = 0;
		 useInt2 = GetBDInfoInt("ColosseumPoint",1000) - useInt;
		 if(useInt2 < 0){
		 	useInt2 = 0;
		 }
		InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText = useInt2.ToString();
		AllManage.AllMge.Keys.Clear();
		AllManage.AllMge.Keys.Add("messages043");
		AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText);
		AllManage.AllMge.SetLabelLanguageAsID(LabelJuedouJiFen);
		TransJueDouButtons.localPosition.y = 0;
	}
	
	for(var i=intLength; i<infos.length; i++){
		PVP1info += infos[i];
	}
	PVP1info += useStr;
	InventoryControl.yt.Rows[0]["PVP1Info"].YuanColumnText = PVP1info;
	cardCLobj.SetActiveRecursively(true);
	LabelJuedouChangCi.text = (GetBDInfoInt("ColosseumVictoryNum",0) + GetBDInfoInt("ColosseumFailNum",0) + GetBDInfoInt("ColosseumDrawNum",0) + GetBDInfoInt("ColosseumOutlineNum",0)).ToString();
	FindPaiming();
	AllManage.AllMge.Keys.Clear();
	AllManage.AllMge.Keys.Add("messages044");
	AllManage.AllMge.Keys.Add(CostTime/60 + "");
	AllManage.AllMge.Keys.Add("messages045");
	AllManage.AllMge.SetLabelLanguageAsID(LabelTime);
	AllManage.UIALLPCStatic.show12();
}

var TransJueDouButtons : Transform;
var TransJueDouJiFen : Transform;
var UISJX  : UISprite;
function NextJueDou(){
//	AllManage.tsStatic.RefreshBaffleOn();
	if(UISJX)
	{
	UISJX.enabled = true;
	}
	var pvpType : int = 0;
	switch(Application.loadedLevelName){
		case "Map321" :
			pvpType = 0;
			break;
		case "Map311" :
			pvpType = 1;
			break;
		case "Map421" :
			pvpType = 3;
			break;
		case "Map411" :
			pvpType = 4;
			break;
	}
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().DoneCardPVP(1 , pvpType));
//	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().DoneCardPVP(AllManage.dungclStatic.level , AllManage.dungclStatic.NowMapLevel , AllManage.dungclStatic.RanItem , pvpType));
//	;	
}

function returnDuelCard(objs : Object[]){
	var itemIDs : String[];
	itemIDs = objs[0];
	var PVPStone : int = 0;
	PVPStone = objs[1];
	cardCL.LabelPVPStone.text = AllManage.AllMge.Loc.Get("info1058") + PVPStone;
	TransJueDouButtons.localPosition.y = 0;
	TransJueDouJiFen.localPosition.y = 1000;
	
	TransZhanChangButtons.localPosition.y = 0;
	TransZhanChangJiFen.localPosition.y = 1000;
	
	var invs : InventoryItem[] = new InventoryItem[5];
	for(var i=0; i<itemIDs.length; i++){
		invs[i] = AllResources.InvmakerStatic.GetItemInfo(itemIDs[i] , invs[i]);
	}
	cardCL.GoShowCards(invs , 1);
}

function ReturnPVPisFall(pvpStone : int){
	cardCL.LabelPVPStoneFall.text = AllManage.AllMge.Loc.Get("info1058") + pvpStone;
}

var LabelGround1 : UILabel;
var LabelTime1 : UILabel;
private var pDead : PlayerDead;
var ButtonJingJiJiXu : Transform;
var ButtonJingJiClose : Transform;
function OverJingJi(){
	 AllManage.UICLStatic.MakePreArena();
	 yield;
	 yield;
	var useFloat : float;
	var useInt : int;
	var useStr : String;
	if(WinTeamID == UIControl.myTeamInfo){
		LabelTitle.Keys.Clear();
		LabelTitle.Keys.Add("messages038");
//		LabelGround1.text = "胜利";
//		InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["PVPPoint"].YuanColumnText) + 10).ToString(); 
	ps.AddPVPPoint(10 * (-1));
		Corps.Rows[0]["VictoryNum"].YuanColumnText = (GetCarpBDInfoInt("VictoryNum" , 0) + 1).ToString(); 
		
		useFloat = GetCarpBDInfoInt("Point" , 0); 
		if(useFloat <= 1500){
			useFloat +=  0.22 *  useFloat + 14;
		}else{
			 useFloat +=  1500 /  useFloat *330 + 14;
		}
		if(ArenaType == "2"){
			useFloat*= 0.75;
		}
		useInt = useFloat;
		Corps.Rows[0]["Point"].YuanColumnText = (GetCarpBDInfoInt("Point" , 0) + useInt).ToString(); 
		try{
			InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.ServerON1Arena, GetCarpBDInfoInt("Point" , 0));
		}catch(e){
			InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.ServerON1Arena,0);
		}
		InventoryControl.yt.Rows[0]["ListsPoint"].YuanColumnText = ( GetBDInfoInt( "ListsPoint" , 0) + useInt).ToString();  
		InRoom.GetInRoomInstantiate().UpdateYuanTable("DarkSword2",Corps,SystemInfo.deviceUniqueIdentifier);
		yield DC.Showwin();
	}else{
		Corps.Rows[0]["FailNum"].YuanColumnText = (GetCarpBDInfoInt("FailNum" , 0) + 1).ToString();
		LabelTitle.Keys.Clear();
		LabelTitle.Keys.Add("messages039");
//		LabelGround1.text = "失败";
	TransJueDouButtons.localPosition.y = 0;
//	TransJueDouJiFen.localPosition.y = 1000;
	}
//	 		LabelJuedouPingJu.text = InventoryControl.yt.Rows[0]["ColosseumDrawNum"].YuanColumnText;
//	 		LabelJuedouDiaoXian.text = InventoryControl.yt.Rows[0]["ColosseumOutlineNum"].YuanColumnText;
	ButtonJingJiClose.localPosition.y = 3000;
	cardCLobj.SetActiveRecursively(true);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages044");
			AllManage.AllMge.Keys.Add(CostTime/60 + "");
			AllManage.AllMge.Keys.Add("messages045");
			AllManage.AllMge.SetLabelLanguageAsID(LabelTime1);
//	LabelTime1.text = " 已用时间：" + CostTime/60+ "分钟";
	AllManage.UIALLPCStatic.show13();
	pDead.parsentJingJi.SetActiveRecursively(false);
}

var TransJingJiButtons : Transform;
var TransJingJiJiFen : Transform;
function NextJingJi(){
			if(!bool30){
				bool30 = true;
				Do30Miao();
			}
	TransJingJiButtons.localPosition.y = 0;
	TransJingJiJiFen.localPosition.y = 1000;
	cardCL.GoShowCards(AllManage.dungclStatic.GetThisMapInventoryItems() , 1);
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

function GetCarpBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(Corps.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}

static var RedTeam : ArenaSettlement[];
static var BlueTeam : ArenaSettlement[];
private var photonView : PhotonView;
private var ps : PlayerStatus;
function AddP(PlayerID : String , myTeamInfo : String , PlayerName : String){
//	ps = playerS;
//	//print(ps);
//	//print(UIControl.myTeamInfo);
//	//print(ps.PlayerID);
	if( !photonView ){
		photonView = GetComponent(PhotonView);
	}
//	yield WaitForSeconds(1);
	AddNewPlayer(PlayerID ,myTeamInfo , PlayerName);
//	photonView.RPC("AddNewPlayer" , PhotonTargets.AllBuffered , ps.PlayerID.ToString() ,UIControl.myTeamInfo , ps.PlayerName);
}

function AddKill(are : int[]){
	InRoom.GetInRoomInstantiate().BattlefieldKill();
//	photonView.RPC("SetKill" , PhotonTargets.All , ps.PlayerID.ToString());
//	AddRongyu(10);
}
//function AddBeKill(are : int[] , playerID : String){
function AddBeKill(){
	InRoom.GetInRoomInstantiate().BattlefieldDie();
//	photonView.RPC("SetBeKill" , PhotonTargets.All , playerID);
}
function AddJiqiao(flagID : String){
	InRoom.GetInRoomInstantiate().BattlefieldGetFlag(flagID);
//	ps.AddPVPPoint(fen * (-1));
//	photonView.RPC("SetJiqiao" , PhotonTargets.All , ps.PlayerID.ToString() , fen);
}
function AddRongyu(fen : int){
//	photonView.RPC("SetRongyu" , PhotonTargets.All , ps.PlayerID.ToString() , fen);
}

@RPC
function SetKill(myID : String){ 
//	//print("zhe li le 111" + myID);
	try{
		FindTeamAsID(myID).at.SetOneKill();	
	}catch(e){
	
	}
}
@RPC
function SetBeKill(myID : String){ 
//	//print("zhe li 555555");
	try{
		FindTeamAsID(myID).at.SetBeKill();
	}catch(e){
	
	}
}
@RPC
function SetJiqiao(myID : String , fen : int){ 
	try{
		FindTeamAsID(myID).at.SetJiqiao(fen);
	}catch(e){
	
	}
}
@RPC
function SetRongyu(myID : String , fen : int){ 
	try{
		FindTeamAsID(myID).at.SetRongyu(fen);
	}catch(e){
	
	}
}

function FindTeamAsID(myID : String) : ArenaSettlement{
	var team : ArenaSettlement;  
	var i : int = 0;	
	for(i=0; i<RedTeam.length; i++){
		if(RedTeam[i] != null){
			if(RedTeam[i].pls.PlayerID.ToString() == myID){
				team = RedTeam[i];
			}
		}
	} 
	for(i=0; i<BlueTeam.length; i++){
		if(BlueTeam[i] != null){
			if(BlueTeam[i].pls.PlayerID.ToString() == myID){
				team = BlueTeam[i];
			}
		}
	}
	return team;
}

var otherPlayerLevel : int = 0;
var otherJueDouName : String;
//@RPC
function AddNewPlayer(id : String , TeamID : String , name : String){
	if(id != InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText){
		otherJueDouName = name;
	}
	var ply : PlayerStatus;
	ply = FindPlayerAsID(id);
//	print(ply + " =---------1111-------------= " + id);
	while(ply == null){
		ply = FindPlayerAsID(id);
		yield;
	}
//	print(ply + " =---------2222-------------= " + id);
	if(ply != null){
		if(id != InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText){
			otherPlayerLevel = parseInt(ply.Level.ToString());
		}
		if(TeamID == "0"){
			isArenaRed = true;
			RedTeam = SetPlayerArray(ply , RedTeam);
		}else
		if(TeamID == "1"){
			isArenaBlue = true;
			BlueTeam = SetPlayerArray(ply , BlueTeam);		
		}
	}
//	refAllPlayer(RedTeam , BlueTeam);
}

//							objs = new object[6];
//							objs[0] = teamRedScore;
//							objs[1] = teamBlueScore;
//							objs[2] = RedTeamFlag;
//							objs[3] = BlueTeamFlag;
//							objs[4] = PlayerCount;
//							objs[5] = plist;

//							new object[6];
var teamRedScore : String = "";
var teamBlueScore: String = "";
var RedTeamFlag: String = "";
var BlueTeamFlag: String = "";
var PlayerCount: String = "";
var uiPanelZhanchang : UIPanel;
var UICenterZhanChang : UICenterOnChild;
private var endPVPTime : int = 0;
function RetrunShowPVPInfo(objs : Object[]){
	invClear();
	teamRedScore = objs[0];
	teamBlueScore = objs[1];
	RedTeamFlag = objs[2];
	BlueTeamFlag = objs[3];
	PlayerCount  = objs[4];
	var ListPlayer : System.Collections.Generic.Dictionary.<String, String>[];
	ListPlayer = objs[5];
	endPVPTime = parseInt((System.DateTime.Parse(UIControl.BattlefieldEndTime) - InRoom.GetInRoomInstantiate().serverTime).TotalMinutes);
	LabelTime2.text = AllManage.AllMge.Loc.Get("info868") + endPVPTime + AllManage.AllMge.Loc.Get("messages045");
	var i : int = 0;
	var Obj : GameObject;
	var useAre : ArenaItem; 
	for(i=0; i<ListPlayer.length; i++){
		Obj = Instantiate(arenaIT.gameObject); 
		Obj.name = Obj.name + Time.time.ToString();
		Obj.transform.parent = invParentZhanChang;
		Obj.transform.localScale = Vector3.one;
		useAre = Obj.GetComponent(ArenaItem);
		useAre.SetArenaItem(ListPlayer[i] , 0);
		useAre.areCL = this; 
		addInvItem(useAre);				
	}
	AllManage.UIALLPCStatic.nowStr = "Arenaccccc";
	ShowZhanChangUI();
//	yield WaitForSeconds(1);
	GID2.repositionNow = true;
	UICenterZhanChang.enabled = false;
	yield;
	yield;
	yield;
	uiPanelZhanchang.transform.localPosition.y = 0;
	uiPanelZhanchang.clipOffset.y = 0;
	UICenterZhanChang.enabled = true;
}

function ReturnSHowPVPWin(objs : Object[]){
	OverZhanChang(objs);
}

function refArenaPlayer(){
	refAllPlayer(RedTeam , BlueTeam);
}

var LabelJuedouChangCi : UILabel;
var LabelJuedouShengLi : UILabel;
var LabelJuedouShiBai : UILabel;
var LabelJuedouPingJu : UILabel;
var LabelJuedouDiaoXian : UILabel;
var LabelJuedouJiFen : UILabel;
var LabelJuedouPaiMing : UILabel;
function SetPlayerArray(PhotonP : PlayerStatus , team : ArenaSettlement[]) : ArenaSettlement[]{
	var tm : ArenaSettlement[];
	var i : int = 0; 
//	if(team == null || team.length == 0){
//		return;
//	} 
	if(team){
		for(i=0; i<team.length; i++){
			if(team[i].name == PhotonP.PlayerName){
				return team;			
			}
		}
	}
	tm = team; 
	try{
		team = new Array(tm.length + 1);	
		for(i=0; i<tm.length; i++){
			team[i] = tm[i];
		}
	}catch(e){
		team = new Array(1);
	}
	team[team.length - 1] = new ArenaSettlement();
	team[team.length - 1].pls = PhotonP;  
	
	switch(areType){
	 	case ArenaType.juedou : 
	 		LabelJuedouShengLi.text = InventoryControl.yt.Rows[0]["ColosseumVictoryNum"].YuanColumnText;
	 		LabelJuedouShiBai.text = InventoryControl.yt.Rows[0]["ColosseumFailNum"].YuanColumnText;
	 		LabelJuedouPingJu.text = InventoryControl.yt.Rows[0]["ColosseumDrawNum"].YuanColumnText;
	 		LabelJuedouDiaoXian.text = InventoryControl.yt.Rows[0]["ColosseumOutlineNum"].YuanColumnText;
	 		LabelJuedouChangCi.text = (GetBDInfoInt("ColosseumVictoryNum",0) + GetBDInfoInt("ColosseumFailNum",0) + GetBDInfoInt("ColosseumDrawNum",0) + GetBDInfoInt("ColosseumOutlineNum",0)).ToString();
	 		LabelJuedouJiFen.text = InventoryControl.yt.Rows[0]["ColosseumPoint"].YuanColumnText;
	 		FindPaiming();
		 	break;
	 	case ArenaType.jingjichang : 
			team[team.length - 1].name = PhotonP.PlayerName;
			team[team.length - 1].jiqiao = "0";
			team[team.length - 1].jisha = "0";
			team[team.length - 1].siwang = "0";
			team[team.length - 1].rongyu = "0";
			switch(PhotonP.ProID){
				case 1 : team[team.length - 1].icon = "head-zhanshi"; break;
				case 2 : team[team.length - 1].icon = "head-youxia"; break;
				case 3 : team[team.length - 1].icon = "head-fashi"; break;
			}
//			var Obj : GameObject = Instantiate(arenaIT.gameObject); 
//			var useAT : ArenaItem; 
//			Obj.transform.parent = invParent; 
//			useAT = Obj.GetComponent(ArenaItem); 
//			team[team.length - 1].at = useAT;
//			useAT.SetArenaItem(team[team.length - 1]);
//			useAT.areCL = this; 
	 		break;
	 	case ArenaType.zhanchang : 
//			team[team.length - 1].name = PhotonP.allProperties["Name"];
//			team[team.length - 1].jiqiao = "0";
//			team[team.length - 1].jisha = "0";
//			team[team.length - 1].siwang = "0";
//			team[team.length - 1].rongyu = "0";
//			switch(PhotonP.allProperties["ProID"]){
//				case 1 : team[team.length - 1].icon = "head-zhanshi"; break;
//				case 2 : team[team.length - 1].icon = "head-youxia"; break;
//				case 3 : team[team.length - 1].icon = "head-fashi"; break;
//			}
	 		break;
	} 
	
	Yieldguiwei();
	return team;
}

var ArenaPlayers : GameObject[];
var arenaIT : ArenaItem; 
var invParent : Transform;
var invParentJingJi : Transform;
var invParentZhanChang : Transform;
var GID : UIGrid;
var GID2 : UIGrid;
var invItemArray : ArenaItem[];
function refAllPlayer( tm1 : ArenaSettlement[] , tm2 : ArenaSettlement[] ){
//	if(tm1 == null || tm2 == null){
//		return;
//	}
	invClear();
	var i : int = 0;
	var Obj : GameObject;
	var useAre : ArenaItem; 
	if(areType == ArenaType.zhanchang){
		invParent = invParentZhanChang;
	}else
	{
		invParent = invParentJingJi;	
	}
	if(tm1){
		for(i=0; i<tm1.length; i++){
			if(tm1[i] != null){
				Obj = Instantiate(arenaIT.gameObject); 
				Obj.name = Obj.name + Time.time.ToString();
				Obj.transform.parent = invParent;
				useAre = Obj.GetComponent(ArenaItem);
//				useAre.SetArenaItem(tm1[i] , 0);
				useAre.areCL = this; 
				addInvItem(useAre);				
				RedTeam[i].at = useAre;
			}
		}  
	}
	if(tm2){
		for(i=0; i<tm2.length; i++){
			if(tm2[i] != null){
				Obj = Instantiate(arenaIT.gameObject); 
				Obj.name = Obj.name + Time.time.ToString();
				Obj.transform.parent = invParent;
				useAre = Obj.GetComponent(ArenaItem);
//				useAre.SetArenaItem(tm2[i] , 1);
				useAre.areCL = this; 
				addInvItem(useAre);	
				BlueTeam[i].at = useAre;
			}
		}
	}
	yield;
	GID.repositionNow = true;
	GID2.repositionNow = true;
}

//function resetList(){
//	SetEqupmentList("");
//}

function addInvItem(are : ArenaItem){
	var use : ArenaItem[]; 
	use = invItemArray; 
	invItemArray = new Array(invItemArray.length + 1);
	for(var i=0; i<(invItemArray.length - 1); i++){
		 invItemArray[i] = use[i];
	} 
	invItemArray[invItemArray.length - 1] = are;
}

function invClear(){
	for(var i=0; i<invItemArray.length; i++){
		if(invItemArray[i]){
			Destroy(invItemArray[i].gameObject);
		}
	}
	invItemArray = new Array(0);
}

function Yieldguiwei(){ 
	yield;
	GID.repositionNow = true;
	GID2.repositionNow = true;
} 

//var yt : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("juedouPaiMing","id"); 
var yt:YuanRank=new YuanRank("juedouPaiMing");
var mmo : boolean = false;
function FindPaiming(){
	var useInt : int = 0;
	var rankType:int=yuan.YuanPhoton.RankingType.Arena;
	//InRoom.GetInRoomInstantiate().GetYuanTable(String.Format("Select * from GameRanking where RankType={0} and PlayerID={1}",rankType,InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText),"DarkSword2",yt);
	InRoom.GetInRoomInstantiate ().GetRankOne (rankType,InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,yt);
	while(!mmo){
//		//print(Time.time);
		if(!yt.IsUpdate){
			 mmo = true;
			//if(yt.Count > 0){
			//	useInt = parseInt(yt.Rows[0]["Ranking"].YuanColumnText);
			//} 
			useInt=yt.myRank;
		}
		yield;
	}
//		//print(" == " + Time.time);
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages051");
//			AllManage.AllMge.Keys.Add(useInt + "");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelJuedouPaiMing);
	 LabelJuedouPaiMing.text = AllManage.AllMge.Loc.Get("messages051")+useInt.ToString();	
}

private var FindPlayers : PlayerStatus[];
function FindPlayerAsID(id : String) : PlayerStatus{
	FindPlayers = FindObjectsOfType(PlayerStatus);
//	//print(PhotonNetwork.room.playerCount + " == PhotonNetwork.room.playerCount");
//	if(PhotonNetwork.room){
		for(var i=0; i< FindPlayers.length; i++){
	//		//print(PhotonNetwork.playerList[i].allProperties["PlayerID"] + " == PhotonNetwork.playerList[i].allProperties");
			if(FindPlayers[i].PlayerID.ToString() == id){
				return FindPlayers[i];
			}
		}
//	}
	return null;
} 

function OpenView(){
	switch(areType){
		case ArenaType.juedou : AllManage.UIALLPCStatic.show12(); break;
		case ArenaType.jingjichang : AllManage.UIALLPCStatic.show13(); break;
		case ArenaType.zhanchang : AllManage.UIALLPCStatic.show14(); break;
	}
}

function likai(){
	switch(areType){
		case ArenaType.juedou : AllManage.UIALLPCStatic.show12(); InRoom.GetInRoomInstantiate().PVPTeamDissolve(); break;
		case ArenaType.jingjichang : AllManage.UIALLPCStatic.show13(); break;
		case ArenaType.zhanchang : AllManage.UIALLPCStatic.show14(); InRoom.GetInRoomInstantiate().LegionOneRemove(); InRoom.GetInRoomInstantiate().ActivityPVPRemove();break;
	}
	InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
	Loading.Level = DungeonControl.ReLevel;
	InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
	InRoom.GetInRoomInstantiate().BattlefieldExit();
	AllManage.UICLStatic.RemoveAllTeam();
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

var reStartRed : boolean = false;
var reStartBlue : boolean = false;
var SpriteReStart : UISprite;
var LabelReStart : UILabel;
function reStart(){
	if(SpriteReStart.spriteName == "UIH_Main_Button_N"){
		SpriteReStart.spriteName = "UIH_Main_Button_A";	
		if(UIControl.myTeamInfo == "Red"){
			photonView.RPC("redReStart" , PhotonTargets.AllBuffered);	
		}else
		if(UIControl.myTeamInfo == "Blue"){
			photonView.RPC("blueReStart" , PhotonTargets.AllBuffered);			
		}
		LocLabelReStart.Keys.Clear();
		LocLabelReStart.Keys.Add("messages040");
//		LabelReStart.text = "正在等待其他玩家。";
	}
}

@RPC
function redReStart(){
	reStartRed = true;
}

@RPC
function blueReStart(){
	reStartBlue = true;
}

var LabelLiKai : UILabel;
function DuiFangLiKai(){
	LocLabelLiKai.Keys.Clear();
	LocLabelLiKai.Keys.Add("messages041");
//	LabelLiKai.text = "对方已离开。";
}

function show0(){
	cardCLobj.SetActiveRecursively(false);
	AllManage.UIALLPCStatic.show0();
}

function ColseZhanChangUI(){
	BackGrounds[2].transform.localPosition.y = 3000;
	cardCLobj.SetActiveRecursively(false);
	show0();
}

var GroundShowZhanChangPlayer : Transform;
function ShowZhanChangUI(){
	GroundShowZhanChangPlayer.localPosition.y = 0;
	BackGrounds[2].transform.localPosition.y = -50;
	BackGrounds[2].SetActiveRecursively(true);
	cardCLobj.SetActiveRecursively(true);
}

var flags : TriggerStone[];
function ReturnBattlefieldScoreInfo(objs : Object[]){
	if(! battleobj.active){
		battleobj.SetActiveRecursively(true);
	}
	var redScore : int = 0;
	var blueScore : int = 0;
	redScore = objs[0];
	blueScore = objs[1];
	
	if(flags == null || flags.length < 5){
		flags = FindObjectsOfType(TriggerStone);
	}
//	print(redScore + " == " + blueScore);
	
	LabelBattlefieldPoint.text = AllManage.AllMge.Loc.Get("messages046")  + redScore + "\n" + AllManage.AllMge.Loc.Get("messages047") + blueScore;
	LabelBattlefieldPoint1.text = AllManage.AllMge.Loc.Get("messages046") + redScore + "\n" + AllManage.AllMge.Loc.Get("messages047") + blueScore;
	
	var blueFlagList : System.Collections.Generic.List.<String>;
	var redFlagList : System.Collections.Generic.List.<String>;
	redFlagList = objs[2];
	blueFlagList = objs[3];
	var i : int = 0;
	var m : int =0 ;
	for(m=0; m<flags.length; m++){
		if(redFlagList != null){
			for(i=0; i<redFlagList.Count; i++){
				if(redFlagList[i] == flags[m].flagID){
					flags[m].SetFlagAsID("Red");
				}
			}
		}
		
		if(blueFlagList != null){
			for(i=0; i<blueFlagList.Count; i++){
				if(blueFlagList[i] == flags[m].flagID){
					flags[m].SetFlagAsID("Blue");
				}
			}
		}
	}
}

//同步boss血量//
function	ReturnPVPBossHP(hps : int[])
{
	var battleItems : BattlefieldCityItem[];
	battleItems = FindObjectsOfType(BattlefieldCityItem);
	for(var i=0; i<battleItems.length ; i++)
	{
		if(	battleItems[i].myTeam	==	"Red"	)
		{
			battleItems[i].SetServerBossHP(hps[0]);
		}
		else
		if(	battleItems[i].myTeam	==	"Blue"	)
		{
			battleItems[i].SetServerBossHP(hps[1]);
		}
	}
}

//查看战绩
function ShowRecords()
{
    AllManage.UIALLPCStatic.show122();
}

//PhotonP.allProperties["PlayerID"]
//}