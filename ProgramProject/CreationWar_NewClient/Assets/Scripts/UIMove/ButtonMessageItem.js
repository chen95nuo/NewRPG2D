#pragma strict
var myTargetName : String;
var bm : UIButtonMessage;
function SetTargetName(str : String){
	myTargetName = str;
	bm = GetComponent(UIButtonMessage);
}

function Start(){
	bm.target = AllManage.buttonMessCL.GetTargetAsName(myTargetName);
}
