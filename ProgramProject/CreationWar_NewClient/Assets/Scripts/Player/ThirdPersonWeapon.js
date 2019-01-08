#pragma strict
//class ThirdPersonWeapon extends XmlControl{

var playerInventory : PersonEquipment[];

var TransHelmet : Transform;
var Spaulders : Transform[];
var Gauntlets : Transform[];
var Leggings : Transform[];
var Rear : Transform;
var Belt : Transform;
var Weapon1 : Transform[];
var Weapon2 : Transform[]; 
var tou : GameObject;
var MainBodyRender : Renderer;
var invcl : InventoryControl;
//var invMaker :  Inventorymaker;
private var photonView : PhotonView;
var otherPC : OtherPlayerControl;
function Awake () {		
	photonView = GetComponent(PhotonView);
	if (!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return;			
//	invMaker = FindObjectOfType(Inventorymaker);
	invcl = FindObjectOfType(InventoryControl);	

}

function Start(){
	if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
		try{
		invcl.TPWeapon = this;
		}catch(e){
		
		}
	}
}


var EquIt : BagItem[];
var weapons : Transform[];
var Antherweapons : Transform[];
function ShowWeapon(inv : InventoryItem , equepmentID : int){
//	//print(equepmentID + " === " + inv);
	if(inv != null){
		RPCShowWeaponFirst(inv.itemID , equepmentID , GetBDInfoInt("HideHelmet",0) );
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_RPCShowWeaponFirst, String.Format("{0};{1};{2}", inv.itemID , equepmentID , GetBDInfoInt("HideHelmet",0)));
//		photonView.RPC("RPCShowWeaponFirst",PhotonTargets.AllBuffered,inv.itemID,equepmentID , GetBDInfoInt("HideHelmet",0));  	
	}else{
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_RPCRemoveWeapon, equepmentID.ToString());
		RPCRemoveWeapon(equepmentID);
//		photonView.RPC("RPCRemoveWeapon",PhotonTargets.AllBuffered,equepmentID);  	
	}
}

var nowItemHelmet : GameObject;
var nowItemSpaulders1 : GameObject;
var nowItemSpaulders2 : GameObject;
var nowItemGauntlets1 : GameObject;
var nowItemGauntlets2 : GameObject;
var nowItemLeggings1 : GameObject;
var nowItemLeggings2 : GameObject;
var nowItemRear : GameObject;
var nowItemWeapon1 : GameObject;
var nowItemWeapon2 : GameObject;
var BeiItemWeapon1 : GameObject;
var BeiItemWeapon2 : GameObject;
 function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}

@RPC
function RPCRemoveWeapon(equepmentID : int){
	for(var i=0; i <playerInventory.length; i++){
		if(i == equepmentID){
			playerInventory[i].inv = null;
		}
	}	
}
function ReturnRPCShowWeaponFirst(strs : String[]){
	RPCShowWeaponFirst(strs[0] , parseInt(strs[1]) , parseInt(strs[2]));
}

@RPC
function RPCShowWeaponFirst(invID : String , equepmentID : int , touInt : int){
//	//print("invID == " + invID + " - " + equepmentID);
	RPCShowWeapon(invID , equepmentID , touInt);
} 

