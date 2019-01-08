#pragma strict

var Vipcl : VipControl;
function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("VipCube")){
		Vipcl.TriggerOnAsID(other.name);
	}
}