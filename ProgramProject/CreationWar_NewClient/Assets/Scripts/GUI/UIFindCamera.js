#pragma strict
class HelpObject{
	var Obj : GameObject[];
	var id : int;
}

var HelpObj : HelpObject[];
var allHelpObj : GameObject;
var UIAH : UIAnchor[];
var cam : Camera;
var cube : GameObject;

var right : Transform;
static var MonsterDie : boolean = false;
function Start () {
	while(!GameObject.Find("2dSongCamera")){
		yield;
	}
	cam = GameObject.Find("2dSongCamera").GetComponent(Camera);
	UIAH[0].uiCamera = cam;
	UIAH[1].uiCamera = cam;	
	UIAH[2].uiCamera = cam;	
//	allHelpObj.SetActiveRecursively(false);
	right.localPosition = Vector3.zero;
}

function Update () {
//	if(MonsterDie && step == 1){
//		allHelpObj.SetActiveRecursively(false);
//		MonsterDie = false;
//	}
} 

function Help200(){
	HelpStep(0);
	yield WaitForSeconds(3);
	HelpStep(1);
}

var step : int;
function HelpStep(id : int){
	step = id;
	var i : int = 0;
	var j : int = 0;
	allHelpObj.SetActiveRecursively(false);
	UIAH[0].gameObject.active = true;
	UIAH[1].gameObject.active = true;
	for(i=0; i<HelpObj.length; i++){
		if(HelpObj[i].id == id){
			cube.SetActiveRecursively(true);
			for(j=0; j<HelpObj[i].Obj.length; j++){
				HelpObj[i].Obj[j].SetActiveRecursively(true);
			}
			return;
		}
	}
}

function guanbi(){
	gameObject.SetActiveRecursively(false);	
}