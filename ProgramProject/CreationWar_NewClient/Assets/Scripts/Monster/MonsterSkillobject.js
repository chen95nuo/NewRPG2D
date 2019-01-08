#pragma strict
var photonView : PhotonView;
var selftag : String;
var skilltype :int;
var skilltime:int;
var scope :int;
var targetp : Vector3;
var PlayerID : int = 0;
var Damagetype=0;
var attackerLV = 1;
var mideffectID : int;
var endeffectID : int;
var damage :int;   //被计算的数值
var buffID:int;
var buffvalue:int;
var bufftime:int;
private var Enemytag :String;
var skillTime = 0.5;


function Start(){
yield;
if(PlayerID==0){
this.collider.enabled = false;
return;
}
if(selftag=="Enemy"||selftag=="Neutral")
Enemytag = "Player";
else if(selftag=="Player")
Enemytag = "Enemy";
photonView.RPC("scopesy",scope,skilltype,skilltime);
yield;
photonView.RPC("findtargetp",targetp);
if(mideffectID!=0)
photonView.RPC("PlayEffect",mideffectID);
yield WaitForSeconds(skillTime);
DestroyOb(mideffectID);

}

private var hittime=0.0;

function OnTriggerEnter(col : Collider) {
 if(col.CompareTag (Enemytag) && hittime+1.5<Time.time ){
	     hittime=Time.time;             
         SkillFX(col);
     }
}
     
function OnTriggerStay (col : Collider) {
if(col.CompareTag (Enemytag) ){
    yield;
    if(hittime+1.5<Time.time ){
	     hittime=Time.time;             
         SkillFX(col);
         }
  }
}

function SkillFX(col : Collider){
	if(col==null) 
	return;
	       var settingsArray = new int[5];
			settingsArray[0]= PlayerID;
			settingsArray[1]= damage;
			settingsArray[2]= damage;
			settingsArray[3]= Damagetype;
			settingsArray[4]= attackerLV;         						
	  col.SendMessage("ApplyDamage",settingsArray,SendMessageOptions.DontRequireReceiver );	
	  var tempbuffvalue = buffvalue;
	  if((buffID >=11 && buffID <=16)||(buffID >=34 && buffID <=38))
	     tempbuffvalue =buffvalue*damage*0.01;
	  if( buffID <=16||(buffID >=34 && buffID <=38)){
	  	   var setArray = new int[4];
            setArray[0]= PlayerID;
            setArray[1]= buffID;            
            setArray[2]= tempbuffvalue; 
            setArray[3]= bufftime;                                          						
	      col.SendMessage("ApplyBuff",setArray,SendMessageOptions.DontRequireReceiver );
	  	 if(endeffectID!=0)
	 photonView.RPC("PlayEffect",endeffectID);
     if(skilltype==5) 
	   DestroyOb(mideffectID);
   }
}

function DestroyOb(mideffectID:int){
      photonView.RPC("UnEffect",mideffectID);
      photonView.RPC("huandong",1,transform.position); 
      yield ;
      yield ;
      try{
            PhotonNetwork.Destroy(gameObject);      
    }catch(e){    
    }
} 

@RPC
function huandong(ID:int,position:Vector3){
	newCamera.huandong(ID,position);
}

@RPC
function PlayEffect(i:int){
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}

@RPC
function UnEffect(i:int){
    gameObject.BroadcastMessage ("Unspawn", i,SendMessageOptions.DontRequireReceiver);
}