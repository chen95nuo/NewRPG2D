#pragma strict

function Start () {

}

function Update () {
		ShowBool();
}
var bagItem : BagItem;
var showBoolObj : GameObject[];
	function ShowBool(){
	if(showBoolObj!=null){
	if(bagItem.inv==null){
		for(var i=0; i<showBoolObj.length; i++){	
			showBoolObj[i].SetActive(false);	
	}
	}else{
	for(var j=0; j<showBoolObj.length; j++){
			showBoolObj[j].SetActive(true);
			}
	}
	}
	}