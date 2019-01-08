#pragma strict
var photonView : PhotonView;
var MonsterSpBoss : MonsterSpawn;
var MonsterSp : MonsterSpawn[];
var cc : SphereCollider ;
var Story : Storycamera;
private var pp = true;
function Awake(){
this.collider.enabled = false;
}
function Start () {
while(!PlayerStatus.MainCharacter){
 yield;
}
yield;
for(var Mp : MonsterSpawn in MonsterSp){
Bigger(Mp.gameObject); 
Mp.gameObject.collider.enabled = true;
     yield WaitForSeconds(2);
}
 yield WaitForSeconds(3); 
 cc.radius = 100;

while (pp){
    if(MonsterSpBoss.IsCleared() ){
     SpawnBoss();
     pp = false;
     }
     yield WaitForSeconds(1);
 }
}

private var gos : GameObject[];
var TC : TalkControl;

function Bigger(tt:GameObject){
tt.transform.localScale = Vector3(0.01,0.01,0.01);
var tempscale = 0.01;
while(tempscale<=1){
tt.transform.localScale = Vector3(tempscale,tempscale,tempscale);
tempscale += Time.deltaTime*0.2;
yield;
}
}

function	SpawnBoss ()
{
	yield	WaitForSeconds(0.5); 
	Story.Playani (1,transform.position);
	Bigger(this.gameObject); 
	yield; 
	this.collider.enabled = true;
	photonView.RPC("PlayEffect",114);
	yield WaitForSeconds(1.5);
	gos	=	GameObject.FindGameObjectsWithTag("Enemy");
	Debug.Log("K_________Spawn Boss Stop Animation. +");
	for(	var go : GameObject in gos) 
	{
		Debug.Log("K_________Spawn Boss Stop Animation. + " + go.name );
		go.SendMessage(	"PauseAttack", true,	SendMessageOptions.DontRequireReceiver);    
	}
	TC	=	FindObjectOfType(TalkControl);
	TC.LevelID = 1;
	TC.step = 0;
	TC.ShowTalkAsStep(gameObject , "BossStep");
	yield WaitForSeconds(2);
	Story.Stopani (1);
}

function BossStep(){
//“审判者”尤利西斯：凡人，你们不会明白！永世光明的时代即将到来。跪下，我可赐予你救赎。
//	yield WaitForSeconds(3);
	TC.LevelID = 3;
	TC.step = 0;
	TC.ShowTalkAsStep(gameObject , "createPlayer");
}

function createPlayer(){
	AllManage.jiaochengCLStatic.CreateRobot2();
	yield WaitForSeconds(1);
	TC.LevelID = 4;
	TC.step = 0;
	TC.ShowTalkAsStep(gameObject , "MonsterGoOn");
}

function MonsterGoOn(){


//审判者：无信是最大的罪恶，你们都接受审判吧。(语音)
var trytime = 5;
while(trytime>0){
  gos = GameObject.FindGameObjectsWithTag("Enemy"); 
   for (var go : GameObject in gos) {
  	 go.SendMessage("ForceAttack",SendMessageOptions.DontRequireReceiver);    
 	}
 	trytime -=1;
    yield WaitForSeconds(1);
}

}

@RPC
function PlayEffect(i:int){
    AllResources.EffectGamepoolStatic.SpawnEffect(i,transform);
}
