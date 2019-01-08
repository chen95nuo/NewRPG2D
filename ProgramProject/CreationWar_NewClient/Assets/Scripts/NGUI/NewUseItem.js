#pragma strict

function Update () {

}

var inv : InventoryItem;
var invSprite : UISprite;
var Labelname : UILabel;
var Labeltype : UILabel;
var LabelisEqu : UILabel;
var Labelzhiye : UILabel;
var Labeldengji : UILabel;
var SpriteBiankuang : UISprite;
var Labelnum : UILabel;
var myID : String = "";
function setInv(iv : InventoryItem){
	inv = iv;
	myID = inv.itemID;
	Labelnum.text = iv.consumablesNum.ToString();
	Labelname.text = inv.itemName ;
	invSprite.spriteName = inv.atlasStr;
}

//var invcl : InventoryControl;
//var Labelid : UILabel;
//var Labelname : UILabel;
//var Labelinfo : UILabel;
//var Labelnum : UILabel; 
//var SpriteIcon : UISprite;
function AddDaoJuObj(id : String , name : String , info : String , num : String){
	myID = id;
	myname = name;
	myinfo = info;
	Labelname.text = name;
	Labelnum.text = num;
	invSprite.spriteName = id.Substring(0,3);
}
//
//var myid : String;
var myname : String;
var myinfo : String;
//function UseDaoju(){
//	invcl.SelectDaojuAsID(myid , myname , myinfo , SpriteIcon.spriteName);
//}

function OnClick(){
	ebCL.UseItemAsID(myID,myname);
}

var ebCL : NewUseItemControl;
function GoSelect(){
//	ebCL.SelectOneInv(this , inv);
}

var jiantou : UISlicedSprite;
function SelectMe(){
	jiantou.enabled = true;
}

function DontSelectMe(){
	jiantou.enabled = false;
}

function OnPress(isclick : boolean){
			SelectClose.select.SetBool();
}
//function DaZao(){
//	ebCL.DoDaZao(this);
//}
