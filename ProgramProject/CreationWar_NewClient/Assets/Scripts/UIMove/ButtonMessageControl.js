#pragma strict
var bool : boolean = false;
var allTarget : GameObject[];
var TransNotActive : Transform[];
var btnMessg : UIButtonMessage[];
function Awake(){
	AllManage.buttonMessCL = this;
}

function Update () {
	if(bool){
		bool = false;
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
		var useBM : UIButtonMessage[];
		useBM = FindObjectsOfType(UIButtonMessage);
		var targetObj : GameObject;
		for(var bm : UIButtonMessage in useBM){
			if(bm.target == null){
				addbtnM(bm);
			}
		}
		
		for(var btnM : UIButtonMessage in btnMessg){
			for(var bm : UIButtonMessage in useBM){
				if(btnM != bm && btnM.functionName == bm.functionName){
					if(bm.target != null){
						btnM.gameObject.AddComponent(ButtonMessageItem);
						btnM.gameObject.SendMessage("SetTargetName" , bm.target.name , SendMessageOptions.DontRequireReceiver);
						AddTarget(bm.target);
					}
				}
			}
		}
		
		for(i=0; i<TransNotActive.length; i++){
			TransNotActive[i].gameObject.SetActiveRecursively(false);
		}
	}
}

function GetTargetAsName(str : String) : GameObject{
//	if(allTarget[4] == null){
//		yield;
//	}
	switch(str){
		case "0UIControl":
			return allTarget[0];
			break;
		case "InventoryControl":
			return allTarget[1];
			break;
		case "SendMangage":
			return allTarget[2];
			break;
		case "UIAllPanelControl":
			return allTarget[3];
			break;
		case "Anchor - CangKu":
			return allTarget[4];
			break;
		case "Button - SelectInfo":
			return allTarget[5];
			break;
	}
	return null;
}

function AddTarget(target : GameObject){
	if(allTarget){
		for(var obj : GameObject in allTarget){
			if(obj == target){
				return;
			}
		}
	}
	var useTrans : GameObject[] ;
	if(!allTarget){
		allTarget = new Array();
	}
	useTrans = allTarget;
	allTarget = new Array(useTrans.length + 1);
	for(var i=0; i<useTrans.length; i++){
		allTarget[i] = useTrans[i];
 	}
 	allTarget[useTrans.length] = target;
}

function addbtnM(ts : UIButtonMessage){
	var useTrans : UIButtonMessage[] ;
	if(!btnMessg){
		btnMessg = new Array();
	}
	useTrans = btnMessg;
	btnMessg = new Array(useTrans.length + 1);
	for(var i=0; i<useTrans.length; i++){
		btnMessg[i] = useTrans[i];
 	}
 	btnMessg[useTrans.length] = ts;
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

//@script ExecuteInEditMode