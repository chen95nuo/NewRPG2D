#pragma strict
//var ts : TiShi; 
var tw : TowerClass;

var remoteType : bulletType;
 var Enemytag ="Player";
private var photonView : PhotonView;
private var mideffect:int;
private var endeffect:int;
var DamagetypeAttack : Damagestype = 0;
var attackRange = 16;
function Start (){
	if(UIControl.myTeamInfo != tw.team){
		this.tag ="Enemy"; 
		Enemytag = "Player";
	}else{
		this.tag ="Player";
		Enemytag = "Enemy";
	}
		photonView = GetComponent.<PhotonView> ();
//	if( !qr){
//		qr = AllManage.qrStatic;
//	}

 if(tw.team == "0"){
	remoteType = 2;
 }else
 if(tw.team == "1"){
 	 remoteType = 3;
 }
 switch (remoteType)
 {	 case 1: 
      mideffect=55;
      endeffect=3;
      DamagetypeAttack = 1;
     break; 
      
   case 2: 
      mideffect=45;
      endeffect=58;
      DamagetypeAttack = 2;
     break;
     
   case 3: 
      mideffect=73;
      endeffect=59;
      DamagetypeAttack = 3;
     break;
     
     case 4: 
      mideffect=74;
      endeffect=76;      
      DamagetypeAttack = 4;
     break;
     
     case 5: 
      mideffect=56;
      endeffect=75; 
      DamagetypeAttack = 5;
     break;  
  }
	
	yield;
	yield;
	yield;
	yield;
   damageT = DungeonControl.level * DungeonControl.NowMapLevel*100;
	Health = DungeonControl.level * 1000;
	while (!dead) {
	    if (CanSeeTarget()){
			  yield StartCoroutine("Shoot");
			  }
		     else yield Idle();
	 }

}
function Idle ()
{
	yield WaitForSeconds(0.5);
}

var delayAttackTime = 2.5;
private var fxobject : GameObject; 
private var damageT =10; 
private var lastatktime=0.0; 
function Shoot () {
	if(fxobject==null)
		fxobject = Resources.Load("BulletObject1", GameObject);
	yield WaitForSeconds(delayAttackTime-1);
	       if(targetp){
   photonView.RPC("PlayAudio",2);
   var cc = PhotonNetwork.Instantiate( fxobject.name, transform.position+transform.up*9,transform.rotation,0);
   var skillobject : Bulletobject = cc.GetComponent(Bulletobject); 
       skillobject.targetp = targetp.transform.position + transform.up*2;
       skillobject.PlayerID = parseInt(photonView.viewID);
       skillobject.Damagetype = DamagetypeAttack;
       skillobject.attackerLV = 1; 
       skillobject.mideffectID = mideffect;    
       skillobject.endeffectID = endeffect; 
       skillobject.damage = damageT;
       skillobject.Enemytag = Enemytag;  
       }
		yield WaitForSeconds(0.7);
		lastatktime = Time.time;
	   return;
}

 var targetp : Transform;
function CanSeeTarget () : boolean {
      targetp = FindClosestEnemy();
      if(!targetp)
        	return false;      
	  else if (Vector3.Distance(transform.position, targetp.position) > attackRange)
		  return false;	
	  var hit : RaycastHit;	
	  if (Physics.Linecast (Vector3(transform.position.x,transform.position.y+3,transform.position.z), Vector3(targetp.position.x,targetp.position.y+3,targetp.position.z), hit))
		 return true;

	return false;
}

private var settime= 0.0; 
private var battlefieldCI : BattlefieldCityItem;
function OnTriggerEnter (other : Collider) { 
	battlefieldCI = other.gameObject.GetComponent(BattlefieldCityItem);
if(battlefieldCI && battlefieldCI.myTeam != tw.team&&settime<Time.time+1){
//    photonView.RPC("PlayEffect",133);  
    settime = Time.time;
	other.SendMessage("Settargetp",transform,SendMessageOptions.DontRequireReceiver );
  }
}

