private var aa=false;
private var m =1.0;
private var color : Color = Color.white;
private var emits1 : ParticleEmitter;
function Awake(){
emits1 =transform.Find("Particle1").GetComponent(ParticleEmitter);
}
function OnEnable(){
aa=true;
m =1.0;
TimeOut();
}

function TimeOut() {
     emits1.emit=true;
    yield WaitForSeconds(0.5);
     emits1.emit=false;
}

function Update () {
	if(aa){	
		m-=0.5*Time.deltaTime; 			
	color.a = m;
	var bodymeshs = GetComponentsInChildren (MeshRenderer);
    for (var bodymesh : MeshRenderer in bodymeshs) 	
	bodymesh.renderer.material.SetColor ("_TintColor", color);
			if(m<=0){			
		     m=0;
//		     gameObject.SetActiveRecursively(false);		
		     aa = false;
		    }		  
		}	

}