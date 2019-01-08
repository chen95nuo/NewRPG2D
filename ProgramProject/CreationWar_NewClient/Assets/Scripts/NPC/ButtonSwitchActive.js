#pragma strict

var type : yuan.YuanPhoton.BenefitsType;
function Start () {
	if(InRoom.GetInRoomInstantiate().GetServerSwitchString(type) == "0"){
		gameObject.SetActiveRecursively(false);
	}
}

function OnEnable(){
//	//print(InRoom.GetInRoomInstantiate().GetServerSwitchString(type) + " ============ " + gameObject);
	if(InRoom.GetInRoomInstantiate().GetServerSwitchString(type) == "0"){
		gameObject.SetActiveRecursively(false);
	}
}
