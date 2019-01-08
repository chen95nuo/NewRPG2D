#pragma strict

var playerInventory : PersonEquipment[];

var TransHelmet : Transform;
var Spaulders : Transform[];
var Gauntlets : Transform[];
var Leggings : Transform[];
var Rear : Transform;
var Belt : Transform;
var Weapon1 : Transform[];
var Weapon2 : Transform[];
var MainBodyRender : Renderer;
var invcl : InventoryControl;
//var invMaker :  Inventorymaker;
private var photonView : PhotonView;
var otherPC : OtherPlayerControl;

//function Awake () {			
//	invMaker = FindObjectOfType(Inventorymaker);
//}

function Start(){
}

//var 
var RemoteName : String;
var RemoteProID : String;
var RemoteLevel : String;
var RemoteID : String;
//var RemoteProfession : String;

var EquIt : BagItem[];
var weapons : Transform[];
function ShowWeapon(inv : InventoryItem , equepmentID : int){
	RPCShowWeapon(inv.itemID, equepmentID); 
//	photonView.RPC("RPCShowWeapon",inv,); 
//	RPCShowWeapon(inv , equepmentID);
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

function RPCShowWeapon(invID : String , equepmentID : int){
	var inv : InventoryItem;
//	if(!invMaker)
//		invMaker = FindObjectOfType(Inventorymaker);
		
	inv = AllResources.InvmakerStatic.GetItem(invID , inv);
	if(inv.itemmodle1 == null && inv.itemmodle2 == null  && inv.slotType != SlotType.Breastplate){
		return;
	}
	for(var i=0; i <playerInventory.length; i++){
//		//print(gameObject.name + " == " + playerInventory[i].invType + " == " + inv.slotType + " == " + i + " == " + equepmentID);
		if(i == equepmentID && playerInventory[i].invType == inv.slotType){
			playerInventory[i].inv = inv;
		}
	}
//	//print(inv.slotType + " == inv.slotType");
	switch(inv.slotType){
		case SlotType.Helmet :
			nowItemHelmet = ObjClose(nowItemHelmet);
			inv.itemmodle1.transform.parent = TransHelmet;
			nowItemHelmet = inv.itemmodle1;
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
//			//print("dao zhe li le");
			MainBodyRender.material.mainTexture = inv.itemtexture;
			break;
		case SlotType.Weapon1 :
			if(inv.itemmodle1 != null){			
				nowItemWeapon1 = ObjClose(nowItemWeapon1);
				inv.itemmodle1.transform.parent = Weapon1[0];
				weapons[0] = inv.itemmodle1.transform;
				nowItemWeapon1 = inv.itemmodle1;
			}
			if(inv.itemmodle2 != null){
				nowItemWeapon2 = ObjClose(nowItemWeapon2);
				inv.itemmodle2.transform.parent = Weapon1[1];
				weapons[1] = inv.itemmodle2.transform;
				nowItemWeapon2 = inv.itemmodle2;
			}
			if(gameObject.name.Substring(0,3) == "You"){ 
				if( inv.WeaponType == PlayerWeaponType.weapon2){
//					isGong = true;
					inv.itemmodle1.transform.parent = Weapon1[2]; 
					weapons[0]  = inv.itemmodle1.transform;			
				}else{
//					isGong = false;
					inv.itemmodle1.transform.parent = Weapon1[0];
					weapons[0] = inv.itemmodle1.transform;
					nowItemWeapon1 = inv.itemmodle1;
				}
			}
			break;
	}
	
	if(inv.itemmodle1 != null){
		inv.itemmodle1.transform.localPosition = Vector3(0,0,0);	
		inv.itemmodle1.transform.localScale = Vector3(1,1,1);		
		inv.itemmodle1.transform.localRotation = Quaternion.identity;
		inv.itemmodle1.SetActiveRecursively(true);
	}
	if(inv.itemmodle2 != null){
		inv.itemmodle2.transform.localPosition = Vector3(0,0,0);	
		inv.itemmodle2.transform.localScale = Vector3(1,1,1);	
		inv.itemmodle2.transform.localRotation = Quaternion.identity;
		inv.itemmodle2.SetActiveRecursively(true);
	}
}

function ObjClose(obj : GameObject) : GameObject{
	if(obj != null){
//				obj.GetComponent(CloneMesh).functionID = 2;
//				obj.active = true;
		Destroy(obj.gameObject);
//		obj.transform.parent = null;
//		obj.SetActiveRecursively(false);
		return null;
	}
	return null;
}

var textue : Texture;
function ObjCloseAsType(){
//	switch(sType){
//		case SlotType.Helmet : 
			nowItemHelmet = ObjClose(nowItemHelmet);
//			break;
//		case SlotType.Spaulders :
			nowItemSpaulders1 = ObjClose(nowItemSpaulders1);
			nowItemSpaulders2 = ObjClose(nowItemSpaulders2);
//			break;
//		case SlotType.Gauntlets :
			nowItemGauntlets1 = ObjClose(nowItemGauntlets1);
			nowItemGauntlets2 = ObjClose(nowItemGauntlets2);
//			break;
//		case SlotType.Leggings :
			nowItemLeggings1 = ObjClose(nowItemLeggings1);
			nowItemLeggings2 = ObjClose(nowItemLeggings2);
//			break;
//		case SlotType.Rear :
			nowItemRear = ObjClose(nowItemRear);
//			break;
//		case SlotType.Breastplate :
			MainBodyRender.material.mainTexture = textue;
//			break;
//		case SlotType.Weapon1 :
			nowItemWeapon1 = ObjClose(nowItemWeapon1);
			nowItemWeapon2 = ObjClose(nowItemWeapon2);
//			break;
//	}
}

var MainTexture : Texture;
function ShowMainTexture(){
	MainBodyRender.material.mainTexture = MainTexture;
}

private var stime : float;
function Update(){
//	if(Time.time > stime){
//		stime = Time.time + 0.3;
//		OnFight(playerS.battlemod);
//	}
}

var playerS : PlayerStatus;
private var useWeapon : Transform[];
function OnFight(bool : boolean){
	useWeapon = new Array(2);
	if(bool){
		useWeapon[0] = Weapon2[0];
		useWeapon[1] = Weapon2[1];
	}else{
		useWeapon[0] = Weapon1[0];
		useWeapon[1] = Weapon1[1];	
	}
	if(weapons[0]){
		weapons[0].parent = useWeapon[0];
		weapons[0].localPosition = Vector3.zero;
		weapons[0].localRotation = Quaternion.identity;
	}
	if(weapons[1]){
		weapons[1].parent = useWeapon[1];
		weapons[1].localPosition = Vector3.zero;
		weapons[1].localRotation = Quaternion.identity;
	}
}

function OnDoubleClick(){
	otherPC.SetNewPlayer(playerInventory);
	// otherPC.ShowRemotePlayer(RemoteName , RemoteLevel , RemoteProID , RemoteID);   
}
