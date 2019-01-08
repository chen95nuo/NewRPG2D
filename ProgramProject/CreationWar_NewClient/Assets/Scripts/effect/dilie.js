var emits :  ParticleEmitter[];
var Delaytime = 0.0;
var tuotime = 0.1;
function OnEnable()  {
Delaystart();
}

function Delaystart(){
yield WaitForSeconds(Delaytime);
	for (var i : int = 0; i < emits.length; i++){
      emits[i].emit=true;
    }
     TimeOut();

}

function TimeOut() {

    yield WaitForSeconds(tuotime);
	for (var i : int = 0; i < emits.length; i++){
      emits[i].emit=false;
    }
}