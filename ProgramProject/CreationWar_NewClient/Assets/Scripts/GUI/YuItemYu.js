#pragma strict

function Update () {
	if(canKao && isKao){
		shuzhi += Time.deltaTime/2;
	}
}

var shuzhi : float;
var isKao : boolean =false;
var canKao : boolean = false;
function OnTriggerEnter(other : Collider){
	if(other.tag == "YuItemHuo"){
		isKao = true;
	}
}

function OnTriggerExit(other : Collider){
	if(other.tag == "YuItemHuo"){
		isKao = false;
	}	
}

function KaoStart(){
	
}

function KaoExit(){
	shuzhi = 0;
}