var useBool : boolean = false;
var isGong : boolean = false;
function RPCShowWeapon(invID : String , equepmentID : int , touInt : int){ 
yield WaitForSeconds(0.5);
//	//print("invID == " + invID + " - " + equepmentID);
	var inv : InventoryItem;
//	while(!invMaker){
//		invMaker = FindObjectOfType(Inventorymaker);
//		yield;
//		}
		
	inv = AllResources.InvmakerStatic.GetItem(invID , inv);
	for(var i=0; i <playerInventory.length; i++){
		if(i == equepmentID && playerInventory[i].invType == inv.slotType){
			playerInventory[i].inv = inv;
		}
	}
	if(inv.itemmodle1 == null && inv.itemmodle2 == null &&inv.slotType != SlotType.Breastplate){
		return;
	}
	switch(inv.slotType){
		case SlotType.Helmet :
			nowItemHelmet = ObjClose(nowItemHelmet);
			inv.itemmodle1.transform.parent = TransHelmet;
			nowItemHelmet = inv.itemmodle1; 
			if(tou != null){	 
				useBool = AllResources.InvmakerStatic.LookTou(nowItemHelmet.name);
			}
			break;
		case SlotType.Spaulders :
			nowItemSpaulders1 = ObjClose(nowItemSpaulders1);
			nowItemSpaulders2 = ObjClose(nowItemSpaulders2);
			inv.itemmodle1.transform.parent = Spaulders[0];
			inv.itemmodle2.transform.parent = Spaulders[1];
			nowItemSpaulders1 = inv.itemmodle1;
			nowItemSpaulders2 = inv.itemmodle2;
			break;
		case SlotType.Gauntlets :
			nowItemGauntlets1 = ObjClose(nowItemGauntlets1);
			nowItemGauntlets2 = ObjClose(nowItemGauntlets2);
			inv.itemmodle1.transform.parent = Gauntlets[0];
			inv.itemmodle2.transform.parent = Gauntlets[1];
			nowItemGauntlets1 = inv.itemmodle1;
			nowItemGauntlets2 = inv.itemmodle2;
			break;
		case SlotType.Leggings :
			nowItemLeggings1 = ObjClose(nowItemLeggings1);
			nowItemLeggings2 = ObjClose(nowItemLeggings2);
			inv.itemmodle1.transform.parent = Leggings[0];
			inv.itemmodle2.transform.parent = Leggings[1];
			nowItemLeggings1 = inv.itemmodle1;
			nowItemLeggings2 = inv.itemmodle2;
			break;
		case SlotType.Rear :
			nowItemRear = ObjClose(nowItemRear);
			inv.itemmodle1.transform.parent = Rear;
			nowItemRear = inv.itemmodle1;
			break;
		case SlotType.Breastplate :
			MainBodyRender.material.mainTexture = inv.itemtexture;
			break;
		case SlotType.Weapon1 :
			if(equepmentID == 10){
					nowItemWeapon1 = ObjClose(nowItemWeapon1);
					nowItemWeapon2 = ObjClose(nowItemWeapon2); 
				if(inv.itemmodle1 != null){			
					inv.itemmodle1.transform.parent = Weapon1[0];
					weapons[0] = inv.itemmodle1.transform;
					nowItemWeapon1 = inv.itemmodle1;
				}
				if(inv.itemmodle2 != null){
					inv.itemmodle2.transform.parent = Weapon1[1];
					weapons[1] = inv.itemmodle2.transform;
					nowItemWeapon2 = inv.itemmodle2;
				}
				if(playerS.ProID == 2){ 
					if( inv.WeaponType == PlayerWeaponType.weapon2){
						isGong = true;
						inv.itemmodle1.transform.parent = Weapon1[2]; 
						weapons[0]  = inv.itemmodle1.transform;			
					}else{
						isGong = false;
						inv.itemmodle1.transform.parent = Weapon1[0];
						weapons[0] = inv.itemmodle1.transform;
						nowItemWeapon1 = inv.itemmodle1;
					}
				}
			}else
			if(equepmentID == 11){
//				//print(equepmentID);
//					BeiItemWeapon1 = ObjClose(BeiItemWeapon1);
//					BeiItemWeapon2 = ObjClose(BeiItemWeapon2); 
				if(inv.itemmodle1 != null){	
					inv.itemmodle1.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle1.SetActiveRecursively(false);
					inv.itemmodle1.transform.parent = Weapon1[0];
					Antherweapons[0] = inv.itemmodle1.transform;
					BeiItemWeapon1 = inv.itemmodle1;
				}
				if(inv.itemmodle2 != null){
					inv.itemmodle2.GetComponent(CloneMesh).functionID = 2;
					inv.itemmodle2.SetActiveRecursively(false);
					inv.itemmodle2.transform.parent = Weapon1[1];
					Antherweapons[1] = inv.itemmodle2.transform;
					BeiItemWeapon2 = inv.itemmodle2;
				}
//				if(BeiItemWeapon1){
//					BeiItemWeapon1.SetActiveRecursively(false); 
//				}
//				if(BeiItemWeapon2){
//					BeiItemWeapon2.SetActiveRecursively(false);
//				}
				
				if(playerS.ProID == 2){ 
					if( inv.WeaponType == PlayerWeaponType.weapon2){
						isGong = true;
						inv.itemmodle1.transform.parent = Weapon1[2]; 
						Antherweapons[0]  = inv.itemmodle1.transform;			
					}else{
						isGong = false;
						inv.itemmodle1.transform.parent = Weapon1[0];
						Antherweapons[0] = inv.itemmodle1.transform;
						BeiItemWeapon1 = inv.itemmodle1;
					}
				}
			}
			break;
	}
	
	if(inv.itemmodle1 != null){
		inv.itemmodle1.transform.localPosition = Vector3(0,0,0);	
		inv.itemmodle1.transform.localScale = Vector3(1,1,1);		
		inv.itemmodle1.transform.localRotation = Quaternion.identity;
		inv.itemmodle1.layer = this.gameObject.layer;
		if(equepmentID != 11){
			inv.itemmodle1.SetActiveRecursively(true);  
		}
	}
	if(inv.itemmodle2 != null){
		inv.itemmodle2.transform.localPosition = Vector3(0,0,0);	
		inv.itemmodle2.transform.localScale = Vector3(1,1,1);	
		inv.itemmodle2.transform.localRotation = Quaternion.identity;
		inv.itemmodle2.layer = this.gameObject.layer;
		if(equepmentID != 11){
			inv.itemmodle2.SetActiveRecursively(true); 
		}
	}
if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
		lookTou(touInt);
	if(!ps){
		ps = GetComponent(PlayerStatus);
	}
	if(ps.oob){
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_PlayOnFight, "0");
	}else{
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_PlayOnFight, "1");
	}
	OnFight(ps.oob);
