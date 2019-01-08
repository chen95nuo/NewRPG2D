#pragma strict

function Start () {

}

function Update () {

}

var inv : InventoryItem;
var bagit : BagItem;
function SetInv(iv : InventoryItem){
	inv = iv;
	bagit.SetInv(inv);
}

var ground : UISprite;
var invSprite1 : UISprite;
var invSprite2 : UISprite;
var scaleItem : GameObject;
var CostBlood : UILabel;
function trunOn(){
	scaleItem.SendMessage("Play" , true , SendMessageOptions.DontRequireReceiver);
	yield WaitForSeconds(0.25);
	ground.transform.localPosition.z = -673.3737;
	ground.spriteName = "fanpai - zheng";
//	ground.enabled = false;
//	ground.enabled = true;	
	invSprite1.enabled = true;
	invSprite2.enabled = true;
	if(CostBlood)
		CostBlood.enabled = false;
	ground.gameObject.GetComponent(Collider).enabled = false;
}

function trunOff(){
	ground.transform.localPosition.z = -900;
	ground.spriteName = "fanpai - bei";
//	ground.enabled = false;
//	ground.enabled = true;	
	invSprite1.enabled = false;
	invSprite2.enabled = false;
	if(CostBlood)
		CostBlood.enabled = true;
}

var cardCL : CardControl;
var canClick : boolean = false;
function ClickMe(){
//	//print("1111" + canClick);
	if(canClick && AllManage.CardCLStatic.canCube){
		cardCL.ClickButton(this , bagit);	
	}
}