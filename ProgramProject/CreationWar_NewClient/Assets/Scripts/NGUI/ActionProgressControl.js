#pragma strict

function Start () {
	AllManage.actionProCL = this;
}

var fsAction : UISlider;
var spriteAction : UISprite;
var objAction : GameObject;
var isAction : boolean = false;
function SetActionProgress(nowValue : float , MaxValue : float){
	if((MaxValue - nowValue) < MaxValue){
		if(!isAction){
			isAction = true;
		}
		if(spriteAction.color != Color.white){
			spriteAction.color = Color.white;
		}
		if(! objAction.active){
			objAction.SetActiveRecursively(true);
		}
		fsAction.sliderValue = (MaxValue - nowValue) / MaxValue;
	}else{
		if(objAction.active){
			objAction.SetActiveRecursively(false);
		}		
	}
}

function BreakActionProgress(){
	if(objAction.active){
		isAction = false;
		spriteAction.color = Color.red;
		yield WaitForSeconds(0.3);
		if(!isAction)
			objAction.SetActiveRecursively(false);
	}
}

function CloseActionProgress(){
	if(objAction.active){
		isAction = false;
		objAction.SetActiveRecursively(false);
	}
}