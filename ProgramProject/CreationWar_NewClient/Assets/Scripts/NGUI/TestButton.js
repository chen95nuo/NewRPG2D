#pragma strict

function Awake(){
	AllManage.AttackButtonStatic = this;
}

function Start () {
	InvokeRepeating("anxia", 0.1, 0.1);  
}

function Update () {
	if (Input.GetButtonUp ("Fire1")) {
		bool = false;
		Faguan.enabled = false;
	}
	
}
//
//function OnScroll (delta : float){
//	//print(delta);
//}
//
//function OnClick(){
//	//print("123123123");
//
//}
function anxia(){
	if(bool && AllManage.SkillCLStatic){
		AllManage.SkillCLStatic.AttackSimple();
		Faguan.enabled = true;
	}
}
//var target : SkillControl;
var bool : boolean = false;
var Faguan : UISprite; 
var mapTalk : MapTalkControl;
var isbool : boolean = false;
function OnClick(){
if(Application.loadedLevelName == "Map200"){
	mapTalk	=	FindObjectOfType(	MapTalkControl	);
		if(mapTalk){
//	mapTalk.TalkObj.SetActive(false);
	yield;
	if(!isbool){
	isbool = true;
//	mapTalk.TalkObj1.SetActive(true);
	}
	}
	}
	
	if(Application.loadedLevelName == "Map212"){
	mapTalk	=	FindObjectOfType(	MapTalkControl	);
	if(mapTalk){
//	mapTalk.TalkObj.SetActive(false);
	}
	}
	bool = false;
	Faguan.enabled = false;
}

function OnPress ( isPressed : boolean){
	bool = true;
}

function ReSetAnXia(){
	bool = false;
	Faguan.enabled = false;
}
