#pragma strict

function Start () {

}

var ptime : int = 0;
function Update () {
	if(Time.time > ptime && !open){
		ptime = Time.time + 2;
		if(LookMSPIsClear()){
			OpenDoor();
		}
	}
}

var open : boolean = false;
var msp : MonsterSpawn[];
function LookMSPIsClear() : boolean{
	for(var i=0; i<msp.length; i++){
		if(!msp[i].IsCleared()){
			return false;
		}
	}
	return true;
}


var moveSpeed : float = 0;
function OpenDoor(){
	open = true;
	while(transform.position.y < 37){
		transform.position.y += moveSpeed*Time.deltaTime;
		yield;
	}
}
