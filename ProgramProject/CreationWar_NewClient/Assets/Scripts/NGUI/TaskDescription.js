#pragma strict

function Start () {

}

function Update () {

}

var invBackground : UISlicedSprite;
var invSprite : UISprite;
var inv : InventoryItem = null; 

var tdi : TaskDescriptionItem;
function SetTaskinv(v : InventoryItem){
	inv = v;
	invSprite.enabled = true; 
	invBackground.enabled = true;
	invSprite.spriteName = inv.atlasStr;
	tdi.inv = inv;
}

function NonTaskinv(){
	inv = null;
	invSprite.enabled = false; 
	invBackground.enabled = false;
}
