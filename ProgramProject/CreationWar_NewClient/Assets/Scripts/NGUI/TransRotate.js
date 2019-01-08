#pragma strict

var rotateSpeed : float = 0;
var rt : float;
function Update () {
	transform.Rotate(Vector3(0,0,rt) , rotateSpeed*Time.deltaTime);
}
