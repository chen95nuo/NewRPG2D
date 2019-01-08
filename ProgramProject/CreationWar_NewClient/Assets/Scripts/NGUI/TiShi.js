#pragma strict
var Loc1 : UILocalize;
var Loc2 : UILocalize;
var Loc3 : UILocalize;
var audioClip : AudioClip;
var particLevelUp : ParticleEmitter;
function Awake(){
	AllManage.tsStatic = this;
}

//private var dTime : int = 0;
function Update () {
	if(Time.time > ptime && isShow){
		Close();
	}
	if(Time.time > ptime1 && isShow1){
		Close1();
	}
	if(Time.time > ptime2 && isShow2){
		Close2();
	}
	if(Time.time > ptimeFinger && isShowFinger){
		CloseFinger();
	}
//	if(Time.time > dTime){
//		if(isAutoMove){
//			
//		}
//	}
}

//private var isAutoMove : boolean = false;
var objAutoMove : GameObject;
function showAutoMove(){
	objAutoMove.SetActiveRecursively(true);
}

function closeAutoMove(){
	objAutoMove.SetActiveRecursively(false);
}

var obj :  GameObject;
var LabelText : UILabel;
var ptime : float = 0;
var isShow : boolean = false;
function Show(str : String){
	isShow = true;
	LabelText.text = AllManage.AllMge.Loc.Get(str);
	obj.SetActiveRecursively(true);
	ptime = Time.time + 3;
	PlayAnyAudioClip();
}

function Show1(str : String){
	isShow = true;
	obj.SetActiveRecursively(true);
	ptime = Time.time + 3;
	LabelText.text = str;
	PlayAnyAudioClip();
}

function Close(){
	isShow = false;
	obj.SetActiveRecursively(false);
}

var charbar : GameObject;
function LiaoTian(str : String , col : Color){
	var obj : Object[];
	obj = new Array(2);
	obj[0] = str;
	obj[1] = col;
//	//print(obj[0] + " ==== " + obj[1]);
	charbar.SendMessage("AddText",obj,SendMessageOptions.DontRequireReceiver);
}

var obj1 :  GameObject;
var LabelText1 : UILabel;
var ptime1 : float = 0;
var isShow1 : boolean = false;
function ShowRed(str : String){
	isShow1 = true;
	LabelText1.text = AllManage.AllMge.Loc.Get(str);
	obj1.SetActiveRecursively(true);
	ptime1 = Time.time + 3;
}

function ShowRed1(str : String){
	isShow1 = true;
	LabelText1.text = str;
	obj1.SetActiveRecursively(true);
	ptime1 = Time.time + 3;
}

function Close1(){
	isShow1 = false;
	obj1.SetActiveRecursively(false);
}

var obj2 :  GameObject;
var LabelText2 : UILabel;
var ptime2 : float = 0;
var isShow2 : boolean = false;
function ShowBlue(str : String){
	isShow2 = true;
	LabelText2.text = AllManage.AllMge.Loc.Get(str);
	obj2.SetActiveRecursively(true);
	ptime2 = Time.time + 3;
	PlayAnyAudioClip();
}

function Close2(){
	isShow2 = false;
	obj2.SetActiveRecursively(false);
}

var objFinger :  GameObject;
var LabelTextFinger : UILabel;
var ptimeFinger : float = 0;
var isShowFinger : boolean = false;
var likai : Transform;
function ShowFinger(str : String , pos : Vector3){
	isShowFinger = true;
	LabelTextFinger.text = str;
	objFinger.transform.position = likai.position;
	objFinger.SetActiveRecursively(true);
	ptimeFinger = Time.time + 10;
}

function CloseFinger(){
	isShowFinger = false;
	objFinger.transform.localPosition.y = 3000;
	objFinger.SetActiveRecursively(false);
}

private var atime : float = 0;
function PlayAnyAudioClip(){
	if(Time.time > atime){
		atime = Time.time + 0.3;
		NGUITools.PlaySound(audioClip, 1, 1);
	}
}


var objBaffle :  GameObject;
function RefreshBaffleOn(){
//	print("sldjflsdkjflsdkjflsdkjflskdjflksdflkj");
//	PanelStatic.StaticBtnGameManager.OpenLoading();
//	objBaffle.SetActiveRecursively(true);
}

function RefreshBaffleOff(){
//	PanelStatic.StaticBtnGameManager.CloseLoading();
//	objBaffle.SetActiveRecursively(false);
}

function DoBaffleOn(){
	objBaffle.SetActiveRecursively(true);
}
