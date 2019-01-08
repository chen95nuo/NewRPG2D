#pragma strict

var message : String;
function OnTriggerEnter(Other : Collider){
	if(Other.tag == "GUICube"){
		Other.SendMessage("TriggerCube" , message , SendMessageOptions.DontRequireReceiver);
	}
}
