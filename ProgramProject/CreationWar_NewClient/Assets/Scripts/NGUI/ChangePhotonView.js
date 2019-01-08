#pragma strict
private var photonView : PhotonView;
var comp : Component;
var ViewID : int;
function Awake(){
	photonView = gameObject.AddComponent(PhotonView);
	photonView.observed = comp;
	photonView.viewID = ViewID;
}

