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
var myid : String;
var myname : String;
var myinfo : String;
var kuang : UISprite;
var level : int = 0;
var pin : int = 0;
var biankuang : UISprite;
var iconName : String;
var biankuangName : String;
function AddRideObj(id : String , name : String , info : String , num : String ,  ivc : InventoryControl){
	invcl = ivc; 
	myid = id;
	myname = name;
	myinfo = info;
	pin = parseInt(myid.Substring(4,1));
	Labelname .text = name;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages075");
			AllManage.AllMge.Keys.Add(pin + "");
			AllManage.AllMge.SetLabelLanguageAsID(Labelnum);
//	Labelnum.text = "品质:"+ pin;
	level = parseInt(num);
	SpriteIcon.spriteName = "Ride_UI" + id.Substring(2,2);
	biankuang.spriteName = "yanse" + pin;
	iconName = SpriteIcon.spriteName;
	biankuangName = biankuang.spriteName;
}

function SelectRide(){
	invcl.SelectRideAsID(myid , myname , myinfo , iconName , biankuangName, this);
}

function YesSelect(){
	kuang.enabled = true;
}

function NoSelect(){
	kuang.enabled = false;
}