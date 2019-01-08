#pragma strict

function Update () {
	if(canKao && isKao){
		shuzhi += Time.deltaTime;
	}
}

var shuzhi : float;
var isKao : boolean =false;
var canKao : boolean = false;
function OnTriggerEnter(other : Collider){
	if(other.CompareTag ("YuItemHuo")){
		isKao = true;
	}
}

function OnTriggerExit(other : Collider){
	if(other.CompareTag ("YuItemHuo")){
		isKao = false;
	}	
}

function KaoStart(){
	
}

function KaoExit(){
	shuzhi = 0;
}
