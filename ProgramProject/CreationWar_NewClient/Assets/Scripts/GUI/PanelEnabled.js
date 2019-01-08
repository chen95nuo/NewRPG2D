#pragma strict


var panel : UIPanel;
function OnDisable(){
	if(!panel){
		panel = GetComponent(UIPanel);	
	}
	if(panel){
		panel.enabled = false;
	}
}

function OnEnable(){
	if(!panel){
		panel = GetComponent(UIPanel);	
	}
	close(true);
}

function close(bool : boolean){
	yield;
	yield;
	yield;
	yield;
	yield;
	yield;
	if(panel){
		panel.enabled = bool;
	}
}
