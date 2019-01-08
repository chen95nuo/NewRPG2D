#pragma strict

function Start () {

}

function Update () {

}

var Player : GameObject;
var PObj : GameObject[];
var equStr : String;
var trans : Transform;
function SelectOnePlayer(yt : yuan.YuanMemoryDB.YuanRow){
	PlayerClear();
	switch(yt["ProID"].YuanColumnText){
		case "1":
//			PhotonNetwork.Instantiate(PObj[1].name, transform.position, Quaternion.identity, 0);
//			Player = Instantiate(PObj[1], trans.position, trans.rotation);
			Player = PObj[1];
			break;
		case "2":
//			PhotonNetwork.Instantiate(PObj[2].name, transform.position, Quaternion.identity, 0);
//			Player = Instantiate(PObj[2], trans.position, trans.rotation);
			Player = PObj[2];
			break;
		case "3":
//			PhotonNetwork.Instantiate(PObj[3].name, transform.position, Quaternion.identity, 0);
//			Player = Instantiate(PObj[3], trans.position, trans.rotation);
			Player = PObj[3];
			break;
	}
	Player.transform.position = trans.position;
	Player.transform.rotation = trans.rotation; 
	Player.SetActiveRecursively(true);
	Player.transform.parent=trans;
	TPWeapon = Player.GetComponent(ThirdPersonWeapon1);
//	Player.GetComponent(PlayerStatus).enabled = false;
//	Player.GetComponent(MainPersonStatus).enabled = false;
//	Player.GetComponent(PassiveSkill).enabled = false;
	yield;
//	//print(yt["EquipItem"].YuanColumnText);
	equStr = yt["EquipItem"].YuanColumnText;
	SetEquipItem(equStr);
}

private var Fstr : String = ";";
//var invMaker : Inventorymaker;
var TPWeapon : ThirdPersonWeapon1;
function SetEquipItem(equStr : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = equStr.Split(Fstr.ToCharArray());
	if(useInvID.length < 2){
		return;
	}
	for(i=0; i<12; i++){	 
		if(useInvID[i] != ""){ 
			useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
			TPWeapon.ShowWeapon(useInv , 0);
		}
	}
}

function PlayerClear(){
	 PObj[1].transform.position = Vector3.zero; 
	 PObj[1].SendMessage("ObjCloseAsType" , SendMessageOptions.DontRequireReceiver); 
	 PObj[1].gameObject.SetActiveRecursively(false);
	 PObj[2].transform.position = Vector3.zero; 
	 PObj[2].SendMessage("ObjCloseAsType" , SendMessageOptions.DontRequireReceiver); 
	 PObj[2].gameObject.SetActiveRecursively(false);
	 PObj[3].transform.position = Vector3.zero; 
	 PObj[3].SendMessage("ObjCloseAsType" , SendMessageOptions.DontRequireReceiver); 
	 PObj[3].gameObject.SetActiveRecursively(false);
}

//31011436241000003000000;;;;;;;;;;;;
//21011434210000003000000;;;;;;;;;;;;
//11011433241000003000000;;;;;;;;;;;;
