#pragma strict
private var Fstr : String = ",";
var parentGold : GameObject;
var parentBlood : GameObject;

var parentInv1 : BagItem;
var parentInv2 : BagItem;
var parentInv3 : BagItem;
var parentInv4 : BagItem;

var par : GameObject;
var textGold : UILabel;
var textBlood : UILabel;
var SpriteBox : UISprite;
var SpriteBlood : UISprite;
function open(id : int , gold : int , blood : int , inv1 : InventoryItem , inv2 : InventoryItem , inv3 : InventoryItem , inv4 : InventoryItem , newStr : String){
//	//print(inv1);
//	//print(inv2);
//	//print(inv3);
//	//print(inv4);
	par.SetActiveRecursively(true);
	if(gold <= 0){
		parentGold.SetActiveRecursively(false);
	}
	if(blood <= 0){
		parentBlood.SetActiveRecursively(false);
	}
	if(inv1 == null){
		parentInv1.gameObject.SetActiveRecursively(false);
	}
	if(inv2 == null){
		parentInv2.gameObject.SetActiveRecursively(false);
	}
	if(inv3 == null){
		parentInv3.gameObject.SetActiveRecursively(false);
	}
	if(inv4 == null){
		parentInv4.gameObject.SetActiveRecursively(false);
	}
	
	
	textGold.text = gold.ToString();
	textBlood.text = blood.ToString();
	if(inv1 != null){
		parentInv1.inv = null;
		parentInv1.SetInv(inv1);
		yield;
		parentInv1.inv = inv1;
	}
	if(inv2 != null){
		parentInv2.inv = null;
		parentInv2.SetInv(inv2);
		yield;
		parentInv2.inv = inv2;
	}
	if(inv3 != null){
		parentInv3.inv = null;
		parentInv3.SetInv(inv3);
		yield;
		parentInv3.inv = inv3;
	}
	if(inv4 != null){
		parentInv4.inv = null;
		parentInv4.SetInv(inv4);
		yield;
		parentInv4.inv = inv4;
	}
	
	if(newStr != ""){
//	Debug.Log("song=====================" + newStr);
		if(newStr.Split(Fstr.ToCharArray())[0] == "2"){
			SpriteBlood.spriteName = "UIM-Crystals";
		}else
		if(newStr.Split(Fstr.ToCharArray())[0] == "3"){
			SpriteBlood.spriteName = "UIM-Sulfur";
		}
		textBlood.text = newStr.Split(Fstr.ToCharArray())[1];
		parentBlood.SetActiveRecursively(true);
	}
//	SpriteBox.spriteName = "baoxiang" + id;
}

function open(id : int , gold : int , blood : int , inv1 : InventoryItem , inv2 : InventoryItem , inv3 : InventoryItem , inv4 : InventoryItem){
//	//print(inv1);
//	//print(inv2);
//	//print(inv3);
//	//print(inv4);
	par.SetActiveRecursively(true);
	if(gold <= 0){
		parentGold.SetActiveRecursively(false);
	}
	if(blood <= 0){
		parentBlood.SetActiveRecursively(false);
	}
	if(inv1 == null){
		parentInv1.gameObject.SetActiveRecursively(false);
	}
	if(inv2 == null){
		parentInv2.gameObject.SetActiveRecursively(false);
	}
	if(inv3 == null){
		parentInv3.gameObject.SetActiveRecursively(false);
	}
	if(inv4 == null){
		parentInv4.gameObject.SetActiveRecursively(false);
	}

	textGold.text = gold.ToString();
	textBlood.text = blood.ToString();
	if(inv1 != null){
		parentInv1.inv = null;
		parentInv1.SetInv(inv1);
		yield;
		parentInv1.inv = inv1;
	}
	if(inv2 != null){
		parentInv2.inv = null;
		parentInv2.SetInv(inv2);
		yield;
		parentInv2.inv = inv2;
	}
	if(inv3 != null){
		parentInv3.inv = null;
		parentInv3.SetInv(inv3);
		yield;
		parentInv3.inv = inv3;
	}
	if(inv4 != null){
		parentInv4.inv = null;
		parentInv4.SetInv(inv4);
		yield;
		parentInv4.inv = inv4;
	}
//	SpriteBox.spriteName = "baoxiang" + id;
}

function closeObj(){
	par.SetActiveRecursively(false);
}

	

