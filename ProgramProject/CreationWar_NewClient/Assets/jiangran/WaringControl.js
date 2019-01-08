#pragma strict

function Start(){
//	yield;
	transform.localScale = oldScale;
}

var oldScale : Vector3;
function CloseNow(){
	oldScale = transform.localScale;
	transform.localScale = Vector3.zero;
}
