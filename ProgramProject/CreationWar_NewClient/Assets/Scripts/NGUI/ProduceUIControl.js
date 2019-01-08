#pragma strict

function Start () {
	UIInit();
}

var leftID : int = 0;
var downID : int = 0;
var spriteLefts : UISprite[];
var spriteDowns : UISprite[];

function UIInit(){
	SelectLeft1();
	SelectDown1();
//	SelectLeftAsID(1);
//	SelectDownAsID(1);
//	AllManage.inventoryProduceStatic.nowSelectLevelMin = 20;
//	AllManage.inventoryProduceStatic.nowSelectLevelMax = 39;
//	AllManage.inventoryProduceStatic.nowSelectPro = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText);
}

function SelectLeft1(){
	AllManage.inventoryProduceStatic.nowSelectLevelMin = 20;
	AllManage.inventoryProduceStatic.nowSelectLevelMax = 39;
	SelectLeftAsID(1);
}

function SelectLeft2(){
	AllManage.inventoryProduceStatic.nowSelectLevelMin = 40;
	AllManage.inventoryProduceStatic.nowSelectLevelMax = 59;
	SelectLeftAsID(2);
}

function SelectLeft3(){
	AllManage.inventoryProduceStatic.nowSelectLevelMin = 60;
	AllManage.inventoryProduceStatic.nowSelectLevelMax = 60;
	SelectLeftAsID(3);
}

function SelectDown1(){
	AllManage.inventoryProduceStatic.nowSelectPro = 1;
	SelectDownAsID(1);
}

function SelectDown2(){
	AllManage.inventoryProduceStatic.nowSelectPro = 2;
	SelectDownAsID(2);
}

function SelectDown3(){
	AllManage.inventoryProduceStatic.nowSelectPro = 3;
	SelectDownAsID(3);
}

var leftOnName : String = "";
var leftOffName : String = "";
function SelectLeftAsID(id : int){
	leftID = id;
	spriteLefts[1].spriteName = leftOffName;
	spriteLefts[2].spriteName = leftOffName;
	spriteLefts[3].spriteName = leftOffName;
	spriteLefts[id].spriteName = leftOnName;
	AllManage.inventoryProduceStatic.RefreshList();
}


var downOnName : String = "";
var downOffName : String = "";
function SelectDownAsID(id : int){
	downID = id;
	spriteDowns[1].spriteName = downOffName;
	spriteDowns[2].spriteName = downOffName;
	spriteDowns[3].spriteName = downOffName;
	spriteDowns[id].spriteName = downOnName;
	AllManage.inventoryProduceStatic.RefreshList();
}

