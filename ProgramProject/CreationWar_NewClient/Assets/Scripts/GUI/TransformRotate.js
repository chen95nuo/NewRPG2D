#pragma strict

var speed : float = 0;
function Update () {
	transform.localEulerAngles.z += speed*Time.deltaTime;
}

function OnEnable(){
	transform.localEulerAngles = Vector3.zero;
}