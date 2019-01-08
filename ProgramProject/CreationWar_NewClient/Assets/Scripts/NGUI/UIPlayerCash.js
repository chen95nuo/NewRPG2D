#pragma strict

function Start(){
	InvokeRepeating("SetText", 2, 2);
}

var LGold : UILabel;
var LBlood : UILabel;
function SetText(){
	LGold.text = InventoryControl.yt.Rows[0]["Money"].YuanColumnText;
	LBlood.text = InventoryControl.yt.Rows[0]["Bloodstone"].YuanColumnText;
}