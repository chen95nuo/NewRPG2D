#pragma strict

var btn : GameObject;
var functionName : String;
var functionObj : GameObject;
function Awake(){
	AllManage.btnInfoStatic = this;
}

function  ShowInfoButton(Obj : GameObject , Fution : String){
	btn.SetActiveRecursively(true);
	functionName = Fution;
	functionObj = Obj;
}

function ClickReturn(){
	functionObj.SendMessage(functionName , SendMessageOptions.DontRequireReceiver );
	functionObj = null;
	btn.SetActiveRecursively(false);
}
