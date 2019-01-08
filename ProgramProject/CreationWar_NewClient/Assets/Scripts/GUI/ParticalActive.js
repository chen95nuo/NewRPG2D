#pragma strict

function Start () {

}

function Update () {

}

var part : ParticleEmitter;
function OnClick(){
	if(part && part.emit ){
		part.emit = false;
	}
}
