#pragma strict

function Start () {
	yield;
	yield;
	yield;
	yield;
	yield;
	if(target != null && str != ""){
		target.SendMessage(str , SendMessageOptions.DontRequireReceiver);
	}

}

function Update () {

}

var target : GameObject;
var str : String;
function OnEnable(){
	if(target != null && str != ""){
		target.SendMessage(str , SendMessageOptions.DontRequireReceiver);
	}
}