//	photonView.RPC("OnFight",ps.oob);

}

if(Application.loadedLevelName == "Login-2"){
	lookTou(0);
}

}

private var ps : PlayerStatus;
function CloneInvItem(from : GameObject , to : GameObject){
	
}

function UsingSkillAsID(i : int , bool : boolean){
if(playerS.ProID != 3){
	if(i >= 3 && i<=8){
		if(playerInventory[10].inv.WeaponType != 1){
			HuanFuWeapon(bool);
		}
	}else if(i >= 9 && i<=14){
		if(playerInventory[10].inv.WeaponType != 2){
			HuanFuWeapon(bool);		
		}	
	}
	}else {
	if(i >= 3 && i<=8){
		if(playerInventory[10].inv.WeaponType != 2){
			HuanFuWeapon(bool);
		}
	}else if(i >= 9 && i<=14){
		if(playerInventory[10].inv.WeaponType != 1){
			HuanFuWeapon(bool);		
		}	
	}	
	
	
	}

}

function HuanFuWeapon(bool : boolean){
		yield;
		yield;
		yield;
		yield;
		yield;
		yield;
		yield;
		yield;
		yield;
		yield;
	try{
	if(bool){
		if(nowItemWeapon1){
			nowItemWeapon1.GetComponent(CloneMesh).functionID = 3;
			nowItemWeapon1.SetActiveRecursively(true);
		}
		if(nowItemWeapon2){
			nowItemWeapon2.GetComponent(CloneMesh).functionID = 3;
			nowItemWeapon2.SetActiveRecursively(true);
		}
		if(BeiItemWeapon1){
			BeiItemWeapon1.GetComponent(CloneMesh).functionID = 1;
			BeiItemWeapon1.SetActiveRecursively(true);
		}
		if(BeiItemWeapon2){
			BeiItemWeapon2.GetComponent(CloneMesh).functionID = 1;
			BeiItemWeapon2.SetActiveRecursively(true);
		}
	}else{
		if(BeiItemWeapon1){
			BeiItemWeapon1.GetComponent(CloneMesh).functionID = 3;
			BeiItemWeapon1.SetActiveRecursively(true);
		}
		if(BeiItemWeapon2){
			BeiItemWeapon2.GetComponent(CloneMesh).functionID = 3;
			BeiItemWeapon2.SetActiveRecursively(true);
		}	
		if(nowItemWeapon1){
			nowItemWeapon1.GetComponent(CloneMesh).functionID = 1;
			nowItemWeapon1.SetActiveRecursively(true);
		}
		if(nowItemWeapon2){
			nowItemWeapon2.GetComponent(CloneMesh).functionID = 1;
			nowItemWeapon2.SetActiveRecursively(true);
		}
	}
	}catch(e){
		
	}
}

function lookTou(it : int){
	ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_lookTouRPC, it.ToString());
	lookTouRPC(it);
//	photonView.RPC("lookTouRPC",PhotonTargets.AllBuffered, it);  	
}

