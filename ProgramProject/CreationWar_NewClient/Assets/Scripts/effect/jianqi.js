
private var dd : ParticleEmitter;

function OnEnable()  {
dd = GetComponentInChildren(ParticleEmitter);
TimeOut();
}


function TimeOut() {
dd.emit=true;
yield WaitForSeconds(0.8);
dd.emit=false;
}
