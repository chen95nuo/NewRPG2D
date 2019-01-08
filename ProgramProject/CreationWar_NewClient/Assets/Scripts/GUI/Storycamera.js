#pragma strict
class StoryAni{
	var SCamera : Camera;
	var animateO : GameObject;
	var animate : AnimationClip;
	}
var Storyc :StoryAni[];
var soul :GameObject;
var soulFX :GameObject;

var TC : TalkControl;
var UIFindCam : UIFindCamera;
function Awake () {
	AllManage.camStoryStatic = this;
	Storyc[0].SCamera.gameObject.SendMessage("BlackAll",SendMessageOptions.DontRequireReceiver);
}

function StartStory(){
	Storyc[0].SCamera.gameObject.SendMessage("BlackIn",SendMessageOptions.DontRequireReceiver);
//	while(!PlayerStatus.MainCharacter){
//		yield;
//	}
//	yield;
//	yield;
	yield StartCoroutine("Playani",0);
//	yield WaitForSeconds(1);
	TC = FindObjectOfType(TalkControl);
	UIFindCam =  FindObjectOfType(UIFindCamera);
	TC.LevelID = 0;
	TC.step = 0;
	TC.ShowTalkAsStep(gameObject , "StartOneStep");
	TC.changeIconAsProID(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText);
	//  yield StartCoroutine("Playani",1);
	//  yield WaitForSeconds(1);
	//  yield StartCoroutine("Playani",2);
}

function StartOneStep(){
  if(!UIFindCam)
 	UIFindCam =  FindObjectOfType(UIFindCamera);
//	UIFindCam.Help200();
//	小Boss：（诡异笑声）异端，这里将是你的葬身之地，如果放弃抵抗，我会赏赐你一个体面的死亡。(语音)
  yield WaitForSeconds(5);
  if(Application.loadedLevelName != "Map200"){
  	MonsterOnebyOne();
  	}
}

function Playani (i:int) {
//Storyc[i].SCamera.enabled = true;
Storyc[i].animateO.animation.Play();
  yield WaitForSeconds(Storyc[i].animate.length);
Storyc[i].SCamera.enabled = false;
return;
}

function Playani (i:int,position:Vector3) {
Storyc[i].animateO.transform.position = position;
Storyc[i].SCamera.enabled = true;
if(i==3){
Storyc[i].SCamera.animation.Play();
soulFX.SetActiveRecursively(true);
soul.SetActiveRecursively(true);
 yield WaitForSeconds(6);  
 if(Storyc[i] && Storyc[i].SCamera)
	Storyc[i].SCamera.gameObject.SendMessage("BlackOut",SendMessageOptions.DontRequireReceiver); 
}
else
Storyc[i].animateO.animation.Play();
if(i==1){
  yield WaitForSeconds(Storyc[i].animate.length);
  if(Storyc[i] && Storyc[i].SCamera)
 Storyc[i].SCamera.enabled = false;
 }
}

function Stopani(i:int){
 Storyc[i].SCamera.enabled = false;
}

function MonsterOnebyOne(){
  var gos = GameObject.FindGameObjectsWithTag("Enemy"); 
   for (var go : GameObject in gos) {
   if(go && go.GetComponent(MonsterStatus).getMonsterLevel()>=3){
  	   go.SendMessage("ForceAttack",SendMessageOptions.DontRequireReceiver); 
  	   yield WaitForSeconds(8);  
  	   }
  	 yield; 
 	}
 	gos = GameObject.FindGameObjectsWithTag("Enemy"); 
   for (var go : GameObject in gos) {
   if(go && go.GetComponent(MonsterStatus).getMonsterLevel()<3){
  	   go.SendMessage("ForceAttack",SendMessageOptions.DontRequireReceiver); 
  	   yield WaitForSeconds(8);  
  	   }
  	 yield; 
 	} 	
}

function BossWasDie(){
//禁止玩家操作
	AllManage.UICLStatic.ObjDontControl.enabled = true;
//（死亡前）动画
	yield WaitForSeconds(3);
//审判者：（狂笑声）我已得永生，而你们，终究逃不出审判。（狂笑声）
	TC.LevelID = 5;
	TC.step = 0;
	TC.ShowTalkAsStep(gameObject , "BossBoom");
}

function BossBoom(){
//————————大爆炸————————
//杀死玩家
}
