#pragma strict

function Start () {

}

function Update () {

}

var btnSprite : UISprite;
var SlSprite : UISlicedSprite;
var equepmentPC : EquepmentPunchControl;
var myID : int = 1;
function clickMe(){
//	//print("dian ji le");
	if(btnSprite.spriteName == "UIB_Punch"){
		equepmentPC.DaKong();
	}else
//	if(btnSprite.spriteName == "UIM_Gem_Bar ")
	{
		equepmentPC.XiangQian(btnSprite , myID);
	}
}
