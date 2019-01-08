#pragma strict
var TransNotActive : Transform[];
function Awake(){
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
		TransNotActive[i].gameObject.AddComponent("ActiveItem");
		TransNotActive[i].gameObject.SendMessage("CloseNow" , SendMessageOptions.DontRequireReceiver);
		TransNotActive[i].gameObject.SetActiveRecursively(false);
	}	
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

function Update () {

}

@script ExecuteInEditMode