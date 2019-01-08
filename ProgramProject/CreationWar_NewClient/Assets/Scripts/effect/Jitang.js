private var emitter: ParticleEmitter;
private var photonView : PhotonView;
private var aa = true;
var amount = 1;
var sound : AudioClip;

function Awake(){
photonView = GetComponent(PhotonView);
emitter = transform.Find("Particle").GetComponent(ParticleEmitter);
animation["jitang"].wrapMode = WrapMode.ClampForever ;
animation.Stop();
aa = true;

} 
	    
function OnTriggerEnter (col : Collider) {
if(aa){
	var playerStatus : PlayerStatus = col.GetComponent(PlayerStatus);	
	if ( playerStatus == null )	
		return;
	amount = DungeonControl.level*28+50;
	 playerStatus.AddHealth(amount);
	  playerStatus.AddMana(amount*0.5);
    photonView.RPC("Bomba",PhotonTargets.All);
    }
}

@RPC
function Bomba(){
	aa=false;
    animation.Play("jitang");  
    yield WaitForSeconds(0.3);        
    if(emitter) 
    emitter.emit = true;
	if (sound)
	AudioSource.PlayClipAtPoint(sound, transform.position, 1);
	AllResources.EffectGamepoolStatic.SpawnEffect(72,transform);	
    yield WaitForSeconds(0.5);
     if(emitter) 
    emitter.emit = false;
    	yield WaitForSeconds(5);
    	try{
          //if(PhotonNetwork.isMasterClient)
      PhotonNetwork.Destroy(gameObject);  
    }catch(e){  
    }  

}
	