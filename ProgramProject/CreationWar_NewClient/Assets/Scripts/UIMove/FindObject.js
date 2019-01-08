#pragma strict
var obj : GameObject;

var TransNotActive : Transform[];
var ColderAll : Collider[];
function Awake () {
	var trans : Transform[];
	var i : int = 0;
	trans = FindObjectsOfType(Transform);
	for(i=0; i<trans.length; i++){
		for(var ts : Transform in trans[i]){
			if(! ts.gameObject.active){
				addTrans(ts);
			}
		}
	}
	for(i=0; i<TransNotActive.length; i++){
		TransNotActive[i].gameObject.SetActiveRecursively(true);
	}
	ColderAll = FindObjectsOfType(Collider);
	for(i=0; i<ColderAll.length; i++){
		ColderAll[i].gameObject.AddComponent("ColliderPosition");
	}
	
	var allCP : ColliderPosition[];
	allCP = FindObjectsOfType(ColliderPosition);
	for(i=0; i<allCP.length; i++){
		allCP[i].getMyColliderPosition();
		if(! allCP[i].gameObject.active){
			allCP[i].thisMove();	
		}
	}
	
	for(i=0; i<TransNotActive.length; i++){
		TransNotActive[i].gameObject.SetActiveRecursively(false);
	}

//	yield;
	CloseNotActive();
}

//var CloseCollider : boolean = false;
function CloseNotActive () {
//	if(CloseCollider){
//		CloseCollider = false;
		var i : int = 0;
		TransNotActive = new Array(); 
		for(i=0; i<ColderAll.length; i++){
			if(! ColderAll[i].gameObject.active){
				addTrans(ColderAll[i].gameObject.transform);
			}
		}
		
		for(i=0; i<TransNotActive.length; i++){
			TransNotActive[i].gameObject.active = true;
			yield;
			TransNotActive[i].gameObject.SendMessage("thisMove" , SendMessageOptions.DontRequireReceiver);
			TransNotActive[i].gameObject.active = false;
		}
//	}
}

function addTrans(ts : Transform){
	var useTrans : Transform[] ;
	if(!TransNotActive){
		TransNotActive = new Array();
	}
	useTrans = TransNotActive;
	TransNotActive = new Array(useTrans.length + 1);
	for(var i=0; i<useTrans.length; i++){
		TransNotActive[i] = useTrans[i];
 	}
 	TransNotActive[useTrans.length] = ts;
}

@script ExecuteInEditMode
