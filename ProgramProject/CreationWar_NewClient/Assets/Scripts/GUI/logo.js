#pragma strict

//var Logobtn : GameObject;
var emits :  ParticleEmitter[];

function StartEffect () {
	yield WaitForSeconds(1);

	for (var i : int = 0; i < emits.length; i++){
      emits[i].emit=true;
    }
//    yield WaitForSeconds(1);
//Logobtn.SetActiveRecursively(true);
}

function OnEnable()
{
	StartCoroutine(StartEffect());
}