@RPC
function lookTouRPC(it : int){
	if(tou != null && nowItemHelmet != null){
		if(it == 0){
			nowItemHelmet.GetComponent(CloneMesh).functionID = 1;
			nowItemHelmet.SetActiveRecursively(true);
			if(useBool){
				tou.SetActiveRecursively(false);
			}else{
				tou.SetActiveRecursively(true);
			}
		}else{ 
			tou.SetActiveRecursively(true);
			nowItemHelmet.GetComponent(CloneMesh).functionID = 3;
			nowItemHelmet.SetActiveRecursively(true);		
		}
	}
//	//print(nowItemHelmet + " == " + tou);
	if(nowItemHelmet == null && tou != null){
		tou.SetActiveRecursively(true);
	}
}

function ObjClose(obj : GameObject) : GameObject{
	if(obj != null){
//		//print("zhe li");
				obj.GetComponent(CloneMesh).functionID = 2;
				obj.active = true;
//		Destroy(obj);
//		obj.transform.parent = null;
//		obj.SetActiveRecursively(false);
		return null;
	}
	return null;
}


function ObjCloseAsTypeFirst(sType : SlotType , EquepmentID : int){
//	//print(sType + " == " + EquepmentID);
	ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , 	CommonDefine.ACT_lookTouRPC, String.Format("{0};{1}", parseInt(sType) , EquepmentID));
	ObjCloseAsType(parseInt(sType) , EquepmentID);
//	photonView.RPC("ObjCloseAsType",PhotonTargets.AllBuffered, parseInt(sType) , EquepmentID); 
}

function ReturnObjCloseAsType(strs : String[]){
	ObjCloseAsType(parseInt(strs[0]) , parseInt(strs[1]));
}

@RPC
function ObjCloseAsType(sType1 : int , EquepmentID : int){
	var sType : SlotType;
	sType = sType1;
//	//print(sType + " == " + EquepmentID);
	switch(sType){
		case SlotType.Helmet : 
			nowItemHelmet = ObjClose(nowItemHelmet);
			break;
		case SlotType.Spaulders :
			nowItemSpaulders1 = ObjClose(nowItemSpaulders1);
			nowItemSpaulders2 = ObjClose(nowItemSpaulders2);
			break;
		case SlotType.Gauntlets :
			nowItemGauntlets1 = ObjClose(nowItemGauntlets1);
			nowItemGauntlets2 = ObjClose(nowItemGauntlets2);
			break;
		case SlotType.Leggings :
			nowItemLeggings1 = ObjClose(nowItemLeggings1);
			nowItemLeggings2 = ObjClose(nowItemLeggings2);
			break;
		case SlotType.Rear :
			nowItemRear = ObjClose(nowItemRear);
			break;
//		case SlotType.Breastplate :
//			MainBodyRender.material.mainTexture = inv.itemtexture;
//			break;
		case SlotType.Weapon1 :
			if(EquepmentID == 10){
				nowItemWeapon1 = ObjClose(nowItemWeapon1);
				nowItemWeapon2 = ObjClose(nowItemWeapon2);
			}else
			if(EquepmentID == 11){
				BeiItemWeapon1 = ObjClose(BeiItemWeapon1);
				BeiItemWeapon2 = ObjClose(BeiItemWeapon2);			
			}
			break;
	}
	lookTou(GetBDInfoInt("HideHelmet",0));
}

var MainTexture : Texture;
function ShowMainTexture(){
	MainBodyRender.material.mainTexture = MainTexture;
}

var playerS : PlayerStatus;
private var useWeapon : Transform[];
function RetrunOnFight(strBool : String){
	if(strBool == "0"){
		OnFight(true);
	}else
	if(strBool == "1"){
		OnFight(false);
	}
}

