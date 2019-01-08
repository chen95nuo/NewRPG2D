#pragma strict
var Soulobject : Transform[];
private var photonView : PhotonView;
private var SoulCon : InventoryControl;
function Start () {
photonView = GetComponent(PhotonView);

if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
    SoulCon = FindObjectOfType(InventoryControl);
    SoulCon.peson = this.transform;
 }
}

var cc : GameObject;
function CallObjectSoul(i:int){
   if(cc){
		PhotonNetwork.Destroy(cc);     	
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_ReMoveSoul, "");
   }
    cc = PhotonNetwork.Instantiate(this.Soulobject[i-1].name, transform.position,transform.rotation,0);
    ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_CallObjectSoul, this.Soulobject[i-1].name);
    var SAI : SoulPetAI = cc.GetComponent(SoulPetAI); 
       SAI.Settargetm(this.transform); 
       SAI.rs = SoulCon.rs ; 
     var selfPet = new ExitGames.Client.Photon.Hashtable();
         selfPet.Add("Pet",i);    
     PhotonNetwork.SetPlayerCustomProperties(selfPet);    
 }
 
function reMoveSoul(){
	if(cc){   
		PhotonNetwork.Destroy(cc); 
		ServerRequest.syncAct(GetComponent(PlayerStatus).instanceID , CommonDefine.ACT_ReMoveSoul, "");
	}
}

function ReturnreMoveSoul(){
	if(cc)
		PhotonNetwork.Destroy(cc); 
}

function ReturnCallObjectSoul(name : String){
	 cc = PhotonNetwork.Instantiate(name , transform.position , transform.rotation , 0);
	 cc.SendMessage("Settargetm" , this.transform , SendMessageOptions.DontRequireReceiver);
	 SendMessage("AddMyPet" , cc , SendMessageOptions.DontRequireReceiver);
}

function InitCallObjectSoul(i : int){
	cc = PhotonNetwork.Instantiate(this.Soulobject[i-1].name, transform.position,transform.rotation,0);
	cc.SendMessage("Settargetm" , this.transform , SendMessageOptions.DontRequireReceiver);
	SendMessage("AddMyPet" , cc , SendMessageOptions.DontRequireReceiver);
}

function RPCPlayEffect(str : String){
	if(cc){
		cc.SendMessage("PlayEffect" , parseInt(str) , SendMessageOptions.DontRequireReceiver);
	}
}

function RPCSyncAnimation(str : String){
	if(cc){
		cc.SendMessage("SyncAnimation" , str , SendMessageOptions.DontRequireReceiver);
	}
}

function RPCSoulShoot(strs : String[]){
	if(cc){
		cc.SendMessage("RPCShoot" , strs , SendMessageOptions.DontRequireReceiver);
	}
}

function RPCSoulSkill(strs : String[]){
	if(cc){
		cc.SendMessage("RPCCallObject" , strs , SendMessageOptions.DontRequireReceiver);
	}
}

