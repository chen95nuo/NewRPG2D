#pragma strict

#pragma strict

//var Logobtn : GameObject;
var emits :  ParticleSystem[];

function StartEffect () {
	yield WaitForSeconds(4.1f);

	for (var i : int = 0; i < emits.length; i++){
      emits[i].Play();
    }
    
//    yield WaitForSeconds(2);
//    for (var j : int = 0; j < emits.length; j++){
//      emits[j].Stop();
//    }
//    yield WaitForSeconds(1);
//Logobtn.SetActiveRecursively(true);
}

function Start()
{
	
}
function Update(){
	StartCoroutine(StartEffect());
}
