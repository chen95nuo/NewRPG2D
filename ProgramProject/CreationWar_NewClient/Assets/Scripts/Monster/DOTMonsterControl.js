#pragma strict
class	TowerClass
{
	var	team	:	String;
	var posID	:	int;
	var levelID	:	int;
}

var MonsterSp : MonsterSpawn[];
var MonsterSpBoss1 : MonsterSpawn;
var MonsterSpBoss2 : MonsterSpawn;
var attacktime = 0;
private	var	photonView	:	PhotonView;
//var ts : TiShi;
var Towers : DotTower[];
var myTeam : String;

var viewID : int;
function	Awake()
{
	photonView	=	GetComponent(	PhotonView	);
	viewID		=	photonView.viewID;
}

function	Start ()
{
	if(	UIControl.myTeamInfo	!=	myTeam	)
	{
		this.tag	=	"Enemy";
	}
	else
	{
		this.tag	=	"Player";
	}
	yield	WaitForSeconds(5);
	yield;
	yield;
	yield;
	yield;
	if(	Health == 10	)
	{
		Health = DungeonControl.level * 1000;
	}
//if( !qr){
//	qr = AllManage.qrStatic;
//}
}

function DotaStart(){
	if(PhotonNetwork.isMasterClient){
		while(!PlayerStatus.MainCharacter){
			yield;
		}
		yield;
		InvokeRepeating("Callattack",5,45);
	}
}

function Callattack(){

// if(attacktime>9)
// return;
  attacktime +=1;
//   //print("call"+attacktime);
//  if(attacktime ==3 || attacktime ==6){
////  photonView.RPC("showtishi","Boss即将到来");
//  MonsterSpBoss1.CallMonster();
//  }
//  else if(attacktime ==9){
////   photonView.RPC("showtishi","最后一波！");  
//  MonsterSpBoss2.CallMonster();
//  }
//  else{
	photonView.RPC("showtishi","第 " + attacktime + " 波攻击即将开始！");     
//  }

	for(	var Mp : MonsterSpawn in MonsterSp	)
	{
		if(	Mp.gameObject.active	)
		{
			Mp.CallMonster( attacktime, attacktime - 1 );	
		}
	}
	Settarget();
}

private var settime= 0.0;
function OnTriggerEnter (other : Collider) {
if(other.CompareTag ("Enemy")&&settime<Time.time+3){
    photonView.RPC("PlayEffect",133);  
    settime = Time.time;
	other.SendMessage("Settargetp",transform,SendMessageOptions.DontRequireReceiver );
  }
}


private var gos : BattlefieldCityItem[];
function Settarget(){
	yield;
	yield;
	var bool : boolean = false;
	gos = FindObjectsOfType(BattlefieldCityItem); 
   for (var go : BattlefieldCityItem in gos) {
		if(go.myTeam != myTeam){
			bool = true;
			for(var i =0; i<Towers.length; i++){
				if(Towers[i].tw.posID == go.myPosLu && !Towers[i].dead && bool){
					bool = false;
					go.gameObject.SendMessage("Settargetm",Towers[i].gameObject.transform,SendMessageOptions.DontRequireReceiver);					
				}
			}
			if(bool){
				go.gameObject.SendMessage("Settargetm",transform,SendMessageOptions.DontRequireReceiver);
			}
		}
 	}
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
    photonView.RPC("PlayEffect",3);         
	Health -= damage;
   if(Damagetype>5){
   AllResources.FontpoolStatic.SpawnEffect(6,transform,"暴击!",2);    
   AllResources.FontpoolStatic.SpawnEffect(4,transform,damage+"",2.5);
   }
   else
   	AllResources.FontpoolStatic.SpawnEffect(0,transform,damage+"",2.5);
    photonView.RPC("SynHealth",Health);	     
	photonView.RPC("hitanimation",PhotonTargets.All);
	photonView.RPC("PlayAudio",1);
	if (!dead &&Health <= 0)
	{
    	photonView.RPC("PlayAudio",2);
    	photonView.RPC("Die",PhotonTargets.All);

	}
}

@RPC
function Die ()
{  
  	dead = true;
  this.tag ="Ground";
  GameOver();
  Burn(); 
}

var bodymesh : MeshRenderer;
function Burn(){
//     var bodymesh= GetComponentInChildren (MeshRenderer);
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
	if(UIControl.myTeamInfo == myTeam){
		AllManage.timeDJStatic.Show(gameObject , "LaterDaojiReturn" ,"ReturnLevel" ,  6, "messages022" , "messages023"  , "messages024" , false);			
//		qr.ShowQueRen(gameObject , "ReturnLevel" , "任务失败！");  			
	}else{
		AllManage.mtwStatic.DoneWin();
		yield WaitForSeconds(4);
		AllManage.timeDJStatic.Show(gameObject , "LaterDaojiReturn" ,"ReturnLevel" ,  6, "messages022" , "messages023"  , "messages024" , false);			
//		qr.ShowQueRen(gameObject , "ReturnLevel" , "完成任务！");  				
	}
}

function LaterDaojiReturn(){
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
//		AllResources.ar.AllLoadLevel("Loading 1");
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
    AllManage.tsStatic.ShowBlue(Str);
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