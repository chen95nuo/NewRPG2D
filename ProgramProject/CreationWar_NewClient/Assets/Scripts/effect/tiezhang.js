var tSound : AudioClip;
private var tiezhang : GameObject;
private var tiezhangp : ParticleEmitter;
private var aa = true;
function Awake(){
tiezhang = GameObject.Find("tiezhanParticle");
tiezhangp = tiezhang.GetComponent( ParticleEmitter);
}
function Start(){

	while (true)
	{  yield WaitForSeconds(animation["work"].length*0.3);
	   if(animation.IsPlaying("work") ){
       tiezhangp.emit = true;
       audio.PlayOneShot(tSound, 1.0 / audio.volume);
       }
       yield; 
       tiezhangp.emit = false;
       yield WaitForSeconds(animation["work"].length*0.7);
   }

}
