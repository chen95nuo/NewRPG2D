//var not : boolean = false;
var UIPobj : GameObject;
var beng : boolean = false;
var canMove : boolean = false;
var Combo : ComboPoints;
////////////////////////////////////////////////////////////////
 var hitt=0.0;
 var hitn=0.0;
private var photonView : PhotonView;

function Awake(){
	photonView = GetComponent(PhotonView);
if (!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return;
	Combo = FindObjectOfType(ComboPoints);
	Combo.hits = this;
	hitt=Time.time-2;
}
var customSkin : GUISkin;
var target : Transform;
function Start(){
if (!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return;
	Combo.target = target.gameObject;
}
function Update () {
if (!PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
    return;	 
	if(Time.time-hitt<2 && hitn>1){
		if(beng){
//			//print("lian ji le");
			canMove = true;
			beng = false;
			Combo.GetCombo(hitn);
		}
	}

	if(Time.time-hitt>2){
		if(canMove){
//			//print("tingzhigongji");
			canMove = false;
			Combo.MoveCombo(hitn*10);
		}
		hitn=0;
	}
}

function Hit(){
	beng = true;
//	//print("da dao le");
	hitt = Time.time + 2;
	hitn += 1;
}
