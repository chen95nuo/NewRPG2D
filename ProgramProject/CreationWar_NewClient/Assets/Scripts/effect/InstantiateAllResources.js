#pragma strict

static var InstantiateBool : boolean = false;
var obj : GameObject;
function Start(){
	if( ! InstantiateBool){
		InstantiateBool = true;
		GameObject.Instantiate(obj);
	}
}
