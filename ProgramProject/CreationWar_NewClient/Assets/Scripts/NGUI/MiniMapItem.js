#pragma strict

function Start () {

}

function Update () {

}

var ebCL : MiniMapControl;
var myNPC : npcAI;
var LabelName : UILabel;
var LabelJinDu : UILabel;
var SpriteTask : UISprite;
function setInv(ai : npcAI){
	myNPC = ai;
	if(myNPC.NPCTypeName!=""){
	LabelName.text = "<"+myNPC.NPCTypeName +">"+ myNPC.NPCname ;
	}else{
	LabelName.text =  myNPC.NPCname ;
	}
	
	SpriteTask.enabled = false;
	if(ai.canFuhao){
		SpriteTask.enabled = true;
		switch(myNPC.fuhaoID){
			case 0:
				SpriteTask.spriteName = "xiaotanhao";
				break;
			case 1:
				SpriteTask.spriteName = "wenhaohui";
				break;
			case 2:
				SpriteTask.spriteName = "wenhao";
				break;
		}
	}
}

var myMonsterSP : MonsterSpawn;
function setInv(sp : MonsterSpawn){
	myMonsterSP = sp;
	if(!myMonsterSP.isTask){
		SpriteTask.enabled = false;	
		LabelName.text = myMonsterSP.MonsterName;
	}else{
		LabelName.text = myMonsterSP.Taskrows["NPCName"].YuanColumnText;
		SpriteTask.enabled = true;	
		SpriteTask.spriteName = "xiaotanhao";
	}
}

function GoSelect(){
	ebCL.SelectOneInv(this);
}

var jiantou : UISprite;
function SelectMe(){
	jiantou.enabled = true;
}

function DontSelectMe(){
	jiantou.enabled = false;
}

function GoItemWay(){
	if(myNPC){
		ebCL.GoNPCWay(myNPC.gameObject.transform.position);
	
	}else
	if(myMonsterSP){
		ebCL.GoNPCWay(myMonsterSP.gameObject.transform.position);	
	}
}