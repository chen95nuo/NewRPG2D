#pragma strict

var uicl : UIControl;
function OnSelect (selected : boolean){
	if(Application.loadedLevelName == "Map200"){
		return;
	}
	uicl.showWenHao(selected);
}