#pragma strict

function Start () {
//	showDaZao();
}

function Update () {

}

var TransDX : Transform[];
var SpriteDX : UISprite[];
//var LabelQiehuan : UILabel;
//var uiallpcl : UIAllPanelControl;

var equBuildCL : EquepmentBuildControl;
function showDaZao(){
//	uiallpcl.show2();
	equBuildCL.resetList();
	equBuildCL.SelectFristInv();
//	LabelQiehuan.text = "装备打造";
//	TransDX[0].localPosition.y = 0;
//	TransDX[1].localPosition.y = 1000;
//	SpriteDX[0].spriteName = "UIB_Equipment_upgrades";
//	SpriteDX[1].spriteName = "UIB_Precious_Stones_Inlaid_O";
}

var baoshiGround : GameObject;
var equPunchCL : EquepmentPunchControl;
function showXiangQian(){
//	uiallpcl.show18();
	equPunchCL.resetList();
	equPunchCL.SelectFristInv();
//	LabelQiehuan.text = "打孔镶嵌";
//	TransDX[0].localPosition.y = 1000;
//	TransDX[1].localPosition.y = 0;
//	SpriteDX[0].spriteName = "UIB_Equipment_upgrades_O";
//	SpriteDX[1].spriteName = "UIB_Precious_Stones_Inlaid";
//	baoshiGround.SetActiveRecursively(false);
}
