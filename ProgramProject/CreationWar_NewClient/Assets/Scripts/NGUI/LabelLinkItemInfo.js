#pragma strict

function Awake(){
	AllManage.yuaninfoStatic = this;
}

var equepmentInfo : EquepmentItemInfo;
//var invMaker : Inventorymaker;
var pl : UIPanel;
var ObjRide : GameObject;
var ObjInv : GameObject;
var ObjSoul : GameObject;
private var useStr : String;
function SetItemID(id : String){
//	ObjRide.SetActiveRecursively(false);
	ObjSoul.SetActiveRecursively(false);
	pl.enabled = true;
	var inv : InventoryItem;
	useStr = id.Substring(0,2);
	if(useStr == "70" || useStr == "71"){
		inv = new InventoryItem();
		inv = AllResources.InvmakerStatic.GetItemInfo(id,inv);
		LookDitemInfo(inv);
	}else
	if(useStr == "72"){
		LookRideItemInfo(id);
	}else{	
		inv = new InventoryItem();
		inv = AllResources.InvmakerStatic.GetItemInfo(id,inv);
		equepmentInfo.showEquItemInfo(inv,null);
	}
	ObjSoul.transform.localPosition = Vector3.zero;
//	ObjRide.transform.localPosition = Vector3.zero;
	ObjInv.transform.localPosition = Vector3.zero;
}

var LabelSkill : UILabel;
var iconSkill : UISprite;
var biankuang : UISprite;
function LookDitemInfo(inv : InventoryItem){ 
	ObjSoul.SetActiveRecursively(true);
	ObjInv.SetActiveRecursively(false);
	LabelSkill.text = "";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages123");
			AllManage.AllMge.Keys.Add(inv.itemName + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "名称: " + inv.itemName + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages062");
			AllManage.AllMge.Keys.Add(" " + inv.itemLevel + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "等级: " + inv.itemLevel + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages075");
			AllManage.AllMge.Keys.Add(" " + inv.itemQuality + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "品质: " + inv.itemQuality + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages124");
			AllManage.AllMge.Keys.Add(inv.ItemInfo + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "说明: " + inv.ItemInfo + "\n"; 
	iconSkill.spriteName = inv.atlasStr; 
	biankuang.spriteName = "yanse" + inv.itemQuality;
}

var LabelRidename : UILabel;
var LabelRidePin : UILabel;
var LabelRideInfo : UILabel;
var SpriteRideIcon : UISprite;
function LookRideItemInfo(id : String){
	var rows : yuan.YuanMemoryDB.YuanRow;
	rows = AllManage.InvclStatic.GetRideRowAsID(id);
	if(rows != null){
		ObjSoul.SetActiveRecursively(true);
		ObjInv.SetActiveRecursively(false);
		LabelSkill.text = "";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages123");
			AllManage.AllMge.Keys.Add(rows["Name"].YuanColumnText + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//		LabelSkill.text += "名称: " + rows["Name"].YuanColumnText + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages075");
			AllManage.AllMge.Keys.Add(id.Substring(4,1) + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//		LabelSkill.text += "品质: " + id.Substring(4,1) + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages124");
			AllManage.AllMge.Keys.Add(rows["ItemInfo"].YuanColumnText + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//		LabelSkill.text += "说明: " + rows["ItemInfo"].YuanColumnText + "\n"; 
		iconSkill.spriteName = "Ride_UI" + id.Substring(2,2); 
		biankuang.spriteName = "yanse" + id.Substring(4,1);
//		ObjInv.SetActiveRecursively(false);
//		ObjRide.SetActiveRecursively(true);
//		ObjSoul.SetActiveRecursively(false);
//		LabelRidename .text = rows["Name"].YuanColumnText;
//		LabelRidePin.text = "品质:"+id.Substring(4,1);
//		LabelRideInfo.text = rows["ItemInfo"].YuanColumnText;
//		SpriteRideIcon.spriteName = "Ride_UI" + id.Substring(2,2);
	}
}
