#pragma strict

function Start () {

}

function Update () {

}

function OnTriggerEnter(other : Collider){
	if(other.tag == "Player"){
		other.SendMessage("DaoDa" , SendMessageOptions.DontRequireReceiver);
	}
}
