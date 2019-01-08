#pragma strict
private var gan : Joystick;

var Done : int = 0;
var level : String;
function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("Player")){
		PlayerPrefs.SetInt("XunLianDone"  + other.GetComponent(PlayerStatus).PlayerID, 1);
		Loading.Level = "Map111";
	#if UNITY_IPHONE || UNITY_ANDROID
	   gan = FindObjectOfType(Joystick);
	   if(gan)
	   gan.ResetJoystick();
    #endif
		var invcl : InventoryControl;
		invcl = FindObjectOfType(InventoryControl);
		invcl.RetrunYT();
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	alljoy.DontJump = true;
	yield;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}
}
	public function desP(){
		while(PhotonNetwork.isMasterClient){
			yield;
		}
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
	}
