#pragma strict
private var photonView : PhotonView;
function Start(){
	photonView = GetComponent(PhotonView);
}

var mess1 : String;
function SendTeamMessage1(){
	if(!photonView){
		photonView = GetComponent(PhotonView);
	}
	photonView.RPC("RPCSendTeamMessage",PhotonTargets.All , InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText , AllManage.AllMge.Loc.Get(mess1) );
}
var mess2 : String;
function SendTeamMessage2(){
	if(!photonView){
		photonView = GetComponent(PhotonView);
	}
	photonView.RPC("RPCSendTeamMessage",PhotonTargets.All , InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText ,  AllManage.AllMge.Loc.Get(mess2));
}
var mess3 : String;
function SendTeamMessage3(){
	if(!photonView){
		photonView = GetComponent(PhotonView);
	}
	photonView.RPC("RPCSendTeamMessage",PhotonTargets.All , InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText ,  AllManage.AllMge.Loc.Get(mess3));
}
var mess4 : String;
function SendTeamMessage4(){
	if(!photonView){
		photonView = GetComponent(PhotonView);
	}
	photonView.RPC("RPCSendTeamMessage",PhotonTargets.All , InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText ,  AllManage.AllMge.Loc.Get(mess4));
}
var mess5 : String;
function SendTeamMessage5(){
	if(!photonView){
		photonView = GetComponent(PhotonView);
	}
	photonView.RPC("RPCSendTeamMessage",PhotonTargets.All ,  InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText , AllManage.AllMge.Loc.Get(mess5));
}
var mess6 : String;
function SendTeamMessage6(){
	if(!photonView){
		photonView = GetComponent(PhotonView);
	}
	photonView.RPC("RPCSendTeamMessage",PhotonTargets.All , InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText , AllManage.AllMge.Loc.Get(mess6));
}
var charB : GameObject;
@RPC
function RPCSendTeamMessage(playername : String , str : String){
//	print(playername + " == "  + str);
	AllManage.UICLStatic.SelectPlayerSendMessage(playername , str);
	charB.SendMessage("AddText" , playername + ":" + str , SendMessageOptions.DontRequireReceiver);
}

