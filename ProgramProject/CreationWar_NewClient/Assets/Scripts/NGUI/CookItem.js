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
var labelItemNum : UILabel;

var LeveCook : int ;
function setInv(iv : InventoryItem){
	inv = iv;
	invSprite.spriteName = inv.atlasStr;
		if(labelItemNum){		
			labelItemNum.text = inv.consumablesNum.ToString();
		}
		
	LeveCook = inv.itemLevel; 
//	Labelname.text = "[0000ff]"+inv.weaponTypeStr[inv.slotType];
//	Labeltype.text = inv.weaponTypeStr[inv.slotType];
//	Labelzhiye.text = inv.professionTypeStr[inv.professionType];
//	Labeldengji.text = "等级" + inv.itemLevel.ToString();
//	LabelisEqu.text = "";
}

var ebCL : CookControl;
function GoSelect(){
	ebCL.SelectOneInv(this , inv);
	AllManage.cookCLStatic.NeedCookLeve(LeveCook);
}

var jiantou : UISprite;
function SelectMe(){
	jiantou.enabled = true;
}

function DontSelectMe(){
	jiantou.enabled = false;
}

//function DaZao(){
//	ebCL.DoDaZao(this);
//}
