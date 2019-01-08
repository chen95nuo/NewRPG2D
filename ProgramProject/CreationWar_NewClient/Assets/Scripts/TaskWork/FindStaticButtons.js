#pragma strict

var id : int;
var button : GameObject;
var uibtn : UIButtonMessage;
function Start () {
	yield;
	yield;
	uibtn.target = ShowStaticButtons.STbtn[id];
}

function Update () {

}