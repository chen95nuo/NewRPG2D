#pragma strict

var MessageTaget : GameObject;
var DefaultBar : Transform;
function Awake(){
	MessageTaget = AllManage.UICLStatic.gameObject;
}
var isFirst : boolean = false;
function Start(){
//	for(){}
}

private var useStr : String;
function OnEnable(){
	if(!isFirst){
		isFirst = true;
		return;
	}
	if(AllManage.UIALLPCStatic.parentNowTask && AllManage.UIALLPCStatic.parentPVP){
		for(var i=0; i<BarTweens.length; i++){
			if(BarTweens[i] == DefaultBar){
				useStr = "Select" + (i + 1);
				//print(useStr + " == useStr");
				SendMessage(useStr , SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}

var Message1 : String;
var Index1 : int = 0;
function Select1(){
	SetActiveAsID(Index1);
	MessageTaget.SendMessage(Message1 , SendMessageOptions.DontRequireReceiver);
}

var Message2 : String;
var Index2 : int = 1;
function Select2(){
	SetActiveAsID(Index2);
	MessageTaget.SendMessage(Message2 , SendMessageOptions.DontRequireReceiver);
}

var Message3 : String;
var Index3 : int = 2;
function Select3(){
	SetActiveAsID(Index3);
	MessageTaget.SendMessage(Message3 , SendMessageOptions.DontRequireReceiver);
}	

var Message4 : String;
var Index4 : int = 3;
function Select4(){
	SetActiveAsID(Index4);
	MessageTaget.SendMessage(Message4 , SendMessageOptions.DontRequireReceiver);
}

var Message5 : String;
var Index5 : int = 4;
function Select5(){
    SetActiveAsID(Index5);
    MessageTaget.SendMessage(Message5 , SendMessageOptions.DontRequireReceiver);
}

var BarSprite : UISprite[];
var BarSpriteNameOn : String;
var BarSpriteNameOff : String;
var BarTweens : Transform[];
function SetActiveAsID(select : int){
	for(var i=0; i<BarTweens.length; i++){
		if(i == select){
			BarSprite[i].spriteName = BarSpriteNameOn;
			BarTweens[i].localPosition.y = 0;
		}else{
			if(BarSprite[i] != BarSprite[select])
				BarSprite[i].spriteName = BarSpriteNameOff;
			BarTweens[i].localPosition.y = 3000;
		}
	}
}

function UnUse(){
	AllManage.tsStatic.Show("tips081");
}
