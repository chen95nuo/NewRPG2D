#pragma strict

private var photonView : PhotonView;
function Awake(){
	photonView = GetComponent(PhotonView); 
}
function Start () {

}

function Update () {

}

function SetGroup( id : int){
	photonView.group = id;
}