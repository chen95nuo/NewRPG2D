#pragma strict

function Start () {

}

var sicl : StoreItemControl;
var biankuang : GameObject;
function Update () {

}

var inv : InventoryItem;
var dazhe : int = 0;
var zheshu : String;
var BtnI : BtnItem;
var cbx : UIToggle;
var timeEnd : String;
var uid : Transform;
var SpriteIcon : UISprite;
var infoBar : GameObject;
var spriteBackground:UISprite;
function setInv(iv : InventoryItem , gold : String , blood : String , id : String){
	inv = iv;
	if(dazhe == 1){
		BtnI.IsFavorable = true;
	}else{
		BtnI.IsFavorable = false;		
	}
//	cbx.radioButtonRoot = uid;
	BtnI.lblName.text = inv.itemName;
	BtnI.lblFavorable.text = zheshu;
	var discount : int = parseInt(zheshu);
	if(discount == 0 || discount == 10)
	{
	    //BtnI.lblFavorable.enabled = false;
	    //BtnI.spriteFavorable.enabled = false;
	    BtnI.IsFavorable = false;
	    zheshu = "10";
	}
	else
	{
	    //BtnI.lblFavorable.enabled = true;
	    //BtnI.spriteFavorable.enabled = true;
	    BtnI.IsFavorable = true;
	}

	BtnI.DtEnd = System.DateTime.Parse(timeEnd);
	BtnI.storeItemID = id;
	BtnI.ItemNeedBlood = parseInt(blood);
	BtnI.ItemNeedCash = parseInt(gold);
	SpriteIcon.spriteName = inv.atlasStr;
	BtnI.spriteBackground.spriteName=spriteBackground.spriteName;

}


var useinv : InventoryItem;
function SetBtn(btnI : BtnItem){
//	//print(btnI.ItemID+ " =======");
	if(btnI.ItemID.Substring(0,2) == "88"){
		SpriteIcon.spriteName = btnI.ItemID.Substring(0,3);
					var objParms: Object[]=new Object[2];
			objParms[0]=btnI.ItemID;
			objParms[1]=spriteBackground;
			PanelStatic.StaticBtnGameManager.invcl.SendMessage ("SetYanSeAsID",objParms,SendMessageOptions.DontRequireReceiver);
	}else{
		useinv = AllResources.InvmakerStatic.GetItemInfo(btnI.ItemID , useinv);
		SpriteIcon.spriteName = useinv.atlasStr;
					var objParms1: Object[]=new Object[2];
			objParms1[0]=btnI.ItemID;
			objParms1[1]=spriteBackground;
			PanelStatic.StaticBtnGameManager.invcl.SendMessage ("SetYanSeAsID",objParms1,SendMessageOptions.DontRequireReceiver);
	}
} 

function OnClick(){ 
	if(infoBar){
		infoBar.SetActiveRecursively(true);
		infoBar.transform.position=transform.position;
	//	infoBar.transform.Translate (infoBar.transform.up);
		infoBar.SendMessage("SetItemID",inv.itemID,SendMessageOptions.DontRequireReceiver);
	}
}

//
//function SelectMe(){	
//	
//}
//
//function isS(){
//	biankuang.active = true;
//}
//
//function noS(){
//	if(biankuang){
//		biankuang.active = false;	
//	}
//}
//
//function OnEnable(){
//	if(biankuang){
//		biankuang.active = false;	
//	}
//}