@RPC
function OnFight(bool : boolean){
	useWeapon = new Array(2);
	if(bool){
		useWeapon[0] = Weapon2[0];
		useWeapon[1] = Weapon2[1];
	}else{
		useWeapon[0] = Weapon1[0];
		useWeapon[1] = Weapon1[1];	
		if(playerS.ProID == 2 && isGong){
			useWeapon[0] = Weapon1[2];
			useWeapon[1] = Weapon1[2];
		}
	}
//			if(playerS.ProID == 2){ 
//				if( inv.WeaponType == PlayerWeaponType.weapon2){
//					inv.itemmodle1.transform.parent = Weapon1[2]; 
//					weapons[0]  = inv.itemmodle1.transform;			
//				}else{
//					inv.itemmodle1.transform.parent = Weapon1[0];
//					weapons[0] = inv.itemmodle1.transform;
//					nowItemWeapon1 = inv.itemmodle1;
//				}
//			}
	if(weapons[0]){
		try{
		weapons[0].GetComponent(CloneMesh).functionID = 3;
		weapons[0].gameObject.active = true;
		}catch(e){
		
		}
		yield;
		yield;
		try{
		weapons[0].parent = useWeapon[0];
		weapons[0].localPosition = Vector3.zero;
		weapons[0].localRotation = Quaternion.identity;
		weapons[0].GetComponent(CloneMesh).functionID = 1;
		weapons[0].gameObject.SetActiveRecursively(true);
		}catch(e){
		
		}
	}
	if(weapons[1]){
		try{
			weapons[1].GetComponent(CloneMesh).functionID = 3;
			weapons[1].gameObject.active = true;
		}catch(e){
		
		}
		yield;
		yield;
		try{
		weapons[1].parent = useWeapon[1];
		weapons[1].localPosition = Vector3.zero;
		weapons[1].localRotation = Quaternion.identity;
		weapons[1].GetComponent(CloneMesh).functionID = 1;
		weapons[1].gameObject.SetActiveRecursively(true);
		}catch(e){
		
		}
	}
	
	if(Antherweapons[0]){
//		Antherweapons[0].GetComponent(CloneMesh).functionID = 3;
//		Antherweapons[0].gameObject.active = true;
		yield;
		yield;
		try{
		Antherweapons[0].parent = useWeapon[0];
		Antherweapons[0].localPosition = Vector3.zero;
		Antherweapons[0].localRotation = Quaternion.identity;
//		Antherweapons[0].GetComponent(CloneMesh).functionID = 1;
//		Antherweapons[0].gameObject.SetActiveRecursively(true);
		}catch(e){
		
		}
	}
	if(Antherweapons[1]){
//		Antherweapons[1].GetComponent(CloneMesh).functionID = 3;
//		Antherweapons[1].gameObject.active = true;
		yield;
		yield;
		try{
		Antherweapons[1].parent = useWeapon[1];
		Antherweapons[1].localPosition = Vector3.zero;
		Antherweapons[1].localRotation = Quaternion.identity;
//		Antherweapons[1].GetComponent(CloneMesh).functionID = 1;
//		Antherweapons[1].gameObject.SetActiveRecursively(true);
		}catch(e){
		
		}
	}
}


function OnClick(){
if(!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && gameObject.CompareTag ("Player") && ( UIControl.mapType == MapType.zhucheng ||  UIControl.mapType == MapType.yewai)){

	AllManage.UIALLPCStatic.showJiaoYi25();
	yield;
	yield;
	yield;

 if(!otherPC){
	 otherPC = FindObjectOfType(OtherPlayerControl);	
 }
 while(!otherPC){
 	yield;
 }
 otherPC.ShowRemotePlayer(playerS.PlayerName ,playerS.Level,playerS.ProID.ToString(), playerS.PlayerID.ToString(), playerS.GuildName,GetComponent(PlayerInfoInit).forceValue.ToString() , GetComponent(PlayerInfoInit).selectTitle, playerS.Maxhealth, playerS.Maxmana ,playerS.PVPRank ,playerS.instanceID , GetComponent(PlayerInfoInit).playerDuelState);
 	otherPC.ClickMe(this);
 }
 if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && PlayerStatus.MainCharacter == this.transform){
 Camera.main.SendMessage("SetTarget", transform);
}
}


function chakanMe(){
	otherPC.SetNewPlayer(playerInventory);
	otherPC.ShowRemotePlayer(playerS.PlayerName ,playerS.Level,playerS.ProID.ToString(), playerS.PlayerID.ToString(), playerS.GuildName,GetComponent(PlayerInfoInit).forceValue.ToString(),GetComponent(PlayerInfoInit).selectTitle,playerS.Maxhealth, playerS.Maxmana ,playerS.PVPRank ,playerS.instanceID , GetComponent(PlayerInfoInit).playerDuelState);
	otherPC.ZHaoXiang(gameObject);
}

function OnDoubleClick(){
if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && PlayerStatus.MainCharacter == this.transform){
 Camera.main.SendMessage("SetTarget", transform);
}
}

function OnDestroy(){
	if(PlayerStatus.MainCharacter != this.transform){
		var arena : ArenaControl;
		arena = FindObjectOfType(ArenaControl);
		if(arena){
			arena.DuiFangLiKai();
		}
	}
}
