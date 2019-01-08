#pragma strict

var AllObj : GameObject;
function Awake(){
	AllManage.exhbtControl = this;
}

function OpenMe(boo1 : boolean[] , id : int){
	CloseAll();
	var i : int = 0;
	for(i=0; i<boo1.length; i++){
		if(boo1[i]){
			OpenOne(i);
		}
	}
	NowOpenThis(id);
	AllManage.jiaochengCLStatic.ParentButton.SetActiveRecursively(false); 
}

var SpriteIcons : UISprite[];
function CloseAll(){
	for(var i=0; i<SpriteIcons.length; i++){
		SpriteIcons[i].color.r = 0.5;
		SpriteIcons[i].color.g = 0.5;
		SpriteIcons[i].color.b = 0.5;
	}
}

function OpenOne(id : int){
	for(var i=0; i<SpriteIcons.length; i++){
		if(i == id){
			SpriteIcons[i].color.r = 1;		
			SpriteIcons[i].color.g = 1;		
			SpriteIcons[i].color.b = 1;		
		}
	}
}

var SpriteQuan : UISprite[];
function NowOpenThis(id : int){
	for(var i=0; i<SpriteQuan.length; i++){
		if(i == id){
			SpriteQuan[i].enabled = true;
		}
	}
}

function DoOpenAsID(id : int){
	if( ! SpriteQuan[id].enabled){
		return;
	}
	for(var i=0; i<SpriteIcons.length; i++){
		if(i == id){
			SpriteQuan[i].enabled = false;
			SpriteIcons[i].color.r = 1;		
			SpriteIcons[i].color.g = 1;		
			SpriteIcons[i].color.b = 1;		
		}
	}
	yield WaitForSeconds(2);
	AllManage.UIALLPCStatic.show0();
//	AllObj.SetActiveRecursively(false);
//	close	
}

function OnButtons0(){
	DoOpenAsID(0);
}

function OnButtons1(){
	DoOpenAsID(1);
}

function OnButtons2(){
	DoOpenAsID(2);
}

function OnButtons3(){
	DoOpenAsID(3);
}

function OnButtons4(){
	DoOpenAsID(4);
}

function OnButtons5(){
	DoOpenAsID(5);
}

function OnButtons6(){
	DoOpenAsID(6);
}
