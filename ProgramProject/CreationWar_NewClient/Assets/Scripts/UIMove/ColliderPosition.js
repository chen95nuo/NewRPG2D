#pragma strict

var oldPositionY : float;
var oldScale : Vector3;
function OnDisable(){
	if(colder){
		transform.localScale = Vector3.zero;
	}
}

function OnEnable(){
	if(colder){
//		transform.localScale = oldScale;
		transform.localScale = Vector3.one;
	}
}

var colder : BoxCollider;
function getMyColliderPosition(){
	colder = GetComponent(BoxCollider);
	if(colder){
		oldScale = transform.localScale;
	}
}

function thisMove(){
	if(colder){
		transform.localScale = Vector3.zero;
	}
}