private function FindClosestEnemy () : Transform
 {
   var gos : GameObject[];
  gos = GameObject.FindGameObjectsWithTag(Enemytag);
  var closest : GameObject;
  var distance = Mathf.Infinity;
for (var go : GameObject in gos) {
    var diff = (go.transform.position - transform.position);
    var curDistance = diff.sqrMagnitude;
    if (curDistance < distance) {
       closest = go;
       distance = curDistance;
       }
    }
 if(closest)
return closest.transform;
else
return null;
}


private var pnumber :int;
private var damage :int;
private var Damagetype=0;    //0为默认物理伤害，1~4为魔法，1是技能（奥术），2是冰?是火?是暗影，5是毒（自然）  
var Health = 10;
var dead = false;
function ApplyDamage (info : int[])
{	////print("hit me!");
  if(dead)
   return;
	pnumber = info[0];
    damage = info[1];
    Damagetype = info[3];   
    photonView.RPC("PlayEffect",133);          
	Health -= damage;
   if(Damagetype>5){
   AllResources.FontpoolStatic.SpawnEffect(6,transform,"暴击!",2);    
   AllResources.FontpoolStatic.SpawnEffect(4,transform,damage+"",2.5);
   }
   else
   	AllResources.FontpoolStatic.SpawnEffect(0,transform,damage+"",2.5);
    photonView.RPC("SynHealth",Health);	     
//	photonView.RPC("hitanimation",PhotonTargets.All);
//	photonView.RPC("PlayAudio",1);
	if (!dead &&Health <= 0)
	{
        photonView.RPC("PlayEffect",29); 
    	photonView.RPC("Die",PhotonTargets.All);
		usePS = FindPlayerAsID(info[0]);
		if(usePS){
			usePS.AddPVPPoint(20 * (-1));
			usePS.AddPVP8Info(PVP8InfoType.Tower , 20);
		}
	}
}

private var usePS : PlayerStatus;
private var pss : PlayerStatus[];
function FindPlayerAsID(id : int) : PlayerStatus{
	pss = FindObjectsOfType(PlayerStatus);
	for(var i=0; i<pss.length; i++){
		if(pss[i].PlayerID == id){
			return pss[i];
		}
	}
}

@RPC
function Die ()
{  
  	dead = true;
  this.tag ="Ground";
   Burn(); 

}

function Burn(){
     var bodymesh= GetComponentInChildren (MeshRenderer);
     var m_Mat = bodymesh.renderer.material;
	 m_Mat.shader = Shader.Find("Dissolve/Dissolve_TexturCoords");
	 m_Mat.SetColor("_DissColor",Color(1, 0.2, 0, 1));
	 m_Mat.SetTexture ("_DissolveSrc",AllManage.NoisemapStatic);
 var m_fTime = 0.0;
 if(PhotonNetwork.isMasterClient)
  photonView.RPC("PlayEffect",164); 
	 while( m_fTime<=1.0 ){
	 m_Mat.SetFloat("_Amount", m_fTime);
	 m_fTime += Time.deltaTime;
	 yield;
	 }
   gameObject.SetActiveRecursively(false);	 
}

//var qr : QueRen;
function GameOver(){
  AllManage.qrStatic.ShowQueRen(gameObject , "ReturnLevel" , "messages001");  	
}

function ReturnLevel(){
	Loading.Level = DungeonControl.ReLevel;
	InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	AllManage.UICLStatic.RemoveAllTeam();
	alljoy.DontJump = true;
	yield;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

}

@RPC
function SynHealth(P : int){
Health = P;
}

@RPC
function hitanimation(){
if(animation["hit"] != null)
 animation.CrossFade("hit",0.1);
}

@RPC
function PlayEffect(i:int){
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}

@RPC
function showtishi(Str:String){
//  if(!ts)
//  ts = FindObjectOfType(TiShi);
    AllManage.tsStatic.Show(Str);
}

var hitSound  : AudioClip;
var deadSound  : AudioClip;
@RPC
function PlayAudio(i:int){
switch (i)
{				
     case 1:
     if(hitSound)	
	 audio.PlayOneShot(hitSound);
	 break;
	 
     case 2:
     if(deadSound)	
	 audio.PlayOneShot(deadSound);
	 break;
 }
}