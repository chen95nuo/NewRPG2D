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
			Player = Instantiate(PObj[1], trans.position, trans.rotation);
			break;
		case "2":
//			PhotonNetwork.Instantiate(PObj[2].name, transform.position, Quaternion.identity, 0);
			Player = Instantiate(PObj[2], trans.position, trans.rotation);
			break;
		case "3":
//			PhotonNetwork.Instantiate(PObj[3].name, transform.position, Quaternion.identity, 0);
			Player = Instantiate(PObj[3], trans.position, trans.rotation);
			break;
	}
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
	if(useInvID[i].Length < 1){
		return;
	}
	for(i=0; i<12; i++){	 
		if(useInvID[i] != ""){ 
			useInv = AllResources.InvmakerStatic.GetItem(useInvID[i] , useInv);
			TPWeapon.ShowWeapon(useInv , 0);
		}
	}
}

function PlayerClear(){
	Destroy(Player);
}

//31011436241000003000000;;;;;;;;;;;;
//21011434210000003000000;;;;;;;;;;;;
//11011433241000003000000;;;;;;;;;;;;
