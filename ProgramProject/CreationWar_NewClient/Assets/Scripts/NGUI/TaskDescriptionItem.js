#pragma strict

function Start () {

}

function Update () {

}
var LabName : UILabel;
var inv : InventoryItem = null;
function OnPress(){
//	//print("sss");
	UITooltip.ShowText(inv.professionType.ToString() + "\n" + inv.slotType.ToString());
	
}

public function ShowLabelName(){
	if(LabName!=null){
	LabName.text = inv.itemName;
	}
}
