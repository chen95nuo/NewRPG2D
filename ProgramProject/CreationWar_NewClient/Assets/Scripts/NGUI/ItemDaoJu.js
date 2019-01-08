#pragma strict

function Start () {

}

function Update () {

}

var invcl : InventoryControl;
var Labelid : UILabel;
var Labelname : UILabel;
var Labelinfo : UILabel;
var Labelnum : UILabel; 
var SpriteIcon : UISprite;
function AddDaoJuObj(id : String , name : String , info : String , num : String ,  ivc : InventoryControl){
	invcl = ivc; 
	myid = id;
	myname = name;
	myinfo = info;
	Labelname .text = name;
	Labelnum.text = num;
	SpriteIcon.spriteName = id.Substring(0,3);
}

var myid : String;
var myname : String;
var myinfo : String;
function UseDaoju(){
	invcl.SelectDaojuAsID(myid , myname , myinfo , SpriteIcon.spriteName);
}
