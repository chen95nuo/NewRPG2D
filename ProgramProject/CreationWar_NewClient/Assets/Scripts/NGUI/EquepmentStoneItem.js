#pragma strict


var EquepmentPC : EquepmentPunchControl;
var inv : InventoryItem;
var invSprite : UISprite;
var Labelname : UILabel;
var LabelNum : UILabel;
var LabelDescription : UILabel;
function setInv(iv : InventoryItem){
	inv = iv;
	invSprite.spriteName = inv.atlasStr;
	LabelNum.text = inv.consumablesNum.ToString();
	Labelname.text = inv.itemName;
	LabelDescription.text = AllResources.ar.GetHoleDescription(inv.itemID.Substring(0,4)) + AllManage.AllMge.Loc.Get(AllResources.StoneValueInfo[ parseInt(inv.itemID.Substring(2,1)) - 1 ]);
}

function SelectMe(){
	EquepmentPC.SelectOneStone(inv);
}
